// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheckRepository
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
  public class DiscountFromSumInCheckRepository : IRepository<DiscountFromSumInCheck>
  {
    public DiscountFromSumInCheck GetActiveItemsByGroup(Guid grUid)
    {
      return this.GetActiveItems().FirstOrDefault<DiscountFromSumInCheck>((Func<DiscountFromSumInCheck, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == grUid))));
    }

    public List<DiscountFromSumInCheck> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Setting> settingByQuery = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false)).Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 80 || x.TYPE == 81 || x.TYPE == 82)));
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<DiscountFromSumInCheck> allItems2 = new List<DiscountFromSumInCheck>();
        foreach (Setting setting in settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountFromSumCheckRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          IEnumerable<string> groupsUids = settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountFromSumCheckRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source1 = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          IEnumerable<DiscountFromSumInCheck.Item> source2 = settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountFromSumCheckRuleItem && x.EntityUid == item.EntityUid)).Select<Setting, DiscountFromSumInCheck.Item>((Func<Setting, DiscountFromSumInCheck.Item>) (x => JsonConvert.DeserializeObject<DiscountFromSumInCheck.Item>(x.Value.ToString())));
          List<DiscountFromSumInCheck> discountFromSumInCheckList = allItems2;
          DiscountFromSumInCheck discountFromSumInCheck = new DiscountFromSumInCheck();
          discountFromSumInCheck.Uid = item.EntityUid;
          discountFromSumInCheck.Name = item.Value.ToString();
          discountFromSumInCheck.Groups = source1.ToList<GoodGroups.Group>();
          discountFromSumInCheck.Items = source2.ToList<DiscountFromSumInCheck.Item>();
          discountFromSumInCheckList.Add(discountFromSumInCheck);
        }
        return allItems2;
      }
    }

    public List<DiscountFromSumInCheck> GetActiveItems()
    {
      return this.GetAllItems().Where<DiscountFromSumInCheck>((Func<DiscountFromSumInCheck, bool>) (x => !x.IsDeleted)).ToList<DiscountFromSumInCheck>();
    }

    public DiscountFromSumInCheck GetByUid(Guid uid)
    {
      return this.GetAllItems().FirstOrDefault<DiscountFromSumInCheck>((Func<DiscountFromSumInCheck, bool>) (x => x.Uid == uid));
    }

    public bool Save(DiscountFromSumInCheck item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.DiscountFromSumCheckRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting1);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountFromSumCheckRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.Groups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting2 = new Setting();
        setting2.Parameter = group.Uid.ToString();
        setting2.EntityUid = item.Uid;
        setting2.Value = (object) string.Empty;
        setting2.IsDeleted = item.IsDeleted;
        setting2.Type = Types.DiscountFromSumCheckRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting2));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountFromSumCheckRuleItem && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (DiscountFromSumInCheck.Item obj in item.Items)
      {
        Guid guid = Guid.NewGuid();
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = guid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) obj.ToJsonString();
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.DiscountFromSumCheckRuleItem;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<DiscountFromSumInCheck> itemsList)
    {
      return itemsList.Count<DiscountFromSumInCheck>(new Func<DiscountFromSumInCheck, bool>(this.Save));
    }

    public int Delete(List<DiscountFromSumInCheck> itemsList)
    {
      throw new NotImplementedException();
    }

    public bool Delete(DiscountFromSumInCheck item) => throw new NotImplementedException();

    public ActionResult Validate(DiscountFromSumInCheck item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.Groups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.Items.Any<DiscountFromSumInCheck.Item>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_указать_хотя_бы_одно_подправило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
