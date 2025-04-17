// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвСчФакт
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
  public class ФайлДокументСвСчФакт
  {
    private ФайлДокументСвСчФактИспрСчФ испрСчФField;
    private УчастникТип[] свПродField;
    private ФайлДокументСвСчФактГрузОт[] грузОтField;
    private УчастникТип[] грузПолучField;
    private ФайлДокументСвСчФактСвПРД[] свПРДField;
    private УчастникТип[] свПокупField;
    private ФайлДокументСвСчФактДопСвФХЖ1 допСвФХЖ1Field;
    private ФайлДокументСвСчФактДокПодтвОтгр[] докПодтвОтгрField;
    private ФайлДокументСвСчФактИнфПолФХЖ1 инфПолФХЖ1Field;
    private string номерСчФField;
    private string датаСчФField;
    private string кодОКВField;

    public ФайлДокументСвСчФактИспрСчФ ИспрСчФ
    {
      get => this.испрСчФField;
      set => this.испрСчФField = value;
    }

    [XmlElement("СвПрод")]
    public УчастникТип[] СвПрод
    {
      get => this.свПродField;
      set => this.свПродField = value;
    }

    [XmlElement("ГрузОт")]
    public ФайлДокументСвСчФактГрузОт[] ГрузОт
    {
      get => this.грузОтField;
      set => this.грузОтField = value;
    }

    [XmlElement("ГрузПолуч")]
    public УчастникТип[] ГрузПолуч
    {
      get => this.грузПолучField;
      set => this.грузПолучField = value;
    }

    [XmlElement("СвПРД")]
    public ФайлДокументСвСчФактСвПРД[] СвПРД
    {
      get => this.свПРДField;
      set => this.свПРДField = value;
    }

    [XmlElement("СвПокуп")]
    public УчастникТип[] СвПокуп
    {
      get => this.свПокупField;
      set => this.свПокупField = value;
    }

    public ФайлДокументСвСчФактДопСвФХЖ1 ДопСвФХЖ1
    {
      get => this.допСвФХЖ1Field;
      set => this.допСвФХЖ1Field = value;
    }

    [XmlElement("ДокПодтвОтгр")]
    public ФайлДокументСвСчФактДокПодтвОтгр[] ДокПодтвОтгр
    {
      get => this.докПодтвОтгрField;
      set => this.докПодтвОтгрField = value;
    }

    public ФайлДокументСвСчФактИнфПолФХЖ1 ИнфПолФХЖ1
    {
      get => this.инфПолФХЖ1Field;
      set => this.инфПолФХЖ1Field = value;
    }

    [XmlAttribute]
    public string НомерСчФ
    {
      get => this.номерСчФField;
      set => this.номерСчФField = value;
    }

    [XmlAttribute]
    public string ДатаСчФ
    {
      get => this.датаСчФField;
      set => this.датаСчФField = value;
    }

    [XmlAttribute]
    public string КодОКВ
    {
      get => this.кодОКВField;
      set => this.кодОКВField = value;
    }
  }
}
