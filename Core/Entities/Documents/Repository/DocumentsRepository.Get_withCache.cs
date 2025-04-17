// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.DocumentsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities.Documents.Repository.Prepare.Return;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Linq;
using Newtonsoft.Json;
using Planfix.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

#nullable enable
namespace Gbs.Core.Entities.Documents
{
  public class DocumentsRepository : IEntityRepository<
  #nullable disable
  Document, DOCUMENTS>
  {
    private readonly Gbs.Core.Db.DataBase _db;

    public List<Document> GetItemsWithFilter(DocumentsRepository.IFilter filter)
    {
      if (filter == null)
        throw new ArgumentNullException();
      if (filter is DocumentsRepository.SingleDocFilter singleDocFilter)
        return new List<Document>()
        {
          this.GetByUid(singleDocFilter.DocumentUid)
        };
      DocumentsRepository.CommonFilter commonFilter = filter as DocumentsRepository.CommonFilter;
      if (commonFilter == null)
        throw new NullReferenceException();
      if (commonFilter.IgnoreTime)
      {
        commonFilter.DateStart = commonFilter.DateStart.Date;
        DocumentsRepository.CommonFilter commonFilter1 = commonFilter;
        DateTime dateTime1 = commonFilter.DateEnd.Date;
        dateTime1 = dateTime1.AddDays(1.0);
        DateTime dateTime2 = dateTime1.AddMilliseconds(-1.0);
        commonFilter1.DateEnd = dateTime2;
      }
      List<Document> source1 = (List<Document>) null;
      if (commonFilter.DateStart > Cache.CacheStart)
        source1 = Cache.GetFromCache();
      if (source1 != null)
      {
        IEnumerable<Document> source2 = source1.Where<Document>((Func<Document, bool>) (x => x.DateTime >= commonFilter.DateStart && x.DateTime <= commonFilter.DateEnd));
        if (!commonFilter.IncludeDeleted)
          source2 = source2.Where<Document>((Func<Document, bool>) (x => !x.IsDeleted));
        if (commonFilter.Types != null)
          source2 = source2.Where<Document>((Func<Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(commonFilter.Types)));
        if (commonFilter.ContractorUid != Guid.Empty)
          source2 = source2.Where<Document>((Func<Document, bool>) (x => x.ContractorUid == commonFilter.ContractorUid));
        if (commonFilter.GoodUid != Guid.Empty)
          source2 = source2.Where<Document>((Func<Document, bool>) (x => x.Items.Any<Item>((Func<Item, bool>) (i => i.GoodUid == commonFilter.GoodUid))));
        return source2.ToList<Document>();
      }
      DataContext dataContext = Gbs.Core.Data.GetDataContext();
      IQueryable<DOCUMENTS> queryable = dataContext.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME >= commonFilter.DateStart && x.DATE_TIME <= commonFilter.DateEnd));
      if (!commonFilter.IncludeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      if (commonFilter.Types != null)
      {
        List<int> intTypes = ((IEnumerable<GlobalDictionaries.DocumentsTypes>) commonFilter.Types).Select<GlobalDictionaries.DocumentsTypes, int>((Func<GlobalDictionaries.DocumentsTypes, int>) (type => (int) type)).ToList<int>();
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => intTypes.Contains(x.TYPE)));
      }
      if (commonFilter.ContractorUid != Guid.Empty)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.CONTRACTOR_UID == commonFilter.ContractorUid));
      if (commonFilter.GoodUid != Guid.Empty)
        queryable = queryable.Join((IEnumerable<DOCUMENT_ITEMS>) dataContext.GetTable<DOCUMENT_ITEMS>(), (Expression<Func<DOCUMENTS, Guid>>) (doc => doc.UID), (Expression<Func<DOCUMENT_ITEMS, Guid>>) (item => item.DOCUMENT_UID), (doc, item) => new
        {
          doc = doc,
          item = item
        }).Where(data => data.item.GOOD_UID == commonFilter.GoodUid).Select(data => data.doc);
      return this.GetByQuery(queryable).ToList<Document>();
    }

    public Document GetByUid(Guid uid)
    {
      Document document = this.GetByQuery(Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == uid))).SingleOrDefault<Document>();
      if (document != null)
        Cache.UpdateInCache(document);
      return document;
    }

    private bool DeleteClosedInventory(Document item)
    {
      LogHelper.OnBegin();
      Document inventoryAct = this.GetItemsWithFilter(item.Uid).Single<Document>();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
        foreach (Item obj in inventoryAct.Items)
        {
          Item documentItem = obj;
          dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == documentItem.GoodStock.Uid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK - documentItem.Quantity)).Update<GOODS_STOCK>();
        }
        dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == item.Uid || x.PARENT_UID == item.Uid)).Set<DOCUMENTS, bool>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENTS>();
        dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == item.Uid || x.DOCUMENT_UID == inventoryAct.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
        connectionTransaction?.Commit();
        LogHelper.OnEnd();
        return true;
      }
    }

    private bool DeleteDraftInventory(Document item)
    {
      LogHelper.OnBegin();
      Document inventoryAct = this.GetItemsWithFilter(item.Uid).SingleOrDefault<Document>();
      if (inventoryAct == null)
      {
        LogHelper.Debug("Не найден черновик для удаления, пропускаем этот шаг.");
        return true;
      }
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == item.Uid || x.PARENT_UID == item.Uid)).Set<DOCUMENTS, bool>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENTS>();
        dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == item.Uid || x.DOCUMENT_UID == inventoryAct.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
      }
      LogHelper.OnEnd();
      return true;
    }

    private bool DeleteClosedSale(Document item)
    {
      LogHelper.OnBegin();
      List<Document> list = this.GetItemsWithFilter(item.Uid).Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn && !x.IsDeleted)).ToList<Document>();
      if (item.Items.Any<Item>((Func<Item, bool>) (g => g.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)))
      {
        int num = (int) MessageBoxHelper.Show(Translate.DocumentsRepository_DeleteClosedSale_Удаление_продаж_с_сертификатами_сейчас_не_поддерживается, string.Empty, icon: MessageBoxImage.Exclamation);
        return false;
      }
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
        foreach (Item obj in item.Items.Where<Item>((Func<Item, bool>) (i => i.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service && !i.IsDeleted)).Where<Item>((Func<Item, bool>) (x => x.GoodStock != null)))
        {
          Item docItem = obj;
          dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == docItem.GoodStock.Uid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK + docItem.Quantity)).Update<GOODS_STOCK>();
        }
        foreach (Document document in list)
        {
          Document returnDoc = document;
          foreach (Item obj in returnDoc.Items.Where<Item>((Func<Item, bool>) (i => i.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service && !i.IsDeleted)))
          {
            Item returnItem = obj;
            dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == returnItem.GoodStock.Uid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK - returnItem.Quantity)).Update<GOODS_STOCK>();
          }
          dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == returnDoc.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
          dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.PARENT_UID == returnDoc.Uid)).Set<PAYMENTS, bool>((Expression<Func<PAYMENTS, bool>>) (x => x.IS_DELETED), true).Update<PAYMENTS>();
        }
        dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == item.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
        dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.PARENT_UID == item.Uid)).Set<PAYMENTS, bool>((Expression<Func<PAYMENTS, bool>>) (x => x.IS_DELETED), true).Update<PAYMENTS>();
        dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == item.Uid || x.PARENT_UID == item.Uid && x.TYPE != 3 && x.TYPE != 15)).Set<DOCUMENTS, bool>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENTS>();
        connectionTransaction?.Commit();
      }
      LogHelper.OnEnd();
      return true;
    }

    private bool DeleteClosedWriteOff(Document item)
    {
      LogHelper.OnBegin();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
        foreach (Item obj in item.Items.Where<Item>((Func<Item, bool>) (i => i.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service && !i.IsDeleted)).ToList<Item>())
        {
          Item docItem = obj;
          Guid uid;
          if (docItem.GoodStock == null)
          {
            uid = docItem.Uid;
            LogHelper.Debug("Для item UID " + uid.ToString() + " не найден остаток");
          }
          else
          {
            dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == docItem.GoodStock.Uid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (x => x.STOCK + docItem.Quantity)).Update<GOODS_STOCK>();
            string[] strArray = new string[6]
            {
              "увеличиваю остаток товара: ",
              docItem.Good.Name,
              "; stockUid: ",
              null,
              null,
              null
            };
            uid = docItem.GoodStock.Uid;
            strArray[3] = uid.ToString();
            strArray[4] = " ; qty: ";
            strArray[5] = docItem.Quantity.ToString();
            Other.ConsoleWrite(string.Concat(strArray));
          }
        }
        dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == item.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
        dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == item.Uid || x.PARENT_UID == item.Uid)).Set<DOCUMENTS, bool>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENTS>();
        connectionTransaction?.Commit();
        LogHelper.OnEnd();
        return true;
      }
    }

    private ActionResult ValidateBuy(Document item)
    {
      ActionResult actionResult = new ActionResult(ActionResult.Results.Ok);
      if (item.Items.Any<Item>((Func<Item, bool>) (x =>
      {
        Gbs.Core.Entities.Goods.Good good = x.Good;
        bool? nullable;
        if (good == null)
        {
          nullable = new bool?();
        }
        else
        {
          GoodGroups.Group group = good.Group;
          if (group == null)
            nullable = new bool?();
          else
            nullable = new bool?(group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service));
        }
        return nullable.GetValueOrDefault();
      })))
        actionResult.AddMessage(ActionResult.Results.Error, Translate.DocumentsRepository_Нельзя_сделать_накладную_на_поступление_для_услуги_или_сертификата);
      return actionResult;
    }

    private bool DeleteClosed(Document item)
    {
      switch (item.Type)
      {
        case GlobalDictionaries.DocumentsTypes.None:
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
        case GlobalDictionaries.DocumentsTypes.Buy:
        case GlobalDictionaries.DocumentsTypes.BuyReturn:
        case GlobalDictionaries.DocumentsTypes.MoveReturn:
        case GlobalDictionaries.DocumentsTypes.UserStockEdit:
        case GlobalDictionaries.DocumentsTypes.MoveStorageChild:
          throw new NotImplementedException();
        case GlobalDictionaries.DocumentsTypes.Sale:
        case GlobalDictionaries.DocumentsTypes.MoveStorage:
        case GlobalDictionaries.DocumentsTypes.ProductionSet:
        case GlobalDictionaries.DocumentsTypes.ClientOrderReserve:
        case GlobalDictionaries.DocumentsTypes.BeerProductionSet:
          return this.DeleteClosedSale(item);
        case GlobalDictionaries.DocumentsTypes.Move:
          return this.DeleteClosedSale(item);
        case GlobalDictionaries.DocumentsTypes.WriteOff:
          return this.DeleteClosedWriteOff(item);
        case GlobalDictionaries.DocumentsTypes.Inventory:
          return this.DeleteClosedInventory(item);
        case GlobalDictionaries.DocumentsTypes.InventoryAct:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private bool DeleteDrafts(Document item)
    {
      switch (item.Type)
      {
        case GlobalDictionaries.DocumentsTypes.None:
        case GlobalDictionaries.DocumentsTypes.Sale:
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
        case GlobalDictionaries.DocumentsTypes.Buy:
        case GlobalDictionaries.DocumentsTypes.BuyReturn:
        case GlobalDictionaries.DocumentsTypes.Move:
        case GlobalDictionaries.DocumentsTypes.MoveReturn:
        case GlobalDictionaries.DocumentsTypes.WriteOff:
        case GlobalDictionaries.DocumentsTypes.UserStockEdit:
        case GlobalDictionaries.DocumentsTypes.MoveStorageChild:
          throw new NotImplementedException();
        case GlobalDictionaries.DocumentsTypes.Inventory:
          return this.DeleteDraftInventory(item);
        case GlobalDictionaries.DocumentsTypes.InventoryAct:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool Delete(Guid uid) => throw new NotImplementedException();

    public int Delete(List<Document> itemsList)
    {
      return itemsList.Count<Document>(new Func<Document, bool>(this.Delete));
    }

    public bool Delete(Document item)
    {
      switch (item.Status)
      {
        case GlobalDictionaries.DocumentsStatuses.None:
        case GlobalDictionaries.DocumentsStatuses.Open:
          throw new NotImplementedException();
        case GlobalDictionaries.DocumentsStatuses.Draft:
          return this.DeleteDrafts(item);
        case GlobalDictionaries.DocumentsStatuses.Close:
          return this.DeleteClosed(item);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool MultiThreadMode { get; set; } = true;

    public DocumentsRepository(Gbs.Core.Db.DataBase db) => this._db = db;

    public List<Document> GetByQuery(IQueryable<DOCUMENTS> query)
    {
      LogHelper.OnBegin();
      Performancer performancer1 = new Performancer(string.Format("Загрукза документов из БД с запросом. MultiThread: {0}", (object) this.MultiThreadMode));
      List<DOCUMENTS> list1 = query.ToList<DOCUMENTS>();
      int count = list1.Count;
      if (count == 0)
      {
        performancer1.Stop("Документов: 0");
        return new List<Document>();
      }
      List<DOCUMENTS> list2 = list1.DistinctBy<DOCUMENTS, Guid>((Func<DOCUMENTS, Guid>) (x => x.UID)).ToList<DOCUMENTS>();
      performancer1.AddPoint(string.Format("Получение документов: {0}", (object) count));
      List<Item> list3 = this.GetDocumentsItemsList(query, count).AsParallel<DocumentsRepository.QueryResult>().Select<DocumentsRepository.QueryResult, Item>((Func<DocumentsRepository.QueryResult, Item>) (x => new Item(x.Di, x.St, x.Storage))).DistinctBy<Item, Guid>((Func<Item, Guid>) (x => x.Uid)).ToList<Item>();
      performancer1.AddPoint(string.Format("Таблица записей документов: {0}", (object) list3.Count));
      List<Task> list4 = new List<Task>();
      List<EntityProperties.PropertyValue> docPropertyValues = new List<EntityProperties.PropertyValue>();
      list4.Add(new Task((Action) (() =>
      {
        Performancer performancer2 = new Performancer("Загрукза доп. полей из БД для доков");
        performancer2.AddPoint("Получили доп. поля запросом из БД");
        docPropertyValues = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Document);
        performancer2.AddPoint("Получили итоговый список");
        performancer2.Stop();
      })));
      List<Gbs.Core.Entities.Payments.Payment> dpl = new List<Gbs.Core.Entities.Payments.Payment>();
      list4.Add(new Task((Action) (() =>
      {
        Performancer performancer3 = new Performancer("Загрукза платежей из БД для доков");
        DataContext dataContext = Gbs.Core.Data.GetDataContext();
        IQueryable<DOCUMENTS> query1 = dataContext.GetQuery<DOCUMENTS>(query);
        performancer3.AddPoint("Получили  платежи из БД");
        dpl = Gbs.Core.Entities.Payments.GetPaymentsList(query1.Join((IEnumerable<PAYMENTS>) dataContext.GetTable<PAYMENTS>(), (Expression<Func<DOCUMENTS, Guid>>) (d => d.UID), (Expression<Func<PAYMENTS, Guid>>) (p => p.PARENT_UID), (d, p) => new
        {
          d = d,
          p = p
        }).Where(data => !data.p.IS_DELETED).Select(data => data.p), this.MultiThreadMode);
        performancer3.AddPoint("Получили итоговые платежи из БД");
        performancer3.Stop();
      })));
      List<Storages.Storage> stList = new List<Storages.Storage>();
      list4.Add(new Task((Action) (() => stList = Storages.GetStorages((IQueryable<STORAGES>) Gbs.Core.Data.GetDataContext().GetTable<STORAGES>()).ToList<Storages.Storage>())));
      List<Sections.Section> secList = new List<Sections.Section>();
      list4.Add(new Task((Action) (() => secList = Sections.GetSectionsList())));
      list4.RunList(this.MultiThreadMode);
      performancer1.AddPoint("Ожидание задач");
      performancer1.AddPoint("Проверка на уникальные записи");
      List<Document> list5 = list2.AsParallel<DOCUMENTS>().Select<DOCUMENTS, Document>((Func<DOCUMENTS, Document>) (document =>
      {
        return new Document()
        {
          Uid = document.UID,
          IsDeleted = document.IS_DELETED,
          IsFiscal = document.IS_FISCAL,
          Type = (GlobalDictionaries.DocumentsTypes) document.TYPE,
          DateTime = document.DATE_TIME,
          Comment = document.COMMENT,
          ParentUid = document.PARENT_UID,
          ContractorUid = document.CONTRACTOR_UID,
          UserUid = document.USER_UID,
          Status = (GlobalDictionaries.DocumentsStatuses) document.STATUS,
          Number = document.NUMBER,
          Storage = stList.FirstOrDefault<Storages.Storage>((Func<Storages.Storage, bool>) (x => x.Uid == document.STORAGE_UID)),
          Section = secList.FirstOrDefault<Sections.Section>((Func<Sections.Section, bool>) (x => x.Uid == document.SECTION_UID))
        };
      })).ToList<Document>();
      performancer1.AddPoint("Заполнение документов");
      List<Gbs.Core.Entities.Payments.Payment> inner = dpl;
      List<Document> list6 = list5.GroupJoin<Document, Gbs.Core.Entities.Payments.Payment, Guid, Document>((IEnumerable<Gbs.Core.Entities.Payments.Payment>) inner, (Func<Document, Guid>) (d => d.Uid), (Func<Gbs.Core.Entities.Payments.Payment, Guid>) (p => p.ParentUid), (Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>, Document>) ((d, p) =>
      {
        d.Payments = p.ToList<Gbs.Core.Entities.Payments.Payment>();
        return d;
      })).AsParallel<Document>().ToList<Document>().GroupJoin<Document, Item, Guid, Document>((IEnumerable<Item>) list3, (Func<Document, Guid>) (d => d.Uid), (Func<Item, Guid>) (di => di.DocumentUid), (Func<Document, IEnumerable<Item>, Document>) ((d, di) =>
      {
        d.Items = di.ToList<Item>();
        return d;
      })).AsParallel<Document>().ToList<Document>().GroupJoin<Document, EntityProperties.PropertyValue, Guid, Document>((IEnumerable<EntityProperties.PropertyValue>) docPropertyValues, (Func<Document, Guid>) (g => g.Uid), (Func<EntityProperties.PropertyValue, Guid>) (p => p.EntityUid), (Func<Document, IEnumerable<EntityProperties.PropertyValue>, Document>) ((g, p) =>
      {
        g.Properties = p.ToList<EntityProperties.PropertyValue>();
        return g;
      })).AsParallel<Document>().ToList<Document>();
      performancer1.AddPoint("Объединение таблиц: платежи, записи, доп. поля");
      performancer1.Stop();
      LogHelper.OnEnd();
      return list6;
    }

    private List<DocumentsRepository.QueryResult> GetDocumentsItemsList(
      IQueryable<DOCUMENTS> query,
      int docsCount)
    {
      List<IQueryable<DOCUMENTS>> queryableList = new List<IQueryable<DOCUMENTS>>();
      if (!this.MultiThreadMode)
        return GetQueryResults(query);
      int num1 = docsCount / 8;
      int num2 = docsCount - num1 * 8;
      for (int index = 0; index < 8; ++index)
      {
        int count = num1;
        if (index == 7)
          count += num2;
        queryableList.Add(query.OrderBy<DOCUMENTS, DateTime>((Expression<Func<DOCUMENTS, DateTime>>) (x => x.DATE_TIME)).Skip<DOCUMENTS>(num1 * index).Take<DOCUMENTS>(count));
      }
      List<Task<List<DocumentsRepository.QueryResult>>> taskList = new List<Task<List<DocumentsRepository.QueryResult>>>();
      foreach (IQueryable<DOCUMENTS> queryable in queryableList)
      {
        IQueryable<DOCUMENTS> part = queryable;
        taskList.Add(Task.Run<List<DocumentsRepository.QueryResult>>((Func<List<DocumentsRepository.QueryResult>>) (() => GetQueryResults(part))));
      }
      return ((IEnumerable<List<DocumentsRepository.QueryResult>>) Task.WhenAll<List<DocumentsRepository.QueryResult>>((IEnumerable<Task<List<DocumentsRepository.QueryResult>>>) taskList).Result).SelectMany<List<DocumentsRepository.QueryResult>, DocumentsRepository.QueryResult>((Func<List<DocumentsRepository.QueryResult>, IEnumerable<DocumentsRepository.QueryResult>>) (r => (IEnumerable<DocumentsRepository.QueryResult>) r)).ToList<DocumentsRepository.QueryResult>();

      static List<DocumentsRepository.QueryResult> GetQueryResults(IQueryable<DOCUMENTS> part)
      {
        DataContext dataContext = Gbs.Core.Data.GetDataContext();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        return dataContext.GetQuery<DOCUMENTS>(part).Join((IEnumerable<DOCUMENT_ITEMS>) dataContext.GetTable<DOCUMENT_ITEMS>(), (Expression<Func<DOCUMENTS, Guid>>) (d => d.UID), (Expression<Func<DOCUMENT_ITEMS, Guid>>) (di => di.DOCUMENT_UID), (d, di) => new
        {
          d = d,
          di = di
        }).Where(data => data.di.IS_DELETED == false).GroupJoin((IEnumerable<GOODS_STOCK>) dataContext.GetTable<GOODS_STOCK>(), data => data.di.STOCK_UID, (Expression<Func<GOODS_STOCK, Guid>>) (st => st.UID), (data, stJoin) => new
        {
          \u003C\u003Eh__TransparentIdentifier0 = data,
          stJoin = stJoin
        }).SelectMany(data => data.stJoin.DefaultIfEmpty<GOODS_STOCK>(), (data, st) => new
        {
          \u003C\u003Eh__TransparentIdentifier1 = data,
          st = st
        }).GroupJoin((IEnumerable<STORAGES>) dataContext.GetTable<STORAGES>(), data => data.st.STORAGE_UID, (Expression<Func<STORAGES, Guid>>) (storage => storage.UID), (data, storageJoin) => new
        {
          \u003C\u003Eh__TransparentIdentifier2 = data,
          storageJoin = storageJoin
        }).SelectMany(data => data.storageJoin.DefaultIfEmpty<STORAGES>(), System.Linq.Expressions.Expression.Lambda<Func<\u003C\u003Ef__AnonymousType19<\u003C\u003Ef__AnonymousType18<\u003C\u003Ef__AnonymousType17<\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>, IEnumerable<GOODS_STOCK>>, GOODS_STOCK>, IEnumerable<STORAGES>>, STORAGES, DocumentsRepository.QueryResult>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.MemberInit(System.Linq.Expressions.Expression.New(typeof (DocumentsRepository.QueryResult)), (MemberBinding) System.Linq.Expressions.Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DocumentsRepository.QueryResult.set_Di)), )))); // Unable to render the statement
      }
    }

    public List<Document> GetAllItems()
    {
      return this.GetByQuery((IQueryable<DOCUMENTS>) Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>());
    }

    public List<Document> GetActiveItems()
    {
      return this.GetByQuery(Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => !x.IS_DELETED)));
    }

    public List<Document> GetByParentUid(Guid parentUid)
    {
      return this.GetByQuery(Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.PARENT_UID == parentUid)));
    }

    public List<Document> GetItemsForContractor(Guid contractorUid, bool includeDeleted = false)
    {
      IQueryable<DOCUMENTS> queryable = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.CONTRACTOR_UID == contractorUid));
      if (!includeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable);
    }

    public List<Document> GetItemsNoLinqPf(DateTime dateStart, bool includeDeleted = false)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DocumentsRepository.\u003C\u003Ec__DisplayClass28_0 cDisplayClass280 = new DocumentsRepository.\u003C\u003Ec__DisplayClass28_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass280.dateStart = dateStart;
      DataContext dataContext = Gbs.Core.Data.GetDataContext();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass280.linqSale = dataContext.GetTable<LINK_ENTITIES>();
      ParameterExpression parameterExpression1;
      ParameterExpression parameterExpression2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      IQueryable<DOCUMENTS> queryable = dataContext.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => !cDisplayClass280.linqSale.Any<LINK_ENTITIES>(System.Linq.Expressions.Expression.Lambda<Func<LINK_ENTITIES, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(l.ENTITY_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)) && x.DATE_TIME.Date >= cDisplayClass280.dateStart.Date && x.TYPE == 1));
      if (!includeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable);
    }

    public List<Document> GetItemReturnNoLinqPf(DateTime dateStart, bool includeDeleted = false)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DocumentsRepository.\u003C\u003Ec__DisplayClass29_0 cDisplayClass290 = new DocumentsRepository.\u003C\u003Ec__DisplayClass29_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass290.dateStart = dateStart;
      DataContext dataContext = Gbs.Core.Data.GetDataContext();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass290.linqSale = dataContext.GetTable<LINK_ENTITIES>();
      ParameterExpression parameterExpression1;
      ParameterExpression parameterExpression2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      IQueryable<DOCUMENTS> queryable = dataContext.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => cDisplayClass290.linqSale.Any<LINK_ENTITIES>(System.Linq.Expressions.Expression.Lambda<Func<LINK_ENTITIES, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(l.ENTITY_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (LINK_ENTITIES.get_ID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 0, typeof (int)))), parameterExpression2)) && x.DATE_TIME.Date >= cDisplayClass290.dateStart.Date && x.TYPE == 2));
      if (!includeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable);
    }

    public List<Document> GetItemsWithFilter(
      GlobalDictionaries.DocumentsTypes type,
      bool includeDeleted = false)
    {
      IQueryable<DOCUMENTS> queryable = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == (int) type));
      if (!includeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable);
    }

    public List<Document> GetItemsWithFilter(Guid parentUid, bool includeDeleted = false)
    {
      IQueryable<DOCUMENTS> queryable = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.PARENT_UID == parentUid));
      if (!includeDeleted)
        queryable = queryable.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable);
    }

    public List<Document> GetItemsWithFilter(DateTime date, bool includeDeleted = false, bool isOnTime = true)
    {
      IQueryable<DOCUMENTS> source = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().AsQueryable<DOCUMENTS>();
      IQueryable<DOCUMENTS> queryable1;
      if (!isOnTime)
        queryable1 = source.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME.Date == date.Date));
      else
        queryable1 = source.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME == date));
      IQueryable<DOCUMENTS> queryable2 = queryable1;
      if (!includeDeleted)
        queryable2 = queryable2.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable2);
    }

    public List<Document> GetItemsWithFilter(
      DateTime startDate,
      DateTime endDate,
      bool includeDeleted = false,
      bool isOnTime = true)
    {
      IQueryable<DOCUMENTS> source = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().AsQueryable<DOCUMENTS>();
      IQueryable<DOCUMENTS> queryable1;
      if (!isOnTime)
        queryable1 = source.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME.Date >= startDate.Date && x.DATE_TIME.Date <= endDate.Date));
      else
        queryable1 = source.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME >= startDate && x.DATE_TIME <= endDate));
      IQueryable<DOCUMENTS> queryable2 = queryable1;
      if (!includeDeleted)
        queryable2 = queryable2.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable2);
    }

    public List<Document> GetItemsWithFilter(
      DateTime startDate,
      DateTime endDate,
      GlobalDictionaries.DocumentsTypes type,
      bool isOnTime,
      bool includeDeleted = false,
      Guid? clientGuid = null)
    {
      IQueryable<DOCUMENTS> source1 = Gbs.Core.Data.GetDataContext().GetTable<DOCUMENTS>().AsQueryable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == (int) type));
      IQueryable<DOCUMENTS> queryable1;
      if (!isOnTime)
        queryable1 = source1.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME.Date >= startDate.Date && x.DATE_TIME.Date <= endDate.Date));
      else
        queryable1 = source1.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME >= startDate && x.DATE_TIME <= endDate));
      IQueryable<DOCUMENTS> source2 = queryable1;
      IQueryable<DOCUMENTS> queryable2;
      if (clientGuid.HasValue)
        queryable2 = source2.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => (Guid?) x.CONTRACTOR_UID == clientGuid));
      else
        queryable2 = source2;
      IQueryable<DOCUMENTS> queryable3 = queryable2;
      if (!includeDeleted)
        queryable3 = queryable3.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      return this.GetByQuery(queryable3);
    }

    private ActionResult ValidateSale(Document item)
    {
      ActionResult actionResult = new ActionResult(ActionResult.Results.Ok);
      Decimal num1 = item.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      Decimal num2 = item.Items.Sum<Item>((Func<Item, Decimal>) (x => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(x.Quantity, x.SellPrice, x.Discount))));
      if (num1 < num2 && item.ContractorUid == Guid.Empty)
        actionResult.AddMessage(ActionResult.Results.Error, Translate.ДляПродажиБзеПлатежейДолженБытьУказанПокупатель);
      if (!item.Items.Any<Item>())
        actionResult.AddMessage(ActionResult.Results.Error, Translate.ПродажаДолжнаСодержатьХотяБыОднуЗапись);
      if (item.Items.Any<Item>((Func<Item, bool>) (x => x.Quantity < 0M)))
        actionResult.AddMessage(ActionResult.Results.Error, Translate.DocumentsRepository_У_одного_или_нескольких_товаров_указано_отрицательное_кол_во);
      if (num1 > num2)
      {
        Other.ConsoleWrite(string.Format("payments: {0}; goods: {1}", (object) num1, (object) num2));
        actionResult.AddMessage(ActionResult.Results.Error, Translate.DocumentsRepository_Сумма_платежей_не_может_быть_больше__чем_сумма_товаров);
      }
      if (item.Items.Any<Item>((Func<Item, bool>) (x =>
      {
        string comment = x.Comment;
        return comment != null && comment.Length > 500;
      })))
        actionResult.AddMessage(ActionResult.Results.Error, string.Format(Translate.DocumentsRepository_ValidateSale_, (object) string.Join(Other.NewLine(), item.Items.Where<Item>((Func<Item, bool>) (x =>
        {
          string comment = x.Comment;
          return comment != null && comment.Length > 500;
        })).Select<Item, string>((Func<Item, string>) (x => x.Good.Name)))));
      return actionResult;
    }

    public ActionResult Validate(Document item)
    {
      Document document1 = item;
      if (document1.Section == null)
        document1.Section = Sections.GetCurrentSection();
      Document document2 = item;
      if (document2.Storage == null)
        document2.Storage = Storages.GetStorages().First<Storages.Storage>();
      List<Item> source1 = new List<Item>();
      List<Item> source2 = new List<Item>();
      foreach (Item obj in item.Items.Where<Item>((Func<Item, bool>) (x => ValidationHelper.DataValidation((Entity) x).Result == ActionResult.Results.Error)))
      {
        if (obj.IsDeleted)
        {
          obj.Quantity = 0M;
          obj.SellPrice = 0M;
          obj.BuyPrice = 0M;
          source2.Add(obj);
        }
        else
          source1.Add(obj);
      }
      if (source2.Any<Item>())
        MessageBoxHelper.Warning(Translate.НекоторыеУдаленныеПозицииНеПрошлиПроверкуПриСохраненииКоличествоИЦеныБылиОбнуленыПродолжаемПопыткуСохранитьДокументNN + string.Join('\n'.ToString(), source2.Select<Item, string>((Func<Item, string>) (x => x.Good.Name))));
      foreach (object obj in source1)
        Other.ConsoleWrite(obj.ToJsonString(true));
      foreach (Item obj in item.Items)
        obj.Comment = obj.Comment?.Trim();
      if (source1.Any<Item>())
        return new ActionResult(ActionResult.Results.Error, Translate.DocumentsRepository_Validate_Одна_или_более_записей_документа_не_прошли_валидацию);
      if (item.Payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (docPayment => ValidationHelper.DataValidation((Entity) docPayment).Result == ActionResult.Results.Error)))
        return new ActionResult(ActionResult.Results.Error, Translate.DocumentsRepository_Validate_Одна_или_более_платежей_документа_не_прошли_валидацию);
      ActionResult result = new ActionResult(ActionResult.Results.Ok);
      switch (item.Type)
      {
        case GlobalDictionaries.DocumentsTypes.None:
          return new ActionResult(ActionResult.Results.Error, Translate.DocumentsRepository_Validate_Тип_документа_не_указан);
        case GlobalDictionaries.DocumentsTypes.Sale:
          result = this.ValidateSale(item);
          goto case GlobalDictionaries.DocumentsTypes.SaleReturn;
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
        case GlobalDictionaries.DocumentsTypes.Move:
        case GlobalDictionaries.DocumentsTypes.WriteOff:
        case GlobalDictionaries.DocumentsTypes.UserStockEdit:
        case GlobalDictionaries.DocumentsTypes.Inventory:
        case GlobalDictionaries.DocumentsTypes.InventoryAct:
        case GlobalDictionaries.DocumentsTypes.CafeOrder:
        case GlobalDictionaries.DocumentsTypes.ClientOrder:
        case GlobalDictionaries.DocumentsTypes.MoveStorage:
        case GlobalDictionaries.DocumentsTypes.ProductionItem:
        case GlobalDictionaries.DocumentsTypes.ProductionSet:
        case GlobalDictionaries.DocumentsTypes.ProductionList:
        case GlobalDictionaries.DocumentsTypes.ClientOrderReserve:
        case GlobalDictionaries.DocumentsTypes.BeerProductionItem:
        case GlobalDictionaries.DocumentsTypes.BeerProductionSet:
        case GlobalDictionaries.DocumentsTypes.BeerProductionList:
          return ValidationHelper.DataValidation((Entity) item).Concat(result);
        case GlobalDictionaries.DocumentsTypes.Buy:
        case GlobalDictionaries.DocumentsTypes.BuyReturn:
        case GlobalDictionaries.DocumentsTypes.MoveStorageChild:
          result = this.ValidateBuy(item);
          goto case GlobalDictionaries.DocumentsTypes.SaleReturn;
        case GlobalDictionaries.DocumentsTypes.MoveReturn:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private (List<DocumentsRepository.StockChange> list, bool result) PrepareToSave(
      Document doc,
      Document secondDoc = null)
    {
      switch (doc.Type)
      {
        case GlobalDictionaries.DocumentsTypes.Sale:
        case GlobalDictionaries.DocumentsTypes.Move:
        case GlobalDictionaries.DocumentsTypes.WriteOff:
          throw new NotSupportedException(Translate.DocumentsRepository_PrepareToSave_Метод_не_поддерживает_указанные_типы_документов);
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
          return this.PrepareSaleReturn(doc);
        case GlobalDictionaries.DocumentsTypes.Buy:
        case GlobalDictionaries.DocumentsTypes.MoveStorageChild:
        case GlobalDictionaries.DocumentsTypes.ProductionItem:
        case GlobalDictionaries.DocumentsTypes.BeerProductionItem:
          return this.PrepareWaybill(doc);
        case GlobalDictionaries.DocumentsTypes.Inventory:
          return new InventoryPreparer(doc, secondDoc, this._db).Prepare();
        default:
          return (new List<DocumentsRepository.StockChange>(), true);
      }
    }

    public static WaybillConfig.RePriceVariants RePriceVariants { get; set; }

    private (List<DocumentsRepository.StockChange> list, bool result) PrepareWaybill(Document doc)
    {
      Document byUid = this.GetByUid(doc.Uid);
      List<DocumentsRepository.StockChange> source = new List<DocumentsRepository.StockChange>();
      Item[] array = new Item[doc.Items.Count];
      doc.Items.CopyTo(array);
      doc.Items.Clear();
      WaybillConfig waybill = new ConfigsRepository<Settings>().Get().Waybill;
      Gbs.Core.Config.DataBase dataBase = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      if (waybill.RePriceRule != WaybillConfig.RePriceVariants.RequestForEachWaybill)
        DocumentsRepository.RePriceVariants = waybill.RePriceRule;
      bool flag = DocumentsRepository.RePriceVariants == WaybillConfig.RePriceVariants.RePriceExitsStocks && dataBase.ModeProgram != GlobalDictionaries.Mode.Home && doc.Type == GlobalDictionaries.DocumentsTypes.Buy && doc.Status != GlobalDictionaries.DocumentsStatuses.Draft;
      Dictionary<Guid, Decimal> list1 = new Dictionary<Guid, Decimal>();
      foreach (Item obj1 in array)
      {
        Item item = obj1;
        Decimal num1 = 0M;
        if (doc.Status != GlobalDictionaries.DocumentsStatuses.Draft && !doc.IsDeleted)
          num1 = item.IsDeleted ? 0M : item.Quantity;
        List<Item> list2 = byUid != null ? byUid.Items.Where<Item>((Func<Item, bool>) (i => i.Uid == item.Uid)).ToList<Item>() : (List<Item>) null;
        Decimal num2 = 0M;
        if (byUid != null && byUid.Status != GlobalDictionaries.DocumentsStatuses.Draft && !byUid.IsDeleted && list2.Any<Item>())
          num2 = list2.Where<Item>((Func<Item, bool>) (i => !i.IsDeleted)).Sum<Item>((Func<Item, Decimal>) (i => i.Quantity));
        GoodsStocks.GoodStock goodStock1 = item.GoodStock;
        // ISSUE: explicit non-virtual call
        if (GoodsStocks.GetStocksByUid(goodStock1 != null ? __nonvirtual (goodStock1.Uid) : Guid.Empty) == null || byUid != null && byUid.Status == GlobalDictionaries.DocumentsStatuses.Draft && doc.Status != GlobalDictionaries.DocumentsStatuses.Draft)
        {
          Item obj2 = item;
          GoodsStocks.GoodStock goodStock2 = new GoodsStocks.GoodStock();
          Guid guid;
          if ((byUid == null || byUid.Status != GlobalDictionaries.DocumentsStatuses.Draft) && byUid != null)
          {
            guid = Guid.NewGuid();
          }
          else
          {
            GoodsStocks.GoodStock goodStock3 = item.GoodStock;
            // ISSUE: explicit non-virtual call
            guid = goodStock3 != null ? __nonvirtual (goodStock3.Uid) : Guid.NewGuid();
          }
          goodStock2.Uid = guid;
          goodStock2.GoodUid = item.GoodUid;
          goodStock2.IsDeleted = false;
          goodStock2.ModificationUid = item.ModificationUid;
          goodStock2.Price = item.SellPrice;
          goodStock2.Storage = doc.Storage;
          obj2.GoodStock = goodStock2;
          if (flag && !list1.ContainsKey(item.GoodUid))
            list1.Add(item.GoodUid, item.SellPrice);
        }
        else if (list2 != null && list2.Any<Item>() && list2.First<Item>().SellPrice != item.SellPrice)
          item.GoodStock.Price = item.SellPrice;
        else if (list2 == null)
        {
          item.GoodStock.Price = item.SellPrice;
        }
        else
        {
          GoodsStocks.GoodStock goodStock4 = item.GoodStock;
          Guid? nullable1;
          Guid? nullable2;
          if (goodStock4 == null)
          {
            nullable1 = new Guid?();
            nullable2 = nullable1;
          }
          else
          {
            // ISSUE: explicit non-virtual call
            nullable2 = new Guid?(__nonvirtual (goodStock4.Uid));
          }
          Guid? nullable3 = nullable2;
          nullable1 = list2 != null ? list2.FirstOrDefault<Item>()?.GoodStock.Uid : new Guid?();
          Guid guid = nullable1 ?? Guid.Empty;
          if ((nullable3.HasValue ? (nullable3.GetValueOrDefault() == guid ? 1 : 0) : 0) != 0)
            item.GoodStock = list2 != null ? list2.First<Item>().GoodStock : (GoodsStocks.GoodStock) null;
        }
        source.Add(new DocumentsRepository.StockChange()
        {
          Stock = item.GoodStock,
          QuantityChange = num1 - num2
        });
        doc.Items.Add(item);
      }
      if (flag)
      {
        DocumentsRepository.GoodsRePricer goodsRePricer = new DocumentsRepository.GoodsRePricer(list1, doc, this._db);
        doc.Saved += new Document.SaveHandler(goodsRePricer.RePrice);
      }
      return (source.Where<DocumentsRepository.StockChange>((Func<DocumentsRepository.StockChange, bool>) (x => !x.Stock.IsDeleted || !(x.QuantityChange == 0M) || !(x.Stock.Stock == 0M))).ToList<DocumentsRepository.StockChange>(), true);
    }

    private (List<DocumentsRepository.StockChange> list, bool result) PrepareSaleReturn(
      Document document)
    {
      List<Document> list1 = this.GetByQuery(this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => !x.IS_DELETED && x.PARENT_UID == document.ParentUid && x.TYPE == 2))).ToList<Document>();
      List<Item> source = new List<Item>();
      foreach (Document document1 in list1)
      {
        foreach (Item obj1 in document1.Items.Where<Item>((Func<Item, bool>) (i => !i.IsDeleted)))
        {
          Item item = obj1;
          if (source.Any<Item>(new Func<Item, bool>(StockFilter)))
          {
            source.First<Item>(new Func<Item, bool>(StockFilter)).Quantity += item.Quantity;
          }
          else
          {
            Item obj2 = item.Clone<Item>();
            obj2.Uid = Guid.NewGuid();
            source.Add(obj2);
          }

          bool StockFilter(Item i) => i.GoodStock.Uid == item.GoodStock.Uid;
        }
      }
      Document document2 = this.GetByQuery(this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == document.ParentUid))).SingleOrDefault<Document>();
      if (document2 == null)
        throw new NullReferenceException(Translate.DocumentsRepository_PrepareSaleReturn_Не_найдена_продажа__для_которой_происходит_попытка_сохранения_возврата);
      Item[] objArray = new Item[document2.Items.Count];
      document2.Items.CopyTo(objArray);
      foreach (Item obj in source)
      {
        Item returnedItem = obj;
        if (returnedItem.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
          ((IEnumerable<Item>) objArray).First<Item>((Func<Item, bool>) (x => x.GoodUid == returnedItem.GoodUid && x.SellPrice == returnedItem.SellPrice)).Quantity -= returnedItem.Quantity;
        else
          ((IEnumerable<Item>) objArray).First<Item>((Func<Item, bool>) (x => x.GoodStock.Uid == returnedItem.GoodStock.Uid)).Quantity -= returnedItem.Quantity;
      }
      Item[] array = new Item[document.Items.Count];
      document.Items.CopyTo(array);
      document.Items.Clear();
      List<DocumentsRepository.StockChange> stockChangeList = new List<DocumentsRepository.StockChange>();
      foreach (Item obj3 in array)
      {
        Item goodToReturn = obj3;
        Decimal num = goodToReturn.Quantity;
        List<Item> list2 = ((IEnumerable<Item>) objArray).Where<Item>((Func<Item, bool>) (x => x.GoodUid == goodToReturn.GoodUid && x.SellPrice == goodToReturn.SellPrice && x.ModificationUid == goodToReturn.ModificationUid)).ToList<Item>();
        while (num > 0M)
        {
          foreach (Item obj4 in list2.Where<Item>((Func<Item, bool>) (x => x.Quantity > 0M)))
          {
            if (!(num == 0M))
            {
              Item obj5 = new Item()
              {
                GoodStock = obj4.GoodStock,
                SellPrice = obj4.SellPrice,
                Discount = obj4.Discount,
                Good = obj4.Good,
                ModificationUid = obj4.ModificationUid
              };
              if (obj4.Quantity >= num)
              {
                obj5.Quantity = num;
                obj4.Quantity -= num;
                num = 0M;
              }
              else
              {
                obj5.Quantity = obj4.Quantity;
                num -= obj4.Quantity;
                obj4.Quantity = 0M;
              }
              document.Items.Add(obj5);
              if (obj5.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service)
              {
                DocumentsRepository.StockChange stockChange = new DocumentsRepository.StockChange()
                {
                  Stock = obj5.GoodStock,
                  QuantityChange = obj5.Quantity
                };
                stockChangeList.Add(stockChange);
              }
            }
          }
        }
      }
      return (stockChangeList, true);
    }

    private void UpdateGoodsInPlanfix(Document document)
    {
      LogHelper.OnBegin();
      if (DevelopersHelper.IsUnitTest())
        return;
      TaskHelper.TaskRun((Action) (() =>
      {
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          return;
        LogHelper.Debug("Передаем данные в ПФ после формирования документа " + document.Type.ToString());
        PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
        if (!planfix.IsActive || document.Type == GlobalDictionaries.DocumentsTypes.UserStockEdit)
          return;
        ConfigManager.Config = new Planfix.Api.Config(planfix.AccountName, planfix.ApiUrl, planfix.DecryptedKeyApi, planfix.DecryptedToken);
        using (Gbs.Core.Db.DataBase db = Gbs.Core.Data.GetDataBase())
          PlanfixHelper.UpdateGoodPf(document.Items.Where<Item>((Func<Item, bool>) (x => !x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Production, GlobalDictionaries.GoodsSetStatuses.Set))).Select<Item, Gbs.Core.Entities.Goods.Good>((Func<Item, Gbs.Core.Entities.Goods.Good>) (x => new GoodRepository(db).GetByUid(x.GoodUid))).ToList<Gbs.Core.Entities.Goods.Good>(), planfix);
      }));
    }

    public bool Save(Document item, bool isEditStock, HomeOfficeHelper officeHelper = null)
    {
      return this.SaveDocument(item, isEditStock, officeHelper: officeHelper);
    }

    private bool SaveDocument(
      Document item,
      bool isEditStock = true,
      bool isTransactionNew = false,
      HomeOfficeHelper officeHelper = null)
    {
      DataConnectionTransaction connectionTransaction = (DataConnectionTransaction) null;
      Performancer performancer = new Performancer(string.Format("Сохранение документа. Type: {0} ", (object) item.Type));
      try
      {
        LogHelper.OnBegin();
        this.DoValidation(item);
        Document secondDoc = (Document) null;
        if (item.Type == GlobalDictionaries.DocumentsTypes.Inventory)
          secondDoc = new Document();
        int num1 = item.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.BuyReturn, GlobalDictionaries.DocumentsTypes.WriteOff, GlobalDictionaries.DocumentsTypes.Move, GlobalDictionaries.DocumentsTypes.MoveStorage, GlobalDictionaries.DocumentsTypes.ProductionSet, GlobalDictionaries.DocumentsTypes.SaleReturn, GlobalDictionaries.DocumentsTypes.BeerProductionSet, GlobalDictionaries.DocumentsTypes.ClientOrderReserve) ? 1 : 0;
        connectionTransaction = this._db.BeginTransaction();
        if (num1 != 0)
        {
          Document byUid = this.GetByUid(item.Uid);
          if (byUid != null)
            this.Delete(byUid);
          List<Document> documentList = new List<Document>();
          switch (item.Type)
          {
            case GlobalDictionaries.DocumentsTypes.None:
            case GlobalDictionaries.DocumentsTypes.UserStockEdit:
            case GlobalDictionaries.DocumentsTypes.Inventory:
            case GlobalDictionaries.DocumentsTypes.InventoryAct:
            case GlobalDictionaries.DocumentsTypes.CafeOrder:
            case GlobalDictionaries.DocumentsTypes.ClientOrder:
            case GlobalDictionaries.DocumentsTypes.SetChildStockChange:
              using (List<Document>.Enumerator enumerator = documentList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  this.WriteDocumentToDb(enumerator.Current);
                break;
              }
            case GlobalDictionaries.DocumentsTypes.Sale:
            case GlobalDictionaries.DocumentsTypes.BuyReturn:
            case GlobalDictionaries.DocumentsTypes.Move:
            case GlobalDictionaries.DocumentsTypes.WriteOff:
            case GlobalDictionaries.DocumentsTypes.MoveStorage:
            case GlobalDictionaries.DocumentsTypes.ProductionSet:
            case GlobalDictionaries.DocumentsTypes.ClientOrderReserve:
            case GlobalDictionaries.DocumentsTypes.BeerProductionSet:
              documentList = new StockOutDocumentPreparer(this._db).Prepare(item);
              goto case GlobalDictionaries.DocumentsTypes.None;
            case GlobalDictionaries.DocumentsTypes.SaleReturn:
              documentList = this.DoSaleReturn(item);
              goto case GlobalDictionaries.DocumentsTypes.None;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        else
        {
          List<DocumentsRepository.StockChange> stockChangeList;
          bool result;
          if (!isEditStock)
          {
            stockChangeList = (List<DocumentsRepository.StockChange>) null;
            result = true;
          }
          else
            (stockChangeList, result) = this.PrepareToSave(item, secondDoc);
          if (!result)
            return false;
          if (isEditStock)
            this.WriteStockChangesToDb(item, stockChangeList);
        }
        this.WriteDocumentToDb(item);
        performancer.AddPoint("Запись документа в БД");
        if (secondDoc != null)
        {
          this.WriteDocumentToDb(secondDoc);
          performancer.AddPoint("Запись второго документа в БД");
        }
        this._db.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == item.Uid));
        int num2 = item.Properties == null ? 0 : (item.Properties.All<EntityProperties.PropertyValue>(new Func<EntityProperties.PropertyValue, bool>(this.SavePropertyValue)) ? 1 : 0);
        performancer.AddPoint("Получение доп. полей");
        if (num2 == 0)
          return false;
        if (item.Type != GlobalDictionaries.DocumentsTypes.Buy)
          this.UpdateGoodsInPlanfix(item);
        if (!DocumentsRepository.SendDocumentFromHomeOffice(item, officeHelper))
        {
          connectionTransaction?.Rollback();
          return false;
        }
        connectionTransaction?.Commit();
        DocumentsRepository.SaveHandler documentSaved = DocumentsRepository.DocumentSaved;
        if (documentSaved != null)
          documentSaved(item);
        item.InvokeSaved();
        LogHelper.OnEnd();
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка сохранения документа");
        connectionTransaction?.Rollback();
        return false;
      }
      finally
      {
        performancer.Stop();
      }
    }

    private List<Document> DoSaleReturn(Document item)
    {
      List<Document> source = new StockOutDocumentPreparerReturn(this._db).Prepare(item);
      List<DocumentsRepository.StockChange> stockChangesList = new List<DocumentsRepository.StockChange>();
      foreach (Item obj in item.Items.Where<Item>(new Func<Item, bool>(NonReturnedGoods)).Concat<Item>(source.SelectMany<Document, Item>((Func<Document, IEnumerable<Item>>) (x => (IEnumerable<Item>) x.Items)).Where<Item>(new Func<Item, bool>(NonReturnedGoods))))
        stockChangesList.Add(new DocumentsRepository.StockChange()
        {
          QuantityChange = obj.Quantity,
          Stock = obj.GoodStock
        });
      this.WriteStockChangesToDb(item, stockChangesList);
      return source;

      static bool NonReturnedGoods(Item x)
      {
        return x.Good.SetStatus != GlobalDictionaries.GoodsSetStatuses.Set && x.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service;
      }
    }

    private void DoValidation(Document item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
        throw new Exception(Translate.DocumentsRepository_Документ_не_может_быть_сохранен__т_к__не_прошел_валидацию__ + string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages));
    }

    private static bool SendDocumentFromHomeOffice(Document item, HomeOfficeHelper officeHelper = null)
    {
      if (DevelopersHelper.IsUnitTest() || new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home)
        return true;
      if (officeHelper == null)
        new HomeOfficeHelper().PrepareAndSend<Document>(item, HomeOfficeHelper.EntityEditHome.Document);
      else
        officeHelper.CreateEditFile<Document>(item, HomeOfficeHelper.EntityEditHome.Document);
      return true;
    }

    private bool SavePropertyValue(EntityProperties.PropertyValue property)
    {
      try
      {
        if (property.Value == null || property.Value.ToString().IsNullOrEmpty())
          return true;
        if (property.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        this._db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
        {
          UID = property.Uid,
          ENTITY_UID = property.EntityUid,
          IS_DELETED = property.IsDeleted,
          TYPE_UID = property.Type.Uid,
          CONTENT = JsonConvert.ToString(property.Value)
        });
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка сохранения значения доп. поля товара");
        return false;
      }
    }

    private void WriteDocumentToDb(Document item)
    {
      DOCUMENTS documents = new DOCUMENTS()
      {
        UID = item.Uid,
        IS_DELETED = item.IsDeleted,
        IS_FISCAL = item.IsFiscal,
        TYPE = (int) item.Type,
        PARENT_UID = item.ParentUid,
        COMMENT = item.Comment,
        CONTRACTOR_UID = item.ContractorUid,
        DATE_TIME = item.DateTime,
        NUMBER = item.Number,
        USER_UID = item.UserUid,
        STATUS = (int) item.Status,
        STORAGE_UID = item.Storage.Uid,
        SECTION_UID = item.Section.Uid
      };
      LogHelper.Trace("Запись в БД документ: " + documents.ToJsonString());
      this._db.InsertOrReplace<DOCUMENTS>(documents);
      if (item.Type == GlobalDictionaries.DocumentsTypes.CafeOrder)
        this._db.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == item.Uid)).Set<DOCUMENT_ITEMS, bool>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED), true).Update<DOCUMENT_ITEMS>();
      foreach (Item obj in item.Items)
      {
        Gbs.Core.Db.DataBase db = this._db;
        DOCUMENT_ITEMS documentItems = new DOCUMENT_ITEMS();
        documentItems.UID = obj.Uid;
        documentItems.IS_DELETED = obj.IsDeleted;
        documentItems.DOCUMENT_UID = item.Uid;
        documentItems.COMMENT = obj.Comment ?? string.Empty;
        documentItems.DISCOUNT = obj.Discount.ToDbDecimal();
        documentItems.BUY_PRICE = obj.BuyPrice.ToDbDecimal();
        documentItems.QUANTITY = obj.Quantity.ToDbDecimal();
        documentItems.SALE_PRICE = obj.SellPrice.ToDbDecimal();
        GoodsStocks.GoodStock goodStock = obj.GoodStock;
        // ISSUE: explicit non-virtual call
        documentItems.STOCK_UID = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
        documentItems.GOOD_UID = obj.GoodUid;
        documentItems.MODIFICATION_UID = obj.ModificationUid;
        db.InsertOrReplace<DOCUMENT_ITEMS>(documentItems);
      }
      foreach (Gbs.Core.Entities.Payments.Payment payment in item.Payments)
      {
        payment.ParentUid = item.Uid;
        if (!payment.Save())
          throw new InvalidOperationException(Translate.DocumentsRepository_WriteDocumentToDb_Платеж_по_документу_не_удалось_сохранить);
      }
      Cache.UpdateInCache(item);
    }

    [Localizable(false)]
    private void WriteStockChangesToDb(
      Document doc,
      List<DocumentsRepository.StockChange> stockChangesList)
    {
      WaybillConfig waybill = new ConfigsRepository<Settings>().Get().Waybill;
      LogHelper.Trace(string.Format("Изменение остатков по документу {0}, {1} от {2} для {3} записей", (object) doc.Type, (object) doc.Number, (object) doc.DateTime, (object) stockChangesList.Count));
      foreach (DocumentsRepository.StockChange stockChanges in stockChangesList)
      {
        DocumentsRepository.StockChange stock = stockChanges;
        if (!this._db.GetTable<GOODS_STOCK>().Any<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == stock.Stock.Uid)))
        {
          GOODS_STOCK goodsStock = new GOODS_STOCK()
          {
            UID = stock.Stock.Uid,
            STOCK = 0M,
            GOOD_UID = stock.Stock.GoodUid,
            MODIFICATION_UID = stock.Stock.ModificationUid,
            PRICE = stock.Stock.Price.ToDbDecimal(),
            STORAGE_UID = stock.Stock.Storage.Uid,
            IS_DELETED = stock.Stock.IsDeleted
          };
          LogHelper.Trace("Запись остатка в БД: " + goodsStock.ToJsonString());
          this._db.InsertOrReplace<GOODS_STOCK>(goodsStock);
        }
        IUpdatable<GOODS_STOCK> source1 = this._db.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (s => s.UID == stock.Stock.Uid)).Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (s => s.STOCK), (Expression<Func<GOODS_STOCK, Decimal>>) (s => s.STOCK + stock.QuantityChange.ToDbDecimal())).Set<GOODS_STOCK, bool>((Expression<Func<GOODS_STOCK, bool>>) (s => s.IS_DELETED), false);
        IUpdatable<GOODS> source2 = (IUpdatable<GOODS>) null;
        if (!GlobalData.IsMarket5ImportAcitve && doc.Type != GlobalDictionaries.DocumentsTypes.Buy)
          source2 = this._db.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (s => s.UID == stock.Stock.GoodUid)).Set<GOODS, bool>((Expression<Func<GOODS, bool>>) (s => s.IS_DELETED), false);
        string message = string.Format("Для остатка {0} изменено кол-во на {1}", (object) stock.Stock.Uid, (object) stock.QuantityChange);
        if (doc.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Buy, GlobalDictionaries.DocumentsTypes.MoveStorageChild))
        {
          source1 = source1.Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (s => s.PRICE), stock.Stock.Price.ToDbDecimal());
          message += string.Format(", цена на {0}", (object) stock.Stock.Price);
        }
        if (doc.Type == GlobalDictionaries.DocumentsTypes.Buy && !GlobalData.IsMarket5ImportAcitve)
        {
          switch (waybill.SaveDeletedGoodVariant)
          {
            case WaybillConfig.SaveDeletedGoodVariants.AllRecover:
              source2 = this._db.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (s => s.UID == stock.Stock.GoodUid)).Set<GOODS, bool>((Expression<Func<GOODS, bool>>) (s => s.IS_DELETED), false);
              break;
            case WaybillConfig.SaveDeletedGoodVariants.NoNullRecover:
              source2 = this._db.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (s => s.UID == stock.Stock.GoodUid && stock.Stock.Stock + stock.QuantityChange > 0M)).Set<GOODS, bool>((Expression<Func<GOODS, bool>>) (s => s.IS_DELETED), false);
              break;
          }
        }
        LogHelper.Trace(message);
        source1.Update<GOODS_STOCK>();
        if (source2 != null)
          source2.Update<GOODS>();
      }
    }

    public bool Save(Document item) => this.SaveDocument(item);

    public int Save(List<Document> itemsList)
    {
      return itemsList.Count<Document>(new Func<Document, bool>(this.Save));
    }

    public static event DocumentsRepository.SaveHandler DocumentSaved;

    public interface IFilter
    {
    }

    public class CommonFilter : DocumentsRepository.IFilter
    {
      public DateTime DateStart { get; set; } = new DateTime(2017, 1, 1);

      public DateTime DateEnd { get; set; } = DateTime.Now;

      public bool IncludeDeleted { get; set; } = true;

      public GlobalDictionaries.DocumentsTypes[] Types { get; set; }

      public bool IgnoreTime { get; set; }

      public Guid ContractorUid { get; set; } = Guid.Empty;

      public Guid GoodUid { get; set; } = Guid.Empty;
    }

    public class SingleDocFilter : DocumentsRepository.IFilter
    {
      public Guid DocumentUid { get; set; } = Guid.Empty;
    }

    private class QueryResult
    {
      public DOCUMENT_ITEMS Di { get; set; }

      public GOODS_STOCK St { get; set; }

      public STORAGES Storage { get; set; }
    }

    public class StockChange
    {
      public GoodsStocks.GoodStock Stock { get; set; }

      public Decimal QuantityChange { get; set; }
    }

    private class GoodsRePricer
    {
      private Dictionary<Guid, Decimal> _list = new Dictionary<Guid, Decimal>();
      private Document _doc;
      private Gbs.Core.Db.DataBase _db;

      public GoodsRePricer(Dictionary<Guid, Decimal> list, Document doc, Gbs.Core.Db.DataBase db)
      {
        this._list = list;
        this._doc = doc;
        this._db = db;
      }

      private void Do()
      {
        LogHelper.OnBegin();
        int num = 0;
        foreach (KeyValuePair<Guid, Decimal> keyValuePair in this._list)
        {
          KeyValuePair<Guid, Decimal> g = keyValuePair;
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          IQueryable<GOODS_STOCK> source = this._db.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.UID == g.Key && x.IS_DELETED == false)).SelectMany<GOODS, GOODS_STOCK, GOODS_STOCK>(System.Linq.Expressions.Expression.Lambda<Func<GOODS, IEnumerable<GOODS_STOCK>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
          {
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(this._db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<GOODS_STOCK, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.GOOD_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
          }), parameterExpression1), (Expression<Func<GOODS, GOODS_STOCK, GOODS_STOCK>>) ((good, s) => s)).Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.STORAGE_UID == this._doc.Storage.Uid && x.IS_DELETED == false && x.PRICE != g.Value));
          if (source.Any<GOODS_STOCK>())
          {
            LogHelper.Trace(string.Format("Переоценка товара UID: {0}; price: {1:N2}", (object) g.Key, (object) g.Value));
            ++num;
            source.Set<GOODS_STOCK, Decimal>((Expression<Func<GOODS_STOCK, Decimal>>) (x => x.PRICE), g.Value).Update<GOODS_STOCK>();
          }
        }
        if (num == 0)
          return;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Text = string.Format(Translate.GoodsRePricer_Do_По_накладной_переоценено__0__товаров, (object) num)
        });
        CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
        CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.CafeMenu);
        LogHelper.OnEnd();
      }

      public void RePrice()
      {
        this._doc.Saved -= new Document.SaveHandler(this.RePrice);
        this.Do();
      }
    }

    public delegate void SaveHandler(Document item);
  }
}
