// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Файл
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
  [XmlRoot(Namespace = "", IsNullable = false)]
  [Serializable]
  public class Файл
  {
    private ФайлСвУчДокОбор свУчДокОборField;
    private ФайлДокумент документField;
    private string идФайлField;
    private string версПрогField;

    public ФайлСвУчДокОбор СвУчДокОбор
    {
      get => this.свУчДокОборField;
      set => this.свУчДокОборField = value;
    }

    public ФайлДокумент Документ
    {
      get => this.документField;
      set => this.документField = value;
    }

    [XmlAttribute]
    public string ИдФайл
    {
      get => this.идФайлField;
      set => this.идФайлField = value;
    }

    [XmlAttribute]
    public string ВерсПрог
    {
      get => this.версПрогField;
      set => this.версПрогField = value;
    }
  }
}
