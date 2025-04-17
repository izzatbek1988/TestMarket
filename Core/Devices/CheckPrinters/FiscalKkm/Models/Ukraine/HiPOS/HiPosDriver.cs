// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.HiPosDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class HiPosDriver
  {
    private Gbs.Core.Config.Devices _config;

    [Localizable(false)]
    public HiPosDriver(Gbs.Core.Config.Devices devices) => this._config = devices;

    public void DoCommand(HiPosDriver.HiPosCommand command)
    {
      LogHelper.Debug("Начинаю выполнять комманду HiPos: " + command.Method);
      string jsonString = command.ToJsonString();
      if (command.GetType().IsEither<Type>(typeof (HiPosDriver.CashInCommand), typeof (HiPosDriver.CashOutCommand), typeof (HiPosDriver.GetXCommand)))
        jsonString = Convert.ToString(command.Body, (IFormatProvider) CultureInfo.InvariantCulture);
      RestHelper restHelper = new RestHelper(this._config.CheckPrinter.Connection.LanPort.UrlAddress, this._config.CheckPrinter.Connection.LanPort.PortNumber, jsonString);
      restHelper.CreateCommand("/api/" + command.Method, command.HttpMethod);
      if (!command.SessionKey.IsNullOrEmpty())
        restHelper.AddHeader("Authorization", "Bearer " + command.SessionKey);
      restHelper.DoCommand();
      command.AnswerString = restHelper.StatusCode == HttpStatusCode.OK ? restHelper.Answer : throw new ErrorHelper.GbsException(restHelper.Answer)
      {
        Direction = ErrorHelper.ErrorDirections.Outer
      };
    }

    public abstract class HiPosCommand
    {
      [JsonIgnore]
      public object Body { get; set; }

      [JsonIgnore]
      public virtual TypeRestRequest HttpMethod { get; } = TypeRestRequest.Post;

      [JsonIgnore]
      public string SessionKey { get; set; }

      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual string Method { get; }
    }

    public class AuthLoginCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Auth/Login";

      [JsonProperty("userName")]
      public string UserName { get; set; }

      [JsonProperty("password")]
      public string Password { get; set; }

      [JsonProperty("checkOutId")]
      public int CheckOutId { get; set; } = 1;

      [JsonProperty("forceLogin")]
      public bool ForceLogin { get; set; } = true;

      [JsonIgnore]
      public HiPosDriver.AuthLoginCommand.AuthorizationAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<HiPosDriver.AuthLoginCommand.AuthorizationAnswer>(this.AnswerString);
        }
      }

      public class AuthorizationAnswer
      {
        [JsonProperty("tokenType")]
        public string TokenType { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("issued")]
        public DateTime Issued { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
      }
    }

    public class OutLoginCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Auth/Logout";
    }

    public class SetOnlineCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "PRRO/Online";
    }

    public class SetOfflineCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "PRRO/Offline";
    }

    public class CashInCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Cash/CashIn";
    }

    public class CashOutCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Cash/CashOut";
    }

    public class OpenShiftCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Shift/Open";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;
    }

    public class CloseSiftCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Shift/Close";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;
    }

    public class GetXCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "SalesReport/GetXReport";
    }

    public class GetTxtXCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "SalesReport/GetTxtXReport";

      [JsonProperty("userId")]
      public int? UserId { get; set; }

      [JsonProperty("paperWidth")]
      public int PaperWidth { get; set; }
    }

    public class GetTxtZCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "SalesReport/GetTxtZReport";

      [JsonProperty("shiftId")]
      public int ShiftId { get; set; }

      [JsonProperty("paperWidth")]
      public int PaperWidth { get; set; }
    }

    public class GetInfoCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "PRRO/GetInfo";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;

      [JsonIgnore]
      public HiPosDriver.GetInfoCommand.InfoAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<HiPosDriver.GetInfoCommand.InfoAnswer>(this.AnswerString);
        }
      }

      public class InfoAnswer
      {
        [JsonProperty("prroFiscalNumber")]
        public double PrroFiscalNumber { get; set; }

        [JsonProperty("lastReceiptFiscalNumber")]
        public string LastReceiptFiscalNumber { get; set; }

        [JsonProperty("currentTime")]
        public string CurrentTime { get; set; }

        [JsonProperty("mode")]
        public int Mode { get; set; }

        [JsonProperty("shiftOpenedAt")]
        public string ShiftOpenedAt { get; set; }

        [JsonProperty("cash")]
        public Decimal? Cash { get; set; }
      }
    }

    public class GetStateShiftCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "PRRO/GetState";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;

      [JsonIgnore]
      public HiPosDriver.GetStateShiftCommand.StateAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<HiPosDriver.GetStateShiftCommand.StateAnswer>(this.AnswerString);
        }
      }

      public class StateAnswer
      {
        [JsonProperty("isZReportNeeded")]
        public bool IsZReportNeeded { get; set; }

        [JsonProperty("is36LockedEnabled")]
        public bool Is36LockedEnabled { get; set; }

        [JsonProperty("is168LockedEnabled")]
        public bool Is168LockedEnabled { get; set; }

        [JsonProperty("isOffline")]
        public bool IsOffline { get; set; }

        [JsonProperty("is2000OfflineReceipt")]
        public bool Is2000OfflineReceipt { get; set; }
      }
    }

    public class GetCertificateStateCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "CertificateState/Get";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;

      [JsonIgnore]
      public HiPosDriver.GetCertificateStateCommand.StateAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<HiPosDriver.GetCertificateStateCommand.StateAnswer>(this.AnswerString);
        }
      }

      public class StateAnswer
      {
        [JsonProperty("validTo")]
        public DateTime? ValidTo { get; set; }
      }
    }

    public class CreateCheckCommand : HiPosDriver.HiPosCommand
    {
      [JsonIgnore]
      public bool IsReturn { get; set; }

      public override string Method => "Receipt/" + (this.IsReturn ? "Refund" : "Create");

      [JsonProperty("recipientEmailAddress")]
      public string RecipientEmailAddress { get; set; }

      [JsonProperty("isNeedToSendReceiptFile")]
      public bool IsNeedToSendReceiptFile { get; set; }

      [JsonProperty("totalAmount")]
      public Decimal TotalAmount { get; set; }

      [JsonProperty("noRoundAmount")]
      public Decimal NoRoundAmount { get; set; }

      [JsonProperty("roundSum")]
      public Decimal RoundSum { get; set; }

      [JsonProperty("totalDiscount")]
      public Decimal TotalDiscount { get; set; }

      [JsonProperty("preFiscalLinesComments")]
      public object PreFiscalLinesComments { get; set; }

      [JsonProperty("afterFiscalLinesComments")]
      public object AfterFiscalLinesComments { get; set; }

      [JsonProperty("fiscalNumber")]
      public string FiscalNumber { get; set; }

      [JsonProperty("products")]
      public List<HiPosDriver.CreateCheckCommand.Item> Products { get; set; } = new List<HiPosDriver.CreateCheckCommand.Item>();

      [JsonProperty("paymentInfos")]
      public List<HiPosDriver.CreateCheckCommand.Payment> PaymentInfos { get; set; } = new List<HiPosDriver.CreateCheckCommand.Payment>();

      public class Item
      {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("totalPrice")]
        public Decimal TotalPrice { get; set; }

        [JsonProperty("fullPrice")]
        public Decimal FullPrice { get; set; }

        [JsonProperty("discount")]
        public Decimal Discount { get; set; }

        [JsonProperty("quantity")]
        public Decimal Quantity { get; set; }

        [JsonProperty("taxGroup")]
        public string TaxGroup { get; set; }

        [JsonProperty("uktzed")]
        public string Uktzed { get; set; }

        [JsonProperty("exciseLabels")]
        public List<string> ExciseLabels { get; set; }
      }

      public class Payment
      {
        [JsonProperty("paymentType")]
        public int PaymentType { get; set; }

        [JsonProperty("paymentFormName")]
        public string PaymentFormName { get; set; }

        [JsonProperty("paid")]
        public Decimal Paid { get; set; }

        [JsonProperty("totalPay")]
        public Decimal TotalPay { get; set; }

        [JsonProperty("payOut")]
        public Decimal PayOut { get; set; }

        [JsonProperty("bankId")]
        public string BankId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("paymentName")]
        public string PaymentName { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("paymentSystem")]
        public string PaymentSystem { get; set; }

        [JsonProperty("authorizationId")]
        public string AuthorizationId { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("posTransactionDate")]
        public DateTime? PosTransactionDate { get; set; }

        [JsonProperty("posTransactionNumber")]
        public string PosTransactionNumber { get; set; }

        [JsonProperty("commission")]
        public double? Commission { get; set; }
      }

      public class Round
      {
      }
    }

    public class GetCheckCommand : HiPosDriver.HiPosCommand
    {
      public override string Method => "Receipt/Get";

      [JsonProperty("prroFiscalNumber")]
      public double PrroFiscalNumber { get; set; }

      [JsonProperty("receiptFiscalNumber")]
      public string ReceiptFiscalNumber { get; set; }

      [JsonIgnore]
      public HiPosDriver.GetCheckCommand.CheckAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<HiPosDriver.GetCheckCommand.CheckAnswer>(this.AnswerString);
        }
      }

      public class Item
      {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("uktzed")]
        public string Uktzed { get; set; }

        [JsonProperty("dkpp")]
        public object Dkpp { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("unitName")]
        public object UnitName { get; set; }

        [JsonProperty("amount")]
        public Decimal Amount { get; set; }

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("discount")]
        public Decimal Discount { get; set; }

        [JsonProperty("cost")]
        public Decimal Cost { get; set; }

        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("taxPrc")]
        public Decimal? TaxPrc { get; set; }

        [JsonProperty("exciseCode")]
        public object ExciseCode { get; set; }

        [JsonProperty("excisePrc")]
        public object ExcisePrc { get; set; }

        [JsonProperty("exciseLables")]
        public List<string> ExciseLables { get; set; }

        [JsonIgnore]
        public string ExciseLable
        {
          get
          {
            List<string> exciseLables = this.ExciseLables;
            return (exciseLables != null ? (exciseLables.Any<string>() ? 1 : 0) : 0) == 0 ? "" : this.ExciseLables[0];
          }
        }

        [JsonProperty("letters")]
        public string Letters { get; set; }
      }

      public class Excise
      {
        [JsonProperty("exciseCode")]
        public string ExciseCode { get; set; }

        [JsonProperty("excisePrc")]
        public Decimal ExcisePrc { get; set; }

        [JsonProperty("exciseSum")]
        public Decimal ExciseSum { get; set; }

        [JsonProperty("turnOver")]
        public Decimal TurnOver { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("letter")]
        public string Letter { get; set; }

        [JsonProperty("sign")]
        public bool Sign { get; set; }
      }

      public class Pay
      {
        [JsonProperty("typeOfPayment")]
        public int TypeOfPayment { get; set; }

        [JsonProperty("payFormNm")]
        public string PayFormNm { get; set; }

        [JsonProperty("payFormCd")]
        public Decimal PayFormCd { get; set; }

        [JsonProperty("sum")]
        public Decimal Sum { get; set; }

        [JsonProperty("provided")]
        public Decimal Provided { get; set; }

        [JsonProperty("remains")]
        public Decimal Remains { get; set; }

        [JsonProperty("paymentSystem")]
        public string PaymentSystem { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("bankId")]
        public string BankId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("paymentName")]
        public string PaymentName { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("authorizationId")]
        public string AuthorizationId { get; set; }

        [JsonProperty("posTransactionDate")]
        public DateTime? PosTransactionDate { get; set; }

        [JsonProperty("posTransactionNumber")]
        public string PosTransactionNumber { get; set; }

        [JsonProperty("commission")]
        public Decimal? Commission { get; set; }
      }

      public class CheckAnswer
      {
        [JsonProperty("shiftId")]
        public int? ShiftId { get; set; }

        [JsonProperty("ver")]
        public int? Ver { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("ipn")]
        public string Ipn { get; set; }

        [JsonProperty("orgName")]
        public string OrgName { get; set; }

        [JsonProperty("pointName")]
        public string PointName { get; set; }

        [JsonProperty("pointAddr")]
        public string PointAddr { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("orderRetNum")]
        public object OrderRetNum { get; set; }

        [JsonProperty("sent")]
        public bool Sent { get; set; }

        [JsonProperty("canceled")]
        public bool Canceled { get; set; }

        [JsonProperty("offline")]
        public bool Offline { get; set; }

        [JsonProperty("oflsid")]
        public int? Oflsid { get; set; }

        [JsonProperty("orderNum")]
        public double? OrderNum { get; set; }

        [JsonProperty("orderTaxNum")]
        public string OrderTaxNum { get; set; }

        [JsonProperty("cashDeskNum")]
        public double? CashDeskNum { get; set; }

        [JsonProperty("cashRegisterNum")]
        public double? CashRegisterNum { get; set; }

        [JsonProperty("revokeLastOnlineDoc")]
        public bool RevokeLastOnlineDoc { get; set; }

        [JsonProperty("cashier")]
        public string Cashier { get; set; }

        [JsonProperty("totalSum")]
        public Decimal? TotalSum { get; set; }

        [JsonProperty("roundSum")]
        public Decimal? RoundSum { get; set; }

        [JsonProperty("noRoundSum")]
        public Decimal? NoRoundSum { get; set; }

        [JsonProperty("discount")]
        public Decimal Discount { get; set; }

        [JsonProperty("headerFiscalLines")]
        public object HeaderFiscalLines { get; set; }

        [JsonProperty("footerFiscalLines")]
        public object FooterFiscalLines { get; set; }

        [JsonProperty("qrLink")]
        public string QrLink { get; set; }

        [JsonProperty("docType")]
        public int DocType { get; set; }

        [JsonProperty("pays")]
        public List<HiPosDriver.GetCheckCommand.Pay> Pays { get; set; }

        [JsonProperty("taxes")]
        public List<HiPosDriver.GetCheckCommand.Taxis> Taxes { get; set; }

        [JsonProperty("excises")]
        public List<HiPosDriver.GetCheckCommand.Excise> Excises { get; set; }

        [JsonProperty("bodies")]
        public List<HiPosDriver.GetCheckCommand.Item> Items { get; set; }
      }

      public class Taxis
      {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("taxPrc")]
        public Decimal TaxPrc { get; set; }

        [JsonProperty("taxSum")]
        public Decimal TaxSum { get; set; }

        [JsonProperty("turnOver")]
        public Decimal TurnOver { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("letter")]
        public string Letter { get; set; }

        [JsonProperty("sign")]
        public bool Sign { get; set; }
      }
    }
  }
}
