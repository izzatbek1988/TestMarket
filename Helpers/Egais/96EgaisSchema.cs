// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActCancelOnlineOrderTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActCancelOnlineOrder")]
  [Serializable]
  public class ActCancelOnlineOrderTypeHeader
  {
    private OrgInfoRus_v2 applicantField;
    private string onlineStoreIdField;
    private string docIDField;
    private string cancelNumberField;
    private DateTime cancelDateTimeField;
    private string causeCancelField;

    public OrgInfoRus_v2 Applicant
    {
      get => this.applicantField;
      set => this.applicantField = value;
    }

    public string OnlineStoreId
    {
      get => this.onlineStoreIdField;
      set => this.onlineStoreIdField = value;
    }

    public string DocID
    {
      get => this.docIDField;
      set => this.docIDField = value;
    }

    public string CancelNumber
    {
      get => this.cancelNumberField;
      set => this.cancelNumberField = value;
    }

    public DateTime CancelDateTime
    {
      get => this.cancelDateTimeField;
      set => this.cancelDateTimeField = value;
    }

    public string CauseCancel
    {
      get => this.causeCancelField;
      set => this.causeCancelField = value;
    }
  }
}
