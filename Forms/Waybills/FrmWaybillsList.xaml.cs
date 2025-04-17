// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Waybills.WaybillsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Goods.GoodGroupEdit;
using Gbs.Forms.Lable;
using Gbs.Forms.SendWaybills;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Waybills
{
  public partial class WaybillsViewModel : ViewModelWithForm
  {
    private System.Timers.Timer _searchTimer;
    private DateTime _searchStartDateTime;
    private bool _searchInProgress;
    public ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> _storageListFilter;
    private Client _supplier;
    private Guid _selectedSection;
    private SaleJournalViewModel.ExtraOption _selectedExtraOption;
    private GlobalDictionaries.DocumentsStatuses _selectedStatus;
    private Decimal _totalGoodsStock;
    private Decimal _totalBuySum;
    private Decimal _totalSaleSum;
    private CancellationTokenSource _cancelToken;
    private readonly object _goodCollectionLock;
    private AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid> _waybillItemsList;
    private string _searchText;
    private Visibility _visibilityCreditSum;
    private Visibility _visibilityPaymentsSum;
    public static string PaymentsColumnUid = "BB1849C0-A66E-4574-AD95-70543FFDB23F";
    public static string CreditColumnUid = "C8BD29E8-A6CA-49A1-B051-75A70771A139";

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Entities.Documents.Cache.ClearAndLoadCache();
          this.GetWaybills();
        }));
      }
    }

    private void SearchTimerInit()
    {
      this._searchTimer = new System.Timers.Timer()
      {
        Interval = 50.0,
        AutoReset = true
      };
      this._searchTimer.Elapsed += new ElapsedEventHandler(this.SearchTimerElapsed);
      this._searchTimer.Start();
    }

    private void SearchTimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (!(this._searchStartDateTime < DateTime.Now))
        return;
      this.StartSearch();
    }

    public void StartSearch()
    {
      if (this._searchInProgress)
        return;
      this._searchStartDateTime = DateTime.Now.AddYears(1);
      this.SearchForFilter();
      this._searchInProgress = false;
    }

    private IEnumerable<Storages.Storage> AllListStorage { get; }

    public string ButtonContentStorage
    {
      get
      {
        int num = this._storageListFilter.Count<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked));
        if (num == this.AllListStorage.Count<Storages.Storage>())
          return Translate.WaybillsViewModel_Все_склады;
        return num != 1 ? Translate.WaybillsViewModel_Складов_ + num.ToString() : this._storageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked)).Item.Name;
      }
    }

    public ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> StorageListFilter
    {
      get => this._storageListFilter;
      set
      {
        this._storageListFilter = value;
        this.OnPropertyChanged(nameof (StorageListFilter));
        this.OnPropertyChanged("ButtonContentStorage");
        this.SearchForFilter();
      }
    }

    public Users.User AuthUser { private get; set; }

    public static string AlsoMenuKey => "AlsoMenu";

    public static string PrintMenuKey => "PrintMenu";

    public WaybillsViewModel.WaybillItemsInfoGrid SelectedWaybill { get; set; }

    public static IEnumerable<Users.User> ListUsers { get; set; } = (IEnumerable<Users.User>) new List<Users.User>();

    public static IEnumerable<Client> ListClient { get; set; } = (IEnumerable<Client>) new List<Client>();

    public static Dictionary<GlobalDictionaries.DocumentsStatuses, string> Statuses { get; set; } = new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
    {
      {
        GlobalDictionaries.DocumentsStatuses.None,
        Translate.WaybillsViewModel_Statuses_Все_статусы
      },
      {
        GlobalDictionaries.DocumentsStatuses.Draft,
        Translate.FrmWaybillCard_ЕщеВПути
      },
      {
        GlobalDictionaries.DocumentsStatuses.Open,
        Translate.WaybillsViewModel_Statuses_Принята
      }
    };

    public static Dictionary<SaleJournalViewModel.ExtraOption, string> StatusesPayment { get; set; } = new Dictionary<SaleJournalViewModel.ExtraOption, string>()
    {
      {
        SaleJournalViewModel.ExtraOption.None,
        Translate.SaleJournalViewModel_ListExtraOption_Все_статусы_оплаты
      },
      {
        SaleJournalViewModel.ExtraOption.OnlyCredit,
        Translate.SaleJournalViewModel_ListExtraOption_Неоплаченные
      },
      {
        SaleJournalViewModel.ExtraOption.CreditAndPayment,
        Translate.SaleJournalViewModel_ListExtraOption_Оплаченные_частично
      },
      {
        SaleJournalViewModel.ExtraOption.OnlyFullPayment,
        Translate.SaleJournalViewModel_ListExtraOption_Оплаченные_полностью
      },
      {
        SaleJournalViewModel.ExtraOption.Credit,
        Translate.SaleJournalViewModel_Полностью_или_частично_неоплаченные
      }
    };

    public List<Sections.Section> SectionList { get; set; }

    public Guid SelectedSection
    {
      get => this._selectedSection;
      set
      {
        this._selectedSection = value;
        this.SearchForFilter();
      }
    }

    public SaleJournalViewModel.ExtraOption SelectedExtraOption
    {
      get => this._selectedExtraOption;
      set
      {
        this._selectedExtraOption = value;
        this.OnPropertyChanged(nameof (SelectedExtraOption));
        this.SearchForFilter();
      }
    }

    public GlobalDictionaries.DocumentsStatuses SelectedStatus
    {
      get => this._selectedStatus;
      set
      {
        this._selectedStatus = value;
        this.SearchForFilter();
      }
    }

    public string ButtonContentSup
    {
      get => this.Supplier != null ? this.Supplier.Name : Translate.FrmGoodsCatalog_ВсеПоставщики;
    }

    public ICommand SelectedStorage { get; set; }

    public ICommand AddNewWaybill { get; set; }

    public ICommand EditCardWaybill { get; set; }

    public ICommand PrintWaybills { get; set; }

    public ICommand PrintLableForWaybill { get; set; }

    public ICommand PrintTagForWaybill { get; set; }

    public ICommand GetSupplier { get; set; }

    public ICommand DeleteWaybill { get; set; }

    public ICommand DoGroupEditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmGoodsForGroupEdit().DoEdit(this.SelectedList.SelectMany<WaybillsViewModel.WaybillItemsInfoGrid, BasketItem>((Func<WaybillsViewModel.WaybillItemsInfoGrid, IEnumerable<BasketItem>>) (d => d.Document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage))))).ToList<BasketItem>(), this.AuthUser)));
      }
    }

    public ICommand CreateMoveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedList.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            List<BasketItem> list = this.SelectedList.SelectMany<WaybillsViewModel.WaybillItemsInfoGrid, BasketItem>((Func<WaybillsViewModel.WaybillItemsInfoGrid, IEnumerable<BasketItem>>) (d => d.Document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage, x.Quantity))))).ToList<BasketItem>();
            Move basket = new Move()
            {
              Items = new ObservableCollection<BasketItem>(list),
              Storage = this.SelectedList.First<WaybillsViewModel.WaybillItemsInfoGrid>().Document.Storage
            };
            new FrmSendWaybillCard().ShowCard(Guid.Empty, out Document _, this.AuthUser, basket);
          }
        }));
      }
    }

    public ICommand DoGroupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<WaybillsViewModel.WaybillItemsInfoGrid> list = ((IEnumerable) obj).Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
          if (!list.Any<WaybillsViewModel.WaybillItemsInfoGrid>())
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            this.SelectedList = new List<WaybillsViewModel.WaybillItemsInfoGrid>((IEnumerable<WaybillsViewModel.WaybillItemsInfoGrid>) list);
            this.ShowMenuAction();
          }
        }));
      }
    }

    public Action ShowMenuAction { get; set; }

    private Client Supplier
    {
      get => this._supplier;
      set
      {
        this._supplier = value;
        this.SearchForFilter();
        this.OnPropertyChanged("ButtonContentSup");
      }
    }

    public List<WaybillsViewModel.WaybillItemsInfoGrid> SelectedList { get; set; }

    public Visibility VisibilityNoForReturn
    {
      get => !this.IsReturnList ? Visibility.Visible : Visibility.Collapsed;
    }

    public WaybillsViewModel()
    {
      List<Sections.Section> sectionList = new List<Sections.Section>();
      Sections.Section section = new Sections.Section();
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      section.Uid = Guid.Empty;
      sectionList.Add(section);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSectionList\u003Ek__BackingField = sectionList;
      this._selectedSection = Guid.Empty;
      this._cancelToken = new CancellationTokenSource();
      this._goodCollectionLock = new object();
      this._waybillItemsList = new AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid>();
      this._searchText = string.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool IsReturnList { get; set; }

    public WaybillsViewModel(bool isReturn = false)
    {
      List<Sections.Section> sectionList = new List<Sections.Section>();
      Sections.Section section = new Sections.Section();
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      section.Uid = Guid.Empty;
      sectionList.Add(section);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSectionList\u003Ek__BackingField = sectionList;
      this._selectedSection = Guid.Empty;
      this._cancelToken = new CancellationTokenSource();
      this._goodCollectionLock = new object();
      this._waybillItemsList = new AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid>();
      this._searchText = string.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SearchTimerInit();
      try
      {
        this.IsReturnList = isReturn;
        using (DataBase dataBase = Data.GetDataBase())
        {
          this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
          this._storageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(this.AllListStorage.Select<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
          {
            IsChecked = true,
            Item = x
          })));
          this.SectionList.AddRange((IEnumerable<Sections.Section>) Sections.GetSectionsList(dataBase.GetTable<SECTIONS>().Where<SECTIONS>((Expression<Func<SECTIONS, bool>>) (x => x.IS_DELETED == false))));
        }
        this.ValueDateTimeStart = DateTime.Now.AddYears(-1);
        this.ValueDateTimeEnd = DateTime.Now.Date;
        this.LoadWaybillsCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.GetWaybills()));
        this.AddNewWaybill = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Users.User authUser = this.AuthUser.Clone();
          using (DataBase dataBase = Data.GetDataBase())
          {
            if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, this.IsReturnList ? Actions.WaybillReturnAdd : Actions.WaybillAdd))
            {
              (bool Result2, Users.User User2) = new Authorization().GetAccess(this.IsReturnList ? Actions.WaybillReturnAdd : Actions.WaybillAdd);
              if (!Result2)
                return;
              authUser = User2;
            }
            new FrmWaybillCard().ShowCardWaybill(Guid.Empty, out Document _, authUser, WaybillsViewModel.CachedDbWaybills, new Action(this.SearchForFilter), this.IsReturnList);
          }
        }));
        this.EditCardWaybill = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedWaybill == null)
            MessageBoxHelper.Warning(Translate.WaybillsViewModel_Нужно_выбрать_накладную_);
          else if (((IEnumerable) obj).Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>().Count > 1)
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            Users.User authUser = this.AuthUser.Clone();
            using (DataBase dataBase = Data.GetDataBase())
            {
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, this.IsReturnList ? Actions.WaybillReturnEdit : Actions.WaybillEdit))
              {
                (bool Result, Users.User User) access = new Authorization().GetAccess(this.IsReturnList ? Actions.WaybillReturnEdit : Actions.WaybillEdit);
                if (!access.Result)
                  return;
                authUser = access.User;
              }
              new FrmWaybillCard().ShowCardWaybill(this.SelectedWaybill.Document.Uid, out Document _, authUser, WaybillsViewModel.CachedDbWaybills, new Action(this.SearchForFilter), this.IsReturnList);
            }
          }
        }));
        this.PrintWaybills = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedWaybill == null)
            MessageBoxHelper.Warning(Translate.WaybillsViewModel_Нужно_выбрать_накладную_);
          else if (this.SelectedList.Count > 1)
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForWaybill(this.SelectedWaybill.Document, this.VisibilityBuySum == Visibility.Visible), this.AuthUser);
        }));
        this.PrintLableForWaybill = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedWaybill == null)
            MessageBoxHelper.Warning(Translate.WaybillsViewModel_Нужно_выбрать_накладную_);
          else
            new FrmLablePrint().Print(LablePrintViewModel.Types.Labels, this.SelectedList.SelectMany<WaybillsViewModel.WaybillItemsInfoGrid, BasketItem>((Func<WaybillsViewModel.WaybillItemsInfoGrid, IEnumerable<BasketItem>>) (d => d.Document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage, x.Quantity))))).ToList<BasketItem>());
        }));
        this.PrintTagForWaybill = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedWaybill == null)
            MessageBoxHelper.Warning(Translate.WaybillsViewModel_Нужно_выбрать_накладную_);
          else
            new FrmLablePrint().Print(LablePrintViewModel.Types.PriceTags, this.SelectedList.SelectMany<WaybillsViewModel.WaybillItemsInfoGrid, BasketItem>((Func<WaybillsViewModel.WaybillItemsInfoGrid, IEnumerable<BasketItem>>) (d => d.Document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage))))).ToList<BasketItem>());
        }));
        this.DeleteWaybill = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<WaybillsViewModel.WaybillItemsInfoGrid> list = ((IEnumerable) obj).Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
          if (!list.Any<WaybillsViewModel.WaybillItemsInfoGrid>())
          {
            MessageBoxHelper.Warning(Translate.WaybillsViewModel_Нужно_выбрать_накладную_);
          }
          else
          {
            if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_ + Translate.WaybillsViewModel_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            using (DataBase dataBase = Data.GetDataBase())
            {
              Users.User user = this.AuthUser.Clone();
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, this.IsReturnList ? Actions.WaybillReturnDelete : Actions.WaybillDelete))
              {
                (bool Result, Users.User User) access = new Authorization().GetAccess(this.IsReturnList ? Actions.WaybillReturnDelete : Actions.WaybillDelete);
                if (!access.Result)
                  return;
                user = access.User;
              }
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              foreach (WaybillsViewModel.WaybillItemsInfoGrid waybillItemsInfoGrid in list)
              {
                Document oldItem = waybillItemsInfoGrid.Document.Clone<Document>();
                waybillItemsInfoGrid.Document.IsDeleted = true;
                if (documentsRepository.Save(waybillItemsInfoGrid.Document))
                {
                  foreach (Entity entity in waybillItemsInfoGrid.Document.Items.GroupBy<Gbs.Core.Entities.Documents.Item, Guid>((Func<Gbs.Core.Entities.Documents.Item, Guid>) (x =>
                  {
                    GoodsStocks.GoodStock goodStock = x.GoodStock;
                    // ISSUE: explicit non-virtual call
                    return goodStock == null ? Guid.Empty : __nonvirtual (goodStock.Uid);
                  })).Select<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, GoodsStocks.GoodStock>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, GoodsStocks.GoodStock>) (x => x.First<Gbs.Core.Entities.Documents.Item>().GoodStock)))
                  {
                    GoodsStocks.GoodStock stocksByUid = GoodsStocks.GetStocksByUid(entity.Uid);
                    if (stocksByUid.Stock == 0M)
                    {
                      stocksByUid.IsDeleted = true;
                      stocksByUid.Save(dataBase);
                    }
                  }
                  this.WaybillItemsList.Remove(waybillItemsInfoGrid);
                  ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) waybillItemsInfoGrid.Document, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, user), true);
                }
              }
            }
          }
        }));
        this.GetSupplier = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (Client client, bool result) client1 = new FrmSearchClient().GetClient(true);
          Client client2 = client1.client;
          if (client1.result)
            this.Supplier = client2;
          else
            this.Supplier = (Client) null;
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка накладных");
      }
    }

    public Decimal TotalGoodsItemsCount
    {
      get => this._totalGoodsStock;
      set
      {
        this._totalGoodsStock = value;
        this.OnPropertyChanged(nameof (TotalGoodsItemsCount));
      }
    }

    public Decimal TotalBuySum
    {
      get => this._totalBuySum;
      set
      {
        this._totalBuySum = value;
        this.OnPropertyChanged(nameof (TotalBuySum));
      }
    }

    public Decimal TotalSaleSum
    {
      get => this._totalSaleSum;
      set
      {
        this._totalSaleSum = value;
        this.OnPropertyChanged(nameof (TotalSaleSum));
      }
    }

    public Decimal TotalCreditSum
    {
      get
      {
        return this.WaybillItemsList.Sum<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, Decimal>) (x => x.SumCredit));
      }
    }

    public Decimal TotalPaymentsSum
    {
      get
      {
        return this.WaybillItemsList.Sum<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, Decimal>) (x => x.SumPayments));
      }
    }

    public ICommand LoadWaybillsCommand { get; set; }

    private void GetWaybills()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.WaybillsViewModel_Загрузка_журнала_поступлений);
      GlobalDictionaries.DocumentsTypes documentsTypes = this.IsReturnList ? GlobalDictionaries.DocumentsTypes.BuyReturn : GlobalDictionaries.DocumentsTypes.Buy;
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
        {
          DateStart = this.ValueDateTimeStart,
          DateEnd = this.ValueDateTimeEnd,
          IncludeDeleted = false,
          IgnoreTime = true,
          Types = new GlobalDictionaries.DocumentsTypes[1]
          {
            documentsTypes
          }
        });
        WaybillsViewModel.ListClient = (IEnumerable<Client>) CachesBox.AllClients();
        WaybillsViewModel.ListUsers = (IEnumerable<Users.User>) CachesBox.AllUsers();
        WaybillsViewModel.CachedDbWaybills = itemsWithFilter.Select<Document, WaybillsViewModel.WaybillItemsInfoGrid>((Func<Document, WaybillsViewModel.WaybillItemsInfoGrid>) (x => new WaybillsViewModel.WaybillItemsInfoGrid(x))).ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
        this.WaybillItemsList = new AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid>((IEnumerable<WaybillsViewModel.WaybillItemsInfoGrid>) WaybillsViewModel.CachedDbWaybills.OrderByDescending<WaybillsViewModel.WaybillItemsInfoGrid, DateTime>((Func<WaybillsViewModel.WaybillItemsInfoGrid, DateTime>) (x => x.Document.DateTime)));
        this.SearchForFilter();
        this.SetCountItems();
        progressBar.Close();
        stopwatch.Stop();
      }
    }

    public AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid> WaybillItemsList
    {
      get => this._waybillItemsList;
      set
      {
        this._waybillItemsList = value;
        this.OnPropertyChanged(nameof (WaybillItemsList));
      }
    }

    private static List<WaybillsViewModel.WaybillItemsInfoGrid> CachedDbWaybills { get; set; } = new List<WaybillsViewModel.WaybillItemsInfoGrid>();

    public string SearchText
    {
      get => this._searchText;
      set
      {
        this._searchText = value;
        this.OnPropertyChanged(nameof (SearchText));
        this._searchStartDateTime = DateTime.Now.AddMilliseconds(500.0);
      }
    }

    public string ContractorUid { get; set; }

    public DateTime ValueDateTimeStart { get; set; }

    public DateTime ValueDateTimeEnd { get; set; }

    private void SearchForFilter()
    {
      string q = this.SearchText.ToLower();
      IEnumerable<WaybillsViewModel.WaybillItemsInfoGrid> source = string.IsNullOrEmpty(q) ? (IEnumerable<WaybillsViewModel.WaybillItemsInfoGrid>) WaybillsViewModel.CachedDbWaybills : WaybillsViewModel.CachedDbWaybills.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.Document.Number.ToLower().Contains(q) || x.Document.Comment.ToLower().Contains(q)));
      if (this.Supplier != null)
        source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.Document.ContractorUid == this.Supplier.Uid));
      if (this.StorageListFilter.Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>())
        source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => this.StorageListFilter.Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (s => s.Item.Uid == x.Document.Storage.Uid && s.IsChecked))));
      if (this.SelectedStatus != GlobalDictionaries.DocumentsStatuses.None)
        source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.Document.Status == this.SelectedStatus));
      if (this.SelectedSection != Guid.Empty)
        source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x =>
        {
          Guid? uid = x.Document.Section?.Uid;
          Guid selectedSection = this.SelectedSection;
          return uid.HasValue && uid.GetValueOrDefault() == selectedSection;
        }));
      if (this.SelectedExtraOption != SaleJournalViewModel.ExtraOption.None)
      {
        switch (this.SelectedExtraOption)
        {
          case SaleJournalViewModel.ExtraOption.OnlyCredit:
            source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.SumPayments == 0M && x.SumItems > 0M));
            break;
          case SaleJournalViewModel.ExtraOption.CreditAndPayment:
            source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.SumPayments < x.SumItems && x.SumPayments != 0M));
            break;
          case SaleJournalViewModel.ExtraOption.OnlyFullPayment:
            source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.SumPayments >= x.SumItems));
            break;
          case SaleJournalViewModel.ExtraOption.Credit:
            source = source.Where<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.SumPayments < x.SumItems));
            break;
        }
      }
      this.WaybillItemsList = new AsyncObservableCollection<WaybillsViewModel.WaybillItemsInfoGrid>((IEnumerable<WaybillsViewModel.WaybillItemsInfoGrid>) source.OrderByDescending<WaybillsViewModel.WaybillItemsInfoGrid, DateTime>((Func<WaybillsViewModel.WaybillItemsInfoGrid, DateTime>) (x => x.Document.DateTime)));
      this.SetCountItems();
    }

    private void SetCountItems()
    {
      this.TotalGoodsItemsCount = this.WaybillItemsList.Sum<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, Decimal>) (x => x.SumCountItems));
      this.TotalBuySum = this.WaybillItemsList.Sum<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, Decimal>) (x => x.SumItems));
      this.TotalSaleSum = this.WaybillItemsList.Sum<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, Decimal>) (x => x.SaleSumItems));
      this.OnPropertyChanged("TotalCreditSum");
      this.OnPropertyChanged("TotalPaymentsSum");
    }

    public Action ShowMenuPrint { get; set; }

    public ICommand ShowMenuPrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SelectedList = ((IEnumerable) obj).Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
          this.ShowMenuPrint();
        }));
      }
    }

    public Visibility VisibilityBuySum { get; set; }

    public Visibility VisibilityCreditSum
    {
      get => this._visibilityCreditSum;
      set
      {
        this._visibilityCreditSum = value;
        this.OnPropertyChanged(nameof (VisibilityCreditSum));
      }
    }

    public Visibility VisibilityPaymentsSum
    {
      get => this._visibilityPaymentsSum;
      set
      {
        this._visibilityPaymentsSum = value;
        this.OnPropertyChanged(nameof (VisibilityPaymentsSum));
      }
    }

    public class WaybillItemsInfoGrid : ViewModelWithForm
    {
      private string _nameContractor;
      private string _nameUser;
      private Decimal _sumCountItems;
      private Decimal _sumItems;

      public Document Document
      {
        get => this.Doc;
        set
        {
          this.Doc = value;
          this.OnPropertyChanged(nameof (Document));
          this.OnPropertyChanged(isUpdateAllProp: true);
        }
      }

      public string NameContractor
      {
        get => this._nameContractor;
        set
        {
          this._nameContractor = value;
          this.OnPropertyChanged(nameof (NameContractor));
        }
      }

      public Decimal SumItems
      {
        get => this._sumItems;
        set
        {
          this._sumItems = value;
          this.OnPropertyChanged(nameof (SumItems));
        }
      }

      public Decimal SaleSumItems => SaleHelper.GetSumDocument(this.Document);

      public Decimal SumPayments
      {
        get
        {
          return this.Document.Payments.Where<Payments.Payment>((Func<Payments.Payment, bool>) (x => !x.IsDeleted)).Sum<Payments.Payment>((Func<Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
        }
      }

      public Decimal SumCredit
      {
        get
        {
          return SaleHelper.GetBuySumDocument(this.Document) - this.Document.Payments.Where<Payments.Payment>((Func<Payments.Payment, bool>) (x => !x.IsDeleted)).Sum<Payments.Payment>((Func<Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
        }
      }

      public Decimal SumCountItems
      {
        get => this._sumCountItems;
        set
        {
          this._sumCountItems = value;
          this.OnPropertyChanged(nameof (SumCountItems));
        }
      }

      public int SumCountNameItems
      {
        get => this.Document.Items.Count<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted));
      }

      public string NameUser
      {
        get => this._nameUser;
        set
        {
          this._nameUser = value;
          this.OnPropertyChanged(nameof (NameUser));
        }
      }

      public string Status
      {
        get
        {
          return WaybillsViewModel.Statuses.FirstOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x => x.Key == this.Doc.Status)).Value;
        }
      }

      private Document Doc { get; set; }

      public WaybillItemsInfoGrid(Document doc)
      {
        this.Document = doc;
        this.SumItems = doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.BuyPrice * x.Quantity));
        this.SumCountItems = doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
        this.NameContractor = doc.ContractorUid == Guid.Empty ? "" : WaybillsViewModel.ListClient.FirstOrDefault<Client>((Func<Client, bool>) (x => x.Uid == doc.ContractorUid))?.Name;
        this.NameUser = doc.UserUid == Guid.Empty ? "" : WaybillsViewModel.ListUsers.FirstOrDefault<Users.User>((Func<Users.User, bool>) (x => x.Uid == doc.UserUid))?.Alias;
      }
    }
  }
}
