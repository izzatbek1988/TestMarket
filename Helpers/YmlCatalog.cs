// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.YmlCatalog
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [XmlRoot(ElementName = "yml_catalog")]
  public class YmlCatalog
  {
    [XmlElement(ElementName = "shop")]
    public Shop Shop { get; set; } = new Shop();

    [XmlAttribute(AttributeName = "date")]
    public DateTime Date { get; set; }

    [XmlText]
    public string Text { get; set; }
  }
}
