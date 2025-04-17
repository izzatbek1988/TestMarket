// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.NetworkHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

#nullable disable
namespace Gbs.Helpers
{
  public static class NetworkHelper
  {
    public static NetworkHelper.UrlPath GetPingedUrl(this IEnumerable<NetworkHelper.UrlPath> urls)
    {
      return urls.FirstOrDefault<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (url => url.Ping()));
    }

    [Localizable(false)]
    public static List<NetworkHelper.UrlPath> GetUrlPaths()
    {
      Settings settings = new ConfigsRepository<Settings>().Get();
      List<NetworkHelper.UrlPath> urlPaths = new List<NetworkHelper.UrlPath>();
      if (settings.Interface.Country != GlobalDictionaries.Countries.Ukraine)
        urlPaths.AddRange((IEnumerable<NetworkHelper.UrlPath>) new NetworkHelper.UrlPath[5]
        {
          new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Updates, "https://updates.app-pos.ru"),
          new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Keys, "https://keys.app-pos.ru"),
          new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Keys, "http://nossl-keys.app-pos.ru/"),
          new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.PingInternet, "https://yandex.ru"),
          new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.PingInternet, "https://mail.ru")
        });
      urlPaths.AddRange((IEnumerable<NetworkHelper.UrlPath>) new NetworkHelper.UrlPath[4]
      {
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Updates, "https://updates.app-pos.com"),
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Keys, "https://keys.app-pos.com"),
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Weboffice, "https://weboffice.gbsmarket.ru"),
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.PingInternet, "https://google.com")
      });
      List<NetworkHelper.UrlPath> collection = new List<NetworkHelper.UrlPath>()
      {
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Keys, "http://keys.test.app-pos.ru"),
        new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.Weboffice, "http://weboffice.test.app-pos.ru")
      };
      Testing.TestConfig config = Testing.GetConfig();
      if (config != null && config.UseTestHosts)
      {
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Используются тестовые хосты!"));
        foreach (NetworkHelper.UrlPath urlPath in collection)
        {
          NetworkHelper.UrlPath h = urlPath;
          urlPaths.RemoveAll((Predicate<NetworkHelper.UrlPath>) (x => x.UrlType == h.UrlType));
        }
        urlPaths.AddRange((IEnumerable<NetworkHelper.UrlPath>) collection);
      }
      if (settings.Interface.Country == GlobalDictionaries.Countries.Ukraine)
        urlPaths.Add(new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.FRTemplates, "https://updates.app-pos.com/8cb8d6dcbb390c302049ca46f13d31261c9d7e0e/templates/ua/"));
      else
        urlPaths.Add(new NetworkHelper.UrlPath(NetworkHelper.UrlTypes.FRTemplates, "https://v6.gbsmarket.ru/templates/ru/"));
      return urlPaths;
    }

    public static bool PingHost(string nameOrAddress, int timeOut = 10000)
    {
      bool flag = false;
      Ping ping = (Ping) null;
      try
      {
        ping = new Ping();
        PingReply pingReply = ping.Send(nameOrAddress, timeOut);
        flag = pingReply != null && pingReply.Status == IPStatus.Success;
      }
      catch (PingException ex)
      {
      }
      finally
      {
        ping?.Dispose();
      }
      return flag;
    }

    public static NetworkHelper.UrlInfo GetLinkInfo(string url)
    {
      SecurityProtocolType[] securityProtocolTypeArray = new SecurityProtocolType[4]
      {
        SecurityProtocolType.SystemDefault,
        SecurityProtocolType.Tls12,
        (SecurityProtocolType) 12288,
        SecurityProtocolType.Tls11
      };
      foreach (SecurityProtocolType securityProtocolType in securityProtocolTypeArray)
      {
        try
        {
          ServicePointManager.SecurityProtocol = securityProtocolType;
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
          httpWebRequest.Method = "HEAD";
          httpWebRequest.Timeout = 30000;
          httpWebRequest.UserAgent = "GBS.Market " + ApplicationInfo.GetInstance().GbsVersion.ToString(2);
          using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
          {
            if (response.StatusCode == HttpStatusCode.OK)
            {
              long contentLength = response.ContentLength;
              NetworkHelper.UrlInfo linkInfo = new NetworkHelper.UrlInfo()
              {
                Protocol = securityProtocolType,
                FileSize = contentLength
              };
              LogHelper.Debug("HEAD для " + url + "; Ответ: " + linkInfo.ToJsonString());
              return linkInfo;
            }
            LogHelper.Debug("Не удалось получить информацию о url: " + url + ". Статус: " + response.StatusCode.ToString());
          }
        }
        catch (WebException ex)
        {
        }
      }
      LogHelper.Trace("Информация о ссылке не была получена");
      return (NetworkHelper.UrlInfo) null;
    }

    public static bool DownloadFile(string sourceFile, string destinationFile, bool overWrite = true)
    {
      try
      {
        string str1 = FileSystemHelper.TempFolderPath();
        FileInfo fileInfo = new FileInfo(destinationFile);
        if (!overWrite && fileInfo.Exists)
        {
          LogHelper.Debug("Не удалось скачать файл: файл уже существует, а перезапись отключена");
          return false;
        }
        if (fileInfo.Directory == null)
        {
          LogHelper.Debug("Папка назначения == null");
          return false;
        }
        if (!FileSystemHelper.ExistsOrCreateFolder(fileInfo.Directory.FullName))
        {
          LogHelper.Debug("Не удалось проверить или создать папку назначения");
          return false;
        }
        if (!NetworkHelper.IsWorkInternet())
        {
          LogHelper.Debug("Нет подключения к интернету, ничего не скачиваем.");
          return false;
        }
        NetworkHelper.UrlInfo linkInfo = NetworkHelper.GetLinkInfo(sourceFile);
        if (linkInfo != null)
          ServicePointManager.SecurityProtocol = linkInfo.Protocol;
        using (WebClient webClient = new WebClient())
        {
          string str2 = "GBS.Market " + ApplicationInfo.GetInstance().GbsVersion.ToString(2);
          webClient.Headers.Add("User-Agent", str2);
          webClient.DownloadFile(sourceFile, str1 + "\\" + fileInfo.Name);
        }
        if (!System.IO.File.Exists(str1 + "\\" + fileInfo.Name))
        {
          LogHelper.Debug("Не удалось скачать файл: файл не был загружен во временную папку");
          return false;
        }
        FileSystemHelper.MoveFile(str1 + "\\" + fileInfo.Name, destinationFile);
        return true;
      }
      catch (Exception ex)
      {
        bool flag = !(ex is WebException);
        string message = "Ошибка загрузки файла из url:" + sourceFile;
        int num = flag ? 1 : 0;
        LogHelper.Error(ex, message, false, num != 0);
        return false;
      }
    }

    public static bool IsWorkInternet()
    {
      LogHelper.Debug("Проверяем наличие интернета");
      foreach (Uri uri in NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.PingInternet)).Select<NetworkHelper.UrlPath, Uri>((Func<NetworkHelper.UrlPath, Uri>) (x => x.Url)).ToList<Uri>())
      {
        if (NetworkHelper.PingHost(uri.Host))
        {
          LogHelper.Debug(string.Format("Есть пинг до {0}", (object) uri));
          return true;
        }
      }
      LogHelper.Debug("Пинга нет ни до одного из хостов");
      return false;
    }

    public static void OpenUrl(string url)
    {
      try
      {
        Process.Start(url);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка открытия ссылки", false);
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.LinkHelper_Не_удалось_открыть_ссылку__0_, (object) url));
      }
    }

    public enum UrlTypes
    {
      FRTemplates,
      Updates,
      Keys,
      Other,
      Weboffice,
      PingInternet,
    }

    public class UrlPath
    {
      public NetworkHelper.UrlTypes UrlType { get; }

      public Uri Url { get; }

      public UrlPath(NetworkHelper.UrlTypes urlType, string url)
      {
        this.UrlType = urlType;
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
          url = "https://" + url;
        this.Url = new Uri(url);
      }

      public bool Ping() => NetworkHelper.PingHost(this.Url.Host);
    }

    public class UrlInfo
    {
      public SecurityProtocolType Protocol { get; set; }

      public long FileSize { get; set; }
    }
  }
}
