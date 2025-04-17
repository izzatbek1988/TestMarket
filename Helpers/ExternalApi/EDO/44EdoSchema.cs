// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвПродПерСвПерТранГруз
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
  public class ФайлДокументСвПродПерСвПерТранГруз
  {
    private ФайлДокументСвПродПерСвПерТранГрузТранНакл[] транНаклField;
    private УчастникТип перевозчикField;
    private string свТранГрузField;

    [XmlElement("ТранНакл")]
    public ФайлДокументСвПродПерСвПерТранГрузТранНакл[] ТранНакл
    {
      get => this.транНаклField;
      set => this.транНаклField = value;
    }

    public УчастникТип Перевозчик
    {
      get => this.перевозчикField;
      set => this.перевозчикField = value;
    }

    [XmlAttribute]
    public string СвТранГруз
    {
      get => this.свТранГрузField;
      set => this.свТранГрузField = value;
    }
  }
}
