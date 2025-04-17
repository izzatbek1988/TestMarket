// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActWOSPosType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAdjustmentData")]
  [Serializable]
  public class ActWOSPosType
  {
    private string aWOSRegIdField;
    private string identityField;
    private Decimal sumSaleField;
    private bool sumSaleFieldSpecified;

    public string AWOSRegId
    {
      get => this.aWOSRegIdField;
      set => this.aWOSRegIdField = value;
    }

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public Decimal SumSale
    {
      get => this.sumSaleField;
      set => this.sumSaleField = value;
    }

    [XmlIgnore]
    public bool SumSaleSpecified
    {
      get => this.sumSaleFieldSpecified;
      set => this.sumSaleFieldSpecified = value;
    }
  }
}
