// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Mercury
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class Mercury : IFiscalKkm, IDevice
  {
    private MercuryDriver _currentDriver;
    private MercuryDriver.KkmCloseCheck.Payments _payment = new MercuryDriver.KkmCloseCheck.Payments();
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;
    private string _sendCheckTo;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.Mercury_Name_Меркурий;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      switch (new MercuryDriver.KkmGetCommonInfo()
      {
        sessionKey = this._currentDriver.SessionKey
      }.Data.ffdFnVer)
      {
        case "1.0":
          return GlobalDictionaries.Devices.FfdVersions.Ffd100;
        case "1.0.5":
          return GlobalDictionaries.Devices.FfdVersions.Ffd105;
        case "1.1":
          return GlobalDictionaries.Devices.FfdVersions.Ffd110;
        case "1.2":
          return GlobalDictionaries.Devices.FfdVersions.Ffd120;
        default:
          return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
      }
    }

    public bool IsCanHoldConnection => false;

    public static void RestartService()
    {
      try
      {
        foreach (Process process in Process.GetProcesses())
        {
          if (process.ProcessName.ToLower().Contains("inecrman"))
            process.Kill();
        }
        ServiceController serviceController = new ServiceController("VT_inecrman");
        TimeSpan timeout = TimeSpan.FromMinutes(1.0);
        if (serviceController.Status != ServiceControllerStatus.Stopped)
        {
          LogHelper.Debug("Перезапуск службы VT_inecrman. Останавливаем службу...");
          serviceController.Stop();
          serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
          LogHelper.Debug("Служба VT_inecrman была успешно остановлена!");
        }
        if (serviceController.Status == ServiceControllerStatus.Running)
          return;
        LogHelper.Debug("Перезапуск службы VT_inecrman. Запускаем службу...");
        serviceController.Start();
        serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
        LogHelper.Debug("Служба VT_inecrman была успешно запущена!");
      }
      catch
      {
        LogHelper.Debug("Недостаточно прав для перезапуска службы или возникла другая ошибка во время перезапуска");
      }
    }

    private void CheckResult(MercuryDriver.MercuryAnswer answer)
    {
      if (answer.result != 0)
      {
        if (answer.result == 3001)
        {
          LogHelper.Debug("Закрываем неактивные порты");
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) new MercuryDriver.KkmClosePorts());
          Mercury.RestartService();
        }
        string str = string.Format(Translate.Код___0____Описание___1_, (object) answer.result, (object) answer.description);
        KkmException.ErrorTypes key = answer.result != 3001 ? KkmException.ErrorTypes.Unknown : KkmException.ErrorTypes.ConnectionError;
        throw new ErrorHelper.GbsException(KkmException.ErrorsDictionary[key] + "\n" + str)
        {
          Direction = key == KkmException.ErrorTypes.Unknown ? ErrorHelper.ErrorDirections.Unknown : ErrorHelper.ErrorDirections.Outer
        };
      }
    }

    private Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses GetStatusKkm(
      MercuryDriver.KkmGetInfo info)
    {
      return !info.Data.paperPresence ? Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.NoPaper : Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.Ready;
    }

    private Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses GetStatusSession(
      MercuryDriver.KkmGetInfo info)
    {
      if (info.Data.shiftInfo.is24Expired)
        return Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.OpenMore24Hours;
      return info.Data.shiftInfo.isOpen ? Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Open : Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Close;
    }

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult();

    public void ShowProperties()
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(devices.CheckPrinter.Connection.LanPort, devices.CheckPrinter.Connection.ComPort, ConnectionSettingsViewModel.PortsConfig.ComAndLan));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Открытие сессии кассы меркурий");
      MercuryDriver.KkmOpenShift command = new MercuryDriver.KkmOpenShift()
      {
        sessionKey = this._currentDriver.SessionKey,
        cashierInfo = new MercuryDriver.CashierInfo(cashier)
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
    }

    public void GetReport(Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.ZReport:
          MercuryDriver.KkmCloseShift command1 = new MercuryDriver.KkmCloseShift()
          {
            sessionKey = this._currentDriver.SessionKey,
            cashierInfo = new MercuryDriver.CashierInfo(cashier)
          };
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command1);
          this.CheckResult((MercuryDriver.MercuryAnswer) command1.Data);
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReport:
          MercuryDriver.KkmGetReport command2 = new MercuryDriver.KkmGetReport()
          {
            sessionKey = this._currentDriver.SessionKey,
            reportCode = (int) reportType
          };
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command2);
          this.CheckResult((MercuryDriver.MercuryAnswer) command2.Data);
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 1.2, Меркурий");
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) new MercuryDriver.AbortMarkingCodeChecking()
      {
        sessionKey = this._currentDriver.SessionKey
      });
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) new MercuryDriver.ClearMarkingCodeValidationTable()
      {
        sessionKey = this._currentDriver.SessionKey
      });
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
      {
        checkGood.MarkedInfo.ValidationResultKkm = (object) null;
        string fnC1 = DataMatrixHelper.ReplaceSomeCharsToFNC1(checkGood.MarkedInfo.FullCode);
        int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
        LogHelper.Debug("Валидация кода: " + fnC1);
        MercuryDriver.CheckMarkingCode command1 = new MercuryDriver.CheckMarkingCode()
        {
          sessionKey = this._currentDriver.SessionKey,
          mc = RuOnlineKkmHelper.Base64Encode(fnC1),
          plannedStatus = markingCodeStatus,
          timeout = 30,
          measureUnit = checkGood.Unit.RuFfdUnitsIndex,
          qty = (int) (checkGood.Quantity * 10000M)
        };
        if (markingCodeStatus.IsEither<int>(2, 4))
        {
          int ruFfdUnitsIndex = checkGood.Unit.RuFfdUnitsIndex;
        }
        this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command1);
        LogHelper.Debug(command1.Data.ToJsonString(true));
        if (command1.Data.result != 0)
        {
          LogHelper.Debug("Ошибка при проверке маркирвоки " + command1.Data.result.ToString() + " (" + command1.Data.description + ")");
          break;
        }
        bool flag = false;
        MercuryDriver.GetMarkingCodeCheckResult command2 = new MercuryDriver.GetMarkingCodeCheckResult()
        {
          sessionKey = this._currentDriver.SessionKey
        };
        for (int index = 0; index < 30; ++index)
        {
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command2);
          if (command2.Data.result != 0)
          {
            LogHelper.Debug("Ошибка при проверке маркирвоки " + command2.Data.result.ToString() + " (" + command2.Data.description + ")");
            break;
          }
          if (command2.Data.isCompleted)
          {
            flag = true;
            break;
          }
          Thread.Sleep(1000);
        }
        if (flag)
        {
          MercuryDriver.GetMarkingCodeCheckResult.GetMarkingCodeCheckResultAnswer.OnlineCheck onlineCheck = command2.Data.onlineCheck;
          int result = command2.Data.onlineCheck.result;
          string description = command2.Data.onlineCheck.description;
          int checkResult = command1.Data.fnCheck.checkResult;
          LogHelper.Debug("Проверка кода маркировки закончилась.");
          LogHelper.Debug("ErrorOnlineResult: " + result.ToString() + ", " + description);
          LogHelper.Debug("ErrorOfflineResult: " + checkResult.ToString());
          checkGood.MarkedInfo.ValidationResultKkm = (object) onlineCheck;
          LogHelper.Debug(string.Format("Validation ready: {0}; result code: {1}", (object) flag, (object) checkGood.MarkedInfo.ValidationResultKkm.ToJsonString(true)));
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) new MercuryDriver.AcceptMarkingCode()
          {
            sessionKey = this._currentDriver.SessionKey
          });
        }
        else
        {
          LogHelper.Debug("Проверка кода не завершена, таймаут проверки, отменяем проверку");
          this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) new MercuryDriver.AbortMarkingCodeChecking()
          {
            sessionKey = this._currentDriver.SessionKey
          });
          break;
        }
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (devices.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.Ffd120CodeValidation(checkData);
      this._payment = new MercuryDriver.KkmCloseCheck.Payments();
      MercuryDriver.KkmOpenCheck command = new MercuryDriver.KkmOpenCheck()
      {
        sessionKey = this._currentDriver.SessionKey,
        buyerInfo = new MercuryDriver.BuyerInfo(checkData?.Client?.Client ?? new Gbs.Core.Entities.Clients.Client()),
        cashierInfo = new MercuryDriver.CashierInfo(checkData?.Cashier ?? new Gbs.Core.Devices.CheckPrinters.Cashier()),
        printDoc = !devices.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck
      };
      this._sendCheckTo = checkData?.AddressForDigitalCheck;
      if (!this._sendCheckTo.IsNullOrEmpty() && devices.CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
        command.printDoc = false;
      MercuryDriver.KkmOpenCheck kkmOpenCheck1 = command;
      GlobalDictionaries.RuTaxSystems? ruTaxSystem = checkData?.RuTaxSystem;
      if (ruTaxSystem.HasValue)
      {
        int num1;
        switch (ruTaxSystem.GetValueOrDefault())
        {
          case GlobalDictionaries.RuTaxSystems.None:
            num1 = 0;
            break;
          case GlobalDictionaries.RuTaxSystems.Osn:
            num1 = 0;
            break;
          case GlobalDictionaries.RuTaxSystems.UsnDohod:
            num1 = 1;
            break;
          case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
            num1 = 2;
            break;
          case GlobalDictionaries.RuTaxSystems.Envd:
            num1 = 3;
            break;
          case GlobalDictionaries.RuTaxSystems.Esn:
            num1 = 4;
            break;
          case GlobalDictionaries.RuTaxSystems.Psn:
            num1 = 5;
            break;
          default:
            goto label_13;
        }
        kkmOpenCheck1.taxSystem = num1;
        MercuryDriver.KkmOpenCheck kkmOpenCheck2 = command;
        int num2;
        switch (checkData.CheckType)
        {
          case CheckTypes.Sale:
            num2 = 0;
            break;
          case CheckTypes.ReturnSale:
            num2 = 1;
            break;
          case CheckTypes.Buy:
            num2 = 2;
            break;
          case CheckTypes.ReturnBuy:
            num2 = 3;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        kkmOpenCheck2.checkType = num2;
        this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
        this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
        return true;
      }
label_13:
      throw new ArgumentOutOfRangeException();
    }

    public bool CloseCheck()
    {
      MercuryDriver.KkmCloseCheck command = new MercuryDriver.KkmCloseCheck()
      {
        payment = this._payment,
        sessionKey = this._currentDriver.SessionKey,
        sendCheckTo = this._sendCheckTo
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this._payment = new MercuryDriver.KkmCloseCheck.Payments();
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
      return true;
    }

    public void CancelCheck()
    {
      MercuryDriver.KkmCancelCheck command = new MercuryDriver.KkmCancelCheck()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      MercuryDriver.KkmWithdrawMoney command = new MercuryDriver.KkmWithdrawMoney()
      {
        sessionKey = this._currentDriver.SessionKey,
        cashierInfo = new MercuryDriver.CashierInfo(cashier),
        cash = (int) (sum * 100M)
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      MercuryDriver.KkmBringMoney command = new MercuryDriver.KkmBringMoney()
      {
        sessionKey = this._currentDriver.SessionKey,
        cashierInfo = new MercuryDriver.CashierInfo(cashier),
        cash = (int) (sum * 100M)
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
      return true;
    }

    public bool WriteOfdAttribute(Gbs.Core.Devices.CheckPrinters.FiscalKkm.OfdAttributes ofdAttribute, object value)
    {
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      if (good == null)
        return false;
      GlobalDictionaries.Devices.FfdVersions ffdVersion = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion;
      MercuryDriver.KkmAddGood kkmAddGood = new MercuryDriver.KkmAddGood();
      kkmAddGood.sessionKey = this._currentDriver.SessionKey;
      kkmAddGood.sum = (int) (Math.Round(good.Quantity * good.Price * 100M, 0, MidpointRounding.AwayFromZero) - good.DiscountSum * 100M);
      kkmAddGood.price = (int) (good.Price * 100M);
      kkmAddGood.qty = (int) (good.Quantity * 10000M);
      kkmAddGood.productTypeCode = good.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None ? 1 : (int) good.RuFfdGoodTypeCode;
      kkmAddGood.productName = good.Name;
      kkmAddGood.paymentFormCode = (int) good.RuFfdPaymentModeCode;
      kkmAddGood.section = good.KkmSectionNumber;
      int? nullable;
      if (ffdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
      {
        nullable = new int?();
      }
      else
      {
        GoodsUnits.GoodUnit unit = good.Unit;
        nullable = new int?(unit != null ? unit.RuFfdUnitsIndex : 0);
      }
      kkmAddGood.measureUnit = nullable;
      MercuryDriver.KkmAddGood command = kkmAddGood;
      command.taxCode = good.TaxRateNumber;
      if (good.MarkedInfo != null && good.MarkedInfo.Type != GlobalDictionaries.RuMarkedProductionTypes.None && good.MarkedInfo.IsValidCode())
      {
        if (ffdVersion.IsEither<GlobalDictionaries.Devices.FfdVersions>(GlobalDictionaries.Devices.FfdVersions.Ffd100, GlobalDictionaries.Devices.FfdVersions.Ffd105, GlobalDictionaries.Devices.FfdVersions.Ffd110))
        {
          if (good.MarkedInfo.Type == GlobalDictionaries.RuMarkedProductionTypes.Tobacco)
          {
            command.nomenclatureCode = good.MarkedInfo.FullCode;
          }
          else
          {
            string str = "01" + good.MarkedInfo.Gtin + "\u001D21" + good.MarkedInfo.Serial + "\u001D";
            if (good.MarkedInfo.CheckKey.Length > 0)
              str = str + "91" + good.MarkedInfo.CheckKey + "\u001D";
            LogHelper.Debug("Маркирвока на Меркурий (не табак) " + str);
            command.nomenclatureCode = str;
          }
        }
        if (ffdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        {
          string fnC1 = DataMatrixHelper.ReplaceSomeCharsToFNC1(good.MarkedInfo.FullCode);
          int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(good, checkType);
          command.mcInfo = new MercuryDriver.KkmAddGood.McInfo()
          {
            mc = RuOnlineKkmHelper.Base64Encode(fnC1),
            plannedStatus = markingCodeStatus
          };
          if (!this._checkData.TrueApiInfoForKkm.IsNullOrEmpty())
            command.industryAttribute = new List<MercuryDriver.KkmAddGood.IndustryAttributeItem>()
            {
              new MercuryDriver.KkmAddGood.IndustryAttributeItem()
              {
                value = this._checkData.TrueApiInfoForKkm,
                docDate = "2023-11-21",
                docNum = "1944",
                idFOIV = "030"
              }
            };
        }
      }
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      int num = (int) (payment.Sum * 100M);
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          this._payment.cash += num;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
        case GlobalDictionaries.KkmPaymentMethods.Bank:
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          this._payment.ecash += num;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          this._payment.credit += num;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          this._payment.prepayment += num;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._currentDriver = new MercuryDriver(dc);
      if (onlyDriverLoad)
        return;
      MercuryDriver.KkmOpenSession command = new MercuryDriver.KkmOpenSession()
      {
        portName = dc.CheckPrinter.Connection.ComPort.PortName,
        model = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.Model ?? throw new KkmException((IDevice) this, Translate.Mercury_Не_удалось_подключиться_к_устройству__Необходимо_в_настройках_указать_модель_ККМ)
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this._currentDriver.SessionKey = command.Data.sessionKey;
      LogHelper.Debug("sessionKey" + this._currentDriver.SessionKey);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
    }

    public bool Disconnect()
    {
      if (this._currentDriver == null)
        return true;
      if (this._currentDriver.SessionKey == null)
      {
        LogHelper.Debug("Ключ сессии не задан. Команда отключения игнорируется");
        return true;
      }
      MercuryDriver.KkmCloseSession command1 = new MercuryDriver.KkmCloseSession()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command1);
      this._currentDriver.SessionKey = (string) null;
      this.CheckResult((MercuryDriver.MercuryAnswer) command1.Data);
      LogHelper.Debug("Закрываем неактивные порты");
      MercuryDriver.KkmClosePorts command2 = new MercuryDriver.KkmClosePorts();
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command2);
      this.CheckResult((MercuryDriver.MercuryAnswer) command2.Data);
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<Gbs.Core.Devices.CheckPrinters.NonFiscalString> nonFiscalStrings)
    {
      MercuryDriver.KkmPrintText command = new MercuryDriver.KkmPrintText()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      int count = nonFiscalStrings.Count;
      foreach (Gbs.Core.Devices.CheckPrinters.NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        command.text = nonFiscalString.Text;
        command.forcePrint = count == 1;
        this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
        this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
        --count;
      }
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      MercuryDriver.KkmPrintBarCode command = new MercuryDriver.KkmPrintBarCode()
      {
        value = code,
        sessionKey = this._currentDriver.SessionKey
      };
      switch (type)
      {
        case BarcodeTypes.None:
          return true;
        case BarcodeTypes.Ean13:
          command.bcType = 2;
          break;
        case BarcodeTypes.QrCode:
          command.bcType = 4;
          break;
      }
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      this.CheckResult((MercuryDriver.MercuryAnswer) command.Data);
      return true;
    }

    public bool CutPaper() => true;

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetStatus()
    {
      MercuryDriver.KkmGetInfo kkmGetInfo = new MercuryDriver.KkmGetInfo()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      MercuryDriver.KkmGetCommonInfo command = new MercuryDriver.KkmGetCommonInfo()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) kkmGetInfo);
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      Gbs.Core.Devices.CheckPrinters.KkmStatus status = new Gbs.Core.Devices.CheckPrinters.KkmStatus();
      MercuryDriver.KkmGetInfo.KkmInfoAnswer.CheckInfo checkInfo1 = kkmGetInfo.Data.checkInfo;
      status.CheckNumber = checkInfo1 != null ? checkInfo1.num : 0;
      MercuryDriver.KkmGetInfo.KkmInfoAnswer.CheckInfo checkInfo2 = kkmGetInfo.Data.checkInfo;
      status.CheckStatus = (checkInfo2 != null ? (checkInfo2.isOpen ? 1 : 0) : 0) != 0 ? Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Open : Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Close;
      status.Model = command.Data.model;
      status.FactoryNumber = command.Data.kktNum;
      status.KkmState = this.GetStatusKkm(kkmGetInfo);
      status.OfdLastSendDateTime = new DateTime?(Convert.ToDateTime(kkmGetInfo.Data.fnInfo.unsignedDocs.firstDateTime));
      status.OfdNotSendDocuments = kkmGetInfo.Data.fnInfo.unsignedDocs.qty;
      status.SessionNumber = kkmGetInfo.Data.shiftInfo.isOpen ? kkmGetInfo.Data.shiftInfo.num : kkmGetInfo.Data.shiftInfo.num + 1;
      status.SessionStarted = new DateTime?(kkmGetInfo.Data.shiftInfo.lastOpen.IsNullOrEmpty() ? new DateTime() : Convert.ToDateTime(kkmGetInfo.Data.shiftInfo.lastOpen));
      status.SessionStatus = this.GetStatusSession(kkmGetInfo);
      status.SoftwareVersion = command.Data.programVer;
      return status;
    }

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetShortStatus()
    {
      MercuryDriver.KkmGetInfo kkmGetInfo = new MercuryDriver.KkmGetInfo()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) kkmGetInfo);
      return new Gbs.Core.Devices.CheckPrinters.KkmStatus()
      {
        KkmState = this.GetStatusKkm(kkmGetInfo),
        SessionStatus = this.GetStatusSession(kkmGetInfo)
      };
    }

    public bool OpenCashDrawer()
    {
      MercuryDriver.KkmOpenBox command = new MercuryDriver.KkmOpenBox()
      {
        sessionKey = this._currentDriver.SessionKey
      };
      this._currentDriver.SendCommand((MercuryDriver.MercuryCommand) command);
      return command.Data.result == 0;
    }

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
