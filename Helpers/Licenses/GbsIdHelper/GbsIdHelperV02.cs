// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Licenses.GbsIdHelperV02
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Helpers.Licenses
{
  public class GbsIdHelperV02 : IGbsIdHelper
  {
    private const string KeyName = "INFOv02";
    private static string _gbsId;

    private static RegistryKey RegistryKey
    {
      get => Registry.CurrentUser.CreateSubKey("Software\\F-Lab\\GBS.Market\\6");
    }

    public string GetGbsIdWithPrefix() => "6:" + this.GetGbsId();

    public string GetGbsId()
    {
      try
      {
        if (!string.IsNullOrEmpty(GbsIdHelperV02._gbsId))
          return GbsIdHelperV02._gbsId;
        GbsIdHelperV02.MigrateFromV01();
        string gbsId;
        try
        {
          gbsId = GbsIdHelperV02.ReadGbsIdFromRegistry();
        }
        catch (Exception ex)
        {
          gbsId = "ERROR ---UNKNOWN GBS ID--- ERROR";
          MessageBoxHelper.Warning(Translate.LicenseHelper_GetGbsId_Не_удалось_получить_информацию_о_текущем_GBS_ID__Попробуйте_перезагрузить_компьютер__При_необходимости_обратитесь_в_службу_поддержки__);
          LogHelper.WriteError(ex, "Не удалось получить ИД из реестра");
        }
        GbsIdHelperV02._gbsId = gbsId;
        return gbsId;
      }
      catch (Exception ex)
      {
        throw new Exception(Translate.LicenseHelper_GetGbsId_Не_удалось_получить_значение_GBS_ID, ex);
      }
    }

    public void DeleteGbsIdFromRegistry() => GbsIdHelperV02.RegistryKey.DeleteValue("INFOv02");

    private static bool CheckHardwareData(GbsIdHelperV02.RegData data)
    {
      HardWareInfoHelper.HardWare info = new HardWareInfoHelper().GetInfo();
      HardWareInfoHelper.HardWare hw = data.HW;
      int num = GbsIdHelperV02.CheckHwString(info.DomainSid, hw.DomainSid) + GbsIdHelperV02.CheckHwString(info.MachineGuid, hw.MachineGuid) + GbsIdHelperV02.CheckHwString(info.CpuInfo.UniqueId, hw.CpuInfo.UniqueId) + GbsIdHelperV02.CheckHwString(info.CpuInfo.ProcessorId, hw.CpuInfo.ProcessorId) + GbsIdHelperV02.CheckHwString(info.CpuInfo.Manufacturer, hw.CpuInfo.Manufacturer) + GbsIdHelperV02.CheckHwString(info.CpuInfo.Name, hw.CpuInfo.Name) + GbsIdHelperV02.CheckHwString(info.MotherBoardInfo.Manufacturer, hw.MotherBoardInfo.Manufacturer) + GbsIdHelperV02.CheckHwString(info.MotherBoardInfo.Model, hw.MotherBoardInfo.Model) + GbsIdHelperV02.CheckHwString(info.MotherBoardInfo.SerialNumber, hw.MotherBoardInfo.SerialNumber) + GbsIdHelperV02.CheckHwString(info.MotherBoardInfo.Name, hw.MotherBoardInfo.Name) + GbsIdHelperV02.CheckHwString(info.BiosInfo.Manufacturer, hw.BiosInfo.Manufacturer) + GbsIdHelperV02.CheckHwString(info.BiosInfo.SerialNumber, hw.BiosInfo.SerialNumber) + GbsIdHelperV02.CheckHwString(info.OSInfo.Version, hw.OSInfo.Version);
      LogHelper.Trace(string.Format("HW Fee value: {0}", (object) num));
      return num <= 4;
    }

    private static int CheckHwString(string string1, string string2)
    {
      if (string1 == string2)
        return 0;
      if (string1.IsNullOrEmpty() || string2.IsNullOrEmpty())
        return 1;
      return string1 != string2 ? 2 : 0;
    }

    private static void MigrateFromV01()
    {
      if (GbsIdHelperV02.RegistryKey.GetValue("INFO") == null)
        LogHelper.Trace("old registry value is null");
      else if (GbsIdHelperV02.RegistryKey.GetValue("INFOv02") != null)
      {
        LogHelper.Trace("New registry value is not null. Migration canceled");
      }
      else
      {
        LogHelper.Trace("Key migration started");
        string gbsId = new GbsIdHelperV01().GetGbsId();
        GbsIdHelperV02.WriteGbsIdToRegistry(ref gbsId);
        LogHelper.Trace("Kye migration done");
      }
    }

    private static void WriteGbsIdToRegistry(ref string gbsId)
    {
      if (string.IsNullOrEmpty(gbsId))
      {
        LogHelper.Trace("id is empty, creating new");
        gbsId = GbsIdHelperV01.GetRandomId();
      }
      HardWareInfoHelper.HardWare info = new HardWareInfoHelper().GetInfo();
      GbsIdHelperV02.WriteDataToRegistry(new GbsIdHelperV02.RegData()
      {
        HW = info,
        GbsId = gbsId
      });
    }

    private static void WriteDataToRegistry(GbsIdHelperV02.RegData data)
    {
      GbsIdHelperV02.RegistryKey.SetValue("INFOv02", (object) CryptoHelper.StringCrypter.Encrypt(JsonConvert.SerializeObject((object) data)));
      GbsIdHelperV02.RegistryKey.Close();
      LogHelper.Debug("В хранилище записаны данные");
    }

    private static string ReadGbsIdFromRegistry()
    {
      GbsIdHelperV02.RegData data = GbsIdHelperV02.ReadDataFromRegistry();
      if (data != null && GbsIdHelperV02.CheckHardwareData(data))
        return data.GbsId;
      string gbsId = (string) null;
      GbsIdHelperV02.WriteGbsIdToRegistry(ref gbsId);
      return GbsIdHelperV02.ReadDataFromRegistry().GbsId;
    }

    public static GbsIdHelperV02.RegData ReadDataFromRegistry()
    {
      LogHelper.Debug("Получаю данные из хранилища");
      object obj = GbsIdHelperV02.RegistryKey.GetValue("INFOv02");
      if (obj == null)
      {
        LogHelper.Debug("Даных в хранилище не существует");
        return (GbsIdHelperV02.RegData) null;
      }
      GbsIdHelperV02.RegData regData = JsonConvert.DeserializeObject<GbsIdHelperV02.RegData>(CryptoHelper.StringCrypter.Decrypt(obj.ToString()));
      GbsIdHelperV02.RegistryKey.Close();
      return regData;
    }

    public class RegData
    {
      public HardWareInfoHelper.HardWare HW { get; set; }

      public string GbsId { get; set; }
    }
  }
}
