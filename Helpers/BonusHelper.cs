// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BonusHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Settings;
using Gbs.Core.Entities.Settings.Discount;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Helpers
{
  public class BonusHelper
  {
    public void UpdateSumBonusesForSale(Document document)
    {
      if (document.ContractorUid == Guid.Empty)
        return;
      using (DataBase dataBase = Data.GetDataBase())
      {
        Client byUid = new ClientsRepository(dataBase).GetByUid(document.ContractorUid);
        if (byUid == null || byUid.Group.IsNonUseBonus || !(bool) new SettingsRepository().GetSettingByType(Types.ActiveBonuses).Value)
          return;
        this.GetSumBonuses(document, byUid, true);
      }
    }

    public Decimal GetSumBonuses(Document document, Client client, bool isSavePayments)
    {
      Bonuses.BonusesOption int32 = (Bonuses.BonusesOption) Convert.ToInt32(new SettingsRepository().GetSettingByType(Types.OptionRuleBonuses).Value);
      if (document.Payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)) && int32 == Bonuses.BonusesOption.OffBonuses)
        return 0M;
      Decimal num1 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
      Decimal num2 = 1.0M;
      Decimal sumDocument = SaleHelper.GetSumDocument(document);
      if (int32 == Bonuses.BonusesOption.SaleOffBonuses)
      {
        if (sumDocument == 0M)
        {
          LogHelper.Debug("Сумма продажи равна нулю, НЕ начисляем баллы");
          return 0M;
        }
        num2 = (sumDocument - num1) / sumDocument;
      }
      List<ClientBonuses> list1 = new ClientBonusesRepository().GetActiveItems().ToList<ClientBonuses>();
      Decimal sumBonuses = 0M;
      Decimal num3 = 1.0M;
      List<Setting> settingListByType = new SettingsRepository().GetSettingListByType(Types.MethodPaymentBonuses, false);
      if (settingListByType.Any<Setting>())
      {
        if (document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid))).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) == 0M)
        {
          LogHelper.Debug("Сумма платежей равна нулю, НЕ начисляем баллы");
          return 0M;
        }
        Decimal num4 = sumDocument - document.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        Decimal num5 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid))).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        List<Gbs.Core.Entities.Payments.Payment> list2 = document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid))).ToList<Gbs.Core.Entities.Payments.Payment>();
        if (int32 == Bonuses.BonusesOption.AllSale)
        {
          num5 += num1;
          list2.AddRange(document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesDocumentPayment))));
        }
        foreach (Gbs.Core.Entities.Payments.Payment payment in list2)
        {
          Gbs.Core.Entities.Payments.Payment p = payment;
          if (settingListByType.Any<Setting>((Func<Setting, bool>) (r => r.Uid == p.Method.Uid)))
            num3 -= (p.SumIn - p.SumOut) / num5;
        }
        if (num4 > 0M)
          num3 -= num4 / sumDocument;
        num2 *= num3;
      }
      foreach (Gbs.Core.Entities.Documents.Item obj in document.Items)
      {
        Gbs.Core.Entities.Documents.Item item = obj;
        ClientBonuses clientBonuses = list1.FirstOrDefault<ClientBonuses>((Func<ClientBonuses, bool>) (x => x.ListGroups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid))));
        if (clientBonuses != null)
        {
          Decimal d = clientBonuses.Percent / 100M * (item.SellPrice * (100M - item.Discount) / 100M) * item.Quantity * num2;
          if (d > 0M)
          {
            if (isSavePayments)
              new Gbs.Core.Entities.Payments.Payment()
              {
                Client = client,
                ParentUid = item.Uid,
                Type = GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment,
                SumOut = Math.Round(d, 4),
                Date = document.DateTime
              }.Save();
            sumBonuses += Math.Round(d, 4);
          }
        }
      }
      return sumBonuses;
    }

    public void UpdateSumBonusesForReturn(Document document, List<BasketItem> items)
    {
      if (document.ContractorUid == Guid.Empty)
        return;
      using (DataBase dataBase = Data.GetDataBase())
      {
        Client byUid = new ClientsRepository(dataBase).GetByUid(document.ContractorUid);
        if (byUid == null || byUid.Group.IsNonUseBonus)
          return;
        if (items == null)
          items = document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, (Storages.Storage) null, x.Quantity, x.Discount, x.Uid))).ToList<BasketItem>();
        if (!items.Any<BasketItem>())
          return;
        List<Gbs.Core.Entities.Payments.Payment> paymentsList = Gbs.Core.Entities.Payments.GetPaymentsList(dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED && x.TYPE == 5)));
        foreach (BasketItem basketItem in items)
        {
          BasketItem item = basketItem;
          if (paymentsList.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.ParentUid == item.Uid)))
          {
            Decimal d = paymentsList.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.ParentUid == item.Uid)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut)) / items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity)) * item.Quantity;
            Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
            payment.Type = GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment;
            Client client = new Client();
            client.Uid = document.ContractorUid;
            payment.Client = client;
            payment.ParentUid = item.Uid;
            payment.SumIn = Math.Round(d, 4);
            payment.Save();
          }
        }
      }
    }
  }
}
