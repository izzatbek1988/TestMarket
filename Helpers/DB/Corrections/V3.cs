// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V3
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers.Logging;
using System;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V3 : ICorrection
  {
    private bool TransferSettingBonuses()
    {
      try
      {
        LogHelper.Debug("Корректировка БД: перенос настреок бонусной системы");
        Gbs.Core.Config.Settings config = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        Discounts discounts = config.Discounts ?? new Discounts();
        bool flag = true;
        if (new SettingsRepository().GetSettingByType(Gbs.Core.Entities.Settings.Types.ActiveBonuses) == null)
          flag = ((flag ? 1 : 0) & (new SettingsRepository().Save(new Setting()
          {
            Type = Gbs.Core.Entities.Settings.Types.ActiveBonuses,
            Value = (object) discounts.IsActiveBonuses
          }) ? 1 : 0)) != 0;
        if (new SettingsRepository().GetSettingByType(Gbs.Core.Entities.Settings.Types.MaxValueBonuses) == null)
          flag = ((flag ? 1 : 0) & (new SettingsRepository().Save(new Setting()
          {
            Type = Gbs.Core.Entities.Settings.Types.MaxValueBonuses,
            Value = (object) discounts.MaxValueBonuses
          }) ? 1 : 0)) != 0;
        if (new SettingsRepository().GetSettingByType(Gbs.Core.Entities.Settings.Types.OptionRuleBonuses) == null)
          flag = ((flag ? 1 : 0) & (new SettingsRepository().Save(new Setting()
          {
            Type = Gbs.Core.Entities.Settings.Types.OptionRuleBonuses,
            Value = (object) (int) discounts.OptionRuleBonuses
          }) ? 1 : 0)) != 0;
        if (new SettingsRepository().GetSettingByType(Gbs.Core.Entities.Settings.Types.MaxDiscountGood) == null)
          flag = ((flag ? 1 : 0) & (new SettingsRepository().Save(new Setting()
          {
            Type = Gbs.Core.Entities.Settings.Types.MaxDiscountGood,
            Value = (object) discounts.MaxDiscountGood
          }) ? 1 : 0)) != 0;
        if (flag)
        {
          config.Discounts = (Discounts) null;
          new ConfigsRepository<Gbs.Core.Config.Settings>().Save(config);
        }
        return flag;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка переноса настроек бонусной системы", false);
        return false;
      }
    }

    public bool Do() => this.TransferSettingBonuses();
  }
}
