// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.KkmServer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class KkmServer : IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.KkmServer_Name_ККМ_Сервер;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    private KkmServerDriver CurrentDriver { get; set; }

    private KkmServerDriver.KkmPrintCheck CurrentCheckCommand { get; set; }

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult()
    {
      ActionResult = ActionsResults.Done
    };

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
      if (!this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmOpenSession()
      {
        CashierName = cashier.Name,
        CashierVATIN = cashier.Inn
      }))
        throw new InvalidOperationException(Translate.KkmServer_Не_удалось_открыть_смену_на_ККМ_Сервер);
    }

    public void GetReport(Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      bool flag;
      switch (reportType)
      {
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.ZReport:
          flag = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmCloseShift()
          {
            CashierName = cashier.Name,
            CashierVATIN = cashier.Inn
          });
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReport:
          flag = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmGetXReport());
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType));
      }
      if (!flag)
        throw new KkmException((IDevice) this, Translate.KkmHelper_Не_удалось_снять_отчет);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      Gbs.Core.Config.FiscalKkm fiscalKkm = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm;
      if (checkData.Cashier.Name.IsNullOrEmpty())
        checkData.Cashier.Name = Translate.CheckData_Кассир;
      this.CurrentCheckCommand = new KkmServerDriver.KkmPrintCheck()
      {
        CashierName = checkData.Cashier.Name,
        CashierVATIN = checkData.Cashier.Inn,
        NotPrint = fiscalKkm.IsAlwaysNoPrintCheck
      };
      if (checkData.Client != null)
      {
        this.CurrentCheckCommand.ClientInfo = checkData.Client.Client.Name;
        this.CurrentCheckCommand.ClientINN = checkData.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
      }
      if (!checkData.AddressForDigitalCheck.IsNullOrEmpty())
      {
        this.CurrentCheckCommand.ClientAddress = checkData.AddressForDigitalCheck;
        if (fiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
          this.CurrentCheckCommand.NotPrint = true;
      }
      KkmServerDriver.KkmPrintCheck currentCheckCommand = this.CurrentCheckCommand;
      int num1;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          num1 = 0;
          break;
        case CheckTypes.ReturnSale:
          num1 = 1;
          break;
        case CheckTypes.Buy:
          num1 = 10;
          break;
        case CheckTypes.ReturnBuy:
          num1 = 11;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      currentCheckCommand.TypeCheck = num1;
      this.CurrentCheckCommand.IsFiscalCheck = checkData.FiscalType == CheckFiscalTypes.Fiscal;
      if (checkData.RuTaxSystem != GlobalDictionaries.RuTaxSystems.None)
        this.CurrentCheckCommand.TaxVariant = new int?((int) checkData.RuTaxSystem);
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      foreach (CheckGood goods in checkData.GoodsList)
      {
        int num2;
        switch (goods.TaxRateNumber)
        {
          case 1:
            num2 = -1;
            break;
          case 2:
            num2 = 0;
            break;
          case 3:
            num2 = 10;
            break;
          case 4:
            num2 = 20;
            break;
          case 5:
            num2 = 110;
            break;
          case 6:
            num2 = 120;
            break;
          case 7:
            num2 = 5;
            break;
          case 8:
            num2 = 7;
            break;
          case 9:
            num2 = 105;
            break;
          case 10:
            num2 = 107;
            break;
          default:
            num2 = -1;
            break;
        }
        int num3 = num2;
        KkmServerDriver.KkmPrintCheck.CheckString checkString1 = new KkmServerDriver.KkmPrintCheck.CheckString();
        KkmServerDriver.KkmPrintCheck.Register register = new KkmServerDriver.KkmPrintCheck.Register();
        register.Name = goods.Name;
        register.Price = goods.Price;
        register.Quantity = goods.Quantity;
        register.Amount = Math.Round(goods.Quantity * goods.Price, 2, MidpointRounding.AwayFromZero) - goods.DiscountSum;
        register.Department = goods.KkmSectionNumber;
        register.SignCalculationObject = goods.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None ? 1 : (int) goods.RuFfdGoodTypeCode;
        register.SignMethodCalculation = (int) goods.RuFfdPaymentModeCode;
        register.Tax = num3;
        int? nullable;
        if (devices.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        {
          nullable = new int?();
        }
        else
        {
          GoodsUnits.GoodUnit unit = goods.Unit;
          nullable = new int?(unit != null ? unit.RuFfdUnitsIndex : 0);
        }
        register.MeasureOfQuantity = nullable;
        checkString1.Register = register;
        KkmServerDriver.KkmPrintCheck.CheckString checkString2 = checkString1;
        if (goods.MarkedInfo != null && goods.MarkedInfo.Type != GlobalDictionaries.RuMarkedProductionTypes.None && goods.MarkedInfo.IsValidCode())
        {
          string str1;
          switch (goods.MarkedInfo.Type)
          {
            case GlobalDictionaries.RuMarkedProductionTypes.None:
              str1 = string.Empty;
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Fur:
              str1 = string.Empty;
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
              str1 = "444d";
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
              str1 = "444d";
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
              str1 = "444d";
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
              str1 = "444d";
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.Tires:
              str1 = "444d";
              break;
            case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
              str1 = "444d";
              break;
            default:
              str1 = "444d";
              break;
          }
          string str2 = str1;
          if (devices.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
            checkString2.Register.GoodCodeData = new KkmServerDriver.KkmPrintCheck.Register.GoodCode()
            {
              BarCode = goods.MarkedInfo.FullCode,
              AcceptOnBad = new bool?(true),
              ContainsSerialNumber = new bool?(false),
              IndustryProps = checkData.TrueApiInfoForKkm
            };
          else
            checkString2.Register.GoodCodeData = new KkmServerDriver.KkmPrintCheck.Register.GoodCode()
            {
              StampType = str2,
              GTIN = goods.MarkedInfo.Gtin,
              SerialNumber = goods.MarkedInfo.Serial
            };
        }
        this.CurrentCheckCommand.CheckStrings.Add(checkString2);
        this.PrintNonFiscalStringsLossOpen(goods.CommentForFiscalCheck.Select<string, Gbs.Core.Devices.CheckPrinters.NonFiscalString>((Func<string, Gbs.Core.Devices.CheckPrinters.NonFiscalString>) (x => new Gbs.Core.Devices.CheckPrinters.NonFiscalString(x))).ToList<Gbs.Core.Devices.CheckPrinters.NonFiscalString>());
      }
      this.CurrentCheckCommand.Cash = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      this.CurrentCheckCommand.ElectronicPayment = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bank, GlobalDictionaries.KkmPaymentMethods.Card, GlobalDictionaries.KkmPaymentMethods.EMoney))).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      this.CurrentCheckCommand.AdvancePayment = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.PrePayment)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      this.CurrentCheckCommand.Credit = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Credit)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      return true;
    }

    public bool CloseCheck()
    {
      return this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) this.CurrentCheckCommand);
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      return this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmCashOut()
      {
        Amount = sum,
        CashierName = cashier.Name,
        CashierVATIN = cashier.Inn
      });
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      return this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmCashIn()
      {
        Amount = sum,
        CashierName = cashier.Name,
        CashierVATIN = cashier.Inn
      });
    }

    public bool WriteOfdAttribute(Gbs.Core.Devices.CheckPrinters.FiscalKkm.OfdAttributes ofdAttribute, object value)
    {
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      KkmServerDriver.KkmGetInfo command = new KkmServerDriver.KkmGetInfo();
      int num = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command) ? 1 : 0;
      sum = command.Data.Info.BalanceCash;
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
      this.CurrentDriver = new KkmServerDriver(dc.CheckPrinter.Connection.LanPort);
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStringsLossOpen(List<Gbs.Core.Devices.CheckPrinters.NonFiscalString> nonFiscalStrings)
    {
      foreach (Gbs.Core.Devices.CheckPrinters.NonFiscalString nonFiscalString in nonFiscalStrings)
        this.CurrentCheckCommand.CheckStrings.Add(new KkmServerDriver.KkmPrintCheck.CheckString()
        {
          PrintText = new KkmServerDriver.KkmPrintCheck.PrintText()
          {
            Text = nonFiscalString.Text
          }
        });
    }

    public void PrintNonFiscalStrings(List<Gbs.Core.Devices.CheckPrinters.NonFiscalString> nonFiscalStrings)
    {
      this.CurrentCheckCommand = new KkmServerDriver.KkmPrintCheck();
      this.PrintNonFiscalStringsLossOpen(nonFiscalStrings);
      this.CloseCheck();
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      this.CurrentCheckCommand = new KkmServerDriver.KkmPrintCheck();
      List<KkmServerDriver.KkmPrintCheck.CheckString> checkStrings = this.CurrentCheckCommand.CheckStrings;
      KkmServerDriver.KkmPrintCheck.CheckString checkString1 = new KkmServerDriver.KkmPrintCheck.CheckString();
      KkmServerDriver.KkmPrintCheck.CheckString checkString2 = checkString1;
      KkmServerDriver.KkmPrintCheck.BarCode barCode1 = new KkmServerDriver.KkmPrintCheck.BarCode();
      barCode1.Barcode = code;
      KkmServerDriver.KkmPrintCheck.BarCode barCode2 = barCode1;
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
      barCode2.BarcodeType = str;
      checkString2.BarCode = barCode1;
      checkStrings.Add(checkString1);
      return this.CloseCheck();
    }

    public bool CutPaper() => true;

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetShortStatus() => this.GetStatus();

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetStatus()
    {
      KkmServerDriver.KkmGetInfo command = new KkmServerDriver.KkmGetInfo();
      if (!this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command))
        throw new Exception(Translate.KkmServer_GetStatus_Не_удалось_получить_статус_ККМ_для_ККМ_Сервер);
      Gbs.Core.Devices.CheckPrinters.KkmStatus status = new Gbs.Core.Devices.CheckPrinters.KkmStatus();
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus = status;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses sessionStatuses;
      switch (command.Data.Info.SessionState)
      {
        case 1:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Close;
          break;
        case 2:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Open;
          break;
        case 3:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.OpenMore24Hours;
          break;
        default:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      status.CheckStatus = Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Close;
      status.CheckNumber = command.Data.CheckNumber;
      status.SessionNumber = command.Data.SessionNumber;
      status.SoftwareVersion = command.Data.Info.Firmware_Version;
      status.FnDateEnd = command.Data.Info.FN_DateEnd;
      return status;
    }

    public bool OpenCashDrawer()
    {
      return this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) new KkmServerDriver.KkmOpenDrawer());
    }

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
