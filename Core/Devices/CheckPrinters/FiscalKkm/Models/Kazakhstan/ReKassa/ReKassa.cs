// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan.ReKassa
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan
{
  public class ReKassa : IFiscalKkm, IDevice
  {
    private string _token;
    private int _idKassa;
    private string _password;
    private int _sessionNumber;
    private ReKassaDriver _driver;
    private ReKassaDriver.TicketCommand _ticket;
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _data;
    private string _modelName;
    private string _rnmKassa;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (ReKassa);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = true
      });
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      if (this._idKassa == 0 || this._sessionNumber == 0)
        this.GetStatus();
      switch (reportType)
      {
        case ReportTypes.ZReport:
          ReKassaDriver.ZReportCommand zreportCommand = new ReKassaDriver.ZReportCommand();
          zreportCommand.Token = this._token;
          zreportCommand.KassaId = this._idKassa;
          zreportCommand.SessionNumber = this._sessionNumber;
          zreportCommand.СashRegisterPassword = this._password;
          ReKassaDriver.ZReportCommand command1 = zreportCommand;
          this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command1);
          this.CheckError((ReKassaDriver.ReKassaCommand) command1);
          break;
        case ReportTypes.XReport:
          ReKassaDriver.XReportCommand xreportCommand = new ReKassaDriver.XReportCommand();
          xreportCommand.Token = this._token;
          xreportCommand.KassaId = this._idKassa;
          xreportCommand.SessionNumber = this._sessionNumber;
          ReKassaDriver.XReportCommand command2 = xreportCommand;
          this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command2);
          this.CheckError((ReKassaDriver.ReKassaCommand) command2);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      if (checkData.PaymentsList.Any<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Credit)))
        throw new KkmException((IDevice) this, Translate.ReKassa_OpenCheck_На_данной_ККМ_запрещена_продажа_в_кредит__требуется_полностью_внести_оплату_за_товар_и_повторить_печать_чека_);
      Decimal num = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.PrePayment, GlobalDictionaries.KkmPaymentMethods.Certificate, GlobalDictionaries.KkmPaymentMethods.Bonus))).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
      this._data = checkData;
      ReKassaDriver.TicketCommand ticketCommand1 = new ReKassaDriver.TicketCommand();
      ticketCommand1.Token = this._token;
      ticketCommand1.KassaId = this._idKassa;
      ticketCommand1.DateTime = new ReKassaDriver.TicketCommand.DateTimeClass(checkData.DateTime);
      ReKassaDriver.TicketCommand ticketCommand2 = ticketCommand1;
      string str;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          str = "OPERATION_SELL";
          break;
        case CheckTypes.ReturnSale:
          str = "OPERATION_SELL_RETURN";
          break;
        case CheckTypes.Buy:
          str = "OPERATION_BUY";
          break;
        case CheckTypes.ReturnBuy:
          str = "OPERATION_BUY_RETURN";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      ticketCommand2.Operation = str;
      ticketCommand1.Domain = new ReKassaDriver.TicketCommand.DomainClass()
      {
        Type = "DOMAIN_TRADING"
      };
      ReKassaDriver.TicketCommand ticketCommand3 = ticketCommand1;
      ReKassaDriver.TicketCommand.AmountsClass amountsClass = new ReKassaDriver.TicketCommand.AmountsClass();
      amountsClass.Total = new ReKassaDriver.SumFormat(checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - checkData.DiscountSum - num);
      amountsClass.Taken = checkData.CheckType == CheckTypes.Sale ? new ReKassaDriver.SumFormat(checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(new GlobalDictionaries.KkmPaymentMethods[1]))).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum))) : new ReKassaDriver.SumFormat(0M);
      amountsClass.Сhange = new ReKassaDriver.SumFormat(checkData.Delivery < 0M ? 0M : checkData.Delivery);
      ReKassaDriver.TicketCommand.DiscountItem discountItem;
      if (!(checkData.DiscountSum + num <= 0M))
        discountItem = new ReKassaDriver.TicketCommand.DiscountItem()
        {
          Sum = new ReKassaDriver.SumFormat(checkData.DiscountSum + num),
          Auxiliary = new List<ReKassaDriver.TicketCommand.Auxiliary>()
          {
            new ReKassaDriver.TicketCommand.Auxiliary()
            {
              Key = "DISCOUNT",
              Value = (checkData.DiscountSum + num).ToString("F")
            }
          }
        };
      else
        discountItem = (ReKassaDriver.TicketCommand.DiscountItem) null;
      amountsClass.Discount = discountItem;
      ticketCommand3.Amounts = amountsClass;
      ticketCommand1.XRequestId = checkData.Number;
      this._ticket = ticketCommand1;
      return true;
    }

    public bool CloseCheck()
    {
      this._ticket.Payments = new List<ReKassaDriver.TicketCommand.Payment>(this._ticket.Payments.GroupBy<ReKassaDriver.TicketCommand.Payment, string>((Func<ReKassaDriver.TicketCommand.Payment, string>) (x => x.Type)).Select<IGrouping<string, ReKassaDriver.TicketCommand.Payment>, ReKassaDriver.TicketCommand.Payment>((Func<IGrouping<string, ReKassaDriver.TicketCommand.Payment>, ReKassaDriver.TicketCommand.Payment>) (x => new ReKassaDriver.TicketCommand.Payment()
      {
        Sum = new ReKassaDriver.SumFormat(x.Sum<ReKassaDriver.TicketCommand.Payment>((Func<ReKassaDriver.TicketCommand.Payment, Decimal>) (p => p.Sum.Sum))),
        Type = x.Key
      })));
      if (this._ticket.Payments == null || !this._ticket.Payments.Any<ReKassaDriver.TicketCommand.Payment>())
        this._ticket.Payments = new List<ReKassaDriver.TicketCommand.Payment>()
        {
          new ReKassaDriver.TicketCommand.Payment()
          {
            Sum = new ReKassaDriver.SumFormat(0M),
            Type = "PAYMENT_CASH"
          }
        };
      if (this._ticket.Payments.Sum<ReKassaDriver.TicketCommand.Payment>((Func<ReKassaDriver.TicketCommand.Payment, Decimal>) (x => x.Sum.Sum)) - this._ticket.Amounts.Total.Sum > 0M)
      {
        Decimal sum = this._ticket.Payments.Sum<ReKassaDriver.TicketCommand.Payment>((Func<ReKassaDriver.TicketCommand.Payment, Decimal>) (x => x.Sum.Sum)) - this._ticket.Amounts.Total.Sum;
        this._ticket.Amounts.Сhange = new ReKassaDriver.SumFormat(sum);
        ReKassaDriver.TicketCommand.Payment payment = this._ticket.Payments.Find((Predicate<ReKassaDriver.TicketCommand.Payment>) (x => x.Type == "PAYMENT_CASH"));
        payment.Sum = new ReKassaDriver.SumFormat(payment.Sum.Sum - sum);
      }
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) this._ticket);
      if (!this.CheckError((ReKassaDriver.ReKassaCommand) this._ticket))
        return false;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._data.CustomData.Add("QRCodeUrl", (object) (this._ticket.Result?.QrCode ?? string.Empty));
      this._data.CustomData.Add("CheckNumber", this._ticket.Result?.TicketNumber ?? (object) string.Empty);
      Dictionary<string, object> customData = this._data.CustomData;
      ReKassaDriver.TicketCommand.TicketAnswer result = this._ticket.Result;
      // ISSUE: variable of a boxed type
      __Boxed<long> local = (ValueType) (result != null ? result.ShiftNumber : 1L);
      customData.Add("ShiftNumber", (object) local);
      this._data.CustomData.Add("SerialNumber", (object) devices.CheckPrinter.FiscalKkm.Model);
      this._data.CustomData.Add("RegistrationNumber", (object) this._rnmKassa);
      this._data.CustomData.Add("KkmName", (object) this._modelName);
      this._data.CustomData.Add("OfdName", (object) (this._ticket.Result?.Fdo.NameRu ?? string.Empty));
      this._data.CustomData.Add("OfdHost", (object) (this._ticket.Result?.Fdo.Url ?? string.Empty));
      this.PrintDocument();
      return true;
    }

    private void PrintDocument()
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(this._data);
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      ReKassaDriver.CashCommand cashCommand = new ReKassaDriver.CashCommand();
      cashCommand.Token = this._token;
      cashCommand.KassaId = this._idKassa;
      cashCommand.Operation = "MONEY_PLACEMENT_WITHDRAWAL";
      cashCommand.Sum = new ReKassaDriver.SumFormat(sum);
      cashCommand.DateTime = new ReKassaDriver.TicketCommand.DateTimeClass(DateTime.Now);
      ReKassaDriver.CashCommand command = cashCommand;
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command);
      return this.CheckError((ReKassaDriver.ReKassaCommand) command);
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      ReKassaDriver.CashCommand cashCommand = new ReKassaDriver.CashCommand();
      cashCommand.Token = this._token;
      cashCommand.KassaId = this._idKassa;
      cashCommand.Operation = "MONEY_PLACEMENT_DEPOSIT";
      cashCommand.Sum = new ReKassaDriver.SumFormat(sum);
      cashCommand.DateTime = new ReKassaDriver.TicketCommand.DateTimeClass(DateTime.Now);
      ReKassaDriver.CashCommand command = cashCommand;
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command);
      return this.CheckError((ReKassaDriver.ReKassaCommand) command);
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      ReKassaDriver.CashRegisterCommand cashRegisterCommand = new ReKassaDriver.CashRegisterCommand();
      cashRegisterCommand.Token = this._token;
      ReKassaDriver.CashRegisterCommand command1 = cashRegisterCommand;
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command1);
      if (this.CheckError((ReKassaDriver.ReKassaCommand) command1))
      {
        ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.CashRegister cashRegister = ((IEnumerable<ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.UserCashRegisterRole>) command1.Result.Embedded.UserCashRegisterRoles).First<ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.UserCashRegisterRole>().CashRegister;
        if (!cashRegister.ShiftOpen)
        {
          sum = 0M;
          return true;
        }
        this._idKassa = (int) cashRegister.Id;
        this._sessionNumber = (int) cashRegister.ShiftNumber;
        ReKassaDriver.XReportCommand xreportCommand = new ReKassaDriver.XReportCommand();
        xreportCommand.Token = this._token;
        xreportCommand.KassaId = this._idKassa;
        xreportCommand.SessionNumber = this._sessionNumber;
        ReKassaDriver.XReportCommand command2 = xreportCommand;
        this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command2);
        if (this.CheckError((ReKassaDriver.ReKassaCommand) command2))
        {
          sum = command2.Result.Data.CashSum.Sum;
          return true;
        }
      }
      sum = 0M;
      return false;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      ConfigsRepository<Gbs.Core.Config.Devices> cr = new ConfigsRepository<Gbs.Core.Config.Devices>();
      Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRates = cr.Get().CheckPrinter.FiscalKkm.TaxRates;
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = taxRates.SingleOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == good.TaxRateNumber)).Value ?? taxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == cr.Get().CheckPrinter.FiscalKkm.DefaultTaxRate)).Value;
      string str = (string) null;
      if (good.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None)
      {
        str = KZ_MarkCodesHelper.PrepareCodeForKkm(DataMatrixHelper.ReplaceSomeCharsToFNC1(good.MarkedInfo.FullCode));
        LogHelper.Debug("FullCode: " + good.MarkedInfo.FullCode + "; PreparedCode: " + str);
      }
      ReKassaDriver.TicketCommand.Item obj = new ReKassaDriver.TicketCommand.Item()
      {
        Type = "ITEM_TYPE_COMMODITY",
        Commodity = new ReKassaDriver.TicketCommand.Commodity()
        {
          Sum = new ReKassaDriver.SumFormat(good.Sum + good.DiscountSum),
          Name = good.Name,
          Quantity = (long) (good.Quantity * 1000M),
          Price = new ReKassaDriver.SumFormat(good.Price),
          SectionCode = (long) good.KkmSectionNumber,
          ExciseStamp = str,
          Auxiliary = new List<ReKassaDriver.TicketCommand.Auxiliary>()
          {
            new ReKassaDriver.TicketCommand.Auxiliary()
            {
              Key = "UNIT_TYPE",
              Value = "PIECE"
            }
          }
        }
      };
      if (taxRate.TaxValue != -1M)
      {
        obj.Commodity.Taxes = new List<ReKassaDriver.TicketCommand.Tax>()
        {
          new ReKassaDriver.TicketCommand.Tax()
          {
            Sum = new ReKassaDriver.SumFormat((good.Sum + good.DiscountSum).GetNdsSum(taxRate.TaxValue)),
            Percent = (long) taxRate.TaxValue * 1000L,
            Type = ReKassaDriver.TicketCommand.Tax.TaxTypeEnum.VAT,
            TaxationType = ReKassaDriver.TicketCommand.Tax.TaxationTypeEnum.STS
          }
        };
        if (good.Discount > 0M)
          obj.Commodity.Auxiliary.Add(new ReKassaDriver.TicketCommand.Auxiliary()
          {
            Key = "DISCOUNT_MARKUP_TAX",
            Value = (-good.DiscountSum.GetNdsSum(taxRate.TaxValue)).ToString("F").Replace(',', '.')
          });
      }
      if (good.Discount > 0M)
        obj.Commodity.Auxiliary.Add(new ReKassaDriver.TicketCommand.Auxiliary()
        {
          Key = "DISCOUNT",
          Value = good.Discount.ToString("N0") + "%"
        });
      this._ticket.Items.Add(obj);
      if (good.Discount > 0M)
        this._ticket.Items.Add(new ReKassaDriver.TicketCommand.Item()
        {
          Type = "ITEM_TYPE_DISCOUNT",
          Discount = new ReKassaDriver.TicketCommand.DiscountItem()
          {
            Sum = new ReKassaDriver.SumFormat(good.DiscountSum),
            Name = good.Name
          }
        });
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      if (payment.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.PrePayment, GlobalDictionaries.KkmPaymentMethods.Credit, GlobalDictionaries.KkmPaymentMethods.Certificate, GlobalDictionaries.KkmPaymentMethods.Bonus))
        return true;
      List<ReKassaDriver.TicketCommand.Payment> payments = this._ticket.Payments;
      ReKassaDriver.TicketCommand.Payment payment1 = new ReKassaDriver.TicketCommand.Payment();
      payment1.Sum = new ReKassaDriver.SumFormat(payment.Sum);
      ReKassaDriver.TicketCommand.Payment payment2 = payment1;
      string str;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          str = "PAYMENT_CASH";
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          str = "PAYMENT_CARD";
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          str = "PAYMENT_CARD";
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          str = "PAYMENT_MOBILE";
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowInvalidOperationException();
          break;
      }
      payment2.Type = str;
      payments.Add(payment1);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._driver = new ReKassaDriver();
      this._password = devicesConfig.CheckPrinter.Connection.LanPort.Password;
      ReKassaDriver.AuthorizationCommand command = new ReKassaDriver.AuthorizationCommand()
      {
        Password = devicesConfig.CheckPrinter.Connection.LanPort.UserLogin,
        Number = devicesConfig.CheckPrinter.FiscalKkm.Model
      };
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command);
      if (!this.CheckError((ReKassaDriver.ReKassaCommand) command))
        return;
      this._token = command.Result.Token;
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintNonFiscalReport(nonFiscalStrings.Select<NonFiscalString, string>((Func<NonFiscalString, string>) (x => x.Text)).ToList<string>());
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      ReKassaDriver.CashRegisterCommand cashRegisterCommand = new ReKassaDriver.CashRegisterCommand();
      cashRegisterCommand.Token = this._token;
      ReKassaDriver.CashRegisterCommand command = cashRegisterCommand;
      this._driver.SendCommand((ReKassaDriver.ReKassaCommand) command);
      if (!this.CheckError((ReKassaDriver.ReKassaCommand) command))
        return (KkmStatus) null;
      ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.CashRegister cashRegister = ((IEnumerable<ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.UserCashRegisterRole>) command.Result.Embedded.UserCashRegisterRoles).First<ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.UserCashRegisterRole>().CashRegister;
      this._idKassa = (int) cashRegister.Id;
      this._sessionNumber = (int) cashRegister.ShiftNumber;
      this._modelName = cashRegister.Name;
      this._rnmKassa = cashRegister.RegistrationNumber;
      return new KkmStatus()
      {
        Model = cashRegister.Model,
        SessionNumber = (int) cashRegister.ShiftNumber,
        FactoryNumber = cashRegister.SerialNumber,
        SessionStarted = new DateTime?(cashRegister.ShiftOpenTime ?? DateTime.MinValue),
        SessionStatus = cashRegister.ShiftExpired ? SessionStatuses.OpenMore24Hours : (cashRegister.ShiftOpen ? SessionStatuses.Open : SessionStatuses.Close),
        CheckNumber = (int) cashRegister.ShiftDocumentNumber,
        CheckStatus = CheckStatuses.Close,
        KkmState = KkmStatuses.Ready
      };
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    private bool CheckError(ReKassaDriver.ReKassaCommand command)
    {
      DeviceException deviceException;
      try
      {
        ReKassaDriver.ErrorReKassa errorReKassa = JsonConvert.DeserializeObject<ReKassaDriver.ErrorReKassa>(command.AnswerString);
        if (errorReKassa.Code.IsNullOrEmpty())
          return true;
        LogHelper.Debug("Error WebKasaa " + errorReKassa.Message + " (" + errorReKassa.Code + ")");
        deviceException = new DeviceException(errorReKassa.Message + " (code: " + errorReKassa.Code + ")");
      }
      catch
      {
        return true;
      }
      if (deviceException != null)
        throw deviceException;
      return true;
    }

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
