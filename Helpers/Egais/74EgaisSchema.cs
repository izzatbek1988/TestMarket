// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionClaimType
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
  public class PositionClaimType
  {
    private string vidAP171fzField;
    private Decimal alcPercentField;
    private Decimal quantityDalField;
    private Decimal capacityField;
    private string identityField;
    private string sampleFSMField;
    private string markTypeField;
    private Decimal quantityField;

    public string VidAP171fz
    {
      get => this.vidAP171fzField;
      set => this.vidAP171fzField = value;
    }

    public Decimal alcPercent
    {
      get => this.alcPercentField;
      set => this.alcPercentField = value;
    }

    public Decimal QuantityDal
    {
      get => this.quantityDalField;
      set => this.quantityDalField = value;
    }

    public Decimal Capacity
    {
      get => this.capacityField;
      set => this.capacityField = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string SampleFSM
    {
      get => this.sampleFSMField;
      set => this.sampleFSMField = value;
    }

    public string MarkType
    {
      get => this.markTypeField;
      set => this.markTypeField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }
  }
}
