// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RequestRepealIPI
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestRepealIPI")]
  [Serializable]
  public class RequestRepealIPI
  {
    private string clientIdField;
    private string requestNumberField;
    private DateTime requestDateField;
    private string iPIRegIdField;

    public string ClientId
    {
      get => this.clientIdField;
      set => this.clientIdField = value;
    }

    public string RequestNumber
    {
      get => this.requestNumberField;
      set => this.requestNumberField = value;
    }

    public DateTime RequestDate
    {
      get => this.requestDateField;
      set => this.requestDateField = value;
    }

    public string IPIRegId
    {
      get => this.iPIRegIdField;
      set => this.iPIRegIdField = value;
    }
  }
}
