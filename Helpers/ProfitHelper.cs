// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ProfitHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public class ProfitHelper
  {
    private Gbs.Core.Entities.Documents.Item _item;
    private Decimal _returnQuantity;
    private readonly BuyPriceCounter _counter;
    private IEnumerable<Document> _documents;

    public ProfitHelper(
      Gbs.Core.Entities.Documents.Item item,
      BuyPriceCounter counter,
      IEnumerable<Document> documents,
      Decimal returnQuantity)
    {
      this._item = item;
      this._counter = counter;
      this._documents = documents;
      this._returnQuantity = returnQuantity;
    }

    public ProfitHelper(BuyPriceCounter counter) => this._counter = counter;

    public Decimal GetProfit()
    {
      Document document = this._documents.SingleOrDefault<Document>((Func<Document, bool>) (x => x.Uid == this._item.DocumentUid));
      if (document == null)
        return 0M;
      this._item.Quantity -= this._returnQuantity;
      Decimal sumItemInDocument = SaleHelper.GetSumItemInDocument(this._item);
      Decimal profit;
      switch (this._item.Good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.None:
        case GlobalDictionaries.GoodsSetStatuses.Range:
          profit = sumItemInDocument - this.GetBuySumForUsualGood();
          break;
        case GlobalDictionaries.GoodsSetStatuses.Set:
          profit = sumItemInDocument - this.GetBuySumForSetGood();
          break;
        case GlobalDictionaries.GoodsSetStatuses.Production:
          profit = sumItemInDocument - this.GetBuySumForProductionGood();
          break;
        default:
          return 0M;
      }
      switch (document.Type)
      {
        case GlobalDictionaries.DocumentsTypes.Sale:
          return profit;
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
          return -profit;
        default:
          return 0M;
      }
    }

    public Decimal GetBuyPriceForItem(Document currentDocument, Gbs.Core.Entities.Documents.Item currentItem)
    {
      this._item = currentItem;
      if (this._documents == null)
      {
        if (this._item.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Production, GlobalDictionaries.GoodsSetStatuses.Set))
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
            {
              Types = new GlobalDictionaries.DocumentsTypes[5]
              {
                GlobalDictionaries.DocumentsTypes.MoveStorageChild,
                GlobalDictionaries.DocumentsTypes.BeerProductionItem,
                GlobalDictionaries.DocumentsTypes.ProductionItem,
                GlobalDictionaries.DocumentsTypes.ProductionSet,
                GlobalDictionaries.DocumentsTypes.BeerProductionSet
              },
              DateStart = DateTime.MinValue,
              DateEnd = DateTime.Now
            });
            itemsWithFilter.ToList<Document>().Add(currentDocument);
            this._documents = (IEnumerable<Document>) itemsWithFilter;
          }
        }
      }
      switch (this._item.Good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.None:
        case GlobalDictionaries.GoodsSetStatuses.Range:
          return this.GetBuySumForUsualGood() / this._item.Quantity;
        case GlobalDictionaries.GoodsSetStatuses.Set:
          return this.GetBuySumForSetGood() / this._item.Quantity;
        case GlobalDictionaries.GoodsSetStatuses.Production:
          return this.GetBuySumForProductionGood() / this._item.Quantity;
        default:
          return 0M;
      }
    }

    private Decimal GetBuySumForUsualGood()
    {
      if (this._item.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
      {
        List<EntityProperties.PropertyValue> properties = this._item.Good.Properties;
        return Convert.ToDecimal((properties != null ? properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid))?.Value : (object) null) ?? (object) 0) * this._item.Quantity;
      }
      BuyPriceCounter counter = this._counter;
      GoodsStocks.GoodStock goodStock = this._item.GoodStock;
      // ISSUE: explicit non-virtual call
      Guid stockUid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
      return counter.GetBuyPrice(stockUid) * this._item.Quantity;
    }

    private Decimal GetBuySumForSetGood()
    {
      return this.GetBuyPriceForSetAndProduction(this._documents.Where<Document>((Func<Document, bool>) (x => x.ParentUid == this._item.Uid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).ToList<Gbs.Core.Entities.Documents.Item>(), this._item);
    }

    private Decimal GetBuySumForProductionGood()
    {
      return this.GetBuySumForProductionGood(this._documents.Where<Document>((Func<Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.ProductionItem, GlobalDictionaries.DocumentsTypes.BeerProductionItem, GlobalDictionaries.DocumentsTypes.MoveStorageChild))).Where<Document>((Func<Document, bool>) (x => x.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (it =>
      {
        Guid? uid1 = it.GoodStock?.Uid;
        Guid? uid2 = this._item.GoodStock?.Uid;
        if (uid1.HasValue != uid2.HasValue)
          return false;
        return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
      })))).GroupBy<Document, Guid>((Func<Document, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Document>, Document>((Func<IGrouping<Guid, Document>, Document>) (x => x.First<Document>())).ToList<Document>(), this._item) * this._item.Quantity;
    }

    private Decimal GetBuySumForProductionGood(List<Document> childDoc, Gbs.Core.Entities.Documents.Item childItem)
    {
      Decimal forProductionGood = 0M;
      Document singleChildDoc = childDoc.SingleOrDefault<Document>();
      if (singleChildDoc == null)
        return forProductionGood;
      Gbs.Core.Entities.Documents.Item obj = singleChildDoc.Items.SingleOrDefault<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
      {
        Guid uid1 = childItem.GoodStock.Uid;
        Guid? uid2 = i.GoodStock?.Uid;
        return uid2.HasValue && uid1 == uid2.GetValueOrDefault();
      }));
      if (obj == null)
        return forProductionGood;
      List<Gbs.Core.Entities.Documents.Item> childSet = this._documents.Where<Document>((Func<Document, bool>) (doc => doc.ParentUid == singleChildDoc.Uid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (doc => (IEnumerable<Gbs.Core.Entities.Documents.Item>) doc.Items)).GroupBy<Gbs.Core.Entities.Documents.Item, Guid>((Func<Gbs.Core.Entities.Documents.Item, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Documents.Item>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Documents.Item>) (g => g.First<Gbs.Core.Entities.Documents.Item>())).ToList<Gbs.Core.Entities.Documents.Item>();
      if (singleChildDoc.Type == GlobalDictionaries.DocumentsTypes.MoveStorageChild)
      {
        obj.Good.SetStatus = GlobalDictionaries.GoodsSetStatuses.None;
        childSet = new List<Gbs.Core.Entities.Documents.Item>() { obj };
      }
      return this.GetBuyPriceForSetAndProduction(childSet, obj);
    }

    private Decimal GetBuyPriceForSetAndProduction(List<Gbs.Core.Entities.Documents.Item> childSet, Gbs.Core.Entities.Documents.Item item)
    {
      Decimal setAndProduction = 0M;
      foreach (Gbs.Core.Entities.Documents.Item child in childSet)
      {
        Gbs.Core.Entities.Documents.Item childItem = child;
        if (childItem.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)
        {
          Decimal forProductionGood = this.GetBuySumForProductionGood(this._documents.Where<Document>((Func<Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.ProductionItem, GlobalDictionaries.DocumentsTypes.BeerProductionItem))).ToList<Document>().Where<Document>((Func<Document, bool>) (x => x.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
          {
            Guid? uid1 = i.GoodStock?.Uid;
            Guid? uid2 = childItem.GoodStock?.Uid;
            if (uid1.HasValue != uid2.HasValue)
              return false;
            return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
          })))).ToList<Document>(), childItem);
          if (item.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)
            setAndProduction += forProductionGood * Math.Abs(item.Quantity);
          else
            setAndProduction += forProductionGood * Math.Abs(childItem.Quantity) / (this._item.Quantity + this._returnQuantity) * this._item.Quantity;
        }
        else if (item.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)
        {
          Decimal num1 = setAndProduction;
          BuyPriceCounter counter = this._counter;
          GoodsStocks.GoodStock goodStock = childItem.GoodStock;
          // ISSUE: explicit non-virtual call
          Guid stockUid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
          Decimal num2 = counter.GetBuyPrice(stockUid) * childItem.Quantity / item.Quantity;
          setAndProduction = num1 + num2;
        }
        else
        {
          Decimal num3 = setAndProduction;
          BuyPriceCounter counter = this._counter;
          GoodsStocks.GoodStock goodStock = childItem.GoodStock;
          // ISSUE: explicit non-virtual call
          Guid stockUid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
          Decimal num4 = counter.GetBuyPrice(stockUid) * Math.Abs(childItem.Quantity) / (this._item.Quantity + this._returnQuantity) * this._item.Quantity;
          setAndProduction = num3 + num4;
        }
      }
      return setAndProduction;
    }
  }
}
