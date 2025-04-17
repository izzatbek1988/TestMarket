// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.SecondMonitor
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class SecondMonitor
  {
    public string MonitorName { get; set; }

    public bool IsActivePhoto { get; set; }

    public string PathImages { get; set; }

    public int Interval { get; set; } = 10;
  }
}
