// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Cache.CachesBox
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Goods;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Cache
{
  public static class CachesBox
  {
    public static List<Users.User> AllUsers()
    {
      return CacheHelper.Get<List<Users.User>>(CacheHelper.CacheTypes.AllUsers, (Func<List<Users.User>>) (() =>
      {
        using (DataBase dataBase = Data.GetDataBase())
          return new UsersRepository(dataBase).GetAllItems();
      }));
    }

    public static List<Gbs.Core.Entities.Goods.Good> AllGoods()
    {
      return CacheHelper.Get<List<Gbs.Core.Entities.Goods.Good>>(CacheHelper.CacheTypes.AllGoods, (Func<List<Gbs.Core.Entities.Goods.Good>>) (() => new GoodRepository()
      {
        MultiThreadMode = false
      }.GetAllItems()));
    }

    public static List<Client> AllClients()
    {
      return CacheHelper.Get<List<Client>>(CacheHelper.CacheTypes.AllClients, (Func<List<Client>>) (() => new ClientsRepository()
      {
        MultiThreadMode = false
      }.GetAllItems()));
    }

    public static List<GoodsUnits.GoodUnit> AllGoodsUnits()
    {
      return CacheHelper.Get<List<GoodsUnits.GoodUnit>>(CacheHelper.CacheTypes.AllUnits, (Func<List<GoodsUnits.GoodUnit>>) (() => GoodsUnits.GetUnitsListWithFilter().ToList<GoodsUnits.GoodUnit>()));
    }

    public static List<BuyPriceCounter.StockDB> AllBuyPrices()
    {
      return CacheHelper.Get<List<BuyPriceCounter.StockDB>>(CacheHelper.CacheTypes.BuyPrices, new Func<List<BuyPriceCounter.StockDB>>(BuyPriceCounter.GetBuyPricesFromDb));
    }
  }
}
