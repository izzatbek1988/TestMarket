// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionType4
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
  [XmlType(TypeName = "PositionType", Namespace = "http://fsrar.ru/WEGAIS/TTNSingle_v2")]
  [Serializable]
  public class PositionType4
  {
    private ProductInfo_v2 productField;
    private string pack_IDField;
    private Decimal quantityField;
    private Decimal alcPercentField;
    private bool alcPercentFieldSpecified;
    private Decimal alcPercentMinField;
    private bool alcPercentMinFieldSpecified;
    private Decimal alcPercentMaxField;
    private bool alcPercentMaxFieldSpecified;
    private Decimal priceField;
    private string partyField;
    private string identityField;
    private InformF1Type informF1Field;
    private InformF2Type informF2Field;

    public ProductInfo_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public string Pack_ID
    {
      get => this.pack_IDField;
      set => this.pack_IDField = value;
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

    public Decimal Price
    {
      get => this.priceField;
      set => this.priceField = value;
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

    public InformF1Type InformF1
    {
      get => this.informF1Field;
      set => this.informF1Field = value;
    }

    public InformF2Type InformF2
    {
      get => this.informF2Field;
      set => this.informF2Field = value;
    }
  }
}
