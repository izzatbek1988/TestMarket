// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActConfirmOnlineOrderPositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ActConfirmOnlineOrder")]
  [Serializable]
  public class ActConfirmOnlineOrderPositionType
  {
    private string identityField;
    private ProductInfo_v2 productField;
    private Decimal quantityField;
    private Decimal sumSaleField;
    private F2DetailType[] f2DetailField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public ProductInfo_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public Decimal SumSale
    {
      get => this.sumSaleField;
      set => this.sumSaleField = value;
    }

    [XmlArrayItem("Form2", IsNullable = false)]
    public F2DetailType[] F2Detail
    {
      get => this.f2DetailField;
      set => this.f2DetailField = value;
    }
  }
}
