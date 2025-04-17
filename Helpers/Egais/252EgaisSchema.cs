// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RepPIInformF1RegTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/RepInformF1Reg")]
  [Serializable]
  public class RepPIInformF1RegTypeHeader
  {
    private string identityField;
    private string repRegIdField;
    private OrgInfoRusReply_v2 clientField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string RepRegId
    {
      get => this.repRegIdField;
      set => this.repRegIdField = value;
    }

    public OrgInfoRusReply_v2 Client
    {
      get => this.clientField;
      set => this.clientField = value;
    }
  }
}
