// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.Polycard.Entity.Client
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
namespace Gbs.Helpers.API.Polycard.Entity
{
  public class Client
  {
    [JsonProperty("uid")]
    public Guid Uid { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("surname")]
    public string SurName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("middlename")]
    public string MiddleName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("birthday")]
    [JsonConverter(typeof (Client.CustomDateTimeConverter))]
    public DateTime? Birthday { get; set; }

    [JsonProperty("barcode")]
    public string Barcode { get; set; }

    [JsonProperty("bonus")]
    public float? Bonus { get; set; }

    [JsonProperty("discount")]
    public float? Discount { get; set; }

    public Client()
    {
    }

    public Client(ClientAdnSum gbsClient)
    {
      this.Uid = gbsClient.Client.Uid;
      this.Phone = gbsClient.Client.Phone.GetOnlyNumbers();
      this.Name = gbsClient.Client.Name;
      this.Email = gbsClient.Client.Email;
      this.Birthday = gbsClient.Client.Birthday;
      this.Barcode = gbsClient.Client.Barcode;
      this.Bonus = new float?((float) ((gbsClient.CurrentBonusSum + gbsClient.CloudBonusSum) * 100M));
    }

    private class CustomDateTimeConverter : IsoDateTimeConverter
    {
      public CustomDateTimeConverter() => this.DateTimeFormat = "yyyyMMdd";
    }
  }
}
