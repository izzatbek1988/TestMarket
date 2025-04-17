// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyHistForm2
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyHistForm2")]
  [Serializable]
  public class ReplyHistForm2
  {
    private string informF2RegIdField;
    private DateTime histForm2DateField;
    private OperationBType1[] historyF2Field;

    public string InformF2RegId
    {
      get => this.informF2RegIdField;
      set => this.informF2RegIdField = value;
    }

    public DateTime HistForm2Date
    {
      get => this.histForm2DateField;
      set => this.histForm2DateField = value;
    }

    [XmlArrayItem("OperationB", IsNullable = false)]
    public OperationBType1[] HistoryF2
    {
      get => this.historyF2Field;
      set => this.historyF2Field = value;
    }
  }
}
