// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/TTNSingle")]
  [Serializable]
  public class PositionType
  {
    private ProductInfo productField;
    private string pack_IDField;
    private Decimal quantityField;
    private Decimal priceField;
    private string partyField;
    private string identityField;
    private InformAType informAField;
    private InformBType informBField;

    [XmlIgnore]
    public Guid UidGoodInDb { get; set; }

    public ProductInfo Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public string Pack_ID
    {
      get => this.pack_IDField;
      set => this.pack_IDField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public Decimal Price
    {
      get => this.priceField;
      set => this.priceField = value;
    }

    public string Party
    {
      get => this.partyField;
      set => this.partyField = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public InformAType InformA
    {
      get => this.informAField;
      set => this.informAField = value;
    }

    public InformBType InformB
    {
      get => this.informBField;
      set => this.informBField = value;
    }
  }
}
