// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.СвИПТип
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
  public class СвИПТип
  {
    private ФИОТип фИОField;
    private string иННФЛField;
    private СвИПТипДефИННФЛ дефИННФЛField;
    private bool дефИННФЛFieldSpecified;
    private string свГосРегИПField;
    private string иныеСведField;

    public ФИОТип ФИО
    {
      get => this.фИОField;
      set => this.фИОField = value;
    }

    [XmlAttribute]
    public string ИННФЛ
    {
      get => this.иННФЛField;
      set => this.иННФЛField = value;
    }

    [XmlAttribute]
    public СвИПТипДефИННФЛ ДефИННФЛ
    {
      get => this.дефИННФЛField;
      set => this.дефИННФЛField = value;
    }

    [XmlIgnore]
    public bool ДефИННФЛSpecified
    {
      get => this.дефИННФЛFieldSpecified;
      set => this.дефИННФЛFieldSpecified = value;
    }

    [XmlAttribute]
    public string СвГосРегИП
    {
      get => this.свГосРегИПField;
      set => this.свГосРегИПField = value;
    }

    [XmlAttribute]
    public string ИныеСвед
    {
      get => this.иныеСведField;
      set => this.иныеСведField = value;
    }
  }
}
