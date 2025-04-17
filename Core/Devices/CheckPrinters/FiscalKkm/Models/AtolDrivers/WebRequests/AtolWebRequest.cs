// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolWebRequest
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class AtolWebRequest : IFiscalKkm, IDevice
  {
    private AtolWebRequestsDriver.FiscalDocument _document;
    private Gbs.Core.Config.Devices _deviceConfig;
    private readonly DateTime _defaultDateTime = new DateTime(1970, 1, 1);
    private AtolWebRequestsDriver.IndustryInfo _industryInfo;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.AtolWebRequests_Name_АТОЛ_Web_Requests;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
    }

    public bool IsCanHoldConnection => false;

    private AtolWebRequestsDriver CurrentDriver { get; set; }

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult();

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = true,
        Type = GlobalDictionaries.Devices.ConnectionTypes.Lan
      });
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.OpenShift()
      {
        Cashier = new AtolWebRequestsDriver.Cashier(cashier)
      });
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.ZReport()
          {
            Cashier = new AtolWebRequestsDriver.Cashier(cashier)
          });
          break;
        case ReportTypes.XReport:
          this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.XReport()
          {
            Cashier = new AtolWebRequestsDriver.Cashier(cashier)
          });
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
      }
    }

    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.CancelMarkingCodeValidation());
      AtolWebRequestsDriver.ValidationMarkingCode command1 = new AtolWebRequestsDriver.ValidationMarkingCode();
      foreach (CheckGood good in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x => x.MarkedInfo.IsValidCode())))
      {
        command1.MarkingCode = new AtolWebRequestsDriver.MarkingCode(good, checkData.CheckType, false);
        this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command1);
        AtolWebRequestsDriver.StatusValidationMarkingCode command2 = new AtolWebRequestsDriver.StatusValidationMarkingCode();
        bool flag = false;
        for (int index = 0; index < 50; ++index)
        {
          this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command2);
          if (command2.Result.IsReady)
          {
            flag = true;
            break;
          }
          Thread.Sleep(100);
        }
        good.MarkedInfo.ValidationResultKkm = flag ? (object) command2.Result.OnlineValidation.Result.ToJsonString(true) : (object) (string) null;
        this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.AcceptMarkingCode());
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.Ffd120CodeValidation(checkData);
      AtolWebRequestsDriver.FiscalDocument fiscalDocument1 = new AtolWebRequestsDriver.FiscalDocument();
      fiscalDocument1.Client = checkData.Client == null ? (AtolWebRequestsDriver.Client) null : new AtolWebRequestsDriver.Client(checkData.Client);
      fiscalDocument1.Cashier = new AtolWebRequestsDriver.Cashier(checkData.Cashier);
      AtolWebRequestsDriver.FiscalDocument fiscalDocument2 = fiscalDocument1;
      DocumentTypeEnum documentTypeEnum;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          documentTypeEnum = DocumentTypeEnum.Sell;
          break;
        case CheckTypes.ReturnSale:
          documentTypeEnum = DocumentTypeEnum.SellReturn;
          break;
        case CheckTypes.Buy:
          documentTypeEnum = DocumentTypeEnum.Buy;
          break;
        case CheckTypes.ReturnBuy:
          documentTypeEnum = DocumentTypeEnum.BuyReturn;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      fiscalDocument2.DocumentType = documentTypeEnum;
      fiscalDocument1.IgnoreNonFiscalPrintErrors = true;
      AtolWebRequestsDriver.FiscalDocument fiscalDocument3 = fiscalDocument1;
      TaxSystemTypeEnum taxSystemTypeEnum;
      switch (checkData.RuTaxSystem)
      {
        case GlobalDictionaries.RuTaxSystems.Osn:
          taxSystemTypeEnum = TaxSystemTypeEnum.Osn;
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohod:
          taxSystemTypeEnum = TaxSystemTypeEnum.UsnIncome;
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
          taxSystemTypeEnum = TaxSystemTypeEnum.UsnIncomeOutcome;
          break;
        case GlobalDictionaries.RuTaxSystems.Esn:
          taxSystemTypeEnum = TaxSystemTypeEnum.Esn;
          break;
        case GlobalDictionaries.RuTaxSystems.Psn:
          taxSystemTypeEnum = TaxSystemTypeEnum.Patent;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      fiscalDocument3.TaxationSystemType = taxSystemTypeEnum;
      fiscalDocument1.Electronically = this._deviceConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck;
      this._document = fiscalDocument1;
      this.GetSalesNotice(checkData.TrueApiInfoForKkm);
      return true;
    }

    public bool CloseCheck()
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) this._document);
      return true;
    }

    public void CancelCheck() => throw new NotImplementedException();

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.OperationСash()
      {
        Cashier = new AtolWebRequestsDriver.Cashier(cashier),
        СashSum = sum,
        Operation = AtolWebRequestsDriver.OperationСash.TypeOperation.Out
      });
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.OperationСash()
      {
        Cashier = new AtolWebRequestsDriver.Cashier(cashier),
        СashSum = sum,
        Operation = AtolWebRequestsDriver.OperationСash.TypeOperation.In
      });
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      AtolWebRequestsDriver.GetCashSum command = new AtolWebRequestsDriver.GetCashSum();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command);
      sum = command.Result.Counters.CashSum;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      List<AtolWebRequestsDriver.Item> items = this._document.Items;
      AtolWebRequestsDriver.FiscalDocument.Position position1 = new AtolWebRequestsDriver.FiscalDocument.Position();
      position1.Amount = good.Sum;
      position1.Name = good.Name;
      position1.Price = good.Price;
      position1.Quantity = good.Quantity;
      AtolWebRequestsDriver.FiscalDocument.Position position2 = position1;
      GoodsUnits.GoodUnit unit1 = good.Unit;
      int num = (unit1 != null ? unit1.RuFfdUnitsIndex : 0) == 0 ? 1 : 0;
      position2.Piece = num != 0;
      AtolWebRequestsDriver.FiscalDocument.Position position3 = position1;
      GoodsUnits.GoodUnit unit2 = good.Unit;
      int ruFfdUnitsIndex = unit2 != null ? unit2.RuFfdUnitsIndex : 0;
      position3.Unit = ruFfdUnitsIndex;
      AtolWebRequestsDriver.FiscalDocument.Position position4 = position1;
      PaymentMethodEnum paymentMethodEnum;
      switch (good.RuFfdPaymentModeCode)
      {
        case GlobalDictionaries.RuFfdPaymentModes.None:
          paymentMethodEnum = PaymentMethodEnum.FullPayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PrePaymentFull:
          paymentMethodEnum = PaymentMethodEnum.FullPrepayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.Prepayment:
          paymentMethodEnum = PaymentMethodEnum.Prepayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.AdvancePayment:
          paymentMethodEnum = PaymentMethodEnum.Advance;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.FullPayment:
          paymentMethodEnum = PaymentMethodEnum.FullPayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PartPaymentAndCredit:
          paymentMethodEnum = PaymentMethodEnum.PartialPayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.FullCredit:
          paymentMethodEnum = PaymentMethodEnum.Credit;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PaymentForCredit:
          paymentMethodEnum = PaymentMethodEnum.CreditPayment;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      position4.PaymentMethod = paymentMethodEnum;
      position1.PaymentObject = ((int) good.RuFfdGoodTypeCode).ToString();
      position1.Department = good.Good.Group.KkmSectionNumber == 0 ? 1 : good.Good.Group.KkmSectionNumber;
      AtolWebRequestsDriver.FiscalDocument.Position position5 = position1;
      AtolWebRequestsDriver.Tax tax1 = new AtolWebRequestsDriver.Tax();
      AtolWebRequestsDriver.Tax tax2 = tax1;
      TaxTypeEnum taxTypeEnum;
      switch (good.TaxRateNumber)
      {
        case 1:
          taxTypeEnum = TaxTypeEnum.None;
          break;
        case 2:
          taxTypeEnum = TaxTypeEnum.Vat0;
          break;
        case 3:
          taxTypeEnum = TaxTypeEnum.Vat10;
          break;
        case 4:
          taxTypeEnum = TaxTypeEnum.Vat20;
          break;
        case 5:
          taxTypeEnum = TaxTypeEnum.Vat110;
          break;
        case 6:
          taxTypeEnum = TaxTypeEnum.Vat120;
          break;
        case 7:
          taxTypeEnum = TaxTypeEnum.Vat5;
          break;
        case 8:
          taxTypeEnum = TaxTypeEnum.Vat7;
          break;
        case 9:
          taxTypeEnum = TaxTypeEnum.Vat105;
          break;
        case 10:
          taxTypeEnum = TaxTypeEnum.Vat107;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      tax2.Type = taxTypeEnum;
      position5.Tax = tax1;
      position1.MarkingInfo = new AtolWebRequestsDriver.MarkingCode(good, checkType, true);
      position1.IndustryInfos = new List<AtolWebRequestsDriver.IndustryInfo>()
      {
        this._industryInfo
      };
      items.Add((AtolWebRequestsDriver.Item) position1);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      List<AtolWebRequestsDriver.FiscalDocument.Payment> payments = this._document.Payments;
      AtolWebRequestsDriver.FiscalDocument.Payment payment1 = new AtolWebRequestsDriver.FiscalDocument.Payment();
      payment1.Sum = payment.Sum;
      AtolWebRequestsDriver.FiscalDocument.Payment payment2 = payment1;
      PaymentType paymentType;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          paymentType = PaymentType.Cash;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          paymentType = PaymentType.Electronically;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          paymentType = PaymentType.Electronically;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          paymentType = PaymentType.Credit;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          paymentType = PaymentType.Prepaid;
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          paymentType = PaymentType.Electronically;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      payment2.Type = paymentType;
      payments.Add(payment1);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      if (onlyDriverLoad)
        return;
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (dc.CheckPrinter.FiscalKkm.FfdVersion < GlobalDictionaries.Devices.FfdVersions.Ffd120)
        throw new KkmException((IDevice) this, "Использование АТОЛ WebRequest возможно только при работе с ФФД версии 1.2 и выше.");
      this._deviceConfig = dc;
      this.CurrentDriver = new AtolWebRequestsDriver(dc);
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.PrintNonFiscal()
      {
        Items = new List<AtolWebRequestsDriver.Item>((IEnumerable<AtolWebRequestsDriver.Item>) nonFiscalStrings.Select<NonFiscalString, AtolWebRequestsDriver.PrintNonFiscal.Text>((Func<NonFiscalString, AtolWebRequestsDriver.PrintNonFiscal.Text>) (x =>
        {
          AtolWebRequestsDriver.PrintNonFiscal.Text text1 = new AtolWebRequestsDriver.PrintNonFiscal.Text();
          AtolWebRequestsDriver.PrintNonFiscal.Text text2 = text1;
          AlignmentEnum alignmentEnum;
          switch (x.Alignment)
          {
            case TextAlignment.Left:
              alignmentEnum = AlignmentEnum.Left;
              break;
            case TextAlignment.Right:
              alignmentEnum = AlignmentEnum.Right;
              break;
            case TextAlignment.Center:
              alignmentEnum = AlignmentEnum.Center;
              break;
            case TextAlignment.Justify:
              alignmentEnum = AlignmentEnum.Center;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          text2.Alignment = alignmentEnum;
          text1.DoubleWidth = x.WideFont;
          return text1;
        })))
      });
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      AtolWebRequestsDriver currentDriver = this.CurrentDriver;
      AtolWebRequestsDriver.PrintNonFiscal command = new AtolWebRequestsDriver.PrintNonFiscal();
      AtolWebRequestsDriver.PrintNonFiscal printNonFiscal = command;
      List<AtolWebRequestsDriver.Item> objList1 = new List<AtolWebRequestsDriver.Item>();
      List<AtolWebRequestsDriver.Item> objList2 = objList1;
      AtolWebRequestsDriver.PrintNonFiscal.Barcode barcode1 = new AtolWebRequestsDriver.PrintNonFiscal.Barcode();
      barcode1.BarcodeText = code;
      AtolWebRequestsDriver.PrintNonFiscal.Barcode barcode2 = barcode1;
      AtolWebRequestsDriver.PrintNonFiscal.Barcode.BarcodeTypeEnum barcodeTypeEnum;
      switch (type)
      {
        case BarcodeTypes.None:
          barcodeTypeEnum = AtolWebRequestsDriver.PrintNonFiscal.Barcode.BarcodeTypeEnum.Code128;
          break;
        case BarcodeTypes.Ean13:
          barcodeTypeEnum = AtolWebRequestsDriver.PrintNonFiscal.Barcode.BarcodeTypeEnum.Ean13;
          break;
        case BarcodeTypes.QrCode:
          barcodeTypeEnum = AtolWebRequestsDriver.PrintNonFiscal.Barcode.BarcodeTypeEnum.Qr;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
      }
      barcode2.BarcodeType = barcodeTypeEnum;
      objList2.Add((AtolWebRequestsDriver.Item) barcode1);
      printNonFiscal.Items = objList1;
      currentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command);
      return true;
    }

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus()
    {
      AtolWebRequestsDriver.GetShiftStatus command = new AtolWebRequestsDriver.GetShiftStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command);
      AtolWebRequestsDriver.GetDeviceStatus getDeviceStatus = new AtolWebRequestsDriver.GetDeviceStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) getDeviceStatus);
      AtolWebRequestsDriver.GetFnStatus getFnStatus = new AtolWebRequestsDriver.GetFnStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) getFnStatus);
      KkmStatus shortStatus = new KkmStatus();
      KkmStatus kkmStatus = shortStatus;
      SessionStatuses sessionStatuses;
      switch (command.Result.Status.Status)
      {
        case ShiftStatusEnum.Closed:
          sessionStatuses = SessionStatuses.Close;
          break;
        case ShiftStatusEnum.Opened:
          sessionStatuses = SessionStatuses.Open;
          break;
        case ShiftStatusEnum.Expired:
          sessionStatuses = SessionStatuses.OpenMore24Hours;
          break;
        default:
          sessionStatuses = SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      shortStatus.KkmState = this.GetKkmState(getDeviceStatus, getFnStatus);
      return shortStatus;
    }

    public KkmStatus GetStatus()
    {
      AtolWebRequestsDriver.GetDeviceInfo command1 = new AtolWebRequestsDriver.GetDeviceInfo();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command1);
      AtolWebRequestsDriver.GetDeviceStatus getDeviceStatus = new AtolWebRequestsDriver.GetDeviceStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) getDeviceStatus);
      AtolWebRequestsDriver.GetShiftStatus command2 = new AtolWebRequestsDriver.GetShiftStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command2);
      AtolWebRequestsDriver.GetFnStatus getFnStatus = new AtolWebRequestsDriver.GetFnStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) getFnStatus);
      AtolWebRequestsDriver.GetOfdStatus command3 = new AtolWebRequestsDriver.GetOfdStatus();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command3);
      AtolWebRequestsDriver.GetFnInfo command4 = new AtolWebRequestsDriver.GetFnInfo();
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) command4);
      KkmStatus status = new KkmStatus();
      status.Model = command1.Result.Info.ModelName;
      status.FactoryNumber = command1.Result.Info.Serial;
      status.SoftwareVersion = command1.Result.Info.FirmwareVersion;
      status.KkmState = this.GetKkmState(getDeviceStatus, getFnStatus);
      KkmStatus kkmStatus = status;
      SessionStatuses sessionStatuses;
      switch (command2.Result.Status.Status)
      {
        case ShiftStatusEnum.Closed:
          sessionStatuses = SessionStatuses.Close;
          break;
        case ShiftStatusEnum.Opened:
          sessionStatuses = SessionStatuses.Open;
          break;
        case ShiftStatusEnum.Expired:
          sessionStatuses = SessionStatuses.OpenMore24Hours;
          break;
        default:
          sessionStatuses = SessionStatuses.Unknown;
          break;
      }
      kkmStatus.SessionStatus = sessionStatuses;
      status.SessionNumber = command2.Result.Status.Number;
      status.SessionStarted = command2.Result.Status.Status == ShiftStatusEnum.Closed ? new DateTime?() : new DateTime?(command2.Result.Status.ExpiredTime.AddHours(-24.0));
      status.CheckNumber = getFnStatus.Result.Status.FiscalDocumentNumber;
      status.OfdNotSendDocuments = command3.Result.Status.NotSentCount;
      status.OfdLastSendDateTime = command3.Result.Status.NotSentFirstDocDateTime == this._defaultDateTime ? new DateTime?() : new DateTime?(command3.Result.Status.NotSentFirstDocDateTime);
      status.FnDateEnd = command4.Result.Info.ValidityDate;
      status.CheckStatus = CheckStatuses.Close;
      return status;
    }

    private KkmStatuses GetKkmState(
      AtolWebRequestsDriver.GetDeviceStatus deviceStatus,
      AtolWebRequestsDriver.GetFnStatus fnStatus)
    {
      if (deviceStatus.Result.Status.CoverOpened)
        return KkmStatuses.CoverOpen;
      if (!deviceStatus.Result.Status.PaperPresent)
        return KkmStatuses.NoPaper;
      if (fnStatus.Result.Status.Warnings.OfdTimeout)
        return KkmStatuses.OfdDocumentsToMany;
      return deviceStatus.Result.Status.Blocked || !deviceStatus.Result.Status.FnPresent || fnStatus.Result.Status.Warnings.CriticalError || fnStatus.Result.Status.Warnings.MemoryOverflow || fnStatus.Result.Status.Warnings.NeedReplacement ? KkmStatuses.HardwareError : KkmStatuses.Ready;
    }

    public bool OpenCashDrawer()
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.OpenCashDrawer());
      return true;
    }

    public bool SendDigitalCheck(string adress)
    {
      if (this._document.Client == null)
        this._document.Client = new AtolWebRequestsDriver.Client()
        {
          EmailOrPhone = adress
        };
      else
        this._document.Client.EmailOrPhone = adress;
      if (this._deviceConfig.CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
        this._document.Electronically = true;
      return true;
    }

    public void FeedPaper(int lines)
    {
      List<AtolWebRequestsDriver.PrintNonFiscal.Text> collection = new List<AtolWebRequestsDriver.PrintNonFiscal.Text>();
      for (; lines != 0; --lines)
        collection.Add(new AtolWebRequestsDriver.PrintNonFiscal.Text());
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.PrintNonFiscal()
      {
        Items = new List<AtolWebRequestsDriver.Item>((IEnumerable<AtolWebRequestsDriver.Item>) collection)
      });
    }

    public bool EndPrintOldCheck()
    {
      this.CurrentDriver.DoCommand((AtolWebRequestsDriver.AtolRequest) new AtolWebRequestsDriver.ContinuePrint());
      return true;
    }

    private void GetSalesNotice(string info)
    {
      if (info.IsNullOrEmpty() || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        return;
      this._industryInfo = new AtolWebRequestsDriver.IndustryInfo()
      {
        Date = "2023.11.21",
        Number = "1944",
        Fois = "030",
        IndustryAttribute = info
      };
    }
  }
}
