// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BuyPriceCounter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Helpers.Cache;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers
{
  public class BuyPriceCounter
  {
    private List<Gbs.Core.Entities.Goods.Good> AlGoods = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).ToList<Gbs.Core.Entities.Goods.Good>();

    private List<BuyPriceCounter.StockDB> Stocks { get; set; }

    public static List<BuyPriceCounter.StockDB> GetBuyPricesFromDb()
    {
      Performancer performancer = new Performancer("Расчет закуп. цен");
      using (DataContext dataContext = Data.GetDataContext())
      {
        IQueryable<DOCUMENTS> inner = dataContext.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (d => !d.IS_DELETED && (d.TYPE == 3 || d.TYPE == 15)));
        ITable<GOODS_STOCK> table = dataContext.GetTable<GOODS_STOCK>();
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        List<BuyPriceCounter.StockDB> list = dataContext.GetTable<DOCUMENT_ITEMS>().Join((IEnumerable<DOCUMENTS>) inner, (Expression<Func<DOCUMENT_ITEMS, Guid>>) (di => di.DOCUMENT_UID), (Expression<Func<DOCUMENTS, Guid>>) (d => d.UID), (di, d) => new
        {
          di = di,
          d = d
        }).Join((IEnumerable<GOODS_STOCK>) table, data => data.di.STOCK_UID, (Expression<Func<GOODS_STOCK, Guid>>) (gs => gs.UID), (data, gs) => new
        {
          \u003C\u003Eh__TransparentIdentifier0 = data,
          gs = gs
        }).Where(data => !data.\u003C\u003Eh__TransparentIdentifier0.di.IS_DELETED).Select(data => new
        {
          DATE_TIME = data.\u003C\u003Eh__TransparentIdentifier0.d.DATE_TIME,
          STOCK_UID = data.\u003C\u003Eh__TransparentIdentifier0.di.STOCK_UID,
          GOOD_UID = data.\u003C\u003Eh__TransparentIdentifier0.di.GOOD_UID,
          BUY_PRICE = data.\u003C\u003Eh__TransparentIdentifier0.di.BUY_PRICE,
          STOCK = data.gs.STOCK,
          IS_DELETED = data.gs.IS_DELETED
        }).Select(Expression.Lambda<Func<\u003C\u003Ef__AnonymousType13<DateTime, Guid, Guid, Decimal, Decimal, bool>, BuyPriceCounter.StockDB>>((Expression) Expression.MemberInit(Expression.New(typeof (BuyPriceCounter.StockDB)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (BuyPriceCounter.StockDB.set_Date)), )))); // Unable to render the statement
        performancer.Stop();
        return list;
      }
    }

    public BuyPriceCounter(bool getFromDb = false)
    {
      if (getFromDb)
        CacheHelper.Clear(CacheHelper.CacheTypes.BuyPrices);
      this.Stocks = CachesBox.AllBuyPrices();
    }

    public Decimal GetLastBuyPrice(Gbs.Core.Entities.Goods.Good good)
    {
      switch (good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.None:
        case GlobalDictionaries.GoodsSetStatuses.Range:
          return this.GetLastBuyPriceForUsualGood(good);
        case GlobalDictionaries.GoodsSetStatuses.Set:
          return this.GetBuyPriceForSet(good.SetContent.ToList<GoodsSets.Set>());
        case GlobalDictionaries.GoodsSetStatuses.Kit:
          return 0M;
        case GlobalDictionaries.GoodsSetStatuses.Production:
          return this.GetBuyPriceForSet(good.SetContent.ToList<GoodsSets.Set>());
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public Decimal GetBuyPrice(Guid stockUid)
    {
      if (stockUid == Guid.Empty)
        return 0M;
      BuyPriceCounter.StockDB stockDb = this.Stocks.Find((Predicate<BuyPriceCounter.StockDB>) (x => x.Uid == stockUid));
      return stockDb == null ? 0M : stockDb.BuyPrice;
    }

    private Decimal GetLastBuyPriceForUsualGood(Gbs.Core.Entities.Goods.Good good)
    {
      List<BuyPriceCounter.StockDB> list = this.Stocks.Where<BuyPriceCounter.StockDB>((Func<BuyPriceCounter.StockDB, bool>) (x => x.GoodUid == good.Uid && x.Stock > 0M)).ToList<BuyPriceCounter.StockDB>();
      BuyPriceCounter.StockDB last = (list.Any<BuyPriceCounter.StockDB>() ? list : this.Stocks).FindLast((Predicate<BuyPriceCounter.StockDB>) (x => x.GoodUid == good.Uid));
      return last == null ? 0M : last.BuyPrice;
    }

    private Decimal GetBuyPriceForSet(List<GoodsSets.Set> childSet)
    {
      Decimal buyPriceForSet = 0M;
      foreach (GoodsSets.Set child in childSet)
      {
        GoodsSets.Set childItem = child;
        Gbs.Core.Entities.Goods.Good good = this.AlGoods.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == childItem.GoodUid));
        if (good != null)
        {
          if (good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)
            buyPriceForSet += this.GetBuyPriceForSet(good.SetContent.ToList<GoodsSets.Set>()) * childItem.Quantity;
          else
            buyPriceForSet += this.GetLastBuyPrice(good) * childItem.Quantity;
        }
      }
      return buyPriceForSet;
    }

    public class StockDB
    {
      public DateTime Date { get; set; }

      public Guid Uid { get; set; }

      public Guid GoodUid { get; set; }

      public Decimal BuyPrice { get; set; }

      public Decimal Stock { get; set; }

      public bool IsDeleted { get; set; }
    }
  }
}
