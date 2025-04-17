// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.StockPositionType1
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
  [XmlType(TypeName = "StockPositionType", Namespace = "http://fsrar.ru/WEGAIS/ReplyRests_v2")]
  [Serializable]
  public class StockPositionType1
  {
    private ProductInfoReply_v2 productField;
    private Decimal quantityField;
    private string informF1RegIdField;
    private string informF2RegIdField;
    private Decimal alcPercentField;
    private bool alcPercentFieldSpecified;
    private Decimal alcPercentMinField;
    private bool alcPercentMinFieldSpecified;
    private Decimal alcPercentMaxField;
    private bool alcPercentMaxFieldSpecified;

    public ProductInfoReply_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public string InformF1RegId
    {
      get => this.informF1RegIdField;
      set => this.informF1RegIdField = value;
    }

    public string InformF2RegId
    {
      get => this.informF2RegIdField;
      set => this.informF2RegIdField = value;
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
  }
}
