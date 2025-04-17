// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionType15
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
  [XmlType(TypeName = "PositionType", Namespace = "http://fsrar.ru/WEGAIS/TTNSingle_v4")]
  [Serializable]
  public class PositionType15
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
    private string eXCISE_NUMBERField;
    private DateTime eXCISE_DATEField;
    private bool eXCISE_DATEFieldSpecified;
    private Decimal eXCISE_SUMField;
    private bool eXCISE_SUMFieldSpecified;
    private Decimal eXCISE_BSField;
    private bool eXCISE_BSFieldSpecified;
    private string eAN13Field;
    private string fARegIdField;
    private InformF2TypeItemBC informF2Field;
    private boxtype[] boxInfoField;

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

    public string EXCISE_NUMBER
    {
      get => this.eXCISE_NUMBERField;
      set => this.eXCISE_NUMBERField = value;
    }

    public DateTime EXCISE_DATE
    {
      get => this.eXCISE_DATEField;
      set => this.eXCISE_DATEField = value;
    }

    [XmlIgnore]
    public bool EXCISE_DATESpecified
    {
      get => this.eXCISE_DATEFieldSpecified;
      set => this.eXCISE_DATEFieldSpecified = value;
    }

    public Decimal EXCISE_SUM
    {
      get => this.eXCISE_SUMField;
      set => this.eXCISE_SUMField = value;
    }

    [XmlIgnore]
    public bool EXCISE_SUMSpecified
    {
      get => this.eXCISE_SUMFieldSpecified;
      set => this.eXCISE_SUMFieldSpecified = value;
    }

    public Decimal EXCISE_BS
    {
      get => this.eXCISE_BSField;
      set => this.eXCISE_BSField = value;
    }

    [XmlIgnore]
    public bool EXCISE_BSSpecified
    {
      get => this.eXCISE_BSFieldSpecified;
      set => this.eXCISE_BSFieldSpecified = value;
    }

    public string EAN13
    {
      get => this.eAN13Field;
      set => this.eAN13Field = value;
    }

    public string FARegId
    {
      get => this.fARegIdField;
      set => this.fARegIdField = value;
    }

    public InformF2TypeItemBC InformF2
    {
      get => this.informF2Field;
      set => this.informF2Field = value;
    }

    [XmlArrayItem("boxtree", IsNullable = false)]
    public boxtype[] boxInfo
    {
      get => this.boxInfoField;
      set => this.boxInfoField = value;
    }
  }
}
