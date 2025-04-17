// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ActInventoryTypeHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ActInventorySingle")]
  [Serializable]
  public class ActInventoryTypeHeader
  {
    private string numberField;
    private string divisionNameField;
    private string inventoryBasisField;
    private string inventoryBasisNumberField;
    private DateTime inventoryBasisDateField;
    private DateTime inventoryDateBeginField;
    private DateTime inventoryDateEndField;
    private string noteField;

    public string Number
    {
      get => this.numberField;
      set => this.numberField = value;
    }

    public string DivisionName
    {
      get => this.divisionNameField;
      set => this.divisionNameField = value;
    }

    public string InventoryBasis
    {
      get => this.inventoryBasisField;
      set => this.inventoryBasisField = value;
    }

    public string InventoryBasisNumber
    {
      get => this.inventoryBasisNumberField;
      set => this.inventoryBasisNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime InventoryBasisDate
    {
      get => this.inventoryBasisDateField;
      set => this.inventoryBasisDateField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime InventoryDateBegin
    {
      get => this.inventoryDateBeginField;
      set => this.inventoryDateBeginField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime InventoryDateEnd
    {
      get => this.inventoryDateEndField;
      set => this.inventoryDateEndField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
