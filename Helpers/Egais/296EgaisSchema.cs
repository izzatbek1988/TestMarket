// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OrgInfoEx_v2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClientRef_v2")]
  [Serializable]
  public class OrgInfoEx_v2
  {
    private string identityField;
    private OrgInfoReply_v2 orgInfoV2Field;
    private OrgAddressType1[] addresslistField;
    private string stateField;
    private string versionWBField;
    private bool? isLicenseField;
    private bool isLicenseFieldSpecified;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public OrgInfoReply_v2 OrgInfoV2
    {
      get => this.orgInfoV2Field;
      set => this.orgInfoV2Field = value;
    }

    [XmlArrayItem("address", IsNullable = false)]
    public OrgAddressType1[] addresslist
    {
      get => this.addresslistField;
      set => this.addresslistField = value;
    }

    [XmlElement(IsNullable = true)]
    public string State
    {
      get => this.stateField;
      set => this.stateField = value;
    }

    [XmlElement(IsNullable = true)]
    public string VersionWB
    {
      get => this.versionWBField;
      set => this.versionWBField = value;
    }

    [XmlElement(IsNullable = true)]
    public bool? isLicense
    {
      get => this.isLicenseField;
      set => this.isLicenseField = value;
    }

    [XmlIgnore]
    public bool isLicenseSpecified
    {
      get => this.isLicenseFieldSpecified;
      set => this.isLicenseFieldSpecified = value;
    }
  }
}
