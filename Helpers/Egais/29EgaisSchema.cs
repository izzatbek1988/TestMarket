// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ProductInfoRus_v2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ProductRef_v2")]
  [Serializable]
  public class ProductInfoRus_v2
  {
    private WbUnitType1 unitTypeField;
    private ProductType1 typeField;
    private bool typeFieldSpecified;
    private string fullNameField;
    private string shortNameField;
    private string alcCodeField;
    private Decimal capacityField;
    private bool capacityFieldSpecified;
    private Decimal alcVolumeField;
    private bool alcVolumeFieldSpecified;
    private OrgInfoRus_v2 producerField;
    private string productVCodeField;

    public WbUnitType1 UnitType
    {
      get => this.unitTypeField;
      set => this.unitTypeField = value;
    }

    public ProductType1 Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    [XmlIgnore]
    public bool TypeSpecified
    {
      get => this.typeFieldSpecified;
      set => this.typeFieldSpecified = value;
    }

    public string FullName
    {
      get => this.fullNameField;
      set => this.fullNameField = value;
    }

    public string ShortName
    {
      get => this.shortNameField;
      set => this.shortNameField = value;
    }

    public string AlcCode
    {
      get => this.alcCodeField;
      set => this.alcCodeField = value;
    }

    public Decimal Capacity
    {
      get => this.capacityField;
      set => this.capacityField = value;
    }

    [XmlIgnore]
    public bool CapacitySpecified
    {
      get => this.capacityFieldSpecified;
      set => this.capacityFieldSpecified = value;
    }

    public Decimal AlcVolume
    {
      get => this.alcVolumeField;
      set => this.alcVolumeField = value;
    }

    [XmlIgnore]
    public bool AlcVolumeSpecified
    {
      get => this.alcVolumeFieldSpecified;
      set => this.alcVolumeFieldSpecified = value;
    }

    public OrgInfoRus_v2 Producer
    {
      get => this.producerField;
      set => this.producerField = value;
    }

    public string ProductVCode
    {
      get => this.productVCodeField;
      set => this.productVCodeField = value;
    }
  }
}
