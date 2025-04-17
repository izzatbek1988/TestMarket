// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Models.Basket.BasketDiscounter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Settings;
using Gbs.Core.Entities.Settings.BackEnd.Discount;
using Gbs.Core.Entities.Settings.Discount;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents.Sales;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Models.Basket
{
  public class BasketDiscounter
  {
    private Gbs.Core.ViewModels.Basket.Basket Basket { get; set; }

    private static BuyPriceCounter BuyPriceCounter { get; set; }

    public bool IsLoadDiscountRule { get; set; } = true;

    public BasketDiscounter(Gbs.Core.ViewModels.Basket.Basket basket) => this.Basket = basket;

    private static BasketDiscounter.RulesBox LoadRules()
    {
      Performancer performancer = new Performancer("Загрузка правил дисконта");
      BasketDiscounter.RulesBox rulesBox = new BasketDiscounter.RulesBox();
      try
      {
        rulesBox.DiscountForDayOfMonthsRules = (IEnumerable<DiscountForDayOfMonth>) new DiscountForDayOfMonthRepository().GetDiscountForDayOfMonth().Where<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x =>
        {
          DateTime dateTime1 = x.TimeStart;
          TimeSpan timeOfDay1 = dateTime1.TimeOfDay;
          dateTime1 = DateTime.Now;
          TimeSpan timeOfDay2 = dateTime1.TimeOfDay;
          if (!(timeOfDay1 <= timeOfDay2))
            return false;
          DateTime dateTime2 = x.TimeFinish;
          TimeSpan timeOfDay3 = dateTime2.TimeOfDay;
          dateTime2 = DateTime.Now;
          TimeSpan timeOfDay4 = dateTime2.TimeOfDay;
          return timeOfDay3 >= timeOfDay4;
        })).ToList<DiscountForDayOfMonth>();
        performancer.AddPoint(string.Format("Скидки в день месяца: {0}", (object) rulesBox.DiscountForDayOfMonthsRules.Count<DiscountForDayOfMonth>()));
        rulesBox.DiscountForBirthdayRule = new DiscountForBirthdayRepository().GetAllItems().SingleOrDefault<DiscountForBirthday>() ?? new DiscountForBirthday();
        performancer.AddPoint("Скидки в день рождения");
        rulesBox.DiscountForWeekDayRules = (IEnumerable<DiscountForWeekday>) new DiscountForWeekdayRepository().GetDiscountForWeekday().Where<DiscountForWeekday>((Func<DiscountForWeekday, bool>) (x =>
        {
          DateTime dateTime3 = x.TimeStart;
          TimeSpan timeOfDay5 = dateTime3.TimeOfDay;
          dateTime3 = DateTime.Now;
          TimeSpan timeOfDay6 = dateTime3.TimeOfDay;
          if (!(timeOfDay5 <= timeOfDay6))
            return false;
          DateTime dateTime4 = x.TimeFinish;
          TimeSpan timeOfDay7 = dateTime4.TimeOfDay;
          dateTime4 = DateTime.Now;
          TimeSpan timeOfDay8 = dateTime4.TimeOfDay;
          return timeOfDay7 >= timeOfDay8;
        })).ToList<DiscountForWeekday>();
        performancer.AddPoint(string.Format("Скидка по дням недели: {0}", (object) rulesBox.DiscountForWeekDayRules.Count<DiscountForWeekday>()));
        rulesBox.DiscountFromSumInCheckRules = (IEnumerable<DiscountFromSumInCheck>) new DiscountFromSumInCheckRepository().GetActiveItems().ToList<DiscountFromSumInCheck>();
        performancer.AddPoint(string.Format("Скидка от суммы чека: {0}", (object) rulesBox.DiscountFromSumInCheckRules.Count<DiscountFromSumInCheck>()));
        rulesBox.DiscountForClientTotalSaleSumRules = (IEnumerable<DiscountSumSale>) new DiscountSumSaleRepository().GetActiveItems().ToList<DiscountSumSale>();
        performancer.AddPoint(string.Format("Скидка от суммы покупок: {0}", (object) rulesBox.DiscountForClientTotalSaleSumRules.Count<DiscountSumSale>()));
        rulesBox.DiscountForCountGoodRules = (IEnumerable<DiscountForCountGood>) new DiscountForCountGoodRepository().GetActiveItems().ToList<DiscountForCountGood>();
        performancer.AddPoint(string.Format("Скидка от количества товара: {0}", (object) rulesBox.DiscountForCountGoodRules.Count<DiscountForCountGood>()));
        rulesBox.MaxDiscountRules = (IEnumerable<MaxDiscountRule>) new MaxDiscountsRuleRepository().GetActiveItems().Where<MaxDiscountRule>((Func<MaxDiscountRule, bool>) (x => !x.IsOff)).ToList<MaxDiscountRule>();
        performancer.AddPoint(string.Format("Макс. скидка: {0}", (object) rulesBox.MaxDiscountRules.Count<MaxDiscountRule>()));
        rulesBox.DefaultMaxDiscount = Convert.ToDecimal(new SettingsRepository().GetSettingByType(Gbs.Core.Entities.Settings.Types.MaxDiscountGood)?.Value ?? (object) 100);
        performancer.AddPoint("Макс. скидка по умолчанию");
        Other.ConsoleWrite("default max discount: " + rulesBox.DefaultMaxDiscount.ToString());
        BasketDiscounter.BuyPriceCounter = new BuyPriceCounter();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки правил дискаунта");
      }
      performancer.Stop();
      return rulesBox;
    }

    public void SetAllDiscount()
    {
      if (!this.IsLoadDiscountRule)
        return;
      this.SetMaxDiscountValue();
      this.ClearRulesDiscount();
      this.SetDiscountForDayOfMonth();
      this.SetDiscountFroWeekDay();
      this.SetDiscountOnCheckSum();
      this.SetClientGroupDiscount();
      this.SetDiscountFromClientTotalSaleSum();
      this.SetDiscountForBirthdayRule();
      this.SetDiscountFromCountGood();
    }

    private void SetDiscountForBirthdayRule()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountForBirthdayRule.IsActive || this.Basket.Client == null)
        return;
      DateTime? birthday = this.Basket.Client.Client.Birthday;
      if (!birthday.HasValue)
        return;
      DateTime now = DateTime.Now;
      int year1 = now.Year;
      birthday = this.Basket.Client.Client.Birthday;
      now = birthday.Value;
      int year2 = now.Year;
      int num1 = year1 - year2;
      birthday = this.Basket.Client.Client.Birthday;
      now = birthday.Value;
      DateTime dateTime = now.AddYears(num1);
      double num2 = Math.Abs((DateTime.Today - dateTime).TotalDays);
      if (num2 > (double) rules.DiscountForBirthdayRule.CountDay)
        return;
      LogHelper.Debug(string.Format("Установка скидки по ДР {0}%; дней до\\после ДР:{1}; ДР: {2}", (object) rules.DiscountForBirthdayRule.Discount, (object) num2, (object) dateTime));
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        if (!(basketItem.Discount.Value >= rules.DiscountForBirthdayRule.Discount))
          basketItem.Discount.SetDiscount(rules.DiscountForBirthdayRule.Discount, SaleDiscount.ReasonEnum.Rules, basketItem, this.Basket);
      }
    }

    private void SetDiscountFromClientTotalSaleSum()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountForClientTotalSaleSumRules.Any<DiscountSumSale>() || this.Basket.Client == null)
        return;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        DiscountSumSale discountSumSale = rules.DiscountForClientTotalSaleSumRules.FirstOrDefault<DiscountSumSale>((Func<DiscountSumSale, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid))));
        if (discountSumSale != null)
        {
          List<DiscountSumSale.Item> list = discountSumSale.Items.Where<DiscountSumSale.Item>((Func<DiscountSumSale.Item, bool>) (x => x.MinSum <= this.Basket.Client.TotalSalesSum)).ToList<DiscountSumSale.Item>();
          Decimal num = list.Any<DiscountSumSale.Item>() ? list.Max<DiscountSumSale.Item>((Func<DiscountSumSale.Item, Decimal>) (x => x.DiscountValue)) : 0M;
          if (!(item.Discount.Value >= num))
            item.Discount.SetDiscount(num, SaleDiscount.ReasonEnum.Rules, item, this.Basket);
        }
      }
    }

    private void SetDiscountFromCountGood()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountForCountGoodRules.Any<DiscountForCountGood>())
        return;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        DiscountForCountGood discountForCountGood = rules.DiscountForCountGoodRules.FirstOrDefault<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid))));
        if (discountForCountGood != null)
        {
          Decimal countSum = this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == item.Good.Uid)).Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
          List<DiscountForCountGood.Item> list = discountForCountGood.Items.Where<DiscountForCountGood.Item>((Func<DiscountForCountGood.Item, bool>) (x => x.MinCount <= countSum)).ToList<DiscountForCountGood.Item>();
          Decimal num = list.Any<DiscountForCountGood.Item>() ? list.Max<DiscountForCountGood.Item>((Func<DiscountForCountGood.Item, Decimal>) (x => x.DiscountValue)) : 0M;
          if (!(item.Discount.Value >= num))
            item.Discount.SetDiscount(num, SaleDiscount.ReasonEnum.Rules, item, this.Basket);
        }
      }
    }

    private void SetDiscountFroWeekDay()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountForWeekDayRules.Any<DiscountForWeekday>())
        return;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        List<DiscountForWeekday> list = rules.DiscountForWeekDayRules.Where<DiscountForWeekday>((Func<DiscountForWeekday, bool>) (x => x.ListGroup.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid)))).ToList<DiscountForWeekday>();
        Decimal num = list.Any<DiscountForWeekday>() ? list.Max<DiscountForWeekday>((Func<DiscountForWeekday, Decimal>) (x => x.Discount)) : 0M;
        if (!(item.Discount.Value >= num))
          item.Discount.SetDiscount(num, SaleDiscount.ReasonEnum.Rules, item, this.Basket);
      }
    }

    private void SetClientGroupDiscount()
    {
      if (this.Basket.Client?.Client?.Group == null)
        return;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)).Where<BasketItem>((Func<BasketItem, bool>) (item => item.Discount.Value < this.Basket.Client.Client.Group.Discount)))
        basketItem.Discount.SetDiscount(this.Basket.Client.Client.Group.Discount, SaleDiscount.ReasonEnum.Rules, basketItem, this.Basket);
    }

    private void SetMaxDiscountValue()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        IEnumerable<MaxDiscountRule> maxDiscountRules = rules.MaxDiscountRules;
        List<MaxDiscountRule> source = (maxDiscountRules != null ? maxDiscountRules.Where<MaxDiscountRule>((Func<MaxDiscountRule, bool>) (x => x.ListGroup.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid)))).ToList<MaxDiscountRule>() : (List<MaxDiscountRule>) null) ?? new List<MaxDiscountRule>();
        Decimal num1 = source.Any<MaxDiscountRule>() ? source.Min<MaxDiscountRule>((Func<MaxDiscountRule, Decimal>) (x => x.MaxDiscount)) : rules.DefaultMaxDiscount;
        if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Sales.IsTabooSaleLessBuyPrice)
        {
          Decimal lastBuyPrice = BasketDiscounter.BuyPriceCounter.GetLastBuyPrice(item.Good);
          Decimal num2 = Math.Round((item.BasePrice - lastBuyPrice) / item.SalePrice * 100M, 2, MidpointRounding.AwayFromZero);
          num1 = !(num2 < num1) || !(num2 >= 0M) ? num1 : num2;
        }
        item.Discount.MaxValue = num1;
      }
    }

    private void SetDiscountForDayOfMonth()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountForDayOfMonthsRules.Any<DiscountForDayOfMonth>())
        return;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        List<DiscountForDayOfMonth> list = rules.DiscountForDayOfMonthsRules.Where<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x => x.ListGroup.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid)))).ToList<DiscountForDayOfMonth>();
        Decimal num = list.Any<DiscountForDayOfMonth>() ? list.Max<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, Decimal>) (x => x.Discount)) : 0M;
        if (!(item.Discount.Value >= num))
          item.Discount.SetDiscount(num, SaleDiscount.ReasonEnum.Rules, item, this.Basket);
      }
    }

    private void ClearRulesDiscount()
    {
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
        basketItem.Discount.CLearRuleValue();
    }

    private void SetDiscountOnCheckSum()
    {
      BasketDiscounter.RulesBox rules = this.GetRules();
      if (!rules.DiscountFromSumInCheckRules.Any<DiscountFromSumInCheck>())
        return;
      Decimal basketSumWithoutDiscount = this.Basket.TotalSum + this.Basket.TotalDiscount;
      foreach (BasketItem basketItem in this.Basket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)))
      {
        BasketItem item = basketItem;
        DiscountFromSumInCheck discountFromSumInCheck = rules.DiscountFromSumInCheckRules.FirstOrDefault<DiscountFromSumInCheck>((Func<DiscountFromSumInCheck, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid))));
        if (discountFromSumInCheck != null)
        {
          List<DiscountFromSumInCheck.Item> list = discountFromSumInCheck.Items.Where<DiscountFromSumInCheck.Item>((Func<DiscountFromSumInCheck.Item, bool>) (x => x.MinSum <= basketSumWithoutDiscount)).ToList<DiscountFromSumInCheck.Item>();
          Decimal num = list.Any<DiscountFromSumInCheck.Item>() ? list.Max<DiscountFromSumInCheck.Item>((Func<DiscountFromSumInCheck.Item, Decimal>) (x => x.DiscountValue)) : 0M;
          if (!(num <= item.Discount.Value))
            item.Discount.SetDiscount(num, SaleDiscount.ReasonEnum.Rules, item, this.Basket);
        }
      }
    }

    public static void ReloadRules()
    {
      CacheHelper.Get<BasketDiscounter.RulesBox>(CacheHelper.CacheTypes.DiscountRules, new Func<BasketDiscounter.RulesBox>(BasketDiscounter.LoadRules));
    }

    private BasketDiscounter.RulesBox GetRules()
    {
      return CacheHelper.Get<BasketDiscounter.RulesBox>(CacheHelper.CacheTypes.DiscountRules, new Func<BasketDiscounter.RulesBox>(BasketDiscounter.LoadRules));
    }

    private class RulesBox
    {
      public IEnumerable<DiscountForWeekday> DiscountForWeekDayRules { get; set; }

      public IEnumerable<DiscountForDayOfMonth> DiscountForDayOfMonthsRules { get; set; }

      public IEnumerable<DiscountFromSumInCheck> DiscountFromSumInCheckRules { get; set; }

      public IEnumerable<DiscountSumSale> DiscountForClientTotalSaleSumRules { get; set; }

      public IEnumerable<DiscountForCountGood> DiscountForCountGoodRules { get; set; }

      public IEnumerable<MaxDiscountRule> MaxDiscountRules { get; set; }

      public DiscountForBirthday DiscountForBirthdayRule { get; set; }

      public Decimal DefaultMaxDiscount { get; set; }
    }
  }
}
