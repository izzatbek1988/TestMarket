// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OrgAddressTypeULFLReply
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
  public class OrgAddressTypeULFLReply
  {
    private string countryField;
    private string regionCodeField;
    private string descriptionField;

    public string Country
    {
      get => this.countryField;
      set => this.countryField = value;
    }

    public string RegionCode
    {
      get => this.regionCodeField;
      set => this.regionCodeField = value;
    }

    public string description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }
  }
}
