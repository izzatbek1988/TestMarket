// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyForm2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyForm2")]
  [Serializable]
  public class ReplyForm2
  {
    private string informF2RegIdField;
    private string tTNNumberField;
    private DateTime tTNDateField;
    private OrgInfoRusReply_v2 shipperField;
    private OrgInfoReply_v2 consigneeField;
    private DateTime shippingDateField;
    private bool shippingDateFieldSpecified;
    private ProductInfoReply_v2 productField;
    private Decimal quantityField;
    private Decimal exciseRateField;
    private bool exciseRateFieldSpecified;
    private MarkInfoType2 markInfoField;

    public string InformF2RegId
    {
      get => this.informF2RegIdField;
      set => this.informF2RegIdField = value;
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

    public OrgInfoRusReply_v2 Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }

    public OrgInfoReply_v2 Consignee
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

    public MarkInfoType2 MarkInfo
    {
      get => this.markInfoField;
      set => this.markInfoField = value;
    }
  }
}
