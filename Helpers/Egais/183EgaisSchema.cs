// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RepImportedType_v3Header
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/RepImportedProduct_v3")]
  [Serializable]
  public class RepImportedType_v3Header
  {
    private string nUMBERField;
    private DateTime dateField;
    private DateTime importedDateField;
    private OrgInfoRus_v2 importerField;
    private OrgInfoForeign_v2 supplierField;
    private string gTDNUMBERField;
    private DateTime gTDDateField;
    private string contractNUMBERField;
    private DateTime contractDateField;
    private string countryField;
    private string noteField;

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
    public DateTime ImportedDate
    {
      get => this.importedDateField;
      set => this.importedDateField = value;
    }

    public OrgInfoRus_v2 Importer
    {
      get => this.importerField;
      set => this.importerField = value;
    }

    public OrgInfoForeign_v2 Supplier
    {
      get => this.supplierField;
      set => this.supplierField = value;
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

    public string ContractNUMBER
    {
      get => this.contractNUMBERField;
      set => this.contractNUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ContractDate
    {
      get => this.contractDateField;
      set => this.contractDateField = value;
    }

    public string Country
    {
      get => this.countryField;
      set => this.countryField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
