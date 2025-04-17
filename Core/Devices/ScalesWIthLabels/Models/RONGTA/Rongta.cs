// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.Rongta
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class Rongta : IScalesWIthLabels, IDevice
  {
    private const string PathDriver = "dll\\label_scale\\rongta\\";

    private Gbs.Core.Config.Devices DevicesConfig { get; }

    public Rongta(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(this.DevicesConfig.ScaleWithLable.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        if (onlyDriverLoad)
          return true;
        string str = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\rongta\\system.cfg");
        if (!FileSystemHelper.CheckFileExistWithMsg(str) || !FileSystemHelper.CheckFileExistWithMsg(str))
          return false;
        DeviceConnection connection = this.DevicesConfig.ScaleWithLable.Connection;
        LogHelper.Debug(connection.ToJsonString(true));
        switch (connection.ConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
            throw new DeviceException(Translate.Rongta_Connect_Не_указан_тип_подключения, (IDevice) this);
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            DeviceHelper.CheckComPortExists(connection.ComPort.PortName, (IDevice) this);
            int num1 = Rtsdrv.rtscaleConnect("", str, 22, connection.ComPort.PortName, connection.ComPort.Speed);
            if (num1 >= 0)
              return true;
            LogHelper.Debug("Connect fail " + num1.ToString());
            return false;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            int num2 = Rtsdrv.rtscaleConnectEx("", str, connection.LanPort.UrlAddress);
            if (num2 >= 0)
              return true;
            LogHelper.Debug("Connect fail " + num2.ToString());
            return false;
          default:
            throw new DeviceException(Translate.LeoCas_Не_указан_способ_подключения, (IDevice) this);
        }
      }
      catch
      {
        throw new DeviceException(Translate.Rongta_Connect_Не_удалось_подключиться_к_весам__проверьте_параметры_подключения, (IDevice) this);
      }
    }

    public bool Disconnect()
    {
      DeviceConnection connection = this.DevicesConfig.ScaleWithLable.Connection;
      switch (connection.ConnectionType)
      {
        case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
          return Rtsdrv.rtscaleDisConnect(22) == 0;
        case GlobalDictionaries.Devices.ConnectionTypes.Lan:
          return Rtsdrv.rtscaleDisConnectEx(connection.LanPort.UrlAddress) == 0;
        default:
          return true;
      }
    }

    public int SendGood(List<GoodForWith> goods)
    {
      string[] source = this.DevicesConfig.BarcodeScanner.Prefixes.WeightGoods.Split(GlobalDictionaries.SplitArr);
      int num1 = !((IEnumerable<string>) source).Any<string>() ? 21 : Convert.ToInt32(source[0]);
      Rtsdrv.PLU[] plu = new Rtsdrv.PLU[4];
      int num2 = 0;
      int index1 = 0;
      while (index1 < goods.Count)
      {
        int num3 = 0;
        for (int index2 = 0; index2 < 4 && index1 < goods.Count; ++index2)
        {
          plu[index2].Name = goods[index1].Name;
          plu[index2].LFCode = goods[index1].Plu;
          plu[index2].UnitPrice = Convert.ToInt32(goods[index1].Price) * (this.DevicesConfig.ScaleWithLable.CorrectPriceForRongta ? 100 : 1);
          plu[index2].Code = goods[index1].Plu.ToString();
          plu[index2].WeightUnit = 4;
          plu[index2].BarCode = 7;
          plu[index2].Deptment = num1;
          ++index1;
          ++num3;
        }
        if (Rtsdrv.rtscaleTransferPLUCluster(plu) == 0)
          num2 += num3;
      }
      return this.SendHotKey() ? num2 : 0;
    }

    private bool SendHotKey()
    {
      int[] HotkeyTable = new int[84];
      for (int index = 0; index < 84; ++index)
        HotkeyTable[index] = 10001 + index;
      if (Rtsdrv.rtscaleTransferHotkey(HotkeyTable, 0) != 0)
        return false;
      for (int index = 0; index < 84; ++index)
        HotkeyTable[index] = 10085 + index;
      if (Rtsdrv.rtscaleTransferHotkey(HotkeyTable, 1) != 0)
        return false;
      for (int index = 0; index < 56; ++index)
        HotkeyTable[index] = 10169 + index;
      return Rtsdrv.rtscaleTransferHotkey(HotkeyTable, 2) == 0;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public string Name => Translate.Devices_ScaleTypesDictionary_Rongta;
  }
}
