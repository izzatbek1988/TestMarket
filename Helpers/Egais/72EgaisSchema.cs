// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.SpiritType
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
  public class SpiritType
  {
    private ProductInfoAsiiu_v2 rawField;
    private Decimal volumeAnhydrousRestField;
    private VolumeSpiritType volumeProducedRawField;
    private VolumeSpiritType volumeOutRawField;
    private VolumeReceivedRawType volumeReceivedRawField;
    private VolumeUsedRawType volumeUsedRawField;
    private ClaimRawType claimRawField;
    private Decimal totalRawField;

    public ProductInfoAsiiu_v2 Raw
    {
      get => this.rawField;
      set => this.rawField = value;
    }

    public Decimal VolumeAnhydrousRest
    {
      get => this.volumeAnhydrousRestField;
      set => this.volumeAnhydrousRestField = value;
    }

    public VolumeSpiritType VolumeProducedRaw
    {
      get => this.volumeProducedRawField;
      set => this.volumeProducedRawField = value;
    }

    public VolumeSpiritType VolumeOutRaw
    {
      get => this.volumeOutRawField;
      set => this.volumeOutRawField = value;
    }

    public VolumeReceivedRawType VolumeReceivedRaw
    {
      get => this.volumeReceivedRawField;
      set => this.volumeReceivedRawField = value;
    }

    public VolumeUsedRawType VolumeUsedRaw
    {
      get => this.volumeUsedRawField;
      set => this.volumeUsedRawField = value;
    }

    public ClaimRawType ClaimRaw
    {
      get => this.claimRawField;
      set => this.claimRawField = value;
    }

    public Decimal TotalRaw
    {
      get => this.totalRawField;
      set => this.totalRawField = value;
    }
  }
}
