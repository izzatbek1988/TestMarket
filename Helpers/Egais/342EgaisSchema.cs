// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformBTypeRegItem
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ActInventoryABInfo")]
  [Serializable]
  public class InformBTypeRegItem
  {
    private string identityField;
    private string tTNNumberField;
    private DateTime tTNDateField;
    private Decimal quantityField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string TTNNumber
    {
      get => this.tTNNumberField;
      set => this.tTNNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime TTNDate
    {
      get => this.tTNDateField;
      set => this.tTNDateField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }
  }
}
