// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.Property
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class Property
  {
    public Guid TypeUid { get; set; }

    public string TypeName { get; set; }

    public string Value { get; set; }

    public Property(EntityProperties.PropertyValue prop)
    {
      this.TypeUid = prop.Type.Uid;
      this.TypeName = prop.Type.Name;
      this.Value = prop.Value.ToJsonString();
    }
  }
}
