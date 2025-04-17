// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.ZVal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class ZVal
  {
    public Decimal TOTALINADVANCE { get; set; }

    public Decimal TOTALINATTACH { get; set; }

    public Decimal TOTALSURRCOLLECTION { get; set; }

    public Decimal COMMISSION { get; set; }

    public int CALCDOCSCNT { get; set; }

    public Decimal ACCEPTEDN { get; set; }

    public Decimal ISSUEDN { get; set; }

    public Decimal COMMISSIONN { get; set; }

    public int TRANSFERSCNT { get; set; }

    public ZValDetailsRow[] DETAILS { get; set; }

    public ZTaxRow[] TAXES { get; set; }
  }
}
