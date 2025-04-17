// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodGroupHomeList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodGroupHomeList
  {
    public List<GoodGroupHome> Groups { get; set; }

    public GoodGroupHomeList()
    {
    }

    public GoodGroupHomeList(List<GoodGroups.Group> groups)
    {
      this.Groups = groups.Select<GoodGroups.Group, GoodGroupHome>((Func<GoodGroups.Group, GoodGroupHome>) (x => new GoodGroupHome(x))).ToList<GoodGroupHome>();
    }
  }
}
