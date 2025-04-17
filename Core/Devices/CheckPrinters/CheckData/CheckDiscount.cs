// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckData.CheckDiscount
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckData
{
  public class CheckDiscount
  {
    public CheckDiscount.Types Type { get; set; }

    public Decimal Sum { get; set; }

    public string Description { get; set; }

    public enum Types
    {
      Unknown,
      TotalsRound,
      Bonuses,
    }
  }
}
