// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WayBillActTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActTTNSingle")]
  [Serializable]
  public class WayBillActTypeHeader
  {
    private AcceptType isAcceptField;
    private bool isAcceptFieldSpecified;
    private string aCTNUMBERField;
    private DateTime actDateField;
    private string wBRegIdField;
    private string noteField;

    public AcceptType IsAccept
    {
      get => this.isAcceptField;
      set => this.isAcceptField = value;
    }

    [XmlIgnore]
    public bool IsAcceptSpecified
    {
      get => this.isAcceptFieldSpecified;
      set => this.isAcceptFieldSpecified = value;
    }

    public string ACTNUMBER
    {
      get => this.aCTNUMBERField;
      set => this.aCTNUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ActDate
    {
      get => this.actDateField;
      set => this.actDateField = value;
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
