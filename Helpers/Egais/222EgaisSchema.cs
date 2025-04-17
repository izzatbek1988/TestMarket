// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ConfirmRepealWBHeader
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
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/ConfirmRepealWB")]
  [Serializable]
  public class ConfirmRepealWBHeader
  {
    private ConclusionType2 isConfirmField;
    private string confirmNumberField;
    private DateTime confirmDateField;
    private string wBRegIdField;
    private string noteField;

    public ConclusionType2 IsConfirm
    {
      get => this.isConfirmField;
      set => this.isConfirmField = value;
    }

    public string ConfirmNumber
    {
      get => this.confirmNumberField;
      set => this.confirmNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime ConfirmDate
    {
      get => this.confirmDateField;
      set => this.confirmDateField = value;
    }

    public string WBRegId
    {
      get => this.wBRegIdField;
      set => this.wBRegIdField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }
  }
}
