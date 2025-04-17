// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.HelpMicroJson
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class HelpMicroJson : IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;
    private static readonly Dictionary<string, string> ErrorCodes = HelpMicroJson.LoadErrorText();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "HelpMicro";

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    private HelpMicroDriver CurrentDriver { get; set; }

    private HelpMicroDriver.KkmRegistreCheckCommand CurrentCheck { get; set; }

    private void CheckResult(string errCode)
    {
      if (!errCode.IsNullOrEmpty())
      {
        string str = "";
        HelpMicroJson.ErrorCodes?.TryGetValue(errCode, out str);
        string deviceMessage = str.IsNullOrEmpty() ? Translate.HelpMicroJson_CheckResult_Код__ + errCode : str + " (код: " + errCode + ")";
        KkmException.ErrorTypes errorTypes;
        switch (errCode)
        {
          case "x25":
            errorTypes = KkmException.ErrorTypes.NoPaper;
            break;
          case "x88":
            errorTypes = KkmException.ErrorTypes.NoPaper;
            break;
          default:
            errorTypes = KkmException.ErrorTypes.Unknown;
            break;
        }
        KkmException.ErrorTypes errorType = errorTypes;
        throw new KkmException((IDevice) this, deviceMessage, errorType);
      }
    }

    public bool SendDigitalCheck(string address) => true;

    public bool ContinuePrint() => true;

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult();

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Снимаем отчет на ККМ Хелп Микро");
      string errCode;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          errCode = this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.GetZReportCommand());
          break;
        case ReportTypes.XReport:
          errCode = this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.GetXReportCommand());
          break;
        case ReportTypes.XReportWithGoods:
          errCode = this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.GetZReportAndGoodsCommand());
          break;
        default:
          errCode = string.Empty;
          break;
      }
      this.CheckResult(errCode);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Открываем чек на ККМ Хелп Микро");
      this.CurrentCheck = new HelpMicroDriver.KkmRegistreCheckCommand();
      this._checkData = checkData;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          this.CurrentCheck.R = (List<HelpMicroDriver.chequeStrings>) null;
          this.CurrentCheck.F.Add(new HelpMicroDriver.chequeStrings()
          {
            C = new HelpMicroDriver.chequeStrings.comment()
            {
              cm = string.Format(Translate.HelpMicroJson_Кассир___0_, (object) (checkData.Cashier?.Name ?? ""))
            }
          });
          break;
        case CheckTypes.ReturnSale:
          this.CurrentCheck.F = (List<HelpMicroDriver.chequeStrings>) null;
          this.CurrentCheck.R.Add(new HelpMicroDriver.chequeStrings()
          {
            C = new HelpMicroDriver.chequeStrings.comment()
            {
              cm = string.Format(Translate.HelpMicroJson_Кассир___0_, (object) (checkData.Cashier?.Name ?? ""))
            }
          });
          break;
      }
      return true;
    }

    public bool CloseCheck()
    {
      int indexCard = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.PaymentsMethods.Single<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (p => p.Key == GlobalDictionaries.KkmPaymentMethods.Card)).Value;
      LogHelper.Debug(this._checkData.Properties.ToJsonString(true));
      LogHelper.Debug(this.CurrentCheck.ToJsonString(true));
      if (this.CurrentCheck.F != null && this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
      {
        Guid? uid = x?.Type?.Uid;
        Guid rrnUid = GlobalDictionaries.RrnUid;
        return uid.HasValue && uid.GetValueOrDefault() == rrnUid;
      }))?.Value != null && this.CurrentCheck.F.Any<HelpMicroDriver.chequeStrings>((Func<HelpMicroDriver.chequeStrings, bool>) (x =>
      {
        if (x == null)
          return false;
        int? no = x.P?.no;
        int num = indexCard;
        return no.GetValueOrDefault() == num & no.HasValue;
      })))
      {
        string str1 = this._checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.IssuerNameUid))?.Value.ToString() ?? "";
        string str2 = str1.IsNullOrEmpty() ? SalePoints.GetSalePointList().First<SalePoints.SalePoint>().Organization.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.BankNameUid))?.Value.ToString() ?? string.Empty : str1;
        LogHelper.Debug("Нашли данные об оплате картой, добавляем в чек");
        this.CurrentCheck.F[this.CurrentCheck.F.FindIndex((Predicate<HelpMicroDriver.chequeStrings>) (x =>
        {
          if (x == null)
            return false;
          int? no = x.P?.no;
          int num = indexCard;
          return no.GetValueOrDefault() == num & no.HasValue;
        }))].P.authcode = this._checkData.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          Guid? uid = x.Type?.Uid;
          Guid approvalCodeUid = GlobalDictionaries.ApprovalCodeUid;
          return uid.HasValue && uid.GetValueOrDefault() == approvalCodeUid;
        })).Value.ToString();
        this.CurrentCheck.F[this.CurrentCheck.F.FindIndex((Predicate<HelpMicroDriver.chequeStrings>) (x =>
        {
          if (x == null)
            return false;
          int? no = x.P?.no;
          int num = indexCard;
          return no.GetValueOrDefault() == num & no.HasValue;
        }))].P.bank = str2;
        this.CurrentCheck.F[this.CurrentCheck.F.FindIndex((Predicate<HelpMicroDriver.chequeStrings>) (x =>
        {
          if (x == null)
            return false;
          int? no = x.P?.no;
          int num = indexCard;
          return no.GetValueOrDefault() == num & no.HasValue;
        }))].P.card = this._checkData.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          Guid? uid = x.Type?.Uid;
          Guid cardNumberUid = GlobalDictionaries.CardNumberUid;
          return uid.HasValue && uid.GetValueOrDefault() == cardNumberUid;
        })).Value.ToString();
        this.CurrentCheck.F[this.CurrentCheck.F.FindIndex((Predicate<HelpMicroDriver.chequeStrings>) (x =>
        {
          if (x == null)
            return false;
          int? no = x.P?.no;
          int num = indexCard;
          return no.GetValueOrDefault() == num & no.HasValue;
        }))].P.rrn = this._checkData.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          Guid? uid = x.Type?.Uid;
          Guid rrnUid = GlobalDictionaries.RrnUid;
          return uid.HasValue && uid.GetValueOrDefault() == rrnUid;
        })).Value.ToString();
        this.CurrentCheck.F[this.CurrentCheck.F.FindIndex((Predicate<HelpMicroDriver.chequeStrings>) (x =>
        {
          if (x == null)
            return false;
          int? no = x.P?.no;
          int num = indexCard;
          return no.GetValueOrDefault() == num & no.HasValue;
        }))].P.term = this._checkData.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          Guid? uid = x.Type?.Uid;
          Guid terminalIdUid = GlobalDictionaries.TerminalIdUid;
          return uid.HasValue && uid.GetValueOrDefault() == terminalIdUid;
        })).Value.ToString();
        LogHelper.Debug(this.CurrentCheck.ToJsonString(true));
      }
      else
        LogHelper.Debug("Не нашли данные об оплате картой");
      LogHelper.Debug("Закрываем чек на ККМ Хелп Микро");
      this.CheckResult(this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) this.CurrentCheck));
      return true;
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Снятие через хелп-микро");
      this.CheckResult(this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.KkmPaymentActionCommand()
      {
        IO = new List<HelpMicroDriver.chequeStrings>()
        {
          new HelpMicroDriver.chequeStrings()
          {
            IO = new HelpMicroDriver.chequeStrings.inOut()
            {
              sum = -sum
            }
          },
          new HelpMicroDriver.chequeStrings()
          {
            C = new HelpMicroDriver.chequeStrings.comment()
            {
              cm = string.Format(Translate.HelpMicroJson_Кассир___0_, (object) (cashier?.Name ?? ""))
            }
          }
        }
      }));
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Внесение через хелп-микро");
      this.CheckResult(this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.KkmPaymentActionCommand()
      {
        IO = new List<HelpMicroDriver.chequeStrings>()
        {
          new HelpMicroDriver.chequeStrings()
          {
            IO = new HelpMicroDriver.chequeStrings.inOut()
            {
              sum = sum
            }
          },
          new HelpMicroDriver.chequeStrings()
          {
            C = new HelpMicroDriver.chequeStrings.comment()
            {
              cm = string.Format(Translate.HelpMicroJson_Кассир___0_, (object) (cashier?.Name ?? ""))
            }
          }
        }
      }));
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      HelpMicroDriver.GetKkmSumCommand getKkmSumCommand = new HelpMicroDriver.GetKkmSumCommand();
      getKkmSumCommand.Answer = this.CurrentDriver.SendGetRequest(getKkmSumCommand.Command);
      sum = getKkmSumCommand.CashItems.Where<HelpMicroDriver.cashItem>((Func<HelpMicroDriver.cashItem, bool>) (x => x.no == 1)).Sum<HelpMicroDriver.cashItem>((Func<HelpMicroDriver.cashItem, Decimal>) (x => x.sum));
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      HelpMicroDriver.chequeStrings chequeStrings1 = new HelpMicroDriver.chequeStrings()
      {
        S = new HelpMicroDriver.chequeStrings.good()
        {
          name = good.Good.Name,
          price = good.Price,
          qty = good.Quantity,
          tax = good.TaxRateNumber,
          code = Convert.ToInt32(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (g => g.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) 0)
        }
      };
      if (good.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol)
        chequeStrings1.S.excise.Add(new HelpMicroDriver.chequeStrings.exciseStamp()
        {
          stamp = good.Description
        });
      Guid uktZedUid = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().GoodsConfig.UktZedUid;
      string str1 = KkmHelper.RemoveSpaceAndEnter(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uktZedUid))?.Value.ToString() ?? "");
      string str2 = ((IEnumerable<char>) str1.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit)) ? str1 : "";
      if (!str2.IsNullOrEmpty())
        chequeStrings1.S.uktzed = str2;
      HelpMicroDriver.chequeStrings chequeStrings2 = new HelpMicroDriver.chequeStrings()
      {
        D = new HelpMicroDriver.chequeStrings.discount()
        {
          sum = -good.DiscountSum,
          all = 0
        }
      };
      if (!chequeStrings1.S.excise.Any<HelpMicroDriver.chequeStrings.exciseStamp>())
        chequeStrings1.S.excise = (List<HelpMicroDriver.chequeStrings.exciseStamp>) null;
      switch (checkType)
      {
        case CheckTypes.Sale:
          this.CurrentCheck.F.Add(chequeStrings1);
          if (good.Discount > 0M)
          {
            this.CurrentCheck.F.Add(chequeStrings2);
            break;
          }
          break;
        case CheckTypes.ReturnSale:
          this.CurrentCheck.R.Add(chequeStrings1);
          if (good.Discount > 0M)
          {
            this.CurrentCheck.R.Add(chequeStrings2);
            break;
          }
          break;
      }
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      HelpMicroDriver.chequeStrings chequeStrings = new HelpMicroDriver.chequeStrings()
      {
        P = new HelpMicroDriver.chequeStrings.payment()
        {
          sum = payment.Sum,
          no = devices.CheckPrinter.FiscalKkm.PaymentsMethods.Single<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (p => p.Key == payment.Method)).Value
        }
      };
      if (this.CurrentCheck.F != null)
        this.CurrentCheck.F.Add(chequeStrings);
      else
        this.CurrentCheck.R.Add(chequeStrings);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      if (description.IsNullOrEmpty())
      {
        HelpMicroDriver.chequeStrings chequeStrings = new HelpMicroDriver.chequeStrings()
        {
          C = new HelpMicroDriver.chequeStrings.comment()
          {
            cm = string.Format("{0}: {1:N2}", (object) description, (object) sum)
          }
        };
        if (this.CurrentCheck.F != null)
          this.CurrentCheck.F.Add(chequeStrings);
        else
          this.CurrentCheck.R.Add(chequeStrings);
      }
      HelpMicroDriver.chequeStrings chequeStrings1 = new HelpMicroDriver.chequeStrings()
      {
        D = new HelpMicroDriver.chequeStrings.discount()
        {
          sum = -sum,
          all = 1
        }
      };
      if (this.CurrentCheck.F != null)
        this.CurrentCheck.F.Add(chequeStrings1);
      else
        this.CurrentCheck.R.Add(chequeStrings1);
      return true;
    }

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      Other.SetAllowUnsafeHeaderParsing();
      if (onlyDriverLoad)
        return;
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.CurrentDriver = new HelpMicroDriver(dc.CheckPrinter.Connection.LanPort);
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      this.CheckResult(this.CurrentDriver.SendPostRequest((HelpMicroDriver.HelpMicroCommand) new HelpMicroDriver.PrintNonFiscalTextCommand()
      {
        P = nonFiscalStrings.Select<NonFiscalString, HelpMicroDriver.chequeStrings>((Func<NonFiscalString, HelpMicroDriver.chequeStrings>) (x => new HelpMicroDriver.chequeStrings()
        {
          N = new HelpMicroDriver.chequeStrings.nonFiscalText()
          {
            cm = x.Text
          }
        })).ToList<HelpMicroDriver.chequeStrings>()
      }));
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper() => true;

    public KkmStatus GetStatus() => new KkmStatus();

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    private static Dictionary<string, string> LoadErrorText()
    {
      string path = "help-micro-errors.json";
      if (!File.Exists(path))
        return (Dictionary<string, string>) null;
      try
      {
        return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка чтение текста ошибки из файла для HelpMicro: " + ex.Message);
        return (Dictionary<string, string>) null;
      }
    }
  }
}
