// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActCOType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAdjustmentData")]
  [Serializable]
  public class ActCOType
  {
    private string aCORegIdField;
    private string numberField;
    private DateTime actDateField;

    public string ACORegId
    {
      get => this.aCORegIdField;
      set => this.aCORegIdField = value;
    }

    public string Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ActDate
    {
      get => this.actDateField;
      set => this.actDateField = value;
    }
  }
}
