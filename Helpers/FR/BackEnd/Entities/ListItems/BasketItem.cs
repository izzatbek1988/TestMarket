// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Extensions.Numeric;
using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  [Serializable]
  public class BasketItem
  {
    public int DocumentsType { get; set; }

    public bool IsFiscal { get; set; }

    public Guid DocumentUid { get; set; }

    public Guid GoodUid
    {
      get
      {
        Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = this.Good;
        return good == null ? Guid.Empty : good.Uid;
      }
    }

    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal Discount { get; set; }

    public string Comment { get; set; }

    public Decimal DiscountSum
    {
      get
      {
        return Math.Round(this.Quantity * this.Price * this.Discount / 100M, 2, MidpointRounding.AwayFromZero);
      }
    }

    public Decimal Sum
    {
      get
      {
        return Math.Round(this.Quantity * this.Price - this.DiscountSum, 2, MidpointRounding.AwayFromZero);
      }
    }

    public Decimal SumLessNds => this.Sum - this.NdsSum;

    public Decimal NdsValue
    {
      get => (this.Price * (100M - this.Discount) / 100M).GetNdsSum(this.Good.NdsValue);
    }

    public Decimal NdsSum => this.Sum.GetNdsSum(this.Good.NdsValue);

    public string GoodModificationBarcode { get; set; }

    public string GoodModificationName { get; set; }

    public string DisplayedName
    {
      get
      {
        return this.Good.Name + (!this.GoodModificationName.IsNullOrEmpty() ? " [" + this.GoodModificationName + "]" : string.Empty);
      }
    }
  }
}
