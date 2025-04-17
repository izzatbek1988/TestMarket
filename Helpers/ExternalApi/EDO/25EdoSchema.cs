// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвСчФактСвПРД
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [GeneratedCode("xsd", "4.7.3081.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true)]
  [Serializable]
  public class ФайлДокументСвСчФактСвПРД
  {
    private string номерПРДField;
    private string датаПРДField;
    private Decimal суммаПРДField;
    private bool суммаПРДFieldSpecified;

    [XmlAttribute]
    public string НомерПРД
    {
      get => this.номерПРДField;
      set => this.номерПРДField = value;
    }

    [XmlAttribute]
    public string ДатаПРД
    {
      get => this.датаПРДField;
      set => this.датаПРДField = value;
    }

    [XmlAttribute]
    public Decimal СуммаПРД
    {
      get => this.суммаПРДField;
      set => this.суммаПРДField = value;
    }

    [XmlIgnore]
    public bool СуммаПРДSpecified
    {
      get => this.суммаПРДFieldSpecified;
      set => this.суммаПРДFieldSpecified = value;
    }
  }
}
