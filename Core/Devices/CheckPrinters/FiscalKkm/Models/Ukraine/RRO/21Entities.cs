// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.TaxObjectsItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class TaxObjectsItem
  {
    public string Address { get; set; }

    public long Entity { get; set; }

    public string Name { get; set; }

    public string OrgName { get; set; }

    public string Tin { get; set; }

    public string Ipn { get; set; }

    public List<TransactionsRegistrarsItem> TransactionsRegistrars { get; set; }
  }
}
