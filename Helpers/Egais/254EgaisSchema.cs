// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TransferToShopPositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/TransferToShop")]
  [Serializable]
  public class TransferToShopPositionType
  {
    private string identityField;
    private string productCodeField;
    private Decimal quantityField;
    private InformF2TypeItem informF2Field;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string ProductCode
    {
      get => this.productCodeField;
      set => this.productCodeField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public InformF2TypeItem InformF2
    {
      get => this.informF2Field;
      set => this.informF2Field = value;
    }
  }
}
