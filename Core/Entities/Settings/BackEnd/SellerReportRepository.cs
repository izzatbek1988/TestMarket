// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.SellerReportRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Settings
{
  public class SellerReportRepository
  {
    public SellerReportSetting GetSetting()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        SETTINGS settings = dataBase.GetTable<SETTINGS>().FirstOrDefault<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 110));
        return settings == null ? new SellerReportSetting() : JsonConvert.DeserializeObject<SellerReportSetting>(JsonConvert.DeserializeObject(settings.VAL).ToString());
      }
    }

    public bool Save(SellerReportSetting item)
    {
      return new SettingsRepository().Save(new Setting()
      {
        Parameter = "SellerReportSetting",
        Value = (object) item.ToJsonString(),
        Type = Types.SellerReportSetting
      });
    }
  }
}
