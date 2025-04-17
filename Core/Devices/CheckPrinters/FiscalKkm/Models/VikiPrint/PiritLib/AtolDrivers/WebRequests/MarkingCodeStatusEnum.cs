// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests.MarkingCodeStatusEnum
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests
{
  public enum MarkingCodeStatusEnum
  {
    [EnumMember(Value = "itemPieceSold")] ItemPieceSold,
    [EnumMember(Value = "itemDryForSale")] ItemDryForSale,
    [EnumMember(Value = "itemPieceReturn")] ItemPieceReturn,
    [EnumMember(Value = "itemDryReturn")] ItemDryReturn,
    [EnumMember(Value = "itemStatusUnchanged")] ItemStatusUnchanged,
  }
}
