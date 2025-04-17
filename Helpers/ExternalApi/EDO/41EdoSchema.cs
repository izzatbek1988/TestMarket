// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвПродПерСвПерСвЛицПерИнЛицоПредОргПер
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
  public class ФайлДокументСвПродПерСвПерСвЛицПерИнЛицоПредОргПер
  {
    private ФИОТип фИОField;
    private string должностьField;
    private string иныеСведField;
    private string наимОргПерField;
    private string оснДоверОргПерField;
    private string оснПолнПредПерField;

    public ФИОТип ФИО
    {
      get => this.фИОField;
      set => this.фИОField = value;
    }

    [XmlAttribute]
    public string Должность
    {
      get => this.должностьField;
      set => this.должностьField = value;
    }

    [XmlAttribute]
    public string ИныеСвед
    {
      get => this.иныеСведField;
      set => this.иныеСведField = value;
    }

    [XmlAttribute]
    public string НаимОргПер
    {
      get => this.наимОргПерField;
      set => this.наимОргПерField = value;
    }

    [XmlAttribute]
    public string ОснДоверОргПер
    {
      get => this.оснДоверОргПерField;
      set => this.оснДоверОргПерField = value;
    }

    [XmlAttribute]
    public string ОснПолнПредПер
    {
      get => this.оснПолнПредПерField;
      set => this.оснПолнПредПерField = value;
    }
  }
}
