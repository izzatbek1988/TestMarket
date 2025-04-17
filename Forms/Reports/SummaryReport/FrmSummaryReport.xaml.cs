// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.SummaryReportViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms.ActionsPayments;
using Gbs.Forms.Clients;
using Gbs.Forms.Reports.SummaryReport.Other;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport
{
  public partial class SummaryReportViewModel : ViewModelWithForm
  {
    private CancellationTokenSource _CTS = new CancellationTokenSource();
    private List<Task> GetDataTasks = new List<Task>();
    private Decimal _profitSumGroups;
    private DateTime _valueDateTimeStart;
    private DateTime _valueDateTimeEnd;
    private SolidColorBrush GreenBrush = (SolidColorBrush) Application.Current.Resources[(object) "ButtonOkBackgroundColor"];
    private SolidColorBrush RedBrush = (SolidColorBrush) Application.Current.Resources[(object) "ButtonCancelBackgroundColor"];

    public bool IsEnabledOtherData { get; set; }

    public ICommand PrintReportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForSummaryReport(new Gbs.Helpers.FR.BackEnd.Entities.SummaryReport()
        {
          DateFinish = this.ValueDateTimeEnd,
          DateStart = this.ValueDateTimeStart,
          MoneyIncomeSum = this.PaymentInsertSum,
          MoneyOutcomeSum = this.PaymentRemoveSum,
          TotalGoods = this.GoodTotalCount,
          SumCash = this.SumCash,
          TotalSaleSum = this.SaleItemsSum,
          TotalReturnsSum = this.ReturnItemsSum,
          TotalReturnCount = this.ReturnCount,
          TotalSalesCount = this.SaleCount,
          IncomeSum = this.IncomeSum,
          DiscountsSum = this.DiscountSum,
          TotalCreditSum = this.TotalCreditSum,
          CreditPaymentsSum = this.CreditPaymentsSum,
          Payments = this.ListPaymentsPaid.GroupBy<Gbs.Core.Entities.Payments.Payment, Guid>((Func<Gbs.Core.Entities.Payments.Payment, Guid>) (x => x.Method.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, CheckPayment>((Func<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, CheckPayment>) (x => new CheckPayment()
          {
            Name = x.First<Gbs.Core.Entities.Payments.Payment>().Method.Name,
            Sum = x.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (s => s.SumIn - s.SumOut))
          })).ToList<CheckPayment>()
        }), this.AuthUser)));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public ICommand UpdateInfoReport { get; set; }

    public ICommand ReloadInfoReport { get; set; }

    public ICommand RemoveCash { get; set; }

    public ICommand DepositСash { get; set; }

    public ICommand SendСash { get; set; }

    public ICommand UpdateSumCash { get; set; }

    public ICommand ShowSaleJournal { get; set; }

    public ICommand ShowReturnJournal { get; set; }

    public ICommand ShowTablePayment { get; set; }

    public ICommand ShowTotalBalance
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new PaymentAccountsAndSumViewModel().ShowSumAccounts(this.AuthUser);
          this.ReloadListPayments();
          this.GetBalanceTotalSum();
          this.SetValueBalanceCash();
          this.SetValueSumCash();
        }));
      }
    }

    public ICommand CorrectBalance
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmRemoveCash().CorrectBalance(this.BalanceCash, this.AuthUser);
          this.ReloadListPayments();
          this.SetValueBalanceCash();
        }));
      }
    }

    public ICommand ShowLogCorrectCash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new LogReCalcCashViewModel().ShowLog(this.ListPayments, true, this.ValueDateTimeStart, this.ValueDateTimeEnd)));
      }
    }

    public ICommand ShowLogCorrectBalance
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new LogReCalcCashViewModel().ShowLog(this.ListPayments, false, this.ValueDateTimeStart, this.ValueDateTimeEnd)));
      }
    }

    private void ReloadListPayments()
    {
      Performancer performancer = new Performancer("Загрузка платежей в сводном отчете");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.ListPayments = Gbs.Core.Entities.Payments.GetPaymentsList(dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED)));
        performancer.Stop();
      }
    }

    public ICommand ShowInsertList { get; set; }

    public ICommand ShowRemoveList { get; set; }

    public ICommand ShowCreditList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new CreditListViewModel().ShowCreditList(authUser: this.AuthUser, dateStart: this.ValueDateTimeStart, dateFinish: this.ValueDateTimeEnd)));
      }
    }

    private List<Gbs.Core.Entities.Documents.Document> ListDocuments { get; set; }

    private List<Gbs.Core.Entities.Documents.Document> ListLastDocuments { get; set; }

    private List<Gbs.Core.Entities.Payments.Payment> ListPayments { get; set; } = new List<Gbs.Core.Entities.Payments.Payment>();

    private List<Gbs.Core.Entities.Payments.Payment> ListPaymentsPaid { get; set; }

    private List<Gbs.Core.Entities.Payments.Payment> ListLastPaymentsPaid { get; set; }

    public SummaryReportViewModel()
    {
    }

    public SummaryReportViewModel(Gbs.Core.Entities.Users.User authUser)
    {
      this.AuthUser = authUser;
      this.ShowSaleJournal = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        SaleJournalViewModel journalViewModel = new SaleJournalViewModel();
        DateTime dateTime = this.ValueDateTimeStart;
        DateTime date1 = dateTime.Date;
        dateTime = this.ValueDateTimeEnd;
        DateTime date2 = dateTime.Date;
        Gbs.Core.Entities.Users.User authUser1 = this.AuthUser;
        Guid uidForm = Guid.NewGuid();
        journalViewModel.ShowCard(date1, date2, true, user: authUser1, uidForm: uidForm);
      }));
      this.ReloadInfoReport = (ICommand) new RelayCommand((Action<object>) (obj => this.GetInfoForReport()));
      this.UpdateInfoReport = (ICommand) new RelayCommand((Action<object>) (obj => this.GetInfoForReport()));
      this.RemoveCash = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
        new FrmRemoveCash().RemoveCash(ref payment, user: this.AuthUser);
        this.GetInfoForReport();
      }));
      this.DepositСash = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        new FrmRemoveCash().InsertCash(user: this.AuthUser);
        this.GetInfoForReport();
      }));
      this.SendСash = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        new FrmRemoveCash().SendCash(this.AuthUser);
        this.GetInfoForReport();
      }));
      this.ShowReturnJournal = (ICommand) new RelayCommand((Action<object>) (obj => new ReturnListViewModel().ShowListReturn(this.ValueDateTimeStart, this.ValueDateTimeEnd)));
      this.ShowTablePayment = (ICommand) new RelayCommand((Action<object>) (obj => new FrmPaymentByMethods().ShowDataPayment(this.ListPaymentsPaid)));
      this.UpdateSumCash = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        ReCalcCashAccountHelper.DoReCalcCashAccount(true, this.AuthUser);
        this.GetInfoForReport();
      }));
      this.ShowInsertList = (ICommand) new RelayCommand((Action<object>) (obj => new FrmListPaymentsActions().ShowInsertList(this.ValueDateTimeStart, this.ValueDateTimeEnd, this.AuthUser)));
      this.ShowRemoveList = (ICommand) new RelayCommand((Action<object>) (obj => new FrmListPaymentsActions().ShowRemoveList(this.ValueDateTimeStart, this.ValueDateTimeEnd, this.AuthUser)));
    }

    private bool LoadingInProgress { get; set; }

    private bool NeedRepeat { get; set; }

    private List<Gbs.Core.Entities.Documents.Document> _setChildList { get; set; } = new List<Gbs.Core.Entities.Documents.Document>();

    private void GetInfoForReport()
    {
      Performancer per = new Performancer("Загрука данных для сводного отчета");
      if (this.LoadingInProgress)
      {
        this._CTS.Cancel();
        this._CTS.Dispose();
        this._CTS = new CancellationTokenSource();
        this.NeedRepeat = true;
      }
      else
      {
        this.LoadingInProgress = true;
        this.GetKkmSum();
        TaskHelper.TaskRun((Action) (() =>
        {
          foreach (Task getDataTask in this.GetDataTasks)
            getDataTask.Wait();
          this.GetDataTasks = new List<Task>();
          this.ProgressBarVisibility = Visibility.Visible;
          this.OnPropertyChanged("ProgressBarVisibility");
          this.ClearData();
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            this.ReloadListPayments();
            DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
            this.ListDocuments = documentsRepository.GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
            {
              DateStart = this.ValueDateTimeStart,
              DateEnd = this.ValueDateTimeEnd,
              IncludeDeleted = false,
              IgnoreTime = true,
              Types = new GlobalDictionaries.DocumentsTypes[2]
              {
                GlobalDictionaries.DocumentsTypes.Sale,
                GlobalDictionaries.DocumentsTypes.SaleReturn
              }
            });
            this._setChildList = documentsRepository.GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
            {
              DateStart = DateTime.MinValue,
              DateEnd = DateTime.Now,
              IncludeDeleted = false,
              IgnoreTime = true,
              Types = new GlobalDictionaries.DocumentsTypes[5]
              {
                GlobalDictionaries.DocumentsTypes.ProductionSet,
                GlobalDictionaries.DocumentsTypes.ProductionItem,
                GlobalDictionaries.DocumentsTypes.BeerProductionSet,
                GlobalDictionaries.DocumentsTypes.BeerProductionItem,
                GlobalDictionaries.DocumentsTypes.SetChildStockChange
              }
            });
            if (this._CTS.Token.IsCancellationRequested)
              return;
            this.GetDataTasks.AddRange((IEnumerable<Task>) new Task[6]
            {
              new Task((Action) (() => this.GetCountAndInfoSale(this.ValueDateTimeStart, this.ValueDateTimeEnd, this.ListDocuments, false)), this._CTS.Token),
              new Task(new Action(this.SetValueSumCash), this._CTS.Token),
              new Task((Action) (() => this.SetValueSumPayments(this.ValueDateTimeStart, this.ValueDateTimeEnd, false)), this._CTS.Token),
              new Task((Action) (() =>
              {
                Decimal incomeSum = 0M;
                this.GetProfitSum(this.ListDocuments, this.ListPayments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
                {
                  DateTime dateTime3 = x.Date;
                  DateTime date5 = dateTime3.Date;
                  dateTime3 = this.ValueDateTimeEnd;
                  DateTime date6 = dateTime3.Date;
                  if (!(date5 <= date6))
                    return false;
                  DateTime dateTime4 = x.Date;
                  DateTime date7 = dateTime4.Date;
                  dateTime4 = this.ValueDateTimeStart;
                  DateTime date8 = dateTime4.Date;
                  return date7 >= date8;
                })).ToList<Gbs.Core.Entities.Payments.Payment>(), ref incomeSum);
                this.IncomeSum = incomeSum;
              }), this._CTS.Token),
              new Task(new Action(this.GetBalanceTotalSum), this._CTS.Token),
              new Task(new Action(this.SetValueBalanceCash), this._CTS.Token)
            });
            if (this.ListDocuments.Any<Gbs.Core.Entities.Documents.Document>())
            {
              double totalDays = (this.ValueDateTimeEnd - this.ValueDateTimeStart).TotalDays;
              DateTime finish = this.ValueDateTimeStart.AddDays(-1.0);
              DateTime start = finish.AddDays(-totalDays);
              this.ListLastDocuments = documentsRepository.GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
              {
                DateStart = start,
                DateEnd = finish,
                IncludeDeleted = false,
                IgnoreTime = true,
                Types = new GlobalDictionaries.DocumentsTypes[2]
                {
                  GlobalDictionaries.DocumentsTypes.Sale,
                  GlobalDictionaries.DocumentsTypes.SaleReturn
                }
              });
              if (this._CTS.Token.IsCancellationRequested)
                return;
              this.GetDataTasks.AddRange((IEnumerable<Task>) new Task[3]
              {
                new Task((Action) (() => this.GetCountAndInfoSale(start, finish, this.ListLastDocuments, true)), this._CTS.Token),
                new Task((Action) (() => this.SetValueSumPayments(start, finish, true)), this._CTS.Token),
                new Task((Action) (() =>
                {
                  Decimal incomeSum = 0M;
                  this.GetProfitSum(this.ListLastDocuments, this.ListPayments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Date.Date <= finish.Date && x.Date.Date >= start.Date)).ToList<Gbs.Core.Entities.Payments.Payment>(), ref incomeSum);
                  this.LastIncomeSum = incomeSum;
                }), this._CTS.Token)
              });
            }
            this.GetDataTasks.RunList(true);
            this.LoadingInProgress = false;
            this.GetDataTasks = new List<Task>();
            this.ProgressBarVisibility = Visibility.Hidden;
            this.OnPropertyChanged(isUpdateAllProp: true);
            if (this.NeedRepeat)
            {
              this.NeedRepeat = false;
              Application.Current.Dispatcher.Invoke(new Action(this.GetInfoForReport));
            }
            per.Stop();
          }
        }));
      }
    }

    private void ClearData()
    {
      this.SaleCount = 0M;
      this.ReturnCount = 0M;
      this.MiddleCheck = 0M;
      this.SaleItemsSum = 0M;
      this.SalesTotalSum = 0M;
      this.DiscountSum = 0M;
      this.GoodTotalCount = 0M;
      this.PaymentsSum = 0M;
      this.CreditPaymentsSum = 0M;
      this.ReturnItemsSum = 0M;
      this.ReturnItemsCount = 0M;
      this.PaymentRemoveSum = 0M;
      this.PaymentInsertSum = 0M;
      this.IncomeSum = 0M;
      this.TotalCreditSum = 0M;
      this.LastSaleCount = 0M;
      this.LastReturnCount = 0M;
      this.LastMiddleCheck = 0M;
      this.LastSaleItemsSum = 0M;
      this.LastSalesTotalSum = 0M;
      this.LastDiscountSum = 0M;
      this.LastGoodTotalCount = 0M;
      this.LastPaymentsSum = 0M;
      this.LastReturnItemsSum = 0M;
      this.LastReturnItemsCount = 0M;
      this.LastPaymentRemoveSum = 0M;
      this.LastPaymentInsertSum = 0M;
      this.LastIncomeSum = 0M;
    }

    private void GetKkmSum()
    {
      try
      {
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        if (!(devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm & devicesConfig.CheckPrinter.FiscalKkm.KkmType != 0))
          return;
        if (devicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas, GlobalDictionaries.Devices.FiscalKkmTypes.FsPRRO, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Devices.FiscalKkmTypes.UzPos, GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm))
          return;
        this.VisibilitySumKkm = Visibility.Visible;
        KkmHelper.UserUid = this.AuthUser.Uid;
        using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
        {
          Decimal sum;
          kkmHelper.GetCashSum(out sum);
          this.SumCashKkm = sum;
          this.OnPropertyChanged("SumCashKkm");
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error((Exception) new ErrorHelper.GbsException(Translate.SellerReportViewModel_SetSumCashKkm_Не_удалось_получить_сумму_в_ККМ_для_отображения_в_отчете_, ex), "");
      }
    }

    private void GetBalanceTotalSum()
    {
      IEnumerable<PaymentsAccounts.PaymentsAccount> paymentsAccounts = PaymentsAccounts.GetPaymentsAccountsList().Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted));
      List<Gbs.Core.Entities.Payments.Payment> list = Gbs.Core.Entities.Payments.GetPaymentsList().Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted && x.Type != GlobalDictionaries.PaymentTypes.Prepaid)).ToList<Gbs.Core.Entities.Payments.Payment>();
      if (this._CTS.Token.IsCancellationRequested)
        return;
      Decimal num1 = 0M;
      foreach (PaymentsAccounts.PaymentsAccount paymentsAccount in paymentsAccounts)
      {
        PaymentsAccounts.PaymentsAccount account = paymentsAccount;
        if (this._CTS.Token.IsCancellationRequested)
          return;
        Decimal num2 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
        {
          Guid? uid1 = x.AccountIn?.Uid;
          Guid uid2 = account.Uid;
          return uid1.HasValue && uid1.GetValueOrDefault() == uid2;
        })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)) - list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
        {
          Guid? uid3 = x.AccountOut?.Uid;
          Guid uid4 = account.Uid;
          return uid3.HasValue && uid3.GetValueOrDefault() == uid4;
        })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        num1 += num2;
      }
      this.TotalBalanceSum = num1;
      this.OnPropertyChanged("TotalBalanceSum");
    }

    private void GetProfitSum(
      List<Gbs.Core.Entities.Documents.Document> documents,
      List<Gbs.Core.Entities.Payments.Payment> payments,
      ref Decimal incomeSum)
    {
      Performancer performancer = new Performancer("Подсчет прибыли для сводного отчета");
      if (!documents.Any<Gbs.Core.Entities.Documents.Document>() && !payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment)) || this._CTS.Token.IsCancellationRequested)
        return;
      List<Gbs.Core.Entities.Documents.Document> documents1 = new List<Gbs.Core.Entities.Documents.Document>((IEnumerable<Gbs.Core.Entities.Documents.Document>) documents);
      documents1.AddRange((IEnumerable<Gbs.Core.Entities.Documents.Document>) this._setChildList);
      List<PaymentGroups.PaymentGroup> paymentGroupsList = PaymentGroups.GetPaymentGroupsList();
      Decimal profitSumGroups;
      incomeSum = SaleHelper.GetProfitSum(documents1, payments, paymentGroupsList, out profitSumGroups, this._CTS);
      this._profitSumGroups = profitSumGroups;
      this.OnPropertyChanged(isUpdateAllProp: true);
      performancer.Stop();
    }

    private void GetCountAndInfoSale(
      DateTime start,
      DateTime finish,
      List<Gbs.Core.Entities.Documents.Document> documents,
      bool isLastData)
    {
      Performancer performancer = new Performancer("Расчет кол-ва и информации о продажах для сводного отчета");
      this.IsEnabledOtherData = false;
      this.OnPropertyChanged("IsEnabledOtherData");
      if (this._CTS.Token.IsCancellationRequested)
        return;
      int num1 = documents.Count<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale));
      int num2 = documents.Count<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn));
      if (isLastData)
      {
        this.LastSaleCount = (Decimal) num1;
        this.LastReturnCount = (Decimal) num2;
      }
      else
      {
        this.SaleCount = (Decimal) num1;
        this.ReturnCount = (Decimal) num2;
      }
      List<Gbs.Core.Entities.Payments.Payment> listPaid = this.ListPayments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
      {
        if (!(x.Date.Date >= start.Date) || !(x.Date.Date <= finish.Date))
          return false;
        return x.Type == GlobalDictionaries.PaymentTypes.MoneyDocumentPayment || x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment;
      })).ToList<Gbs.Core.Entities.Payments.Payment>();
      if (isLastData)
        this.ListLastPaymentsPaid = listPaid;
      else
        this.ListPaymentsPaid = listPaid;
      CancellationToken token = this._CTS.Token;
      if (token.IsCancellationRequested)
        return;
      Decimal num3 = listPaid.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      if (isLastData)
        this.LastPaymentsSum = num3;
      else
        this.PaymentsSum = num3;
      performancer.AddPoint("point 10");
      TaskHelper.TaskRun((Action) (() =>
      {
        foreach (Gbs.Core.Entities.Payments.Payment payment in listPaid.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyDocumentPayment)).AsParallel<Gbs.Core.Entities.Payments.Payment>())
        {
          Gbs.Core.Entities.Payments.Payment x = payment;
          if (isLastData || this._CTS.Token.IsCancellationRequested)
            break;
          Gbs.Core.Entities.Documents.Document document = documents.FirstOrDefault<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (d => d.Uid == x.ParentUid));
          if (Math.Abs((x.Date - (document != null ? document.DateTime : new DateTime(1, 1, 1))).TotalMilliseconds) >= 60000.0)
          {
            if (!isLastData)
              this.CreditPaymentsSum += x.SumIn;
            this.OnPropertyChanged("CreditPaymentsSum");
          }
        }
      }));
      performancer.AddPoint("point 20");
      foreach (Gbs.Core.Entities.Documents.Document document in documents)
      {
        token = this._CTS.Token;
        if (token.IsCancellationRequested)
          return;
        Decimal num4 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted && x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        switch (document.Type)
        {
          case GlobalDictionaries.DocumentsTypes.Sale:
            if (isLastData)
            {
              this.LastSalesTotalSum += SaleHelper.GetSumDocument(document);
              this.LastSaleItemsSum += SaleHelper.GetSumDocumentLessBonuses(document) - num4;
              this.LastDiscountSum += SaleHelper.GetSumDiscountDocument(document) + num4;
              this.LastGoodTotalCount += document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              break;
            }
            this.SalesTotalSum += SaleHelper.GetSumDocument(document);
            this.SaleItemsSum += SaleHelper.GetSumDocumentLessBonuses(document) - num4;
            this.DiscountSum += SaleHelper.GetSumDiscountDocument(document) + num4;
            this.GoodTotalCount += document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            break;
          case GlobalDictionaries.DocumentsTypes.SaleReturn:
            if (isLastData)
            {
              this.LastReturnItemsSum += SaleHelper.GetSumDocument(document);
              this.LastReturnItemsCount += document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.LastSaleItemsSum -= SaleHelper.GetSumDocumentLessBonuses(document) + num4;
              this.LastDiscountSum -= SaleHelper.GetSumDiscountDocument(document) - num4;
              break;
            }
            this.ReturnItemsSum += SaleHelper.GetSumDocument(document);
            this.ReturnItemsCount += document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.SaleItemsSum -= SaleHelper.GetSumDocumentLessBonuses(document) + num4;
            this.DiscountSum -= SaleHelper.GetSumDiscountDocument(document) - num4;
            break;
        }
        Thread.Sleep(0);
        token = this._CTS.Token;
        if (token.IsCancellationRequested)
          return;
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
      performancer.AddPoint("point 30");
      if (this._CTS.Token.IsCancellationRequested)
        return;
      if (num1 != 0)
      {
        if (isLastData)
          this.LastMiddleCheck = !(this.LastSaleCount == 0M) ? Math.Round(this.LastSalesTotalSum / this.LastSaleCount, 2, MidpointRounding.AwayFromZero) : 0M;
        else
          this.MiddleCheck = !(this.SaleCount == 0M) ? Math.Round(this.SalesTotalSum / this.SaleCount, 2, MidpointRounding.AwayFromZero) : 0M;
      }
      performancer.AddPoint("point 40");
      if (!isLastData)
      {
        List<Gbs.Core.Entities.Documents.Document> list1 = documents.Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale)).ToList<Gbs.Core.Entities.Documents.Document>();
        List<Gbs.Core.Entities.Documents.Document> salesAndReturn = documents.Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn))).ToList<Gbs.Core.Entities.Documents.Document>();
        performancer.AddPoint("point 50");
        ConcurrentBag<Decimal> sumsBag = new ConcurrentBag<Decimal>();
        List<Task> list2 = new List<Task>();
        foreach (IEnumerable<Gbs.Core.Entities.Documents.Document> chunk1 in list1.ToChunks<Gbs.Core.Entities.Documents.Document>(10))
        {
          IEnumerable<Gbs.Core.Entities.Documents.Document> chunk = chunk1;
          Task task = new Task((Action) (() => sumsBag.Add(chunk.Sum<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, Decimal>) (x => ClientsRepository.GetCreditSumForDocuments(x, salesAndReturn, true))))));
          list2.Add(task);
        }
        list2.RunList(true);
        this.TotalCreditSum = sumsBag.Sum();
      }
      this.IsEnabledOtherData = true;
      this.OnPropertyChanged(isUpdateAllProp: true);
      performancer.Stop();
    }

    private void SetValueSumCash()
    {
      if (this._CTS.Token.IsCancellationRequested)
        return;
      (this.SumCash, this.CurrentSumCash) = SaleHelper.GetAllSumCash();
      this.OnPropertyChanged("SumCash");
      this.OnPropertyChanged("CurrentSumCash");
    }

    private void SetValueBalanceCash()
    {
      if (this._CTS.Token.IsCancellationRequested)
        return;
      List<Gbs.Core.Entities.Payments.Payment> list = this.ListPayments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
      {
        if (!x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.RecountSumCash, GlobalDictionaries.PaymentTypes.BalanceCorrection))
          return false;
        Sections.Section section = x.Section;
        // ISSUE: explicit non-virtual call
        return (section != null ? __nonvirtual (section.Uid) : Guid.Empty) == Sections.GetCurrentSection().Uid;
      })).ToList<Gbs.Core.Entities.Payments.Payment>();
      if (list.Any<Gbs.Core.Entities.Payments.Payment>())
        this.BalanceCash = list.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      this.OnPropertyChanged("BalanceCash");
    }

    private void SetValueSumPayments(DateTime start, DateTime finish, bool isLastData)
    {
      Performancer performancer = new Performancer(string.Format("Загрузка сумм платежей для cводного отчета за период {0:dd.MM.yyyy} - {1:dd.MM.yyyy}", (object) start, (object) finish));
      List<Gbs.Core.Entities.Payments.Payment> list = this.ListPayments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Date.Date >= start.Date && x.Date.Date <= finish.Date)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.AccountOut != null || x.AccountIn != null)).ToList<Gbs.Core.Entities.Payments.Payment>();
      Decimal num1 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn == 0M)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
      Decimal num2 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn != 0M)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
      if (isLastData)
      {
        this.LastPaymentRemoveSum = num1;
        this.LastPaymentInsertSum = num2;
      }
      else
      {
        this.PaymentRemoveSum = num1;
        this.PaymentInsertSum = num2;
      }
      this.OnPropertyChanged("LastPaymentRemoveSum");
      this.OnPropertyChanged("LastPaymentInsertSum");
      this.OnPropertyChanged("PaymentInsertSum");
      this.OnPropertyChanged("PaymentRemoveSum");
      performancer.Stop();
    }

    public Visibility VisibilityDate { get; set; } = Visibility.Collapsed;

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set
      {
        this._valueDateTimeStart = value;
        this.OnPropertyChanged(nameof (ValueDateTimeStart));
      }
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set
      {
        this._valueDateTimeEnd = value;
        this.OnPropertyChanged(nameof (ValueDateTimeEnd));
      }
    }

    public Decimal SumCash { get; set; }

    public Decimal CurrentSumCash { get; set; }

    public Decimal BalanceCash { get; set; }

    public Decimal SumCashKkm { get; set; }

    public Visibility VisibilitySumKkm { get; set; } = Visibility.Collapsed;

    private Decimal LastSaleCount { get; set; }

    public Decimal SaleCount { get; set; }

    public Decimal SaleItemsSum { get; set; }

    public Decimal LastSaleItemsSum { get; set; }

    public Decimal SalesTotalSum { get; set; }

    private Decimal LastSalesTotalSum { get; set; }

    public Decimal DiscountSum { get; set; }

    private Decimal LastDiscountSum { get; set; }

    public Decimal MiddleCheck { get; set; }

    private Decimal LastMiddleCheck { get; set; }

    public Decimal IncomeSum { get; set; }

    private Decimal LastIncomeSum { get; set; }

    public Decimal ReturnCount { get; set; }

    private Decimal LastReturnCount { get; set; }

    public Decimal ReturnItemsSum { get; set; }

    private Decimal LastReturnItemsSum { get; set; }

    public Decimal ReturnItemsCount { get; set; }

    private Decimal LastReturnItemsCount { get; set; }

    public Decimal GoodTotalCount { get; set; }

    private Decimal LastGoodTotalCount { get; set; }

    public Decimal PaymentsSum { get; set; }

    private Decimal LastPaymentsSum { get; set; }

    public Decimal TotalBalanceSum { get; set; }

    private Decimal LastTotalBalanceSum { get; set; }

    public Decimal CreditPaymentsSum { get; set; }

    public Decimal TotalCreditSum { get; set; }

    public Decimal PaymentRemoveSum { get; set; }

    private Decimal LastPaymentRemoveSum { get; set; }

    public Decimal PaymentInsertSum { get; set; }

    private Decimal LastPaymentInsertSum { get; set; }

    public Visibility ProgressBarVisibility { get; set; }

    public string ProfitPercent
    {
      get
      {
        return this.IncomeSum == 0M || this.SaleItemsSum == 0M || this.SaleItemsSum == 0M ? string.Empty : " | " + ((this.IncomeSum - this._profitSumGroups) / this.SaleItemsSum * 100.0M).ToString("N1") + "%";
      }
    }

    public Brush SaleItemsSumBrush
    {
      get
      {
        return !(this.SaleItemsSum > this.LastSaleItemsSum) ? (Brush) this.RedBrush : (Brush) this.GreenBrush;
      }
    }

    public Brush IncomeSumBrush
    {
      get
      {
        return !(this.IncomeSum > this.LastIncomeSum) ? (Brush) this.RedBrush : (Brush) this.GreenBrush;
      }
    }

    public Brush AvgCheckBrush
    {
      get
      {
        return !(this.MiddleCheck > this.LastMiddleCheck) ? (Brush) this.RedBrush : (Brush) this.GreenBrush;
      }
    }

    public string AvgCheckPercent => this.GetStrPercent(this.MiddleCheck, this.LastMiddleCheck);

    public string SaleItemsSumPercent
    {
      get => this.GetStrPercent(this.SaleItemsSum, this.LastSaleItemsSum);
    }

    public string IncomeSumPercent => this.GetStrPercent(this.IncomeSum, this.LastIncomeSum);

    private string GetStrPercent(Decimal newSum, Decimal oldSum)
    {
      if (oldSum == 0M)
        return "";
      Decimal num = (newSum - oldSum) / oldSum * 100.0M;
      string str = num.ToString("N1");
      return num > 999M ? " >999" + "%" : (num > 0M ? "+" : "") + str + "%";
    }
  }
}
