// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RecheckNotificationsFSMTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/RecheckNotificationsFSM")]
  [Serializable]
  public class RecheckNotificationsFSMTypeHeader
  {
    private string nUMBERField;
    private DateTime dateField;
    private string requestNUMBERField;
    private DateTime requestDateField;
    private string requestFSMField;
    private OrgInfoRus_ClaimIssue declarerField;
    private string commentField;

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

    public string RequestNUMBER
    {
      get => this.requestNUMBERField;
      set => this.requestNUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime RequestDate
    {
      get => this.requestDateField;
      set => this.requestDateField = value;
    }

    public string RequestFSM
    {
      get => this.requestFSMField;
      set => this.requestFSMField = value;
    }

    public OrgInfoRus_ClaimIssue Declarer
    {
      get => this.declarerField;
      set => this.declarerField = value;
    }

    public string Comment
    {
      get => this.commentField;
      set => this.commentField = value;
    }
  }
}
