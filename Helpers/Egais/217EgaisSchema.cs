// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.StockPositionType2
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
  [XmlType(TypeName = "StockPositionType", Namespace = "http://fsrar.ru/WEGAIS/ReplyRests_Mini")]
  [Serializable]
  public class StockPositionType2
  {
    private string alcCodeField;
    private Decimal quantityField;
    private string inform1RegIdField;
    private string inform2RegIdField;

    public string AlcCode
    {
      get => this.alcCodeField;
      set => this.alcCodeField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public string Inform1RegId
    {
      get => this.inform1RegIdField;
      set => this.inform1RegIdField = value;
    }

    public string Inform2RegId
    {
      get => this.inform2RegIdField;
      set => this.inform2RegIdField = value;
    }
  }
}
