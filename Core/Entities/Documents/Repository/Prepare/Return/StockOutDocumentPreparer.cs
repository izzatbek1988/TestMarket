// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.Repository.Prepare.Return.StockOutDocumentPreparerReturn
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Helpers;
using Gbs.Helpers.Fifo;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Documents.Repository.Prepare.Return
{
  public class StockOutDocumentPreparerReturn
  {
    private Document _doc;
    private Document _saleDocument;
    private readonly DataBase _db;
    private Gbs.Core.Entities.Documents.Item[] _soldItemsList;

    public StockOutDocumentPreparerReturn(DataBase db) => this._db = db;

    public List<Document> Prepare(Document doc)
    {
      if (!doc.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.SaleReturn))
        throw new NotSupportedException(Translate.StockOutDocumentPreparerReturn_Prepare_Не_поддерживаемый_тип_документа);
      if (doc.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit))))
        throw new NotSupportedException(Translate.StockOutDocumentPreparerReturn_Prepare_Не_поддерживаемые_типы_товаров_в_документе);
      this._doc = doc;
      List<Gbs.Core.Entities.Documents.Item> list1 = this._doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Clone<IEnumerable<Gbs.Core.Entities.Documents.Item>>().ToList<Gbs.Core.Entities.Documents.Item>();
      this._doc.Items.Clear();
      List<Document> list2 = new DocumentsRepository(this._db).GetByQuery(this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => !x.IS_DELETED && x.PARENT_UID == this._doc.ParentUid && x.TYPE == 2))).ToList<Document>();
      List<Gbs.Core.Entities.Documents.Item> source = new List<Gbs.Core.Entities.Documents.Item>();
      foreach (Gbs.Core.Entities.Documents.Item obj1 in list2.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (d => d.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => !i.IsDeleted)))))
      {
        Gbs.Core.Entities.Documents.Item item = obj1;
        Gbs.Core.Entities.Documents.Item obj2 = source.SingleOrDefault<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x =>
        {
          Guid? uid1 = x.GoodStock?.Uid;
          Guid? uid2 = item.GoodStock?.Uid;
          if (uid1.HasValue != uid2.HasValue)
            return false;
          return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
        }));
        if (obj2 != null)
        {
          obj2.Quantity += item.Quantity;
        }
        else
        {
          Gbs.Core.Entities.Documents.Item obj3 = item.Clone<Gbs.Core.Entities.Documents.Item>();
          obj3.Uid = Guid.NewGuid();
          source.Add(obj3);
        }
      }
      this._saleDocument = new DocumentsRepository(this._db).GetByQuery(this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == this._doc.ParentUid))).Single<Document>();
      this._soldItemsList = new Gbs.Core.Entities.Documents.Item[this._saleDocument.Items.Count];
      this._saleDocument.Items.CopyTo(this._soldItemsList);
      foreach (Gbs.Core.Entities.Documents.Item obj in source)
      {
        Gbs.Core.Entities.Documents.Item item = obj;
        ((IEnumerable<Gbs.Core.Entities.Documents.Item>) this._soldItemsList).First<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x =>
        {
          Guid? uid3 = x.GoodStock?.Uid;
          Guid? uid4 = item.GoodStock?.Uid;
          if (uid3.HasValue != uid4.HasValue)
            return false;
          return !uid3.HasValue || uid3.GetValueOrDefault() == uid4.GetValueOrDefault();
        })).Quantity -= item.Quantity;
      }
      this.PrepareSimpleGoods(list1);
      return this.PrepareSetGoods((IEnumerable<Gbs.Core.Entities.Documents.Item>) list1);
    }

    private List<Document> PrepareSetGoods(IEnumerable<Gbs.Core.Entities.Documents.Item> itemsCopy)
    {
      List<Document> documentList = new List<Document>();
      List<Gbs.Core.Entities.Documents.Item> list = itemsCopy.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)).ToList<Gbs.Core.Entities.Documents.Item>();
      if (!list.Any<Gbs.Core.Entities.Documents.Item>())
        return documentList;
      FifoHelper fifoHelper = new FifoHelper(this._db, this._doc.Storage.Uid);
      this._doc.Items.AddRange((IEnumerable<Gbs.Core.Entities.Documents.Item>) list);
      Document doc = this._doc;
      Document saleDocument = this._saleDocument;
      return fifoHelper.AddSetAndCreateDocuments(doc, saleDocument);
    }

    private void PrepareSimpleGoods(List<Gbs.Core.Entities.Documents.Item> itemsCopy)
    {
      foreach (Gbs.Core.Entities.Documents.Item obj1 in itemsCopy.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production))).ToList<Gbs.Core.Entities.Documents.Item>())
      {
        Gbs.Core.Entities.Documents.Item goodToReturn = obj1;
        Decimal num = goodToReturn.Quantity;
        List<Gbs.Core.Entities.Documents.Item> list = ((IEnumerable<Gbs.Core.Entities.Documents.Item>) this._soldItemsList).Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == goodToReturn.GoodUid && x.SellPrice == goodToReturn.SellPrice && x.ModificationUid == goodToReturn.ModificationUid)).ToList<Gbs.Core.Entities.Documents.Item>();
        while (num > 0M)
        {
          foreach (Gbs.Core.Entities.Documents.Item obj2 in list.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Quantity > 0M)))
          {
            if (!(num == 0M))
            {
              Gbs.Core.Entities.Documents.Item obj3 = new Gbs.Core.Entities.Documents.Item()
              {
                GoodStock = obj2.GoodStock,
                SellPrice = obj2.SellPrice,
                Discount = obj2.Discount,
                Good = obj2.Good,
                ModificationUid = obj2.ModificationUid
              };
              if (obj2.Quantity >= num)
              {
                obj3.Quantity = num;
                num = 0M;
              }
              else
              {
                obj3.Quantity = obj2.Quantity;
                num -= obj2.Quantity;
              }
              this._doc.Items.Add(obj3);
            }
          }
        }
      }
    }
  }
}
