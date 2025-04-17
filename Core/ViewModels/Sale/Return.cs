// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Sale.Return
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents;
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
namespace Gbs.Core.ViewModels.Sale
{
  public class Return : Gbs.Core.ViewModels.Basket.Basket
  {
    private Decimal _sumStockItemSale;
    private Decimal _sumStockReturn;

    public ObservableCollection<BasketItem> ReturnList { get; set; } = new ObservableCollection<BasketItem>();

    public BasketItem SelectedReturn { get; set; }

    public override ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.EditQuantity));
    }

    public ICommand AddInListReturnCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.AddToReturnList));
    }

    public ICommand AddRangeReturnCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.AddRangeToReturnList));
    }

    public ICommand DeleteGoodCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.DeleteFromReturnList));
    }

    public Decimal SumStockItemSale
    {
      get => this._sumStockItemSale;
      set
      {
        this._sumStockItemSale = value;
        this.OnPropertyChanged(nameof (SumStockItemSale));
      }
    }

    public Decimal SumStockItemReturn
    {
      get => this._sumStockReturn;
      set
      {
        this._sumStockReturn = value;
        this.OnPropertyChanged(nameof (SumStockItemReturn));
      }
    }

    public Decimal SumItemReturn
    {
      get => this.ReturnList.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.TotalSum));
    }

    private void EditQuantity(object obj)
    {
      List<BasketItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return;
      (bool result, Decimal? quantity) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) castedList, true));
      if (!result)
        return;
      foreach (BasketItem basketItem1 in castedList)
      {
        BasketItem item = basketItem1;
        BasketItem basketItem2 = this.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid));
        item.Quantity = quantity ?? item.Quantity;
        if (basketItem2.Quantity < item.Quantity)
        {
          MessageBoxHelper.Warning(string.Format(Translate.Return_EditQuantity_, (object) item.Quantity, (object) basketItem2.Quantity, (object) item.Good.Name));
          this.ReturnList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Quantity = basketItem2.Quantity;
        }
        else
          this.ReturnList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Quantity = item.Quantity;
      }
      this.OnPropertyChanged("Items");
      this.ReCalc();
    }

    private void AddToReturnList(object obj)
    {
      if (this.SelectedItem == null)
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.ReturnItemsViewModel_Необходимо_выбрать_товар_для_возврата);
      }
      else if (!this.SelectedItem.Comment.IsNullOrEmpty() && this.SelectedItem.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate && GoodsCertificate.GetCertificateByBarcode(this.SelectedItem.Comment) != null)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.Return_Невозможно_вернуть_товар__так_как_он_является_сертификатом);
      }
      else
      {
        foreach (BasketItem basketItem1 in ((IEnumerable) obj).Cast<BasketItem>().ToList<BasketItem>())
        {
          BasketItem item = basketItem1.Clone();
          if (item.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None)
          {
            List<BasketItem> source = new List<BasketItem>()
            {
              item
            };
            this.EditComment((object) source, true);
            item = source.Single<BasketItem>();
            this.ShowCommentNotifications();
          }
          ActionResult actionResult = item.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None ? new ActionResult(ActionResult.Results.Ok) : this.EditQuantityItem(item);
          if (item.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None)
            item.Quantity = item.Quantity < 1M ? item.Quantity : 1M;
          if (actionResult.Result == ActionResult.Results.Ok)
          {
            if (basketItem1.Quantity < item.Quantity)
              MessageBoxHelper.Warning(string.Format(Translate.Return_EditQuantity_, (object) item.Quantity, (object) basketItem1.Quantity, (object) item.Good.Name));
            else if (this.ReturnList.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)))
            {
              if (basketItem1.Quantity < this.ReturnList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Quantity + item.Quantity)
              {
                MessageBoxHelper.Warning(string.Format(Translate.Return_EditQuantity_, (object) (this.ReturnList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Quantity + item.Quantity), (object) basketItem1.Quantity, (object) item.Good.Name));
              }
              else
              {
                BasketItem basketItem2 = this.ReturnList.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid));
                basketItem2.Quantity = basketItem2.Quantity + item.Quantity;
              }
            }
            else
              this.ReturnList.Add(item);
          }
        }
        this.ReCalc();
      }
    }

    private void AddRangeToReturnList(object _)
    {
      this.ReturnList.Clear();
      foreach (BasketItem basketItem1 in this.Items.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (sales =>
      {
        Gbs.Core.Entities.Goods.Good good = sales.Good;
        GoodsModifications.GoodModification goodModification = sales.GoodModification;
        // ISSUE: explicit non-virtual call
        Guid modificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
        Decimal salePrice = sales.SalePrice;
        Storages.Storage storage = sales.Storage;
        Decimal quantity = sales.Quantity;
        Decimal discount = sales.Discount.Value;
        Guid uid = sales.Uid;
        string comment = sales.Comment;
        Guid goodStockUid = new Guid();
        return new BasketItem(good, modificationUid, salePrice, storage, quantity, discount, uid, comment, goodStockUid);
      })))
      {
        BasketItem basketItem2 = basketItem1;
        if (basketItem2.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None)
        {
          List<BasketItem> source = new List<BasketItem>()
          {
            basketItem2
          };
          this.EditComment((object) source, true);
          basketItem2 = source.Single<BasketItem>();
        }
        if (!basketItem2.Comment.IsNullOrEmpty() && basketItem2.Comment.Length < 30 && GoodsCertificate.GetCertificateByBarcode(basketItem2.Comment) != null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.Return_Невозможно_вернуть_товар__так_как_он_является_сертификатом);
          return;
        }
        this.ReturnList.Add(basketItem2);
      }
      this.ShowCommentNotifications();
      this.ReCalc();
    }

    private void DeleteFromReturnList(object obj)
    {
      if (this.SelectedReturn == null)
      {
        int num = (int) MessageBoxHelper.Show(Translate.ReturnItemsViewModel_Необходимо_выбрать_товар_);
      }
      else
      {
        List<BasketItem> list = ((IEnumerable) obj).Cast<BasketItem>().ToList<BasketItem>();
        if (MessageBoxHelper.Show(string.Format(Translate.Return_Вы_уверены__что_хотите_удалить__0__позиций_из_списка_возврата_, (object) list.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
          foreach (BasketItem basketItem in list)
            this.ReturnList.Remove(basketItem);
        }
        this.ReCalc();
      }
    }

    public void ReCalc()
    {
      this.OnPropertyChanged("SumItemReturn");
      this.SumStockItemSale = this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
      this.SumStockItemReturn = this.ReturnList.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
    }

    private ActionResult EditQuantityItem(BasketItem item)
    {
      if (item == null)
        return new ActionResult(ActionResult.Results.Error);
      (bool result, Decimal? quantity) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<DocumentItemViewModel>) new List<BasketItem>()
      {
        item
      })
      {
        SalePrice = new Decimal?(item.SalePrice)
      });
      if (!result)
        return new ActionResult(ActionResult.Results.Error);
      item.Quantity = quantity ?? item.Quantity;
      this.OnPropertyChanged("Items");
      return new ActionResult(ActionResult.Results.Ok);
    }

    public override ActionResult Save() => throw new NotImplementedException();
  }
}
