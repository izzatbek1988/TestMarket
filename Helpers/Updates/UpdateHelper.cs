// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UpdateHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Updates;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class UpdateHelper
  {
    private static int _attemptsCount;
    private static (bool isDownlandUpdate, string hash) _infoDownland = (false, string.Empty);

    private static string GetUpadteFolderUrl()
    {
      NetworkHelper.UrlPath pingedUrl = NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.Updates)).GetPingedUrl();
      if (pingedUrl == null)
        throw new Exception(Translate.UpdateHelper_Не_удалось_подключиться_к_серверу_обновлений);
      string md5Hash = CryptoHelper.GetMd5Hash((Vendor.GetConfig()?.CodeName ?? "gbsmarket") + "3364a3b8");
      string lower = new ConfigsRepository<Settings>().Get().Other.UpdateConfig.VersionUpdate.ToString().ToLower();
      return string.Format("{0}{1}/updates/{2}/", (object) pingedUrl.Url, (object) md5Hash, (object) lower);
    }

    public static void HandCheckUpdate()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ПроверкаОбновления);
      try
      {
        if (!NetworkHelper.IsWorkInternet())
          throw new IOException(Translate.UpdateHelper_HandCheckUpdate_Не_удалось_скачать_информацию_об_обновлении__Нет_подключения_к_интернету_);
        if (!UpdateHelper.DownloadMarketUpdateInfo())
          throw new IOException(Translate.UpdateHelper_Не_удалось_скачать_информацию_об_обновлении);
        TaskHelper.TaskRun(new Action(UpdateHelper_from65to66.DownLoadUpdate));
        Version newVersion;
        if (!UpdateHelper.CheckUpdateFromInfoFile(out newVersion))
        {
          progressBar.Close();
          int num = (int) MessageBoxHelper.Show(Translate.UpdateHelper_Обновление_не_найдено);
          LogHelper.Debug("Обновление не найдено");
        }
        else
        {
          progressBar.Close();
          if (MessageBoxHelper.Show(string.Format(Translate.UpdateHelper_Текущая_версия___0_, (object) ApplicationInfo.GetInstance().GbsVersion) + Other.NewLine() + string.Format(Translate.UpdateHelper_Найдена_новая_версия__0_, (object) newVersion) + Other.NewLine(2) + Translate.UpdateHelper_Скачать_сейчас_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
            UpdateHelper.DeleteUpdateFolder();
          else
            UpdateHelper.DownloadUpdateArchive();
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        int num = (int) MessageBoxHelper.Show(Translate.UpdateHelper_Текущая_версия__ + ApplicationInfo.GetInstance().GbsVersion?.ToString() + Other.NewLine(2) + ex.Message, icon: MessageBoxImage.Hand);
      }
      finally
      {
        progressBar.Close();
      }
    }

    private static bool DeleteUpdateFolder() => true;

    private static bool DownloadMarketUpdateInfo()
    {
      if (!NetworkHelper.IsWorkInternet())
        throw new IOException(Translate.UpdateHelper_HandCheckUpdate_Не_удалось_скачать_информацию_об_обновлении__Нет_подключения_к_интернету_);
      string str = "info.json";
      return NetworkHelper.DownloadFile(UpdateHelper.GetUpadteFolderUrl() + "/" + str, ApplicationInfo.GetInstance().Paths.UpdatesPath + str);
    }

    private static void DownloadUpdateArchive()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.UpdateHelper_Загрузка_обновления);
      try
      {
        if (!UpdateHelper.DownloadMarketUpdateInfo())
          return;
        string str = File.ReadAllText(ApplicationInfo.GetInstance().Paths.UpdatesPath + "info.json");
        UpdateHelper.UpdateInfo updateInfo;
        try
        {
          updateInfo = JsonConvert.DeserializeObject<UpdateHelper.UpdateInfo>(str);
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошиба фоновой проверки обновления", false);
          return;
        }
        bool flag = UpdateHelper.CheckUpdateForProduct(updateInfo);
        if (UpdateHelper._infoDownland.hash == updateInfo.Md5 && UpdateHelper._infoDownland.isDownlandUpdate || !flag)
          return;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.UpdateHelper_Доступно_обновление,
          Text = string.Format(Translate.UpdateHelper_DownloadUpdateArchive_Новая_версия_программы__0__доступна_и_готова_к_установке__Перезапустите_программу_для_установки_обновления_, (object) PartnersHelper.ProgramName())
        });
        UpdateHelper._infoDownland = (true, updateInfo.Md5);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошиба фоновой проверки обновления", false);
      }
      finally
      {
        progressBar.Close();
      }
    }

    public static bool CheckUpdateFromInfoFile(out Version newVersion)
    {
      newVersion = new Version(1, 0);
      try
      {
        string str = "info.json";
        string path = ApplicationInfo.GetInstance().Paths.DataPath + "\\Updates\\" + str;
        if (!File.Exists(path))
          return false;
        UpdateHelper.UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateHelper.UpdateInfo>(File.ReadAllText(path));
        newVersion = updateInfo.Version;
        return updateInfo.Version > ApplicationInfo.GetInstance().GbsVersion;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка проверки обновления", false);
        throw new Exception(Translate.UpdateHelper_CheckUpdateFromInfoFile_Ошибка_при_поиске_обновления__обратитесь_в_техническую_поддержку_или_обновите_программу_вручную_);
      }
    }

    public static bool CheckReadyToUpdate()
    {
      string str1 = "info.json";
      string path = ApplicationInfo.GetInstance().Paths.UpdatesPath + str1;
      if (!File.Exists(path))
        return false;
      UpdateHelper.UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateHelper.UpdateInfo>(File.ReadAllText(path));
      if (updateInfo.Version <= ApplicationInfo.GetInstance().GbsVersion)
        return false;
      string str2 = "update.zip";
      string str3 = ApplicationInfo.GetInstance().Paths.UpdatesPath + str2;
      return File.Exists(str3) && !(CryptoHelper.GetMd5HashForFile(str3) != updateInfo.Md5);
    }

    private static bool CheckUpdateForProduct(UpdateHelper.UpdateInfo updateInfo)
    {
      Version gbsVersion = ApplicationInfo.GetInstance().GbsVersion;
      if (updateInfo.Version <= gbsVersion)
        return false;
      string str1 = "update.zip";
      string str2 = ApplicationInfo.GetInstance().Paths.UpdatesPath + str1;
      if (NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.Updates)).GetPingedUrl() == null)
        throw new Exception(Translate.UpdateHelper_Не_удалось_подключиться_к_серверу_обновлений);
      string empty = string.Empty;
      if (File.Exists(str2) && CryptoHelper.GetMd5HashForFile(str2) != updateInfo.Md5)
        File.Delete(str2);
      if (!File.Exists(str2) && !NetworkHelper.DownloadFile(UpdateHelper.GetUpadteFolderUrl() + "/" + str1, str2))
        throw new Exception(Translate.UpdateHelper_CheckUpdateForProduct_Не_удалось_скачать_файл_архива_с_обновлением);
      if (CryptoHelper.GetMd5HashForFile(str2) == updateInfo.Md5)
      {
        LogHelper.Debug(string.Format("Найдено обновление, версия: {0}. Обновление загружено и готово к установке", (object) updateInfo.Version));
        return true;
      }
      File.Delete(str2);
      return false;
    }

    public static void CheckUpdate()
    {
      try
      {
        if (UpdateHelper._attemptsCount > 3)
        {
          LogHelper.Debug("Превышено количество попыток загрузки обновления");
        }
        else
        {
          ++UpdateHelper._attemptsCount;
          UpdateHelper.DownloadUpdateArchive();
          UpdateHelper_from65to66.DownLoadUpdate();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка проверки или загрузки обновления", false);
      }
    }

    private class UpdateInfo
    {
      public Version Version { get; set; } = new Version(1, 0);

      public string Md5 { get; set; } = string.Empty;
    }
  }
}
