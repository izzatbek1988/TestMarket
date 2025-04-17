// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TRType
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
  public class TRType
  {
    private string clientRegIdField;
    private OrgAddressTypeTR addressField;

    public string ClientRegId
    {
      get => this.clientRegIdField;
      set => this.clientRegIdField = value;
    }

    public OrgAddressTypeTR address
    {
      get => this.addressField;
      set => this.addressField = value;
    }
  }
}
