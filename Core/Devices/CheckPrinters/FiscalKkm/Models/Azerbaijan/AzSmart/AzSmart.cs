// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart
{
  public class AzSmart : IFiscalKkm, IDevice
  {
    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    public AzSmart(Gbs.Core.Config.Devices devicesConfig)
    {
      string azSmartMerchantId = devicesConfig.CheckPrinter.FiscalKkm.AzSmartMerchantId;
      if (azSmartMerchantId.IsNullOrEmpty())
        throw new KkmException((IDevice) this, Translate.НеУказанMerchantIDПроверьтеНастройки, KkmException.ErrorTypes.None);
      this.Driver = new AzSmartDriver(devicesConfig.CheckPrinter.Connection.LanPort, azSmartMerchantId);
    }

    private void CheckResult(AzSmartDriver.AnswerAzSmart answer, string msg = "")
    {
      if (answer.Code != 0)
      {
        string str = msg;
        string deviceMessage = string.Format(Translate.Код___0____Описание___1_, (object) answer.Code, (object) str);
        int code = answer.Code;
        KkmException.ErrorTypes errorType = KkmException.ErrorTypes.Unknown;
        throw new KkmException((IDevice) this, deviceMessage, errorType);
      }
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "AZ SMART";

    private AzSmartDriver Driver { get; set; }

    private object Check { get; set; }

    public KkmLastActionResult LasActionResult
    {
      get
      {
        return new KkmLastActionResult()
        {
          ActionResult = ActionsResults.Done
        };
      }
    }

    public void ShowProperties()
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(devices.CheckPrinter.Connection.LanPort, devices.CheckPrinter.Connection.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyLan));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AzSmartDriver.OpenShiftCommand openShiftCommand = new AzSmartDriver.OpenShiftCommand();
      openShiftCommand.Data = (object) new AzSmartDriver.OpenShiftCommand.OpenShiftData()
      {
        EmployeeName = cashier.Name
      };
      AzSmartDriver.OpenShiftCommand command = openShiftCommand;
      this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command);
      this.CheckResult((AzSmartDriver.AnswerAzSmart) command.Result);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          AzSmartDriver.CloseShiftCommand closeShiftCommand = new AzSmartDriver.CloseShiftCommand();
          closeShiftCommand.Data = (object) new AzSmartDriver.CloseShiftCommand.CloseShiftData()
          {
            EmployeeName = cashier.Name
          };
          AzSmartDriver.CloseShiftCommand command1 = closeShiftCommand;
          this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command1);
          this.CheckResult((AzSmartDriver.AnswerAzSmart) command1.Result);
          break;
        case ReportTypes.XReport:
          AzSmartDriver.XReportCommand xreportCommand = new AzSmartDriver.XReportCommand();
          xreportCommand.Data = (object) new AzSmartDriver.XReportCommand.XReportData();
          AzSmartDriver.XReportCommand command2 = xreportCommand;
          this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command2);
          this.CheckResult((AzSmartDriver.AnswerAzSmart) command2.Result);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      AzSmartDriver.SaleCommand.SaleData saleData;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          saleData = new AzSmartDriver.SaleCommand.SaleData()
          {
            Amount = (int) (checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) * 100M),
            DocNumber = checkData.Number,
            DocTime = checkData.DateTime.ToString("yyyy-MM-dd hh:mm:ss"),
            EmployeeName = checkData.Cashier.Name
          };
          break;
        case CheckTypes.ReturnSale:
          AzSmartDriver.ReturnCommand.ReturnData returnData = new AzSmartDriver.ReturnCommand.ReturnData();
          returnData.Amount = (int) (checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) * 100M);
          returnData.DocNumber = checkData.Number;
          returnData.DocTime = checkData.DateTime.ToString("yyyy-MM-dd hh:mm:ss");
          returnData.EmployeeName = checkData.Cashier.Name;
          returnData.ParentDocID = checkData.FiscalNum;
          saleData = (AzSmartDriver.SaleCommand.SaleData) returnData;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.Check = (object) saleData;
      return true;
    }

    public bool CloseCheck() => true;

    public bool CloseCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      if (this.Check.GetType() == typeof (AzSmartDriver.SaleCommand.SaleData))
      {
        AzSmartDriver.SaleCommand saleCommand = new AzSmartDriver.SaleCommand();
        saleCommand.Data = this.Check;
        AzSmartDriver.SaleCommand command = saleCommand;
        this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command);
        this.CheckResult((AzSmartDriver.AnswerAzSmart) command.Result, command.Result.Message);
        data.FiscalNum = command.Result.FiscalID;
      }
      else
      {
        AzSmartDriver.ReturnCommand returnCommand = new AzSmartDriver.ReturnCommand();
        returnCommand.Data = this.Check;
        AzSmartDriver.ReturnCommand command = returnCommand;
        this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command);
        this.CheckResult((AzSmartDriver.AnswerAzSmart) command.Result, command.Result.Message);
      }
      return true;
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AzSmartDriver.OutputCommand outputCommand = new AzSmartDriver.OutputCommand();
      outputCommand.Data = (object) new AzSmartDriver.InputCommand.DepositData()
      {
        EmployeeName = cashier.Name,
        Amount = ((int) sum * 100)
      };
      AzSmartDriver.OutputCommand command = outputCommand;
      this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command);
      this.CheckResult((AzSmartDriver.AnswerAzSmart) command.Result);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      AzSmartDriver.InputCommand inputCommand = new AzSmartDriver.InputCommand();
      inputCommand.Data = (object) new AzSmartDriver.InputCommand.DepositData()
      {
        EmployeeName = cashier.Name,
        Amount = ((int) sum * 100)
      };
      AzSmartDriver.InputCommand command = inputCommand;
      this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command);
      this.CheckResult((AzSmartDriver.AnswerAzSmart) command.Result);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates[good.TaxRateNumber];
      string str = good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value?.ToString() ?? "1";
      ((AzSmartDriver.SaleCommand.SaleData) this.Check).Items.Add(new AzSmartDriver.SaleCommand.SaleData.Item()
      {
        Discount = (int) (good.DiscountSum * 100M),
        ItemAmount = (int) (good.Sum * 100M),
        ItemId = str,
        ItemName = good.Name,
        ItemQty = (int) (good.Quantity * 1000M),
        ItemsTaxes = new List<AzSmartDriver.SaleCommand.SaleData.Item.ItemTax>()
        {
          new AzSmartDriver.SaleCommand.SaleData.Item.ItemTax()
          {
            TaxName = taxRate.Name,
            CalcType = 1,
            TaxPrc = (int) taxRate.TaxValue / 100
          }
        }
      });
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.CashAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.CashlessAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.CashlessAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.CreditAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.PrepaymentAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          ((AzSmartDriver.SaleCommand.SaleData) this.Check).Payments.CashlessAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      AzSmartDriver.CheckShiftCommand checkShiftCommand = new AzSmartDriver.CheckShiftCommand();
      checkShiftCommand.Data = (object) new AzSmartDriver.CheckShiftCommand.CheckShiftData();
      AzSmartDriver.CheckShiftCommand command1 = checkShiftCommand;
      this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command1);
      this.CheckResult((AzSmartDriver.AnswerAzSmart) command1.Result);
      AzSmartDriver.GetInfoCommand getInfoCommand = new AzSmartDriver.GetInfoCommand();
      getInfoCommand.Data = (object) new AzSmartDriver.GetInfoCommand.GetInfoData();
      AzSmartDriver.GetInfoCommand command2 = getInfoCommand;
      this.Driver.SendCommand((AzSmartDriver.IKkmAzSmart) command2);
      this.CheckResult((AzSmartDriver.AnswerAzSmart) command2.Result);
      return new KkmStatus()
      {
        SessionStatus = command1.Result.IsShiftOpen ? SessionStatuses.Open : SessionStatuses.Close,
        Model = command2.Result.CashRegisterModel,
        DriverVersion = new Version(command2.Result.CashBoxAppVersion),
        SoftwareVersion = command2.Result.FirmwareVersion,
        KkmState = command2.Result.State == "ACTIVE" ? KkmStatuses.Ready : KkmStatuses.Unknown
      };
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
