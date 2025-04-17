// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WayBillType_v2Header
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/TTNSingle_v2")]
  [Serializable]
  public class WayBillType_v2Header
  {
    private WbType1 typeField;
    private string nUMBERField;
    private DateTime dateField;
    private DateTime shippingDateField;
    private TransportType1 transportField;
    private OrgInfoRus_v2 shipperField;
    private OrgInfo_v2 consigneeField;
    private string baseField;
    private string noteField;
    private string varField1Field;
    private string varField2Field;
    private string varField3Field;

    public WayBillType_v2Header() => this.typeField = WbType1.WBInvoiceFromMe;

    public WbType1 Type
    {
      get => this.typeField;
      set => this.typeField = value;
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

    public TransportType1 Transport
    {
      get => this.transportField;
      set => this.transportField = value;
    }

    public OrgInfoRus_v2 Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }

    public OrgInfo_v2 Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
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

    public string VarField1
    {
      get => this.varField1Field;
      set => this.varField1Field = value;
    }

    public string VarField2
    {
      get => this.varField2Field;
      set => this.varField2Field = value;
    }

    public string VarField3
    {
      get => this.varField3Field;
      set => this.varField3Field = value;
    }
  }
}
