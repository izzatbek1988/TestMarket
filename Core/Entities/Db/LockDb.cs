// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Db.LockDb
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Db
{
  public static class LockDb
  {
    private static bool CheckLockDb()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        SETTINGS settings = dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -2));
        if (settings == null || settings.ENTITY_UID == Guid.Empty && settings.VAL.IsNullOrEmpty())
          return false;
        if (!(settings.ENTITY_UID == new Guid(GbsIdHelperMain.GetGbsId())))
          return (DateTime.Now.Date - (DateTime) JsonConvert.DeserializeObject(settings.VAL)).Hours <= 24;
        dataBase.GetTable<SETTINGS>().Delete<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -2));
        return false;
      }
    }

    public static bool LockedDb()
    {
      bool flag = LockDb.CheckLockDb();
      LogHelper.Debug("Статус блокировки БД для корректировки: bloced - " + flag.ToString());
      if (flag)
        return false;
      SettingsRepository settingsRepository = new SettingsRepository();
      Setting setting = new Setting();
      setting.Parameter = nameof (LockDb);
      setting.Type = Gbs.Core.Entities.Settings.Types.LockDb;
      setting.EntityUid = new Guid(GbsIdHelperMain.GetGbsId());
      setting.Value = (object) DateTime.Now.Date;
      setting.IsDeleted = false;
      // ISSUE: explicit non-virtual call
      return __nonvirtual (settingsRepository.Save(setting));
    }

    public static bool UnLockedDb()
    {
      using (DataBase dataBase = Data.GetDataBase())
        dataBase.GetTable<SETTINGS>().Delete<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -2));
      return true;
    }
  }
}
