// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.BarcodeType
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum BarcodeType
  {
    [EnumMember(Value = "UPC_A")] UpcA,
    [EnumMember(Value = "EAN_13")] Ean13,
    [EnumMember(Value = "EAN_8")] Ean8,
    [EnumMember(Value = "CODE_39")] Code39,
    [EnumMember(Value = "INTERLEAVED_2_5")] Interleaved25,
    [EnumMember(Value = "CODABAR")] CodBar,
    [EnumMember(Value = "PDF_417")] Pdf417,
    [EnumMember(Value = "QR_CODE")] QrCode,
    [EnumMember(Value = "CODE_128")] Code128,
  }
}
