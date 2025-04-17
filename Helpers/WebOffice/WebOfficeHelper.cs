// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.WebOfficeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Clients;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings;
using Gbs.Forms._shared;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class WebOfficeHelper
  {
    private static Gbs.Core.Config.WebOffice _webOfficeSetting;
    private static Setting _infoDb;

    private static bool CreateCatalogFile(string tempFolder, out string pathFile)
    {
      string path = Path.Combine(tempFolder, "catalog.json");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        string contents = JsonConvert.SerializeObject((object) new GoodRepository(dataBase).GetActiveItems().Select<Gbs.Core.Entities.Goods.Good, Good>((Func<Gbs.Core.Entities.Goods.Good, Good>) (x => new Good(x))), Formatting.Indented);
        System.IO.File.WriteAllText(path, contents);
        pathFile = path;
        return true;
      }
    }

    private static bool CreateGroupsGoodFile(string tempFolder, out string pathFile)
    {
      string path = Path.Combine(tempFolder, "groupsGood.json");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        string contents = JsonConvert.SerializeObject((object) new GoodGroupsRepository(dataBase).GetAllItems().Select<GoodGroups.Group, GoodGroup>((Func<GoodGroups.Group, GoodGroup>) (x => new GoodGroup(x))), Formatting.Indented);
        System.IO.File.WriteAllText(path, contents);
        pathFile = path;
        return true;
      }
    }

    private static bool CreateClientsFile(string tempFolder, out string pathFile)
    {
      string path = Path.Combine(tempFolder, "clients.json");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<ClientAdnSum> list1 = new ClientsRepository(dataBase).GetListActiveItemAndSum().ToList<ClientAdnSum>();
        List<ClientAdnSum> list2 = new ClientsRepository(dataBase).GetByQuery(dataBase.GetTable<CLIENTS>().Where<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.IS_DELETED == true))).Select<Gbs.Core.Entities.Clients.Client, ClientAdnSum>((Func<Gbs.Core.Entities.Clients.Client, ClientAdnSum>) (x => new ClientAdnSum()
        {
          Client = x
        })).ToList<ClientAdnSum>();
        list1.AddRange((IEnumerable<ClientAdnSum>) list2);
        string contents = JsonConvert.SerializeObject((object) list1.Select<ClientAdnSum, Client>((Func<ClientAdnSum, Client>) (x => new Client(x))), Formatting.Indented);
        System.IO.File.WriteAllText(path, contents);
        pathFile = path;
        return true;
      }
    }

    private static bool CreateUsersFile(string tempFolder, out string pathFile)
    {
      string path = Path.Combine(tempFolder, "users.json");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        string contents = JsonConvert.SerializeObject((object) new UsersRepository(dataBase).GetAllItems().ToList<Gbs.Core.Entities.Users.User>().Select<Gbs.Core.Entities.Users.User, User>((Func<Gbs.Core.Entities.Users.User, User>) (x => new User(x))), Formatting.Indented);
        System.IO.File.WriteAllText(path, contents);
        pathFile = path;
        return true;
      }
    }

    private static bool CreateInfoFile(string tempFolder, out string pathFile)
    {
      string path = Path.Combine(tempFolder, "info.json");
      string contents = JsonConvert.SerializeObject((object) new HomeOfficeHelper.InfoArchive()
      {
        UidDevice = GbsIdHelperMain.GetGbsId(),
        NameDataBase = WebOfficeHelper._infoDb.Value.ToString()
      }, Formatting.Indented);
      System.IO.File.WriteAllText(path, contents);
      pathFile = path;
      return true;
    }

    private static bool CreateDocumentFile(string tempFolder, out string pathFile)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Documents.Document> list = new DocumentsRepository(dataBase).GetItemsWithFilter(DateTime.Now.AddYears(-1), DateTime.Now).Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.SaleReturn, GlobalDictionaries.DocumentsTypes.Sale))).ToList<Gbs.Core.Entities.Documents.Document>();
        string path = Path.Combine(tempFolder, "documents.json");
        using (TextWriter text = (TextWriter) System.IO.File.CreateText(path))
          new JsonSerializer()
          {
            Formatting = Formatting.Indented
          }.Serialize(text, (object) list.Select<Gbs.Core.Entities.Documents.Document, Document>((Func<Gbs.Core.Entities.Documents.Document, Document>) (x => new Document(x))));
        pathFile = path;
        return true;
      }
    }

    private static string Url
    {
      get
      {
        return NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.Weboffice)).GetPingedUrl().Url.ToString();
      }
    }

    public static void CreateArchive(bool isShowMsg = false)
    {
      ProgressBarHelper.ProgressBar progressBar = (ProgressBarHelper.ProgressBar) null;
      try
      {
        Performancer performancer = new Performancer("Подготовка архива для Web Office");
        WebOfficeHelper._webOfficeSetting = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.WebOffice;
        if (!WebOfficeHelper._webOfficeSetting.IsActive || WebOfficeHelper._webOfficeSetting.Token.IsNullOrEmpty())
          return;
        WebOfficeHelper._infoDb = UidDb.GetUid();
        string str1 = FileSystemHelper.TempFolderPath();
        progressBar = new ProgressBarHelper.ProgressBar(Translate.WebOfficeHelper_CreateArchive_Подготовка_архива_для_Web_Office);
        string pathFile1;
        string pathFile2;
        string pathFile3;
        string pathFile4;
        string pathFile5;
        string pathFile6;
        if (!WebOfficeHelper.CreateCatalogFile(str1, out pathFile1) || !WebOfficeHelper.CreateGroupsGoodFile(str1, out pathFile2) || !WebOfficeHelper.CreateInfoFile(str1, out pathFile3) || !WebOfficeHelper.CreateDocumentFile(str1, out pathFile4) || !WebOfficeHelper.CreateClientsFile(str1, out pathFile5) || !WebOfficeHelper.CreateUsersFile(str1, out pathFile6))
          return;
        List<FileInfo> sourceFilePaths = new List<FileInfo>()
        {
          new FileInfo(pathFile1),
          new FileInfo(pathFile3),
          new FileInfo(pathFile4),
          new FileInfo(pathFile5),
          new FileInfo(pathFile6),
          new FileInfo(pathFile2)
        };
        string str2 = Path.Combine(str1, WebOfficeHelper._infoDb.EntityUid.ToString()) + ".zip";
        FileSystemHelper.CreateZip(str2, (IEnumerable<FileInfo>) sourceFilePaths);
        performancer.AddPoint("Подготовка архива");
        if (!WebOfficeHelper.SendFileTextAndMessage(str2))
        {
          progressBar.Close();
          if (!isShowMsg)
            return;
          MessageBoxHelper.Warning(Translate.WebOfficeHelper_CreateArchive_Не_удалось_выгрузить_архив_с_данными_для_WEB_офиса__Обратиесь_в_службу_технической_поддержки_);
        }
        else
        {
          progressBar.Close();
          Directory.Delete(str1, true);
          if (isShowMsg)
          {
            int num = (int) MessageBoxHelper.Show(Translate.WebOfficeHelper_CreateArchive_Архив_с_данными_успешно_выгружен_в_WEB_офис_);
          }
          performancer.Stop();
        }
      }
      catch (Exception ex)
      {
        progressBar?.Close();
        LogHelper.Error(ex, "Ошибка создания архива для WO", false);
      }
    }

    public static void SetStatusForPoint()
    {
      WebOfficeHelper._webOfficeSetting = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.WebOffice;
      if (!WebOfficeHelper._webOfficeSetting.IsActive || WebOfficeHelper._webOfficeSetting.Token.IsNullOrEmpty())
        return;
      WebOfficeHelper.DoCommand((WebOfficeHelper.WebOfficeCommand) new WebOfficeHelper.SetStatus()
      {
        UidDb = UidDb.GetUid().EntityUid.ToString()
      });
    }

    private static bool SendFileTextAndMessage(string path)
    {
      try
      {
        Gbs.Core.Config.WebOffice webOffice = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.WebOffice;
        if (webOffice.Token.IsNullOrEmpty())
          return false;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.DefaultConnectionLimit = 9999;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        string url = WebOfficeHelper.Url;
        if (url == null)
        {
          LogHelper.Debug("weboffice host is null");
          return false;
        }
        string requestUri = url + "api/Upload/sendFile";
        byte[] buffer = System.IO.File.ReadAllBytes(path);
        using (HttpClient httpClient = new HttpClient())
        {
          using (MultipartFormDataContent content = new MultipartFormDataContent())
          {
            content.Add((HttpContent) new StreamContent((Stream) new MemoryStream(buffer)), "Data", Path.GetFileName(path));
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "Data",
              FileName = Path.GetFileName(path)
            };
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", webOffice.Token ?? "");
            return httpClient.PostAsync(requestUri, (HttpContent) content).Result.StatusCode == HttpStatusCode.OK;
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправке данных в WebOffice");
        return false;
      }
    }

    private static bool DoCommand(WebOfficeHelper.WebOfficeCommand command)
    {
      try
      {
        Gbs.Core.Config.WebOffice webOffice = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.WebOffice;
        if (webOffice.Token.IsNullOrEmpty())
          return false;
        string url = WebOfficeHelper.Url;
        if (url == null)
        {
          LogHelper.Debug("weboffice host is null");
          return false;
        }
        string requestUri = url + command.Method + command.Data;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.DefaultConnectionLimit = 9999;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        using (HttpClient httpClient = new HttpClient())
        {
          httpClient.Timeout = TimeSpan.FromMinutes(3.0);
          httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", webOffice.Token ?? "");
          HttpResponseMessage result = httpClient.PostAsync(requestUri, (HttpContent) new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new List<KeyValuePair<string, string>>())).Result;
          if (result.StatusCode == HttpStatusCode.OK)
            return true;
          LogHelper.Debug(string.Format("Ошибка при отправке данных. Код состояния: {0}", (object) result.StatusCode));
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправке данных в WebOffice", false);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.WebOfficeHelper_DoCommand_Не_удалось_выгрузить_информаци_в_WebOffice__Обратитесь_в_службу_технической_поддержки_для_решения_вопроса_));
        return false;
      }
    }

    protected abstract class WebOfficeCommand
    {
      public virtual string Method { get; }

      public virtual string Data { get; }
    }

    protected class SetStatus : WebOfficeHelper.WebOfficeCommand
    {
      public override string Method => "api/Upload/SetStatus/";

      public override string Data => "?storeId=" + this.UidDb;

      public string UidDb { get; set; }
    }
  }
}
