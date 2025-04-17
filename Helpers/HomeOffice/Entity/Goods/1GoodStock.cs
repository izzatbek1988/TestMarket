// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodStockHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodStockHome
  {
    public Guid Uid { get; set; }

    public List<EntityProperties.PropertyValue> Properties { get; set; }

    public bool IsDeleted { get; set; }

    public Guid GoodUid { get; set; } = Guid.Empty;

    public Guid StorageUid { get; set; }

    public Decimal Stock { get; set; }

    public Decimal Price { get; set; }

    public Guid ModificationUid { get; set; } = Guid.Empty;

    public GoodStockHome(GoodsStocks.GoodStock stock)
    {
      this.Uid = stock.Uid;
      this.Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) stock.Properties);
      this.IsDeleted = stock.IsDeleted;
      this.GoodUid = stock.GoodUid;
      this.Stock = stock.Stock;
      this.Price = stock.Price;
      this.ModificationUid = stock.ModificationUid;
      this.StorageUid = stock.Storage.Uid;
    }

    public GoodStockHome()
    {
    }
  }
}
