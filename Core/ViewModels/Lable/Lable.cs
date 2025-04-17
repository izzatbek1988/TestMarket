// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Lable.Lable
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.ViewModels.Lable
{
  public class Lable : DocumentViewModel<BasketItem>
  {
    public Decimal TotalSaleSum
    {
      get
      {
        return this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.SalePrice * x.Quantity));
      }
    }

    public override ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.EditQuantity));
    }

    public override ActionResult Save() => throw new NotImplementedException();

    private void EditQuantity(object obj)
    {
      List<BasketItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return;
      (bool result, Decimal? quantity1) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) castedList.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (x =>
      {
        Good good = x.Good;
        GoodsModifications.GoodModification goodModification = x.GoodModification;
        // ISSUE: explicit non-virtual call
        Guid modificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
        Decimal salePrice = x.SalePrice;
        Storages.Storage storage = x.Storage;
        Decimal quantity2 = x.Quantity;
        Guid uid = x.Uid;
        string comment = x.Comment;
        Guid goodStockUid = new Guid();
        return new BasketItem(good, modificationUid, salePrice, storage, quantity2, guid: uid, comment: comment, goodStockUid: goodStockUid);
      })).ToList<BasketItem>(), false));
      if (!result)
        return;
      foreach (BasketItem basketItem in castedList)
        basketItem.Quantity = quantity1 ?? basketItem.Quantity;
      this.ReCalcTotals();
    }

    public override void ReCalcTotals()
    {
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged("TotalSaleSum");
      this.OnPropertyChanged("TotalQuantity");
    }
  }
}
