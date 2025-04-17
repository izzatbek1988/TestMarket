// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Db.UidDb
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Settings;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Db
{
  public static class UidDb
  {
    public static Setting GetUid()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        SETTINGS dbItem = dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -3));
        Setting uid;
        if (dbItem != null)
        {
          uid = new Setting(dbItem);
        }
        else
        {
          uid = new Setting();
          uid.EntityUid = Guid.Empty;
          uid.Type = Types.UidDb;
          uid.Parameter = nameof (UidDb);
        }
        return uid;
      }
    }

    public static bool SetUid(Guid uid, string name)
    {
      return new SettingsRepository().Save(new Setting()
      {
        Parameter = nameof (UidDb),
        Type = Types.UidDb,
        EntityUid = uid,
        Value = (object) name
      });
    }

    public static bool SetUid(Setting s) => new SettingsRepository().Save(s);
  }
}
