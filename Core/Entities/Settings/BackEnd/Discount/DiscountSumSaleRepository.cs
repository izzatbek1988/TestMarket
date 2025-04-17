// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Discount.DiscountSumSaleRepository
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

#nullable disable
namespace Gbs.Core.Entities.Settings.Discount
{
  public class DiscountSumSaleRepository : IRepository<DiscountSumSale>
  {
    public DiscountSumSale GetActiveItemsByGroup(Guid grUid)
    {
      return this.GetActiveItems().FirstOrDefault<DiscountSumSale>((Func<DiscountSumSale, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == grUid))));
    }

    public int Delete(List<DiscountSumSale> itemsList) => throw new NotImplementedException();

    public bool Delete(DiscountSumSale item) => throw new NotImplementedException();

    public List<DiscountSumSale> GetActiveItems()
    {
      return this.GetAllItems().Where<DiscountSumSale>((Func<DiscountSumSale, bool>) (x => !x.IsDeleted)).ToList<DiscountSumSale>();
    }

    public List<DiscountSumSale> GetAllItems()
    {
      List<Setting> list = new SettingsRepository().GetActiveItems().ToList<Setting>().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRules || x.Type == Types.DiscountSumSaleRuleGroups || x.Type == Types.DiscountSumSaleRuleItems)).ToList<Setting>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<DiscountSumSale> allItems2 = new List<DiscountSumSale>();
        foreach (Setting setting in list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          IEnumerable<string> groupsUids = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source1 = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          IEnumerable<DiscountSumSale.Item> source2 = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRuleItems && x.EntityUid == item.EntityUid)).Select<Setting, DiscountSumSale.Item>((Func<Setting, DiscountSumSale.Item>) (x => JsonConvert.DeserializeObject<DiscountSumSale.Item>(x.Value.ToString())));
          List<DiscountSumSale> discountSumSaleList = allItems2;
          DiscountSumSale discountSumSale = new DiscountSumSale();
          discountSumSale.Uid = item.EntityUid;
          discountSumSale.Name = item.Value.ToString();
          discountSumSale.Groups = source1.ToList<GoodGroups.Group>();
          discountSumSale.Items = source2.ToList<DiscountSumSale.Item>();
          discountSumSaleList.Add(discountSumSale);
        }
        return allItems2;
      }
    }

    public DiscountSumSale GetByUid(Guid uid)
    {
      return this.GetAllItems().FirstOrDefault<DiscountSumSale>((Func<DiscountSumSale, bool>) (x => x.Uid == uid));
    }

    public bool Save(DiscountSumSale item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.DiscountSumSaleRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting1);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.Groups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting2 = new Setting();
        setting2.Parameter = group.Uid.ToString();
        setting2.EntityUid = item.Uid;
        setting2.Value = (object) string.Empty;
        setting2.IsDeleted = item.IsDeleted;
        setting2.Type = Types.DiscountSumSaleRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting2));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountSumSaleRuleItems && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (DiscountSumSale.Item obj in item.Items)
      {
        Guid guid = Guid.NewGuid();
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = guid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) obj.ToJsonString();
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.DiscountSumSaleRuleItems;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<DiscountSumSale> itemsList)
    {
      return itemsList.Count<DiscountSumSale>(new Func<DiscountSumSale, bool>(this.Save));
    }

    public ActionResult Validate(DiscountSumSale item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.Groups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.Items.Any<DiscountSumSale.Item>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_указать_хотя_бы_одно_подправило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
