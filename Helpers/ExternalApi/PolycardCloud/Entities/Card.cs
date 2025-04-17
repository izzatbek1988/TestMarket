// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExternalApi.PolycardCloud.Entities.Card
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#nullable disable
namespace Gbs.Helpers.ExternalApi.PolycardCloud.Entities
{
  public class Card : IPolyCloudEntity
  {
    public string client_id { get; set; }

    public string external_id { get; set; }

    public string code { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public Card.CardCodeTypes card_code_type { get; set; }

    public string loyalty_group_id { get; set; }

    public bool is_deleted { get; set; }

    public enum CardCodeTypes
    {
      bar,
      qr,
      phone,
    }
  }
}
