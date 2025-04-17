// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RequestAddSSPPositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAddProducts")]
  [Serializable]
  public class RequestAddSSPPositionType
  {
    private string producerField;
    private ProductType1 typeField;
    private string vidCodeField;
    private string countryCodeField;
    private string fullNameField;
    private string shortNameField;
    private UnpackedType unpacked_FlagField;
    private Decimal capacityField;
    private bool capacityFieldSpecified;
    private Decimal pERCENT_ALCField;
    private Decimal pERCENT_ALC_minField;
    private bool pERCENT_ALC_minFieldSpecified;
    private Decimal pERCENT_ALC_maxField;
    private bool pERCENT_ALC_maxFieldSpecified;
    private string fRAPIDField;
    private string brandField;
    private string packageTypeField;

    public string Producer
    {
      get => this.producerField;
      set => this.producerField = value;
    }

    public ProductType1 Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    public string VidCode
    {
      get => this.vidCodeField;
      set => this.vidCodeField = value;
    }

    public string CountryCode
    {
      get => this.countryCodeField;
      set => this.countryCodeField = value;
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

    public UnpackedType Unpacked_Flag
    {
      get => this.unpacked_FlagField;
      set => this.unpacked_FlagField = value;
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

    public Decimal PERCENT_ALC
    {
      get => this.pERCENT_ALCField;
      set => this.pERCENT_ALCField = value;
    }

    public Decimal PERCENT_ALC_min
    {
      get => this.pERCENT_ALC_minField;
      set => this.pERCENT_ALC_minField = value;
    }

    [XmlIgnore]
    public bool PERCENT_ALC_minSpecified
    {
      get => this.pERCENT_ALC_minFieldSpecified;
      set => this.pERCENT_ALC_minFieldSpecified = value;
    }

    public Decimal PERCENT_ALC_max
    {
      get => this.pERCENT_ALC_maxField;
      set => this.pERCENT_ALC_maxField = value;
    }

    [XmlIgnore]
    public bool PERCENT_ALC_maxSpecified
    {
      get => this.pERCENT_ALC_maxFieldSpecified;
      set => this.pERCENT_ALC_maxFieldSpecified = value;
    }

    public string FRAPID
    {
      get => this.fRAPIDField;
      set => this.fRAPIDField = value;
    }

    public string Brand
    {
      get => this.brandField;
      set => this.brandField = value;
    }

    public string PackageType
    {
      get => this.packageTypeField;
      set => this.packageTypeField = value;
    }
  }
}
