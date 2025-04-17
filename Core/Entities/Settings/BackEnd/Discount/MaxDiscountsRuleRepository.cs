// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.MaxDiscountsRuleRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd.Discount
{
  public class MaxDiscountsRuleRepository : IRepository<MaxDiscountRule>
  {
    public List<MaxDiscountRule> GetAllItems()
    {
      try
      {
        List<Setting> list = new SettingsRepository().GetActiveItems().ToList<Setting>().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.MaxDiscountRules || x.Type == Types.MaxDiscountRuleGroups)).ToList<Setting>();
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
          List<MaxDiscountRule> allItems2 = new List<MaxDiscountRule>();
          foreach (Setting setting in list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.MaxDiscountRules && x.Parameter == "Name")))
          {
            Setting item = setting;
            Decimal num = Convert.ToDecimal(list.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "MaxDiscount")).Value);
            IEnumerable<string> groupsUids = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.MaxDiscountRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
            IEnumerable<GoodGroups.Group> source = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
            object obj = list.Single<Setting>((Func<Setting, bool>) (w => w.Parameter == "IsOff" && w.EntityUid == item.EntityUid)).Value;
            List<MaxDiscountRule> maxDiscountRuleList = allItems2;
            MaxDiscountRule maxDiscountRule = new MaxDiscountRule();
            maxDiscountRule.Uid = item.EntityUid;
            maxDiscountRule.Name = item.Value.ToString();
            maxDiscountRule.MaxDiscount = num;
            maxDiscountRule.ListGroup = source.ToList<GoodGroups.Group>();
            maxDiscountRule.IsOff = (bool) obj;
            maxDiscountRuleList.Add(maxDiscountRule);
          }
          return allItems2;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения правил максимальных скидок");
        return new List<MaxDiscountRule>();
      }
    }

    public List<MaxDiscountRule> GetActiveItems()
    {
      return this.GetAllItems().Where<MaxDiscountRule>((Func<MaxDiscountRule, bool>) (x => !x.IsDeleted)).ToList<MaxDiscountRule>();
    }

    public MaxDiscountRule GetByUid(Guid uid)
    {
      return this.GetActiveItems().FirstOrDefault<MaxDiscountRule>((Func<MaxDiscountRule, bool>) (x => x.Uid == uid));
    }

    public bool Save(MaxDiscountRule item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.MaxDiscountRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      Setting setting2 = setting1;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "MaxDiscount";
      setting2.Value = (object) item.MaxDiscount;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "IsOff";
      setting2.Value = (object) item.IsOff;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.MaxDiscountRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.ListGroup)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = group.Uid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) string.Empty;
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.MaxDiscountRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<MaxDiscountRule> itemsList)
    {
      return itemsList.Count<MaxDiscountRule>(new Func<MaxDiscountRule, bool>(this.Save));
    }

    public int Delete(List<MaxDiscountRule> itemsList) => throw new NotImplementedException();

    public bool Delete(MaxDiscountRule item) => throw new NotImplementedException();

    public ActionResult Validate(MaxDiscountRule item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.ListGroup.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
