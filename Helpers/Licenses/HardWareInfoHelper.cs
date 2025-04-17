// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Licenses.HardWareInfoHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Security.Principal;

#nullable disable
namespace Gbs.Helpers.Licenses
{
  public class HardWareInfoHelper
  {
    private static HardWareInfoHelper.HardWare _info;

    public HardWareInfoHelper.HardWare GetInfo()
    {
      if (HardWareInfoHelper._info != null)
        return HardWareInfoHelper._info;
      HardWareInfoHelper._info = new HardWareInfoHelper.HardWare()
      {
        DomainSid = HardWareInfoHelper.GetDomainSid(),
        MachineGuid = HardWareInfoHelper.GetMachineGuid(),
        CpuInfo = HardWareInfoHelper.GetCpuInfo(),
        BiosInfo = HardWareInfoHelper.GetBiosInfo(),
        DiskInfo = HardWareInfoHelper.GetDiskInfo(),
        MotherBoardInfo = HardWareInfoHelper.GetMotherBoardInfo(),
        NetworkAdapterInfo = HardWareInfoHelper.GetNetworkAdapterInfo(),
        OSInfo = HardWareInfoHelper.GetOsInfo(),
        VideoInfo = HardWareInfoHelper.GetVideoInfo()
      };
      return HardWareInfoHelper._info;
    }

    private static string GetDomainSid()
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

    private static string GetMachineGuid()
    {
      try
      {
        string name1 = "SOFTWARE\\Microsoft\\Cryptography";
        string name2 = "MachineGuid";
        using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
        {
          using (RegistryKey registryKey2 = registryKey1.OpenSubKey(name1))
            return ((registryKey2 != null ? registryKey2.GetValue(name2) : throw new KeyNotFoundException(string.Format("Key Not Found: {0}", (object) name1))) ?? throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", (object) name2))).ToString();
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения MachineGuid");
        throw;
      }
    }

    private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
    {
      string str = "";
      ManagementObjectCollection instances = new ManagementClass(wmiClass).GetInstances();
      string logMessage = string.Empty;
      foreach (ManagementObject managementObject in instances)
      {
        if (!(managementObject[wmiMustBeTrue].ToString() != "True"))
        {
          if (!(str != ""))
          {
            try
            {
              if (managementObject[wmiProperty] == null)
              {
                LogHelper.WriteError(new Exception("Значение для идентификатора " + wmiProperty + " пустое"));
                break;
              }
              str = managementObject[wmiProperty].ToString();
              break;
            }
            catch (Exception ex)
            {
              logMessage = logMessage + ex?.ToString() + "\r\n\r\n";
            }
          }
        }
      }
      if (logMessage.Length > 0)
        LogHelper.WriteError(new Exception(), logMessage, false);
      return str;
    }

    private static string Identifier(string wmiClass, string wmiProperty)
    {
      string str = "";
      ManagementObjectCollection instances = new ManagementClass(wmiClass).GetInstances();
      string logMessage = string.Empty;
      foreach (ManagementObject managementObject in instances)
      {
        if (!(str != ""))
        {
          try
          {
            str = managementObject[wmiProperty]?.ToString();
            break;
          }
          catch (Exception ex)
          {
            logMessage = logMessage + ex?.ToString() + "\r\n\r\n";
          }
        }
      }
      if (logMessage.Length > 0)
        LogHelper.WriteError(new Exception(), logMessage, false);
      return str;
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.CPU GetCpuInfo()
    {
      return new HardWareInfoHelper.HardWare.CPU()
      {
        UniqueId = HardWareInfoHelper.Identifier("Win32_Processor", "UniqueId"),
        ProcessorId = HardWareInfoHelper.Identifier("Win32_Processor", "ProcessorId"),
        Name = HardWareInfoHelper.Identifier("Win32_Processor", "Name"),
        Manufacturer = HardWareInfoHelper.Identifier("Win32_Processor", "Manufacturer")
      };
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.BIOS GetBiosInfo()
    {
      return new HardWareInfoHelper.HardWare.BIOS()
      {
        Manufacturer = HardWareInfoHelper.Identifier("Win32_BIOS", "Manufacturer"),
        SMBIOSBIOSVersion = HardWareInfoHelper.Identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
        IdentificationCode = HardWareInfoHelper.Identifier("Win32_BIOS", "IdentificationCode"),
        SerialNumber = HardWareInfoHelper.Identifier("Win32_BIOS", "SerialNumber"),
        ReleaseDate = HardWareInfoHelper.Identifier("Win32_BIOS", "ReleaseDate"),
        Version = HardWareInfoHelper.Identifier("Win32_BIOS", "Version")
      };
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.Disk GetDiskInfo()
    {
      return new HardWareInfoHelper.HardWare.Disk()
      {
        Model = HardWareInfoHelper.Identifier("Win32_DiskDrive", "Model"),
        Manufacturer = HardWareInfoHelper.Identifier("Win32_DiskDrive", "Manufacturer"),
        Signature = HardWareInfoHelper.Identifier("Win32_DiskDrive", "Signature"),
        TotalHeads = HardWareInfoHelper.Identifier("Win32_DiskDrive", "TotalHeads")
      };
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.MotherBoard GetMotherBoardInfo()
    {
      return new HardWareInfoHelper.HardWare.MotherBoard()
      {
        Model = HardWareInfoHelper.Identifier("Win32_BaseBoard", "Model"),
        Manufacturer = HardWareInfoHelper.Identifier("Win32_BaseBoard", "Manufacturer"),
        Name = HardWareInfoHelper.Identifier("Win32_BaseBoard", "Name"),
        SerialNumber = HardWareInfoHelper.Identifier("Win32_BaseBoard", "SerialNumber")
      };
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.Video GetVideoInfo()
    {
      return new HardWareInfoHelper.HardWare.Video()
      {
        Name = HardWareInfoHelper.Identifier("Win32_VideoController", "Name")
      };
    }

    [Localizable(false)]
    private static HardWareInfoHelper.HardWare.NetworkAdapter GetNetworkAdapterInfo()
    {
      return new HardWareInfoHelper.HardWare.NetworkAdapter()
      {
        MAC = HardWareInfoHelper.Identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled")
      };
    }

    private static HardWareInfoHelper.HardWare.OperationSystem GetOsInfo()
    {
      return new HardWareInfoHelper.HardWare.OperationSystem()
      {
        Version = HardWareInfoHelper.OsVersion(),
        InstallDate = HardWareInfoHelper.OsInstallDate()
      };
    }

    [Localizable(false)]
    private static DateTime OsInstallDate()
    {
      DateTime dateTime = DateTime.MinValue;
      foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get())
        dateTime = ManagementDateTimeConverter.ToDateTime(Convert.ToString(managementBaseObject["InstallDate"]));
      return dateTime;
    }

    [Localizable(false)]
    private static string OsVersion()
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

    public class HardWare
    {
      public string DomainSid { get; set; }

      public string MachineGuid { get; set; }

      public HardWareInfoHelper.HardWare.CPU CpuInfo { get; set; }

      public HardWareInfoHelper.HardWare.BIOS BiosInfo { get; set; }

      public HardWareInfoHelper.HardWare.Disk DiskInfo { get; set; }

      public HardWareInfoHelper.HardWare.MotherBoard MotherBoardInfo { get; set; }

      public HardWareInfoHelper.HardWare.Video VideoInfo { get; set; }

      public HardWareInfoHelper.HardWare.NetworkAdapter NetworkAdapterInfo { get; set; }

      public HardWareInfoHelper.HardWare.OperationSystem OSInfo { get; set; }

      public class CPU
      {
        public string UniqueId { get; set; }

        public string ProcessorId { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }
      }

      public class BIOS
      {
        public string Manufacturer { get; set; }

        public string SMBIOSBIOSVersion { get; set; }

        public string IdentificationCode { get; set; }

        public string SerialNumber { get; set; }

        public string ReleaseDate { get; set; }

        public string Version { get; set; }
      }

      public class Disk
      {
        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string Signature { get; set; }

        public string TotalHeads { get; set; }
      }

      public class MotherBoard
      {
        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string Name { get; set; }

        public string SerialNumber { get; set; }
      }

      public class Video
      {
        public string Name { get; set; }
      }

      public class NetworkAdapter
      {
        public string MAC { get; set; }
      }

      public class OperationSystem
      {
        public string Version { get; set; }

        public DateTime InstallDate { get; set; }
      }
    }
  }
}
