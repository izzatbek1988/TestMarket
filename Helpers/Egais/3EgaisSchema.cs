// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ReplyGenerationOpenKeyType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyGenerationOpenKey")]
  [Serializable]
  public class ReplyGenerationOpenKeyType
  {
    private string clientIdField;
    private byte[] certificateField;

    public string ClientId
    {
      get => this.clientIdField;
      set => this.clientIdField = value;
    }

    [XmlElement(DataType = "base64Binary")]
    public byte[] Certificate
    {
      get => this.certificateField;
      set => this.certificateField = value;
    }
  }
}
