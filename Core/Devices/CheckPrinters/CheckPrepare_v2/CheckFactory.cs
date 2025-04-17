// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2.CheckFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2
{
  public class CheckFactory
  {
    public static void PrepareCheckForCreditPayment(
      CreditListViewModel.CreditItem item,
      List<SelectPaymentMethods.PaymentGrid> paymentsList,
      Gbs.Core.Entities.Users.User authUser)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Document> returnsDocuments = new DocumentsRepository(dataBase).GetByParentUid(item.Document.Uid).Where<Document>((Func<Document, bool>) (d => d.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)).ToList<Document>();
        List<CheckGood> list1 = item.Document.Items.Select<Gbs.Core.Entities.Documents.Item, CheckGood>((Func<Gbs.Core.Entities.Documents.Item, CheckGood>) (documentItem =>
        {
          Decimal num = returnsDocuments.Sum<Document>((Func<Document, Decimal>) (d => d.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
          {
            Guid? uid5 = i.GoodStock?.Uid;
            Guid? uid6 = documentItem.GoodStock?.Uid;
            if (uid5.HasValue != uid6.HasValue)
              return false;
            return !uid5.HasValue || uid5.GetValueOrDefault() == uid6.GetValueOrDefault();
          })).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => i.Quantity))));
          Decimal quantity = documentItem.Quantity - num;
          foreach (Gbs.Core.Entities.Documents.Item obj in returnsDocuments.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (d => d.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
          {
            Guid? uid7 = i.GoodStock?.Uid;
            Guid? uid8 = documentItem.GoodStock?.Uid;
            if (uid7.HasValue != uid8.HasValue)
              return false;
            return !uid7.HasValue || uid7.GetValueOrDefault() == uid8.GetValueOrDefault();
          })))))
            obj.Quantity = 0M;
          return new CheckGood(documentItem.Good, documentItem.SellPrice, 0M, quantity, "", "");
        })).ToList<CheckGood>();
        list1.RemoveAll((Predicate<CheckGood>) (x => x.Quantity == 0M));
        Gbs.Core.Config.Devices config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        CheckFactory checkFactory = new CheckFactory(config.CheckPrinter);
        List<CheckPayment> list2 = paymentsList.Select<SelectPaymentMethods.PaymentGrid, CheckPayment>((Func<SelectPaymentMethods.PaymentGrid, CheckPayment>) (payment => new CheckPayment()
        {
          Method = payment.Method.KkmMethod,
          Sum = payment.Sum.GetValueOrDefault()
        })).ToList<CheckPayment>();
        Cashier cashier = new Cashier(authUser);
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData = new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(list1, list2, CheckFiscalTypes.Fiscal, cashier)
        {
          Client = new ClientAdnSum()
          {
            Client = item.Client
          },
          Number = item.Document.Number
        };
        Decimal sum = list2.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
        List<CheckFactory.AdjustableItem> list3 = checkData.GoodsList.Select<CheckGood, CheckFactory.AdjustableItem>((Func<CheckGood, CheckFactory.AdjustableItem>) (x => new CheckFactory.AdjustableItem((object) x, x.Sum))).ToList<CheckFactory.AdjustableItem>();
        CheckFactory.AdjustSumForPositions(list3, sum);
        foreach (CheckFactory.AdjustableItem adjustableItem in list3)
        {
          CheckGood checkGood = (CheckGood) adjustableItem.Object;
          checkGood.Price = adjustableItem.Sum;
          checkGood.Quantity = 1M;
          checkGood.Discount = 0M;
        }
        checkData.GoodsList.RemoveAll((Predicate<CheckGood>) (x => x.Price == 0M));
        checkData.PaymentsList = list2;
        CheckFactory.PrepareOptions options = new CheckFactory.PrepareOptions()
        {
          RoundCheck = false
        };
        PreparedCheckData preparedCheckData = checkFactory.PrepareCheckData(checkData, options);
        int kkmIndex = config.CheckPrinter.FiscalKkm.TaxRates.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Value.TaxValue == -1M)).Value.KkmIndex;
        foreach (CheckGood goods in preparedCheckData.CheckData.GoodsList)
        {
          goods.RuFfdPaymentModeCode = GlobalDictionaries.RuFfdPaymentModes.PaymentForCredit;
          goods.RuFfdGoodTypeCode = GlobalDictionaries.RuFfdGoodsTypes.Payment;
          goods.TaxRateNumber = kkmIndex;
          goods.Unit.RuFfdUnitsIndex = 0;
        }
        if (!new CheckPrinterHelper(config).PrintCheck(preparedCheckData.CheckData))
          throw new Exception(Translate.CheckFactory_PrepareCheckForCreditPayment_Не_удалось_напечатать_чек_по_задолженности);
      }
    }

    public static void AdjustSumForPositions(List<CheckFactory.AdjustableItem> items, Decimal sum)
    {
      Decimal num1 = items.Sum<CheckFactory.AdjustableItem>((Func<CheckFactory.AdjustableItem, Decimal>) (p => p.Sum));
      if (sum > num1)
        throw new InvalidOperationException("Adjustable summ more than items sum");
      foreach (CheckFactory.AdjustableItem adjustableItem in items)
      {
        Decimal num2 = adjustableItem.Sum / num1;
        adjustableItem.Sum = Math.Round(sum * num2, 2, MidpointRounding.AwayFromZero);
      }
      Decimal num3 = items.Sum<CheckFactory.AdjustableItem>((Func<CheckFactory.AdjustableItem, Decimal>) (p => p.Sum));
      Decimal remainder = Math.Round(sum - num3, 2);
      if (remainder > 0M)
      {
        items[0].Sum += remainder;
        remainder = 0M;
      }
      else
      {
        foreach (CheckFactory.AdjustableItem adjustableItem in items.TakeWhile<CheckFactory.AdjustableItem>((Func<CheckFactory.AdjustableItem, bool>) (checkItem => remainder != 0M)))
        {
          if (Math.Abs(remainder) > adjustableItem.Sum)
          {
            remainder += adjustableItem.Sum;
            adjustableItem.Sum = 0M;
          }
          else
          {
            adjustableItem.Sum += remainder;
            remainder = 0M;
          }
        }
      }
      if (remainder != 0M)
        throw new Exception(string.Format("It was not possible to distribute the amount by positions. Remainder: {0}", (object) remainder));
    }

    private bool IsOnlineKkm
    {
      get
      {
        return this.CheckPrinterConfig.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm && new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia;
      }
    }

    private Gbs.Core.Config.CheckPrinter CheckPrinterConfig { get; }

    public CheckFactory(Gbs.Core.Config.CheckPrinter config) => this.CheckPrinterConfig = config;

    public PreparedCheckData PrepareCheckData(
      Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData,
      CheckFactory.PrepareOptions options = null)
    {
      Performancer performancer = new Performancer("Подготовка данных чека");
      if (options == null)
        options = new CheckFactory.PrepareOptions();
      IEnumerable<CheckFactory.DiscountInfo> discountList = checkData.GoodsList.Select<CheckGood, CheckFactory.DiscountInfo>((Func<CheckGood, CheckFactory.DiscountInfo>) (x => new CheckFactory.DiscountInfo()
      {
        Uid = x.Uid,
        Discount = x.Discount,
        PriceWithoutDiscount = x.Price
      }));
      performancer.AddPoint("100");
      if (checkData.FiscalType == CheckFiscalTypes.Fiscal && this.IsOnlineKkm)
      {
        Decimal num1 = checkData.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
        checkData.PaymentsList.RemoveAll((Predicate<CheckPayment>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Bonus));
        performancer.AddPoint("130");
        Decimal num2 = 0M;
        if (options.RoundCheck)
          num2 = this.GetChangeSumToRoundCheck(checkData);
        performancer.AddPoint("135");
        Decimal sumToSplit = num2 + num1 + checkData.DiscountSum;
        checkData.DiscountSum = 0M;
        checkData.GoodsList = this.PrepareCheckGoodsForOnlineKkm(checkData.GoodsList, sumToSplit);
        performancer.AddPoint("140");
        RuOnlineKkmHelper.PrepareCertificatePayments(checkData);
        performancer.AddPoint("150");
      }
      else
        checkData.DiscountSum += this.GetChangeSumToRoundCheck(checkData);
      performancer.AddPoint("200");
      if (checkData.FiscalType == CheckFiscalTypes.Fiscal)
      {
        foreach (CheckGood goods in checkData.GoodsList)
        {
          goods.Price = goods.Price < 0M ? 0M : goods.Price;
          CheckGood checkGood = goods;
          string description = goods.Description;
          Gbs.Core.Entities.Goods.Good good = goods.Good;
          int markedProductionType = good != null ? (int) good.Group.RuMarkedProductionType : 0;
          MarkedInfo markedInfo = new MarkedInfo(description, (GlobalDictionaries.RuMarkedProductionTypes) markedProductionType);
          checkGood.MarkedInfo = markedInfo;
        }
        this.SetAndCheckTaxSystem(checkData);
        this.SetTaxRates(checkData);
        if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
          this.SetFfdGoodTypeCode(checkData);
      }
      CheckFactory.WriteGoodsToConsole(checkData);
      new CheckDataPreparer().PrepareCreditPayment(checkData);
      if (checkData.FiscalType == CheckFiscalTypes.Fiscal && this.IsOnlineKkm)
        CheckFactory.SetDiscountComment(discountList, checkData);
      performancer.AddPoint("400");
      this.SetGoodsComments(checkData);
      try
      {
        LogHelper.Debug("Содержимое чека:\r\n" + new CheckVisualizer(checkData, this.CheckPrinterConfig).Do());
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось визуализировать содержимое чека");
      }
      performancer.Stop();
      return new PreparedCheckData(checkData);
    }

    private static void SetDiscountComment(
      IEnumerable<CheckFactory.DiscountInfo> discountList,
      Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      foreach (CheckFactory.DiscountInfo discountInfo in discountList.Where<CheckFactory.DiscountInfo>((Func<CheckFactory.DiscountInfo, bool>) (x => x.Discount != 0M)))
      {
        CheckFactory.DiscountInfo item = discountInfo;
        foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x => x.Uid == item.Uid)))
        {
          Decimal num = item.PriceWithoutDiscount * checkGood.Quantity - checkGood.Sum;
          checkGood.CommentForFiscalCheck.Add(string.Format(Translate.KkmHelper_В_т_ч__СКИДКА___0____1_N2___, (object) num, (object) item.Discount));
        }
      }
    }

    public PreparedCheckData CreateCheckFromBasket(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      List<CheckGood> goodsListFromBasket = this.GetCheckGoodsListFromBasket(basket);
      List<CheckPayment> checkPaymentList = this.PrepareCheckPayments(basket);
      Cashier cashier = this.GetCashier(basket);
      Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData1 = new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData()
      {
        GoodsList = goodsListFromBasket,
        PaymentsList = checkPaymentList,
        Cashier = cashier,
        Client = basket.Client,
        FiscalType = basket.CheckFiscalType,
        Number = basket.SaleNumber,
        Comment = basket.Comment,
        TrueApiInfoForKkm = basket.TrueApiInfoForKkm
      };
      if (basket.GetType() == typeof (CafeBasket))
      {
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData2 = checkData1;
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
        checkData2.Properties = propertyValueList;
      }
      if (basket.Document != null)
        checkData1.Properties.AddRange((IEnumerable<EntityProperties.PropertyValue>) basket.Document.Properties);
      return this.PrepareCheckData(checkData1);
    }

    private void SetGoodsComments(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      if (!this.CheckPrinterConfig.IsPrintCommentByGood)
        return;
      foreach (CheckGood checkGood in data.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (good =>
      {
        MarkedInfo markedInfo = good.MarkedInfo;
        return ((markedInfo != null ? (markedInfo.Type == GlobalDictionaries.RuMarkedProductionTypes.None ? 1 : 0) : 0) != 0 || good.MarkedInfo == null) && !string.IsNullOrEmpty(good.Description);
      })))
        checkGood.CommentForFiscalCheck.Add(string.Format(Translate.Atol8_RegisterGood_Комментарий___0_, (object) checkGood.Description));
    }

    private Cashier GetCashier(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      Cashier cashier = new Cashier();
      cashier.Name = basket.User?.Client?.Name ?? "";
      Gbs.Core.Entities.Users.User user1 = basket.User;
      // ISSUE: explicit non-virtual call
      cashier.UserUid = user1 != null ? __nonvirtual (user1.Uid) : Guid.Empty;
      Gbs.Core.Entities.Users.User user2 = basket.User;
      string str;
      if (user2 == null)
      {
        str = (string) null;
      }
      else
      {
        Client client = user2.Client;
        str = client != null ? client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() : (string) null;
      }
      if (str == null)
        str = "";
      cashier.Inn = str;
      return cashier;
    }

    private static void WriteGoodsToConsole(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      foreach (CheckGood goods in checkData.GoodsList)
        Other.ConsoleWrite(string.Format("{0}: p: {1}; q: {2}; d: {3}", (object) goods.Name, (object) goods.Price, (object) goods.Quantity, (object) goods.Discount));
    }

    private List<CheckPayment> PrepareCheckPayments(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      List<CheckPayment> list = basket.Payments.ToList<SelectPaymentMethods.PaymentGrid>().Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Sum.HasValue && x.Method != null)).Select<SelectPaymentMethods.PaymentGrid, CheckPayment>((Func<SelectPaymentMethods.PaymentGrid, CheckPayment>) (item => new CheckPayment()
      {
        Method = item.Method.KkmMethod,
        Name = item.Method.Name,
        Sum = item.Sum.GetValueOrDefault(),
        Type = item.Method.PaymentMethodsType
      })).ToList<CheckPayment>();
      foreach (CheckPayment checkPayment in list.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Certificate)))
        checkPayment.Method = GlobalDictionaries.KkmPaymentMethods.PrePayment;
      list.RemoveAll((Predicate<CheckPayment>) (x => x.Sum < 0.01M));
      if (basket.Delivery > 0M)
      {
        CheckPayment checkPayment = list.FirstOrDefault<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash));
        if (checkPayment == null)
          list.Add(new CheckPayment()
          {
            Sum = basket.Delivery,
            Method = GlobalDictionaries.KkmPaymentMethods.Cash
          });
        else
          checkPayment.Sum += basket.Delivery;
      }
      return list;
    }

    private Decimal GetChangeSumToRoundCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      RoundTotal roundTotals = new ConfigsRepository<Settings>().Get().Sales.RoundTotals;
      if (!roundTotals.IsEnable || roundTotals.Coefficient == 0M || checkData.CheckType == CheckTypes.ReturnSale)
        return 0M;
      Decimal sum = checkData.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      if (roundTotals.Coefficient > sum)
      {
        LogHelper.Debug("Сумма по чеку меньше, чем коэффициент округления. Не округляем");
        return 0M;
      }
      Decimal num = Math.Round(MathHelper.RoundToCoefficient(sum, roundTotals.Coefficient), 2, MidpointRounding.AwayFromZero);
      return sum - num;
    }

    private List<CheckGood> PrepareCheckGoodsForOnlineKkm(
      List<CheckGood> goodsForCheck,
      Decimal sumToSplit)
    {
      Performancer performancer = new Performancer("Подготовка позиций чека для онлайн-кассс");
      CheckRounder checkRounder = new CheckRounder();
      List<CheckGood> list1 = checkRounder.UndiscountCheckItemsForOnlineKkm(goodsForCheck).Where<CheckGood>((Func<CheckGood, bool>) (x => x.Sum > 0M)).ToList<CheckGood>();
      performancer.AddPoint("5");
      Decimal num = list1.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - goodsForCheck.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      sumToSplit += num;
      checkRounder.SplitSumForItems(list1, sumToSplit);
      performancer.AddPoint("10");
      this.UnionCheckItems(list1);
      performancer.AddPoint("20");
      List<CheckGood> list2 = list1.Select<CheckGood, CheckGood>((Func<CheckGood, CheckGood>) (x => x)).ToList<CheckGood>();
      performancer.AddPoint("30");
      performancer.Stop();
      return list2;
    }

    private void UnionCheckItems(List<CheckGood> checkItems)
    {
      List<CheckGood> checkGoodList = new List<CheckGood>();
      foreach (CheckGood checkItem in checkItems)
      {
        CheckGood item = checkItem;
        if (!checkGoodList.Any<CheckGood>(new Func<CheckGood, bool>(Predicate)))
        {
          Decimal num = checkItems.Where<CheckGood>(new Func<CheckGood, bool>(Predicate)).Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Quantity));
          item.Quantity = num;
          checkGoodList.Add(item);
        }

        bool Predicate(CheckGood x) => x.Uid == item.Uid && x.Price == item.Price;
      }
      checkItems.Clear();
      checkItems.AddRange((IEnumerable<CheckGood>) checkGoodList);
    }

    private List<CheckGood> GetCheckGoodsListFromBasket(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      List<CheckGood> goodsListFromBasket = new List<CheckGood>();
      foreach (BasketItem basketItem in (Collection<BasketItem>) basket.Items)
      {
        Decimal quantity = basketItem.Quantity;
        string displayName = basketItem.DisplayedName;
        Decimal salePrice = basketItem.SalePrice;
        if (basketItem.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Tobacco)
        {
          int num = 10;
          if (basketItem.Quantity == (Decimal) num)
          {
            quantity /= (Decimal) num;
            salePrice *= (Decimal) num;
            displayName = basketItem.DisplayedName + string.Format(Translate.CheckFactory_GetCheckGoodsListFromBasket____0__шт_, (object) num);
          }
        }
        CheckGood checkGood = new CheckGood(basketItem.Good, salePrice, basketItem.Discount.Value, quantity, basketItem.Comment, displayName)
        {
          Uid = basketItem.Uid,
          CertificateInfo = basketItem.Certificate
        };
        goodsListFromBasket.Add(checkGood);
      }
      return goodsListFromBasket;
    }

    private GlobalDictionaries.RuTaxSystems GetRuTaxSystemForGoods(List<Gbs.Core.Entities.Goods.Good> goods)
    {
      LogHelper.Debug(string.Format("Устанавливаю СНО для чека. СНО в натройках по умаолчанию: {0}", (object) this.CheckPrinterConfig.FiscalKkm.DefaultRuTaxSystem));
      List<GlobalDictionaries.RuTaxSystems> list = goods.Select<Gbs.Core.Entities.Goods.Good, GlobalDictionaries.RuTaxSystems>((Func<Gbs.Core.Entities.Goods.Good, GlobalDictionaries.RuTaxSystems>) (g => g?.Group?.RuTaxSystem.GetValueOrDefault(GlobalDictionaries.RuTaxSystems.None))).ToList<GlobalDictionaries.RuTaxSystems>().Distinct<GlobalDictionaries.RuTaxSystems>().ToList<GlobalDictionaries.RuTaxSystems>();
      if (list.Any<GlobalDictionaries.RuTaxSystems>((Func<GlobalDictionaries.RuTaxSystems, bool>) (x => x == GlobalDictionaries.RuTaxSystems.None)))
        list.Add(this.CheckPrinterConfig.FiscalKkm.DefaultRuTaxSystem);
      if (this.CheckPrinterConfig.FiscalKkm.DefaultRuTaxSystem != GlobalDictionaries.RuTaxSystems.None)
        list.RemoveAll((Predicate<GlobalDictionaries.RuTaxSystems>) (x => x == GlobalDictionaries.RuTaxSystems.None));
      switch (list.Distinct<GlobalDictionaries.RuTaxSystems>().Count<GlobalDictionaries.RuTaxSystems>())
      {
        case 0:
          return this.CheckPrinterConfig.FiscalKkm.DefaultRuTaxSystem;
        case 1:
          return list.First<GlobalDictionaries.RuTaxSystems>();
        default:
          throw new ArgumentException(Translate.CheckData_Чек_содержит_товары_с_разной_СНО__В_чеке_не_могут_быть_товары_с_разной_СНО);
      }
    }

    public void SetAndCheckTaxSystem(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      checkData.RuTaxSystem = this.GetRuTaxSystemForGoods(checkData.GoodsList.Select<CheckGood, Gbs.Core.Entities.Goods.Good>((Func<CheckGood, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>());
      LogHelper.Debug(string.Format("Устанавливаю СНО для чека: {0} СНО в натройках по умаолчанию: {1}", (object) checkData.RuTaxSystem, (object) devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem));
    }

    private void SetTaxRates(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x =>
      {
        Gbs.Core.Entities.Goods.Good good = x.Good;
        if (good == null)
          return false;
        int? taxRateNumber = good.Group?.TaxRateNumber;
        int num = 0;
        return taxRateNumber.GetValueOrDefault() == num & taxRateNumber.HasValue;
      })))
      {
        int defaultTaxRateId = this.CheckPrinterConfig.FiscalKkm.DefaultTaxRate;
        List<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>> list = this.CheckPrinterConfig.FiscalKkm.TaxRates.Where<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>((Func<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>, bool>) (x => x.Key == defaultTaxRateId)).ToList<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>();
        int num = list.Count == 1 ? list.Single<KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate>>().Value.KkmIndex : 1;
        checkGood.TaxRateNumber = num;
      }
    }

    private void SetFfdGoodTypeCode(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (x => x.Good.Group.RuMarkedProductionType != 0)))
      {
        switch (checkGood.RuFfdGoodTypeCode)
        {
          case GlobalDictionaries.RuFfdGoodsTypes.SimpleGood:
            checkGood.RuFfdGoodTypeCode = string.IsNullOrEmpty(checkGood.MarkedInfo?.FullCode) ? GlobalDictionaries.RuFfdGoodsTypes.Tnm : GlobalDictionaries.RuFfdGoodsTypes.Tm;
            continue;
          case GlobalDictionaries.RuFfdGoodsTypes.ExcisableGood:
            checkGood.RuFfdGoodTypeCode = string.IsNullOrEmpty(checkGood.MarkedInfo?.FullCode) ? GlobalDictionaries.RuFfdGoodsTypes.Atnm : GlobalDictionaries.RuFfdGoodsTypes.Atm;
            continue;
          default:
            continue;
        }
      }
    }

    public class AdjustableItem
    {
      public object Object { get; set; }

      public Decimal Sum { get; set; }

      public AdjustableItem(object o, Decimal sum)
      {
        this.Object = o;
        this.Sum = sum;
      }
    }

    private class DiscountInfo
    {
      public Guid Uid { get; set; }

      public Decimal Discount { get; set; }

      public Decimal PriceWithoutDiscount { get; set; }
    }

    public class PrepareOptions
    {
      public bool RoundCheck { get; set; } = true;
    }
  }
}
