// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Server
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.API.GbsApi.v1.Entities;
using Gbs.Helpers.API.GbsApi.v1.Repos;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1
{
  public static class Server
  {
    private static string ApiUrl = "/api/v1/";

    public static JsonApi ApiConfig => new ConfigsRepository<Integrations>().Get().JsonApi;

    public static void StartServer()
    {
      if (!Server.ApiConfig.IsEnable)
        return;
      if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
      {
        Server.BaseUrl = "http://*";
      }
      else
      {
        Server.BaseUrl = "http://localhost";
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Text = Translate.Server_StartServer_JSON_API_прослушивает_только_localhost_запросы__т_к__программа_запущена_без_прав_администратора_ОС,
          Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
        });
      }
      Server.BaseUrl = Server.BaseUrl + ":" + Server.ApiConfig.PortNumber.ToString() + Server.ApiUrl;
      LogHelper.Debug("Запуск web api на " + Server.BaseUrl);
      Task.Run((Action) (() =>
      {
        try
        {
          Server.ListenerStart();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));
    }

    private static string BaseUrl { get; set; }

    private static async Task ListenerStart()
    {
      HttpListener listener = new HttpListener();
      listener.Prefixes.Add(Server.BaseUrl);
      listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
      try
      {
        listener.Start();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка запуска службы Web API");
        goto label_7;
      }
      while (true)
      {
        try
        {
          await Server.DoListen(listener);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }
label_7:
      listener = (HttpListener) null;
    }

    private static void CheckAuth(HttpListenerContext context)
    {
      HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity) context.User.Identity;
      if ((!(identity.Name == Server.ApiConfig.Login.DecryptedValue) ? 0 : (identity.Password == Server.ApiConfig.Password.DecryptedValue ? 1 : 0)) == 0)
        throw new AuthenticationException(Translate.Server_CheckAuth_Ошибка_авторизации);
    }

    private static async Task DoListen(HttpListener listener)
    {
      HttpListenerContext contextAsync = await listener.GetContextAsync();
      HttpListenerRequest request = contextAsync.Request;
      HttpListenerResponse response = contextAsync.Response;
      response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
      response.AddHeader("Pragma", "no-cache");
      response.AddHeader("Expires", "0");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Uri url = request.Url;
      try
      {
        Server.CheckAuth(contextAsync);
        string lower = url.Segments[3].Replace("/", "").ToLower();
        IAnswer answer;
        switch (lower)
        {
          case "status":
            answer = (IAnswer) new SingleObjectAnswer()
            {
              Status = AnswerStatuses.Ok,
              Data = (IEntity) new StatusInfo()
            };
            break;
          case "load_caches":
            CacheHelper.Clear(CacheHelper.CacheTypes.AllGoods);
            CachesBox.AllGoods();
            answer = (IAnswer) new SingleObjectAnswer()
            {
              Status = AnswerStatuses.Ok,
              Data = (IEntity) new MessageInfo()
              {
                Message = Translate.Server_DoListen_Кэш_данных_успешно_обновлен
              }
            };
            break;
          default:
            answer = Server.GetAnswer(lower, url);
            break;
        }
        if (answer == null)
          throw new NullReferenceException("Answer is null");
        answer.ResponseTime = stopwatch.ElapsedMilliseconds;
        Server.SendAnswer(response, answer);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "", false);
        SingleObjectAnswer singleObjectAnswer = new SingleObjectAnswer()
        {
          Status = AnswerStatuses.Error,
          ResponseTime = stopwatch.ElapsedMilliseconds,
          Data = (IEntity) new Error()
          {
            Message = ex.Message,
            Stack = ex.StackTrace
          }
        };
        Server.SendAnswer(response, (IAnswer) singleObjectAnswer);
      }
      SingleObjectAnswer singleObjectAnswer1 = new SingleObjectAnswer()
      {
        Status = AnswerStatuses.Error,
        ResponseTime = stopwatch.ElapsedMilliseconds,
        Data = (IEntity) new Error()
        {
          Message = Translate.Server_DoListen_Неизвестный_запрос
        }
      };
      Server.SendAnswer(response, (IAnswer) singleObjectAnswer1);
    }

    private static IAnswer GetAnswer(string segment, Uri uri)
    {
      IRepository repository;
      switch (segment)
      {
        case "goods":
          repository = (IRepository) new GoodsRepository();
          break;
        case "good_groups":
          repository = (IRepository) new GoodGroupsRepository();
          break;
        case "documents":
          repository = (IRepository) new DocumentsRepository();
          break;
        default:
          throw new NullReferenceException(Translate.Server_GetAnswer_Неизвестный_репозиторий);
      }
      return repository.GetData(uri);
    }

    private static void SendAnswer(HttpListenerResponse response, IAnswer answer)
    {
      Server.SendAnswer(response, answer.ToJsonString(true));
    }

    private static void SendAnswer(HttpListenerResponse response, string answer)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(answer);
      response.ContentLength64 = (long) bytes.Length;
      response.ContentEncoding = Encoding.UTF8;
      response.ContentType = "application/json; charset=utf-8";
      Stream outputStream = response.OutputStream;
      outputStream.Write(bytes, 0, bytes.Length);
      outputStream.Close();
    }
  }
}
