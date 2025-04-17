// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodsSearchModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods
{
  public class GoodsSearchModelView : ViewModelWithForm
  {
    private bool _loadingImagesIsRunning;
    private string _filter;
    private string _selectedFilterPrice;
    private string _filterPrice;
    private string _selectedFilterCount;
    private int? _filterCount;
    private ObservableCollection<GoodsSearchModelView.FilterProperty> _filterProperties;
    private Storages.Storage _storage;
    private ObservableCollection<GoodGroups.Group> _groupsList;
    private static Dictionary<Guid, Gbs.Core.Entities.Goods.Good> _goodsDictionary;

    private void SearchForFilter()
    {
      Performancer performancer = new Performancer("Фильтрация товаров");
      string str;
      if (this.Filter != null)
        str = this.Filter.ToLower().Trim('\r', '\n', ' ');
      else
        str = string.Empty;
      string filterText = str;
      List<GoodsCatalogModelView.GoodsInfoGrid> goodList = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.SelectedSearchType.IsEither<GoodsCatalogModelView.FilterSearchTypeEnum>(GoodsCatalogModelView.FilterSearchTypeEnum.Usual, GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect))
      {
        this.UsualSearch(filterText, goodList);
        if (this.SelectedSearchType == GoodsCatalogModelView.FilterSearchTypeEnum.Incorrect)
          this.IncorrectSearching(filterText, goodList);
      }
      if (this.SelectedSearchType == GoodsCatalogModelView.FilterSearchTypeEnum.Full)
        this.FullSearch(filterText, goodList);
      performancer.AddPoint("Фильтрация товаров: 0");
      goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good != null)).GroupBy<GoodsCatalogModelView.GoodsInfoGrid, Guid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Guid>) (x => x.Good.Uid)).Select<IGrouping<Guid, GoodsCatalogModelView.GoodsInfoGrid>, GoodsCatalogModelView.GoodsInfoGrid>((Func<IGrouping<Guid, GoodsCatalogModelView.GoodsInfoGrid>, GoodsCatalogModelView.GoodsInfoGrid>) (x => x.First<GoodsCatalogModelView.GoodsInfoGrid>())).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (this.SelectedStorage != null)
      {
        Guid? uid = this.SelectedStorage?.Uid;
        Guid empty = Guid.Empty;
        if ((uid.HasValue ? (uid.GetValueOrDefault() != empty ? 1 : 0) : 1) != 0)
          goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == this.SelectedStorage.Uid)))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      }
      if (!this.FilterPrice.IsNullOrEmpty())
      {
        Decimal price = Convert.ToDecimal(this.FilterPrice);
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(GoodStosksPredicate)).Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (p => Functions.MathCompare(p.Price, price, this.SelectedFilterPrice))))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      }
      performancer.AddPoint("Фильтрация товаров: 1");
      if (this.GroupsList.Any<GoodGroups.Group>())
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => this.GroupsList.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Good.Group.Uid)))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => this.ListGoodType.Any<GlobalDictionaries.GoodTypes>((Func<GlobalDictionaries.GoodTypes, bool>) (t => t == x.Good.Group.GoodsType)))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (!this.IsVisibilityNullStock)
        goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x =>
        {
          if (x.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
            return true;
          if (this.DocumentType == GlobalDictionaries.DocumentsTypes.Sale)
          {
            if (x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit))
              return x.Good.SetContent.Any<GoodsSets.Set>();
          }
          return false;
        })).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      this.TotalGoodsStock = 0M;
      foreach (GoodsCatalogModelView.GoodsInfoGrid good in goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x =>
      {
        if (!x.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
          return false;
        return !x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set);
      })))
        this.SetGoodsTotalQty(good);
      if (this.FilterCount.HasValue)
        goodList = this.FilteringByQty(goodList);
      goodList = goodList.OrderByDescending<GoodsCatalogModelView.GoodsInfoGrid, DateTime>((Func<GoodsCatalogModelView.GoodsInfoGrid, DateTime>) (x => x.Good.DateAdd)).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        this.GoodsList.Clear();
        this.GoodsList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) goodList);
      }));
      this.TotalGoodsStock = goodList.Sum<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, Decimal>) (x => x.GoodTotalStock.GetValueOrDefault()));
      this.OnPropertyChanged("TotalGoodsStock");
      performancer.Stop();

      bool GoodStosksPredicate(GoodsStocks.GoodStock s)
      {
        Guid? uid1 = this.SelectedStorage?.Uid;
        Guid empty = Guid.Empty;
        if ((uid1.HasValue ? (uid1.GetValueOrDefault() == empty ? 1 : 0) : 0) != 0)
          return true;
        Guid uid2 = s.Storage.Uid;
        uid1 = this.SelectedStorage?.Uid;
        return uid1.HasValue && uid2 == uid1.GetValueOrDefault();
      }
    }

    private void UsualSearch(string filterText, List<GoodsCatalogModelView.GoodsInfoGrid> goodList)
    {
      if (filterText.IsNullOrEmpty())
      {
        goodList.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) this.CachedDbGoods);
      }
      else
      {
        foreach (GoodsSearchModelView.FilterProperty filterProperty1 in this.FilterProperties.Where<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          GoodsSearchModelView.FilterProperty filterProperty = filterProperty1;
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
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcode.ToLower().Contains(filterText))));
                continue;
              case "Barcodes":
                goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => x.Good.Barcodes.Any<string>((Func<string, bool>) (barcode => barcode.ToLower().Contains(filterText))))));
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

    private void IncorrectSearching(
      string filterText,
      List<GoodsCatalogModelView.GoodsInfoGrid> goodList)
    {
      if (!this.CachedDbGoods.Any<GoodsCatalogModelView.GoodsInfoGrid>())
        return;
      int firstDist = this.CachedDbGoods.Min<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, int>) (x => LevinshtaingHelper.IsSimilarTo(x.Good.Name, filterText)));
      goodList.AddRange(this.CachedDbGoods.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => LevinshtaingHelper.IsSimilarTo(x.Good.Name, filterText) <= firstDist)));
    }

    private void FullSearch(string filterText, List<GoodsCatalogModelView.GoodsInfoGrid> goodList)
    {
      foreach (GoodsSearchModelView.FilterProperty filterProperty1 in this.FilterProperties.Where<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
      {
        GoodsSearchModelView.FilterProperty filterProperty = filterProperty1;
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

    public void LoadingImageForGoodInList()
    {
      if (this._loadingImagesIsRunning)
        return;
      this._loadingImagesIsRunning = true;
      TaskHelper.TaskRun((Action) (() => Parallel.ForEach<GoodsCatalogModelView.GoodsInfoGrid>((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) this.GoodsList, new Action<GoodsCatalogModelView.GoodsInfoGrid>(GoodsCatalogModelView.LoadingImageForOneGood))), false);
    }

    public bool IsEditGoodInFrm { get; set; }

    public Gbs.Core.Entities.Users.User AuthUser { private get; set; }

    public Visibility OptionAfterAddVisibility { get; set; }

    public Visibility OptionAllCountVisibility { get; set; }

    public Visibility AddOrEditGoodVisibility { get; set; }

    public bool OptionAfterAdd
    {
      get => this.Setting.SearchGood.IsNonCloseWindow;
      set => this.Setting.SearchGood.IsNonCloseWindow = value;
    }

    public bool OptionAllCount
    {
      get => this.Setting.SearchGood.IsAddAllCount;
      set => this.Setting.SearchGood.IsAddAllCount = value;
    }

    public string Filter
    {
      get => this._filter;
      set
      {
        if (this._filter == value)
          return;
        this._filter = value;
        this.SearchTimerStart();
        this.OnPropertyChanged(nameof (Filter));
      }
    }

    public System.Timers.Timer TimerSearch { get; }

    private void SearchTimerStart()
    {
      if (this.TimerSearch.Enabled)
        this.TimerSearch.Stop();
      this.TimerSearch.Start();
    }

    public ICommand SelectGoodsCommand { get; set; }

    public ICommand AddGoodsCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.AddGoods()));
    }

    public ICommand EditGoodsCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditGood(obj)));
    }

    public ICommand SearchGoods
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.SearchForFilter()));
    }

    public Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool> AddAction { private get; set; }

    public FilterOptions Setting { get; }

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

    public List<Gbs.Core.Entities.Goods.Good> SelectedGoodList { get; set; }

    public BulkObservableCollection<GoodsCatalogModelView.GoodsInfoGrid> GoodsList { get; set; }

    public Decimal TotalGoodsStock { get; set; }

    private void AddGoods()
    {
      Gbs.Core.Entities.Users.User user = (Gbs.Core.Entities.Users.User) null;
      if (!new UsersRepository().GetAccess(this.AuthUser, Actions.GoodsCreate))
      {
        (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsCreate);
        if (!access.Result)
          return;
        user = access.User;
      }
      Gbs.Core.Entities.Goods.Good goodNew = new Gbs.Core.Entities.Goods.Good();
      if (!this.Filter.IsNullOrEmpty())
      {
        if (double.TryParse(this.Filter, out double _))
          goodNew.Barcode = this.Filter;
        else
          goodNew.Name = this.Filter;
      }
      if (this.GroupsList.Count == 1)
        goodNew.Group = this.GroupsList.Single<GoodGroups.Group>();
      Gbs.Core.Entities.Goods.Good good;
      if (!new FrmGoodCard().ShowGoodCard(Guid.Empty, out good, authUser: user ?? this.AuthUser, goodNew: goodNew))
        return;
      if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
      {
        if (this.DocumentType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.WriteOff, GlobalDictionaries.DocumentsTypes.None))
          return;
      }
      SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
      GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = this.UpdateGoodGrid(good, salePriceType);
      this.GoodsList.Insert(0, goodsInfoGrid);
      GoodsCatalogModelView.LoadingImageForOneGood(goodsInfoGrid);
      this.CachedDbGoods.Add(goodsInfoGrid);
      this.TotalGoodsStock = goodsInfoGrid.GoodTotalStock.GetValueOrDefault();
      this.IsEditGoodInFrm = true;
    }

    private void EditGood(object obj)
    {
      List<GoodsCatalogModelView.GoodsInfoGrid> list = ((IEnumerable) obj).Cast<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      if (list.Any<GoodsCatalogModelView.GoodsInfoGrid>())
      {
        if (list.Count<GoodsCatalogModelView.GoodsInfoGrid>() > 1)
        {
          MessageBoxHelper.Warning(Translate.GoodsCatalogModelView_Возможно_выбрать_только_один_товар_);
        }
        else
        {
          Gbs.Core.Entities.Users.User user = (Gbs.Core.Entities.Users.User) null;
          if (!new UsersRepository().GetAccess(this.AuthUser, Actions.GoodsEdit))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GoodsEdit);
            if (!access.Result)
              return;
            user = access.User;
          }
          Gbs.Core.Entities.Goods.Good good;
          if (!new FrmGoodCard().ShowGoodCard(list.Single<GoodsCatalogModelView.GoodsInfoGrid>().Good.Uid, out good, true, user ?? this.AuthUser, documentTypes: this.DocumentType))
            return;
          SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
          GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = this.UpdateGoodGrid(good, salePriceType);
          GoodsCatalogModelView.LoadingImageForOneGood(goodsInfoGrid);
          this.GoodsList[this.GoodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Uid))] = goodsInfoGrid;
          this.CachedDbGoods[this.CachedDbGoods.ToList<GoodsCatalogModelView.GoodsInfoGrid>().FindIndex((Predicate<GoodsCatalogModelView.GoodsInfoGrid>) (x => x.Good.Uid == good.Uid))] = goodsInfoGrid;
          this.IsEditGoodInFrm = true;
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.GoodsCatalogModelView_Требуется_выбрать_нужный_товар, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public string SelectedFilterPrice
    {
      get => this._selectedFilterPrice;
      set
      {
        if (this._selectedFilterPrice == value)
          return;
        this._selectedFilterPrice = value;
        if (this.FilterPrice.IsNullOrEmpty())
          return;
        this.SearchTimerStart();
      }
    }

    public string FilterPrice
    {
      get => this._filterPrice;
      set
      {
        if (this._filterPrice == value)
          return;
        this._filterPrice = value;
        this.SearchTimerStart();
      }
    }

    private static List<string> _filterItems { get; set; } = new List<string>()
    {
      "=",
      ">",
      "<"
    };

    public List<string> FilterCountItems { get; set; }

    public List<string> FilterPriceItems { get; set; }

    public Dictionary<GoodsCatalogModelView.FilterSearchTypeEnum, string> ListSearchType { get; set; }

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

    public string SelectedFilterCount
    {
      get => this._selectedFilterCount;
      set
      {
        if (this._selectedFilterCount == value)
          return;
        this._selectedFilterCount = value;
        if (!this.FilterCount.HasValue)
          return;
        this.SearchTimerStart();
      }
    }

    public int? FilterCount
    {
      get => this._filterCount;
      set
      {
        int? filterCount = this._filterCount;
        int? nullable = value;
        if (filterCount.GetValueOrDefault() == nullable.GetValueOrDefault() & filterCount.HasValue == nullable.HasValue)
          return;
        this._filterCount = value;
        this.SearchTimerStart();
      }
    }

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
        MessageBoxHelper.Warning(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
      }
    }

    public List<Storages.Storage> ListStorages { get; set; }

    public Storages.Storage SelectedStorage
    {
      get => this._storage;
      set
      {
        if (this._storage == value)
          return;
        this._storage = value;
        this.OnPropertyChanged(nameof (SelectedStorage));
        if (value == null)
          return;
        this.SearchTimerStart();
      }
    }

    public Visibility VisibilityStock { get; set; }

    private GlobalDictionaries.DocumentsTypes DocumentType { get; }

    private bool IsVisibilityNullStock { get; }

    private List<GoodsCatalogModelView.GoodsInfoGrid> CachedDbGoods { get; set; }

    public ObservableCollection<GoodGroups.Group> GroupsList
    {
      get => this._groupsList;
      set
      {
        this._groupsList = value;
        this.OnPropertyChanged(nameof (GroupsList));
        this.SearchForFilter();
      }
    }

    private List<GlobalDictionaries.GoodsSetStatuses> ListStatus { get; }

    private List<GlobalDictionaries.GoodTypes> ListGoodType { get; }

    public GoodsSearchModelView()
    {
      List<Storages.Storage> storageList = new List<Storages.Storage>();
      Storages.Storage storage = new Storages.Storage();
      storage.Name = Translate.WaybillsViewModel_Все_склады;
      storage.Uid = Guid.Empty;
      storageList.Add(storage);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListStorages\u003Ek__BackingField = storageList;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCachedDbGoods\u003Ek__BackingField = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      this._groupsList = new ObservableCollection<GoodGroups.Group>();
      this.ListStatus = new List<GlobalDictionaries.GoodsSetStatuses>()
      {
        GlobalDictionaries.GoodsSetStatuses.Kit,
        GlobalDictionaries.GoodsSetStatuses.None,
        GlobalDictionaries.GoodsSetStatuses.Production,
        GlobalDictionaries.GoodsSetStatuses.Range,
        GlobalDictionaries.GoodsSetStatuses.Set
      };
      this.ListGoodType = new List<GlobalDictionaries.GoodTypes>()
      {
        GlobalDictionaries.GoodTypes.Certificate,
        GlobalDictionaries.GoodTypes.Service,
        GlobalDictionaries.GoodTypes.Single,
        GlobalDictionaries.GoodTypes.Weight
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<FrmSearchGoods>())
        return;
      this.Filter = barcode;
      Task.Run((Action) (() =>
      {
        this.HandleTimerElapsed((object) null, (ElapsedEventArgs) null);
        Thread.Sleep(50);
      }));
    }

    public bool IsNonShow { get; set; }

    public GoodsSearchModelView(
      GlobalDictionaries.DocumentsTypes documentType,
      string query = null,
      bool flag = false,
      Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool> addGood = null)
    {
      List<Storages.Storage> storageList = new List<Storages.Storage>();
      Storages.Storage storage = new Storages.Storage();
      storage.Name = Translate.WaybillsViewModel_Все_склады;
      storage.Uid = Guid.Empty;
      storageList.Add(storage);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListStorages\u003Ek__BackingField = storageList;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCachedDbGoods\u003Ek__BackingField = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      this._groupsList = new ObservableCollection<GoodGroups.Group>();
      this.ListStatus = new List<GlobalDictionaries.GoodsSetStatuses>()
      {
        GlobalDictionaries.GoodsSetStatuses.Kit,
        GlobalDictionaries.GoodsSetStatuses.None,
        GlobalDictionaries.GoodsSetStatuses.Production,
        GlobalDictionaries.GoodsSetStatuses.Range,
        GlobalDictionaries.GoodsSetStatuses.Set
      };
      this.ListGoodType = new List<GlobalDictionaries.GoodTypes>()
      {
        GlobalDictionaries.GoodTypes.Certificate,
        GlobalDictionaries.GoodTypes.Service,
        GlobalDictionaries.GoodTypes.Single,
        GlobalDictionaries.GoodTypes.Weight
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GoodsSearchModelView._goodsDictionary = GoodsSearchModelView.GetGoodsDictionary();
      this.AddAction = addGood;
      this.OptionAfterAddVisibility = addGood == null ? Visibility.Collapsed : Visibility.Visible;
      this.OnPropertyChanged(nameof (OptionAfterAddVisibility));
      this.DocumentType = documentType;
      switch (this.DocumentType)
      {
        case GlobalDictionaries.DocumentsTypes.None:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set)));
          break;
        case GlobalDictionaries.DocumentsTypes.Sale:
          this.OptionAllCountVisibility = Visibility.Collapsed;
          break;
        case GlobalDictionaries.DocumentsTypes.Buy:
          this.OptionAllCountVisibility = Visibility.Collapsed;
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Production)));
          this.ListGoodType.RemoveAll((Predicate<GlobalDictionaries.GoodTypes>) (x => x.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service)));
          break;
        case GlobalDictionaries.DocumentsTypes.Move:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set)));
          break;
        case GlobalDictionaries.DocumentsTypes.WriteOff:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit)));
          this.ListGoodType.RemoveAll((Predicate<GlobalDictionaries.GoodTypes>) (x => x.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service)));
          break;
        case GlobalDictionaries.DocumentsTypes.ClientOrder:
          this.ListGoodType.RemoveAll((Predicate<GlobalDictionaries.GoodTypes>) (x => x.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate)));
          this.OptionAllCountVisibility = Visibility.Collapsed;
          break;
        case GlobalDictionaries.DocumentsTypes.MoveStorage:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set)));
          this.ListGoodType.RemoveAll((Predicate<GlobalDictionaries.GoodTypes>) (x => x.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service)));
          break;
        case GlobalDictionaries.DocumentsTypes.LablePrint:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit)));
          break;
        case GlobalDictionaries.DocumentsTypes.ProductionList:
          this.ListStatus.RemoveAll((Predicate<GlobalDictionaries.GoodsSetStatuses>) (x => !x.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Production)));
          break;
        case GlobalDictionaries.DocumentsTypes.BeerProductionList:
          this.OptionAllCountVisibility = Visibility.Collapsed;
          this.AddOrEditGoodVisibility = Visibility.Collapsed;
          this.ListGoodType.RemoveAll((Predicate<GlobalDictionaries.GoodTypes>) (x => x.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Service, GlobalDictionaries.GoodTypes.Certificate)));
          break;
      }
      this.Setting = new ConfigsRepository<FilterOptions>().Get();
      this.IsVisibilityNullStock = flag;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.ListStorages.AddRange(Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => !x.IS_DELETED))));
      this.LoadingProperty(this.Setting);
      this.TimerSearch.Elapsed += new ElapsedEventHandler(this.HandleTimerElapsed);
      this.SelectGoodsCommand = (ICommand) new RelayCommand(new Action<object>(this.SetSelectedGoodsList));
      this.Filter = query;
      this.TimerSearch.Stop();
      Settings settings = new ConfigsRepository<Settings>().Get();
      this.SelectedFilterCount = this.FilterCountItems[1];
      this.SelectedFilterPrice = this.FilterPriceItems[0];
      this._storage = this.ListStorages.FirstOrDefault<Storages.Storage>((Func<Storages.Storage, bool>) (x => x.Uid == Guid.Empty));
      if (!settings.Sales.AllowSalesToMinus)
      {
        if (this.DocumentType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.WriteOff, GlobalDictionaries.DocumentsTypes.Move))
          this._filterCount = new int?(0);
      }
      this.GetGoodsItem();
      if (this.GoodsList.Count != 1 || this.DocumentType != GlobalDictionaries.DocumentsTypes.Sale || this._filter.IsNullOrEmpty() || !BarcodeHelper.IsEan13Barcode(this._filter) && !BarcodeHelper.IsEan8Barcode(this._filter))
        return;
      this.SelectedGoodList = new List<Gbs.Core.Entities.Goods.Good>(this.GoodsList.Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good)));
      this.IsNonShow = true;
    }

    private GoodsCatalogModelView.GoodsInfoGrid UpdateGoodGrid(Gbs.Core.Entities.Goods.Good good, SalePriceType typePrice)
    {
      GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = new GoodsCatalogModelView.GoodsInfoGrid();
      GoodsCatalogModelView.GoodsInfoGrid good1;
      switch (good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.None:
        case GlobalDictionaries.GoodsSetStatuses.Range:
          good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good,
            MaxPrice = SaleHelper.GetSalePriceForGood(good, typePrice, this.SelectedStorage),
            GoodTotalStock = !good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
            {
              if (x.IsDeleted)
                return false;
              return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
            })) || good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service ? new Decimal?() : new Decimal?(good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
            {
              if (x.IsDeleted)
                return false;
              return this.SelectedStorage.Uid == Guid.Empty || this.SelectedStorage.Uid == x.Storage.Uid;
            })).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)))
          };
          break;
        case GlobalDictionaries.GoodsSetStatuses.Set:
        case GlobalDictionaries.GoodsSetStatuses.Production:
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

    public static void GetPriceForKit(GoodsCatalogModelView.GoodsInfoGrid good)
    {
      try
      {
        List<GoodsSets.Set> list1 = good.Good.SetContent.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => !x.IsDeleted)).ToList<GoodsSets.Set>();
        Dictionary<Guid, Gbs.Core.Entities.Goods.Good> goodsDictionary = GoodsSearchModelView.GetGoodsDictionary();
        List<Gbs.Core.Entities.Goods.Good> list2 = list1.Select<GoodsSets.Set, Gbs.Core.Entities.Goods.Good>((Func<GoodsSets.Set, Gbs.Core.Entities.Goods.Good>) (z => goodsDictionary[z.GoodUid])).ToList<Gbs.Core.Entities.Goods.Good>();
        SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
        switch (good.Good.SetStatus)
        {
          case GlobalDictionaries.GoodsSetStatuses.Set:
            good.MaxPrice = SaleHelper.GetSalePriceForGood(good.Good, salePriceType);
            break;
          case GlobalDictionaries.GoodsSetStatuses.Kit:
            good.MaxPrice = new Decimal?(0M);
            using (List<Gbs.Core.Entities.Goods.Good>.Enumerator enumerator = list2.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Gbs.Core.Entities.Goods.Good x = enumerator.Current;
                Decimal? nullable = SaleHelper.GetSalePriceForGood(x, salePriceType);
                Decimal valueOrDefault = nullable.GetValueOrDefault();
                Decimal quantity = list1.First<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (i => i.GoodUid == x.Uid)).Quantity;
                Decimal num1 = 1M - list1.First<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (i => i.GoodUid == x.Uid)).Discount / 100M;
                GoodsCatalogModelView.GoodsInfoGrid goodsInfoGrid = good;
                nullable = goodsInfoGrid.MaxPrice;
                Decimal num2 = valueOrDefault * quantity * num1;
                goodsInfoGrid.MaxPrice = nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() + num2) : new Decimal?();
              }
              break;
            }
        }
        good.GoodTotalStock = new Decimal?(GoodsSearchModelView.GetStockSet(good.Good, list2));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка расчета цены для составного товара");
      }
    }

    public static Decimal GetStockSet(Gbs.Core.Entities.Goods.Good good, List<Gbs.Core.Entities.Goods.Good> sets)
    {
      List<GoodsSets.Set> s = good.SetContent.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => !x.IsDeleted)).ToList<GoodsSets.Set>();
      if (sets == null)
        sets = GoodsSearchModelView.GetGoodsFromCache().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => s.Any<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (z => z.GoodUid == x.Uid)))).ToList<Gbs.Core.Entities.Goods.Good>();
      Decimal? nullable1 = new Decimal?();
      foreach (GoodsSets.Set set in s.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => x.Quantity > 0M)))
      {
        GoodsSets.Set item = set;
        Gbs.Core.Entities.Goods.Good good1 = sets.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == item.GoodUid));
        if (good1 != null)
        {
          Decimal num1 = good1.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted && x.ModificationUid == item.ModificationUid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
          Decimal num2 = item.Quantity > num1 ? 0M : num1 / item.Quantity;
          Decimal? nullable2;
          Decimal? nullable3;
          if (nullable1.HasValue)
          {
            Decimal num3 = num2;
            nullable2 = nullable1;
            Decimal valueOrDefault = nullable2.GetValueOrDefault();
            nullable3 = num3 < valueOrDefault & nullable2.HasValue ? new Decimal?(num2) : nullable1;
          }
          else
            nullable3 = new Decimal?(num2);
          nullable1 = nullable3;
          nullable2 = nullable1;
          Decimal num4 = 0M;
          if (nullable2.GetValueOrDefault() == num4 & nullable2.HasValue)
            return 0M;
        }
      }
      return nullable1.GetValueOrDefault();
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

    private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
    {
      this.TimerSearch.Stop();
      this.SearchForFilter();
    }

    private void SetSelectedGoodsList(object goodsList)
    {
      Settings.GoodsSearchConfig goodsSearch = new ConfigsRepository<Settings>().Get().GoodsSearch;
      IList source = (IList) goodsList;
      if (source.Count == 0)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GoodsSearchModelView_Требуется_выбрать_хотя_бы_один_товар, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        this.SelectedGoodList = new List<Gbs.Core.Entities.Goods.Good>(source.Cast<GoodsCatalogModelView.GoodsInfoGrid>().ToList<GoodsCatalogModelView.GoodsInfoGrid>().Select<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>((Func<GoodsCatalogModelView.GoodsInfoGrid, Gbs.Core.Entities.Goods.Good>) (x => x.Good)));
        if (this.OptionAfterAdd)
        {
          if (this.OptionAfterAddVisibility == Visibility.Visible)
          {
            try
            {
              this.AddAction((IEnumerable<Gbs.Core.Entities.Goods.Good>) this.SelectedGoodList, this.OptionAllCount && this.OptionAllCountVisibility == Visibility.Visible, true);
            }
            catch
            {
              this.SelectedGoodList = new List<Gbs.Core.Entities.Goods.Good>();
              throw;
            }
            this.SelectedGoodList = new List<Gbs.Core.Entities.Goods.Good>();
            if (!goodsSearch.ClearQueryAfterAdd)
              return;
            this.Filter = string.Empty;
            return;
          }
        }
        this.CloseAction();
      }
    }

    private void SetGoodsTotalQty(GoodsCatalogModelView.GoodsInfoGrid good)
    {
      SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
      good.MaxPrice = SaleHelper.GetSalePriceForGood(good.Good, salePriceType, this.SelectedStorage);
      if (this.SelectedStorage != null)
      {
        Guid? uid = this.SelectedStorage?.Uid;
        Guid empty = Guid.Empty;
        if ((uid.HasValue ? (uid.GetValueOrDefault() != empty ? 1 : 0) : 1) != 0)
        {
          good.GoodTotalStock = new Decimal?(good.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Storage.Uid == this.SelectedStorage.Uid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
          goto label_4;
        }
      }
      good.GoodTotalStock = new Decimal?(good.Good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
label_4:
      if (good.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service)
        return;
      good.GoodTotalStock = new Decimal?();
    }

    private List<GoodsCatalogModelView.GoodsInfoGrid> FilteringByQty(
      List<GoodsCatalogModelView.GoodsInfoGrid> goodList)
    {
      IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> collection1 = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>(new Func<GoodsCatalogModelView.GoodsInfoGrid, bool>(IsService));
      IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> collection2 = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>(new Func<GoodsCatalogModelView.GoodsInfoGrid, bool>(IsSetOrKit));
      Decimal count = Convert.ToDecimal((object) this.FilterCount);
      goodList = goodList.Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => Functions.MathCompare(x.GoodTotalStock.GetValueOrDefault(), count, this.SelectedFilterCount))).Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => !IsSetOrKit(x))).Where<GoodsCatalogModelView.GoodsInfoGrid>((Func<GoodsCatalogModelView.GoodsInfoGrid, bool>) (x => !IsService(x))).ToList<GoodsCatalogModelView.GoodsInfoGrid>();
      goodList.AddRange(collection1);
      goodList.AddRange(collection2);
      return goodList;

      static bool IsService(GoodsCatalogModelView.GoodsInfoGrid x)
      {
        return x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service;
      }

      static bool IsSetOrKit(GoodsCatalogModelView.GoodsInfoGrid x)
      {
        return x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit);
      }
    }

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._loadingImagesIsRunning = false;
          GoodsSearchModelView.ClearCacheGoodsDictionary();
          CacheHelper.Clear(CacheHelper.CacheTypes.AllGoods);
          this.GetGoodsItem();
          this.LoadingImageForGoodInList();
        }));
      }
    }

    private void GetGoodsItem()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsSearchModelView_Загрузка_товаров);
      Performancer performancer = new Performancer("Кэшированный поиск");
      List<Gbs.Core.Entities.Goods.Good> goodsFromCache = GoodsSearchModelView.GetGoodsFromCache();
      List<Gbs.Core.Entities.Goods.Good> cacheGoods = new List<Gbs.Core.Entities.Goods.Good>((IEnumerable<Gbs.Core.Entities.Goods.Good>) goodsFromCache);
      performancer.AddPoint("100");
      List<Gbs.Core.Entities.Goods.Good> list1 = goodsFromCache.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid != GlobalDictionaries.PercentForServiceGoodUid)).ToList<Gbs.Core.Entities.Goods.Good>();
      performancer.AddPoint("110");
      List<Gbs.Core.Entities.Goods.Good> list2 = list1.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => this.ListStatus.Any<GlobalDictionaries.GoodsSetStatuses>((Func<GlobalDictionaries.GoodsSetStatuses, bool>) (s => x.SetStatus == s)) && this.ListGoodType.Any<GlobalDictionaries.GoodTypes>((Func<GlobalDictionaries.GoodTypes, bool>) (t => t == x.Group.GoodsType)))).ToList<Gbs.Core.Entities.Goods.Good>();
      performancer.AddPoint("120");
      if (this.DocumentType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.CafeOrder, GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.ClientOrder))
        list2 = list2.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.Group.IsCompositeGood)).ToList<Gbs.Core.Entities.Goods.Good>();
      if (this.DocumentType == GlobalDictionaries.DocumentsTypes.BeerProductionList)
        list2 = list2.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x =>
        {
          if (!x.Group.IsCompositeGood)
            return false;
          return EgaisHelper.GetAlcoholType(x) == EgaisHelper.AlcoholTypeGorEgais.Beer || x.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol;
        })).ToList<Gbs.Core.Entities.Goods.Good>();
      if (this.DocumentType == GlobalDictionaries.DocumentsTypes.ProductionList)
        list2 = list2.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.SetContent.All<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (g => EgaisHelper.GetAlcoholType(cacheGoods.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (cg => cg.Uid == g.GoodUid))) == EgaisHelper.AlcoholTypeGorEgais.NoAlcohol)))).ToList<Gbs.Core.Entities.Goods.Good>();
      if (this.DocumentType == GlobalDictionaries.DocumentsTypes.None)
        list2 = list2.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => EgaisHelper.GetAlcoholType(x) == EgaisHelper.AlcoholTypeGorEgais.NoAlcohol)).ToList<Gbs.Core.Entities.Goods.Good>();
      performancer.AddPoint("190");
      SalePriceType typePrice = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
      this.CachedDbGoods = new List<GoodsCatalogModelView.GoodsInfoGrid>();
      performancer.AddPoint("195");
      this.CachedDbGoods.AddRange((IEnumerable<GoodsCatalogModelView.GoodsInfoGrid>) list2.AsParallel<Gbs.Core.Entities.Goods.Good>().Select<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>((Func<Gbs.Core.Entities.Goods.Good, GoodsCatalogModelView.GoodsInfoGrid>) (x => this.UpdateGoodGrid(x, typePrice))));
      performancer.AddPoint("210");
      this.SearchForFilter();
      this.LoadingImageForGoodInList();
      performancer.Stop();
      progressBar.Close();
      Thread.Sleep(50);
      FrmSearchGoods formToShow = (FrmSearchGoods) this.FormToSHow;
      if (formToShow == null)
        return;
      Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow(formToShow.ListGoodsSearch);
    }

    public static void ClearCacheGoodsDictionary()
    {
      GoodsSearchModelView._goodsDictionary = (Dictionary<Guid, Gbs.Core.Entities.Goods.Good>) null;
      CacheHelper.Clear(CacheHelper.CacheTypes.AllGoods);
    }

    private static Dictionary<Guid, Gbs.Core.Entities.Goods.Good> GetGoodsDictionary()
    {
      return GoodsSearchModelView._goodsDictionary != null ? GoodsSearchModelView._goodsDictionary : GoodsSearchModelView.GetGoodsFromCache().ToDictionary<Gbs.Core.Entities.Goods.Good, Guid>((Func<Gbs.Core.Entities.Goods.Good, Guid>) (x => x.Uid));
    }

    private static List<Gbs.Core.Entities.Goods.Good> GetGoodsFromCache()
    {
      return CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).ToList<Gbs.Core.Entities.Goods.Good>();
    }

    public class FilterProperty : ViewModel
    {
      private bool _isChecked;

      public bool IsChecked
      {
        get => this._isChecked;
        set
        {
          this._isChecked = value;
          this.OnPropertyChanged(nameof (IsChecked));
        }
      }

      public string Name { get; set; }

      public string Text { get; set; }
    }

    [Serializable]
    public class GoodsInfoGrid : ViewModelWithForm
    {
      private Gbs.Core.Entities.Goods.Good _good;
      private Decimal? _goodTotalStock;
      private Decimal? _maxPrice;

      public Gbs.Core.Entities.Goods.Good Good
      {
        get => this._good;
        set
        {
          this._good = value;
          this.OnPropertyChanged(nameof (Good));
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
    }
  }
}
