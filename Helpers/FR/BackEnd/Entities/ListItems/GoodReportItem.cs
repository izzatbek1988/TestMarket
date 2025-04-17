// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.GoodReportItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  [Serializable]
  public class GoodReportItem
  {
    public User User { get; set; }

    public DateTime DocumentDate { get; set; }

    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

    public Decimal Discount { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal Income { get; set; }

    public Decimal DiscountSum
    {
      get => ItemsTotalSumCalculator.DiscountForPosition(this.Quantity, this.Price, this.Discount);
    }

    public Decimal Sum
    {
      get
      {
        return ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(this.Quantity, this.Price, this.Discount));
      }
    }
  }
}
