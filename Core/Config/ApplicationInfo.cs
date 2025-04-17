// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ApplicationInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Config
{
  public class ApplicationInfo
  {
    private static ApplicationInfo _instance;

    private ApplicationInfo()
    {
    }

    public static ApplicationInfo GetInstance()
    {
      if (ApplicationInfo._instance != null)
        return ApplicationInfo._instance;
      ApplicationInfo._instance = new ApplicationInfo()
      {
        Paths = new ApplicationInfo.PathsInfo()
      };
      ApplicationInfo._instance.SetApplicationPath();
      ApplicationInfo._instance.SetDataPath();
      ApplicationInfo._instance.SetVersion();
      return ApplicationInfo._instance;
    }

    private void SetDataPath()
    {
      if (DevelopersHelper.IsUnitTest())
        ApplicationInfo.SetTestConfigs();
      if (ApplicationInfo._instance.Paths.ApplicationPath == null)
        return;
      VendorConfig config = Vendor.GetConfig();
      ApplicationInfo._instance.Paths.DataPath = config != null ? System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\" + config.ApplicationName + "\\data\\" : "C:\\ProgramData\\F-Lab\\GBS.Market\\6\\";
      string path = Path.Combine(ApplicationInfo._instance.Paths.ApplicationPath, "AppConfig.conf");
      if (!File.Exists(path))
        return;
      ApplicationInfo.AppConfig appConfig = JsonConvert.DeserializeObject<ApplicationInfo.AppConfig>(File.ReadAllText(path));
      if (!FileSystemHelper.ExistsOrCreateFolder(appConfig.DataPath))
      {
        MessageBoxHelper.Error(string.Format(Translate.ApplicationInfo_, (object) appConfig.DataPath));
        System.Environment.Exit(0);
      }
      ApplicationInfo._instance.Paths.DataPath = appConfig.DataPath;
    }

    private static void SetTestConfigs()
    {
      string path = "C:\\TestDb\\test_data\\";
      Directory.CreateDirectory(path);
      ConfigsRepository<DataBase> configsRepository = new ConfigsRepository<DataBase>();
      DataBase config = configsRepository.Get();
      config.Connection = new DataBase.DbConnection()
      {
        Path = path + "main.fdb"
      };
      configsRepository.Save(config);
    }

    private void SetApplicationPath()
    {
      if (DevelopersHelper.IsUnitTest())
      {
        ApplicationInfo._instance.Paths.ApplicationPath = string.Empty;
      }
      else
      {
        try
        {
          string fileName = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
          ApplicationInfo._instance.Paths.ApplicationPath = new FileInfo(fileName).DirectoryName;
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка получения пути к папке с программой");
        }
      }
    }

    public Version GbsVersion { get; private set; }

    [Localizable(false)]
    public string AppVersion
    {
      get
      {
        string str1;
        if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Ukraine)
        {
          str1 = this.GbsVersion.ToString();
        }
        else
        {
          DateTime dateTime = new FileInfo(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "market.exe")).CreationTime;
          dateTime = dateTime.Date;
          str1 = dateTime.ToString("yyyy.MM.dd.") + this.GbsVersion.Revision.ToString();
        }
        string str2 = System.Environment.Is64BitProcess ? "x64" : "x86";
        return str1 + " (" + str2 + ")";
      }
      set => throw new NotImplementedException();
    }

    private void SetVersion()
    {
      if (DevelopersHelper.IsUnitTest())
      {
        this.GbsVersion = new Version(1, 22, 333, 4444);
      }
      else
      {
        try
        {
          this.GbsVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка получения версии приложения", false);
          this.GbsVersion = new Version(6, 0);
        }
      }
    }

    public ApplicationInfo.PathsInfo Paths { get; private set; }

    public class AppConfig
    {
      public string DataPath { get; set; }
    }

    public class PathsInfo
    {
      public string ApplicationPath { get; set; }

      public string DataPath { get; set; }

      public string LogsPath => this.DataPath + "Logs\\";

      public string CrptLogsPath => this.DataPath + "CRPT_logs\\";

      public string ConfigsPath => this.DataPath + "Configs\\";

      public string UpdatesPath => this.DataPath + "Updates\\";

      public string ArchivesPath => this.DataPath + "Archives\\";

      public string ArchivesFromPointsMovePath => this.ArchivesPath + "FromPointsMove\\";

      public string ArchivesFromHomePath => this.ArchivesPath + "FromHome\\";

      public string GoodCatalogExcelTemplatesPath
      {
        get => this.ConfigsPath + "ExcelTemplates\\ImportToCatalog\\";
      }

      public string WaybillExcelTemplatesPath
      {
        get => this.ConfigsPath + "ExcelTemplates\\ImportToWaybill\\";
      }

      public string TemplatesFrPath
      {
        get => new ConfigsRepository<Settings>().Get().Interface.TemplatesFrPath;
      }

      public string AutoSavePath => this.DataPath + "AutoSave\\";

      public string CachePath => this.DataPath + "Cache\\";
    }
  }
}
