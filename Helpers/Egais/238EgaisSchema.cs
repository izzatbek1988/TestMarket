// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.DataTypeEgais
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/Asiiu", TypeName = "DataType")]
  [Serializable]
  public class DataTypeEgais
  {
    private ProductInfoAsiiu_v2 productField;
    private DateTime startDateField;
    private DateTime endDateField;
    private Decimal vbsStartField;
    private Decimal vbsEndField;
    private Decimal aStartField;
    private Decimal aEndField;
    private Decimal percentAlcField;
    private Decimal bottleCountStartField;
    private Decimal bottleCountEndField;
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

    public DateTime StartDate
    {
      get => this.startDateField;
      set => this.startDateField = value;
    }

    public DateTime EndDate
    {
      get => this.endDateField;
      set => this.endDateField = value;
    }

    public Decimal VbsStart
    {
      get => this.vbsStartField;
      set => this.vbsStartField = value;
    }

    public Decimal VbsEnd
    {
      get => this.vbsEndField;
      set => this.vbsEndField = value;
    }

    public Decimal AStart
    {
      get => this.aStartField;
      set => this.aStartField = value;
    }

    public Decimal AEnd
    {
      get => this.aEndField;
      set => this.aEndField = value;
    }

    public Decimal PercentAlc
    {
      get => this.percentAlcField;
      set => this.percentAlcField = value;
    }

    public Decimal BottleCountStart
    {
      get => this.bottleCountStartField;
      set => this.bottleCountStartField = value;
    }

    public Decimal BottleCountEnd
    {
      get => this.bottleCountEndField;
      set => this.bottleCountEndField = value;
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
