// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.GoodSimple
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class GoodSimple
  {
    public Guid Uid { get; set; }

    public string Barcode { get; set; }

    public string Name { get; set; }

    public GoodGroupSimple Group { get; set; }

    public GoodSimple(Gbs.Core.Entities.Goods.Good good)
    {
      this.Uid = good.Uid;
      this.Barcode = good.Barcode;
      this.Name = good.Name;
      this.Group = new GoodGroupSimple(good.Group);
    }
  }
}
