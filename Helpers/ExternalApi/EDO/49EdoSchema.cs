// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументПодписантЮЛ
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
  public class ФайлДокументПодписантЮЛ
  {
    private ФИОТип фИОField;
    private string госРегИПВыдДовField;
    private string иННЮЛField;
    private string наимОргField;
    private string должнField;
    private string иныеСведField;

    public ФИОТип ФИО
    {
      get => this.фИОField;
      set => this.фИОField = value;
    }

    [XmlAttribute]
    public string ГосРегИПВыдДов
    {
      get => this.госРегИПВыдДовField;
      set => this.госРегИПВыдДовField = value;
    }

    [XmlAttribute]
    public string ИННЮЛ
    {
      get => this.иННЮЛField;
      set => this.иННЮЛField = value;
    }

    [XmlAttribute]
    public string НаимОрг
    {
      get => this.наимОргField;
      set => this.наимОргField = value;
    }

    [XmlAttribute]
    public string Должн
    {
      get => this.должнField;
      set => this.должнField = value;
    }

    [XmlAttribute]
    public string ИныеСвед
    {
      get => this.иныеСведField;
      set => this.иныеСведField = value;
    }
  }
}
