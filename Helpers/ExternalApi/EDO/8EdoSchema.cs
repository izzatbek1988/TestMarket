// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.КонтактТип
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
  [Serializable]
  public class КонтактТип
  {
    private string тлфField;
    private string элПочтаField;

    [XmlAttribute]
    public string Тлф
    {
      get => this.тлфField;
      set => this.тлфField = value;
    }

    [XmlAttribute]
    public string ЭлПочта
    {
      get => this.элПочтаField;
      set => this.элПочтаField = value;
    }
  }
}
