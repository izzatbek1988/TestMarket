// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ConfigsCache
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class ConfigsCache
  {
    private static ConfigsCache _configsCache;

    public Dictionary<Type, IConfig> CacheDictionary { get; } = new Dictionary<Type, IConfig>()
    {
      {
        typeof (Devices),
        (IConfig) new Devices()
      },
      {
        typeof (Settings),
        (IConfig) new Settings()
      },
      {
        typeof (DataBase),
        (IConfig) new DataBase()
      },
      {
        typeof (Cafe),
        (IConfig) new Cafe()
      },
      {
        typeof (FilterOptions),
        (IConfig) new FilterOptions()
      },
      {
        typeof (Integrations),
        (IConfig) new Integrations()
      }
    };

    private ConfigsCache()
    {
    }

    public static ConfigsCache GetInstance()
    {
      try
      {
        if (ConfigsCache._configsCache != null)
          return ConfigsCache._configsCache;
        ConfigsCache._configsCache = new ConfigsCache();
        ConfigsCache._configsCache.ReloadAllCache();
        return ConfigsCache._configsCache;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
        return (ConfigsCache) null;
      }
    }

    public void ReloadAllCache()
    {
      foreach (IConfigRepository configRepository in new List<IConfigRepository>()
      {
        (IConfigRepository) new ConfigsRepository<Devices>(),
        (IConfigRepository) new ConfigsRepository<Settings>(),
        (IConfigRepository) new ConfigsRepository<DataBase>(),
        (IConfigRepository) new ConfigsRepository<FilterOptions>(),
        (IConfigRepository) new ConfigsRepository<Integrations>(),
        (IConfigRepository) new ConfigsRepository<Cafe>()
      })
        configRepository.ReloadCache();
    }
  }
}
