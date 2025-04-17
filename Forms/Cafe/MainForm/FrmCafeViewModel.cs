// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.FrmCafeViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Users;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Forms._shared;
using Gbs.Forms.ActionsPayments;
using Gbs.Forms.Clients;
using Gbs.Forms.Goods;
using Gbs.Forms.Main;
using Gbs.Forms.Settings.Devices;
using Gbs.Forms.Users;
using Gbs.Helpers;
using Gbs.Helpers.Barcodes;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Factories;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public class FrmCafeViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodsSearchModelView.FilterProperty> _filterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>();
    public Action SearchFocusAction;
    private readonly bool _isVisibilityStockInfo = WindowWithSize.IsVisibilityStock;
    private CafeBasket _basket = new CafeBasket();
    private double _fontSize;
    private static Dictionary<Guid, BitmapSource> _imageList;
    private List<SelectGood> _cacheSelectGood;
    public static int GoodCardWidth = 200;
    private string _searchQuery = string.Empty;
    private System.Timers.Timer _searchTimer = new System.Timers.Timer();
    private DateTime _searchStartDateTime = DateTime.MaxValue;
    private bool _searchInProgress;

    public ICommand ExitProgramCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.Basket.Items.Any<BasketItem>())
          {
            if (MessageBoxHelper.Show(string.Format(Translate.MainWindow__0__При_закрытии_программы_все_не_сохраненные_данные_будут_утеряны__1__Продолжить_закрытие_программы_, (object) Translate.MainWindow_В_корзине_есть_товары__, (object) Gbs.Helpers.Other.NewLine(2)), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
              return;
            this.Basket.Items.Clear();
          }
          new FrmCloseProgram().GetClosed();
        }));
      }
    }

    public ICommand ShowActiveOrdersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MainWindowViewModel.DoWithPause();
          if (this.Basket.Items.Any<BasketItem>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.FrmCafeViewModel_Невозможно_открыть__так_как_в_корзине_есть_товары, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            new CafeActiveOrdersViewModel().ShowActiveOrder(new Action<Document>(this.AddToBasket));
            this.SearchFocusAction();
          }
        }));
      }
    }

    public static ICommand ChangeSkinCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          Gbs.Core.Config.Settings config = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
          if (config.Interface.BackgroundColor != null)
          {
            Color color1 = ColorTranslator.FromHtml(config.Interface.BackgroundColor);
            Color color2 = Color.FromArgb(238, 238, 238);
            if (color1 == Color.Transparent)
              color1 = config.Interface.Theme != GlobalDictionaries.Skin.Default ? color2.InvertBrightness() : color2;
            Color c = color1.InvertBrightness();
            config.Interface.BackgroundColor = ColorTranslator.ToHtml(c);
          }
          config.Interface.Theme = config.Interface.Theme == GlobalDictionaries.Skin.Default ? GlobalDictionaries.Skin.Dark : GlobalDictionaries.Skin.Default;
          new ConfigsRepository<Gbs.Core.Config.Settings>().Save(config);
          ((App) System.Windows.Application.Current).ChangeSkin(config.Interface.Theme);
          ((App) System.Windows.Application.Current).UpdateColors();
        }));
      }
    }

    public ICommand SaveOrder
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.Basket.ReCalcTotals();
          if (!this.Basket.Items.Any<BasketItem>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.FrmCafeViewModel_В_корзине_нет_товаров__сохранить_невозможно_);
            this.SearchFocusAction();
          }
          else
          {
            if (new PreSaveOrderViewModel().ShowPreSave(this.Basket))
            {
              this.SearchQuery = string.Empty;
              this.Basket = new CafeBasket();
              this.OnPropertyChanged("Basket");
              this.OnPropertyChanged("TableInfoText");
              MainWindowViewModel.InitDisplayBuyer();
              this.Basket.SendDisplayBuyerNumbersInfo(0M);
            }
            this.SearchFocusAction();
          }
        }));
      }
    }

    public ICommand UsersShow
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmLoginUser().ShowLogin(out Gbs.Core.Entities.Users.User _);
          this.GetStringOnlineUser();
          this.SetFontSize();
        }));
      }
    }

    public ICommand ShowScalesWeightCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ScalesTestViewModel().TestScales()));
      }
    }

    public ICommand CloseCafeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.Basket.Items.Any<BasketItem>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.FrmCafeViewModel_Нельзя_закрыть__т_к__в_списке_есть_товары, icon: MessageBoxImage.Exclamation);
          }
          else
            this.CloseAction();
        }));
      }
    }

    public ICommand RemoveCash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
          new FrmRemoveCash().RemoveCash(ref payment, true);
        }));
      }
    }

    public ICommand DepositСash
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmRemoveCash().InsertCash(true)));
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

    public ICommand ItemClick => (ICommand) new RelayCommand(new Action<object>(this.LoadMenu));

    public ICommand GoHomeCommand => (ICommand) new RelayCommand(new Action<object>(this.LoadMenu));

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => this.Cancel()));
    }

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmCafeViewModel_Обновление_данных);
          CacheHelper.Clear(CacheHelper.CacheTypes.CafeMenu);
          CacheHelper.Clear(CacheHelper.CacheTypes.AllGoods);
          this.LoadMenu();
          progressBar.Close();
        }));
      }
    }

    public ICommand GetClientCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Basket.Client != null)
          {
            this.Basket.Client = (ClientAdnSum) null;
            this.OnPropertyChanged("VisibilityPanelClient");
          }
          else
          {
            (Client client, bool result) client1 = new FrmSearchClient().GetClient();
            Client client2 = client1.client;
            int num = client1.result ? 1 : 0;
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmCafeViewModel_Загрузка_данных_о_покупателе);
            if (num != 0)
            {
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              {
                this.Basket.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(client2.Uid);
                this.OnPropertyChanged("IsEnabledClient");
              }
            }
            this.Basket.ReCalcTotals();
            progressBar.Close();
          }
        }));
      }
    }

    public ICommand EditClientCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (Client client, bool result) client1 = new FrmSearchClient().GetClient();
          Client client2 = client1.client;
          if (!client1.result)
            return;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmCafeViewModel_Загрузка_данных_о_покупателе);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            this.Basket.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(client2.Uid);
            this.OnPropertyChanged("IsEnabledClient");
            this.Basket.ReCalcTotals();
            progressBar.Close();
          }
        }));
      }
    }

    public ICommand UpdateInfoTableCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          int numTable = this.Basket.NumTable;
          int countGuest = this.Basket.CountGuest;
          if (!new TableCafeViewModel().Show(ref numTable, ref countGuest))
            return;
          this.Basket.NumTable = numTable;
          this.Basket.CountGuest = countGuest;
          this.OnPropertyChanged("TableInfoText");
        }));
      }
    }

    public ICommand PrintCheckCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => SaleHelper.PrintNoFiscalCheck(new DocumentsFactory().Create((Gbs.Core.ViewModels.Basket.Basket) this.Basket, true))));
      }
    }

    public string DoneTextButton
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsSpeedCafeOrder ? Translate.FrmExcelGoods_Сохранить : Translate.FrmMainWindow_ИТОГ;
      }
    }

    public Visibility VisibilityAllDiscountBtn
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsVisibilityAllDiscountBtn ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private void LoadingProperty(FilterOptions setting)
    {
      ObservableCollection<GoodsSearchModelView.FilterProperty> observableCollection1 = new ObservableCollection<GoodsSearchModelView.FilterProperty>();
      observableCollection1.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.FrmReadExcel_Название,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedName
      });
      observableCollection1.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Barcode",
        Text = Translate.FrmAuthorization_ШтрихКод,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedBarcode
      });
      observableCollection1.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Description",
        Text = Translate.ExcelDataViewModel_Описание,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedBarcodes
      });
      observableCollection1.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Barcodes",
        Text = Translate.ExcelDataViewModel_Доп__штрих_коды,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedDescription
      });
      observableCollection1.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "ModificationBarcode",
        Text = Translate.GoodsCatalogModelView_GoodsCatalogModelView_Штрих_коды_ассортимента,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedModificationBarcode
      });
      ObservableCollection<GoodsSearchModelView.FilterProperty> collection = observableCollection1;
      foreach (EntityProperties.PropertyType propertyType in (IEnumerable<EntityProperties.PropertyType>) EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good, false).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid))).OrderBy<EntityProperties.PropertyType, string>((Func<EntityProperties.PropertyType, string>) (x => x.Name)))
      {
        EntityProperties.PropertyType type = propertyType;
        if (!(type.Uid == GlobalDictionaries.AlcCodeUid) || new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        {
          ObservableCollection<GoodsSearchModelView.FilterProperty> observableCollection2 = collection;
          GoodsSearchModelView.FilterProperty filterProperty = new GoodsSearchModelView.FilterProperty();
          filterProperty.Name = type.Uid.ToString();
          filterProperty.Text = type.Name;
          GoodProp.PropItem propItem = setting.SearchGood.GoodProp.PropList.FirstOrDefault<GoodProp.PropItem>((Func<GoodProp.PropItem, bool>) (x => x.Uid == type.Uid));
          filterProperty.IsChecked = propItem != null && propItem.IsChecked;
          observableCollection2.Add(filterProperty);
        }
      }
      this.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>((IEnumerable<GoodsSearchModelView.FilterProperty>) collection);
    }

    public string TextPropButton
    {
      get
      {
        int num = this.FilterProperties.Count<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked));
        if (num == this.FilterProperties.Count<GoodsSearchModelView.FilterProperty>())
          return Translate.GoodsSearchModelView_Все_поля;
        return num != 1 ? Translate.GoodsSearchModelView_Полей__ + num.ToString() : this.FilterProperties.First<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)).Text;
      }
    }

    public FilterOptions Setting { get; } = new FilterOptions();

    public ObservableCollection<GoodsSearchModelView.FilterProperty> FilterProperties
    {
      get => this._filterProperties;
      set
      {
        this._filterProperties = value;
        new ConfigsRepository<FilterOptions>().Save(this.Setting);
        this.OnPropertyChanged("TextPropButton");
        if (value.Any<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
          return;
        int num = (int) MessageBoxHelper.Show(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
      }
    }

    public Visibility VisibilityScalesWeight
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Scale.IsShowBtnTestWeight ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public static BitmapImage IconFolder { get; set; }

    private Gbs.Core.Config.Cafe CafeConfig { get; set; }

    private System.Windows.Controls.DataGrid BasketGrid { get; set; }

    public double MaxBasketWidth => FrmCafeViewModel.GetMaxBasketWidth();

    private static double GetMaxBasketWidth()
    {
      return (double) Screen.PrimaryScreen.WorkingArea.Width - 200.0;
    }

    public static string AlsoMenuKey => "AlsoMenu";

    public CafeBasket Basket
    {
      get => this._basket;
      set
      {
        this._basket = value;
        this.OnPropertyChanged(nameof (Basket));
        MainWindowViewModel.MonitorViewModel.UpdateBasket((Gbs.Core.ViewModels.Basket.Basket) value);
      }
    }

    public double FontSize
    {
      get => this._fontSize;
      set
      {
        this._fontSize = value;
        this.OnPropertyChanged(nameof (FontSize));
      }
    }

    private void GetStringOnlineUser()
    {
      this.UserOnlineString = string.Empty;
      foreach (Gbs.Core.Entities.Users.User user in WindowWithSize.ListOnlineUser)
        this.UserOnlineString = this.UserOnlineString + user.Alias + "; ";
      this.OnPropertyChanged("UserOnlineString");
      new WindowWithSize().UpdateColumnStock(this.BasketGrid, out Visibility _, (System.Windows.Controls.ContextMenu) this.BasketGrid.FindResource((object) "ContextMenuGrid"));
    }

    public string UserOnlineString { get; set; } = string.Empty;

    private void SetFontSize()
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

    public Visibility KkmMenuVisibility
    {
      get
      {
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        return checkPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ObservableCollection<FrmCafeViewModel.MenuItem> MenuItems { get; set; }

    public FrmCafeViewModel()
    {
    }

    public string Time => TimerHelper.CurrentTime;

    private void UpdateTime() => this.OnPropertyChanged("Time");

    private void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<FrmCafeMain>())
        return;
      this.SearchQuery = barcode;
      this.DoSearch();
    }

    public void ClearImagesList()
    {
    }

    public FrmCafeViewModel(System.Windows.Controls.DataGrid grid)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmCafeViewModel_Загрузка_режима_КАФЕ);
      Performancer performancer = new Performancer("Загрузка окна кафе");
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.ComPortScannerOnBarcodeChanged));
      TimerHelper.InitializeTimers(new Action(this.UpdateTime));
      performancer.AddPoint("Инициализация таймеров");
      this.CafeConfig = new ConfigsRepository<Gbs.Core.Config.Cafe>().Get();
      performancer.AddPoint("Получение настроек кафе");
      this.BasketGrid = grid;
      this.SearchTimerInit();
      this.GetStringOnlineUser();
      performancer.AddPoint("Таймер поиска запущен");
      CacheHelper.Clear(CacheHelper.CacheTypes.CafeMenu);
      FrmCafeViewModel.GoodCardWidth = new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().Menu.CardSize;
      this.Setting = new ConfigsRepository<FilterOptions>().Get();
      this.LoadingProperty(this.Setting);
      this.LoadMenu();
      performancer.AddPoint("товары получены");
      progressBar.Close();
      MainWindowViewModel.MonitorViewModel.UpdateBasket((Gbs.Core.ViewModels.Basket.Basket) this.Basket);
      performancer.Stop();
    }

    private void TimeTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.OnPropertyChanged("Time");
    }

    public Visibility VisibilityButtonPrint
    {
      get
      {
        Gbs.Core.Config.Cafe cafeConfig = this.CafeConfig;
        return (cafeConfig != null ? (cafeConfig.IsButtonPrint ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityPanelClient
    {
      get
      {
        Gbs.Core.Config.Cafe cafeConfig = this.CafeConfig;
        return (cafeConfig != null ? (cafeConfig.IsPanelClient ? 1 : 0) : 0) == 0 && this.Basket?.Client == null ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityPanelTableInfo
    {
      get
      {
        Gbs.Core.Config.Cafe cafeConfig = this.CafeConfig;
        return (cafeConfig != null ? (cafeConfig.IsTableAndGuest ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public void LoadMenu(object obj = null)
    {
      this._searchQuery = string.Empty;
      this.OnPropertyChanged("VisibilityPanelClient");
      this.OnPropertyChanged("SearchQuery");
      List<FrmCafeViewModel.MenuItem> source = CacheHelper.Get<List<FrmCafeViewModel.MenuItem>>(CacheHelper.CacheTypes.CafeMenu, new Func<List<FrmCafeViewModel.MenuItem>>(this.LoadMenuFromDB));
      if (obj == null)
      {
        this.MenuItems = new ObservableCollection<FrmCafeViewModel.MenuItem>(source.Where<FrmCafeViewModel.MenuItem>((Func<FrmCafeViewModel.MenuItem, bool>) (x => !this.IsEmptyGroup(x))));
        this.UpdateSelectGoods(false);
        this.OnPropertyChanged("MenuItems");
        this.OnPropertyChanged("TotalGood");
      }
      else
      {
        FrmCafeViewModel.MenuItem menuItem1 = (FrmCafeViewModel.MenuItem) obj;
        if (!menuItem1.IsGroup)
        {
          this.AddToBasket((Gbs.Core.Entities.Goods.Good) menuItem1.Item);
          this.BasketGrid.ScrollToSelectedRow();
          if (!new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().Menu.IsReturnInMain)
            return;
          this.GoHomeCommand.Execute((object) null);
          this.OnPropertyChanged("TotalGood");
          this.OnPropertyChanged("MenuItems");
        }
        else
        {
          if (menuItem1.IsGroup)
          {
            List<FrmCafeViewModel.MenuItem> list = new List<FrmCafeViewModel.MenuItem>(menuItem1.ChildList.Where<FrmCafeViewModel.MenuItem>((Func<FrmCafeViewModel.MenuItem, bool>) (x => !this.IsEmptyGroup(x) || !x.IsGroup)));
            if (!new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.AllowSalesToMinus)
            {
              List<FrmCafeViewModel.MenuItem> menuItemList = new List<FrmCafeViewModel.MenuItem>();
              foreach (FrmCafeViewModel.MenuItem menuItem2 in list)
              {
                if (menuItem2.IsGroup)
                  menuItemList.Add(menuItem2);
                else if (((Gbs.Core.Entities.Goods.Good) menuItem2.Item).StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (st => st.Stock)) > 0M)
                  menuItemList.Add(menuItem2);
              }
            }
            this.MenuItems = new ObservableCollection<FrmCafeViewModel.MenuItem>(list);
          }
          this.OnPropertyChanged("MenuItems");
          this.OnPropertyChanged("TotalGood");
        }
      }
    }

    private void AddToBasket(Gbs.Core.Entities.Goods.Good good)
    {
      new BasketHelper((Gbs.Core.ViewModels.Basket.Basket) this.Basket).AddItemToBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) new List<Gbs.Core.Entities.Goods.Good>()
      {
        good
      }, checkMinus: false);
    }

    private void AddToBasket(Document document)
    {
      this.Basket.Document = document;
      this.Basket.Storage = document.Storage;
      CafeHelper cafeHelper = new CafeHelper(this.Basket);
      if (!document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == GlobalDictionaries.PercentForServiceGoodUid)))
        this.Basket.IsDeletedPercentForServiceGood = true;
      Document document1 = document;
      cafeHelper.AddItemCafeOrder(document1);
      this.OnPropertyChanged("TableInfoText");
    }

    private void SetImage(List<FrmCafeViewModel.MenuItem> items)
    {
      try
      {
        if (!this.CafeConfig.Menu.IsShowImageGood)
          return;
        if (FrmCafeViewModel._imageList == null)
          FrmCafeViewModel._imageList = new Dictionary<Guid, BitmapSource>();
        string goodsImagesPath = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath;
        BitmapSource nullBitmapSource = ImagesHelpers.NullBitmapSource;
        List<string> supportedImages = new List<string>()
        {
          "jpg",
          "jpeg",
          "png",
          "bmp"
        };
        foreach (FrmCafeViewModel.MenuItem menuItem in items)
        {
          FrmCafeViewModel.MenuItem item = menuItem;
          Guid uid = item.Item.Uid;
          if (FrmCafeViewModel._imageList.ContainsKey(item.Item.Uid))
          {
            item.Image = FrmCafeViewModel._imageList[uid];
          }
          else
          {
            string path1 = Path.Combine(goodsImagesPath, uid.ToString());
            try
            {
              List<string> files = new List<string>();
              if (Directory.Exists(path1))
              {
                files = ((IEnumerable<string>) Directory.GetFiles(path1)).ToList<string>();
                files = files.Where<string>((Func<string, bool>) (x => supportedImages.Any<string>(new Func<string, bool>(x.ToLower().EndsWith)))).ToList<string>();
              }
              if (files.Any<string>())
                System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
                {
                  string path2 = files.First<string>();
                  try
                  {
                    int maxSize = this.CafeConfig.Menu.CardSize * 2;
                    BitmapImage image = ImagesHelpers.ConvertToImage(path2, maxSize);
                    item.Image = (BitmapSource) image;
                    FrmCafeViewModel._imageList.Add(uid, (BitmapSource) image);
                  }
                  catch (Exception ex)
                  {
                    string logMessage = "Ошибка получения фото из файла " + path2 + " и его установки";
                    LogHelper.WriteError(ex, logMessage);
                  }
                }));
              else if (item.IsGroup)
              {
                FrmCafeViewModel.IconFolder.Freeze();
                item.Image = (BitmapSource) FrmCafeViewModel.IconFolder;
                FrmCafeViewModel._imageList.Add(uid, (BitmapSource) FrmCafeViewModel.IconFolder);
              }
              else
              {
                nullBitmapSource.Freeze();
                item.Image = nullBitmapSource;
                FrmCafeViewModel._imageList.Add(uid, nullBitmapSource);
              }
            }
            catch (Exception ex)
            {
              LogHelper.WriteError(ex);
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void Cancel()
    {
      (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.CancelSale);
      if (!access.Result || MessageBoxHelper.Show(Translate.FrmCafeViewModel_Уверены__что_хотите_отменить_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
        return;
      this.Basket.AddActionsHistoryByCancel(access.User);
      this.LoadMenu();
      this.Basket = new CafeBasket();
      this.OnPropertyChanged("Basket");
      this.OnPropertyChanged("TableInfoText");
      this.LoadMenu();
      this.SearchFocusAction();
    }

    public bool IsEnabledClient => this.Basket.Client != null;

    public string TableInfoText
    {
      get
      {
        return string.Format(Translate.FrmCafeViewModelСтоликПерсоны, (object) this.Basket.NumTable, (object) this.Basket.CountGuest);
      }
    }

    public Decimal TotalGood
    {
      get
      {
        ObservableCollection<FrmCafeViewModel.MenuItem> menuItems = this.MenuItems;
        return (Decimal) (menuItems != null ? menuItems.Count<FrmCafeViewModel.MenuItem>((Func<FrmCafeViewModel.MenuItem, bool>) (x => !x.IsGroup)) : 0);
      }
    }

    private bool IsEmptyGroup(FrmCafeViewModel.MenuItem group)
    {
      if (!group.ChildList.Any<FrmCafeViewModel.MenuItem>())
        return true;
      if (group.ChildList.Any<FrmCafeViewModel.MenuItem>((Func<FrmCafeViewModel.MenuItem, bool>) (x => !x.IsGroup)))
        return false;
      bool flag = true;
      foreach (FrmCafeViewModel.MenuItem group1 in group.ChildList.Where<FrmCafeViewModel.MenuItem>((Func<FrmCafeViewModel.MenuItem, bool>) (x => x.IsGroup)))
      {
        flag &= this.IsEmptyGroup(group1);
        if (!flag)
          return false;
      }
      return true;
    }

    private List<FrmCafeViewModel.MenuItem> LoadMenuFromDB()
    {
      Performancer performancer = new Performancer("Загрузка меню для кафе из БД");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> allGoods = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x =>
        {
          if (x.Group.IsCompositeGood)
            return false;
          return x.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st => !st.IsDeleted)) || x.SetStatus == GlobalDictionaries.GoodsSetStatuses.Kit;
        })).ToList<Gbs.Core.Entities.Goods.Good>();
        performancer.AddPoint(string.Format("Получены товары: {0}", (object) allGoods.Count<Gbs.Core.Entities.Goods.Good>()));
        Gbs.Core.Config.Settings settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        List<GoodGroups.Group> allGroups = new GoodGroupsRepository(dataBase).GetActiveItems();
        performancer.AddPoint(string.Format("Получены категории: {0}", (object) allGroups.Count));
        List<FrmCafeViewModel.MenuItem> list = allGroups.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.ParentGroupUid == Guid.Empty)).Select<GoodGroups.Group, FrmCafeViewModel.MenuItem>((Func<GoodGroups.Group, FrmCafeViewModel.MenuItem>) (x => new FrmCafeViewModel.MenuItem(x))).OrderBy<FrmCafeViewModel.MenuItem, string>((Func<FrmCafeViewModel.MenuItem, string>) (x => x.Name)).ToList<FrmCafeViewModel.MenuItem>();
        performancer.AddPoint("Создание записей меню");
        foreach (FrmCafeViewModel.MenuItem menuItem in list)
          LoadChild(menuItem);
        performancer.AddPoint("Загрузка дочерних элементов");
        this.SetImage(list);
        performancer.AddPoint("Установка изображений");
        List<FrmCafeViewModel.MenuItem> menuItemList = new List<FrmCafeViewModel.MenuItem>();
        foreach (FrmCafeViewModel.MenuItem menuItem in list)
        {
          menuItemList.Add(menuItem);
          if (menuItem.ChildList.Any<FrmCafeViewModel.MenuItem>())
          {
            foreach (FrmCafeViewModel.MenuItem child in menuItem.ChildList)
              LoadImageChild(child, menuItemList);
          }
        }
        performancer.AddPoint("Установка изображений для дочерних элементов");
        this.SetImage(menuItemList);
        performancer.AddPoint("Установка изображений для всех товаров");
        Gbs.Helpers.Other.ConsoleWrite(string.Format("Категории загружены: {0}", (object) ((double) stopwatch.ElapsedMilliseconds / 1000.0)));
        performancer.Stop();
        return list;

        void LoadChild(FrmCafeViewModel.MenuItem menuItem)
        {
          Guid groupUid = ((Entity) menuItem.Item).Uid;
          foreach (FrmCafeViewModel.MenuItem menuItem1 in allGroups.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.ParentGroupUid == groupUid)).OrderBy<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name)).Select<GoodGroups.Group, FrmCafeViewModel.MenuItem>((Func<GoodGroups.Group, FrmCafeViewModel.MenuItem>) (g => new FrmCafeViewModel.MenuItem(g))))
          {
            LoadChild(menuItem1);
            menuItem.ChildList.Add(menuItem1);
          }
          foreach (Gbs.Core.Entities.Goods.Good good in (IEnumerable<Gbs.Core.Entities.Goods.Good>) allGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Group.Uid == groupUid)).OrderBy<Gbs.Core.Entities.Goods.Good, string>((Func<Gbs.Core.Entities.Goods.Good, string>) (x => x.Name)))
          {
            FrmCafeViewModel.MenuItem menuItem2 = new FrmCafeViewModel.MenuItem(good, this.CafeConfig, this._isVisibilityStockInfo);
            if (this.CafeConfig.Menu.IsShowStockGood && this._isVisibilityStockInfo)
            {
              int setStatus = (int) good.SetStatus;
              GlobalDictionaries.GoodsSetStatuses[] goodsSetStatusesArray = new GlobalDictionaries.GoodsSetStatuses[2]
              {
                GlobalDictionaries.GoodsSetStatuses.Set,
                GlobalDictionaries.GoodsSetStatuses.Kit
              };
              menuItem2.Stock = !((GlobalDictionaries.GoodsSetStatuses) setStatus).IsEither<GlobalDictionaries.GoodsSetStatuses>(goodsSetStatusesArray) ? new Decimal?(good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock))) : new Decimal?(GoodsSearchModelView.GetStockSet(good, (List<Gbs.Core.Entities.Goods.Good>) null));
            }
            if (!settings.Sales.AllowSalesToMinus)
            {
              Decimal? stock = menuItem2.Stock;
              Decimal num = 0M;
              if (stock.GetValueOrDefault() <= num & stock.HasValue)
                continue;
            }
            menuItem.ChildList.Add(menuItem2);
          }
        }
      }

      static void LoadImageChild(
        FrmCafeViewModel.MenuItem menuItem,
        List<FrmCafeViewModel.MenuItem> list)
      {
        list.Add(menuItem);
        foreach (FrmCafeViewModel.MenuItem child1 in menuItem.ChildList)
        {
          list.Add(child1);
          if (child1.ChildList.Any<FrmCafeViewModel.MenuItem>())
          {
            foreach (FrmCafeViewModel.MenuItem child2 in child1.ChildList)
              LoadImageChild(child2, list);
          }
        }
      }
    }

    private void UpdateSelectGoods(bool isLoading)
    {
      try
      {
        Interface @interface = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface;
        Gbs.Core.Config.Cafe cafe = new ConfigsRepository<Gbs.Core.Config.Cafe>().Get();
        if (@interface.CountSelectGood <= 0 || cafe.Menu.SelectGoodForCafe == Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.None)
          return;
        if (isLoading || this._cacheSelectGood == null)
        {
          this._cacheSelectGood = new List<SelectGood>();
          EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
          if (egais.IsActive && egais.IsShowTapInSelectGood)
          {
            IEnumerable<Gbs.Core.Entities.Goods.Good> cacheGoods = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted));
            List<InfoToTapBeer> list = new InfoTapBeerRepository().GetActiveItems().Where<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => x.GoodUid != Guid.Empty && !x.IsDeleted && !x.Tap.IsDeleted)).ToList<InfoToTapBeer>();
            if (list.Any<InfoToTapBeer>())
              this._cacheSelectGood = new List<SelectGood>(list.Select<InfoToTapBeer, SelectGood>((Func<InfoToTapBeer, SelectGood>) (x => new SelectGood()
              {
                DisplayName = x.Tap.Name,
                Index = x.Tap.Index,
                Good = cacheGoods.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => g.Uid == x.ChildGoodUid)),
                Uid = Guid.Empty,
                ParentUid = x.Tap.Uid
              }))).Where<SelectGood>((Func<SelectGood, bool>) (x => x.Good != null && !x.Good.IsDeleted)).OrderBy<SelectGood, int>((Func<SelectGood, int>) (x => x.Index)).ToList<SelectGood>();
          }
          this._cacheSelectGood.AddRange((IEnumerable<SelectGood>) new SelectGoodsRepository().GetActiveItems().Where<SelectGood>((Func<SelectGood, bool>) (x =>
          {
            Gbs.Core.Entities.Goods.Good good = x.Good;
            return good != null && !good.IsDeleted;
          })).OrderBy<SelectGood, int>((Func<SelectGood, int>) (x => x.Index)));
        }
        List<FrmCafeViewModel.MenuItem> list1 = this._cacheSelectGood.Select<SelectGood, FrmCafeViewModel.MenuItem>((Func<SelectGood, FrmCafeViewModel.MenuItem>) (x => new FrmCafeViewModel.MenuItem(x.Good, new ConfigsRepository<Gbs.Core.Config.Cafe>().Get(), this._isVisibilityStockInfo, x.DisplayName))).ToList<FrmCafeViewModel.MenuItem>();
        this.SetImage(list1);
        switch (cafe.Menu.SelectGoodForCafe)
        {
          case Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.None:
            this.OnPropertyChanged("MenuItems");
            break;
          case Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.OverGroup:
            this.MenuItems = new ObservableCollection<FrmCafeViewModel.MenuItem>(list1.Concat<FrmCafeViewModel.MenuItem>((IEnumerable<FrmCafeViewModel.MenuItem>) this.MenuItems));
            goto case Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.None;
          case Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.UnderGroup:
            this.MenuItems = new ObservableCollection<FrmCafeViewModel.MenuItem>(this.MenuItems.Concat<FrmCafeViewModel.MenuItem>((IEnumerable<FrmCafeViewModel.MenuItem>) list1));
            goto case Gbs.Core.Config.Cafe.MenuConfig.SelectGoodForCafeEnum.None;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки избранных товаров");
      }
    }

    public string SearchQuery
    {
      get => this._searchQuery;
      set
      {
        this._searchQuery = value;
        this.OnPropertyChanged(nameof (SearchQuery));
        if (this._searchQuery.Length == 0)
        {
          this._searchStartDateTime = DateTime.Now.AddYears(1);
          this.LoadMenu();
        }
        else
        {
          if (this._searchQuery.Length < 3)
            return;
          int num = 500;
          if (BarcodeHelper.IsEan13Barcode(this._searchQuery))
            num = 100;
          this._searchStartDateTime = DateTime.Now.AddMilliseconds((double) num);
        }
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

    private bool IsCafeFormActive()
    {
      bool active = false;
      System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() => active = System.Windows.Application.Current.Windows.OfType<FrmCafeMain>().Any<FrmCafeMain>((Func<FrmCafeMain, bool>) (f => f.IsActive))));
      return active;
    }

    public void StartSearch()
    {
      if (this._searchInProgress || this._searchQuery.Length < 3 || !this.IsCafeFormActive())
        return;
      this._searchInProgress = true;
      this._searchStartDateTime = DateTime.Now.AddYears(1);
      this.DoSearch();
      this._searchInProgress = false;
      this.SearchFocusAction();
    }

    private List<Gbs.Core.Entities.Goods.Good> GetGoodsFromMenu()
    {
      ConcurrentBag<Gbs.Core.Entities.Goods.Good> goodsList = new ConcurrentBag<Gbs.Core.Entities.Goods.Good>();
      foreach (FrmCafeViewModel.MenuItem menuItem in CacheHelper.Get<List<FrmCafeViewModel.MenuItem>>(CacheHelper.CacheTypes.CafeMenu, new Func<List<FrmCafeViewModel.MenuItem>>(this.LoadMenuFromDB)).AsParallel<FrmCafeViewModel.MenuItem>())
        AllGoodsToList(menuItem);
      return goodsList.ToList<Gbs.Core.Entities.Goods.Good>();

      void AllGoodsToList(FrmCafeViewModel.MenuItem item)
      {
        foreach (FrmCafeViewModel.MenuItem menuItem in item.ChildList.AsParallel<FrmCafeViewModel.MenuItem>())
        {
          if (!menuItem.IsGroup)
          {
            Gbs.Core.Entities.Goods.Good good = (Gbs.Core.Entities.Goods.Good) menuItem.Item;
            goodsList.Add(good);
          }
          else
            AllGoodsToList(menuItem);
        }
      }
    }

    private void DoSearch()
    {
      if (this.SearchQuery.IsNullOrEmpty())
      {
        this.LoadMenu();
      }
      else
      {
        List<Gbs.Core.Entities.Goods.Good> goodsFromMenu = this.GetGoodsFromMenu();
        if (new BarcodeSearcher((Gbs.Core.ViewModels.Basket.Basket) this.Basket, goodsFromMenu).SearchByBarcodeAndAddToBasket(this.SearchQuery, new Action(((Gbs.Helpers.ControlsHelpers.DataGrid.Other) this.BasketGrid).ScrollToSelectedRow)))
        {
          this.OnPropertyChanged("VisibilityPanelClient");
          this.SearchQuery = string.Empty;
        }
        else
        {
          using (Data.GetDataBase())
          {
            string f = this.SearchQuery.ToLower();
            List<Gbs.Core.Entities.Goods.Good> filteredGoods = new List<Gbs.Core.Entities.Goods.Good>();
            foreach (GoodsSearchModelView.FilterProperty filterProperty1 in this.FilterProperties.Where<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
            {
              GoodsSearchModelView.FilterProperty filterProperty = filterProperty1;
              Guid result;
              if (Guid.TryParse(filterProperty.Name, out result))
              {
                if (result == GlobalDictionaries.GoodIdUid)
                {
                  int intValue;
                  if (int.TryParse(f, out intValue))
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && int.Parse(p.Value.ToString()) == intValue)))));
                }
                else
                  filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && p.Value.ToString().ToLower().Contains(f))))));
              }
              else
              {
                switch (filterProperty.Name)
                {
                  case "Name":
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Name.ToLower().Contains(f))));
                    IEnumerable<Gbs.Core.Entities.Goods.Good> source = ((IEnumerable<string>) f.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<Gbs.Core.Entities.Goods.Good>>(goodsFromMenu.AsEnumerable<Gbs.Core.Entities.Goods.Good>(), (Func<IEnumerable<Gbs.Core.Entities.Goods.Good>, string, IEnumerable<Gbs.Core.Entities.Goods.Good>>) ((current, s) => current.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Name.ToLower().Contains(s)))));
                    filteredGoods.AddRange((IEnumerable<Gbs.Core.Entities.Goods.Good>) source.ToList<Gbs.Core.Entities.Goods.Good>());
                    continue;
                  case "Barcode":
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode.ToLower().Contains(f))));
                    continue;
                  case "Barcodes":
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcodes.Any<string>((Func<string, bool>) (barcode => barcode.ToLower().Contains(f))))));
                    continue;
                  case "Description":
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Description.ToLower().Contains(f))));
                    continue;
                  case "ModificationBarcode":
                    filteredGoods.AddRange(goodsFromMenu.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Modifications.Any<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Barcode.ToLower().Contains(f))))));
                    continue;
                  default:
                    continue;
                }
              }
            }
            filteredGoods = filteredGoods.Distinct<Gbs.Core.Entities.Goods.Good>().ToList<Gbs.Core.Entities.Goods.Good>();
            if (!new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.AllowSalesToMinus)
              filteredGoods = filteredGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (st => st.Stock)) > 0M)).ToList<Gbs.Core.Entities.Goods.Good>();
            System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
            {
              this.MenuItems = new ObservableCollection<FrmCafeViewModel.MenuItem>((IEnumerable<FrmCafeViewModel.MenuItem>) filteredGoods.Select<Gbs.Core.Entities.Goods.Good, FrmCafeViewModel.MenuItem>((Func<Gbs.Core.Entities.Goods.Good, FrmCafeViewModel.MenuItem>) (x => new FrmCafeViewModel.MenuItem(x, this.CafeConfig, this._isVisibilityStockInfo))).OrderBy<FrmCafeViewModel.MenuItem, string>((Func<FrmCafeViewModel.MenuItem, string>) (x => x.Name)));
              if (this.CafeConfig.Menu.IsShowImageGood)
                this.SetImage(this.MenuItems.ToList<FrmCafeViewModel.MenuItem>());
              if (!this.CafeConfig.IsAddToOrderByBarcode || filteredGoods.Count != 1 || !(filteredGoods.Single<Gbs.Core.Entities.Goods.Good>().Barcode == this.SearchQuery) && !filteredGoods.Single<Gbs.Core.Entities.Goods.Good>().Barcodes.Any<string>((Func<string, bool>) (x => x.Trim() == this.SearchQuery.Trim())))
                return;
              this.AddToBasket((Gbs.Core.Entities.Goods.Good) this.MenuItems.Single<FrmCafeViewModel.MenuItem>().Item);
              this.BasketGrid.ScrollToSelectedRow();
              this.SearchQuery = string.Empty;
            }));
            this.OnPropertyChanged("MenuItems");
            this.OnPropertyChanged("TotalGood");
          }
        }
      }
    }

    public class MenuItem : ViewModel
    {
      private BitmapSource _image;

      public int CardWidth => FrmCafeViewModel.GoodCardWidth;

      public int CardHeight => this.CardWidth / 2;

      public int ImageSize => this.CardWidth - 5;

      public double FontSize
      {
        get
        {
          int cardWidth = this.CardWidth;
          return this.CardWidth <= 160 ? (this.CardWidth <= 140 ? (this.CardWidth <= 120 ? 12.0 : 14.0) : 15.0) : 16.0;
        }
      }

      public string Name { get; set; }

      public bool IsGroup { get; set; }

      public FontWeight FontWeight => !this.IsGroup ? FontWeights.Regular : FontWeights.Bold;

      public string Info { get; set; }

      public Decimal? Stock { get; set; }

      public List<FrmCafeViewModel.MenuItem> ChildList { get; set; } = new List<FrmCafeViewModel.MenuItem>();

      public IEntity Item { get; set; }

      public BitmapSource Image
      {
        get => this._image;
        set
        {
          this._image = value;
          this.OnPropertyChanged(nameof (Image));
          this.OnPropertyChanged("VisibilityImage");
        }
      }

      public Visibility VisibilityImage
      {
        get => this.Image != null ? Visibility.Visible : Visibility.Collapsed;
      }

      public Visibility VisibilityStocks
      {
        get => !this.Stock.HasValue ? Visibility.Collapsed : Visibility.Visible;
      }

      public MenuItem(GoodGroups.Group group)
      {
        this.Name = group.Name;
        this.IsGroup = true;
        this.Item = (IEntity) group;
      }

      public MenuItem(Gbs.Core.Entities.Goods.Good good, Gbs.Core.Config.Cafe _cafe, bool isVisibilityStockInfo, string displayName = null)
      {
        int num1;
        if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(OnlyActiveStocks)))
          num1 = good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit) ? 1 : 0;
        else
          num1 = 1;
        bool flag = num1 != 0;
        Decimal num2;
        if (good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set))
        {
          GoodsCatalogModelView.GoodsInfoGrid good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good
          };
          GoodsSearchModelView.GetPriceForKit(good1);
          num2 = good1.MaxPrice.GetValueOrDefault();
          this.Stock = isVisibilityStockInfo ? good1.GoodTotalStock : new Decimal?();
        }
        else
          num2 = good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock > 0M)) & flag ? good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted && x.Stock > 0M)).Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)) : (flag ? good.StocksAndPrices.Where<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(OnlyActiveStocks)).Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)) : 0M);
        this.Name = displayName ?? good.Name;
        this.IsGroup = false;
        this.Item = (IEntity) good;
        if (_cafe.Menu.IsShowStockGood & isVisibilityStockInfo)
        {
          if (!good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit))
            this.Stock = new Decimal?(good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
        }
        this.Info += flag ? num2.ToString("N2") : "";

        static bool OnlyActiveStocks(GoodsStocks.GoodStock x) => !x.IsDeleted;
      }
    }
  }
}
