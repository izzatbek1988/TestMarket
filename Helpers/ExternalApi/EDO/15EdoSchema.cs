// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.УчастникТип
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
  public class УчастникТип
  {
    private УчастникТипИдСв идСвField;
    private АдресТип адресField;
    private КонтактТип контактField;
    private УчастникТипБанкРекв банкРеквField;
    private string оКПОField;
    private string структПодрField;
    private string инфДляУчастField;
    private string краткНазвField;

    public УчастникТипИдСв ИдСв
    {
      get => this.идСвField;
      set => this.идСвField = value;
    }

    public АдресТип Адрес
    {
      get => this.адресField;
      set => this.адресField = value;
    }

    public КонтактТип Контакт
    {
      get => this.контактField;
      set => this.контактField = value;
    }

    public УчастникТипБанкРекв БанкРекв
    {
      get => this.банкРеквField;
      set => this.банкРеквField = value;
    }

    [XmlAttribute]
    public string ОКПО
    {
      get => this.оКПОField;
      set => this.оКПОField = value;
    }

    [XmlAttribute]
    public string СтруктПодр
    {
      get => this.структПодрField;
      set => this.структПодрField = value;
    }

    [XmlAttribute]
    public string ИнфДляУчаст
    {
      get => this.инфДляУчастField;
      set => this.инфДляУчастField = value;
    }

    [XmlAttribute]
    public string КраткНазв
    {
      get => this.краткНазвField;
      set => this.краткНазвField = value;
    }
  }
}
