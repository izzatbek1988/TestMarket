// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Discount.DiscountForWeekdayRepository
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
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Settings.Discount
{
  public class DiscountForWeekdayRepository : IRepository<DiscountForWeekday>
  {
    public List<DiscountForWeekday> GetDiscountForWeekday()
    {
      DateTime day = DateTime.Now;
      List<DiscountForWeekday> list = this.GetActiveItems().Where<DiscountForWeekday>((Func<DiscountForWeekday, bool>) (x => x.DateStart.Date <= day && x.DateFinish >= day.Date && x.WeekdaysList.Single<WeekDayItem>((Func<WeekDayItem, bool>) (w => w.Weekday == day.DayOfWeek)).IsChecked && !x.IsOff)).ToList<DiscountForWeekday>();
      return list.Any<DiscountForWeekday>() ? list : new List<DiscountForWeekday>();
    }

    public int Delete(List<DiscountForWeekday> itemsList) => throw new NotImplementedException();

    public bool Delete(DiscountForWeekday item) => throw new NotImplementedException();

    public List<DiscountForWeekday> GetActiveItems()
    {
      return this.GetAllItems().Where<DiscountForWeekday>((Func<DiscountForWeekday, bool>) (x => !x.IsDeleted)).ToList<DiscountForWeekday>();
    }

    public List<DiscountForWeekday> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Setting> database = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false)).Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 51 || x.TYPE == 50)));
        List<GoodGroups.Group> allItems1 = new GoodGroupsRepository(dataBase).GetAllItems();
        List<DiscountForWeekday> allItems2 = new List<DiscountForWeekday>();
        foreach (Setting setting in database.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForWeekdaysRules && x.Parameter == "Name")))
        {
          Setting item = setting;
          Decimal num = Convert.ToDecimal(database.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "Discount")).Value);
          object obj1 = database.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "DateStart")).Value;
          object obj2 = database.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "DateFinish")).Value;
          object obj3 = database.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "TimeStart")).Value;
          object obj4 = database.Single<Setting>((Func<Setting, bool>) (d => d.EntityUid == item.EntityUid && d.Parameter == "TimeFinish")).Value;
          IEnumerable<string> groupsUids = database.Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForWeekdaysRuleGroups && x.EntityUid == item.EntityUid)).Select<Setting, string>((Func<Setting, string>) (x => x.Parameter));
          IEnumerable<GoodGroups.Group> source = allItems1.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => groupsUids.Any<string>((Func<string, bool>) (x => x == g.Uid.ToString()))));
          List<WeekDayItem> weekdays = DiscountForWeekday.GetWeekdays();
          weekdays.ForEach((Action<WeekDayItem>) (x => x.IsChecked = (bool) database.Single<Setting>((Func<Setting, bool>) (w => w.Parameter == ((int) x.Weekday).ToString() && w.EntityUid == item.EntityUid)).Value));
          object obj5 = database.Single<Setting>((Func<Setting, bool>) (w => w.Parameter == "IsOff" && w.EntityUid == item.EntityUid)).Value;
          List<DiscountForWeekday> discountForWeekdayList = allItems2;
          DiscountForWeekday discountForWeekday = new DiscountForWeekday();
          discountForWeekday.Uid = item.EntityUid;
          discountForWeekday.Name = item.Value.ToString();
          discountForWeekday.Discount = num;
          discountForWeekday.DateStart = (DateTime) obj1;
          discountForWeekday.DateFinish = (DateTime) obj2;
          discountForWeekday.TimeStart = (DateTime) obj3;
          discountForWeekday.TimeFinish = (DateTime) obj4;
          discountForWeekday.ListGroup = source.ToList<GoodGroups.Group>();
          discountForWeekday.WeekdaysList = weekdays;
          discountForWeekday.IsOff = (bool) obj5;
          discountForWeekdayList.Add(discountForWeekday);
        }
        return allItems2;
      }
    }

    public DiscountForWeekday GetByUid(Guid uid)
    {
      return this.GetActiveItems().FirstOrDefault<DiscountForWeekday>((Func<DiscountForWeekday, bool>) (x => x.Uid == uid));
    }

    public bool Save(DiscountForWeekday item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Type = Types.DiscountForWeekdaysRules;
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
      foreach (WeekDayItem weekdays in item.WeekdaysList)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting3 = new Setting();
        setting3.Type = Types.DiscountForWeekdaysRules;
        setting3.Parameter = ((int) weekdays.Weekday).ToString();
        setting3.EntityUid = item.Uid;
        setting3.Value = (object) weekdays.IsChecked;
        setting3.IsDeleted = item.IsDeleted;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting3));
      }
      new SettingsRepository().Delete(new SettingsRepository().GetActiveItems().Where<Setting>((Func<Setting, bool>) (x => x.Type == Types.DiscountForWeekdaysRuleGroups && x.EntityUid == item.Uid)).ToList<Setting>());
      foreach (GoodGroups.Group group in item.ListGroup)
      {
        SettingsRepository settingsRepository = new SettingsRepository();
        Setting setting4 = new Setting();
        setting4.Parameter = group.Uid.ToString();
        setting4.EntityUid = item.Uid;
        setting4.Value = (object) string.Empty;
        setting4.IsDeleted = item.IsDeleted;
        setting4.Type = Types.DiscountForWeekdaysRuleGroups;
        // ISSUE: explicit non-virtual call
        __nonvirtual (settingsRepository.Save(setting4));
      }
      return true;
    }

    public int Save(List<DiscountForWeekday> itemsList)
    {
      return itemsList.Count<DiscountForWeekday>(new Func<DiscountForWeekday, bool>(this.Save));
    }

    public ActionResult Validate(DiscountForWeekday item)
    {
      List<string> stringList = new List<string>();
      if (item.Name.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (!item.ListGroup.Any<GoodGroups.Group>())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_выбрать_категории__для_которых_будет_действовать_правило);
      if (!item.WeekdaysList.Any<WeekDayItem>((Func<WeekDayItem, bool>) (x => x.IsChecked)))
        stringList.Add(Translate.DiscountForWeekdayRepository_Требуется_выбрать_хотя_бы_один_день_недели__когда_будет_действовать_это_правило_);
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
