// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументТаблСчФактСведТов
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
  public class ФайлДокументТаблСчФактСведТов
  {
    private СумАкцизТип акцизField;
    private СумНДСТип сумНалField;
    private ФайлДокументТаблСчФактСведТовСвТД[] свТДField;
    private ФайлДокументТаблСчФактСведТовДопСведТов допСведТовField;
    private ТекстИнфТип[] инфПолФХЖ2Field;
    private string номСтрField;
    private string наимТовField;
    private string оКЕИ_ТовField;
    private ФайлДокументТаблСчФактСведТовДефОКЕИ_Тов дефОКЕИ_ТовField;
    private bool дефОКЕИ_ТовFieldSpecified;
    private Decimal колТовField;
    private Decimal КолТовДоField;
    private bool колТовFieldSpecified;
    private Decimal ценаТовField;
    private bool ценаТовFieldSpecified;
    private Decimal стТовБезНДСField;
    private bool стТовБезНДСFieldSpecified;
    private ФайлДокументТаблСчФактСведТовНалСт налСтField;
    private Decimal стТовУчНалField;
    private bool стТовУчНалFieldSpecified;
    private ФайлДокументТаблСчФактСведТовДефСтТовУчНал дефСтТовУчНалField;
    private bool дефСтТовУчНалFieldSpecified;

    public СумАкцизТип Акциз
    {
      get => this.акцизField;
      set => this.акцизField = value;
    }

    public СумНДСТип СумНал
    {
      get => this.сумНалField;
      set => this.сумНалField = value;
    }

    [XmlElement("СвТД")]
    public ФайлДокументТаблСчФактСведТовСвТД[] СвТД
    {
      get => this.свТДField;
      set => this.свТДField = value;
    }

    public ФайлДокументТаблСчФактСведТовДопСведТов ДопСведТов
    {
      get => this.допСведТовField;
      set => this.допСведТовField = value;
    }

    [XmlElement("ИнфПолФХЖ2")]
    public ТекстИнфТип[] ИнфПолФХЖ2
    {
      get => this.инфПолФХЖ2Field;
      set => this.инфПолФХЖ2Field = value;
    }

    [XmlAttribute(DataType = "integer")]
    public string НомСтр
    {
      get => this.номСтрField;
      set => this.номСтрField = value;
    }

    [XmlAttribute]
    public string НаимТов
    {
      get => this.наимТовField;
      set => this.наимТовField = value;
    }

    [XmlAttribute]
    public string ОКЕИ_Тов
    {
      get => this.оКЕИ_ТовField;
      set => this.оКЕИ_ТовField = value;
    }

    [XmlAttribute]
    public ФайлДокументТаблСчФактСведТовДефОКЕИ_Тов ДефОКЕИ_Тов
    {
      get => this.дефОКЕИ_ТовField;
      set => this.дефОКЕИ_ТовField = value;
    }

    [XmlIgnore]
    public bool ДефОКЕИ_ТовSpecified
    {
      get => this.дефОКЕИ_ТовFieldSpecified;
      set => this.дефОКЕИ_ТовFieldSpecified = value;
    }

    [XmlAttribute]
    public Decimal КолТов
    {
      get => this.колТовField;
      set => this.колТовField = value;
    }

    [XmlAttribute]
    public Decimal КолТовДо
    {
      get => this.КолТовДоField;
      set => this.КолТовДоField = value;
    }

    [XmlIgnore]
    public bool КолТовSpecified
    {
      get => this.колТовFieldSpecified;
      set => this.колТовFieldSpecified = value;
    }

    [XmlAttribute]
    public Decimal ЦенаТов
    {
      get => this.ценаТовField;
      set => this.ценаТовField = value;
    }

    [XmlIgnore]
    public bool ЦенаТовSpecified
    {
      get => this.ценаТовFieldSpecified;
      set => this.ценаТовFieldSpecified = value;
    }

    [XmlAttribute]
    public Decimal СтТовБезНДС
    {
      get => this.стТовБезНДСField;
      set => this.стТовБезНДСField = value;
    }

    [XmlIgnore]
    public bool СтТовБезНДСSpecified
    {
      get => this.стТовБезНДСFieldSpecified;
      set => this.стТовБезНДСFieldSpecified = value;
    }

    [XmlAttribute]
    public ФайлДокументТаблСчФактСведТовНалСт НалСт
    {
      get => this.налСтField;
      set => this.налСтField = value;
    }

    [XmlAttribute]
    public Decimal СтТовУчНал
    {
      get => this.стТовУчНалField;
      set => this.стТовУчНалField = value;
    }

    [XmlElement(ElementName = "СтТовУчНал")]
    public ФайлДокументТаблСчФактСведТов.СтТовУчНалКласс СтТовУчНалОбъект { get; set; }

    [XmlIgnore]
    public bool СтТовУчНалSpecified
    {
      get => this.стТовУчНалFieldSpecified;
      set => this.стТовУчНалFieldSpecified = value;
    }

    [XmlAttribute]
    public ФайлДокументТаблСчФактСведТовДефСтТовУчНал ДефСтТовУчНал
    {
      get => this.дефСтТовУчНалField;
      set => this.дефСтТовУчНалField = value;
    }

    [XmlIgnore]
    public bool ДефСтТовУчНалSpecified
    {
      get => this.дефСтТовУчНалFieldSpecified;
      set => this.дефСтТовУчНалFieldSpecified = value;
    }

    [XmlRoot(ElementName = "СтТовУчНал")]
    public class СтТовУчНалКласс
    {
      [XmlAttribute(AttributeName = "СтоимДоИзм")]
      public double СтоимДоИзм { get; set; }

      [XmlAttribute(AttributeName = "СтоимПослеИзм")]
      public double СтоимПослеИзм { get; set; }

      [XmlAttribute(AttributeName = "СтоимУм")]
      public Decimal СтоимУм { get; set; }
    }
  }
}
