// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.TransferFromShopTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/TransferFromShop")]
  [Serializable]
  public class TransferFromShopTypeHeader
  {
    private string transferNumberField;
    private DateTime transferDateField;
    private string noteField;

    public string TransferNumber
    {
      get => this.transferNumberField;
      set => this.transferNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime TransferDate
    {
      get => this.transferDateField;
      set => this.transferDateField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
