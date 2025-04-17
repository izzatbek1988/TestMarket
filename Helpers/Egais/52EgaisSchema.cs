// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ProductContractType1
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
  [XmlType(TypeName = "ProductContractType", Namespace = "http://fsrar.ru/WEGAIS/ClientRef_v2")]
  [Serializable]
  public class ProductContractType1
  {
    private string numberField;
    private DateTime dateField;
    private OrgInfo_v2 supplierField;
    private OrgInfo_v2 contragentField;

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

    public OrgInfo_v2 Supplier
    {
      get => this.supplierField;
      set => this.supplierField = value;
    }

    public OrgInfo_v2 Contragent
    {
      get => this.contragentField;
      set => this.contragentField = value;
    }
  }
}
