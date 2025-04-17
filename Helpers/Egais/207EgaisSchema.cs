// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.DocDataType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyHistoryShop")]
  [Serializable]
  public class DocDataType
  {
    private string docTypeField;
    private string docIdField;
    private DateTime operDateField;
    private Decimal quantityField;
    private string regForm2Field;

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

    public DateTime OperDate
    {
      get => this.operDateField;
      set => this.operDateField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public string RegForm2
    {
      get => this.regForm2Field;
      set => this.regForm2Field = value;
    }
  }
}
