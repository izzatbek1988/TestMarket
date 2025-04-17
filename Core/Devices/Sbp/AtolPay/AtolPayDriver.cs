// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.AtolPayDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class AtolPayDriver
  {
    private readonly string _userToken;

    public AtolPayDriver(string userToken) => this._userToken = userToken;

    public void DoCommand(AtolPayDriver.AtolPayCommand command)
    {
      try
      {
        string jsonString = command.ToJsonString();
        LogHelper.Debug("Начинаю выполнять комманду AtolPay: " + Other.NewLine() + jsonString);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://api.atolpay.ru/v1/" + command.Url);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = command.Method;
        httpWebRequest.Headers.Add("Authorization", "Bearer " + this._userToken);
        if (command.Method == "POST")
        {
          using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            streamWriter.Write(jsonString);
        }
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.AnswerString = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ  AtolPay :\r\n" + command.AnswerString);
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        string end = new StreamReader(responseStream)?.ReadToEnd();
        command.AnswerString = end;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошиба отправки команды на  AtolPay");
      }
    }

    public abstract class AtolPayCommand
    {
      [JsonIgnore]
      public virtual string Method { get; } = "POST";

      [JsonIgnore]
      public virtual string Url { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public class AtolPayAnswer
    {
      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("errorCode")]
      public string ErrorCode { get; set; }

      [JsonProperty("errorMessage")]
      public string ErrorMessage { get; set; }
    }

    public class CreateOperationCommand : AtolPayDriver.AtolPayCommand
    {
      public override string Url => "qr/register";

      [JsonProperty("rmkId")]
      public string KassaId { get; set; }

      [JsonProperty("amount")]
      public int Sum { get; set; }

      [JsonIgnore]
      public AtolPayDriver.CreateOperationCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolPayDriver.CreateOperationCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : AtolPayDriver.AtolPayAnswer
      {
        [JsonProperty("data")]
        public AtolPayDriver.CreateOperationCommand.Data Data { get; set; } = new AtolPayDriver.CreateOperationCommand.Data();
      }

      public class Data
      {
        [JsonProperty("payload")]
        public string Qr { get; set; }

        [JsonProperty("transactionId")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
      }
    }

    public class ReturnOperationCommand : AtolPayDriver.AtolPayCommand
    {
      public override string Url => "qr/refund";

      [JsonProperty("transactionId")]
      public string OrderId { get; set; }

      [JsonProperty("amount")]
      public int Sum { get; set; }

      [JsonIgnore]
      public AtolPayDriver.AtolPayAnswer Result
      {
        get => JsonConvert.DeserializeObject<AtolPayDriver.AtolPayAnswer>(this.AnswerString);
      }
    }

    public class GetStatusOperationCommand : AtolPayDriver.AtolPayCommand
    {
      [JsonProperty("transactionId")]
      public string OrderId { get; set; }

      public override string Method => "POST";

      public override string Url => "qr/status";

      [JsonIgnore]
      public AtolPayDriver.GetStatusOperationCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolPayDriver.GetStatusOperationCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : AtolPayDriver.AtolPayAnswer
      {
        [JsonProperty("data")]
        public AtolPayDriver.GetStatusOperationCommand.Data Data { get; set; } = new AtolPayDriver.GetStatusOperationCommand.Data();
      }

      public class Data
      {
        [JsonProperty("transactionId")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
      }
    }
  }
}
