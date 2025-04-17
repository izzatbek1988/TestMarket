// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.InventoryItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  public class InventoryItem
  {
    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal BaseQuantity { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal QuantityChange => this.Quantity - this.BaseQuantity;

    public Decimal Quantity { get; set; }

    public DateTime UpdateTime { get; set; }

    public string GoodModificationName { get; set; }

    public string GoodModificationBarcode { get; set; }

    public string DisplayedName
    {
      get
      {
        return this.Good.Name + (!this.GoodModificationName.IsNullOrEmpty() ? " [" + this.GoodModificationName + "]" : string.Empty);
      }
    }
  }
}
