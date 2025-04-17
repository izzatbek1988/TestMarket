// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OperationResultType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/Ticket")]
  [Serializable]
  public class OperationResultType
  {
    private string operationNameField;
    private ConclusionType operationResultField;
    private string operationCommentField;
    private DateTime operationDateField;
    private bool operationDateFieldSpecified;

    public string OperationName
    {
      get => this.operationNameField;
      set => this.operationNameField = value;
    }

    public ConclusionType OperationResult
    {
      get => this.operationResultField;
      set => this.operationResultField = value;
    }

    public string OperationComment
    {
      get => this.operationCommentField;
      set => this.operationCommentField = value;
    }

    public DateTime OperationDate
    {
      get => this.operationDateField;
      set => this.operationDateField = value;
    }

    [XmlIgnore]
    public bool OperationDateSpecified
    {
      get => this.operationDateFieldSpecified;
      set => this.operationDateFieldSpecified = value;
    }
  }
}
