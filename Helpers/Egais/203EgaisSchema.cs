// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.DataType2
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
  [XmlType(TypeName = "DataTypeEgais", Namespace = "http://fsrar.ru/WEGAIS/AscpNavigation")]
  [Serializable]
  public class DataType2
  {
    private string numberField;
    private Decimal readingsField;
    private Decimal temperatureField;
    private Decimal densityField;

    [XmlElement(DataType = "integer")]
    public string Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    public Decimal Readings
    {
      get => this.readingsField;
      set => this.readingsField = value;
    }

    public Decimal Temperature
    {
      get => this.temperatureField;
      set => this.temperatureField = value;
    }

    public Decimal Density
    {
      get => this.densityField;
      set => this.densityField = value;
    }
  }
}
