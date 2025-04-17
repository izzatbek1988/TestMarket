// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.SbpStatus
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class SbpStatus : SbpCommon
  {
    [JsonProperty("qrcId", NullValueHandling = NullValueHandling.Ignore)]
    public string QrcId { get; set; }

    [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
    public string Code { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
    public string Status { get; set; }

    [JsonProperty("operationId", NullValueHandling = NullValueHandling.Ignore)]
    public long? OperationId { get; set; }

    [JsonProperty("trxId", NullValueHandling = NullValueHandling.Ignore)]
    public string TrxId { get; set; }

    [JsonProperty("operationTimestamp", NullValueHandling = NullValueHandling.Ignore)]
    public string OperationTimestamp { get; set; }
  }
}
