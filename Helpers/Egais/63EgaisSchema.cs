// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.VolumeMarkedProductType
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
  public class VolumeMarkedProductType
  {
    private string sampleFSMField;
    private string vidAP171fzField;
    private Decimal volumeSpiritField;
    private Decimal volumeMarkedAPField;
    private Decimal capacityField;
    private Decimal amountFSMField;
    private Decimal volumeAnhydrousSpiritField;

    public string SampleFSM
    {
      get => this.sampleFSMField;
      set => this.sampleFSMField = value;
    }

    public string VidAP171fz
    {
      get => this.vidAP171fzField;
      set => this.vidAP171fzField = value;
    }

    public Decimal VolumeSpirit
    {
      get => this.volumeSpiritField;
      set => this.volumeSpiritField = value;
    }

    public Decimal VolumeMarkedAP
    {
      get => this.volumeMarkedAPField;
      set => this.volumeMarkedAPField = value;
    }

    public Decimal Capacity
    {
      get => this.capacityField;
      set => this.capacityField = value;
    }

    public Decimal AmountFSM
    {
      get => this.amountFSMField;
      set => this.amountFSMField = value;
    }

    public Decimal VolumeAnhydrousSpirit
    {
      get => this.volumeAnhydrousSpiritField;
      set => this.volumeAnhydrousSpiritField = value;
    }
  }
}
