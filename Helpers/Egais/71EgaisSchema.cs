// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ClaimRawType
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
  public class ClaimRawType
  {
    private string claimNumberField;
    private DateTime claimDateField;
    private bool claimDateFieldSpecified;
    private Decimal sumAdvancePaymentField;
    private bool sumAdvancePaymentFieldSpecified;
    private Decimal volumeAnhydrousField;
    private bool volumeAnhydrousFieldSpecified;
    private string claimNumberFNSField;
    private DateTime claimDateFNSField;
    private bool claimDateFNSFieldSpecified;
    private Decimal volumeNotUsedField;

    public string ClaimNumber
    {
      get => this.claimNumberField;
      set => this.claimNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ClaimDate
    {
      get => this.claimDateField;
      set => this.claimDateField = value;
    }

    [XmlIgnore]
    public bool ClaimDateSpecified
    {
      get => this.claimDateFieldSpecified;
      set => this.claimDateFieldSpecified = value;
    }

    public Decimal SumAdvancePayment
    {
      get => this.sumAdvancePaymentField;
      set => this.sumAdvancePaymentField = value;
    }

    [XmlIgnore]
    public bool SumAdvancePaymentSpecified
    {
      get => this.sumAdvancePaymentFieldSpecified;
      set => this.sumAdvancePaymentFieldSpecified = value;
    }

    public Decimal VolumeAnhydrous
    {
      get => this.volumeAnhydrousField;
      set => this.volumeAnhydrousField = value;
    }

    [XmlIgnore]
    public bool VolumeAnhydrousSpecified
    {
      get => this.volumeAnhydrousFieldSpecified;
      set => this.volumeAnhydrousFieldSpecified = value;
    }

    public string ClaimNumberFNS
    {
      get => this.claimNumberFNSField;
      set => this.claimNumberFNSField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ClaimDateFNS
    {
      get => this.claimDateFNSField;
      set => this.claimDateFNSField = value;
    }

    [XmlIgnore]
    public bool ClaimDateFNSSpecified
    {
      get => this.claimDateFNSFieldSpecified;
      set => this.claimDateFNSFieldSpecified = value;
    }

    public Decimal VolumeNotUsed
    {
      get => this.volumeNotUsedField;
      set => this.volumeNotUsedField = value;
    }
  }
}
