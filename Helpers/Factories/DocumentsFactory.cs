// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Factories.DocumentsFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Forms._shared;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Factories
{
  public class DocumentsFactory : IFactory<Document>
  {
    public Document Create(Gbs.Core.ViewModels.Basket.Basket basket, bool isCafe = false, bool isClose = true, Guid documentUid = default (Guid))
    {
      Document document1 = new Document();
      document1.Uid = documentUid == Guid.Empty ? Guid.NewGuid() : documentUid;
      document1.Number = basket.SaleNumber;
      document1.Comment = basket.Comment;
      document1.DateTime = DateTime.Now;
      document1.Status = isClose ? GlobalDictionaries.DocumentsStatuses.Close : GlobalDictionaries.DocumentsStatuses.Draft;
      document1.Type = isCafe ? GlobalDictionaries.DocumentsTypes.CafeOrder : GlobalDictionaries.DocumentsTypes.Sale;
      document1.Section = Sections.GetCurrentSection();
      Users.User user = basket.User;
      // ISSUE: explicit non-virtual call
      document1.UserUid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
      ClientAdnSum client = basket.Client;
      document1.ContractorUid = client != null ? client.Client.Uid : Guid.Empty;
      document1.Storage = basket.Storage ?? Storages.GetStorages().FirstOrDefault<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted));
      document1.ParentUid = basket.ClientOrderUid;
      Document d = document1;
      using (Data.GetDataBase())
      {
        d.Items = basket.Items.Select<BasketItem, Gbs.Core.Entities.Documents.Item>((Func<BasketItem, Gbs.Core.Entities.Documents.Item>) (x =>
        {
          Gbs.Core.Entities.Documents.Item obj = new Gbs.Core.Entities.Documents.Item();
          obj.Uid = documentUid == Guid.Empty ? Guid.NewGuid() : x.Uid;
          obj.SellPrice = x.SalePrice;
          obj.BaseSalePrice = x.BasePrice;
          obj.Discount = x.Discount.Value;
          obj.DocumentUid = d.Uid;
          obj.Quantity = x.Quantity;
          obj.Good = x.Good;
          obj.Comment = x.Comment;
          GoodsModifications.GoodModification goodModification = x.GoodModification;
          // ISSUE: explicit non-virtual call
          obj.ModificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
          return obj;
        })).ToList<Gbs.Core.Entities.Documents.Item>();
        d.Payments = basket.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.Uid != GlobalDictionaries.BonusesPaymentUid)).Select<SelectPaymentMethods.PaymentGrid, Payments.Payment>((Func<SelectPaymentMethods.PaymentGrid, Payments.Payment>) (x => new Payments.Payment()
        {
          Date = d.DateTime,
          ParentUid = d.Uid,
          Type = x.Type == GlobalDictionaries.KkmPaymentMethods.PrePayment ? GlobalDictionaries.PaymentTypes.Prepaid : GlobalDictionaries.PaymentTypes.MoneyDocumentPayment,
          User = basket.User,
          Method = x.Method,
          AccountIn = x.Method.AccountUid == Guid.Empty ? (PaymentsAccounts.PaymentsAccount) null : PaymentsAccounts.GetPaymentsAccountByUid(x.Method.AccountUid),
          SumIn = x.Sum.GetValueOrDefault(),
          Comment = x.Comment
        })).ToList<Payments.Payment>();
        if (basket.Payments.Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.Uid == GlobalDictionaries.BonusesPaymentUid)))
        {
          SelectPaymentMethods.PaymentGrid paymentGrid = basket.Payments.Single<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.Uid == GlobalDictionaries.BonusesPaymentUid));
          d.Payments.Add(new Payments.Payment()
          {
            Date = d.DateTime,
            ParentUid = d.Uid,
            Method = paymentGrid.Method,
            SumIn = paymentGrid.Sum.GetValueOrDefault(),
            Type = GlobalDictionaries.PaymentTypes.BonusesDocumentPayment,
            User = basket.User
          });
        }
        Decimal kkmCheckCorrection = basket.KkmCheckCorrection;
        if (kkmCheckCorrection != 0M)
          d.Payments.Add(new Payments.Payment()
          {
            Date = d.DateTime,
            ParentUid = d.Uid,
            SumIn = kkmCheckCorrection,
            Type = GlobalDictionaries.PaymentTypes.CheckDiscount,
            User = basket.User,
            Comment = Translate.DocumentsFactory_Create_Скидка_на_чек
          });
        if (basket.GetType() == typeof (CafeBasket))
        {
          Document document2 = d;
          List<EntityProperties.PropertyValue> propertyValueList = new List<EntityProperties.PropertyValue>();
          EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
          propertyType1.Uid = GlobalDictionaries.CountGuestUid;
          propertyValue1.Type = propertyType1;
          propertyValue1.Value = (object) ((CafeBasket) basket).CountGuest;
          propertyValueList.Add(propertyValue1);
          EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
          EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
          propertyType2.Uid = GlobalDictionaries.NumTableUid;
          propertyValue2.Type = propertyType2;
          propertyValue2.Value = (object) ((CafeBasket) basket).NumTable;
          propertyValueList.Add(propertyValue2);
          document2.Properties = propertyValueList;
        }
        return d;
      }
    }
  }
}
