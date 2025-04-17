// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodRemoteCatalogViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Db;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods
{
  public partial class GoodRemoteCatalogViewModel : ViewModelWithForm
  {
    private Guid _selectedPoint;
    private ObservableCollection<GoodsCatalogModelView.FilterProperty> _filterProperties;
    private string _filterGoods;
    private ListBoxItem _selectedFilterPrice;
    private Decimal? _filterPrice;
    private ListBoxItem _selectedFilterCount;
    private Decimal? _filterCount;

    public Dictionary<Guid, string> Points { get; set; }

    public Guid SelectedPoint
    {
      get => this._selectedPoint;
      set
      {
        this._selectedPoint = value;
        this.SearchForFilter();
      }
    }

    public FilterOptions Setting { get; set; }

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

    private void LoadingFilterProperties()
    {
      ObservableCollection<GoodsCatalogModelView.FilterProperty> collection = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
      collection.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.FrmReadExcel_Название,
        IsChecked = this.Setting.RemoteGoodsCatalog.IsCheckedName
      });
      collection.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Barcode",
        Text = Translate.FrmAuthorization_ШтрихКод,
        IsChecked = this.Setting.RemoteGoodsCatalog.IsCheckedBarcode
      });
      collection.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Description",
        Text = Translate.ExcelDataViewModel_Описание,
        IsChecked = this.Setting.RemoteGoodsCatalog.IsCheckedBarcodes
      });
      collection.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Barcodes",
        Text = Translate.ExcelDataViewModel_Доп__штрих_коды,
        IsChecked = this.Setting.RemoteGoodsCatalog.IsCheckedBarcodes
      });
      this.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>((IEnumerable<GoodsCatalogModelView.FilterProperty>) collection);
    }

    public string FilterGoods
    {
      get => this._filterGoods;
      set
      {
        this._filterGoods = value;
        this.OnPropertyChanged(nameof (FilterGoods));
        if (this.TimerSearch.Enabled)
        {
          this.TimerSearch.Stop();
          this.TimerSearch.Start();
        }
        this.TimerSearch.Start();
      }
    }

    public ObservableCollection<ExchangeDataHelper.Good> Goods { get; set; }

    private List<ExchangeDataHelper.Good> CachedDbGoods { get; set; }

    public Timer TimerSearch { get; }

    public void ShowCatalog()
    {
      if (!Other.IsActiveAndShowForm<FrmGoodRemoteCatalog>())
        return;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodRemoteCatalogViewModel_ShowCatalog_Загрузка_данных_из_облака);
      if (!this.Load())
      {
        MessageBoxHelper.Warning(Translate.GoodRemoteCatalogViewModel_Load_Нет_данных_для_отображения_в_сетевом_каталоге_товара__Настройте_выгрузку_данных_на_других_торговых_точках_);
        progressBar.Close();
      }
      else
      {
        progressBar.Close();
        this.TimerSearch.Elapsed += new ElapsedEventHandler(this.TimerSearchOnElapsed);
        this.Setting = new ConfigsRepository<FilterOptions>().Get();
        this.LoadingFilterProperties();
        this.SelectedFilterCount = this.FilterItems[1];
        this.SelectedFilterPrice = this.FilterItems[0];
        this.CountFilterConditionText = this.SelectedFilterCount.Content.ToString();
        this.PriceFilterConditionText = this.SelectedFilterPrice.Content.ToString();
        this.FormToSHow = (WindowWithSize) new FrmGoodRemoteCatalog();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ShowForm(false);
      }
    }

    private void TimerSearchOnElapsed(object sender, ElapsedEventArgs e)
    {
      this.TimerSearch.Stop();
      this.SearchForFilter();
    }

    public void SearchForFilter()
    {
      string filterText = this.FilterGoods.ToLower();
      List<ExchangeDataHelper.Good> source1 = new List<ExchangeDataHelper.Good>();
      if (filterText.IsNullOrEmpty())
      {
        source1.AddRange((IEnumerable<ExchangeDataHelper.Good>) this.CachedDbGoods);
      }
      else
      {
        foreach (GoodsCatalogModelView.FilterProperty filterProperty in this.FilterProperties.Where<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          if (!Guid.TryParse(filterProperty.Name, out Guid _))
          {
            switch (filterProperty.Name)
            {
              case "Name":
                source1.AddRange(this.CachedDbGoods.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.Name.ToLower().Contains(filterText))));
                IEnumerable<ExchangeDataHelper.Good> source2 = ((IEnumerable<string>) filterText.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<ExchangeDataHelper.Good>>(this.CachedDbGoods.AsEnumerable<ExchangeDataHelper.Good>(), (Func<IEnumerable<ExchangeDataHelper.Good>, string, IEnumerable<ExchangeDataHelper.Good>>) ((current, s) => current.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.Name.ToLower().Contains(s)))));
                source1.AddRange((IEnumerable<ExchangeDataHelper.Good>) source2.ToList<ExchangeDataHelper.Good>());
                continue;
              case "Barcode":
                List<ExchangeDataHelper.Good> list1 = this.CachedDbGoods.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.Barcode.ToLower().Contains(filterText))).ToList<ExchangeDataHelper.Good>();
                source1.AddRange((IEnumerable<ExchangeDataHelper.Good>) list1);
                continue;
              case "Barcodes":
                List<ExchangeDataHelper.Good> list2 = this.CachedDbGoods.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.Barcodes.ToLower().Contains(filterText))).ToList<ExchangeDataHelper.Good>();
                source1.AddRange((IEnumerable<ExchangeDataHelper.Good>) list2);
                continue;
              case "Description":
                List<ExchangeDataHelper.Good> list3 = this.CachedDbGoods.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.Description.ToLower().Contains(filterText))).ToList<ExchangeDataHelper.Good>();
                source1.AddRange((IEnumerable<ExchangeDataHelper.Good>) list3);
                continue;
              default:
                continue;
            }
          }
        }
      }
      List<ExchangeDataHelper.Good> goodList1 = source1.GroupBy(x => new
      {
        Uid = x.Uid,
        UidPoint = x.UidPoint
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType6<Guid, Guid>, ExchangeDataHelper.Good>, ExchangeDataHelper.Good>(x => x.First<ExchangeDataHelper.Good>()).ToList<ExchangeDataHelper.Good>();
      if (this.SelectedPoint != Guid.Empty)
        goodList1 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x => x.UidPoint == this.SelectedPoint)).ToList<ExchangeDataHelper.Good>();
      if (this.FilterPrice.HasValue)
      {
        switch (this.PriceFilterConditionText)
        {
          case "=":
            goodList1 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal maxPrice = x.MaxPrice;
              Decimal? filterPrice = this.FilterPrice;
              Decimal valueOrDefault = filterPrice.GetValueOrDefault();
              return maxPrice == valueOrDefault & filterPrice.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
          case ">":
            goodList1 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal maxPrice = x.MaxPrice;
              Decimal? filterPrice = this.FilterPrice;
              Decimal valueOrDefault = filterPrice.GetValueOrDefault();
              return maxPrice > valueOrDefault & filterPrice.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
          case "<":
            goodList1 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal maxPrice = x.MaxPrice;
              Decimal? filterPrice = this.FilterPrice;
              Decimal valueOrDefault = filterPrice.GetValueOrDefault();
              return maxPrice < valueOrDefault & filterPrice.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
        }
      }
      if (this.FilterCount.HasValue)
      {
        List<ExchangeDataHelper.Good> goodList2;
        switch (this.CountFilterConditionText)
        {
          case "=":
            goodList2 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal totalStock = x.TotalStock;
              Decimal? filterCount = this.FilterCount;
              Decimal valueOrDefault = filterCount.GetValueOrDefault();
              return totalStock == valueOrDefault & filterCount.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
          case ">":
            goodList2 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal totalStock = x.TotalStock;
              Decimal? filterCount = this.FilterCount;
              Decimal valueOrDefault = filterCount.GetValueOrDefault();
              return totalStock > valueOrDefault & filterCount.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
          case "<":
            goodList2 = goodList1.Where<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, bool>) (x =>
            {
              Decimal totalStock = x.TotalStock;
              Decimal? filterCount = this.FilterCount;
              Decimal valueOrDefault = filterCount.GetValueOrDefault();
              return totalStock < valueOrDefault & filterCount.HasValue;
            })).ToList<ExchangeDataHelper.Good>();
            break;
          default:
            goodList2 = goodList1;
            break;
        }
        goodList1 = goodList2;
      }
      this.Goods = new ObservableCollection<ExchangeDataHelper.Good>(goodList1);
      this.OnPropertyChanged("Goods");
      this.OnPropertyChanged("TotalCount");
      this.OnPropertyChanged("TotalSum");
    }

    private bool Load()
    {
      Settings settings = new ConfigsRepository<Settings>().Get();
      if (settings.RemoteControl.Cloud.Path.IsNullOrEmpty())
        return false;
      string path = Path.Combine(settings.RemoteControl.Cloud.Path, "goods");
      if (!FileSystemHelper.ExistsOrCreateFolder(path, false))
        return false;
      string str1 = FileSystemHelper.TempFolderPath();
      List<FileInfo> list = ((IEnumerable<string>) Directory.GetFiles(path, "*.zip")).Where<string>((Func<string, bool>) (x => !x.Contains(UidDb.GetUid().EntityUid.ToString()))).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).ToList<FileInfo>().Where<FileInfo>((Func<FileInfo, bool>) (x => Guid.TryParse(x.Name.Replace(".zip", ""), out Guid _))).ToList<FileInfo>();
      this.CachedDbGoods = new List<ExchangeDataHelper.Good>();
      foreach (FileSystemInfo fileSystemInfo in list)
      {
        FileSystemHelper.ExtractAllFile(fileSystemInfo.FullName, str1, "HMWRnLTMKdGUjw46rSFL");
        string str2 = ((IEnumerable<string>) Directory.GetFiles(str1, "*.json")).FirstOrDefault<string>() ?? "";
        List<ExchangeDataHelper.Good> goodList = !str2.IsNullOrEmpty() ? JsonConvert.DeserializeObject<List<ExchangeDataHelper.Good>>(File.ReadAllText(str2)) : throw new Exception(Translate.GoodRemoteCatalogViewModel_Load_Не_удалось_загрузить_товары_из_облака);
        HomeOfficeHelper.InfoArchive infoArchive = JsonConvert.DeserializeObject<HomeOfficeHelper.InfoArchive>(File.ReadAllText(((IEnumerable<string>) Directory.GetFiles(str1, "*.info")).FirstOrDefault<string>()));
        foreach (ExchangeDataHelper.Good good in goodList)
        {
          good.NamePoint = infoArchive.NameDataBase;
          good.UidPoint = Guid.Parse(infoArchive.UidDevice);
          this.CachedDbGoods.Add(good);
        }
        Directory.Delete(str1, true);
      }
      this.Goods = new ObservableCollection<ExchangeDataHelper.Good>(this.CachedDbGoods);
      this.Points = new Dictionary<Guid, string>()
      {
        {
          Guid.Empty,
          Translate.SendWaybillsJournalViewModel__selectedPointSale_Все_точки
        }
      };
      this.CachedDbGoods.GroupBy<ExchangeDataHelper.Good, Guid>((Func<ExchangeDataHelper.Good, Guid>) (x => x.UidPoint)).ToList<IGrouping<Guid, ExchangeDataHelper.Good>>().ForEach((Action<IGrouping<Guid, ExchangeDataHelper.Good>>) (x => this.Points.Add(x.Key, x.First<ExchangeDataHelper.Good>().NamePoint)));
      this.OnPropertyChanged("SelectedPoint");
      return this.Goods.Any<ExchangeDataHelper.Good>();
    }

    public ListBoxItem SelectedFilterPrice
    {
      get => this._selectedFilterPrice;
      set
      {
        this._selectedFilterPrice = value;
        if (!this.FilterPrice.HasValue)
          return;
        this.SearchForFilter();
      }
    }

    public Decimal? FilterPrice
    {
      get => this._filterPrice;
      set
      {
        this._filterPrice = value;
        this.SearchForFilter();
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
        this.SearchForFilter();
      }
    }

    public Decimal? FilterCount
    {
      get => this._filterCount;
      set
      {
        this._filterCount = value;
        this.SearchForFilter();
      }
    }

    public List<ListBoxItem> FilterItems { get; set; }

    public string CountFilterConditionText { get; set; }

    public string PriceFilterConditionText { get; set; }

    public Decimal TotalCount
    {
      get
      {
        return this.Goods.Sum<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, Decimal>) (x => x.TotalStock));
      }
    }

    public Decimal TotalSum
    {
      get
      {
        return this.Goods.Sum<ExchangeDataHelper.Good>((Func<ExchangeDataHelper.Good, Decimal>) (x => x.TotalStock * x.MaxPrice));
      }
    }

    public ICommand ReloadData
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Load()));
    }

    public GoodRemoteCatalogViewModel()
    {
      List<ListBoxItem> listBoxItemList = new List<ListBoxItem>();
      ListBoxItem listBoxItem1 = new ListBoxItem();
      listBoxItem1.Content = (object) "=";
      listBoxItemList.Add(listBoxItem1);
      ListBoxItem listBoxItem2 = new ListBoxItem();
      listBoxItem2.Content = (object) ">";
      listBoxItemList.Add(listBoxItem2);
      ListBoxItem listBoxItem3 = new ListBoxItem();
      listBoxItem3.Content = (object) "<";
      listBoxItemList.Add(listBoxItem3);
      // ISSUE: reference to a compiler-generated field
      this.\u003CFilterItems\u003Ek__BackingField = listBoxItemList;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
