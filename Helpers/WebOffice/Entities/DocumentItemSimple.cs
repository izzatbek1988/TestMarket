// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.DocumentItemSimple
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class DocumentItemSimple
  {
    public Guid Uid { get; set; }

    public GoodSimple Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal Discount { get; set; }

    public DocumentItemSimple(Gbs.Core.Entities.Documents.Item item)
    {
      this.Uid = item.Uid;
      this.Good = new GoodSimple() { Uid = item.GoodUid };
      this.Quantity = item.Quantity;
      this.BuyPrice = item.BuyPrice;
      this.SalePrice = item.SellPrice;
      this.Discount = SaleHelper.GetSumDiscountItem(item);
    }
  }
}
