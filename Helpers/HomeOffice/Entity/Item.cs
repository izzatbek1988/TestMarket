// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.Item
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class Item : Gbs.Core.Entities.Entity
  {
    public Guid DocumentUid { get; set; }

    public Guid GoodUid { get; set; }

    public Guid ModificationUid { get; set; } = Guid.Empty;

    public Guid GoodStockUid { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Discount { get; set; }

    public string Comment { get; set; }

    public Decimal BuyPrice { get; set; }

    public Decimal SellPrice { get; set; }

    public string FbNumberForEgais { get; set; }

    public Item(Gbs.Core.Entities.Documents.Item item)
    {
      this.Uid = item.Uid;
      this.IsDeleted = item.IsDeleted;
      this.DocumentUid = item.DocumentUid;
      this.GoodUid = item.GoodUid;
      this.ModificationUid = item.ModificationUid;
      GoodsStocks.GoodStock goodStock1 = item.GoodStock;
      // ISSUE: explicit non-virtual call
      this.GoodStockUid = goodStock1 != null ? __nonvirtual (goodStock1.Uid) : Guid.Empty;
      this.Quantity = item.Quantity;
      this.Discount = item.Discount;
      this.Comment = item.Comment;
      this.BuyPrice = item.BuyPrice;
      this.SellPrice = item.SellPrice;
      this.SellPrice = item.SellPrice;
      string str;
      if (item == null)
      {
        str = (string) null;
      }
      else
      {
        GoodsStocks.GoodStock goodStock2 = item.GoodStock;
        str = goodStock2 != null ? goodStock2.Properties.LastOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RegIdForGoodStockUidEgais))?.Value.ToString() : (string) null;
      }
      if (str == null)
        str = "";
      this.FbNumberForEgais = str;
    }

    public Item()
    {
    }
  }
}
