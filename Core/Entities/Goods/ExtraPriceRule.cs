// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.ExtraPriceRule
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public class ExtraPriceRule : Entity
  {
    public string Name { get; set; }

    public List<GoodGroups.Group> Groups { get; set; } = new List<GoodGroups.Group>();

    public List<ExtraPriceRule.ItemPricing> Items { get; set; } = GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)).Select<GoodsExtraPrice.GoodExtraPrice, ExtraPriceRule.ItemPricing>((Func<GoodsExtraPrice.GoodExtraPrice, ExtraPriceRule.ItemPricing>) (x => new ExtraPriceRule.ItemPricing()
    {
      Price = x
    })).ToList<ExtraPriceRule.ItemPricing>();

    public class ItemPricing
    {
      public GoodsExtraPrice.GoodExtraPrice Price { get; set; }

      public GoodsExtraPrice.TypeCoeff Type { get; set; } = GoodsExtraPrice.TypeCoeff.SalePrice;

      public Decimal Value { get; set; }
    }
  }
}
