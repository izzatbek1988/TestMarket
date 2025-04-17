// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Payments
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using LinqToDB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class Payments
  {
    public static List<Gbs.Core.Entities.Payments.Payment> GetPaymentsList(
      IQueryable<PAYMENTS> query = null,
      bool MultiThreadMode = true)
    {
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрукза списка платежей из БД");
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<PAYMENTS>().AsQueryable<PAYMENTS>();
        List<Task> list1 = new List<Task>();
        List<PaymentsAccounts.PaymentsAccount> ac = new List<PaymentsAccounts.PaymentsAccount>();
        list1.Add(new Task((Action) (() => ac = PaymentsAccounts.GetPaymentsAccountsList())));
        List<Users.User> us = new List<Users.User>();
        list1.Add(new Task((Action) (() => us = new UsersRepository().GetAllItems())));
        List<PaymentMethods.PaymentMethod> m = new List<PaymentMethods.PaymentMethod>();
        list1.Add(new Task((Action) (() => m = PaymentMethods.GetActionPaymentsList())));
        List<Client> cl = new List<Client>();
        list1.Add(new Task((Action) (() =>
        {
          DataContext dataContext = Data.GetDataContext();
          IQueryable<PAYMENTS> query1 = dataContext.GetQuery<PAYMENTS>(query);
          cl = new ClientsRepository().GetByQuery(dataContext.GetTable<CLIENTS>().Join<CLIENTS, PAYMENTS, Guid, CLIENTS>((IEnumerable<PAYMENTS>) query1, (Expression<Func<CLIENTS, Guid>>) (c => c.UID), (Expression<Func<PAYMENTS, Guid>>) (q => q.PAYER_UID), (Expression<Func<CLIENTS, PAYMENTS, CLIENTS>>) ((c, q) => c)));
        })));
        List<Sections.Section> sections = new List<Sections.Section>();
        list1.Add(new Task((Action) (() => sections = Sections.GetSectionsList())));
        List<PAYMENTS> lp = new List<PAYMENTS>();
        list1.Add(new Task((Action) (() => lp = Data.GetDataContext().GetQuery<PAYMENTS>(query).ToList<PAYMENTS>())));
        list1.RunList(MultiThreadMode);
        performancer.AddPoint("Ожидание задач");
        Dictionary<Guid, PaymentsAccounts.PaymentsAccount> accountDict = ac.ToDictionary<PaymentsAccounts.PaymentsAccount, Guid>((Func<PaymentsAccounts.PaymentsAccount, Guid>) (x => x.Uid));
        Dictionary<Guid, PaymentMethods.PaymentMethod> methodDict = m.ToDictionary<PaymentMethods.PaymentMethod, Guid>((Func<PaymentMethods.PaymentMethod, Guid>) (x => x.Uid));
        Dictionary<Guid, Sections.Section> sectionDict = sections.ToDictionary<Sections.Section, Guid>((Func<Sections.Section, Guid>) (x => x.Uid));
        Dictionary<Guid, Users.User> userDict = us.ToDictionary<Users.User, Guid>((Func<Users.User, Guid>) (x => x.Uid));
        Dictionary<Guid, Client> clientDict = cl.GroupBy<Client, Guid>((Func<Client, Guid>) (x => x.Uid)).ToDictionary<IGrouping<Guid, Client>, Guid, Client>((Func<IGrouping<Guid, Client>, Guid>) (g => g.Key), (Func<IGrouping<Guid, Client>, Client>) (g => g.First<Client>()));
        ConcurrentBag<Gbs.Core.Entities.Payments.Payment> payments = new ConcurrentBag<Gbs.Core.Entities.Payments.Payment>();
        Parallel.ForEach<PAYMENTS>((IEnumerable<PAYMENTS>) lp, (Action<PAYMENTS>) (p =>
        {
          Client client;
          PaymentsAccounts.PaymentsAccount paymentsAccount1;
          PaymentsAccounts.PaymentsAccount paymentsAccount2;
          PaymentMethods.PaymentMethod paymentMethod;
          Sections.Section section;
          Users.User user;
          payments.Add(new Gbs.Core.Entities.Payments.Payment()
          {
            Uid = p.UID,
            Date = p.DATE_TIME,
            Comment = p.COMMENT,
            IsDeleted = p.IS_DELETED,
            ParentUid = p.PARENT_UID,
            SumIn = p.SUM_IN,
            SumOut = p.SUM_OUT,
            Type = (GlobalDictionaries.PaymentTypes) p.TYPE,
            Client = clientDict.TryGetValue(p.PAYER_UID, out client) ? client : (Client) null,
            AccountIn = p.ACCOUNT_IN_UID == Guid.Empty ? (PaymentsAccounts.PaymentsAccount) null : (accountDict.TryGetValue(p.ACCOUNT_IN_UID, out paymentsAccount1) ? paymentsAccount1 : (PaymentsAccounts.PaymentsAccount) null),
            AccountOut = p.ACCOUNT_OUT_UID == Guid.Empty ? (PaymentsAccounts.PaymentsAccount) null : (accountDict.TryGetValue(p.ACCOUNT_OUT_UID, out paymentsAccount2) ? paymentsAccount2 : (PaymentsAccounts.PaymentsAccount) null),
            Method = p.METHOD_UID == Guid.Empty ? (PaymentMethods.PaymentMethod) null : (methodDict.TryGetValue(p.METHOD_UID, out paymentMethod) ? paymentMethod : (PaymentMethods.PaymentMethod) null),
            Section = p.SECTION_UID == Guid.Empty ? (Sections.Section) null : (sectionDict.TryGetValue(p.SECTION_UID, out section) ? section : (Sections.Section) null),
            User = p.USER_UID == Guid.Empty ? (Users.User) null : (userDict.TryGetValue(p.USER_UID, out user) ? user : (Users.User) null)
          });
        }));
        List<Gbs.Core.Entities.Payments.Payment> list2 = payments.ToList<Gbs.Core.Entities.Payments.Payment>();
        performancer.Stop();
        LogHelper.OnEnd();
        return list2;
      }
    }

    public static IEnumerable<Gbs.Core.Entities.Payments.Payment> GetItemPaymentNoLinqPf(
      DateTime dateStart,
      bool includeDeleted = false)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Gbs.Core.Entities.Payments.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10 = new Gbs.Core.Entities.Payments.\u003C\u003Ec__DisplayClass1_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass10.dateStart = dateStart;
      using (DataBase dataBase = Data.GetDataBase())
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass10.linqSale = dataBase.GetTable<LINK_ENTITIES>();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        IQueryable<PAYMENTS> queryable = dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => cDisplayClass10.linqSale.Any<LINK_ENTITIES>(Expression.Lambda<Func<LINK_ENTITIES, bool>>((Expression) Expression.AndAlso((Expression) Expression.Equal(l.ENTITY_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (LINK_ENTITIES.get_ID))), (Expression) Expression.Constant((object) 0, typeof (int)))), parameterExpression2)) && x.DATE_TIME.Date >= cDisplayClass10.dateStart.Date));
        if (!includeDeleted)
          queryable = queryable.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.IS_DELETED == false));
        return (IEnumerable<Gbs.Core.Entities.Payments.Payment>) Gbs.Core.Entities.Payments.GetPaymentsList(queryable);
      }
    }

    public class Payment : Gbs.Core.Entities.Entity
    {
      [Range(-999999999.999, 999999999.999)]
      public Decimal SumIn { get; set; }

      [Range(-999999999.999, 999999999.999)]
      public Decimal SumOut { get; set; }

      [Required]
      public GlobalDictionaries.PaymentTypes Type { get; set; }

      public Guid ParentUid { get; set; } = Guid.Empty;

      [Required]
      public DateTime Date { get; set; } = DateTime.Now;

      public PaymentsAccounts.PaymentsAccount AccountOut { get; set; }

      public PaymentsAccounts.PaymentsAccount AccountIn { get; set; }

      [StringLength(100)]
      public string Comment { get; set; } = string.Empty;

      public Client Client { get; set; }

      public PaymentMethods.PaymentMethod Method { get; set; }

      public Users.User User { get; set; }

      public Sections.Section Section { get; set; }

      public bool IsFiscal { get; set; }

      public Payment()
      {
      }

      public Payment(Gbs.Helpers.HomeOffice.Entity.Payment item)
      {
        this.IsDeleted = item.IsDeleted;
        this.Uid = item.Uid;
        this.SumIn = item.SumIn;
        this.SumOut = item.SumOut;
        this.Type = item.Type;
        this.ParentUid = item.ParentUid;
        this.Date = item.Date;
        PaymentsAccounts.PaymentsAccount paymentsAccount1;
        if (!(item.AccountOutUid == Guid.Empty))
        {
          PaymentsAccounts.PaymentsAccount paymentsAccount2 = new PaymentsAccounts.PaymentsAccount();
          paymentsAccount2.Uid = item.AccountOutUid;
          paymentsAccount1 = paymentsAccount2;
        }
        else
          paymentsAccount1 = (PaymentsAccounts.PaymentsAccount) null;
        this.AccountOut = paymentsAccount1;
        this.Comment = item.Comment;
        PaymentsAccounts.PaymentsAccount paymentsAccount3;
        if (!(item.AccountInUid == Guid.Empty))
        {
          PaymentsAccounts.PaymentsAccount paymentsAccount4 = new PaymentsAccounts.PaymentsAccount();
          paymentsAccount4.Uid = item.AccountInUid;
          paymentsAccount3 = paymentsAccount4;
        }
        else
          paymentsAccount3 = (PaymentsAccounts.PaymentsAccount) null;
        this.AccountIn = paymentsAccount3;
        Client client1;
        if (!(item.ClientUid == Guid.Empty))
        {
          Client client2 = new Client();
          client2.Uid = item.ClientUid;
          client1 = client2;
        }
        else
          client1 = (Client) null;
        this.Client = client1;
        PaymentMethods.PaymentMethod paymentMethod1;
        if (!(item.MethodUid == Guid.Empty))
        {
          PaymentMethods.PaymentMethod paymentMethod2 = new PaymentMethods.PaymentMethod();
          paymentMethod2.Uid = item.MethodUid;
          paymentMethod1 = paymentMethod2;
        }
        else
          paymentMethod1 = (PaymentMethods.PaymentMethod) null;
        this.Method = paymentMethod1;
        Users.User user1;
        if (!(item.UserUid == Guid.Empty))
        {
          Users.User user2 = new Users.User();
          user2.Uid = item.UserUid;
          user1 = user2;
        }
        else
          user1 = (Users.User) null;
        this.User = user1;
        this.IsFiscal = item.IsFiscal;
      }

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase1 = Data.GetDataBase())
        {
          DataBase dataBase2 = dataBase1;
          PAYMENTS payments = new PAYMENTS();
          payments.UID = this.Uid;
          PaymentMethods.PaymentMethod method = this.Method;
          // ISSUE: explicit non-virtual call
          payments.METHOD_UID = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
          PaymentsAccounts.PaymentsAccount accountIn = this.AccountIn;
          // ISSUE: explicit non-virtual call
          payments.ACCOUNT_IN_UID = accountIn != null ? __nonvirtual (accountIn.Uid) : Guid.Empty;
          PaymentsAccounts.PaymentsAccount accountOut = this.AccountOut;
          // ISSUE: explicit non-virtual call
          payments.ACCOUNT_OUT_UID = accountOut != null ? __nonvirtual (accountOut.Uid) : Guid.Empty;
          payments.DATE_TIME = this.Date;
          payments.PARENT_UID = this.ParentUid;
          payments.TYPE = (int) this.Type;
          payments.SECTION_UID = Sections.GetCurrentSection().Uid;
          payments.SUM_IN = Math.Round(this.SumIn, 4, MidpointRounding.AwayFromZero);
          payments.SUM_OUT = Math.Round(this.SumOut, 4, MidpointRounding.AwayFromZero);
          Users.User user = this.User;
          // ISSUE: explicit non-virtual call
          payments.USER_UID = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
          payments.COMMENT = this.Comment;
          Client client = this.Client;
          // ISSUE: explicit non-virtual call
          payments.PAYER_UID = client != null ? __nonvirtual (client.Uid) : Guid.Empty;
          payments.IS_DELETED = this.IsDeleted;
          dataBase2.InsertOrReplace<PAYMENTS>(payments);
          return true;
        }
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();
    }
  }
}
