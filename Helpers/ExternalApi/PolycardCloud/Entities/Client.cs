// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExternalApi.PolycardCloud.Entities.Client
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Helpers.ExternalApi.PolycardCloud.Entities
{
  public class Client : IPolyCloudEntity
  {
    public string external_id { get; set; }

    public string name { get; set; }

    public string surname { get; set; } = "   ";

    public string phone { get; set; } = string.Empty;

    public string email { get; set; }

    public string address { get; set; }

    public bool is_deleted { get; set; }
  }
}
