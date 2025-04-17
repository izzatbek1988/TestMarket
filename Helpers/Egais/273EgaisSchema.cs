// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.InformF1F2RegType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ActChargeOn_v2")]
  [Serializable]
  public class InformF1F2RegType
  {
    private InformF1RegType informF1Field;

    public InformF1RegType InformF1
    {
      get => this.informF1Field;
      set => this.informF1Field = value;
    }
  }
}
