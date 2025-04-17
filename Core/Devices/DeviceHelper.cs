// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DeviceHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices
{
  public class DeviceHelper
  {
    public static void CheckComPortExists(string comPortName, IDevice device)
    {
      if (!((IEnumerable<string>) SerialPort.GetPortNames()).Any<string>((Func<string, bool>) (x => x == comPortName)))
        throw new DeviceException(string.Format(Translate.DeviceHelper_неУдалосьПодключитьсяКУстройству, (object) device.Name, (object) comPortName), device);
    }
  }
}
