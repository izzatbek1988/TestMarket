// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.CheckDocumentType
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public enum CheckDocumentType
  {
    SaleGoods = 0,
    TransferFunds = 1,
    CurrencyExchange = 2,
    CashWithdrawal = 3,
    OpenShift = 100, // 0x00000064
    CloseShift = 101, // 0x00000065
    OfflineBegin = 102, // 0x00000066
    OfflineEnd = 103, // 0x00000067
  }
}
