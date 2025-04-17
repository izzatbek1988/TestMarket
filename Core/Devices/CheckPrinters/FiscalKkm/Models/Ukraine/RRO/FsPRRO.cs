// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.RRO.FsPRRO
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.RRO
{
  public class FsPRRO : IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _data;
    private long _numFiscal;
    private string _certificate = "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";
    private string _privateKey = "MIIDrjAcBgwrBgEEAYGXRgEBAQIwDAQEEu2O0QQEO1O5BASCA4ztEfaVaBoFG7FMD3Mu8XNXznGTk2otmo+x9DrRRme6gWZ8oOMR7hqZoG68fFzxhEQ+WOb6D+98D1WtxyFFgbs4LMQVExhves6G5f2TNuTUFgm1q8BceMxvrsdmvDQJrmas4NzfGJsLrIB8yu2cPu/qE/BuFGxRwo1iPsKOaittONRpDWzfMxhOyCX1GUnPLW1tbc2e4AsOKfSJPdMJnVAJM+iEa8C1qvgYTfwTDsR38nlK5XTCo9+8YzVLUo5E0v65HosjUv9cxMcYOBCB9d9BgSBKZe3ILO54I4txHqgiF3bIRlc5NwkY5RI7gtbGR+Pzuiz2F+ttadDWesyj1j+oU2eZHPL0TP39l2nXDOENHpsNs8jFPsOLruqGTRhQuYH8TNSdKMNUunZc0q6/0Tgw76UTYx89YmufEmQwrKZRCfERnbFtNLtinoGgA2QlfUuQWEZLZ3ZORDakWwLVVo8QzDEE3G2pQ8FTZ5kc8vRM/fv3IVXOYK+1Gjc0dyz/3fFblEo8L+eoCr5W+EuJIUPMTKGvaHB06I/BLXxKj3l7NvSPhloEVwSnMjJubzkoNPA+oHoQ4xrdosMl+NiDykf8QWNnlkwjCXEYiw1ODhJ4chZra+0jmNqZB8wHJ6JOkfrmQlGCHY1WyG12j+sGdRa4oGZPaLWspAY7yh8HglyVGAS6dR3pQIQwL8TMlL8ZpHuHEmJsL8pGwSx69iBOQo53LHr2IE5CjncsevYgTkKOd/5GJ0F+Zp3l+5kQYdA8WROLaPAgDGg1ujtGpY/ucbDKU4zyfDVxN2CycPWXL+HFoB/bp6jWmsi1PwMfIQFMx6cNZlZsP+wjojQbrOU9rlSMqYiRboiBlVqMW/5PsRyRIH4zAveabW8DE3/r3PwovDYtWU5+sWUxakvR9Mb8VzWAzVoaXSk8D0sRTt/lx/4tMitc53ZGwDJvzwr+fYyzWmV+IMiK5YFErY+dn2rowE6Z1MZTzxKf6W2iX3KClvgoICHdWrn84JgL33wDADqHWtofOe+zXvwcuVhE4JzyBak6cYSd7P6zbDMhBfCioBoHvp+F/Mah6NBxaAoonYiOYq9Jbwc0mw1pkhwZVSth2474kvKN2jZPoyiZpfpiPpo+Rv4VQnym8Pae12xgxMad6K6+gv64Ec3G++Gu7lYDOi5gMnVeB13gSdRucVLse/gUn1Dor/dDj8pE/Eqkqw==";
    private string _password = "tect4";
    private CheckContent _check;
    private List<CPayRow> _payments = new List<CPayRow>();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (FsPRRO);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show("Это настройки кассы");
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Команда: открытие смены");
      OpenSessionRequest openSessionRequest1 = new OpenSessionRequest();
      openSessionRequest1.Certificate = this._certificate;
      openSessionRequest1.PrivateKey = this._privateKey;
      openSessionRequest1.Password = this._password;
      openSessionRequest1.NumFiscal = this._numFiscal;
      OpenSessionRequest openSessionRequest2 = openSessionRequest1;
      DataExchangeHelper.SendCommand((IRequest) openSessionRequest2);
      LogHelper.Debug("Answer: " + openSessionRequest2.AnswerString);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) openSessionRequest2.Result);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Команда: получения данных для отчета");
      LastShiftTotalsRequest shiftTotalsRequest1 = new LastShiftTotalsRequest();
      shiftTotalsRequest1.Certificate = this._certificate;
      shiftTotalsRequest1.PrivateKey = this._privateKey;
      shiftTotalsRequest1.Password = this._password;
      shiftTotalsRequest1.NumFiscal = this._numFiscal;
      LastShiftTotalsRequest shiftTotalsRequest2 = shiftTotalsRequest1;
      DataExchangeHelper.SendCommand((IRequest) shiftTotalsRequest2);
      LogHelper.Debug("Ответ: " + shiftTotalsRequest2.AnswerString);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) shiftTotalsRequest2.Result);
      switch (reportType)
      {
        case ReportTypes.ZReport:
          LogHelper.Debug("Команда: закрытие смены");
          CloseSessionRequest closeSessionRequest1 = new CloseSessionRequest();
          closeSessionRequest1.Certificate = this._certificate;
          closeSessionRequest1.PrivateKey = this._privateKey;
          closeSessionRequest1.Password = this._password;
          closeSessionRequest1.NumFiscal = this._numFiscal;
          closeSessionRequest1.ZRepAuto = true;
          CloseSessionRequest closeSessionRequest2 = closeSessionRequest1;
          DataExchangeHelper.SendCommand((IRequest) closeSessionRequest2);
          LogHelper.Debug("Ответ: " + closeSessionRequest2.AnswerString);
          this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) closeSessionRequest2.Result);
          goto case ReportTypes.XReport;
        case ReportTypes.XReport:
          this.PrintReport(shiftTotalsRequest2.Result.Totals);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._data = checkData;
      this._check = new CheckContent()
      {
        CHECKHEAD = new CHead()
        {
          DOCTYPE = CheckDocumentType.SaleGoods,
          ORGNM = checkData.Cashier.Name
        }
      };
      this._payments = new List<CPayRow>();
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          this._check.CHECKHEAD.DOCSUBTYPE = CheckDocumentSubType.CheckGoods;
          break;
        case CheckTypes.ReturnSale:
          this._check.CHECKHEAD.DOCSUBTYPE = CheckDocumentSubType.CheckReturn;
          this._check.CHECKHEAD.ORDERRETNUM = checkData.FiscalNum;
          break;
      }
      this._check.CHECKTOTAL = new CTotal()
      {
        SUM = checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum))
      };
      this._check.CHECKTAX = new CTaxRow[1];
      this._check.CHECKTAX[0] = new CTaxRow();
      this._check.CHECKBODY = checkData.GoodsList.Select<CheckGood, CBodyRow>((Func<CheckGood, CBodyRow>) (x => new CBodyRow()
      {
        CODE = x.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value.ToString() ?? "1",
        BARCODE = x.Barcode,
        NAME = x.Name,
        COST = x.Sum,
        PRICE = x.Price,
        AMOUNT = x.Quantity,
        DESCRIPTION = x.Description,
        DISCOUNTSUM = x.DiscountSum,
        VALOUTCD = "980"
      })).ToArray<CBodyRow>();
      return true;
    }

    public bool CloseCheck()
    {
      this._check.CHECKPAY = this._payments.ToArray();
      LogHelper.Debug("Команда: закрытие чека");
      DocumentRequest documentRequest1 = new DocumentRequest();
      documentRequest1.Certificate = this._certificate;
      documentRequest1.PrivateKey = this._privateKey;
      documentRequest1.Password = this._password;
      documentRequest1.NumFiscal = this._numFiscal;
      documentRequest1.Check = this._check;
      DocumentRequest documentRequest2 = documentRequest1;
      DataExchangeHelper.SendCommand((IRequest) documentRequest2);
      LogHelper.Debug("Ответ: " + documentRequest2.AnswerString);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) documentRequest2.Result);
      this._data.FiscalNum = documentRequest2.Result.NumFiscal;
      this.PrintSaleDocument();
      return true;
    }

    public void CancelCheck() => throw new NotImplementedException();

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this._check = new CheckContent()
      {
        CHECKHEAD = {
          DOCTYPE = CheckDocumentType.SaleGoods,
          DOCSUBTYPE = CheckDocumentSubType.ServiceIssue,
          CASHIER = cashier.Name
        },
        CHECKTOTAL = {
          SUM = sum
        }
      };
      LogHelper.Debug("Команда: снятие денежных средств");
      DocumentRequest documentRequest1 = new DocumentRequest();
      documentRequest1.Certificate = this._certificate;
      documentRequest1.PrivateKey = this._privateKey;
      documentRequest1.Password = this._password;
      documentRequest1.NumFiscal = this._numFiscal;
      documentRequest1.Check = this._check;
      DocumentRequest documentRequest2 = documentRequest1;
      DataExchangeHelper.SendCommand((IRequest) documentRequest2);
      LogHelper.Debug("Ответ: " + documentRequest2.AnswerString);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) documentRequest2.Result);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this._check = new CheckContent()
      {
        CHECKHEAD = {
          DOCTYPE = CheckDocumentType.SaleGoods,
          DOCSUBTYPE = CheckDocumentSubType.ServiceDeposit,
          CASHIER = cashier.Name
        },
        CHECKTOTAL = {
          SUM = sum
        }
      };
      LogHelper.Debug("Команда: внесение денежных средств");
      DocumentRequest documentRequest1 = new DocumentRequest();
      documentRequest1.Certificate = this._certificate;
      documentRequest1.PrivateKey = this._privateKey;
      documentRequest1.Password = this._password;
      documentRequest1.NumFiscal = this._numFiscal;
      documentRequest1.Check = this._check;
      DocumentRequest documentRequest2 = documentRequest1;
      DataExchangeHelper.SendCommand((IRequest) documentRequest2);
      LogHelper.Debug("Ответ: " + documentRequest2.AnswerString);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) documentRequest2.Result);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType) => true;

    public bool RegisterPayment(CheckPayment payment)
    {
      this._payments.Add(new CPayRow()
      {
        SUM = payment.Sum,
        PAYFORMNM = payment.Name,
        PAYFORMCD = payment.Method == GlobalDictionaries.KkmPaymentMethods.Cash ? "0" : "1"
      });
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      LogHelper.Debug("Команда: получение номера ККМ");
      ObjectsRequest objectsRequest1 = new ObjectsRequest();
      objectsRequest1.Certificate = this._certificate;
      objectsRequest1.PrivateKey = this._privateKey;
      objectsRequest1.Password = this._password;
      ObjectsRequest objectsRequest2 = objectsRequest1;
      DataExchangeHelper.SendCommand((IRequest) objectsRequest2);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) objectsRequest2.Result);
      this._numFiscal = objectsRequest2.Result.TaxObjects.First<TaxObjectsItem>().TransactionsRegistrars.First<TransactionsRegistrarsItem>().NumFiscal;
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper() => true;

    public KkmStatus GetStatus()
    {
      LogHelper.Debug("Команда: открытие смены");
      TransactionsRegistrarStateRequest registrarStateRequest1 = new TransactionsRegistrarStateRequest();
      registrarStateRequest1.Certificate = this._certificate;
      registrarStateRequest1.PrivateKey = this._privateKey;
      registrarStateRequest1.Password = this._password;
      registrarStateRequest1.NumFiscal = this._numFiscal;
      TransactionsRegistrarStateRequest registrarStateRequest2 = registrarStateRequest1;
      DataExchangeHelper.SendCommand((IRequest) registrarStateRequest2);
      this.CheckError((Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer) registrarStateRequest2.Result);
      return new KkmStatus()
      {
        SessionStatus = registrarStateRequest2.Result.ShiftState == 0 ? SessionStatuses.Close : SessionStatuses.Open,
        SessionNumber = registrarStateRequest2.Result.ShiftId,
        CheckNumber = registrarStateRequest2.Result.NextLocalNum
      };
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    private void CheckError(Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Answer answer)
    {
      if (answer.ErrorCode == ErrorCode.InconsistentRegistrarState)
      {
        CleanupRequest cleanupRequest = new CleanupRequest();
        cleanupRequest.Certificate = this._certificate;
        cleanupRequest.PrivateKey = this._privateKey;
        cleanupRequest.Password = this._password;
        cleanupRequest.NumFiscal = this._numFiscal;
        DataExchangeHelper.SendCommand((IRequest) cleanupRequest);
        throw new Exception(Translate.FsPRRO_Во_время_выполнения_команды_РРО_возникла_ошибка__попробуйте_еще_раз);
      }
      if (answer.ErrorCode != ErrorCode.Ok)
      {
        LogHelper.Debug(answer.ErrorMessage);
        throw new Exception(answer.ErrorMessage);
      }
    }

    private void PrintSaleDocument()
    {
      new UsualPrinter(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(this._data);
    }

    private void PrintReport(ZRepContent content) => throw new NotImplementedException();

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
