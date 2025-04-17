// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Facade.Bonuses
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.DbConfigs;
using Gbs.Core.Entities.Settings.Discount;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Settings.Facade
{
  public class Bonuses : IDbConfig
  {
    public ClientBonusesRepository ClientBonusesRepository { get; set; } = new ClientBonusesRepository();

    public bool IsActiveBonuses { get; set; }

    public Decimal MaxValueBonuses { get; set; }

    public int ValidityPeriodBonuses { get; set; } = -1;

    public List<Guid> ListMethod { get; set; }

    public Bonuses.BonusesOption OptionRuleBonuses { get; set; }

    public bool Save()
    {
      bool flag1 = true;
      SettingsRepository settingsRepository1 = new SettingsRepository();
      bool flag2 = ((((((((flag1 ? 1 : 0) & (settingsRepository1.Save(new Setting()
      {
        Type = Types.ActiveBonuses,
        Value = (object) this.IsActiveBonuses
      }) ? 1 : 0)) != 0 ? 1 : 0) & (settingsRepository1.Save(new Setting()
      {
        Type = Types.MaxValueBonuses,
        Value = (object) this.MaxValueBonuses
      }) ? 1 : 0)) != 0 ? 1 : 0) & (settingsRepository1.Save(new Setting()
      {
        Type = Types.OptionRuleBonuses,
        Value = (object) (int) this.OptionRuleBonuses
      }) ? 1 : 0)) != 0 ? 1 : 0) & (settingsRepository1.Save(new Setting()
      {
        Type = Types.ValidityPeriodBonuses,
        Value = (object) this.ValidityPeriodBonuses
      }) ? 1 : 0)) != 0;
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.MethodPaymentBonuses)).ToList<Setting>());
      foreach (Guid guid in this.ListMethod)
      {
        SettingsRepository settingsRepository2 = new SettingsRepository();
        Setting setting = new Setting();
        setting.Uid = guid;
        setting.Parameter = guid.ToString();
        setting.Value = (object) string.Empty;
        setting.Type = Types.MethodPaymentBonuses;
        setting.IsDeleted = false;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository2.Save(setting));
      }
      return flag2;
    }

    public bool Load()
    {
      SettingsRepository settingsRepository = new SettingsRepository();
      this.IsActiveBonuses = (bool) (settingsRepository.GetSettingByType(Types.ActiveBonuses)?.Value ?? (object) false);
      this.MaxValueBonuses = Convert.ToDecimal(settingsRepository.GetSettingByType(Types.MaxValueBonuses)?.Value ?? (object) 100);
      this.OptionRuleBonuses = (Bonuses.BonusesOption) Convert.ToInt32(settingsRepository.GetSettingByType(Types.OptionRuleBonuses)?.Value ?? (object) Bonuses.BonusesOption.AllSale);
      this.ListMethod = settingsRepository.GetSettingListByType(Types.MethodPaymentBonuses, false).Select<Setting, Guid>((Func<Setting, Guid>) (x => x.Uid)).ToList<Guid>();
      this.ValidityPeriodBonuses = Convert.ToInt32(settingsRepository.GetSettingByType(Types.ValidityPeriodBonuses)?.Value ?? (object) -1);
      return true;
    }

    public enum BonusesOption
    {
      AllSale,
      SaleOffBonuses,
      OffBonuses,
    }
  }
}
