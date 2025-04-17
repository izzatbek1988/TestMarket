// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TelegramHelper_v2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

#nullable disable
namespace Gbs.Helpers
{
  public class TelegramHelper_v2
  {
    private TelegramHelper_v2.ServerData ServerInfo { get; set; }

    public TelegramHelper_v2()
    {
      Settings settings = new ConfigsRepository<Settings>().Get();
      string str = "https://ru-repors.app-pos.ru";
      if (settings.Interface.Country == GlobalDictionaries.Countries.Ukraine)
        str = "https://ua-reports.app-pos.com";
      this.ServerInfo = new TelegramHelper_v2.ServerData()
      {
        Url = str,
        ApiKey = "f9ca53ea-6c3d-45b0-83d4-6d263b4f95be"
      };
    }

    public void SendText(string chatIdArray, string message)
    {
      if (this.ServerInfo == null)
      {
        LogHelper.Trace("Сервер ТГ-бота не указан");
      }
      else
      {
        string str = chatIdArray;
        char[] separator = new char[3]{ ',', ' ', ';' };
        foreach (string chatId in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          this.SendTextToServer(chatId, message);
      }
    }

    public void SendFile(string chatIdArray, string filePath)
    {
      if (this.ServerInfo == null)
      {
        LogHelper.Trace("Сервер ТГ-бота не указан");
      }
      else
      {
        LogHelper.Debug("Начинаем отправлять файл через ТГ: " + filePath);
        string str = chatIdArray;
        char[] separator = new char[3]{ ',', ' ', ';' };
        foreach (string chatId in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          this.SendFileToServer(chatId, filePath);
      }
    }

    private void SendTextToServer(string chatId, string message)
    {
      try
      {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string requestUri = this.ServerInfo.Url + "/api/Message/textmessage";
        using (HttpClient httpClient = new HttpClient())
        {
          httpClient.DefaultRequestHeaders.Add("x-api-secret", this.ServerInfo.ApiKey);
          string content = JsonConvert.SerializeObject((object) new TelegramHelper_v2.TgMessage()
          {
            chatId = chatId,
            message = message
          });
          HttpResponseMessage result = httpClient.PostAsync(requestUri, (HttpContent) new StringContent(content, System.Text.Encoding.UTF8, "application/json")).Result;
          if (result.StatusCode != HttpStatusCode.OK)
          {
            LogHelper.Debug(result.Content.ReadAsStringAsync().Result);
            throw new WebException(Translate.TelegramHelper_v2_SendTextToServer_);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки сообщения в телеграм", false, false);
        throw;
      }
    }

    private void SendFileToServer(string chatId, string filePath)
    {
      try
      {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string requestUri = this.ServerInfo.Url + "/api/Message/sendfile";
        byte[] buffer = System.IO.File.ReadAllBytes(filePath);
        using (HttpClient httpClient = new HttpClient())
        {
          using (MultipartFormDataContent content = new MultipartFormDataContent())
          {
            content.Add((HttpContent) new StreamContent((Stream) new MemoryStream(buffer)), "data", Path.GetFileName(filePath));
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "data",
              FileName = Path.GetFileName(filePath)
            };
            content.Add((HttpContent) new StringContent(chatId), nameof (chatId));
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = nameof (chatId)
            };
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-secret", this.ServerInfo.ApiKey ?? "");
            HttpResponseMessage result = httpClient.PostAsync(requestUri, (HttpContent) content).Result;
            LogHelper.Debug("Результат отправки файла через ТГ: " + result.StatusCode.ToString());
            LogHelper.Debug("chatId: " + chatId);
            if (result.StatusCode != HttpStatusCode.OK)
              throw new WebException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки файла в телеграм", false, false);
        throw;
      }
    }

    public void SendEmailToServer(Gbs.Core.Entities.Emails.Email email)
    {
      try
      {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string requestUri = this.ServerInfo.Url + "/api/Message/SendEmail";
        using (HttpClient httpClient = new HttpClient())
        {
          using (MultipartFormDataContent content = new MultipartFormDataContent())
          {
            if (email.Attach.Any<string>())
            {
              foreach (string path in email.Attach)
              {
                byte[] buffer = System.IO.File.ReadAllBytes(path);
                content.Add((HttpContent) new StreamContent((Stream) new MemoryStream(buffer)), "Files", Path.GetFileName(path));
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                  Name = "data",
                  FileName = Path.GetFileName(path)
                };
              }
            }
            content.Add((HttpContent) new StringContent(email.Body.ToString()), "Body");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "Body"
            };
            content.Add((HttpContent) new StringContent(email.Subject), "Subject");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "Subject"
            };
            content.Add((HttpContent) new StringContent(email.AddressTo), "Receivers");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "Receivers"
            };
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-secret", this.ServerInfo.ApiKey);
            if (httpClient.PostAsync(requestUri, (HttpContent) content).Result.StatusCode != HttpStatusCode.OK)
              throw new WebException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки Email", false, false);
        throw;
      }
    }

    private class ServerData
    {
      public string Url { get; set; }

      public string ApiKey { get; set; }
    }

    public class TgMessage
    {
      public string chatId { get; set; }

      public string message { get; set; }
    }
  }
}
