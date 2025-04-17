// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.АдрРФТип
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
  public class АдрРФТип
  {
    private string индексField;
    private string кодРегионField;
    private string районField;
    private string городField;
    private string населПунктField;
    private string улицаField;
    private string домField;
    private string корпусField;
    private string квартField;

    [XmlAttribute]
    public string Индекс
    {
      get => this.индексField;
      set => this.индексField = value;
    }

    [XmlAttribute]
    public string КодРегион
    {
      get => this.кодРегионField;
      set => this.кодРегионField = value;
    }

    [XmlAttribute]
    public string Район
    {
      get => this.районField;
      set => this.районField = value;
    }

    [XmlAttribute]
    public string Город
    {
      get => this.городField;
      set => this.городField = value;
    }

    [XmlAttribute]
    public string НаселПункт
    {
      get => this.населПунктField;
      set => this.населПунктField = value;
    }

    [XmlAttribute]
    public string Улица
    {
      get => this.улицаField;
      set => this.улицаField = value;
    }

    [XmlAttribute]
    public string Дом
    {
      get => this.домField;
      set => this.домField = value;
    }

    [XmlAttribute]
    public string Корпус
    {
      get => this.корпусField;
      set => this.корпусField = value;
    }

    [XmlAttribute]
    public string Кварт
    {
      get => this.квартField;
      set => this.квартField = value;
    }
  }
}
