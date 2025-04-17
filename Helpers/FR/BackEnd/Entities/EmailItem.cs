// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.EmailItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities
{
  [Serializable]
  public class EmailItem
  {
    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

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

    public DateTime SaleDate { get; set; }
  }
}
