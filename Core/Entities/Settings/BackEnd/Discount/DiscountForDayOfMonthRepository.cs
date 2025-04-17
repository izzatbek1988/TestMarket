// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForDayOfMonthRepository
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
  public class DiscountForDayOfMonthRepository : IRepository<DiscountForDayOfMonth>
  {
    public List<DiscountForDayOfMonth> GetDiscountForDayOfMonth()
    {
      DateTime day = DateTime.Now;
      return this.GetActiveItems().Where<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x => x.DateStart.Date <= day && x.DateFinish >= day.Date && x.Day == day.Day && !x.IsOff)).ToList<DiscountForDayOfMonth>();
    }

    public int Delete(List<DiscountForDayOfMonth> itemsList) => throw new NotImplementedException();

    public bool Delete(DiscountForDayOfMonth item) => throw new NotImplementedException();

    public List<DiscountForDayOfMonth> GetActiveItems()
    {
      return this.GetAllItems().Where<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x => !x.IsDeleted)).ToList<DiscountForDayOfMonth>();
    }

    public List<DiscountForDayOfMonth> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Setting> settingByQuery = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false)).Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 41 || x.TYPE == 40)));
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<DiscountForDayOfMonth> allItems2 = new List<DiscountForDayOfMonth>();
        foreach (Setting setting in settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForDayOfMonthRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          Decimal num1 = Convert.ToDecimal(settingByQuery.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "Discount")).Value);
          int num2 = JsonConvert.DeserializeObject<int>(settingByQuery.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "Day")).Value.ToString());
          object obj1 = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "DateStart"))?.Value;
          object obj2 = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "DateFinish"))?.Value;
          object obj3 = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "TimeStart"))?.Value;
          object obj4 = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "TimeFinish"))?.Value;
          object obj5 = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (w => w.Parameter == "IsOff" && w.EntityUid == item.EntityUid))?.Value;
          IEnumerable<string> groupsUids = settingByQuery.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForDayOfMonthRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          List<DiscountForDayOfMonth> discountForDayOfMonthList = allItems2;
          DiscountForDayOfMonth discountForDayOfMonth = new DiscountForDayOfMonth();
          discountForDayOfMonth.Uid = item.EntityUid;
          discountForDayOfMonth.Name = item.Value.ToString();
          discountForDayOfMonth.Discount = num1;
          discountForDayOfMonth.Day = num2;
          discountForDayOfMonth.DateStart = (DateTime) (obj1 ?? (object) DateTime.Now);
          discountForDayOfMonth.DateFinish = (DateTime) (obj2 ?? (object) DateTime.Now);
          discountForDayOfMonth.TimeStart = (DateTime) (obj3 ?? (object) DateTime.Now);
          discountForDayOfMonth.TimeFinish = (DateTime) (obj4 ?? (object) DateTime.Now);
          discountForDayOfMonth.ListGroup = source.ToList<GoodGroups.Group>();
          discountForDayOfMonth.IsOff = (bool) (obj5 ?? (object) false);
          discountForDayOfMonthList.Add(discountForDayOfMonth);
        }
        return allItems2;
      }
    }

    public DiscountForDayOfMonth GetByUid(Guid uid)
    {
      return this.GetActiveItems().FirstOrDefault<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x => x.Uid == uid));
    }

    public bool Save(DiscountForDayOfMonth item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.DiscountForDayOfMonthRules;
      setting1.Parameter = "Name";
      setting1.Value = (object) item.Name;
      setting1.IsDeleted = item.IsDeleted;
      Setting setting2 = setting1;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "Discount";
      setting2.Value = (object) item.Discount;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "DateStart";
      setting2.Value = (object) item.DateStart;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "DateFinish";
      setting2.Value = (object) item.DateFinish;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "TimeStart";
      setting2.Value = (object) item.TimeStart;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "TimeFinish";
      setting2.Value = (object) item.TimeFinish;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "IsOff";
      setting2.Value = (object) item.IsOff;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "Day";
      setting2.Value = (object) item.Day;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForDayOfMonthRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.ListGroup)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Parameter = group.Uid.ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) string.Empty;
        setting3.IsDeleted = item.IsDeleted;
        setting3.Type = Types.DiscountForDayOfMonthRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      return true;
    }

    public int Save(List<DiscountForDayOfMonth> itemsList)
    {
      return itemsList.Count<DiscountForDayOfMonth>(new Func<DiscountForDayOfMonth, bool>(this.Save));
    }

    public ActionResult Validate(DiscountForDayOfMonth item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.ListGroup.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (item.Day < 1 || item.Day > 31)
        stringList.Add(Translate.DiscountForDayOfMonthRepository_Требуется_указать_день__когда_будем_действовать_правило);
      TimeSpan timeOfDay1 = item.TimeStart.TimeOfDay;
      DateTime dateTime = item.TimeFinish;
      TimeSpan timeOfDay2 = dateTime.TimeOfDay;
      if (timeOfDay1 > timeOfDay2)
        stringList.Add(Translate.DiscountForDayOfMonthRepository_Время_начала_правила_должно_быть_меньше_времени_ее_окончания);
      dateTime = item.DateStart;
      if (dateTime.Date > item.DateFinish)
        stringList.Add(Translate.DiscountForDayOfMonthRepository_Дата_начала_правила_должна_быть_меньше_даты_ее_окончания);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
