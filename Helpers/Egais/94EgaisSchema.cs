// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActConfirmOnlineOrderTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActConfirmOnlineOrder")]
  [Serializable]
  public class ActConfirmOnlineOrderTypeHeader
  {
    private OrgInfoRus_v2 applicantField;
    private string onlineStoreIdField;
    private string docIDField;
    private string confirmNumberField;
    private DateTime confirmDateTimeField;
    private string noteField;

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

    public string ConfirmNumber
    {
      get => this.confirmNumberField;
      set => this.confirmNumberField = value;
    }

    public DateTime ConfirmDateTime
    {
      get => this.confirmDateTimeField;
      set => this.confirmDateTimeField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
