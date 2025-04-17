// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Clients.ClientsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Payments;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Core.Entities.Clients
{
  public class ClientsRepository : IEntityRepository<Client, CLIENTS>
  {
    private readonly Gbs.Core.Db.DataBase _db;

    public bool MultiThreadMode { get; set; } = true;

    public ClientsRepository(Gbs.Core.Db.DataBase db) => this._db = db;

    public ClientsRepository()
    {
    }

    public List<Client> GetByQuery(IQueryable<CLIENTS> query)
    {
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрузка контактов");
      List<EntityProperties.PropertyValue> pl = new List<EntityProperties.PropertyValue>();
      List<Group> clientGroups = new List<Group>();
      List<Task> list1 = new List<Task>();
      list1.Add(new Task((Action) (() =>
      {
        pl = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client);
        clientGroups = new GroupRepository().GetAllItems();
      })));
      List<CLIENTS> lc = new List<CLIENTS>();
      list1.Add(new Task((Action) (() =>
      {
        DataContext dataContext = Data.GetDataContext();
        if (query == null)
          query = (IQueryable<CLIENTS>) dataContext.GetTable<CLIENTS>();
        lc = dataContext.GetQuery<CLIENTS>(query).ToList<CLIENTS>();
      })));
      list1.RunList(this.MultiThreadMode);
      performancer.AddPoint("Ожидание задач");
      List<Client> list2 = lc.Join<CLIENTS, Group, Guid, Client>((IEnumerable<Group>) clientGroups, (Func<CLIENTS, Guid>) (c => c.GROUP_UID), (Func<Group, Guid>) (cg => cg.Uid), (Func<CLIENTS, Group, Client>) ((c, cg) => new Client(c)
      {
        Group = cg
      })).AsParallel<Client>().ToList<Client>().GroupJoin<Client, EntityProperties.PropertyValue, Guid, Client>((IEnumerable<EntityProperties.PropertyValue>) pl, (Func<Client, Guid>) (c => c.Uid), (Func<EntityProperties.PropertyValue, Guid>) (p => p.EntityUid), (Func<Client, IEnumerable<EntityProperties.PropertyValue>, Client>) ((c, p) =>
      {
        c.Properties = p.ToList<EntityProperties.PropertyValue>();
        return c;
      })).AsParallel<Client>().ToList<Client>();
      ClientsRepository.GetPropertyDic((IEnumerable<Client>) list2);
      performancer.Stop();
      LogHelper.OnEnd();
      return list2;
    }

    public List<Client> GetAllItems()
    {
      return this.GetByQuery((IQueryable<CLIENTS>) Data.GetDataContext().GetTable<CLIENTS>());
    }

    public List<Client> GetActiveItems()
    {
      return this.GetByQuery(Data.GetDataContext().GetTable<CLIENTS>().Where<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => !x.IS_DELETED)));
    }

    public void RemoveClient(List<Client> clients, Guid uid)
    {
      try
      {
        LogHelper.OnBegin();
        foreach (Client client1 in clients)
        {
          Client client = client1;
          List<DOCUMENTS> list1 = this._db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.CONTRACTOR_UID == client.Uid)).ToList<DOCUMENTS>();
          list1.ForEach((Action<DOCUMENTS>) (x => x.CONTRACTOR_UID = uid));
          list1.ForEach((Action<DOCUMENTS>) (x => this._db.InsertOrReplace<DOCUMENTS>(x)));
          List<PAYMENTS> list2 = this._db.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.PAYER_UID == client.Uid)).ToList<PAYMENTS>();
          list2.ForEach((Action<PAYMENTS>) (x => x.PAYER_UID = uid));
          list2.ForEach((Action<PAYMENTS>) (x => this._db.InsertOrReplace<PAYMENTS>(x)));
          List<USERS> list3 = this._db.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => x.CLIENT_UID == client.Uid)).ToList<USERS>();
          list3.ForEach((Action<USERS>) (x => x.CLIENT_UID = uid));
          list3.ForEach((Action<USERS>) (x => this._db.InsertOrReplace<USERS>(x)));
          client.IsDeleted = true;
        }
        this.Save(clients);
        LogHelper.OnEnd();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось объеденить контакты");
      }
    }

    public IEnumerable<Client> GetClientsForUser()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ClientsRepository.\u003C\u003Ec__DisplayClass11_0 cDisplayClass110 = new ClientsRepository.\u003C\u003Ec__DisplayClass11_0();
      DataContext dataContext = Data.GetDataContext();
      IQueryable<CLIENTS> source = dataContext.GetTable<CLIENTS>().Where<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => !x.IS_DELETED));
      IQueryable<Guid> queryable = dataContext.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED && !x.IS_KICKED)).Select<USERS, Guid>((Expression<Func<USERS, Guid>>) (x => x.CLIENT_UID));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.userList = queryable;
      ParameterExpression parameterExpression1;
      ParameterExpression parameterExpression2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      return (IEnumerable<Client>) this.GetByQuery(source.Where<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => cDisplayClass110.userList.Any<Guid>(Expression.Lambda<Func<Guid, bool>>((Expression) Expression.Equal(u, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (CLIENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)))));
    }

    public Client GetByUid(Guid uid)
    {
      LogHelper.OnBegin();
      List<EntityProperties.PropertyValue> list = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).ToList<EntityProperties.PropertyValue>();
      CLIENTS query = Data.GetDataContext().GetTable<CLIENTS>().FirstOrDefault<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.UID == uid));
      if (query == null)
        return (Client) null;
      Group byUid1 = new GroupRepository(this._db).GetByUid(query.GROUP_UID);
      Client byUid2 = new Client(query);
      byUid2.Properties = list.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.EntityUid == query.UID)).ToList<EntityProperties.PropertyValue>();
      byUid2.Group = byUid1;
      LogHelper.OnEnd();
      return byUid2;
    }

    public Client GetByBarcode(string barcode)
    {
      return this.GetByQuery(Data.GetDataContext().GetTable<CLIENTS>().Where<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.BARCODE == barcode && !x.IS_DELETED))).FirstOrDefault<Client>();
    }

    public bool Save(Client item) => this.Save(item, true);

    public bool Save(Client item, bool isWriteJson)
    {
      LogHelper.OnBegin();
      if (!DevelopersHelper.IsUnitTest() && isWriteJson)
        new HomeOfficeHelper().PrepareAndSend<Client>(item, HomeOfficeHelper.EntityEditHome.Client);
      if (item == null)
        return false;
      Client client = item;
      DateTime? birthday = client.Birthday;
      DateTime dateTime = birthday.GetValueOrDefault();
      if (!birthday.HasValue)
      {
        dateTime = new DateTime(1, 1, 1);
        client.Birthday = new DateTime?(dateTime);
      }
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      DataContext dataContext = Data.GetDataContext();
      CLIENTS clients = new CLIENTS();
      clients.UID = item.Uid;
      clients.IS_DELETED = item.IsDeleted;
      clients.NAME = item.Name;
      birthday = item.Birthday;
      clients.BIRTHDAY = birthday ?? new DateTime(1, 1, 1);
      clients.GROUP_UID = item.Group.Uid;
      clients.COMMENT = item.Comment;
      clients.BARCODE = item.Barcode;
      clients.ADDRESS = item.Address;
      clients.EMAIL = item.Email;
      clients.PHONE = item.Phone;
      clients.DATE_ADD = item.DateAdd;
      dataContext.InsertOrReplace<CLIENTS>(clients);
      dataContext.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == item.Uid));
      int num = item.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Value != null && !p.Value.ToString().IsNullOrEmpty())).All<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p =>
      {
        p.EntityUid = item.Uid;
        return p.Save();
      })) ? 1 : 0;
      LogHelper.OnEnd();
      return num != 0;
    }

    public int Save(List<Client> itemsList)
    {
      return itemsList.Count<Client>(new Func<Client, bool>(this.Save));
    }

    public int Delete(List<Client> itemsList)
    {
      foreach (Entity items in itemsList)
        items.IsDeleted = true;
      return this.Save(itemsList);
    }

    public bool Delete(Client item)
    {
      item.IsDeleted = true;
      return this.Save(item);
    }

    public ActionResult Validate(Client item)
    {
      ActionResult actionResult1 = ValidationHelper.DataValidation((Entity) item);
      if (actionResult1.Result != ActionResult.Results.Ok)
        return actionResult1;
      foreach (Entity property in item.Properties)
      {
        ActionResult actionResult2 = ValidationHelper.DataValidation(property);
        if (actionResult2.Result != ActionResult.Results.Ok)
          return actionResult2;
      }
      return new ActionResult(ActionResult.Results.Ok);
    }

    private static void GetPropertyDic(IEnumerable<Client> r1)
    {
      List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client);
      foreach (Client client in r1)
      {
        Dictionary<Guid, object> d = client.Properties.ToDictionary<EntityProperties.PropertyValue, Guid, object>((Func<EntityProperties.PropertyValue, Guid>) (y => y.Type.Uid), (Func<EntityProperties.PropertyValue, object>) (z => z.Value));
        foreach (EntityProperties.PropertyType propertyType in typesList.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (t => !d.Any<KeyValuePair<Guid, object>>((Func<KeyValuePair<Guid, object>, bool>) (di => di.Key == t.Uid)))))
          d.Add(propertyType.Uid, (object) null);
        client.PropertiesDictionary = d;
      }
    }

    public IEnumerable<ClientAdnSum> GetListActiveItemAndSum()
    {
      Bonuses settingBonuses = new Bonuses();
      settingBonuses.Load();
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрукза активных клиентов с суммами");
      DataContext dataContext = Data.GetDataContext();
      List<Document> docs2 = new DocumentsRepository(this._db).GetByQuery(dataContext.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.CONTRACTOR_UID != Guid.Empty && (x.TYPE == 1 || x.TYPE == 2) && x.IS_DELETED == false))).ToList<Document>();
      performancer.AddPoint("Документы");
      List<Gbs.Core.Entities.Payments.Payment> payments = Gbs.Core.Entities.Payments.GetPaymentsList(dataContext.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.TYPE == 6 || x.TYPE == 5)));
      performancer.AddPoint("Платежи");
      List<ClientAdnSum> list1 = this.GetAllItems().Select<Client, ClientAdnSum>((Func<Client, ClientAdnSum>) (x => new ClientAdnSum()
      {
        Client = x
      })).ToList<ClientAdnSum>();
      performancer.AddPoint("Контакты");
      list1.Where<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => !x.Client.Group.IsNonUseBonus)).ToList<ClientAdnSum>().ForEach((Action<ClientAdnSum>) (x => x.CurrentBonusSum += payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Client != null && p.Client.Uid == x.Client.Uid)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumOut - p.SumIn))));
      performancer.AddPoint("Сумма баллов");
      Dictionary<Guid, List<Document>> docsGroupedByContractor = docs2.GroupBy<Document, Guid>((Func<Document, Guid>) (x => x.ContractorUid)).ToDictionary<IGrouping<Guid, Document>, Guid, List<Document>>((Func<IGrouping<Guid, Document>, Guid>) (g => g.Key), (Func<IGrouping<Guid, Document>, List<Document>>) (g => g.ToList<Document>()));
      List<CreditListViewModel.CreditItem> creditsCache = CacheHelper.Get<List<CreditListViewModel.CreditItem>>(CacheHelper.CacheTypes.ClientsCredits, new Func<List<CreditListViewModel.CreditItem>>(CreditListViewModel.UpdateCreditsCache));
      Parallel.ForEach<ClientAdnSum>((IEnumerable<ClientAdnSum>) list1, (Action<ClientAdnSum>) (clientAdnSum =>
      {
        List<Document> documentList;
        if (!docsGroupedByContractor.TryGetValue(clientAdnSum.Client.Uid, out documentList))
          return;
        foreach (Document doc in documentList)
          this.EditSumForClient(doc, clientAdnSum, docs2, settingBonuses);
        clientAdnSum.CurrentCreditSum = creditsCache.Where<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, bool>) (x => x.Client.Uid == clientAdnSum.Client.Uid)).Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit));
      }));
      performancer.AddPoint("Цикл");
      List<ClientAdnSum> list2 = list1.Where<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => !x.Client.IsDeleted)).ToList<ClientAdnSum>();
      if (list2.Any<ClientAdnSum>() && new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Clients.SyncMode == GlobalDictionaries.ClientSyncModes.FileSync)
      {
        ClientsExchangeHelper.GetCashClient();
        performancer.AddPoint("Загрзука данных покупателей из облака");
        foreach (ClientAdnSum clientAdnSum in list2)
        {
          List<ClientCloud> clientByData = ClientsExchangeHelper.GetClientByData(clientAdnSum.Client);
          if (clientByData.Any<ClientCloud>())
          {
            clientAdnSum.CloudBonusSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.Bonuses));
            clientAdnSum.CloudSalesSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.SalesSum));
            clientAdnSum.CloudCreditSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.CreditSum));
          }
        }
        performancer.AddPoint("Получение данных о покупателей из облака");
      }
      performancer.Stop();
      LogHelper.OnEnd();
      return list2.AsEnumerable<ClientAdnSum>();
    }

    public ClientAdnSum GetClientByUidAndSum(Guid uid)
    {
      LogHelper.OnBegin();
      Bonuses settingBonuses = new Bonuses();
      settingBonuses.Load();
      Client byUid = this.GetByUid(uid);
      if (byUid == null)
        return (ClientAdnSum) null;
      ClientAdnSum client = new ClientAdnSum()
      {
        Client = byUid
      };
      List<Document> list = new DocumentsRepository(this._db).GetItemsForContractor(uid).Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale || x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)).ToList<Document>();
      List<Gbs.Core.Entities.Payments.Payment> paymentsList = Gbs.Core.Entities.Payments.GetPaymentsList(Data.GetDataContext().GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => (x.TYPE == 6 || x.TYPE == 5) && x.PAYER_UID == uid)));
      foreach (Document doc in list)
        this.EditSumForClient(doc, client, list, settingBonuses);
      IEnumerable<CreditListViewModel.CreditItem> source = CacheHelper.Get<List<CreditListViewModel.CreditItem>>(CacheHelper.CacheTypes.ClientsCredits, new Func<List<CreditListViewModel.CreditItem>>(CreditListViewModel.UpdateCreditsCache)).Where<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, bool>) (x => x.Client.Uid == uid));
      client.CurrentCreditSum = source.Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit));
      if (!client.Client.Group.IsNonUseBonus)
      {
        foreach (Gbs.Core.Entities.Payments.Payment payment in paymentsList)
        {
          if (!client.Client.Group.IsNonUseBonus)
          {
            client.CurrentBonusSum += -payment.SumIn;
            if (settingBonuses.ValidityPeriodBonuses == -1 || Math.Truncate((DateTime.Now - payment.Date).TotalDays) <= (double) settingBonuses.ValidityPeriodBonuses)
              client.CurrentBonusSum += payment.SumOut;
          }
        }
      }
      ClientsExchangeHelper.GetCashClient();
      List<ClientCloud> clientByData = ClientsExchangeHelper.GetClientByData(client.Client);
      if (clientByData.Any<ClientCloud>())
      {
        client.CloudBonusSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.Bonuses));
        client.CloudSalesSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.SalesSum));
        client.CloudCreditSum += clientByData.Sum<ClientCloud>((Func<ClientCloud, Decimal>) (x => x.CreditSum));
      }
      LogHelper.OnEnd();
      return client;
    }

    private void EditSumForClient(
      Document doc,
      ClientAdnSum client,
      List<Document> documents,
      Bonuses settingBonuses)
    {
      if (client == null || doc == null)
        return;
      switch (doc.Type)
      {
        case GlobalDictionaries.DocumentsTypes.Sale:
          client.CurrentSalesSum += doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => (i.SellPrice - i.SellPrice * (i.Discount / 100M)) * i.Quantity));
          if (client.Client.Group.IsNonUseBonus)
            break;
          client.CurrentBonusSum += doc.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => -x.SumIn));
          if (settingBonuses.ValidityPeriodBonuses != -1 && Math.Truncate((DateTime.Now - doc.DateTime).TotalDays) > (double) settingBonuses.ValidityPeriodBonuses)
            break;
          client.CurrentBonusSum += doc.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
          break;
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
          client.CurrentSalesSum -= doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => (i.SellPrice - i.SellPrice * (i.Discount / 100M)) * i.Quantity));
          if (client.Client.Group.IsNonUseBonus)
            break;
          client.CurrentBonusSum -= doc.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
          break;
      }
    }

    public static Decimal GetCreditSumForDocuments(
      Document doc,
      List<Document> documents,
      bool isTotalCredit = false)
    {
      Decimal num1 = doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(i.Quantity, i.SellPrice, i.Discount))));
      List<Document> list1 = documents.Where<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Uid && !x.IsDeleted)).ToList<Document>();
      Decimal num2 = list1.Sum<Document>((Func<Document, Decimal>) (x => x.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => !i.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(i.Quantity, i.SellPrice, i.Discount))))));
      List<Gbs.Core.Entities.Payments.Payment> list2 = doc.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p =>
      {
        if (p.IsDeleted)
          return false;
        return !p.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesCorrection, GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment);
      })).ToList<Gbs.Core.Entities.Payments.Payment>();
      Decimal num3 = isTotalCredit ? list2.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (d => Math.Abs((d.Date - doc.DateTime).TotalMilliseconds) < 60000.0)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (d => d.SumIn)) + list1.Sum<Document>((Func<Document, Decimal>) (x => x.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (d => !d.IsDeleted && Math.Abs((d.Date - doc.DateTime).TotalMilliseconds) < 60000.0)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (d => d.SumIn)))) : list2.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn)) + list1.Sum<Document>((Func<Document, Decimal>) (x => x.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => !p.IsDeleted)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn))));
      Decimal num4 = list2.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumOut)) + list1.Sum<Document>((Func<Document, Decimal>) (x => x.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => !p.IsDeleted)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumOut))));
      Decimal num5 = num2;
      return num1 - num5 - num3 + num4;
    }
  }
}
