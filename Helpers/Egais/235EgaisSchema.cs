// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.DataType1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(TypeName = "DataTypeEgais", Namespace = "http://fsrar.ru/WEGAIS/AsiiuTime")]
  [Serializable]
  public class DataType1
  {
    private ProductInfoAsiiu_v2 productField;
    private DateTime controlDateField;
    private Decimal vbsControlField;
    private Decimal aControlField;
    private Decimal percentAlcField;
    private Decimal bottleCountControlField;
    private Decimal temperatureField;
    private ModeEnum modeField;
    private Decimal crotonaldehydField;
    private bool crotonaldehydFieldSpecified;
    private Decimal tolueneField;
    private bool tolueneFieldSpecified;

    public ProductInfoAsiiu_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public DateTime ControlDate
    {
      get => this.controlDateField;
      set => this.controlDateField = value;
    }

    public Decimal VbsControl
    {
      get => this.vbsControlField;
      set => this.vbsControlField = value;
    }

    public Decimal AControl
    {
      get => this.aControlField;
      set => this.aControlField = value;
    }

    public Decimal PercentAlc
    {
      get => this.percentAlcField;
      set => this.percentAlcField = value;
    }

    public Decimal BottleCountControl
    {
      get => this.bottleCountControlField;
      set => this.bottleCountControlField = value;
    }

    public Decimal Temperature
    {
      get => this.temperatureField;
      set => this.temperatureField = value;
    }

    public ModeEnum Mode
    {
      get => this.modeField;
      set => this.modeField = value;
    }

    public Decimal Crotonaldehyd
    {
      get => this.crotonaldehydField;
      set => this.crotonaldehydField = value;
    }

    [XmlIgnore]
    public bool CrotonaldehydSpecified
    {
      get => this.crotonaldehydFieldSpecified;
      set => this.crotonaldehydFieldSpecified = value;
    }

    public Decimal Toluene
    {
      get => this.tolueneField;
      set => this.tolueneField = value;
    }

    [XmlIgnore]
    public bool TolueneSpecified
    {
      get => this.tolueneFieldSpecified;
      set => this.tolueneFieldSpecified = value;
    }
  }
}
