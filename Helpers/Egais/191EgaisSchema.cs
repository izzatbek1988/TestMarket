// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformF1F21
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
  [XmlType(TypeName = "InformF1F2", Namespace = "http://fsrar.ru/WEGAIS/ActWriteOff_v3")]
  [Serializable]
  public class InformF1F21
  {
    private object itemField;

    [XmlElement("InformF1", typeof (InformF1Type))]
    [XmlElement("InformF2", typeof (InformF2TypeItem))]
    public object Item
    {
      get => this.itemField;
      set => this.itemField = value;
    }
  }
}
