// Decompiled with JetBrains decompiler
// Type: Gbs.MainWindowViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Users;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models;
using Gbs.Core.Devices.DisplayBuyers;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms;
using Gbs.Forms._shared;
using Gbs.Forms.ActionsPayments;
using Gbs.Forms.Cafe;
using Gbs.Forms.ClientOrder;
using Gbs.Forms.Clients;
using Gbs.Forms.Egais;
using Gbs.Forms.EntityGroupEdit.GoodGroup;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods;
using Gbs.Forms.Goods.GoodGroupEdit;
using Gbs.Forms.HomeOffice;
using Gbs.Forms.Inventory;
using Gbs.Forms.Lable;
using Gbs.Forms.License;
using Gbs.Forms.Main;
using Gbs.Forms.MovingStorage;
using Gbs.Forms.Other;
using Gbs.Forms.Productions;
using Gbs.Forms.Reports;
using Gbs.Forms.Reports.MasterReport;
using Gbs.Forms.Reports.SummaryReport;
using Gbs.Forms.Sale;
using Gbs.Forms.SendWaybills;
using Gbs.Forms.Settings;
using Gbs.Forms.Settings.Devices;
using Gbs.Forms.Users;
using Gbs.Forms.Waybills;
using Gbs.Forms.WriteOff;
using Gbs.Helpers;
using Gbs.Helpers.Barcodes;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Extensions.Linq;
using Gbs.Helpers.Factories;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs
{
  public class MainWindowViewModel : ViewModelWithForm
  {
    private string _searchQuery = string.Empty;
    private Timer _searchTimer = new Timer();
    private DateTime _searchStartDateTime = DateTime.MaxValue;
    private bool _searchInProgress;
    private bool _isEnableSearchBox = true;
    public static SecondMonitorViewModel MonitorViewModel = new SecondMonitorViewModel();
    private int _currentBasketIndex = 1;
    private double _fontSize;
    public Action SearchFocusAction;
    private Gbs.Core.ViewModels.Basket.Basket _currentBasket = new Gbs.Core.ViewModels.Basket.Basket();
    private List<SelectGood> _cacheSelectGood;
    public static LicenseHelper.LicenseInformation CachedLicenseInformation = LicenseHelper.GetInfo();

    public string SearchQuery
    {
      get => this._searchQuery;
      set
      {
        this._searchQuery = value;
        this.OnPropertyChanged(nameof (SearchQuery));
        if (this._searchQuery.Length < 3)
          return;
        int num = 500;
        if (BarcodeHelper.IsEan13Barcode(this._searchQuery))
          num = 100;
        this._searchStartDateTime = DateTime.Now.AddMilliseconds((double) num);
      }
    }

    private bool IsMainFormActive()
    {
      bool active = false;
      Application.Current?.Dispatcher?.Invoke((Action) (() => active = Application.Current.Windows.OfType<MainWindow>().Any<MainWindow>((Func<MainWindow, bool>) (f => f.IsActive))));
      return active;
    }

    private List<Gbs.Core.Entities.Goods.Good> GetGoodsSearchCache()
    {
      Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      List<Gbs.Core.Entities.Goods.Good> list = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.Group.IsCompositeGood)).ToList<Gbs.Core.Entities.Goods.Good>();
      if (!settings.Sales.AllowSalesToMinus)
        list = list.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x =>
        {
          if (!(x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (s => s.Stock)) > 0M))
          {
            if (!x.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set))
              return x.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service);
          }
          return true;
        })).ToList<Gbs.Core.Entities.Goods.Good>();
      return list;
    }

    public void StartSearch()
    {
      if (this._searchInProgress || this._searchQuery.Length < 3)
        return;
      string searchQuery = this._searchQuery;
      if (!this.IsMainFormActive())
        return;
      this.SearchQuery = string.Empty;
      this._searchInProgress = true;
      this._searchStartDateTime = DateTime.Now.AddYears(1);
      this.DoSearch(searchQuery);
      this._searchInProgress = false;
      this.SearchFocusAction();
    }

    private void DoSearch(string query)
    {
      string mainQuery = query;
      query = query.Trim('\r', '\n', ' ');
      Gbs.Core.Config.Settings sett = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      List<Gbs.Core.Entities.Goods.Good> goodsSearchCache = this.GetGoodsSearchCache();
      if (new BarcodeSearcher(this.CurrentBasket, goodsSearchCache).SearchByBarcodeAndAddToBasket(query, new Action(this.ScrollToSelectedRow)))
        return;
      Gbs.Core.Entities.Goods.Good singleGood = (!sett.Sales.IsSearchAllBarcode ? goodsSearchCache.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode == query)).ToList<Gbs.Core.Entities.Goods.Good>() : goodsSearchCache.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode.Trim() == query || x.Barcodes.Any<string>((Func<string, bool>) (b => string.Join<char>("", b.Where<char>(new Func<char, bool>(char.IsLetterOrDigit))).Trim().ToLower() == query)))).ToList<Gbs.Core.Entities.Goods.Good>()).SingleOrNull<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid != GlobalDictionaries.PercentForServiceGoodUid));
      if (singleGood != null)
      {
        if (!singleGood.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => (x.Stock > 0M || sett.Sales.AllowSalesToMinus) && !x.IsDeleted)).ToList<GoodsStocks.GoodStock>().Any<GoodsStocks.GoodStock>())
        {
          if (singleGood.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
          {
            if (singleGood.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range))
            {
              int num = (int) MessageBoxHelper.Show(Translate.MainWindowViewModel_Нет_доступных_остатков_для_данного_товара);
              return;
            }
          }
        }
        Application.Current?.Dispatcher?.Invoke((Action) (() => this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) new List<Gbs.Core.Entities.Goods.Good>()
        {
          singleGood
        })));
      }
      else
        Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Sale, mainQuery, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
        }));
    }

    private void SearchTimerInit()
    {
      this._searchTimer = new Timer()
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

    public bool IsEnableSearchBox
    {
      get => this._isEnableSearchBox;
      set
      {
        this._isEnableSearchBox = value;
        this.OnPropertyChanged(nameof (IsEnableSearchBox));
      }
    }

    public ICommand FiscalLastSaleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
          if (checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || checkPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None)
          {
            MessageBoxHelper.Warning(Translate.MainWindowViewModel_FiscalLastSaleCommand_Для_фискализации_продажи_необходимо_указать_модель_ККМ_в_настройках_программы__после_этого_повторите_операцию_);
          }
          else
          {
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.MainWindowViewModel_FiscalLastSaleCommand_Фискализация_последней_продажи);
              IEnumerable<Document> source = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Sale).Where<Document>((Func<Document, bool>) (x =>
              {
                Guid? uid3 = x.Section?.Uid;
                Guid uid4 = Sections.GetCurrentSection().Uid;
                return uid3.HasValue && uid3.GetValueOrDefault() == uid4;
              }));
              if (!source.Any<Document>())
              {
                progressBar.Close();
                MessageBoxHelper.Warning(Translate.MainWindowViewModel_FiscalLastSaleCommand_Нет_доступных_продаж_для_проведения_фискализации_);
              }
              else
              {
                Document document = source.OrderBy<Document, DateTime>((Func<Document, DateTime>) (x => x.DateTime)).Last<Document>();
                if (document.IsFiscal)
                {
                  progressBar.Close();
                  MessageBoxHelper.Warning(string.Format(Translate.MainWindowViewModel_FiscalLastSaleCommand_Последняя_продажа___0__уже_является_фискальной__повторить_действие_невозможно_, (object) document.Number));
                }
                else if (MessageBoxHelper.Question(string.Format(Translate.MainWindowViewModel_FiscalLastSaleCommand_, (object) document.Number, (object) document.DateTime.ToString("dd.MM.yyyy HH:mm"), (object) string.Join("\n", document.Items.Select<Gbs.Core.Entities.Documents.Item, string>((Func<Gbs.Core.Entities.Documents.Item, string>) (x => x.Good.Name))))) == MessageBoxResult.No)
                {
                  progressBar.Close();
                }
                else
                {
                  if (!SaleCardViewModel.PrintFiscalCheck(document))
                    return;
                  progressBar.Close();
                  ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.MainWindowViewModel_FiscalLastSaleCommand_Продажа_успешно_фискализирована__Данные_отправлены_в_ККМ_));
                }
              }
            }
          }
        }));
      }
    }

    public void UpdateInfo() => this.OnPropertyChanged(isUpdateAllProp: true);

    public Visibility VisibilityCheckUpdate
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Other.UpdateConfig.UpdateType != OtherConfig.UpdateType.NoUpdate ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityAllDiscountBtn
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsVisibilityAllDiscountBtn ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public string TitleMainWindow
    {
      get
      {
        string str = "   " + (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home ? Translate.MainWindowViewModel__ДОМ_ОФИС_ : "[" + Sections.GetCurrentSection().Name + "]");
        return PartnersHelper.ProgramName() + " : " + UidDb.GetUid().Value?.ToString() + str;
      }
    }

    public Visibility VisibilityButtonCafe
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Cafe ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand ShowCafeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmCafeMain().ShowDialog();
          TimerHelper.InitializeTimers(new Action(this.UpdateTime));
          MainWindowViewModel.MonitorViewModel.UpdateBasket(this.CurrentBasket);
          this.UpdateSelectGoods(false);
        }));
      }
    }

    public Visibility VisibilityItemsSelectGood
    {
      get
      {
        return !this.MenuItems.Any<MainWindowViewModel.SelectGoodView>() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public RowDefinition RowDefinitionSelectGood { get; set; }

    public Visibility VisibilitySelectPoint { get; set; } = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home ? Visibility.Visible : Visibility.Collapsed;

    public bool IsActiveMain
    {
      get => true;
      set
      {
      }
    }

    public ObservableCollection<MainWindowViewModel.SelectGoodView> MenuItems { get; set; } = new ObservableCollection<MainWindowViewModel.SelectGoodView>();

    public System.Windows.Controls.DataGrid BasketGrid { get; set; }

    public static string AlsoMenuKey => "AlsoMenu";

    public double FontSize
    {
      get => this._fontSize;
      set
      {
        this._fontSize = value;
        this.OnPropertyChanged(nameof (FontSize));
      }
    }

    public Visibility KkmMenuVisibility
    {
      get
      {
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        return checkPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility HiPosMenuVisibility
    {
      get
      {
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        return checkPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || checkPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.HiPos ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility EgaisMenuVisibility
    {
      get
      {
        EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
        GlobalDictionaries.Countries country = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country;
        return !egais.IsActive || country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility FindOrgByInnVisibility
    {
      get
      {
        DaData daData = new ConfigsRepository<Integrations>().Get().DaData;
        GlobalDictionaries.Countries country = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country;
        if (daData.IsActive)
        {
          if (country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Belarus, GlobalDictionaries.Countries.Kazakhstan))
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public Visibility AcquiringMenuVisibility
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Devices>().Get().AcquiringTerminal.Type != GlobalDictionaries.Devices.AcquiringTerminalTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility AcquiringServiceMenuVisibility
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().AcquiringTerminal.Type.IsEither<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.SBRF, GlobalDictionaries.Devices.AcquiringTerminalTypes.Sberbank) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SellerReportMenuVisibility
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home || !new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Users.IsSellerReport ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Gbs.Core.ViewModels.Basket.Basket CurrentBasket
    {
      get => this._currentBasket;
      set
      {
        this._currentBasket = value;
        MainWindowViewModel.MonitorViewModel.UpdateBasket(value);
      }
    }

    public int CurrentBasketIndex
    {
      get => this._currentBasketIndex;
      set
      {
        this._currentBasketIndex = value;
        this.OnPropertyChanged(nameof (CurrentBasketIndex));
      }
    }

    public Visibility BasketSwitcherVisibility { get; set; } = Visibility.Collapsed;

    public Dictionary<int, Gbs.Core.ViewModels.Basket.Basket> BasketsList { get; set; } = new Dictionary<int, Gbs.Core.ViewModels.Basket.Basket>();

    public int TotalBaskets => this.BasketsList.Count;

    public ICommand NextBasketCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.BasketsList.Count == 1)
            return;
          int basketIndex = this.CurrentBasketIndex >= this.BasketsList.Count ? 1 : this.CurrentBasketIndex + 1;
          Gbs.Helpers.Other.ConsoleWrite("next basket index: " + basketIndex.ToString());
          this.SwitchBasket(basketIndex);
          MainWindowViewModel.InitDisplayBuyer();
          this.CurrentBasket.SendDisplayBuyerNumbersInfo(this.CurrentBasket.TotalSum);
        }));
      }
    }

    public ICommand PrevBasketCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.BasketsList.Count == 1)
            return;
          int basketIndex = this.CurrentBasketIndex == 1 ? this.BasketsList.Count : this.CurrentBasketIndex - 1;
          Gbs.Helpers.Other.ConsoleWrite("prev basket index: " + basketIndex.ToString());
          this.SwitchBasket(basketIndex);
          MainWindowViewModel.InitDisplayBuyer();
          this.CurrentBasket.SendDisplayBuyerNumbersInfo(this.CurrentBasket.TotalSum);
        }));
      }
    }

    public ICommand OpenCreditList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new CreditListViewModel().ShowCreditList(this.CurrentBasket.Client.Client)));
      }
    }

    public ICommand SelectGoodCommand { get; set; }

    public ICommand CancelDocumentCommand { get; set; }

    public ICommand SaveSaleCommand { get; set; }

    public ICommand SelectClientCommand { get; set; }

    public ICommand PrintDocumentCommand { get; set; }

    public ICommand ExtraActionsCommand { get; set; }

    public ICommand ShowScalesWeightCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ScalesTestViewModel().TestScales()));
      }
    }

    public Visibility VisibilityScalesWeight
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Scale.IsShowBtnTestWeight ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Action ShowAlsoMenu { get; set; }

    public string UserOnlineString { get; set; } = string.Empty;

    public ICommand AddSelectGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (Data.GetDataBase())
          {
            MainWindowViewModel.SelectGoodView gObj = (MainWindowViewModel.SelectGoodView) obj;
            if (gObj.SelectGood.Uid == Guid.Empty && gObj.SelectGood.Good == null)
              this.UpdateSelectGoods(false);
            else if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsGroupSelectGood && (gObj.SelectGood.Good == null || gObj.SelectGood.Good.Uid == Guid.Empty))
            {
              int countSelectGood = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.CountSelectGood;
              List<SelectGood> list1 = new SelectGoodsRepository().GetActiveItems().Where<SelectGood>((Func<SelectGood, bool>) (x =>
              {
                Guid? uid3 = x.Good?.Group.Uid;
                Guid uid4 = gObj.SelectGood.Uid;
                if ((uid3.HasValue ? (uid3.GetValueOrDefault() == uid4 ? 1 : 0) : 0) == 0)
                  return false;
                Gbs.Core.Entities.Goods.Good good = x.Good;
                return good != null && !__nonvirtual (good.IsDeleted);
              })).ToList<SelectGood>();
              List<MainWindowViewModel.SelectGoodView> list2 = new List<MainWindowViewModel.SelectGoodView>();
              list2.Insert(0, new MainWindowViewModel.SelectGoodView()
              {
                SelectGood = new SelectGood()
                {
                  DisplayName = Translate.MainWindowViewModel_____НАЗАД,
                  Uid = Guid.Empty,
                  Index = -1
                },
                FontWeight = "Bold"
              });
              Guid? uid = gObj?.SelectGood?.Good?.Uid;
              Guid empty = Guid.Empty;
              if ((uid.HasValue ? (uid.GetValueOrDefault() == empty ? 1 : 0) : 0) != 0)
                list2.AddRange((IEnumerable<MainWindowViewModel.SelectGoodView>) new ObservableCollection<MainWindowViewModel.SelectGoodView>((IEnumerable<MainWindowViewModel.SelectGoodView>) this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Uid == Guid.Empty)).Select<SelectGood, MainWindowViewModel.SelectGoodView>((Func<SelectGood, MainWindowViewModel.SelectGoodView>) (x => new MainWindowViewModel.SelectGoodView()
                {
                  SelectGood = x
                })).OrderBy<MainWindowViewModel.SelectGoodView, int>((Func<MainWindowViewModel.SelectGoodView, int>) (x => x.SelectGood.Index))));
              else
                list2.AddRange(list1.Select<SelectGood, MainWindowViewModel.SelectGoodView>((Func<SelectGood, MainWindowViewModel.SelectGoodView>) (x => new MainWindowViewModel.SelectGoodView()
                {
                  SelectGood = x
                })).OrderBy<MainWindowViewModel.SelectGoodView, int>((Func<MainWindowViewModel.SelectGoodView, int>) (x => x.SelectGood.Index)).Take<MainWindowViewModel.SelectGoodView>(countSelectGood));
              this.MenuItems = new ObservableCollection<MainWindowViewModel.SelectGoodView>(list2);
              this.OnPropertyChanged("MenuItems");
            }
            else
            {
              Gbs.Core.Entities.Goods.Good good = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == gObj.SelectGood.Good.Uid));
              if (!new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.AllowSalesToMinus && good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)) <= 0M && good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service)
              {
                if (!good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit))
                {
                  MessageBoxHelper.Warning(Translate.FrmSelectGoodStock_У_данного_товара_нет_положительных_остатков__выбор_невозможен_);
                  return;
                }
              }
              this.AddItemInBasketByTap((IEnumerable<Gbs.Core.Entities.Goods.Good>) new List<Gbs.Core.Entities.Goods.Good>()
              {
                good
              }, checkMinus: false, tapUid: gObj.SelectGood.ParentUid);
              this.SearchFocusAction();
              this.UpdateSelectGoods(false);
            }
          }
        }));
      }
    }

    public MainWindowViewModel()
    {
    }

    public string Time => TimerHelper.CurrentTime;

    public string Date => TimerHelper.CurrentDate;

    public void UpdateTime()
    {
      this.OnPropertyChanged("Time");
      this.OnPropertyChanged("Date");
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<MainWindow>())
        return;
      this.SearchQuery = barcode;
      this.StartSearch();
    }

    public MainWindowViewModel(System.Windows.Controls.DataGrid grid)
    {
      try
      {
        ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.ComPortScannerOnBarcodeChanged));
        TimerHelper.InitializeTimers(new Action(this.UpdateTime));
        MainWindowViewModel.MonitorViewModel.ShowSecondForm();
        this.SearchTimerInit();
        this.OnPropertyChanged(nameof (TitleMainWindow));
        this.CreateBasketsList();
        this.CurrentBasket = this.BasketsList[this.CurrentBasketIndex];
        this.BasketGrid = grid;
        this.GetStringOnlineUser();
        this.SetPrintCheckCheckbox();
        this.CancelDocumentCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Cancel()));
        this.SelectGoodCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Sale, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
          this.SearchFocusAction();
        }));
        this.SelectClientCommand = (ICommand) new RelayCommand((Action<object>) (x =>
        {
          MainWindowViewModel.DoWithPause();
          this.GetClient();
          this.SearchFocusAction();
        }));
        this.SaveSaleCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SaveSale();
          this.SearchFocusAction();
          MainWindowViewModel.InitDisplayBuyer();
          this.CurrentBasket.SendDisplayBuyerNumbersInfo(0M);
        }));
        this.PrintDocumentCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.PrintDocument()));
        this.ExtraActionsCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (((ICollection) obj).Count == 0)
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          else if (((ICollection) obj).Count > 1)
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          else
            this.ShowAlsoMenu();
        }));
        this.UpdateSelectGoods(true);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка на главной форме");
      }
    }

    private void CreateBasketsList()
    {
      int basketsCountInMainForm = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.BasketsCountInMainForm;
      int count = this.BasketsList.Count;
      if (count == basketsCountInMainForm)
        return;
      if (count > basketsCountInMainForm)
      {
        MessageBoxHelper.Warning(Translate.MainWindowViewModel_Для_уменьшения_количества_корзин_необходим_перезапуск_программы);
      }
      else
      {
        for (int index = count + 1; index < basketsCountInMainForm + 1; ++index)
        {
          this.BasketsList.Add(index, new Gbs.Core.ViewModels.Basket.Basket());
          this.SetPrintCheckCheckbox(index);
        }
        if (basketsCountInMainForm > 1)
          this.BasketSwitcherVisibility = Visibility.Visible;
        this.OnPropertyChanged("TotalBaskets");
        this.OnPropertyChanged("BasketSwitcherVisibility");
        this.OnPropertyChanged("BasketsList");
      }
    }

    private void SwitchBasket(int basketIndex)
    {
      Gbs.Core.ViewModels.Basket.Basket baskets = this.BasketsList[basketIndex];
      this.CurrentBasketIndex = basketIndex;
      this.CurrentBasket = baskets;
      bool isPrintCheck = this.CurrentBasket.IsPrintCheck;
      this.SetPrintCheckCheckbox();
      if (this.CurrentBasket.IsEnablePrintCheck)
        this.CurrentBasket.IsPrintCheck = isPrintCheck;
      this.OnPropertyChanged("CurrentBasket");
      this.SearchFocusAction();
    }

    private void Cancel(bool isMsg = true)
    {
      this.IsEnableSearchBox = true;
      MainWindowViewModel.DoWithPause();
      (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Gbs.Core.Entities.Actions.CancelSale);
      if (!access.Result)
        return;
      if ((isMsg ? (int) MessageBoxHelper.Question(Translate.MainWindowViewModel_Вы_действительно_хотите_отменить_) : 6) == 6)
      {
        this.CurrentBasket.AddActionsHistoryByCancel(access.User);
        this.CurrentBasket.CheckOrder();
        this.ClearInfoDocument();
        ProgressBarHelper.Close();
      }
      this.SearchFocusAction();
    }

    private async void UpdateSelectGoods(bool isLoading)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        return;
      await Task.Run((Action) (() =>
      {
        try
        {
          if (isLoading || this._cacheSelectGood == null)
          {
            this._cacheSelectGood = new List<SelectGood>();
            EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
            if (egais.IsActive && egais.IsShowTapInSelectGood)
            {
              List<InfoToTapBeer> list = new InfoTapBeerRepository().GetActiveItems().Where<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => x.GoodUid != Guid.Empty && !x.IsDeleted && !x.Tap.IsDeleted)).ToList<InfoToTapBeer>();
              if (list.Any<InfoToTapBeer>())
                this._cacheSelectGood = new List<SelectGood>((IEnumerable<SelectGood>) list.Select<InfoToTapBeer, SelectGood>((Func<InfoToTapBeer, SelectGood>) (x =>
                {
                  return new SelectGood()
                  {
                    DisplayName = x.Tap.Name,
                    Index = x.Tap.Index,
                    Good = new Gbs.Core.Entities.Goods.Good()
                    {
                      Uid = x.ChildGoodUid
                    },
                    Uid = Guid.Empty,
                    ParentUid = x.Tap.Uid
                  };
                })).OrderBy<SelectGood, int>((Func<SelectGood, int>) (x => x.Index)));
            }
            this._cacheSelectGood.AddRange(new SelectGoodsRepository().GetActiveItems().Where<SelectGood>((Func<SelectGood, bool>) (x =>
            {
              Gbs.Core.Entities.Goods.Good good = x.Good;
              return good != null && !good.IsDeleted;
            })));
          }
          int countSelectGood = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.CountSelectGood;
          if (countSelectGood <= 0 && !this.MenuItems.Any<MainWindowViewModel.SelectGoodView>())
          {
            Application.Current.Dispatcher?.Invoke((Action) (() => this.RowDefinitionSelectGood.Height = new GridLength(0.0)));
            this.OnPropertyChanged("VisibilityItemsSelectGood");
          }
          else
          {
            if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsGroupSelectGood)
            {
              this.MenuItems = new ObservableCollection<MainWindowViewModel.SelectGoodView>(this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Uid != Guid.Empty)).GroupBy<SelectGood, Guid>((Func<SelectGood, Guid>) (x => x.Good.Group.Uid)).Select<IGrouping<Guid, SelectGood>, MainWindowViewModel.SelectGoodView>((Func<IGrouping<Guid, SelectGood>, MainWindowViewModel.SelectGoodView>) (x => new MainWindowViewModel.SelectGoodView()
              {
                SelectGood = new SelectGood()
                {
                  DisplayName = x.First<SelectGood>().Good.Group.Name + string.Format(" ({0})", (object) this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (c => c.Uid != Guid.Empty)).Count<SelectGood>((Func<SelectGood, bool>) (c => c.Good.Group.Uid == x.First<SelectGood>().Good.Group.Uid))),
                  Uid = x.First<SelectGood>().Good.Group.Uid
                },
                FontWeight = "Bold"
              })).OrderBy<MainWindowViewModel.SelectGoodView, string>((Func<MainWindowViewModel.SelectGoodView, string>) (x => x.SelectGood.DisplayName)).ToList<MainWindowViewModel.SelectGoodView>().Take<MainWindowViewModel.SelectGoodView>(countSelectGood));
              if (this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Uid == Guid.Empty)).ToList<SelectGood>().Any<SelectGood>())
              {
                ObservableCollection<MainWindowViewModel.SelectGoodView> menuItems = this.MenuItems;
                menuItems.Insert(0, new MainWindowViewModel.SelectGoodView()
                {
                  FontWeight = "Bold",
                  SelectGood = new SelectGood()
                  {
                    DisplayName = "Краны",
                    Uid = Guid.Empty,
                    Good = new Gbs.Core.Entities.Goods.Good() { Uid = Guid.Empty }
                  }
                });
              }
            }
            else
            {
              List<SelectGood> list = this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Uid == Guid.Empty)).ToList<SelectGood>();
              this.MenuItems = new ObservableCollection<MainWindowViewModel.SelectGoodView>(this._cacheSelectGood.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Uid != Guid.Empty)).Select<SelectGood, MainWindowViewModel.SelectGoodView>((Func<SelectGood, MainWindowViewModel.SelectGoodView>) (x => new MainWindowViewModel.SelectGoodView()
              {
                SelectGood = x,
                FontWeight = x.Uid == Guid.Empty ? "Bold" : ""
              })).OrderBy<MainWindowViewModel.SelectGoodView, int>((Func<MainWindowViewModel.SelectGoodView, int>) (x => x.SelectGood.Index)).Take<MainWindowViewModel.SelectGoodView>(countSelectGood));
              if (list.Any<SelectGood>())
                this.MenuItems = new ObservableCollection<MainWindowViewModel.SelectGoodView>(list.Select<SelectGood, MainWindowViewModel.SelectGoodView>((Func<SelectGood, MainWindowViewModel.SelectGoodView>) (x => new MainWindowViewModel.SelectGoodView()
                {
                  SelectGood = x,
                  FontWeight = "Bold"
                })).Concat<MainWindowViewModel.SelectGoodView>((IEnumerable<MainWindowViewModel.SelectGoodView>) this.MenuItems.ToList<MainWindowViewModel.SelectGoodView>()));
            }
            this.OnPropertyChanged("MenuItems");
            Application.Current.Dispatcher?.Invoke((Action) (() =>
            {
              if (this.RowDefinitionSelectGood.Height.Value < 50.0 && this.MenuItems.Any<MainWindowViewModel.SelectGoodView>())
              {
                this.RowDefinitionSelectGood.Height = new GridLength(60.0);
              }
              else
              {
                if (this.MenuItems.Any<MainWindowViewModel.SelectGoodView>())
                  return;
                this.RowDefinitionSelectGood.Height = new GridLength(0.0);
                this.OnPropertyChanged("VisibilityItemsSelectGood");
              }
            }));
            this.OnPropertyChanged("VisibilityItemsSelectGood");
          }
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка загрузки избранных товаров");
        }
      }));
    }

    private void PrintDocument()
    {
      MainWindowViewModel.DoWithPause();
      (bool Result, Gbs.Core.Entities.Users.User user) = Gbs.Helpers.Other.GetUserForDocument(Gbs.Core.Entities.Actions.SaleSave);
      if (!Result)
        return;
      this.CurrentBasket.User = user;
      new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForSaleDocument(new DocumentsFactory().Create(this.CurrentBasket)), user);
    }

    private void GetStringOnlineUser()
    {
      this.UserOnlineString = string.Empty;
      foreach (Gbs.Core.Entities.Users.User user in WindowWithSize.ListOnlineUser)
        this.UserOnlineString = this.UserOnlineString + user.Alias + "; ";
      this.OnPropertyChanged("UserOnlineString");
      new WindowWithSize().UpdateColumnStock(this.BasketGrid, out Visibility _, (ContextMenu) this.BasketGrid.FindResource((object) "ContextMenuGrid"));
    }

    public void SetFontSize()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        List<Gbs.Core.Entities.Users.User> byQuery = new UsersRepository(dataBase).GetByQuery(dataBase.GetTable<USERS>().Where<USERS>(System.Linq.Expressions.Expression.Lambda<Func<USERS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso(!x.IS_DELETED && !x.IS_KICKED, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.SECTION_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<System.Linq.Expressions.Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression)));
        double num = 12.0;
        if (byQuery.Any<Gbs.Core.Entities.Users.User>())
          num = byQuery.Count == 1 ? (double) byQuery.Single<Gbs.Core.Entities.Users.User>().FontSize : (double) byQuery.Max<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, Decimal>) (x => x.FontSize));
        this.FontSize = num;
      }
    }

    public static void DoWithPause()
    {
      if (LicenseHelper.GetInfo().IsActive)
        return;
      new LicenseNotificationViewModel().Show();
    }

    private void AddItemInBasketByTap(
      IEnumerable<Gbs.Core.Entities.Goods.Good> goods,
      bool addAllCount = false,
      bool checkMinus = true,
      Guid tapUid = default (Guid))
    {
      new BasketHelper(this.CurrentBasket).AddItemToBasket(goods, addAllCount, checkMinus, tapUid);
      this.ScrollToSelectedRow();
      this.IsEnableSearchBox = true;
    }

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool addAllCount = false, bool checkMinus = true)
    {
      this.AddItemInBasketByTap(goods, addAllCount, checkMinus);
    }

    private void ScrollToSelectedRow() => this.BasketGrid.ScrollToSelectedRow();

    private void SaveSale()
    {
      try
      {
        MainWindowViewModel.DoWithPause();
        LogHelper.Debug("Начинаю сохранение продажи");
        this.IsEnableSearchBox = false;
        this.IsActiveMain = false;
        ActionResult actionResult = this.CurrentBasket.Save();
        this.IsActiveMain = true;
        try
        {
          this.IsEnableSearchBox = true;
        }
        catch (ArithmeticException ex)
        {
          LogHelper.WriteError((Exception) ex, "Ошибка IsEnableSearchBox");
        }
        if (actionResult.Result != ActionResult.Results.Ok)
        {
          string str = string.Join(Gbs.Helpers.Other.NewLine(), (IEnumerable<string>) actionResult.Messages);
          if (str.IsNullOrEmpty())
            return;
          switch (actionResult.Result)
          {
            case ActionResult.Results.Cancel:
              break;
            case ActionResult.Results.Warning:
              MessageBoxHelper.Warning(str);
              break;
            case ActionResult.Results.Error:
              MessageBoxHelper.Error(str);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        else
        {
          this.ClearInfoDocument();
          this.SearchFocusAction();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
      finally
      {
        if (!this.IsActiveMain)
          this.IsActiveMain = true;
      }
    }

    private void ClearInfoDocument()
    {
      this.SearchQuery = string.Empty;
      this.CurrentBasket = new Gbs.Core.ViewModels.Basket.Basket();
      this.BasketsList[this.CurrentBasketIndex] = this.CurrentBasket;
      foreach (KeyValuePair<int, Gbs.Core.ViewModels.Basket.Basket> baskets in this.BasketsList)
        baskets.Value.SaleNumber = this.CurrentBasket.SaleNumber;
      this.OnPropertyChanged("CurrentBasket");
      this.SetPrintCheckCheckbox();
      MainWindowViewModel.InitDisplayBuyer();
      this.CurrentBasket.SendDisplayBuyerNumbersInfo(0M);
    }

    private void SetPrintCheckCheckbox(int basketIndex = 0)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      Gbs.Core.ViewModels.Basket.Basket basket = basketIndex == 0 ? this.CurrentBasket : this.BasketsList[basketIndex];
      if (devices.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.None)
      {
        basket.IsEnablePrintCheck = false;
        basket.IsPrintCheck = false;
      }
      else if (devices.CheckPrinter.IsShowPrintConfirmationForm)
      {
        basket.IsEnablePrintCheck = false;
        basket.IsPrintCheck = true;
      }
      else if (!devices.CheckPrinter.PrintCheckOnEverySale)
      {
        basket.IsEnablePrintCheck = true;
        basket.IsPrintCheck = false;
      }
      else if (devices.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && devices.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.None && !devices.CheckPrinter.FiscalKkm.AllowSalesWithoutCheck)
      {
        basket.IsEnablePrintCheck = false;
        basket.IsPrintCheck = true;
      }
      else
      {
        basket.IsEnablePrintCheck = true;
        basket.IsPrintCheck = devices.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.None && devices.CheckPrinter.PrintCheckOnEverySale;
      }
    }

    public ICommand EditBonusesClientCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.CurrentBasket.Client != null)
            return;
          MessageBoxHelper.Warning(Translate.MainWindowViewModel_EditBonusesClientCommand_Необходимо_выбрать_контакт_);
        }));
      }
    }

    private void GetClient()
    {
      this.OnPropertyChanged("IsCheckedClient");
      if (!this.CurrentBasket.IsCheckedClient)
      {
        this.CurrentBasket.Client = (ClientAdnSum) null;
      }
      else
      {
        (Gbs.Core.Entities.Clients.Client client, bool result) client1 = new FrmSearchClient().GetClient();
        Gbs.Core.Entities.Clients.Client client2 = client1.client;
        int num = client1.result ? 1 : 0;
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmCafeViewModel_Загрузка_данных_о_покупателе);
        if (num != 0)
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            this.CurrentBasket.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(client2.Uid);
            ProgressBarHelper.Close();
          }
        }
        this.CurrentBasket.IsCheckedClient = this.CurrentBasket.Client != null;
        progressBar.Close();
      }
    }

    public void SearchByMarkCode()
    {
      bool flag = false;
      CultureInfo currentInputLanguage = InputLanguageManager.Current.CurrentInputLanguage;
      if ((Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
      {
        flag = true;
        KeyboardLayoutHelper.IsDownCapsLock();
      }
      IEnumerable availableInputLanguages = InputLanguageManager.Current.AvailableInputLanguages;
      CultureInfo cultureInfo = availableInputLanguages != null ? availableInputLanguages.OfType<CultureInfo>().FirstOrDefault<CultureInfo>((Func<CultureInfo, bool>) (l => l.Name.StartsWith("en"))) : (CultureInfo) null;
      if (cultureInfo != null)
      {
        LogHelper.Debug("Устанавливаю EN раскладку для ввода кода маркировки");
        InputLanguageManager.Current.CurrentInputLanguage = cultureInfo;
      }
      (bool result, string output) = MessageBoxHelper.Input("", Translate.MainWindowViewModel_SearchByMarkCode_Отсканируйте_код_маркировки_с_упаковки_товара);
      if (result)
        this.SearchQuery = output;
      if (currentInputLanguage != null)
        InputLanguageManager.Current.CurrentInputLanguage = currentInputLanguage;
      if (!flag || (Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
        return;
      KeyboardLayoutHelper.IsDownCapsLock();
    }

    public static void InitDisplayBuyer()
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().DisplayPayer.Type == DisplayBuyerTypes.None)
        return;
      using (DisplayBuyerHelper displayBuyerHelper = new DisplayBuyerHelper((IConfig) new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
        displayBuyerHelper.WriteText(new List<string>()
        {
          Translate.MainWindowViewModel_InitDisplayBuyer_Добро_пожаловать_
        });
    }

    private Gbs.Core.Config.Settings SettingsConfig { get; set; } = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();

    public bool IsEnabledChangeTheme
    {
      get => new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsSwitchThemeForClickToTime;
    }

    public ICommand Settings
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmSettings().ShowSetting();
          ConfigsCache.GetInstance().ReloadAllCache();
          this.SettingsConfig = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
          this.OnPropertyChanged("KkmMenuVisibility");
          this.OnPropertyChanged("HiPosMenuVisibility");
          this.OnPropertyChanged("EgaisMenuVisibility");
          this.OnPropertyChanged("FindOrgByInnVisibility");
          this.OnPropertyChanged("AcquiringMenuVisibility");
          this.OnPropertyChanged("AcquiringServiceMenuVisibility");
          this.OnPropertyChanged("SellerReportMenuVisibility");
          this.OnPropertyChanged("VisibilityButtonCafe");
          this.OnPropertyChanged("TitleMainWindow");
          this.OnPropertyChanged("VisibilityCheckUpdate");
          this.OnPropertyChanged("MarkedLableMenuVisibility");
          this.OnPropertyChanged("VisibilityScalesWeight");
          this.OnPropertyChanged("VisibilityAllDiscountBtn");
          this.OnPropertyChanged("VisibilitySeparatorService");
          this.OnPropertyChanged("IsEnabledChangeTheme");
          this.OnPropertyChanged("MenuShowRemoteCatalogVisibility");
          this.UpdateVisibilityService();
          this.CurrentBasket.UpdateVisibilityStorage();
          this.SetPrintCheckCheckbox();
          this.SetFontSize();
          TimerHelper.InitializeTimers(new Action(this.UpdateTime));
          this.UpdateSelectGoods(true);
          this.CreateBasketsList();
          this.GetStringOnlineUser();
          CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.ExtraPriceRules);
          CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.DiscountRules);
          this.CurrentBasket.ReCalcTotals();
          Action closeAction = MainWindowViewModel.MonitorViewModel.CloseAction;
          if (closeAction != null)
            closeAction();
          MainWindowViewModel.MonitorViewModel.ShowSecondForm();
          DataBaseHelper.CreatePropertiesForCountry();
          try
          {
            ComPortScanner.Stop();
            ComPortScanner.Start();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex);
          }
          MainWindowViewModel.InitDisplayBuyer();
        }));
      }
    }

    public Visibility VkAndChangelogMenuItemsVisibility
    {
      get
      {
        return PartnersHelper.IsPartner() || new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Ukraine || Vendor.GetConfig() != null ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SendDataSupportMenuVisibility
    {
      get
      {
        if (PartnersHelper.IsPartner())
          return Visibility.Collapsed;
        Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        return settings.Interface.Country == GlobalDictionaries.Countries.Ukraine || settings.Interface.Country == GlobalDictionaries.Countries.Kazakhstan || Vendor.GetConfig() != null ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility MainPageLinkMenuItemVisibility
    {
      get
      {
        VendorConfig config = Vendor.GetConfig();
        if (config != null)
        {
          List<VendorConfig.LinkItem> links = config.Links;
          if ((links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.Main))?.Value : (string) null) == null)
            return Visibility.Collapsed;
        }
        return Visibility.Visible;
      }
    }

    public Visibility KbLinkMenuItemVisibility
    {
      get
      {
        VendorConfig config = Vendor.GetConfig();
        if (config != null)
        {
          List<VendorConfig.LinkItem> links = config.Links;
          if ((links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.KB))?.Value : (string) null) == null)
            return Visibility.Collapsed;
        }
        return Visibility.Visible;
      }
    }

    public Visibility SupportLinkMenuItemVisibility
    {
      get
      {
        VendorConfig config = Vendor.GetConfig();
        if (config != null)
        {
          List<VendorConfig.LinkItem> links = config.Links;
          if ((links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.Support))?.Value : (string) null) == null)
            return Visibility.Collapsed;
        }
        return Visibility.Visible;
      }
    }

    public ICommand UsersShow
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          GlobalDictionaries.Mode modeProgram = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram;
          new FrmLoginUser().ShowLogin(out Gbs.Core.Entities.Users.User _, modeProgram == GlobalDictionaries.Mode.Home);
          this.GetStringOnlineUser();
          CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllUsers);
          this.SetFontSize();
        }));
      }
    }

    public ICommand PointShow
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (Gbs.Helpers.Other.IsAnyForm())
          {
            if (MessageBoxHelper.Show(Translate.MainWindowViewModel_Для_того__чтобы_сменить_базу_данных__нужно_закрыть_все_вкладки_программы__Закрыть_автоматически_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
              return;
            Gbs.Helpers.Other.CloseAllForm();
          }
          if (this.CurrentBasket.Items.Any<BasketItem>() && MessageBoxHelper.Show(Translate.MainWindowViewModel_После_смены_базы_данных_корзина_будет_очищена__Вы_хотите_продолжить_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No || !new PointSelectViewModel().ShowLogin())
            return;
          if (HomeOfficeHelper.IsAuthRequire)
            this.UsersShow.Execute((object) null);
          this.OnPropertyChanged("TitleMainWindow");
          this.UpdateSelectGoods(true);
          this.Cancel(false);
        }));
      }
    }

    public ICommand ShowSelectGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmFavoritesGoods().ShowCard(new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.UpdateSelectGoods(true);
        }));
      }
    }

    public ICommand Exit
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmCloseProgram().GetClosed()));
      }
    }

    public ICommand SummaryReport
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmSummaryReport().ShowReport()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand SalesJournal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new SaleJournalViewModel().ShowCard(DateTime.Now, DateTime.Now);
        }));
      }
    }

    public ICommand SellerReport
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new SellerReportViewModel().ShowSellReport();
        }));
      }
    }

    public ICommand ReportWizard
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new MasterReportViewModel().ShowReport();
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand CustomReports
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          throw new NotImplementedException();
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand RemoveCash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
          new FrmRemoveCash().RemoveCash(ref payment, true);
        }));
      }
    }

    public ICommand DepositСash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new FrmRemoveCash().InsertCash(true);
        }));
      }
    }

    public ICommand SendСash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new FrmRemoveCash().SendCash();
        }));
      }
    }

    public ICommand CountKassa
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          ReCalcCashAccountHelper.DoReCalcCashAccount(true);
        }));
      }
    }

    public ICommand GetKkmXReport
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => KkmViewHelper.GetKkmXReport()));
    }

    public ICommand GetKkmZReport
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => KkmViewHelper.GetKkmZReport()));
    }

    public ICommand SetOnlineModePrro
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.SetModePrro(true)));
    }

    public ICommand SetOfflineModePrro
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.SetModePrro(false)));
    }

    public ICommand OpenPrroInBrowser
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          LanConnection lanPort = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort;
          string url = (lanPort.UrlAddress.IsNullOrEmpty() ? "http://127.0.0.1" : lanPort.UrlAddress) + ":11001";
          if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            url = "http://" + url;
          NetworkHelper.OpenUrl(url);
        }));
      }
    }

    private void SetModePrro(bool isOnline)
    {
      try
      {
        (bool Result, Gbs.Core.Entities.Users.User User) = Gbs.Helpers.Other.GetUserForDocument(Gbs.Core.Entities.Actions.PrintKkmReport);
        if (!Result)
          return;
        KkmHelper.UserUid = User.Uid;
        HiPos hiPos = new HiPos();
        hiPos.Connect(false, new ConfigsRepository<Gbs.Core.Config.Devices>().Get());
        hiPos.GetStatus();
        hiPos.SetModePrro(isOnline);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.MainWindowViewModel_Режим_работы_ПРРО_изменен_));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось получить изментить режим работы ПРРО");
      }
    }

    public ICommand CatalogGoods
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FrmGoodsCatalog.UpdateSelectGoods = new Action<bool>(this.UpdateSelectGoods);
          new FrmGoodsCatalog().ShowCatalog();
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand GroupsGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FormGroup().ShowDialog()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand NewWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Gbs.Core.Entities.Actions.WaybillAdd);
          if (!access.Result)
            return;
          new FrmWaybillCard().ShowCardWaybill(Guid.Empty, out Document _, access.User);
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand NewReturnWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Gbs.Core.Entities.Actions.WaybillAdd);
          if (!access.Result)
            return;
          new FrmWaybillCard().ShowCardWaybill(Guid.Empty, out Document _, access.User, isReturnBuy: true);
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand JornalWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmWaybillsList().ShowWaybills()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand JornalReturnWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmWaybillsList().ShowReturnWaybills()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand NewClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new FrmClientCard().ShowCard(Guid.Empty, out ClientAdnSum _);
        }));
      }
    }

    public ICommand GroupsClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmClientGroup().ShowDialog()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand CatalogClients
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmListClients().ShowClientsList()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand Borrower
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          new CreditListViewModel().ShowCreditList();
        }));
      }
    }

    public ICommand FindOrgByInn
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          DaDataRepository.Search();
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand SalesOrder
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ClientOrderList().ShowList(this.CurrentBasket)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand SalesOrderCard
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ClientOrderViewModel().ShowOrder(Guid.Empty, out Document _)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand ProductionCard
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ProductionCardViewModel().ShowCard(Guid.Empty, out Document _, out List<Document> _)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand EgaisTtnList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new EgaisTtnListViewModel().ShowEgaisTtn()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand EgaisWriteOff
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new EgaisWriteOffViewModel().Show()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand EgaisWriteOffList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new EgaisWriteOffListViewModel().Show()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand EgaisListOpenKegaBeer
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new LogOpeningBeerViewModel().ShowLog()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand EgaisManagerForTapBeer
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new ManagementForTapBeerViewModel().Show();
          this.UpdateSelectGoods(true);
          CacheHelper.Clear(CacheHelper.CacheTypes.AllGoods);
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand ProductionList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ProductionListViewModel().Show()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand SpeedProduction
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new SpeedProductionViewModel().ShowProductionForm()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand PriceTag
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLablePrint().Print(LablePrintViewModel.Types.PriceTags)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand Lable
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLablePrint().Print(LablePrintViewModel.Types.Labels)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand Packing
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLablePrint().Print(LablePrintViewModel.Types.Packing)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MarkedLable
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = Translate.MainWindowViewModel_Файл_с_кодами____csv_ + "|*.csv",
            Multiselect = false
          };
          if (!openFileDialog.ShowDialog().GetValueOrDefault())
            return;
          string fileName = openFileDialog.FileName;
          string str = FileSystemHelper.TempFolderPath();
          string path = Path.Combine(str, "dataMatrix.code");
          string destFileName = path;
          File.Copy(fileName, destFileName);
          new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForMarkedLable(((IEnumerable<string>) File.ReadAllLines(path)).Select<string, string>((Func<string, string>) (x => x.Split('\t', ' ')[0]))), (Gbs.Core.Entities.Users.User) null);
          Directory.Delete(str, true);
        }), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand ProgramInfo
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
          Gbs.Core.Config.DataBase dbConfig = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
          string getVersionUpdate = GlobalDictionaries.GetVersionUpdateDictionary[settings.Other.UpdateConfig.VersionUpdate];
          string str = GlobalDictionaries.ModeProgramDictionary().SingleOrDefault<KeyValuePair<GlobalDictionaries.Mode, string>>((Func<KeyValuePair<GlobalDictionaries.Mode, string>, bool>) (x => x.Key == dbConfig.ModeProgram)).Value;
          int version = VersionDb.GetVersion();
          int num = (int) MessageBoxHelper.Show(string.Format(Translate.MainWindowViewModel_aboutInfo, (object) PartnersHelper.ProgramName(), (object) ApplicationInfo.GetInstance().AppVersion, (object) getVersionUpdate, (object) str, (object) version));
        }));
      }
    }

    public ICommand CheckUpdateCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => Task.Run(new Action(UpdateHelper.HandCheckUpdate))));
      }
    }

    public ICommand LicenseInfo
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLicenseInfo().ShowDialog()));
      }
    }

    public ICommand ChangeLog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => FileSystemHelper.OpenSite("https://gbsmarket.ru/changelog/?utm_source=desktop_app&utm_medium=interface")));
      }
    }

    public ICommand WriteSupport
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          VendorConfig config = Vendor.GetConfig();
          if (config != null)
          {
            List<VendorConfig.LinkItem> links = config.Links;
            string url = links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.Support))?.Value : (string) null;
            if (url != null)
            {
              NetworkHelper.OpenUrl(url);
              return;
            }
          }
          PartnersHelper.OpenPage(PartnersHelper.PageTypes.Support);
        }));
      }
    }

    public ICommand OnlineHelp
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          VendorConfig config = Vendor.GetConfig();
          if (config != null)
          {
            List<VendorConfig.LinkItem> links = config.Links;
            string url = links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.KB))?.Value : (string) null;
            if (url != null)
            {
              NetworkHelper.OpenUrl(url);
              return;
            }
          }
          PartnersHelper.OpenPage(PartnersHelper.PageTypes.KnowlageBase);
        }));
      }
    }

    public ICommand Site
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          VendorConfig config = Vendor.GetConfig();
          if (config != null)
          {
            List<VendorConfig.LinkItem> links = config.Links;
            string url = links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.Main))?.Value : (string) null;
            if (url != null)
            {
              NetworkHelper.OpenUrl(url);
              return;
            }
          }
          PartnersHelper.OpenPage(PartnersHelper.PageTypes.MainPage);
        }));
      }
    }

    public ICommand Vk
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => FileSystemHelper.OpenSite("https://vk.com/gbsmarket")));
      }
    }

    public ICommand SendDataForSupport
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new SendDataForSupportViewModel().Show()));
      }
    }

    public ICommand AcquiringCloseSessionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (AcquiringHelper acquiringHelper = new AcquiringHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
            acquiringHelper.CloseSession();
        }));
      }
    }

    public ICommand AcquiringServiceMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (AcquiringHelper acquiringHelper = new AcquiringHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
            acquiringHelper.ShowServiceMenu();
        }));
      }
    }

    public ICommand AcquiringGetReportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (AcquiringHelper acquiringHelper = new AcquiringHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
            acquiringHelper.GetReport();
        }));
      }
    }

    public ICommand MenuInventoryJournal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmInventoryJournal().ShowInventoryJournal()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuInventoryDo
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new FrmInventoryCard_v2().ShowCard((Document) null, (Func<bool>) null)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuWriteOffJournal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmWriteOffJournal().ShowCard()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuWriteOffGoods
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new FrmWriteOffCard().ShowCard(Guid.Empty, out Document _)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuSendWaybillJournal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmListSendWaybills().ShowMove()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuSendWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new FrmSendWaybillCard().ShowCard(Guid.Empty, out Document _)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuSendStorageJournal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new SendStorageJournalViewModel().ShowListSendStorage()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuSendStorage
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new SendStorageCardViewModel().ShowCard(Guid.Empty, out Document _, out Document _)), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuGoodGroupEditing
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new FrmGoodsForGroupEdit().DoEdit()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand MenuCategoriesGroupEditing
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new FrmGroupEditCategories().DoEdit()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public Visibility MenuShowRemoteCatalogVisibility
    {
      get
      {
        string path = Path.Combine(new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.Cloud.Path, "goods");
        if (!FileSystemHelper.ExistsOrCreateFolder(path, false))
          return Visibility.Collapsed;
        FileSystemHelper.TempFolderPath();
        return !((IEnumerable<string>) Directory.GetFiles(path, "*.zip")).Where<string>((Func<string, bool>) (x => !x.Contains(UidDb.GetUid().EntityUid.ToString()))).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).ToList<FileInfo>().Any<FileInfo>() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand MenuShowRemoteCatalog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (ovj => new GoodRemoteCatalogViewModel().ShowCatalog()), (Func<object, bool>) (_ => MainWindowViewModel.CachedLicenseInformation.IsActive));
      }
    }

    public ICommand CreateFileCatalogLocal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          bool flag = ExchangeDataHelper.DoOnlyExchangeCatalogLocal(out string _);
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.MainWindowViewModel_Выгрузка_каталога,
            Text = flag ? Translate.ExchangeDataViewModel_Файл_выгружен_успешно_ : Translate.ExchangeDataViewModel_Ошибка_при_выгрузке_файла
          });
        }));
      }
    }

    public ICommand CreateFileCatalogFtp
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          bool flag = ExchangeDataHelper.DoOnlyExchangeCatalogFtp();
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.MainWindowViewModel_Выгрузка_каталога,
            Text = flag ? Translate.ExchangeDataViewModel_Файл_выгружен_успешно_ : Translate.ExchangeDataViewModel_Ошибка_при_выгрузке_файла
          });
        }));
      }
    }

    public Visibility VisibilitySeparatorService
    {
      get
      {
        return this.CreateFileHomeVisibility != Visibility.Visible || this.CreateFileCatalogFtpVisibility != Visibility.Visible && this.CreateFileCatalogLocalVisibility != Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand CreateFileHome
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          LogHelper.Debug("Выгрузка архива для дом/офис (нажата кнопка на главной форме)");
          try
          {
            HomeOfficeHelper.CreateArchive();
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.HomeOfficeHelper_Дом_офис,
              Text = Translate.MainWindowViewModel_Архив_с_данными_для_дом_офис_успешно_выгружен_
            });
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex);
          }
        }));
      }
    }

    public Visibility CreateFileHomeVisibility
    {
      get
      {
        return !this.SettingsConfig.RemoteControl.Cloud.IsActive ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility CreateFileCatalogFtpVisibility
    {
      get
      {
        return !this.SettingsConfig.ExchangeData.CatalogExchange.Ftp.IsSend ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility CreateFileCatalogLocalVisibility
    {
      get
      {
        return !this.SettingsConfig.ExchangeData.CatalogExchange.Local.IsSend ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility CatalogExchangeVisibility
    {
      get
      {
        return this.CreateFileCatalogFtpVisibility != Visibility.Visible && this.CreateFileCatalogLocalVisibility != Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility ExchangeDataMenuVisibility
    {
      get
      {
        return this.CatalogExchangeVisibility != Visibility.Visible && this.CreateFileHomeVisibility != Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility ServiceMenuVisibility
    {
      get
      {
        return this.ExchangeDataMenuVisibility != Visibility.Visible || new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility BlockMenuForHomeVisibility
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility MarkedLableMenuVisibility
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private void UpdateVisibilityService()
    {
      this.OnPropertyChanged("CreateFileHomeVisibility");
      this.OnPropertyChanged("CreateFileCatalogFtpVisibility");
      this.OnPropertyChanged("CreateFileCatalogLocalVisibility");
      this.OnPropertyChanged("CatalogExchangeVisibility");
      this.OnPropertyChanged("ExchangeDataMenuVisibility");
      this.OnPropertyChanged("ServiceMenuVisibility");
      this.OnPropertyChanged("BlockMenuForHomeVisibility");
    }

    public class SelectGoodView
    {
      public SelectGood SelectGood { get; set; }

      public string FontWeight { get; set; } = "Normal";
    }
  }
}
