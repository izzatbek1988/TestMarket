// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.DatalogicComPort
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class DatalogicComPort : IScale, IDevice
  {
    private SerialPort _port = new SerialPort();

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public DatalogicComPort(Gbs.Core.Config.Devices devConfig) => this.DevicesConfig = devConfig;

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
      LogHelper.Debug(string.Format("Scales datalogic COM-port: {0}, {1} / {2}", (object) this._port.PortName, (object) this._port.BaudRate, (object) this._port.ReadTimeout));
      this._port.Parity = comPort.Parity;
      this._port.DataBits = comPort.DataBit;
      this._port.StopBits = comPort.StopBit;
      this._port.Open();
    }

    public bool Disconnect()
    {
      this._port.Close();
      return true;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      LogHelper.Debug("send W");
      byte[] bytes = Encoding.ASCII.GetBytes("W");
      this._port.Write(bytes, 0, bytes.Length);
      Thread.Sleep(500);
      byte[] numArray = new byte[20];
      this._port.Read(numArray, 0, 19);
      LogHelper.Debug("as1: " + BitConverter.ToString(numArray));
      string str = Encoding.ASCII.GetString(numArray).Trim();
      LogHelper.Debug("st:" + str);
      weight = str.ToDecimal();
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara() => throw new NotImplementedException();

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => throw new NotImplementedException();

    public string Name => "DataLogic (com-port)";
  }
}
