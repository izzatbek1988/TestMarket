// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.Bta
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class Bta : IScale, IDevice
  {
    private const string PathDriver = "dll\\scale\\bta\\";

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public string Name => "BTA";

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Bta(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      if (!FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\scale\\bta\\UniScalesDriver.exe")))
        throw new DeviceException(Translate.Bta_Connect_Не_удалось_найти_файл_драйвера_UniScalesDriver_exe, (IDevice) this);
    }

    public bool Disconnect() => true;

    public void GetWeight(out Decimal weight, Decimal price)
    {
      LogHelper.Debug("Начинаем читать вес BTA");
      try
      {
        Process.Start(new ProcessStartInfo()
        {
          FileName = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\scale\\bta\\UniScalesDriver.exe"),
          WindowStyle = ProcessWindowStyle.Hidden,
          Arguments = "1 " + this.DevicesConfig.Scale.ComPort.PortName + " 3"
        })?.WaitForExit();
        Thread.Sleep(50);
        string path = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\scale\\bta\\result.txt");
        if (!File.Exists(path))
        {
          LogHelper.Debug("Файл result.txt несущесвует, не можем прочитать вес");
          weight = 0M;
        }
        else
        {
          LogHelper.Debug(File.ReadAllText(path));
          string[] strArray = File.ReadAllLines(path);
          if (strArray[0] != "0")
          {
            LogHelper.Debug("Во время получения веса произошла ошибка " + strArray[0] + " (" + strArray[1] + ")");
            weight = 0M;
          }
          else if (Decimal.TryParse(strArray[3], NumberStyles.AllowDecimalPoint, (IFormatProvider) new NumberFormatInfo(), out weight))
            LogHelper.Debug("Получили вес с весов " + weight.ToString());
          else
            weight = 0M;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось получить вес с весов", false);
        weight = 0M;
      }
    }

    public void Tara()
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\scale\\bta\\UniScalesDriver.exe"),
        WindowStyle = ProcessWindowStyle.Hidden,
        Arguments = "1 " + this.DevicesConfig.Scale.ComPort.PortName + " 1"
      })?.WaitForExit();
      Thread.Sleep(50);
    }

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => this.Tara();
  }
}
