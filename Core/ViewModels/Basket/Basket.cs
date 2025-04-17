// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Basket.Basket
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Devices;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.DisplayBuyers;
using Gbs.Core.Devices.DisplayNumbers;
using Gbs.Core.Devices.Sbp;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Emails;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Core.Models.Basket;
using Gbs.Core.ViewModels.Documents;
using Gbs.Core.ViewModels.Documents.Sales;
using Gbs.Forms._shared;
using Gbs.Forms.ClientOrder;
using Gbs.Forms.Goods;
using Gbs.Forms.Sale;
using Gbs.Helpers;
using Gbs.Helpers.BackgroundTasks;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Factories;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Core.ViewModels.Basket
{
  public class Basket : DocumentViewModel<BasketItem>
  {
    protected bool _isAcsessSms;
    protected bool _isPrintCheckLocal;
    protected Gbs.Core.Config.Settings _settings;
    protected Gbs.Core.Config.Devices _devices;
    protected Decimal _sumForAcquiring;
    protected Decimal _sumForSbp;
    private List<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate> _certificatesForPay = new List<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>();
    private ClientAdnSum _client;
    private bool _isCheckedClient;
    private bool _isEnablePrintCheck;
    private bool _isPrintCheck;
    private Decimal? _receive = new Decimal?(0M);
    private string _saleNumber;
    private bool _isCollectionChanged;
    private CancellationTokenSource _CTS = new CancellationTokenSource();
    public bool IsDeletedPercentForServiceGood;
    private System.Timers.Timer _recalcTimer = new System.Timers.Timer()
    {
      Interval = 100.0,
      AutoReset = false
    };

    private void recalcTotalAsync()
    {
      Performancer performancer = new Performancer("Пересчет корзины");
      this.OnPropertyChanged("TotalSum");
      LogHelper.Debug("Пересчет итогов в корзине");
      this.RoundQuantity();
      this.RemoveZeroQuantityItems();
      this.BasketExtraPricer?.ReCalcPrices();
      this.BasketDiscounter?.SetAllDiscount();
      performancer.AddPoint("point 10");
      try
      {
        CheckFactory checkFactory = new CheckFactory(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter);
        performancer.AddPoint("point 14");
        PreparedCheckData checkFromBasket = checkFactory.CreateCheckFromBasket(this);
        performancer.AddPoint("point 15");
        Decimal num = checkFromBasket.CheckData.GetTotalGoodsSum() - checkFromBasket.CheckData.DiscountSum;
        this.KkmCheckCorrection = this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.TotalSum)) - num;
        if (checkFromBasket.CheckData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)) == 0M)
          this.KkmCheckCorrection -= this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка пересчета чека");
      }
      performancer.AddPoint("point 20");
      this.UpdateProperties();
      if (!this.Items.Any<BasketItem>())
      {
        this.CheckOrder();
        this.Storage = (Storages.Storage) null;
      }
      performancer.AddPoint("point 30");
      MainWindowViewModel.MonitorViewModel?.SetVisibilityImage();
      performancer.AddPoint("point 40");
      this.SendDisplayBuyerNumbersInfo(this.TotalSum);
      performancer.Stop();
    }

    public void CheckPriceForTobacco()
    {
      foreach (BasketItem basketItem in (Collection<BasketItem>) this.Items)
      {
        basketItem.ErrorStrForPrice = (string) null;
        this.SetItemPriceColor(basketItem);
      }
      if (!new ConfigsRepository<Integrations>().Get().Crpt.IsCheckTobaccoPriceForMark)
        return;
      foreach (BasketItem basketItem in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.TobaccoSalePrice > 0M)))
      {
        Decimal num = basketItem.TobaccoSalePrice / basketItem.Quantity;
        if (basketItem.TotalSum != basketItem.TobaccoSalePrice)
        {
          Decimal basePrice = basketItem.BasePrice;
          basketItem.BasePrice = num;
          basketItem.SalePrice = num;
          basketItem.Discount = new SaleDiscount()
          {
            MaxValue = 0M
          };
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format("Для товара '{0}' установлена цена согласно МРЦ в марке ({1:N2} -> {2:N2}), которая была отсканирована.", (object) basketItem.DisplayedName, (object) basePrice, (object) num)));
          basketItem.ErrorStrForPrice = string.Format("Установлена цена согласно МРЦ в марке ({0:N2}), которая была отсканирована.", (object) num);
          this.SetItemPriceColor(basketItem, Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange);
        }
      }
      this.ReCalcTotals();
    }

    private void PrepareRotationCheck()
    {
      int fiscalCheckInterval = this._settings.BasicConfig.FiscalCheckInterval;
      int fiscalCheckCount = this._settings.BasicConfig.NoFiscalCheckCount;
      this._isPrintCheckLocal = fiscalCheckInterval == 0 || fiscalCheckCount == fiscalCheckInterval;
    }

    protected void CommitRotationCheck()
    {
      int fiscalCheckInterval = this._settings.BasicConfig.FiscalCheckInterval;
      int fiscalCheckCount = this._settings.BasicConfig.NoFiscalCheckCount;
      if (fiscalCheckInterval == 0)
        return;
      int num = fiscalCheckCount + 1;
      if (num > fiscalCheckInterval)
        num = 0;
      this._settings.BasicConfig.NoFiscalCheckCount = num;
      new ConfigsRepository<Gbs.Core.Config.Settings>().Save(this._settings);
    }

    protected ActionResult CheckStatusKkm(bool isPrintCheck)
    {
      if (this._devices.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || this._devices.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None || !isPrintCheck || !this._isPrintCheckLocal)
        return new ActionResult(ActionResult.Results.Ok);
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.Basket_Save_Проверка_соединения_с_ККМ);
      string str = "";
      Exception ex1 = (Exception) null;
      try
      {
        using (KkmHelper kkmHelper1 = new KkmHelper(this._devices))
        {
          KkmHelper kkmHelper2 = kkmHelper1;
          Cashier cashier = new Cashier();
          Gbs.Core.Entities.Users.User user = this.User;
          // ISSUE: explicit non-virtual call
          cashier.UserUid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
          cashier.Name = this.User?.Client?.Name ?? string.Empty;
          cashier.Inn = this.User?.Client?.GetInn() ?? string.Empty;
          kkmHelper2.CheckKkmReady(cashier);
          progressBar.Close();
        }
      }
      catch (DeviceException ex2)
      {
        ex1 = (Exception) ex2;
        str = ex2.Message;
      }
      catch (Exception ex3)
      {
        ex1 = ex3;
        str = ex3.Message;
      }
      progressBar.Close();
      if (ex1 == null)
        return new ActionResult(ActionResult.Results.Ok);
      LogHelper.WriteError(ex1, "Ошибка при проверки связи с ККМ");
      if (MessageBoxHelper.Show(Translate.Basket_Save_ + str + Translate.Basket_Save_Продолжить_Без_Печати_чека, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) != MessageBoxResult.Yes)
        return new ActionResult(ActionResult.Results.Cancel);
      this.IsPrintCheck = false;
      this._isPrintCheckLocal = false;
      return new ActionResult(ActionResult.Results.Ok);
    }

    private void InsertTotalForDisplayPayer()
    {
      if (this._devices.DisplayPayer.Type == DisplayBuyerTypes.None)
        return;
      List<string> lines = new List<string>();
      if (this.TotalDiscount > 0M)
        lines.Add(DisplayBuyerHelper.GetTextAndSum(Translate.Basket_Save_СКИДКА, this.TotalDiscount));
      lines.Add(DisplayBuyerHelper.GetTextAndSum(Translate.FrmInsertPaymentMethods_ИТОГО, this.TotalSum));
      using (DisplayBuyerHelper displayBuyerHelper = new DisplayBuyerHelper((IConfig) new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
        displayBuyerHelper.WriteText(lines);
    }

    protected ActionResult PrintOrShowCheck(
      bool isPrintCheck,
      bool isShowForm,
      ActionPrintViewModel.TypePrint type)
    {
      if (isShowForm)
      {
        if (!new ActionPrintViewModel().Print(this, type))
          return new ActionResult(ActionResult.Results.Cancel);
      }
      else if (isPrintCheck && this._isPrintCheckLocal)
      {
        if (!this.TryPrintCheck(out MessageBoxResult _))
          return new ActionResult(ActionResult.Results.Cancel);
      }
      else if (!this._isPrintCheckLocal)
        LogHelper.Debug("Не печатаем чек, так как чередуем чеки для казани");
      else
        LogHelper.Debug("Не печатаем чек, так как не стоит галочка в главной форме, IsPrintCheck = " + this.IsPrintCheck.ToString());
      return new ActionResult(ActionResult.Results.Ok);
    }

    protected ActionResult GetPaymentsByDocument(bool isCafe)
    {
      Decimal delivery;
      if (!this.GetPayments(out delivery, isCafe))
        return new ActionResult(ActionResult.Results.Cancel);
      if (this.Payments.Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type == GlobalDictionaries.KkmPaymentMethods.Bonus)) && this.Client != null && this._settings.Clients.BonusesAuthType != GlobalDictionaries.ActionAuthType.None && !this._isAcsessSms)
      {
        if (!new SmsHelper().CheckingCode(this.Client.Client.Phone, this._settings.Clients.BonusesAuthType, Gbs.Core.Entities.Actions.DoUseBonusesIfOffSmsCode))
          return new ActionResult(ActionResult.Results.Cancel);
        this._isAcsessSms = true;
      }
      this.Delivery = delivery;
      if (this.Payments.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault())) < this.TotalSum)
      {
        ActionResult paymentsByDocument = this.DoCredit();
        if (paymentsByDocument.Result != ActionResult.Results.Ok)
          return paymentsByDocument;
      }
      if (this.Payments.Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type == GlobalDictionaries.KkmPaymentMethods.Card)))
        this._isPrintCheckLocal = true;
      this._sumForAcquiring = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
      {
        PaymentMethods.PaymentMethod method = p.Method;
        return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
      })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (p => p.Sum.GetValueOrDefault()));
      this._sumForSbp = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
      {
        PaymentMethods.PaymentMethod method = p.Method;
        return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
      })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (p => p.Sum.GetValueOrDefault()));
      return new ActionResult(ActionResult.Results.Ok);
    }

    public List<SelectPaymentMethods.PaymentGrid> Payments { get; set; } = new List<SelectPaymentMethods.PaymentGrid>();

    public List<Gbs.Core.Entities.Payments.Payment> PaymentsPrepaid { get; set; } = new List<Gbs.Core.Entities.Payments.Payment>();

    public ActionResult PrepareCheck()
    {
      this._isAcsessSms = false;
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        return new ActionResult(ActionResult.Results.Warning, Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_);
      this.ReCalcTotals();
      ActionResult actionResult = this.ValidateBasket();
      if (actionResult.Result != ActionResult.Results.Ok)
        return actionResult;
      if (!this.UserAuthorization())
        return new ActionResult(ActionResult.Results.Cancel);
      this._settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      this._devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.PrepareRotationCheck();
      this.InsertTotalForDisplayPayer();
      return new ActionResult(ActionResult.Results.Ok);
    }

    public override ActionResult Save()
    {
      try
      {
        ActionResult actionResult1 = this.PrepareCheck();
        if (actionResult1.Result != ActionResult.Results.Ok)
          return actionResult1;
        ActionResult actionResult2 = this.CheckStatusKkm(this.IsPrintCheck);
        if (actionResult2.Result != ActionResult.Results.Ok)
          return actionResult2;
        ActionResult paymentsByDocument = this.GetPaymentsByDocument(false);
        if (paymentsByDocument.Result != ActionResult.Results.Ok)
          return paymentsByDocument;
        this.SaleNumber = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.Sale);
        this.UpdatePaymentListForCertificate(this.Payments, this.CertificatesPaymentSum);
        this.Document = new DocumentsFactory().Create(this);
        if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsCommentSale)
        {
          (bool result, string output) tuple = MessageBoxHelper.Input("", Translate.Basket_Save_Укажите_комментарий_к_продаже);
          if (tuple.result)
          {
            this.Comment = tuple.output;
            this.Document.Comment = tuple.output;
          }
        }
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
          ActionResult actionResult3 = documentsRepository.Validate(this.Document);
          if (actionResult3.Result != ActionResult.Results.Ok)
          {
            int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult3.Messages));
            return new ActionResult(ActionResult.Results.Cancel);
          }
          bool isShowForm = this._devices.CheckPrinter.IsShowPrintConfirmationForm && this._devices.CheckPrinter.Type != 0;
          ActionPrintViewModel.TypePrint type = ActionPrintViewModel.TypePrint.NoCheck;
          if (isShowForm && this._isPrintCheckLocal)
          {
            (ActionPrintViewModel.TypePrint type, bool isPrint) typePrint = new ActionPrintViewModel().GetTypePrint(this);
            type = typePrint.type;
            if (!typePrint.isPrint)
              return new ActionResult(ActionResult.Results.Cancel);
          }
          new EgaisRepository().DoSaleStrongItem(this.Items.ToList<BasketItem>(), this.Document);
          Gbs.Core.Entities.Users.User user = this.User;
          // ISSUE: explicit non-virtual call
          KkmHelper.UserUid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
          if (!this.PayAcquiring(this._sumForAcquiring, this.Document))
          {
            ProgressBarHelper.Close();
            return new ActionResult(ActionResult.Results.Cancel);
          }
          if (!this.PaySBP(this._sumForSbp, this.Document))
          {
            ProgressBarHelper.Close();
            return new ActionResult(ActionResult.Results.Cancel);
          }
          ActionResult actionResult4 = this.PrintOrShowCheck(this.IsPrintCheck, isShowForm, type);
          if (actionResult4.Result != ActionResult.Results.Ok)
            return actionResult4;
          if (!this.TrueApiInfoForKkm.IsNullOrEmpty())
          {
            List<EntityProperties.PropertyValue> properties = this.Document.Properties;
            EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
            EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
            propertyType.Uid = GlobalDictionaries.InfoWithTrueApiUid;
            propertyValue.Type = propertyType;
            propertyValue.Value = (object) this.TrueApiInfoForKkm;
            propertyValue.EntityUid = this.Document.Uid;
            properties.Add(propertyValue);
          }
          ExtraPrinters.PrepareExtraPrint(this.Document);
          if (!documentsRepository.Save(this.Document))
            return new ActionResult(ActionResult.Results.Error, Translate.Basket_Не_удалось_сохранить_продажу_в_базу_данных);
          ExtraPrinters.Print(this.Document);
          new BonusHelper().UpdateSumBonusesForSale(this.Document);
          this.SaveCertificates();
          this.RemoteControlActions();
          this.CommitRotationCheck();
          this.SaveEgaisItems(this.Document);
          this.SaveTapBeerItems(this.Items.ToList<BasketItem>());
          return new ActionResult(ActionResult.Results.Ok);
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.WriteError(ex, "Ошибка сохранения корзины в продажу");
        return new ActionResult(ActionResult.Results.Error, Translate.Basket_Не_удалось_сохранить_корзину_в_продажу + Other.NewLine(2) + ex.Message);
      }
    }

    protected bool GetPayments(out Decimal delivery, bool isCafe = false)
    {
      if (DevelopersHelper.IsUnitTest())
      {
        delivery = 0M;
        return true;
      }
      Decimal num1 = this.IsUpdateReceive ? this.ReceiveSum.GetValueOrDefault() : this.TotalSum;
      Decimal num2 = this.PaymentsPrepaid.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      FrmInsertPaymentMethods insertPaymentMethods = new FrmInsertPaymentMethods();
      Decimal totalSum = this.TotalSum;
      Decimal receiveSum = num1;
      int num3 = !isCafe ? 1 : 0;
      Decimal certificatesPaymentSum = this.CertificatesPaymentSum;
      ClientAdnSum client = this.Client;
      Decimal totalBonusSum = client != null ? client.TotalBonusSum : 0M;
      Decimal sumPrepaid = num2;
      (bool Result, List<SelectPaymentMethods.PaymentGrid> paymentGridList, Decimal Delivery) = insertPaymentMethods.GetValuePayment(totalSum, receiveSum, num3 != 0, sumCertificate: certificatesPaymentSum, sumBonuses: totalBonusSum, sumPrepaid: sumPrepaid, basket: this);
      if (!Result)
      {
        this.Payments.RemoveAll((System.Predicate<SelectPaymentMethods.PaymentGrid>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Bonus));
        delivery = 0M;
        this.ReCalcTotals();
        return false;
      }
      delivery = Delivery;
      this.Payments = paymentGridList.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type != GlobalDictionaries.KkmPaymentMethods.PrePayment)).ToList<SelectPaymentMethods.PaymentGrid>();
      this.PaymentsPrepaid.ForEach((Action<Gbs.Core.Entities.Payments.Payment>) (x => x.Method.KkmMethod = GlobalDictionaries.KkmPaymentMethods.PrePayment));
      this.Payments.AddRange(this.PaymentsPrepaid.Select<Gbs.Core.Entities.Payments.Payment, SelectPaymentMethods.PaymentGrid>((Func<Gbs.Core.Entities.Payments.Payment, SelectPaymentMethods.PaymentGrid>) (x => new SelectPaymentMethods.PaymentGrid()
      {
        Comment = Translate.Basket_GetPayments_Предполата_по_заказу,
        Sum = new Decimal?(x.SumIn),
        Method = x.Method,
        Type = GlobalDictionaries.KkmPaymentMethods.PrePayment
      })));
      return true;
    }

    protected ActionResult DoCredit()
    {
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Certificate.IsCertificate)))
        return new ActionResult(ActionResult.Results.Error, Translate.Basket_);
      Decimal sum = this.TotalSum - this.Payments.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault()));
      (bool, Gbs.Core.Entities.Clients.Client, string) valueTuple = DevelopersHelper.IsUnitTest() ? (true, this.Client.Client, this.Comment) : new FrmMakingCredit().GetCredit(sum, this.Client?.Client);
      if (!valueTuple.Item1)
        return new ActionResult(ActionResult.Results.Cancel);
      this.BasketDiscounter.IsLoadDiscountRule = false;
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        this.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(valueTuple.Item2.Uid);
        this.IsCheckedClient = true;
        this.BasketDiscounter.IsLoadDiscountRule = true;
        if (this.Client.Client == null)
          return new ActionResult(ActionResult.Results.Error, Translate.Basket_Контакт_для_получения_кредита_не_указан);
        Decimal? maxSumCredit = this.Client.Client.Group.MaxSumCredit;
        if (maxSumCredit.HasValue)
        {
          ClientAdnSum clientByUidAndSum = new ClientsRepository(dataBase).GetClientByUidAndSum(this.Client.Client.Uid);
          Decimal num = clientByUidAndSum.TotalCreditSum + sum;
          maxSumCredit = clientByUidAndSum.Client.Group.MaxSumCredit;
          Decimal valueOrDefault = maxSumCredit.GetValueOrDefault();
          if (num > valueOrDefault & maxSumCredit.HasValue)
          {
            this.Client = clientByUidAndSum;
            return new ActionResult(ActionResult.Results.Error, string.Format(Translate.Basket_Нельзя_оформить_продажу_на_данного_клиента__так_как_сумма_долга_превышает_допустимую_, (object) (clientByUidAndSum.TotalCreditSum + sum), (object) clientByUidAndSum.Client.Group.MaxSumCredit));
          }
        }
        this.Comment = valueTuple.Item3;
        Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        if (settings.Clients.CreditAuthType != GlobalDictionaries.ActionAuthType.None && !this._isAcsessSms)
        {
          if (!new SmsHelper().CheckingCode(this.Client.Client.Phone, settings.Clients.CreditAuthType, Gbs.Core.Entities.Actions.DoSaleCreditIfOffSmsCode))
            return new ActionResult(ActionResult.Results.Cancel);
          this._isAcsessSms = true;
        }
        return new ActionResult(ActionResult.Results.Ok);
      }
    }

    protected void SaveCertificates()
    {
      foreach (BasketItem basketItem in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Certificate.IsCertificate)))
      {
        GoodsCertificate.Certificate certificateByUd = GoodsCertificate.GetCertificateByUd(basketItem.Certificate.Uid);
        certificateByUd.Status = GlobalDictionaries.CertificateStatus.Saled;
        certificateByUd.Save();
      }
      List<GoodsStocks.GoodStock> list = this.CertificatesForPay.GroupBy<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, Guid>((Func<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, Guid>) (x => x.Certificate.Stock.Uid)).Select<IGrouping<Guid, Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>, GoodsStocks.GoodStock>((Func<IGrouping<Guid, Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>, GoodsStocks.GoodStock>) (x => x.First<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>().Certificate.Stock)).ToList<GoodsStocks.GoodStock>();
      using (Gbs.Core.Db.DataBase db = Gbs.Core.Data.GetDataBase())
      {
        foreach (Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate basketPayCertificate in (IEnumerable<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>) this.CertificatesForPay)
        {
          Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate c = basketPayCertificate;
          object obj = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Good, db.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == c.Certificate.Stock.GoodUid && x.TYPE_UID == GlobalDictionaries.CertificateReusableUid))).FirstOrDefault<EntityProperties.PropertyValue>()?.Value ?? (object) false;
          int index = list.FindIndex((System.Predicate<GoodsStocks.GoodStock>) (x => x.Uid == c.Certificate.Stock.Uid));
          c.Certificate.Status = Convert.ToInt32(obj) == 0 ? GlobalDictionaries.CertificateStatus.Close : GlobalDictionaries.CertificateStatus.Open;
          if (c.Certificate.Status == GlobalDictionaries.CertificateStatus.Open)
            list[index].Stock += 1M;
          c.Certificate.Save();
          c.Certificate.SaveDocumentActivatedCertificate(list[index], this.Document);
        }
        list.ForEach((Action<GoodsStocks.GoodStock>) (x => x.Save(db)));
      }
    }

    protected void SaveTapBeerItems(List<BasketItem> items)
    {
      foreach (BasketItem basketItem in items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.InfoToTapBeer != null)))
      {
        InfoToTapBeer byUid = new InfoTapBeerRepository().GetByUid(basketItem.InfoToTapBeer.Uid);
        byUid.SaleQuantity += basketItem.Quantity;
        new InfoTapBeerRepository().Save(byUid);
      }
    }

    protected void SaveEgaisItems(Document doc)
    {
      LogHelper.WriteToEgaisLog("Зашли записывать упакованное пиво");
      if (!new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        return;
      List<EgaisWriteOffActsItems> writeOffActsItemsList = new List<EgaisWriteOffActsItems>();
      IEnumerable<Gbs.Core.Entities.Documents.Item> objs;
      if (doc == null)
      {
        objs = (IEnumerable<Gbs.Core.Entities.Documents.Item>) null;
      }
      else
      {
        List<Gbs.Core.Entities.Documents.Item> items = doc.Items;
        objs = items != null ? items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => EgaisHelper.GetAlcoholType(x?.Good) == EgaisHelper.AlcoholTypeGorEgais.Beer)) : (IEnumerable<Gbs.Core.Entities.Documents.Item>) null;
      }
      if (objs == null)
        objs = (IEnumerable<Gbs.Core.Entities.Documents.Item>) new List<Gbs.Core.Entities.Documents.Item>();
      foreach (Gbs.Core.Entities.Documents.Item obj in objs)
      {
        LogHelper.WriteToEgaisLog("Нашли пиво в чеке: " + obj.Good.Name);
        obj.GoodStock = GoodsStocks.GetStocksByUid(obj.GoodStock.Uid);
        string numberForGoodStock = SharedRepository.GetFbNumberForGoodStock(obj.GoodStock);
        LogHelper.WriteToEgaisLog("fbNumber: " + numberForGoodStock);
        if (!numberForGoodStock.IsNullOrEmpty())
          writeOffActsItemsList.Add(new EgaisWriteOffActsItems()
          {
            FbNumber = numberForGoodStock,
            MarkInfo = obj.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol ? obj.Comment : "",
            Quantity = obj.Quantity,
            StockUid = obj.GoodStock.Uid,
            Sum = SaleHelper.GetSumItemInDocument(obj),
            ActType = TypeWriteOff1.Реализация
          });
      }
      LogHelper.WriteToEgaisLog("Итоговый лист:\n" + writeOffActsItemsList.ToJsonString(true));
      if (!writeOffActsItemsList.Any<EgaisWriteOffActsItems>())
        return;
      LogHelper.WriteToEgaisLog("Отпарвляем данные в БД");
      new EgaisWriteOffActsItemRepository().Save(writeOffActsItemsList);
    }

    protected void RemoteControlActions()
    {
      Task.Run((Action) (() =>
      {
        try
        {
          Gbs.Core.Config.Email email = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.Email;
          if (!email.IsActive || !email.IsSendForSum || !(email.SumSend <= this.Document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.SellPrice * x.Quantity))) || this.Document.Status != GlobalDictionaries.DocumentsStatuses.Close)
            return;
          LogHelper.Debug("Отправка отчета на почту из-за суммы продажи - " + email.EmailTo);
          new EmailRepository().Send(DateTime.Now, email.EmailTo);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));
      Task.Run((Action) (() =>
      {
        try
        {
          Telegram telegram = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.Telegram;
          if (!telegram.IsActive || !telegram.IsSendForSum || !(telegram.SumSend <= this.Document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.SellPrice * x.Quantity))) || this.Document.Status != GlobalDictionaries.DocumentsStatuses.Close)
            return;
          LogHelper.Debug("Отправка отчета в телеграм из-за суммы продажи - " + telegram.UsernameTo);
          TelegramHelper.SendReport(DateTime.Now, telegram.UsernameTo);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));
      Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      if (!settings.RemoteControl.Cloud.IsAutoSend || !settings.RemoteControl.Cloud.IsActive)
        return;
      BackgroundTasksHelper.AddTaskToQueue((Action) (() => HomeOfficeHelper.CreateArchive()), BackgroundTaskTypes.DataBaseSyncDataPreparing);
    }

    protected void UpdatePaymentListForCertificate(
      List<SelectPaymentMethods.PaymentGrid> listPayment,
      Decimal sumCertificate)
    {
      if (!this.CertificatesForPay.Any<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>())
        return;
      PaymentMethods.PaymentMethod payMethod = listPayment.First<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).Method;
      Decimal? nullable1 = listPayment.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
      Decimal num1 = sumCertificate;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        listPayment.RemoveAll((System.Predicate<SelectPaymentMethods.PaymentGrid>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate));
        listPayment.AddRange(this.CertificatesForPay.Select<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, SelectPaymentMethods.PaymentGrid>((Func<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, SelectPaymentMethods.PaymentGrid>) (x => new SelectPaymentMethods.PaymentGrid()
        {
          Method = payMethod,
          Comment = x.Certificate.Barcode,
          Sum = new Decimal?(x.Nominal)
        })));
      }
      else
      {
        nullable1 = listPayment.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
        Decimal num2 = sumCertificate;
        if (!(nullable1.GetValueOrDefault() < num2 & nullable1.HasValue))
          return;
        Decimal? nullable2 = listPayment.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
        listPayment.RemoveAll((System.Predicate<SelectPaymentMethods.PaymentGrid>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate));
        List<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate> collection = new List<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>((IEnumerable<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>) this.CertificatesForPay);
        foreach (Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate basketPayCertificate in this.CertificatesForPay.OrderBy<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, Decimal>((Func<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, Decimal>) (c => c.Nominal)).Reverse<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>())
        {
          SelectPaymentMethods.PaymentGrid paymentGrid = new SelectPaymentMethods.PaymentGrid();
          paymentGrid.Method = payMethod;
          paymentGrid.Comment = basketPayCertificate.Certificate.Barcode;
          Decimal nominal1 = basketPayCertificate.Nominal;
          nullable1 = nullable2;
          Decimal valueOrDefault = nullable1.GetValueOrDefault();
          Decimal? nullable3;
          if (!(nominal1 >= valueOrDefault & nullable1.HasValue))
          {
            nullable3 = new Decimal?(basketPayCertificate.Nominal);
          }
          else
          {
            nullable1 = nullable2;
            Decimal num3 = 0M;
            nullable3 = nullable1.GetValueOrDefault() < num3 & nullable1.HasValue ? new Decimal?(0M) : nullable2;
          }
          paymentGrid.Sum = nullable3;
          SelectPaymentMethods.PaymentGrid cert = paymentGrid;
          nullable1 = cert.Sum;
          Decimal num4 = 0M;
          if (nullable1.GetValueOrDefault() <= num4 & nullable1.HasValue)
            collection.RemoveAll((System.Predicate<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>) (x => x.Certificate.Barcode == cert.Comment));
          else
            listPayment.Add(cert);
          nullable1 = nullable2;
          Decimal nominal2 = basketPayCertificate.Nominal;
          nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() - nominal2) : new Decimal?();
        }
        this._certificatesForPay.Clear();
        this._certificatesForPay.AddRange((IEnumerable<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>) collection);
      }
    }

    public void SendDisplayBuyerNumbersInfo(Decimal sum)
    {
      try
      {
        if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().DisplayNumbersPayer.Type == DisplayNumbersTypes.None)
          return;
        using (DisplayNumbersHelper displayNumbersHelper = new DisplayNumbersHelper((IConfig) new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
          displayNumbersHelper.WriteNumber(sum);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка в передаче данных на однострочный дисплей");
      }
    }

    public void SendDisplayBuyerInfo(BasketItem item)
    {
      try
      {
        DisplayPayer displayPayer = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().DisplayPayer;
        if (displayPayer.Type == DisplayBuyerTypes.None)
          return;
        List<string> lines = new List<string>();
        string str = item.DisplayedName;
        int length = str.Length;
        int startIndex = (displayPayer.CountRow - 1) * displayPayer.CountCharInRow;
        int num = startIndex;
        if (length > num)
          str = str.Remove(startIndex);
        for (; str.Length > displayPayer.CountCharInRow; str = str.Remove(0, displayPayer.CountCharInRow))
          lines.Add(str.Remove(displayPayer.CountCharInRow));
        lines.Add(str);
        lines.Add(DisplayBuyerHelper.GetTextAndSum(string.Format("*{0:N2}", (object) item.Quantity), item.TotalSum));
        using (DisplayBuyerHelper displayBuyerHelper = new DisplayBuyerHelper((IConfig) new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
          displayBuyerHelper.WriteText(lines);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public GlobalDictionaries.DocumentsTypes DocumentsType { get; set; }

    public void UpdateVisibilityStorage() => this.OnPropertyChanged("VisibilityStorageInfo");

    public Visibility VisibilityStorageInfo
    {
      get
      {
        return Storages.GetStorages().Count<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted)) <= 1 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Decimal KkmCheckCorrection { get; set; }

    public bool IsNeedComment { get; set; } = true;

    public string TrueApiInfoForKkm { get; set; }

    public Guid ClientOrderUid { get; set; } = Guid.Empty;

    public Action UpdateOrderList { get; set; }

    public Visibility VisibilityInfoCredit
    {
      get
      {
        ClientAdnSum client = this.Client;
        Decimal? maxSumCredit;
        int num1;
        if (client == null)
        {
          num1 = 1;
        }
        else
        {
          maxSumCredit = client.Client.Group.MaxSumCredit;
          num1 = !maxSumCredit.HasValue ? 1 : 0;
        }
        if (num1 != 0)
          return Visibility.Collapsed;
        Decimal num2 = this.TotalSum + this.Client.TotalCreditSum;
        maxSumCredit = this.Client.Client.Group.MaxSumCredit;
        Decimal valueOrDefault = maxSumCredit.GetValueOrDefault();
        return !(num2 <= valueOrDefault & maxSumCredit.HasValue) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand ShowNotificationCredit
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.Basket_Информация_о_кредитном_лимите,
          Text = string.Format(Translate.Basket_Существует_вероятность_превышения_кредитного_лимита__0_Текущая_задолженность___1_N2__2_Кредитный_лимит___3_N2_, (object) Other.NewLine(2), (object) this.Client.TotalCreditSum, (object) Other.NewLine(), (object) this.Client.Client.Group.MaxSumCredit)
        })));
      }
    }

    public bool IsUpdateReceive { get; set; }

    public Decimal? ReceiveSum
    {
      get => this._receive;
      set
      {
        this._receive = value;
        this.OnPropertyChanged(nameof (ReceiveSum));
        this.IsUpdateReceive = value.HasValue;
        this.ReCalcTotals();
      }
    }

    public string SaleNumber
    {
      get => this._saleNumber;
      set
      {
        this._saleNumber = value;
        this.OnPropertyChanged(nameof (SaleNumber));
      }
    }

    public bool IsPrintCheck
    {
      get => this._isPrintCheck;
      set
      {
        this._isPrintCheck = value;
        this.OnPropertyChanged(nameof (IsPrintCheck));
      }
    }

    public bool IsEnablePrintCheck
    {
      get => this._isEnablePrintCheck;
      set
      {
        this._isEnablePrintCheck = value;
        this.OnPropertyChanged(nameof (IsEnablePrintCheck));
      }
    }

    public bool IsCheckedClient
    {
      get => this._isCheckedClient;
      set
      {
        this._isCheckedClient = value;
        if (!value)
          this.Client = (ClientAdnSum) null;
        this.OnPropertyChanged(nameof (IsCheckedClient));
      }
    }

    public Decimal ChangeSum
    {
      get => !this.IsUpdateReceive ? 0M : this.ReceiveSum.GetValueOrDefault() - this.TotalSum;
    }

    public Visibility VisibilitySumCertificate
    {
      get
      {
        return !this.CertificatesForPay.Any<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilitySumPrepaid
    {
      get
      {
        return !this.PaymentsPrepaid.Any<Gbs.Core.Entities.Payments.Payment>() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityPhone
    {
      get
      {
        ClientAdnSum client1 = this.Client;
        bool? nullable;
        if (client1 == null)
        {
          nullable = new bool?();
        }
        else
        {
          Gbs.Core.Entities.Clients.Client client2 = client1.Client;
          if (client2 == null)
          {
            nullable = new bool?();
          }
          else
          {
            string phone = client2.Phone;
            nullable = phone != null ? new bool?(phone.IsNullOrEmpty()) : new bool?();
          }
        }
        return !nullable.GetValueOrDefault(true) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public string ClientName
    {
      get => this.Client?.Client.Name ?? Translate.MainWindowViewModel_Выберите_клиента;
    }

    public string ClientPhone => "(" + this.Client?.Client.Phone + ")";

    public ClientAdnSum Client
    {
      get => this._client;
      set
      {
        this._client = value;
        this.ReCalcTotals();
        this.OnPropertyChanged(nameof (Client));
        this.OnPropertyChanged("ClientName");
        this.OnPropertyChanged("VisibilityInfoCredit");
        this.OnPropertyChanged("VisibilityPhone");
        this.OnPropertyChanged("ClientPhone");
      }
    }

    public IReadOnlyList<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate> CertificatesForPay
    {
      get => (IReadOnlyList<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>) this._certificatesForPay;
    }

    public Decimal CertificatesPaymentSum
    {
      get
      {
        return this.CertificatesForPay.Sum<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>((Func<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, Decimal>) (x => x.Nominal));
      }
    }

    public Decimal PrepaidPaymentsSum
    {
      get
      {
        return this.PaymentsPrepaid.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      }
    }

    public string Comment { get; set; } = string.Empty;

    public ICommand EditCommentCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditComment(obj, false)));
    }

    public ICommand EditDiscountCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.EditDiscount));
    }

    public ICommand EditAllDiscountCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditAllDiscount()));
    }

    public override ICommand DeleteItemCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.DeleteItems));
    }

    public override ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditQuantity(obj)));
    }

    public Decimal TotalDiscount
    {
      get => this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (i => i.DiscountSum));
    }

    public Decimal TotalSum
    {
      get
      {
        return this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (i => i.TotalSum)) - this.KkmCheckCorrection;
      }
    }

    public Decimal Delivery { get; set; }

    private BasketDiscounter BasketDiscounter { get; }

    private ExtraPricer BasketExtraPricer { get; }

    public Basket()
    {
      this._recalcTimer.Elapsed += (ElapsedEventHandler) ((a, b) => this.recalcTotalAsync());
      this.SaleNumber = SalePoints.GetSalePointList().FirstOrDefault<SalePoints.SalePoint>()?.Number.SaleNumber.ToString() ?? "";
      this.BasketDiscounter = new BasketDiscounter(this);
      this.BasketExtraPricer = new ExtraPricer(this);
      this.Items.CollectionChanged += (NotifyCollectionChangedEventHandler) ((_, __) => { });
      this.DocumentsType = GlobalDictionaries.DocumentsTypes.Sale;
      this.OnPropertyChanged(nameof (VisibilityStorageInfo));
    }

    private Basket(bool isClone)
    {
    }

    public void CheckOrder()
    {
      if (!(this.ClientOrderUid != Guid.Empty))
        return;
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        Document byUid = documentsRepository.GetByUid(this.ClientOrderUid);
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.Basket_ПродажаБылаСформированаИзЗаказаНоНеБылаЗавершена, (object) byUid.Number));
        byUid.Status = GlobalDictionaries.DocumentsStatuses.Open;
        documentsRepository.Save(byUid);
        if (byUid.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)))
        {
          ClientOrderViewModel.OptionalClientOrder optionalClientOrder = JsonConvert.DeserializeObject<ClientOrderViewModel.OptionalClientOrder>(byUid.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)).Value.ToString());
          if (optionalClientOrder != null && optionalClientOrder.IsReserveGood)
            ClientOrderViewModel.ReserveGood(byUid, optionalClientOrder.IsReserveGood);
        }
        this.PaymentsPrepaid.Clear();
        this.ClientOrderUid = Guid.Empty;
        this.PaymentsPrepaid.Clear();
        this.OnPropertyChanged("PrepaidPaymentsSum");
        Action updateOrderList = this.UpdateOrderList;
        if (updateOrderList == null)
          return;
        updateOrderList();
      }
    }

    public void AddCertificate(Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate certificate)
    {
      this._certificatesForPay.Add(certificate);
      this.OnPropertyChanged("CertificatesPaymentSum");
      this.OnPropertyChanged("VisibilitySumCertificate");
    }

    private void DeleteItems(object obj)
    {
      (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Gbs.Core.Entities.Actions.DeleteItemBasket);
      List<BasketItem> castedList;
      if (!access.Result || !this.CheckSelectedItems(obj, out castedList) || MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) castedList.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        return;
      foreach (BasketItem basketItem in castedList)
      {
        this.Items.Remove(basketItem);
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) basketItem, (IEntity) basketItem, ActionType.Delete, GlobalDictionaries.EntityTypes.ItemList, access.User), false);
      }
      if (!this.Items.Any<BasketItem>())
        this.Storage = (Storages.Storage) null;
      this.ReCalcTotals();
      this.OnPropertyChanged("Items");
    }

    public void EditComment(object obj, bool isReturn)
    {
      List<BasketItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return;
      if (castedList.Count > 1)
        MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
      else if (this.SelectedItem.Certificate.Uid != Guid.Empty)
      {
        MessageBoxHelper.Warning(Translate.Basket_Невозможно_сменить_комментарий_для_товара_сертификата);
      }
      else
      {
        string comment = this.SelectedItem.Comment;
        string message = Translate.MainWindowViewModel_Введите_комментарий_к_товару__ + this.SelectedItem.Good.Name;
        CultureInfo cultureInfo1 = (CultureInfo) null;
        bool flag = false;
        EgaisHelper.AlcoholTypeGorEgais alcoholType = EgaisHelper.GetAlcoholType(this.SelectedItem.Good);
        if (this.SelectedItem.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None || alcoholType == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)
        {
          cultureInfo1 = InputLanguageManager.Current.CurrentInputLanguage;
          if ((Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
          {
            flag = true;
            KeyboardLayoutHelper.IsDownCapsLock();
          }
          IEnumerable availableInputLanguages = InputLanguageManager.Current.AvailableInputLanguages;
          CultureInfo cultureInfo2 = availableInputLanguages != null ? availableInputLanguages.OfType<CultureInfo>().FirstOrDefault<CultureInfo>((Func<CultureInfo, bool>) (l => l.Name.StartsWith("en"))) : (CultureInfo) null;
          if (cultureInfo2 != null)
          {
            LogHelper.Debug("Устанавливаю EN раскладку для ввода кода маркировки");
            InputLanguageManager.Current.CurrentInputLanguage = cultureInfo2;
          }
          message = alcoholType == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol ? "Отсканируйте акцизную марку с упаковки товара: " + this.SelectedItem.DisplayedName : string.Format(Translate.Basket_Отсканируйте_код_маркировки_с_упаковки_товара___0_, (object) this.SelectedItem.Good.Name);
        }
        (bool result, string output) = MessageBoxHelper.Input(comment, message);
        if (cultureInfo1 != null)
          InputLanguageManager.Current.CurrentInputLanguage = cultureInfo1;
        if (flag && (Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) != KeyStates.Toggled)
          KeyboardLayoutHelper.IsDownCapsLock();
        this.SelectedItem.Comment = result ? output : comment;
        if (isReturn)
          return;
        this.ShowCommentNotifications();
      }
    }

    private void SetItemCommentColor(BasketItem item, Gbs.Core.ViewModels.Basket.Basket.CustomColors color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.None)
    {
      try
      {
        Color color1;
        switch (color)
        {
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.None:
            color1 = Colors.Transparent;
            break;
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.Red:
            color1 = Colors.Red;
            break;
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.Green:
            color1 = Colors.Green;
            break;
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange:
            color1 = Colors.Orange;
            break;
          default:
            color1 = Colors.Transparent;
            break;
        }
        SolidColorBrush sbc = new SolidColorBrush(color1);
        sbc.Freeze();
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => item.CommentColor = sbc));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void SetItemPriceColor(BasketItem item, Gbs.Core.ViewModels.Basket.Basket.CustomColors color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.None)
    {
      try
      {
        Color color1;
        switch (color)
        {
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.None:
            color1 = Colors.Transparent;
            break;
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange:
            color1 = Colors.Orange;
            break;
          default:
            color1 = Colors.Transparent;
            break;
        }
        SolidColorBrush sbc = new SolidColorBrush(color1);
        sbc.Freeze();
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => item.PriceColor = sbc));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public void SetItemNameColor(BasketItem item, Gbs.Core.ViewModels.Basket.Basket.CustomColors color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.None)
    {
      try
      {
        Color color1;
        switch (color)
        {
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.None:
            color1 = Colors.Transparent;
            break;
          case Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange:
            color1 = Colors.Orange;
            break;
          default:
            color1 = Colors.Transparent;
            break;
        }
        SolidColorBrush sbc = new SolidColorBrush(color1);
        sbc.Freeze();
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => item.DisplayedNameColor = sbc));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public void ShowCommentNotifications()
    {
      if (this.SelectedItem == null)
        return;
      GlobalDictionaries.Countries country = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country;
      if (!country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Ukraine))
        return;
      this.SelectedItem.ErrorStr = (string) null;
      this.SetItemCommentColor(this.SelectedItem);
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Comment == this.SelectedItem.Comment && x.Uid != this.SelectedItem.Uid && x.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None && this.SelectedItem.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None && !string.IsNullOrEmpty(this.SelectedItem.Comment))))
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = PartnersHelper.ProgramName(),
          Text = Translate.Basket_EditComment_,
          Type = ProgressBarViewModel.Notification.NotificationsTypes.Warning
        });
      if (this.SelectedItem.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.None)
        return;
      MarkedInfo markedInfo = new MarkedInfo(this.SelectedItem.Comment, this.SelectedItem.Good.Group.RuMarkedProductionType);
      bool flag = markedInfo.IsValidCode();
      Gbs.Core.Config.Sales sales = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales;
      if (!flag)
      {
        string str = this.SelectedItem.Comment.IsNullOrEmpty() ? string.Format(Translate.Basket_ShowCommentNotifications_Не_указан_код_маркировки_для_товара___0___, (object) this.SelectedItem.DisplayedName) : string.Format(Translate.Basket_ShowCommentNotifications_Указанный_код_маркировки_для_товара___0___имеет_некорректный_формат_, (object) this.SelectedItem.DisplayedName);
        this.SelectedItem.ErrorStr = this.SelectedItem.Comment.IsNullOrEmpty() ? Translate.Basket_ShowCommentNotifications_Не_указан_код_маркировки : Translate.Basket_ShowCommentNotifications_Указанный_код_маркировки_имеет_некорректный_формат;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = PartnersHelper.ProgramName(),
          Text = str,
          Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
        });
        this.SetItemCommentColor(this.SelectedItem, Gbs.Core.ViewModels.Basket.Basket.CustomColors.Red);
      }
      else
      {
        List<string> listError = new List<string>();
        bool isErrorCheck = false;
        bool isSkipVerified = false;
        if (sales.IsCheckMarkInfoTrueApi)
        {
          if (country == GlobalDictionaries.Countries.Russia)
          {
            Decimal tobaccoSalePrice;
            flag &= TrueApiRepository.CheckCode(markedInfo.FullCode, out listError, this.DocumentsType, out isErrorCheck, out isSkipVerified, out tobaccoSalePrice);
            this.SelectedItem.TobaccoSalePrice = tobaccoSalePrice;
          }
        }
        else
          isSkipVerified = true;
        if (!flag && !isSkipVerified)
        {
          string str = string.Join("\n - ", (IEnumerable<string>) listError);
          this.SelectedItem.ErrorStr = str;
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = PartnersHelper.ProgramName(),
            Text = string.Format(Translate.Basket_ShowCommentNotifications_УказанныйКодМаркировкиНеПрошелПроверкувЧЗ, (object) str),
            Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
          });
          this.SetItemCommentColor(this.SelectedItem, Gbs.Core.ViewModels.Basket.Basket.CustomColors.Red);
        }
        else
        {
          Gbs.Core.ViewModels.Basket.Basket.CustomColors color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.Green;
          if (isErrorCheck)
            color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange;
          if (isSkipVerified)
            color = Gbs.Core.ViewModels.Basket.Basket.CustomColors.None;
          this.SetItemCommentColor(this.SelectedItem, color);
          this.ReCalcTotals();
          this.CheckPriceForTobacco();
        }
      }
    }

    private void EditAllDiscount()
    {
      this.EditDiscount((object) this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)).ToList<BasketItem>());
    }

    private void EditDiscount(object obj)
    {
      List<BasketItem> castedList;
      if (!new Authorization().GetAccess(Gbs.Core.Entities.Actions.EditDiscountItem).Result || !this.CheckSelectedItems(obj, out castedList))
        return;
      if (castedList.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
      {
        MessageBoxHelper.Warning(Translate.GoodSetPageViewModel_Невозможно_изменить_скидку_для_сертификата);
      }
      else
      {
        (bool result, Decimal discount) = new EditGoodDiscountViewModel().ShowCard(new EditGoodDiscountViewModel.DiscountInfo((IReadOnlyCollection<BasketItem>) castedList));
        if (!result)
          return;
        foreach (BasketItem basketItem1 in castedList)
        {
          BasketItem item = basketItem1;
          BasketItem basketItem2 = this.Items.Single<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid));
          if (item.Certificate.IsCertificate && item.TotalSum < item.Certificate.Nominal)
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.Basket_Пересчет_корзины,
              Text = Translate.Basket_Невозможно_установить_скидку__так_как_цена_сертификата_будет_меньше_его_номинала_
            });
          else
            basketItem2.Discount.SetDiscount(discount, SaleDiscount.ReasonEnum.UserEdit, item, this);
        }
        this.ReCalcTotals();
        this.CheckPriceForTobacco();
      }
    }

    private bool EditQuantity(object obj, bool needAuth = true, bool isEnableCount = true, bool isReCalcTotals = true)
    {
      List<BasketItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return false;
      if (castedList.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
      {
        MessageBoxHelper.Warning(Translate.GoodSetPageViewModel_Невозможно_изменить_кол_во_для_сертификата);
        return false;
      }
      if (castedList.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
      {
        MessageBoxHelper.Warning(string.Format(Translate.Basket_Невозможно_изменить_стоимость_для_услуги___0____так_как_стоимость_рассчитывается_автоматически_, (object) castedList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).DisplayedName));
        return false;
      }
      if (!this.DocumentsType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Move, GlobalDictionaries.DocumentsTypes.MoveStorage, GlobalDictionaries.DocumentsTypes.BeerProductionSet, GlobalDictionaries.DocumentsTypes.ProductionSet))
      {
        List<GoodsUnits.GoodUnit> units = CachesBox.AllGoodsUnits();
        if (castedList.Any<BasketItem>((Func<BasketItem, bool>) (x =>
        {
          if (EgaisHelper.GetAlcoholType(x.Good) != EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)
          {
            if (x.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol))
              return false;
          }
          GoodsUnits.GoodUnit goodUnit = units.SingleOrDefault<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (u => x.Good.Group.UnitsUid == u.Uid));
          return (goodUnit != null ? goodUnit.RuFfdUnitsIndex : 0) == 0;
        })))
        {
          if (castedList.All<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Tobacco))))
          {
            isEnableCount = true;
          }
          else
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.Basket_EditQuantity_Изменить_количество_для_маркируемой_продукции_невозможно__Количество_всегда_будет_равно_1__));
            isEnableCount = false;
          }
        }
      }
      if (needAuth && !new Authorization().GetAccess(Gbs.Core.Entities.Actions.EditCountItemBasket).Result)
        return false;
      Gbs.Core.Config.Sales sales = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales;
      List<BasketItem> list1 = castedList.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (x => x.Clone())).ToList<BasketItem>();
      if (sales.IsUnitsInGrams)
        list1.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight)).ToList<BasketItem>().ForEach((Action<BasketItem>) (x =>
        {
          x.Quantity *= 1000M;
        }));
      EditGoodQuantityViewModel.QuantityRequest info = new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) list1, true);
      info.IsEnableCount = isEnableCount;
      if (this.DocumentsType == GlobalDictionaries.DocumentsTypes.ProductionSet && list1.All<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.Beer || x.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol)))
        info.IsTextQuantityForBeer = true;
      info.IsVisibilitySaleSumForBasket = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsVisibilitySaleSumForBasket && this.DocumentsType == GlobalDictionaries.DocumentsTypes.Sale;
      bool isOfferPreviousSalePrice = false;
      if (this.DocumentsType == GlobalDictionaries.DocumentsTypes.ProductionSet)
        isOfferPreviousSalePrice = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Production.IsOfferPreviousSalePrice;
      (bool result, Decimal? quantity, Decimal? salePrice1) = new EditGoodQuantityViewModel().ShowQuantityWithSalePriceEdit(info, isOfferPreviousSalePrice);
      if (!result)
        return false;
      Decimal? salePrice2 = info.SalePrice;
      Decimal? nullable1 = salePrice1;
      bool flag = !(salePrice2.GetValueOrDefault() == nullable1.GetValueOrDefault() & salePrice2.HasValue == nullable1.HasValue) && salePrice1.HasValue;
      Other.ConsoleWrite(string.Format("{0} : old: {1}; new: {2}", (object) flag, (object) info.SalePrice, (object) salePrice1));
      foreach (BasketItem basketItem1 in castedList)
      {
        Decimal num1 = !sales.IsUnitsInGrams || basketItem1.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Weight ? 1M : 1000M;
        Decimal? nullable2 = quantity;
        Decimal num2 = num1;
        nullable1 = nullable2.HasValue ? new Decimal?(nullable2.GetValueOrDefault() / num2) : new Decimal?();
        Decimal q = nullable1 ?? basketItem1.Quantity;
        if (!this.DocumentsType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Move, GlobalDictionaries.DocumentsTypes.MoveStorage))
        {
          List<int> list2 = ((IEnumerable<string>) sales.SmokeBlockValues.Split(new char[3]
          {
            ' ',
            ';',
            ','
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>(new Func<string, int>(int.Parse)).ToList<int>();
          list2.Add(1);
          if (basketItem1.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Tobacco && list2.All<int>((Func<int, bool>) (x => (Decimal) x != q)))
          {
            MessageBoxHelper.Warning(string.Format(Translate.Basket_EditQuantity_Для_товара___0___невозможно_указать_количество_кроме_1_или_10__так_как_он_является_маркируемой_табачной_продукцией_, (object) basketItem1.DisplayedName, (object) string.Join<int>(", ", list2.Where<int>((Func<int, bool>) (x => x != 1)))));
            q = 1M;
          }
          if (basketItem1.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol) && q > 1M)
          {
            MessageBoxHelper.Warning(string.Format(Translate.Basket_EditQuantity_Для_товара___0___невозможно_указать_количество_больше_1__так_как_он_является_маркируемой_продукцией_, (object) basketItem1.DisplayedName));
            q = 1M;
          }
        }
        basketItem1.Quantity = q;
        basketItem1.IsPriceEditByUser = flag || basketItem1.IsPriceEditByUser;
        BasketItem basketItem2 = basketItem1;
        Decimal num3;
        if (!basketItem1.IsPriceEditByUser)
        {
          num3 = basketItem1.BasePrice;
        }
        else
        {
          nullable1 = salePrice1;
          num3 = nullable1 ?? basketItem1.SalePrice;
        }
        basketItem2.BasePrice = num3;
      }
      if (isReCalcTotals)
        this.ReCalcTotals();
      this.CheckPriceForTobacco();
      if (castedList.Count == 1)
        this.SendDisplayBuyerInfo(castedList.Single<BasketItem>());
      return true;
    }

    public bool EditQuantity(BasketItem item)
    {
      return this.EditQuantity((object) new List<BasketItem>()
      {
        item
      }, false);
    }

    public CheckFiscalTypes CheckFiscalType { get; set; }

    public bool TryPrintCheck(out MessageBoxResult result, CheckFiscalTypes checkFiscalType = CheckFiscalTypes.Fiscal)
    {
      this.CheckFiscalType = checkFiscalType;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      PreparedCheckData checkFromBasket = new CheckFactory(devices.CheckPrinter).CreateCheckFromBasket(this);
      if (checkFromBasket.CheckData.DiscountSum > 0M && (devices.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter || checkFiscalType == CheckFiscalTypes.NonFiscal))
        checkFromBasket.CheckData.PaymentsList.Add(new CheckPayment()
        {
          Method = GlobalDictionaries.KkmPaymentMethods.Cash,
          Name = Translate.FrmCardSale_СкидкаПоЧеку,
          Sum = checkFromBasket.CheckData.DiscountSum
        });
      while (!this.PrintCheck(checkFromBasket))
      {
        MessageBoxResult messageBoxResult = MessageBoxHelper.Show(Translate.Basket_Чек_не_удалось_распечатать__Попробовать_еще_раз_, buttons: MessageBoxButton.YesNoCancel, icon: MessageBoxImage.Hand);
        result = messageBoxResult;
        switch (messageBoxResult)
        {
          case MessageBoxResult.Cancel:
            return false;
          case MessageBoxResult.Yes:
            continue;
          case MessageBoxResult.No:
            return true;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      result = MessageBoxResult.OK;
      return true;
    }

    protected ActionResult ValidateBasket()
    {
      if (!this.Items.Any<BasketItem>())
        return new ActionResult(ActionResult.Results.Warning, Translate.CafeBasket_В_корзине_нет_ни_одного_товара);
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.SalePrice == 0M)) && MessageBoxHelper.Question(Translate.Basket_В_корзине_есть_товары_с_нулевой_ценой__Вы_уверены__что_хотите_сохранить_эту_продажу_) == MessageBoxResult.No)
        return new ActionResult(ActionResult.Results.Warning);
      EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
      if (egais.IsLimitedForTime && this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) != EgaisHelper.AlcoholTypeGorEgais.NoAlcohol || EgaisHelper.IsBeerKega(x.Good) || x.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol)) && DateTime.Now.IsBetween(egais.StartTimeLimited, egais.FinishTimeLimited))
      {
        MessageBoxHelper.Error("В период времени с " + egais.StartTimeLimited.ToString("HH:mm") + " до " + egais.FinishTimeLimited.ToString("HH:mm") + " запрещена продажа алкогольной продукции\n\nДля сохранения продажи необходимо удалить все позиции алкоголя из чека.");
        return new ActionResult(ActionResult.Results.Warning);
      }
      if (this.CertificatesForPay.Any<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>() && this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Certificate.IsCertificate)))
        return new ActionResult(ActionResult.Results.Warning, Translate.CafeBasket_Невозможно_использовать_сертификат_в_продаже__так_как_в_корзине_есть_еще_один_сертификат__Разделите_продажи_);
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x =>
      {
        string comment = x.Comment;
        return comment != null && comment.Length > 500;
      })))
        return new ActionResult(ActionResult.Results.Warning, string.Format(Translate.Basket_ValidateBasket_, (object) string.Join("\r\n", this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Comment.Length > 500)).Select<BasketItem, string>((Func<BasketItem, string>) (x => x.Good.Name)))));
      if (!new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.AllowSalesMissingItems)
      {
        IEnumerable<IGrouping<\u003C\u003Ef__AnonymousType29<Guid, Guid>, BasketItem>> groupings = this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))).GroupBy(x =>
        {
          Guid uid = x.Good.Uid;
          GoodsModifications.GoodModification goodModification = x.GoodModification;
          // ISSUE: explicit non-virtual call
          Guid guid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
          return new{ g = uid, m = guid };
        });
        List<string> messages = new List<string>()
        {
          Translate.Basket_колво_для_следующих_больше
        };
        foreach (IGrouping<\u003C\u003Ef__AnonymousType29<Guid, Guid>, BasketItem> grouping in groupings)
        {
          IGrouping<\u003C\u003Ef__AnonymousType29<Guid, Guid>, BasketItem> i = grouping;
          Decimal num1 = i.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
          Decimal num2 = i.First<BasketItem>().Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.ModificationUid == i.Key.m && x.Storage.Uid == this.Storage.Uid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
          if (i.First<BasketItem>().Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit))
          {
            GoodsCatalogModelView.GoodsInfoGrid good = new GoodsCatalogModelView.GoodsInfoGrid();
            good.Good = i.First<BasketItem>().Good;
            GoodsSearchModelView.GetPriceForKit(good);
            num2 = good.GoodTotalStock.GetValueOrDefault();
          }
          if (num1 > num2)
            messages.Add(i.First<BasketItem>().DisplayedName + " (" + Translate.Basket_на_остатке__ + num2.ToString("N3") + ")");
        }
        if (messages.Count > 1)
          return new ActionResult(ActionResult.Results.Warning, messages);
      }
      if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsTabooSaleLessBuyPrice)
      {
        BuyPriceCounter buyPriceCounter = new BuyPriceCounter();
        List<string> messages = new List<string>()
        {
          Translate.Basket_ValidateBasket_УСледующихТоваровУказанаЦенаМеньшеСебестоимости
        };
        foreach (BasketItem basketItem in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))))
        {
          Decimal lastBuyPrice = buyPriceCounter.GetLastBuyPrice(basketItem.Good);
          if (!(lastBuyPrice == 0M) && lastBuyPrice > basketItem.SalePrice * (1M - basketItem.Discount.Value / 100M))
          {
            Decimal num = Math.Round((basketItem.BasePrice - lastBuyPrice) / basketItem.SalePrice * 100M, MidpointRounding.AwayFromZero);
            if (num < 0M)
            {
              basketItem.ErrorStrForDisplayedName = Translate.Basket_ValidateBasket_Цена_товара_скорректирована_в_соответвеии_с_настройками_программы__запрет_на_продажу_менее_закупочной_цены_;
              basketItem.SalePrice = lastBuyPrice;
              basketItem.BasePrice = lastBuyPrice;
              basketItem.Discount = new SaleDiscount()
              {
                MaxValue = 0M
              };
            }
            else
            {
              basketItem.ErrorStrForDisplayedName = Translate.Basket_ValidateBasket_Скидка_на_товар_скорректирована_в_соответвеии_с_настройками_программы__запрет_на_продажу_менее_закупочной_цены_;
              basketItem.Discount = new SaleDiscount()
              {
                MaxValue = num
              };
            }
            this.SetItemNameColor(basketItem, Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange);
            messages.Add(basketItem.DisplayedName);
          }
        }
        if (messages.Count > 1)
        {
          this.ReCalcTotals();
          return new ActionResult(ActionResult.Results.Warning, messages);
        }
      }
      List<BasketItem> list1 = this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => !x.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Kz_Shoes, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol))).ToList<BasketItem>();
      List<string> source1 = new List<string>()
      {
        Translate.Basket_ValidateBasket_Данные_товары_не_прошли_предварительную_проверку_в_Честном_знаке_
      };
      List<string> source2 = new List<string>()
      {
        "У следующей табачной продукции розничная цена не соответствует заложенному значению МРЦ в коде:"
      };
      if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsCheckMarkInfoTrueApi && list1.Any<BasketItem>((Func<BasketItem, bool>) (x => !x.Comment.IsNullOrEmpty())) && new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia)
      {
        string infoForKkm;
        List<CheckCodesHelper.CheckCodeInfoItem> source3 = TrueApiRepository.CheckCodes(list1.Where<BasketItem>((Func<BasketItem, bool>) (x => !x.Comment.IsNullOrEmpty())).Select<BasketItem, CheckCodesHelper.CheckCodeInfoItem>((Func<BasketItem, CheckCodesHelper.CheckCodeInfoItem>) (x => new CheckCodesHelper.CheckCodeInfoItem()
        {
          Uid = x.Uid,
          MarkedInfo = new MarkedInfo(x.Comment, x.Good.Group.RuMarkedProductionType).FullCode
        })).ToList<CheckCodesHelper.CheckCodeInfoItem>(), out infoForKkm);
        if (new ConfigsRepository<Integrations>().Get().Crpt.IsBanSaleIfFailConnect && infoForKkm.IsNullOrEmpty())
          return new ActionResult(ActionResult.Results.Warning, "Не удалось выполнить проверку маркировки в Честном знаке. Продажа запрещена настройками программы.");
        if (source3.Any<CheckCodesHelper.CheckCodeInfoItem>())
        {
          this.TrueApiInfoForKkm = infoForKkm;
          foreach (CheckCodesHelper.CheckCodeInfoItem checkCodeInfoItem in source3.Where<CheckCodesHelper.CheckCodeInfoItem>((Func<CheckCodesHelper.CheckCodeInfoItem, bool>) (x => !x.AllowForSale)))
          {
            CheckCodesHelper.CheckCodeInfoItem infoItem = checkCodeInfoItem;
            source1.Add(list1.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == infoItem.Uid)).DisplayedName);
            list1.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == infoItem.Uid)).ErrorStr = string.Join("\n", (IEnumerable<string>) infoItem.ListError);
          }
          foreach (BasketItem basketItem in list1)
          {
            BasketItem item = basketItem;
            CheckCodesHelper.CheckCodeInfoItem checkCodeInfoItem = source3.SingleOrDefault<CheckCodesHelper.CheckCodeInfoItem>((Func<CheckCodesHelper.CheckCodeInfoItem, bool>) (x => x.Uid == item.Uid));
            if (checkCodeInfoItem != null)
            {
              item.TobaccoSalePrice = checkCodeInfoItem.TobaccoSalePrice;
              Gbs.Core.ViewModels.Basket.Basket.CustomColors color = source3.Any<CheckCodesHelper.CheckCodeInfoItem>() ? checkCodeInfoItem.Color : Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange;
              this.SetItemCommentColor(item, color);
            }
          }
          if (new ConfigsRepository<Integrations>().Get().Crpt.IsCheckTobaccoPriceForMark)
            source2.AddRange(list1.Where<BasketItem>((Func<BasketItem, bool>) (x => x.TobaccoSalePrice != 0M && x.TobaccoSalePrice != x.TotalSum)).Select<BasketItem, string>((Func<BasketItem, string>) (x => string.Format(" - {0} цена {1:N2}, МРЦ: {2:N2};", (object) x.DisplayedName, (object) x.TotalSum, (object) x.TobaccoSalePrice))));
        }
        else
        {
          foreach (BasketItem basketItem in list1.Where<BasketItem>((Func<BasketItem, bool>) (x => x.CommentColor.Color == Colors.Red)))
            source1.Add(basketItem.DisplayedName);
        }
      }
      else if (list1.Any<BasketItem>())
        LogHelper.WriteToCrptLog("Онлайн-проверка КМ отключена пользователем, попытка продать следующие КМ:\\n" + string.Join("\n", list1.Select<BasketItem, string>((Func<BasketItem, string>) (x => x.Comment))), NLog.LogLevel.Info);
      if (list1.Any<BasketItem>())
      {
        List<string> source4 = new List<string>()
        {
          Translate.Basket_УказанНекорректныйКодМаркировки
        };
        List<string> source5 = new List<string>()
        {
          Translate.Basket_УТоваровДублируетсяКодМаркировки
        };
        List<string> source6 = new List<string>()
        {
          Translate.Basket_УТоваровДлинаКодаМаркировкиНекорректная
        };
        Regex reg = new Regex("^[A-Za-z0-9%*;.,=+/#$-_%()/\"!?↔: \u001D]+$");
        List<string> source7 = new List<string>();
        foreach (BasketItem basketItem1 in list1)
        {
          BasketItem basketItem = basketItem1;
          if (!basketItem.Comment.IsNullOrEmpty())
          {
            if (basketItem.Comment.Length < 15 || basketItem.Comment.Length > 140)
            {
              source6.Add(basketItem.DisplayedName);
            }
            else
            {
              source7.AddRange(basketItem.Comment.Where<char>((Func<char, bool>) (c => !reg.Match(c.ToString()).Success)).Select<char, string>((Func<char, string>) (x => x.ToString())));
              if (!reg.IsMatch(basketItem.Comment))
                source4.Add(basketItem.DisplayedName);
              else if (this.Items.Count<BasketItem>((Func<BasketItem, bool>) (x => string.Equals(x.Comment, basketItem.Comment, StringComparison.CurrentCultureIgnoreCase))) > 1)
                source5.Add(basketItem.DisplayedName);
            }
          }
        }
        List<string> list2 = source6.Distinct<string>().ToList<string>();
        List<string> list3 = source4.Distinct<string>().ToList<string>();
        List<string> list4 = source5.Distinct<string>().ToList<string>();
        List<string> list5 = source7.Distinct<string>().ToList<string>();
        List<string> list6 = source1.Distinct<string>().ToList<string>();
        List<string> list7 = source2.Distinct<string>().ToList<string>();
        List<string> stringList1 = new List<string>();
        if (list5.Any<string>())
          LogHelper.Debug("Некорректные символы в коде маркировки: " + string.Join(", ", (IEnumerable<string>) list5));
        char ch;
        if (list6.Count > 1)
        {
          if (stringList1.Any<string>())
          {
            List<string> stringList2 = stringList1;
            ch = '\n';
            string str = ch.ToString();
            stringList2.Add(str);
          }
          stringList1.AddRange((IEnumerable<string>) list6);
        }
        if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsTabooSaleNoСorrected)
        {
          if (list3.Count > 1)
            stringList1.AddRange((IEnumerable<string>) list3);
          if (list4.Count > 1)
          {
            if (stringList1.Any<string>())
            {
              List<string> stringList3 = stringList1;
              ch = '\n';
              string str = ch.ToString();
              stringList3.Add(str);
            }
            stringList1.AddRange((IEnumerable<string>) list4);
          }
          if (list2.Count > 1)
          {
            if (stringList1.Any<string>())
            {
              List<string> stringList4 = stringList1;
              ch = '\n';
              string str = ch.ToString();
              stringList4.Add(str);
            }
            stringList1.AddRange((IEnumerable<string>) list2);
          }
          if (list7.Count > 1)
          {
            if (stringList1.Any<string>())
            {
              List<string> stringList5 = stringList1;
              ch = '\n';
              string str = ch.ToString();
              stringList5.Add(str);
            }
            stringList1.AddRange((IEnumerable<string>) list7);
          }
          if (stringList1.Count > 1)
            return new ActionResult(ActionResult.Results.Warning, stringList1);
        }
      }
      if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsTabooSaleNoLabel)
      {
        List<string> messages = new List<string>()
        {
          Translate.Basket_УТоваровНеУказанКодМаркировки
        };
        List<BasketItem> source8 = new List<BasketItem>();
        foreach (BasketItem basketItem in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => !x.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Kz_Shoes, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol))))
        {
          if (basketItem.Comment.IsNullOrEmpty())
            source8.Add(basketItem);
        }
        List<BasketItem> list8 = source8.Distinct<BasketItem>().ToList<BasketItem>();
        if (list8.Any<BasketItem>())
        {
          messages.AddRange(list8.Select<BasketItem, string>((Func<BasketItem, string>) (x => x.DisplayedName)));
          if (messages.Count > 1)
            return new ActionResult(ActionResult.Results.Warning, messages);
        }
      }
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)))
      {
        List<string> values1 = new List<string>()
        {
          Translate.Basket_ValidateBasket_уСледующихПозицийОдинаковаяМарка
        };
        List<string> values2 = new List<string>()
        {
          Translate.Basket_ValidateBasket_уПозицийНеОтсканированаМарка
        };
        foreach (BasketItem basketItem2 in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)))
        {
          BasketItem basketItem = basketItem2;
          if (this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)).Count<BasketItem>((Func<BasketItem, bool>) (x => string.Equals(x.Comment, basketItem.Comment, StringComparison.CurrentCultureIgnoreCase))) > 1)
            values1.Add(basketItem.DisplayedName);
          if (basketItem.Comment.IsNullOrEmpty())
            values2.Add(basketItem.DisplayedName);
        }
        if (values1.Count > 1 || values2.Count > 1)
          return new ActionResult(ActionResult.Results.Warning, "Невозможно сохранить продажу.\n\n" + (values2.Count > 1 ? string.Join("\n", (IEnumerable<string>) values2) + "\n\n" : "") + (values1.Count > 1 ? string.Join("\n", (IEnumerable<string>) values1) : ""));
      }
      if (this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.InfoToTapBeer != null)))
      {
        List<string> messages1 = new List<string>()
        {
          "Невозможно продать больше объема кег, которые были вскрыты для следующих товаров:\n"
        };
        List<string> messages2 = new List<string>()
        {
          "Невозможно продать следующую продукцию, так как истек срок годности:\n"
        };
        foreach (IGrouping<Guid, BasketItem> source9 in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.InfoToTapBeer != null)).GroupBy<BasketItem, Guid>((Func<BasketItem, Guid>) (x => x.InfoToTapBeer.Uid)))
        {
          InfoToTapBeer byUid = new InfoTapBeerRepository().GetByUid(source9.First<BasketItem>().InfoToTapBeer.Uid);
          Decimal? nullable1 = byUid.Quantity;
          Decimal saleQuantity = byUid.SaleQuantity;
          Decimal? nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() - saleQuantity) : new Decimal?();
          Decimal num3 = source9.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
          Decimal? nullable3;
          if (!nullable2.HasValue)
          {
            nullable1 = new Decimal?();
            nullable3 = nullable1;
          }
          else
            nullable3 = new Decimal?(nullable2.GetValueOrDefault() - num3);
          Decimal? nullable4 = nullable3;
          Decimal num4 = 0M;
          if (nullable4.GetValueOrDefault() < num4 & nullable4.HasValue)
          {
            List<string> stringList = messages1;
            string[] strArray = new string[5]
            {
              source9.First<BasketItem>().DisplayedName,
              " (",
              Translate.Basket_на_остатке__,
              null,
              null
            };
            nullable4 = byUid.Quantity;
            num4 = nullable4.GetValueOrDefault() - byUid.SaleQuantity;
            strArray[3] = num4.ToString("N2");
            strArray[4] = " л.)";
            string str = string.Concat(strArray);
            stringList.Add(str);
          }
          DateTime date = DateTime.Now.Date;
          DateTime? expirationDate = byUid.ExpirationDate;
          if ((expirationDate.HasValue ? (date > expirationDate.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            messages2.Add(source9.First<BasketItem>().DisplayedName + string.Format(" (срок годности до {0:dd.MM.yyyy});", (object) byUid.ExpirationDate));
        }
        if (messages1.Count > 1)
          return new ActionResult(ActionResult.Results.Warning, messages1);
        if (messages2.Count > 1)
          return new ActionResult(ActionResult.Results.Warning, messages2);
      }
      return new ActionResult(ActionResult.Results.Ok);
    }

    public bool UserAuthorization()
    {
      if (DevelopersHelper.IsUnitTest())
        return true;
      Gbs.Core.Config.Users users = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Users;
      (bool, Gbs.Core.Entities.Users.User) valueTuple = (true, (Gbs.Core.Entities.Users.User) null);
      if (users.RequestAuthorizationOnSale)
        valueTuple = Other.GetUserForDocument(Gbs.Core.Entities.Actions.SaleSave);
      if (!valueTuple.Item1)
        return false;
      this.User = valueTuple.Item2;
      return true;
    }

    public bool PayAcquiring(Decimal sum, Document document)
    {
      if (sum <= 0M)
        return true;
      using (AcquiringHelper acquiringHelper = new AcquiringHelper())
      {
        string rrn;
        string method;
        string approvalCode;
        string issuerName;
        string terminalId;
        string cardNumber;
        string paymentSystem;
        int num = acquiringHelper.DoPayment(sum, out rrn, out method, out approvalCode, out issuerName, out terminalId, out cardNumber, out paymentSystem) ? 1 : 0;
        if (num != 0 && document != null)
        {
          List<EntityProperties.PropertyValue> properties1 = document.Properties;
          EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
          propertyType1.Uid = GlobalDictionaries.RrnUid;
          propertyValue1.Type = propertyType1;
          propertyValue1.EntityUid = this.Document.Uid;
          propertyValue1.Value = (object) rrn;
          properties1.Add(propertyValue1);
          List<EntityProperties.PropertyValue> properties2 = document.Properties;
          EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
          propertyType2.Uid = GlobalDictionaries.TypeCardMethodUid;
          propertyValue2.Type = propertyType2;
          propertyValue2.EntityUid = this.Document.Uid;
          propertyValue2.Value = (object) method;
          properties2.Add(propertyValue2);
          List<EntityProperties.PropertyValue> properties3 = document.Properties;
          EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
          propertyType3.Uid = GlobalDictionaries.ApprovalCodeUid;
          propertyValue3.Type = propertyType3;
          propertyValue3.Value = (object) approvalCode;
          properties3.Add(propertyValue3);
          List<EntityProperties.PropertyValue> properties4 = document.Properties;
          EntityProperties.PropertyValue propertyValue4 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType4 = new EntityProperties.PropertyType();
          propertyType4.Uid = GlobalDictionaries.IssuerNameUid;
          propertyValue4.Type = propertyType4;
          propertyValue4.Value = (object) issuerName;
          properties4.Add(propertyValue4);
          List<EntityProperties.PropertyValue> properties5 = document.Properties;
          EntityProperties.PropertyValue propertyValue5 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType5 = new EntityProperties.PropertyType();
          propertyType5.Uid = GlobalDictionaries.TerminalIdUid;
          propertyValue5.Type = propertyType5;
          propertyValue5.Value = (object) terminalId;
          properties5.Add(propertyValue5);
          List<EntityProperties.PropertyValue> properties6 = document.Properties;
          EntityProperties.PropertyValue propertyValue6 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType6 = new EntityProperties.PropertyType();
          propertyType6.Uid = GlobalDictionaries.CardNumberUid;
          propertyValue6.Type = propertyType6;
          propertyValue6.Value = (object) cardNumber;
          properties6.Add(propertyValue6);
          List<EntityProperties.PropertyValue> properties7 = document.Properties;
          EntityProperties.PropertyValue propertyValue7 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType7 = new EntityProperties.PropertyType();
          propertyType7.Uid = GlobalDictionaries.PaymentSystemUid;
          propertyValue7.Type = propertyType7;
          propertyValue7.Value = (object) paymentSystem;
          properties7.Add(propertyValue7);
        }
        return num != 0;
      }
    }

    protected bool PaySBP(Decimal sum, Document document)
    {
      try
      {
        if (sum <= 0M)
          return true;
        using (SpbHelper spbHelper = new SpbHelper(sum, Convert.ToInt64(this.SaleNumber)))
        {
          string rrn;
          int num = spbHelper.Pay(sum, out rrn) ? 1 : 0;
          if (num != 0 && document != null)
          {
            List<EntityProperties.PropertyValue> properties = document.Properties;
            EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
            EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
            propertyType.Uid = GlobalDictionaries.RrnSbpUid;
            propertyValue.Type = propertyType;
            propertyValue.EntityUid = this.Document.Uid;
            propertyValue.Value = (object) rrn;
            properties.Add(propertyValue);
          }
          return num != 0;
        }
      }
      catch (AcquiringException ex)
      {
        MessageBoxHelper.Error(Translate.Basket_PaySBP_Ошибка_оплаты_продажи_по_СБП_ + "\n\n" + ex.ExtMessage);
        return false;
      }
      catch (Exception ex)
      {
        MessageBoxHelper.Error(Translate.Basket_PaySBP_ + "\n\n" + ex.Message);
        return false;
      }
    }

    protected bool PrintCheck(PreparedCheckData checkData)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SaleCardViewModel_Печать_чека);
      bool flag;
      try
      {
        Gbs.Core.Config.Devices config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        flag = new CheckPrinterHelper(config).PrintCheck(checkData.CheckData);
        this.Document.IsFiscal = ((config.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None ? 0 : (config.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? 1 : 0)) & (flag ? 1 : 0)) != 0 && this.CheckFiscalType == CheckFiscalTypes.Fiscal;
        List<EntityProperties.PropertyValue> properties1 = this.Document.Properties;
        EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
        propertyValue1.Value = (object) checkData.CheckData.FiscalNum;
        propertyValue1.EntityUid = this.Document.Uid;
        EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
        propertyType1.Uid = GlobalDictionaries.FiscalNumUid;
        propertyValue1.Type = propertyType1;
        properties1.Add(propertyValue1);
        List<EntityProperties.PropertyValue> properties2 = this.Document.Properties;
        EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
        propertyValue2.Value = (object) checkData.CheckData.FrNumber;
        propertyValue2.EntityUid = this.Document.Uid;
        EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
        propertyType2.Uid = GlobalDictionaries.FrNumber;
        propertyValue2.Type = propertyType2;
        properties2.Add(propertyValue2);
        this.Document.Properties.AddRange(checkData.CheckData.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid.IsEither<Guid>(GlobalDictionaries.ReceiptSeqPropertyUid, GlobalDictionaries.DateTimePropertyUid, GlobalDictionaries.TerminalIdPropertyUid, GlobalDictionaries.FiscalSignPropertyUid))));
        this.Document.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.EntityUid = this.Document.Uid));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка печати чека для корзины");
        flag = false;
      }
      progressBar.Close();
      return flag;
    }

    public static bool PrintPreCheck(Document document, Gbs.Core.Entities.Users.User user, CheckFiscalTypes type = CheckFiscalTypes.Fiscal)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SaleCardViewModel_Печать_чека);
      bool flag;
      try
      {
        List<CheckGood> list = document.Items.Select<Gbs.Core.Entities.Documents.Item, CheckGood>((Func<Gbs.Core.Entities.Documents.Item, CheckGood>) (x => new CheckGood(x.Good, x.SellPrice, x.Discount, x.Quantity, x.Comment, x.Good.Name)
        {
          RuFfdGoodTypeCode = GlobalDictionaries.RuFfdGoodsTypes.Payment,
          Description = x.Comment,
          RuFfdPaymentModeCode = GlobalDictionaries.RuFfdPaymentModes.Prepayment
        })).ToList<CheckGood>();
        List<CheckPayment> checkPaymentList1;
        if (document == null)
        {
          checkPaymentList1 = (List<CheckPayment>) null;
        }
        else
        {
          List<Gbs.Core.Entities.Payments.Payment> payments = document.Payments;
          checkPaymentList1 = payments != null ? payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Method != null)).Select<Gbs.Core.Entities.Payments.Payment, CheckPayment>((Func<Gbs.Core.Entities.Payments.Payment, CheckPayment>) (x => new CheckPayment()
          {
            Name = x.Method.Name,
            Method = x.Method.KkmMethod,
            Sum = x.SumIn
          })).ToList<CheckPayment>() : (List<CheckPayment>) null;
        }
        List<CheckPayment> checkPaymentList2 = checkPaymentList1;
        Cashier cashier1 = new Cashier();
        cashier1.Name = user?.Client?.Name ?? "";
        // ISSUE: explicit non-virtual call
        cashier1.UserUid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
        string str;
        if (user == null)
        {
          str = (string) null;
        }
        else
        {
          Gbs.Core.Entities.Clients.Client client = user.Client;
          str = client != null ? client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() : (string) null;
        }
        if (str == null)
          str = "";
        cashier1.Inn = str;
        Cashier cashier2 = cashier1;
        List<CheckPayment> paymentsList = checkPaymentList2;
        int fiscalType = (int) type;
        Cashier cashier3 = cashier2;
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data = new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(list, paymentsList, (CheckFiscalTypes) fiscalType, cashier3)
        {
          Number = document?.Number ?? "",
          CheckType = document.Type == GlobalDictionaries.DocumentsTypes.Sale ? CheckTypes.Sale : CheckTypes.ReturnSale,
          Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) document.Properties)
        };
        if (document.ContractorUid == Guid.Empty)
        {
          data.Client = (ClientAdnSum) null;
        }
        else
        {
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            ClientsRepository clientsRepository = new ClientsRepository(dataBase);
            data.Client = clientsRepository.GetClientByUidAndSum(document.ContractorUid);
          }
        }
        Other.ConsoleWrite(JsonConvert.SerializeObject((object) data, Formatting.Indented));
        flag = new CheckPrinterHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(data);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка печати чека для корзины");
        flag = false;
      }
      progressBar.Close();
      return flag;
    }

    protected void RemoveZeroQuantityItems()
    {
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        for (int index = this.Items.Count - 1; index >= 0; --index)
        {
          if (this.Items[index].Quantity == 0M)
            this.Items.RemoveAt(index);
        }
      }));
    }

    protected void RoundQuantity()
    {
      foreach (BasketItem basketItem in (Collection<BasketItem>) this.Items)
        basketItem.Quantity = Math.Round(basketItem.Quantity, 3, MidpointRounding.AwayFromZero);
    }

    public void SetPercentForServiceGood()
    {
      if (this._isCollectionChanged)
        return;
      if (!this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        this.Items.Clear();
      }
      else
      {
        if (this.IsDeletedPercentForServiceGood && !this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
          return;
        this.IsDeletedPercentForServiceGood = false;
        List<PercentForServiceSetting> list1 = new PercentForServiceRuleRepository().GetActiveItems().Where<PercentForServiceSetting>((Func<PercentForServiceSetting, bool>) (x => !x.IsOff)).ToList<PercentForServiceSetting>();
        Decimal num1 = 0M;
        foreach (BasketItem basketItem in this.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
        {
          BasketItem item = basketItem;
          List<PercentForServiceSetting> list2 = list1.Where<PercentForServiceSetting>((Func<PercentForServiceSetting, bool>) (x => x.ListGroup.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid)) && !x.IsOff)).ToList<PercentForServiceSetting>();
          Decimal num2 = list2.Any<PercentForServiceSetting>() ? list2.Max<PercentForServiceSetting>((Func<PercentForServiceSetting, Decimal>) (x => x.Percent)) : 0M;
          num1 += Math.Round(item.TotalSum * num2 / 100M, 2);
        }
        if (num1 == 0M)
        {
          if (!this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
            return;
          this.Items.Remove(this.Items.Single<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)));
        }
        else if (!this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
        {
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            BasketItem basketItem1 = new BasketItem();
            basketItem1.Good = new GoodRepository(dataBase).GetByUid(GlobalDictionaries.PercentForServiceGoodUid);
            basketItem1.SalePrice = num1;
            basketItem1.Quantity = 1M;
            basketItem1.Storage = this.Storage;
            basketItem1.IsPriceEditByUser = true;
            basketItem1.Comment = string.Empty;
            BasketItem basketItem2 = basketItem1;
            basketItem2.Good.Group.RuTaxSystem = this.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)).Good.Group.RuTaxSystem;
            this.Items.Add(basketItem2);
          }
        }
        else
        {
          this.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).SalePrice = num1;
          this.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).Quantity = 1M;
        }
      }
    }

    private void UpdateProperties()
    {
      this.OnPropertyChanged("ChangeSum");
      this.OnPropertyChanged("TotalQuantity");
      this.OnPropertyChanged("TotalDiscount");
      this.OnPropertyChanged("TotalSum");
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged("VisibilityInfoCredit");
      this.OnPropertyChanged("PrepaidPaymentsSum");
      this.OnPropertyChanged("VisibilitySumPrepaid");
      this.OnPropertyChanged("KkmCheckCorrection");
      this.OnPropertyChanged("CheckDiscountVisibility");
    }

    public override void ReCalcTotals()
    {
      this._recalcTimer.Stop();
      this._recalcTimer.Start();
    }

    public Visibility CheckDiscountVisibility
    {
      get => !(this.KkmCheckCorrection != 0M) ? Visibility.Collapsed : Visibility.Visible;
    }

    public void CheckRuTaxSystem(BasketItem newItem)
    {
      List<Gbs.Core.Entities.Goods.Good> list = this.Items.Select<BasketItem, Gbs.Core.Entities.Goods.Good>((Func<BasketItem, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>();
      list.Add(newItem.Good);
      int taxSystemForGoods = (int) Gbs.Core.Devices.CheckPrinters.CheckData.CheckData.GetRuTaxSystemForGoods(list);
    }

    public virtual ActionResult Add(BasketItem item, bool checkMinus = true, bool isWeightPlu = false)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (item.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid && this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
      {
        MessageBoxHelper.Warning(string.Format(Translate.Basket_Услуга___0___уже_добавлена_в_чек__повторно_сделать_это_невозможно_, (object) this.Items.Single<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).DisplayedName));
        return new ActionResult(ActionResult.Results.Ok);
      }
      LogHelper.Debug("В чек добавлена позиция " + item.ToJsonString(true));
      if (this.Storage != null && this.Storage.Uid != item.Storage.Uid)
        return new ActionResult(ActionResult.Results.Error, Translate.Basket_Add_В_одном_документе_могут_быть_товары_только_из_одного_склада_);
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      if (devices.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
      {
        if (settings.Interface.Country == GlobalDictionaries.Countries.Russia)
        {
          try
          {
            this.CheckRuTaxSystem(item);
          }
          catch (Exception ex)
          {
            return new ActionResult(ActionResult.Results.Error, ex.Message);
          }
        }
      }
      this.Storage = item.Storage;
      this.Items.Any<BasketItem>();
      this.BasketExtraPricer.SetPriceForItem(item);
      if (item.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Weight) || item.Good.Group.IsRequestCount)
      {
        bool isEnableCount = true;
        List<GoodsUnits.GoodUnit> source = CachesBox.AllGoodsUnits();
        if (EgaisHelper.GetAlcoholType(item.Good) != EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)
        {
          if (!item.Good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol))
          {
            GoodsUnits.GoodUnit goodUnit = source.SingleOrDefault<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (u => item.Good.Group.UnitsUid == u.Uid));
            if ((goodUnit != null ? goodUnit.RuFfdUnitsIndex : 0) != 0)
              goto label_15;
          }
          else
            goto label_15;
        }
        isEnableCount = item.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Tobacco;
label_15:
        if (!isWeightPlu)
        {
          if (!this.EditQuantity((object) new List<BasketItem>()
          {
            item
          }, false, isEnableCount, false))
            return new ActionResult(ActionResult.Results.Cancel);
          if (item.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            item.SalePrice = item.BasePrice;
        }
      }
      this.AddItem(item);
      if (item.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)
      {
        GoodGroups.Group group = item.Good.Group;
        BasketItem basketItem = this.Items.FirstOrDefault<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid));
        // ISSUE: explicit non-virtual call
        int num = basketItem != null ? (int) __nonvirtual (basketItem.Good).Group.RuTaxSystem : (int) new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.DefaultRuTaxSystem;
        group.RuTaxSystem = (GlobalDictionaries.RuTaxSystems) num;
      }
      this.OnPropertyChanged("Items");
      this.ReCalcTotals();
      this.UnionItems(item);
      this.SendDisplayBuyerInfo(this.SelectedItem);
      return new ActionResult(ActionResult.Results.Ok);
    }

    private void UnionItems(BasketItem item)
    {
      if (this.DocumentsType == GlobalDictionaries.DocumentsTypes.ClientOrder && !new ConfigsRepository<Gbs.Core.Config.Settings>().Get().ClientOrder.IsUnitePositions)
        return;
      if (item.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Weight) || EgaisHelper.GetAlcoholType(item.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol || item.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None || !this.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => this.Predicate(x, item) && x.Uid != item.Uid)))
        return;
      BasketItem basketItem1 = this.Items.First<BasketItem>((Func<BasketItem, bool>) (x => this.Predicate(x, item) && x.Uid != item.Uid));
      this.Items.Remove(basketItem1);
      this.Items.Remove(item);
      BasketItem basketItem2 = basketItem1;
      basketItem2.Quantity = basketItem2.Quantity + item.Quantity;
      this.Items.Add(basketItem1);
      this.SelectedItem = basketItem1;
    }

    public void AddItem(BasketItem item, bool isNeedComment = true)
    {
      this.Items.Add(item);
      this.SelectedItem = item;
      if (((item.Good.Group.NeedComment || item.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None ? 1 : (EgaisHelper.GetAlcoholType(item.Good) != 0 ? 1 : 0)) & (isNeedComment ? 1 : 0)) == 0 || !this.IsNeedComment)
        return;
      this.EditComment((object) new List<BasketItem>()
      {
        item
      }, false);
    }

    private bool Predicate(BasketItem x, BasketItem item)
    {
      if (x.SalePrice == item.SalePrice && x.BasePrice == item.BasePrice && x.Good.Uid == item.Good.Uid && x.Discount.Value == item.Discount.Value)
      {
        Guid? uid1 = x.GoodModification?.Uid;
        Guid? uid2 = item.GoodModification?.Uid;
        if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() == uid2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
          return x.Comment == item.Comment;
      }
      return false;
    }

    public void AddActionsHistoryByCancel(Gbs.Core.Entities.Users.User authUser)
    {
      string str = Translate.ОтменаЧека + "\n\r\n\r";
      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateTextHistory(!this.Items.Any<BasketItem>() ? str + Translate.Basket_AddActionsHistoryByCancel_Корзина_была_пустая : str + Translate.СодержимоеЧека + "\n\r" + string.Join("\n\r", (IEnumerable<string>) this.Items.Select<BasketItem, string>((Func<BasketItem, string>) (x => x.DisplayedName)).ToList<string>()), GlobalDictionaries.EntityTypes.Document, authUser), false);
    }

    public enum CustomColors
    {
      None,
      Red,
      Green,
      Orange,
    }

    public class BasketPayCertificate
    {
      public GoodsCertificate.Certificate Certificate { get; set; }

      public Decimal Nominal { get; private set; }

      public BasketPayCertificate(GoodsCertificate.Certificate certificate)
      {
        this.Certificate = certificate;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          this.Nominal = Convert.ToDecimal(EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Good, dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == certificate.Stock.GoodUid && x.TYPE_UID == GlobalDictionaries.CertificateNominalUid))).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid && x.EntityUid == certificate.Stock.GoodUid))?.Value);
      }
    }
  }
}
