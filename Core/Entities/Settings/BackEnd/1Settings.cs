// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Setting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Entities.Settings
{
  public class Setting : Entity
  {
    public Types Type { get; set; }

    public Guid EntityUid { get; set; } = Guid.Empty;

    public string Parameter { get; set; } = string.Empty;

    public object Value { get; set; }

    public Setting()
    {
    }

    public Setting(SETTINGS dbItem)
    {
      this.Uid = dbItem.UID;
      this.Type = (Types) dbItem.TYPE;
      this.EntityUid = dbItem.ENTITY_UID;
      this.Parameter = dbItem.PARAM;
      this.IsDeleted = dbItem.IS_DELETED;
      try
      {
        this.Value = JsonConvert.DeserializeObject(dbItem.VAL);
      }
      catch (Exception ex)
      {
        string message = "Не удалось дессеризировать строку " + dbItem.VAL;
        LogHelper.Error(ex, message, false);
      }
    }
  }
}
