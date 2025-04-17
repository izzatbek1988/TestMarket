// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolKkmServer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class AtolKkmServer : IFiscalKkm, IDevice
  {
    private AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.IndustryInfo _industryInfo;

    private void CheckResult(AtolServerDriver.AtolServerCommand command)
    {
      AtolServerDriver.Answer answer = command.DeviceAnswer.results.First<AtolServerDriver.Answer>();
      if (answer.errorCode != 0)
        throw new DeviceException(string.Format(Translate.AtolKkmServer_CheckResult_, (object) answer.errorDescription, (object) answer.errorCode));
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.AtolKkmServer_Name_АТОЛ_Web_Сервер;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      AtolServerDriver.KkmGetDeviceInfoCommand command = new AtolServerDriver.KkmGetDeviceInfoCommand();
      if (this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command))
      {
        switch (command.Result.results.First<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult>().result.deviceInfo.fnFfdVersion)
        {
          case "1.0":
            return GlobalDictionaries.Devices.FfdVersions.Ffd100;
          case "1.05":
            return GlobalDictionaries.Devices.FfdVersions.Ffd105;
          case "1.1":
            return GlobalDictionaries.Devices.FfdVersions.Ffd110;
          case "1.2":
            return GlobalDictionaries.Devices.FfdVersions.Ffd120;
        }
      }
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
    }

    public bool IsCanHoldConnection => false;

    private AtolServerDriver.KkmRegistreCheckCommand CurrentCheck { get; set; }

    private AtolServerDriver CurrentDriver { get; set; }

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult();

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = true,
        Type = GlobalDictionaries.Devices.ConnectionTypes.Lan
      });
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AtolServerDriver.KkmOpenSessionCommand command = new AtolServerDriver.KkmOpenSessionCommand()
      {
        Command = new AtolServerDriver.KkmOpenSessionCommand.KkmOpenSession()
        {
          @operator = new AtolServerDriver.Cashier(cashier)
        }
      };
      this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command);
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
    }

    public void GetReport(Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.ZReport:
          AtolServerDriver.KkmCloseSessionCommand command1 = new AtolServerDriver.KkmCloseSessionCommand()
          {
            Command = new AtolServerDriver.KkmCloseSessionCommand.KkmCloseSession()
            {
              @operator = new AtolServerDriver.Cashier(cashier)
            }
          };
          this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command1);
          this.CheckResult((AtolServerDriver.AtolServerCommand) command1);
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReport:
          AtolServerDriver.KkmGetXReportommand command2 = new AtolServerDriver.KkmGetXReportommand()
          {
            Command = new AtolServerDriver.KkmGetXReportommand.KkmGetXReport()
            {
              @operator = new AtolServerDriver.Cashier(cashier)
            }
          };
          this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command2);
          this.CheckResult((AtolServerDriver.AtolServerCommand) command2);
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
      }
    }

    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 120, АТОЛ 10 ВЕБ СЕРВЕР");
      this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) new AtolServerDriver.СancelMarkingCodeValidationCommand()
      {
        Command = new AtolServerDriver.СancelMarkingCodeValidationCommand.СancelMarkingCodeValidation()
      });
      this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) new AtolServerDriver.СlearMarkingCodeCommand()
      {
        Command = new AtolServerDriver.СlearMarkingCodeCommand.СlearMarkingCode()
      });
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
      {
        checkGood.MarkedInfo.ValidationResultKkm = (object) null;
        string str1 = this.PrepareMarkCodeForFfd120(checkGood.MarkedInfo.FullCode);
        int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
        LogHelper.Debug("Валидация кода: " + str1);
        checkGood.MarkedInfo.FullCode = str1;
        AtolServerDriver.BeginMarkingCodeValidationCommand command1 = new AtolServerDriver.BeginMarkingCodeValidationCommand()
        {
          Command = new AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidation()
          {
            @params = new AtolServerDriver.MarkedInfo()
            {
              imc = RuOnlineKkmHelper.Base64Encode(checkGood.MarkedInfo.FullCode),
              imcType = 256,
              itemEstimatedStatus = markingCodeStatus,
              imcModeProcessing = 0
            }
          }
        };
        if (markingCodeStatus.IsEither<int>(2, 4))
        {
          command1.Command.@params.itemQuantity = new double?((double) checkGood.Quantity);
          command1.Command.@params.itemUnits = new int?(checkGood.Unit.RuFfdUnitsIndex);
          int ruFfdUnitsIndex = checkGood.Unit.RuFfdUnitsIndex;
        }
        this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command1);
        this.CheckResult((AtolServerDriver.AtolServerCommand) command1);
        bool flag = false;
        AtolServerDriver.GetMarkingCodeValidationStatusCommand command2 = new AtolServerDriver.GetMarkingCodeValidationStatusCommand();
        for (int index = 0; index < 50; ++index)
        {
          command2.uuid = Guid.NewGuid().ToString();
          this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command2);
          this.CheckResult((AtolServerDriver.AtolServerCommand) command2);
          if (command2.Result.results.First<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult>().result.ready)
          {
            flag = true;
            break;
          }
          Thread.Sleep(100);
        }
        if (flag)
        {
          AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult.Result result = command2.Result.results.First<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult>().result;
          AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult.Result.OnlineValidation onlineValidation = result.onlineValidation;
          string jsonString = onlineValidation != null ? onlineValidation.itemInfoCheckResult.ToJsonString(true) : (string) null;
          string str2 = result.driverError != null ? string.Format("code: {0} ({1}): {2}", (object) result.driverError.code, (object) result.driverError.error, (object) result.driverError.description) : "";
          LogHelper.Debug("Проверка кода маркировки закончилась.");
          LogHelper.Debug("ErrorOnlineResult: " + jsonString);
          LogHelper.Debug("ErrorOfflineResult: " + str2);
          checkGood.MarkedInfo.ValidationResultKkm = (object) result.onlineValidation?.itemInfoCheckResult;
          LogHelper.Debug(string.Format("Validation ready: {0}; result code: {1}", (object) flag, checkGood.MarkedInfo.ValidationResultKkm));
          this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) new AtolServerDriver.AcceptMarkingCodeCommand());
        }
        else
        {
          LogHelper.Debug("Проверка кода не завершена, таймаут проверки, отменяем проверку, но отправляем в чек");
          this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) new AtolServerDriver.AcceptMarkingCodeCommand());
          break;
        }
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      Gbs.Core.Config.FiscalKkm fiscalKkm = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm;
      if (fiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.Ffd120CodeValidation(checkData);
      this.CurrentCheck = new AtolServerDriver.KkmRegistreCheckCommand()
      {
        Command = new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck()
        {
          @operator = new AtolServerDriver.Cashier(checkData.Cashier),
          Type = checkData.CheckType,
          electronically = fiscalKkm.IsAlwaysNoPrintCheck
        }
      };
      if (fiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.CurrentCheck.Command.validateMarkingCodes = new bool?(false);
      if (checkData.Client != null)
      {
        AtolServerDriver.ClientInfo clientInfo = new AtolServerDriver.ClientInfo(checkData.Client.Client);
        if (this.CurrentCheck.Command.clientInfo == null)
        {
          this.CurrentCheck.Command.clientInfo = clientInfo;
        }
        else
        {
          this.CurrentCheck.Command.clientInfo.name = clientInfo.name;
          this.CurrentCheck.Command.clientInfo.vatin = clientInfo.vatin;
        }
      }
      AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck command = this.CurrentCheck.Command;
      string str1;
      switch (checkData.RuTaxSystem)
      {
        case GlobalDictionaries.RuTaxSystems.None:
          str1 = (string) null;
          break;
        case GlobalDictionaries.RuTaxSystems.Osn:
          str1 = "osn";
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohod:
          str1 = "usnIncome";
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
          str1 = "usnIncomeOutcome";
          break;
        case GlobalDictionaries.RuTaxSystems.Envd:
          str1 = "envd";
          break;
        case GlobalDictionaries.RuTaxSystems.Esn:
          str1 = "esn";
          break;
        case GlobalDictionaries.RuTaxSystems.Psn:
          str1 = "patent";
          break;
        default:
          str1 = (string) null;
          break;
      }
      command.taxationType = str1;
      this.CurrentCheck.Command.items = new List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem>();
      this.GetSalesNotice(checkData.TrueApiInfoForKkm);
      foreach (CheckGood goods1 in checkData.GoodsList)
      {
        GlobalDictionaries.RuFfdGoodsTypes ruFfdGoodsTypes = goods1.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None ? GlobalDictionaries.RuFfdGoodsTypes.SimpleGood : goods1.RuFfdGoodTypeCode;
        AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods goods2 = new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods();
        goods2.name = goods1.Name;
        goods2.amount = Math.Round(goods1.Quantity * goods1.Price, 2, MidpointRounding.AwayFromZero) - goods1.DiscountSum;
        goods2.price = goods1.Price;
        goods2.quantity = goods1.Quantity;
        goods2.infoDiscountAmount = goods1.DiscountSum;
        goods2.paymentObject = ((int) ruFfdGoodsTypes).ToString();
        AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods goods3 = goods2;
        string str2;
        switch (goods1.RuFfdPaymentModeCode)
        {
          case GlobalDictionaries.RuFfdPaymentModes.None:
            str2 = "fullPayment";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.PrePaymentFull:
            str2 = "fullPrepayment";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.Prepayment:
            str2 = "prepayment";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.AdvancePayment:
            str2 = "advance";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.FullPayment:
            str2 = "fullPayment";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.PartPaymentAndCredit:
            str2 = "partialPayment";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.FullCredit:
            str2 = "credit";
            break;
          case GlobalDictionaries.RuFfdPaymentModes.PaymentForCredit:
            str2 = "creditPayment";
            break;
          default:
            str2 = "fullPayment";
            break;
        }
        goods3.paymentMethod = str2;
        AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods goods4 = goods2;
        if (fiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
          goods4.measurementUnit = new int?(goods1.Unit.RuFfdUnitsIndex);
        if (goods1.MarkedInfo != null && goods1.MarkedInfo.Type != GlobalDictionaries.RuMarkedProductionTypes.None && goods1.MarkedInfo.IsValidCode())
        {
          if (fiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
          {
            goods4.industryInfo = new List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.IndustryInfo>()
            {
              this._industryInfo
            };
            goods4.measurementUnit = new int?(goods1.Unit.RuFfdUnitsIndex);
            goods4.itemUnits = new int?(goods1.Unit.RuFfdUnitsIndex);
            if (goods1.MarkedInfo != null && !goods1.MarkedInfo.FullCode.IsNullOrEmpty() && goods1.MarkedInfo.ValidationResultKkm != null)
              goods4.imcParams = new AtolServerDriver.MarkedInfo()
              {
                imc = RuOnlineKkmHelper.Base64Encode(goods1.MarkedInfo.FullCode),
                imcType = 256,
                itemEstimatedStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(goods1, checkData.CheckType),
                imcModeProcessing = 0,
                itemInfoCheckResult = (AtolServerDriver.InfoCheckResult) goods1.MarkedInfo.ValidationResultKkm
              };
          }
          else
            goods4.nomenclatureCode = this.GetMarkedInfoBase64(goods1.MarkedInfo);
        }
        AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods.Tax tax = goods4.tax;
        string str3;
        switch (goods1.TaxRateNumber)
        {
          case 1:
            str3 = "none";
            break;
          case 2:
            str3 = "vat0";
            break;
          case 3:
            str3 = "vat10";
            break;
          case 4:
            str3 = "vat20";
            break;
          case 5:
            str3 = "vat110";
            break;
          case 6:
            str3 = "vat120";
            break;
          case 7:
            str3 = "vat5";
            break;
          case 8:
            str3 = "vat7";
            break;
          case 9:
            str3 = "vat105";
            break;
          case 10:
            str3 = "vat107";
            break;
          default:
            str3 = "none";
            break;
        }
        tax.type = str3;
        this.CurrentCheck.Command.items.Add((AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem) goods4);
        foreach (string _text in goods1.CommentForFiscalCheck)
          this.CurrentCheck.Command.items.Add((AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem) new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString(_text));
      }
      this.CurrentCheck.Command.payments = new List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments>()
      {
        new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments()
        {
          sum = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)),
          type = "cash"
        },
        new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments()
        {
          sum = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bank, GlobalDictionaries.KkmPaymentMethods.Card, GlobalDictionaries.KkmPaymentMethods.EMoney))).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)),
          type = "electronically"
        },
        new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments()
        {
          sum = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.PrePayment)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)),
          type = "prepaid"
        },
        new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments()
        {
          sum = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Credit)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)),
          type = "credit"
        }
      };
      return true;
    }

    private string GetMarkedInfoBase64(Gbs.Core.Devices.CheckPrinters.CheckData.MarkedInfo mark)
    {
      string str = mark.GetHexStringAttribute().ToUpper().Replace(" ", string.Empty);
      int length = str.Length;
      byte[] inArray = new byte[length / 2];
      for (int startIndex = 0; startIndex < length; startIndex += 2)
      {
        if (str.Length - startIndex >= 2)
          inArray[startIndex / 2] = Convert.ToByte(str.Substring(startIndex, 2), 16);
      }
      return Convert.ToBase64String(inArray);
    }

    public bool CloseCheck()
    {
      int num = this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) this.CurrentCheck) ? 1 : 0;
      this.CheckResult((AtolServerDriver.AtolServerCommand) this.CurrentCheck);
      Gbs.Core.Config.FiscalKkm fiscalKkm = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm;
      return num != 0;
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AtolServerDriver.KkmPaymentActionCommand command = new AtolServerDriver.KkmPaymentActionCommand()
      {
        Command = new AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction()
        {
          @operator = new AtolServerDriver.Cashier(cashier),
          Type = "cashOut",
          cashSum = sum
        }
      };
      int num = this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command) ? 1 : 0;
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
      return num != 0;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AtolServerDriver.KkmPaymentActionCommand command = new AtolServerDriver.KkmPaymentActionCommand()
      {
        Command = new AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction()
        {
          @operator = new AtolServerDriver.Cashier(cashier),
          Type = "cashIn",
          cashSum = sum
        }
      };
      int num = this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command) ? 1 : 0;
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
      return num != 0;
    }

    public bool WriteOfdAttribute(Gbs.Core.Devices.CheckPrinters.FiscalKkm.OfdAttributes ofdAttribute, object value)
    {
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      AtolServerDriver.KkmGetCashDrawerCommand command = new AtolServerDriver.KkmGetCashDrawerCommand();
      int num = this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command) ? 1 : 0;
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
      ref Decimal local = ref sum;
      AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult.Result result = command.Result.results.First<AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult>().result;
      Decimal cashSum = result != null ? result.counters.cashSum : 0M;
      local = cashSum;
      return num != 0;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType) => true;

    public bool RegisterPayment(CheckPayment payment) => true;

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      if (onlyDriverLoad)
        return;
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.CurrentDriver = new AtolServerDriver(dc.CheckPrinter.Connection.LanPort);
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<Gbs.Core.Devices.CheckPrinters.NonFiscalString> nonFiscalStrings)
    {
      if (nonFiscalStrings.Count == 0)
        return;
      AtolServerDriver.KkmPrintNonFiscalCommand command = new AtolServerDriver.KkmPrintNonFiscalCommand()
      {
        Command = new AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalText()
        {
          items = nonFiscalStrings.Select<Gbs.Core.Devices.CheckPrinters.NonFiscalString, AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString>((Func<Gbs.Core.Devices.CheckPrinters.NonFiscalString, AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString>) (x =>
          {
            AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString nonFiscalString1 = new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString(x.Text);
            AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString nonFiscalString2 = nonFiscalString1;
            string str;
            switch (x.Alignment)
            {
              case TextAlignment.Left:
                str = "left";
                break;
              case TextAlignment.Right:
                str = "right";
                break;
              case TextAlignment.Center:
                str = "center";
                break;
              case TextAlignment.Justify:
                str = "center";
                break;
              default:
                str = "left";
                break;
            }
            nonFiscalString2.alignment = str;
            return nonFiscalString1;
          })).ToList<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString>()
        }
      };
      this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command);
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      AtolServerDriver.KkmPrintBarcodeCommand printBarcodeCommand1 = new AtolServerDriver.KkmPrintBarcodeCommand();
      AtolServerDriver.KkmPrintBarcodeCommand printBarcodeCommand2 = printBarcodeCommand1;
      AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode kkmPrintBarode1 = new AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode();
      AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode kkmPrintBarode2 = kkmPrintBarode1;
      List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode> barcodeList1 = new List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode>();
      List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode> barcodeList2 = barcodeList1;
      AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode barcode1 = new AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode();
      barcode1.barcode = code;
      AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode barcode2 = barcode1;
      string str;
      switch (type)
      {
        case BarcodeTypes.None:
          str = "CODE128";
          break;
        case BarcodeTypes.Ean13:
          str = "EAN13";
          break;
        case BarcodeTypes.QrCode:
          str = "QR";
          break;
        default:
          str = "CODE128";
          break;
      }
      barcode2.alignment = str;
      barcode1.scale = 7;
      barcodeList2.Add(barcode1);
      kkmPrintBarode2.items = barcodeList1;
      printBarcodeCommand2.Command = kkmPrintBarode1;
      AtolServerDriver.KkmPrintBarcodeCommand command = printBarcodeCommand1;
      int num = this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command) ? 1 : 0;
      this.CheckResult((AtolServerDriver.AtolServerCommand) command);
      return num != 0;
    }

    public bool CutPaper() => true;

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetShortStatus()
    {
      AtolServerDriver.KkmGetStatusCommand command = new AtolServerDriver.KkmGetStatusCommand();
      this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command);
      AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult.Result.DeviceStatus deviceStatus = command.Result.results.First<AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult>().result.deviceStatus;
      Gbs.Core.Devices.CheckPrinters.KkmStatus shortStatus = new Gbs.Core.Devices.CheckPrinters.KkmStatus();
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus = shortStatus;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses sessionStatuses;
      switch (deviceStatus.shift)
      {
        case "closed":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Close;
          break;
        case "opened":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Open;
          break;
        case "expired":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.OpenMore24Hours;
          break;
        default:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      shortStatus.KkmState = this.GetKkmState(deviceStatus);
      return shortStatus;
    }

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetStatus()
    {
      AtolServerDriver.KkmGetStatusCommand command1 = new AtolServerDriver.KkmGetStatusCommand();
      AtolServerDriver.KkmGetShiftStatusCommand command2 = new AtolServerDriver.KkmGetShiftStatusCommand();
      AtolServerDriver.KkmGetFnStatusCommand command3 = new AtolServerDriver.KkmGetFnStatusCommand();
      AtolServerDriver.KkmGetOfdStatusCommand command4 = new AtolServerDriver.KkmGetOfdStatusCommand();
      AtolServerDriver.KkmGetDeviceInfoCommand command5 = new AtolServerDriver.KkmGetDeviceInfoCommand();
      if ((!this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command1) || !this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command2) || !this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command3) || !this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command4) ? 0 : (this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) command5) ? 1 : 0)) == 0)
        throw new Exception(Translate.AtolKkmServer_Не_удалось_получить_статус_ККМ_Атол_Веб_сервер);
      AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult.Result.DeviceStatus deviceStatus = command1.Result.results.First<AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult>().result.deviceStatus;
      AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult.Result.ShiftStatus shiftStatus = command2.Result.results.First<AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult>().result.shiftStatus;
      AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult.Result.FnStatus fnStatus = command3.Result.results.First<AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult>().result.fnStatus;
      AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult.Result.Status status1 = command4.Result.results.First<AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult>().result.status;
      AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult.Result.DeviceInfo deviceInfo = command5.Result.results.First<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult>().result.deviceInfo;
      Gbs.Core.Devices.CheckPrinters.KkmStatus status2 = new Gbs.Core.Devices.CheckPrinters.KkmStatus();
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus = status2;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses sessionStatuses;
      switch (deviceStatus.shift)
      {
        case "closed":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Close;
          break;
        case "opened":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Open;
          break;
        case "expired":
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.OpenMore24Hours;
          break;
        default:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      status2.KkmState = this.GetKkmState(deviceStatus);
      status2.SessionStarted = new DateTime?(DateTime.Parse(shiftStatus.expiredTime));
      status2.SessionNumber = shiftStatus.number;
      status2.CheckNumber = fnStatus.fiscalDocumentNumber;
      status2.OfdLastSendDateTime = new DateTime?(DateTime.Parse(status1.notSentFirstDocDateTime));
      status2.OfdNotSendDocuments = status1.notSentCount;
      status2.FactoryNumber = deviceInfo.serial;
      status2.Model = deviceInfo.modelName;
      status2.SoftwareVersion = deviceInfo.firmwareVersion;
      return status2;
    }

    public Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses GetKkmState(
      AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult.Result.DeviceStatus statusKkm)
    {
      if (statusKkm.coverOpened)
        return Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.CoverOpen;
      if (!statusKkm.paperPresent)
        return Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.NoPaper;
      return statusKkm.blocked ? Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.HardwareError : Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.Ready;
    }

    public bool OpenCashDrawer()
    {
      return this.CurrentDriver.SendCommand((AtolServerDriver.AtolServerCommand) new AtolServerDriver.KkmOpenCashDrawerCommand());
    }

    public bool SendDigitalCheck(string adress)
    {
      Gbs.Core.Config.FiscalKkm fiscalKkm = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm;
      if (this.CurrentCheck.Command.clientInfo == null)
        this.CurrentCheck.Command.clientInfo = new AtolServerDriver.ClientInfo()
        {
          emailOrPhone = adress
        };
      else
        this.CurrentCheck.Command.clientInfo.emailOrPhone = adress;
      if (fiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
        this.CurrentCheck.Command.electronically = true;
      return true;
    }

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    public string PrepareMarkCodeForFfd120(string code)
    {
      return DataMatrixHelper.ReplaceSomeCharsToFNC1(code);
    }

    private void GetSalesNotice(string info)
    {
      if (info.IsNullOrEmpty() || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        return;
      this._industryInfo = new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.IndustryInfo()
      {
        date = "2023.11.21",
        number = "1944",
        fois = "030",
        industryAttribute = info
      };
    }
  }
}
