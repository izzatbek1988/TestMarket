// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WayBillInformF2RegTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/TTNInformF2Reg")]
  [Serializable]
  public class WayBillInformF2RegTypeHeader
  {
    private string identityField;
    private string wBRegIdField;
    private string eGAISFixNumberField;
    private DateTime eGAISFixDateField;
    private bool eGAISFixDateFieldSpecified;
    private string wBNUMBERField;
    private DateTime wBDateField;
    private OrgInfoRus_v2 shipperField;
    private OrgInfo_v2 consigneeField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
    }

    public string EGAISFixNumber
    {
      get => this.eGAISFixNumberField;
      set => this.eGAISFixNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime EGAISFixDate
    {
      get => this.eGAISFixDateField;
      set => this.eGAISFixDateField = value;
    }

    [XmlIgnore]
    public bool EGAISFixDateSpecified
    {
      get => this.eGAISFixDateFieldSpecified;
      set => this.eGAISFixDateFieldSpecified = value;
    }

    public string WBNUMBER
    {
      get => this.wBNUMBERField;
      set => this.wBNUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime WBDate
    {
      get => this.wBDateField;
      set => this.wBDateField = value;
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
  }
}
