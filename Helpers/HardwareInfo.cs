// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HardwareInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.ComponentModel;
using System.Management;
using System.Security.Principal;

#nullable disable
namespace Gbs.Helpers
{
  public static class HardwareInfo
  {
    [Localizable(false)]
    private static string _stableInfo = string.Empty;

    [Localizable(false)]
    public static void GetRamValue()
    {
      try
      {
        foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT TotalVisibleMemorySize,FreePhysicalMemory FROM Win32_OperatingSystem").Get())
        {
          Decimal num = Math.Round((Decimal) Convert.ToUInt64(managementObject["TotalVisibleMemorySize"]) / 1024M / 1024M, 2);
          LogHelper.Debug("Total RAM = " + num.ToString() + " GB");
          LogHelper.Debug("Busy RAM = " + Math.Round(num - (Decimal) Convert.ToUInt64(managementObject["FreePhysicalMemory"]) / 1024M / 1024M, 2).ToString() + " GB");
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось получить размер ОЗУ", false);
      }
    }

    public static void UpdateInfo() => HardwareInfo._stableInfo = string.Empty;

    public static string GetDomainSid()
    {
      try
      {
        string str = WindowsIdentity.GetCurrent().User?.AccountDomainSid.Value;
        if (str == null)
          return string.Empty;
        string[] strArray = str.Split('-');
        return strArray[4] + strArray[5] + strArray[6];
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения domain sid");
        return string.Empty;
      }
    }

    public static string HashInfo()
    {
      LogHelper.Debug("Get current hardware hash");
      return CryptoHelper.GetMd5Hash(HardwareInfo.StableInfo()).ToUpper();
    }

    public static string StableInfo()
    {
      if (HardwareInfo._stableInfo == string.Empty)
        HardwareInfo._stableInfo = "MB: " + HardwareInfo.MotherBoardId() + "\r\nBIOS: " + HardwareInfo.BiosId() + "\r\nCPU: " + HardwareInfo.CpuId();
      return HardwareInfo._stableInfo;
    }

    private static string Identifier(string wmiClass, string wmiProperty)
    {
      string str = "";
      foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances())
      {
        if (!(str != ""))
        {
          try
          {
            str = instance[wmiProperty]?.ToString();
            break;
          }
          catch
          {
          }
        }
      }
      return str;
    }

    [Localizable(false)]
    private static string CpuId()
    {
      string str1 = HardwareInfo.Identifier("Win32_Processor", "UniqueId");
      if (str1 != "")
        return str1;
      string str2 = HardwareInfo.Identifier("Win32_Processor", "ProcessorId");
      if (str2 != "")
        return str2;
      string str3 = HardwareInfo.Identifier("Win32_Processor", "Name");
      if (str3 == "")
        str3 = HardwareInfo.Identifier("Win32_Processor", "Manufacturer");
      return str3;
    }

    [Localizable(false)]
    public static string CpuManufacturer()
    {
      return HardwareInfo.Identifier("Win32_Processor", "Manufacturer");
    }

    [Localizable(false)]
    public static string CpuName() => HardwareInfo.Identifier("Win32_Processor", "Name");

    [Localizable(false)]
    private static string BiosId()
    {
      return HardwareInfo.Identifier("Win32_BIOS", "Manufacturer") + HardwareInfo.Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + HardwareInfo.Identifier("Win32_BIOS", "IdentificationCode") + HardwareInfo.Identifier("Win32_BIOS", "SerialNumber") + HardwareInfo.Identifier("Win32_BIOS", "ReleaseDate") + HardwareInfo.Identifier("Win32_BIOS", "Version");
    }

    [Localizable(false)]
    private static string MotherBoardId()
    {
      return HardwareInfo.Identifier("Win32_BaseBoard", "Model") + HardwareInfo.Identifier("Win32_BaseBoard", "Manufacturer") + HardwareInfo.Identifier("Win32_BaseBoard", "Name") + HardwareInfo.Identifier("Win32_BaseBoard", "SerialNumber");
    }

    [Localizable(false)]
    public static string OsVersion()
    {
      OperatingSystem osVersion = System.Environment.OSVersion;
      string str = "";
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().GetEnumerator())
      {
        if (enumerator.MoveNext())
          str = enumerator.Current["Caption"].ToString();
      }
      if (str == "")
        return str;
      if (osVersion.ServicePack != "")
        str = str + " " + osVersion.ServicePack;
      return str + " " + (System.Environment.Is64BitOperatingSystem ? "64" : "32") + "-bit";
    }
  }
}
