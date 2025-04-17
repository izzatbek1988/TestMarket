// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TTNIssueFSMTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/TTNIssueFSM")]
  [Serializable]
  public class TTNIssueFSMTypeHeader
  {
    private string nUMBERField;
    private DateTime dateField;
    private OrgInfoRus_ClaimIssue senderField;
    private OrgInfoRus_ClaimIssue consigneeField;
    private string requestFSMField;
    private string manufacturerShortNameField;
    private DateTime actualShipmentDateField;
    private string totalQuantityField;

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

    public OrgInfoRus_ClaimIssue Sender
    {
      get => this.senderField;
      set => this.senderField = value;
    }

    public OrgInfoRus_ClaimIssue Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
    }

    public string RequestFSM
    {
      get => this.requestFSMField;
      set => this.requestFSMField = value;
    }

    public string ManufacturerShortName
    {
      get => this.manufacturerShortNameField;
      set => this.manufacturerShortNameField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ActualShipmentDate
    {
      get => this.actualShipmentDateField;
      set => this.actualShipmentDateField = value;
    }

    [XmlElement(DataType = "integer")]
    public string TotalQuantity
    {
      get => this.totalQuantityField;
      set => this.totalQuantityField = value;
    }
  }
}
