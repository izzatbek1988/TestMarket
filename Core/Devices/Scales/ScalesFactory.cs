// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.ScalesFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;

#nullable disable
namespace Gbs.Core.Devices.Scales
{
  public static class ScalesFactory
  {
    private static ScalesHelper ScaleHelper { get; set; }

    public static ScalesHelper Create(IConfig config)
    {
      return ScalesFactory.ScaleHelper ?? (ScalesFactory.ScaleHelper = new ScalesHelper(config));
    }

    public static void Dispose()
    {
      ScalesFactory.ScaleHelper?.Dispose();
      ScalesFactory.ScaleHelper = (ScalesHelper) null;
    }
  }
}
