// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.Goods.Good
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.Goods
{
  public class Good
  {
    public Guid Uid { get; set; }

    public int Id { get; set; }

    public string Name { get; set; }

    public GoodGroup Group { get; set; }

    public string Barcode { get; set; }

    public string Barcodes { get; set; }

    public string UnitName { get; set; }

    public Unit Unit { get; set; }

    public Decimal VatValue { get; set; }

    public string NdsName { get; set; }

    public Decimal NdsValue { get; set; }

    public Decimal TotalCount { get; set; }

    public string Description { get; set; }

    public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
  }
}
