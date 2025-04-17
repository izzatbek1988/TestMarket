// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.GbsUzOnlineKkm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan
{
  public class GbsUzOnlineKkm : IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    public string Name => "FiscalModule UZ";

    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData CheckData { get; set; }

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show("Это настройки кассы");
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Команда: открытие смены");
      OpenSessionRequest openSessionRequest = new OpenSessionRequest();
      DataExchangeHelper.SendCommand((IRequest) openSessionRequest);
      LogHelper.Debug("Answer: " + openSessionRequest.AnswerString);
      LogHelper.Debug("Result: " + openSessionRequest.Result.ToJsonString(true));
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          LogHelper.Debug("Команда: закрытие смены");
          CloseSessionRequest closeSessionRequest = new CloseSessionRequest();
          DataExchangeHelper.SendCommand((IRequest) closeSessionRequest);
          LogHelper.Debug("Ответ: " + closeSessionRequest.AnswerString);
          if (closeSessionRequest.Error == null)
            break;
          throw new Exception(Translate.GbsUzOnlineKkm_Ошибка_снятия_отчета_на_фискальном_модуле);
        case ReportTypes.XReport:
          throw new NotImplementedException();
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.CheckData = checkData;
      return true;
    }

    public bool CloseCheck()
    {
      LogHelper.Debug("Команда: закрытие чека");
      Receipt receipt = new Receipt()
      {
        Items = new List<ItemReceipt>(),
        Time = this.CheckData.DateTime,
        ReceivedCash = (int) (this.CheckData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)) * 100M),
        ReceivedCard = (int) (this.CheckData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Card)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)) * 100M)
      };
      foreach (CheckGood goods in this.CheckData.GoodsList)
        receipt.Items.Add(new ItemReceipt()
        {
          Name = goods.Name,
          Amount = (int) (goods.Quantity * 1000M),
          Price = (int) (goods.Price * 100M),
          Barcode = goods.Barcode,
          Discount = (int) (goods.DiscountSum * 100M)
        });
      ParamsSendSale paramsSendSale = new ParamsSendSale(receipt);
      string answerString;
      ErrorInfo error;
      string qrCodeUrl;
      switch (this.CheckData.CheckType)
      {
        case CheckTypes.Sale:
          SendSaleRequest sendSaleRequest = new SendSaleRequest();
          sendSaleRequest.Params = (object) paramsSendSale;
          DataExchangeHelper.SendCommand((IRequest) sendSaleRequest);
          answerString = sendSaleRequest.AnswerString;
          error = sendSaleRequest.Error;
          qrCodeUrl = sendSaleRequest.Result?.QrCodeUrl;
          break;
        case CheckTypes.ReturnSale:
          SendReturnRequest sendReturnRequest = new SendReturnRequest();
          sendReturnRequest.Params = (object) paramsSendSale;
          DataExchangeHelper.SendCommand((IRequest) sendReturnRequest);
          answerString = sendReturnRequest.AnswerString;
          error = sendReturnRequest.Error;
          qrCodeUrl = sendReturnRequest.Result?.QrCodeUrl;
          break;
        case CheckTypes.Buy:
          throw new NotImplementedException();
        case CheckTypes.ReturnBuy:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException();
      }
      LogHelper.Debug("Ответ: " + answerString);
      if (error != null)
        return false;
      this.CheckData.CustomData.Add("QRCodeUrl", (object) (qrCodeUrl ?? string.Empty));
      this.PrintDocument();
      return true;
    }

    private void PrintDocument()
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(this.CheckData);
    }

    public void CancelCheck() => throw new NotImplementedException();

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      throw new NotSupportedException();
    }

    public bool GetCashSum(out Decimal sum)
    {
      sum = 100000000M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType) => true;

    public bool RegisterPayment(CheckPayment payment) => true;

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public bool GetCheckRemainder(out Decimal remainder) => throw new NotImplementedException();

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus();
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.GetZReportInfo getZreportInfo = new Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.GetZReportInfo();
      DataExchangeHelper.SendCommand((IRequest) getZreportInfo);
      if (!getZreportInfo.Result.OpenTime.HasValue)
      {
        status.SessionStatus = SessionStatuses.Close;
      }
      else
      {
        status.SessionStatus = SessionStatuses.Open;
        status.SessionStarted = new DateTime?(getZreportInfo.Result.OpenTime.GetValueOrDefault());
      }
      status.SoftwareVersion = getZreportInfo.Result.AppletVersion;
      status.FactoryNumber = getZreportInfo.Result.TerminalId;
      status.SessionNumber = getZreportInfo.Result.Number;
      status.Model = "FiscalDriverApi";
      return status;
    }

    public bool OpenCashDrawer() => throw new NotImplementedException();

    public bool SendDigitalCheck(string adress) => throw new NotSupportedException();

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
