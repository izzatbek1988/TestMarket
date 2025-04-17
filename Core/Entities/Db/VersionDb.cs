// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.VersionDb
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class VersionDb
  {
    public static int GetVersion()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
          return int.Parse(dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -1))?.VAL ?? "1");
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return 0;
      }
    }

    public static void SetVersion(int version)
    {
      LogHelper.Debug(string.Format("Установка версии БД. Текущая версия: {0}; новая: {1}", (object) VersionDb.GetVersion(), (object) version));
      new SettingsRepository().Save(new Setting()
      {
        Parameter = "Version",
        Type = Types.VersionDb,
        Value = (object) version
      });
    }
  }
}
