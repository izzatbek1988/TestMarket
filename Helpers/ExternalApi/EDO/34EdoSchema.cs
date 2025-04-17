// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ФайлДокументТаблСчФактСведТовДопСведТовСведПрослеж
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  [GeneratedCode("xsd", "4.7.3081.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true)]
  [Serializable]
  public class ФайлДокументТаблСчФактСведТовДопСведТовСведПрослеж
  {
    private string номТовПрослежField;
    private string едИзмПрослежField;
    private string наимЕдИзмПрослежField;
    private Decimal колВЕдПрослежField;
    private string допПрослежField;

    [XmlAttribute]
    public string НомТовПрослеж
    {
      get => this.номТовПрослежField;
      set => this.номТовПрослежField = value;
    }

    [XmlAttribute]
    public string ЕдИзмПрослеж
    {
      get => this.едИзмПрослежField;
      set => this.едИзмПрослежField = value;
    }

    [XmlAttribute]
    public string НаимЕдИзмПрослеж
    {
      get => this.наимЕдИзмПрослежField;
      set => this.наимЕдИзмПрослежField = value;
    }

    [XmlAttribute]
    public Decimal КолВЕдПрослеж
    {
      get => this.колВЕдПрослежField;
      set => this.колВЕдПрослежField = value;
    }

    [XmlAttribute]
    public string ДопПрослеж
    {
      get => this.допПрослежField;
      set => this.допПрослежField = value;
    }
  }
}
