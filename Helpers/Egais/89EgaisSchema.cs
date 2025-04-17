// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.F2DetailType
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
  public class F2DetailType
  {
    private InformF2TypeItemBC informF2Field;
    private Decimal quantityF2Field;
    private Decimal priceField;

    public InformF2TypeItemBC InformF2
    {
      get => this.informF2Field;
      set => this.informF2Field = value;
    }

    public Decimal QuantityF2
    {
      get => this.quantityF2Field;
      set => this.quantityF2Field = value;
    }

    public Decimal Price
    {
      get => this.priceField;
      set => this.priceField = value;
    }
  }
}
