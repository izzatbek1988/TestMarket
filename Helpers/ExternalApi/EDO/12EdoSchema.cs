// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.СвФЛТип
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
  [Serializable]
  public class СвФЛТип
  {
    private ФИОТип фИОField;
    private string госРегИПВыдДовField;
    private string иННФЛField;
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
    public string ИННФЛ
    {
      get => this.иННФЛField;
      set => this.иННФЛField = value;
    }

    [XmlAttribute]
    public string ИныеСвед
    {
      get => this.иныеСведField;
      set => this.иныеСведField = value;
    }
  }
}
