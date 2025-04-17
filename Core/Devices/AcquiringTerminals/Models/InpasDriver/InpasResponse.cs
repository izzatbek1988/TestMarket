// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver.InpasResponse
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver
{
  [XmlRoot(ElementName = "response")]
  public class InpasResponse
  {
    [XmlElement(ElementName = "timeout")]
    public int Timeout { get; set; } = 180;

    [XmlElement(ElementName = "field")]
    public List<Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver.Field> Field { get; set; }
  }
}
