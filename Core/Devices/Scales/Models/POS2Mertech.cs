// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.POS2Mertech
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class POS2Mertech : IScale, IDevice
  {
    private SerialPort _port = new SerialPort();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public POS2Mertech(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public string Name => "POS2-M/POS2-M Pro";

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
      byte[] numArray = this.DoCommand((byte) 58);
      if (!((int) numArray[5]).GetBit(0))
      {
        LogHelper.Debug("Вес нестабилен, возвращаю значение 0.");
        weight = 0M;
      }
      else
      {
        byte num1 = numArray[7];
        byte num2 = numArray[8];
        byte num3 = numArray[9];
        byte num4 = numArray[10];
        weight = (Decimal) BitConverter.ToInt32(new byte[4]
        {
          num1,
          num2,
          num3,
          num4
        }, 0) / 1000.0M;
      }
    }

    public void Tara()
    {
      byte num = this.DoCommand((byte) 49)[4];
      if (num != (byte) 0)
        throw new DeviceException(Translate.POS2Mertech_Tara_При_установке_тары_на_весах_возникла_ошибка__Код_ошибки_ + num.ToString());
    }

    public void Zero()
    {
      byte num = this.DoCommand((byte) 48, new byte[2])[4];
      if (num != (byte) 0)
        throw new DeviceException(Translate.POS2Mertech_Zero_При_установке_нуля_на_весах_возникла_ошибка__Код_ошибки_ + num.ToString());
    }

    public void TaraReset() => this.Tara();

    private byte[] DoCommand(byte command, byte[] parametr = null)
    {
      DeviceHelper.CheckComPortExists(this._port.PortName, (IDevice) this);
      if (!this._port.IsOpen)
        this._port.Open();
      LogHelper.Trace("send 0x05");
      this._port.Write(new byte[1]{ (byte) 5 }, 0, 1);
      Thread.Sleep(300);
      this._port.ReadExisting();
      Thread.Sleep(300);
      List<byte> byteList = new List<byte>()
      {
        (byte) 2,
        (byte) 0,
        command,
        (byte) 30,
        (byte) 30,
        (byte) 33,
        (byte) 30
      };
      if (parametr != null && parametr.Length != 0)
        byteList.AddRange((IEnumerable<byte>) parametr);
      byteList.Add((byte) 0);
      byteList[1] = (byte) (byteList.Count - 3);
      byte num = 0;
      for (int index = 1; index < byteList.Count - 1; ++index)
        num ^= byteList[index];
      byteList[byteList.Count - 1] = num;
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      Thread.Sleep(300);
      int bytesToRead = this._port.BytesToRead;
      byte[] buffer = new byte[bytesToRead];
      this._port.Read(buffer, 0, bytesToRead);
      return buffer;
    }
  }
}
