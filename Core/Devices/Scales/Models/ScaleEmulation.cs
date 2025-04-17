// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.ScaleEmulation
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using System;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class ScaleEmulation : IScale, IDevice
  {
    private static Decimal _randomWeight;

    public bool ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show("Scale emulator config!");
      return true;
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      if (new Random().Next(1, 5) == 1)
        this.ThrowException();
      ScaleEmulation._randomWeight = (Decimal) new Random().Next(100, 10000);
    }

    private void ThrowException() => throw new InvalidOperationException("тестовое исключение");

    public bool Disconnect()
    {
      if (new Random().Next(1, 5) == 1)
        this.ThrowException();
      return true;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      if (ScaleEmulation._randomWeight < 100M)
        ScaleEmulation._randomWeight += 10M;
      int num = new Random().Next(1, 10);
      if (num == 1)
        ScaleEmulation._randomWeight += 1M;
      if (num == 2)
        ScaleEmulation._randomWeight -= 1M;
      if (num == 3)
        this.ThrowException();
      if (num == 4)
        ScaleEmulation._randomWeight += 10M;
      if (num == 5)
        ScaleEmulation._randomWeight -= 10M;
      Thread.Sleep(500);
      weight = ScaleEmulation._randomWeight / 1000M;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara() => throw new NotImplementedException();

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => throw new NotImplementedException();

    public string Name => "Scale Emulator";
  }
}
