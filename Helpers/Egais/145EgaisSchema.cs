// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RequestAdjustmentPositionType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAdjustmentData")]
  [Serializable]
  public class RequestAdjustmentPositionType
  {
    private object itemField;

    [XmlElement("ActCO", typeof (ActCOType))]
    [XmlElement("ActTTN", typeof (ActTTNType))]
    [XmlElement("ActWO", typeof (ActWOType))]
    [XmlElement("ActWOPos", typeof (ActWOPosType))]
    [XmlElement("ActWOS", typeof (ActWOSType))]
    [XmlElement("ActWOSPos", typeof (ActWOSPosType))]
    [XmlElement("ReportImported", typeof (ReportImportedType))]
    [XmlElement("ReportProduced", typeof (ReportProducedType))]
    [XmlElement("Route", typeof (RouteType))]
    [XmlElement("TTN", typeof (TTNType))]
    [XmlElement("TTNPos", typeof (TTNPosType))]
    public object Item
    {
      get => this.itemField;
      set => this.itemField = value;
    }
  }
}
