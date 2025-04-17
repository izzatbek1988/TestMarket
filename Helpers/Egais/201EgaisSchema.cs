// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.WayBillType_v3
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/TTNSingle_v3")]
  [Serializable]
  public class WayBillType_v3
  {
    private string identityField;
    private WayBillType_v3Header headerField;
    private PositionType6[] contentField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public WayBillType_v3Header Header
    {
      get => this.headerField;
      set => this.headerField = value;
    }

    [XmlArrayItem("Position", IsNullable = false)]
    public PositionType6[] Content
    {
      get => this.contentField;
      set => this.contentField = value;
    }
  }
}
