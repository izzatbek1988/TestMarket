// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OperationBType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyHistFormB")]
  [Serializable]
  public class OperationBType
  {
    private string docTypeField;
    private string docIdField;
    private string operationField;
    private Decimal quantityField;
    private DateTime operDateField;

    public string DocType
    {
      get => this.docTypeField;
      set => this.docTypeField = value;
    }

    public string DocId
    {
      get => this.docIdField;
      set => this.docIdField = value;
    }

    public string Operation
    {
      get => this.operationField;
      set => this.operationField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public DateTime OperDate
    {
      get => this.operDateField;
      set => this.operDateField = value;
    }
  }
}
