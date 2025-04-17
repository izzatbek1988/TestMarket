// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyFormB
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyFormB")]
  [Serializable]
  public class ReplyFormB
  {
    private string informBRegIdField;
    private string tTNNumberField;
    private DateTime tTNDateField;
    private OrgInfo shipperField;
    private OrgInfo consigneeField;
    private DateTime shippingDateField;
    private bool shippingDateFieldSpecified;
    private ProductInfo productField;
    private Decimal quantityField;
    private Decimal exciseRateField;
    private bool exciseRateFieldSpecified;
    private MarkInfoType markInfoField;

    public string InformBRegId
    {
      get => this.informBRegIdField;
      set => this.informBRegIdField = value;
    }

    public string TTNNumber
    {
      get => this.tTNNumberField;
      set => this.tTNNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime TTNDate
    {
      get => this.tTNDateField;
      set => this.tTNDateField = value;
    }

    public OrgInfo Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }

    public OrgInfo Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ShippingDate
    {
      get => this.shippingDateField;
      set => this.shippingDateField = value;
    }

    [XmlIgnore]
    public bool ShippingDateSpecified
    {
      get => this.shippingDateFieldSpecified;
      set => this.shippingDateFieldSpecified = value;
    }

    public ProductInfo Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public Decimal ExciseRate
    {
      get => this.exciseRateField;
      set => this.exciseRateField = value;
    }

    [XmlIgnore]
    public bool ExciseRateSpecified
    {
      get => this.exciseRateFieldSpecified;
      set => this.exciseRateFieldSpecified = value;
    }

    public MarkInfoType MarkInfo
    {
      get => this.markInfoField;
      set => this.markInfoField = value;
    }
  }
}
