// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументТаблСчФактСведТовДопСведТов
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
  public class ФайлДокументТаблСчФактСведТовДопСведТов
  {
    private ФайлДокументТаблСчФактСведТовДопСведТовСведПрослеж[] сведПрослежField;
    private ФайлДокументТаблСчФактСведТовДопСведТовНомСредИдентТов[] номСредИдентТовField;
    private ФайлДокументТаблСчФактСведТовДопСведТовПрТовРаб прТовРабField;
    private bool прТовРабFieldSpecified;
    private string допПризнField;
    private string наимЕдИзмField;
    private string крНаимСтрПрField;
    private Decimal надлОтпField;
    private bool надлОтпFieldSpecified;
    private string характерТовField;
    private string сортТовField;
    private string артикулТовField;
    private string кодТовField;
    private string кодКатField;
    private string кодВидТовField;

    [XmlElement("СведПрослеж")]
    public ФайлДокументТаблСчФактСведТовДопСведТовСведПрослеж[] СведПрослеж
    {
      get => this.сведПрослежField;
      set => this.сведПрослежField = value;
    }

    [XmlElement("НомСредИдентТов")]
    public ФайлДокументТаблСчФактСведТовДопСведТовНомСредИдентТов[] НомСредИдентТов
    {
      get => this.номСредИдентТовField;
      set => this.номСредИдентТовField = value;
    }

    [XmlAttribute]
    public ФайлДокументТаблСчФактСведТовДопСведТовПрТовРаб ПрТовРаб
    {
      get => this.прТовРабField;
      set => this.прТовРабField = value;
    }

    [XmlIgnore]
    public bool ПрТовРабSpecified
    {
      get => this.прТовРабFieldSpecified;
      set => this.прТовРабFieldSpecified = value;
    }

    [XmlAttribute]
    public string ДопПризн
    {
      get => this.допПризнField;
      set => this.допПризнField = value;
    }

    [XmlAttribute]
    public string НаимЕдИзм
    {
      get => this.наимЕдИзмField;
      set => this.наимЕдИзмField = value;
    }

    [XmlAttribute]
    public string КрНаимСтрПр
    {
      get => this.крНаимСтрПрField;
      set => this.крНаимСтрПрField = value;
    }

    [XmlAttribute]
    public Decimal НадлОтп
    {
      get => this.надлОтпField;
      set => this.надлОтпField = value;
    }

    [XmlIgnore]
    public bool НадлОтпSpecified
    {
      get => this.надлОтпFieldSpecified;
      set => this.надлОтпFieldSpecified = value;
    }

    [XmlAttribute]
    public string ХарактерТов
    {
      get => this.характерТовField;
      set => this.характерТовField = value;
    }

    [XmlAttribute]
    public string СортТов
    {
      get => this.сортТовField;
      set => this.сортТовField = value;
    }

    [XmlAttribute]
    public string АртикулТов
    {
      get => this.артикулТовField;
      set => this.артикулТовField = value;
    }

    [XmlAttribute]
    public string КодТов
    {
      get => this.кодТовField;
      set => this.кодТовField = value;
    }

    [XmlAttribute]
    public string КодКат
    {
      get => this.кодКатField;
      set => this.кодКатField = value;
    }

    [XmlAttribute]
    public string КодВидТов
    {
      get => this.кодВидТовField;
      set => this.кодВидТовField = value;
    }
  }
}
