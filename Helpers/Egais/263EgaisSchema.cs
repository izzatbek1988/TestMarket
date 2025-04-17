// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActWriteOffType_v2Header
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActWriteOff_v2")]
  [Serializable]
  public class ActWriteOffType_v2Header
  {
    private string actNumberField;
    private DateTime actDateField;
    private TypeWriteOff1 typeWriteOffField;
    private string noteField;

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

    public TypeWriteOff1 TypeWriteOff
    {
      get => this.typeWriteOffField;
      set => this.typeWriteOffField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
