// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Vendor
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Newtonsoft.Json;
using System.IO;

#nullable disable
namespace Gbs.Core.Config
{
  internal class Vendor
  {
    private static VendorConfig _config;
    private static bool noVendor;

    public static VendorConfig GetConfig()
    {
      if (Vendor.noVendor)
        return (VendorConfig) null;
      if (Vendor._config != null)
        return Vendor._config;
      string applicationPath = ApplicationInfo.GetInstance().Paths.ApplicationPath;
      string str1 = applicationPath + "\\\\vendor.dll";
      if (!File.Exists(str1))
      {
        Vendor.noVendor = true;
        return (VendorConfig) null;
      }
      string path = applicationPath + "\\\\vendor.hash";
      if (!File.Exists(path))
      {
        Vendor.noVendor = true;
        return (VendorConfig) null;
      }
      string str2 = File.ReadAllText(path);
      if (CryptoHelper.GetSHA256Hash(str1, true) != str2)
      {
        Vendor.noVendor = true;
        return (VendorConfig) null;
      }
      Vendor._config = JsonConvert.DeserializeObject<VendorConfig>(File.ReadAllText(str1));
      return Vendor._config;
    }
  }
}
