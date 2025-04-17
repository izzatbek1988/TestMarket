// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.Answer`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities
{
  public class Answer<T>
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("jsonrpc")]
    public string JsonRpc => "2.0";

    [JsonProperty("result")]
    public T Result { get; set; }

    [JsonProperty("error")]
    public ErrorInfo Error { get; set; }
  }
}
