// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ZplSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class ZplSetting
  {
    public bool IsPrintAsBitmap { get; set; }

    public ZplSetting.ZplDensityType Density { get; set; } = ZplSetting.ZplDensityType.D12;

    public static Dictionary<ZplSetting.ZplDensityType, string> ZplDensityDictionary
    {
      get
      {
        return new Dictionary<ZplSetting.ZplDensityType, string>()
        {
          {
            ZplSetting.ZplDensityType.D6,
            "6 dpmm (152 dpi)"
          },
          {
            ZplSetting.ZplDensityType.D8,
            "8 dpmm (203 dpi)"
          },
          {
            ZplSetting.ZplDensityType.D12,
            "12 dpmm (300 dpi)"
          },
          {
            ZplSetting.ZplDensityType.D24,
            "24 dpmm (600 dpi)"
          }
        };
      }
    }

    public enum ZplDensityType
    {
      D6,
      D8,
      D12,
      D24,
    }
  }
}
