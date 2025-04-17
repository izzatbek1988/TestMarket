// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.PrintableReportFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Forms;
using Gbs.Forms.Goods;
using Gbs.Forms.Goods.GoodCard.Pages.Сertificate;
using Gbs.Forms.Reports.MasterReport;
using Gbs.Helpers.Factories;
using Gbs.Helpers.FR.BackEnd.Entities;
using Gbs.Helpers.FR.BackEnd.Entities.Clients;
using Gbs.Helpers.FR.BackEnd.Entities.Goods;
using Gbs.Helpers.FR.BackEnd.Entities.ListItems;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.Client;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.Company;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.Document;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.MasterReport;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.SummaryReport;
using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.User;
using Gbs.Helpers.FR.v2.Entities.ReportProperties.Company;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.FR
{
  public class PrintableReportFactory : IFactory<IPrintableReport>
  {
    private SalePoints.SalePoint CurrentSalePoint
    {
      get => SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
    }

    public IPrintableReport CreateForHiPosFiscalCheck(
      HiPosDriver.GetCheckCommand checkData,
      Dictionary<string, object> props)
    {
      PrintableReport hiPosFiscalCheck = new PrintableReport();
      IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      hiPosFiscalCheck.AddProperties(properties1);
      hiPosFiscalCheck.AddData("items", (IEnumerable) checkData.Result.Items);
      hiPosFiscalCheck.AddData("payments", (IEnumerable) checkData.Result.Pays);
      hiPosFiscalCheck.AddData("taxes", (IEnumerable) checkData.Result.Taxes);
      hiPosFiscalCheck.AddData("excises", (IEnumerable) checkData.Result.Excises);
      List<IReportProperty> properties2 = new List<IReportProperty>();
      foreach (PropertyInfo property in checkData.Result.GetType().GetProperties())
      {
        object obj = property.GetValue((object) checkData.Result);
        if (!(obj is IEnumerable<object>))
        {
          if (obj == null)
            obj = (object) "";
          properties2.Add((IReportProperty) new CustomReportProperty(property.Name, obj));
        }
      }
      if (props != null && props.Any<KeyValuePair<string, object>>())
      {
        foreach (KeyValuePair<string, object> prop in props)
          properties2.Add((IReportProperty) new CustomReportProperty(prop.Key, prop.Value));
      }
      hiPosFiscalCheck.AddProperties((IEnumerable<IReportProperty>) properties2);
      hiPosFiscalCheck.Type = ReportType.CashMemo;
      return (IPrintableReport) hiPosFiscalCheck;
    }

    public IPrintableReport CreateForNonFiscalReport(List<string> texts)
    {
      PrintableReport forNonFiscalReport = new PrintableReport();
      forNonFiscalReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forNonFiscalReport.AddData("data", (IEnumerable) PrintableReportFactory.DataFactory.CreateTextData((IEnumerable<string>) texts));
      forNonFiscalReport.Type = ReportType.NonFiscalPrint;
      return (IPrintableReport) forNonFiscalReport;
    }

    public IPrintableReport CreateForSummaryReport(Gbs.Helpers.FR.BackEnd.Entities.SummaryReport reportData, bool forSellerReport = false)
    {
      PrintableReport forSummaryReport = new PrintableReport();
      forSummaryReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forSummaryReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(reportData));
      forSummaryReport.AddData("paymentsSum", (IEnumerable) PrintableReportFactory.DataFactory.CreatePaymentsItemsData((IEnumerable<CheckPayment>) reportData.Payments).ToList<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>());
      forSummaryReport.Type = forSellerReport ? ReportType.SellerReport : ReportType.SummaryReport;
      return (IPrintableReport) forSummaryReport;
    }

    public IPrintableReport CreateForClients(IEnumerable<ClientAdnSum> clients)
    {
      PrintableReport forClients = new PrintableReport();
      forClients.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forClients.AddData(nameof (clients), (IEnumerable) PrintableReportFactory.DataFactory.CreateClientItemsData(clients));
      forClients.Type = ReportType.ClientsCatalog;
      return (IPrintableReport) forClients;
    }

    public IPrintableReport CreateForSaleDocument(Gbs.Core.Entities.Documents.Document document)
    {
      IPrintableReport forDocument = this.CreateForDocument(document);
      forDocument.Type = ReportType.SaleDocs;
      return forDocument;
    }

    public IPrintableReport CreateForMoveDocument(Gbs.Core.Entities.Documents.Document document, string fromPointName)
    {
      IPrintableReport forDocument = this.CreateForDocument(document);
      forDocument.AddProperties((IEnumerable<IReportProperty>) new List<IReportProperty>()
      {
        (IReportProperty) new CustomReportProperty("FromPointName", UidDb.GetUid().Value),
        (IReportProperty) new CustomReportProperty("ToPointName", (object) fromPointName)
      });
      forDocument.Type = ReportType.SendWaybill;
      return forDocument;
    }

    public IPrintableReport CreateForMoveStorageDocument(Gbs.Core.Entities.Documents.Document document, string fromStorageName)
    {
      IPrintableReport forDocument = this.CreateForDocument(document);
      forDocument.AddProperties((IEnumerable<IReportProperty>) new List<IReportProperty>()
      {
        (IReportProperty) new CustomReportProperty("FromStorageName", (object) document.Storage.Name),
        (IReportProperty) new CustomReportProperty("ToStorageName", (object) fromStorageName)
      });
      forDocument.Type = ReportType.SendWaybillStorage;
      return forDocument;
    }

    public IPrintableReport CreateForUsualCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      PrintableReport forUsualCheck = new PrintableReport();
      Gbs.Core.Entities.Documents.Document document = new Gbs.Core.Entities.Documents.Document()
      {
        Number = data.Number,
        DateTime = data.DateTime,
        Comment = data.Comment,
        IsFiscal = data.FiscalType == CheckFiscalTypes.Fiscal,
        Items = data.GoodsList.Select<CheckGood, Gbs.Core.Entities.Documents.Item>((Func<CheckGood, Gbs.Core.Entities.Documents.Item>) (x => new Gbs.Core.Entities.Documents.Item()
        {
          Good = x.Good,
          SellPrice = x.Price,
          Quantity = x.Quantity,
          Discount = x.Sum + x.DiscountSum == 0M ? 0M : x.DiscountSum / (x.DiscountSum + x.Sum) * 100M
        })).ToList<Gbs.Core.Entities.Documents.Item>(),
        Payments = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Select<CheckPayment, Gbs.Core.Entities.Payments.Payment>((Func<CheckPayment, Gbs.Core.Entities.Payments.Payment>) (x => new Gbs.Core.Entities.Payments.Payment()
        {
          Type = GlobalDictionaries.PaymentTypes.BonusesDocumentPayment,
          SumIn = x.Sum
        })).ToList<Gbs.Core.Entities.Payments.Payment>(),
        Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) data.Properties)
      };
      IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      forUsualCheck.AddProperties(properties1);
      IEnumerable<IReportProperty> properties2 = PrintableReportFactory.PropertiesFactory.Create(document);
      forUsualCheck.AddProperties(properties2);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ClientAdnSum client;
        if ((data.Client?.Client?.Uid ?? Guid.Empty) == Guid.Empty)
        {
          client = new ClientAdnSum();
        }
        else
        {
          client = data.Client;
          if (client != null)
          {
            Decimal sumBonuses = new BonusHelper().GetSumBonuses(document, (Gbs.Core.Entities.Clients.Client) null, false);
            client.CurrentBonusSum += sumBonuses;
            if (data.PaymentsList.Any<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)))
              client.CurrentBonusSum -= data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
            Decimal num = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Credit)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
            client.CurrentCreditSum += num;
            if (sumBonuses > 0M)
              data.CustomData.Add("ReceiveBonuses", (object) sumBonuses);
          }
        }
        IEnumerable<IReportProperty> properties3 = PrintableReportFactory.PropertiesFactory.Create(client);
        forUsualCheck.AddProperties(properties3);
        IEnumerable<IReportProperty> properties4 = PrintableReportFactory.PropertiesFactory.Create(new UsersRepository(dataBase).GetByUid(data.Cashier.UserUid));
        forUsualCheck.AddProperties(properties4);
        data.CustomData.Add("Delivery", (object) data.Delivery);
        data.CustomData.Add("ReceiveSum", (object) data.ReceiveSum);
        data.CustomData.Add("TotalPaymentSum", (object) data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => !x.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Credit))).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum)));
        data.CustomData.Add("DiscountOnSale", (object) data.DiscountSum);
        List<IReportProperty> properties5 = new List<IReportProperty>();
        foreach (KeyValuePair<string, object> keyValuePair in data.CustomData)
          properties5.Add((IReportProperty) new CustomReportProperty(keyValuePair.Key, keyValuePair.Value));
        forUsualCheck.AddProperties((IEnumerable<IReportProperty>) properties5);
        IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItemsData = PrintableReportFactory.DataFactory.CreateBasketItemsData((IEnumerable<CheckGood>) data.GoodsList);
        forUsualCheck.AddData("goods", (IEnumerable) basketItemsData);
        IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem> paymentsItemsData = PrintableReportFactory.DataFactory.CreatePaymentsItemsData((IEnumerable<CheckPayment>) data.PaymentsList);
        List<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem> data1 = (paymentsItemsData != null ? paymentsItemsData.ToList<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>() : (List<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>) null) ?? new List<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>();
        forUsualCheck.AddData("payments", (IEnumerable) data1);
        forUsualCheck.Type = ReportType.CashMemo;
        return (IPrintableReport) forUsualCheck;
      }
    }

    public IPrintableReport CreateForWaybill(Gbs.Core.Entities.Documents.Document document, bool isVisibilityBuyPrice)
    {
      IPrintableReport forDocument = this.CreateForDocument(document, isVisibilityBuyPrice);
      forDocument.Type = ReportType.Waybill;
      return forDocument;
    }

    public IPrintableReport CreateForEmail(IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
    {
      PrintableReport forEmail = new PrintableReport();
      forEmail.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forEmail.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateEmailItemsData(documents));
      forEmail.Type = ReportType.EmailOrders;
      return (IPrintableReport) forEmail;
    }

    public IPrintableReport CreateForSaleOrder(
      IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid> documents,
      ClientAdnSum client,
      DateTime dateStart,
      DateTime dateFinish)
    {
      PrintableReport forSaleOrder = new PrintableReport();
      using (Data.GetDataBase())
      {
        ClientAdnSum client1 = new ClientAdnSum();
        if (client?.Client != null)
          client1 = client.Clone<ClientAdnSum>();
        IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(client1);
        forSaleOrder.AddProperties(properties1);
        IEnumerable<IReportProperty> properties2 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
        forSaleOrder.AddProperties(properties2);
        IEnumerable<IReportProperty> properties3 = PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish);
        forSaleOrder.AddProperties(properties3);
        List<SaleOrderItem> list = PrintableReportFactory.DataFactory.CreateSaleOrderItemsData(documents).ToList<SaleOrderItem>();
        forSaleOrder.AddData("goods", (IEnumerable) list);
        List<IReportProperty> properties4 = new List<IReportProperty>()
        {
          (IReportProperty) new CustomReportProperty("totalSales", (object) documents.Count<SaleJournalViewModel.SaleItemsInfoGrid>())
        };
        forSaleOrder.AddProperties((IEnumerable<IReportProperty>) properties4);
        forSaleOrder.Type = ReportType.SaleOrder;
        return (IPrintableReport) forSaleOrder;
      }
    }

    private IPrintableReport CreateForDocument(Gbs.Core.Entities.Documents.Document document, bool isVisibilityBuyPrice = true)
    {
      PrintableReport forDocument = new PrintableReport();
      IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      forDocument.AddProperties(properties1);
      IEnumerable<IReportProperty> properties2 = PrintableReportFactory.PropertiesFactory.Create(document);
      forDocument.AddProperties(properties2);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ClientAdnSum client = document.ContractorUid == Guid.Empty ? new ClientAdnSum() : new ClientsRepository(dataBase).GetClientByUidAndSum(document.ContractorUid);
        if (client != null)
        {
          IEnumerable<IReportProperty> properties3 = PrintableReportFactory.PropertiesFactory.Create(client);
          forDocument.AddProperties(properties3);
        }
        IEnumerable<IReportProperty> properties4 = PrintableReportFactory.PropertiesFactory.Create(new UsersRepository(dataBase).GetByUid(document.UserUid));
        forDocument.AddProperties(properties4);
        List<IReportProperty> properties5 = new List<IReportProperty>();
        properties5.AddRange((IEnumerable<IReportProperty>) ((client != null ? client.Client.PropertiesDictionary.Select<KeyValuePair<Guid, object>, CustomReportProperty>((Func<KeyValuePair<Guid, object>, CustomReportProperty>) (d => new CustomReportProperty(d.Key.ToString(), d.Value))).Cast<IReportProperty>().ToList<IReportProperty>() : (List<IReportProperty>) null) ?? new List<IReportProperty>()));
        properties5.Add((IReportProperty) new CustomReportProperty("TotalPaymentSum", (object) document.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))));
        properties5.Add((IReportProperty) new CustomReportProperty("DiscountOnSale", (object) document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))));
        forDocument.AddProperties((IEnumerable<IReportProperty>) properties5);
        List<Gbs.Core.Entities.Documents.Item> items = document.Items;
        List<Gbs.Core.Entities.Documents.Document> documents = new List<Gbs.Core.Entities.Documents.Document>();
        documents.Add(document);
        int num = isVisibilityBuyPrice ? 1 : 0;
        IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItemsData = PrintableReportFactory.DataFactory.CreateBasketItemsData((IEnumerable<Gbs.Core.Entities.Documents.Item>) items, documents, num != 0);
        forDocument.AddData("goods", (IEnumerable) basketItemsData);
        return (IPrintableReport) forDocument;
      }
    }

    public IPrintableReport CreateForGoodsCatalog(
      IEnumerable<GoodsCatalogModelView.GoodsInfoGrid> goodsList)
    {
      PrintableReport forGoodsCatalog = new PrintableReport();
      forGoodsCatalog.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forGoodsCatalog.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateGoodsItemsData(goodsList.ToList<GoodsCatalogModelView.GoodsInfoGrid>()));
      forGoodsCatalog.Type = ReportType.GoodsCatalog;
      return (IPrintableReport) forGoodsCatalog;
    }

    public IPrintableReport CreateForGoodCard(Gbs.Core.Entities.Goods.Good good, List<Gbs.Core.Entities.Documents.Item> sets)
    {
      PrintableReport forGoodCard = new PrintableReport();
      forGoodCard.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      List<Gbs.Core.Entities.Documents.Document> list = new List<Gbs.Core.Entities.Documents.Document>()
      {
        new Gbs.Core.Entities.Documents.Document()
        {
          Items = new List<Gbs.Core.Entities.Documents.Item>((IEnumerable<Gbs.Core.Entities.Documents.Item>) sets),
          ParentUid = good.Uid
        }
      }.ToList<Gbs.Core.Entities.Documents.Document>();
      (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> productions, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> sets) reportProductionData = PrintableReportFactory.DataFactory.CreateReportProductionData(list, list);
      forGoodCard.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateGoodsItemsData(new List<Gbs.Core.Entities.Goods.Good>()
      {
        good
      }));
      forGoodCard.AddData("goodSets", (IEnumerable) reportProductionData.sets);
      forGoodCard.Type = ReportType.GoodCard;
      return (IPrintableReport) forGoodCard;
    }

    public IPrintableReport CreateForOldGoodsReport(IEnumerable<Gbs.Core.Entities.Goods.Good> goodsList)
    {
      PrintableReport forOldGoodsReport = new PrintableReport();
      forOldGoodsReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forOldGoodsReport.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateGoodsItemsData(goodsList.ToList<Gbs.Core.Entities.Goods.Good>()));
      forOldGoodsReport.Type = ReportType.ReportOldGoods;
      return (IPrintableReport) forOldGoodsReport;
    }

    public IPrintableReport CreateForSaleReport(
      IEnumerable<Gbs.Core.Entities.Documents.Item> items,
      IEnumerable<Gbs.Core.Entities.Documents.Document> documents,
      DateTime dateStart,
      DateTime dateFinish)
    {
      PrintableReport forSaleReport = new PrintableReport();
      forSaleReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forSaleReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish));
      forSaleReport.AddData(nameof (items), (IEnumerable) PrintableReportFactory.DataFactory.CreateReportItemsData((IEnumerable<Gbs.Core.Entities.Documents.Item>) items.ToList<Gbs.Core.Entities.Documents.Item>(), (IEnumerable<Gbs.Core.Entities.Documents.Document>) documents.ToList<Gbs.Core.Entities.Documents.Document>()));
      forSaleReport.Type = ReportType.ReportSale;
      return (IPrintableReport) forSaleReport;
    }

    public IPrintableReport CreateForGoodHistoryReport(
      IEnumerable<Gbs.Core.Entities.Goods.Good> items,
      IEnumerable<Gbs.Core.Entities.Documents.Document> documents,
      DateTime dateStart,
      DateTime dateFinish)
    {
      PrintableReport goodHistoryReport = new PrintableReport();
      goodHistoryReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      goodHistoryReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish));
      goodHistoryReport.AddData(nameof (items), (IEnumerable) PrintableReportFactory.DataFactory.CreateReportHistoryData(items, documents, dateStart, dateFinish));
      documents = (IEnumerable<Gbs.Core.Entities.Documents.Document>) documents.Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.DateTime >= dateStart.Date && x.DateTime.Date <= dateFinish.Date)).ToList<Gbs.Core.Entities.Documents.Document>();
      goodHistoryReport.AddData("documentItems", (IEnumerable) PrintableReportFactory.DataFactory.CreateBasketItemsData(documents.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)), documents.ToList<Gbs.Core.Entities.Documents.Document>()));
      goodHistoryReport.Type = ReportType.ReportHistoryGoods;
      return (IPrintableReport) goodHistoryReport;
    }

    public IPrintableReport CreatePaymentsForSupplierReport(
      IEnumerable<Gbs.Core.Entities.Documents.Document> documents,
      DateTime dateStart,
      DateTime dateFinish,
      Decimal saldo)
    {
      PrintableReport forSupplierReport = new PrintableReport();
      IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      forSupplierReport.AddProperties(properties1);
      IEnumerable<IReportProperty> properties2 = PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish);
      forSupplierReport.AddProperties(properties2);
      List<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem> list = documents.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (d => (IEnumerable<Gbs.Core.Entities.Payments.Payment>) d.Payments)).Select<Gbs.Core.Entities.Payments.Payment, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>((Func<Gbs.Core.Entities.Payments.Payment, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>) (x => new Gbs.Helpers.FR.BackEnd.Entities.PaymentItem()
      {
        DateTime = x.Date,
        Sum = x.SumIn - x.SumOut,
        Name = Translate.PrintableReportFactory_CreatePaymentsForSupplierReport_платеж_по_накладной + documents.Single<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (d => d.Uid == x.ParentUid)).Number,
        ParentUid = x.ParentUid
      })).ToList<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>();
      list.AddRange(documents.Select<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>((Func<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>) (x => new Gbs.Helpers.FR.BackEnd.Entities.PaymentItem()
      {
        DateTime = x.DateTime,
        ParentUid = x.Uid,
        Sum = SaleHelper.GetBuySumDocument(x),
        Name = Translate.PrintableReportFactory_CreatePaymentsForSupplierReport_поставка_по_накладной + x.Number
      })));
      forSupplierReport.AddData("payments", (IEnumerable) list);
      IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItemsData = PrintableReportFactory.DataFactory.CreateBasketItemsData(documents.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)), documents.ToList<Gbs.Core.Entities.Documents.Document>());
      forSupplierReport.AddData("items", (IEnumerable) basketItemsData);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Entities.Documents.Document document = documents.FirstOrDefault<Gbs.Core.Entities.Documents.Document>();
        ClientAdnSum client = (document != null ? document.ContractorUid : Guid.Empty) == Guid.Empty ? new ClientAdnSum() : new ClientsRepository(dataBase).GetClientByUidAndSum(documents.First<Gbs.Core.Entities.Documents.Document>().ContractorUid);
        if (client != null)
        {
          IEnumerable<IReportProperty> properties3 = PrintableReportFactory.PropertiesFactory.Create(client);
          forSupplierReport.AddProperties(properties3);
        }
        forSupplierReport.AddProperties((IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new CustomReportProperty("Saldo", (object) saldo)
        });
        IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Document> documentsData = PrintableReportFactory.DataFactory.CreateDocumentsData(documents);
        forSupplierReport.AddData(nameof (documents), (IEnumerable) documentsData);
        forSupplierReport.Type = ReportType.ReportPaymentsForSupplier;
        return (IPrintableReport) forSupplierReport;
      }
    }

    public IPrintableReport CreatePaymentsMoveReport(
      IEnumerable<Gbs.Core.Entities.Payments.Payment> payments,
      DateTime dateStart,
      DateTime dateFinish,
      Decimal sumOld,
      Decimal sumCurrent,
      Dictionary<Guid, string> groupsPayments,
      string accountName)
    {
      PrintableReport paymentsMoveReport = new PrintableReport();
      paymentsMoveReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      paymentsMoveReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish));
      paymentsMoveReport.AddProperties((IEnumerable<IReportProperty>) new List<IReportProperty>()
      {
        (IReportProperty) new CustomReportProperty("SumStart", (object) sumOld),
        (IReportProperty) new CustomReportProperty("SumFinish", (object) sumCurrent),
        (IReportProperty) new CustomReportProperty("AccountName", (object) accountName)
      });
      paymentsMoveReport.AddData(nameof (payments), (IEnumerable) PrintableReportFactory.DataFactory.CreateReportPaymentData(payments.ToList<Gbs.Core.Entities.Payments.Payment>(), groupsPayments));
      paymentsMoveReport.Type = ReportType.ReportPaymentsMove;
      return (IPrintableReport) paymentsMoveReport;
    }

    public IPrintableReport CreateForMarkedLable(IEnumerable<string> codes)
    {
      PrintableReport forMarkedLable = new PrintableReport();
      forMarkedLable.AddData("items", (IEnumerable) codes.Select<string, MarkedCode>((Func<string, MarkedCode>) (x => new MarkedCode()
      {
        Code = x
      })));
      forMarkedLable.Type = ReportType.ReportMarkedLable;
      return (IPrintableReport) forMarkedLable;
    }

    public IPrintableReport CreateClientOrderReport(IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
    {
      PrintableReport clientOrderReport = new PrintableReport();
      clientOrderReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      (IEnumerable<ClientOrder> orders, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> items) reportItemsData = PrintableReportFactory.DataFactory.CreateReportItemsData(documents.ToList<Gbs.Core.Entities.Documents.Document>());
      clientOrderReport.AddData("orders", (IEnumerable) reportItemsData.orders);
      clientOrderReport.AddData("items", (IEnumerable) reportItemsData.items);
      clientOrderReport.Type = ReportType.ClientsOrder;
      return (IPrintableReport) clientOrderReport;
    }

    public IPrintableReport CreateProductionReport(
      List<Gbs.Core.Entities.Documents.Document> prodDocuments,
      List<Gbs.Core.Entities.Documents.Document> setsDocuments)
    {
      PrintableReport productionReport = new PrintableReport();
      productionReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> productions, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> sets) reportProductionData = PrintableReportFactory.DataFactory.CreateReportProductionData(prodDocuments, setsDocuments);
      productionReport.AddData("productions", (IEnumerable) reportProductionData.productions);
      productionReport.AddData("sets", (IEnumerable) reportProductionData.sets);
      productionReport.Type = ReportType.Production;
      return (IPrintableReport) productionReport;
    }

    public IPrintableReport CreateForOrderGoodReport(
      IEnumerable<MasterReportViewModel.GoodOrder> items,
      IEnumerable<Gbs.Core.Entities.Documents.Document> documents,
      DateTime dateStart,
      DateTime dateFinish)
    {
      PrintableReport forOrderGoodReport = new PrintableReport();
      forOrderGoodReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      forOrderGoodReport.AddProperties(PrintableReportFactory.PropertiesFactory.Create(dateStart, dateFinish));
      forOrderGoodReport.AddData(nameof (items), (IEnumerable) PrintableReportFactory.DataFactory.CreateReportItemsData((IEnumerable<MasterReportViewModel.GoodOrder>) items.ToList<MasterReportViewModel.GoodOrder>(), (IEnumerable<Gbs.Core.Entities.Documents.Document>) documents.ToList<Gbs.Core.Entities.Documents.Document>()));
      forOrderGoodReport.Type = ReportType.ReportOrderGood;
      return (IPrintableReport) forOrderGoodReport;
    }

    public IPrintableReport CreateForPriceTags(IEnumerable<Gbs.Core.ViewModels.Basket.BasketItem> goodsList)
    {
      IPrintableReport forLabel = this.CreateForLabel(goodsList, true);
      forLabel.Type = ReportType.PriceTags;
      return forLabel;
    }

    public IPrintableReport CreateForLabel(IEnumerable<Gbs.Core.ViewModels.Basket.BasketItem> goodsList, bool isSplitQ)
    {
      PrintableReport forLabel = new PrintableReport();
      forLabel.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.WeightGoods.Split(GlobalDictionaries.SplitArr);
      string str = "";
      if (strArray.Length != 0)
        str = strArray[0];
      forLabel.AddProperties((IEnumerable<IReportProperty>) new List<IReportProperty>()
      {
        (IReportProperty) new CustomReportProperty("PrefixesWeightGoods", (object) str)
      });
      forLabel.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateLabelItemsData(goodsList, isSplitQ));
      forLabel.Type = ReportType.Label;
      return (IPrintableReport) forLabel;
    }

    public IPrintableReport CreateForCertifications(
      List<CertificateBasicViewModel.CertificateView> certificates,
      Gbs.Core.Entities.Goods.Good good,
      Decimal nominal)
    {
      if (good == null)
        return (IPrintableReport) null;
      PrintableReport forCertifications = new PrintableReport();
      List<Gbs.Helpers.FR.BackEnd.Entities.Certificate> data = new List<Gbs.Helpers.FR.BackEnd.Entities.Certificate>();
      foreach (CertificateBasicViewModel.CertificateView certificate1 in certificates)
      {
        Gbs.Helpers.FR.BackEnd.Entities.Certificate certificate2 = new Gbs.Helpers.FR.BackEnd.Entities.Certificate()
        {
          Barcode = certificate1.Certificate.Barcode,
          Name = good.Name,
          Uid = certificate1.Certificate.Uid,
          Nominal = nominal
        };
        data.Add(certificate2);
      }
      forCertifications.AddData(nameof (certificates), (IEnumerable) data);
      IEnumerable<IReportProperty> properties = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      forCertifications.AddProperties(properties);
      forCertifications.Type = ReportType.Certificates;
      return (IPrintableReport) forCertifications;
    }

    public IPrintableReport CreateForInventory(Gbs.Core.Entities.Documents.Document document, bool isVisibilityStock)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Documents.Document> byParentUid = new DocumentsRepository(dataBase).GetByParentUid(document.Uid);
        Gbs.Core.Entities.Documents.Document document1 = (byParentUid != null ? byParentUid.First<Gbs.Core.Entities.Documents.Document>() : (Gbs.Core.Entities.Documents.Document) null) ?? new Gbs.Core.Entities.Documents.Document();
        PrintableReport forInventory = new PrintableReport();
        forInventory.AddProperties(PrintableReportFactory.PropertiesFactory.Create(document));
        forInventory.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
        forInventory.AddProperties(PrintableReportFactory.PropertiesFactory.Create(new UsersRepository(dataBase).GetByUid(document.UserUid)));
        forInventory.AddData("goods", (IEnumerable) PrintableReportFactory.DataFactory.CreateInventoryItemsData((IEnumerable<Gbs.Core.Entities.Documents.Item>) document.Items, (IEnumerable<Gbs.Core.Entities.Documents.Item>) document1.Items, isVisibilityStock, document.Status == GlobalDictionaries.DocumentsStatuses.Close));
        forInventory.Type = ReportType.Inventory;
        return (IPrintableReport) forInventory;
      }
    }

    public IPrintableReport CreateForWriteOff(Gbs.Core.Entities.Documents.Document document)
    {
      if (document == null)
        return (IPrintableReport) null;
      PrintableReport forWriteOff = new PrintableReport();
      IEnumerable<IReportProperty> properties1 = PrintableReportFactory.PropertiesFactory.Create(document);
      forWriteOff.AddProperties(properties1);
      IEnumerable<IReportProperty> properties2 = PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint);
      forWriteOff.AddProperties(properties2);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<IReportProperty> properties3 = PrintableReportFactory.PropertiesFactory.Create(new UsersRepository(dataBase).GetByUid(document.UserUid));
        forWriteOff.AddProperties(properties3);
        IEnumerable<WriteOffItem> writeOffItemsData = PrintableReportFactory.DataFactory.CreateWriteOffItemsData((IEnumerable<Gbs.Core.Entities.Documents.Item>) document.Items);
        forWriteOff.AddData("goods", (IEnumerable) writeOffItemsData);
        forWriteOff.Type = ReportType.WriteOff;
        return (IPrintableReport) forWriteOff;
      }
    }

    public IPrintableReport CreateGoodGroups(List<GoodGroups.Group> groups)
    {
      if (groups == null)
        return (IPrintableReport) null;
      PrintableReport goodGroups = new PrintableReport();
      goodGroups.AddProperties(PrintableReportFactory.PropertiesFactory.Create(this.CurrentSalePoint));
      goodGroups.AddData(nameof (groups), (IEnumerable) PrintableReportFactory.DataFactory.CreateGoodGroupsItemsData(groups));
      goodGroups.Type = ReportType.GoodGroups;
      return (IPrintableReport) goodGroups;
    }

    private class DataFactory : IFactory<IEnumerable>
    {
      public static IEnumerable<GoodGroup> CreateGoodGroupsItemsData(List<GoodGroups.Group> groups)
      {
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        return (IEnumerable<GoodGroup>) groups.Select<GoodGroups.Group, GoodGroup>((Func<GoodGroups.Group, GoodGroup>) (item => PrintableReportFactory.DataFactory.CreateGoodGroup(item, devicesConfig))).ToList<GoodGroup>();
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> CreateGoodsItemsData(
        List<GoodsCatalogModelView.GoodsInfoGrid> goods)
      {
        IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) goods.Select(item => new
        {
          item = item,
          good = PrintableReportFactory.DataFactory.CreateGood(item.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units)
        }).Select(_param1 => new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
        {
          Good = _param1.good,
          Price = _param1.item.MaxPrice.GetValueOrDefault(),
          Quantity = _param1.item.GoodTotalStock.GetValueOrDefault(),
          BuyPrice = _param1.item.LastBuyPrice.GetValueOrDefault()
        }).ToList<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>();
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> CreateGoodsItemsData(
        List<Gbs.Core.Entities.Goods.Good> goods)
      {
        List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> goodsItemsData = new List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>();
        List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        IEnumerable<GoodsUnits.GoodUnit> unitsListWithFilter = GoodsUnits.GetUnitsListWithFilter();
        foreach (Gbs.Core.Entities.Goods.Good good1 in goods)
        {
          Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good2 = PrintableReportFactory.DataFactory.CreateGood(good1, devicesConfig, (IEnumerable<EntityProperties.PropertyType>) typesList, unitsListWithFilter);
          Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem basketItem1 = new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem();
          basketItem1.Good = good2;
          Decimal? nullable1;
          Decimal num;
          if (!good1.StocksAndPrices.Any<GoodsStocks.GoodStock>())
          {
            num = 0M;
          }
          else
          {
            List<GoodsStocks.GoodStock> stocksAndPrices = good1.StocksAndPrices;
            Decimal? nullable2;
            if (stocksAndPrices == null)
            {
              nullable1 = new Decimal?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new Decimal?(stocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)));
            nullable1 = nullable2;
            num = nullable1.Value;
          }
          basketItem1.Price = num;
          List<GoodsStocks.GoodStock> stocksAndPrices1 = good1.StocksAndPrices;
          Decimal? nullable3;
          if (stocksAndPrices1 == null)
          {
            nullable1 = new Decimal?();
            nullable3 = nullable1;
          }
          else
            nullable3 = new Decimal?(stocksAndPrices1.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
          nullable1 = nullable3;
          basketItem1.Quantity = nullable1.Value;
          Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem basketItem2 = basketItem1;
          goodsItemsData.Add(basketItem2);
        }
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) goodsItemsData;
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> CreateBasketItemsData(
        IEnumerable<Gbs.Core.Entities.Documents.Item> documentItems,
        List<Gbs.Core.Entities.Documents.Document> documents,
        bool isVisibilityBuyPrice = true)
      {
        List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItemsData = new List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>();
        List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        IEnumerable<GoodsUnits.GoodUnit> unitsListWithFilter = GoodsUnits.GetUnitsListWithFilter();
        foreach (Gbs.Core.Entities.Documents.Item documentItem in documentItems)
        {
          Gbs.Core.Entities.Documents.Item item = documentItem;
          Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = PrintableReportFactory.DataFactory.CreateGood(item.Good, devicesConfig, (IEnumerable<EntityProperties.PropertyType>) typesList, unitsListWithFilter);
          Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem basketItem = new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
          {
            DocumentsType = (int) documents.Single<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Uid == item.DocumentUid)).Type,
            DocumentUid = item.DocumentUid,
            Good = good,
            BuyPrice = isVisibilityBuyPrice ? item.BuyPrice : 0M,
            Comment = item.Comment,
            Discount = item.Discount,
            Quantity = item.Quantity,
            Price = item.SellPrice,
            GoodModificationName = item.ModificationUid != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.ModificationUid))?.Name : "",
            GoodModificationBarcode = item.ModificationUid != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.ModificationUid))?.Barcode : "",
            IsFiscal = documents.Single<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Uid == item.DocumentUid)).IsFiscal
          };
          basketItemsData.Add(basketItem);
        }
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) basketItemsData;
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> CreateBasketItemsData(
        IEnumerable<CheckGood> items)
      {
        List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItemsData = new List<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>();
        List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        IEnumerable<GoodsUnits.GoodUnit> unitsListWithFilter = GoodsUnits.GetUnitsListWithFilter();
        foreach (CheckGood checkGood in items)
        {
          Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = PrintableReportFactory.DataFactory.CreateGood(checkGood.Good, devicesConfig, (IEnumerable<EntityProperties.PropertyType>) typesList, unitsListWithFilter);
          Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem basketItem = new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
          {
            Good = good,
            Comment = checkGood.Description,
            Discount = checkGood.Sum + checkGood.DiscountSum == 0M ? 0M : checkGood.DiscountSum / (checkGood.DiscountSum + checkGood.Sum) * 100M,
            Quantity = checkGood.Quantity,
            Price = checkGood.Price
          };
          basketItemsData.Add(basketItem);
        }
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) basketItemsData;
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.PaymentItem> CreatePaymentsItemsData(
        IEnumerable<CheckPayment> payments)
      {
        return payments.Select<CheckPayment, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>((Func<CheckPayment, Gbs.Helpers.FR.BackEnd.Entities.PaymentItem>) (x => new Gbs.Helpers.FR.BackEnd.Entities.PaymentItem()
        {
          Sum = x.Sum,
          Name = x.Name
        }));
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Text> CreateTextData(
        IEnumerable<string> texts)
      {
        return texts.Select<string, Gbs.Helpers.FR.BackEnd.Entities.Text>((Func<string, Gbs.Helpers.FR.BackEnd.Entities.Text>) (x => new Gbs.Helpers.FR.BackEnd.Entities.Text()
        {
          Line = x
        }));
      }

      public static IEnumerable<GoodReportItem> CreateReportItemsData(
        IEnumerable<Gbs.Core.Entities.Documents.Item> documentItems,
        IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Documents.Document> docs = new DocumentsRepository(dataBase).GetActiveItems().ToList<Gbs.Core.Entities.Documents.Document>();
          List<Gbs.Core.Entities.Users.User> allItems = new UsersRepository(dataBase).GetAllItems();
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          BuyPriceCounter counter = new BuyPriceCounter();
          Dictionary<Guid, Gbs.Core.Entities.Documents.Document> documentDictionary = documents.ToDictionary<Gbs.Core.Entities.Documents.Document, Guid>((Func<Gbs.Core.Entities.Documents.Document, Guid>) (doc => doc.Uid));
          Dictionary<Guid, Gbs.Core.Entities.Users.User> userDictionary = allItems.ToDictionary<Gbs.Core.Entities.Users.User, Guid>((Func<Gbs.Core.Entities.Users.User, Guid>) (user => user.Uid));
          Gbs.Core.Entities.Users.User user1;
          List<GoodReportItem> list = documentItems.Select(item => new
          {
            item = item,
            good = PrintableReportFactory.DataFactory.CreateGood(item.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units)
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier0 = _param1,
            document = documentDictionary[_param1.item.DocumentUid]
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier1 = _param1,
            user = userDictionary.TryGetValue(_param1.document.UserUid, out user1) ? user1 : (Gbs.Core.Entities.Users.User) null
          }).Select(_param1 => new GoodReportItem()
          {
            Good = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.good,
            Discount = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item.Discount,
            Quantity = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item.Quantity,
            Price = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item.SellPrice,
            DocumentDate = _param1.\u003C\u003Eh__TransparentIdentifier1.document.DateTime,
            Income = SaleHelper.GetSumIncomeItemInDocument(_param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item, (IEnumerable<Gbs.Core.Entities.Documents.Document>) docs, counter, 0M, true),
            User = _param1.user != null ? new Gbs.Helpers.FR.BackEnd.Entities.User(_param1.user) : (Gbs.Helpers.FR.BackEnd.Entities.User) null
          }).ToList<GoodReportItem>();
          list.ForEach((Action<GoodReportItem>) (x => x.BuyPrice = Math.Round(x.Sum - x.Income / x.Quantity, 2)));
          return (IEnumerable<GoodReportItem>) list;
        }
      }

      public static IEnumerable<GoodHistoryReportItem> CreateReportHistoryData(
        IEnumerable<Gbs.Core.Entities.Goods.Good> goods,
        IEnumerable<Gbs.Core.Entities.Documents.Document> documents,
        DateTime dateStart,
        DateTime dateFinish)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        using (Data.GetDataBase())
        {
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          return (IEnumerable<GoodHistoryReportItem>) goods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => documents.Any<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (d => d.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == x.Uid)))))).Select<Gbs.Core.Entities.Goods.Good, GoodHistoryReportItem>((Func<Gbs.Core.Entities.Goods.Good, GoodHistoryReportItem>) (x => new GoodHistoryReportItem()
          {
            MaxPrice = x.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? x.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (p => p.Price)) : 0M,
            Good = PrintableReportFactory.DataFactory.CreateGood(x, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units),
            CountFinish = countForData(x, documents.Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (d => d.DateTime.Date <= dateFinish.Date))),
            CountStart = countForData(x, documents.Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (d => d.DateTime <= dateStart.Date)))
          })).ToList<GoodHistoryReportItem>();
        }

        static Decimal countForData(Gbs.Core.Entities.Goods.Good good, IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
        {
          Decimal num = 0M;
          foreach (Gbs.Core.Entities.Documents.Document document in documents)
          {
            foreach (Gbs.Core.Entities.Documents.Item obj in document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == good.Uid)))
            {
              switch (document.Type)
              {
                case GlobalDictionaries.DocumentsTypes.Sale:
                case GlobalDictionaries.DocumentsTypes.BuyReturn:
                case GlobalDictionaries.DocumentsTypes.Move:
                case GlobalDictionaries.DocumentsTypes.WriteOff:
                case GlobalDictionaries.DocumentsTypes.ProductionSet:
                  num -= obj.Quantity;
                  continue;
                case GlobalDictionaries.DocumentsTypes.SaleReturn:
                case GlobalDictionaries.DocumentsTypes.Buy:
                case GlobalDictionaries.DocumentsTypes.MoveReturn:
                case GlobalDictionaries.DocumentsTypes.UserStockEdit:
                case GlobalDictionaries.DocumentsTypes.InventoryAct:
                case GlobalDictionaries.DocumentsTypes.SetChildStockChange:
                case GlobalDictionaries.DocumentsTypes.ProductionItem:
                  num += obj.Quantity;
                  continue;
                default:
                  continue;
              }
            }
          }
          return num;
        }
      }

      public static (IEnumerable<ClientOrder> orders, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> items) CreateReportItemsData(
        List<Gbs.Core.Entities.Documents.Document> ordersList)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          IEnumerable<ClientAdnSum> clients = new ClientsRepository(dataBase).GetListActiveItemAndSum();
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItems = ordersList.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>>) (x => x.Items.Select<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) (i => new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
          {
            Good = PrintableReportFactory.DataFactory.CreateGood(i.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units),
            Quantity = i.Quantity,
            Discount = i.Discount,
            Price = i.SellPrice,
            Comment = i.Comment,
            GoodModificationName = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Name : "",
            GoodModificationBarcode = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Barcode : "",
            BuyPrice = i.BuyPrice,
            DocumentUid = x.Uid
          }))));
          List<EntityProperties.PropertyType> propertyClientTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client);
          return (ordersList.Select<Gbs.Core.Entities.Documents.Document, ClientOrder>((Func<Gbs.Core.Entities.Documents.Document, ClientOrder>) (x => new ClientOrder()
          {
            Uid = x.Uid,
            Number = x.Number,
            Comment = x.Comment,
            Status = GlobalDictionaries.ClientOrderStatusDictionary.SingleOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (k => k.Key == x.Status)).Value ?? "",
            DateCreate = x.DateTime,
            DateClose = x.DateTime,
            SumPayment = x.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (s => s.SumIn)),
            Client = PrintableReportFactory.DataFactory.CreateClient(clients.SingleOrDefault<ClientAdnSum>((Func<ClientAdnSum, bool>) (c => c.Client.Uid == x.ContractorUid)), (IEnumerable<EntityProperties.PropertyType>) propertyClientTypes)
          })), basketItems);
        }
      }

      public static (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> productions, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> sets) CreateReportProductionData(
        List<Gbs.Core.Entities.Documents.Document> production,
        List<Gbs.Core.Entities.Documents.Document> setsDocuments)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        using (Data.GetDataBase())
        {
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem> basketItems = setsDocuments.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>>) (x => x.Items.Select<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) (i => new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
          {
            Good = PrintableReportFactory.DataFactory.CreateGood(i.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units),
            Quantity = i.Quantity,
            Discount = i.Discount,
            Price = i.SellPrice,
            Comment = i.Comment,
            GoodModificationName = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Name : "",
            GoodModificationBarcode = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Barcode : "",
            BuyPrice = i.BuyPrice,
            DocumentUid = x.ParentUid
          }))));
          return (production.SelectMany<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Document, IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>>) (x => x.Items.Select<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem>) (i => new Gbs.Helpers.FR.BackEnd.Entities.ListItems.BasketItem()
          {
            Good = PrintableReportFactory.DataFactory.CreateGood(i.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units),
            Quantity = i.Quantity,
            Discount = i.Discount,
            Price = i.SellPrice,
            Comment = i.Comment,
            GoodModificationName = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Name : "",
            GoodModificationBarcode = i.ModificationUid != Guid.Empty ? i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Barcode : "",
            BuyPrice = i.BuyPrice,
            DocumentUid = x.Uid
          })))), basketItems);
        }
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Payment> CreateReportPaymentData(
        List<Gbs.Core.Entities.Payments.Payment> payments,
        Dictionary<Guid, string> groupPayments)
      {
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client);
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Payment>) payments.Select<Gbs.Core.Entities.Payments.Payment, Gbs.Helpers.FR.BackEnd.Entities.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Gbs.Helpers.FR.BackEnd.Entities.Payment>) (x =>
        {
          Gbs.Helpers.FR.BackEnd.Entities.Payment reportPaymentData = new Gbs.Helpers.FR.BackEnd.Entities.Payment();
          reportPaymentData.GroupInfo = groupPayments.Single<KeyValuePair<Guid, string>>((Func<KeyValuePair<Guid, string>, bool>) (g => g.Key == x.Uid)).Value;
          reportPaymentData.Comment = x.Comment;
          reportPaymentData.Type = x.Type;
          reportPaymentData.Date = x.Date;
          reportPaymentData.Client = PrintableReportFactory.DataFactory.CreateClient(new ClientAdnSum()
          {
            Client = x.Client
          }, (IEnumerable<EntityProperties.PropertyType>) propertyTypes);
          reportPaymentData.SumOut = x.SumOut;
          Gbs.Helpers.FR.BackEnd.Entities.PaymentsAccount paymentsAccount1 = new Gbs.Helpers.FR.BackEnd.Entities.PaymentsAccount();
          paymentsAccount1.Name = x.AccountIn?.Name ?? Translate.PaymentsActionsViewModel_Не_указан;
          PaymentsAccounts.PaymentsAccount accountIn = x.AccountIn;
          // ISSUE: explicit non-virtual call
          paymentsAccount1.Uid = accountIn != null ? __nonvirtual (accountIn.Uid) : Guid.Empty;
          reportPaymentData.AccountIn = paymentsAccount1;
          Gbs.Helpers.FR.BackEnd.Entities.PaymentsAccount paymentsAccount2 = new Gbs.Helpers.FR.BackEnd.Entities.PaymentsAccount();
          paymentsAccount2.Name = x.AccountOut?.Name ?? Translate.PaymentsActionsViewModel_Не_указан;
          PaymentsAccounts.PaymentsAccount accountOut = x.AccountOut;
          // ISSUE: explicit non-virtual call
          paymentsAccount2.Uid = accountOut != null ? __nonvirtual (accountOut.Uid) : Guid.Empty;
          reportPaymentData.AccountOut = paymentsAccount2;
          reportPaymentData.IsFiscal = x.IsFiscal;
          Gbs.Helpers.FR.BackEnd.Entities.PaymentMethod paymentMethod = new Gbs.Helpers.FR.BackEnd.Entities.PaymentMethod();
          paymentMethod.Name = x.Method?.Name;
          PaymentMethods.PaymentMethod method = x.Method;
          // ISSUE: explicit non-virtual call
          paymentMethod.Uid = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
          reportPaymentData.Method = paymentMethod;
          reportPaymentData.ParentUid = x.ParentUid;
          reportPaymentData.Section = new Gbs.Helpers.FR.BackEnd.Entities.Section()
          {
            Name = x.Section.Name,
            Uid = x.Section.Uid
          };
          reportPaymentData.SumIn = x.SumIn;
          reportPaymentData.User = new Gbs.Helpers.FR.BackEnd.Entities.User(x.User);
          return reportPaymentData;
        })).ToList<Gbs.Helpers.FR.BackEnd.Entities.Payment>();
      }

      public static IEnumerable<GoodOrderItem> CreateReportItemsData(
        IEnumerable<MasterReportViewModel.GoodOrder> items,
        IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Documents.Document> docWaybill = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).ToList<Gbs.Core.Entities.Documents.Document>();
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          return (IEnumerable<GoodOrderItem>) items.Select(item => new
          {
            item = item,
            good = PrintableReportFactory.DataFactory.CreateGood(item.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units)
          }).Select(_param1 => new GoodOrderItem()
          {
            Good = _param1.good,
            SaleQuantity = _param1.item.SaleQuantity,
            BuyPrice = SaleHelper.GetLastBuyPriceForGood(_param1.item.Good, (IEnumerable<Gbs.Core.Entities.Documents.Document>) docWaybill),
            Stock = _param1.item.Good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)),
            OrderQuantity = _param1.item.Count,
            SalePrice = _param1.item.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? _param1.item.Good.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)) : 0M
          }).ToList<GoodOrderItem>();
        }
      }

      public static IEnumerable<EmailItem> CreateEmailItemsData(IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
        return (IEnumerable<EmailItem>) documents.SelectMany(document => document.Items.GroupBy(x => new
        {
          GoodUid = x.GoodUid,
          SellPrice = x.SellPrice,
          Discount = x.Discount,
          ModificationUid = x.ModificationUid,
          Comment = x.Comment
        }), (document, item) => new
        {
          document = document,
          item = item
        }).Select(_param1 => new
        {
          \u003C\u003Eh__TransparentIdentifier0 = _param1,
          good = PrintableReportFactory.DataFactory.CreateGood(_param1.item.First<Gbs.Core.Entities.Documents.Item>().Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units)
        }).Select(_param1 => new EmailItem()
        {
          Good = _param1.good,
          Comment = _param1.\u003C\u003Eh__TransparentIdentifier0.item.First<Gbs.Core.Entities.Documents.Item>().Comment,
          Discount = _param1.\u003C\u003Eh__TransparentIdentifier0.item.First<Gbs.Core.Entities.Documents.Item>().Discount,
          Quantity = _param1.\u003C\u003Eh__TransparentIdentifier0.item.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)),
          Price = _param1.\u003C\u003Eh__TransparentIdentifier0.item.First<Gbs.Core.Entities.Documents.Item>().SellPrice,
          SaleDate = _param1.\u003C\u003Eh__TransparentIdentifier0.document.DateTime
        }).ToList<EmailItem>();
      }

      public static IEnumerable<SaleOrderItem> CreateSaleOrderItemsData(
        IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid> items)
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          new DocumentsRepository(dataBase).GetActiveItems();
          Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
          List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
          IEnumerable<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter();
          BuyPriceCounter buyPriceCounter = new BuyPriceCounter();
          return (IEnumerable<SaleOrderItem>) items.Select(item => new
          {
            item = item,
            good = PrintableReportFactory.DataFactory.CreateGood(item.Item.Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, units)
          }).Select(_param1 =>
          {
            SaleOrderItem saleOrderItemsData = new SaleOrderItem();
            saleOrderItemsData.ParentUid = _param1.item.Document.ParentUid;
            saleOrderItemsData.SaleNum = _param1.item.Document.Number;
            saleOrderItemsData.SaleDate = _param1.item.Document.DateTime;
            saleOrderItemsData.Good = _param1.good;
            saleOrderItemsData.Comment = _param1.item.Item.Comment;
            saleOrderItemsData.Discount = _param1.item.Item.Discount.Value;
            saleOrderItemsData.Quantity = _param1.item.Item.Quantity;
            saleOrderItemsData.SalePrice = _param1.item.Item.SalePrice;
            Guid? uid5 = _param1.item.Item.GoodModification?.Uid;
            Guid empty1 = Guid.Empty;
            saleOrderItemsData.GoodModificationName = (uid5.HasValue ? (uid5.GetValueOrDefault() != empty1 ? 1 : 0) : 1) != 0 ? _param1.item.Item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m =>
            {
              Guid uid6 = m.Uid;
              Guid? uid7 = _param1.item.Item.GoodModification?.Uid;
              return uid7.HasValue && uid6 == uid7.GetValueOrDefault();
            }))?.Name : "";
            Guid? uid8 = _param1.item.Item.GoodModification?.Uid;
            Guid empty2 = Guid.Empty;
            saleOrderItemsData.GoodModificationBarcode = (uid8.HasValue ? (uid8.GetValueOrDefault() != empty2 ? 1 : 0) : 1) != 0 ? _param1.item.Item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m =>
            {
              Guid uid9 = m.Uid;
              Guid? uid10 = _param1.item.Item.GoodModification?.Uid;
              return uid10.HasValue && uid9 == uid10.GetValueOrDefault();
            }))?.Barcode : "";
            return saleOrderItemsData;
          }).ToList<SaleOrderItem>();
        }
      }

      public static IEnumerable<WriteOffItem> CreateWriteOffItemsData(
        IEnumerable<Gbs.Core.Entities.Documents.Item> documentItems)
      {
        using (Data.GetDataBase())
        {
          List<WriteOffItem> writeOffItemsData = new List<WriteOffItem>();
          Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
          List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
          IEnumerable<GoodsUnits.GoodUnit> unitsListWithFilter = GoodsUnits.GetUnitsListWithFilter();
          BuyPriceCounter buyPriceCounter1 = new BuyPriceCounter();
          foreach (Gbs.Core.Entities.Documents.Item documentItem in documentItems)
          {
            Gbs.Core.Entities.Documents.Item item = documentItem;
            Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = PrintableReportFactory.DataFactory.CreateGood(item.Good, devicesConfig, (IEnumerable<EntityProperties.PropertyType>) typesList, unitsListWithFilter);
            WriteOffItem writeOffItem1 = new WriteOffItem();
            writeOffItem1.Good = good;
            writeOffItem1.Comment = item.Comment;
            writeOffItem1.Quantity = item.Quantity;
            writeOffItem1.SalePrice = item.SellPrice;
            BuyPriceCounter buyPriceCounter2 = buyPriceCounter1;
            GoodsStocks.GoodStock goodStock = item.GoodStock;
            // ISSUE: explicit non-virtual call
            Guid stockUid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
            writeOffItem1.BuyPrice = buyPriceCounter2.GetBuyPrice(stockUid);
            writeOffItem1.GoodModificationName = item.ModificationUid != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.ModificationUid))?.Name : "";
            writeOffItem1.GoodModificationBarcode = item.ModificationUid != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.ModificationUid))?.Barcode : "";
            WriteOffItem writeOffItem2 = writeOffItem1;
            writeOffItemsData.Add(writeOffItem2);
          }
          return (IEnumerable<WriteOffItem>) writeOffItemsData;
        }
      }

      public static IEnumerable<LabelItem> CreateLabelItemsData(
        IEnumerable<Gbs.Core.ViewModels.Basket.BasketItem> goodsList,
        bool isSplitQ)
      {
        List<LabelItem> labelItemsData = new List<LabelItem>();
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        IEnumerable<GoodsUnits.GoodUnit> unitsListWithFilter = GoodsUnits.GetUnitsListWithFilter();
        foreach (Gbs.Core.ViewModels.Basket.BasketItem goods in goodsList)
        {
          Gbs.Core.ViewModels.Basket.BasketItem item = goods;
          Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = PrintableReportFactory.DataFactory.CreateGood(item.Good, devicesConfig, (IEnumerable<EntityProperties.PropertyType>) typesList, unitsListWithFilter);
          for (int index = 0; (Decimal) index < item.Quantity; ++index)
          {
            LabelItem labelItem1 = new LabelItem();
            labelItem1.Good = good;
            labelItem1.DisplayName = item.DisplayedName;
            labelItem1.Price = item.SalePrice;
            labelItem1.Quantity = item.Quantity;
            GoodsModifications.GoodModification goodModification1 = item.GoodModification;
            // ISSUE: explicit non-virtual call
            labelItem1.GoodModificationName = (goodModification1 != null ? __nonvirtual (goodModification1.Uid) : Guid.Empty) != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x =>
            {
              Guid uid = x.Uid;
              GoodsModifications.GoodModification goodModification2 = item.GoodModification;
              // ISSUE: explicit non-virtual call
              Guid guid = goodModification2 != null ? __nonvirtual (goodModification2.Uid) : Guid.Empty;
              return uid == guid;
            }))?.Name : "";
            GoodsModifications.GoodModification goodModification3 = item.GoodModification;
            // ISSUE: explicit non-virtual call
            labelItem1.GoodModificationBarcode = (goodModification3 != null ? __nonvirtual (goodModification3.Uid) : Guid.Empty) != Guid.Empty ? item.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x =>
            {
              Guid uid = x.Uid;
              GoodsModifications.GoodModification goodModification4 = item.GoodModification;
              // ISSUE: explicit non-virtual call
              Guid guid = goodModification4 != null ? __nonvirtual (goodModification4.Uid) : Guid.Empty;
              return uid == guid;
            }))?.Barcode : "";
            LabelItem labelItem2 = labelItem1;
            labelItemsData.Add(labelItem2);
            if (!isSplitQ)
              break;
          }
        }
        return (IEnumerable<LabelItem>) labelItemsData;
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Clients.Client> CreateClientItemsData(
        IEnumerable<ClientAdnSum> clients)
      {
        List<Gbs.Helpers.FR.BackEnd.Entities.Clients.Client> clientItemsData = new List<Gbs.Helpers.FR.BackEnd.Entities.Clients.Client>();
        List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client);
        foreach (ClientAdnSum client1 in clients)
        {
          Gbs.Helpers.FR.BackEnd.Entities.Clients.Client client2 = PrintableReportFactory.DataFactory.CreateClient(client1, (IEnumerable<EntityProperties.PropertyType>) typesList);
          clientItemsData.Add(client2);
        }
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Clients.Client>) clientItemsData;
      }

      public static IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Document> CreateDocumentsData(
        IEnumerable<Gbs.Core.Entities.Documents.Document> documents)
      {
        List<Gbs.Helpers.FR.BackEnd.Entities.Document> documentsData = new List<Gbs.Helpers.FR.BackEnd.Entities.Document>();
        foreach (Gbs.Core.Entities.Documents.Document document1 in documents)
        {
          Gbs.Helpers.FR.BackEnd.Entities.Document document2 = PrintableReportFactory.DataFactory.CreateDocument(document1);
          documentsData.Add(document2);
        }
        return (IEnumerable<Gbs.Helpers.FR.BackEnd.Entities.Document>) documentsData;
      }

      public static IEnumerable<InventoryItem> CreateInventoryItemsData(
        IEnumerable<Gbs.Core.Entities.Documents.Item> itemList,
        IEnumerable<Gbs.Core.Entities.Documents.Item> actItems,
        bool isVisibilityStock,
        bool isClose)
      {
        Gbs.Core.Config.Devices configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        List<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
        List<GoodsUnits.GoodUnit> units = GoodsUnits.GetUnitsListWithFilter().ToList<GoodsUnits.GoodUnit>();
        List<IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>> list = itemList.GroupBy(x => new
        {
          GoodUid = x.GoodUid,
          ModificationUid = x.ModificationUid,
          SellPrice = x.SellPrice
        }).ToList<IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>>();
        List<IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>> groupActItems = actItems.GroupBy(x => new
        {
          GoodUid = x.GoodUid,
          ModificationUid = x.ModificationUid,
          SellPrice = x.SellPrice
        }).ToList<IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>>();
        foreach (IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item> grouping in list)
        {
          IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item> item = grouping;
          IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item> source = groupActItems.SingleOrDefault<IGrouping<\u003C\u003Ef__AnonymousType28<Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>>(actItemGroup => item.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (itemGroup => Predicte(itemGroup, actItemGroup.First<Gbs.Core.Entities.Documents.Item>(), isClose))));
          InventoryItem inventoryItem = new InventoryItem();
          inventoryItem.Good = PrintableReportFactory.DataFactory.CreateGood(item.First<Gbs.Core.Entities.Documents.Item>().Good, configDevices, (IEnumerable<EntityProperties.PropertyType>) propertyTypes, (IEnumerable<GoodsUnits.GoodUnit>) units);
          inventoryItem.Quantity = item.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
          Decimal num1;
          if (!isVisibilityStock)
          {
            num1 = 0M;
          }
          else
          {
            Decimal num2 = item.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            Decimal? nullable = source != null ? new Decimal?(source.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity))) : new Decimal?();
            num1 = (nullable.HasValue ? new Decimal?(num2 - nullable.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault();
          }
          inventoryItem.BaseQuantity = num1;
          inventoryItem.SalePrice = item.Key.SellPrice;
          inventoryItem.GoodModificationName = item.Key.ModificationUid != Guid.Empty ? item.First<Gbs.Core.Entities.Documents.Item>().Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.Key.ModificationUid))?.Name : "";
          inventoryItem.GoodModificationBarcode = item.Key.ModificationUid != Guid.Empty ? item.First<Gbs.Core.Entities.Documents.Item>().Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.Key.ModificationUid))?.Barcode : "";
          yield return inventoryItem;
        }

        static bool Predicte(Gbs.Core.Entities.Documents.Item x, Gbs.Core.Entities.Documents.Item actItem, bool iActClose)
        {
          if (iActClose)
            return x.GoodStock.Uid == actItem.GoodStock.Uid;
          return x.ModificationUid == actItem.ModificationUid && x.GoodUid == actItem.GoodUid && x.SellPrice == actItem.SellPrice;
        }
      }

      private static Gbs.Helpers.FR.BackEnd.Entities.Goods.Good CreateGood(
        Gbs.Core.Entities.Goods.Good good,
        Gbs.Core.Config.Devices devicesConfig,
        IEnumerable<EntityProperties.PropertyType> propertyTypes,
        IEnumerable<GoodsUnits.GoodUnit> units)
      {
        Gbs.Core.Config.FiscalKkm ndsConfig = devicesConfig.CheckPrinter.FiscalKkm;
        GoodGroups.Group group = good.Group;
        Gbs.Core.Config.FiscalKkm.TaxRate taxRate = (group != null ? (group.TaxRateNumber == 0 ? 1 : 0) : 0) != 0 || ndsConfig.TaxRates.FirstOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x =>
        {
          int key = x.Key;
          int? taxRateNumber = good.Group?.TaxRateNumber;
          int valueOrDefault = taxRateNumber.GetValueOrDefault();
          return key == valueOrDefault & taxRateNumber.HasValue;
        })).Value == null ? ndsConfig.TaxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == ndsConfig.DefaultTaxRate)).Value : ndsConfig.TaxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x =>
        {
          int key = x.Key;
          int? taxRateNumber = good.Group?.TaxRateNumber;
          int valueOrDefault = taxRateNumber.GetValueOrDefault();
          return key == valueOrDefault & taxRateNumber.HasValue;
        })).Value;
        GoodsUnits.GoodUnit goodUnit = units.SingleOrDefault<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (x =>
        {
          Guid uid = x.Uid;
          Guid? unitsUid = good.Group?.UnitsUid;
          return unitsUid.HasValue && uid == unitsUid.GetValueOrDefault();
        }));
        Unit unit = new Unit()
        {
          FullName = goodUnit?.FullName,
          ShortName = goodUnit?.ShortName,
          Code = goodUnit?.Code
        };
        Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good1 = new Gbs.Helpers.FR.BackEnd.Entities.Goods.Good()
        {
          Barcode = good.Barcode,
          Barcodes = string.Join("\n", good.Barcodes),
          Name = good.Name,
          Uid = good.Uid,
          UnitName = unit.FullName,
          Unit = unit,
          VatValue = 0M,
          Group = PrintableReportFactory.DataFactory.CreateGoodGroup(good.Group, devicesConfig),
          NdsName = taxRate.Name,
          NdsValue = taxRate.TaxValue,
          Description = good.Description,
          TotalCount = good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)) : 0M
        };
        Dictionary<string, object> d = good.Properties.ToDictionary<EntityProperties.PropertyValue, string, object>((Func<EntityProperties.PropertyValue, string>) (p => p.Type.Uid.ToString()), (Func<EntityProperties.PropertyValue, object>) (p => p.Value));
        foreach (EntityProperties.PropertyType propertyType in propertyTypes.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (t => !d.Any<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (di => di.Key == t.Uid.ToString())))))
          d.Add(propertyType.Uid.ToString(), (object) null);
        good1.Properties = d;
        object obj = d[GlobalDictionaries.GoodIdUid.ToString()];
        good1.Id = obj == null ? 0 : Convert.ToInt32(obj);
        return good1;
      }

      private static GoodGroup CreateGoodGroup(GoodGroups.Group group, Gbs.Core.Config.Devices devicesConfig)
      {
        Gbs.Core.Config.FiscalKkm ndsConfig = devicesConfig.CheckPrinter.FiscalKkm;
        Gbs.Core.Config.FiscalKkm.TaxRate taxRate = group.TaxRateNumber == 0 || ndsConfig.TaxRates.FirstOrDefault<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == group.TaxRateNumber)).Value == null ? ndsConfig.TaxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == ndsConfig.DefaultTaxRate)).Value : ndsConfig.TaxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == group.TaxRateNumber)).Value;
        Dictionary<GlobalDictionaries.GoodTypes, string> source1 = GlobalDictionaries.GoodTypesDictionary();
        Dictionary<GlobalDictionaries.RuTaxSystems, string> source2 = GlobalDictionaries.RuTaxSystemsDictionary();
        Dictionary<GlobalDictionaries.RuFfdGoodsTypes, string> source3 = GlobalDictionaries.RuFfdGoodsTypesDictionary();
        List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>> productionTypesList = GlobalDictionaries.MarkedProductionTypesList;
        return new GoodGroup()
        {
          Name = group.Name,
          Uid = group.Uid,
          GoodsType = source1.SingleOrDefault<KeyValuePair<GlobalDictionaries.GoodTypes, string>>((Func<KeyValuePair<GlobalDictionaries.GoodTypes, string>, bool>) (x => x.Key == group.GoodsType)).Value,
          IsDataParent = group.IsDataParent,
          UnitsUid = group.UnitsUid,
          IsRequestCount = group.IsRequestCount,
          TaxRateNumber = group.TaxRateNumber,
          ParentGroupUid = group.ParentGroupUid,
          RuTaxSystem = source2.FirstOrDefault<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>>((Func<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>, bool>) (x => x.Key == group.RuTaxSystem)).Value ?? source2.First<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>>((Func<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>, bool>) (x => x.Key == GlobalDictionaries.RuTaxSystems.None)).Value,
          DecimalPlace = group.DecimalPlace,
          KkmSectionNumber = group.KkmSectionNumber,
          RuFfdGoodsType = source3.Single<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>>((Func<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>, bool>) (x => x.Key == group.RuFfdGoodsType)).Value,
          IsFreePrice = group.IsFreePrice,
          NeedComment = group.NeedComment,
          RuMarkedProductionType = productionTypesList.Single<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Type == group.RuMarkedProductionType)).TypeName,
          TaxRateNumberText = taxRate.Name
        };
      }

      private static Gbs.Helpers.FR.BackEnd.Entities.Clients.Client CreateClient(
        ClientAdnSum client,
        IEnumerable<EntityProperties.PropertyType> propertyTypes)
      {
        if (client == null)
          return new Gbs.Helpers.FR.BackEnd.Entities.Clients.Client();
        if (client.Client == null)
          return new Gbs.Helpers.FR.BackEnd.Entities.Clients.Client();
        Gbs.Helpers.FR.BackEnd.Entities.Clients.Client client1 = new Gbs.Helpers.FR.BackEnd.Entities.Clients.Client()
        {
          Barcode = client.Client.Barcode,
          Group = new ClientGroups()
          {
            Name = client.Client.Group.Name,
            Discount = client.Client.Group.Discount,
            MaxSumCredit = client.Client.Group.MaxSumCredit
          },
          Name = client.Client.Name,
          Address = client.Client.Address,
          Comment = client.Client.Address,
          Phone = client.Client.Phone,
          Bonuses = client.TotalBonusSum,
          Birthday = client.Client.Birthday,
          Email = client.Client.Email,
          SalesSum = client.TotalSalesSum,
          CreditSum = client.TotalCreditSum
        };
        Dictionary<string, object> d = client.Client.Properties.ToDictionary<EntityProperties.PropertyValue, string, object>((Func<EntityProperties.PropertyValue, string>) (p => p.Type.Uid.ToString()), (Func<EntityProperties.PropertyValue, object>) (p => p.Value));
        foreach (EntityProperties.PropertyType propertyType in propertyTypes.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (t => !d.Any<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (di => di.Key == t.Uid.ToString())))))
          d.Add(propertyType.Uid.ToString(), (object) null);
        client1.Properties = d;
        return client1;
      }

      private static Gbs.Helpers.FR.BackEnd.Entities.Document CreateDocument(Gbs.Core.Entities.Documents.Document document)
      {
        if (document == null)
          return new Gbs.Helpers.FR.BackEnd.Entities.Document();
        return new Gbs.Helpers.FR.BackEnd.Entities.Document()
        {
          Date = document.DateTime,
          Comment = document.Comment,
          ContractorUid = document.ContractorUid,
          Number = document.Number,
          Uid = document.Uid
        };
      }
    }

    private class PropertiesFactory : IFactory<IReportProperty>
    {
      public static IEnumerable<IReportProperty> Create(Gbs.Helpers.FR.BackEnd.Entities.SummaryReport report)
      {
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new DateFinishReport((object) report.DateFinish),
          (IReportProperty) new DateStartReport((object) report.DateStart),
          (IReportProperty) new MoneyIncomeSum((object) report.MoneyIncomeSum),
          (IReportProperty) new MoneyOutcomeSum((object) report.MoneyOutcomeSum),
          (IReportProperty) new SumCash((object) report.SumCash),
          (IReportProperty) new TotalGoods((object) report.TotalGoods),
          (IReportProperty) new TotalReturnCount((object) report.TotalReturnCount),
          (IReportProperty) new TotalReturnsSum((object) report.TotalReturnsSum),
          (IReportProperty) new TotalSaleSum((object) report.TotalSaleSum),
          (IReportProperty) new TotalSalesCount((object) report.TotalSalesCount),
          (IReportProperty) new IncomeSum((object) report.IncomeSum),
          (IReportProperty) new DiscountsSum((object) report.DiscountsSum),
          (IReportProperty) new CreditPaymentsSum((object) report.CreditPaymentsSum),
          (IReportProperty) new TotalCreditSum((object) report.TotalCreditSum)
        };
      }

      public static IEnumerable<IReportProperty> Create(Gbs.Core.Entities.Documents.Document document)
      {
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new DocumentDate((object) document.DateTime),
          (IReportProperty) new DocumentNum((object) document.Number),
          (IReportProperty) new DocumentComment((object) document.Comment),
          (IReportProperty) new IsFiscal((object) document.IsFiscal),
          (IReportProperty) new TableNum(document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.NumTableUid))?.Value ?? (object) string.Empty),
          (IReportProperty) new GuestsCount(document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CountGuestUid))?.Value ?? (object) string.Empty)
        };
      }

      public static IEnumerable<IReportProperty> Create(ClientAdnSum client)
      {
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new ClientName((object) client.Client.Name),
          (IReportProperty) new ClientPhone((object) client.Client.Phone),
          (IReportProperty) new ClientBarcode((object) client.Client.Barcode),
          (IReportProperty) new ClientAddress((object) client.Client.Address),
          (IReportProperty) new ClientBank((object) GetProperty(GlobalDictionaries.BankNameUid)),
          (IReportProperty) new ClientInn((object) GetProperty(GlobalDictionaries.InnUid)),
          (IReportProperty) new ClientKPP((object) GetProperty(GlobalDictionaries.KppUid)),
          (IReportProperty) new ClientKs((object) GetProperty(GlobalDictionaries.KsUid)),
          (IReportProperty) new ClientBik((object) GetProperty(GlobalDictionaries.BikUid)),
          (IReportProperty) new ClientOGRN((object) GetProperty(GlobalDictionaries.OgrnUid)),
          (IReportProperty) new ClientRs((object) GetProperty(GlobalDictionaries.RsUid)),
          (IReportProperty) new ClientEmail((object) client.Client.Email),
          (IReportProperty) new ClientBonusesSum((object) client.TotalBonusSum),
          (IReportProperty) new ClientCreditSum((object) client.TotalCreditSum),
          (IReportProperty) new ClientComment((object) client.Client.Comment),
          (IReportProperty) new ClientBirthday((object) client.Client.Birthday)
        };

        string GetProperty(Guid uid)
        {
          return client.Client.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uid))?.Value.ToString() ?? string.Empty;
        }
      }

      public static IEnumerable<IReportProperty> Create(Gbs.Core.Entities.Users.User user)
      {
        bool userNameForPrint = new ConfigsRepository<Settings>().Get().Users.IsCutUserNameForPrint;
        string fullName = user?.Client?.Name ?? new string(' ', 10);
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new UserName(userNameForPrint ? (object) Functions.CutName(fullName) : (object) fullName),
          (IReportProperty) new UserPhone((object) (user?.Client?.Phone ?? ""))
        };
      }

      public static IEnumerable<IReportProperty> Create(DateTime dateStart, DateTime dateFinish)
      {
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new DateStartReport((object) dateStart),
          (IReportProperty) new DateFinishReport((object) dateFinish)
        };
      }

      public static IEnumerable<IReportProperty> Create(SalePoints.SalePoint salePoint)
      {
        return (IEnumerable<IReportProperty>) new List<IReportProperty>()
        {
          (IReportProperty) new CompanyAddress((object) salePoint.Description.Adress),
          (IReportProperty) new CompanyName((object) salePoint.Organization.Name),
          (IReportProperty) new CompanyPhone((object) salePoint.Description.Phone),
          (IReportProperty) new CompanyPublicPointName((object) salePoint.Description.NamePoint),
          (IReportProperty) new CompanyInfo((object) salePoint.Description.ExtraInfo),
          (IReportProperty) new CompanyInn((object) GetProperty(GlobalDictionaries.InnUid)),
          (IReportProperty) new CompanyKPP((object) GetProperty(GlobalDictionaries.KppUid)),
          (IReportProperty) new CompanyOGRN((object) GetProperty(GlobalDictionaries.OgrnUid)),
          (IReportProperty) new CompanyBank((object) GetProperty(GlobalDictionaries.BankNameUid)),
          (IReportProperty) new CompanyBik((object) GetProperty(GlobalDictionaries.BikUid)),
          (IReportProperty) new CompanyKs((object) GetProperty(GlobalDictionaries.KsUid)),
          (IReportProperty) new CompanyRs((object) GetProperty(GlobalDictionaries.RsUid)),
          (IReportProperty) new DbName(UidDb.GetUid().Value)
        };

        string GetProperty(Guid uid)
        {
          return salePoint.Organization.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uid))?.Value.ToString() ?? string.Empty;
        }
      }
    }
  }
}
