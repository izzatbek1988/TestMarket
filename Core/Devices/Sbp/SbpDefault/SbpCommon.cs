// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.SbpCommon
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class SbpCommon
  {
    [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
    public string Account { get; set; }

    [JsonProperty("merchantId", NullValueHandling = NullValueHandling.Ignore)]
    public string MerchantId { get; set; }

    [JsonProperty("templateVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string TemplateVersion { get; set; } = "01";

    [JsonProperty("qrcType", NullValueHandling = NullValueHandling.Ignore)]
    public string QrcType { get; set; }

    [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
    public long? Amount { get; set; }

    [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
    public string Currency { get; set; } = "RUB";

    [JsonProperty("paymentPurpose", NullValueHandling = NullValueHandling.Ignore)]
    public string PaymentPurpose { get; set; }

    [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
    public SbpParams Params { get; set; } = new SbpParams();

    [JsonProperty("qrcIds", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> QrcIds { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public List<SbpStatus> Statuses { get; set; }
  }
}
