// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformF2TypeItem
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ProductRef_v2")]
  [Serializable]
  public class InformF2TypeItem
  {
    private string f2RegIdField;
    private MarkInfoType2 markInfoField;

    public InformF2TypeItem() => this.f2RegIdField = "FIRSTSHIPMENT";

    public string F2RegId
    {
      get => this.f2RegIdField;
      set => this.f2RegIdField = value;
    }

    public MarkInfoType2 MarkInfo
    {
      get => this.markInfoField;
      set => this.markInfoField = value;
    }
  }
}
