// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.UzPos
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
using Gbs.Resources.Localizations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class UzPos : IFiscalKkm, IDevice
  {
    private UzPosDriver _driver;
    private UzPosDriver.OpenCheck _check;
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (UzPos);

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
        NeedAuth = false
      });
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      UzPosDriver.KkmOpenSession command = new UzPosDriver.KkmOpenSession();
      this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
      this.CheckError(command.Data);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          UzPosDriver.KkmCloseSession command = new UzPosDriver.KkmCloseSession();
          this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
          this.CheckError(command.Data);
          this.ConvertToPdf(GetPdfZReport(false));
          break;
        case ReportTypes.XReport:
          this.ConvertToPdf(GetPdfZReport(true));
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }

      string GetPdfZReport(bool isCurrent)
      {
        UzPosDriver.GetZReportInfo command = new UzPosDriver.GetZReportInfo()
        {
          ZReportId = !isCurrent ? 1 : 0
        };
        this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
        if (command.Data.Error)
          throw new KkmException((IDevice) this, command.Data.Info.ToString());
        return ((JToken) command.Data.Info).ToObject<UzPosDriver.GetZReportInfo.GetZReportInfoAnswer.MessageInfo>().Paycheck;
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().GoodsConfig.UktZedUid == Guid.Empty)
        throw new KkmException((IDevice) this, Translate.UzPos_OpenCheck_В_настройках_программы_не_указано_поле_для_ИКПУ_кода__необходимо_указать_эти_данные_в_разделе_Файл___Настройки___Оборудование___Печать_чеков_);
      Gbs.Core.Entities.Clients.Client organization = SalePoints.GetSalePointList().First<SalePoints.SalePoint>().Organization;
      string str1 = organization.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "123456789";
      UzPosDriver.OpenCheck openCheck1 = new UzPosDriver.OpenCheck();
      UzPosDriver.OpenCheck openCheck2 = openCheck1;
      string str2;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          str2 = "sale";
          break;
        case CheckTypes.ReturnSale:
          str2 = "refund";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      openCheck2.Method = str2;
      openCheck1.CompanyAddress = organization.Address;
      openCheck1.CompanyInn = str1;
      openCheck1.CompanyName = organization.Name;
      openCheck1.UserName = checkData.Cashier.Name;
      openCheck1.CompanyPhoneNumber = organization.Phone;
      openCheck1.Parameters = new UzPosDriver.OpenCheck.Params()
      {
        PaycheckNumber = checkData.Number,
        ClientName = checkData.Client?.Client.Name
      };
      this._check = openCheck1;
      if (checkData.CheckType == CheckTypes.ReturnSale)
        this._check.RefundInfo = new UzPosDriver.OpenCheck.OpenCheckAnswer.Info()
        {
          DateTime = checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.DateTimePropertyUid))?.Value.ToString() ?? "",
          FiscalSign = checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalSignPropertyUid))?.Value.ToString() ?? "",
          ReceiptSeq = checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.ReceiptSeqPropertyUid))?.Value.ToString() ?? "",
          TerminalId = checkData.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.TerminalIdPropertyUid))?.Value.ToString() ?? ""
        };
      return true;
    }

    public bool CloseCheck()
    {
      this._driver.DoCommand((UzPosDriver.UzPosCommand) this._check);
      this.CheckError((UzPosDriver.UzPosAnswer) this._check.Data);
      UzPosDriver.OpenCheck.OpenCheckAnswer data = this._check.Data;
      this._checkData.CustomData.Add("QRCodeUrl", (object) data.CheckInfo.QrCode);
      this._checkData.CustomData.Add("FiscalSign", (object) data.CheckInfo.FiscalSign);
      this._checkData.CustomData.Add("ReceiptSeq", (object) data.CheckInfo.ReceiptSeq);
      this._checkData.CustomData.Add("TerminalId", (object) data.CheckInfo.TerminalId);
      this._checkData.CustomData.Add("ReceivedCash", (object) this._check.Parameters.ReceivedCash);
      this._checkData.CustomData.Add("ReceivedCard", (object) this._check.Parameters.ReceivedCard);
      this._checkData.CustomData.Add("CheckType", (object) this._checkData.CheckType);
      this._checkData.CustomData.Add("ChekTuri", (object) "Xarid");
      if (this._checkData.CheckType == CheckTypes.Sale)
      {
        List<EntityProperties.PropertyValue> properties1 = this._checkData.Properties;
        EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
        propertyType1.Uid = GlobalDictionaries.TerminalIdPropertyUid;
        propertyValue1.Type = propertyType1;
        propertyValue1.Value = (object) data.CheckInfo.TerminalId;
        properties1.Add(propertyValue1);
        List<EntityProperties.PropertyValue> properties2 = this._checkData.Properties;
        EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
        propertyType2.Uid = GlobalDictionaries.ReceiptSeqPropertyUid;
        propertyValue2.Type = propertyType2;
        propertyValue2.Value = (object) data.CheckInfo.ReceiptSeq;
        properties2.Add(propertyValue2);
        List<EntityProperties.PropertyValue> properties3 = this._checkData.Properties;
        EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
        propertyType3.Uid = GlobalDictionaries.FiscalSignPropertyUid;
        propertyValue3.Type = propertyType3;
        propertyValue3.Value = (object) data.CheckInfo.FiscalSign;
        properties3.Add(propertyValue3);
        List<EntityProperties.PropertyValue> properties4 = this._checkData.Properties;
        EntityProperties.PropertyValue propertyValue4 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType4 = new EntityProperties.PropertyType();
        propertyType4.Uid = GlobalDictionaries.DateTimePropertyUid;
        propertyValue4.Type = propertyType4;
        propertyValue4.Value = (object) data.CheckInfo.DateTime;
        properties4.Add(propertyValue4);
        LogHelper.Debug(this._checkData.Properties.ToJsonString(true));
      }
      this.PrintDocument();
      return true;
    }

    private void PrintDocument()
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(this._checkData);
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier) => true;

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier) => true;

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRates = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates;
      Gbs.Core.Config.FiscalKkm.TaxRate taxRate = taxRates.SingleOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == good.TaxRateNumber)).Value ?? taxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.DefaultTaxRate)).Value;
      string str = good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == new ConfigsRepository<Gbs.Core.Config.Settings>().Get().GoodsConfig.UktZedUid))?.Value.ToString() ?? "";
      if (str.IsNullOrEmpty())
        throw new KkmException((IDevice) this, string.Format(Translate.UzPos_RegisterGood_Для_товара__0__не_указан_код_ИКПУ_в_карточке_товара__продажа_невозможна__Удалите_товар_из_чека_или_укажите_код_в_карточке_товара_, (object) good.Name));
      this._check.Parameters.Items.Add(new UzPosDriver.OpenCheck.Item()
      {
        Price = (int) (good.Sum * 100M) + (int) (good.DiscountSum * 100M),
        Name = good.Name,
        Barcode = good.Barcode,
        Discount = (int) (good.DiscountSum * 100M),
        Amount = (int) (good.Quantity * 1000M),
        VatPercent = taxRate.TaxValue == -1M ? 0 : (int) taxRate.TaxValue,
        Vat = (int) (good.Sum.GetNdsSum(taxRate.TaxValue) * 100M),
        ClassCode = str,
        Other = 0,
        PackageCode = good.Unit.Code,
        OwnerType = good.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service ? 2 : 0
      });
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          this._check.Parameters.ReceivedCash += (int) (payment.Sum * 100M);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          this._check.Parameters.ReceivedCard += (int) (payment.Sum * 100M);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          this._check.Parameters.ReceivedCard += (int) (payment.Sum * 100M);
          break;
        default:
          return true;
      }
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._driver = new UzPosDriver(devicesConfig);
      UzPosDriver.KkmCheckStatus command = new UzPosDriver.KkmCheckStatus();
      this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
      this.CheckError(command.Data);
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
      KkmStatus status = new KkmStatus();
      UzPosDriver.GetZReportInfo command = new UzPosDriver.GetZReportInfo();
      this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
      if (command.Data.Error)
      {
        command.ZReportId = 1;
        this._driver.DoCommand((UzPosDriver.UzPosCommand) command);
      }
      if (command.Data.Error)
      {
        LogHelper.Debug("Не удалось получить данные о ККМ");
        return new KkmStatus()
        {
          KkmState = KkmStatuses.Unknown
        };
      }
      UzPosDriver.GetZReportInfo.GetZReportInfoAnswer.MessageInfo messageInfo = ((JToken) command.Data.Info).ToObject<UzPosDriver.GetZReportInfo.GetZReportInfoAnswer.MessageInfo>();
      status.SessionStatus = messageInfo.CloseTime.IsNullOrEmpty() ? SessionStatuses.Open : SessionStatuses.Close;
      status.SessionNumber = messageInfo.Number;
      status.SessionStarted = messageInfo.OpenTime.IsNullOrEmpty() ? new DateTime?() : new DateTime?(Convert.ToDateTime(messageInfo.OpenTime));
      status.Model = messageInfo.TerminalId;
      status.CheckNumber = messageInfo.TotalSaleCount + messageInfo.TotalRefundCount;
      status.KkmState = KkmStatuses.Ready;
      return status;
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => true;

    private void ConvertToPdf(string base64String)
    {
      byte[] bytes = Convert.FromBase64String(base64String);
      string str = Path.Combine(FileSystemHelper.TempFolderPath(), "cheque.pdf");
      File.WriteAllBytes(str, bytes);
      Process.Start(str);
    }

    private void CheckError(UzPosDriver.UzPosAnswer answer)
    {
      if (answer.Error)
        throw new KkmException((IDevice) this, answer.Message);
    }
  }
}
