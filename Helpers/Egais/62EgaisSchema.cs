// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WinemakingAPType
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
  public class WinemakingAPType
  {
    private ProductInfoAsiiu_v2 rawField;
    private Decimal volumeGrapeField;
    private Decimal volumeGrapeGeographicalField;
    private Decimal volumeGrapeRegionField;
    private Decimal volumeGrapeGrowingField;
    private Decimal totalRawField;

    public ProductInfoAsiiu_v2 Raw
    {
      get => this.rawField;
      set => this.rawField = value;
    }

    public Decimal VolumeGrape
    {
      get => this.volumeGrapeField;
      set => this.volumeGrapeField = value;
    }

    public Decimal VolumeGrapeGeographical
    {
      get => this.volumeGrapeGeographicalField;
      set => this.volumeGrapeGeographicalField = value;
    }

    public Decimal VolumeGrapeRegion
    {
      get => this.volumeGrapeRegionField;
      set => this.volumeGrapeRegionField = value;
    }

    public Decimal VolumeGrapeGrowing
    {
      get => this.volumeGrapeGrowingField;
      set => this.volumeGrapeGrowingField = value;
    }

    public Decimal TotalRaw
    {
      get => this.totalRawField;
      set => this.totalRawField = value;
    }
  }
}
