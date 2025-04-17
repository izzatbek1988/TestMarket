// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.StockOutDocumentPreparer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Gbs.Helpers.Fifo;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public class StockOutDocumentPreparer
  {
    private Document _doc;
    private readonly DataBase _db;

    public StockOutDocumentPreparer(DataBase db) => this._db = db;

    public List<Document> Prepare(Document doc)
    {
      if (!doc.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.ClientOrderReserve, GlobalDictionaries.DocumentsTypes.WriteOff, GlobalDictionaries.DocumentsTypes.BuyReturn, GlobalDictionaries.DocumentsTypes.Move, GlobalDictionaries.DocumentsTypes.MoveStorage, GlobalDictionaries.DocumentsTypes.ProductionSet, GlobalDictionaries.DocumentsTypes.BeerProductionSet))
        throw new NotSupportedException(Translate.StockOutDocumentPreparerReturn_Prepare_Не_поддерживаемый_тип_документа);
      if (doc.Items.Any<Item>((Func<Item, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit))))
        throw new NotSupportedException(Translate.StockOutDocumentPreparerReturn_Prepare_Не_поддерживаемые_типы_товаров_в_документе);
      this._doc = doc;
      List<Item> list = this._doc.Items.Where<Item>((Func<Item, bool>) (x => !x.IsDeleted)).Clone<IEnumerable<Item>>().ToList<Item>();
      this._doc.Items = list.Where<Item>((Func<Item, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)).ToList<Item>();
      this.PrepareSimpleGoods(list);
      return this.PrepareSetGoods(list);
    }

    private List<Document> PrepareSetGoods(List<Item> itemsCopy)
    {
      List<Document> documentList = new List<Document>();
      List<Item> list = itemsCopy.Where<Item>((Func<Item, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)).ToList<Item>();
      if (!list.Any<Item>())
        return documentList;
      FifoHelper fifoHelper = new FifoHelper(this._db, this._doc.Storage.Uid);
      this._doc.Items.AddRange((IEnumerable<Item>) list);
      Document doc = this._doc;
      return fifoHelper.WriteOffSetAndCreateDocuments(doc);
    }

    private void PrepareSimpleGoods(List<Item> itemsCopy)
    {
      List<Item> list = itemsCopy.Where<Item>((Func<Item, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production))).ToList<Item>();
      FifoHelper fifoHelper = new FifoHelper(this._db, this._doc.Storage.Uid);
      foreach (Item source in list)
      {
        FifoItem fifoItem = new FifoItem(source.Good, source.Quantity, source.BaseSalePrice, source.ModificationUid);
        foreach (FifoHelper.WriteOffResult writeOffResult in fifoHelper.WriteOffSimpleGood(fifoItem))
        {
          Item obj1 = source.Clone<Item>();
          obj1.Uid = Guid.NewGuid();
          obj1.Quantity = writeOffResult.QuantityChange;
          Item obj2 = obj1;
          GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock();
          goodStock.Uid = writeOffResult.StockUid;
          goodStock.GoodUid = obj1.GoodUid;
          goodStock.ModificationUid = obj1.ModificationUid;
          goodStock.Price = obj1.BaseSalePrice;
          goodStock.Stock = 0M;
          goodStock.Storage = this._doc.Storage;
          goodStock.IsDeleted = false;
          obj2.GoodStock = goodStock;
          this._doc.Items.Add(obj1);
        }
      }
    }
  }
}
