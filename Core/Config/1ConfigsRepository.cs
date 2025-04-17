// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ConfigsRepository`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.IO;

#nullable disable
namespace Gbs.Core.Config
{
  public class ConfigsRepository<T> : IConfigRepository where T : class, IConfig, new()
  {
    private readonly string _fileName = typeof (T).Name;

    public T Get()
    {
      return (ConfigsCache.GetInstance().CacheDictionary[typeof (T)] ?? (IConfig) new T()) as T;
    }

    public void ReloadCache()
    {
      string path = ApplicationInfo.GetInstance().Paths.ConfigsPath + this._fileName + ".json";
      if (!File.Exists(path))
        return;
      string str = File.ReadAllText(path);
      T obj;
      try
      {
        obj = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore,
          ObjectCreationHandling = ObjectCreationHandling.Replace
        });
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка десериализации файла настроек при загрузке в кэш", false);
        string загрузкиФайлаНастроек = Translate.ОшибкаЗагрузкиФайлаНастроек;
        LogHelper.ShowErrorMgs(ex, загрузкиФайлаНастроек, LogHelper.MsgTypes.Notification);
        obj = new T();
      }
      ConfigsCache.GetInstance().CacheDictionary[typeof (T)] = (IConfig) obj;
    }

    public bool Save(T config)
    {
      try
      {
        string contents = JsonConvert.SerializeObject((object) config, Formatting.Indented, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore
        });
        if (!FileSystemHelper.ExistsOrCreateFolder(ApplicationInfo.GetInstance().Paths.ConfigsPath))
        {
          LogHelper.Error((Exception) new DirectoryNotFoundException(), "Не удалось найти или создать папку для хранения настроек");
          return false;
        }
        File.WriteAllText(ApplicationInfo.GetInstance().Paths.ConfigsPath + this._fileName + ".json", contents);
        ConfigsCache.GetInstance().CacheDictionary[typeof (T)] = (IConfig) config;
        return true;
      }
      catch (UnauthorizedAccessException ex)
      {
        string message = "Ошибка записи настроек в файл " + this._fileName;
        LogHelper.Error((Exception) ex, message, false, false);
        MessageBoxHelper.Error(Translate.ConfigFile_Не_удалось_записать_файл_настроек__Возможно__нет_доступа_к_папке_с_данными_);
        return false;
      }
      catch (Exception ex)
      {
        string message = "Ошибка записи настроек в файл " + this._fileName;
        LogHelper.Error(ex, message, false);
        return false;
      }
    }
  }
}
