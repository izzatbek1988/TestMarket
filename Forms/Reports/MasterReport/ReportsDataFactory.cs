// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.MasterReport.ReportsDataFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms.Reports.SummaryReport.Other;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Forms.Reports.MasterReport
{
  public class ReportsDataFactory
  {
    private readonly ReportsDataFactory.ReportFilter _filer;

    public ReportsDataFactory(ReportsDataFactory.ReportFilter filter) => this._filer = filter;

    public (List<Gbs.Core.Entities.Documents.Item> items, List<Document> documents) GetDataForSaleReport()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        List<Document> saleDocument = documentsRepository.GetItemsWithFilter(this._filer.StartDateTime, this._filer.EndDateTime, GlobalDictionaries.DocumentsTypes.Sale, false).Where<Document>((Func<Document, bool>) (x => this._filer.Storages.Any<Storages.Storage>((Func<Storages.Storage, bool>) (st => x.Storage.Uid == st.Uid)))).ToList<Document>();
        List<Document> list1 = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.SaleReturn).Where<Document>((Func<Document, bool>) (x => saleDocument.Any<Document>((Func<Document, bool>) (s => s.Uid == x.ParentUid)))).Where<Document>((Func<Document, bool>) (x => this._filer.Storages.Any<Storages.Storage>((Func<Storages.Storage, bool>) (st => x.Storage.Uid == st.Uid)))).ToList<Document>();
        IEnumerable<Document> source = saleDocument.Concat<Document>((IEnumerable<Document>) list1);
        List<Document> waybills = new List<Document>();
        if (this._filer.Supplier != null)
          waybills = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).Where<Document>((Func<Document, bool>) (x => x.ContractorUid == this._filer.Supplier.Uid)).ToList<Document>();
        List<Gbs.Core.Entities.Documents.Item> items = saleDocument.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Where<Gbs.Core.Entities.Documents.Item>(new Func<Gbs.Core.Entities.Documents.Item, bool>(ItemsPredicate)).ToList<Gbs.Core.Entities.Documents.Item>();
        List<Gbs.Core.Entities.Documents.Item> list2 = list1.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Where<Gbs.Core.Entities.Documents.Item>(new Func<Gbs.Core.Entities.Documents.Item, bool>(ItemsPredicate)).Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (s => s.GoodUid == x.GoodUid)))).ToList<Gbs.Core.Entities.Documents.Item>();
        list2.ForEach((Action<Gbs.Core.Entities.Documents.Item>) (x => x.Quantity *= -1M));
        items.AddRange((IEnumerable<Gbs.Core.Entities.Documents.Item>) list2);
        return (items.ToList<Gbs.Core.Entities.Documents.Item>(), source.ToList<Document>());

        bool ItemsPredicate(Gbs.Core.Entities.Documents.Item x)
        {
          bool flag = !x.IsDeleted && this._filer.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Good.Group.Uid));
          if (!flag)
            return false;
          if (this._filer.Supplier != null)
            flag &= waybills.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (i => (IEnumerable<Gbs.Core.Entities.Documents.Item>) i.Items)).Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (g => x.GoodUid == g.GoodUid && !g.IsDeleted));
          return flag;
        }
      }
    }

    public (List<Gbs.Core.Entities.Goods.Good> goods, List<Document> documents) GetDataForHistoryGoodReport()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> goods = new GoodRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => !x.IS_DELETED))).Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Any<GoodsStocks.GoodStock>() && this._filer.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Group.Uid)) && x.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => !s.IsDeleted && this._filer.Storages.Any<Storages.Storage>((Func<Storages.Storage, bool>) (st => st.Uid == s.Storage.Uid)))))).ToList<Gbs.Core.Entities.Goods.Good>();
        List<Document> allDocuments = new DocumentsRepository(dataBase).GetActiveItems();
        if (this._filer.Supplier != null)
          goods = goods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => allDocuments.Any<Document>((Func<Document, bool>) (d => d.Type == GlobalDictionaries.DocumentsTypes.Buy && d.ContractorUid == this._filer.Supplier.Uid && d.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == x.Uid)))))).ToList<Gbs.Core.Entities.Goods.Good>();
        List<Document> list = allDocuments.Where<Document>((Func<Document, bool>) (x => x.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => goods.Any<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => g.Uid == i.GoodUid)))))).ToList<Document>();
        foreach (Document document in list)
          document.Items = document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => this._filer.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Good.Group.Uid)) && goods.Any<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => g.Uid == x.Good.Uid)))).ToList<Gbs.Core.Entities.Documents.Item>();
        List<Document> documentList = new List<Document>();
        foreach (Document document in list)
        {
          if (document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (it => goods.Any<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => it.GoodUid == g.Uid)))))
            documentList.Add(document);
        }
        return (goods, documentList);
      }
    }

    public List<Document> GetDataPaymentsForSupplierReport(out Decimal saldo)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter(DateTime.MinValue, this._filer.StartDateTime.Date.AddDays(-1.0), GlobalDictionaries.DocumentsTypes.Buy, false, clientGuid: this._filer.Supplier?.Uid);
        saldo = itemsWithFilter.Sum<Document>(new Func<Document, Decimal>(SaleHelper.GetBuySumDocument)) - itemsWithFilter.SelectMany<Document, Gbs.Core.Entities.Payments.Payment>((Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (x => (IEnumerable<Gbs.Core.Entities.Payments.Payment>) x.Payments)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        return new DocumentsRepository(dataBase).GetItemsWithFilter(this._filer.StartDateTime.Date, this._filer.EndDateTime.Date, GlobalDictionaries.DocumentsTypes.Buy, false, clientGuid: this._filer.Supplier?.Uid);
      }
    }

    public (List<Gbs.Core.Entities.Payments.Payment> payments, Dictionary<Guid, string> groupPayments) GetDataForPaymentMoveReport(
      out Decimal sumOld,
      out Decimal sumCurrent,
      out string accountName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReportsDataFactory.\u003C\u003Ec__DisplayClass6_0 cDisplayClass60 = new ReportsDataFactory.\u003C\u003Ec__DisplayClass6_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass60.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass60.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        IQueryable<PAYMENTS> queryable1 = cDisplayClass60.db.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED && x.TYPE != 7 && x.DATE_TIME.Date >= this._filer.StartDateTime.Date && x.DATE_TIME.Date <= this._filer.EndDateTime));
        if (this._filer.PaymentsAccount.Uid != Guid.Empty)
          queryable1 = queryable1.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.ACCOUNT_IN_UID == this._filer.PaymentsAccount.Uid || x.ACCOUNT_OUT_UID == this._filer.PaymentsAccount.Uid));
        IEnumerable<PaymentsAccounts.PaymentsAccount> source1 = PaymentsAccounts.GetPaymentsAccountsList().Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted));
        ref string local = ref accountName;
        Guid? uid = this._filer.PaymentsAccount?.Uid;
        Guid empty = Guid.Empty;
        // ISSUE: reference to a compiler-generated method
        string str1 = (uid.HasValue ? (uid.GetValueOrDefault() == empty ? 1 : 0) : 0) != 0 ? string.Format(Translate.ReportsDataFactory_GetDataForPaymentMoveReport_Все_счета___0__, (object) source1.Count<PaymentsAccounts.PaymentsAccount>()) : source1.SingleOrDefault<PaymentsAccounts.PaymentsAccount>(new Func<PaymentsAccounts.PaymentsAccount, bool>(cDisplayClass60.\u003CGetDataForPaymentMoveReport\u003Eb__3))?.Name ?? "";
        local = str1;
        List<Gbs.Core.Entities.Payments.Payment> list1 = Gbs.Core.Entities.Payments.GetPaymentsList(queryable1).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyMovement, GlobalDictionaries.PaymentTypes.MoneyPayment, GlobalDictionaries.PaymentTypes.MoneyCorrection, GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid))).ToList<Gbs.Core.Entities.Payments.Payment>();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        IQueryable<DOCUMENTS> queryable2 = queryable1.SelectMany<PAYMENTS, DOCUMENTS, DOCUMENTS>(Expression.Lambda<Func<PAYMENTS, IEnumerable<DOCUMENTS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass60.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<DOCUMENTS, bool>>((Expression) Expression.Equal(x.UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_PARENT_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
        }), parameterExpression1), (Expression<Func<PAYMENTS, DOCUMENTS, DOCUMENTS>>) ((p, doc) => doc));
        List<Gbs.Core.Entities.Payments.Payment> list2 = list1.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.MoneyMovement)).ToList<Gbs.Core.Entities.Payments.Payment>();
        foreach (Gbs.Core.Entities.Payments.Payment source2 in list1.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyMovement)).ToList<Gbs.Core.Entities.Payments.Payment>().Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyMovement)))
        {
          Gbs.Core.Entities.Payments.Payment payment1 = source2.Clone<Gbs.Core.Entities.Payments.Payment>();
          payment1.Uid = Guid.NewGuid();
          payment1.SumOut = 0M;
          list2.Add(payment1);
          Gbs.Core.Entities.Payments.Payment payment2 = source2.Clone<Gbs.Core.Entities.Payments.Payment>();
          payment2.SumIn = 0M;
          list2.Add(payment2);
        }
        // ISSUE: reference to a compiler-generated field
        List<Document> byQuery1 = new DocumentsRepository(cDisplayClass60.db).GetByQuery(queryable2);
        sumOld = PaymentAccountsAndSumViewModel.GetSumForAccount(this._filer.PaymentsAccount.Uid, new DateTime?(this._filer.StartDateTime));
        sumCurrent = PaymentAccountsAndSumViewModel.GetSumForAccount(this._filer.PaymentsAccount.Uid, new DateTime?(this._filer.EndDateTime));
        List<PaymentGroups.PaymentGroup> paymentGroupsList = PaymentGroups.GetPaymentGroupsList();
        ParameterExpression parameterExpression3;
        ParameterExpression parameterExpression4;
        ParameterExpression parameterExpression5;
        ParameterExpression parameterExpression6;
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        IQueryable<CLIENTS> query = queryable1.SelectMany<PAYMENTS, CLIENTS, CLIENTS>(Expression.Lambda<Func<PAYMENTS, IEnumerable<CLIENTS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass60.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<CLIENTS, bool>>((Expression) Expression.Equal(x.UID, (Expression) Expression.Property((Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_PAYER_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4))
        }), parameterExpression3), (Expression<Func<PAYMENTS, CLIENTS, CLIENTS>>) ((p, client) => client)).Union<CLIENTS>((IEnumerable<CLIENTS>) queryable2.SelectMany<DOCUMENTS, CLIENTS, CLIENTS>(Expression.Lambda<Func<DOCUMENTS, IEnumerable<CLIENTS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass60.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<CLIENTS, bool>>((Expression) Expression.Equal(x.UID, (Expression) Expression.Property((Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_CONTRACTOR_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression6))
        }), parameterExpression5), (Expression<Func<DOCUMENTS, CLIENTS, CLIENTS>>) ((docCl, client) => client)));
        // ISSUE: reference to a compiler-generated field
        List<Client> byQuery2 = new ClientsRepository(cDisplayClass60.db).GetByQuery(query);
        Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
        foreach (Gbs.Core.Entities.Payments.Payment payment in list2)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ReportsDataFactory.\u003C\u003Ec__DisplayClass6_1 cDisplayClass61 = new ReportsDataFactory.\u003C\u003Ec__DisplayClass6_1();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass61.payment = payment;
          string str2 = string.Empty;
          // ISSUE: reference to a compiler-generated field
          switch (cDisplayClass61.payment.Type)
          {
            case GlobalDictionaries.PaymentTypes.MoneyPayment:
              // ISSUE: reference to a compiler-generated field
              if (cDisplayClass61.payment.SumIn > 0M)
              {
                // ISSUE: reference to a compiler-generated method
                PaymentGroups.PaymentGroup paymentGroup = paymentGroupsList.SingleOrDefault<PaymentGroups.PaymentGroup>(new Func<PaymentGroups.PaymentGroup, bool>(cDisplayClass61.\u003CGetDataForPaymentMoveReport\u003Eb__17));
                str2 = Translate.ReportsDataFactory_GetDataForPaymentMoveReport_Внесение_на_счет_ + (paymentGroup != null ? "(" + paymentGroup.Name + ")" : "");
                goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass61.payment.SumOut > 0M)
                {
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ReportsDataFactory.\u003C\u003Ec__DisplayClass6_2 cDisplayClass62 = new ReportsDataFactory.\u003C\u003Ec__DisplayClass6_2();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  cDisplayClass62.doc = byQuery1.SingleOrDefault<Document>(new Func<Document, bool>(cDisplayClass61.\u003CGetDataForPaymentMoveReport\u003Eb__18));
                  // ISSUE: reference to a compiler-generated field
                  if (cDisplayClass62.doc != null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    cDisplayClass61.payment.Client = byQuery2.FirstOrDefault<Client>(new Func<Client, bool>(cDisplayClass62.\u003CGetDataForPaymentMoveReport\u003Eb__19));
                    // ISSUE: reference to a compiler-generated field
                    str2 = Translate.ReportsDataFactory_GetDataForPaymentMoveReport_ + cDisplayClass62.doc.Number;
                    goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    PaymentGroups.PaymentGroup paymentGroup = paymentGroupsList.FirstOrDefault<PaymentGroups.PaymentGroup>(new Func<PaymentGroups.PaymentGroup, bool>(cDisplayClass61.\u003CGetDataForPaymentMoveReport\u003Eb__20));
                    str2 = Translate.ReportsDataFactory_GetDataForPaymentMoveReport_Снятие_со_счета_ + (paymentGroup != null ? "(" + paymentGroup.Name + ")" : "");
                    goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
                  }
                }
                else
                  break;
              }
            case GlobalDictionaries.PaymentTypes.MoneyDocumentPayment:
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              ReportsDataFactory.\u003C\u003Ec__DisplayClass6_3 cDisplayClass63 = new ReportsDataFactory.\u003C\u003Ec__DisplayClass6_3();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              cDisplayClass63.doc = byQuery1.SingleOrDefault<Document>(new Func<Document, bool>(cDisplayClass61.\u003CGetDataForPaymentMoveReport\u003Eb__21));
              // ISSUE: reference to a compiler-generated field
              if (cDisplayClass63.doc != null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                cDisplayClass61.payment.Client = byQuery2.FirstOrDefault<Client>(new Func<Client, bool>(cDisplayClass63.\u003CGetDataForPaymentMoveReport\u003Eb__22));
                // ISSUE: reference to a compiler-generated field
                switch (cDisplayClass63.doc.Type)
                {
                  case GlobalDictionaries.DocumentsTypes.Sale:
                    // ISSUE: reference to a compiler-generated field
                    str2 = str2 + Translate.FrmMainWindow_Продажа + cDisplayClass63.doc.Number;
                    goto label_29;
                  case GlobalDictionaries.DocumentsTypes.SaleReturn:
                    // ISSUE: reference to a compiler-generated field
                    str2 = str2 + Translate.ReportsDataFactory_GetDataForPaymentMoveReport_возврат_продажи + cDisplayClass63.doc.Number;
                    goto label_29;
                  case GlobalDictionaries.DocumentsTypes.Buy:
                    // ISSUE: reference to a compiler-generated field
                    str2 = str2 + Translate.PrintableReportFactory_CreatePaymentsForSupplierReport_ + cDisplayClass63.doc.Number;
                    goto label_29;
                  default:
                    goto label_29;
                }
              }
              else
                goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
            case GlobalDictionaries.PaymentTypes.MoneyMovement:
              str2 = Translate.FrmRemoveCash_Перемещение_денежных_средств;
              goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
            case GlobalDictionaries.PaymentTypes.MoneyCorrection:
              str2 = Translate.UserGroupViewModel_Корректировка_суммы_на_счете;
              goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
            case GlobalDictionaries.PaymentTypes.BonusesDocumentPayment:
              str2 = Translate.ReportsDataFactory_GetDataForPaymentMoveReport_Оплата_баллами_при_продаже;
              goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
            case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment:
            case GlobalDictionaries.PaymentTypes.BonusesCorrection:
label_29:
              // ISSUE: reference to a compiler-generated field
              dictionary.Add(cDisplayClass61.payment.Uid, str2);
              continue;
            case GlobalDictionaries.PaymentTypes.Prepaid:
              str2 = Translate.ReportsDataFactory_GetDataForPaymentMoveReport_Предоплата_за_товар_по_заказу;
              goto case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
          }
          throw new ArgumentOutOfRangeException();
        }
        return (list2, dictionary);
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass60.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass60.db.Dispose();
        }
      }
    }

    public class ReportFilter
    {
      public DateTime StartDateTime { get; set; }

      public DateTime EndDateTime { get; set; }

      public List<GoodGroups.Group> Groups { get; set; }

      public List<Storages.Storage> Storages { get; set; }

      public Client Supplier { get; set; }

      public PaymentsAccounts.PaymentsAccount PaymentsAccount { get; set; }
    }
  }
}
