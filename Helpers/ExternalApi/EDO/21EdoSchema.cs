// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокумент
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
  public class ФайлДокумент
  {
    private ФайлДокументСвСчФакт свСчФактField;
    private ФайлДокументТаблСчФакт таблСчФактField;
    private ФайлДокументТаблСчФакт таблКСчФField;
    private ФайлДокументСвПродПер свПродПерField;
    private ФайлДокументПодписант[] подписантField;
    private string поФактХЖField;
    private string наимДокОпрField;
    private string датаИнфПрField;
    private string времИнфПрField;
    private string наимЭконСубСостField;
    private string оснДоверОргСостField;
    private string соглСтрДопИнфField;

    public ФайлДокументСвСчФакт СвСчФакт
    {
      get => this.свСчФактField;
      set => this.свСчФактField = value;
    }

    public ФайлДокументТаблСчФакт ТаблСчФакт
    {
      get => this.таблСчФактField;
      set => this.таблСчФактField = value;
    }

    public ФайлДокументТаблСчФакт ТаблКСчФ
    {
      get => this.таблКСчФField;
      set => this.таблКСчФField = value;
    }

    public ФайлДокументСвПродПер СвПродПер
    {
      get => this.свПродПерField;
      set => this.свПродПерField = value;
    }

    [XmlElement("Подписант")]
    public ФайлДокументПодписант[] Подписант
    {
      get => this.подписантField;
      set => this.подписантField = value;
    }

    [XmlAttribute]
    public string ПоФактХЖ
    {
      get => this.поФактХЖField;
      set => this.поФактХЖField = value;
    }

    [XmlAttribute]
    public string НаимДокОпр
    {
      get => this.наимДокОпрField;
      set => this.наимДокОпрField = value;
    }

    [XmlAttribute]
    public string ДатаИнфПр
    {
      get => this.датаИнфПрField;
      set => this.датаИнфПрField = value;
    }

    [XmlAttribute]
    public string ВремИнфПр
    {
      get => this.времИнфПрField;
      set => this.времИнфПрField = value;
    }

    [XmlAttribute]
    public string ОснДоверОргСост
    {
      get => this.оснДоверОргСостField;
      set => this.оснДоверОргСостField = value;
    }

    [XmlAttribute]
    public string СоглСтрДопИнф
    {
      get => this.соглСтрДопИнфField;
      set => this.соглСтрДопИнфField = value;
    }
  }
}
