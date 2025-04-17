// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.HiPos
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class HiPos : IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;
    private HiPosDriver.CreateCheckCommand _check;
    private HiPosDriver _driver;
    private DateTime? _dateTimeNotification;

    private string SessionKey { get; set; }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name { get; }

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
    }

    public bool IsCanHoldConnection { get; }

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
      HiPosDriver.OpenShiftCommand command = new HiPosDriver.OpenShiftCommand();
      command.SessionKey = this.SessionKey;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
    }

    public void SetModePrro(bool isOnline)
    {
      if (isOnline)
      {
        HiPosDriver.SetOnlineCommand command = new HiPosDriver.SetOnlineCommand();
        command.SessionKey = this.SessionKey;
        this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      }
      else
      {
        HiPosDriver.SetOfflineCommand command = new HiPosDriver.SetOfflineCommand();
        command.SessionKey = this.SessionKey;
        this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      }
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string str = string.Empty;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          HiPosDriver.CloseSiftCommand closeSiftCommand = new HiPosDriver.CloseSiftCommand();
          closeSiftCommand.SessionKey = this.SessionKey;
          HiPosDriver.CloseSiftCommand command1 = closeSiftCommand;
          this._driver.DoCommand((HiPosDriver.HiPosCommand) command1);
          HiPosDriver.GetTxtZCommand getTxtZcommand = new HiPosDriver.GetTxtZCommand();
          getTxtZcommand.SessionKey = this.SessionKey;
          getTxtZcommand.ShiftId = int.Parse(command1.AnswerString);
          HiPosDriver.GetTxtZCommand command2 = getTxtZcommand;
          this._driver.DoCommand((HiPosDriver.HiPosCommand) command2);
          str = command2.AnswerString;
          goto case ReportTypes.XReportWithGoods;
        case ReportTypes.XReport:
          HiPosDriver.GetXCommand command3 = new HiPosDriver.GetXCommand();
          command3.SessionKey = this.SessionKey;
          command3.Body = (object) 0;
          this._driver.DoCommand((HiPosDriver.HiPosCommand) command3);
          HiPosDriver.GetTxtXCommand getTxtXcommand = new HiPosDriver.GetTxtXCommand();
          getTxtXcommand.SessionKey = this.SessionKey;
          HiPosDriver.GetTxtXCommand command4 = getTxtXcommand;
          this._driver.DoCommand((HiPosDriver.HiPosCommand) command4);
          str = command4.AnswerString;
          goto case ReportTypes.XReportWithGoods;
        case ReportTypes.XReportWithGoods:
          this.PrintNonFiscalStrings(((IEnumerable<string>) str.Split('\n')).Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x))).ToList<NonFiscalString>());
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      Decimal num = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)) + checkData.DiscountSum;
      HiPosDriver.CreateCheckCommand createCheckCommand = new HiPosDriver.CreateCheckCommand();
      createCheckCommand.IsReturn = checkData.CheckType == CheckTypes.ReturnSale;
      createCheckCommand.TotalAmount = checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - num;
      createCheckCommand.TotalDiscount = checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.DiscountSum));
      createCheckCommand.SessionKey = this.SessionKey;
      createCheckCommand.FiscalNumber = checkData.FiscalNum;
      this._check = createCheckCommand;
      this._check.NoRoundAmount = checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      this._check.RoundSum = num;
      return true;
    }

    private void PrintDocument(IPrintableReport report)
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintPrepareCheck(this._checkData, report);
    }

    public bool CloseCheck()
    {
      if (this._check.PaymentInfos.Sum<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, Decimal>) (x => x.Paid)) > this._check.TotalAmount)
      {
        Decimal num = this._check.PaymentInfos.Sum<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, Decimal>) (x => x.Paid)) - this._check.TotalAmount;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 0)).PayOut = num;
      }
      this._check.PaymentInfos = new List<HiPosDriver.CreateCheckCommand.Payment>(this._check.PaymentInfos.Where<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.TotalPay > 0M)));
      this._checkData.PaymentsList = new List<CheckPayment>(this._checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Sum > 0M)));
      if (this._checkData.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
      {
        Guid? uid = x?.Type?.Uid;
        Guid rrnUid = GlobalDictionaries.RrnUid;
        return uid.HasValue && uid.GetValueOrDefault() == rrnUid;
      })) && this._check.PaymentInfos.Any<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x != null && x.PaymentType == 1)) && !this._check.IsReturn)
      {
        LogHelper.Debug("Нашли данные об оплате картой, добавляем в чек");
        string str1 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.ApprovalCodeUid))?.Value.ToString() ?? "";
        string str2 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CardNumberUid))?.Value.ToString() ?? "";
        string str3 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RrnUid))?.Value.ToString() ?? "";
        string str4 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.TerminalIdUid))?.Value.ToString() ?? "";
        string str5 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.PaymentSystemUid))?.Value.ToString() ?? "";
        string str6 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.IssuerNameUid))?.Value.ToString() ?? "";
        string str7 = str6.IsNullOrEmpty() ? SalePoints.GetSalePointList().First<SalePoints.SalePoint>().Organization.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.BankNameUid))?.Value.ToString() ?? string.Empty : str6;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).AuthorizationId = str1;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).BankId = str7;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).CardNumber = str2;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).PosTransactionNumber = str3;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).TerminalId = str4;
        this._check.PaymentInfos.First<HiPosDriver.CreateCheckCommand.Payment>((Func<HiPosDriver.CreateCheckCommand.Payment, bool>) (x => x.PaymentType == 1)).PaymentSystem = str5;
      }
      else
        LogHelper.Debug("Не нашли данные об оплате картой");
      this._driver.DoCommand((HiPosDriver.HiPosCommand) this._check);
      this._checkData.FiscalNum = this._check.AnswerString;
      HiPosDriver.GetCheckCommand getCheckCommand1 = new HiPosDriver.GetCheckCommand();
      getCheckCommand1.ReceiptFiscalNumber = this._checkData.FiscalNum;
      getCheckCommand1.PrroFiscalNumber = this._prroFiscalNumber;
      getCheckCommand1.SessionKey = this.SessionKey;
      HiPosDriver.GetCheckCommand getCheckCommand2 = getCheckCommand1;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) getCheckCommand2);
      this._checkData.CustomData.Add("PrroFiscalNumber", (object) this._prroFiscalNumber);
      this._checkData.CustomData.Add("CheckType", (object) (int) this._checkData.CheckType);
      this.PrintDocument(new PrintableReportFactory().CreateForHiPosFiscalCheck(getCheckCommand2, this._checkData.CustomData));
      return true;
    }

    public void CancelCheck()
    {
    }

    public HiPosDriver.GetCheckCommand GetCheck(string numCheck)
    {
      HiPosDriver.GetCheckCommand getCheckCommand = new HiPosDriver.GetCheckCommand();
      getCheckCommand.ReceiptFiscalNumber = numCheck;
      getCheckCommand.PrroFiscalNumber = this._prroFiscalNumber;
      getCheckCommand.SessionKey = this.SessionKey;
      HiPosDriver.GetCheckCommand command = getCheckCommand;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      return command;
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      HiPosDriver.CashOutCommand command = new HiPosDriver.CashOutCommand();
      command.SessionKey = this.SessionKey;
      command.Body = (object) sum;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      HiPosDriver.CashInCommand command = new HiPosDriver.CashInCommand();
      command.SessionKey = this.SessionKey;
      command.Body = (object) sum;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      throw new NotImplementedException();
    }

    public bool GetCashSum(out Decimal sum)
    {
      HiPosDriver.GetInfoCommand getInfoCommand = new HiPosDriver.GetInfoCommand();
      getInfoCommand.SessionKey = this.SessionKey;
      HiPosDriver.GetInfoCommand command = getInfoCommand;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      sum = command.Result.Cash.GetValueOrDefault();
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRates = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates;
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = taxRates.SingleOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == good.TaxRateNumber)).Value ?? taxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.DefaultTaxRate)).Value;
      HiPosDriver.CreateCheckCommand.Item obj = new HiPosDriver.CreateCheckCommand.Item()
      {
        Name = good.Name.Length >= 128 ? good.Name.Remove((int) sbyte.MaxValue) : good.Name,
        Barcode = good.Barcode,
        Discount = good.DiscountSum,
        Quantity = good.Quantity,
        FullPrice = Math.Round(good.Price * good.Quantity, 2, MidpointRounding.AwayFromZero),
        Price = good.Price,
        TotalPrice = good.Sum,
        Id = Convert.ToInt32(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (g => g.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) 0),
        TaxGroup = taxRate.Name
      };
      if (good.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol)
      {
        obj.ExciseLabels = new List<string>();
        obj.ExciseLabels.Add(good.Description);
      }
      Guid uktZedUid = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().GoodsConfig.UktZedUid;
      string str1 = KkmHelper.RemoveSpaceAndEnter(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uktZedUid))?.Value.ToString() ?? "");
      string str2 = ((IEnumerable<char>) str1.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit)) ? str1 : "";
      if (!str2.IsNullOrEmpty())
        obj.Uktzed = str2;
      this._check.Products.Add(obj);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      int num;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          num = 1;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          num = 3;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          num = 2;
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          return true;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this._check.PaymentInfos.Add(new HiPosDriver.CreateCheckCommand.Payment()
      {
        PaymentType = num,
        Paid = payment.Sum,
        PaymentName = payment.Name,
        TotalPay = payment.Sum
      });
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Entities.Users.User byUid = new UsersRepository(dataBase).GetByUid(KkmHelper.UserUid);
        this._driver = new HiPosDriver(devicesConfig);
        if (DevelopersHelper.IsDebug())
          return;
        HiPosDriver.AuthLoginCommand command = new HiPosDriver.AuthLoginCommand()
        {
          UserName = byUid == null ? devicesConfig.CheckPrinter.Connection.LanPort.UserLogin : byUid.LoginForKkm,
          Password = byUid == null ? devicesConfig.CheckPrinter.Connection.LanPort.Password : byUid.PasswordForKkm
        };
        this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
        this.SessionKey = command.Result.AccessToken;
      }
    }

    public bool Disconnect()
    {
      if (this._driver == null)
        return true;
      KkmHelper.UserUid = Guid.Empty;
      HiPosDriver.OutLoginCommand command = new HiPosDriver.OutLoginCommand();
      command.SessionKey = this.SessionKey;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command);
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintNonFiscalReport(nonFiscalStrings.Select<NonFiscalString, string>((Func<NonFiscalString, string>) (x => x.Text)).ToList<string>());
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    private double _prroFiscalNumber { get; set; }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      if (DevelopersHelper.IsDebug())
        return new KkmStatus()
        {
          KkmState = KkmStatuses.Ready
        };
      HiPosDriver.GetInfoCommand getInfoCommand = new HiPosDriver.GetInfoCommand();
      getInfoCommand.SessionKey = this.SessionKey;
      HiPosDriver.GetInfoCommand command1 = getInfoCommand;
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command1);
      int result;
      int int32 = int.TryParse(command1.Result.LastReceiptFiscalNumber, out result) ? Convert.ToInt32(command1.Result.LastReceiptFiscalNumber) : 0;
      KkmStatus status = new KkmStatus()
      {
        SessionStatus = command1.Result.ShiftOpenedAt == null ? SessionStatuses.Close : SessionStatuses.Open,
        SessionStarted = command1.Result.ShiftOpenedAt == null ? new DateTime?() : new DateTime?(DateTime.Parse(command1.Result.ShiftOpenedAt, (IFormatProvider) CultureInfo.CurrentCulture)),
        FactoryNumber = command1.Result.PrroFiscalNumber.ToString(),
        CheckNumber = int32,
        KkmState = KkmStatuses.Ready
      };
      this._prroFiscalNumber = command1.Result.PrroFiscalNumber;
      if (status.SessionStatus == SessionStatuses.Open)
      {
        HiPosDriver.GetStateShiftCommand stateShiftCommand = new HiPosDriver.GetStateShiftCommand();
        stateShiftCommand.SessionKey = this.SessionKey;
        HiPosDriver.GetStateShiftCommand command2 = stateShiftCommand;
        this._driver.DoCommand((HiPosDriver.HiPosCommand) command2);
        if (command2.Result.IsZReportNeeded)
          status.SessionStatus = SessionStatuses.OpenMore24Hours;
      }
      DateTime? nullable;
      if (this._dateTimeNotification.HasValue)
      {
        DateTime now = DateTime.Now;
        nullable = this._dateTimeNotification;
        if ((nullable.HasValue ? new TimeSpan?(now - nullable.GetValueOrDefault()) : new TimeSpan?()).Value.TotalHours <= 12.0)
          goto label_16;
      }
      result = command1.Result.Mode;
      string str1;
      switch (result)
      {
        case 0:
          str1 = "Offline";
          break;
        case 1:
          str1 = "Online";
          break;
        case 2:
          str1 = "Blocked";
          break;
        case 3:
          str1 = "ManualOffline";
          break;
        default:
          str1 = "Error";
          break;
      }
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.HiPos_Напоминание__Режим_работы_ПРРО___ + str1));
      HiPosDriver.GetCertificateStateCommand certificateStateCommand = new HiPosDriver.GetCertificateStateCommand();
      certificateStateCommand.SessionKey = this.SessionKey;
      HiPosDriver.GetCertificateStateCommand command3 = certificateStateCommand;
      this._dateTimeNotification = new DateTime?(DateTime.Now);
      this._driver.DoCommand((HiPosDriver.HiPosCommand) command3);
      nullable = command3.Result.ValidTo;
      if (nullable.HasValue)
      {
        nullable = command3.Result.ValidTo;
        if ((nullable.Value - DateTime.Now).TotalDays < 14.0)
        {
          string hiPos = Translate.HiPos_;
          nullable = command3.Result.ValidTo;
          string str2 = nullable.Value.ToString("dd.MM.yyyy");
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(hiPos + str2));
        }
      }
label_16:
      return status;
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress)
    {
      this._check.IsNeedToSendReceiptFile = true;
      this._check.RecipientEmailAddress = adress;
      return true;
    }

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
