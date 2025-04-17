// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ItemsTotalSumCalculator
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public class ItemsTotalSumCalculator
  {
    private List<ItemsTotalSumCalculator.Item> ItemsList { get; set; } = new List<ItemsTotalSumCalculator.Item>();

    public ItemsTotalSumCalculator(Document document)
    {
      this.ItemsList = document.Items.Select<Gbs.Core.Entities.Documents.Item, ItemsTotalSumCalculator.Item>((Func<Gbs.Core.Entities.Documents.Item, ItemsTotalSumCalculator.Item>) (x => new ItemsTotalSumCalculator.Item(x))).ToList<ItemsTotalSumCalculator.Item>();
    }

    public static Decimal DiscountForPosition(Decimal quantity, Decimal price, Decimal discount)
    {
      return Math.Round(discount / 100M * price * quantity, 2, MidpointRounding.AwayFromZero);
    }

    public static Decimal SumForGoodPosition(ItemsTotalSumCalculator.GoodItem item)
    {
      Decimal num = ItemsTotalSumCalculator.DiscountForPosition(item.Quantity, item.Price, item.Discount);
      return Math.Round(item.Quantity * item.Price - num, 2, MidpointRounding.AwayFromZero);
    }

    public static Decimal SumForItem(Decimal quantity, Decimal price, Decimal discount)
    {
      return ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(quantity, price, discount));
    }

    public Decimal Calculate()
    {
      return this.ItemsList.GroupBy(x => new
      {
        GoodUid = x.GoodUid,
        Price = x.Price,
        Discount = x.Discount,
        ModificationUid = x.ModificationUid,
        Comment = x.Comment
      }).Select(item => new
      {
        item = item,
        qty = item.ToList<ItemsTotalSumCalculator.Item>().Sum<ItemsTotalSumCalculator.Item>((Func<ItemsTotalSumCalculator.Item, Decimal>) (x => x.Quantity))
      }).Select(_param1 => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(_param1.qty, _param1.item.Key.Price, _param1.item.Key.Discount))).Sum();
    }

    public class GoodItem
    {
      public Decimal Quantity { get; set; }

      public Decimal Price { get; set; }

      public Decimal Discount { get; set; }

      public GoodItem(Decimal quantity, Decimal price, Decimal discount)
      {
        this.Quantity = quantity;
        this.Price = price;
        this.Discount = discount;
      }
    }

    private class Item
    {
      public Guid GoodUid { get; set; }

      public Decimal Price { get; set; }

      public Decimal Quantity { get; set; }

      public Decimal Discount { get; set; }

      public Guid ModificationUid { get; set; } = Guid.Empty;

      public string Comment { get; set; } = string.Empty;

      public Item(BasketItem basketItem)
      {
        this.GoodUid = basketItem.Good.Uid;
        this.Price = basketItem.SalePrice;
        this.Quantity = basketItem.Quantity;
        this.Discount = basketItem.Discount.Value;
        this.ModificationUid = basketItem.GoodModification.Uid;
        this.Comment = basketItem.Comment;
      }

      public Item(Gbs.Core.Entities.Documents.Item documentItem)
      {
        this.GoodUid = documentItem.GoodUid;
        this.Price = documentItem.SellPrice;
        this.Quantity = documentItem.Quantity;
        this.Discount = documentItem.Discount;
        this.ModificationUid = documentItem.ModificationUid;
        this.Comment = documentItem.Comment;
      }
    }
  }
}
