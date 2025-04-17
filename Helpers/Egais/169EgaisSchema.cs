// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformParentF2Type
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/TTNHistoryF2Reg")]
  [Serializable]
  public class InformParentF2Type
  {
    private string identityField;
    private stepType[] histF2Field;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    [XmlArrayItem("step", IsNullable = false)]
    public stepType[] HistF2
    {
      get => this.histF2Field;
      set => this.histF2Field = value;
    }
  }
}
