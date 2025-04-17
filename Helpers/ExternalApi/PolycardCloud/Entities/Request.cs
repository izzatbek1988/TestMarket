// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExternalApi.PolycardCloud.Entities.Request
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#nullable disable
namespace Gbs.Helpers.ExternalApi.PolycardCloud.Entities
{
  public class Request
  {
    [JsonConverter(typeof (StringEnumConverter))]
    public Request.Type action { get; set; }

    public IPolyCloudEntity data { get; set; }

    public enum EntityTypes
    {
      client,
      card,
    }

    public enum Type
    {
      create,
      update,
      delete,
      replace,
    }
  }
}
