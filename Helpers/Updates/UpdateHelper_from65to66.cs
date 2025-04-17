// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Updates.UpdateHelper_from65to66
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Gbs.Helpers.Updates
{
  internal static class UpdateHelper_from65to66
  {
    private static string GetUpdateFolder()
    {
      string md5Hash = CryptoHelper.GetMd5Hash(ApplicationInfo.GetInstance().Paths.ApplicationPath);
      return ApplicationInfo.GetInstance().Paths.UpdatesPath + md5Hash + "\\";
    }

    private static string GetVersionFilePath()
    {
      string lower = new ConfigsRepository<Settings>().Get().Other.UpdateConfig.VersionUpdate.ToString().ToLower();
      return UpdateHelper_from65to66.GetUpdateFolder() + lower + "_info.json";
    }

    private static string GetUpdateFilePath()
    {
      string lower = new ConfigsRepository<Settings>().Get().Other.UpdateConfig.VersionUpdate.ToString().ToLower();
      return UpdateHelper_from65to66.GetUpdateFolder() + lower + "_update.exe";
    }

    private static bool Is65version()
    {
      Version gbsVersion = ApplicationInfo.GetInstance().GbsVersion;
      bool flag = gbsVersion.Major == 6 && gbsVersion.Minor == 5;
      LogHelper.Debug(string.Format("Is 6.5: {0}", (object) flag));
      return flag;
    }

    internal static void DownLoadUpdate()
    {
      if (!UpdateHelper_from65to66.Is65version())
        return;
      Settings settings = new ConfigsRepository<Settings>().Get();
      if (settings.Other.UpdateConfig.UpdateType != OtherConfig.UpdateType.AutoUpdate)
        return;
      string str1 = string.Empty;
      foreach (string uriString in new List<string>()
      {
        "https://updates.app-pos.ru",
        "https://updates.app-pos.com"
      })
      {
        if (NetworkHelper.PingHost(new Uri(uriString).Host))
        {
          str1 = uriString;
          break;
        }
      }
      string str2 = str1 + "/gbsmarket6-5/";
      string updateFolder = UpdateHelper_from65to66.GetUpdateFolder();
      if (Directory.Exists(updateFolder))
        Directory.Delete(updateFolder, true);
      Directory.CreateDirectory(updateFolder);
      string lower = settings.Other.UpdateConfig.VersionUpdate.ToString().ToLower();
      string str3 = lower + "_info.json";
      string sourceFile1 = str2 + str3;
      string versionFilePath = UpdateHelper_from65to66.GetVersionFilePath();
      string destinationFile1 = versionFilePath;
      if (!NetworkHelper.DownloadFile(sourceFile1, destinationFile1))
      {
        LogHelper.Debug("Не удалось загрузить файл информации об обновлении до 6-6");
      }
      else
      {
        if (!File.Exists(versionFilePath))
          return;
        UpdateHelper_from65to66.UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateHelper_from65to66.UpdateInfo>(File.ReadAllText(versionFilePath));
        Version gbsVersion = ApplicationInfo.GetInstance().GbsVersion;
        if (gbsVersion != updateInfo.ForVersion)
          LogHelper.Debug("Обновление не предназначено для текущей версии");
        else if (gbsVersion >= updateInfo.NewVersion)
        {
          LogHelper.Debug("Текущая версия новее обновления");
        }
        else
        {
          string str4 = lower + "_update.exe";
          string sourceFile2 = str2 + str4;
          string updateFilePath = UpdateHelper_from65to66.GetUpdateFilePath();
          string destinationFile2 = updateFilePath;
          if (!NetworkHelper.DownloadFile(sourceFile2, destinationFile2))
          {
            LogHelper.Debug("Не удалось скачать файл обновления");
          }
          else
          {
            if (!File.Exists(updateFilePath))
              return;
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.UpdateHelper_Доступно_обновление,
              Text = Translate.UpdateHelper_DownloadUpdateArchive_Новая_версия_программы__0__доступна_и_готова_к_установке__Перезапустите_программу_для_установки_обновления_
            });
          }
        }
      }
    }

    internal static bool IsReadyToUpdate()
    {
      if (!UpdateHelper_from65to66.Is65version() || !Directory.Exists(UpdateHelper_from65to66.GetUpdateFolder()) || !File.Exists(UpdateHelper_from65to66.GetVersionFilePath()) || !File.Exists(UpdateHelper_from65to66.GetUpdateFilePath()))
        return false;
      UpdateHelper_from65to66.UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateHelper_from65to66.UpdateInfo>(File.ReadAllText(UpdateHelper_from65to66.GetVersionFilePath()));
      Version gbsVersion = ApplicationInfo.GetInstance().GbsVersion;
      if (gbsVersion != updateInfo.ForVersion || gbsVersion >= updateInfo.NewVersion)
        return false;
      LogHelper.Debug("Ready to update to 6-6");
      return true;
    }

    [Localizable(false)]
    internal static void DoUpdate()
    {
      if (!UpdateHelper_from65to66.IsReadyToUpdate())
        return;
      LogHelper.Debug("Start update 6-5 to 6-6");
      string applicationPath = ApplicationInfo.GetInstance().Paths.ApplicationPath;
      string contents = "@echo Update in progress. Please wait...\r\n@echo off\r\ntimeout /T 5 /NOBREAK\r\nstart /wait " + new FileInfo(UpdateHelper_from65to66.GetUpdateFilePath()).Name + " /S /D=" + applicationPath + "\r\nstart \"\" \"" + applicationPath + "\\market.exe\"";
      string str = Path.Combine(UpdateHelper_from65to66.GetUpdateFolder(), "update.cmd");
      File.WriteAllText(str, contents);
      LogHelper.Debug("Run update script");
      ProcessStartInfo processStartInfo = new ProcessStartInfo()
      {
        FileName = str,
        WorkingDirectory = new FileInfo(str).DirectoryName
      };
      new Process() { StartInfo = processStartInfo }.Start();
      System.Environment.Exit(0);
    }

    public class UpdateInfo
    {
      public Version ForVersion { get; set; }

      public Version NewVersion { get; set; }
    }
  }
}
