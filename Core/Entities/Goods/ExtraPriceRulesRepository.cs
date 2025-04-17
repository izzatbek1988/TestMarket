// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.ExtraPriceRulesRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public class ExtraPriceRulesRepository : IRepository<ExtraPriceRule>
  {
    public int Delete(List<ExtraPriceRule> itemsList) => throw new NotImplementedException();

    public bool Delete(ExtraPriceRule item) => throw new NotImplementedException();

    public List<ExtraPriceRule> GetActiveItems()
    {
      return this.GetAllItems().Where<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => !x.IsDeleted)).ToList<ExtraPriceRule>();
    }

    public List<ExtraPriceRule> GetAllItems()
    {
      LogHelper.OnBegin();
      List<Setting> list1 = new SettingsRepository().GetActiveItems().ToList<Setting>().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRules || x.Type == Types.ExtraPricingRuleGroups || x.Type == Types.ExtraPricingRuleItem)).ToList<Setting>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<ExtraPriceRule> allItems2 = new List<ExtraPriceRule>();
        foreach (Setting setting in list1.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          IEnumerable<string> groupsUids = list1.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          List<GoodsExtraPrice.GoodExtraPrice> list2 = GoodsExtraPrice.GetGoodExtraPriceList().ToList<GoodsExtraPrice.GoodExtraPrice>();
          List<ExtraPriceRule.ItemPricing> list3 = list1.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRuleItem && x.EntityUid == item.EntityUid)).Select<Setting, ExtraPriceRule.ItemPricing>((Func<Setting, ExtraPriceRule.ItemPricing>) (x => JsonConvert.DeserializeObject<ExtraPriceRule.ItemPricing>(x.Value.ToString(), new JsonSerializerSettings()
          {
            ObjectCreationHandling = ObjectCreationHandling.Replace
          }))).ToList<ExtraPriceRule.ItemPricing>();
          foreach (ExtraPriceRule.ItemPricing itemPricing1 in list3)
          {
            ExtraPriceRule.ItemPricing itemPricing = itemPricing1;
            itemPricing.Price = list2.Single<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => x.Uid == itemPricing.Price.Uid));
          }
          List<ExtraPriceRule> extraPriceRuleList = allItems2;
          ExtraPriceRule extraPriceRule = new ExtraPriceRule();
          extraPriceRule.Uid = item.EntityUid;
          extraPriceRule.Name = item.Value.ToString();
          extraPriceRule.Groups = source.ToList<GoodGroups.Group>();
          extraPriceRule.Items = list3;
          extraPriceRuleList.Add(extraPriceRule);
        }
        LogHelper.OnEnd();
        return allItems2;
      }
    }

    public ExtraPriceRule GetByUid(Guid uid)
    {
      return this.GetAllItems().FirstOrDefault<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => x.Uid == uid));
    }

    [Localizable(false)]
    public bool Save(ExtraPriceRule item)
    {
      LogHelper.OnBegin();
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.ExtraPricingRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting1);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.Groups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting2 = new Setting();
        setting2.Parameter = group.Uid.ToString();
        setting2.EntityUid = item.Uid;
        setting2.Value = (object) string.Empty;
        setting2.IsDeleted = item.IsDeleted;
        setting2.Type = Types.ExtraPricingRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting2));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.ExtraPricingRuleItem && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (ExtraPriceRule.ItemPricing itemPricing in item.Items)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = itemPricing.Price.Uid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) itemPricing.ToJsonString();
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.ExtraPricingRuleItem;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      LogHelper.OnEnd();
      return true;
    }

    public int Save(List<ExtraPriceRule> itemsList)
    {
      return itemsList.Count<ExtraPriceRule>(new Func<ExtraPriceRule, bool>(this.Save));
    }

    public ActionResult Validate(ExtraPriceRule item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.Groups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.Items.Any<ExtraPriceRule.ItemPricing>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_указать_хотя_бы_одно_подправило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
