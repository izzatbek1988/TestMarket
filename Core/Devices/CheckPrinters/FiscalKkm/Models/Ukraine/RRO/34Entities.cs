// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.ZTaxRow
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class ZTaxRow
  {
    public int TYPE { get; set; }

    public string NAME { get; set; }

    public string LETTER { get; set; }

    public Decimal PRC { get; set; }

    public bool SIGN { get; set; }

    public Decimal TURNOVER { get; set; }

    public Decimal SOURCESUM { get; set; }

    public Decimal SUM { get; set; }
  }
}
