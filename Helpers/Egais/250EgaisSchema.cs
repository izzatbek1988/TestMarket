// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformF1PositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RepInformF1Reg")]
  [Serializable]
  public class InformF1PositionType
  {
    private string identityField;
    private string informF1RegIdField;
    private string informF2RegIdField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string InformF1RegId
    {
      get => this.informF1RegIdField;
      set => this.informF1RegIdField = value;
    }

    public string InformF2RegId
    {
      get => this.informF2RegIdField;
      set => this.informF2RegIdField = value;
    }
  }
}
