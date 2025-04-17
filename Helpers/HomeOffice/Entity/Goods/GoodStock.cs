// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodStockHomeList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodStockHomeList
  {
    public List<GoodStockHome> GoodsStockList { get; set; }

    public GoodStockHomeList(List<GoodsStocks.GoodStock> goods)
    {
      this.GoodsStockList = goods.Select<GoodsStocks.GoodStock, GoodStockHome>((Func<GoodsStocks.GoodStock, GoodStockHome>) (x => new GoodStockHome(x))).ToList<GoodStockHome>();
    }

    public GoodStockHomeList()
    {
    }
  }
}
