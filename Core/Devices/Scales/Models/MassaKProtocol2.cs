// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.MassaKProtocol2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class MassaKProtocol2 : IScale, IDevice
  {
    private SerialPort _port = new SerialPort();

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public MassaKProtocol2(Gbs.Core.Config.Devices devicesConfig)
    {
      this.DevicesConfig = devicesConfig;
    }

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
      this._port.Encoding = Encoding.GetEncoding(1252);
      this._port.Open();
    }

    public bool Disconnect()
    {
      this._port.Close();
      return true;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      DeviceHelper.CheckComPortExists(this._port.PortName, (IDevice) this);
      if (!this._port.IsOpen)
        this._port.Open();
      LogHelper.Debug("send 0x45");
      this._port.Write(new byte[1]
      {
        Convert.ToByte("0x45", 16)
      }, 0, 1);
      Thread.Sleep(100);
      string s = this._port.ReadExisting();
      byte[] bytes = Encoding.GetEncoding(1252).GetBytes(s);
      string str = BitConverter.ToString(bytes);
      int num = 0;
      if (bytes.Length == 2)
        num = (int) BitConverter.ToInt16(bytes, 0);
      else
        LogHelper.Debug("Массив должен быть длиной в 2 байт");
      LogHelper.Debug("bytes from port: " + str + "; ves: " + num.ToString());
      weight = (Decimal) num / 1000.0M;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara()
    {
      DeviceHelper.CheckComPortExists(this._port.PortName, (IDevice) this);
      if (!this._port.IsOpen)
        this._port.Open();
      LogHelper.Debug("send 0x0D");
      this._port.Write(new byte[1]
      {
        Convert.ToByte("0x0D", 16)
      }, 0, 1);
      Thread.Sleep(100);
    }

    public void Zero()
    {
      DeviceHelper.CheckComPortExists(this._port.PortName, (IDevice) this);
      if (!this._port.IsOpen)
        this._port.Open();
      LogHelper.Debug("send 0x0E");
      this._port.Write(new byte[1]
      {
        Convert.ToByte("0x0E", 16)
      }, 0, 1);
      Thread.Sleep(100);
    }

    public void TaraReset() => this.Tara();

    public string Name => Translate.MassaKProtocol2_Name_Масса_К__Протокол_2_;
  }
}
