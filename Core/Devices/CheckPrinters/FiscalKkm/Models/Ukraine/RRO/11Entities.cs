// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.CheckContent
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class CheckContent
  {
    public CHead CHECKHEAD { get; set; } = new CHead();

    public CTotal CHECKTOTAL { get; set; } = new CTotal();

    public CPayRow[] CHECKPAY { get; set; }

    public CTaxRow[] CHECKTAX { get; set; }

    public CPtks CHECKPTKS { get; set; } = new CPtks();

    public CBodyRow[] CHECKBODY { get; set; }
  }
}
