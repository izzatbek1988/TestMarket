// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsAgentRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace Gbs.Helpers
{
  public class SmsAgentRepository
  {
    private readonly Integrations _integrations;

    private static string Url => "https://api3.sms-agent.ru";

    public SmsAgentRepository(Integrations integrations) => this._integrations = integrations;

    public void DoCommand(SmsAgentRepository.Command command)
    {
      string jsonString = command.ToJsonString(true, true);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(SmsAgentRepository.Url + command.UrlCommand);
      httpWebRequest.ContentType = "application/json; charset=utf-8";
      httpWebRequest.Method = "POST";
      httpWebRequest.Timeout = 60000;
      httpWebRequest.ReadWriteTimeout = 120000;
      httpWebRequest.KeepAlive = false;
      string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(Translate.AtolWebRequestsDriver_GetAuthorization__0___1_, (object) this._integrations.Sms.Login.DecryptedValue, (object) this._integrations.Sms.Password.DecryptedValue)));
      httpWebRequest.Headers.Add("Authorization", "Basic " + base64String);
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        streamWriter.Write(jsonString);
      Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
      if (responseStream == null)
        throw new InvalidOperationException();
      using (StreamReader streamReader = new StreamReader(responseStream))
        command.AnswerString = streamReader.ReadToEnd();
      LogHelper.Debug("Ответ СМС-Агент:\r\n" + command.AnswerString);
    }

    public abstract class Command
    {
      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual string UrlCommand { get; }
    }

    public class Answer
    {
      [JsonProperty("error")]
      public string Error { get; set; }
    }

    public class SendSmsCommand : SmsAgentRepository.Command
    {
      public override string UrlCommand => "/v2.0/json/send/";

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("sender")]
      public string Sender { get; set; }

      [JsonProperty("text")]
      public string Text { get; set; }

      [JsonProperty("payload")]
      public List<SmsAgentRepository.SendSmsCommand.Payloads> PayloadsList { get; set; } = new List<SmsAgentRepository.SendSmsCommand.Payloads>();

      [JsonIgnore]
      public List<SmsAgentRepository.SendSmsCommand.AnswerForPhone> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<List<SmsAgentRepository.SendSmsCommand.AnswerForPhone>>(this.AnswerString);
        }
      }

      [JsonIgnore]
      public SmsAgentRepository.Answer ResultGlobalError
      {
        get
        {
          try
          {
            return JsonConvert.DeserializeObject<SmsAgentRepository.Answer>(this.AnswerString);
          }
          catch
          {
            return (SmsAgentRepository.Answer) null;
          }
        }
      }

      public class Payloads
      {
        [JsonProperty("phone")]
        public string Phone { get; set; }
      }

      public class AnswerForPhone
      {
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("id_sms")]
        public int IdSms { get; set; }

        [JsonProperty("count_sms")]
        public int CountSms { get; set; }

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
      }
    }

    public class GetBalanceCommand : SmsAgentRepository.Command
    {
      [JsonProperty("login")]
      public string Login { get; set; }

      [JsonProperty("pass")]
      public string Password { get; set; }

      public override string UrlCommand => "/v2.0/json/balance";

      [JsonIgnore]
      public SmsAgentRepository.GetBalanceCommand.BalanceAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsAgentRepository.GetBalanceCommand.BalanceAnswer>(this.AnswerString);
        }
      }

      public class BalanceAnswer : SmsAgentRepository.Answer
      {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("balance")]
        public Decimal Balance { get; set; }
      }
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum TypeCommand
    {
      [JsonProperty("sms")] Sms,
      [JsonProperty("app")] SmsAgentApp,
      [JsonProperty("flashcall")] FlashCall,
      [JsonProperty("callpassword")] CallPassword,
      [JsonProperty("voice_lo")] VoiceLo,
      [JsonProperty("voice_hi")] VoiceHi,
      [JsonProperty("wa")] WhatsApp,
      [JsonProperty("viberer")] Viber,
    }
  }
}
