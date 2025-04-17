// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвСчФактДопСвФХЖ1ИнфПродГосЗакКазн
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
  public class ФайлДокументСвСчФактДопСвФХЖ1ИнфПродГосЗакКазн
  {
    private string датаГосКонтField;
    private string номерГосКонтField;
    private string лицСчетПродField;
    private string кодПродБюджКлассField;
    private string кодЦелиПродField;
    private string кодКазначПродField;
    private string наимКазначПродField;

    [XmlAttribute]
    public string ДатаГосКонт
    {
      get => this.датаГосКонтField;
      set => this.датаГосКонтField = value;
    }

    [XmlAttribute]
    public string НомерГосКонт
    {
      get => this.номерГосКонтField;
      set => this.номерГосКонтField = value;
    }

    [XmlAttribute]
    public string ЛицСчетПрод
    {
      get => this.лицСчетПродField;
      set => this.лицСчетПродField = value;
    }

    [XmlAttribute]
    public string КодПродБюджКласс
    {
      get => this.кодПродБюджКлассField;
      set => this.кодПродБюджКлассField = value;
    }

    [XmlAttribute]
    public string КодЦелиПрод
    {
      get => this.кодЦелиПродField;
      set => this.кодЦелиПродField = value;
    }

    [XmlAttribute]
    public string КодКазначПрод
    {
      get => this.кодКазначПродField;
      set => this.кодКазначПродField = value;
    }

    [XmlAttribute]
    public string НаимКазначПрод
    {
      get => this.наимКазначПродField;
      set => this.наимКазначПродField = value;
    }
  }
}
