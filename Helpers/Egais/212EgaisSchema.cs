// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.AddFOType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAddFProducer")]
  [Serializable]
  public class AddFOType
  {
    private string localClientCodeField;
    private string fullNameField;
    private string shortNameField;
    private OrgAddressTypeFOTS addressField;

    public string LocalClientCode
    {
      get => this.localClientCodeField;
      set => this.localClientCodeField = value;
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

    public OrgAddressTypeFOTS address
    {
      get => this.addressField;
      set => this.addressField = value;
    }
  }
}
