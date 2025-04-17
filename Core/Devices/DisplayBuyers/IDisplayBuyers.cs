// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayBuyers.IDisplayBuyers
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.DisplayBuyers
{
  public interface IDisplayBuyers : IDevice
  {
    string LastResultCodeDescriptor { get; }

    bool ShowProperties();

    bool Connect(bool onlyDriverLoad = false);

    bool Disconnect();

    bool WriteText(string line, int index);

    bool Clear();
  }
}
