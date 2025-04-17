// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ProductContractType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClientRef")]
  [Serializable]
  public class ProductContractType
  {
    private string numberField;
    private DateTime dateField;
    private OrgInfo supplierField;
    private OrgInfo contragentField;

    public string number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    public OrgInfo Supplier
    {
      get => this.supplierField;
      set => this.supplierField = value;
    }

    public OrgInfo Contragent
    {
      get => this.contragentField;
      set => this.contragentField = value;
    }
  }
}
