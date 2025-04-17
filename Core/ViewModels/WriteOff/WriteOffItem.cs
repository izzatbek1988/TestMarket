// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.WriteOff.WriteOffItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Documents;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Core.ViewModels.WriteOff
{
  public class WriteOffItem : DocumentItemViewModel
  {
    public Decimal SalePrice { get; set; }

    public Decimal BuyPrice { get; set; }

    public string Comment { get; set; }

    public Storages.Storage Storage { get; set; }

    public WriteOffItem()
    {
    }

    public WriteOffItem(Item item)
    {
      this.Good = item.Good;
      this.SalePrice = item.SellPrice;
      this.BuyPrice = item.BuyPrice;
      this.Quantity = item.Quantity;
    }

    public WriteOffItem(
      Good good,
      Guid modificationUid,
      Decimal price,
      Storages.Storage storage,
      Decimal q = 1M,
      Decimal discount = 0M,
      Guid guid = default (Guid),
      string comment = "")
    {
      this.Good = good;
      if (modificationUid != Guid.Empty)
      {
        this.GoodModification = good.Modifications.Single<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == modificationUid));
      }
      else
      {
        GoodsModifications.GoodModification goodModification = new GoodsModifications.GoodModification();
        goodModification.Uid = Guid.Empty;
        this.GoodModification = goodModification;
      }
      this.Quantity = q;
      this.SalePrice = price;
      this.Storage = storage;
      this.Uid = guid == new Guid() ? Guid.NewGuid() : guid;
      this.Comment = comment;
    }
  }
}
