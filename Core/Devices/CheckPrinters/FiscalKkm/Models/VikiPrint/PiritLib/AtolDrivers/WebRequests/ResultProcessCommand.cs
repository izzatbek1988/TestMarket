// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests.ResultProcessCommand
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests
{
  public enum ResultProcessCommand
  {
    [EnumMember(Value = "ready")] Ready,
    [EnumMember(Value = "error")] Error,
    [EnumMember(Value = "interrupted")] Interrupted,
    [EnumMember(Value = "canceled")] Canceled,
    [EnumMember(Value = "blocked")] Blocked,
    [EnumMember(Value = "wait")] Wait,
    [EnumMember(Value = "inProgress")] InProgress,
  }
}
