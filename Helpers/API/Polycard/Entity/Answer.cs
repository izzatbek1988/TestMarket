// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.Polycard.Entity.SuccessAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Helpers.API.Polycard.Entity
{
  public class SuccessAnswer : IAnswer
  {
    public AnswerStatuses Status { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    public SuccessAnswer(Guid uid, string code)
    {
      this.Id = uid;
      this.Code = code;
    }
  }
}
