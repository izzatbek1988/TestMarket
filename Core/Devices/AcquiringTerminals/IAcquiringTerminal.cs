// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.IAcquiringTerminal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals
{
  public interface IAcquiringTerminal : IDevice
  {
    void ShowProperties();

    void ShowServiceMenu(out string slip);

    bool DoPayment(Decimal sum, out string slip, out string rrn, out string method);

    bool ReturnPayment(Decimal sum, out string slip, string rrn, string method);

    bool GetReport(out string slip);

    bool CloseSession(out string slip);

    bool Connect();

    bool Disconnect();

    void EmergencyCancel();
  }
}
