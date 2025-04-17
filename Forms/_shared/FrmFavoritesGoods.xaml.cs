// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.SelectGoodsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class SelectGoodsViewModel : ViewModelWithForm
  {
    public bool IsEditing;
    private string _searchQuery;
    private ObservableCollection<SelectGood> _selectGoodsList = new ObservableCollection<SelectGood>();
    private ObservableCollection<GoodsSearchModelView.FilterProperty> _filterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>();

    public string SearchQuery
    {
      get => this._searchQuery;
      set
      {
        this._searchQuery = value;
        this.Search();
      }
    }

    public void Search()
    {
      if (this._searchQuery.IsNullOrEmpty())
      {
        this.SelectGoodsList = new ObservableCollection<SelectGood>((IEnumerable<SelectGood>) this.CacheSelectGoodsList.OrderBy<SelectGood, int>((Func<SelectGood, int>) (x => x.Index)));
      }
      else
      {
        List<SelectGood> source1 = new List<SelectGood>();
        string s1 = this._searchQuery.ToLower();
        foreach (GoodsSearchModelView.FilterProperty filterProperty in this.FilterProperties.Where<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          switch (filterProperty.Name)
          {
            case "Name":
              source1.AddRange(this.CacheSelectGoodsList.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Good.Name.ToLower().Contains(s1))));
              IEnumerable<SelectGood> source2 = ((IEnumerable<string>) s1.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<SelectGood>>(this.CacheSelectGoodsList.AsEnumerable<SelectGood>(), (Func<IEnumerable<SelectGood>, string, IEnumerable<SelectGood>>) ((current, s) => current.Where<SelectGood>((Func<SelectGood, bool>) (x => x.Good.Name.ToLower().Contains(s)))));
              source1.AddRange((IEnumerable<SelectGood>) source2.ToList<SelectGood>());
              continue;
            case "Alias":
              source1.AddRange(this.CacheSelectGoodsList.Where<SelectGood>((Func<SelectGood, bool>) (x => x.DisplayName.ToLower().Contains(s1))));
              continue;
            default:
              continue;
          }
        }
        this.SelectGoodsList = new ObservableCollection<SelectGood>((IEnumerable<SelectGood>) source1.Distinct<SelectGood>().ToList<SelectGood>().OrderBy<SelectGood, int>((Func<SelectGood, int>) (x => x.Index)));
      }
    }

    public Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool> AddBasket { get; set; }

    public ICommand AddBasketCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddBasket(((IEnumerable) obj).Cast<SelectGood>().Select<SelectGood, Gbs.Core.Entities.Goods.Good>((Func<SelectGood, Gbs.Core.Entities.Goods.Good>) (x => x.Good)), false, false);
          this.CloseAction();
        }));
      }
    }

    public ObservableCollection<SelectGood> SelectGoodsList
    {
      get => this._selectGoodsList;
      set
      {
        this._selectGoodsList = value;
        this.OnPropertyChanged(nameof (SelectGoodsList));
      }
    }

    private List<SelectGood> CacheSelectGoodsList { get; set; } = new List<SelectGood>(new SelectGoodsRepository().GetActiveItems().Where<SelectGood>((Func<SelectGood, bool>) (x =>
    {
      Gbs.Core.Entities.Goods.Good good = x.Good;
      return good != null && !__nonvirtual (good.IsDeleted);
    })));

    public SelectGood SelectGood { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public SelectGoodsViewModel()
    {
      try
      {
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Sale, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItem));
          this.AddItem((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
          this.IsEditing = true;
        }));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<SelectGood> list = ((IEnumerable) obj).Cast<SelectGood>().ToList<SelectGood>();
          if (!list.Any<SelectGood>())
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          if (list.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            foreach (SelectGood selectGood in list)
            {
              SelectGood item = selectGood;
              (bool result2, string output2) = MessageBoxHelper.Input(item.DisplayName, Translate.SelectGoodsViewModel_Введите_название, 3);
              if (result2)
              {
                this.SelectGoodsList[this.SelectGoodsList.ToList<SelectGood>().FindIndex((Predicate<SelectGood>) (x => x.Uid == item.Uid))].DisplayName = output2;
                this.SelectGoodsList = new ObservableCollection<SelectGood>((IEnumerable<SelectGood>) this.SelectGoodsList);
                new SelectGoodsRepository().Save(item);
              }
            }
            this.IsEditing = true;
          }
        }));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<SelectGood> list = ((IEnumerable) obj).Cast<SelectGood>().ToList<SelectGood>();
          if (!list.Any<SelectGood>())
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            if (MessageBoxHelper.Question(string.Format(Translate.SelectGoodsViewModel_Вы_уверены__что_хотите_удалить_выделенные_товары_из_избранных___0___, (object) list.Count)) == MessageBoxResult.No)
              return;
            foreach (SelectGood selectGood in list)
            {
              selectGood.IsDeleted = true;
              new SelectGoodsRepository().Save(selectGood);
              this.SelectGoodsList.Remove(selectGood);
              this.CacheSelectGoodsList.Remove(selectGood);
            }
            this.IsEditing = true;
          }
        }));
        this.Setting = new ConfigsRepository<FilterOptions>().Get();
        this.LoadingProperty(this.Setting);
        this.Search();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка избранных товаров");
      }
    }

    private void AddItem(IEnumerable<Gbs.Core.Entities.Goods.Good> listGoods, bool allCount, bool checkMinus = false)
    {
      List<Gbs.Core.Entities.Goods.Good> source = new List<Gbs.Core.Entities.Goods.Good>();
      foreach (Gbs.Core.Entities.Goods.Good listGood in listGoods)
      {
        Gbs.Core.Entities.Goods.Good good = listGood;
        if (this.SelectGoodsList.Any<SelectGood>((Func<SelectGood, bool>) (x => x.Good.Uid == good.Uid && !x.IsDeleted)))
        {
          source.Add(good);
        }
        else
        {
          (bool result, string output) = MessageBoxHelper.Input(good.Name, Translate.SelectGoodsViewModel_Введите_название, 3);
          if (result)
          {
            SelectGood selectGood = new SelectGood()
            {
              Good = good,
              DisplayName = output
            };
            if (new SelectGoodsRepository().Save(selectGood))
            {
              this.SelectGoodsList.Add(selectGood);
              this.CacheSelectGoodsList.Add(selectGood);
            }
          }
        }
      }
      if (!source.Any<Gbs.Core.Entities.Goods.Good>())
        return;
      MessageBoxHelper.Warning(string.Format(Translate.SelectGoodsViewModel_AddItem_, (object) string.Join("\n", source.Select<Gbs.Core.Entities.Goods.Good, string>((Func<Gbs.Core.Entities.Goods.Good, string>) (x => x.Name)))));
    }

    private void LoadingProperty(FilterOptions setting)
    {
      ObservableCollection<GoodsSearchModelView.FilterProperty> collection = new ObservableCollection<GoodsSearchModelView.FilterProperty>();
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.HandbookGoodSettingViewModel_Название_товара,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedName
      });
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Alias",
        Text = Translate.SelectGoodsViewModel_LoadingProperty_Псевдоним_товара,
        IsChecked = setting.SearchGood.GoodProp.IsCheckedBarcode
      });
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
        MessageBoxHelper.Warning(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
        List<GoodsSearchModelView.FilterProperty> list = this._filterProperties.ToList<GoodsSearchModelView.FilterProperty>();
        list.ForEach((Action<GoodsSearchModelView.FilterProperty>) (x => x.IsChecked = true));
        this.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>(list);
      }
    }
  }
}
