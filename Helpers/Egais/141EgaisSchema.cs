// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReportImportedType
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
  public class ReportImportedType
  {
    private string regIdField;
    private string nUMBERField;
    private DateTime dateField;
    private bool dateFieldSpecified;
    private DateTime importedDateField;
    private bool importedDateFieldSpecified;
    private string gTDNUMBERField;
    private DateTime gTDDateField;
    private bool gTDDateFieldSpecified;
    private string countryField;
    private string supplierOwneridField;

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
    public DateTime ImportedDate
    {
      get => this.importedDateField;
      set => this.importedDateField = value;
    }

    [XmlIgnore]
    public bool ImportedDateSpecified
    {
      get => this.importedDateFieldSpecified;
      set => this.importedDateFieldSpecified = value;
    }

    public string GTDNUMBER
    {
      get => this.gTDNUMBERField;
      set => this.gTDNUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime GTDDate
    {
      get => this.gTDDateField;
      set => this.gTDDateField = value;
    }

    [XmlIgnore]
    public bool GTDDateSpecified
    {
      get => this.gTDDateFieldSpecified;
      set => this.gTDDateFieldSpecified = value;
    }

    public string Country
    {
      get => this.countryField;
      set => this.countryField = value;
    }

    public string supplierOwnerid
    {
      get => this.supplierOwneridField;
      set => this.supplierOwneridField = value;
    }
  }
}
