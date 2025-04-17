// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.Polycard.PalycardApi
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers.API.Polycard.Entity;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers.API.Polycard
{
  public static class PalycardApi
  {
    private const string PolyApiUrl = "/polycard_api/v1/";

    private static Gbs.Core.Config.Polycard PolyConfigs
    {
      get => new ConfigsRepository<Integrations>().Get().Polycard;
    }

    private static List<ClientAdnSum> Cache
    {
      get
      {
        return CacheHelper.Get<List<ClientAdnSum>>(CacheHelper.CacheTypes.ClientForPolyCard, new Func<List<ClientAdnSum>>(PalycardApi.LoadCache));
      }
    }

    public static void StartListeners()
    {
      if (!PalycardApi.PolyConfigs.IsActive)
        return;
      if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
      {
        MessageBoxHelper.Error(Translate.PalycardApi_Запуск_службы_Polycard_возможен_только_с_правами_администратора__Перезапустите_программу_от_имени_администратора_Windows__);
      }
      else
      {
        LogHelper.Debug("Запуск службы PolyCard");
        Task.Run((Action) (() =>
        {
          try
          {
            LogHelper.Debug(string.Format("Записей в кэше: {0}", (object) PalycardApi.Cache.Count));
            PalycardApi.ListenerStart();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex);
          }
        }));
      }
    }

    private static List<ClientAdnSum> LoadCache()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        return new ClientsRepository(dataBase).GetListActiveItemAndSum().ToList<ClientAdnSum>();
    }

    private static string BaseUrl
    {
      get => "http://*:" + PalycardApi.PolyConfigs.Port.ToString() + "/polycard_api/v1/";
    }

    private static void GetClient(
      HttpListenerContext context,
      HttpListenerResponse response,
      HttpListenerRequest request)
    {
      if (!PalycardApi.CheckAuth(context))
      {
        IAnswer answer = (IAnswer) new ErrorAnswer("Требуется авторизация", AnswerStatuses.NotAuthorized);
        PalycardApi.SendAnswer(response, answer);
      }
      else if (request.HttpMethod == "POST")
      {
        IAnswer answer = (IAnswer) new ErrorAnswer(Translate.PalycardApi_Необходимо_использовать_GET_метод);
        PalycardApi.SendAnswer(response, answer);
      }
      else
      {
        Encoding contentEncoding = request.ContentEncoding;
        string phone = request.QueryString["phone"] ?? string.Empty;
        string str = request.QueryString["uid"] ?? string.Empty;
        if (!phone.IsNullOrEmpty() && !str.IsNullOrEmpty())
        {
          IAnswer answer = (IAnswer) new ErrorAnswer(Translate.PalycardApi_Может_быть_задан_только_один_параметр_для_поиска);
          PalycardApi.SendAnswer(response, answer);
        }
        else if (phone.IsNullOrEmpty() && str.IsNullOrEmpty())
        {
          IAnswer answer = (IAnswer) new ErrorAnswer(Translate.PalycardApi_Должен_быть_задан_параметр_для_поиска);
          PalycardApi.SendAnswer(response, answer);
        }
        else
        {
          phone = PalycardApi.ToUtf8(contentEncoding, phone);
          string utf8 = PalycardApi.ToUtf8(contentEncoding, str);
          ClientAdnSum gbsClient = (ClientAdnSum) null;
          if (!utf8.IsNullOrEmpty())
          {
            Guid clientUid;
            try
            {
              clientUid = Guid.Parse(utf8);
            }
            catch
            {
              IAnswer answer = (IAnswer) new ErrorAnswer(Translate.PalycardApi_UID_имеет_некорректный_формат);
              PalycardApi.SendAnswer(response, answer);
              return;
            }
            gbsClient = PalycardApi.Cache.SingleOrDefault<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => x.Client.Uid == clientUid && !x.Client.IsDeleted));
            if (gbsClient == null)
            {
              PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer(string.Format("Контакт с UID {0} не найден", (object) clientUid), AnswerStatuses.NotFound));
              return;
            }
          }
          if (!phone.IsNullOrEmpty())
          {
            gbsClient = PalycardApi.Cache.FirstOrDefault<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => x.Client.Phone.GetOnlyNumbers() == phone.GetOnlyNumbers() && !x.Client.IsDeleted));
            if (gbsClient == null)
            {
              PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Контакт с телефоном " + phone + " не найден", AnswerStatuses.NotFound));
              return;
            }
          }
          if (gbsClient == null)
          {
            PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Контакт не удалось найти", AnswerStatuses.NotFound));
          }
          else
          {
            Gbs.Helpers.API.Polycard.Entity.Client client = new Gbs.Helpers.API.Polycard.Entity.Client(gbsClient);
            PalycardApi.SendAnswer(response, client.ToJsonString(true));
          }
        }
      }
    }

    private static bool CheckAuth(HttpListenerContext context)
    {
      HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity) context.User.Identity;
      return identity.Name == PalycardApi.PolyConfigs.DecryptedLogin && identity.Password == PalycardApi.PolyConfigs.DecryptedPassword;
    }

    private static string ToUtf8(Encoding encoding, string phone)
    {
      return Encoding.UTF8.GetString(encoding.GetBytes(phone));
    }

    private static void SendAnswer(HttpListenerResponse response, IAnswer answer)
    {
      PalycardApi.SendAnswer(response, answer.ToJsonString(true));
    }

    private static void SendAnswer(HttpListenerResponse response, string answer)
    {
      LogHelper.Debug("Отправка ответа: " + answer);
      byte[] bytes = Encoding.UTF8.GetBytes(answer);
      response.ContentLength64 = (long) bytes.Length;
      response.ContentEncoding = Encoding.UTF8;
      response.ContentType = "application/json; charset=utf-8";
      Stream outputStream = response.OutputStream;
      outputStream.Write(bytes, 0, bytes.Length);
      outputStream.Close();
    }

    private static async Task ListenerStart()
    {
      HttpListener listener = new HttpListener();
      listener.Prefixes.Add(PalycardApi.BaseUrl);
      listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
      try
      {
        listener.Start();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка запуска службы PolyCard API");
        goto label_7;
      }
      while (true)
      {
        try
        {
          await PalycardApi.DoListen(listener);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }
label_7:
      listener = (HttpListener) null;
    }

    private static async Task DoListen(HttpListener listener)
    {
      HttpListenerContext contextAsync = await listener.GetContextAsync();
      HttpListenerRequest request = contextAsync.Request;
      HttpListenerResponse response = contextAsync.Response;
      if (request.Url.ToString().ToLower().Contains("/polycard_api/v1/addClient".ToLower()))
        PalycardApi.AddClient(contextAsync, response, request);
      else if (request.Url.ToString().ToLower().Contains("/polycard_api/v1/getClient".ToLower()))
        PalycardApi.GetClient(contextAsync, response, request);
      else
        PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Не удалось распознать тип запроса. Некорректный URL"));
    }

    private static void AddClient(
      HttpListenerContext context,
      HttpListenerResponse response,
      HttpListenerRequest request)
    {
      if (!PalycardApi.CheckAuth(context))
        PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer(Translate.PalycardApi_Требуется_авторизация, AnswerStatuses.NotAuthorized));
      else if (request.HttpMethod != "POST")
      {
        PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer(Translate.PalycardApi_Необходимо_использовать_метод_POST));
      }
      else
      {
        Gbs.Helpers.API.Polycard.Entity.Client client = JsonConvert.DeserializeObject<Gbs.Helpers.API.Polycard.Entity.Client>(new StreamReader(context.Request.InputStream).ReadToEnd());
        if (client.Phone.Length < 10)
          PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Номер телефона должен быть не менее 10 симоволов"));
        else if (client.Name.Length < 3)
        {
          PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Имя должно быть не менее 3 символов"));
        }
        else
        {
          ClientAdnSum gbsClient = PalycardApi.Cache.FirstOrDefault<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => x.Client.Phone.GetOnlyNumbers() == client.Phone.GetOnlyNumbers() && !x.Client.IsDeleted));
          if (gbsClient != null)
            PalycardApi.SendAnswer(response, (IAnswer) new ExistedAnswer(new Gbs.Helpers.API.Polycard.Entity.Client(gbsClient)));
          else if (PalycardApi.PolyConfigs.GroupContactUid == Guid.Empty)
          {
            PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Не указана группа, в которую будут добавляться контакты. Необходимо проверить настройки интеграции в GBS.Market"));
          }
          else
          {
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              Group byUid = new GroupRepository(dataBase).GetByUid(PalycardApi.PolyConfigs.GroupContactUid);
              if (byUid == null)
              {
                PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer(string.Format("Группа контактов с UID {0} не найдена в БД. Необходимо проверить настройки интеграции в GBS.Market", (object) PalycardApi.PolyConfigs.GroupContactUid)));
              }
              else
              {
                Gbs.Core.Entities.Clients.Client client1 = new Gbs.Core.Entities.Clients.Client()
                {
                  Name = (client.SurName + " " + client.Name + " " + client.MiddleName).Trim(),
                  Email = client.Email,
                  Phone = client.Phone,
                  Birthday = client.Birthday,
                  Group = byUid
                };
                if (new ClientsRepository(dataBase).Save(client1))
                {
                  PalycardApi.SendAnswer(response, (IAnswer) new SuccessAnswer(client1.Uid, client1.Barcode));
                  CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.ClientForPolyCard);
                }
                else
                  PalycardApi.SendAnswer(response, (IAnswer) new ErrorAnswer("Контакт не был сохранен. Неизвестная ошибка"));
              }
            }
          }
        }
      }
    }
  }
}
