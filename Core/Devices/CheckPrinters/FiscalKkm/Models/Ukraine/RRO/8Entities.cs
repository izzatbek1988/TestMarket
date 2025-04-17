// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.ResultAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class ResultAnswer : Answer
  {
    [JsonProperty("NumFiscal")]
    public string NumFiscal { get; set; }

    [JsonProperty("NumLocal")]
    public string NumLocal { get; set; }

    [JsonProperty("Offline")]
    public bool Offline { get; set; }
  }
}
