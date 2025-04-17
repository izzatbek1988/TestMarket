// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BackupHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Db;
using Gbs.Helpers.DB;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class BackupHelper
  {
    private static string BackUpdFolderWithId
    {
      get
      {
        return new ConfigsRepository<DataBase>().Get().BackUp.Path + "\\" + GbsIdHelperMain.GetGbsId();
      }
    }

    public static bool CreateBackup(string pathForSave = "", string name = "")
    {
      try
      {
        string path = pathForSave.IsNullOrEmpty() ? BackupHelper.BackUpdFolderWithId : pathForSave;
        if (DevelopersHelper.IsUnitTest())
          return true;
        LogHelper.Debug("Начинаю создание резервной копии");
        if (!BackupHelper.CheckBackFolderExitsAndWritable(ref path))
          return false;
        string archiveFileName = path + "\\" + (name.IsNullOrEmpty() ? DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".zip" : name);
        LogHelper.Debug("Путь к архиву с резервной копией: " + archiveFileName);
        BackupHelper.PrepareBackupDataArchive(archiveFileName);
        LogHelper.Debug("Создание резервной копии завершено");
        TaskHelper.TaskRun((Action) (() =>
        {
          Telegram telegram = new ConfigsRepository<Settings>().Get().RemoteControl.Telegram;
          if (!telegram.IsSendBackUp || !(TelegramHelper.SendText(telegram.UsernameTo, string.Format(Translate.BackupHelper_CreateBackup_, UidDb.GetUid().Value, (object) GbsIdHelperMain.GetGbsId())) & TelegramHelper.SendFile(telegram.UsernameTo, archiveFileName)))
            return;
          LogHelper.Debug("Отправили РК в Телеграмм");
        }));
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка создания резервной копии.");
        return false;
      }
    }

    private static bool CheckBackFolderExitsAndWritable(ref string path)
    {
      if (!FileSystemHelper.ExistsOrCreateFolder(path, false))
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.BackupHelper_Не_удалось_найти_папку_для_сохранения_резервных_копий__проверьте_путь_до_нее_в_настройках_программы_);
        LogHelper.Debug("Не найдена папка с резервными копии и не удается ее создать, устанавливаю по-умолчанию");
        string path1 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\GBS.Market6_backups\\";
        if (!FileSystemHelper.ExistsOrCreateFolder(path1))
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.BackupHelper_Не_удалось_создать_папку_по_умолчанию_для_сохранения_резервных_копий_);
          return false;
        }
        ConfigsRepository<DataBase> configsRepository = new ConfigsRepository<DataBase>();
        DataBase config = configsRepository.Get();
        config.BackUp.Path = path1;
        configsRepository.Save(config);
        path = path1;
      }
      if (FileSystemHelper.IsDirectoryWritable(path))
        return true;
      MessageBoxHelper.Error(Translate.BackupHelper_CreateBackup_Папка_для_резервных_копий_недоступна_для_записи__Проверьте_права_доступа_к_папке_или_измените_папку_для_сохранения_резервных_копий_);
      LogHelper.Debug("Папка для резервной копии недоступна для записи");
      return false;
    }

    public static void ClearOldBackUps()
    {
      int backUpSavePeriod = new ConfigsRepository<DataBase>().Get().BackUp.StorageLifeDays;
      if (!Directory.Exists(BackupHelper.BackUpdFolderWithId))
        return;
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<FileInfo>) new DirectoryInfo(BackupHelper.BackUpdFolderWithId).GetFiles("*.zip")).Where<FileInfo>((Func<FileInfo, bool>) (x => x.CreationTime.AddDays((double) backUpSavePeriod) < DateTime.Now)))
        File.Delete(fileSystemInfo.FullName);
    }

    private static void PrepareBackupDataArchive(string archiveFilePath)
    {
      string str = FileSystemHelper.TempFolderPath() + "\\";
      Performancer performancer = new Performancer("Подготовка архива резервной копии");
      try
      {
        BackupHelper.CreateDataBaseCopy(str);
        performancer.AddPoint("Создание бэкапа БД");
      }
      catch (Exception ex)
      {
        Directory.Delete(str, true);
        int num = (int) MessageBoxHelper.Show(Translate.BackupHelper_Не_удалось_создать_копию_базы_данных_ + "\r\n\r\n" + Translate.BackupHelper_Рекомендуем_проверить_настройки_резервных_копий__При_отсутствии_копии_вы_можете_потерять_все_данные_программы_ + "\r\n\r\n" + ex.Message.ToString(), string.Empty, icon: MessageBoxImage.Hand);
        LogHelper.WriteError(ex, "Резервная копия БД не была создана");
      }
      ApplicationInfo instance = ApplicationInfo.GetInstance();
      FileSystemHelper.CopyFolder(instance.Paths.ConfigsPath, str + new DirectoryInfo(instance.Paths.ConfigsPath).Name + "\\");
      FileSystemHelper.CopyFolder(instance.Paths.TemplatesFrPath, str + "TemplatesFR\\");
      performancer.AddPoint("Копирование папок");
      IEnumerable<DirectoryInfo> directoryInfos = ((IEnumerable<string>) Directory.GetDirectories(instance.Paths.LogsPath)).Select<string, DirectoryInfo>((Func<string, DirectoryInfo>) (x => new DirectoryInfo(x)));
      Directory.CreateDirectory(str + "Logs\\");
      foreach (DirectoryInfo directoryInfo in directoryInfos)
      {
        DateTime result;
        if (DateTime.TryParse(directoryInfo.Name, out result) && (DateTime.Now - result).Days < 30)
          FileSystemHelper.CopyFolder(directoryInfo.FullName, str + "Logs\\" + directoryInfo.Name);
      }
      performancer.AddPoint("Копирование логов");
      FileSystemHelper.CreateZip(archiveFilePath, str);
      performancer.AddPoint("Создание архива");
      Directory.Delete(str, true);
      performancer.Stop();
    }

    private static string gbakUtilPath
    {
      get
      {
        ServiceController service = FirebirdHelper.GetCurrentFirebirdService();
        if (service == null)
          throw new ErrorHelper.GbsException(Translate.BackupHelper_gbakUtilPath_Расположение_исполняющих_файлов_службы_Firebird_не_определено);
        return FirebirdHelper.FirebirdSericesList.First<FirebirdHelper.FirebirdInfo>((Func<FirebirdHelper.FirebirdInfo, bool>) (x => x.Name == service.ServiceName)).Path + "gbak.exe";
      }
    }

    private static void CheckGbakFileExits()
    {
      if (!File.Exists(BackupHelper.gbakUtilPath))
        throw new FileNotFoundException(Translate.BackupHelper_Файл_gbak_exe_не_найден);
    }

    private static void CreateDataBaseCopy(string tempFolderPath)
    {
      DataBase dataBase = new ConfigsRepository<DataBase>().Get();
      if (dataBase.Connection.ConnectionType != DataBase.DbConnection.ConnectionTypes.Local)
        LogHelper.Debug("Резервная копия для БД не будет создана, т.к. подключение к БД не локальное");
      else if (!dataBase.BackUp.CreateDbInBackup)
      {
        LogHelper.Debug("Резервная копия для БД не будет создана, т.к. отключено в настройках");
      }
      else
      {
        BackupHelper.CheckGbakFileExits();
        FileInfo fileInfo = new FileInfo(dataBase.Connection.Path);
        if (!fileInfo.Exists)
          throw new FileNotFoundException(Translate.BackupHelper_Файл_базы_данных_не_найден);
        string path1 = tempFolderPath + fileInfo.Name.Replace(fileInfo.Extension, ".fbk");
        string str1 = dataBase.Connection.ServerUrl;
        if (str1.ToLower() == "localhost")
          str1 = "127.0.0.1";
        string path2 = FileSystemHelper.TempFolderPath() + "\\backup_" + DateTime.Now.ToString("HH_mm_ss") + ".log";
        DateTime now1 = DateTime.Now;
        string path3 = ApplicationInfo.GetInstance().Paths.LogsPath + DateTime.Now.ToString("yyyy-MM-dd") + "\\backup.log";
        string path4 = dataBase.Connection.Path;
        Process process = new Process()
        {
          StartInfo = {
            FileName = BackupHelper.gbakUtilPath,
            Arguments = " -user " + dataBase.Connection.DecryptedLogin + " -pass " + dataBase.Connection.DecryptedPassword + " -v -y \"" + path2 + "\" -b \"" + str1 + "/" + dataBase.Connection.ServerPort.ToString() + ":" + path4 + "\" \"" + path1 + "\"",
            WindowStyle = ProcessWindowStyle.Hidden
          }
        };
        Other.ConsoleWrite("backup arguments: " + process.StartInfo.Arguments);
        process.Start();
        process.WaitForExit();
        DateTime now2 = DateTime.Now;
        int exitCode = process.ExitCode;
        if (File.Exists(path2))
        {
          List<string> list = ((IEnumerable<string>) File.ReadAllLines(path2)).ToList<string>();
          list.Insert(0, "start " + now1.ToString("HH:mm:ss"));
          list.Insert(0, new string('-', 64));
          list.Add("finish " + now2.ToString("HH:mm:ss"));
          using (StreamWriter streamWriter = new StreamWriter(path3, true, Encoding.Unicode))
          {
            foreach (string str2 in list)
              streamWriter.WriteLine(str2);
          }
          File.Delete(path2);
        }
        if (exitCode != 0)
          LogHelper.Debug(string.Format("gbak exit code: {0}", (object) exitCode));
        if (!File.Exists(path1))
          throw new FileNotFoundException(string.Format(Translate.BackupHelper_Файл_резервной_копии_БД_не_был_создан__0_, (object) path1));
      }
    }

    public static void RestoreDataBase(string pathFbk, string pathDb)
    {
      DataBase dataBase = new ConfigsRepository<DataBase>().Get();
      BackupHelper.CheckGbakFileExits();
      if (!new FileInfo(pathFbk).Exists)
        throw new FileNotFoundException(Translate.BackupHelper_Файл_резервной_копии_не_найден);
      Process process = new Process()
      {
        StartInfo = {
          FileName = BackupHelper.gbakUtilPath,
          Arguments = "-r \"" + pathFbk + "\" \"" + pathDb + "\" -user " + dataBase.Connection.DecryptedLogin + " -pas " + dataBase.Connection.DecryptedPassword,
          WindowStyle = ProcessWindowStyle.Hidden
        }
      };
      process.Start();
      process.WaitForExit();
      int exitCode = process.ExitCode;
      if (exitCode != 0)
        LogHelper.Debug(string.Format("gbak exit code: {0}", (object) exitCode));
      if (!File.Exists(pathDb))
        throw new FileNotFoundException(Translate.BackupHelper_Файл_БД_не_был_создан);
      Other.SetCorrectExit();
    }
  }
}
