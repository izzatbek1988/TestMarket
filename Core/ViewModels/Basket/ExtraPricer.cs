// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Basket.ExtraPricer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Gbs.Core.ViewModels.Basket
{
  public class ExtraPricer
  {
    private static BuyPriceCounter _counter;

    private Gbs.Core.ViewModels.Basket.Basket Basket { get; set; }

    private static List<ExtraPriceRule> Rules
    {
      get
      {
        return CacheHelper.Get<List<ExtraPriceRule>>(CacheHelper.CacheTypes.ExtraPriceRules, new Func<List<ExtraPriceRule>>(ExtraPricer.GetRules));
      }
    }

    public ExtraPricer(Gbs.Core.ViewModels.Basket.Basket basket) => this.Basket = basket;

    private static List<ExtraPriceRule> GetRules()
    {
      Performancer performancer = new Performancer("Загрузка правил доп. цен");
      List<ExtraPriceRule> list = new ExtraPriceRulesRepository().GetActiveItems().ToList<ExtraPriceRule>();
      ExtraPricer._counter = new BuyPriceCounter();
      performancer.Stop();
      return list;
    }

    public bool ReCalcPrices()
    {
      try
      {
        foreach (BasketItem basketItem in (Collection<BasketItem>) this.Basket.Items)
          this.SetPriceForItem(basketItem);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка установки доп. цен на товары");
        return false;
      }
    }

    public bool SetPriceForItem(BasketItem item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      if (item.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate || item.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)
        return true;
      Decimal num1 = 1M;
      GoodsExtraPrice.GoodExtraPrice clientPrice = this.Basket.Client?.Client?.Group?.Price;
      GoodsExtraPrice.TypeCoeff typeCoeff = GoodsExtraPrice.TypeCoeff.SalePrice;
      if (clientPrice != null && clientPrice.Uid != Guid.Empty && !item.IsPriceEditByUser)
      {
        ExtraPriceRule extraPriceRule1 = ExtraPricer.Rules.SingleOrDefault<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => x.Items.Any<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (i => i.Price.Uid == clientPrice.Uid)) && !x.IsDeleted && x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid)) && x.Uid != GlobalDictionaries.DefaultExtraRuleUid));
        ExtraPriceRule.ItemPricing itemPricing1 = extraPriceRule1 != null ? extraPriceRule1.Items.First<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (x => x.Price.Uid == clientPrice.Uid)) : (ExtraPriceRule.ItemPricing) null;
        if (itemPricing1 != null)
        {
          num1 = itemPricing1.Value;
          typeCoeff = itemPricing1.Type;
        }
        else
        {
          ExtraPriceRule extraPriceRule2 = ExtraPricer.Rules.SingleOrDefault<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => x.Items.Any<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (i => i.Price.Uid == clientPrice.Uid)) && !x.IsDeleted && x.Uid == GlobalDictionaries.DefaultExtraRuleUid));
          ExtraPriceRule.ItemPricing itemPricing2 = extraPriceRule2 != null ? extraPriceRule2.Items.First<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (x => x.Price.Uid == clientPrice.Uid)) : (ExtraPriceRule.ItemPricing) null;
          if (itemPricing2 != null)
          {
            num1 = itemPricing2.Value;
            typeCoeff = itemPricing2.Type;
          }
        }
      }
      Decimal num2 = typeCoeff == GoodsExtraPrice.TypeCoeff.SalePrice ? item.BasePrice : ExtraPricer._counter.GetLastBuyPrice(item.Good);
      if (num2 != 0M)
        item.SalePrice = Math.Round(num2 * num1, 2, MidpointRounding.AwayFromZero);
      return true;
    }
  }
}
