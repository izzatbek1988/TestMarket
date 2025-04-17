// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Offer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [XmlRoot(ElementName = "offer")]
  public class Offer
  {
    [XmlElement(ElementName = "count")]
    public Decimal Count { get; set; }

    [XmlElement(ElementName = "price")]
    public Decimal Price { get; set; }

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "categoryId")]
    public int CategoryId { get; set; }

    [XmlElement(ElementName = "picture")]
    public string Picture { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "barcode")]
    public string Barcode { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public int Id { get; set; }

    [XmlText]
    public string Text { get; set; }

    [XmlElement(ElementName = "param")]
    public List<Gbs.Helpers.Param> Param { get; set; }
  }
}
