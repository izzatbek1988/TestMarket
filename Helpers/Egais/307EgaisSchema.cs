// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.MarkInfoType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ProductRef")]
  [Serializable]
  public class MarkInfoType
  {
    private string typeField;
    private MarkInfoTypeRange[] rangesField;

    public string Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlArrayItem("Range", IsNullable = false)]
    public MarkInfoTypeRange[] Ranges
    {
      get => this.rangesField;
      set => this.rangesField = value;
    }
  }
}
