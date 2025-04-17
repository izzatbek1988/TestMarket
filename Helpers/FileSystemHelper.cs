// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FileSystemHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Gbs.Helpers
{
  public static class FileSystemHelper
  {
    public const string TempFolderPreffix = "b16d85d1-a305-4724-a3cc-ccb7ca3a796a";

    public static ServiceController GetService(string serviceName, bool showError = true)
    {
      ServiceController serviceController = ((IEnumerable<ServiceController>) ServiceController.GetServices()).FirstOrDefault<ServiceController>((Func<ServiceController, bool>) (s => s.ServiceName == serviceName));
      return !(serviceController == null & showError) ? serviceController : throw new InvalidOperationException(string.Format(Translate.DataBaseHelper_Служба__0__не_найдена_на_данном_компьютере, (object) serviceName));
    }

    [Localizable(false)]
    public static void RunBat(string scriptText, bool byAdmin = false)
    {
      try
      {
        LogHelper.OnBegin();
        string str = FileSystemHelper.TempFolderPath();
        string path = Path.Combine(str, "script.cmd");
        File.WriteAllText(path, scriptText);
        Process process = new Process()
        {
          StartInfo = {
            FileName = path,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
          }
        };
        if (byAdmin)
          process.StartInfo.Verb = "runas";
        process.Start();
        process.WaitForExit();
        LogHelper.OnEnd();
        Directory.Delete(str, true);
      }
      catch (Exception ex)
      {
        throw new ErrorHelper.GbsException(Translate.FileSystemHelper_RunBat_Не_удалось_выполнить_скрипт_из_bat_файла, ex);
      }
    }

    public static void StartService(string serviceName)
    {
      FileSystemHelper.RunBat("net start " + serviceName, true);
    }

    public static void StopService(string serviceName)
    {
      FileSystemHelper.RunBat("net stop " + serviceName, true);
    }

    public static void DeleteService(string serviceName)
    {
      FileSystemHelper.RunBat("sc stop \"" + serviceName + "\"\r\nsc delete \"" + serviceName + "\"");
    }

    public static string GetSizeFile(string filePath)
    {
      try
      {
        if (File.Exists(filePath))
        {
          long length = new FileInfo(filePath).Length;
          string[] strArray = new string[5]
          {
            Translate.FileSystemHelper_GetSizeFile_байт,
            Translate.FileSystemHelper_GetSizeFile_кБайт,
            Translate.FileSystemHelper_GetSizeFile_мБайт,
            Translate.FileSystemHelper_GetSizeFile_гБайт,
            Translate.FileSystemHelper_GetSizeFile_тБайт
          };
          for (int index = 0; index < 5; ++index)
          {
            if (length < 1024L)
              return length.ToString() + " " + strArray[index];
            length /= 1024L;
          }
        }
        return "file not found";
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось получить размер файла", false);
        return "-error-";
      }
    }

    public static void GrantAccess(string folder)
    {
      if (!Directory.Exists(folder))
        return;
      DirectoryInfo directoryInfo = new DirectoryInfo(folder);
      DirectorySecurity accessControl = directoryInfo.GetAccessControl();
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier) null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
      directoryInfo.SetAccessControl(accessControl);
    }

    public static void ShowFolderDriver(string path)
    {
      if (!FileSystemHelper.ExistsOrCreateFolderBat(path))
        MessageBoxHelper.Error(string.Format(Translate.FileSystemHelper_ShowFolderDriver_, (object) path));
      else
        FileSystemHelper.OpenFolder(path);
    }

    public static bool SetAutoRunValue(bool autoRun)
    {
      string executablePath = Application.ExecutablePath;
      RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
      if (subKey == null)
        return false;
      try
      {
        if (autoRun)
          subKey.SetValue("AutoRunMarket", (object) executablePath);
        else if (((IEnumerable<string>) subKey.GetValueNames()).Contains<string>("AutoRunMarket"))
          subKey.DeleteValue("AutoRunMarket");
        subKey.Close();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static string IsSoftwareInstalled(string name)
    {
      try
      {
        string empty = string.Empty;
        RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
        if (registryKey1 == null)
          return (string) null;
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
          if (registryKey2 != null && registryKey2.GetValue("DisplayName")?.ToString() == name)
            empty = registryKey2.GetValue("InstallLocation") as string;
        }
        if (string.IsNullOrEmpty(empty))
        {
          RegistryKey registryKey3 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
          if (registryKey3 == null)
            return (string) null;
          foreach (string subKeyName in registryKey3.GetSubKeyNames())
          {
            RegistryKey registryKey4 = registryKey3.OpenSubKey(subKeyName);
            if (registryKey4 != null && registryKey4.GetValue("DisplayName")?.ToString() == name)
              empty = registryKey4.GetValue("InstallLocation") as string;
          }
        }
        return empty;
      }
      catch (Exception ex)
      {
        string message = "Не удалось проерить наличие программы " + name + " на ПК";
        LogHelper.Error(ex, message, false);
        return (string) null;
      }
    }

    public static void OpenSite(string path)
    {
      try
      {
        new Process()
        {
          StartInfo = new ProcessStartInfo()
          {
            FileName = path,
            UseShellExecute = true
          }
        }.Start();
      }
      catch (Win32Exception ex)
      {
        throw new InvalidOperationException(Translate.FileSystemHelper_Не_удалось_запустить_браузер_по_умолчанию__проверьте_настройки_системы_, (Exception) ex);
      }
    }

    public static string TempFolderPath()
    {
      string path = Path.Combine(Path.GetTempPath(), "b16d85d1-a305-4724-a3cc-ccb7ca3a796a_" + Guid.NewGuid().ToString());
      FileSystemHelper.ExistsOrCreateFolder(path);
      return path;
    }

    public static bool ExistsOrCreateFolder(string path, bool isShowError = true)
    {
      try
      {
        if (Directory.Exists(path))
          return true;
        Directory.CreateDirectory(path);
        return true;
      }
      catch (UnauthorizedAccessException ex)
      {
        int num = isShowError ? 1 : 0;
        LogHelper.Error((Exception) ex, "Не удалось проверить существование папки или создать новую. Недостаточно прав доступа.", num != 0);
        return false;
      }
      catch (Exception ex)
      {
        int num = isShowError ? 1 : 0;
        LogHelper.Error(ex, "Ошибка проверки существования папки или создания новой", num != 0);
        return false;
      }
    }

    public static bool ExistsOrCreateFolderBat(string path, bool isShowError = true)
    {
      try
      {
        if (Directory.Exists(path))
          return true;
        List<string> contents = new List<string>()
        {
          "@echo off",
          "ver |>NUL find /v \"5.\" && if \"%~1\"==\"\" (",
          "Echo CreateObject^(\"Shell.Application\"^).ShellExecute WScript.Arguments^(0^),\"1\",\"\",\"runas\",1 >\"%~dp0Elevating.vbs\"",
          "cscript.exe //nologo \"%~dp0Elevating.vbs\" \"%~f0\"& goto :eof",
          ")",
          "MD \"" + path + "\""
        };
        string path1 = FileSystemHelper.TempFolderPath();
        string str = path1 + "\\\\create.bat";
        LogHelper.Debug("Crate folder bat. Text: " + contents.ToJsonString(true) + "; path: " + str);
        File.WriteAllLines(str, (IEnumerable<string>) contents);
        Process process = Process.Start(str);
        Thread.Sleep(3000);
        process?.WaitForExit();
        Directory.Delete(path1, true);
        return Directory.Exists(path);
      }
      catch (UnauthorizedAccessException ex)
      {
        int num = isShowError ? 1 : 0;
        LogHelper.Error((Exception) ex, "Не удалось проверить существование папки или создать новую. Недостаточно прав доступа.", num != 0);
        return false;
      }
      catch (Exception ex)
      {
        int num = isShowError ? 1 : 0;
        LogHelper.Error(ex, "Ошибка проверки существования папки или создания новой", num != 0);
        return false;
      }
    }

    public static bool OpenFolder(string path)
    {
      Process.Start("explorer", path);
      return true;
    }

    public static bool MoveFile(string sourceFile, string destinationFile, bool overWrite = true)
    {
      return FileSystemHelper.MoveOrCopyFile(sourceFile, destinationFile, overWrite, true);
    }

    public static bool CopyFile(string sourceFile, string destinationFile, bool overWrite = true)
    {
      return FileSystemHelper.MoveOrCopyFile(sourceFile, destinationFile, overWrite, false);
    }

    public static bool IsFileLocked(string filePath)
    {
      try
      {
        using (FileStream fileStream = new FileInfo(filePath).Open(FileMode.Open, FileAccess.Read, FileShare.None))
          fileStream.Close();
      }
      catch (IOException ex)
      {
        return true;
      }
      return false;
    }

    public static bool CopyFolder(string sourceFolder, string destinationFolder, bool overWrite = true)
    {
      try
      {
        if (!sourceFolder.EndsWith("\\"))
          sourceFolder += "\\";
        if (!destinationFolder.EndsWith("\\"))
          destinationFolder += "\\";
        return ((IEnumerable<string>) Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories)).All<string>((Func<string, bool>) (dirPath => FileSystemHelper.ExistsOrCreateFolder(dirPath.Replace(sourceFolder, destinationFolder)))) & ((IEnumerable<string>) Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories)).All<string>((Func<string, bool>) (newPath => FileSystemHelper.CopyFile(newPath, newPath.Replace(sourceFolder, destinationFolder), overWrite)));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при копировании папки");
        return false;
      }
    }

    public static bool IsDirectoryWritable(string dirPath)
    {
      try
      {
        using (File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
          ;
        return true;
      }
      catch (Exception ex)
      {
        string logMessage = "Проверка папки " + dirPath + " на возможность записи";
        LogHelper.WriteError(ex, logMessage, false);
        return false;
      }
    }

    private static bool MoveOrCopyFile(
      string sourceFile,
      string destinationFile,
      bool overWrite,
      bool move)
    {
      try
      {
        if (!File.Exists(sourceFile))
        {
          LogHelper.Debug("Не удалось скопировать файл: файл-источник не существует");
          return false;
        }
        FileInfo fileInfo = new FileInfo(destinationFile);
        if (fileInfo.Directory == null)
        {
          LogHelper.Debug("Не удалось скопировать файл: нет данных о папке назначения");
          return false;
        }
        if (!FileSystemHelper.ExistsOrCreateFolder(fileInfo.Directory.FullName))
        {
          LogHelper.Debug("Не удалось скопировать файл: папка назначения не существует или ее не удалось создать");
          return false;
        }
        if (fileInfo.Exists && !overWrite)
        {
          LogHelper.Debug("Не удалось скопировать файл: файл уже существует, а перезапись отключена");
          return false;
        }
        File.Copy(sourceFile, destinationFile, overWrite);
        if (move)
          File.Delete(sourceFile);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка копирования файла");
        return false;
      }
    }

    public static void CreateZip(string zipFileName, string sourceFolderPath, string password = "")
    {
      FileInfo fileInfo = new FileInfo(zipFileName);
      if (fileInfo.Directory != null && !Directory.Exists(fileInfo.Directory.FullName))
        Directory.CreateDirectory(fileInfo.Directory.FullName);
      if (File.Exists(zipFileName))
        File.Delete(zipFileName);
      using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
      {
        if (!password.IsNullOrEmpty())
          zipFile.Password = password;
        if (!sourceFolderPath.IsNullOrEmpty())
          zipFile.AddDirectory(sourceFolderPath);
        zipFile.Save(zipFileName);
      }
    }

    public static void CreateZip(
      string zipFileName,
      IEnumerable<FileInfo> sourceFilePaths,
      string password = "")
    {
      FileInfo fileInfo = new FileInfo(zipFileName);
      if (fileInfo.Directory != null && !Directory.Exists(fileInfo.Directory.FullName))
        Directory.CreateDirectory(fileInfo.Directory.FullName);
      string str = FileSystemHelper.TempFolderPath();
      foreach (FileInfo sourceFilePath in sourceFilePaths)
        FileSystemHelper.MoveOrCopyFile(sourceFilePath.FullName, Path.Combine(str, sourceFilePath.Name), true, false);
      using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
      {
        if (!password.IsNullOrEmpty())
          zipFile.Password = password;
        zipFile.AddDirectory(str);
        zipFile.Save(zipFileName);
      }
      Directory.Delete(str, true);
    }

    public static ZipFile OpenZip(string zipFileName)
    {
      try
      {
        if (!File.Exists(zipFileName))
        {
          LogHelper.Debug("Архив не существует, распаковка невозможна");
          return (ZipFile) null;
        }
        string fileName = zipFileName;
        using (ZipFile zipFile = ZipFile.Read(fileName, new ReadOptions()
        {
          Encoding = Encoding.UTF8
        }))
          return zipFile;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка извлечения архива", false);
        return (ZipFile) null;
      }
    }

    public static void ExtractToFile(ZipFile zip, string fileName, string path, string pass = "")
    {
      zip[fileName]?.ExtractWithPassword(path, pass);
      zip.Dispose();
    }

    public static void ExtractAllFile(string pathZip, string folderPath, string password = "")
    {
      string empty = string.Empty;
      string fileName1 = pathZip;
      using (ZipFile zipFile = ZipFile.Read(fileName1, new ReadOptions()
      {
        Encoding = Encoding.UTF8
      }))
      {
        foreach (ZipEntry zipEntry in zipFile)
        {
          string fileName2 = zipEntry.FileName;
          if (password.IsNullOrEmpty())
            zipEntry.Extract(folderPath, ExtractExistingFileAction.DoNotOverwrite);
          else
            zipEntry.ExtractWithPassword(folderPath, password);
        }
      }
    }

    public static bool CheckFileExistWithMsg(string pathToFile, bool showMsg = true)
    {
      bool flag = File.Exists(pathToFile);
      if (!flag & showMsg)
        MessageBoxHelper.Error(string.Format(Translate.FileSystemHelperФайлНеНайденПроверитеНаличиеНеобходимогоФайлаИПопробуйтеСнова, (object) pathToFile));
      LogHelper.Debug(string.Format("Проверка налаичия файла по пути '{0}'; Результат: {1}", (object) pathToFile, (object) flag));
      return flag;
    }

    public static void CheckActiveThisPath()
    {
      try
      {
        string sa = ((IEnumerable<string>) System.Environment.GetCommandLineArgs()).FirstOrDefault<string>() ?? string.Empty;
        Process pr = Process.GetCurrentProcess();
        foreach (Process process in ((IEnumerable<Process>) Process.GetProcesses()).Where<Process>((Func<Process, bool>) (n => n.ProcessName == pr.ProcessName)).Where<Process>((Func<Process, bool>) (item => FileSystemHelper.GetProcessPath(item.Id) == sa && item.Id != pr.Id)))
        {
          LogHelper.Debug("Приложение из данного местоположения уже запущено перевожу фокус и завершаю работу");
          FileSystemHelper.SetForegroundWindow(process.MainWindowHandle.ToInt32());
          Other.SetCorrectExit();
          System.Environment.Exit(0);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при проверке активных процессов");
      }
    }

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(int windowHandle);

    private static string GetProcessPath(int processId)
    {
      try
      {
        using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId.ToString()))
        {
          using (ManagementObjectCollection source = managementObjectSearcher.Get())
            return source.Cast<ManagementObject>().Select<ManagementObject, object>((Func<ManagementObject, object>) (mo => mo["ExecutablePath"])).First<object>()?.ToString() ?? "";
        }
      }
      catch
      {
        return string.Empty;
      }
    }

    public static bool CheckIfFileIsBeingUsed(string fileName)
    {
      try
      {
        if (!File.Exists(fileName))
          return false;
        using (File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
          ;
      }
      catch
      {
        return true;
      }
      return false;
    }

    public static bool IsFramework47()
    {
      using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
      {
        if (registryKey == null)
        {
          LogHelper.Debug("Не удалось получить ключ из реестра для проверки версии .net Framework");
          return false;
        }
        (string version, bool is47) tuple = FileSystemHelper.CheckFor45DotVersion(Convert.ToInt32(registryKey.GetValue("Release")));
        LogHelper.Debug("Version .net Framework: " + tuple.version);
        return tuple.is47;
      }
    }

    private static (string version, bool is47) CheckFor45DotVersion(int releaseKey)
    {
      if (releaseKey >= 461808)
        return ("4.7.2 or later", true);
      if (releaseKey >= 461308)
        return ("4.7.1 or later", true);
      if (releaseKey >= 460798)
        return ("4.7 or later", true);
      if (releaseKey >= 394802)
        return ("4.6.2 or later", false);
      if (releaseKey >= 394254)
        return ("4.6.1 or later", false);
      if (releaseKey >= 393295)
        return ("4.6 or later", false);
      if (releaseKey >= 393273)
        return ("4.6 RC or later", false);
      if (releaseKey >= 379893)
        return ("4.5.2 or later", false);
      if (releaseKey >= 378675)
        return ("4.5.1 or later", false);
      return releaseKey >= 378389 ? ("4.5 or later", false) : ("No 4.5 or later version detected", false);
    }

    public static class ShortLongPathTool
    {
      private const int MAX_PATH = 255;

      [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
      private static extern int GetShortPathName(
        [MarshalAs(UnmanagedType.LPTStr)] string path,
        [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
        int shortPathLength);

      public static string GetShortPath(string path)
      {
        StringBuilder shortPath = new StringBuilder((int) byte.MaxValue);
        FileSystemHelper.ShortLongPathTool.GetShortPathName(path, shortPath, (int) byte.MaxValue);
        return shortPath.ToString();
      }
    }
  }
}
