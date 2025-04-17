// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TTNPosType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAdjustmentData")]
  [Serializable]
  public class TTNPosType
  {
    private string wBRegIdField;
    private string identityField;
    private Decimal priceField;
    private bool priceFieldSpecified;
    private string eXCISE_NUMBERField;
    private DateTime eXCISE_DATEField;
    private bool eXCISE_DATEFieldSpecified;
    private Decimal eXCISE_SUMField;
    private bool eXCISE_SUMFieldSpecified;
    private Decimal eXCISE_BSField;
    private bool eXCISE_BSFieldSpecified;

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public Decimal Price
    {
      get => this.priceField;
      set => this.priceField = value;
    }

    [XmlIgnore]
    public bool PriceSpecified
    {
      get => this.priceFieldSpecified;
      set => this.priceFieldSpecified = value;
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
  }
}
