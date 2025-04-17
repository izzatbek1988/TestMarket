// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Testing
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

#nullable disable
namespace Gbs.Core.Config
{
  public static class Testing
  {
    private static Testing.TestConfig _testConfig;

    public static Testing.TestConfig GetConfig()
    {
      if (Testing._testConfig != null)
        return !Testing._testConfig.UseTestConfig ? (Testing.TestConfig) null : Testing._testConfig;
      try
      {
        string path = ApplicationInfo.GetInstance().Paths.ConfigsPath + "testing.json";
        if (!File.Exists(path))
          return new Testing.TestConfig()
          {
            UseTestConfig = false
          };
        return JsonConvert.DeserializeObject<Testing.TestConfig>(File.ReadAllText(path), new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore,
          ObjectCreationHandling = ObjectCreationHandling.Replace
        });
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return new Testing.TestConfig()
        {
          UseTestConfig = false
        };
      }
    }

    public class TestConfig
    {
      public bool UseTestConfig { get; set; }

      public bool UseTestHosts { get; set; }

      public bool UseTestNotification { get; set; }
    }
  }
}
