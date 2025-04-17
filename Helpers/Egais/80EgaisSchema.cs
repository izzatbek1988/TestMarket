// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.HeaderTTN
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
  public class HeaderTTN
  {
    private DateTime dateField;
    private string billNumberField;
    private string tTNNumberField;
    private TYPE typeField;

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    public string BillNumber
    {
      get => this.billNumberField;
      set => this.billNumberField = value;
    }

    public string TTNNumber
    {
      get => this.tTNNumberField;
      set => this.tTNNumberField = value;
    }

    public TYPE Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }
  }
}
