// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForCountGoodRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd.Discount
{
  public class DiscountForCountGoodRepository : IRepository<DiscountForCountGood>
  {
    public DiscountForCountGood GetActiveItemsByGroup(Guid grUid)
    {
      return this.GetActiveItems().FirstOrDefault<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == grUid))));
    }

    public List<DiscountForCountGood> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Setting> settingByQuery = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false)).Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 140 || x.TYPE == 141 || x.TYPE == 142)));
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<DiscountForCountGood> allItems2 = new List<DiscountForCountGood>();
        foreach (Setting setting in settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForCountGoodRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          IEnumerable<string> groupsUids = settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForCountGoodRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source1 = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          IEnumerable<DiscountForCountGood.Item> source2 = settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForCountGoodRuleItem && x.EntityUid == item.EntityUid)).Select<Setting, DiscountForCountGood.Item>((Func<Setting, DiscountForCountGood.Item>) (x => JsonConvert.DeserializeObject<DiscountForCountGood.Item>(x.Value.ToString())));
          List<DiscountForCountGood> discountForCountGoodList = allItems2;
          DiscountForCountGood discountForCountGood = new DiscountForCountGood();
          discountForCountGood.Uid = item.EntityUid;
          discountForCountGood.Name = item.Value.ToString();
          discountForCountGood.Groups = source1.ToList<GoodGroups.Group>();
          discountForCountGood.Items = source2.ToList<DiscountForCountGood.Item>();
          discountForCountGoodList.Add(discountForCountGood);
        }
        return allItems2;
      }
    }

    public List<DiscountForCountGood> GetActiveItems()
    {
      return this.GetAllItems().Where<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (x => !x.IsDeleted)).ToList<DiscountForCountGood>();
    }

    public DiscountForCountGood GetByUid(Guid uid)
    {
      return this.GetAllItems().FirstOrDefault<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (x => x.Uid == uid));
    }

    public bool Save(DiscountForCountGood item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.DiscountForCountGoodRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting1);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForCountGoodRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.Groups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting2 = new Setting();
        setting2.Parameter = group.Uid.ToString();
        setting2.EntityUid = item.Uid;
        setting2.Value = (object) string.Empty;
        setting2.IsDeleted = item.IsDeleted;
        setting2.Type = Types.DiscountForCountGoodRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting2));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForCountGoodRuleItem && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (DiscountForCountGood.Item obj in item.Items)
      {
        Guid guid = Guid.NewGuid();
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = guid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) obj.ToJsonString();
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.DiscountForCountGoodRuleItem;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<DiscountForCountGood> itemsList)
    {
      return itemsList.Count<DiscountForCountGood>(new Func<DiscountForCountGood, bool>(this.Save));
    }

    public int Delete(List<DiscountForCountGood> itemsList) => throw new NotImplementedException();

    public bool Delete(DiscountForCountGood item) => throw new NotImplementedException();

    public ActionResult Validate(DiscountForCountGood item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.Groups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.Items.Any<DiscountForCountGood.Item>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_указать_хотя_бы_одно_подправило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
