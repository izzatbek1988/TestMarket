// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Tooltips.HelpTip
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Windows.Controls.Primitives;

#nullable disable
namespace Gbs.Helpers.Tooltips
{
  public class HelpTip
  {
    public string Header { get; set; }

    public string Text { get; set; }

    public PlacementMode PlacementMode { get; set; } = PlacementMode.Bottom;

    public string DisabledText { get; set; }

    public HotKeysHelper.Hotkey Hotkey { get; set; }

    public HelpTip(string header, string text)
    {
      this.Header = header;
      this.Text = text;
    }

    public HelpTip(string header, string text, PlacementMode placement)
    {
      this.Header = header;
      this.Text = text;
      this.PlacementMode = placement;
    }

    public HelpTip(string header, string text, string disabledText, PlacementMode placement)
    {
      this.Header = header;
      this.Text = text;
      this.PlacementMode = placement;
      this.DisabledText = disabledText;
    }

    public HelpTip(string header, string text, string disabledText)
    {
      this.Header = header;
      this.Text = text;
      this.DisabledText = disabledText;
    }
  }
}
