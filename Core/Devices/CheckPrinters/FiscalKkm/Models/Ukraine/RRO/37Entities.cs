// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.ZPay
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class ZPay
  {
    public Decimal SUM { get; set; }

    public int ORDERSCNT { get; set; }

    public int TOTALCURRENCYCOST { get; set; }

    public Decimal TOTALCURRENCYCOMMISSION { get; set; }

    public ZPayFormsRow[] PAYFORMS { get; set; }

    public ZTaxRow[] TAXES { get; set; }
  }
}
