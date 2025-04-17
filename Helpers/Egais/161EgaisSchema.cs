// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RailwayType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/CarrierNotice_v4")]
  [Serializable]
  public class RailwayType
  {
    private string railwayModelField;
    private string railwayVidField;
    private string railwayNumberField;

    public string RailwayModel
    {
      get => this.railwayModelField;
      set => this.railwayModelField = value;
    }

    public string RailwayVid
    {
      get => this.railwayVidField;
      set => this.railwayVidField = value;
    }

    public string RailwayNumber
    {
      get => this.railwayNumberField;
      set => this.railwayNumberField = value;
    }
  }
}
