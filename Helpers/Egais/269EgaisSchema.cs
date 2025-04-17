// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformInvPositionType2
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
  [XmlType(TypeName = "InformInvPositionType", Namespace = "http://fsrar.ru/WEGAIS/ActInventoryInformF2Reg")]
  [Serializable]
  public class InformInvPositionType2
  {
    private string identityField;
    private string informF1RegIdField;
    private InformInvF2RegItem[] informF2Field;

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

    [XmlArrayItem("InformF2Item", IsNullable = false)]
    public InformInvF2RegItem[] InformF2
    {
      get => this.informF2Field;
      set => this.informF2Field = value;
    }
  }
}
