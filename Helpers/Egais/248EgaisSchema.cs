// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.NoAnswerType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyNoAnswerTTN")]
  [Serializable]
  public class NoAnswerType
  {
    private string wbRegIDField;
    private string ttnNumberField;
    private DateTime ttnDateField;
    private string shipperField;

    public string WbRegID
    {
      get => this.wbRegIDField;
      set => this.wbRegIDField = value;
    }

    public string ttnNumber
    {
      get => this.ttnNumberField;
      set => this.ttnNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ttnDate
    {
      get => this.ttnDateField;
      set => this.ttnDateField = value;
    }

    public string Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }
  }
}
