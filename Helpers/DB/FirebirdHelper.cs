// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.FirebirdHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Helpers.DB
{
  public static class FirebirdHelper
  {
    public static List<FirebirdHelper.FirebirdInfo> FirebirdSericesList = new List<FirebirdHelper.FirebirdInfo>()
    {
      new FirebirdHelper.FirebirdInfo()
      {
        Version = 1,
        Name = "FirebirdServerFirebirdForGBS",
        Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) + "\\F-Lab\\Firebird 3\\"
      },
      new FirebirdHelper.FirebirdInfo()
      {
        Version = 2,
        Name = "FirebirdServer_9b6e7ae5",
        Path = FirebirdHelper.GetProgramFilesFolder() + "\\FirebirdServer_9b6e7ae5\\"
      }
    };
    private const string instsvcexe = "instsvc.exe";

    private static string GetProgramFilesFolder()
    {
      string environmentVariable = System.Environment.GetEnvironmentVariable("ProgramW6432");
      return string.IsNullOrEmpty(environmentVariable) ? System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) : environmentVariable;
    }

    public static ServiceController GetCurrentFirebirdService()
    {
      ServiceController[] services = ServiceController.GetServices();
      foreach (FirebirdHelper.FirebirdInfo firebirdInfo in (IEnumerable<FirebirdHelper.FirebirdInfo>) FirebirdHelper.FirebirdSericesList.OrderByDescending<FirebirdHelper.FirebirdInfo, int>((Func<FirebirdHelper.FirebirdInfo, int>) (x => x.Version)))
      {
        FirebirdHelper.FirebirdInfo service = firebirdInfo;
        ServiceController currentFirebirdService = ((IEnumerable<ServiceController>) services).FirstOrDefault<ServiceController>((Func<ServiceController, bool>) (x => x.ServiceName == service.Name));
        if (currentFirebirdService != null)
        {
          LogHelper.Trace("Текущая служба Firebird: " + currentFirebirdService.ServiceName);
          return currentFirebirdService;
        }
      }
      LogHelper.Debug("Ни одна из служб Firebird не была найдена");
      return (ServiceController) null;
    }

    public static void CheckLocalServiceRunning()
    {
      string serverUrl = new ConfigsRepository<DataBase>().Get().Connection.ServerUrl;
      if (!serverUrl.ToLower().Contains("localhost") && !serverUrl.Contains("127.0.0.1"))
        return;
      LogHelper.Debug("Проверка статуса слжубы Firebird");
      ServiceController currentFirebirdService = FirebirdHelper.GetCurrentFirebirdService();
      if (currentFirebirdService == null)
      {
        LogHelper.Trace("Службы Firebird не найдены");
        FirebirdHelper.InstallFirebirdService();
      }
      else
        LogHelper.Trace("Найдена служба: " + currentFirebirdService.ServiceName);
      FirebirdHelper.RunFirebirdService();
    }

    private static void RunFirebirdService()
    {
      ServiceController currentFirebirdService1 = FirebirdHelper.GetCurrentFirebirdService();
      if (currentFirebirdService1 == null)
        throw new ErrorHelper.GbsException(Translate.FirebirdHelper_RunFirebirdService_Служба_Firebird_не_найдена_на_этом_компьютере);
      if (currentFirebirdService1.Status != ServiceControllerStatus.Running)
      {
        LogHelper.Debug("Запускаем службу " + currentFirebirdService1.ServiceName);
        FileSystemHelper.StartService(currentFirebirdService1.ServiceName);
        Thread.Sleep(5000);
        ServiceController currentFirebirdService2 = FirebirdHelper.GetCurrentFirebirdService();
        if (currentFirebirdService2.Status != ServiceControllerStatus.Running)
          throw new ErrorHelper.GbsException(string.Format(Translate.FileSystemHelper_StartService_Не_удалось_запустить_службу__0_, (object) currentFirebirdService2.ServiceName));
      }
      else
        LogHelper.Debug("Служба " + currentFirebirdService1.ServiceName + " уже запущена");
    }

    private static void InstallFirebirdService()
    {
      LogHelper.Debug("Устанавливаем службу Firebird");
      FirebirdHelper.FirebirdInfo firebirdCurrentInfo = FirebirdHelper.GetFirebirdCurrentInfo();
      if (firebirdCurrentInfo == null)
      {
        FirebirdHelper.DownloadFirebird();
        if (FirebirdHelper.GetCurrentFirebirdService().Status == ServiceControllerStatus.Running)
          return;
        firebirdCurrentInfo = FirebirdHelper.GetFirebirdCurrentInfo();
      }
      string str = firebirdCurrentInfo != null ? firebirdCurrentInfo.Name.Replace("FirebirdServer", "") : throw new ErrorHelper.GbsException(Translate.FirebirdHelper_InstallFirebirdService_Не_удалось_установить_службу_Firebird__т_к_не_найдены_исполняющие_файлы);
      FileSystemHelper.RunBat("\"" + firebirdCurrentInfo.Path + "instsvc.exe\" install -name \"" + str + "\"", true);
      Thread.Sleep(5000);
    }

    private static FirebirdHelper.FirebirdInfo GetFirebirdCurrentInfo()
    {
      foreach (FirebirdHelper.FirebirdInfo firebirdCurrentInfo in (IEnumerable<FirebirdHelper.FirebirdInfo>) FirebirdHelper.FirebirdSericesList.OrderByDescending<FirebirdHelper.FirebirdInfo, int>((Func<FirebirdHelper.FirebirdInfo, int>) (x => x.Version)))
      {
        if (File.Exists(firebirdCurrentInfo.Path + "instsvc.exe"))
        {
          LogHelper.Debug("Найдены исполняющие файлы в папке " + firebirdCurrentInfo.Path + " для службы " + firebirdCurrentInfo.Name);
          return firebirdCurrentInfo;
        }
      }
      return (FirebirdHelper.FirebirdInfo) null;
    }

    public static void RestartFirebird()
    {
      ServiceController currentFirebirdService = FirebirdHelper.GetCurrentFirebirdService();
      if (currentFirebirdService == null)
        throw new ErrorHelper.GbsException(Translate.FirebirdHelper_RestartFirebird_Служба_Firebird_не_может_быть_перезапущена__т_к__не_установлена_на_этом_устройстве_);
      try
      {
        FileSystemHelper.StopService(currentFirebirdService.ServiceName);
        Thread.Sleep(500);
        FileSystemHelper.StartService(currentFirebirdService.ServiceName);
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка перезапуска службы");
      }
    }

    private static void DownloadFirebird()
    {
      try
      {
        if (MessageBoxHelper.Show(Translate.DataBaseHelper_DownloadFirebird_СлужбаFirebirdForGbsнеНайденаНаУстройстве, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) == MessageBoxResult.No)
          System.Environment.Exit(0);
        ProgressBarHelper.ProgressBar progressBar1 = new ProgressBarHelper.ProgressBar(Translate.DataBaseHelper_DownloadFirebird_Скачивание_СУБД_Firebird_3_0);
        string path1 = FileSystemHelper.TempFolderPath();
        string str = Path.Combine(path1, "firebird.exe");
        List<string> stringList = new List<string>();
        stringList.Add("dl.gbsmarket.ru");
        stringList.Add("files.pos-app.ru");
        stringList.Add("files.pos-app.com");
        string sourceFile = string.Empty;
        foreach (string nameOrAddress in stringList)
        {
          if (NetworkHelper.PingHost(nameOrAddress))
          {
            sourceFile = "https://" + nameOrAddress + "/Downloads/Components/Firebird/FirebirdServer_9b6e7ae5-Setup.exe";
            break;
          }
        }
        if (!NetworkHelper.DownloadFile(sourceFile, str))
        {
          progressBar1.Close();
          MessageBoxHelper.Error(Translate.DataBaseHelper_DownloadFirebird_Не_удалось_выполнить_скачивание_файла_для_установки_Firebird_3_0__обратитесь_в_службу_технической_поддержки_для_решения_вопроса_);
          System.Environment.Exit(0);
        }
        ProgressBarHelper.ProgressBar progressBar2 = new ProgressBarHelper.ProgressBar(Translate.DataBaseHelper_DownloadFirebird_Установка_СУБД_Firebird_3_0);
        Process.Start(new ProcessStartInfo(str)
        {
          Arguments = " /S",
          WorkingDirectory = path1,
          UseShellExecute = true
        })?.WaitForExit();
        ProgressBarHelper.Close();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка скачивания Firebird");
        System.Environment.Exit(0);
      }
    }

    public class FirebirdInfo
    {
      public int Version { get; set; }

      public string Name { get; set; }

      public string Path { get; set; }
    }
  }
}
