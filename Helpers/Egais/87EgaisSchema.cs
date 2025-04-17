// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActReceiptOnlineOrderTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActReceiptOnlineOrder")]
  [Serializable]
  public class ActReceiptOnlineOrderTypeHeader
  {
    private OrgInfoRus_v2 applicantField;
    private string onlineStoreIdField;
    private string orderNumberField;
    private DateTime orderDateTimeField;
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

    public string OrderNumber
    {
      get => this.orderNumberField;
      set => this.orderNumberField = value;
    }

    public DateTime OrderDateTime
    {
      get => this.orderDateTimeField;
      set => this.orderDateTimeField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
