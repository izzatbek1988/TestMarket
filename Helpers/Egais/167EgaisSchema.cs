// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.CarrierNoticeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/CarrierNotice")]
  [Serializable]
  public class CarrierNoticeHeader
  {
    private string clientIdentityField;
    private string serialField;
    private OrgInfo_v2 shipperField;
    private OrgInfo_v2 consigneeField;
    private OrgInfoRus_v2 carrierField;
    private OrgInfoRus_v2 clientTransportField;
    private DateTime shipmentOutDateField;
    private DateTime shipmentInDateField;
    private string eGAISFixNumberTTNField;
    private string notifNumberField;
    private DateTime notifDateField;

    public string ClientIdentity
    {
      get => this.clientIdentityField;
      set => this.clientIdentityField = value;
    }

    public string Serial
    {
      get => this.serialField;
      set => this.serialField = value;
    }

    public OrgInfo_v2 Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }

    public OrgInfo_v2 Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
    }

    public OrgInfoRus_v2 Carrier
    {
      get => this.carrierField;
      set => this.carrierField = value;
    }

    public OrgInfoRus_v2 ClientTransport
    {
      get => this.clientTransportField;
      set => this.clientTransportField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ShipmentOutDate
    {
      get => this.shipmentOutDateField;
      set => this.shipmentOutDateField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ShipmentInDate
    {
      get => this.shipmentInDateField;
      set => this.shipmentInDateField = value;
    }

    public string EGAISFixNumberTTN
    {
      get => this.eGAISFixNumberTTNField;
      set => this.eGAISFixNumberTTNField = value;
    }

    public string NotifNumber
    {
      get => this.notifNumberField;
      set => this.notifNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime NotifDate
    {
      get => this.notifDateField;
      set => this.notifDateField = value;
    }
  }
}
