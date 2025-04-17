// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TicketType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/Ticket")]
  [Serializable]
  public class TicketType
  {
    private DateTime ticketDateField;
    private bool ticketDateFieldSpecified;
    private string identityField;
    private string docIdField;
    private string transportIdField;
    private string regIDField;
    private string docHashField;
    private string docTypeField;
    private TicketResultType resultField;
    private OperationResultType operationResultField;

    public DateTime TicketDate
    {
      get => this.ticketDateField;
      set => this.ticketDateField = value;
    }

    [XmlIgnore]
    public bool TicketDateSpecified
    {
      get => this.ticketDateFieldSpecified;
      set => this.ticketDateFieldSpecified = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string DocId
    {
      get => this.docIdField;
      set => this.docIdField = value;
    }

    public string TransportId
    {
      get => this.transportIdField;
      set => this.transportIdField = value;
    }

    public string RegID
    {
      get => this.regIDField;
      set => this.regIDField = value;
    }

    public string DocHash
    {
      get => this.docHashField;
      set => this.docHashField = value;
    }

    public string DocType
    {
      get => this.docTypeField;
      set => this.docTypeField = value;
    }

    public TicketResultType Result
    {
      get => this.resultField;
      set => this.resultField = value;
    }

    public OperationResultType OperationResult
    {
      get => this.operationResultField;
      set => this.operationResultField = value;
    }
  }
}
