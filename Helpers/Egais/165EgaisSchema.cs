// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionType10
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
  [XmlType(TypeName = "PositionType", Namespace = "http://fsrar.ru/WEGAIS/CarrierNotice")]
  [Serializable]
  public class PositionType10
  {
    private string posIdentityField;
    private ProductInfo_v2 productField;
    private Decimal quantity20Field;
    private Decimal alcPerc20Field;

    public string PosIdentity
    {
      get => this.posIdentityField;
      set => this.posIdentityField = value;
    }

    public ProductInfo_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public Decimal Quantity20
    {
      get => this.quantity20Field;
      set => this.quantity20Field = value;
    }

    public Decimal AlcPerc20
    {
      get => this.alcPerc20Field;
      set => this.alcPerc20Field = value;
    }
  }
}
