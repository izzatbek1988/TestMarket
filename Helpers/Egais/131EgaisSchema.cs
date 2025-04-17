// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RequestManufacturerFSMTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/RequestManufacturerFSM")]
  [Serializable]
  public class RequestManufacturerFSMTypeHeader
  {
    private string nUMBERField;
    private DateTime dateField;
    private string requestFSMField;
    private OrgInfoRus_ClaimIssue clientField;
    private OrgInfoRus_ClaimIssue terrOrganRARField;

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

    public string RequestFSM
    {
      get => this.requestFSMField;
      set => this.requestFSMField = value;
    }

    public OrgInfoRus_ClaimIssue Client
    {
      get => this.clientField;
      set => this.clientField = value;
    }

    public OrgInfoRus_ClaimIssue TerrOrganRAR
    {
      get => this.terrOrganRARField;
      set => this.terrOrganRARField = value;
    }
  }
}
