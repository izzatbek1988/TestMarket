// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TrueApiHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public class TrueApiHelper
  {
    public static string Key = "25f05d8a-d4d2-40f9-a78e-3a19c97ff2ac";
    public string Url = DevelopersHelper.IsDebug() ? "https://markirovka.sandbox.crptech.ru" : "";

    public void DoCommand(TrueApiHelper.Command command)
    {
      if (this.Url.IsNullOrEmpty())
      {
        LogHelper.WriteToCrptLog("Не удалось получить URL площадки для выполнения запроса", NLog.LogLevel.Info);
        command.StatusCode = HttpStatusCode.RequestTimeout;
        command.AnswerString = new TrueApiHelper.GeneralAnswer().ToJsonString();
      }
      else
      {
        RestHelper restHelper = new RestHelper(this.Url, new int?(), command.ToJsonString(true, true));
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        restHelper.CreateCommand(command.Path, command.Method);
        if (!command.Token.IsNullOrEmpty())
          restHelper.AddHeader("Authorization", "Bearer " + command.Token);
        if (!command.ApiKey.IsNullOrEmpty())
          restHelper.AddHeader("X-API-KEY", command.ApiKey);
        restHelper.SetTimeout(command.Timeout);
        LogHelper.WriteToCrptLog(restHelper.ToString(), NLog.LogLevel.Info);
        restHelper.DoCommand();
        command.AnswerString = restHelper.Answer;
        command.StatusCode = restHelper.StatusCode;
        string answerString = command.AnswerString;
        string str = (answerString != null ? answerString.TryFormatingJsonString() : (string) null) ?? "";
        LogHelper.WriteToCrptLog(string.Format("Ответ от TrueApi (status code {0}): \r\n{1}", (object) command.StatusCode, (object) str), NLog.LogLevel.Info);
      }
    }

    public abstract class Command
    {
      [JsonIgnore]
      public Decimal Timeout { get; set; } = 30M;

      [JsonIgnore]
      public string Token { get; set; }

      [JsonIgnore]
      public string ApiKey { get; set; }

      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual TypeRestRequest Method { get; }

      [JsonIgnore]
      public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

      [JsonIgnore]
      public virtual string Path { get; }
    }

    public class GeneralAnswer
    {
      [JsonProperty("code")]
      public int Code { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }

      [JsonProperty("error_message")]
      public string ErrorMessage { get; set; }
    }

    public class CheckCodeCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Post;

      public override string Path => "/api/v4/true-api/codes/check";

      [JsonProperty("codes")]
      public List<string> Codes { get; set; }

      [JsonIgnore]
      public List<Guid> ListGuid { get; set; }

      [JsonIgnore]
      public TrueApiHelper.CheckCodeCommand.Answer Result
      {
        get
        {
          try
          {
            return JsonConvert.DeserializeObject<TrueApiHelper.CheckCodeCommand.Answer>(this.AnswerString);
          }
          catch
          {
            return new TrueApiHelper.CheckCodeCommand.Answer();
          }
        }
      }

      public class Answer : TrueApiHelper.GeneralAnswer
      {
        [JsonProperty("codes")]
        public List<TrueApiHelper.CheckCodeCommand.Answer.CodeItem> Codes { get; set; }

        [JsonProperty("reqId")]
        public string ReqId { get; set; }

        [JsonProperty("reqTimestamp")]
        public long ReqTimestamp { get; set; }

        public class CodeItem
        {
          [JsonProperty("cis")]
          public string Cis { get; set; }

          [JsonProperty("valid")]
          public bool Valid { get; set; }

          [JsonProperty("printView")]
          public string PrintView { get; set; }

          [JsonProperty("gtin")]
          public string Gtin { get; set; }

          [JsonProperty("groupIds")]
          public List<MarkGroupEnum> GroupIds { get; set; } = new List<MarkGroupEnum>();

          [JsonProperty("verified")]
          public bool Verified { get; set; }

          [JsonProperty("realizable")]
          public bool Realizable { get; set; }

          [JsonProperty("found")]
          public bool Found { get; set; }

          [JsonProperty("utilised")]
          public bool Utilised { get; set; }

          [JsonProperty("expireDate")]
          public DateTime ExpireDate { get; set; }

          [JsonProperty("productionDate")]
          public DateTime ProductionDate { get; set; }

          [JsonProperty("productWeight")]
          public Decimal ProductWeight { get; set; }

          [JsonProperty("prVetDocument")]
          public string PrVetDocument { get; set; }

          [JsonProperty("isOwner")]
          public bool IsOwner { get; set; }

          [JsonProperty("isBlocked")]
          public bool IsBlocked { get; set; }

          [JsonProperty("grayZone")]
          public bool GrayZone { get; set; }

          [JsonProperty("errorCode")]
          public int ErrorCode { get; set; }

          [JsonProperty("message")]
          public string Message { get; set; }

          [JsonProperty("isTracking")]
          public bool IsTracking { get; set; }

          [JsonProperty("sold")]
          public bool Sold { get; set; }

          [JsonProperty("mrp")]
          public Decimal? Mrp { get; set; }

          [JsonProperty("smp")]
          public Decimal? Smp { get; set; }

          [JsonProperty("packageType")]
          public string PackageType { get; set; }

          [JsonProperty("packageQuantity")]
          public Decimal? PackageQuantity { get; set; }
        }
      }
    }

    public class UrlForCheckCodeCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Get;

      public override string Path => "/api/v4/true-api/cdn/info";

      [JsonIgnore]
      public TrueApiHelper.UrlForCheckCodeCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<TrueApiHelper.UrlForCheckCodeCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : TrueApiHelper.GeneralAnswer
      {
        [JsonProperty("hosts")]
        public List<TrueApiHelper.UrlForCheckCodeCommand.Answer.HostItem> Hosts { get; set; }

        public class HostItem
        {
          [JsonProperty("host")]
          public string Host { get; set; }
        }
      }
    }

    public class GetInfoUrlCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Get;

      public override string Path => "/api/v4/true-api/cdn/health/check";

      [JsonIgnore]
      public TrueApiHelper.GetInfoUrlCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<TrueApiHelper.GetInfoUrlCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : TrueApiHelper.GeneralAnswer
      {
        [JsonProperty("avgTimeMs")]
        public int AvgTimeMs { get; set; }

        [JsonProperty("error_message")]
        public new string ErrorMessage { get; set; }
      }
    }

    public class AuthKeyCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Get;

      public override string Path => "/api/v3/true-api/auth/key";

      [JsonIgnore]
      public TrueApiHelper.AuthKeyCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<TrueApiHelper.AuthKeyCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("uuid")]
        public string Uid { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
      }
    }

    public class SimpleSignInCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Post;

      public override string Path => "/api/v3/true-api/auth/simpleSignIn";

      [JsonProperty("uuid")]
      public string Uid { get; set; }

      [JsonProperty("data")]
      public string Data { get; set; }

      [JsonIgnore]
      public TrueApiHelper.SimpleSignInCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<TrueApiHelper.SimpleSignInCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : TrueApiHelper.GeneralAnswer
      {
        [JsonProperty("error_message")]
        public new string ErrorMessage { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
      }
    }

    public class CreateDocumentCommand : TrueApiHelper.Command
    {
      public override TypeRestRequest Method => TypeRestRequest.Post;

      public override string Path => "/api/v3/true-api/lk/documents/create?pg=" + this.GoodGroup;

      [JsonIgnore]
      public string GoodGroup { get; set; }

      [JsonProperty("document_format")]
      public string DocumentFormat => "MANUAL";

      [JsonProperty("product_document")]
      public string ProductDocument { get; set; }

      [JsonProperty("signature")]
      public string Signature { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonIgnore]
      public string ResultStr => this.AnswerString;

      [JsonIgnore]
      public TrueApiHelper.CreateDocumentCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<TrueApiHelper.CreateDocumentCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer : TrueApiHelper.GeneralAnswer
      {
        [JsonProperty("error_message")]
        public new string ErrorMessage { get; set; }
      }
    }

    public class ConnectTapDocument
    {
      [JsonIgnore]
      public string Type => "CONNECT_TAP";

      [JsonProperty("participantInn")]
      public string ParticipantInn { get; set; }

      [JsonProperty("participantKpp")]
      public string ParticipantKpp { get; set; }

      [JsonProperty("fiasId")]
      public string FiasId { get; set; }

      [JsonProperty("codes")]
      public List<TrueApiHelper.ConnectTapDocument.CodeBeer> Codes { get; set; } = new List<TrueApiHelper.ConnectTapDocument.CodeBeer>();

      public class CodeBeer
      {
        [JsonProperty("cis")]
        public string Cis { get; set; }

        [JsonProperty("connectDate")]
        public string ConnectDate { get; set; }
      }
    }
  }
}
