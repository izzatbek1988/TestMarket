// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументСвСчФактДопСвФХЖ1
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
  public class ФайлДокументСвСчФактДопСвФХЖ1
  {
    private ФайлДокументСвСчФактДопСвФХЖ1ИнфПродГосЗакКазн инфПродГосЗакКазнField;
    private УчастникТип свФакторField;
    private ОснованиеТип оснУстДенТребField;
    private string идГосКонField;
    private string наимОКВField;
    private Decimal курсВалField;
    private bool курсВалFieldSpecified;
    private ФайлДокументСвСчФактДопСвФХЖ1ОбстФормСЧФ обстФормСЧФField;
    private bool обстФормСЧФFieldSpecified;

    public ФайлДокументСвСчФактДопСвФХЖ1ИнфПродГосЗакКазн ИнфПродГосЗакКазн
    {
      get => this.инфПродГосЗакКазнField;
      set => this.инфПродГосЗакКазнField = value;
    }

    public УчастникТип СвФактор
    {
      get => this.свФакторField;
      set => this.свФакторField = value;
    }

    public ОснованиеТип ОснУстДенТреб
    {
      get => this.оснУстДенТребField;
      set => this.оснУстДенТребField = value;
    }

    [XmlAttribute]
    public string ИдГосКон
    {
      get => this.идГосКонField;
      set => this.идГосКонField = value;
    }

    [XmlAttribute]
    public string НаимОКВ
    {
      get => this.наимОКВField;
      set => this.наимОКВField = value;
    }

    [XmlAttribute]
    public Decimal КурсВал
    {
      get => this.курсВалField;
      set => this.курсВалField = value;
    }

    [XmlIgnore]
    public bool КурсВалSpecified
    {
      get => this.курсВалFieldSpecified;
      set => this.курсВалFieldSpecified = value;
    }

    [XmlAttribute]
    public ФайлДокументСвСчФактДопСвФХЖ1ОбстФормСЧФ ОбстФормСЧФ
    {
      get => this.обстФормСЧФField;
      set => this.обстФормСЧФField = value;
    }

    [XmlIgnore]
    public bool ОбстФормСЧФSpecified
    {
      get => this.обстФормСЧФFieldSpecified;
      set => this.обстФормСЧФFieldSpecified = value;
    }
  }
}
