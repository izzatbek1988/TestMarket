// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.AsiiuTime
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/AsiiuTime")]
  [Serializable]
  public class AsiiuTime
  {
    private AsiiuTimeSensor sensorField;
    private OrgInfoRus_v2 producerField;
    private DataType1[] dataField;

    public AsiiuTimeSensor Sensor
    {
      get => this.sensorField;
      set => this.sensorField = value;
    }

    public OrgInfoRus_v2 Producer
    {
      get => this.producerField;
      set => this.producerField = value;
    }

    [XmlArrayItem("Position", IsNullable = false)]
    public DataType1[] Data
    {
      get => this.dataField;
      set => this.dataField = value;
    }
  }
}
