// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.InventoryPreparer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public class InventoryPreparer
  {
    private readonly Document _document;
    private readonly Document _secondDocument;
    private readonly DataBase _db;

    public InventoryPreparer(Document document, Document secondDocument, DataBase db)
    {
      this._document = document;
      this._secondDocument = secondDocument;
      this._db = db;
    }

    public (List<DocumentsRepository.StockChange> list, bool result) Prepare()
    {
      this._secondDocument.Status = this._document.Status;
      this._secondDocument.Type = GlobalDictionaries.DocumentsTypes.InventoryAct;
      this._secondDocument.ParentUid = this._document.Uid;
      this._secondDocument.Storage = this._document.Storage;
      this._secondDocument.Section = this._document.Section;
      this._secondDocument.DateTime = this._document.DateTime;
      this._secondDocument.Items.Clear();
      switch (this._document.Status)
      {
        case GlobalDictionaries.DocumentsStatuses.Draft:
          return (new List<DocumentsRepository.StockChange>(), this.PrepareDraftInventory(this._document, this._secondDocument));
        case GlobalDictionaries.DocumentsStatuses.Close:
          return this.PrepareCloseInventory(this._document, this._secondDocument);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private bool PrepareDraftInventory(Document doc, Document secondDoc)
    {
      List<Good> allItems = new GoodRepository(this._db).GetAllItems();
      foreach (Item obj in doc.Items)
      {
        Item item = obj;
        Decimal num = allItems.First<Good>((Func<Good, bool>) (g => g.Uid == item.GoodUid)).StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Price == item.SellPrice && s.ModificationUid == item.ModificationUid && s.Storage.Uid == doc.Storage.Uid && !s.IsDeleted)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (s => s.Stock));
        secondDoc.Items.Add(new Item()
        {
          Good = item.Good,
          SellPrice = item.SellPrice,
          ModificationUid = item.ModificationUid,
          Quantity = item.Quantity - num
        });
      }
      return true;
    }

    private (List<DocumentsRepository.StockChange> list, bool result) PrepareCloseInventory(
      Document doc,
      Document secondDoc)
    {
      secondDoc.Items.Clear();
      Item[] array = new Item[doc.Items.Count];
      doc.Items.CopyTo(array);
      doc.Items.Clear();
      InventoryItemPreparer inventoryItemPreparer = new InventoryItemPreparer(doc, secondDoc, this._db);
      foreach (Item obj in array)
        inventoryItemPreparer.Prepare(obj);
      return (secondDoc.Items.Select<Item, DocumentsRepository.StockChange>((Func<Item, DocumentsRepository.StockChange>) (item => new DocumentsRepository.StockChange()
      {
        Stock = item.GoodStock,
        QuantityChange = item.Quantity
      })).ToList<DocumentsRepository.StockChange>(), true);
    }
  }
}
