// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ULType_ClaimIssue
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
  public class ULType_ClaimIssue
  {
    private string clientRegIdField;
    private string fullNameField;
    private string iNNField;
    private string kPPField;
    private OrgUrAddressTypeULFL address_urField;
    private OrgAddressTypeULFL addressField;

    public string ClientRegId
    {
      get => this.clientRegIdField;
      set => this.clientRegIdField = value;
    }

    public string FullName
    {
      get => this.fullNameField;
      set => this.fullNameField = value;
    }

    [XmlElement(IsNullable = true)]
    public string INN
    {
      get => this.iNNField;
      set => this.iNNField = value;
    }

    public string KPP
    {
      get => this.kPPField;
      set => this.kPPField = value;
    }

    public OrgUrAddressTypeULFL address_ur
    {
      get => this.address_urField;
      set => this.address_urField = value;
    }

    public OrgAddressTypeULFL address
    {
      get => this.addressField;
      set => this.addressField = value;
    }
  }
}
