// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.SmartOne
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Settings.Devices;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class SmartOne : IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _data;
    private Gbs.Core.Config.Devices _devicesConfig;
    private SmartOneDriver _driver;
    private SmartOneDriver.DocumentCommand _documentCommand;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (SmartOne);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
    }

    public bool IsCanHoldConnection => false;

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      SmartOneDriver.OpenShiftCommand command = new SmartOneDriver.OpenShiftCommand()
      {
        UserName = cashier.Name
      };
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command);
      this.CheckError((SmartOneDriver.SmartOneAnswer) command.Result);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          SmartOneDriver.CloseShiftCommand command1 = new SmartOneDriver.CloseShiftCommand()
          {
            UserName = cashier.Name
          };
          this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command1);
          this.CheckError((SmartOneDriver.SmartOneAnswer) command1.Result);
          break;
        case ReportTypes.XReport:
          SmartOneDriver.XReportCommand command2 = new SmartOneDriver.XReportCommand();
          this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command2);
          this.CheckError((SmartOneDriver.SmartOneAnswer) command2.Result);
          break;
        case ReportTypes.XReportWithGoods:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._data = checkData;
      this._documentCommand = new SmartOneDriver.DocumentCommand()
      {
        DateTime = checkData.DateTime.AddHours(2.0),
        DocNumber = checkData.Number,
        EmployeeName = checkData.Cashier?.Name ?? "КАССИР",
        Amount = (int) (checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) * 100M),
        CheckType = checkData.CheckType
      };
      if (this._devicesConfig.CheckPrinter.FiscalKkm.IsSaveInfoClient)
      {
        this._documentCommand.ClientName = checkData.Client.Client.Name;
        this._documentCommand.ClientPhone = checkData.Client.Client.Phone;
      }
      if (checkData.CheckType == CheckTypes.ReturnSale)
        this._documentCommand.ParentDocId = checkData.FiscalNum;
      return true;
    }

    public bool CreditPay(
      List<SelectPaymentMethods.PaymentGrid> paymentsList,
      Gbs.Core.Entities.Users.User user,
      string docId)
    {
      SmartOneDriver.CreditPayCommand creditPayCommand = new SmartOneDriver.CreditPayCommand();
      creditPayCommand.DateTime = DateTime.Now;
      creditPayCommand.EmployeeName = user.Client?.Name ?? "КАССИР";
      creditPayCommand.Amount = (int) (paymentsList.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => ((Decimal?) x?.Sum).GetValueOrDefault())) * 100M);
      creditPayCommand.ParentDocId = docId;
      this._documentCommand = (SmartOneDriver.DocumentCommand) creditPayCommand;
      Decimal? sum1;
      Decimal? nullable1;
      foreach (SelectPaymentMethods.PaymentGrid paymentGrid in paymentsList.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
      {
        if (x == null)
          return false;
        Decimal? sum2 = x.Sum;
        Decimal num = 0M;
        return sum2.GetValueOrDefault() > num & sum2.HasValue;
      })))
      {
        switch (paymentGrid.Type)
        {
          case GlobalDictionaries.KkmPaymentMethods.Cash:
            SmartOneDriver.DocumentPayments payments1 = this._documentCommand.Payments;
            int cashAmount = payments1.CashAmount;
            sum1 = paymentGrid.Sum;
            Decimal num1 = (Decimal) 100;
            Decimal? nullable2;
            if (!sum1.HasValue)
            {
              nullable1 = new Decimal?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new Decimal?(sum1.GetValueOrDefault() * num1);
            nullable1 = nullable2;
            int num2 = (int) nullable1.Value;
            payments1.CashAmount = cashAmount + num2;
            continue;
          case GlobalDictionaries.KkmPaymentMethods.Card:
            SmartOneDriver.DocumentPayments payments2 = this._documentCommand.Payments;
            int cashlessAmount = payments2.CashlessAmount;
            sum1 = paymentGrid.Sum;
            Decimal num3 = (Decimal) 100;
            Decimal? nullable3;
            if (!sum1.HasValue)
            {
              nullable1 = new Decimal?();
              nullable3 = nullable1;
            }
            else
              nullable3 = new Decimal?(sum1.GetValueOrDefault() * num3);
            nullable1 = nullable3;
            int num4 = (int) nullable1.Value;
            payments2.CashlessAmount = cashlessAmount + num4;
            continue;
          case GlobalDictionaries.KkmPaymentMethods.Bank:
            SmartOneDriver.DocumentPayments payments3 = this._documentCommand.Payments;
            int invoiceAmount = payments3.InvoiceAmount;
            sum1 = paymentGrid.Sum;
            Decimal num5 = (Decimal) 100;
            Decimal? nullable4;
            if (!sum1.HasValue)
            {
              nullable1 = new Decimal?();
              nullable4 = nullable1;
            }
            else
              nullable4 = new Decimal?(sum1.GetValueOrDefault() * num5);
            nullable1 = nullable4;
            int num6 = (int) nullable1.Value;
            payments3.InvoiceAmount = invoiceAmount + num6;
            continue;
          default:
            continue;
        }
      }
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) this._documentCommand);
      this.CheckError((SmartOneDriver.SmartOneAnswer) this._documentCommand.Result);
      return true;
    }

    public bool CloseCheck()
    {
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) this._documentCommand);
      this.CheckError((SmartOneDriver.SmartOneAnswer) this._documentCommand.Result);
      this._data.FiscalNum = this._documentCommand.Result.FiscalId;
      return true;
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      SmartOneDriver.WithdrawCommand command = new SmartOneDriver.WithdrawCommand()
      {
        UserName = cashier.Name,
        Amount = (int) (sum * 100M),
        Currency = "UZS"
      };
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command);
      this.CheckError((SmartOneDriver.SmartOneAnswer) command.Result);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      SmartOneDriver.DepositCommand command = new SmartOneDriver.DepositCommand()
      {
        UserName = cashier.Name,
        Amount = (int) (sum * 100M),
        Currency = "UZS"
      };
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command);
      this.CheckError((SmartOneDriver.SmartOneAnswer) command.Result);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      sum = -1M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      ConfigsRepository<Gbs.Core.Config.Devices> cr = new ConfigsRepository<Gbs.Core.Config.Devices>();
      Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRates = cr.Get().CheckPrinter.FiscalKkm.TaxRates;
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = taxRates.SingleOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == good.TaxRateNumber)).Value ?? taxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == cr.Get().CheckPrinter.FiscalKkm.DefaultTaxRate)).Value;
      List<SmartOneDriver.DocumentItem> items = this._documentCommand.Items;
      SmartOneDriver.DocumentItem documentItem = new SmartOneDriver.DocumentItem();
      documentItem.ItemId = good.Good.Id.ToString();
      documentItem.ItemName = good.Name;
      documentItem.ItemUnit = good.Unit?.ShortName;
      documentItem.ItemUnitCode = good.Unit?.Code;
      documentItem.ItemBarcode = good.Barcode;
      documentItem.ItemQty = (int) (good.Quantity * 1000M);
      documentItem.ItemAmount = (int) (good.Sum * 100M);
      documentItem.Discount = (int) (good.DiscountSum * 100M);
      List<EntityProperties.PropertyValue> properties = good.Good.Properties;
      documentItem.ItemCode = (properties != null ? properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.IkpuUid))?.Value?.ToString() : (string) null) ?? "";
      documentItem.ItemTaxes = new List<SmartOneDriver.DocumentItem.Tax>()
      {
        new SmartOneDriver.DocumentItem.Tax()
        {
          TaxName = taxRate.Name,
          TaxPrc = (int) (taxRate.TaxValue * 100M),
          CalcType = 1
        }
      };
      items.Add(documentItem);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          this._documentCommand.Payments.CashAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          this._documentCommand.Payments.CashlessAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          this._documentCommand.Payments.InvoiceAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          this._documentCommand.Payments.CreditAmount += (int) (payment.Sum * 100M);
          goto case GlobalDictionaries.KkmPaymentMethods.Bonus;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      this._documentCommand.Payments.BonusesAmount += (int) (sum * 100M);
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (this._devicesConfig == null)
        this._devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._driver = new SmartOneDriver(this._devicesConfig.CheckPrinter.Connection.LanPort)
      {
        MerchantId = this._devicesConfig.CheckPrinter.FiscalKkm.Model
      };
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    public KkmStatus GetStatus()
    {
      SmartOneDriver.CheckShiftCommand command = new SmartOneDriver.CheckShiftCommand()
      {
        UserName = "КАССИР"
      };
      this._driver.DoCommand((SmartOneDriver.SmartOneCommand) command);
      this.CheckError((SmartOneDriver.SmartOneAnswer) command.Result);
      KkmStatus status = new KkmStatus();
      KkmStatus kkmStatus = status;
      SessionStatuses sessionStatuses;
      switch (command.Result.IsShiftOpen)
      {
        case "true":
          sessionStatuses = SessionStatuses.Open;
          break;
        case "false":
          sessionStatuses = SessionStatuses.Close;
          break;
        default:
          sessionStatuses = SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      return status;
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => true;

    private void CheckError(SmartOneDriver.SmartOneAnswer answer)
    {
      if (!(answer.Status == "success") && answer.Code != 0)
        throw new KkmException((IDevice) this, answer.Message + ", код ошибки - " + answer.Code.ToString());
    }
  }
}
