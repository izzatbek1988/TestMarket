// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan.WebKassa
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan
{
  public class WebKassa : IFiscalKkm, IDevice
  {
    public static (string token, string user, string pass, DateTime authDateTime) OldAuthToken = ("", "", "", DateTime.Now);
    private string _token;
    private string _cashboxUniqueNumber;

    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData CheckData { get; set; }

    private WebKassaDriver Driver { get; set; }

    private WebKassaDriver.CheckCommand Check { get; set; }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "WEB KASSA";

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

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
      switch (reportType)
      {
        case ReportTypes.ZReport:
          WebKassaDriver.ZReportCommand command1 = new WebKassaDriver.ZReportCommand();
          command1.Token = this._token;
          command1.CashboxUniqueNumber = this._cashboxUniqueNumber;
          this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command1);
          break;
        case ReportTypes.XReport:
          WebKassaDriver.XReportCommand command2 = new WebKassaDriver.XReportCommand();
          command2.Token = this._token;
          command2.CashboxUniqueNumber = this._cashboxUniqueNumber;
          this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command2);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.CheckData = checkData;
      WebKassaDriver.CheckCommand checkCommand1 = new WebKassaDriver.CheckCommand();
      checkCommand1.Token = this._token;
      checkCommand1.CashboxUniqueNumber = this._cashboxUniqueNumber;
      checkCommand1.Change = checkData.Delivery;
      checkCommand1.CustomerEmail = checkData.Client?.Client?.Email;
      WebKassaDriver.CheckCommand checkCommand2 = checkCommand1;
      ClientAdnSum client1 = checkData.Client;
      string str;
      if (client1 == null)
      {
        str = (string) null;
      }
      else
      {
        Gbs.Core.Entities.Clients.Client client2 = client1.Client;
        if (client2 == null)
        {
          str = (string) null;
        }
        else
        {
          string phone = client2.Phone;
          str = phone != null ? phone.ClearPhone() : (string) null;
        }
      }
      checkCommand2.CustomerPhone = str;
      checkCommand1.CustomerXin = checkData.Client?.Client?.GetInn();
      checkCommand1.ExternalCheckNumber = Guid.NewGuid().ToString();
      WebKassaDriver.CheckCommand checkCommand3 = checkCommand1;
      int num;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          num = 2;
          break;
        case CheckTypes.ReturnSale:
          num = 3;
          break;
        case CheckTypes.Buy:
          num = 0;
          break;
        case CheckTypes.ReturnBuy:
          num = 1;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      checkCommand3.OperationType = num;
      checkCommand1.RoundType = 0;
      this.Check = checkCommand1;
      return true;
    }

    public bool CloseCheck()
    {
      Decimal num = Math.Round(this.Check.Payments.Sum<WebKassaDriver.CheckCommand.Payment>((Func<WebKassaDriver.CheckCommand.Payment, Decimal>) (x => x.Sum)) - this.Check.Positions.Sum<WebKassaDriver.CheckCommand.Position>((Func<WebKassaDriver.CheckCommand.Position, Decimal>) (x => x.Count * x.Price)), 2);
      this.Check.Change = num > 0M ? num : 0M;
      this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) this.Check);
      this.CheckData.CustomData.Add("QRCodeUrl", (object) (this.Check.Result.Data.TicketUrl ?? string.Empty));
      this.CheckData.CustomData.Add("UniqueNumber", (object) (this.Check.Result.Data.Cashbox.UniqueNumber ?? string.Empty));
      this.CheckData.CustomData.Add("RegistrationNumber", (object) (this.Check.Result.Data.Cashbox.RegistrationNumber ?? string.Empty));
      this.CheckData.CustomData.Add("CheckNumber", (object) (this.Check.Result.Data.CheckNumber ?? string.Empty));
      this.CheckData.CustomData.Add("OfdName", (object) (this.Check.Result.Data.Cashbox.Ofd.Name ?? string.Empty));
      this.CheckData.CustomData.Add("OfdHost", (object) (this.Check.Result.Data.Cashbox.Ofd.Host ?? string.Empty));
      this.CheckData.CustomData.Add("OfflineMode", (object) this.Check.Result.Data.OfflineMode);
      this.CheckData.Number = this.Check.Result.Data.CheckOrderNumber.ToString();
      this.CheckData.DateTime = DateTime.Parse(this.Check.Result.Data.DateTime);
      this.PrintDocument();
      return true;
    }

    private void PrintDocument()
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(this.CheckData);
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      WebKassaDriver.MoneyOperationCommand command = new WebKassaDriver.MoneyOperationCommand();
      command.Token = this._token;
      command.CashboxUniqueNumber = this._cashboxUniqueNumber;
      command.Sum = (long) sum;
      command.OperationType = 1L;
      command.ExternalCheckNumber = Guid.NewGuid().ToString();
      this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      WebKassaDriver.MoneyOperationCommand command = new WebKassaDriver.MoneyOperationCommand();
      command.Token = this._token;
      command.CashboxUniqueNumber = this._cashboxUniqueNumber;
      command.Sum = (long) sum;
      command.OperationType = 0L;
      command.ExternalCheckNumber = Guid.NewGuid().ToString();
      this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      WebKassaDriver.MoneyOperationCommand operationCommand = new WebKassaDriver.MoneyOperationCommand();
      operationCommand.Token = this._token;
      operationCommand.CashboxUniqueNumber = this._cashboxUniqueNumber;
      operationCommand.Sum = 0L;
      operationCommand.OperationType = 1L;
      WebKassaDriver.MoneyOperationCommand command = operationCommand;
      this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command);
      sum = command.Result.Data.Sum;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      ConfigsRepository<Gbs.Core.Config.Devices> cr = new ConfigsRepository<Gbs.Core.Config.Devices>();
      Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRates = cr.Get().CheckPrinter.FiscalKkm.TaxRates;
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = taxRates.SingleOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == good.TaxRateNumber)).Value ?? taxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == cr.Get().CheckPrinter.FiscalKkm.DefaultTaxRate)).Value;
      GoodsUnits.GoodUnit byUid = GoodsUnits.GetByUid(good.Good.Group.UnitsUid);
      if (byUid == null || byUid.Code.IsNullOrEmpty() || !int.TryParse(byUid.Code, out int _))
        throw new KkmException((IDevice) this, Translate.WebKassa_RegisterGood_Не_указан_код_или_является_некорректным_для_единицы_измерения__невозможно_напечатать_чек_);
      WebKassaDriver.CheckCommand.Position position = new WebKassaDriver.CheckCommand.Position()
      {
        Count = good.Quantity,
        Discount = good.DiscountSum,
        PositionCode = good.Barcode,
        PositionName = good.Name,
        Price = good.Price,
        UnitCode = int.Parse(byUid.Code),
        Mark = good.MarkedInfo.Type == GlobalDictionaries.RuMarkedProductionTypes.None ? (string) null : DataMatrixHelper.ReplaceSomeCharsToFNC1(good.MarkedInfo.FullCode)
      };
      if (taxRate.TaxValue == -1M)
      {
        position.TaxType = 0L;
        position.TaxPercent = new int?();
        position.Tax = 0M;
      }
      else
      {
        position.TaxType = 100L;
        position.TaxPercent = new int?((int) taxRate.TaxValue);
        position.Tax = good.Sum.GetNdsSum((Decimal) (int) taxRate.TaxValue);
      }
      this.Check.Positions.Add(position);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      List<WebKassaDriver.CheckCommand.Payment> payments = this.Check.Payments;
      WebKassaDriver.CheckCommand.Payment payment1 = new WebKassaDriver.CheckCommand.Payment();
      payment1.Sum = payment.Sum;
      WebKassaDriver.CheckCommand.Payment payment2 = payment1;
      int num;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          num = 1;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          num = 1;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          num = 2;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          num = 0;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      payment2.PaymentType = (long) num;
      payments.Add(payment1);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      this.Check.TicketModifiers.Add(new WebKassaDriver.CheckCommand.TicketModifier()
      {
        Sum = sum,
        Text = description,
        Type = 1L,
        Tax = 0M,
        TaxType = 0L
      });
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.Driver = new WebKassaDriver();
      string userLogin = devicesConfig.CheckPrinter.Connection.LanPort.UserLogin;
      string password = devicesConfig.CheckPrinter.Connection.LanPort.Password;
      if (WebKassa.OldAuthToken.token.IsNullOrEmpty() || WebKassa.OldAuthToken.user != userLogin || WebKassa.OldAuthToken.pass != password || (DateTime.Now - WebKassa.OldAuthToken.authDateTime).TotalHours > 12.0)
      {
        WebKassaDriver.AuthorizationCommand command = new WebKassaDriver.AuthorizationCommand()
        {
          Login = userLogin,
          Password = password
        };
        this.Driver.SendCommand((WebKassaDriver.WebKassaCommand) command);
        WebKassa.OldAuthToken = (command.Result.Data.Token, userLogin, password, DateTime.Now);
      }
      this._token = WebKassa.OldAuthToken.token;
      this._cashboxUniqueNumber = devicesConfig.CheckPrinter.FiscalKkm.Model;
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
      return new KkmStatus()
      {
        CheckStatus = CheckStatuses.Close,
        KkmState = KkmStatuses.Ready
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
