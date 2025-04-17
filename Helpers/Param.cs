// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Param
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [XmlRoot(ElementName = "param")]
  public class Param
  {
    public Param()
    {
    }

    public Param(string name, object text)
    {
      this.Name = name;
      if (Decimal.TryParse(text.ToString(), out Decimal _))
      {
        this.Text = text.ToString().Replace(',', '.');
      }
      else
      {
        DateTime result;
        if (DateTime.TryParse(text.ToString(), out result))
          this.Text = result.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
        else
          this.Text = text.ToString();
      }
    }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlText]
    public string Text { get; set; }
  }
}
