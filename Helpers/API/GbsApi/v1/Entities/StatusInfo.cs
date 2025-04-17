// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.StatusInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class StatusInfo : IEntity
  {
    public EntityTypes Type => EntityTypes.StatusInfo;

    [JsonIgnore]
    public Guid Uid { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }

    public string GbsId => LicenseHelper.GetInfo().GbsId;

    public DateTime LicenseExpiration => LicenseHelper.GetInfo().KeyDateEnd;

    public string AppVersion => ApplicationInfo.GetInstance().GbsVersion.ToString();
  }
}
