// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodHomeList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodHomeList
  {
    public List<GoodHome> GoodsList { get; set; }

    public GoodHomeList(List<Gbs.Core.Entities.Goods.Good> goods)
    {
      this.GoodsList = goods.Select<Gbs.Core.Entities.Goods.Good, GoodHome>((Func<Gbs.Core.Entities.Goods.Good, GoodHome>) (x => new GoodHome(x))).ToList<GoodHome>();
    }

    public GoodHomeList()
    {
    }
  }
}
