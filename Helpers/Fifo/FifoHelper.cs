// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Fifo.FifoHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.Fifo
{
  public class FifoHelper
  {
    private Gbs.Core.Db.DataBase _db;
    private Guid _storageUid;

    public FifoHelper(Gbs.Core.Db.DataBase db, Guid storageUid)
    {
      this._db = db;
      this._storageUid = storageUid;
    }

    public List<FifoHelper.WriteOffResult> WriteOffSimpleGood(FifoItem item)
    {
      item.Validate();
      return this.WriteOff(item);
    }

    private List<FifoHelper.WriteOffResult> WriteOff(FifoItem item)
    {
      List<FifoHelper.WriteOffResult> result = new List<FifoHelper.WriteOffResult>();
      if (item.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
        return result;
      if (!item.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production))
        throw new NotSupportedException(Translate.FifoHelper_WriteOff_Поддерживается_списание_только_одиночных_товаров_и_товаров_с_ассортиментом);
      List<GOODS_STOCK> list1 = this.GetAllStockQuery(item).ToList<GOODS_STOCK>();
      List<GOODS_STOCK> list2 = this.GetAllStocksWithBuyDocQuery(item).ToList<GOODS_STOCK>();
      List<GOODS_STOCK> source = this.ListExcludeOther(list1, list2);
      if (NonWriteOffQty() > 0M)
        WorkWithPositiveStocksFromList(list2.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M && x.PRICE == item.Price)));
      if (NonWriteOffQty() > 0M)
        WorkWithPositiveStocksFromList(source.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M && x.PRICE == item.Price)));
      if (NonWriteOffQty() > 0M)
        WorkWithPositiveStocksFromList(list2.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M && x.PRICE != item.Price)));
      if (NonWriteOffQty() > 0M)
        WorkWithPositiveStocksFromList(source.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M && x.PRICE != item.Price)));
      if (NonWriteOffQty() > 0M && new ConfigsRepository<Settings>().Get().Sales.UseLastIncomeStockForMinusActions)
      {
        List<GOODS_STOCK> list3 = this.GetAllStocksWithBuyDocQuery(item).ToList<GOODS_STOCK>();
        if (list3.Any<GOODS_STOCK>())
        {
          GOODS_STOCK goodsStock = list3.Last<GOODS_STOCK>();
          result.Add(new FifoHelper.WriteOffResult(goodsStock.UID, NonWriteOffQty()));
        }
      }
      if (NonWriteOffQty() > 0M)
      {
        GOODS_STOCK goodsStock = source.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK <= 0M && x.PRICE == item.Price)).FirstOrDefault<GOODS_STOCK>();
        if (goodsStock != null)
          result.Add(new FifoHelper.WriteOffResult(goodsStock.UID, NonWriteOffQty()));
      }
      if (NonWriteOffQty() > 0M)
      {
        GOODS_STOCK goodsStock = new GOODS_STOCK()
        {
          UID = Guid.NewGuid(),
          GOOD_UID = item.Good.Uid,
          MODIFICATION_UID = item.ModificationUid,
          PRICE = item.Price,
          STOCK = 0M,
          STORAGE_UID = this._storageUid,
          IS_DELETED = false
        };
        this._db.InsertOrReplace<GOODS_STOCK>(goodsStock);
        result.Add(new FifoHelper.WriteOffResult(goodsStock.UID, NonWriteOffQty()));
      }
      foreach (FifoHelper.WriteOffResult writeOffResult in result)
      {
        FifoHelper.WriteOffResult r = writeOffResult;
        this._db.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == r.StockUid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK - r.QuantityChange.ToDbDecimal())).Update<GOODS_STOCK>();
      }
      return result;

      Decimal NonWriteOffQty()
      {
        return item.Quantity - result.Sum<FifoHelper.WriteOffResult>((Func<FifoHelper.WriteOffResult, Decimal>) (x => x.QuantityChange));
      }

      void WorkWithPositiveStocksFromList(IEnumerable<GOODS_STOCK> goodsStocks)
      {
        foreach (GOODS_STOCK goodsStock in goodsStocks.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M)))
        {
          Decimal num = NonWriteOffQty();
          if (num <= 0M)
            break;
          Decimal quantityChange = num >= goodsStock.STOCK ? goodsStock.STOCK : num;
          result.Add(new FifoHelper.WriteOffResult(goodsStock.UID, quantityChange));
        }
      }
    }

    private List<GOODS_STOCK> ListExcludeOther(List<GOODS_STOCK> list1, List<GOODS_STOCK> list2)
    {
      return list1.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (p => list2.All<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.UID != p.UID)))).ToList<GOODS_STOCK>();
    }

    private IQueryable<GOODS_STOCK> GetAllStocksWithBuyDocQuery(FifoItem item)
    {
      ParameterExpression parameterExpression1;
      ParameterExpression parameterExpression2;
      ParameterExpression parameterExpression3;
      ParameterExpression parameterExpression4;
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: type reference
      // ISSUE: method reference
      // ISSUE: method reference
      return this.GetAllStockQuery(item).SelectMany(Expression.Lambda<Func<GOODS_STOCK, IEnumerable<DOCUMENT_ITEMS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
      {
        (Expression) Expression.Call(this._db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<Expression>()),
        (Expression) Expression.Quote((Expression) Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((Expression) Expression.Equal(x.STOCK_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS_STOCK.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
      }), parameterExpression1), (gs, di) => new
      {
        gs = gs,
        di = di
      }).SelectMany(Expression.Lambda<Func<\u003C\u003Ef__AnonymousType1<GOODS_STOCK, DOCUMENT_ITEMS>, IEnumerable<DOCUMENTS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
      {
        (Expression) Expression.Call(this._db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<Expression>()),
        (Expression) Expression.Quote((Expression) Expression.Lambda<Func<DOCUMENTS, bool>>((Expression) Expression.Equal(x.UID, (Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType1<GOODS_STOCK, DOCUMENT_ITEMS>.get_di), __typeref (\u003C\u003Ef__AnonymousType1<GOODS_STOCK, DOCUMENT_ITEMS>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_DOCUMENT_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4))
      }), parameterExpression3), (data, d) => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = data,
        d = d
      }).Where(data => data.d.IS_DELETED == false && data.d.TYPE == 3 && data.\u003C\u003Eh__TransparentIdentifier0.di.IS_DELETED == false).OrderBy(data => data.d.DATE_TIME).Select(data => data.\u003C\u003Eh__TransparentIdentifier0.gs);
    }

    private IQueryable<GOODS_STOCK> GetAllStockQuery(FifoItem item)
    {
      return this._db.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.GOOD_UID == item.Good.Uid && x.MODIFICATION_UID == item.ModificationUid && x.STORAGE_UID == this._storageUid && x.IS_DELETED == false));
    }

    public List<Document> WriteOffSetAndCreateDocuments(Document doc)
    {
      List<Document> documents = new List<Document>();
      List<Gbs.Core.Entities.Documents.Item> list = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)).ToList<Gbs.Core.Entities.Documents.Item>();
      if (!list.Any<Gbs.Core.Entities.Documents.Item>())
        return documents;
      GoodRepository goodRepository = new GoodRepository(this._db);
      foreach (Gbs.Core.Entities.Documents.Item obj1 in list)
      {
        Document document1 = new Document();
        document1.Uid = Guid.NewGuid();
        document1.ParentUid = obj1.Uid;
        document1.Storage = doc.Storage;
        document1.Status = GlobalDictionaries.DocumentsStatuses.Close;
        document1.Type = GlobalDictionaries.DocumentsTypes.SetChildStockChange;
        document1.DateTime = DateTime.Now;
        document1.Section = doc.Section;
        Document document2 = document1;
        foreach (GoodsSets.Set set in obj1.Good.SetContent)
        {
          Gbs.Core.Entities.Goods.Good byUid = goodRepository.GetByUid(set.GoodUid);
          Decimal quantity = set.Quantity * obj1.Quantity;
          foreach (FifoHelper.WriteOffResult writeOffResult in this.WriteOff(new FifoItem(byUid, quantity, 0M, set.ModificationUid)))
          {
            List<Gbs.Core.Entities.Documents.Item> items = document2.Items;
            Gbs.Core.Entities.Documents.Item obj2 = new Gbs.Core.Entities.Documents.Item();
            obj2.DocumentUid = document2.Uid;
            obj2.Good = byUid;
            obj2.Quantity = -writeOffResult.QuantityChange;
            GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock();
            goodStock.Uid = writeOffResult.StockUid;
            goodStock.GoodUid = byUid.Uid;
            goodStock.ModificationUid = set.ModificationUid;
            goodStock.Price = 0M;
            goodStock.Stock = 0M;
            goodStock.Storage = doc.Storage;
            obj2.GoodStock = goodStock;
            items.Add(obj2);
          }
        }
        documents.Add(document2);
      }
      return documents;
    }

    public List<Document> AddSetAndCreateDocuments(Document doc, Document saleDocument)
    {
      List<Document> documents = new List<Document>();
      List<Gbs.Core.Entities.Documents.Item> list = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)).ToList<Gbs.Core.Entities.Documents.Item>();
      if (!list.Any<Gbs.Core.Entities.Documents.Item>())
        return documents;
      GoodRepository goodRepository = new GoodRepository(this._db);
      foreach (Gbs.Core.Entities.Documents.Item obj1 in list)
      {
        Gbs.Core.Entities.Documents.Item item = obj1;
        Gbs.Core.Entities.Documents.Item obj2 = saleDocument.Items.FirstOrDefault<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
        {
          if (i.GoodUid == item.GoodUid)
          {
            Guid? uid1 = i.GoodStock?.Uid;
            Guid? uid2 = item.GoodStock?.Uid;
            if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() == uid2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0 && i.ModificationUid == item.ModificationUid && i.Discount == item.Discount && i.SellPrice == item.SellPrice)
              return i.Comment == item.Comment;
          }
          return false;
        }));
        // ISSUE: explicit non-virtual call
        Guid saleItemUid = obj2 != null ? __nonvirtual (obj2.Uid) : Guid.Empty;
        List<Document> byQuery = new DocumentsRepository(this._db).GetByQuery(this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.PARENT_UID == saleItemUid)));
        Document document1 = new Document();
        document1.Uid = Guid.NewGuid();
        document1.ParentUid = item.Uid;
        document1.Storage = doc.Storage;
        document1.Status = GlobalDictionaries.DocumentsStatuses.Close;
        document1.Type = GlobalDictionaries.DocumentsTypes.SetChildStockChange;
        document1.DateTime = DateTime.Now;
        document1.Section = doc.Section;
        Document document2 = document1;
        IEnumerable<Gbs.Core.Entities.Documents.Item> objs = byQuery.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (d => (IEnumerable<Gbs.Core.Entities.Documents.Item>) d.Items));
        Decimal quantity1 = item.Quantity;
        Decimal quantity2 = obj2.Quantity;
        foreach (Gbs.Core.Entities.Documents.Item obj3 in objs)
        {
          Decimal num = -obj3.Quantity / quantity2 * quantity1;
          Other.ConsoleWrite(string.Format("{0}, qty: {1}", (object) obj3.Good.Name, (object) num));
          List<Gbs.Core.Entities.Documents.Item> items = document2.Items;
          Gbs.Core.Entities.Documents.Item obj4 = new Gbs.Core.Entities.Documents.Item();
          obj4.DocumentUid = document2.Uid;
          obj4.Good = obj3.Good;
          obj4.Quantity = num;
          GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock();
          goodStock.Uid = obj3.GoodStock.Uid;
          goodStock.GoodUid = obj3.GoodUid;
          goodStock.ModificationUid = Guid.Empty;
          goodStock.Price = 0M;
          goodStock.Stock = 0M;
          goodStock.Storage = doc.Storage;
          obj4.GoodStock = goodStock;
          items.Add(obj4);
        }
        documents.Add(document2);
      }
      return documents;
    }

    public class WriteOffResult
    {
      public Guid StockUid { get; set; }

      public Decimal QuantityChange { get; set; }

      public WriteOffResult(Guid stockUid, Decimal quantityChange)
      {
        this.StockUid = stockUid;
        this.QuantityChange = quantityChange;
      }
    }
  }
}
