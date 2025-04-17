// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.Request`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities
{
  public abstract class Request<T> : IRequest
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("jsonrpc")]
    public string JsonRpc => "2.0";

    [JsonProperty("method")]
    public abstract string Method { get; }

    [JsonProperty("params")]
    public abstract object Params { get; set; }

    [JsonIgnore]
    public string AnswerString { get; set; }

    [JsonIgnore]
    public T Result => JsonConvert.DeserializeObject<Answer<T>>(this.AnswerString).Result;

    [JsonIgnore]
    public ErrorInfo Error => JsonConvert.DeserializeObject<Answer<T>>(this.AnswerString).Error;
  }
}
