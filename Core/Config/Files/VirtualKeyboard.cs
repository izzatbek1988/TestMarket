// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.VirtualKeyboard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class VirtualKeyboard
  {
    public bool IsEnabled { get; set; }

    public bool ActivateOnlyByClick { get; set; }

    public int ButtonSize { get; set; } = 50;

    public bool AutoAdaptiveForInputType { get; set; }
  }
}
