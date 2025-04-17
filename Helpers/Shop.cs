// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Shop
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [XmlRoot(ElementName = "shop")]
  public class Shop
  {
    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "company")]
    public string Company { get; set; }

    [XmlElement(ElementName = "url")]
    public string Url { get; set; }

    [XmlElement(ElementName = "platform")]
    public string Platform { get; set; }

    [XmlElement(ElementName = "version")]
    public double Version { get; set; }

    [XmlElement(ElementName = "agency")]
    public string Agency { get; set; }

    [XmlElement(ElementName = "email")]
    public string Email { get; set; }

    [XmlElement(ElementName = "categories")]
    public Categories Categories { get; set; } = new Categories();

    [XmlElement(ElementName = "offers")]
    public Offers Offers { get; set; } = new Offers();
  }
}
