// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActChargeOnPositionType1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(TypeName = "ActChargeOnPositionType", Namespace = "http://fsrar.ru/WEGAIS/ActChargeOn_v2")]
  [Serializable]
  public class ActChargeOnPositionType1
  {
    private string identityField;
    private ProductInfo_v2 productField;
    private Decimal quantityField;
    private Decimal alcPercentField;
    private bool alcPercentFieldSpecified;
    private Decimal alcPercentMinField;
    private bool alcPercentMinFieldSpecified;
    private Decimal alcPercentMaxField;
    private bool alcPercentMaxFieldSpecified;
    private ActChargeOnPositionTypeInformF1F2 informF1F2Field;
    private string[] markCodeInfoField;

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

    public Decimal alcPercent
    {
      get => this.alcPercentField;
      set => this.alcPercentField = value;
    }

    [XmlIgnore]
    public bool alcPercentSpecified
    {
      get => this.alcPercentFieldSpecified;
      set => this.alcPercentFieldSpecified = value;
    }

    public Decimal alcPercentMin
    {
      get => this.alcPercentMinField;
      set => this.alcPercentMinField = value;
    }

    [XmlIgnore]
    public bool alcPercentMinSpecified
    {
      get => this.alcPercentMinFieldSpecified;
      set => this.alcPercentMinFieldSpecified = value;
    }

    public Decimal alcPercentMax
    {
      get => this.alcPercentMaxField;
      set => this.alcPercentMaxField = value;
    }

    [XmlIgnore]
    public bool alcPercentMaxSpecified
    {
      get => this.alcPercentMaxFieldSpecified;
      set => this.alcPercentMaxFieldSpecified = value;
    }

    public ActChargeOnPositionTypeInformF1F2 InformF1F2
    {
      get => this.informF1F2Field;
      set => this.informF1F2Field = value;
    }

    [XmlArrayItem("MarkCode", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
    public string[] MarkCodeInfo
    {
      get => this.markCodeInfoField;
      set => this.markCodeInfoField = value;
    }
  }
}
