// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвПродПерСвПер
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
  public class ФайлДокументСвПродПерСвПер
  {
    private ОснованиеТип[] оснПерField;
    private ФайлДокументСвПродПерСвПерСвЛицПер свЛицПерField;
    private ФайлДокументСвПродПерСвПерТранГруз транГрузField;
    private ФайлДокументСвПродПерСвПерСвПерВещи свПерВещиField;
    private string содОперField;
    private string видОперField;
    private string датаПерField;
    private string датаНачField;
    private string датаОконField;

    [XmlElement("ОснПер")]
    public ОснованиеТип[] ОснПер
    {
      get => this.оснПерField;
      set => this.оснПерField = value;
    }

    public ФайлДокументСвПродПерСвПерСвЛицПер СвЛицПер
    {
      get => this.свЛицПерField;
      set => this.свЛицПерField = value;
    }

    public ФайлДокументСвПродПерСвПерТранГруз ТранГруз
    {
      get => this.транГрузField;
      set => this.транГрузField = value;
    }

    public ФайлДокументСвПродПерСвПерСвПерВещи СвПерВещи
    {
      get => this.свПерВещиField;
      set => this.свПерВещиField = value;
    }

    [XmlAttribute]
    public string СодОпер
    {
      get => this.содОперField;
      set => this.содОперField = value;
    }

    [XmlAttribute]
    public string ВидОпер
    {
      get => this.видОперField;
      set => this.видОперField = value;
    }

    [XmlAttribute]
    public string ДатаПер
    {
      get => this.датаПерField;
      set => this.датаПерField = value;
    }

    [XmlAttribute]
    public string ДатаНач
    {
      get => this.датаНачField;
      set => this.датаНачField = value;
    }

    [XmlAttribute]
    public string ДатаОкон
    {
      get => this.датаОконField;
      set => this.датаОконField = value;
    }
  }
}
