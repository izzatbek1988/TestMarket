// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SendSmsAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers
{
  public class SendSmsAnswer
  {
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_code")]
    public int ErrorCode { get; set; }

    [JsonProperty("phones")]
    public List<SendSmsAnswer.PhoneAnswer> Phones { get; set; } = new List<SendSmsAnswer.PhoneAnswer>();

    public class PhoneAnswer
    {
      [JsonProperty("phone")]
      public string Phone { get; set; }

      [JsonProperty("cost")]
      public string Cost { get; set; }

      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("error")]
      public string Error { get; set; }
    }
  }
}
