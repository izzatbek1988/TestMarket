// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplySSP_v2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplySSP_v2")]
  [Serializable]
  public class ReplySSP_v2
  {
    private ProductInfoReply_v2[] productsField;

    [XmlArrayItem("Product", IsNullable = false)]
    public ProductInfoReply_v2[] Products
    {
      get => this.productsField;
      set => this.productsField = value;
    }
  }
}
