// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WayBillTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/TTNSingle")]
  [Serializable]
  public class WayBillTypeHeader
  {
    private WbType typeField;
    private WbUnitType unitTypeField;
    private string nUMBERField;
    private DateTime dateField;
    private DateTime shippingDateField;
    private TransportType transportField;
    private OrgInfo shipperField;
    private OrgInfo consigneeField;
    private OrgInfo supplierField;
    private string baseField;
    private string noteField;

    public WayBillTypeHeader() => this.typeField = WbType.WBInvoiceFromMe;

    public WbType Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    public WbUnitType UnitType
    {
      get => this.unitTypeField;
      set => this.unitTypeField = value;
    }

    public string NUMBER
    {
      get => this.nUMBERField;
      set => this.nUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ShippingDate
    {
      get => this.shippingDateField;
      set => this.shippingDateField = value;
    }

    public TransportType Transport
    {
      get => this.transportField;
      set => this.transportField = value;
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

    public OrgInfo Supplier
    {
      get => this.supplierField;
      set => this.supplierField = value;
    }

    public string Base
    {
      get => this.baseField;
      set => this.baseField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
