// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.Good
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class Good : GoodSimple, IEntity
  {
    public EntityTypes Type => EntityTypes.Good;

    public bool IsDeleted { get; set; }

    public List<Property> Properties { get; set; }

    public List<GoodStockSimple> Stocks { get; set; }

    public Good(Gbs.Core.Entities.Goods.Good good)
      : base(good)
    {
      this.IsDeleted = good.IsDeleted;
      this.Properties = good.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => !x.IsDeleted)).Select<EntityProperties.PropertyValue, Property>((Func<EntityProperties.PropertyValue, Property>) (x => new Property(x))).ToList<Property>();
      this.Stocks = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)).GroupBy(x => new
      {
        Storage = x.Storage,
        ModificationUid = x.ModificationUid,
        Price = x.Price
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType10<Storages.Storage, Guid, Decimal>, GoodsStocks.GoodStock>, GoodStockSimple>(x => new GoodStockSimple()
      {
        Price = x.Key.Price,
        Quantity = x.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (y => y.Stock)),
        Storage = new StorageSimple(x.Key.Storage)
      }).ToList<GoodStockSimple>();
    }
  }
}
