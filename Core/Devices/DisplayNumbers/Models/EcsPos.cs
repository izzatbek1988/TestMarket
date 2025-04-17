// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayNumbers.EcsPos
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.DisplayNumbers
{
  internal class EcsPos : IDisplayNumbers, IDevice
  {
    private SerialPort _port = new SerialPort();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.NumberDisplayBuyer;

    public string Name => "ECS/POS";

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public EcsPos(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.DisplayNumbersPayer.Port, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return true;
      ComPort port = this.DevicesConfig.DisplayNumbersPayer.Port;
      DeviceHelper.CheckComPortExists(port.PortName, (IDevice) this);
      this._port = new SerialPort(port.PortName)
      {
        BaudRate = port.Speed,
        ReadTimeout = port.TimeOut
      };
      LogHelper.Debug(string.Format("Display numbers COM-port: {0}, {1} / {2}", (object) this._port.PortName, (object) this._port.BaudRate, (object) this._port.ReadTimeout));
      this._port.Open();
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[2]
      {
        (byte) 27,
        (byte) 64
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public bool Disconnect()
    {
      this._port.Close();
      this._port.Dispose();
      return true;
    }

    public bool WriteNumber(Decimal number)
    {
      string s = number.ToString("0.##").Replace(',', '.');
      LogHelper.Debug("Передаем число: " + s);
      byte[] bytes = Encoding.ASCII.GetBytes(s);
      this._port.Write(new byte[3]
      {
        (byte) 27,
        (byte) 81,
        (byte) 65
      }, 0, 3);
      foreach (byte num in bytes)
        this._port.Write(new byte[1]{ num }, 0, 1);
      this._port.Write(new byte[1]{ (byte) 13 }, 0, 1);
      return true;
    }

    public bool Clear()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[1]
      {
        (byte) 12
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }
  }
}
