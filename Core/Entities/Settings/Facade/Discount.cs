// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Facade.Discount
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.DbConfigs;
using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Core.Entities.Settings.BackEnd.Discount;
using Gbs.Core.Entities.Settings.Discount;
using System;

#nullable disable
namespace Gbs.Core.Entities.Settings.Facade
{
  public class Discount : IDbConfig
  {
    public Decimal DefaultMaxDiscount { get; set; } = 100M;

    public DiscountForBirthdayRepository DiscountForBirthdayRepository { get; set; }

    public MaxDiscountsRuleRepository MaxDiscountsRuleRepository { get; set; }

    public PercentForServiceRuleRepository PercentForServiceRuleRepository { get; set; }

    public DiscountForDayOfMonthRepository DiscountForDayOfMonthRepository { get; set; }

    public DiscountForWeekdayRepository DiscountForWeekdayRepository { get; set; }

    public DiscountFromSumInCheckRepository DiscountFromSumInCheckRepository { get; set; }

    public DiscountForCountGoodRepository DiscountForCountGoodRepository { get; set; }

    public DiscountSumSaleRepository DiscountSumSaleRepository { get; set; }

    public bool Save()
    {
      return (1 & (new SettingsRepository().Save(new Setting()
      {
        Type = Types.MaxDiscountGood,
        Value = (object) this.DefaultMaxDiscount
      }) ? 1 : 0)) != 0;
    }

    public bool Load()
    {
      this.MaxDiscountsRuleRepository = new MaxDiscountsRuleRepository();
      this.DiscountForDayOfMonthRepository = new DiscountForDayOfMonthRepository();
      this.DiscountForWeekdayRepository = new DiscountForWeekdayRepository();
      this.DiscountFromSumInCheckRepository = new DiscountFromSumInCheckRepository();
      this.DiscountSumSaleRepository = new DiscountSumSaleRepository();
      this.PercentForServiceRuleRepository = new PercentForServiceRuleRepository();
      this.DiscountForCountGoodRepository = new DiscountForCountGoodRepository();
      this.DiscountForBirthdayRepository = new DiscountForBirthdayRepository();
      this.DefaultMaxDiscount = Convert.ToDecimal(new SettingsRepository().GetSettingByType(Types.MaxDiscountGood)?.Value ?? (object) 100);
      return true;
    }
  }
}
