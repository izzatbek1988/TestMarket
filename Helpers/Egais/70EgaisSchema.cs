// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.VolumeUsedRawType
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
  public class VolumeUsedRawType
  {
    private VolumeSpiritType volumeToAgingRawField;
    private VolumeSpiritType volumeToProducedRawField;

    public VolumeSpiritType VolumeToAgingRaw
    {
      get => this.volumeToAgingRawField;
      set => this.volumeToAgingRawField = value;
    }

    public VolumeSpiritType VolumeToProducedRaw
    {
      get => this.volumeToProducedRawField;
      set => this.volumeToProducedRawField = value;
    }
  }
}
