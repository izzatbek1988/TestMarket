// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.PricingRulesRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public class PricingRulesRepository : IRepository<PricingRules>
  {
    public int Delete(List<PricingRules> itemsList) => throw new NotImplementedException();

    public bool Delete(PricingRules item) => throw new NotImplementedException();

    public List<PricingRules> GetActiveItems()
    {
      return this.GetAllItems().Where<PricingRules>((Func<PricingRules, bool>) (x => !x.IsDeleted)).ToList<PricingRules>();
    }

    public List<PricingRules> GetAllItems()
    {
      List<Setting> list = new SettingsRepository().GetActiveItems().ToList<Setting>().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRuleGroups || x.Type == Types.PricingRules || x.Type == Types.PricingRuleItem)).ToList<Setting>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<PricingRules> allItems2 = new List<PricingRules>();
        foreach (Setting setting in list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          IEnumerable<string> groupsUids = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source1 = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          IEnumerable<PricingRules.ItemPricing> source2 = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRuleItem && x.EntityUid == item.EntityUid)).Select<Setting, PricingRules.ItemPricing>((Func<Setting, PricingRules.ItemPricing>) (x => JsonConvert.DeserializeObject<PricingRules.ItemPricing>(x.Value.ToString())));
          List<PricingRules> pricingRulesList = allItems2;
          PricingRules pricingRules = new PricingRules();
          pricingRules.Uid = item.EntityUid;
          pricingRules.Name = item.Value.ToString();
          pricingRules.Groups = source1.ToList<GoodGroups.Group>();
          pricingRules.Items = source2.ToList<PricingRules.ItemPricing>();
          pricingRulesList.Add(pricingRules);
        }
        return allItems2;
      }
    }

    public PricingRules GetByUid(Guid uid)
    {
      return this.GetAllItems().FirstOrDefault<PricingRules>((Func<PricingRules, bool>) (x => x.Uid == uid));
    }

    public bool Save(PricingRules item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        MessageBoxHelper.Warning(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.PricingRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting1);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.Groups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting2 = new Setting();
        setting2.Parameter = group.Uid.ToString();
        setting2.EntityUid = item.Uid;
        setting2.Value = (object) string.Empty;
        setting2.IsDeleted = item.IsDeleted;
        setting2.Type = Types.PricingRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting2));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.PricingRuleItem && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (PricingRules.ItemPricing itemPricing in item.Items)
      {
        Guid guid = Guid.NewGuid();
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = guid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) itemPricing.ToJsonString();
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.PricingRuleItem;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<PricingRules> itemsList)
    {
      return itemsList.Count<PricingRules>(new Func<PricingRules, bool>(this.Save));
    }

    public ActionResult Validate(PricingRules item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.Groups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.Items.Any<PricingRules.ItemPricing>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_указать_хотя_бы_одно_подправило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
