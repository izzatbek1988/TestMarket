// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodsCatalogModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Devices.ScalesWIthLabels;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Excel;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Goods
{
  public partial class GoodsCatalogModelView : ViewModelWithForm
  {
    private bool _loadImages;
    private bool _isEnableReloadData;
    private CancellationTokenSource _reloadCts;
    private readonly Gbs.Core.Config.Devices _devicesSetting;
    private Storages.Storage _storage;
    public CancellationTokenSource cts;
    private Task _taskSearch;
    private Decimal _sumPriceGoods;
    private Decimal _sumInPriceGoods;
    private readonly object _goodCollectionLock;
    private Client _supplier;
    private bool _isEnabledParameters;
    private bool _isEnabledSupp;
    private ListBoxItem _selectedFilterPrice;
    private Decimal? _filterPrice;
    private ListBoxItem _selectedFilterCount;
    private Decimal? _filterCount;
    private bool _isLoadingDeleteGood;
    private GoodsCatalogModelView.FilterEqualEnum _selectedFilterEqual;
    private string _filterGoods;
    private ObservableCollection<GoodsCatalogModelView.FilterProperty> _filterProperties;
    private GlobalDictionaries.GoodsSetStatuses _selectedStatusSet;
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private Decimal _totalGoodsStock;

    public static void LoadingImageForOneGood(GoodsCatalogModelView.GoodsInfoGrid item)
    {
      string path = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, item.Good.Uid.ToString());
      if (!Directory.Exists(path))
        return;
      try
      {
        string[] files = Directory.GetFiles(path);
        if (files.Length == 0)
          return;
        string firstFile = files[0];
        Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          item.Image = (BitmapSource) ImagesHelpers.ConvertToImage(firstFile, 100);
          item.VisibilityImage = Visibility.Visible;
        }));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения фото из файла и его установки");
        item.Image = (BitmapSource) null;
      }
    }

    public void LoadingImageForGood()
    {
      if (this._loadImages)
        return;
      this._loadImages = true;
      Task.Run((Action) (() => Parallel.ForEach<GoodsCatalogModelView.GoodsInfoGrid>((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) this.CachedDbGoods, new Action<GoodsCatalogModelView.GoodsInfoGrid>(GoodsCatalogModelView.LoadingImageForOneGood))));
    }

    public bool IsEnableReloadData
    {
      get => this._isEnableReloadData;
      set
      {
        this._isEnableReloadData = value;
        this.OnPropertyChanged(nameof (IsEnableReloadData));
      }
    }

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            this._reloadCts?.Cancel();
            this._reloadCts = new CancellationTokenSource();
            CancellationToken token = this._reloadCts.Token;
            this.IsEnableReloadData = false;
            this._isLoadingDeleteGood = false;
            this._loadImages = false;
            GoodsSearchModelView.ClearCacheGoodsDictionary();
            Task.Run((Func<Task>) (async () =>
            {
              try
              {
                Task taskSearch = this._taskSearch;
                if (taskSearch != null && !taskSearch.IsCompleted)
                  await this._taskSearch;
                token.ThrowIfCancellationRequested();
                this.GetGoodThread();
                token.ThrowIfCancellationRequested();
                await Application.Current.Dispatcher.InvokeAsync((Action) (() =>
                {
                  this.SearchForFilter();
                  token.ThrowIfCancellationRequested();
                  CollectionViewSource.GetDefaultView((object) this.GoodsList).Refresh();
                }));
                token.ThrowIfCancellationRequested();
                this.LoadingImageForGood();
              }
              catch (OperationCanceledException ex)
              {
              }
              catch (Exception ex)
              {
                LogHelper.Error(ex, "Ошибка при обновлении данных");
              }
            }), token).ContinueWith((Action<Task>) (t =>
            {
              if (t.IsFaulted)
                LogHelper.Error((Exception) t.Exception, "");
              this.IsEnableReloadData = true;
              this._reloadCts = (CancellationTokenSource) null;
            }), TaskScheduler.FromCurrentSynchronizationContext());
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "");
            this.IsEnableReloadData = true;
          }
        }));
      }
    }

    public System.Timers.Timer TimerSearch { get; }

    public Visibility VisibilityHarvester
    {
      get
      {
        return !new ConfigsRepository<Integrations>().Get().IsActiveBarcodeHarvester ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand CopyInHarvesterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string str1 = string.Empty;
          foreach (GoodsCatalogModelView.GoodsInfoGrid goods in (Collection<GoodsCatalogModelView.GoodsInfoGrid>) this.GoodsList)
          {
            string str2 = ((IEnumerable<string>) goods.Good.Barcode.Split(new string[4]
            {
              " ",
              ",",
              ".",
              ";"
            }, StringSplitOptions.RemoveEmptyEntries)).FirstOrDefault<string>() ?? "";
            string str3 = goods.Good.Name.Trim(' ', '\t', '\r', '\n').Replace('\t', ' ');
            Decimal? nullable = goods.MaxPrice;
            Decimal num;
            string str4;
            if (nullable.HasValue)
            {
              nullable = goods.MaxPrice;
              num = nullable.Value;
              str4 = num.ToString("#####0.00");
            }
            else
              str4 = "0.00";
            string str5 = str4;
            nullable = goods.GoodTotalStock;
            string str6;
            if (nullable.HasValue)
            {
              nullable = goods.GoodTotalStock;
              num = nullable.Value;
              str6 = num.ToString("#####0.000");
            }
            else
              str6 = "0.00";
            string str7 = str6;
            string description = goods.Good.Description;
            str1 = str1 + str3 + "\t" + str2 + "\t" + str5 + "\t" + str7 + "\t" + description + "\r\n";
          }
          string data = str1.Trim('\r', '\n');
          Clipboard.SetData(DataFormats.StringFormat, (object) data);
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.GoodsCatalogModelView_Barcode_Harvester,
            Text = Translate.GoodsCatalogModelView_Данные_для_Barcode_Harvester_скопированы_в_буфер_обмена
          });
        }));
      }
    }

    public Visibility VisibilitySendGoodScales
    {
      get
      {
        return this._devicesSetting.ScaleWithLable.Type != GlobalDictionaries.Devices.ScaleLableTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand SendGoodInScalesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.GoodsList.Any<GoodsCatalogModelView.GoodsInfoGrid>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_Список_не_может_быть_пустым);
          }
          else
          {
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsCatalogModelView_Загрузка_товаров_в_весы);
            try
            {
              using (ScalesWIthLabelsHelper withLabelsHelper = new ScalesWIthLabelsHelper((IConfig) new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
              {
                int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelView_В_весы_было_загружено__0__наименований_из__1_, (object) withLabelsHelper.SendGood(this.GoodsList.Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good))), (object) this.GoodsList.Count));
              }
            }
            catch (Exception ex)
            {
              LogHelper.Error(ex, "Ошибка при загрузке товаров в весы");
            }
            progressBar.Close();
          }
        }));
      }
    }

    public ICommand AddRangeGoodsInPlanFix
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.GoodsList.Any<GoodsCatalogModelView.GoodsInfoGrid>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_Список_не_может_быть_пустым);
          }
          else
          {
            (int count2, int countOld2) = PlanfixHelper.UpdateGoodPf(this.GoodsList.Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>(), this._planfixSetting, true);
            int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelView_Добавлено__0___1__товаров, (object) count2, (object) this.GoodsList.Count) + (countOld2 > 0 ? Gbs.Helpers.Other.NewLine(2) + string.Format(Translate.GoodsCatalogModelView__0___1__обновлено, (object) countOld2, (object) this.GoodsList.Count) : ""));
          }
        }));
      }
    }

    public Visibility VisibilityMenuPlanFix
    {
      get
      {
        PlanfixSetting planfixSetting = this._planfixSetting;
        return (planfixSetting != null ? (planfixSetting.IsActive ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private GoodsCatalogModelView.GoodsInfoGrid UpdateGoodGrid(Gbs.Core.Entities.Goods.Good good, SalePriceType priceType)
    {
      GoodsCatalogModelView.GoodsInfoGrid good1;
      switch (good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.None:
        case GlobalDictionaries.GoodsSetStatuses.Range:
        case GlobalDictionaries.GoodsSetStatuses.Production:
          good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good,
            MaxPrice = SaleHelper.GetSalePriceForGood(good, priceType, this.SelectedStorage),
            GoodTotalStock = good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
            {
              if (x.IsDeleted)
                return false;
              return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
            })) ? new Decimal?(good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
            {
              if (x.IsDeleted)
                return false;
              return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
            })).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock))) : new Decimal?(),
            GoodTotalIncomeSum = new Decimal?(this.GetBuyPriceForGood(good))
          };
          break;
        case GlobalDictionaries.GoodsSetStatuses.Set:
          good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good
          };
          GoodsSearchModelView.GetPriceForKit(good1);
          break;
        case GlobalDictionaries.GoodsSetStatuses.Kit:
          good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good
          };
          GoodsSearchModelView.GetPriceForKit(good1);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return good1;
    }

    public ICommand PrintCatalog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.GoodsList.Any<GoodsCatalogModelView.GoodsInfoGrid>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.LablePrintViewModel_В_списке_нет_товаров_);
          }
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForGoodsCatalog((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) this.GoodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>()), this.AuthUser);
        }));
      }
    }

    public string TextPropButton
    {
      get
      {
        int num = this.FilterProperties.Count<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked));
        if (num == this.FilterProperties.Count<GoodsCatalogModelView.FilterProperty>())
          return Translate.GoodsSearchModelView_Все_поля;
        return num != 1 ? Translate.GoodsSearchModelView_Полей__ + num.ToString() : this.FilterProperties.First<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)).Text;
      }
    }

    public List<Storages.Storage> ListStorages { get; set; }

    public string CountFilterConditionText { get; set; }

    public string PriceFilterConditionText { get; set; }

    public Storages.Storage SelectedStorage
    {
      get
      {
        return this._storage ?? (this._storage = this.ListStorages.First<Storages.Storage>((Func<Storages.Storage, bool>) (x => x.Uid == Guid.Empty)));
      }
      set
      {
        if (value == null || value == this._storage)
          return;
        List<GoodsCatalogModelView.GoodsInfoGrid> cachedDbGoods = this.CachedDbGoods;
        if ((cachedDbGoods != null ? (cachedDbGoods.Any<GoodsCatalogModelView.GoodsInfoGrid>() ? 1 : 0) : 0) == 0)
          return;
        this._storage = value;
        this.OnPropertyChanged(nameof (SelectedStorage));
        this.SearchGoods?.Execute((object) null);
      }
    }

    public FilterOptions Setting { get; set; }

    public Gbs.Core.Entities.Users.User AuthUser { private get; set; }

    public Gbs.Core.Entities.Goods.Good SelectedGood { get; set; }

    private PlanfixSetting _planfixSetting { get; }

    public GoodsCatalogModelView()
    {
      List<Storages.Storage> storageList = new List<Storages.Storage>();
      Storages.Storage storage = new Storages.Storage();
      storage.Name = Translate.WaybillsViewModel_Все_склады;
      storage.Uid = Guid.Empty;
      storageList.Add(storage);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListStorages\u003Ek__BackingField = storageList;
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetting\u003Ek__BackingField = new FilterOptions();
      this._planfixSetting = new ConfigsRepository<Integrations>().Get().Planfix;
      this._goodCollectionLock = new object();
      // ISSUE: reference to a compiler-generated field
      this.\u003CGoodsList\u003Ek__BackingField = new BulkObservableCollection<GoodsCatalogModelView.GoodsInfoGrid>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCachedDbGoods\u003Ek__BackingField = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CListFilterEqual\u003Ek__BackingField = new Dictionary<GoodsCatalogModelView.FilterEqualEnum, string>()
      {
        {
          GoodsCatalogModelView.FilterEqualEnum.NoDeletedGood,
          Translate.FrmGoodsCatalog_ВсеТовары
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.EqualBarcode,
          Translate.FrmGoodsCatalog_ОдинаковыйШК
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.EqualName,
          Translate.FrmGoodsCatalog_ОдинаковоеНазвание
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.DeleteGoods,
          Translate.GoodsCatalogModelView_ListFilterEqual_Удаленные_товары
        }
      };
      // ISSUE: reference to a compiler-generated field
      this.\u003CListSearchType\u003Ek__BackingField = new Dictionary<GoodsCatalogModelView.FilterSearchTypeEnum, string>()
      {
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Usual,
          Translate.FrmGoodsCatalog_ОбычныйПоиск
        },
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Full,
          Translate.FrmGoodsCatalog_ПолноеСовпадение
        },
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect,
          Translate.FrmGoodsCatalog_НеточноеСовпадение
        }
      };
      this._filterGoods = string.Empty;
      this._filterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterCountItems\u003Ek__BackingField = GoodsCatalogModelView._filterItems;
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterPriceItems\u003Ek__BackingField = GoodsCatalogModelView._filterItems;
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterStatusSet\u003Ek__BackingField = new Dictionary<GlobalDictionaries.GoodsSetStatuses, string>()
      {
        {
          GlobalDictionaries.GoodsSetStatuses.AllStatus,
          Translate.GoodsCatalogModelView_FilterStatusSet_Все_модификации
        },
        {
          GlobalDictionaries.GoodsSetStatuses.None,
          Translate.GlobalDictionaries_Обычный_товар
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Range,
          Translate.GoodsCatalogModelView_FilterStatusSet_Ассортимент
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Set,
          Translate.GoodsCatalogModelView_FilterStatusSet_Комплект
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Kit,
          Translate.GoodsCatalogModelView_FilterStatusSet_Набор
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Production,
          Translate.GoodsCatalogModelView_FilterStatusSet_Производство__рецепт_
        }
      };
      this._selectedStatusSet = GlobalDictionaries.GoodsSetStatuses.AllStatus;
      this._groupsListFilter = new ObservableCollection<GoodGroups.Group>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<FrmGoodsCatalog>())
        return;
      this.FilterGoods = barcode;
      Task.Run((Action) (() =>
      {
        this.SearchGoods?.Execute((object) null);
        Thread.Sleep(50);
      }));
    }

    public Visibility VisibilityBuyPrice { get; set; }

    public GoodsCatalogModelView(bool isShowBuyPrice)
    {
      List<Storages.Storage> storageList = new List<Storages.Storage>();
      Storages.Storage storage = new Storages.Storage();
      storage.Name = Translate.WaybillsViewModel_Все_склады;
      storage.Uid = Guid.Empty;
      storageList.Add(storage);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListStorages\u003Ek__BackingField = storageList;
      // ISSUE: reference to a compiler-generated field
      this.\u003CSetting\u003Ek__BackingField = new FilterOptions();
      this._planfixSetting = new ConfigsRepository<Integrations>().Get().Planfix;
      this._goodCollectionLock = new object();
      // ISSUE: reference to a compiler-generated field
      this.\u003CGoodsList\u003Ek__BackingField = new BulkObservableCollection<GoodsCatalogModelView.GoodsInfoGrid>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCachedDbGoods\u003Ek__BackingField = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CListFilterEqual\u003Ek__BackingField = new Dictionary<GoodsCatalogModelView.FilterEqualEnum, string>()
      {
        {
          GoodsCatalogModelView.FilterEqualEnum.NoDeletedGood,
          Translate.FrmGoodsCatalog_ВсеТовары
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.EqualBarcode,
          Translate.FrmGoodsCatalog_ОдинаковыйШК
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.EqualName,
          Translate.FrmGoodsCatalog_ОдинаковоеНазвание
        },
        {
          GoodsCatalogModelView.FilterEqualEnum.DeleteGoods,
          Translate.GoodsCatalogModelView_ListFilterEqual_Удаленные_товары
        }
      };
      // ISSUE: reference to a compiler-generated field
      this.\u003CListSearchType\u003Ek__BackingField = new Dictionary<GoodsCatalogModelView.FilterSearchTypeEnum, string>()
      {
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Usual,
          Translate.FrmGoodsCatalog_ОбычныйПоиск
        },
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Full,
          Translate.FrmGoodsCatalog_ПолноеСовпадение
        },
        {
          GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect,
          Translate.FrmGoodsCatalog_НеточноеСовпадение
        }
      };
      this._filterGoods = string.Empty;
      this._filterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterCountItems\u003Ek__BackingField = GoodsCatalogModelView._filterItems;
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterPriceItems\u003Ek__BackingField = GoodsCatalogModelView._filterItems;
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterStatusSet\u003Ek__BackingField = new Dictionary<GlobalDictionaries.GoodsSetStatuses, string>()
      {
        {
          GlobalDictionaries.GoodsSetStatuses.AllStatus,
          Translate.GoodsCatalogModelView_FilterStatusSet_Все_модификации
        },
        {
          GlobalDictionaries.GoodsSetStatuses.None,
          Translate.GlobalDictionaries_Обычный_товар
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Range,
          Translate.GoodsCatalogModelView_FilterStatusSet_Ассортимент
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Set,
          Translate.GoodsCatalogModelView_FilterStatusSet_Комплект
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Kit,
          Translate.GoodsCatalogModelView_FilterStatusSet_Набор
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Production,
          Translate.GoodsCatalogModelView_FilterStatusSet_Производство__рецепт_
        }
      };
      this._selectedStatusSet = GlobalDictionaries.GoodsSetStatuses.AllStatus;
      this._groupsListFilter = new ObservableCollection<GoodGroups.Group>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Stopwatch stopwatch1 = new Stopwatch();
      stopwatch1.Start();
      this.Setting = new ConfigsRepository<FilterOptions>().Get();
      this.TimerSearch.Elapsed += new ElapsedEventHandler(this.TimerSearchOnElapsed);
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        this.ListStorages.AddRange(Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => !x.IS_DELETED))));
      this.VisibilityBuyPrice = isShowBuyPrice ? Visibility.Visible : Visibility.Collapsed;
      this.GetGoodThread();
      Task.Run((Action) (() =>
      {
        try
        {
          this.IsEnabledSupp = false;
          Stopwatch stopwatch2 = Stopwatch.StartNew();
          stopwatch2.Start();
          stopwatch2.Stop();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
        finally
        {
          this.IsEnabledSupp = true;
        }
      }));
      ObservableCollection<GoodsCatalogModelView.FilterProperty> observableCollection1 = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.FrmReadExcel_Название,
        IsChecked = this.Setting.GoodsCatalog.GoodProp.IsCheckedName
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Barcode",
        Text = Translate.FrmAuthorization_ШтрихКод,
        IsChecked = this.Setting.GoodsCatalog.GoodProp.IsCheckedBarcode
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Description",
        Text = Translate.ExcelDataViewModel_Описание,
        IsChecked = this.Setting.SearchGood.GoodProp.IsCheckedBarcodes
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Barcodes",
        Text = Translate.ExcelDataViewModel_Доп__штрих_коды,
        IsChecked = this.Setting.GoodsCatalog.GoodProp.IsCheckedBarcodes
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "ModificationBarcode",
        Text = Translate.GoodsCatalogModelView_GoodsCatalogModelView_Штрих_коды_ассортимента,
        IsChecked = this.Setting.GoodsCatalog.GoodProp.IsCheckedModificationBarcode
      });
      ObservableCollection<GoodsCatalogModelView.FilterProperty> collection = observableCollection1;
      foreach (EntityProperties.PropertyType propertyType in (IEnumerable<EntityProperties.PropertyType>) EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good, false).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid))).OrderBy<EntityProperties.PropertyType, string>((Func<EntityProperties.PropertyType, string>) (x => x.Name)))
      {
        EntityProperties.PropertyType type = propertyType;
        if (!(type.Uid == GlobalDictionaries.AlcCodeUid) || new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        {
          ObservableCollection<GoodsCatalogModelView.FilterProperty> observableCollection2 = collection;
          GoodsCatalogModelView.FilterProperty filterProperty = new GoodsCatalogModelView.FilterProperty();
          filterProperty.Name = type.Uid.ToString();
          filterProperty.Text = type.Name;
          GoodProp.PropItem propItem = this.Setting.GoodsCatalog.GoodProp.PropList.FirstOrDefault<GoodProp.PropItem>((Func<GoodProp.PropItem, bool>) (x => x.Uid == type.Uid));
          filterProperty.IsChecked = propItem != null && propItem.IsChecked;
          observableCollection2.Add(filterProperty);
        }
      }
      this.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>((IEnumerable<GoodsCatalogModelView.FilterProperty>) collection);
      this.AddGoods = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.Users.User user = (Gbs.Core.Entities.Users.User) null;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsCreate))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsCreate);
            if (!access.Result)
              return;
            user = access.User;
          }
          Gbs.Core.Entities.Goods.Good good1 = new Gbs.Core.Entities.Goods.Good();
          if (!this.FilterGoods.IsNullOrEmpty())
          {
            if (double.TryParse(this.FilterGoods, out double _))
              good1.Barcode = this.FilterGoods;
            else
              good1.Name = this.FilterGoods;
          }
          if (this.GroupsListFilter.Count == 1)
            good1.Group = this.GroupsListFilter.Single<GoodGroups.Group>();
          Gbs.Core.Entities.Goods.Good good2;
          if (!new FrmGoodCard().ShowGoodCard(Guid.Empty, out good2, authUser: user ?? this.AuthUser, goodNew: good1))
            return;
          SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
          GoodsCatalogModelView.GoodsInfoGrid good3 = this.UpdateGoodGrid(good1, salePriceType);
          good3.LastBuyPrice = new Decimal?(this.BuyPriceCounter.GetLastBuyPrice(good2));
          this.GoodsList.Insert(0, good3);
          this.CachedDbGoods.Insert(0, good3);
          this.CountSum(false);
          this.UpdateGoodInPlanFix(good3);
          GoodsCatalogModelView.LoadingImageForOneGood(good3);
        }
      }));
      this.EditGoods = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedGood != null)
        {
          if (((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().Count<GoodsCatalogModelView.GoodsInfoGrid>() > 1 && ((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().Count<GoodsCatalogModelView.GoodsInfoGrid>() > 1)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Возможно_выбрать_только_один_товар_);
          }
          else
          {
            Gbs.Core.Entities.Users.User user = (Gbs.Core.Entities.Users.User) null;
            using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsEdit))
              {
                (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsEdit);
                if (!access.Result)
                  return;
                user = access.User;
              }
              Gbs.Core.Entities.Goods.Good good;
              if (!new FrmGoodCard().ShowGoodCard(this.SelectedGood.Uid, out good, true, user ?? this.AuthUser))
                return;
              SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
              GoodsCatalogModelView.GoodsInfoGrid good4 = this.UpdateGoodGrid(good, salePriceType);
              good4.LastBuyPrice = new Decimal?(this.BuyPriceCounter.GetLastBuyPrice(good));
              this.GoodsList[this.GoodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Uid))] = good4;
              this.CachedDbGoods[this.CachedDbGoods.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Uid))] = good4;
              if (this.SelectedFilterEqual == GoodsCatalogModelView.FilterEqualEnum.DeleteGoods)
                this.SearchForFilter();
              this.CountSum(false);
              this.UpdateGoodInPlanFix(good4);
              GoodsCatalogModelView.LoadingImageForOneGood(good4);
            }
          }
        }
        else
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Требуется_выбрать_нужный_товар, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.CopyGoods = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedGood != null)
        {
          if (MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelView_Вы_уверены__что_хотите_сделать_копии__0__товаров_, (object) ((ICollection) obj).Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          this.AuthUser.Clone();
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsCreate) && !new Authorization().GetAccess(Actions.GoodsCreate).Result)
              return;
            List<GoodsCatalogModelView.GoodsInfoGrid> list = ((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>();
            int num3 = 0;
            int num4 = 0;
            SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
            foreach (GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid in list)
            {
              if (goodsInfoGrid.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
              {
                ++num4;
              }
              else
              {
                (bool Result2, Gbs.Core.Entities.Goods.Good good6) = new GoodRepository(dataBase).SaveCopyGood(goodsInfoGrid.Good);
                if (Result2)
                {
                  GoodsCatalogModelView.GoodsInfoGrid good7 = this.UpdateGoodGrid(good6, salePriceType);
                  ++num3;
                  this.CachedDbGoods.Insert(0, good7);
                  this.GoodsList.Insert(0, good7);
                  this.UpdateGoodInPlanFix(good7);
                  ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) new Gbs.Core.Entities.Goods.Good(), (IEntity) good7.Good, ActionType.Add, GlobalDictionaries.EntityTypes.Good, this.AuthUser), true);
                }
              }
            }
            if (num4 != 0)
            {
              int num5 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelViewСкопированоЯвляютсяСертификатамиИНеБылиСкопированы, (object) num3, (object) list.ToList<GoodsCatalogModelView.GoodsInfoGrid>().Count<GoodsCatalogModelView.GoodsInfoGrid>(), (object) num4, (object) list.Count<GoodsCatalogModelView.GoodsInfoGrid>()));
            }
            this.CountSum(false);
          }
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Требуется_выбрать_нужный_товар, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteGoods = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedGood == null)
        {
          int num6 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Требуется_выбрать_нужный_товар, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else
        {
          List<GoodsCatalogModelView.GoodsInfoGrid> list = ((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
          {
            int num7 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelView_Удалить_услугу___0___нельзя__исключите_ее_из_списка__, (object) list.First<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).Good.Name), icon: MessageBoxImage.Exclamation);
          }
          else if (this.SelectedGood.IsDeleted)
          {
            int num8 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_GoodsCatalogModelView_Данные_товары_уже_являются_удаленными__выберите_другие_товары_);
          }
          else
          {
            using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              Gbs.Core.Entities.Users.User user = this.AuthUser.Clone();
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsDelete))
              {
                (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsDelete);
                if (!access.Result)
                  return;
                user = access.User;
              }
              if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) ((ICollection) obj).Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsCatalogModelView_Удаление_товаров_из_каталога);
              foreach (GoodsCatalogModelView.GoodsInfoGrid source in list)
              {
                GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = source.Clone<GoodsCatalogModelView.GoodsInfoGrid>();
                source.Good.IsDeleted = true;
                new GoodRepository(dataBase).Save(source.Good, false);
                this.GoodsList.Remove(source);
                this.TotalGoodsStock -= source.GoodTotalStock.GetValueOrDefault();
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) goodsInfoGrid.Good, (IEntity) source.Good, ActionType.Delete, GlobalDictionaries.EntityTypes.Good, user), false);
              }
              if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
                new HomeOfficeHelper().PrepareAndSend<List<Gbs.Core.Entities.Goods.Good>>(list.Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>(), HomeOfficeHelper.EntityEditHome.GoodList);
              progressBar.Close();
              this.CountSum(false);
            }
          }
        }
      }));
      this.SearchGoods = (ICommand) new RelayCommand((Action<object>) (obj => this._taskSearch = Task.Run(new Action(this.SearchForFilter))));
      this.AddFromExcel = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsCreate) && !new Authorization().GetAccess(Actions.GoodsCreate).Result || !new FrmExcelData().Import(this.AuthUser))
            return;
          this.GetGoodThread();
        }
      }));
      this.GetSupplier = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (Client client, bool result) client = new FrmSearchClient().GetClient(true);
        this.Supplier = client.result ? client.client : (Client) null;
      }));
      this.SelectedFilterCount = this.FilterCountItems[1];
      this.SelectedFilterPrice = this.FilterPriceItems[0];
      this.CountFilterConditionText = this.SelectedFilterCount.Content.ToString();
      this.PriceFilterConditionText = this.SelectedFilterPrice.Content.ToString();
      stopwatch1.Stop();
    }

    private void TimerSearchOnElapsed(object sender, ElapsedEventArgs e)
    {
      this.TimerSearch.Stop();
      this.SearchForFilter();
    }

    private void UpdateGoodInPlanFix(GoodsCatalogModelView.GoodsInfoGrid good)
    {
      Task.Run((Action) (() =>
      {
        if (!this._planfixSetting.IsActive)
          return;
        PlanfixHelper.UpdateGoodPf(new List<Gbs.Core.Entities.Goods.Good>()
        {
          good.Good
        }, this._planfixSetting);
      }));
    }

    private Decimal GetBuyPriceForGood(Gbs.Core.Entities.Goods.Good good)
    {
      return good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Kit ? 0M : good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
      {
        if (x.IsDeleted || !(x.Stock != 0M))
          return false;
        return this.SelectedStorage.Uid == Guid.Empty || x.Storage.Uid == this.SelectedStorage.Uid;
      })).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock * this.BuyPriceCounter.GetBuyPrice(x.Uid)));
    }

    private void SearchForFilter()
    {
      string filterText = this.FilterGoods.ToLower();
      List<GoodsCatalogModelView.GoodsInfoGrid> goodList = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.SelectedFilterEqual == GoodsCatalogModelView.FilterEqualEnum.DeleteGoods && !this._isLoadingDeleteGood)
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          this.CachedDbGoods.AddRange(new GoodRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.IS_DELETED == true))).Select<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>((Func<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>) (x => new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = x
          })));
          this.CountSum(true);
          this._isLoadingDeleteGood = true;
        }
      }
      if (this.SelectedSearchType.IsEither<GoodsCatalogModelView.FilterSearchTypeEnum>(GoodsCatalogModelView.FilterSearchTypeEnum.Usual, GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect))
      {
        if (filterText.IsNullOrEmpty())
        {
          goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) this.CachedDbGoods);
        }
        else
        {
          foreach (GoodsCatalogModelView.FilterProperty filterProperty1 in this.FilterProperties.Where<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
          {
            GoodsCatalogModelView.FilterProperty filterProperty = filterProperty1;
            Guid result;
            if (Guid.TryParse(filterProperty.Name, out result))
            {
              if (result == GlobalDictionaries.GoodIdUid)
              {
                int intValue;
                if (int.TryParse(filterText, out intValue))
                  goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && int.Parse(p.Value.ToString()) == intValue)))));
              }
              else
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && p.Value.ToString().ToLower().Contains(filterText))))));
            }
            else
            {
              switch (filterProperty.Name)
              {
                case "Name":
                  goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Name.ToLower().Contains(filterText))));
                  IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> source1 = ((IEnumerable<string>) filterText.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>>(this.CachedDbGoods.AsEnumerable<GoodsCatalogModelView.GoodsInfoGrid>(), (Func<IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>, string, IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>>) ((current, s) => current.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Name.ToLower().Contains(s)))));
                  goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) source1.ToList<GoodsCatalogModelView.GoodsInfoGrid>());
                  continue;
                case "Barcode":
                  List<GoodsCatalogModelView.GoodsInfoGrid> list1 = this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcode.ToLower().Contains(filterText))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
                  goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) list1);
                  continue;
                case "Barcodes":
                  List<GoodsCatalogModelView.GoodsInfoGrid> list2 = this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcodes.Any<string>((Func<string, bool>) (barcode => barcode.ToLower().Contains(filterText))))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
                  goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) list2);
                  continue;
                case "Description":
                  goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Description.ToLower().Contains(filterText))));
                  IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> source2 = ((IEnumerable<string>) filterText.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>>(this.CachedDbGoods.AsEnumerable<GoodsCatalogModelView.GoodsInfoGrid>(), (Func<IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>, string, IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>>) ((current, s) => current.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Description.ToLower().Contains(s)))));
                  goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) source2.ToList<GoodsCatalogModelView.GoodsInfoGrid>());
                  continue;
                case "ModificationBarcode":
                  goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Modifications.Any<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Barcode.ToLower().Contains(filterText))))));
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (this.SelectedSearchType == GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect)
      {
        int firstDist = this.CachedDbGoods.Min<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, int>) (x => LevinshtaingHelper.IsSimilarTo(x.Good.Name, filterText)));
        goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => LevinshtaingHelper.IsSimilarTo(x.Good.Name, filterText) <= firstDist)));
      }
      if (this.SelectedSearchType == GoodsCatalogModelView.FilterSearchTypeEnum.Full)
      {
        foreach (GoodsCatalogModelView.FilterProperty filterProperty2 in this.FilterProperties.Where<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          GoodsCatalogModelView.FilterProperty filterProperty = filterProperty2;
          Guid result;
          if (Guid.TryParse(filterProperty.Name, out result))
          {
            if (result == GlobalDictionaries.GoodIdUid)
            {
              int intValue;
              if (int.TryParse(filterText, out intValue))
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && int.Parse(p.Value.ToString()) == intValue)))));
            }
            else
              goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && p.Value.ToString().ToLower() == filterText)))));
          }
          else
          {
            switch (filterProperty.Name)
            {
              case "Name":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Name.ToLower() == filterText)));
                continue;
              case "Barcode":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcode.ToLower() == filterText)));
                continue;
              case "Barcodes":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcodes.Any<string>((Func<string, bool>) (barcode => barcode.ToLower() == filterText)))));
                continue;
              case "Description":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Description.ToLower() == filterText)));
                continue;
              case "ModificationBarcode":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Modifications.Any<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Barcode.ToLower() == filterText)))));
                continue;
              default:
                continue;
            }
          }
        }
      }
      goodList = goodList.OrderBy<GoodsCatalogModelView.GoodsInfoGrid, DateTime>((Func<GoodsCatalogModelView.GoodsInfoGrid, DateTime>) (x => x.Good.DateAdd)).Reverse<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      switch (this.SelectedFilterEqual)
      {
        case GoodsCatalogModelView.FilterEqualEnum.NoDeletedGood:
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => !x.Good.IsDeleted)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          break;
        case GoodsCatalogModelView.FilterEqualEnum.DeleteGoods:
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.IsDeleted)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          break;
        case GoodsCatalogModelView.FilterEqualEnum.EqualBarcode:
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcode != string.Empty && !x.Good.IsDeleted)).GetDuplicate<GoodsCatalogModelView.GoodsInfoGrid, string>((Func<GoodsCatalogModelView.GoodsInfoGrid, string>) (x => x.Good.Barcode.Trim())).OrderBy<GoodsCatalogModelView.GoodsInfoGrid, string>((Func<GoodsCatalogModelView.GoodsInfoGrid, string>) (x => x.Good.Barcode)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          break;
        case GoodsCatalogModelView.FilterEqualEnum.EqualName:
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => !x.Good.IsDeleted)).GetDuplicate<GoodsCatalogModelView.GoodsInfoGrid, string>((Func<GoodsCatalogModelView.GoodsInfoGrid, string>) (x => x.Good.Name.Trim().ToLower())).OrderBy<GoodsCatalogModelView.GoodsInfoGrid, string>((Func<GoodsCatalogModelView.GoodsInfoGrid, string>) (x => x.Good.Name)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          break;
      }
      goodList = goodList.GroupBy<GoodsCatalogModelView.GoodsInfoGrid, Guid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Guid>) (x => x.Good.Uid)).Select<IGrouping<Guid, GoodsCatalogModelView.GoodsInfoGrid>, GoodsCatalogModelView.GoodsInfoGrid>((Func<IGrouping<Guid, GoodsCatalogModelView.GoodsInfoGrid>, GoodsCatalogModelView.GoodsInfoGrid>) (x => x.First<GoodsCatalogModelView.GoodsInfoGrid>())).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.SelectedStatusSet != GlobalDictionaries.GoodsSetStatuses.AllStatus)
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus == this.SelectedStatusSet)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.FilterPrice.HasValue)
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => MathHelper.CompareNumbers(new Decimal?(s.Price), this.FilterPrice, this.PriceFilterConditionText))))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.FilterCount.HasValue)
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => MathHelper.CompareNumbers(x.GoodTotalStock, this.FilterCount, this.CountFilterConditionText))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.Supplier != null)
      {
        Guid uid = this.Supplier.Uid;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          List<Document> waybills = new DocumentsRepository(dataBase).GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
          {
            DateStart = DateTime.MinValue,
            DateEnd = DateTime.Now,
            Types = new GlobalDictionaries.DocumentsTypes[1]
            {
              GlobalDictionaries.DocumentsTypes.Buy
            },
            IgnoreTime = false,
            IncludeDeleted = true,
            ContractorUid = uid
          });
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => waybills.Any<Document>((Func<Document, bool>) (w => w.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodStock.GoodUid == x.Good.Uid)))))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
        }
      }
      if (this.SelectedStorage != null && this.SelectedStorage.Uid != Guid.Empty)
      {
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == this.SelectedStorage.Uid)))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
        SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
        List<GoodsCatalogModelView.GoodsInfoGrid> goodsInfoGridList = new List<GoodsCatalogModelView.GoodsInfoGrid>();
        foreach (GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid1 in goodList)
        {
          GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid2 = this.UpdateGoodGrid(goodsInfoGrid1.Good, salePriceType);
          goodsInfoGrid2.LastBuyPrice = goodsInfoGrid1.LastBuyPrice;
          goodsInfoGridList.Add(goodsInfoGrid2);
        }
        goodList = goodsInfoGridList;
      }
      if (this.GroupsListFilter.Any<GoodGroups.Group>())
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => this.GroupsListFilter.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Good.Group.Uid)))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        this.GoodsList.Clear();
        this.GoodsList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) goodList);
      }));
      this.OnPropertyChanged("GoodsList");
      this.CountSum(false);
    }

    private void CountSum(bool reCalcCache)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsCatalogModelView_Подсчет_итогов);
      try
      {
        if (reCalcCache)
        {
          this.CalculateSalePriceSum();
          this.CalculateBuyPrice();
        }
        List<GoodsCatalogModelView.GoodsInfoGrid> list = this.GoodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>();
        Decimal sumInPrice = list.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Production))).Sum<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Decimal>) (g => g.GoodTotalIncomeSum.GetValueOrDefault()));
        sumInPrice += list.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Production))).Sum<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Decimal>) (g =>
        {
          Decimal? nullable = g.LastBuyPrice;
          Decimal valueOrDefault1 = nullable.GetValueOrDefault();
          nullable = g.GoodTotalStock;
          Decimal valueOrDefault2 = nullable.GetValueOrDefault();
          return valueOrDefault1 * valueOrDefault2;
        }));
        Decimal totalStock = list.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production, GlobalDictionaries.GoodsSetStatuses.None))).Sum<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Decimal>) (x => x.GoodTotalStock.GetValueOrDefault()));
        Decimal sumPrice = list.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Production))).Sum<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Decimal>) (g => g.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
        {
          if (x.IsDeleted)
            return false;
          return this.SelectedStorage.Uid == Guid.Empty || x.Storage.Uid == this.SelectedStorage.Uid;
        })).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock * x.Price))));
        Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          this.SumInPriceGoods = sumInPrice;
          this.TotalGoodsStock = totalStock;
          this.SumPriceGoods = sumPrice;
        }));
      }
      finally
      {
        progressBar.Close();
      }
    }

    private void GetGoodThread()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsCatalogModelView_Загрузка_каталога_товаров);
      try
      {
        this.GetGoodsItems();
        this.CountSum(true);
        Application.Current?.Dispatcher?.Invoke((Action) (() => CollectionViewSource.GetDefaultView((object) this.GoodsList).Refresh()));
      }
      finally
      {
        progressBar.Close();
      }
    }

    private BuyPriceCounter BuyPriceCounter { get; set; }

    private void CalculateSalePriceSum()
    {
      Performancer performancer = new Performancer("Расчет сумм розничных цен в каталоге");
      this.IsEnableReloadData = false;
      this.IsEnabledParameters = false;
      try
      {
        this.SumPriceGoods = 0M;
        if (this.cts.Token.IsCancellationRequested)
        {
          Gbs.Helpers.Other.ConsoleWrite("Операция прервана");
          return;
        }
        performancer.AddPoint("point 10");
        SalePriceType typePrice = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
        List<Task> list1 = new List<Task>();
        foreach (IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> chunk1 in this.CachedDbGoods.ToChunks<GoodsCatalogModelView.GoodsInfoGrid>(10))
        {
          IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> chunk = chunk1;
          Task task = new Task((Action) (() =>
          {
            foreach (GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid1 in chunk)
            {
              GoodsCatalogModelView.GoodsInfoGrid good = goodsInfoGrid1;
              int index = this.CachedDbGoods.FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Good.Uid));
              if (index != -1)
              {
                GoodsCatalogModelView.GoodsInfoGrid cachedDbGood = this.CachedDbGoods[index];
                if (cachedDbGood != null)
                {
                  if (cachedDbGood.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit))
                    GoodsSearchModelView.GetPriceForKit(cachedDbGood);
                  else if (cachedDbGood.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set))
                  {
                    GoodsSearchModelView.GetPriceForKit(cachedDbGood);
                  }
                  else
                  {
                    Decimal? nullable1;
                    if (good.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
                    {
                      if (x.IsDeleted)
                        return false;
                      return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
                    })))
                    {
                      List<GoodsStocks.GoodStock> list2 = good.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
                      {
                        if (x.IsDeleted)
                          return false;
                        return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
                      })).ToList<GoodsStocks.GoodStock>();
                      cachedDbGood.MaxPrice = SaleHelper.GetSalePriceForGood(good.Good, typePrice, this.SelectedStorage);
                      if (cachedDbGood.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
                      {
                        GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid2 = cachedDbGood;
                        nullable1 = new Decimal?();
                        Decimal? nullable2 = nullable1;
                        goodsInfoGrid2.GoodTotalStock = nullable2;
                      }
                      else
                        cachedDbGood.GoodTotalStock = new Decimal?(list2.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
                    }
                    else
                    {
                      GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid3 = cachedDbGood;
                      nullable1 = new Decimal?();
                      Decimal? nullable3 = nullable1;
                      goodsInfoGrid3.MaxPrice = nullable3;
                      GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid4 = cachedDbGood;
                      nullable1 = new Decimal?();
                      Decimal? nullable4 = nullable1;
                      goodsInfoGrid4.GoodTotalStock = nullable4;
                    }
                    Decimal totalGoodsStock = this.TotalGoodsStock;
                    nullable1 = cachedDbGood.GoodTotalStock;
                    Decimal valueOrDefault = nullable1.GetValueOrDefault();
                    this.TotalGoodsStock = totalGoodsStock + valueOrDefault;
                  }
                }
              }
            }
          }));
          list1.Add(task);
        }
        list1.RunList(true);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        string пересчетаСуммВКаталоге = Translate.GoodsCatalogModelView_Ошибка_пересчета_сумм_в_каталоге;
        LogHelper.ShowErrorMgs(ex, пересчетаСуммВКаталоге, LogHelper.MsgTypes.Notification);
      }
      performancer.Stop();
    }

    private void CalculateBuyPrice()
    {
      Performancer performancer = new Performancer("Расчет сумм закупочных цен в каталоге");
      try
      {
        this.SumInPriceGoods = 0M;
        if (this.cts.Token.IsCancellationRequested)
        {
          Gbs.Helpers.Other.ConsoleWrite("Операция прервана");
          return;
        }
        this.BuyPriceCounter = new BuyPriceCounter();
        performancer.AddPoint("point 10");
        List<Task> list = new List<Task>();
        foreach (IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> chunk1 in this.CachedDbGoods.ToChunks<GoodsCatalogModelView.GoodsInfoGrid>(10))
        {
          IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> chunk = chunk1;
          Task task = new Task((Action) (() =>
          {
            foreach (GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid in chunk)
            {
              GoodsCatalogModelView.GoodsInfoGrid good = goodsInfoGrid;
              int index = this.CachedDbGoods.FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Good.Uid));
              if (index != -1)
              {
                GoodsCatalogModelView.GoodsInfoGrid cachedDbGood = this.CachedDbGoods[index];
                if (cachedDbGood != null)
                {
                  if (!cachedDbGood.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit))
                  {
                    if (cachedDbGood.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set))
                      cachedDbGood.LastBuyPrice = new Decimal?(this.BuyPriceCounter.GetLastBuyPrice(good.Good));
                    else if (good.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
                    {
                      if (x.IsDeleted)
                        return false;
                      return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
                    })))
                    {
                      cachedDbGood.LastBuyPrice = new Decimal?(this.BuyPriceCounter.GetLastBuyPrice(good.Good));
                      Decimal buyPriceForGood = this.GetBuyPriceForGood(good.Good);
                      cachedDbGood.GoodTotalIncomeSum = new Decimal?(buyPriceForGood);
                      this.SumInPriceGoods += buyPriceForGood;
                    }
                  }
                }
              }
            }
          }));
          list.Add(task);
        }
        performancer.AddPoint("chunks created");
        list.RunList(true);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        string пересчетаСуммВКаталоге = Translate.GoodsCatalogModelView_Ошибка_пересчета_сумм_в_каталоге;
        LogHelper.ShowErrorMgs(ex, пересчетаСуммВКаталоге, LogHelper.MsgTypes.Notification);
      }
      this.IsEnabledParameters = true;
      this.IsEnableReloadData = true;
      performancer.Stop();
    }

    private void GetGoodsItems()
    {
      Performancer performancer = new Performancer("Получение товаров для каталога");
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          Application.Current?.Dispatcher?.Invoke((Action) (() => this.IsEnableReloadData = false));
          List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
          performancer.AddPoint("10");
          List<GoodsCatalogModelView.GoodsInfoGrid> newItems = activeItems.ToList<Gbs.Core.Entities.Goods.Good>().Select<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>((Func<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>) (x => new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = x
          })).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
          Application.Current?.Dispatcher?.Invoke((Action) (() =>
          {
            this.CachedDbGoods = newItems;
            this.GoodsList.Clear();
            this.GoodsList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) newItems.OrderByDescending<GoodsCatalogModelView.GoodsInfoGrid, DateTime>((Func<GoodsCatalogModelView.GoodsInfoGrid, DateTime>) (x => x.Good.DateAdd)));
            this.OnPropertyChanged("GoodsList");
          }));
          this.TotalGoodsStock = 0M;
          this.SumInPriceGoods = 0M;
          this.SumPriceGoods = 0M;
        }
      }
      finally
      {
        performancer.Stop();
      }
    }

    public Visibility VisibilityStock { get; set; }

    private static IEnumerable<GoodGroups.Group> ListGroups { get; set; }

    public Decimal TotalGoodsStock
    {
      get => this._totalGoodsStock;
      set
      {
        this._totalGoodsStock = value;
        this.OnPropertyChanged(nameof (TotalGoodsStock));
      }
    }

    public Decimal SumPriceGoods
    {
      get => this._sumPriceGoods;
      set
      {
        this._sumPriceGoods = value;
        this.OnPropertyChanged(nameof (SumPriceGoods));
      }
    }

    public Decimal SumInPriceGoods
    {
      get => this._sumInPriceGoods;
      set
      {
        this._sumInPriceGoods = value;
        this.OnPropertyChanged(nameof (SumInPriceGoods));
      }
    }

    public BulkObservableCollection<GoodsCatalogModelView.GoodsInfoGrid> GoodsList { get; set; }

    private List<GoodsCatalogModelView.GoodsInfoGrid> CachedDbGoods { get; set; }

    private List<Document> CachedDbWaybills { get; set; }

    public ICommand GetSupplier { get; set; }

    private Client Supplier
    {
      get => this._supplier;
      set
      {
        this._supplier = value;
        this.SearchGoods?.Execute((object) null);
        this.OnPropertyChanged("ButtonContentSup");
      }
    }

    public string ButtonContentSup
    {
      get => this.Supplier != null ? this.Supplier.Name : Translate.FrmGoodsCatalog_ВсеПоставщики;
    }

    public bool IsEnabledParameters
    {
      get => this._isEnabledParameters;
      set
      {
        this._isEnabledParameters = value;
        this.OnPropertyChanged(nameof (IsEnabledParameters));
      }
    }

    public bool IsEnabledSupp
    {
      get => this._isEnabledSupp;
      set
      {
        this._isEnabledSupp = value;
        this.OnPropertyChanged(nameof (IsEnabledSupp));
      }
    }

    public ListBoxItem SelectedFilterPrice
    {
      get => this._selectedFilterPrice;
      set
      {
        this._selectedFilterPrice = value;
        if (!this.FilterPrice.HasValue)
          return;
        this.StartSearchTimer();
      }
    }

    public Decimal? FilterPrice
    {
      get => this._filterPrice;
      set
      {
        this._filterPrice = value;
        this.StartSearchTimer();
      }
    }

    public ListBoxItem SelectedFilterCount
    {
      get => this._selectedFilterCount;
      set
      {
        this._selectedFilterCount = value;
        if (!this.FilterCount.HasValue)
          return;
        this.StartSearchTimer();
      }
    }

    public Decimal? FilterCount
    {
      get => this._filterCount;
      set
      {
        this._filterCount = value;
        this.StartSearchTimer();
      }
    }

    private void StartSearchTimer()
    {
      if (this.TimerSearch.Enabled)
        this.TimerSearch.Stop();
      this.TimerSearch.Start();
    }

    public Dictionary<GoodsCatalogModelView.FilterEqualEnum, string> ListFilterEqual { get; set; }

    public GoodsCatalogModelView.FilterSearchTypeEnum SelectedSearchType
    {
      get
      {
        if (this.Setting.SearchGood.IsIncorrectSearch)
          return GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect;
        return this.Setting.SearchGood.IsUsualSearch || !this.Setting.SearchGood.IsFullSearch ? GoodsCatalogModelView.FilterSearchTypeEnum.Usual : GoodsCatalogModelView.FilterSearchTypeEnum.Full;
      }
      set
      {
        this.Setting.SearchGood.IsFullSearch = false;
        this.Setting.SearchGood.IsIncorrectSearch = false;
        this.Setting.SearchGood.IsUsualSearch = false;
        switch (value)
        {
          case GoodsCatalogModelView.FilterSearchTypeEnum.Full:
            this.Setting.SearchGood.IsFullSearch = true;
            break;
          case GoodsCatalogModelView.FilterSearchTypeEnum.Usual:
            this.Setting.SearchGood.IsUsualSearch = true;
            break;
          case GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect:
            this.Setting.SearchGood.IsIncorrectSearch = true;
            break;
        }
        this.SearchGoods?.Execute((object) null);
      }
    }

    public Dictionary<GoodsCatalogModelView.FilterSearchTypeEnum, string> ListSearchType { get; set; }

    public GoodsCatalogModelView.FilterEqualEnum SelectedFilterEqual
    {
      get => this._selectedFilterEqual;
      set
      {
        this._selectedFilterEqual = value;
        this.StartSearchTimer();
      }
    }

    public string FilterGoods
    {
      get => this._filterGoods;
      set
      {
        this._filterGoods = value;
        this.OnPropertyChanged(nameof (FilterGoods));
        this.StartSearchTimer();
      }
    }

    public ObservableCollection<GoodsCatalogModelView.FilterProperty> FilterProperties
    {
      get => this._filterProperties;
      set
      {
        ConfigsRepository<FilterOptions> configsRepository = new ConfigsRepository<FilterOptions>();
        this._filterProperties = value;
        FilterOptions setting = this.Setting;
        configsRepository.Save(setting);
        this.OnPropertyChanged("TextPropButton");
        if (value.Any<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
          return;
        int num = (int) MessageBoxHelper.Show(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
      }
    }

    private static List<ListBoxItem> _filterItems
    {
      get
      {
        List<ListBoxItem> filterItems = new List<ListBoxItem>();
        ListBoxItem listBoxItem1 = new ListBoxItem();
        listBoxItem1.Content = (object) "=";
        filterItems.Add(listBoxItem1);
        ListBoxItem listBoxItem2 = new ListBoxItem();
        listBoxItem2.Content = (object) ">";
        filterItems.Add(listBoxItem2);
        ListBoxItem listBoxItem3 = new ListBoxItem();
        listBoxItem3.Content = (object) "<";
        filterItems.Add(listBoxItem3);
        return filterItems;
      }
    }

    public List<ListBoxItem> FilterCountItems { get; set; }

    public List<ListBoxItem> FilterPriceItems { get; set; }

    public Dictionary<GlobalDictionaries.GoodsSetStatuses, string> FilterStatusSet { get; set; }

    public GlobalDictionaries.GoodsSetStatuses SelectedStatusSet
    {
      get => this._selectedStatusSet;
      set
      {
        this._selectedStatusSet = value;
        this.SearchGoods?.Execute((object) null);
      }
    }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
        this.SearchForFilter();
      }
    }

    public ICommand AddGoods { get; set; }

    public ICommand EditGoods { get; set; }

    public ICommand CopyGoods { get; set; }

    public ICommand JoinGoods
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            List<GoodsCatalogModelView.GoodsInfoGrid> list = ((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>();
            if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
            {
              int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodsCatalogModelView_Объединить_услугу___0___нельзя__исключите_ее_из_списка__, (object) list.First<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)).Good.Name), icon: MessageBoxImage.Exclamation);
            }
            else
            {
              Gbs.Core.Entities.Users.User user = this.AuthUser.Clone();
              using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
              {
                if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.GoodsJoin))
                {
                  (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsJoin);
                  if (!access.Result)
                    return;
                  user = access.User;
                }
                if (list.Count < 2)
                {
                  int num3 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Требуется_выбрать_2_или_более_товара_для_объединения);
                }
                else
                {
                  GoodsCatalogModelView.GoodsInfoGrid itemFirst = list.First<GoodsCatalogModelView.GoodsInfoGrid>();
                  if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Group.GoodsType != itemFirst.Good.Group.GoodsType)))
                  {
                    int num4 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Все_товары_из_списка_должны_быть_одного_типа_);
                  }
                  else if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
                  {
                    int num5 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Объединить_Сертификаты_нельзя__нужно_убрать_их_списка_);
                  }
                  else if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)))
                  {
                    int num6 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_JoinGoods_Объединять_товары_рецепты_для_производства_недопустимо_);
                  }
                  else if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit))))
                  {
                    int num7 = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Объединить_составные_товары_нельзя__нужно_убрать_их_списка_, icon: MessageBoxImage.Exclamation);
                  }
                  else
                  {
                    if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.IsDeleted)) && MessageBoxHelper.Show(Translate.GoodsCatalogModelView_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No || MessageBoxHelper.Show(string.Format(string.Format(Translate.GoodsCatalogModelView_Вы_уверены__что_хотите_объединить__0__товаров_, (object) list.Count), (object) ((ICollection) obj).Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                      return;
                    ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.UserGroupViewModel_Объединение_товаров);
                    list.RemoveAll((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == itemFirst.Good.Uid));
                    new GoodRepository(dataBase).RemoveGood(list.Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>(), itemFirst.Good.Uid);
                    itemFirst.Good.IsDeleted = false;
                    List<string> barcodes = itemFirst.Good.Barcodes.ToList<string>();
                    barcodes.AddRange(list.SelectMany<GoodsCatalogModelView.GoodsInfoGrid, string>((Func<GoodsCatalogModelView.GoodsInfoGrid, IEnumerable<string>>) (x => x.Good.Barcodes)));
                    list.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => !x.Good.Barcode.IsNullOrEmpty())).ToList<GoodsCatalogModelView.GoodsInfoGrid>().ForEach((Action<GoodsCatalogModelView.GoodsInfoGrid>) (x => barcodes.Add(x.Good.Barcode)));
                    barcodes = barcodes.Distinct<string>().ToList<string>();
                    itemFirst.Good.Barcodes = (IEnumerable<string>) new List<string>((IEnumerable<string>) barcodes);
                    new GoodRepository(dataBase).Save(itemFirst.Good);
                    SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
                    foreach (GoodsCatalogModelView.GoodsInfoGrid source in list)
                    {
                      GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = source.Clone<GoodsCatalogModelView.GoodsInfoGrid>();
                      if (this.SelectedFilterEqual != GoodsCatalogModelView.FilterEqualEnum.DeleteGoods)
                        this.GoodsList.Remove(source);
                      this.TotalGoodsStock -= source.GoodTotalStock.GetValueOrDefault();
                      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) goodsInfoGrid.Good, (IEntity) itemFirst.Good, ActionType.JoinGood, GlobalDictionaries.EntityTypes.Good, user), false);
                    }
                    Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(itemFirst.Good.Uid);
                    itemFirst.Good = byUid;
                    GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid1 = this.UpdateGoodGrid(byUid, salePriceType);
                    goodsInfoGrid1.LastBuyPrice = new Decimal?(this.BuyPriceCounter.GetLastBuyPrice(itemFirst.Good));
                    this.GoodsList[this.GoodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == itemFirst.Good.Uid))] = goodsInfoGrid1;
                    this.CachedDbGoods[this.CachedDbGoods.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == itemFirst.Good.Uid))] = goodsInfoGrid1;
                    if (this.SelectedFilterEqual == GoodsCatalogModelView.FilterEqualEnum.DeleteGoods)
                    {
                      this.SelectedFilterEqual = GoodsCatalogModelView.FilterEqualEnum.NoDeletedGood;
                      this.OnPropertyChanged("SelectedFilterEqual");
                    }
                    ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
                    {
                      Title = Translate.UserGroupViewModel_Объединение_товаров,
                      Text = string.Format(Translate.GoodsCatalogModelView_Объединение_товаров_выполнено__Остатки_всех_выделенных_позиций_добавлены_в_товар__0___ID__1__, (object) itemFirst.Good.Name, itemFirst.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) "")
                    });
                    progressBar.Close();
                    this.CountSum(false);
                  }
                }
              }
            }
          }
        }));
      }
    }

    public ICommand DeleteGoods { get; set; }

    public ICommand SearchGoods { get; set; }

    public ICommand AddFromExcel { get; set; }

    public class GoodsInfoGrid : ViewModelWithForm
    {
      private Visibility _visibilityImage = Visibility.Collapsed;
      private BitmapSource _image;
      private Gbs.Core.Entities.Goods.Good _good;
      private Decimal? _goodTotalIncomeSum;
      private Decimal? _goodTotalStock;
      private Decimal? _maxPrice;
      private Decimal? _lastBuyPrice;

      public Visibility VisibilityImage
      {
        get => this._visibilityImage;
        set
        {
          this._visibilityImage = value;
          this.OnPropertyChanged(nameof (VisibilityImage));
        }
      }

      public ICommand ShowImageCommand
      {
        get
        {
          return (ICommand) new RelayCommand((Action<object>) (obj =>
          {
            if (!Directory.Exists(Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, this._good.Uid.ToString())))
              MessageBoxHelper.Warning(Translate.ДляВыбранногоТовараНетИзображений);
            else
              new ShowImageGoodViewModel().ShowImage(this._good.Uid);
          }));
        }
      }

      [JsonIgnore]
      public BitmapSource Image
      {
        get => this._image;
        set
        {
          this._image = value;
          this.OnPropertyChanged(nameof (Image));
        }
      }

      public Gbs.Core.Entities.Goods.Good Good
      {
        get => this._good;
        set
        {
          this._good = value;
          this.OnPropertyChanged(nameof (Good));
        }
      }

      public string Barcodes
      {
        get
        {
          return string.Join(" ", this.Good.Barcodes.Select<string, string>((Func<string, string>) (x => x.Trim('\r'))));
        }
      }

      public Decimal? GoodTotalStock
      {
        get => this._goodTotalStock;
        set
        {
          this._goodTotalStock = value;
          this.OnPropertyChanged(nameof (GoodTotalStock));
        }
      }

      public Decimal? MaxPrice
      {
        get => this._maxPrice;
        set
        {
          this._maxPrice = value;
          this.OnPropertyChanged(nameof (MaxPrice));
        }
      }

      public Decimal? LastBuyPrice
      {
        get => this._lastBuyPrice;
        set
        {
          this._lastBuyPrice = value;
          this.OnPropertyChanged(nameof (LastBuyPrice));
        }
      }

      public Decimal? GoodTotalIncomeSum
      {
        get => this._goodTotalIncomeSum;
        set
        {
          this._goodTotalIncomeSum = value;
          this.OnPropertyChanged(nameof (GoodTotalIncomeSum));
        }
      }
    }

    public enum FilterEqualEnum
    {
      NoDeletedGood,
      DeleteGoods,
      EqualBarcode,
      EqualName,
    }

    public enum FilterSearchTypeEnum
    {
      Full,
      Usual,
      Incorrect,
    }

    public class FilterProperty
    {
      public bool IsChecked { get; set; }

      public string Name { get; set; }

      public string Text { get; set; }
    }
  }
}
