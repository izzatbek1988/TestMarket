// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.VolumeLocateAgainRawType
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
  public class VolumeLocateAgainRawType
  {
    private string typeRawMaterialField;
    private Decimal volumeSpiritField;
    private Decimal volumeRestOldField;
    private Decimal volumeTransferredField;
    private Decimal volumeReturnedField;
    private Decimal volumeRestField;

    public string TypeRawMaterial
    {
      get => this.typeRawMaterialField;
      set => this.typeRawMaterialField = value;
    }

    public Decimal VolumeSpirit
    {
      get => this.volumeSpiritField;
      set => this.volumeSpiritField = value;
    }

    public Decimal VolumeRestOld
    {
      get => this.volumeRestOldField;
      set => this.volumeRestOldField = value;
    }

    public Decimal VolumeTransferred
    {
      get => this.volumeTransferredField;
      set => this.volumeTransferredField = value;
    }

    public Decimal VolumeReturned
    {
      get => this.volumeReturnedField;
      set => this.volumeReturnedField = value;
    }

    public Decimal VolumeRest
    {
      get => this.volumeRestField;
      set => this.volumeRestField = value;
    }
  }
}
