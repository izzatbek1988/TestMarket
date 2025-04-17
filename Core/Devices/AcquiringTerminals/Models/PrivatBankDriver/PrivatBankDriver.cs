// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.PrivatBankDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Timers;
using WebSocketSharp;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class PrivatBankDriver
  {
    private readonly LanConnection _lanConnection;
    private WebSocket ws;

    public PrivatBankDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public bool DoCommand(PrivatBankDriver.PrivatBankCommand command)
    {
      try
      {
        this.ws = new WebSocket(string.Format("ws://{0}:{1}/echo", (object) this._lanConnection.UrlAddress, (object) this._lanConnection.PortNumber), Array.Empty<string>())
        {
          WaitTime = new TimeSpan(0, 2, 0)
        };
        this.ws.OnMessage += (EventHandler<MessageEventArgs>) ((sender, e) =>
        {
          LogHelper.Debug("Answer:" + e.Data);
          command.AnswerString = e.Data;
          this.ws.Close(CloseStatusCode.Normal);
        });
        this.ws.OnClose += new EventHandler<CloseEventArgs>(this.Ws_OnClose);
        this.ws.OnError += (EventHandler<ErrorEventArgs>) ((sender, args) => LogHelper.Debug("Error: " + args.Message));
        this.ws.OnOpen += new EventHandler(this.Ws_OnOpen);
        this.ws.Connect();
        string data = JsonConvert.SerializeObject((object) command);
        LogHelper.Debug("command: " + data);
        this.ws.Send(data);
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Interval = 120000.0;
        timer.AutoReset = false;
        timer.Elapsed += (ElapsedEventHandler) ((sender, args) =>
        {
          LogHelper.Debug("Отвалились по таймауту, завершаем");
          this.ws.Close(CloseStatusCode.NoStatus);
          throw new TimeoutException();
        });
        timer.Start();
        while (this.ws.IsAlive)
          Thread.Sleep(1000);
        return !command.Error;
      }
      catch
      {
        LogHelper.Debug("Таймаут для команды на терминал ПриватБанка");
        return false;
      }
    }

    private void Ws_OnOpen(object sender, EventArgs e) => LogHelper.Debug("Open ");

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
      LogHelper.Debug("Closed: " + e.Reason);
    }

    private interface IPrivatBankCommand
    {
      [JsonProperty("method")]
      string Method { get; }
    }

    public abstract class PrivatBankCommand
    {
      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonProperty("step")]
      public int Step => 0;

      [JsonProperty("error")]
      public bool Error { get; set; }

      [JsonProperty("errorDescription")]
      public string ErrorDescription { get; set; }
    }

    public class PurchaseCommand : 
      PrivatBankDriver.PrivatBankCommand,
      PrivatBankDriver.IPrivatBankCommand
    {
      public string Method => "Purchase";

      [JsonIgnore]
      public PrivatBankDriver.PurchaseCommand.PurchaseAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<PrivatBankDriver.PurchaseCommand.PurchaseAnswer>(this.AnswerString);
        }
      }

      [JsonProperty("params")]
      public PrivatBankDriver.PurchaseCommand.ParamsDoPayment Params { get; set; } = new PrivatBankDriver.PurchaseCommand.ParamsDoPayment();

      public class ParamsDoPayment : PrivatBankDriver.SendParams
      {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; } = string.Empty;
      }

      public class PurchaseAnswer : PrivatBankDriver.Answer
      {
        [JsonProperty("params")]
        public PrivatBankDriver.PurchaseCommand.PurchaseAnswer.TerminalDoPaymentAnswer Params { get; set; } = new PrivatBankDriver.PurchaseCommand.PurchaseAnswer.TerminalDoPaymentAnswer();

        public class TerminalDoPaymentAnswer
        {
          [JsonProperty("responseCode")]
          public string ResponseCode { get; set; }

          [JsonProperty("receipt")]
          public string Receipt { get; set; }

          [JsonProperty("rrn")]
          public string Rrn { get; set; }

          [JsonProperty("pan")]
          public string CardNumber { get; set; }

          [JsonProperty("terminalId")]
          public string TerminalId { get; set; }

          [JsonProperty("bankAcquirer")]
          public string BankAcquirer { get; set; }

          [JsonProperty("approvalCode")]
          public string ApprovalCode { get; set; }

          [JsonProperty("paymentSystem")]
          public string PaymentSystem { get; set; }

          [JsonProperty("issuerName")]
          public string IssuerName { get; set; }
        }
      }
    }

    public class RefundCommand : 
      PrivatBankDriver.PrivatBankCommand,
      PrivatBankDriver.IPrivatBankCommand
    {
      public string Method => "Refund";

      [JsonIgnore]
      public PrivatBankDriver.PurchaseCommand.PurchaseAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<PrivatBankDriver.PurchaseCommand.PurchaseAnswer>(this.AnswerString);
        }
      }

      [JsonProperty("params")]
      public PrivatBankDriver.RefundCommand.ParamsRefundPayment Params { get; set; } = new PrivatBankDriver.RefundCommand.ParamsRefundPayment();

      public class ParamsRefundPayment : PrivatBankDriver.SendParams
      {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; } = string.Empty;

        [JsonProperty("rrn")]
        public string Rrn { get; set; }
      }
    }

    public class VerifyCommand : 
      PrivatBankDriver.PrivatBankCommand,
      PrivatBankDriver.IPrivatBankCommand
    {
      [JsonIgnore]
      public PrivatBankDriver.VerifyCommand.VerifyAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<PrivatBankDriver.VerifyCommand.VerifyAnswer>(this.AnswerString);
        }
      }

      public string Method => "Verify";

      [JsonProperty("params")]
      public PrivatBankDriver.SendParams Params { get; set; } = new PrivatBankDriver.SendParams();

      public class VerifyAnswer : PrivatBankDriver.Answer
      {
        [JsonProperty("params")]
        public PrivatBankDriver.VerifyCommand.VerifyAnswer.TerminalCloseShiftAnswer Params { get; set; } = new PrivatBankDriver.VerifyCommand.VerifyAnswer.TerminalCloseShiftAnswer();

        public class TerminalCloseShiftAnswer
        {
          [JsonProperty("responseCode")]
          public string ResponseCode { get; set; }

          [JsonProperty("receipt")]
          public string Receipt { get; set; }
        }
      }
    }

    public class AuditCommand : 
      PrivatBankDriver.PrivatBankCommand,
      PrivatBankDriver.IPrivatBankCommand
    {
      [JsonIgnore]
      public PrivatBankDriver.AuditCommand.VerifyAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<PrivatBankDriver.AuditCommand.VerifyAnswer>(this.AnswerString);
        }
      }

      public string Method => "Audit";

      [JsonProperty("params")]
      public PrivatBankDriver.SendParams Params { get; set; } = new PrivatBankDriver.SendParams();

      public class VerifyAnswer : PrivatBankDriver.Answer
      {
        [JsonProperty("params")]
        public PrivatBankDriver.AuditCommand.VerifyAnswer.TerminalCloseShiftAnswer Params { get; set; } = new PrivatBankDriver.AuditCommand.VerifyAnswer.TerminalCloseShiftAnswer();

        public class TerminalCloseShiftAnswer
        {
          [JsonProperty("responseCode")]
          public string ResponseCode { get; set; }

          [JsonProperty("receipt")]
          public string Receipt { get; set; }
        }
      }
    }

    public class SendParams
    {
      [JsonProperty("merchantId")]
      public string MerchantId { get; set; } = "0";
    }

    public abstract class Answer
    {
      [JsonProperty("error")]
      public bool Error { get; set; }

      [JsonProperty("errorDescription")]
      public string ErrorDescription { get; set; }
    }
  }
}
