// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.MarkType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/QueryBarcode")]
  [Serializable]
  public class MarkType
  {
    private string identityField;
    private TypeEnum typeField;
    private string rankField;
    private string numberField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public TypeEnum Type
    {
      get => this.typeField;
      set => this.typeField = value;
    }

    public string Rank
    {
      get => this.rankField;
      set => this.rankField = value;
    }

    public string Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }
  }
}
