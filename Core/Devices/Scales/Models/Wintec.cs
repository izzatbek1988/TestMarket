// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.Wintec
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class Wintec : IScale, IDevice
  {
    private const string WintecPath = "dll\\scale\\wintec\\";
    private const string WintecLibPath = "dll\\scale\\wintec\\pos_ad_dll.dll";
    private const string WintecXmlFilePath = "dll\\scale\\wintec\\WintecScalePOS.xml";
    private bool _isConnection;

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Wintec(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "send_zero_stdcall")]
    private static extern int ZeroSend();

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "send_tare_stdcall")]
    private static extern int TareSend(IntPtr buf);

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "clear_tare_stdcall")]
    private static extern int ClearTare(IntPtr buf);

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "OpenADCom_stdcall")]
    private static extern bool OpenPort(uint port, uint speed);

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "CloseADCom_stdcall")]
    private static extern bool ClosePort();

    [DllImport("dll\\scale\\wintec\\pos_ad_dll.dll", EntryPoint = "read_standard_stdcall")]
    private static extern int Read_standard_stdcall(IntPtr buf);

    public void Connect(bool onlyDriverLoad = false)
    {
      if (!FileSystemHelper.CheckFileExistWithMsg("dll\\scale\\wintec\\pos_ad_dll.dll"))
        throw new FileNotFoundException(Translate.Wintec_Connect_Не_найден_файл_драйвера_Wintec);
      if (!FileSystemHelper.CheckFileExistWithMsg("dll\\scale\\wintec\\WintecScalePOS.xml"))
        throw new FileNotFoundException(Translate.Wintec_Connect_Не_найден_XML_файл_настроек_драйвера_Wintec);
      LogHelper.Debug("Библиотека pos_ad_dll.dll и файл WintecScalePOS.xml присутствует");
      if (onlyDriverLoad)
        return;
      ComPort comPort = this.DevicesConfig.Scale.ComPort;
      DeviceHelper.CheckComPortExists(comPort.PortName, (IDevice) this);
      uint portNumber = (uint) comPort.PortNumber;
      uint speed = (uint) comPort.Speed;
      bool flag = Wintec.OpenPort(portNumber, speed);
      this._isConnection = flag;
      LogHelper.Debug("Открытие порта; num: " + portNumber.ToString() + "; speed:  " + speed.ToString() + "; result: " + flag.ToString());
      if (!flag)
        throw new DeviceException(Translate.ScalesHelper_Не_удалось_подключиться_к_весам, (IDevice) this);
    }

    public bool Disconnect()
    {
      if (!this._isConnection)
        return true;
      this._isConnection = false;
      return Wintec.ClosePort();
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      IntPtr num1 = Marshal.AllocHGlobal(20);
      int num2 = Wintec.Read_standard_stdcall(num1);
      LogHelper.Debug("Начинаю получать вес от весов: ");
      string stringAnsi = Marshal.PtrToStringAnsi(num1);
      LogHelper.Debug("Ответ весов: buf: " + num2.ToString() + "; str: " + stringAnsi);
      if (stringAnsi?.Substring(0, 1) == "0")
        throw new InvalidOperationException(Translate.Wintec_Вес_нестабилен);
      LogHelper.Debug("Вес стабилен");
      string str = stringAnsi?.Substring(1, 7)?.Replace(".", ",")?.Trim();
      weight = Convert.ToDecimal(str);
    }

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara()
    {
      IntPtr num = Marshal.AllocHGlobal(20);
      LogHelper.Debug(string.Format("send_tare. result: {0}; answer: {1}", (object) Wintec.TareSend(num), (object) Marshal.PtrToStringAnsi(num)));
    }

    public void Zero()
    {
      LogHelper.Debug(string.Format("send_zero. result: {0};", (object) Wintec.ZeroSend()));
    }

    public void TaraReset()
    {
      IntPtr num = Marshal.AllocHGlobal(20);
      LogHelper.Debug(string.Format("clear_tare. result: {0}; answer: {1}", (object) Wintec.ClearTare(num), (object) Marshal.PtrToStringAnsi(num)));
    }

    public string Name => nameof (Wintec);
  }
}
