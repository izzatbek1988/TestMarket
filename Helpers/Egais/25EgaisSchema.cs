// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReferenceOfDeficienciesHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ReferenceOfDeficiencies")]
  [Serializable]
  public class ReferenceOfDeficienciesHeader
  {
    private DateTime referenceDateField;
    private string requestFSMField;
    private string nUMBERIssueFSMField;
    private DateTime dateIssueFSMField;
    private OrgInfoRus_ClaimIssue terrOrganRARField;
    private OrgInfoRus_ClaimIssue declarerField;
    private string commentsField;
    private byte[] signReferenceField;
    private byte[] signCertificateField;

    [XmlElement(DataType = "date")]
    public DateTime ReferenceDate
    {
      get => this.referenceDateField;
      set => this.referenceDateField = value;
    }

    public string RequestFSM
    {
      get => this.requestFSMField;
      set => this.requestFSMField = value;
    }

    public string NUMBERIssueFSM
    {
      get => this.nUMBERIssueFSMField;
      set => this.nUMBERIssueFSMField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime DateIssueFSM
    {
      get => this.dateIssueFSMField;
      set => this.dateIssueFSMField = value;
    }

    public OrgInfoRus_ClaimIssue TerrOrganRAR
    {
      get => this.terrOrganRARField;
      set => this.terrOrganRARField = value;
    }

    public OrgInfoRus_ClaimIssue Declarer
    {
      get => this.declarerField;
      set => this.declarerField = value;
    }

    public string Comments
    {
      get => this.commentsField;
      set => this.commentsField = value;
    }

    [XmlElement(DataType = "base64Binary")]
    public byte[] SignReference
    {
      get => this.signReferenceField;
      set => this.signReferenceField = value;
    }

    [XmlElement(DataType = "base64Binary")]
    public byte[] SignCertificate
    {
      get => this.signCertificateField;
      set => this.signCertificateField = value;
    }
  }
}
