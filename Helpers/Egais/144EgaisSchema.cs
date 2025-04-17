// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TTNType
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
  public class TTNType
  {
    private string wBRegIdField;
    private string nUMBERField;
    private DateTime dateField;
    private bool dateFieldSpecified;
    private DateTime shippingDateField;
    private bool shippingDateFieldSpecified;
    private WbType3 typeField;
    private bool typeFieldSpecified;
    private TransportType3 transportField;

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
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

    [XmlIgnore]
    public bool DateSpecified
    {
      get => this.dateFieldSpecified;
      set => this.dateFieldSpecified = value;
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

    public WbType3 Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlIgnore]
    public bool TypeSpecified
    {
      get => this.typeFieldSpecified;
      set => this.typeFieldSpecified = value;
    }

    public TransportType3 Transport
    {
      get => this.transportField;
      set => this.transportField = value;
    }
  }
}
