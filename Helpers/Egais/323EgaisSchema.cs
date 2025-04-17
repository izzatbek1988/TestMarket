// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RepProducedTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/RepProducedProduct")]
  [Serializable]
  public class RepProducedTypeHeader
  {
    private OperType typeField;
    private string nUMBERField;
    private DateTime dateField;
    private DateTime producedDateField;
    private OrgInfoRus_v2 producerField;
    private string noteField;

    public RepProducedTypeHeader() => this.typeField = OperType.OperProduction;

    public OperType Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    public string NUMBER
    {
      get => this.nUMBERField;
      set => this.nUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ProducedDate
    {
      get => this.producedDateField;
      set => this.producedDateField = value;
    }

    public OrgInfoRus_v2 Producer
    {
      get => this.producerField;
      set => this.producerField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
