// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformF1RegType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ActInventoryF1F2Info")]
  [Serializable]
  public class InformF1RegType
  {
    private Decimal quantityField;
    private DateTime bottlingDateField;
    private string tTNNumberField;
    private DateTime tTNDateField;
    private string eGAISFixNumberField;
    private DateTime eGAISFixDateField;
    private bool eGAISFixDateFieldSpecified;

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime BottlingDate
    {
      get => this.bottlingDateField;
      set => this.bottlingDateField = value;
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

    public string EGAISFixNumber
    {
      get => this.eGAISFixNumberField;
      set => this.eGAISFixNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime EGAISFixDate
    {
      get => this.eGAISFixDateField;
      set => this.eGAISFixDateField = value;
    }

    [XmlIgnore]
    public bool EGAISFixDateSpecified
    {
      get => this.eGAISFixDateFieldSpecified;
      set => this.eGAISFixDateFieldSpecified = value;
    }
  }
}
