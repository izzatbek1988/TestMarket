// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.MasterReport.MasterReportViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FastReport;
using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Reports.MasterReport
{
  public partial class MasterReportViewModel : ViewModelWithForm
  {
    private MasterReportViewModel.TypeReport _selectedTypeReport;
    private int _currentPage;
    private DateTime _valueDateTimeStart;
    private DateTime _valueDateTimeEnd;
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private string _buttonContentStorage;
    private IEnumerable<Storages.Storage> _allListStorage;
    private List<Storages.Storage> _storageListFilter;
    private Client _supplier;
    private string _filterTextGroups;
    private Visibility? _isBusy;
    private int _countDay;
    private int _coeffDay;
    private bool _isGoodForSum;
    private Decimal _sumOrder;

    public MasterReportViewModel.TypeReport SelectedTypeReport
    {
      get => this._selectedTypeReport;
      set
      {
        switch (value)
        {
          case MasterReportViewModel.TypeReport.None:
            this._selectedTypeReport = value;
            this.OnPropertyChanged(nameof (SelectedTypeReport));
            break;
          case MasterReportViewModel.TypeReport.OldGood:
            this.TitleReport = Translate.ЗалежавшиесяТовары;
            goto case MasterReportViewModel.TypeReport.None;
          case MasterReportViewModel.TypeReport.SaleItem:
            this.TitleReport = Translate.ReportType_Отчет_по_продажам;
            goto case MasterReportViewModel.TypeReport.None;
          case MasterReportViewModel.TypeReport.OrderGood:
            this.TitleReport = Translate.ТоварыНеобходимыеДляДозаказа;
            goto case MasterReportViewModel.TypeReport.None;
          case MasterReportViewModel.TypeReport.HistoryGood:
            this.TitleReport = Translate.FrmMasterReport_ИсторияДвиженияТоваров;
            goto case MasterReportViewModel.TypeReport.None;
          case MasterReportViewModel.TypeReport.PaymentsForSupplier:
            this.TitleReport = Translate.FrmMasterReport_ВзаиморасчетыСПоставщиком;
            goto case MasterReportViewModel.TypeReport.None;
          case MasterReportViewModel.TypeReport.PaymentsMove:
            this.TitleReport = Translate.FrmMasterReport_ДвижениеДенежныхСредств;
            goto case MasterReportViewModel.TypeReport.None;
          default:
            throw new ArgumentOutOfRangeException(nameof (value), (object) value, (string) null);
        }
      }
    }

    public List<MasterReportViewModel.ReportItem> ReportItems { get; set; }

    public Visibility VisibilityCategory
    {
      get
      {
        return !this.SelectedTypeReport.IsEither<MasterReportViewModel.TypeReport>(MasterReportViewModel.TypeReport.OldGood, MasterReportViewModel.TypeReport.OrderGood, MasterReportViewModel.TypeReport.SaleItem, MasterReportViewModel.TypeReport.HistoryGood) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityStorage
    {
      get
      {
        return !this.SelectedTypeReport.IsEither<MasterReportViewModel.TypeReport>(MasterReportViewModel.TypeReport.OldGood, MasterReportViewModel.TypeReport.OrderGood, MasterReportViewModel.TypeReport.SaleItem, MasterReportViewModel.TypeReport.HistoryGood) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilitySupplier
    {
      get
      {
        return !this.SelectedTypeReport.IsEither<MasterReportViewModel.TypeReport>(MasterReportViewModel.TypeReport.OldGood, MasterReportViewModel.TypeReport.OrderGood, MasterReportViewModel.TypeReport.SaleItem, MasterReportViewModel.TypeReport.HistoryGood, MasterReportViewModel.TypeReport.PaymentsForSupplier) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityPaymentAccounts
    {
      get
      {
        return this.SelectedTypeReport != MasterReportViewModel.TypeReport.PaymentsMove ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private void GetVisibilityFilter()
    {
      this.OnPropertyChanged("VisibilityCategory");
      this.OnPropertyChanged("VisibilityStorage");
      this.OnPropertyChanged("VisibilitySupplier");
      this.OnPropertyChanged("VisibilityPaymentAccounts");
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Cancel()));
    }

    public void Cancel()
    {
      if (this._currentPage == 1)
        this.CloseAction();
      else
        this.ChangePage(1);
    }

    public Visibility? IsBusy
    {
      get => this._isBusy;
      set
      {
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public Visibility FirstPageVisibility { get; set; }

    public Visibility SecondPageVisibility { get; set; }

    public string OkButtonText { get; set; }

    public string CancelButtonText { get; set; }

    private void ChangePage(int pageIndex)
    {
      switch (pageIndex)
      {
        case 1:
          this._currentPage = 1;
          this.FirstPageVisibility = Visibility.Visible;
          this.SecondPageVisibility = Visibility.Collapsed;
          this.OkButtonText = Translate.MasterReportViewModel_ChangePage_Далее;
          this.CancelButtonText = Translate.FrmInputMessage_Отмена;
          break;
        case 2:
          this._currentPage = 2;
          this.FirstPageVisibility = Visibility.Collapsed;
          this.SecondPageVisibility = Visibility.Visible;
          this.OkButtonText = Translate.MasterReportViewModel_ChangePage_Сформировать;
          this.CancelButtonText = Translate.FrmInventoryCard_Назад;
          break;
      }
      this.OnPropertyChanged("TitleReport");
      this.OnPropertyChanged("ButtonContentSup");
      this.OnPropertyChanged("VisibilityOptionForOrderGoodReport");
      this.OnPropertyChanged("FirstPageVisibility");
      this.OnPropertyChanged("SecondPageVisibility");
      this.OnPropertyChanged("OkButtonText");
      this.OnPropertyChanged("CancelButtonText");
    }

    public ICommand PrepareReportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (this._currentPage == 1)
            {
              this.GetVisibilityFilter();
              this.ChangePage(2);
            }
            else
              Task.Run(new Action(this.DoReports));
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex);
            string формированииОтчета = Translate.MasterReportViewModel_Ошибка_при_формировании_отчета;
            LogHelper.ShowErrorMgs(ex, формированииОтчета, LogHelper.MsgTypes.MessageBox);
          }
        }));
      }
    }

    private void SetBusy(bool isBusy)
    {
      this.IsBusy = new Visibility?(isBusy ? Visibility.Visible : Visibility.Hidden);
    }

    private MasterReportViewModel.ReportCache Cache { get; set; }

    private ReportType getGlobalReportType(MasterReportViewModel.TypeReport type)
    {
      ReportType globalReportType;
      switch (type)
      {
        case MasterReportViewModel.TypeReport.None:
          globalReportType = (ReportType) null;
          break;
        case MasterReportViewModel.TypeReport.OldGood:
          globalReportType = ReportType.ReportOldGoods;
          break;
        case MasterReportViewModel.TypeReport.SaleItem:
          globalReportType = ReportType.ReportSale;
          break;
        case MasterReportViewModel.TypeReport.OrderGood:
          globalReportType = ReportType.ReportOrderGood;
          break;
        case MasterReportViewModel.TypeReport.HistoryGood:
          globalReportType = ReportType.ReportHistoryGoods;
          break;
        case MasterReportViewModel.TypeReport.PaymentsForSupplier:
          globalReportType = ReportType.ReportPaymentsForSupplier;
          break;
        case MasterReportViewModel.TypeReport.PaymentsMove:
          globalReportType = ReportType.ReportPaymentsMove;
          break;
        default:
          globalReportType = (ReportType) null;
          break;
      }
      return globalReportType;
    }

    private void DoReports()
    {
      try
      {
        this.SetBusy(true);
        this.IsEnabledForm = false;
        FastReportFacade fastReportFacade = new FastReportFacade();
        if (this.Supplier == null && this.SelectedTypeReport == MasterReportViewModel.TypeReport.PaymentsForSupplier)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.MasterReportViewModel_DoReports_Для_формирования_данного_отчета_необходимо_выбрать_поставщика_, icon: MessageBoxImage.Exclamation);
        }
        else
        {
          if (!this.GroupsListFilter.Any<GoodGroups.Group>())
          {
            if (this.SelectedTypeReport.IsEither<MasterReportViewModel.TypeReport>(MasterReportViewModel.TypeReport.HistoryGood, MasterReportViewModel.TypeReport.SaleItem, MasterReportViewModel.TypeReport.OldGood, MasterReportViewModel.TypeReport.OrderGood))
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.MasterReportViewModel_Для_формирвоания_отчета_необходимо_выбрать_категории_товара_, icon: MessageBoxImage.Exclamation);
              return;
            }
          }
          ReportType globalReportType = this.getGlobalReportType(this.SelectedTypeReport);
          string str = fastReportFacade.SelectTemplate(globalReportType, (Users.User) null);
          if (str.IsNullOrEmpty())
            return;
          IPrintableReport printableReport;
          if (this.Cache != null)
          {
            if (this.SelectedTypeReport == this.Cache.ReportType)
            {
              printableReport = this.Cache.Report;
            }
            else
            {
              this.Cache = (MasterReportViewModel.ReportCache) null;
              printableReport = this.PrepareReport();
            }
          }
          else
            printableReport = this.PrepareReport();
          if (printableReport == null)
            throw new ArgumentNullException();
          Report r = fastReportFacade.PrepareReport(str, printableReport.DataDictionary, printableReport.Properties);
          if (r == null)
            throw new ArgumentNullException();
          r.Prepare();
          Application.Current?.Dispatcher?.Invoke((Action) (() => r.ShowPrepared(false)));
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.MasterReportViewModel_Операция_выполнена,
            Text = Translate.MasterReportViewModel_Формирование_отчета_успешно_завершено
          });
          GC.Collect();
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        string удалосьСформировать = Translate.MasterReportViewModel_Отчет_не_удалось_сформировать;
        LogHelper.ShowErrorMgs(ex, удалосьСформировать, LogHelper.MsgTypes.MessageBox);
      }
      finally
      {
        this.IsEnabledForm = true;
        this.SetBusy(false);
      }
    }

    private IPrintableReport PrepareReport()
    {
      IPrintableReport printableReport = (IPrintableReport) null;
      PrintableReportFactory printableReportFactory = new PrintableReportFactory();
      ReportsDataFactory.ReportFilter reportFilter = new ReportsDataFactory.ReportFilter();
      reportFilter.StartDateTime = this.ValueDateTimeStart;
      DateTime dateTime = this.ValueDateTimeEnd;
      dateTime = dateTime.AddHours(23.0);
      dateTime = dateTime.AddMinutes(59.0);
      reportFilter.EndDateTime = dateTime.AddSeconds(59.0);
      reportFilter.Groups = this.GroupsListFilter.ToList<GoodGroups.Group>();
      reportFilter.Storages = this.StorageListFilter;
      reportFilter.Supplier = this.Supplier;
      PaymentsAccounts.PaymentsAccount paymentsAccount = new PaymentsAccounts.PaymentsAccount();
      paymentsAccount.Uid = this.PaymentsAccountUid;
      reportFilter.PaymentsAccount = paymentsAccount;
      ReportsDataFactory.ReportFilter filter = reportFilter;
      switch (this.SelectedTypeReport)
      {
        case MasterReportViewModel.TypeReport.OldGood:
          List<Gbs.Core.Entities.Goods.Good> forOldGoodReport = this.GetDataForOldGoodReport();
          printableReport = printableReportFactory.CreateForOldGoodsReport((IEnumerable<Gbs.Core.Entities.Goods.Good>) forOldGoodReport);
          break;
        case MasterReportViewModel.TypeReport.SaleItem:
          (List<Gbs.Core.Entities.Documents.Item> items1, List<Document> documents1) = new ReportsDataFactory(filter).GetDataForSaleReport();
          printableReport = printableReportFactory.CreateForSaleReport((IEnumerable<Gbs.Core.Entities.Documents.Item>) items1, (IEnumerable<Document>) documents1, this.ValueDateTimeStart, this.ValueDateTimeEnd);
          break;
        case MasterReportViewModel.TypeReport.OrderGood:
          (List<MasterReportViewModel.GoodOrder> items2, IEnumerable<Document> documents2) = this.GetDataForOrderGoodReport();
          printableReport = printableReportFactory.CreateForOrderGoodReport((IEnumerable<MasterReportViewModel.GoodOrder>) items2, documents2, this.ValueDateTimeStart, this.ValueDateTimeEnd);
          break;
        case MasterReportViewModel.TypeReport.HistoryGood:
          (List<Gbs.Core.Entities.Goods.Good> goodList, List<Document> documents3) = new ReportsDataFactory(filter).GetDataForHistoryGoodReport();
          printableReport = printableReportFactory.CreateForGoodHistoryReport((IEnumerable<Gbs.Core.Entities.Goods.Good>) goodList, (IEnumerable<Document>) documents3, this.ValueDateTimeStart, this.ValueDateTimeEnd);
          break;
        case MasterReportViewModel.TypeReport.PaymentsForSupplier:
          Decimal saldo;
          List<Document> forSupplierReport = new ReportsDataFactory(filter).GetDataPaymentsForSupplierReport(out saldo);
          printableReport = printableReportFactory.CreatePaymentsForSupplierReport((IEnumerable<Document>) forSupplierReport, this.ValueDateTimeStart, this.ValueDateTimeEnd, saldo);
          break;
        case MasterReportViewModel.TypeReport.PaymentsMove:
          Decimal sumOld;
          Decimal sumCurrent;
          string accountName;
          (List<Payments.Payment> payments, Dictionary<Guid, string> groupPayments) paymentMoveReport = new ReportsDataFactory(filter).GetDataForPaymentMoveReport(out sumOld, out sumCurrent, out accountName);
          if (!paymentMoveReport.payments.Any<Payments.Payment>())
            throw new ArgumentNullException("Нет данных для формирования отчета, измените значения фильтра и попробуйте еще раз.");
          printableReport = printableReportFactory.CreatePaymentsMoveReport((IEnumerable<Payments.Payment>) paymentMoveReport.payments, this.ValueDateTimeStart, this.ValueDateTimeEnd, sumOld, sumCurrent, paymentMoveReport.groupPayments, accountName);
          break;
      }
      if (printableReport != null)
        this.Cache = new MasterReportViewModel.ReportCache()
        {
          Report = printableReport,
          ReportType = this.SelectedTypeReport
        };
      return printableReport;
    }

    public void ShowReport()
    {
      if (!Other.IsActiveAndShowForm<FrmMasterReport>())
        return;
      (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.ShowMasterReport);
      if (!access.Result)
        return;
      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.MasterReportViewModel_ShowReport_Открыт_мастер_отчетов, access.User), true);
      using (DataBase dataBase = Data.GetDataBase())
      {
        this._groupsListFilter = new ObservableCollection<GoodGroups.Group>(new GoodGroupsRepository(dataBase).GetAllItems());
        this._allListStorage = (IEnumerable<Storages.Storage>) new List<Storages.Storage>(Storages.GetStorages().Where<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted)));
        this._storageListFilter = new List<Storages.Storage>(this._allListStorage);
        this.Accounts.AddRange(PaymentsAccounts.GetPaymentsAccountsList().Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted)));
        this.FormToSHow = (WindowWithSize) new FrmMasterReport();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ChangePage(1);
        this.ShowForm(false);
      }
    }

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._valueDateTimeStart = value;
        this.OnPropertyChanged(nameof (ValueDateTimeStart));
      }
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._valueDateTimeEnd = value;
        this.OnPropertyChanged(nameof (ValueDateTimeEnd));
      }
    }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._groupsListFilter = value;
      }
    }

    public string ButtonContentStorage
    {
      get => this._buttonContentStorage;
      set
      {
        this._buttonContentStorage = value;
        this.OnPropertyChanged(nameof (ButtonContentStorage));
      }
    }

    public ICommand SelectedStorage
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FrmSelectedStorage frmSelectedStorage = new FrmSelectedStorage();
          List<Storages.Storage> collection = new List<Storages.Storage>((IEnumerable<Storages.Storage>) this.StorageListFilter);
          ref List<Storages.Storage> local = ref collection;
          if (!frmSelectedStorage.GetListSelectedStorages(ref local))
            return;
          this.StorageListFilter = new List<Storages.Storage>((IEnumerable<Storages.Storage>) collection);
          this.Cache = (MasterReportViewModel.ReportCache) null;
        }));
      }
    }

    private List<Storages.Storage> StorageListFilter
    {
      get => this._storageListFilter;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._storageListFilter = value;
        this.OnPropertyChanged(nameof (StorageListFilter));
        int count = this._storageListFilter.Count;
        if (count != 0 && count != this._allListStorage.Count<Storages.Storage>())
          this.ButtonContentStorage = count == 1 ? this._storageListFilter.First<Storages.Storage>().Name : Translate.WaybillsViewModel_Складов_ + count.ToString();
        else if (count == 1)
        {
          this.ButtonContentStorage = this._storageListFilter.First<Storages.Storage>().Name;
        }
        else
        {
          this.ButtonContentStorage = Translate.WaybillsViewModel_Все_склады;
          this._storageListFilter = new List<Storages.Storage>(this._allListStorage);
        }
      }
    }

    public string TitleReport { get; set; }

    public string ButtonContentSup
    {
      get
      {
        if (this.Supplier != null)
          return this.Supplier.Name;
        return this.SelectedTypeReport != MasterReportViewModel.TypeReport.PaymentsForSupplier ? Translate.FrmGoodsCatalog_ВсеПоставщики : Translate.MasterReportViewModel_Выберите_поставщика;
      }
    }

    private Client Supplier
    {
      get => this._supplier;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._supplier = value;
        this.OnPropertyChanged("ButtonContentSup");
      }
    }

    private Guid _paymentsAccountUid { get; set; }

    public Guid PaymentsAccountUid
    {
      get => this._paymentsAccountUid;
      set
      {
        this.Cache = (MasterReportViewModel.ReportCache) null;
        this._paymentsAccountUid = value;
        this.OnPropertyChanged(nameof (PaymentsAccountUid));
      }
    }

    public List<PaymentsAccounts.PaymentsAccount> Accounts { get; set; }

    public ICommand GetSupplierCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (Client client, bool result) client1 = new FrmSearchClient().GetClient(true);
          Client client2 = client1.client;
          this.Supplier = !client1.result ? (Client) null : client2;
          this.Cache = (MasterReportViewModel.ReportCache) null;
        }));
      }
    }

    public List<Gbs.Core.Entities.Goods.Good> GetDataForOldGoodReport()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<Gbs.Core.Entities.Goods.Good> source = new GoodRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => !x.IS_DELETED))).Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Any<GoodsStocks.GoodStock>() && x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (s => s.Stock)) > 0M && this.GroupsListFilter.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Group.Uid)) && x.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => !s.IsDeleted && this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (st => st.Uid == s.Storage.Uid))))));
        IEnumerable<Gbs.Core.Entities.Goods.Good> itemsGood = new DocumentsRepository(dataBase).GetItemsWithFilter(this.ValueDateTimeStart, this.ValueDateTimeEnd, GlobalDictionaries.DocumentsTypes.Sale, false).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Select<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).GroupBy<Gbs.Core.Entities.Goods.Good, Guid>((Func<Gbs.Core.Entities.Goods.Good, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>, Gbs.Core.Entities.Goods.Good>((Func<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>, Gbs.Core.Entities.Goods.Good>) (x => x.First<Gbs.Core.Entities.Goods.Good>()));
        IEnumerable<Gbs.Core.Entities.Goods.Good> list = (IEnumerable<Gbs.Core.Entities.Goods.Good>) source.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => itemsGood.All<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => g.Uid != x.Uid)))).ToList<Gbs.Core.Entities.Goods.Good>();
        if (this.Supplier != null)
        {
          IEnumerable<Gbs.Core.Entities.Documents.Item> waybillItems = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).Where<Document>((Func<Document, bool>) (x => x.ContractorUid == this.Supplier.Uid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => x.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => !i.IsDeleted))));
          list = (IEnumerable<Gbs.Core.Entities.Goods.Good>) list.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (good => waybillItems.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (item => item.GoodUid == good.Uid)))).ToList<Gbs.Core.Entities.Goods.Good>();
        }
        return list.ToList<Gbs.Core.Entities.Goods.Good>();
      }
    }

    public Visibility VisibilityOptionForOrderGoodReport
    {
      get
      {
        return this.SelectedTypeReport != MasterReportViewModel.TypeReport.OrderGood ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public int CountDay
    {
      get => this._countDay;
      set
      {
        this._countDay = value;
        this.Cache = (MasterReportViewModel.ReportCache) null;
      }
    }

    public Dictionary<int, string> DictionaryDay { get; set; }

    public int CoeffDay
    {
      get => this._coeffDay;
      set
      {
        this._coeffDay = value;
        this.Cache = (MasterReportViewModel.ReportCache) null;
      }
    }

    public bool IsGoodForSum
    {
      get => this._isGoodForSum;
      set
      {
        this._isGoodForSum = value;
        this.Cache = (MasterReportViewModel.ReportCache) null;
      }
    }

    public Decimal SumOrder
    {
      get => this._sumOrder;
      set
      {
        this._sumOrder = value;
        this.Cache = (MasterReportViewModel.ReportCache) null;
      }
    }

    public (List<MasterReportViewModel.GoodOrder> items, IEnumerable<Document> documents) GetDataForOrderGoodReport()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Document> itemsWithFilter1 = new DocumentsRepository(dataBase).GetItemsWithFilter(this.ValueDateTimeStart, this.ValueDateTimeEnd, GlobalDictionaries.DocumentsTypes.Sale, false);
        List<Document> itemsWithFilter2 = new DocumentsRepository(dataBase).GetItemsWithFilter(this.ValueDateTimeStart, this.ValueDateTimeEnd, GlobalDictionaries.DocumentsTypes.SetChildStockChange, false);
        List<Document> waybill = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).ToList<Document>();
        List<Gbs.Core.Entities.Documents.Item> list = itemsWithFilter2.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).ToList<Gbs.Core.Entities.Documents.Item>();
        list.ForEach((Action<Gbs.Core.Entities.Documents.Item>) (x => x.Quantity = Math.Abs(x.Quantity)));
        IEnumerable<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>> source1 = itemsWithFilter1.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Union<Gbs.Core.Entities.Documents.Item>((IEnumerable<Gbs.Core.Entities.Documents.Item>) list).GroupBy<Gbs.Core.Entities.Documents.Item, Guid>((Func<Gbs.Core.Entities.Documents.Item, Guid>) (x => x.GoodUid)).Where<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, bool>) (x =>
        {
          if (this.GroupsListFilter.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => x.First<Gbs.Core.Entities.Documents.Item>().Good.Group.Uid == g.Uid)) && this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (st => x.First<Gbs.Core.Entities.Documents.Item>().Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == st.Uid)))))
          {
            if (x.First<Gbs.Core.Entities.Documents.Item>().Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
              return x.First<Gbs.Core.Entities.Documents.Item>().Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range);
          }
          return false;
        }));
        if (this.Supplier != null)
          source1 = (IEnumerable<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>>) source1.Where<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, bool>) (good => waybill.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Buy && x.ContractorUid == this.Supplier.Uid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (item =>
          {
            Guid? uid1 = item.GoodStock?.Uid;
            Guid? uid2 = good.First<Gbs.Core.Entities.Documents.Item>().GoodStock?.Uid;
            if (uid1.HasValue != uid2.HasValue)
              return false;
            return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
          })))).ToList<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>>();
        List<Gbs.Core.Entities.Goods.Good> allItems = new GoodRepository(dataBase).GetAllItems();
        Decimal totalDays = (Decimal) (this.ValueDateTimeEnd - this.ValueDateTimeStart).TotalDays;
        int num1 = this.CountDay * this.CoeffDay;
        List<MasterReportViewModel.GoodOrder> source2 = new List<MasterReportViewModel.GoodOrder>();
        foreach (IGrouping<Guid, Gbs.Core.Entities.Documents.Item> grouping in source1)
        {
          IGrouping<Guid, Gbs.Core.Entities.Documents.Item> good = grouping;
          if (!good.First<Gbs.Core.Entities.Documents.Item>().Good.IsDeleted)
          {
            Gbs.Core.Entities.Goods.Good good1 = allItems.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == good.First<Gbs.Core.Entities.Documents.Item>().GoodUid));
            Decimal? nullable1;
            Decimal? nullable2;
            if (good1 == null)
            {
              nullable1 = new Decimal?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new Decimal?(good1.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
            Decimal num2 = good.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)) / totalDays * (Decimal) num1;
            nullable1 = nullable2;
            Decimal? nullable3;
            Decimal? nullable4;
            if (!nullable1.HasValue)
            {
              nullable3 = new Decimal?();
              nullable4 = nullable3;
            }
            else
              nullable4 = new Decimal?(num2 - nullable1.GetValueOrDefault());
            nullable3 = nullable4;
            Decimal valueOrDefault = nullable3.GetValueOrDefault();
            if (!(valueOrDefault <= 0M))
              source2.Add(new MasterReportViewModel.GoodOrder()
              {
                Good = good.First<Gbs.Core.Entities.Documents.Item>().Good,
                Count = valueOrDefault,
                SaleQuantity = good.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)),
                BuyPrice = SaleHelper.GetLastBuyPriceForGood(good.First<Gbs.Core.Entities.Documents.Item>().Good, (IEnumerable<Document>) waybill)
              });
          }
        }
        if (this.IsGoodForSum)
          throw new NotImplementedException();
        source2.ForEach((Action<MasterReportViewModel.GoodOrder>) (x => x.Count = x.Count));
        return (source2.Where<MasterReportViewModel.GoodOrder>((Func<MasterReportViewModel.GoodOrder, bool>) (x => x.Count > 0M)).ToList<MasterReportViewModel.GoodOrder>(), (IEnumerable<Document>) waybill);
      }
    }

    public MasterReportViewModel()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      this._valueDateTimeStart = dateTime.AddMonths(-1);
      this._valueDateTimeEnd = DateTime.Now.Date;
      this._buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
      this._storageListFilter = new List<Storages.Storage>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CTitleReport\u003Ek__BackingField = Translate.ReportType_Отчет_по_продажам;
      // ISSUE: reference to a compiler-generated field
      this.\u003C_paymentsAccountUid\u003Ek__BackingField = Guid.Empty;
      List<PaymentsAccounts.PaymentsAccount> paymentsAccountList = new List<PaymentsAccounts.PaymentsAccount>();
      PaymentsAccounts.PaymentsAccount paymentsAccount = new PaymentsAccounts.PaymentsAccount();
      paymentsAccount.Uid = Guid.Empty;
      paymentsAccount.Name = Translate.MasterReportViewModel_Accounts_Все_счета;
      paymentsAccountList.Add(paymentsAccount);
      // ISSUE: reference to a compiler-generated field
      this.\u003CAccounts\u003Ek__BackingField = paymentsAccountList;
      this._filterTextGroups = Translate.GoodsCatalogModelView_Все_категории;
      this._isBusy = new Visibility?(Visibility.Collapsed);
      this._countDay = 1;
      this._coeffDay = 7;
      // ISSUE: reference to a compiler-generated field
      this.\u003CDictionaryDay\u003Ek__BackingField = new Dictionary<int, string>()
      {
        {
          1,
          Translate.MasterReportViewModel_дней
        },
        {
          7,
          Translate.MasterReportViewModel_недель
        },
        {
          30,
          Translate.MasterReportViewModel_месяцев
        }
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class ReportItem
    {
      public string Name { get; set; }

      public string Description { get; set; }

      public MasterReportViewModel.TypeReport TypeReport { get; set; }
    }

    private class ReportCache
    {
      public MasterReportViewModel.TypeReport ReportType;

      public IPrintableReport Report { get; set; }
    }

    public enum TypeReport
    {
      None,
      OldGood,
      SaleItem,
      OrderGood,
      HistoryGood,
      PaymentsForSupplier,
      PaymentsMove,
    }

    public class GoodOrder
    {
      public Gbs.Core.Entities.Goods.Good Good { get; set; }

      public Decimal SaleQuantity { get; set; }

      public Decimal BuyPrice { get; set; }

      public Decimal Count { get; set; }
    }
  }
}
