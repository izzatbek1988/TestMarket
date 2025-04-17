// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.stepBType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ReplyParentHistForm2")]
  [Serializable]
  public class stepBType
  {
    private string levField;
    private string form2Field;
    private string parentForm2Field;
    private string shipperField;
    private string consigneeField;
    private string wBRegIdField;
    private Decimal amountField;

    [XmlElement(DataType = "integer")]
    public string lev
    {
      get => this.levField;
      set => this.levField = value;
    }

    public string Form2
    {
      get => this.form2Field;
      set => this.form2Field = value;
    }

    public string parentForm2
    {
      get => this.parentForm2Field;
      set => this.parentForm2Field = value;
    }

    public string Shipper
    {
      get => this.shipperField;
      set => this.shipperField = value;
    }

    public string Consignee
    {
      get => this.consigneeField;
      set => this.consigneeField = value;
    }

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
    }

    public Decimal amount
    {
      get => this.amountField;
      set => this.amountField = value;
    }
  }
}
