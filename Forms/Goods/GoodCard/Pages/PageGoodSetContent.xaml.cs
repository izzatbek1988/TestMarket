// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodSetContentPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
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
namespace Gbs.Forms.Goods
{
  public partial class GoodSetContentPageViewModel : ViewModelWithForm
  {
    private Settings _settings = new ConfigsRepository<Settings>().Get();

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ObservableCollection<GoodSetContentPageViewModel.GoodInContent> GoodsInContent { get; set; }

    public Decimal TotalGood
    {
      get
      {
        ObservableCollection<GoodSetContentPageViewModel.GoodInContent> goodsInContent = this.GoodsInContent;
        return goodsInContent == null ? 0M : goodsInContent.Sum<GoodSetContentPageViewModel.GoodInContent>((Func<GoodSetContentPageViewModel.GoodInContent, Decimal>) (x => x.Quantity));
      }
    }

    public Decimal TotalSum
    {
      get
      {
        ObservableCollection<GoodSetContentPageViewModel.GoodInContent> goodsInContent = this.GoodsInContent;
        return goodsInContent == null ? 0M : goodsInContent.Sum<GoodSetContentPageViewModel.GoodInContent>((Func<GoodSetContentPageViewModel.GoodInContent, Decimal>) (x => x.Price.GetValueOrDefault() * x.Quantity));
      }
    }

    public GoodSetContentPageViewModel.GoodInContent SelectedGood { get; set; }

    private Gbs.Core.Entities.Goods.Good ParentGood { get; }

    public GoodSetContentPageViewModel()
    {
    }

    public GoodSetContentPageViewModel(Gbs.Core.Entities.Goods.Good good)
    {
      this.ParentGood = good;
      this.LoadSetContent();
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGood()));
      this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditGood));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteGood));
    }

    private void LoadSetContent()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.GoodsInContent = new ObservableCollection<GoodSetContentPageViewModel.GoodInContent>();
        foreach (GoodsSets.Set set1 in this.ParentGood.SetContent.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => !x.IsDeleted)))
        {
          GoodsSets.Set set = set1;
          Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(set.GoodUid);
          GoodsModifications.GoodModification goodModification = byUid.Modifications.SingleOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == set.ModificationUid));
          if (goodModification != null)
          {
            Gbs.Core.Entities.Goods.Good good = byUid;
            good.Name = good.Name + " [" + goodModification.Name + "]";
          }
          this.GoodsInContent.Add(new GoodSetContentPageViewModel.GoodInContent()
          {
            Good = byUid,
            Uid = set.Uid,
            Quantity = set.Quantity,
            IsDeleted = set.IsDeleted,
            ModificationUid = set.ModificationUid,
            Price = SaleHelper.GetSalePriceForGood(byUid, this._settings.GoodsConfig.SalePriceType)
          });
        }
      }
    }

    private void DeleteGood(object obj)
    {
      if (this.SelectedGood == null)
      {
        int num = (int) MessageBoxHelper.Show(Translate.НеобходимоВыбратьТоварДляУдаления);
      }
      else
      {
        List<GoodSetContentPageViewModel.GoodInContent> list = ((IEnumerable) obj).Cast<GoodSetContentPageViewModel.GoodInContent>().ToList<GoodSetContentPageViewModel.GoodInContent>();
        if (MessageBoxHelper.Show(string.Format(Translate.GoodSetContentPageViewModel_DeleteGood_Вы_уверены__что_хотите_удалить__0__товаров_из_состава_, (object) list.Count), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
          return;
        foreach (GoodSetContentPageViewModel.GoodInContent goodInContent in list)
        {
          goodInContent.IsDeleted = true;
          this.GoodsInContent.Remove(goodInContent);
        }
        this.ContentToGood();
      }
    }

    private void EditGood(object obj)
    {
      List<GoodSetContentPageViewModel.GoodInContent> list1 = ((IEnumerable) obj).Cast<GoodSetContentPageViewModel.GoodInContent>().ToList<GoodSetContentPageViewModel.GoodInContent>();
      if (!list1.Any<GoodSetContentPageViewModel.GoodInContent>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.GoodSetPageViewModel_Требуется_выбрать_товар_для_редактирования);
      }
      else
      {
        List<BasketItem> list2 = list1.Select<GoodSetContentPageViewModel.GoodInContent, BasketItem>((Func<GoodSetContentPageViewModel.GoodInContent, BasketItem>) (x => new BasketItem(x.Good, Guid.Empty, 0M, (Storages.Storage) null, x.Quantity))).ToList<BasketItem>();
        if (list1.Any<GoodSetContentPageViewModel.GoodInContent>((Func<GoodSetContentPageViewModel.GoodInContent, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.GoodSetPageViewModel_Невозможно_изменить_кол_во_для_сертификата);
        }
        else
        {
          (bool result, Decimal? quantity, Decimal? _) = new EditGoodQuantityViewModel().ShowQuantityWithSalePriceEdit(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) list2, true)
          {
            VisibilityPrice = Visibility.Collapsed
          });
          if (!result)
            return;
          foreach (GoodSetContentPageViewModel.GoodInContent goodInContent in list1)
            goodInContent.Quantity = quantity ?? goodInContent.Quantity;
          this.ContentToGood();
        }
      }
    }

    private void AddGood()
    {
      this.AddInItems((IEnumerable<Gbs.Core.Entities.Goods.Good>) new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.None, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddInItems)).goods);
    }

    private void AddInItems(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool addAllCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        GoodSetContentPageViewModel.GoodInContent goodInContent = new GoodSetContentPageViewModel.GoodInContent()
        {
          Good = good.Clone<Gbs.Core.Entities.Goods.Good>(),
          Quantity = 1M,
          Price = SaleHelper.GetSalePriceForGood(good, this._settings.GoodsConfig.SalePriceType)
        };
        if (good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range)
          MessageBoxHelper.Warning(string.Format(Translate.GoodSetContentPageViewModel_AddInItems_Добавить_товар__0__не_получится__так_как_он_является_ассортиментом__такие_типы_товаров_нельзя_добавлять_в_состав_комплектов_, (object) good.Name));
        else if (!good.Group.RuMarkedProductionType.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol))
        {
          MessageBoxHelper.Warning(string.Format("Добавить товар {0} не получится, так как он является маркируемым, такие товаров нельзя добавлять в состав комплектов.", (object) good.Name));
        }
        else
        {
          this.GoodsInContent.Add(goodInContent);
          this.ContentToGood();
        }
      }
    }

    private void ContentToGood()
    {
      List<GoodsSets.Set> setList = new List<GoodsSets.Set>();
      foreach (GoodSetContentPageViewModel.GoodInContent goodInContent in (Collection<GoodSetContentPageViewModel.GoodInContent>) this.GoodsInContent)
        setList.Add(new GoodsSets.Set()
        {
          Uid = goodInContent.Uid,
          GoodUid = goodInContent.Good.Uid,
          Quantity = goodInContent.Quantity,
          Discount = 0M,
          IsDeleted = goodInContent.IsDeleted,
          ModificationUid = goodInContent.ModificationUid
        });
      this.ParentGood.SetContent = (IEnumerable<GoodsSets.Set>) setList;
      this.OnPropertyChanged("TotalGood");
      this.OnPropertyChanged("TotalSum");
    }

    public class GoodInContent : ViewModelWithForm
    {
      private Decimal _quantity = 1M;
      private Decimal? _price;

      public Gbs.Core.Entities.Goods.Good Good { get; set; }

      public bool IsDeleted { get; set; }

      public Guid ModificationUid { get; set; } = Guid.Empty;

      public Decimal Quantity
      {
        get => this._quantity;
        set
        {
          this._quantity = value;
          this.OnPropertyChanged(nameof (Quantity));
        }
      }

      public Decimal? Price
      {
        get => this._price;
        set
        {
          this._price = value;
          this.OnPropertyChanged(nameof (Price));
        }
      }

      public Guid Uid { get; set; }
    }
  }
}
