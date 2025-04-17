// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyForm1
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyForm1")]
  [Serializable]
  public class ReplyForm1
  {
    private string informF1RegIdField;
    private OrgInfoRusReply_v2 originalClientField;
    private string originalDocNumberField;
    private DateTime originalDocDateField;
    private ProductInfoReply_v2 productField;
    private DateTime bottlingDateField;
    private bool bottlingDateFieldSpecified;
    private Decimal quantityField;
    private string eGAISNumberField;
    private DateTime eGAISDateField;
    private string gTDNUMBERField;
    private DateTime gTDDateField;
    private bool gTDDateFieldSpecified;
    private MarkInfoType2 markInfoField;

    public string InformF1RegId
    {
      get => this.informF1RegIdField;
      set => this.informF1RegIdField = value;
    }

    public OrgInfoRusReply_v2 OriginalClient
    {
      get => this.originalClientField;
      set => this.originalClientField = value;
    }

    public string OriginalDocNumber
    {
      get => this.originalDocNumberField;
      set => this.originalDocNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime OriginalDocDate
    {
      get => this.originalDocDateField;
      set => this.originalDocDateField = value;
    }

    public ProductInfoReply_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime BottlingDate
    {
      get => this.bottlingDateField;
      set => this.bottlingDateField = value;
    }

    [XmlIgnore]
    public bool BottlingDateSpecified
    {
      get => this.bottlingDateFieldSpecified;
      set => this.bottlingDateFieldSpecified = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public string EGAISNumber
    {
      get => this.eGAISNumberField;
      set => this.eGAISNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime EGAISDate
    {
      get => this.eGAISDateField;
      set => this.eGAISDateField = value;
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

    public MarkInfoType2 MarkInfo
    {
      get => this.markInfoField;
      set => this.markInfoField = value;
    }
  }
}
