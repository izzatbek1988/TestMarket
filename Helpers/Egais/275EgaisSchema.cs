// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActChargeOnPositionTypeInformF1F2
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActChargeOn_v2")]
  [Serializable]
  public class ActChargeOnPositionTypeInformF1F2
  {
    private InformF1F2RegType informF1F2RegField;

    public InformF1F2RegType InformF1F2Reg
    {
      get => this.informF1F2RegField;
      set => this.informF1F2RegField = value;
    }
  }
}
