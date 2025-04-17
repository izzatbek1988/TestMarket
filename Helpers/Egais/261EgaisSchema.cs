// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActWriteOffPositionType1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(TypeName = "ActWriteOffPositionType", Namespace = "http://fsrar.ru/WEGAIS/ActWriteOff_v2")]
  [Serializable]
  public class ActWriteOffPositionType1
  {
    private string identityField;
    private Decimal quantityField;
    private InformF1F2 informF1F2Field;
    private string[] markCodeInfoField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public Decimal Quantity
    {
      get => this.quantityField;
      set => this.quantityField = value;
    }

    public InformF1F2 InformF1F2
    {
      get => this.informF1F2Field;
      set => this.informF1F2Field = value;
    }

    [XmlArrayItem("MarkCode", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
    public string[] MarkCodeInfo
    {
      get => this.markCodeInfoField;
      set => this.markCodeInfoField = value;
    }
  }
}
