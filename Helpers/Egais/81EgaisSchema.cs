// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.Header
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ChequeV3")]
  [Serializable]
  public class Header
  {
    private DateTime dateField;
    private string kassaField;
    private string shiftField;
    private string numberField;
    private TYPE typeField;
    private string confirmOrderField;

    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    public string Kassa
    {
      get => this.kassaField;
      set => this.kassaField = value;
    }

    [XmlElement(DataType = "integer")]
    public string Shift
    {
      get => this.shiftField;
      set => this.shiftField = value;
    }

    [XmlElement(DataType = "integer")]
    public string Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    public TYPE Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    public string ConfirmOrder
    {
      get => this.confirmOrderField;
      set => this.confirmOrderField = value;
    }
  }
}
