// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.VikiPrintWeb
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class VikiPrintWeb : IFiscalKkm, IDevice
  {
    private VikiPrintRepository _driver;
    private VikiPrintRepository.CreateDocumentCommand _document;
    private Gbs.Core.Config.Devices _devicesConfig;
    private SalePoints.SalePoint _salePoint;
    private VikiPrintRepository.CreateDocumentCommand.IndustryInfo _industryInfo;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "ВикиПринт (WEB)";

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => false;

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(this._devicesConfig.CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = false,
        Type = GlobalDictionaries.Devices.ConnectionTypes.Lan
      });
    }

    private void CheckError(VikiPrintRepository.VikiAnswer answer)
    {
      if (answer.Error != 0)
        throw new DeviceException(answer.Message + string.Format(" (код ошибки {0})", (object) answer.Error));
    }

    private void ActionWithSession(
      VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType type,
      Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      VikiPrintRepository.ActionWithShiftCommand command = new VikiPrintRepository.ActionWithShiftCommand()
      {
        ActionShift = type,
        Cashier = new VikiPrintRepository.Cashier()
        {
          Name = cashier.Name,
          Inn = cashier.Inn.IsNullOrEmpty() ? (string) null : cashier.Inn
        },
        SalePointAddress = this._salePoint.Organization.Address,
        SalePointName = this._salePoint.Description.NamePoint
      };
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError(command.Answer);
    }

    private void ActionWithOperation(
      VikiPrintRepository.CashOperationCommand.OperationTypes type,
      Gbs.Core.Devices.CheckPrinters.Cashier cashier,
      Decimal amount)
    {
      VikiPrintRepository.CashOperationCommand operationCommand1 = new VikiPrintRepository.CashOperationCommand();
      operationCommand1.OperationType = type;
      operationCommand1.Cashier = new VikiPrintRepository.Cashier()
      {
        Name = cashier.Name,
        Inn = cashier.Inn.IsNullOrEmpty() ? (string) null : cashier.Inn
      };
      operationCommand1.SalePointAddress = this._salePoint.Organization.Address;
      operationCommand1.Amount = amount;
      VikiPrintRepository.CashOperationCommand operationCommand2 = operationCommand1;
      SnoTypes? nullable;
      switch (this._devicesConfig.CheckPrinter.FiscalKkm.DefaultRuTaxSystem)
      {
        case GlobalDictionaries.RuTaxSystems.None:
          nullable = new SnoTypes?();
          break;
        case GlobalDictionaries.RuTaxSystems.Osn:
          nullable = new SnoTypes?(SnoTypes.Osn);
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohod:
          nullable = new SnoTypes?(SnoTypes.SimpleUsn);
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
          nullable = new SnoTypes?(SnoTypes.Usn);
          break;
        case GlobalDictionaries.RuTaxSystems.Envd:
          throw new ArgumentOutOfRangeException("Указана устаревшая СНО, укажите корректную в настройках программы.");
        case GlobalDictionaries.RuTaxSystems.Esn:
          nullable = new SnoTypes?(SnoTypes.Esn);
          break;
        case GlobalDictionaries.RuTaxSystems.Psn:
          nullable = new SnoTypes?(SnoTypes.Patent);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      operationCommand2.Sno = nullable;
      VikiPrintRepository.CashOperationCommand command = operationCommand1;
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError(command.Answer);
    }

    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 120, ВикиПринт WEB");
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
      {
        checkGood.MarkedInfo.ValidationResultKkm = (object) 0;
        string fnC1 = DataMatrixHelper.ReplaceSomeCharsToFNC1(checkGood.MarkedInfo.FullCode);
        RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
        LogHelper.Debug("Валидация кода: " + fnC1);
        VikiPrintRepository.CheckCodeMarkCommand checkCodeMarkCommand1 = new VikiPrintRepository.CheckCodeMarkCommand();
        checkCodeMarkCommand1.CodeMark = fnC1;
        VikiPrintRepository.CheckCodeMarkCommand checkCodeMarkCommand2 = checkCodeMarkCommand1;
        DocumentTypes documentTypes;
        switch (checkData.CheckType)
        {
          case CheckTypes.Sale:
            documentTypes = DocumentTypes.Sale;
            break;
          case CheckTypes.ReturnSale:
            documentTypes = DocumentTypes.Refund;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        checkCodeMarkCommand2.DocumentType = documentTypes;
        checkCodeMarkCommand1.Quantity = checkGood.Quantity;
        checkCodeMarkCommand1.Unit = (UnitTypes) checkGood.Unit.RuFfdUnitsIndex;
        VikiPrintRepository.CheckCodeMarkCommand command1 = checkCodeMarkCommand1;
        this._driver.DoCommand((VikiPrintRepository.VikiCommand) command1);
        this.CheckError((VikiPrintRepository.VikiAnswer) command1.Answer);
        LogHelper.Debug("Validation result code: " + command1.Answer.Result.ToJsonString(true));
        VikiPrintRepository.AcceptCodeMarkCommand command2 = new VikiPrintRepository.AcceptCodeMarkCommand();
        this._driver.DoCommand((VikiPrintRepository.VikiCommand) command2);
        this.CheckError(command2.Answer);
      }
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.ActionWithSession(VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType.OpenShift, cashier);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          this.ActionWithSession(VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType.CloseShift, cashier);
          break;
        case ReportTypes.XReport:
          VikiPrintRepository.XReportCommand command = new VikiPrintRepository.XReportCommand()
          {
            Cashier = new VikiPrintRepository.Cashier()
            {
              Name = cashier.Name,
              Inn = cashier.Inn.IsNullOrEmpty() ? (string) null : cashier.Inn
            }
          };
          this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
          this.CheckError(command.Answer);
          break;
        case ReportTypes.XReportWithGoods:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      if (this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.Ffd120CodeValidation(checkData);
      VikiPrintRepository.CreateDocumentCommand createDocumentCommand1 = new VikiPrintRepository.CreateDocumentCommand();
      createDocumentCommand1.Cashier = new VikiPrintRepository.Cashier()
      {
        Name = checkData.Cashier.Name,
        Inn = checkData.Cashier.Inn.IsNullOrEmpty() ? (string) null : checkData.Cashier.Inn
      };
      createDocumentCommand1.SalePointAddress = this._salePoint.Organization.Address;
      VikiPrintRepository.CreateDocumentCommand createDocumentCommand2 = createDocumentCommand1;
      SnoTypes? nullable;
      switch (checkData.RuTaxSystem)
      {
        case GlobalDictionaries.RuTaxSystems.None:
          nullable = new SnoTypes?();
          break;
        case GlobalDictionaries.RuTaxSystems.Osn:
          nullable = new SnoTypes?(SnoTypes.Osn);
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohod:
          nullable = new SnoTypes?(SnoTypes.SimpleUsn);
          break;
        case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
          nullable = new SnoTypes?(SnoTypes.Usn);
          break;
        case GlobalDictionaries.RuTaxSystems.Envd:
          throw new ArgumentOutOfRangeException("Указана устаревшая СНО, укажите корректную в настройках программы.");
        case GlobalDictionaries.RuTaxSystems.Esn:
          nullable = new SnoTypes?(SnoTypes.Esn);
          break;
        case GlobalDictionaries.RuTaxSystems.Psn:
          nullable = new SnoTypes?(SnoTypes.Patent);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      createDocumentCommand2.Sno = nullable;
      VikiPrintRepository.CreateDocumentCommand createDocumentCommand3 = createDocumentCommand1;
      DocumentTypes documentTypes;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          documentTypes = DocumentTypes.Sale;
          break;
        case CheckTypes.ReturnSale:
          documentTypes = DocumentTypes.Refund;
          break;
        case CheckTypes.Buy:
          documentTypes = DocumentTypes.Outflow;
          break;
        case CheckTypes.ReturnBuy:
          documentTypes = DocumentTypes.OutflowRefund;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      createDocumentCommand3.DocumentType = documentTypes;
      this._document = createDocumentCommand1;
      if (this._devicesConfig.CheckPrinter.FiscalKkm.SendBuyerInfoToCheck && checkData.Client != null)
      {
        VikiPrintRepository.CreateDocumentCommand document = this._document;
        VikiPrintRepository.Buyer buyer = new VikiPrintRepository.Buyer();
        buyer.Name = checkData.Client.Client.Name;
        DateTime? birthday = checkData.Client.Client.Birthday;
        ref DateTime? local = ref birthday;
        buyer.DateOfBirth = (local.HasValue ? local.GetValueOrDefault().ToString("dd.MM.yyyy") : (string) null) ?? "";
        buyer.Inn = checkData.Client.Client.GetInn();
        document.Buyer = buyer;
      }
      this.GetSalesNotice(checkData.TrueApiInfoForKkm);
      return true;
    }

    private void CalculateCreditForPositions()
    {
      Decimal sum = this._document.Payments.Where<VikiPrintRepository.CreateDocumentCommand.PaymentItem>((Func<VikiPrintRepository.CreateDocumentCommand.PaymentItem, bool>) (x => x.Type == VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Credit)).Sum<VikiPrintRepository.CreateDocumentCommand.PaymentItem>((Func<VikiPrintRepository.CreateDocumentCommand.PaymentItem, Decimal>) (x => x.Sum));
      List<CheckFactory.AdjustableItem> list = this._document.Positions.Select<VikiPrintRepository.CreateDocumentCommand.PositionItem, CheckFactory.AdjustableItem>((Func<VikiPrintRepository.CreateDocumentCommand.PositionItem, CheckFactory.AdjustableItem>) (x => new CheckFactory.AdjustableItem((object) x, x.Price * x.Quantity))).ToList<CheckFactory.AdjustableItem>();
      CheckFactory.AdjustSumForPositions(list, sum);
      foreach (CheckFactory.AdjustableItem adjustableItem in list)
        ((VikiPrintRepository.CreateDocumentCommand.PositionItem) adjustableItem.Object).CreditSum = adjustableItem.Sum;
    }

    public bool CloseCheck()
    {
      this.CalculateCreditForPositions();
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) this._document);
      this.CheckError(this._document.Answer);
      return true;
    }

    public void CancelCheck()
    {
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.ActionWithOperation(VikiPrintRepository.CashOperationCommand.OperationTypes.Reserve, cashier, sum);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.ActionWithOperation(VikiPrintRepository.CashOperationCommand.OperationTypes.Insertion, cashier, sum);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      VikiPrintRepository.SumInfoKkmCommand command = new VikiPrintRepository.SumInfoKkmCommand();
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError((VikiPrintRepository.VikiAnswer) command.Answer);
      sum = command.Answer.AmountOfCashInDrawer;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      List<VikiPrintRepository.CreateDocumentCommand.PositionItem> positions = this._document.Positions;
      VikiPrintRepository.CreateDocumentCommand.PositionItem positionItem1 = new VikiPrintRepository.CreateDocumentCommand.PositionItem();
      positionItem1.Name = good.Name;
      positionItem1.Quantity = good.Quantity;
      positionItem1.Price = good.Price;
      VikiPrintRepository.CreateDocumentCommand.PositionItem positionItem2 = positionItem1;
      NdsTypes ndsTypes;
      switch (good.TaxRateNumber)
      {
        case 1:
          ndsTypes = NdsTypes.NdsZero;
          break;
        case 2:
          ndsTypes = NdsTypes.NdsNo;
          break;
        case 3:
          ndsTypes = NdsTypes.Nds10;
          break;
        case 4:
          ndsTypes = NdsTypes.Nds20;
          break;
        case 5:
          ndsTypes = NdsTypes.Nds110;
          break;
        case 6:
          ndsTypes = NdsTypes.Nds120;
          break;
        case 7:
          ndsTypes = NdsTypes.Nds5;
          break;
        case 8:
          ndsTypes = NdsTypes.Nds7;
          break;
        case 9:
          ndsTypes = NdsTypes.Nds105;
          break;
        case 10:
          ndsTypes = NdsTypes.Nds107;
          break;
        default:
          throw new ArgumentOutOfRangeException("Указаны некорректные индексы НДС в настройках программы.");
      }
      positionItem2.Nds = ndsTypes;
      positionItem1.Discount = new VikiPrintRepository.CreateDocumentCommand.DiscountItem()
      {
        Type = DiscountTypes.Discount,
        Sum = good.DiscountSum
      };
      positionItem1.Unit = (UnitTypes) good.Unit.RuFfdUnitsIndex;
      VikiPrintRepository.CreateDocumentCommand.PositionItem positionItem3 = positionItem1;
      MarkedInfo markedInfo = good.MarkedInfo;
      string str = (markedInfo != null ? (markedInfo.Type == GlobalDictionaries.RuMarkedProductionTypes.None ? 1 : 0) : 0) != 0 ? "" : DataMatrixHelper.ReplaceSomeCharsToFNC1(good?.MarkedInfo?.FullCode) ?? "";
      positionItem3.CodeMark = str;
      VikiPrintRepository.CreateDocumentCommand.PositionItem positionItem4 = positionItem1;
      CalculationTypes calculationTypes1;
      switch (good.RuFfdPaymentModeCode)
      {
        case GlobalDictionaries.RuFfdPaymentModes.None:
          calculationTypes1 = CalculationTypes.FullPayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PrePaymentFull:
          calculationTypes1 = CalculationTypes.FullPrepaid;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.Prepayment:
          calculationTypes1 = CalculationTypes.Prepaid;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.AdvancePayment:
          calculationTypes1 = CalculationTypes.Advance;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.FullPayment:
          calculationTypes1 = CalculationTypes.FullPayment;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PartPaymentAndCredit:
          calculationTypes1 = CalculationTypes.Credit;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.FullCredit:
          calculationTypes1 = CalculationTypes.FullCredit;
          break;
        case GlobalDictionaries.RuFfdPaymentModes.PaymentForCredit:
          calculationTypes1 = CalculationTypes.PaymentCredit;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      positionItem4.CalculationType = calculationTypes1;
      VikiPrintRepository.CreateDocumentCommand.PositionItem positionItem5 = positionItem1;
      GoodCalculationTypes calculationTypes2;
      switch (good.RuFfdGoodTypeCode)
      {
        case GlobalDictionaries.RuFfdGoodsTypes.None:
          calculationTypes2 = GoodCalculationTypes.Goods;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.SimpleGood:
          calculationTypes2 = GoodCalculationTypes.Goods;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.ExcisableGood:
          calculationTypes2 = GoodCalculationTypes.Excisable;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Work:
          calculationTypes2 = GoodCalculationTypes.Job;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Service:
          calculationTypes2 = GoodCalculationTypes.Service;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.GamePayment:
          calculationTypes2 = GoodCalculationTypes.GamblingRate;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.GameWin:
          calculationTypes2 = GoodCalculationTypes.GamblingWinnings;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.LotteryPayment:
          calculationTypes2 = GoodCalculationTypes.LotteryTicket;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.LotteryWin:
          calculationTypes2 = GoodCalculationTypes.LotteryWinnings;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Rid:
          calculationTypes2 = GoodCalculationTypes.Rid;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Payment:
          calculationTypes2 = GoodCalculationTypes.Payment;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.AgentPayment:
          calculationTypes2 = GoodCalculationTypes.AgencyRewards;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.WithPayment:
          calculationTypes2 = GoodCalculationTypes.Payment;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Other:
          calculationTypes2 = GoodCalculationTypes.Another;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.PropertyLaw:
          calculationTypes2 = GoodCalculationTypes.PropertyLaw;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.NonOperatingIncome:
          calculationTypes2 = GoodCalculationTypes.UnrealIncome;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.OtherPayment:
          calculationTypes2 = GoodCalculationTypes.Payment;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.TradeFee:
          calculationTypes2 = GoodCalculationTypes.TradeFee;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.ResortFee:
          calculationTypes2 = GoodCalculationTypes.ResortFee;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Deposit:
          calculationTypes2 = GoodCalculationTypes.Deposit;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Expenditure:
          calculationTypes2 = GoodCalculationTypes.Excisable;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.PensionInsuranceIP:
          calculationTypes2 = GoodCalculationTypes.PensionInsuranceNoIndividual;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.PensionInsurance:
          calculationTypes2 = GoodCalculationTypes.PensionInsuranceIndividual;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.MedicalInsuranceIP:
          calculationTypes2 = GoodCalculationTypes.HealthInsuranceNoIndividual;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.MedicalInsurance:
          calculationTypes2 = GoodCalculationTypes.HealthInsuranceIndividual;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.SocialInsurance:
          calculationTypes2 = GoodCalculationTypes.SocialInsurance;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.CasinoPayment:
          calculationTypes2 = GoodCalculationTypes.Casino;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.OutOfFunds:
          calculationTypes2 = GoodCalculationTypes.InsuranceContr;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Atnm:
          calculationTypes2 = GoodCalculationTypes.ExcisableNoCodeMark;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Atm:
          calculationTypes2 = GoodCalculationTypes.ExcisableCodeMark;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Tnm:
          calculationTypes2 = GoodCalculationTypes.GoodsNoCodeMark;
          break;
        case GlobalDictionaries.RuFfdGoodsTypes.Tm:
          calculationTypes2 = GoodCalculationTypes.GoodsCodeMark;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      positionItem5.GoodCalculationType = calculationTypes2;
      positionItem1.IndustryInfo = this._industryInfo;
      positions.Add(positionItem1);
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      List<VikiPrintRepository.CreateDocumentCommand.PaymentItem> payments = this._document.Payments;
      VikiPrintRepository.CreateDocumentCommand.PaymentItem paymentItem1 = new VikiPrintRepository.CreateDocumentCommand.PaymentItem();
      paymentItem1.Description = payment.Name;
      paymentItem1.Sum = payment.Sum;
      VikiPrintRepository.CreateDocumentCommand.PaymentItem paymentItem2 = paymentItem1;
      VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes paymentTypes;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Cash;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Bank;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Bank;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Prepaid;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Credit;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Prepaid;
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          paymentTypes = VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes.Bank;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      paymentItem2.Type = paymentTypes;
      payments.Add(paymentItem1);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      if (this._document.Discount == null)
        this._document.Discount = new VikiPrintRepository.CreateDocumentCommand.DiscountItem()
        {
          Type = DiscountTypes.Discount,
          Sum = sum,
          Description = description
        };
      else
        this._document.Discount.Sum += sum;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      this._devicesConfig = devicesConfig ?? new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (onlyDriverLoad)
        return;
      this._driver = new VikiPrintRepository(this._devicesConfig.CheckPrinter.Connection.LanPort, this._devicesConfig.CheckPrinter.FiscalKkm.Model);
      this._salePoint = SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
      if (!this._devicesConfig.CheckPrinter.FiscalKkm.Model.IsNullOrEmpty())
        return;
      VikiPrintRepository.GetPrintersCommand command = new VikiPrintRepository.GetPrintersCommand();
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError((VikiPrintRepository.VikiAnswer) command.Answer);
      if (!command.Answer.Printers.Any<VikiPrintRepository.GetPrintersCommand.PrintersAnswer.Printer>())
        throw new DeviceException(Translate.VikiPrintWeb_Connect_Необходимо_настроить_связь_с_ККМ_в_VikiDriver_и_после_повторить_попытку_печати_чека_);
      if (command.Answer.Printers.Count > 1)
        throw new DeviceException(Translate.VikiPrintWeb_Connect_Необходимо_указать_настройках_программы_заводской_номер_кассы__которая_используется__Затем_повторите_попытку_печати_чека_);
      this._devicesConfig.CheckPrinter.FiscalKkm.Model = command.Answer.Printers.Single<VikiPrintRepository.GetPrintersCommand.PrintersAnswer.Printer>().PlantNumber;
      new ConfigsRepository<Gbs.Core.Config.Devices>().Save(this._devicesConfig);
      this._driver = new VikiPrintRepository(this._devicesConfig.CheckPrinter.Connection.LanPort, this._devicesConfig.CheckPrinter.FiscalKkm.Model);
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      VikiPrintRepository.PrintTextCommand command = new VikiPrintRepository.PrintTextCommand()
      {
        Text = string.Join("\n", nonFiscalStrings.Select<NonFiscalString, string>((Func<NonFiscalString, string>) (x => x.Text)))
      };
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError(command.Answer);
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      VikiPrintRepository.PrintBarcodeCommand command = new VikiPrintRepository.PrintBarcodeCommand();
      switch (type)
      {
        case BarcodeTypes.None:
          return true;
        case BarcodeTypes.Ean13:
          command.BarcodeType = BarcodeType.Ean13;
          command.Height = 2;
          command.Width = 100;
          break;
        case BarcodeTypes.QrCode:
          command.BarcodeType = BarcodeType.QrCode;
          command.Width = 8;
          command.Height = 254;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
      }
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError(command.Answer);
      return true;
    }

    public bool CutPaper() => throw new NotImplementedException();

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      VikiPrintRepository.InfoKkmCommand command = new VikiPrintRepository.InfoKkmCommand();
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError((VikiPrintRepository.VikiAnswer) command.Answer);
      KkmStatus status = new KkmStatus();
      status.FactoryNumber = command.Answer.FactoryNumber;
      status.SessionStatus = command.Answer.PrinterAnswer.CurrentStateKkm.IsShiftMore24 ? SessionStatuses.OpenMore24Hours : (command.Answer.PrinterAnswer.CurrentStateKkm.IsShiftOpened ? SessionStatuses.Open : SessionStatuses.Close);
      KkmStatus kkmStatus = status;
      CheckStatuses checkStatuses;
      switch (command.Answer.PrinterAnswer.OpenDocumentState)
      {
        case 0:
          checkStatuses = CheckStatuses.Close;
          break;
        case 2:
          checkStatuses = CheckStatuses.Open;
          break;
        case 3:
          checkStatuses = CheckStatuses.Open;
          break;
        case 4:
          checkStatuses = CheckStatuses.Open;
          break;
        case 8:
          checkStatuses = CheckStatuses.Open;
          break;
        default:
          checkStatuses = CheckStatuses.Unknown;
          break;
      }
      kkmStatus.CheckStatus = checkStatuses;
      int result1;
      status.SessionNumber = int.TryParse(command.Answer.CurrentShiftNum, out result1) ? result1 : 0;
      DateTime result2;
      status.SessionStarted = new DateTime?(DateTime.TryParse(command.Answer.DateTimeOfOpenShift, out result2) ? result2 : DateTime.MinValue);
      status.OfdNotSendDocuments = command.Answer.Ofd.CountNotSentDocument;
      DateTime result3;
      status.OfdLastSendDateTime = new DateTime?(DateTime.TryParseExact(command.Answer.Ofd.DateFirstDocNotSent, "ddMMyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result3) ? result3.Date : DateTime.MinValue);
      status.CheckNumber = command.Answer.PrinterAnswer.OpenDocumentState == 0 ? command.Answer.NextReceiptNum : command.Answer.NextReceiptNum - 1;
      DateTime result4;
      status.FnDateEnd = DateTime.TryParse(command.Answer.DateTimeOfFnFinished, out result4) ? result4 : DateTime.MinValue;
      status.SoftwareVersion = command.Answer.PrinterFirmwareVersion;
      status.KkmState = KkmStatuses.Ready;
      return status;
    }

    public bool OpenCashDrawer()
    {
      VikiPrintRepository.OpenDrawerCommand command = new VikiPrintRepository.OpenDrawerCommand();
      this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
      this.CheckError(command.Answer);
      return true;
    }

    public bool SendDigitalCheck(string adress)
    {
      this._document.PhoneOrEmailForClient = adress;
      return true;
    }

    public void FeedPaper(int lines)
    {
      VikiPrintRepository.FeedLineCommand command = new VikiPrintRepository.FeedLineCommand();
      for (; lines != 0; --lines)
      {
        this._driver.DoCommand((VikiPrintRepository.VikiCommand) command);
        this.CheckError(command.Answer);
      }
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    private void GetSalesNotice(string info)
    {
      if (info.IsNullOrEmpty() || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        return;
      this._industryInfo = new VikiPrintRepository.CreateDocumentCommand.IndustryInfo()
      {
        Value = info,
        Identifier = "030",
        Date = "21112023",
        Number = "1944"
      };
    }
  }
}
