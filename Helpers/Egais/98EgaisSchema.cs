// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.boxtype
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/CommonV3")]
  [Serializable]
  public class boxtype
  {
    private string boxnumField;
    private object[] itemsField;

    [XmlElement(Order = 0)]
    public string boxnum
    {
      get => this.boxnumField;
      set => this.boxnumField = value;
    }

    [XmlElement("bl", typeof (boxtype), Order = 1)]
    [XmlElement("boxnum", typeof (string), Order = 1)]
    public object[] Items
    {
      get => this.itemsField;
      set => this.itemsField = value;
    }
  }
}
