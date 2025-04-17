// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.Request`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  [JsonObject]
  public abstract class Request<T> : IRequest
  {
    [JsonProperty("Command")]
    public abstract string Command { get; }

    public string Certificate { get; set; }

    public string PrivateKey { get; set; }

    public string Password { get; set; }

    [JsonIgnore]
    public string AnswerString { get; set; }

    [JsonIgnore]
    public T Result => JsonConvert.DeserializeObject<T>(this.AnswerString);
  }
}
