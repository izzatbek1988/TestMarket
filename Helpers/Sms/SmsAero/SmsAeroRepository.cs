// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsAeroRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public class SmsAeroRepository
  {
    private string _apiKey { get; set; }

    private string _login { get; set; }

    public SmsAeroRepository(string apiKey, string login)
    {
      this._apiKey = apiKey;
      this._login = login;
    }

    public void DoCommand(SmsAeroRepository.Command command)
    {
      try
      {
        string requestUriString = "https://gate.smsaero.ru/v2/" + command.RequestStr;
        LogHelper.Debug("Отправляем запрос на отправку SmsAero " + requestUriString);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.Credentials = (ICredentials) new NetworkCredential(this._login, this._apiKey);
        httpWebRequest.Method = "GET";
        httpWebRequest.Timeout = 120000;
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
        {
          command.AnswerString = streamReader.ReadToEnd();
          LogHelper.Debug("Получен ответ SmsAero:\n" + command.AnswerString);
        }
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        string end = new StreamReader(responseStream)?.ReadToEnd();
        command.AnswerString = end;
      }
    }

    private static string UrlEncode(string data) => WebUtility.UrlEncode(data);

    public abstract class Command
    {
      public virtual string RequestStr { get; }

      public string AnswerString { get; set; }

      public SmsAeroRepository.AllAnswer Result
      {
        get => JsonConvert.DeserializeObject<SmsAeroRepository.AllAnswer>(this.AnswerString);
      }
    }

    public class AllAnswer
    {
      [JsonProperty("success")]
      public bool Success { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
    }

    public class SendSmsCommand : SmsAeroRepository.Command
    {
      public string Sender { get; set; }

      public List<string> Phones { get; set; }

      public string SmsText { get; set; }

      public SmsAeroRepository.SendSmsCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsAeroRepository.SendSmsCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public SmsAeroRepository.AllAnswer AnswerError
      {
        get => JsonConvert.DeserializeObject<SmsAeroRepository.AllAnswer>(this.AnswerString);
      }

      public override string RequestStr
      {
        get
        {
          return "sms/send?numbers[]=" + string.Join("&numbers[]=", (IEnumerable<string>) this.Phones) + "&text=" + SmsAeroRepository.UrlEncode(this.SmsText) + (this.Sender.IsNullOrEmpty() ? "" : "&sign=" + SmsAeroRepository.UrlEncode(this.Sender));
        }
      }

      public class AnswerCommand : SmsAeroRepository.AllAnswer
      {
        [JsonProperty("data")]
        public List<SmsAeroRepository.SendSmsCommand.PhoneAnswer> Data { get; set; } = new List<SmsAeroRepository.SendSmsCommand.PhoneAnswer>();

        public class Date
        {
          public List<SmsAeroRepository.SendSmsCommand.PhoneAnswer> sign { get; set; }
        }
      }

      public class PhoneAnswer
      {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("extendStatus")]
        public string ExtendStatus { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("cost")]
        public double Cost { get; set; }

        [JsonProperty("dateCreate")]
        public int DateCreate { get; set; }

        [JsonProperty("dateSend")]
        public int DateSend { get; set; }
      }
    }

    public class SendWhatsAppCommand : SmsAeroRepository.Command
    {
      public string Sign { get; set; }

      public string Address { get; set; }

      public string ContentType { get; set; } = "text";

      public string Text { get; set; }

      public SmsAeroRepository.SendWhatsAppCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsAeroRepository.SendWhatsAppCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public SmsAeroRepository.AllAnswer AnswerError
      {
        get => JsonConvert.DeserializeObject<SmsAeroRepository.AllAnswer>(this.AnswerString);
      }

      public override string RequestStr
      {
        get
        {
          return "whatsapp/send?address=" + this.Address + "&contentType=" + this.ContentType + "&text=" + SmsAeroRepository.UrlEncode(this.Text) + "&sign=" + SmsAeroRepository.UrlEncode(this.Sign);
        }
      }

      public class AnswerCommand : SmsAeroRepository.AllAnswer
      {
        [JsonProperty("data")]
        public SmsAeroRepository.SendSmsCommand.PhoneAnswer Data { get; set; } = new SmsAeroRepository.SendSmsCommand.PhoneAnswer();
      }
    }

    public class SendViberCommand : SmsAeroRepository.Command
    {
      public string Sign { get; set; }

      public List<string> Phones { get; set; }

      public string ContentType { get; set; } = "text";

      public string Text { get; set; }

      public string Channel { get; set; } = "CASCADE";

      public SmsAeroRepository.SendViberCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsAeroRepository.SendViberCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public SmsAeroRepository.AllAnswer AnswerError
      {
        get => JsonConvert.DeserializeObject<SmsAeroRepository.AllAnswer>(this.AnswerString);
      }

      public override string RequestStr
      {
        get
        {
          return "viber/send?numbers[]=" + string.Join("&numbers[]=", (IEnumerable<string>) this.Phones) + "&text=" + SmsAeroRepository.UrlEncode(this.Text) + "&sign=" + SmsAeroRepository.UrlEncode(this.Sign) + "&channel=" + this.Channel;
        }
      }

      public class AnswerCommand : SmsAeroRepository.AllAnswer
      {
        [JsonProperty("data")]
        public SmsAeroRepository.SendSmsCommand.PhoneAnswer Data { get; set; } = new SmsAeroRepository.SendSmsCommand.PhoneAnswer();
      }
    }

    public class GetBalanceCommand : SmsAeroRepository.Command
    {
      public string Phone { get; set; }

      public override string RequestStr => "balance";

      public SmsAeroRepository.GetBalanceCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsAeroRepository.GetBalanceCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public class AnswerCommand : SmsAeroRepository.AllAnswer
      {
        [JsonProperty("data")]
        public SmsAeroRepository.GetBalanceCommand.AnswerCommand.BalanceAnswer Data { get; set; } = new SmsAeroRepository.GetBalanceCommand.AnswerCommand.BalanceAnswer();

        public class BalanceAnswer
        {
          [JsonProperty("balance")]
          public Decimal Balance { get; set; }
        }
      }
    }
  }
}
