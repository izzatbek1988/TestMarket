// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyWOCheque
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyWriteOffCheque")]
  [Serializable]
  public class ReplyWOCheque
  {
    private DateTime replyDateField;
    private string monthReportField;
    private string yearReportField;
    private string alcCodeField;
    private string writeOffChField;
    private string returnChField;

    public DateTime ReplyDate
    {
      get => this.replyDateField;
      set => this.replyDateField = value;
    }

    public string monthReport
    {
      get => this.monthReportField;
      set => this.monthReportField = value;
    }

    public string yearReport
    {
      get => this.yearReportField;
      set => this.yearReportField = value;
    }

    public string AlcCode
    {
      get => this.alcCodeField;
      set => this.alcCodeField = value;
    }

    [XmlElement(DataType = "integer")]
    public string WriteOffCh
    {
      get => this.writeOffChField;
      set => this.writeOffChField = value;
    }

    [XmlElement(DataType = "integer")]
    public string ReturnCh
    {
      get => this.returnChField;
      set => this.returnChField = value;
    }
  }
}
