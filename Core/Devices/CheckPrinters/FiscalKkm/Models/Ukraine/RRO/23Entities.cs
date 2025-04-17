// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.TransactionsRegistrarStateAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class TransactionsRegistrarStateAnswer : Answer
  {
    public int ShiftState { get; set; }

    public int ShiftId { get; set; }

    public string OpenShiftFiscalNum { get; set; }

    public bool ZRepPresent { get; set; }

    public string Name { get; set; }

    public string SubjectKeyId { get; set; }

    public int FirstLocalNum { get; set; }

    public int NextLocalNum { get; set; }

    public int? LastFiscalNum { get; set; }

    public bool OfflineSupported { get; set; }

    public bool ChiefCashier { get; set; }

    public string OfflineSessionId { get; set; }

    public string OfflineSeed { get; set; }

    public string OfflineNextLocalNum { get; set; }

    public string OfflineSessionDuration { get; set; }

    public string OfflineSessionsMonthlyDuration { get; set; }

    public bool Closed { get; set; }

    public bool OfflineDocumentsPresent { get; set; }
  }
}
