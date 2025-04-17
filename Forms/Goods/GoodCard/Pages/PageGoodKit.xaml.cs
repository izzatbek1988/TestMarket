// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodSetPageViewModel
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
  public partial class GoodSetPageViewModel : ViewModelWithForm
  {
    private Settings _settings = new ConfigsRepository<Settings>().Get();

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand EditDiscountCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ObservableCollection<GoodSetPageViewModel.GoodInContent> GoodsInContent { get; set; }

    public GoodSetPageViewModel.GoodInContent SelectedGood { get; set; }

    public Decimal TotalStock
    {
      get
      {
        ObservableCollection<GoodSetPageViewModel.GoodInContent> goodsInContent = this.GoodsInContent;
        return goodsInContent == null ? 0M : goodsInContent.Sum<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, Decimal>) (x => x.Quantity));
      }
    }

    public Decimal TotalSum
    {
      get
      {
        ObservableCollection<GoodSetPageViewModel.GoodInContent> goodsInContent = this.GoodsInContent;
        return (goodsInContent != null ? goodsInContent.Sum<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, Decimal?>) (x =>
        {
          Decimal? price = x.Price;
          Decimal num = 1M - x.Discount / 100M;
          Decimal? nullable = price.HasValue ? new Decimal?(price.GetValueOrDefault() * num) : new Decimal?();
          Decimal quantity = x.Quantity;
          return !nullable.HasValue ? new Decimal?() : new Decimal?(nullable.GetValueOrDefault() * quantity);
        })) : new Decimal?()).GetValueOrDefault();
      }
    }

    private Gbs.Core.Entities.Goods.Good ParentGood { get; }

    public GoodSetPageViewModel()
    {
    }

    public GoodSetPageViewModel(Gbs.Core.Entities.Goods.Good good)
    {
      this.ParentGood = good;
      this.LoadSetContent();
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGood()));
      this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditGood));
      this.EditDiscountCommand = (ICommand) new RelayCommand(new Action<object>(this.EditDiscount));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteGood));
    }

    private void LoadSetContent()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.GoodsInContent = new ObservableCollection<GoodSetPageViewModel.GoodInContent>();
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
          this.GoodsInContent.Add(new GoodSetPageViewModel.GoodInContent()
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
        if (MessageBoxHelper.Show(string.Format(Translate.GoodSetContentPageViewModel_DeleteGood_Вы_уверены__что_хотите_удалить__0__товаров_из_состава_, (object) ((IEnumerable) obj).Cast<GoodSetPageViewModel.GoodInContent>().ToList<GoodSetPageViewModel.GoodInContent>().Count), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
          return;
        foreach (GoodSetPageViewModel.GoodInContent goodInContent in ((IEnumerable) obj).Cast<GoodSetPageViewModel.GoodInContent>().ToList<GoodSetPageViewModel.GoodInContent>())
        {
          goodInContent.IsDeleted = true;
          this.GoodsInContent.Remove(goodInContent);
        }
        this.ContentToGood();
      }
    }

    private void EditGood(object obj)
    {
      List<GoodSetPageViewModel.GoodInContent> list1 = ((IEnumerable) obj).Cast<GoodSetPageViewModel.GoodInContent>().ToList<GoodSetPageViewModel.GoodInContent>();
      if (!list1.Any<GoodSetPageViewModel.GoodInContent>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.GoodSetPageViewModel_Требуется_выбрать_товар_для_редактирования);
      }
      else
      {
        List<BasketItem> list2 = list1.Select<GoodSetPageViewModel.GoodInContent, BasketItem>((Func<GoodSetPageViewModel.GoodInContent, BasketItem>) (x => new BasketItem(x.Good, Guid.Empty, 0M, (Storages.Storage) null, x.Quantity, x.Discount))).ToList<BasketItem>();
        if (list1.Any<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
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
          foreach (GoodSetPageViewModel.GoodInContent goodInContent in list1)
            goodInContent.Quantity = quantity ?? goodInContent.Quantity;
          this.ContentToGood();
        }
      }
    }

    private void EditDiscount(object obj)
    {
      List<GoodSetPageViewModel.GoodInContent> list1 = ((IEnumerable) obj).Cast<GoodSetPageViewModel.GoodInContent>().ToList<GoodSetPageViewModel.GoodInContent>();
      if (!list1.Any<GoodSetPageViewModel.GoodInContent>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.GoodSetPageViewModel_Требуется_выбрать_товар_для_редактирования);
      }
      else
      {
        List<BasketItem> list2 = list1.Select<GoodSetPageViewModel.GoodInContent, BasketItem>((Func<GoodSetPageViewModel.GoodInContent, BasketItem>) (x => new BasketItem(x.Good, Guid.Empty, 0M, (Storages.Storage) null, x.Quantity, x.Discount))).ToList<BasketItem>();
        if (list1.Any<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.GoodSetPageViewModel_Невозможно_изменить_скидку_для_сертификата);
        }
        else
        {
          (bool result, Decimal discount) = new EditGoodDiscountViewModel().ShowCard(new EditGoodDiscountViewModel.DiscountInfo((IReadOnlyCollection<BasketItem>) list2), false);
          if (!result)
            return;
          foreach (GoodSetPageViewModel.GoodInContent goodInContent in list1)
            goodInContent.Discount = discount;
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
      foreach (Gbs.Core.Entities.Goods.Good good1 in goods)
      {
        if (!good1.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range))
        {
          int num1 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodSetPageViewModel_Товар__0__невозможно_добавить_в_состав_набора__так_как_у_него_уже_имеется_модификация_, (object) good1.Name));
        }
        else
        {
          GoodSetPageViewModel.GoodInContent g = new GoodSetPageViewModel.GoodInContent()
          {
            Good = good1,
            Quantity = 1M,
            Price = SaleHelper.GetSalePriceForGood(good1, this._settings.GoodsConfig.SalePriceType)
          };
          if (good1.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range)
          {
            if (good1.Modifications.Count<GoodsModifications.GoodModification>() != 0)
            {
              List<GoodsModifications.GoodModification> modifications;
              if (!new FrmSelectGoodStock().SelectedModification(good1, out modifications))
                break;
              g.ModificationUid = modifications.Single<GoodsModifications.GoodModification>().Uid;
              Gbs.Core.Entities.Goods.Good good2 = g.Good;
              good2.Name = good2.Name + " [" + modifications.Single<GoodsModifications.GoodModification>().Name + "]";
            }
            else
            {
              int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.GoodSetContentPageViewModel_Добавить_товар__0__нельзя__так_как_нет_ни_одной_модификации_для_товара_ассортимента_, (object) good1.Name));
              continue;
            }
          }
          if (this.GoodsInContent.Any<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, bool>) (x => x.Good.Uid == g.Good.Uid)))
            this.GoodsInContent.First<GoodSetPageViewModel.GoodInContent>((Func<GoodSetPageViewModel.GoodInContent, bool>) (x => x.Good.Uid == g.Good.Uid)).Quantity += g.Quantity;
          else
            this.GoodsInContent.Add(g);
          this.ContentToGood();
        }
      }
    }

    private void ContentToGood()
    {
      List<GoodsSets.Set> setList = new List<GoodsSets.Set>();
      foreach (GoodSetPageViewModel.GoodInContent goodInContent in (Collection<GoodSetPageViewModel.GoodInContent>) this.GoodsInContent)
        setList.Add(new GoodsSets.Set()
        {
          Uid = goodInContent.Uid,
          GoodUid = goodInContent.Good.Uid,
          Quantity = goodInContent.Quantity,
          Discount = goodInContent.Discount,
          IsDeleted = goodInContent.IsDeleted,
          ModificationUid = goodInContent.ModificationUid
        });
      this.ParentGood.SetContent = (IEnumerable<GoodsSets.Set>) setList;
      this.OnPropertyChanged("TotalStock");
      this.OnPropertyChanged("TotalSum");
    }

    public class GoodInContent : ViewModelWithForm
    {
      private Decimal _discount;
      private Decimal? _price;
      private Decimal _quantity = 1M;

      public Guid ModificationUid { get; set; } = Guid.Empty;

      public Gbs.Core.Entities.Goods.Good Good { get; set; }

      public bool IsDeleted { get; set; }

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

      public Decimal Discount
      {
        get => this._discount;
        set
        {
          this._discount = value;
          this.OnPropertyChanged(nameof (Discount));
        }
      }

      public Guid Uid { get; set; }
    }
  }
}
