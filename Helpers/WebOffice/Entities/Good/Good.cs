// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.Good
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class Good : GoodSimple, IEntity
  {
    public bool IsDeleted { get; set; }

    public string Barcode { get; set; }

    public string[] Barcodes { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public GoodGroupSimple Group { get; set; }

    public List<Property> Properties { get; set; }

    public List<GoodStockSimple> Stocks { get; set; }

    public Good(Gbs.Core.Entities.Goods.Good good)
    {
      this.Uid = good.Uid;
      this.Barcode = good.Barcode;
      this.Barcodes = good.Barcodes.ToArray<string>();
      this.Name = good.Name;
      this.Group = new GoodGroupSimple(good.Group);
      this.Description = good.Description;
      this.IsDeleted = good.IsDeleted;
      this.Properties = good.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => !x.IsDeleted)).Select<EntityProperties.PropertyValue, Property>((Func<EntityProperties.PropertyValue, Property>) (x => new Property(x))).ToList<Property>();
      if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
      {
        this.Stocks = new List<GoodStockSimple>();
      }
      else
      {
        List<GoodsStocks.GoodStock> list = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)).ToList<GoodsStocks.GoodStock>();
        if (!list.Any<GoodsStocks.GoodStock>())
          list.Add(good.StocksAndPrices.Last<GoodsStocks.GoodStock>());
        this.Stocks = list.GroupBy(x => new
        {
          Uid = x.Storage.Uid,
          ModificationUid = x.ModificationUid,
          Price = x.Price
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType42<Guid, Guid, Decimal>, GoodsStocks.GoodStock>, GoodStockSimple>(x => new GoodStockSimple()
        {
          Price = x.Key.Price,
          Quantity = x.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (y => y.Stock)),
          Storage = new StorageSimple(x.First<GoodsStocks.GoodStock>().Storage)
        }).ToList<GoodStockSimple>();
      }
    }
  }
}
