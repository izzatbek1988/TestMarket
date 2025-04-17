// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.InventoryItemPreparer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public class InventoryItemPreparer
  {
    private readonly Document _document;
    private readonly Document _secondDocument;
    private readonly Gbs.Core.Db.DataBase _db;
    private Item _item;
    private Decimal _itemFactQuantity;

    public InventoryItemPreparer(Document document, Document secondDocument, Gbs.Core.Db.DataBase db)
    {
      this._document = document;
      this._secondDocument = secondDocument;
      this._db = db;
    }

    public void Prepare(Item item)
    {
      this._item = item;
      this.PrepareItem(item);
    }

    private GoodsStocks.GoodStock CreateNewStock(Item item)
    {
      GoodsStocks.GoodStock newStock = new GoodsStocks.GoodStock();
      newStock.Uid = Guid.NewGuid();
      newStock.GoodUid = item.GoodUid;
      newStock.ModificationUid = item.ModificationUid;
      newStock.Price = item.SellPrice;
      newStock.Stock = 0M;
      newStock.Storage = this._document.Storage;
      newStock.IsDeleted = false;
      return newStock;
    }

    private Item CrateCopyOfItem(Item item, Guid stockUid, Decimal quantity)
    {
      Item obj = item.Clone<Item>();
      obj.Uid = Guid.NewGuid();
      obj.Quantity = quantity;
      if (item.GoodStock == null)
        obj.GoodStock = this.CreateNewStock(item);
      obj.GoodStock.Uid = stockUid;
      return obj;
    }

    private void PrepareItem(Item item)
    {
      this._itemFactQuantity = item.Quantity;
      List<GOODS_STOCK> list1 = this._db.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.GOOD_UID == item.GoodUid && x.MODIFICATION_UID == item.ModificationUid && x.PRICE == item.SellPrice && x.STORAGE_UID == this._document.Storage.Uid && !x.IS_DELETED)).ToList<GOODS_STOCK>();
      STORAGES currentStorage = this._db.GetTable<STORAGES>().Single<STORAGES>((Expression<Func<STORAGES, bool>>) (s => s.UID == this._document.Storage.Uid));
      Decimal num1 = list1.Sum<GOODS_STOCK>((Func<GOODS_STOCK, Decimal>) (x => x.STOCK));
      LogHelper.Trace(string.Format("Подготовка позиции по инвентаризации. Товар {0}; Uid: {1}; FactQty: {2}; DbQty: {3}", (object) item.Good.Name, (object) item.GoodUid, (object) this._itemFactQuantity, (object) num1));
      bool flag = this._itemFactQuantity > num1;
      foreach (GoodsStocks.GoodStock stock in list1.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK < 0M)).Select<GOODS_STOCK, GoodsStocks.GoodStock>((Func<GOODS_STOCK, GoodsStocks.GoodStock>) (x => new GoodsStocks.GoodStock(x, currentStorage))).ToList<GoodsStocks.GoodStock>())
        this.AddSingleToDocuments(stock, 0M);
      List<GoodsStocks.GoodStock> list2 = list1.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M)).Select<GOODS_STOCK, GoodsStocks.GoodStock>((Func<GOODS_STOCK, GoodsStocks.GoodStock>) (x => new GoodsStocks.GoodStock(x, currentStorage))).ToList<GoodsStocks.GoodStock>();
      if (!list2.Any<GoodsStocks.GoodStock>())
      {
        this.AddSingleToDocuments(this.CreateNewStock(item), this._itemFactQuantity);
        LogHelper.Trace("Положительных остатков у товара нет. Создаем новый остаток. Выходим из метода");
      }
      else
      {
        List<GoodsStocks.GoodStock> list3 = list1.Where<GOODS_STOCK>((Func<GOODS_STOCK, bool>) (x => x.STOCK > 0M)).SelectMany((Func<GOODS_STOCK, IEnumerable<DOCUMENT_ITEMS>>) (gs => (IEnumerable<DOCUMENT_ITEMS>) this._db.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.STOCK_UID == gs.UID))), (gs, di) => new
        {
          gs = gs,
          di = di
        }).SelectMany(_param1 => (IEnumerable<DOCUMENTS>) this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == _param1.di.DOCUMENT_UID)), (_param1, d) => new
        {
          \u003C\u003Eh__TransparentIdentifier0 = _param1,
          d = d
        }).Where(_param1 => !_param1.d.IS_DELETED && _param1.d.TYPE == 3 && !_param1.\u003C\u003Eh__TransparentIdentifier0.di.IS_DELETED).OrderByDescending(_param1 => _param1.d.DATE_TIME).Select(_param1 => new GoodsStocks.GoodStock(_param1.\u003C\u003Eh__TransparentIdentifier0.gs, currentStorage)).ToList<GoodsStocks.GoodStock>();
        foreach (GoodsStocks.GoodStock goodStock in list3)
        {
          GoodsStocks.GoodStock stock = goodStock;
          list2.RemoveAll((Predicate<GoodsStocks.GoodStock>) (x => x.Uid == stock.Uid));
        }
        int num2 = new ConfigsRepository<Settings>().Get().Sales.IsOverStockForInventoryAddLastWaybill ? 1 : 0;
        GoodsStocks.GoodStock stock1 = list3.LastOrDefault<GoodsStocks.GoodStock>();
        if ((num2 & (flag ? 1 : 0)) != 0)
        {
          LogHelper.Trace("Включена опция сохранения закупки для излишков");
          if (stock1 != null)
            list3.Remove(stock1);
        }
        this.AddListToDocuments(list3);
        this.AddListToDocuments(list2);
        if ((num2 & (flag ? 1 : 0)) != 0)
        {
          if (this._itemFactQuantity < 0M && num1 >= 0M)
            throw new Exception("Inventory prepare error. Code 001");
          if (this._itemFactQuantity >= 0M && stock1 != null)
            this.AddSingleToDocuments(stock1, this._itemFactQuantity);
        }
        if (!(this._itemFactQuantity != 0M))
          return;
        this.AddSingleToDocuments(this.CreateNewStock(item), this._itemFactQuantity);
      }
    }

    private void AddListToDocuments(List<GoodsStocks.GoodStock> stocksList)
    {
      foreach (GoodsStocks.GoodStock stock in stocksList.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock > 0M)))
      {
        Decimal factQuantity = Math.Min(this._itemFactQuantity, stock.Stock);
        this.AddSingleToDocuments(stock, factQuantity);
      }
    }

    private void AddSingleToDocuments(GoodsStocks.GoodStock stock, Decimal factQuantity)
    {
      Decimal stock1 = stock.Stock;
      this._secondDocument.Items.Add(this.CrateCopyOfItem(this._item, stock.Uid, factQuantity - stock1));
      this._document.Items.Add(this.CrateCopyOfItem(this._item, stock.Uid, factQuantity));
      this._itemFactQuantity -= factQuantity;
    }
  }
}
