// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументПодписант
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
  public class ФайлДокументПодписант
  {
    private object itemField;
    private ФайлДокументПодписантОблПолн облПолнField;
    private ФайлДокументПодписантСтатус статусField;
    private string оснПолнField;
    private string оснПолнОргField;

    [XmlElement("ИП", typeof (СвИПТип))]
    [XmlElement("ФЛ", typeof (СвФЛТип))]
    [XmlElement("ЮЛ", typeof (ФайлДокументПодписантЮЛ))]
    public object Item
    {
      get => this.itemField;
      set => this.itemField = value;
    }

    [XmlAttribute]
    public ФайлДокументПодписантОблПолн ОблПолн
    {
      get => this.облПолнField;
      set => this.облПолнField = value;
    }

    [XmlAttribute]
    public ФайлДокументПодписантСтатус Статус
    {
      get => this.статусField;
      set => this.статусField = value;
    }

    [XmlAttribute]
    public string ОснПолн
    {
      get => this.оснПолнField;
      set => this.оснПолнField = value;
    }

    [XmlAttribute]
    public string ОснПолнОрг
    {
      get => this.оснПолнОргField;
      set => this.оснПолнОргField = value;
    }
  }
}
