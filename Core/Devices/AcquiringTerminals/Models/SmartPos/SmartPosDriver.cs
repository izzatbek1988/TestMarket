// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.SmartPosDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class SmartPosDriver
  {
    private readonly LanConnection _lanConnection;

    public SmartPosDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public void DoCommand(SmartPosDriver.SmartPosCommand command)
    {
      string str = this._lanConnection.UrlAddress;
      if (!str.ToLower().StartsWith("http://"))
        str = "http://" + str;
      string requestUriString = string.Format("{0}:{1}{2}", (object) str, (object) this._lanConnection.PortNumber, (object) command.Method);
      LogHelper.Debug("Выполняем команду SmartPos " + requestUriString);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = "GET";
      httpWebRequest.Timeout = 120000;
      httpWebRequest.ReadWriteTimeout = 120000;
      httpWebRequest.KeepAlive = false;
      try
      {
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
        {
          command.AnswerString = streamReader.ReadToEnd();
          LogHelper.Debug(command.AnswerString);
        }
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        string end = new StreamReader(responseStream)?.ReadToEnd();
        LogHelper.Error((Exception) ex, end);
        throw new Exception(end);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошбика в момент обмена с терминалом SmartPos");
        throw;
      }
    }

    public abstract class SmartPosCommand
    {
      public string AnswerString { get; set; }

      public virtual string Method { get; }
    }

    public class PaymentCommand : SmartPosDriver.SmartPosCommand
    {
      public SmartPosDriver.PaymentCommand.PaymentAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartPosDriver.PaymentCommand.PaymentAnswer>(this.AnswerString);
        }
      }

      public override string Method
      {
        get => string.Format("/payment?amount={0}&owncheque=false", (object) this.Amount);
      }

      public int Amount { get; set; }

      public class PaymentAnswer
      {
        [JsonProperty("processId")]
        public string ProcessId { get; set; }

        [JsonProperty("status")]
        public SmartPosDriver.Status Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
      }
    }

    public class RefundPaymentCommand : SmartPosDriver.SmartPosCommand
    {
      public SmartPosDriver.RefundPaymentCommand.RefundPaymentAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartPosDriver.RefundPaymentCommand.RefundPaymentAnswer>(this.AnswerString);
        }
      }

      public override string Method
      {
        get
        {
          return string.Format("/refund?method={0}&amount={1}&transactionId={2}&owncheque=true", (object) this.MethodPayment, (object) this.Amount, (object) this.TransactionId);
        }
      }

      public int Amount { get; set; }

      public string TransactionId { get; set; }

      public string MethodPayment { get; set; } = "card";

      public class RefundPaymentAnswer
      {
        [JsonProperty("processId")]
        public string ProcessId { get; set; }

        [JsonProperty("status")]
        public SmartPosDriver.Status Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
      }
    }

    public class StatusCommand : SmartPosDriver.SmartPosCommand
    {
      public SmartPosDriver.StatusCommand.StatusAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartPosDriver.StatusCommand.StatusAnswer>(this.AnswerString);
        }
      }

      public override string Method => "/status?processId=" + this.ProcessId;

      public string ProcessId { get; set; }

      public class StatusAnswer
      {
        [JsonProperty("processId")]
        public string ProcessId { get; set; }

        [JsonProperty("status")]
        public SmartPosDriver.Status Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("chequeInfo")]
        public SmartPosDriver.StatusCommand.ChequeInfo ChequeInfo { get; set; } = new SmartPosDriver.StatusCommand.ChequeInfo();
      }

      public class ChequeInfo
      {
        [JsonProperty("method")]
        public string Method { get; set; }
      }
    }

    public enum Status
    {
      wait,
      success,
      fail,
    }
  }
}
