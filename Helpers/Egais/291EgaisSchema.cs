// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyRests_v2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyRests_v2")]
  [Serializable]
  public class ReplyRests_v2
  {
    private DateTime restsDateField;
    private StockPositionType1[] productsField;

    public DateTime RestsDate
    {
      get => this.restsDateField;
      set => this.restsDateField = value;
    }

    [XmlArrayItem("StockPosition", IsNullable = false)]
    public StockPositionType1[] Products
    {
      get => this.productsField;
      set => this.productsField = value;
    }
  }
}
