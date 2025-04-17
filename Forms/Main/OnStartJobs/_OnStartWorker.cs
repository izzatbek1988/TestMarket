// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.OnStartWorker
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Main
{
  public class OnStartWorker
  {
    public void DoWork()
    {
      this.CreateDataDirectories();
      try
      {
        this.ZipLogs();
        this.ClearOldData();
        BackupHelper.ClearOldBackUps();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
      this.ValidDataBaseConfig();
    }

    private void CreateDataDirectories()
    {
      ApplicationInfo.PathsInfo paths = ApplicationInfo.GetInstance().Paths;
      List<string> source = new List<string>()
      {
        paths.DataPath,
        paths.ConfigsPath,
        paths.GoodCatalogExcelTemplatesPath,
        paths.UpdatesPath,
        paths.AutoSavePath,
        paths.WaybillExcelTemplatesPath
      };
      try
      {
        new FastReportFacade().CreateTemplatesFolders();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        string шаблоновДокументов = Translate.OnStartWorker_Не_удалось_создать_папки_для_шаблонов_документов;
        LogHelper.ShowErrorMgs(ex, шаблоновДокументов, LogHelper.MsgTypes.Notification);
      }
      source.Where<string>((Func<string, bool>) (path => !Directory.Exists(path))).ToList<string>().ForEach((Action<string>) (x => Directory.CreateDirectory(x)));
    }

    private void ValidDataBaseConfig()
    {
      string path = Path.Combine(ApplicationInfo.GetInstance().Paths.ConfigsPath, "DataBase.json");
      try
      {
        if (!File.Exists(path))
          LogHelper.Debug("Файл " + path + " не сущестует");
        else
          JsonConvert.DeserializeObject<DataBase>(File.ReadAllText(path), new JsonSerializerSettings()
          {
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace
          });
      }
      catch (Exception ex)
      {
        string str = "";
        try
        {
          str = File.ReadAllText(path);
        }
        catch
        {
        }
        LogHelper.Error(ex, "Не удалось проверить файл " + path + ", возможно файл поврежден.\n\n" + str);
      }
    }

    private void ClearOldData()
    {
      this.DeleteOldFolder(ApplicationInfo.GetInstance().Paths.LogsPath, 30);
      this.DeleteOldFolder(ApplicationInfo.GetInstance().Paths.ArchivesFromHomePath, 0);
      this.DeleteOldFolder(ApplicationInfo.GetInstance().Paths.ArchivesFromPointsMovePath, 0);
      this.DeleteOldCrptLogs(ApplicationInfo.GetInstance().Paths.CrptLogsPath, 0);
      this.CompressOldCrptLogs(ApplicationInfo.GetInstance().Paths.CrptLogsPath);
    }

    private void ZipLogs()
    {
    }

    private void DeleteOldFolder(string path, int countSkipFile)
    {
      if (!Directory.Exists(path))
        return;
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<string>) Directory.GetDirectories(path)).Select<string, DirectoryInfo>((Func<string, DirectoryInfo>) (x => new DirectoryInfo(x))).OrderByDescending<DirectoryInfo, DateTime>((Func<DirectoryInfo, DateTime>) (x => x.CreationTime)).Skip<DirectoryInfo>(countSkipFile).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => x.CreationTime.AddDays(30.0) < DateTime.Now)).ToList<DirectoryInfo>())
        Directory.Delete(fileSystemInfo.FullName, true);
    }

    private void DeleteOldCrptLogs(string path, int countSkipFile)
    {
      if (!Directory.Exists(path))
        return;
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<string>) Directory.GetFiles(path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.CreationTime)).Skip<FileInfo>(countSkipFile).Where<FileInfo>((Func<FileInfo, bool>) (x => x.CreationTime.AddDays(90.0) < DateTime.Now)).ToList<FileInfo>())
        File.Delete(fileSystemInfo.FullName);
    }

    private void CompressOldCrptLogs(string path)
    {
      if (!Directory.Exists(path))
        return;
      foreach (FileInfo fileInfo in ((IEnumerable<string>) Directory.GetFiles(path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).Where<FileInfo>((Func<FileInfo, bool>) (x =>
      {
        DateTime dateTime = x.CreationTime;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.Now;
        DateTime date2 = dateTime.Date;
        return date1 < date2 && x.Extension.Contains("log");
      })).ToList<FileInfo>())
      {
        string str = FileSystemHelper.TempFolderPath();
        string destinationFile = Path.Combine(str, fileInfo.Name);
        FileSystemHelper.CopyFile(fileInfo.FullName, destinationFile);
        FileSystemHelper.CreateZip(fileInfo.FullName.Replace(fileInfo.Extension, "") + ".zip", str);
        File.Delete(fileInfo.FullName);
      }
    }
  }
}
