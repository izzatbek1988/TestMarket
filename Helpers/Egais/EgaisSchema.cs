// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.Cheque
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [XmlRoot(ElementName = "Cheque")]
  public class Cheque
  {
    [XmlElement(ElementName = "Bottle")]
    public List<Cheque.BottleItem> Bottle { get; set; }

    [XmlAttribute(AttributeName = "inn")]
    public string Inn { get; set; }

    [XmlAttribute(AttributeName = "datetime")]
    public string Datetime { get; set; }

    [XmlAttribute(AttributeName = "kpp")]
    public string Kpp { get; set; }

    [XmlAttribute(AttributeName = "kassa")]
    public string Kassa { get; set; }

    [XmlAttribute(AttributeName = "address")]
    public string Address { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "number")]
    public string Number { get; set; }

    [XmlAttribute(AttributeName = "shift")]
    public int Shift { get; set; }

    [XmlRoot(ElementName = "Bottle")]
    public class BottleItem
    {
      [XmlAttribute(AttributeName = "barcode")]
      public string Barcode { get; set; }

      [XmlAttribute(AttributeName = "ean")]
      public string Ean { get; set; }

      [XmlAttribute(AttributeName = "price")]
      public string Price { get; set; }

      [XmlAttribute(AttributeName = "volume")]
      public string Volume { get; set; }
    }
  }
}
