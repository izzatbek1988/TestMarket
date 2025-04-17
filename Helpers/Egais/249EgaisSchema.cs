// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.NoAnswerTTN
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
  public class NoAnswerTTN
  {
    private string consigneeField;
    private DateTime replyDateField;
    private NoAnswerType[] ttnlistField;

    public string Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
    }

    public DateTime ReplyDate
    {
      get => this.replyDateField;
      set => this.replyDateField = value;
    }

    [XmlArrayItem("NoAnswer", IsNullable = false)]
    public NoAnswerType[] ttnlist
    {
      get => this.ttnlistField;
      set => this.ttnlistField = value;
    }
  }
}
