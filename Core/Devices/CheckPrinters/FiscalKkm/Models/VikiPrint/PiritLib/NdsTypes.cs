// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.NdsTypes
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum NdsTypes
  {
    [EnumMember(Value = "NDS_20")] Nds20,
    [EnumMember(Value = "NDS_10")] Nds10,
    [EnumMember(Value = "NDS_0")] NdsZero,
    [EnumMember(Value = "NDS_NO")] NdsNo,
    [EnumMember(Value = "NDS_20_120")] Nds120,
    [EnumMember(Value = "NDS_10_110")] Nds110,
    [EnumMember(Value = "NDS_5")] Nds5,
    [EnumMember(Value = "NDS_7")] Nds7,
    [EnumMember(Value = "NDS_5_105")] Nds105,
    [EnumMember(Value = "NDS_7_107")] Nds107,
  }
}
