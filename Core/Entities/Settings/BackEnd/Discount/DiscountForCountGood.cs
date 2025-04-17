// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForCountGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd.Discount
{
  public class DiscountForCountGood : Entity
  {
    public string Name { get; set; }

    public List<GoodGroups.Group> Groups { get; set; } = new List<GoodGroups.Group>();

    public List<DiscountForCountGood.Item> Items { get; set; } = new List<DiscountForCountGood.Item>()
    {
      new DiscountForCountGood.Item()
    };

    public class Item
    {
      public Decimal MinCount { get; set; }

      public Decimal DiscountValue { get; set; }
    }
  }
}
