// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвСчФактИспрСчФ
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
  public class ФайлДокументСвСчФактИспрСчФ
  {
    private string номИспрСчФField;
    private ФайлДокументСвСчФактИспрСчФДефНомИспрСчФ дефНомИспрСчФField;
    private bool дефНомИспрСчФFieldSpecified;
    private string датаИспрСчФField;
    private ФайлДокументСвСчФактИспрСчФДефДатаИспрСчФ дефДатаИспрСчФField;
    private bool дефДатаИспрСчФFieldSpecified;

    [XmlAttribute(DataType = "integer")]
    public string НомИспрСчФ
    {
      get => this.номИспрСчФField;
      set => this.номИспрСчФField = value;
    }

    [XmlAttribute]
    public ФайлДокументСвСчФактИспрСчФДефНомИспрСчФ ДефНомИспрСчФ
    {
      get => this.дефНомИспрСчФField;
      set => this.дефНомИспрСчФField = value;
    }

    [XmlIgnore]
    public bool ДефНомИспрСчФSpecified
    {
      get => this.дефНомИспрСчФFieldSpecified;
      set => this.дефНомИспрСчФFieldSpecified = value;
    }

    [XmlAttribute]
    public string ДатаИспрСчФ
    {
      get => this.датаИспрСчФField;
      set => this.датаИспрСчФField = value;
    }

    [XmlAttribute]
    public ФайлДокументСвСчФактИспрСчФДефДатаИспрСчФ ДефДатаИспрСчФ
    {
      get => this.дефДатаИспрСчФField;
      set => this.дефДатаИспрСчФField = value;
    }

    [XmlIgnore]
    public bool ДефДатаИспрСчФSpecified
    {
      get => this.дефДатаИспрСчФFieldSpecified;
      set => this.дефДатаИспрСчФFieldSpecified = value;
    }
  }
}
