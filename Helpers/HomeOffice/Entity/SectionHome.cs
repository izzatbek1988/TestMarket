// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.SectionHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class SectionHome
  {
    public Guid Uid { get; set; }

    public string Name { get; set; }

    public string GbsId { get; set; }

    public bool IsDeleted { get; set; }

    public SectionHome()
    {
    }

    public SectionHome(Sections.Section section)
    {
      this.Uid = section.Uid;
      this.GbsId = section.GbsId;
      this.Name = section.Name;
      this.IsDeleted = section.IsDeleted;
    }
  }
}
