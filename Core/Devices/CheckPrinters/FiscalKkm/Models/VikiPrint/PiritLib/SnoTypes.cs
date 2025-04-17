// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.SnoTypes
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum SnoTypes
  {
    [EnumMember(Value = "TOTAL")] Osn,
    [EnumMember(Value = "SIMPLIFIED_INCOME")] SimpleUsn,
    [EnumMember(Value = "SIMPLIFIED_INCOME_EXPENSE")] Usn,
    [EnumMember(Value = "SINGLE_AGRICULTURAL")] Esn,
    [EnumMember(Value = "PATENT")] Patent,
  }
}
