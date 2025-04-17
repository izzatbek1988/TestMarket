// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.ResultGetZReportInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities
{
  public class ResultGetZReportInfo
  {
    [JsonProperty("OpenTime")]
    [JsonConverter(typeof (CustomDateTimeConverter))]
    public DateTime? OpenTime { get; set; }

    [JsonProperty("Number")]
    public int Number { get; set; }

    [JsonProperty("TerminalID")]
    public string TerminalId { get; set; }

    [JsonProperty("AppletVersion")]
    public string AppletVersion { get; set; }
  }
}
