// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.NewPayDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class NewPayDriver
  {
    private readonly string _userToken;

    public NewPayDriver(string userToken) => this._userToken = userToken;

    private string _apiKey => "Nz2xhQMv8IWlHPsQmh13";

    public void DoCommand(NewPayDriver.NewPayCommand command)
    {
      try
      {
        string jsonString = command.ToJsonString(isIgnoreNull: true);
        LogHelper.Debug("Начинаю выполнять комманду 'Яндекс Пэй': " + Other.NewLine() + jsonString);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://newpayb2b.pro/" + command.Url);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = command.Method;
        httpWebRequest.Headers.Add("api-key", this._apiKey);
        httpWebRequest.Headers.Add("user-token", this._userToken);
        if (command.Method == "POST")
        {
          using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            streamWriter.Write(jsonString);
        }
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream, true))
          command.AnswerString = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ  NewPay :\r\n" + command.AnswerString);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошиба отправки команды на 'Яндекс Пэй'");
      }
    }

    public abstract class NewPayCommand
    {
      [JsonIgnore]
      public virtual string Method { get; } = "POST";

      [JsonIgnore]
      public virtual string Url { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public class NewPayAnswer
    {
      [JsonProperty("code")]
      public int Code { get; set; }

      [JsonProperty("codeError")]
      public int CodeError { get; set; }

      [JsonProperty("error")]
      public string Error { get; set; }
    }

    public class CreateOperationCommand : NewPayDriver.NewPayCommand
    {
      public override string Url => "api/s1/operation/create";

      [JsonProperty("key")]
      public string KeyKassa { get; set; }

      [JsonProperty("cost")]
      public Decimal Sum { get; set; }

      [JsonIgnore]
      public NewPayDriver.CreateOperationCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<NewPayDriver.CreateOperationCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : NewPayDriver.NewPayAnswer
      {
        [JsonProperty("qrKey")]
        public string Qr { get; set; }

        [JsonProperty("sale_token")]
        public string SaleToken { get; set; }

        [JsonProperty("data_socket")]
        public NewPayDriver.DataSocketAnswer DataSocket { get; set; }
      }
    }

    public class ReturnOperationCommand : NewPayDriver.NewPayCommand
    {
      public override string Url => "api/s1/operation/return";

      [JsonProperty("sale_token")]
      public string SaleToken { get; set; }

      [JsonProperty("cost")]
      public Decimal Sum { get; set; }

      [JsonIgnore]
      public NewPayDriver.NewPayAnswer Result
      {
        get => JsonConvert.DeserializeObject<NewPayDriver.NewPayAnswer>(this.AnswerString);
      }
    }

    public class GetAccountsCommand : NewPayDriver.NewPayCommand
    {
      public override string Method => "GET";

      public override string Url => "api/s1/accounts";

      [JsonIgnore]
      public List<NewPayDriver.GetAccountsCommand.Account> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<List<NewPayDriver.GetAccountsCommand.Account>>(this.AnswerString);
        }
      }

      [JsonIgnore]
      public NewPayDriver.NewPayAnswer ErrorResult
      {
        get
        {
          try
          {
            return JsonConvert.DeserializeObject<NewPayDriver.NewPayAnswer>(this.AnswerString);
          }
          catch
          {
            return (NewPayDriver.NewPayAnswer) null;
          }
        }
      }

      public class Account
      {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string KeyKassa { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }
      }
    }

    public class GetStatusOperationCommand : NewPayDriver.NewPayCommand
    {
      public string SaleToken { get; set; }

      public override string Method => "GET";

      public override string Url => "operation/result?sale_token=" + this.SaleToken;

      [JsonIgnore]
      public NewPayDriver.GetStatusOperationCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<NewPayDriver.GetStatusOperationCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : NewPayDriver.NewPayAnswer
      {
        [JsonProperty("sale_token")]
        public string SaleToken { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
      }
    }

    public class CancelOperationCommand : NewPayDriver.NewPayCommand
    {
      public override string Url => "api/s1/operation/cancel";

      [JsonProperty("key")]
      public string KeyKassa { get; set; }

      [JsonIgnore]
      public NewPayDriver.NewPayAnswer Result
      {
        get => JsonConvert.DeserializeObject<NewPayDriver.NewPayAnswer>(this.AnswerString);
      }
    }

    public class DataSocketAnswer
    {
      [JsonProperty("web_socket_server")]
      public string WebSocketServer { get; set; }
    }

    public class DataSocketCommand
    {
      [JsonProperty("sale_token")]
      public string SaleToken { get; set; }
    }

    public class OperationAnswer
    {
      [JsonProperty("sale_token")]
      public string SaleToken { get; set; }

      [JsonProperty("result")]
      public bool Result { get; set; }
    }
  }
}
