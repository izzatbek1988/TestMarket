// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.GoodOrderItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  [Serializable]
  internal class GoodOrderItem
  {
    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal SaleQuantity { get; set; }

    public Decimal Stock { get; set; }

    public Decimal OrderQuantity { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal BuySum
    {
      get => Math.Round(this.OrderQuantity * this.BuyPrice, 2, MidpointRounding.AwayFromZero);
    }
  }
}
