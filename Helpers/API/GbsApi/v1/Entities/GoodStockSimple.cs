// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.GoodStockSimple
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class GoodStockSimple
  {
    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

    public StorageSimple Storage { get; set; }

    public GoodStockSimple(GoodsStocks.GoodStock stock)
    {
      this.Quantity = stock.Stock;
      this.Price = stock.Price;
      this.Storage = new StorageSimple(stock.Storage);
    }

    public GoodStockSimple()
    {
    }
  }
}
