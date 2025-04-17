// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Db.AuthRequireDb
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
  public class AuthRequireDb
  {
    public static bool Get()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          string val = dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == -4))?.VAL;
          bool flag;
          if (val == null)
          {
            flag = true;
            val = flag.ToString();
          }
          flag = bool.Parse(val);
          return flag;
        }
      }
      catch
      {
        return true;
      }
    }

    public static bool Set(bool value)
    {
      return new SettingsRepository().Save(new Setting()
      {
        Parameter = "Parameter",
        Type = Types.AuthRequireDb,
        Value = (object) value
      });
    }
  }
}
