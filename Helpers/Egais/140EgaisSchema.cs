// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActWOType
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
  public class ActWOType
  {
    private string aWORegIdField;
    private string actNumberField;
    private DateTime actDateField;
    private bool actDateFieldSpecified;
    private TypeWriteOff1 typeWriteOffField;
    private bool typeWriteOffFieldSpecified;

    public string AWORegId
    {
      get => this.aWORegIdField;
      set => this.aWORegIdField = value;
    }

    public string ActNumber
    {
      get => this.actNumberField;
      set => this.actNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ActDate
    {
      get => this.actDateField;
      set => this.actDateField = value;
    }

    [XmlIgnore]
    public bool ActDateSpecified
    {
      get => this.actDateFieldSpecified;
      set => this.actDateFieldSpecified = value;
    }

    public TypeWriteOff1 TypeWriteOff
    {
      get => this.typeWriteOffField;
      set => this.typeWriteOffField = value;
    }

    [XmlIgnore]
    public bool TypeWriteOffSpecified
    {
      get => this.typeWriteOffFieldSpecified;
      set => this.typeWriteOffFieldSpecified = value;
    }
  }
}
