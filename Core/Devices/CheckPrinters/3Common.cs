// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.NonFiscalString
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class NonFiscalString
  {
    private readonly string _text = string.Empty;

    public TextAlignment Alignment { get; set; }

    public string Text => this._text.Replace('\r', ' ').Replace('\n', ' ');

    public bool WideFont { get; set; }

    public NonFiscalString()
    {
    }

    public NonFiscalString(string text) => this._text = text;

    public NonFiscalString(string text, TextAlignment alignment)
    {
      this._text = text;
      this.Alignment = alignment;
    }
  }
}
