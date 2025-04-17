// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WinemakingAPSTPType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClaimIssueFSM")]
  [Serializable]
  public class WinemakingAPSTPType
  {
    private ProductInfoAsiiu_v2 rawField;
    private Decimal volumeRestField;
    private Decimal volumeGrapeCollectField;
    private Decimal volumeGrapeUseField;
    private Decimal volumeToAgingRawField;
    private Decimal volumeFromAgingRawField;
    private Decimal volumeUsedgRawField;
    private Decimal totalRawField;

    public ProductInfoAsiiu_v2 Raw
    {
      get => this.rawField;
      set => this.rawField = value;
    }

    public Decimal VolumeRest
    {
      get => this.volumeRestField;
      set => this.volumeRestField = value;
    }

    public Decimal VolumeGrapeCollect
    {
      get => this.volumeGrapeCollectField;
      set => this.volumeGrapeCollectField = value;
    }

    public Decimal VolumeGrapeUse
    {
      get => this.volumeGrapeUseField;
      set => this.volumeGrapeUseField = value;
    }

    public Decimal VolumeToAgingRaw
    {
      get => this.volumeToAgingRawField;
      set => this.volumeToAgingRawField = value;
    }

    public Decimal VolumeFromAgingRaw
    {
      get => this.volumeFromAgingRawField;
      set => this.volumeFromAgingRawField = value;
    }

    public Decimal VolumeUsedgRaw
    {
      get => this.volumeUsedgRawField;
      set => this.volumeUsedgRawField = value;
    }

    public Decimal TotalRaw
    {
      get => this.totalRawField;
      set => this.totalRawField = value;
    }
  }
}
