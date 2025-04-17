// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.SettingsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Settings
{
  public class SettingsRepository : IRepository<Setting>
  {
    public Setting GetSettingByType(Types type)
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          SETTINGS dbItem = dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == (int) type));
          return dbItem == null ? (Setting) null : new Setting(dbItem);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения настройки по типу из БД");
        return (Setting) null;
      }
    }

    public List<Setting> GetSettingListByType(Types type, bool includeDeleted = true)
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          IQueryable<SETTINGS> source = dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == (int) type));
          if (!includeDeleted)
            source = source.Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false));
          return source.ToList<SETTINGS>().Select<SETTINGS, Setting>((Func<SETTINGS, Setting>) (x => new Setting(x))).ToList<Setting>();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения настройки по типу из БД");
        return (List<Setting>) null;
      }
    }

    public List<Setting> GetSettingByQuery(IQueryable<SETTINGS> query)
    {
      try
      {
        using (Data.GetDataBase())
          return query.Select<SETTINGS, Setting>((Expression<Func<SETTINGS, Setting>>) (x => new Setting(x))).ToList<Setting>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения настройки по типу из БД");
        return (List<Setting>) null;
      }
    }

    public List<Setting> GetAllItems()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
          return dataBase.GetTable<SETTINGS>().Select<SETTINGS, Setting>((Expression<Func<SETTINGS, Setting>>) (x => new Setting(x))).ToList<Setting>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения списка настроек из БД");
        return new List<Setting>();
      }
    }

    public List<Setting> GetActiveItems()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
          return dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.IS_DELETED == false)).Select<SETTINGS, Setting>((Expression<Func<SETTINGS, Setting>>) (x => new Setting(x))).ToList<Setting>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения списка активных настроек из БД");
        return new List<Setting>();
      }
    }

    public Setting GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(Setting item)
    {
      try
      {
        if (this.Validate(item).Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Guid> list = dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == (int) item.Type && x.PARAM == item.Parameter && x.ENTITY_UID == item.EntityUid)).Select<SETTINGS, Guid>((Expression<Func<SETTINGS, Guid>>) (x => x.UID)).ToList<Guid>();
          SETTINGS settings = new SETTINGS()
          {
            UID = list.Count == 1 ? list.Single<Guid>() : item.Uid,
            TYPE = (int) item.Type,
            ENTITY_UID = item.EntityUid,
            PARAM = item.Parameter,
            VAL = JsonConvert.ToString(item.Value),
            IS_DELETED = item.IsDeleted
          };
          dataBase.InsertOrReplace<SETTINGS>(settings);
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка сохранения настроек в БД");
        return false;
      }
    }

    public int Save(List<Setting> itemsList)
    {
      return itemsList.Count<Setting>(new Func<Setting, bool>(this.Save));
    }

    public int Delete(List<Setting> itemsList)
    {
      foreach (Entity items in itemsList)
        items.IsDeleted = true;
      return this.Save(itemsList);
    }

    public bool Delete(Setting item)
    {
      item.IsDeleted = true;
      return this.Save(item);
    }

    public ActionResult Validate(Setting item) => ValidationHelper.DataValidation((Entity) item);
  }
}
