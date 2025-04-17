// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.CasComPort
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Logging;
using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class CasComPort : IScale, IDevice
  {
    private SerialPort _port = new SerialPort();

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public CasComPort(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return;
      ComPort comPort = this.DevicesConfig.Scale.ComPort;
      DeviceHelper.CheckComPortExists(comPort.PortName, (IDevice) this);
      this._port = new SerialPort(comPort.PortName)
      {
        BaudRate = comPort.Speed,
        ReadTimeout = comPort.TimeOut
      };
      LogHelper.Debug(string.Format("Scales COM-port: {0}, {1} / {2}", (object) this._port.PortName, (object) this._port.BaudRate, (object) this._port.ReadTimeout));
      this._port.Open();
    }

    public bool Disconnect()
    {
      this._port.Close();
      this._port.Dispose();
      return true;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      DeviceHelper.CheckComPortExists(this._port.PortName, (IDevice) this);
      if (!this._port.IsOpen)
        this._port.Open();
      weight = 0M;
      LogHelper.Trace("send 0x05");
      this._port.Write(new byte[1]
      {
        Convert.ToByte("0x05", 16)
      }, 0, 1);
      Thread.Sleep(300);
      string str1 = this._port.ReadExisting();
      if (str1.IsNullOrEmpty())
        return;
      byte[] bytes = Encoding.ASCII.GetBytes(str1);
      if (bytes.Length == 0)
        return;
      LogHelper.Trace("string1 from port: " + str1);
      if ((int) bytes[0] == (int) Convert.ToByte("0x06", 16))
      {
        LogHelper.Trace("Получент 0x06 байт. Новый протокол");
        this._port.Write(new byte[1]
        {
          Convert.ToByte("0x11", 16)
        }, 0, 1);
        Thread.Sleep(300);
        str1 = this._port.ReadExisting();
        LogHelper.Trace("string2 from port:" + str1);
      }
      string str2 = new Regex("\\d{1,2}\\.\\d{3}").Match(str1).Value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
      if (str2.IsNullOrEmpty())
        return;
      Decimal.TryParse(str2, out weight);
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara() => throw new NotImplementedException();

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => throw new NotImplementedException();

    public string Name => "CAS (com-port)";
  }
}
