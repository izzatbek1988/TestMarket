// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ChequeV3Type
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ChequeV3")]
  [Serializable]
  public class ChequeV3Type
  {
    private string identityField;
    private object itemField;
    private ChequeV3TypeContent contentField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    [XmlElement("Header", typeof (Header))]
    [XmlElement("HeaderTTN", typeof (HeaderTTN))]
    public object Item
    {
      get => this.itemField;
      set => this.itemField = value;
    }

    public ChequeV3TypeContent Content
    {
      get => this.contentField;
      set => this.contentField = value;
    }
  }
}
