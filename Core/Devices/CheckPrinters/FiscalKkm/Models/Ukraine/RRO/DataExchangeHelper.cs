// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.RRO.DataExchangeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.RRO
{
  public static class DataExchangeHelper
  {
    public static void SendCommand(IRequest request)
    {
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("http://localhost:80/fsapi/");
        httpWebRequest.ContentType = "application/json; charset=UTF-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 120000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        string str = JsonConvert.SerializeObject((object) request, Formatting.Indented, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Include
        });
        LogHelper.Debug("commnand json: " + str);
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          request.AnswerString = streamReader.ReadToEnd();
      }
      catch (WebException ex)
      {
        LogHelper.Error((Exception) ex, "Ошибка отправки комманды");
        switch (ex.Status)
        {
          case WebExceptionStatus.ProtocolError:
            if (!(ex.Response is HttpWebResponse response))
              break;
            int statusCode = (int) response.StatusCode;
            break;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки комманды");
      }
    }
  }
}
