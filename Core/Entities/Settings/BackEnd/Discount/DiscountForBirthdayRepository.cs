// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForBirthdayRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd.Discount
{
  public class DiscountForBirthdayRepository : IRepository<DiscountForBirthday>
  {
    public int Delete(List<DiscountForBirthday> itemsList) => throw new NotImplementedException();

    public bool Delete(DiscountForBirthday item) => throw new NotImplementedException();

    public List<DiscountForBirthday> GetActiveItems() => throw new NotImplementedException();

    public List<DiscountForBirthday> GetAllItems()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Setting> settingByQuery = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false && x.TYPE == 120)));
          return new List<DiscountForBirthday>()
          {
            new DiscountForBirthday()
            {
              CountDay = Convert.ToInt32(settingByQuery.SingleOrDefault<Setting>((Func<Setting, bool>) (x => x.Parameter == "CountDay"))?.Value ?? (object) 0),
              Discount = Convert.ToDecimal(settingByQuery.SingleOrDefault<Setting>((Func<Setting, bool>) (x => x.Parameter == "Discount"))?.Value ?? (object) 0),
              IsActive = Convert.ToBoolean(settingByQuery.SingleOrDefault<Setting>((Func<Setting, bool>) (x => x.Parameter == "IsActive"))?.Value ?? (object) false)
            }
          };
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения скидки на день рождения", false);
        return new List<DiscountForBirthday>()
        {
          new DiscountForBirthday()
          {
            CountDay = 0,
            Discount = 0M,
            IsActive = false
          }
        };
      }
    }

    public DiscountForBirthday GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(DiscountForBirthday item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = Guid.Empty;
      setting1.Type = Types.DiscountForBirthdaySetting;
      setting1.Parameter = "CountDay";
      setting1.Value = (object) item.CountDay;
      setting1.IsDeleted = item.IsDeleted;
      Setting setting2 = setting1;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "Discount";
      setting2.Value = (object) item.Discount;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "IsActive";
      setting2.Value = (object) item.IsActive;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      return true;
    }

    public int Save(List<DiscountForBirthday> itemsList)
    {
      return itemsList.Count<DiscountForBirthday>(new Func<DiscountForBirthday, bool>(this.Save));
    }

    public ActionResult Validate(DiscountForBirthday item)
    {
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
