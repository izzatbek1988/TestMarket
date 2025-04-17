// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Discounts
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings.Facade;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  [Serializable]
  public class Discounts
  {
    public Decimal MaxDiscountGood { get; set; } = 100M;

    public bool IsActiveBonuses { get; set; }

    public Decimal MaxValueBonuses { get; set; } = 100M;

    public Bonuses.BonusesOption OptionRuleBonuses { get; set; }
  }
}
