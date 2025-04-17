// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.RestHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public class RestHelper
  {
    private HttpWebRequest _client;
    private TypeRestRequest _typeRequest;
    private readonly string _body;
    private readonly string _url;
    private string _path;
    private bool _writeLog;

    [Localizable(false)]
    public override string ToString()
    {
      return string.Format("URL: {0}\n", (object) this._client.RequestUri) + "Method: " + this._client.Method + "\n" + string.Format("Headers: \n{0}\n", (object) this._client.Headers) + "Body: \n" + this._body + "\n--------------";
    }

    public string Answer { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public RestHelper(string url, int? port, string body, bool writeLog = true)
    {
      if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
        url = "http://" + url;
      this._url = url + (!port.HasValue ? "" : string.Format(":{0}", (object) port));
      this._body = body;
      this._writeLog = writeLog;
    }

    public void CreateCommand(string path, TypeRestRequest typeRequest)
    {
      this._typeRequest = typeRequest;
      this._path = path;
      this._client = (HttpWebRequest) WebRequest.Create(this._url + this._path);
      this._client.ContentType = "application/json";
      this._client.Timeout = 60000;
      this._client.ReadWriteTimeout = 120000;
      this._client.KeepAlive = false;
      HttpWebRequest client = this._client;
      string str;
      switch (this._typeRequest)
      {
        case TypeRestRequest.Get:
          str = "GET";
          break;
        case TypeRestRequest.Post:
          str = "POST";
          break;
        case TypeRestRequest.Put:
          str = "PUT";
          break;
        case TypeRestRequest.Delete:
          str = "DELETE";
          break;
        default:
          throw new ArgumentException();
      }
      client.Method = str;
    }

    public void SetTimeout(Decimal timeout) => this._client.Timeout = (int) (timeout * 1000M);

    public void DoCommand()
    {
      this.StatusCode = HttpStatusCode.OK;
      if (this._writeLog)
      {
        LogHelper.Debug(string.Format("Выполнение {0} запроса: {1}, тело:\n{2}", (object) this._client.Method, (object) this._client.Address, (object) this._body));
        LogHelper.Debug("Заголовки запроса\n: " + this._client.Headers.ToJsonString(true));
      }
      foreach (string header in (NameObjectCollectionBase) this._client.Headers)
      {
        if (this._writeLog)
          LogHelper.Debug("Заголовок: " + header + ", значение: " + this._client.Headers.Get(header));
      }
      try
      {
        this.StatusCode = HttpStatusCode.OK;
        switch (this._typeRequest)
        {
          case TypeRestRequest.Get:
            this.Get();
            break;
          case TypeRestRequest.Post:
            this.Post();
            break;
          case TypeRestRequest.Put:
            this.Get();
            break;
          case TypeRestRequest.Delete:
            break;
          default:
            throw new ArgumentOutOfRangeException("_typeRequest", (object) this._typeRequest, (string) null);
        }
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        this.Answer = new StreamReader(responseStream)?.ReadToEnd();
        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse response)
          this.StatusCode = response.StatusCode;
        else
          this.StatusCode = (HttpStatusCode) ex.Status;
      }
      catch (Exception ex)
      {
        string logMessage = "Ошибка выполнения rest-запроса " + this._client.Address.AbsolutePath;
        LogHelper.WriteError(ex, logMessage);
      }
      finally
      {
        if (this._writeLog)
          LogHelper.Debug(string.Format("Ответ на {0} запрос: {1}, ответ:\n{2}, StatusCode = {3}", (object) this._client.Method, (object) this._client.Address.AbsolutePath, (object) this.Answer, (object) this.StatusCode));
      }
    }

    public void AddHeader(string name, string value) => this._client.Headers.Add(name, value);

    public void SetAccept(string value) => this._client.Accept = value;

    private void Get()
    {
      HttpWebResponse response = (HttpWebResponse) this._client.GetResponse();
      LogHelper.Debug("get запрос: " + response.StatusCode.ToString());
      Stream responseStream = response.GetResponseStream();
      if (responseStream == null)
        throw new InvalidOperationException();
      using (StreamReader streamReader = new StreamReader(responseStream))
        this.Answer = streamReader.ReadToEnd();
    }

    private void Post()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._client.GetRequestStream()))
        streamWriter.Write(this._body);
      this.Get();
    }

    public abstract class RestCommand
    {
      [JsonIgnore]
      public virtual string Path { get; }

      [JsonIgnore]
      public virtual TypeRestRequest Type { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }
  }
}
