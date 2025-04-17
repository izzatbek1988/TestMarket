// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.AscpNav
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/AscpNavigation")]
  [Serializable]
  public class AscpNav
  {
    private AscpNavSensor sensorField;
    private string timeUTCField;
    private Decimal latitudeField;
    private Decimal longitudeField;
    private string countSatelliteField;
    private Decimal accuracyField;
    private string courseField;
    private string speedField;
    private DataType2[] dataLevelGaugeField;

    public AscpNavSensor Sensor
    {
      get => this.sensorField;
      set => this.sensorField = value;
    }

    [XmlElement(DataType = "integer")]
    public string TimeUTC
    {
      get => this.timeUTCField;
      set => this.timeUTCField = value;
    }

    public Decimal Latitude
    {
      get => this.latitudeField;
      set => this.latitudeField = value;
    }

    public Decimal Longitude
    {
      get => this.longitudeField;
      set => this.longitudeField = value;
    }

    [XmlElement(DataType = "integer")]
    public string CountSatellite
    {
      get => this.countSatelliteField;
      set => this.countSatelliteField = value;
    }

    public Decimal Accuracy
    {
      get => this.accuracyField;
      set => this.accuracyField = value;
    }

    [XmlElement(DataType = "integer")]
    public string Course
    {
      get => this.courseField;
      set => this.courseField = value;
    }

    [XmlElement(DataType = "integer")]
    public string Speed
    {
      get => this.speedField;
      set => this.speedField = value;
    }

    [XmlArrayItem("LevelGauge", IsNullable = false)]
    public DataType2[] DataLevelGauge
    {
      get => this.dataLevelGaugeField;
      set => this.dataLevelGaugeField = value;
    }
  }
}
