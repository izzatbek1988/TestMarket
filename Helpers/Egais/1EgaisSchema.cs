// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.Documents
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01")]
  [XmlRoot(Namespace = "http://fsrar.ru/WEGAIS/WB_DOC_SINGLE_01", IsNullable = false)]
  [Serializable]
  public class Documents
  {
    private SenderInfo ownerField;
    private DocBody documentField;
    private string versionField;

    public Documents() => this.versionField = "1.0";

    public SenderInfo Owner
    {
      get => this.ownerField;
      set => this.ownerField = value;
    }

    public DocBody Document
    {
      get => this.documentField;
      set => this.documentField = value;
    }

    [XmlAttribute]
    [DefaultValue("1.0")]
    public string Version
    {
      get => this.versionField;
      set => this.versionField = value;
    }
  }
}
