// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ConfirmTicketTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ConfirmTicket")]
  [Serializable]
  public class ConfirmTicketTypeHeader
  {
    private ConclusionType1 isConfirmField;
    private string ticketNumberField;
    private DateTime ticketDateField;
    private string wBRegIdField;
    private string noteField;

    public ConclusionType1 IsConfirm
    {
      get => this.isConfirmField;
      set => this.isConfirmField = value;
    }

    public string TicketNumber
    {
      get => this.ticketNumberField;
      set => this.ticketNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime TicketDate
    {
      get => this.ticketDateField;
      set => this.ticketDateField = value;
    }

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
