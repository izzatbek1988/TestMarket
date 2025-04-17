// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Discount.ClientBonusesRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Settings.Discount
{
  public class ClientBonusesRepository : IRepository<ClientBonuses>
  {
    public int Delete(List<ClientBonuses> itemsList) => throw new NotImplementedException();

    public bool Delete(ClientBonuses item) => throw new NotImplementedException();

    public List<ClientBonuses> GetActiveItems()
    {
      return this.GetAllItems().Where<ClientBonuses>((Func<ClientBonuses, bool>) (x => !x.IsDeleted)).ToList<ClientBonuses>();
    }

    public List<ClientBonuses> GetAllItems()
    {
      List<Setting> list = new SettingsRepository().GetActiveItems().ToList<Setting>().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.BonusRuleGroups || x.Type == Types.BonusRules)).ToList<Setting>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<ClientBonuses> allItems2 = new List<ClientBonuses>();
        foreach (Setting setting in list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.BonusRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          Decimal num = Convert.ToDecimal(list.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "Percent")).Value);
          IEnumerable<string> groupsUids = list.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.BonusRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          List<ClientBonuses> clientBonusesList = allItems2;
          ClientBonuses clientBonuses = new ClientBonuses();
          clientBonuses.Uid = item.EntityUid;
          clientBonuses.Name = item.Value.ToString();
          clientBonuses.ListGroups = source.ToList<GoodGroups.Group>();
          clientBonuses.Percent = num;
          clientBonuses.IsDeleted = item.IsDeleted;
          clientBonusesList.Add(clientBonuses);
        }
        return allItems2;
      }
    }

    public ClientBonuses GetByUid(Guid uid)
    {
      return this.GetActiveItems().FirstOrDefault<ClientBonuses>((Func<ClientBonuses, bool>) (x => x.Uid == uid));
    }

    public bool Save(ClientBonuses item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        MessageBoxHelper.Warning(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.BonusRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      Setting setting2 = setting1;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "Percent";
      setting2.Value = (object) item.Percent;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.BonusRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group listGroup in item.ListGroups)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = listGroup.Uid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) string.Empty;
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.BonusRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<ClientBonuses> itemsList)
    {
      return itemsList.Count<ClientBonuses>(new Func<ClientBonuses, bool>(this.Save));
    }

    public ActionResult Validate(ClientBonuses item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.ListGroups.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
