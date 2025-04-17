// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Market5.Market5ImportHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Helpers.Market5
{
  public static class Market5ImportHelper
  {
    private static void GetSale()
    {
      try
      {
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_продаж, Market5ImportHelper.ImportData.sales.Count);
        List<Document> source1 = new List<Document>();
        Sections.Section currentSection = Sections.GetCurrentSection();
        Storages.Storage storage = Storages.GetStorages().First<Storages.Storage>();
        PaymentsAccounts.PaymentsAccount paymentsAccount1 = new PaymentsAccounts.PaymentsAccount()
        {
          Name = Translate.Market5ImportHelper_GetSale_Денежный_ящик__v_5_,
          Type = PaymentsAccounts.MoneyType.Cash
        };
        PaymentsAccounts.PaymentsAccount paymentsAccount2 = new PaymentsAccounts.PaymentsAccount()
        {
          Name = Translate.Market5ImportHelper_GetSale_Карта__v_5_,
          Type = PaymentsAccounts.MoneyType.Card
        };
        PaymentsAccounts.PaymentsAccount paymentsAccount3 = new PaymentsAccounts.PaymentsAccount()
        {
          Name = Translate.Market5ImportHelper_GetSale_Банк__из_5_й_версии_,
          Type = PaymentsAccounts.MoneyType.Bank
        };
        PaymentMethods.PaymentMethod cash = new PaymentMethods.PaymentMethod()
        {
          Name = Translate.Market5ImportHelper_Наличными__v_5_,
          AccountUid = paymentsAccount1.Uid,
          KkmMethod = GlobalDictionaries.KkmPaymentMethods.Cash
        };
        PaymentMethods.PaymentMethod card = new PaymentMethods.PaymentMethod()
        {
          Name = Translate.Market5ImportHelper_Картой__v_5_,
          AccountUid = paymentsAccount2.Uid,
          KkmMethod = GlobalDictionaries.KkmPaymentMethods.Card
        };
        PaymentMethods.PaymentMethod bank = new PaymentMethods.PaymentMethod()
        {
          Name = Translate.Market5ImportHelper_Безнал__v_5_,
          AccountUid = paymentsAccount3.Uid,
          KkmMethod = GlobalDictionaries.KkmPaymentMethods.Bank
        };
        if (!card.Save() || !cash.Save() || !bank.Save() || !paymentsAccount1.Save() || !paymentsAccount2.Save() || !paymentsAccount3.Save())
        {
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_НЕ_УДАЛОСЬ_СОХРАНИТЬ_ПРОДАЖИ, 0);
        }
        else
        {
          List<GoodsStocks.GoodStock> goodsStocks = GoodsStocks.GetGoodStockList();
          foreach (Market5Data.Sale sale1 in Market5ImportHelper.ImportData.sales)
          {
            Market5Data.Sale sale = sale1;
            int num = Market5ImportHelper.ProgressInfo.CurrentTaskStep++;
            Document document = new Document();
            document.IsDeleted = sale.is_deleted;
            document.Uid = Guid.NewGuid();
            document.Type = GlobalDictionaries.DocumentsTypes.Sale;
            document.DateTime = sale.date_time;
            document.Comment = sale.comment;
            Market5ImportHelper.LinkImport5 linkImport5_1 = Market5ImportHelper.ListClients.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (x => x.Id == sale.client_id));
            document.ContractorUid = linkImport5_1 != null ? linkImport5_1.Uid : Guid.Empty;
            Market5ImportHelper.LinkImport5 linkImport5_2 = Market5ImportHelper.ListUser.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (x => x.Id == sale.user_id));
            document.UserUid = linkImport5_2 != null ? linkImport5_2.Uid : Guid.Empty;
            num = sale.id;
            document.Number = num.ToString();
            document.Section = currentSection;
            document.Status = GlobalDictionaries.DocumentsStatuses.Close;
            document.Storage = storage;
            Document saleDoc = document;
            IEnumerable<Market5Data.SaleItem> source2 = Market5ImportHelper.ImportData.sale_items.Where<Market5Data.SaleItem>((Func<Market5Data.SaleItem, bool>) (x => x.sale_id == sale.id));
            saleDoc.Items = source2.Select<Market5Data.SaleItem, Gbs.Core.Entities.Documents.Item>((Func<Market5Data.SaleItem, Gbs.Core.Entities.Documents.Item>) (x =>
            {
              Gbs.Core.Entities.Documents.Item sale2 = new Gbs.Core.Entities.Documents.Item();
              Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
              Market5ImportHelper.LinkImport5 linkImport5_4 = Market5ImportHelper.GoodLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (g => g.Id == x.good_id));
              good.Uid = linkImport5_4 != null ? linkImport5_4.Uid : Guid.Empty;
              sale2.Good = good;
              sale2.Comment = x.comment;
              sale2.GoodStock = goodsStocks.FirstOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s =>
              {
                Guid goodUid = s.GoodUid;
                Market5ImportHelper.LinkImport5 linkImport5_5 = Market5ImportHelper.GoodLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (g => g.Id == x.good_id));
                Guid guid = linkImport5_5 != null ? linkImport5_5.Uid : Guid.Empty;
                return goodUid == guid;
              }));
              sale2.DocumentUid = saleDoc.Uid;
              sale2.Quantity = x.count;
              sale2.SellPrice = x.price;
              sale2.Discount = x.discount;
              sale2.BuyPrice = x.in_price1;
              Market5ImportHelper.LinkImport5 linkImport5_6 = Market5ImportHelper.ModificationLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (m => m.Id == x.mod_id));
              sale2.ModificationUid = linkImport5_6 != null ? linkImport5_6.Uid : Guid.Empty;
              sale2.IsDeleted = x.is_deleted;
              return sale2;
            })).ToList<Gbs.Core.Entities.Documents.Item>();
            IEnumerable<Market5Data.SalePayment> source3 = Market5ImportHelper.ImportData.sales_payments.Where<Market5Data.SalePayment>((Func<Market5Data.SalePayment, bool>) (x => x.sale_id == sale.id));
            saleDoc.Payments = source3.Select<Market5Data.SalePayment, Gbs.Core.Entities.Payments.Payment>((Func<Market5Data.SalePayment, Gbs.Core.Entities.Payments.Payment>) (x =>
            {
              Gbs.Core.Entities.Payments.Payment sale3 = new Gbs.Core.Entities.Payments.Payment();
              sale3.Date = x.date_time;
              sale3.ParentUid = saleDoc.Uid;
              Gbs.Core.Entities.Payments.Payment payment = sale3;
              PaymentMethods.PaymentMethod paymentMethod;
              switch (x.method)
              {
                case 0:
                  paymentMethod = cash;
                  break;
                case 1:
                  paymentMethod = card;
                  break;
                case 2:
                  paymentMethod = bank;
                  break;
                case 3:
                  paymentMethod = new PaymentMethods.PaymentMethod()
                  {
                    Uid = GlobalDictionaries.BonusesPaymentUid
                  };
                  break;
                case 4:
                  paymentMethod = new PaymentMethods.PaymentMethod()
                  {
                    Uid = GlobalDictionaries.CertificateNominalUid
                  };
                  break;
                default:
                  paymentMethod = cash;
                  break;
              }
              payment.Method = paymentMethod;
              sale3.SumIn = x.summ;
              sale3.Comment = x.comment ?? string.Empty;
              sale3.IsDeleted = x.is_deleted;
              return sale3;
            })).ToList<Gbs.Core.Entities.Payments.Payment>();
            source1.Add(saleDoc);
          }
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_продаж_в_базу_данных, source1.Count);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            DocumentsRepository dr = new DocumentsRepository(dataBase);
            source1.AsParallel<Document>().ForAll<Document>((Action<Document>) (x =>
            {
              if (!dr.Save(x, false))
                return;
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            }));
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта продаж из GBS.Market 5");
      }
    }

    public static void GetWaybill()
    {
      try
      {
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_накладных, Market5ImportHelper.ImportData.waybills.Count);
        List<Document> source1 = new List<Document>();
        Sections.Section currentSection = Sections.GetCurrentSection();
        Storages.Storage storage = Storages.GetStorages().First<Storages.Storage>();
        foreach (Market5Data.Waybill waybill1 in Market5ImportHelper.ImportData.waybills)
        {
          Market5Data.Waybill waybill = waybill1;
          ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          Document document = new Document();
          document.IsDeleted = waybill.is_deleted;
          document.Uid = Guid.NewGuid();
          document.Type = GlobalDictionaries.DocumentsTypes.Buy;
          document.DateTime = waybill.date_time;
          document.Comment = waybill.comment;
          Market5ImportHelper.LinkImport5 linkImport5_1 = Market5ImportHelper.SuppliersLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (x => x.Id == waybill.supplier_id));
          document.ContractorUid = linkImport5_1 != null ? linkImport5_1.Uid : Guid.Empty;
          Market5ImportHelper.LinkImport5 linkImport5_2 = Market5ImportHelper.ListUser.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (x => x.Id == waybill.user_id));
          document.UserUid = linkImport5_2 != null ? linkImport5_2.Uid : Guid.Empty;
          document.Number = waybill.number;
          document.Section = currentSection;
          document.Status = GlobalDictionaries.DocumentsStatuses.Close;
          document.Storage = storage;
          Document waybillDoc = document;
          IEnumerable<Market5Data.WaybillItem> source2 = Market5ImportHelper.ImportData.waybill_items.Where<Market5Data.WaybillItem>((Func<Market5Data.WaybillItem, bool>) (x => x.waybill_id == waybill.id));
          waybillDoc.Items = source2.Select<Market5Data.WaybillItem, Gbs.Core.Entities.Documents.Item>((Func<Market5Data.WaybillItem, Gbs.Core.Entities.Documents.Item>) (x =>
          {
            Gbs.Core.Entities.Documents.Item waybill2 = new Gbs.Core.Entities.Documents.Item();
            waybill2.Good = Market5ImportHelper.ListGoods.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (good =>
            {
              Guid uid = good.Uid;
              Market5ImportHelper.LinkImport5 linkImport5_4 = Market5ImportHelper.GoodLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (g => g.Id == x.good_id));
              Guid guid = linkImport5_4 != null ? linkImport5_4.Uid : Guid.Empty;
              return uid == guid;
            }));
            waybill2.DocumentUid = waybillDoc.Uid;
            waybill2.Quantity = x.income;
            Market5Data.Good good1 = Market5ImportHelper.ImportData.goods.FirstOrDefault<Market5Data.Good>((Func<Market5Data.Good, bool>) (g => g.Id == x.good_id));
            waybill2.SellPrice = good1 != null ? good1.price : 0M;
            waybill2.BuyPrice = x.in_price;
            Market5ImportHelper.LinkImport5 linkImport5_5 = Market5ImportHelper.ModificationLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (m => m.Id == x.mod_id));
            waybill2.ModificationUid = linkImport5_5 != null ? linkImport5_5.Uid : Guid.Empty;
            waybill2.IsDeleted = x.is_deleted;
            GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock();
            Market5ImportHelper.LinkImport5 linkImport5_6 = Market5ImportHelper.GoodLinks.FirstOrDefault<Market5ImportHelper.LinkImport5>((Func<Market5ImportHelper.LinkImport5, bool>) (g => g.Id == x.good_id));
            goodStock.GoodUid = linkImport5_6 != null ? linkImport5_6.Uid : Guid.Empty;
            Market5Data.Good good2 = Market5ImportHelper.ImportData.goods.FirstOrDefault<Market5Data.Good>((Func<Market5Data.Good, bool>) (g => g.Id == x.good_id));
            goodStock.Price = good2 != null ? good2.price : 0M;
            goodStock.Storage = storage;
            waybill2.GoodStock = goodStock;
            return waybill2;
          })).ToList<Gbs.Core.Entities.Documents.Item>();
          source1.Add(waybillDoc);
        }
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_накладных_в_базу_данных, source1.Count);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          DocumentsRepository dr = new DocumentsRepository(dataBase);
          source1.AsParallel<Document>().ForAll<Document>((Action<Document>) (x =>
          {
            if (!dr.Save(x, false))
              return;
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          }));
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта накладных из GBS.Market 5");
      }
    }

    private static List<Market5ImportHelper.LinkImport5> ListClients { get; set; }

    private static List<Market5ImportHelper.LinkImport5> ListUser { get; set; }

    private static void GetUsers()
    {
      try
      {
        UserGroups.UserGroup userGroup = new UserGroups.UserGroup()
        {
          Name = Translate.Market5ImportHelper_GetUsers_Сотрудники_Market5,
          IsSuper = false
        };
        userGroup.Save();
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetUsers_Загрузка_сотрудников, Market5ImportHelper.ImportData.Users.Count);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Clients.Client> list = new ClientsRepository(dataBase).GetAllItems().ToList<Gbs.Core.Entities.Clients.Client>();
          List<Gbs.Core.Entities.Users.User> userList = new List<Gbs.Core.Entities.Users.User>();
          Random random = new Random();
          Market5ImportHelper.ListUser = new List<Market5ImportHelper.LinkImport5>();
          foreach (Market5Data.User5 user1 in Market5ImportHelper.ImportData.Users)
          {
            Market5Data.User5 user5 = user1;
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            Market5Data.Client client5 = Market5ImportHelper.ImportData.Clients.First<Market5Data.Client>((Func<Market5Data.Client, bool>) (x => x.Id == user5.Client_id));
            Gbs.Core.Entities.Users.User user2 = new Gbs.Core.Entities.Users.User();
            user2.Alias = user5.alias;
            user2.DateIn = user5.date_in;
            user2.Barcode = string.Empty;
            user2.Client = list.Single<Gbs.Core.Entities.Clients.Client>((Func<Gbs.Core.Entities.Clients.Client, bool>) (u => u.Uid == client5.uid));
            user2.DateOut = new DateTime?(user5.date_out);
            user2.IsDeleted = user5.is_deleted;
            user2.Uid = Guid.NewGuid();
            user2.Password = random.Next(100000, 999999).ToString("000000");
            user2.Group = userGroup;
            user2.IsKicked = user5.is_kicked;
            user2.IsOnline = false;
            user2.OnlineOnSectionUid = Guid.Empty;
            Gbs.Core.Entities.Users.User user3 = user2;
            Market5ImportHelper.ListUser.Add(new Market5ImportHelper.LinkImport5()
            {
              Uid = user3.Uid,
              Id = user5.Id
            });
            userList.Add(user3);
          }
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetUsers_Сохранение_сотрудников_в_базу_данных, userList.Count);
          foreach (Gbs.Core.Entities.Users.User user in userList)
          {
            if (new UsersRepository(dataBase).Save(user))
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта сотрудников из GBS.Market 5");
      }
    }

    private static void GetGroupsClients()
    {
      try
      {
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetGroupsClients_Загрузка_групп_контактов, Market5ImportHelper.ImportData.clients_groups.Count);
        List<Gbs.Core.Entities.Clients.Group> groupList = new List<Gbs.Core.Entities.Clients.Group>();
        foreach (Market5Data.ClientsGroups clientsGroup in Market5ImportHelper.ImportData.clients_groups)
        {
          ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          Gbs.Core.Entities.Clients.Group group1 = new Gbs.Core.Entities.Clients.Group();
          group1.Uid = clientsGroup.uid;
          group1.Name = clientsGroup.Name;
          group1.Discount = clientsGroup.discount;
          group1.MaxSumCredit = new Decimal?(clientsGroup.maxCreditSum);
          group1.IsNonUseBonus = clientsGroup.nonUseBonus;
          group1.IsDeleted = clientsGroup.is_deleted;
          Gbs.Core.Entities.Clients.Group group2 = group1;
          groupList.Add(group2);
        }
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetGroupsClients_Сохранение_групп_контактов_в_базе_данных, groupList.Count);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GroupRepository groupRepository = new GroupRepository(dataBase);
          foreach (Gbs.Core.Entities.Clients.Group group in groupList)
          {
            if (groupRepository.Save(group))
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          }
          dataBase.Dispose();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта групп покупателей из 5й версии");
      }
    }

    private static (Decimal SaleSum, Decimal PaymentSum, Decimal Bonuses) GetClientOldSum(
      int clientId)
    {
      try
      {
        List<Market5Data.Sale> list = Market5ImportHelper.ImportData.sales.Where<Market5Data.Sale>((Func<Market5Data.Sale, bool>) (sale => sale.client_id == clientId && !sale.is_deleted)).ToList<Market5Data.Sale>();
        List<Market5Data.SaleItem> saleItems = new List<Market5Data.SaleItem>();
        foreach (Market5Data.Sale sale1 in list)
        {
          Market5Data.Sale sale = sale1;
          saleItems.AddRange(Market5ImportHelper.ImportData.sale_items.Where<Market5Data.SaleItem>((Func<Market5Data.SaleItem, bool>) (si => !si.is_deleted && si.sale_id == sale.id)));
        }
        Decimal num1 = saleItems.Sum<Market5Data.SaleItem>((Func<Market5Data.SaleItem, Decimal>) (si =>
        {
          Decimal num2 = Market5ImportHelper.ImportData.sale_return_items.Where<Market5Data.SaleReturnItem>((Func<Market5Data.SaleReturnItem, bool>) (sri => sri.sale_item_id == si.id && !Market5ImportHelper.ImportData.sale_returns.Single<Market5Data.SaleReturn>((Func<Market5Data.SaleReturn, bool>) (sr => sr.id == sri.return_id)).is_deleted && !sri.is_deleted)).Sum<Market5Data.SaleReturnItem>((Func<Market5Data.SaleReturnItem, Decimal>) (sri => sri.count));
          Decimal num3 = si.count - num2;
          Decimal num4 = Math.Round(num3 * si.price * si.discount / 100M, 2, MidpointRounding.AwayFromZero);
          return Math.Round(num3 * si.price, 2, MidpointRounding.AwayFromZero) - num4;
        })) - list.Sum<Market5Data.Sale>((Func<Market5Data.Sale, Decimal>) (s => s.discount));
        List<Market5Data.SalePayment> source1 = new List<Market5Data.SalePayment>();
        foreach (Market5Data.Sale sale2 in list)
        {
          Market5Data.Sale sale = sale2;
          source1.AddRange(Market5ImportHelper.ImportData.sales_payments.Where<Market5Data.SalePayment>((Func<Market5Data.SalePayment, bool>) (sp => !sp.is_deleted && sp.sale_id == sale.id)));
        }
        Decimal num5 = source1.Sum<Market5Data.SalePayment>((Func<Market5Data.SalePayment, Decimal>) (sp => sp.summ));
        List<Market5Data.Bonus> source2 = new List<Market5Data.Bonus>();
        foreach (Market5Data.SaleItem saleItem1 in saleItems)
        {
          Market5Data.SaleItem saleItem = saleItem1;
          source2.AddRange(Market5ImportHelper.ImportData.deposit_journal.Where<Market5Data.Bonus>((Func<Market5Data.Bonus, bool>) (dj => saleItem.id == dj.sale_item_id && !dj.is_deleted)));
        }
        Decimal d = source2.Sum<Market5Data.Bonus>((Func<Market5Data.Bonus, Decimal>) (b => b.points / saleItems.Single<Market5Data.SaleItem>((Func<Market5Data.SaleItem, bool>) (si => si.id == b.sale_item_id)).count * (saleItems.Single<Market5Data.SaleItem>((Func<Market5Data.SaleItem, bool>) (si => si.id == b.sale_item_id)).count - Market5ImportHelper.ImportData.sale_return_items.Where<Market5Data.SaleReturnItem>((Func<Market5Data.SaleReturnItem, bool>) (sri => sri.sale_item_id == b.sale_item_id && !sri.is_deleted && !Market5ImportHelper.ImportData.sale_returns.Single<Market5Data.SaleReturn>((Func<Market5Data.SaleReturn, bool>) (sr => sr.id == sri.return_id)).is_deleted)).Sum<Market5Data.SaleReturnItem>((Func<Market5Data.SaleReturnItem, Decimal>) (sri => sri.count))))) - source1.Where<Market5Data.SalePayment>((Func<Market5Data.SalePayment, bool>) (p => p.method == 3)).Sum<Market5Data.SalePayment>((Func<Market5Data.SalePayment, Decimal>) (p => p.summ));
        return (Math.Round(num1 + Market5ImportHelper.ImportData.Clients.Single<Market5Data.Client>((Func<Market5Data.Client, bool>) (c => c.Id == clientId)).startSaleSumm, 2), Math.Round(num5 + Market5ImportHelper.ImportData.Clients.Single<Market5Data.Client>((Func<Market5Data.Client, bool>) (c => c.Id == clientId)).startSaleSumm, 2), Math.Round(d, 2));
      }
      catch (Exception ex)
      {
        Other.ConsoleWrite(ex.ToString());
        return (0M, 0M, 0M);
      }
    }

    private static void GetClients()
    {
      try
      {
        Market5ImportHelper.GetGroupsClients();
        GoodGroups.Group group1 = new GoodGroups.Group();
        group1.Name = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5;
        group1.IsDeleted = true;
        GoodGroups.Group group2 = group1;
        using (Gbs.Core.Db.DataBase dataBase1 = Data.GetDataBase())
        {
          new GoodGroupsRepository(dataBase1).Save(group2);
          Storages.Storage storage1 = new Storages.Storage();
          storage1.Name = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5;
          storage1.IsDeleted = true;
          Storages.Storage storage2 = storage1;
          storage2.Save();
          Gbs.Core.Entities.Goods.Good good1 = new Gbs.Core.Entities.Goods.Good();
          good1.Name = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5;
          good1.StocksAndPrices = new List<GoodsStocks.GoodStock>()
          {
            new GoodsStocks.GoodStock()
            {
              Price = 0M,
              Stock = 999999M,
              Storage = storage2
            }
          };
          good1.Group = group2;
          good1.IsDeleted = true;
          Gbs.Core.Entities.Goods.Good good2 = good1;
          new GoodRepository(dataBase1).Save(good2);
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_контактов, Market5ImportHelper.ImportData.Clients.Count);
          List<Gbs.Core.Entities.Clients.Client> source = new List<Gbs.Core.Entities.Clients.Client>();
          List<EntityProperties.PropertyType> list1 = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client).ToList<EntityProperties.PropertyType>();
          GroupRepository groupRepository = new GroupRepository(dataBase1);
          List<Gbs.Core.Entities.Clients.Group> list2 = groupRepository.GetAllItems().ToList<Gbs.Core.Entities.Clients.Group>();
          Gbs.Core.Entities.Clients.Group group3 = new Gbs.Core.Entities.Clients.Group();
          group3.Name = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5;
          group3.IsDeleted = true;
          Gbs.Core.Entities.Clients.Group group4 = group3;
          groupRepository.Save(group4);
          List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client);
          Market5ImportHelper.ListClients = new List<Market5ImportHelper.LinkImport5>();
          foreach (Market5Data.Client client1 in Market5ImportHelper.ImportData.Clients)
          {
            Market5Data.Client client = client1;
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            Gbs.Core.Entities.Clients.Group group5 = list2.Any<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (g => g.Uid == Market5ImportHelper.ImportData.clients_groups.First<Market5Data.ClientsGroups>((Func<Market5Data.ClientsGroups, bool>) (x => x.Id == client.group_id)).uid)) ? list2.First<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (g => g.Uid == Market5ImportHelper.ImportData.clients_groups.First<Market5Data.ClientsGroups>((Func<Market5Data.ClientsGroups, bool>) (x => x.Id == client.group_id)).uid)) : group4;
            DateTime result;
            DateTime.TryParse(client.date_in, out result);
            Gbs.Core.Entities.Clients.Client client2 = new Gbs.Core.Entities.Clients.Client();
            client2.Uid = client.uid;
            client2.Name = client.name;
            client2.Group = group5;
            client2.Address = string.IsNullOrEmpty(client.address) ? string.Empty : client.address;
            client2.Barcode = string.IsNullOrEmpty(client.card_barcode) ? string.Empty : client.card_barcode;
            client2.Email = string.IsNullOrEmpty(client.email) ? string.Empty : client.email;
            client2.Phone = string.IsNullOrEmpty(client.phone) ? string.Empty : client.phone;
            client2.Birthday = new DateTime?(client.birthday);
            client2.IsDeleted = client.is_deleted;
            client2.DateAdd = result;
            Gbs.Core.Entities.Clients.Client client3 = client2;
            List<EntityProperties.PropertyValue> list3 = client3.Properties.ToList<EntityProperties.PropertyValue>();
            Market5ImportHelper.AddNewProp("ext1", client.uid, client.ext1, ref list1, ref list3);
            Market5ImportHelper.AddNewProp("ext2", client.uid, client.ext2, ref list1, ref list3);
            Market5ImportHelper.AddNewProp("ext3", client.uid, client.ext3, ref list1, ref list3);
            EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.InnUid)),
              Value = (object) client.inn
            };
            list3.Add(propertyValue1);
            EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.KppUid)),
              Value = (object) client.kpp
            };
            list3.Add(propertyValue2);
            EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.RsUid)),
              Value = (object) client.rs
            };
            list3.Add(propertyValue3);
            EntityProperties.PropertyValue propertyValue4 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.BikUid)),
              Value = (object) client.bik
            };
            list3.Add(propertyValue4);
            EntityProperties.PropertyValue propertyValue5 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.BankNameUid)),
              Value = (object) client.bank
            };
            list3.Add(propertyValue5);
            EntityProperties.PropertyValue propertyValue6 = new EntityProperties.PropertyValue()
            {
              EntityUid = client.uid,
              Type = typesList.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.KsUid)),
              Value = (object) client.ks
            };
            list3.Add(propertyValue6);
            client3.Properties = list3;
            Market5ImportHelper.ListClients.Add(new Market5ImportHelper.LinkImport5()
            {
              Uid = client3.Uid,
              Id = client.Id
            });
            source.Add(client3);
          }
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_контактов_в_базу_данных, source.Count);
          source.AsParallel<Gbs.Core.Entities.Clients.Client>().ForAll<Gbs.Core.Entities.Clients.Client>((Action<Gbs.Core.Entities.Clients.Client>) (x =>
          {
            using (Gbs.Core.Db.DataBase dataBase2 = Data.GetDataBase())
            {
              if (!new ClientsRepository(dataBase2).Save(x))
                return;
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            }
          }));
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Получение_сумм_покупок__задолженности_и_баллов_контактов, source.Count);
          Storages.Storage storage3 = Storages.GetStorages().First<Storages.Storage>();
          Sections.Section currentSection = Sections.GetCurrentSection();
          foreach (Gbs.Core.Entities.Clients.Client client4 in source)
          {
            Gbs.Core.Entities.Clients.Client client = client4;
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            (Decimal SaleSum, Decimal PaymentSum, Decimal Bonuses) clientOldSum = Market5ImportHelper.GetClientOldSum(Market5ImportHelper.ImportData.Clients.First<Market5Data.Client>((Func<Market5Data.Client, bool>) (c => c.uid == client.Uid)).Id);
            if (client.IsDeleted)
            {
              client.IsDeleted = clientOldSum.SaleSum - clientOldSum.PaymentSum <= 0.01M;
              if (!client.IsDeleted)
                new ClientsRepository(dataBase1).Save(client);
            }
            DateTime dateTime = new DateTime(2018, 1, 1);
            if (clientOldSum.PaymentSum != 0M || clientOldSum.SaleSum != 0M)
            {
              Document document = new Document()
              {
                Section = currentSection,
                Storage = storage3,
                Type = GlobalDictionaries.DocumentsTypes.Sale,
                DateTime = dateTime,
                ContractorUid = client.Uid
              };
              if (!Market5ImportHelper.IsTransferHistoryGood)
              {
                document.Items = new List<Gbs.Core.Entities.Documents.Item>()
                {
                  new Gbs.Core.Entities.Documents.Item()
                  {
                    DocumentUid = document.Uid,
                    GoodStock = good2.StocksAndPrices.First<GoodsStocks.GoodStock>(),
                    Good = good2,
                    ModificationUid = Guid.Empty,
                    Quantity = 1M,
                    SellPrice = clientOldSum.SaleSum
                  }
                };
                document.Payments = new List<Gbs.Core.Entities.Payments.Payment>()
                {
                  new Gbs.Core.Entities.Payments.Payment()
                  {
                    Date = dateTime,
                    SumIn = clientOldSum.PaymentSum > clientOldSum.SaleSum ? clientOldSum.SaleSum : clientOldSum.PaymentSum,
                    Client = client,
                    ParentUid = document.Uid
                  }
                };
              }
              new DocumentsRepository(dataBase1).Save(document);
            }
            if (clientOldSum.Bonuses != 0M)
              new Gbs.Core.Entities.Payments.Payment()
              {
                SumOut = Math.Round(clientOldSum.Bonuses, 4),
                Type = GlobalDictionaries.PaymentTypes.BonusesCorrection,
                Comment = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5,
                Client = client,
                Date = dateTime
              }.Save();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта покупателей из 5й версии");
      }
    }

    private static void AddNewProp(
      string nameProp,
      Guid uidClient,
      string value,
      ref List<EntityProperties.PropertyType> typeProp,
      ref List<EntityProperties.PropertyValue> props)
    {
      if (typeProp.Any<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Name == nameProp)))
      {
        props.Add(new EntityProperties.PropertyValue()
        {
          EntityUid = uidClient,
          Type = typeProp.First<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Name == nameProp)),
          Value = (object) value
        });
      }
      else
      {
        EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType()
        {
          Name = nameProp,
          EntityType = GlobalDictionaries.EntityTypes.Client
        };
        propertyType.Save();
        typeProp.Add(propertyType);
        props.Add(new EntityProperties.PropertyValue()
        {
          EntityUid = uidClient,
          Value = (object) value,
          Type = propertyType
        });
      }
    }

    private static List<Market5ImportHelper.LinkImport5> GoodLinks { get; set; } = new List<Market5ImportHelper.LinkImport5>();

    private static List<Market5ImportHelper.LinkImport5> ModificationLinks { get; set; } = new List<Market5ImportHelper.LinkImport5>();

    private static List<Gbs.Core.Entities.Goods.Good> ListGoods { get; set; }

    private static void GetGoods()
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase1 = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase1);
          List<GoodGroups.Group> activeItems = groupsRepository.GetActiveItems();
          activeItems.AddRange(Market5ImportHelper.GetGroups());
          Market5ImportHelper.ListGoods = new List<Gbs.Core.Entities.Goods.Good>();
          List<EntityProperties.PropertyType> list1 = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).ToList<EntityProperties.PropertyType>();
          List<GoodsSets.Set> setGood = Market5ImportHelper.GetSetGood(Market5ImportHelper.ImportData.Good_sets, Market5ImportHelper.ImportData.goods);
          List<GoodsModifications.GoodModification> modificationGood = Market5ImportHelper.GetModificationGood(Market5ImportHelper.ImportData.goods_modifications, Market5ImportHelper.ImportData.goods);
          Storages.Storage storage = Storages.GetStorages().First<Storages.Storage>();
          GoodGroups.Group group = new GoodGroups.Group()
          {
            Name = Translate.Market5ImportHelper_unknown,
            GoodsType = GlobalDictionaries.GoodTypes.Single
          };
          groupsRepository.Save(group, false);
          EntityProperties.PropertyType propertyType1 = list1.First<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.GoodIdUid));
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Обработка_данных_товаров, Market5ImportHelper.ImportData.goods.Count);
          foreach (Market5Data.Good good1 in Market5ImportHelper.ImportData.goods)
          {
            Market5Data.Good good5 = good1;
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            Market5Data.GoodsCategory goodsCategory = Market5ImportHelper.ImportData.GoodsCategories.FirstOrDefault<Market5Data.GoodsCategory>((Func<Market5Data.GoodsCategory, bool>) (x => x.Id == good5.group_id));
            if ((goodsCategory != null ? goodsCategory.goods_type : 0) != 7)
            {
              Gbs.Core.Entities.Goods.Good good2 = new Gbs.Core.Entities.Goods.Good();
              good2.Uid = good5.uid;
              good2.Name = good5.name;
              good2.IsDeleted = good5.is_deleted;
              good2.SetStatus = (GlobalDictionaries.GoodsSetStatuses) good5.set_status;
              good2.Group = goodsCategory == null || good5.Id == 0 ? group : activeItems.First<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == Market5ImportHelper.ImportData.GoodsCategories.First<Market5Data.GoodsCategory>((Func<Market5Data.GoodsCategory, bool>) (x => x.Id == good5.group_id)).uid));
              good2.Modifications = (IEnumerable<GoodsModifications.GoodModification>) modificationGood.Where<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.GoodUid == good5.uid)).ToList<GoodsModifications.GoodModification>();
              good2.SetContent = (IEnumerable<GoodsSets.Set>) setGood.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => x.ParentUid == good5.uid)).ToList<GoodsSets.Set>();
              good2.Tag = (object) good5.Id;
              good2.Description = good5.comment;
              Gbs.Core.Entities.Goods.Good good3 = good2;
              if (good3.Name.Length < 3)
                good3.Name += "000";
              List<string> list2 = ((IEnumerable<string>) good5.barcode.Replace(",", " ").Split()).ToList<string>();
              good3.Barcode = list2[0];
              list2.Remove(list2[0]);
              good3.Barcodes = (IEnumerable<string>) list2;
              List<EntityProperties.PropertyValue> list3 = good3.Properties.ToList<EntityProperties.PropertyValue>();
              for (int i = 1; i <= 5; i++)
              {
                if (list1.Any<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Name == "ext" + i.ToString())))
                {
                  list3.Add(new EntityProperties.PropertyValue()
                  {
                    EntityUid = good3.Uid,
                    Value = Market5ImportHelper.GetPropValue((object) good5, "ext" + i.ToString()),
                    Type = list1.First<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Name == "ext" + i.ToString() && x.EntityType == GlobalDictionaries.EntityTypes.Good))
                  });
                }
                else
                {
                  EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType()
                  {
                    Name = "ext" + i.ToString(),
                    EntityType = GlobalDictionaries.EntityTypes.Good
                  };
                  propertyType2.Save();
                  list1.Add(propertyType2);
                  list3.Add(new EntityProperties.PropertyValue()
                  {
                    EntityUid = good3.Uid,
                    Value = Market5ImportHelper.GetPropValue((object) good5, "ext" + i.ToString()),
                    Type = propertyType2
                  });
                }
              }
              list3.Add(new EntityProperties.PropertyValue()
              {
                EntityUid = good3.Uid,
                Type = propertyType1,
                Value = (object) good5.Id
              });
              good3.Properties = list3;
              Market5ImportHelper.ListGoods.Add(good3);
            }
          }
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_товаров_в_базу_данных, Market5ImportHelper.ListGoods.Count);
          foreach (Gbs.Core.Entities.Goods.Good good4 in Market5ImportHelper.ListGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Group.GoodsType == GlobalDictionaries.GoodTypes.Service || x.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)))
          {
            Gbs.Core.Entities.Goods.Good good = good4;
            Market5Data.Good good5 = Market5ImportHelper.ImportData.goods.Single<Market5Data.Good>((Func<Market5Data.Good, bool>) (g => g.uid == good.Uid));
            good.StocksAndPrices.Add(new GoodsStocks.GoodStock()
            {
              GoodUid = good.Uid,
              Price = good5.price,
              Stock = 0M,
              Storage = storage
            });
          }
          int ink = 0;
          Market5ImportHelper.ListGoods.AsParallel<Gbs.Core.Entities.Goods.Good>().ForAll<Gbs.Core.Entities.Goods.Good>((Action<Gbs.Core.Entities.Goods.Good>) (x =>
          {
            using (Gbs.Core.Db.DataBase dataBase2 = Data.GetDataBase())
            {
              if (new GoodRepository(dataBase2).Save(x, false))
              {
                Interlocked.Increment(ref ink);
                Market5ImportHelper.ProgressInfo.CurrentTaskStep = ink;
                Market5ImportHelper.GoodLinks.Add(new Market5ImportHelper.LinkImport5()
                {
                  Uid = x.Uid,
                  Id = Convert.ToInt32(x.Tag)
                });
              }
              else
                Other.ConsoleWrite("Товар не был сохранен: " + x.ToJsonString());
            }
          }));
          Market5ImportHelper.ProgressInfo.CurrentTaskStep = ink;
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Подготовка_данных_об_остатках_и_закупочной_стоимости_товаров, Market5ImportHelper.ListGoods.Count);
          Document doc = new Document()
          {
            Section = Sections.GetCurrentSection(),
            Type = GlobalDictionaries.DocumentsTypes.Buy,
            Comment = Translate.Market5ImportHelper_GetClients_Перенос_данных_из_v_5,
            DateTime = new DateTime(2018, 1, 1),
            Storage = storage
          };
          List<Market5Data.WaybillItem> allWbi = Market5ImportHelper.ImportData.waybills.SelectMany((Func<Market5Data.Waybill, IEnumerable<Market5Data.WaybillItem>>) (w => Market5ImportHelper.ImportData.waybill_items.Where<Market5Data.WaybillItem>((Func<Market5Data.WaybillItem, bool>) (x => x.waybill_id == w.id))), (w, wi) => new
          {
            w = w,
            wi = wi
          }).Where(_param1 => !_param1.w.is_deleted && !_param1.wi.is_deleted && !_param1.w.in_way).OrderByDescending(_param1 => _param1.w.date_time).Select(_param1 => _param1.wi).ToList<Market5Data.WaybillItem>();
          object lockO = new object();
          ConcurrentBag<Gbs.Core.Entities.Documents.Item> waybillItems = new ConcurrentBag<Gbs.Core.Entities.Documents.Item>();
          GC.Collect();
          GC.WaitForPendingFinalizers();
          Market5ImportHelper.ListGoods.ForEach((Action<Gbs.Core.Entities.Goods.Good>) (good =>
          {
            lock (lockO)
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
            if (!good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range))
              return;
            Market5Data.Good good5 = Market5ImportHelper.ImportData.goods.Single<Market5Data.Good>((Func<Market5Data.Good, bool>) (g => g.uid == good.Uid));
            List<Market5Data.WaybillItem> list4 = allWbi.Where<Market5Data.WaybillItem>((Func<Market5Data.WaybillItem, bool>) (w => w.good_id == good5.Id)).ToList<Market5Data.WaybillItem>();
            if (good.Modifications.Any<GoodsModifications.GoodModification>())
            {
              using (IEnumerator<GoodsModifications.GoodModification> enumerator = good.Modifications.GetEnumerator())
              {
label_17:
                if (!enumerator.MoveNext())
                  return;
                GoodsModifications.GoodModification m = enumerator.Current;
                int modId = Market5ImportHelper.ImportData.goods_modifications.First<Market5Data.GoodsModificationsIn5>((Func<Market5Data.GoodsModificationsIn5, bool>) (mod => mod.Uid == m.Uid)).Id;
                Decimal num1 = Market5ImportHelper.ImportData.goods_modifications.First<Market5Data.GoodsModificationsIn5>((Func<Market5Data.GoodsModificationsIn5, bool>) (mod => mod.Id == modId)).stock;
                do
                {
                  IEnumerable<Market5Data.WaybillItem> source = list4.Where<Market5Data.WaybillItem>((Func<Market5Data.WaybillItem, bool>) (wi => wi.income > 0M && wi.mod_id == modId));
                  Decimal num2;
                  Decimal num3;
                  if (source.Any<Market5Data.WaybillItem>())
                  {
                    Market5Data.WaybillItem waybillItem = source.First<Market5Data.WaybillItem>();
                    if (waybillItem.income >= num1)
                    {
                      num2 = num1;
                      num3 = waybillItem.in_price;
                      waybillItem.income -= num1;
                      num1 = 0M;
                    }
                    else
                    {
                      num2 = waybillItem.income;
                      num3 = waybillItem.in_price;
                      num1 -= waybillItem.income;
                      waybillItem.income = 0M;
                    }
                  }
                  else
                  {
                    num2 = num1;
                    num1 = 0M;
                    num3 = 0M;
                  }
                  waybillItems.Add(new Gbs.Core.Entities.Documents.Item()
                  {
                    Quantity = num2,
                    BuyPrice = num3,
                    SellPrice = good5.price,
                    Good = good,
                    ModificationUid = m.Uid,
                    GoodStock = new GoodsStocks.GoodStock()
                    {
                      GoodUid = good.Uid,
                      ModificationUid = m.Uid,
                      Price = good5.price,
                      Storage = storage
                    }
                  });
                }
                while (num1 != 0M);
                goto label_17;
              }
            }
            else
            {
              Decimal num4 = good5.stock;
              IEnumerable<Market5Data.WaybillItem> source = list4.Where<Market5Data.WaybillItem>((Func<Market5Data.WaybillItem, bool>) (wi => wi.income > 0M && wi.mod_id == -1));
              do
              {
                Decimal num5;
                Decimal num6;
                if (source.Any<Market5Data.WaybillItem>())
                {
                  Market5Data.WaybillItem waybillItem = source.First<Market5Data.WaybillItem>();
                  if (waybillItem.income >= num4)
                  {
                    num5 = num4;
                    num6 = waybillItem.in_price;
                    waybillItem.income -= num4;
                    num4 = 0M;
                  }
                  else
                  {
                    num5 = waybillItem.income;
                    num6 = waybillItem.in_price;
                    num4 -= waybillItem.income;
                    waybillItem.income = 0M;
                  }
                }
                else
                {
                  num5 = num4;
                  num4 = 0M;
                  num6 = 0M;
                }
                waybillItems.Add(new Gbs.Core.Entities.Documents.Item()
                {
                  Quantity = num5,
                  BuyPrice = num6,
                  SellPrice = good5.price,
                  Good = good,
                  ModificationUid = Guid.Empty,
                  GoodStock = new GoodsStocks.GoodStock()
                  {
                    GoodUid = good.Uid,
                    Price = good5.price,
                    Storage = storage
                  }
                });
                int id = good.Id;
              }
              while (num4 != 0M);
            }
          }));
          GC.Collect();
          GC.WaitForPendingFinalizers();
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_закупочных_цен_и_остатков_в_базу_данных, 1);
          doc.Items.AddRange((IEnumerable<Gbs.Core.Entities.Documents.Item>) waybillItems);
          Market5ImportHelper.FixNonValidDataInWaybill(doc);
          doc.Items.RemoveAll((Predicate<Gbs.Core.Entities.Documents.Item>) (x => x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service)));
          if (new DocumentsRepository(dataBase1).Save(doc))
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          Market5ImportHelper.ImageImport();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта товаров из 5й версии");
      }
    }

    public static void ImageImport()
    {
      if (Market5ImportHelper.PathFolderForImage.IsNullOrEmpty() || !Directory.Exists(Market5ImportHelper.PathFolderForImage))
        return;
      Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetGoods_Перенос_изображений_товаров, Market5ImportHelper.ListGoods.Count);
      string goodsImagesPath = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath;
      Directory.CreateDirectory(goodsImagesPath);
      foreach (Gbs.Core.Entities.Goods.Good listGood in Market5ImportHelper.ListGoods)
      {
        Gbs.Core.Entities.Goods.Good good = listGood;
        Market5Data.Good good1 = Market5ImportHelper.ImportData.goods.Single<Market5Data.Good>((Func<Market5Data.Good, bool>) (g => g.uid == good.Uid));
        string pathFolderForImage = Market5ImportHelper.PathFolderForImage;
        int index = good1.Id;
        string path2_1 = index.ToString();
        string path = Path.Combine(pathFolderForImage, path2_1);
        if (Directory.Exists(path))
        {
          if (((IEnumerable<string>) Directory.GetFiles(path)).Any<string>())
          {
            try
            {
              string path1_1 = goodsImagesPath;
              Guid uid = good.Uid;
              string path2_2 = uid.ToString();
              Directory.CreateDirectory(Path.Combine(path1_1, path2_2));
              string[] files = Directory.GetFiles(path);
              for (index = 0; index < files.Length; ++index)
              {
                string str = files[index];
                string extension = Path.GetExtension(str);
                string path1_2 = goodsImagesPath;
                uid = good.Uid;
                string path2_3 = uid.ToString();
                string path3 = DateTime.Now.ToString("ddMMyyyyHHmmss") + extension;
                ImagesHelpers.CompressImage(str, Path.Combine(path1_2, path2_3, path3));
              }
              index = Market5ImportHelper.ProgressInfo.CurrentTaskStep++;
              continue;
            }
            catch
            {
              continue;
            }
          }
        }
        index = Market5ImportHelper.ProgressInfo.CurrentTaskStep++;
      }
    }

    private static void MoveImage(Gbs.Core.Entities.Goods.Good good)
    {
    }

    private static void FixNonValidDataInWaybill(Document doc)
    {
      foreach (Gbs.Core.Entities.Documents.Item obj in doc.Items)
      {
        if (obj.Quantity > 99999999.9M)
          obj.Quantity = 99999999.9M;
        if (obj.BuyPrice > 99999999.9M)
          obj.BuyPrice = 99999999.9M;
        if (obj.SellPrice > 99999999.9M)
          obj.SellPrice = 99999999.9M;
        if (obj.Quantity < -99999999.9M)
          obj.Quantity = -99999999.9M;
        if (obj.BuyPrice < -99999999.9M)
          obj.BuyPrice = -99999999.9M;
        if (obj.SellPrice < -99999999.9M)
          obj.SellPrice = -99999999.9M;
      }
    }

    private static IEnumerable<GoodGroups.Group> GetGroups()
    {
      try
      {
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_категорий_товаров, Market5ImportHelper.ImportData.GoodsCategories.Count);
        List<GoodGroups.Group> source = new List<GoodGroups.Group>();
        List<GoodsUnits.GoodUnit> list = GoodsUnits.GetUnitsListWithFilter().ToList<GoodsUnits.GoodUnit>();
        foreach (Market5Data.GoodsCategory goodsCategory in Market5ImportHelper.ImportData.GoodsCategories)
        {
          Market5Data.GoodsCategory category = goodsCategory;
          ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          GoodGroups.Group group1 = new GoodGroups.Group();
          group1.Uid = category.uid;
          group1.Name = category.Name;
          group1.NeedComment = category.needComment;
          group1.IsFreePrice = category.is_freePrice;
          group1.KkmSectionNumber = category.sectionNumber;
          group1.TaxRateNumber = category.taxType;
          group1.IsDeleted = category.is_deleted;
          group1.RuFfdGoodsType = (GlobalDictionaries.RuFfdGoodsTypes) category.ffdGoodType;
          group1.RuTaxSystem = (GlobalDictionaries.RuTaxSystems) category.taxSystem;
          GoodGroups.Group group2 = group1;
          switch (category.goods_type)
          {
            case 0:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Single;
              break;
            case 1:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Weight;
              break;
            case 2:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Service;
              break;
            case 3:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Single;
              group2.IsCompositeGood = true;
              break;
            case 7:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Certificate;
              break;
            default:
              group2.GoodsType = GlobalDictionaries.GoodTypes.Single;
              break;
          }
          if (list.Any<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (x => x.ShortName == category.units)))
          {
            group2.UnitsUid = list.First<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (x => x.ShortName == category.units)).Uid;
          }
          else
          {
            GoodsUnits.GoodUnit goodUnit = new GoodsUnits.GoodUnit()
            {
              FullName = category.units,
              ShortName = category.units,
              Code = "0"
            };
            list.Add(goodUnit);
            if (goodUnit.Save())
              group2.UnitsUid = goodUnit.Uid;
          }
          source.Add(group2);
        }
        foreach (Market5Data.GoodsCategory goodsCategory in Market5ImportHelper.ImportData.GoodsCategories)
        {
          Market5Data.GoodsCategory category = goodsCategory;
          if (category.id_parent != -1)
            source.First<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == category.uid)).ParentGroupUid = Market5ImportHelper.ImportData.GoodsCategories.First<Market5Data.GoodsCategory>((Func<Market5Data.GoodsCategory, bool>) (x => x.Id == category.id_parent)).uid;
        }
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Сохранение_категорий_товаров_в_базу_данных, source.Count);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          foreach (GoodGroups.Group group in source.AsParallel<GoodGroups.Group>())
          {
            if (group.Name.Length < 3)
              group.Name += "___";
            if (groupsRepository.Save(group))
              ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
          }
          return (IEnumerable<GoodGroups.Group>) source;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта групп товаров из 5й версии");
        return (IEnumerable<GoodGroups.Group>) new List<GoodGroups.Group>();
      }
    }

    private static List<GoodsSets.Set> GetSetGood(
      List<Market5Data.GoodSet> listSets,
      List<Market5Data.Good> goodList)
    {
      try
      {
        List<GoodsSets.Set> setGood = new List<GoodsSets.Set>();
        foreach (Market5Data.GoodSet listSet in listSets)
        {
          Market5Data.GoodSet goodSets = listSet;
          GoodsSets.Set set = new GoodsSets.Set()
          {
            Discount = goodSets.discount,
            IsDeleted = goodSets.is_deleted,
            ParentUid = goodList.First<Market5Data.Good>((Func<Market5Data.Good, bool>) (x => x.Id == goodSets.parent_id)).uid,
            Quantity = goodSets.count
          };
          Market5Data.Good good = goodList.FirstOrDefault<Market5Data.Good>((Func<Market5Data.Good, bool>) (x => x.Id == goodSets.good_id));
          if (good != null)
          {
            set.GoodUid = good.uid;
            setGood.Add(set);
          }
        }
        return setGood;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта составных товаров из 5й версии");
        return new List<GoodsSets.Set>();
      }
    }

    private static List<GoodsModifications.GoodModification> GetModificationGood(
      List<Market5Data.GoodsModificationsIn5> listModification,
      List<Market5Data.Good> goodList)
    {
      try
      {
        List<GoodsModifications.GoodModification> modificationGood = new List<GoodsModifications.GoodModification>();
        foreach (Market5Data.GoodsModificationsIn5 modificationsIn5 in listModification)
        {
          Market5Data.GoodsModificationsIn5 goodModification = modificationsIn5;
          GoodsModifications.GoodModification goodModification1 = new GoodsModifications.GoodModification();
          goodModification1.Uid = goodModification.Uid;
          goodModification1.Name = goodModification.name;
          goodModification1.GoodUid = goodList.First<Market5Data.Good>((Func<Market5Data.Good, bool>) (x => x.Id == goodModification.good_id)).uid;
          goodModification1.IsDeleted = goodModification.is_deleted;
          GoodsModifications.GoodModification goodModification2 = goodModification1;
          Market5ImportHelper.ModificationLinks.Add(new Market5ImportHelper.LinkImport5()
          {
            Uid = goodModification2.Uid,
            Id = goodModification.Id
          });
          modificationGood.Add(goodModification2);
        }
        return modificationGood;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта модификаций товаров из 5й версии");
        return new List<GoodsModifications.GoodModification>();
      }
    }

    private static List<Market5ImportHelper.LinkImport5> SuppliersLinks { get; set; } = new List<Market5ImportHelper.LinkImport5>();

    private static void GetSuppliers()
    {
      try
      {
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetSuppliers_Загрузка_поставщиков, 0);
        Gbs.Core.Entities.Clients.Group group = new Gbs.Core.Entities.Clients.Group()
        {
          Name = Translate.Market5ImportHelper_GetSuppliers_Поставщики_из_market_5,
          IsSupplier = true,
          Discount = 0M
        };
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          new GroupRepository(dataBase).Save(group);
          List<Gbs.Core.Entities.Clients.Client> source = new List<Gbs.Core.Entities.Clients.Client>();
          foreach (Market5Data.Supplier supplier in Market5ImportHelper.ImportData.Suppliers)
          {
            Gbs.Core.Entities.Clients.Client client1 = new Gbs.Core.Entities.Clients.Client();
            client1.Group = group;
            client1.Name = supplier.short_name.Length < 3 ? supplier.short_name + "___" : supplier.short_name;
            client1.IsDeleted = supplier.is_deleted;
            client1.Comment = supplier.full_name + "\n" + supplier.information;
            Gbs.Core.Entities.Clients.Client client2 = client1;
            source.Add(client2);
            Market5ImportHelper.SuppliersLinks.Add(new Market5ImportHelper.LinkImport5()
            {
              Uid = client2.Uid,
              Id = supplier.Id
            });
          }
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_GetSuppliers_Сохранение_поставщиков_в_базу_данных, source.Count);
          ClientsRepository cr = new ClientsRepository(dataBase);
          foreach (Gbs.Core.Entities.Clients.Client client in source.Where<Gbs.Core.Entities.Clients.Client>((Func<Gbs.Core.Entities.Clients.Client, bool>) (sup => cr.Save(sup))))
            ++Market5ImportHelper.ProgressInfo.CurrentTaskStep;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта поставщиков из 5й версии");
      }
    }

    private static bool IsTransferHistoryGood => false;

    private static string PathFolderForImage { get; set; }

    public static ProgressInfoViewModel ProgressInfo { get; set; }

    private static Market5Data ImportData { get; set; }

    public static bool ImportDataOfMarket5(Market5Data data, string pathImage = "")
    {
      Market5ImportHelper.ImportData = data;
      Market5ImportHelper.PathFolderForImage = pathImage;
      try
      {
        if (Market5ImportHelper.ProgressInfo == null)
          Market5ImportHelper.ProgressInfo = new ProgressInfoViewModel();
        GlobalData.IsMarket5ImportAcitve = true;
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_начата, 0);
        Market5ImportHelper.GetGoods();
        Market5ImportHelper.GetClients();
        Market5ImportHelper.GetSuppliers();
        Market5ImportHelper.GetUsers();
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_завершена, 0);
        Market5ImportHelper.ProgressInfo.IsIndeterminate = false;
        Market5ImportHelper.ProgressInfo.CanClose = true;
        GlobalData.IsMarket5ImportAcitve = false;
        Other.ConsoleWrite("Импорт завершен");
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта данных из Маркет5", false);
        GlobalData.IsMarket5ImportAcitve = false;
        int num = (int) MessageBoxHelper.Show(Translate.Market5ImportHelper_При_импорте_данных_возникла_ошибка__Обратитесь_в_службу_поддержки, icon: MessageBoxImage.Hand);
        return false;
      }
    }

    public static bool ImportOnlyImageOfMarket5(string pathImage = "")
    {
      Market5ImportHelper.PathFolderForImage = pathImage;
      try
      {
        if (Market5ImportHelper.ProgressInfo == null)
          Market5ImportHelper.ProgressInfo = new ProgressInfoViewModel();
        GlobalData.IsMarket5ImportAcitve = true;
        Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_начата, 0);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Goods.Good> allItems = new GoodRepository(dataBase).GetAllItems();
          Market5ImportHelper.ListGoods = new List<Gbs.Core.Entities.Goods.Good>((IEnumerable<Gbs.Core.Entities.Goods.Good>) allItems);
          Market5ImportHelper.ImportData = new Market5Data()
          {
            goods = allItems.Select<Gbs.Core.Entities.Goods.Good, Market5Data.Good>((Func<Gbs.Core.Entities.Goods.Good, Market5Data.Good>) (x => new Market5Data.Good()
            {
              Id = Convert.ToInt32(x.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value.ToString() ?? "0"),
              uid = x.Uid
            })).ToList<Market5Data.Good>()
          };
          Market5ImportHelper.ImageImport();
          Market5ImportHelper.ProgressInfo.NewProgress(Translate.Market5ImportHelper_Загрузка_завершена, 0);
          Market5ImportHelper.ProgressInfo.IsIndeterminate = false;
          Market5ImportHelper.ProgressInfo.CanClose = true;
          GlobalData.IsMarket5ImportAcitve = false;
          Other.ConsoleWrite("Импорт фото завершен");
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка импорта фото из Маркет5", false);
        GlobalData.IsMarket5ImportAcitve = false;
        int num = (int) MessageBoxHelper.Show(Translate.Market5ImportHelper_При_импорте_данных_возникла_ошибка__Обратитесь_в_службу_поддержки, icon: MessageBoxImage.Hand);
        return false;
      }
    }

    private static object GetPropValue(object src, string propName)
    {
      return src.GetType().GetProperty(propName)?.GetValue(src, (object[]) null);
    }

    public class LinkImport5
    {
      public Guid Uid { get; set; }

      public int Id { get; set; }
    }
  }
}
