// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументТаблСчФактВсегоОпл
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
  public class ФайлДокументТаблСчФактВсегоОпл
  {
    private СумНДСТип сумНалВсегоField;
    private Decimal колНеттоВсField;
    private bool колНеттоВсFieldSpecified;
    private Decimal стТовБезНДСВсегоField;
    private bool стТовБезНДСВсегоFieldSpecified;
    private Decimal стТовУчНалВсегоField;
    private bool стТовУчНалВсегоFieldSpecified;
    private ФайлДокументТаблСчФактВсегоОплДефСтТовУчНалВсего дефСтТовУчНалВсегоField;
    private bool дефСтТовУчНалВсегоFieldSpecified;

    public СумНДСТип СумНалВсего
    {
      get => this.сумНалВсегоField;
      set => this.сумНалВсегоField = value;
    }

    public Decimal КолНеттоВс
    {
      get => this.колНеттоВсField;
      set => this.колНеттоВсField = value;
    }

    [XmlIgnore]
    public bool КолНеттоВсSpecified
    {
      get => this.колНеттоВсFieldSpecified;
      set => this.колНеттоВсFieldSpecified = value;
    }

    [XmlAttribute]
    public Decimal СтТовБезНДСВсего
    {
      get => this.стТовБезНДСВсегоField;
      set => this.стТовБезНДСВсегоField = value;
    }

    [XmlIgnore]
    public bool СтТовБезНДСВсегоSpecified
    {
      get => this.стТовБезНДСВсегоFieldSpecified;
      set => this.стТовБезНДСВсегоFieldSpecified = value;
    }

    [XmlAttribute]
    public Decimal СтТовУчНалВсего
    {
      get => this.стТовУчНалВсегоField;
      set => this.стТовУчНалВсегоField = value;
    }

    [XmlIgnore]
    public bool СтТовУчНалВсегоSpecified
    {
      get => this.стТовУчНалВсегоFieldSpecified;
      set => this.стТовУчНалВсегоFieldSpecified = value;
    }

    [XmlAttribute]
    public ФайлДокументТаблСчФактВсегоОплДефСтТовУчНалВсего ДефСтТовУчНалВсего
    {
      get => this.дефСтТовУчНалВсегоField;
      set => this.дефСтТовУчНалВсегоField = value;
    }

    [XmlIgnore]
    public bool ДефСтТовУчНалВсегоSpecified
    {
      get => this.дефСтТовУчНалВсегоFieldSpecified;
      set => this.дефСтТовУчНалВсегоFieldSpecified = value;
    }
  }
}
