// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia.Hdm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia
{
  public class Hdm : IFiscalKkm, IDevice
  {
    private HdmDriver _driver;
    private Gbs.Core.Config.Devices _devicesConfig;
    private bool _isOpenSession;
    private HdmDriver.HdmCommand _command;
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _data;
    public static List<Hdm.CategoryItem> CategoryItems = Hdm.LoadAmCategory();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "HDM";

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
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
      long timeMilliseconds1 = ((DateTimeOffset) DateTime.UtcNow.Date).ToUnixTimeMilliseconds();
      long timeMilliseconds2 = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeMilliseconds();
      HdmDriver.ReportCommand command = new HdmDriver.ReportCommand();
      HdmDriver.ReportCommand reportCommand = command;
      HdmDriver.ReportCommand.ReportType reportType1;
      if (reportType != ReportTypes.ZReport)
      {
        if (reportType != ReportTypes.XReport)
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
        reportType1 = HdmDriver.ReportCommand.ReportType.XReport;
      }
      else
        reportType1 = HdmDriver.ReportCommand.ReportType.ZReport;
      reportCommand.Type = reportType1;
      command.EndDate = new double?((double) timeMilliseconds2);
      command.StartDate = new double?((double) timeMilliseconds1);
      this._driver.DoCommand((HdmDriver.HdmCommand) command);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._data = checkData;
      LogHelper.Debug(checkData.ToJsonString(true));
      if (checkData.CheckType == CheckTypes.Sale)
      {
        this._command = (HdmDriver.HdmCommand) new HdmDriver.CheckCommand()
        {
          ClientTin = (checkData.Client?.Client?.GetInn() ?? (string) null),
          Mode = HdmDriver.ModePrint.InGood,
          PartialAmount = 0.0,
          Items = new List<HdmDriver.Item>(),
          UseExtCardTerminal = checkData.PaymentsList.All<CheckPayment>((Func<CheckPayment, bool>) (x => x.Type == GlobalDictionaries.PaymentMethodsType.NoBroker))
        };
        if (checkData.GoodsList.Any<CheckGood>((Func<CheckGood, bool>) (x =>
        {
          MarkedInfo markedInfo = x.MarkedInfo;
          return (markedInfo != null ? (markedInfo.Type != 0 ? 1 : 0) : 1) != 0 && !x.Description.IsNullOrEmpty();
        })))
          ((HdmDriver.CheckCommand) this._command).Marks = checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x =>
          {
            MarkedInfo markedInfo = x.MarkedInfo;
            return (markedInfo != null ? (markedInfo.Type != 0 ? 1 : 0) : 1) != 0 && !x.Description.IsNullOrEmpty();
          })).Select<CheckGood, string>((Func<CheckGood, string>) (x => DataMatrixHelper.ReplaceSomeCharsToFNC1(x.Description))).ToList<string>();
      }
      else
      {
        HdmDriver.CheckInfoCommand command = new HdmDriver.CheckInfoCommand()
        {
          Сrn = checkData.FrNumber,
          FiscalNumber = checkData.FiscalNum
        };
        this._driver.DoCommand((HdmDriver.HdmCommand) command);
        this._command = (HdmDriver.HdmCommand) new HdmDriver.ReturnCheckCommand()
        {
          FiscalNumber = Convert.ToInt64(checkData.FiscalNum),
          Crn = checkData.FrNumber
        };
        if (checkData.GoodsList.Any<CheckGood>((Func<CheckGood, bool>) (x =>
        {
          MarkedInfo markedInfo = x.MarkedInfo;
          return (markedInfo != null ? (markedInfo.Type != 0 ? 1 : 0) : 1) != 0 && !x.Description.IsNullOrEmpty();
        })))
          ((HdmDriver.ReturnCheckCommand) this._command).Marks = checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x =>
          {
            MarkedInfo markedInfo = x.MarkedInfo;
            return (markedInfo != null ? (markedInfo.Type != 0 ? 1 : 0) : 1) != 0 && !x.Description.IsNullOrEmpty();
          })).Select<CheckGood, string>((Func<CheckGood, string>) (x => DataMatrixHelper.ReplaceSomeCharsToFNC1(x.Description))).ToList<string>();
        ((HdmDriver.ReturnCheckCommand) this._command).ReturnItems = new List<HdmDriver.ReturnCheckCommand.ReturnItem>();
        foreach (CheckGood goods in checkData.GoodsList)
        {
          HdmDriver.CheckInfoCommand.Answer.TotalItem totalItem1 = (HdmDriver.CheckInfoCommand.Answer.TotalItem) null;
          foreach (HdmDriver.CheckInfoCommand.Answer.TotalItem totalItem2 in command.Result.TotalItems)
          {
            Decimal num1 = Other.ConvertToDecimal(totalItem2.Price);
            Decimal num2 = Other.ConvertToDecimal(totalItem2.Discount);
            LogHelper.Debug("p = " + num1.ToString());
            LogHelper.Debug("d = " + num2.ToString());
            LogHelper.Debug("g = " + totalItem2.GoodCode);
            LogHelper.Debug("p1 = " + goods.Price.ToString());
            LogHelper.Debug("d1 = " + goods.Discount.ToString());
            LogHelper.Debug("g1 = " + goods.Good.Uid.ToString());
            if (num1 == goods.Price && num2 == goods.Discount && totalItem2.GoodCode == goods.Good.Uid.ToString())
            {
              totalItem1 = totalItem2;
              break;
            }
          }
          if (totalItem1 == null)
            throw new DeviceException(Translate.Hdm_OpenCheck_Не_удалось_опеределить_товар_для_возврата__обратитесь_в_поддержку_);
          ((HdmDriver.ReturnCheckCommand) this._command).ReturnItems.Add(new HdmDriver.ReturnCheckCommand.ReturnItem()
          {
            Quantity = (double) goods.Quantity,
            Id = Convert.ToInt64(totalItem1.Id)
          });
        }
      }
      return true;
    }

    public bool CloseCheck()
    {
      this._driver.DoCommand(this._command);
      HdmDriver.CheckCommand.Answer answer = this._command.GetType() == typeof (HdmDriver.CheckCommand) ? ((HdmDriver.CheckCommand) this._command).Result : ((HdmDriver.ReturnCheckCommand) this._command).Result;
      this._data.FiscalNum = answer.CheckNumber.ToString();
      this._data.FrNumber = answer.Crn;
      this._data.CustomData.Add("Tin", (object) (answer?.Tin ?? string.Empty));
      this._data.CustomData.Add("Address", (object) (answer?.Address ?? string.Empty));
      this._data.CustomData.Add("CheckNumber", (object) (answer?.CheckNumber.ToString("D") ?? string.Empty));
      this._data.CustomData.Add("Crn", (object) (answer?.Crn ?? string.Empty));
      this._data.CustomData.Add("FiscalNumber", (object) (answer?.FiscalNumber ?? string.Empty));
      this._data.CustomData.Add("SerialNumber", (object) (answer?.SerialNumber ?? string.Empty));
      this._data.CustomData.Add("TaxPayer", (object) (answer?.TaxPayer ?? string.Empty));
      this._data.CustomData.Add("Time", (object) Other.UnixTimeStampToDateTime(answer.Time));
      this._data.CustomData.Add("CheckType", (object) (int) this._data.CheckType);
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
      (int id, string password) user = this.GetUser();
      this._driver.DoCommand((HdmDriver.HdmCommand) new HdmDriver.MoneyCommand()
      {
        Amount = (double) sum,
        IsCashIn = false,
        CashierId = user.id
      });
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      (int id, string password) user = this.GetUser();
      this._driver.DoCommand((HdmDriver.HdmCommand) new HdmDriver.MoneyCommand()
      {
        Amount = (double) sum,
        IsCashIn = true,
        CashierId = user.id
      });
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
      if (checkType == CheckTypes.Sale)
      {
        HdmDriver.Item obj = new HdmDriver.Item()
        {
          Name = good.Name.Length > 50 ? good.Name.Remove(50) : good.Name,
          Price = (double) good.Price,
          Qty = (double) good.Quantity,
          UnitName = good.Unit.FullName,
          ProductCode = good.Good.Uid.ToString(),
          Department = good.KkmSectionNumber,
          AdgCode = (good.Good.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.AmClassifierIdUid)) ?? throw new DeviceException(string.Format(Translate.Hdm_RegisterGood_Для_товара__0__не_указан_классификатор_, (object) good.Name))).Value.ToString()
        };
        if (good.Discount > 0M)
        {
          obj.Discount = new Decimal?(good.Discount);
          obj.DiscountType = new HdmDriver.DiscountType?(HdmDriver.DiscountType.PercentByPrice);
        }
        ((HdmDriver.CheckCommand) this._command).Items.Add(obj);
      }
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      if (this._command.GetType() == typeof (HdmDriver.CheckCommand))
      {
        switch (payment.Method)
        {
          case GlobalDictionaries.KkmPaymentMethods.Cash:
            ((HdmDriver.CheckCommand) this._command).SumCash += (double) payment.Sum;
            break;
          case GlobalDictionaries.KkmPaymentMethods.Card:
          case GlobalDictionaries.KkmPaymentMethods.Bank:
          case GlobalDictionaries.KkmPaymentMethods.EMoney:
            ((HdmDriver.CheckCommand) this._command).SumCard += (double) payment.Sum;
            break;
          default:
            return true;
        }
      }
      else if (this._command.GetType() == typeof (HdmDriver.ReturnCheckCommand))
      {
        switch (payment.Method)
        {
          case GlobalDictionaries.KkmPaymentMethods.Cash:
            ((HdmDriver.ReturnCheckCommand) this._command).CashAmountForReturn += payment.Sum;
            break;
          case GlobalDictionaries.KkmPaymentMethods.Card:
          case GlobalDictionaries.KkmPaymentMethods.Bank:
          case GlobalDictionaries.KkmPaymentMethods.EMoney:
            ((HdmDriver.ReturnCheckCommand) this._command).CardAmountForReturn += payment.Sum;
            break;
          default:
            return true;
        }
      }
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (this._devicesConfig == null)
        this._devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._driver = new HdmDriver(this._devicesConfig.CheckPrinter.Connection.LanPort.UrlAddress, this._devicesConfig.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault(), this._devicesConfig.CheckPrinter.FiscalKkm.Model);
      (int id, string password) user = this.GetUser();
      this._driver.DoCommand((HdmDriver.HdmCommand) new HdmDriver.LoginCommand()
      {
        CashierId = user.id,
        Pin = user.password
      });
      this._isOpenSession = true;
    }

    public bool Disconnect()
    {
      if (this._isOpenSession)
        this._driver.DoCommand((HdmDriver.HdmCommand) new HdmDriver.LoginOutCommand());
      this._driver?.Dispose();
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintNonFiscalReport(nonFiscalStrings.Select<NonFiscalString, string>((Func<NonFiscalString, string>) (x => x.Text)).ToList<string>());
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    public KkmStatus GetStatus()
    {
      return new KkmStatus()
      {
        KkmState = KkmStatuses.Ready
      };
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => true;

    private (int id, string password) GetUser()
    {
      if (KkmHelper.UserUid == Guid.Empty)
      {
        int result;
        if (!int.TryParse(this._devicesConfig.CheckPrinter.Connection.LanPort.UserLogin, out result))
          throw new DeviceException(Translate.Hdm_GetUser_Указан_некорректный_идентификатор_пользователя_в_настройках_программы_);
        return (result, this._devicesConfig.CheckPrinter.Connection.LanPort.Password);
      }
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Entities.Users.User byUid = new UsersRepository(dataBase).GetByUid(KkmHelper.UserUid);
        int result;
        if (!int.TryParse(byUid.LoginForKkm, out result))
          throw new DeviceException(Translate.Hdm_GetUser_Указан_некорректный_идентификатор_пользователя_в_настройках_программы_);
        return (result, byUid.PasswordForKkm);
      }
    }

    private static List<Hdm.CategoryItem> LoadAmCategory()
    {
      string path = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "ArmCategory.csv");
      if (!File.Exists(path))
        return new List<Hdm.CategoryItem>();
      string str1 = File.ReadAllText(path);
      List<Hdm.CategoryItem> source = new List<Hdm.CategoryItem>();
      string[] separator = new string[1]{ "\r\n" };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        if (!str2.IsNullOrEmpty())
        {
          string[] strArray = str2.Split(new string[1]
          {
            ";----"
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length == 2)
          {
            Hdm.CategoryItem categoryItem = new Hdm.CategoryItem()
            {
              Code = strArray[0],
              Name = strArray[1]
            };
            source.Add(categoryItem);
          }
        }
      }
      return source.OrderBy<Hdm.CategoryItem, string>((Func<Hdm.CategoryItem, string>) (x => x.Code)).ToList<Hdm.CategoryItem>();
    }

    public class CategoryItem
    {
      public override string ToString() => this.DisplayName;

      public string Code { get; set; }

      public string Name { get; set; }

      public string DisplayName => this.Code + " " + this.Name;

      public Visibility Visibility { get; set; }
    }
  }
}
