// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.OrgLicenseType
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
  public class OrgLicenseType
  {
    private string numberField;
    private DateTime datefromField;
    private DateTime datetoField;
    private string orgdistributeField;

    public string number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime datefrom
    {
      get => this.datefromField;
      set => this.datefromField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime dateto
    {
      get => this.datetoField;
      set => this.datetoField = value;
    }

    public string orgdistribute
    {
      get => this.orgdistributeField;
      set => this.orgdistributeField = value;
    }
  }
}
