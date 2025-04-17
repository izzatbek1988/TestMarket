// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReportDestructionAMFSMTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ReportDestructionAMFSM")]
  [Serializable]
  public class ReportDestructionAMFSMTypeHeader
  {
    private string nUMBERField;
    private DateTime dateField;
    private OrgInfoRus_ClaimIssue clientField;
    private string totalQuantityField;

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

    public OrgInfoRus_ClaimIssue Client
    {
      get => this.clientField;
      set => this.clientField = value;
    }

    [XmlElement(DataType = "integer")]
    public string TotalQuantity
    {
      get => this.totalQuantityField;
      set => this.totalQuantityField = value;
    }
  }
}
