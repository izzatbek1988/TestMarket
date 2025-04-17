// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionInvoiceIssueAMType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/InvoiceIssueAM")]
  [Serializable]
  public class PositionInvoiceIssueAMType
  {
    private string identityField;
    private string markTypeField;
    private string markTypeCodeField;
    private string rankField;
    private string startField;
    private string lastField;
    private string quantityRangeField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string MarkType
    {
      get => this.markTypeField;
      set => this.markTypeField = value;
    }

    public string MarkTypeCode
    {
      get => this.markTypeCodeField;
      set => this.markTypeCodeField = value;
    }

    public string Rank
    {
      get => this.rankField;
      set => this.rankField = value;
    }

    public string Start
    {
      get => this.startField;
      set => this.startField = value;
    }

    public string Last
    {
      get => this.lastField;
      set => this.lastField = value;
    }

    [XmlElement(DataType = "integer")]
    public string QuantityRange
    {
      get => this.quantityRangeField;
      set => this.quantityRangeField = value;
    }
  }
}
