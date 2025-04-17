// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OrgAddressType1
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
  [XmlType(TypeName = "OrgAddressType", Namespace = "http://fsrar.ru/WEGAIS/ClientRef_v2")]
  [Serializable]
  public class OrgAddressType1
  {
    private string countryField;
    private string indexField;
    private string regionCodeField;
    private string areaField;
    private string cityField;
    private string placeField;
    private string streetField;
    private string houseField;
    private string buildingField;
    private string literField;
    private string descriptionField;

    public string Country
    {
      get => this.countryField;
      set => this.countryField = value;
    }

    public string Index
    {
      get => this.indexField;
      set => this.indexField = value;
    }

    public string RegionCode
    {
      get => this.regionCodeField;
      set => this.regionCodeField = value;
    }

    public string area
    {
      get => this.areaField;
      set => this.areaField = value;
    }

    public string city
    {
      get => this.cityField;
      set => this.cityField = value;
    }

    public string place
    {
      get => this.placeField;
      set => this.placeField = value;
    }

    public string street
    {
      get => this.streetField;
      set => this.streetField = value;
    }

    public string house
    {
      get => this.houseField;
      set => this.houseField = value;
    }

    public string building
    {
      get => this.buildingField;
      set => this.buildingField = value;
    }

    public string liter
    {
      get => this.literField;
      set => this.literField = value;
    }

    public string description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }
  }
}
