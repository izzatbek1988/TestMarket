// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ОснованиеТип
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
  public class ОснованиеТип
  {
    private string наимОснField;
    private string номОснField;
    private string датаОснField;
    private string допСвОснField;
    private string идентОснField;

    [XmlAttribute]
    public string НаимОсн
    {
      get => this.наимОснField;
      set => this.наимОснField = value;
    }

    [XmlAttribute]
    public string НомОсн
    {
      get => this.номОснField;
      set => this.номОснField = value;
    }

    [XmlAttribute]
    public string ДатаОсн
    {
      get => this.датаОснField;
      set => this.датаОснField = value;
    }

    [XmlAttribute]
    public string ДопСвОсн
    {
      get => this.допСвОснField;
      set => this.допСвОснField = value;
    }

    [XmlAttribute]
    public string ИдентОсн
    {
      get => this.идентОснField;
      set => this.идентОснField = value;
    }
  }
}
