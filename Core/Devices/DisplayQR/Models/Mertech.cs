// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayQR.Models.Mertech
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
namespace Gbs.Core.Devices.DisplayQR.Models
{
  public class Mertech : IDisplayQr, IDevice
  {
    private SerialPort _port = new SerialPort();

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Mertech(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.DisplayQr.Port, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.DisplayBuyer;

    public string Name => nameof (Mertech);

    public bool Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return true;
      ComPort port = this.DevicesConfig.DisplayQr.Port;
      DeviceHelper.CheckComPortExists(port.PortName, (IDevice) this);
      this._port = new SerialPort(port.PortName)
      {
        BaudRate = port.Speed,
        ReadTimeout = port.TimeOut,
        Encoding = Encoding.GetEncoding(866)
      };
      LogHelper.Debug(string.Format("Display QR COM-port: {0}, {1} / {2}", (object) this._port.PortName, (object) this._port.BaudRate, (object) this._port.ReadTimeout));
      this._port.Open();
      return true;
    }

    public bool Disconnect()
    {
      this._port.Close();
      this._port.Dispose();
      return true;
    }

    public bool WriteQr(string qr)
    {
      if (string.IsNullOrEmpty(qr))
        return false;
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 2,
        (byte) 242,
        (byte) 2
      });
      byteList.Add((byte) 0);
      byte[] bytes = Encoding.ASCII.GetBytes(qr);
      byteList.Add(Convert.ToByte(bytes.Length));
      byteList.AddRange((IEnumerable<byte>) bytes);
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 2,
        (byte) 242,
        (byte) 3
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public bool Clear()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[7]
      {
        (byte) 2,
        (byte) 240,
        (byte) 3,
        (byte) 67,
        (byte) 76,
        (byte) 83,
        (byte) 3
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public bool Done()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[11]
      {
        (byte) 2,
        (byte) 240,
        (byte) 3,
        (byte) 99,
        (byte) 111,
        (byte) 114,
        (byte) 114,
        (byte) 101,
        (byte) 99,
        (byte) 116,
        (byte) 3
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public bool Error()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[11]
      {
        (byte) 2,
        (byte) 240,
        (byte) 3,
        (byte) 109,
        (byte) 105,
        (byte) 115,
        (byte) 116,
        (byte) 97,
        (byte) 107,
        (byte) 101,
        (byte) 3
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }
  }
}
