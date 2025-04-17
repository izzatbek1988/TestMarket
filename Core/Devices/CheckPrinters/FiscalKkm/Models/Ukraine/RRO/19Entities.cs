// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.CTotal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class CTotal
  {
    public Decimal SUM { get; set; }

    public Decimal RNDSUM { get; set; }

    public Decimal NORNDSUM { get; set; }

    public Decimal NOTAXSUM { get; set; }

    public Decimal COMMISSION { get; set; }

    public int? USAGETYPE { get; set; }

    public Decimal SUBCHECK { get; set; }

    public int? DISCOUNTTYPE { get; set; }

    public Decimal DISCOUNTPERCENT { get; set; }

    public Decimal DISCOUNTSUM { get; set; }
  }
}
