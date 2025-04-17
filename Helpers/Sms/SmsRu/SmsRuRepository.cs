// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsRuRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public class SmsRuRepository
  {
    public static string _apiKey { get; set; }

    public SmsRuRepository(string apiKey) => SmsRuRepository._apiKey = apiKey;

    public void DoCommand(SmsRuRepository.Command command)
    {
      string requestUriString = "https://sms.ru/" + command.GetRequestStr;
      LogHelper.Debug("Отправляем запрос на отправку СМС " + requestUriString);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = "GET";
      httpWebRequest.Timeout = 120000;
      Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
      if (responseStream == null)
        throw new InvalidOperationException();
      using (StreamReader streamReader = new StreamReader(responseStream))
      {
        command.AnswerString = streamReader.ReadToEnd();
        LogHelper.Debug("Получен ответ:\n" + command.AnswerString);
      }
    }

    public abstract class Command
    {
      public virtual string GetRequestStr { get; }

      public string AnswerString { get; set; }

      public SmsRuRepository.AllAnswer Result
      {
        get => JsonConvert.DeserializeObject<SmsRuRepository.AllAnswer>(this.AnswerString);
      }
    }

    public class AllAnswer
    {
      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("status_code")]
      public int StatusCode { get; set; }
    }

    public class SendEqualSmsCommand : SmsRuRepository.Command
    {
      public List<string> Phones { get; set; }

      public string SmsText { get; set; }

      public SmsRuRepository.SendEqualSmsCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsRuRepository.SendEqualSmsCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public override string GetRequestStr
      {
        get
        {
          return "sms/send?api_id=" + SmsRuRepository._apiKey + "&to=" + string.Join(",", (IEnumerable<string>) this.Phones) + "&msg=" + this.SmsText.Replace(' ', '+') + "&json=1" + (DevelopersHelper.IsDebug() ? "&test=1" : "") + "partner_id=206786";
        }
      }

      public static SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer GetPropertyValue(
        JToken answer,
        string phone)
      {
        return JsonConvert.DeserializeObject<SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer>((answer != null ? answer.Children().Single<JToken>((Func<JToken, bool>) (x => x.Path == phone))?.First?.ToString() : (string) null) ?? "");
      }

      public class AnswerCommand : SmsRuRepository.AllAnswer
      {
        [JsonProperty("sms")]
        public JToken SmsAnswers { get; set; }

        [JsonProperty("balance")]
        public Decimal Balance { get; set; }

        public class SmsAnswer
        {
          [JsonProperty("status")]
          public string Status { get; set; }

          [JsonProperty("status_code")]
          public int StatusCode { get; set; }

          [JsonProperty("status_text")]
          public string StatusText { get; set; }

          [JsonProperty("sms_id")]
          public string SmsId { get; set; }
        }
      }
    }

    public class SendOtherSmsCommand : SmsRuRepository.Command
    {
      public Dictionary<int, string> Phones { get; set; }

      public override string GetRequestStr
      {
        get
        {
          string str = "sms/send?api_id=" + SmsRuRepository._apiKey;
          foreach (KeyValuePair<int, string> phone in this.Phones)
            str += string.Format("&to[{0}]={1}", (object) phone.Key, (object) phone.Value.Replace(' ', '+'));
          return str + "&json=1" + (DevelopersHelper.IsDebug() ? "&test=1" : "");
        }
      }

      public SmsRuRepository.SendEqualSmsCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsRuRepository.SendEqualSmsCommand.AnswerCommand>(this.AnswerString);
        }
      }
    }

    public class SendPhoneCommand : SmsRuRepository.Command
    {
      [JsonIgnore]
      public string Ip { get; set; }

      public string Phone { get; set; }

      public override string GetRequestStr
      {
        get
        {
          return "code/call?phone=" + this.Phone + "&api_id=" + SmsRuRepository._apiKey + "&json=1" + (this.Ip.IsNullOrEmpty() ? "" : "&ip=" + this.Ip);
        }
      }

      public SmsRuRepository.SendPhoneCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsRuRepository.SendPhoneCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public class AnswerCommand : SmsRuRepository.AllAnswer
      {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("call_id")]
        public string Call_id { get; set; }

        [JsonProperty("cost")]
        public Decimal Cost { get; set; }

        [JsonProperty("balance")]
        public Decimal Balance { get; set; }
      }
    }

    public class GetBalanceCommand : SmsRuRepository.Command
    {
      public string Phone { get; set; }

      public override string GetRequestStr
      {
        get => "my/balance?&api_id=" + SmsRuRepository._apiKey + "&json=1";
      }

      public SmsRuRepository.GetBalanceCommand.AnswerCommand Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmsRuRepository.GetBalanceCommand.AnswerCommand>(this.AnswerString);
        }
      }

      public class AnswerCommand : SmsRuRepository.AllAnswer
      {
        [JsonProperty("balance")]
        public Decimal Balance { get; set; }
      }
    }
  }
}
