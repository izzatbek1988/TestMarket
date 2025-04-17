// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.IScale
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.Scales
{
  public interface IScale : IDevice
  {
    bool ShowProperties();

    void Connect(bool onlyDriverLoad = false);

    bool Disconnect();

    void GetWeight(out Decimal weight, Decimal price);

    void Tara();

    void Zero();

    void TaraReset();
  }
}
