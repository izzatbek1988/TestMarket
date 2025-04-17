// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.UsedResourceType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RepProducedProduct")]
  [Serializable]
  public class UsedResourceType
  {
    private string identityResField;
    private ProductInfo_v2 productField;
    private string regForm2Field;
    private Decimal quantityField;

    public string IdentityRes
    {
      get => this.identityResField;
      set => this.identityResField = value;
    }

    public ProductInfo_v2 Product
    {
      get => this.productField;
      set => this.productField = value;
    }

    public string RegForm2
    {
      get => this.regForm2Field;
      set => this.regForm2Field = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }
  }
}
