// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SaleHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class SaleHelper
  {
    public static (Decimal allSum, Decimal currentSum) GetAllSumCash()
    {
      List<Gbs.Core.Entities.Payments.Payment> payments = SaleHelper.GetPayments();
      return (SaleHelper.GetSumCash(new Guid?(), payments), SaleHelper.GetSumCash(new Guid?(Sections.GetCurrentSection().Uid), payments));
    }

    public static Decimal GetSumCash()
    {
      return SaleHelper.GetSumCash(new Guid?(), (List<Gbs.Core.Entities.Payments.Payment>) null);
    }

    public static Decimal GetSumCash(
      Guid? sectionUid,
      List<Gbs.Core.Entities.Payments.Payment> payments,
      DateTime dateFinish = default (DateTime))
    {
      Performancer performancer = new Performancer("Загрукза суммы наличных для сводного отчета");
      if (payments == null)
        payments = SaleHelper.GetPayments(dateFinish);
      IEnumerable<PaymentMethods.PaymentMethod> methods = PaymentMethods.GetActionPaymentsList().Where<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid != Guid.Empty && !x.IsDeleted));
      if (sectionUid.HasValue)
        methods = methods.Where<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x =>
        {
          Guid sectionUid1 = x.SectionUid;
          Guid? nullable = sectionUid;
          return nullable.HasValue && sectionUid1 == nullable.GetValueOrDefault();
        }));
      IEnumerable<PaymentsAccounts.PaymentsAccount> paymentsAccounts = PaymentsAccounts.GetPaymentsAccountsList().Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted && methods.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (m => m.AccountUid == x.Uid))));
      Decimal sumCash = 0M;
      foreach (PaymentsAccounts.PaymentsAccount paymentsAccount in paymentsAccounts)
      {
        PaymentsAccounts.PaymentsAccount account = paymentsAccount;
        Decimal num = payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
        {
          Guid? uid1 = x.AccountIn?.Uid;
          Guid uid2 = account.Uid;
          return uid1.HasValue && uid1.GetValueOrDefault() == uid2;
        })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)) - payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
        {
          Guid? uid3 = x.AccountOut?.Uid;
          Guid uid4 = account.Uid;
          return uid3.HasValue && uid3.GetValueOrDefault() == uid4;
        })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        sumCash += num;
      }
      performancer.Stop();
      return sumCash;
    }

    private static List<Gbs.Core.Entities.Payments.Payment> GetPayments(DateTime dateFinish = default (DateTime))
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<PAYMENTS> queryable = dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED));
        if (dateFinish != new DateTime())
          queryable = queryable.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.DATE_TIME < dateFinish));
        return Gbs.Core.Entities.Payments.GetPaymentsList(queryable).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.Prepaid)).ToList<Gbs.Core.Entities.Payments.Payment>();
      }
    }

    public static Decimal GetSumDocument(Document document)
    {
      Decimal num = Math.Round(document != null ? document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(i.Quantity, i.SellPrice, i.Discount)))) : 0M, 2, MidpointRounding.AwayFromZero);
      Decimal? nullable = document != null ? new Decimal?(document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))) : new Decimal?();
      return (nullable.HasValue ? new Decimal?(num - nullable.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault();
    }

    public static Decimal GetSumDocumentLessBonuses(Document document)
    {
      Decimal num1 = document != null ? document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(i.Quantity, i.SellPrice, i.Discount)))) : 0M;
      Decimal num2 = document != null ? document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
      {
        PaymentMethods.PaymentMethod method = x.Method;
        // ISSUE: explicit non-virtual call
        return (method != null ? __nonvirtual (method.Uid) : Guid.Empty) == GlobalDictionaries.BonusesPaymentUid;
      })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) : 0M;
      if (document != null && document.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)
        num2 *= -1M;
      Decimal num3 = num2;
      return Math.Round(num1 - num3, 2, MidpointRounding.AwayFromZero);
    }

    public static Decimal GetBuySumDocument(Document document)
    {
      return document == null ? 0M : document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(i.Quantity, i.BuyPrice, i.Discount))));
    }

    public static Decimal GetSumDiscountDocument(Document document)
    {
      return document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => ItemsTotalSumCalculator.DiscountForPosition(i.Quantity, i.SellPrice, i.Discount)));
    }

    public static Decimal GetSumDiscountItem(Gbs.Core.Entities.Documents.Item i)
    {
      return ItemsTotalSumCalculator.DiscountForPosition(i.Quantity, i.SellPrice, i.Discount);
    }

    public static Decimal GetSumItemInDocument(Gbs.Core.Entities.Documents.Item item)
    {
      return ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(item.Quantity, item.SellPrice, item.Discount));
    }

    public static Decimal GetPriceItemWithDiscount(Gbs.Core.Entities.Documents.Item item)
    {
      return ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(1M, item.SellPrice, item.Discount));
    }

    public static Decimal GetSumIncomeItemInDocument(
      Gbs.Core.Entities.Documents.Item item,
      IEnumerable<Document> documents,
      BuyPriceCounter buyPriceCounter,
      Decimal returnQuantity,
      bool isMasterReport = false)
    {
      Gbs.Core.Entities.Documents.Item obj = new Gbs.Core.Entities.Documents.Item(item);
      if (isMasterReport)
        obj.Quantity = Math.Abs(item.Quantity);
      return new ProfitHelper(obj, buyPriceCounter, documents, returnQuantity).GetProfit();
    }

    public static Decimal GetLastBuyPriceForGood(Gbs.Core.Entities.Goods.Good good, IEnumerable<Document> documentsWaybills)
    {
      Document document1 = documentsWaybills.LastOrDefault<Document>((Func<Document, bool>) (document => document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (docItem => docItem.GoodUid == good.Uid))));
      return document1 == null ? 0M : document1.Items.Last<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (it => it.GoodUid == good.Uid)).BuyPrice;
    }

    public static Decimal? GetSalePriceForGood(
      Gbs.Core.Entities.Goods.Good good,
      SalePriceType type,
      Storages.Storage storage = null)
    {
      if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
        return new Decimal?();
      List<GoodsStocks.GoodStock> source = new List<GoodsStocks.GoodStock>(good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock > 0M && !x.IsDeleted)));
      if (!source.Any<GoodsStocks.GoodStock>())
        source = new List<GoodsStocks.GoodStock>((IEnumerable<GoodsStocks.GoodStock>) good.StocksAndPrices);
      Storages.Storage storage1 = storage;
      // ISSUE: explicit non-virtual call
      if ((storage1 != null ? __nonvirtual (storage1.Uid) : Guid.Empty) != Guid.Empty)
        source = source.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Storage.Uid == storage.Uid)).ToList<GoodsStocks.GoodStock>();
      if (!source.Any<GoodsStocks.GoodStock>())
        return new Decimal?();
      switch (type)
      {
        case SalePriceType.Max:
          return new Decimal?(source.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)));
        case SalePriceType.Min:
          return new Decimal?(source.Min<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)));
        case SalePriceType.Avg:
          return new Decimal?(Math.Round(source.Average<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)), 2, MidpointRounding.AwayFromZero));
        case SalePriceType.Last:
          return new Decimal?(source.Last<GoodsStocks.GoodStock>().Price);
        default:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
      }
    }

    private static Gbs.Core.Devices.CheckPrinters.CheckData.CheckData getDataForDocument(
      Document document)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<CheckGood> list1 = document.Items.GroupBy(g => new
        {
          GoodUid = g.GoodUid,
          Comment = g.Comment,
          Discount = g.Discount,
          ModificationUid = g.ModificationUid,
          SellPrice = g.SellPrice
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType33<Guid, string, Decimal, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, CheckGood>(x => new CheckGood(x.First<Gbs.Core.Entities.Documents.Item>().Good, x.First<Gbs.Core.Entities.Documents.Item>().SellPrice, x.First<Gbs.Core.Entities.Documents.Item>().Discount, x.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (g => g.Quantity)), x.First<Gbs.Core.Entities.Documents.Item>().Comment, x.First<Gbs.Core.Entities.Documents.Item>().ModificationUid != Guid.Empty ? x.First<Gbs.Core.Entities.Documents.Item>().Good.Name + " [" + x.First<Gbs.Core.Entities.Documents.Item>().Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == x.Key.ModificationUid))?.Name + "]" : "")).ToList<CheckGood>();
        foreach (Gbs.Core.Entities.Payments.Payment payment in document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)))
        {
          payment.Comment = string.Empty;
          payment.Method = new PaymentMethods.PaymentMethod()
          {
            Name = Translate.FrmCardSale_СкидкаПоЧеку
          };
        }
        List<CheckPayment> list2 = document.Payments.Select<Gbs.Core.Entities.Payments.Payment, CheckPayment>((Func<Gbs.Core.Entities.Payments.Payment, CheckPayment>) (x => new CheckPayment()
        {
          Name = x.Method.Name,
          Method = x.Method.KkmMethod,
          Sum = x.SumIn
        })).ToList<CheckPayment>();
        Gbs.Core.Entities.Users.User user = document.UserUid == Guid.Empty ? (Gbs.Core.Entities.Users.User) null : new UsersRepository(dataBase).GetByUid(document.UserUid);
        List<CheckGood> goodsList = list1;
        List<CheckPayment> paymentsList = list2;
        Cashier cashier = new Cashier();
        cashier.Name = user?.Client.Name ?? "";
        cashier.Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          if (!(x.Type.Uid == GlobalDictionaries.InnUid))
            return false;
          Guid entityUid = x.EntityUid;
          Guid? uid = user?.Client.Uid;
          return uid.HasValue && entityUid == uid.GetValueOrDefault();
        }))?.Value.ToString() ?? "";
        Gbs.Core.Entities.Users.User user1 = user;
        // ISSUE: explicit non-virtual call
        cashier.UserUid = user1 != null ? __nonvirtual (user1.Uid) : Guid.Empty;
        return new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(goodsList, paymentsList, CheckFiscalTypes.NonFiscal, cashier)
        {
          Number = document.Number,
          Client = document.ContractorUid == Guid.Empty ? (ClientAdnSum) null : new ClientsRepository(dataBase).GetClientByUidAndSum(document.ContractorUid),
          Comment = document.Comment,
          DateTime = document.DateTime,
          FiscalNum = document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalNumUid))?.Value.ToString() ?? "",
          FrNumber = document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FrNumber))?.Value.ToString() ?? "",
          Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) document.Properties)
        };
      }
    }

    public static bool PrintNoFiscalCheck(Document document)
    {
      try
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SaleCardViewModel_Печать_чека);
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData dataForDocument = SaleHelper.getDataForDocument(document);
        int num = new CheckPrinterHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()).PrintCheck(dataForDocument) ? 1 : 0;
        progressBar.Close();
        return num != 0;
      }
      catch
      {
        ProgressBarHelper.Close();
        return false;
      }
    }

    public static bool PrintFiscalCheck(Document document)
    {
      try
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SaleCardViewModel_Печать_чека);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          Gbs.Core.Entities.Users.User byUid = new UsersRepository(dataBase).GetByUid(document.UserUid);
          Gbs.Core.ViewModels.Basket.Basket basket = new Gbs.Core.ViewModels.Basket.Basket();
          basket.Document = document;
          basket.Items = new ObservableCollection<BasketItem>(document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
          basket.Payments = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Select<Gbs.Core.Entities.Payments.Payment, SelectPaymentMethods.PaymentGrid>((Func<Gbs.Core.Entities.Payments.Payment, SelectPaymentMethods.PaymentGrid>) (x => new SelectPaymentMethods.PaymentGrid()
          {
            Sum = new Decimal?(x.SumIn),
            Type = x.Method.KkmMethod,
            Name = x.Method.Name,
            Method = x.Method
          })).ToList<SelectPaymentMethods.PaymentGrid>();
          basket.User = byUid;
          basket.SaleNumber = document.Number;
          basket.TrueApiInfoForKkm = document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InfoWithTrueApiUid))?.Value.ToString() ?? "";
          MessageBoxResult result;
          bool flag = basket.TryPrintCheck(out result);
          progressBar.Close();
          return result == MessageBoxResult.OK & flag;
        }
      }
      catch
      {
        ProgressBarHelper.Close();
        return false;
      }
    }

    public static Decimal GetProfitSum(
      List<Document> documents,
      List<Gbs.Core.Entities.Payments.Payment> payments,
      List<PaymentGroups.PaymentGroup> groups,
      out Decimal profitSumGroups,
      CancellationTokenSource token = null)
    {
      Performancer performancer = new Performancer("Получение прибыли для списка документов");
      try
      {
        profitSumGroups = 0M;
        List<Document> list1 = documents.Where<Document>((Func<Document, bool>) (d => d.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn))).ToList<Document>();
        BuyPriceCounter buyPriceCounter = new BuyPriceCounter();
        performancer.AddPoint("point 10");
        ConcurrentBag<Decimal> sumBag = new ConcurrentBag<Decimal>();
        List<Task> list2 = new List<Task>();
        foreach (IEnumerable<Document> chunk in list1.ToChunks<Document>(10))
        {
          IEnumerable<Document> da = chunk;
          Task task = new Task((Action) (() =>
          {
            Decimal num1 = 0M;
            foreach (Document document in da)
            {
              Decimal num2 = document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => SaleHelper.GetSumIncomeItemInDocument(i, (IEnumerable<Document>) documents, buyPriceCounter, 0M)));
              num1 += num2;
              Decimal num3 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
              {
                PaymentMethods.PaymentMethod method = x.Method;
                // ISSUE: explicit non-virtual call
                return (method != null ? __nonvirtual (method.Uid) : Guid.Empty) == GlobalDictionaries.BonusesPaymentUid;
              })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
              num1 -= num3;
              Decimal num4 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted && x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
              num1 -= num4;
            }
            sumBag.Add(num1);
          }));
          list2.Add(task);
        }
        list2.RunList(true);
        Decimal num = sumBag.Sum();
        performancer.AddPoint("point 20");
        foreach (Gbs.Core.Entities.Payments.Payment payment in payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment && x.ParentUid != Guid.Empty)))
        {
          Gbs.Core.Entities.Payments.Payment p = payment;
          PaymentGroups.PaymentGroup paymentGroup = groups.SingleOrDefault<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (x => x.Uid == p.ParentUid));
          if (paymentGroup != null && paymentGroup.IsUseForProfit)
            profitSumGroups += p.SumIn - p.SumOut;
        }
        return num + profitSumGroups;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Во время подсчета прибыли произошла ошибка");
        profitSumGroups = 0M;
        return 0M;
      }
      finally
      {
        performancer.Stop();
      }
    }
  }
}
