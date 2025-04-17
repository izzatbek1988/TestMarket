// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.GbsIdHelperV01
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Licenses;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Threading;

#nullable disable
namespace Gbs.Helpers
{
  public class GbsIdHelperV01 : IGbsIdHelper
  {
    public const string KeyName = "INFO";
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
        if (!string.IsNullOrEmpty(GbsIdHelperV01._gbsId))
          return GbsIdHelperV01._gbsId;
        string gbsId;
        try
        {
          gbsId = GbsIdHelperV01.ReadGbsIdFromRegistry();
        }
        catch (Exception ex)
        {
          GbsIdHelperV01._gbsId = "ERROR ---UNKNOWN GBS ID--- ERROR";
          MessageBoxHelper.Warning(Translate.LicenseHelper_GetGbsId_Не_удалось_получить_информацию_о_текущем_GBS_ID__Попробуйте_перезагрузить_компьютер__При_необходимости_обратитесь_в_службу_поддержки__);
          LogHelper.WriteError(ex, "Не удалось получить ИД из реестра");
          return GbsIdHelperV01._gbsId;
        }
        if (string.IsNullOrEmpty(gbsId))
        {
          LogHelper.Debug("Значение ИД из хранилща не указано, записываю новое");
          gbsId = (string) null;
          GbsIdHelperV01.WriteGbsIdToRegistry(ref gbsId);
        }
        GbsIdHelperV01._gbsId = gbsId;
        return gbsId;
      }
      catch (Exception ex)
      {
        throw new Exception(Translate.LicenseHelper_GetGbsId_Не_удалось_получить_значение_GBS_ID, ex);
      }
    }

    public void DeleteGbsIdFromRegistry() => GbsIdHelperV01.RegistryKey.DeleteValue("INFO");

    private static string ReadGbsIdFromRegistry()
    {
      LogHelper.Debug("Получаю ИД из хранилища");
      object obj = GbsIdHelperV01.RegistryKey.GetValue("INFO");
      if (obj == null)
      {
        LogHelper.Debug("ЗНачения ИД в хранилище не существует");
        return (string) null;
      }
      string cipherText = obj.ToString();
      LogHelper.Debug("Reg value: " + cipherText);
      GbsIdHelperV01.GbsIdRegistry gbsIdRegistry = JsonConvert.DeserializeObject<GbsIdHelperV01.GbsIdRegistry>(CryptoHelper.StringCrypter.Decrypt(cipherText));
      GbsIdHelperV01.RegistryKey.Close();
      string str = HardwareInfo.HashInfo();
      bool flag = false;
      for (int index = 0; index < 3; ++index)
      {
        LogHelper.Trace(string.Format("Проверка HW Hash {0}", (object) index));
        if (gbsIdRegistry.HwInfoHash != str)
        {
          Thread.Sleep(2000);
          flag = false;
          HardwareInfo.UpdateInfo();
          str = HardwareInfo.HashInfo();
        }
        else
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        LogHelper.Debug("Hardware conflict");
        return (string) null;
      }
      string gbsId = gbsIdRegistry.GbsId;
      if (string.IsNullOrEmpty(gbsIdRegistry.DomainSid) && !string.IsNullOrEmpty(HardwareInfo.GetDomainSid()))
      {
        LogHelper.Debug("Update domain information");
        GbsIdHelperV01.WriteGbsIdToRegistry(ref gbsId);
      }
      else if (gbsIdRegistry.DomainSid != HardwareInfo.GetDomainSid())
      {
        LogHelper.Debug("Software conflict");
        return (string) null;
      }
      Other.ConsoleWrite("info: " + gbsIdRegistry.ToJsonString());
      LogHelper.Debug("Возвращаю ИД из хранилища");
      return gbsId;
    }

    private static void WriteGbsIdToRegistry(ref string gbsId)
    {
      if (string.IsNullOrEmpty(gbsId))
        gbsId = GbsIdHelperV01.GetRandomId();
      GbsIdHelperV01.RegistryKey.SetValue("INFO", (object) CryptoHelper.StringCrypter.Encrypt(JsonConvert.SerializeObject((object) new GbsIdHelperV01.GbsIdRegistry()
      {
        GbsId = gbsId,
        HwInfoHash = HardwareInfo.HashInfo(),
        DomainSid = HardwareInfo.GetDomainSid()
      })));
      GbsIdHelperV01.RegistryKey.Close();
      LogHelper.Debug("В хранилище записан ИД: " + gbsId);
    }

    public static string GetRandomId() => Guid.NewGuid().ToString().Replace("-", "").ToUpper();

    private class GbsIdRegistry
    {
      public string GbsId { get; set; }

      public string HwInfoHash { get; set; }

      public string DomainSid { get; set; }
    }
  }
}
