// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReportProducedType
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
  public class ReportProducedType
  {
    private string regIdField;
    private string nUMBERField;
    private DateTime dateField;
    private bool dateFieldSpecified;
    private DateTime producedDateField;
    private bool producedDateFieldSpecified;

    public string RegId
    {
      get => this.regIdField;
      set => this.regIdField = value;
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
    public DateTime ProducedDate
    {
      get => this.producedDateField;
      set => this.producedDateField = value;
    }

    [XmlIgnore]
    public bool ProducedDateSpecified
    {
      get => this.producedDateFieldSpecified;
      set => this.producedDateFieldSpecified = value;
    }
  }
}
