// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyBarcode
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyBarcode")]
  [Serializable]
  public class ReplyBarcode
  {
    private string queryNumberField;
    private DateTime dateField;
    private BarcodeType[] marksField;

    public string QueryNumber
    {
      get => this.queryNumberField;
      set => this.queryNumberField = value;
    }

    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    [XmlArrayItem("Mark", IsNullable = false)]
    public BarcodeType[] Marks
    {
      get => this.marksField;
      set => this.marksField = value;
    }
  }
}
