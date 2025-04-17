// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionReportType1
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
  [XmlType(TypeName = "PositionReportType", Namespace = "http://fsrar.ru/WEGAIS/RequestManufacturerFSMforCheck")]
  [Serializable]
  public class PositionReportType1
  {
    private string identityField;
    private string vidAP171fzField;
    private string markTypeField;
    private string quantityField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public string VidAP171fz
    {
      get => this.vidAP171fzField;
      set => this.vidAP171fzField = value;
    }

    public string MarkType
    {
      get => this.markTypeField;
      set => this.markTypeField = value;
    }

    [XmlElement(DataType = "integer")]
    public string Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }
  }
}
