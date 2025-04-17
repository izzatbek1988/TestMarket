// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OrgInfoEx
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClientRef")]
  [Serializable]
  public class OrgInfoEx
  {
    private string identityField;
    private string clientRegIdField;
    private string fullNameField;
    private string shortNameField;
    private string iNNField;
    private string kPPField;
    private string uNPField;
    private string rNNField;
    private OrgAddressType addressField;
    private OrgAddressType[] addresslistField;
    private string stateField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

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

    public string ShortName
    {
      get => this.shortNameField;
      set => this.shortNameField = value;
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

    [XmlElement(IsNullable = true)]
    public string UNP
    {
      get => this.uNPField;
      set => this.uNPField = value;
    }

    [XmlElement(IsNullable = true)]
    public string RNN
    {
      get => this.rNNField;
      set => this.rNNField = value;
    }

    public OrgAddressType address
    {
      get => this.addressField;
      set => this.addressField = value;
    }

    [XmlArrayItem("address", IsNullable = false)]
    public OrgAddressType[] addresslist
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
  }
}
