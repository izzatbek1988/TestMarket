// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.GoodGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class GoodGroup : GoodGroupSimple, IEntity
  {
    public EntityTypes Type => EntityTypes.GoodGroup;

    public bool IsDeleted { get; set; }

    public GoodGroup(GoodGroups.Group group)
      : base(group)
    {
      this.IsDeleted = group.IsDeleted;
    }
  }
}
