// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.CalculationDemandType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClaimIssueFSM")]
  [Serializable]
  public class CalculationDemandType
  {
    private SpiritType[] spiritField;
    private WineMaterialType[] wineMaterialField;
    private RawAgedType[] rawAgedField;
    private MarkedAPType[] markedAPField;
    private WinemakingAPType[] winemakingAPField;
    private WinemakingAPSTPType[] winemakingAPSTPField;

    [XmlElement("Spirit")]
    public SpiritType[] Spirit
    {
      get => this.spiritField;
      set => this.spiritField = value;
    }

    [XmlElement("WineMaterial")]
    public WineMaterialType[] WineMaterial
    {
      get => this.wineMaterialField;
      set => this.wineMaterialField = value;
    }

    [XmlElement("RawAged")]
    public RawAgedType[] RawAged
    {
      get => this.rawAgedField;
      set => this.rawAgedField = value;
    }

    [XmlElement("MarkedAP")]
    public MarkedAPType[] MarkedAP
    {
      get => this.markedAPField;
      set => this.markedAPField = value;
    }

    [XmlElement("WinemakingAP")]
    public WinemakingAPType[] WinemakingAP
    {
      get => this.winemakingAPField;
      set => this.winemakingAPField = value;
    }

    [XmlElement("WinemakingAPSTP")]
    public WinemakingAPSTPType[] WinemakingAPSTP
    {
      get => this.winemakingAPSTPField;
      set => this.winemakingAPSTPField = value;
    }
  }
}
