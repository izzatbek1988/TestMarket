// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.SaleOrderItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  [Serializable]
  public class SaleOrderItem
  {
    public Guid ParentUid { get; set; }

    public string SaleNum { get; set; }

    public DateTime SaleDate { get; set; }

    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal Discount { get; set; }

    public string Comment { get; set; }

    public Decimal DiscountSaleSum
    {
      get
      {
        return Math.Round(this.Quantity * this.SalePrice * this.Discount / 100M, 2, MidpointRounding.AwayFromZero);
      }
    }

    public string GoodModificationName { get; set; }

    public string GoodModificationBarcode { get; set; }

    public Decimal Sum
    {
      get
      {
        return Math.Round(this.Quantity * this.SalePrice - this.DiscountSaleSum, 2, MidpointRounding.AwayFromZero);
      }
    }

    public string DisplayedName
    {
      get
      {
        return this.Good.Name + (!this.GoodModificationName.IsNullOrEmpty() ? " [" + this.GoodModificationName + "]" : string.Empty);
      }
    }
  }
}
