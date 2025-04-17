// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionType2
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
  [XmlType(TypeName = "PositionType", Namespace = "http://fsrar.ru/WEGAIS/RepProducedProduct")]
  [Serializable]
  public class PositionType2
  {
    private string productCodeField;
    private Decimal quantityField;
    private Decimal alcPercentField;
    private bool alcPercentFieldSpecified;
    private Decimal alcPercentMinField;
    private bool alcPercentMinFieldSpecified;
    private Decimal alcPercentMaxField;
    private bool alcPercentMaxFieldSpecified;
    private string partyField;
    private string identityField;
    private string comment1Field;
    private string comment2Field;
    private string comment3Field;
    private MarkInfoType2 markInfoField;

    public string ProductCode
    {
      get => this.productCodeField;
      set => this.productCodeField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public Decimal alcPercent
    {
      get => this.alcPercentField;
      set => this.alcPercentField = value;
    }

    [XmlIgnore]
    public bool alcPercentSpecified
    {
      get => this.alcPercentFieldSpecified;
      set => this.alcPercentFieldSpecified = value;
    }

    public Decimal alcPercentMin
    {
      get => this.alcPercentMinField;
      set => this.alcPercentMinField = value;
    }

    [XmlIgnore]
    public bool alcPercentMinSpecified
    {
      get => this.alcPercentMinFieldSpecified;
      set => this.alcPercentMinFieldSpecified = value;
    }

    public Decimal alcPercentMax
    {
      get => this.alcPercentMaxField;
      set => this.alcPercentMaxField = value;
    }

    [XmlIgnore]
    public bool alcPercentMaxSpecified
    {
      get => this.alcPercentMaxFieldSpecified;
      set => this.alcPercentMaxFieldSpecified = value;
    }

    public string Party
    {
      get => this.partyField;
      set => this.partyField = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string Comment1
    {
      get => this.comment1Field;
      set => this.comment1Field = value;
    }

    public string Comment2
    {
      get => this.comment2Field;
      set => this.comment2Field = value;
    }

    public string Comment3
    {
      get => this.comment3Field;
      set => this.comment3Field = value;
    }

    public MarkInfoType2 MarkInfo
    {
      get => this.markInfoField;
      set => this.markInfoField = value;
    }
  }
}
