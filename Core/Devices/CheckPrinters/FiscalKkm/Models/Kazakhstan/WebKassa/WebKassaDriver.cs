// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan.WebKassaDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan
{
  public class WebKassaDriver
  {
    public void SendCommand(WebKassaDriver.WebKassaCommand command)
    {
      string url = (DevelopersHelper.IsDebug() ? "https://devkkm.webkassa.kz/" : "https://kkm.webkassa.kz/") + "api/";
      string str = JsonConvert.SerializeObject((object) command, Formatting.None, new JsonSerializerSettings());
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.DefaultConnectionLimit = 9999;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      int? port = new int?();
      string body = str;
      RestHelper restHelper = new RestHelper(url, port, body);
      restHelper.CreateCommand(command.Method, TypeRestRequest.Post);
      restHelper.AddHeader("X-API-KEY", DevelopersHelper.IsDebug() ? "WKD-3B8A0B00-6A91-4439-83C0-5C2484244238" : "WK-DF7ABAB6-408E-4911-BDE5-82C817BFF298");
      restHelper.DoCommand();
      command.AnswerString = restHelper.Answer;
      this.CheckError(command);
    }

    private void CheckError(WebKassaDriver.WebKassaCommand command)
    {
      WebKassaDriver.WebKassaAnswer webKassaAnswer = JsonConvert.DeserializeObject<WebKassaDriver.WebKassaAnswer>(command.AnswerString);
      if (webKassaAnswer.Errors != null && webKassaAnswer.Errors.Any<WebKassaDriver.ErrorWebKassa>())
      {
        WebKassa.OldAuthToken.token = string.Empty;
        string message = string.Join('\n'.ToString(), webKassaAnswer.Errors.Select<WebKassaDriver.ErrorWebKassa, string>((Func<WebKassaDriver.ErrorWebKassa, string>) (x => string.Format("{0} (код: {1})", (object) x.Text, (object) x.Code))));
        LogHelper.Debug("Error WebKasaa " + message);
        throw new DeviceException(message);
      }
    }

    public class WebKassaAnswer
    {
      public List<WebKassaDriver.ErrorWebKassa> Errors { get; set; }
    }

    public abstract class WebKassaCommand
    {
      public string Token { get; set; }

      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual string Method { get; }
    }

    public class ErrorWebKassa
    {
      public int Code { get; set; }

      public string Text { get; set; }
    }

    public class AuthorizationCommand : WebKassaDriver.WebKassaCommand
    {
      [JsonIgnore]
      public WebKassaDriver.AuthorizationCommand.AuthorizationAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<WebKassaDriver.AuthorizationCommand.AuthorizationAnswer>(this.AnswerString);
        }
      }

      public override string Method => "Authorize";

      public string Login { get; set; }

      public string Password { get; set; }

      public class AuthorizationAnswer : WebKassaDriver.WebKassaAnswer
      {
        public WebKassaDriver.AuthorizationCommand.Data Data { get; set; }
      }

      public class Data
      {
        public string Token { get; set; }
      }
    }

    public class ZReportCommand : WebKassaDriver.WebKassaCommand
    {
      [JsonIgnore]
      public WebKassaDriver.ZReportCommand.ZReportAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<WebKassaDriver.ZReportCommand.ZReportAnswer>(this.AnswerString);
        }
      }

      public override string Method => "ZReport";

      public string CashboxUniqueNumber { get; set; }

      public class ZReportAnswer : WebKassaDriver.WebKassaAnswer
      {
        public WebKassaDriver.ZReportCommand.Data Data { get; set; }
      }

      public class Data
      {
        public int ReportNumber { get; set; }

        public string TaxPayerName { get; set; }

        public string TaxPayerIN { get; set; }

        public bool TaxPayerVAT { get; set; }

        public string TaxPayerVATSeria { get; set; }

        public string TaxPayerVATNumber { get; set; }

        public string CashboxSN { get; set; }

        public string CashboxIN { get; set; }

        public string CashboxRN { get; set; }

        public string StartOn { get; set; }

        public string ReportOn { get; set; }

        public string CloseOn { get; set; }

        public int CashierCode { get; set; }

        public int ShiftNumber { get; set; }

        public int DocumentCount { get; set; }

        public Decimal PutMoneySum { get; set; }

        public Decimal TakeMoneySum { get; set; }

        public string ControlSum { get; set; }

        public bool OfflineMode { get; set; }

        public bool CashboxOfflineMode { get; set; }

        public Decimal SumInCashbox { get; set; }

        public WebKassaDriver.ZReportCommand.OperationTypeSummary Sell { get; set; }

        public WebKassaDriver.ZReportCommand.OperationTypeSummary Buy { get; set; }

        public WebKassaDriver.ZReportCommand.OperationTypeSummary ReturnSell { get; set; }

        public WebKassaDriver.ZReportCommand.OperationTypeSummary ReturnBuy { get; set; }

        public WebKassaDriver.ZReportCommand.NonNullableApiModel EndNonNullable { get; set; }

        public WebKassaDriver.ZReportCommand.NonNullableApiModel StartNonNullable { get; set; }

        public WebKassaDriver.OfdInformation Ofd { get; set; }
      }

      public class OperationTypeSummary
      {
        public List<WebKassaDriver.ZReportCommand.OperationTypeSummary.PaymentsByTypeApiModel> PaymentsByTypesApiModel { get; set; }

        public Decimal Discount { get; set; }

        public Decimal Markup { get; set; }

        public Decimal Taken { get; set; }

        public Decimal Change { get; set; }

        public Decimal VAT { get; set; }

        public int Count { get; set; }

        public class PaymentsByTypeApiModel
        {
          public Decimal Sum { get; set; }

          public int Type { get; set; }
        }
      }

      public class NonNullableApiModel
      {
        public Decimal Sell { get; set; }

        public Decimal Buy { get; set; }

        public Decimal ReturnSell { get; set; }

        public Decimal ReturnBuy { get; set; }
      }
    }

    public class XReportCommand : WebKassaDriver.WebKassaCommand
    {
      [JsonIgnore]
      public WebKassaDriver.ZReportCommand.ZReportAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<WebKassaDriver.ZReportCommand.ZReportAnswer>(this.AnswerString);
        }
      }

      public override string Method => "XReport";

      public string CashboxUniqueNumber { get; set; }
    }

    public class CheckCommand : WebKassaDriver.WebKassaCommand
    {
      [JsonIgnore]
      public WebKassaDriver.CheckCommand.CheckAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<WebKassaDriver.CheckCommand.CheckAnswer>(this.AnswerString);
        }
      }

      public override string Method => "Check";

      public string CashboxUniqueNumber { get; set; }

      public int OperationType { get; set; }

      public List<WebKassaDriver.CheckCommand.Position> Positions { get; set; } = new List<WebKassaDriver.CheckCommand.Position>();

      public List<WebKassaDriver.CheckCommand.TicketModifier> TicketModifiers { get; set; } = new List<WebKassaDriver.CheckCommand.TicketModifier>();

      public List<WebKassaDriver.CheckCommand.Payment> Payments { get; set; } = new List<WebKassaDriver.CheckCommand.Payment>();

      public Decimal Change { get; set; }

      public int RoundType { get; set; } = 2;

      public string ExternalCheckNumber { get; set; }

      public string CustomerEmail { get; set; }

      public string CustomerXin { get; set; }

      public string CustomerPhone { get; set; }

      public class Position
      {
        public Decimal Count { get; set; }

        public Decimal Price { get; set; }

        public int? TaxPercent { get; set; }

        public Decimal Tax { get; set; }

        public long TaxType { get; set; }

        public string PositionName { get; set; }

        public string PositionCode { get; set; }

        public Decimal Discount { get; set; }

        public Decimal Markup { get; set; }

        public string SectionCode { get; set; }

        public bool IsStorno { get; set; }

        public bool MarkupDeleted { get; set; }

        public bool DiscountDeleted { get; set; }

        public int UnitCode { get; set; }

        public string Mark { get; set; }
      }

      public class TicketModifier
      {
        public Decimal Sum { get; set; }

        public string Text { get; set; }

        public long Type { get; set; }

        public Decimal Tax { get; set; }

        public long TaxType { get; set; }
      }

      public class Payment
      {
        public Decimal Sum { get; set; }

        public long PaymentType { get; set; }
      }

      public class CheckAnswer : WebKassaDriver.WebKassaAnswer
      {
        public WebKassaDriver.CheckCommand.Data Data { get; set; }
      }

      public class Data
      {
        public string CheckNumber { get; set; }

        public string DateTime { get; set; }

        public bool OfflineMode { get; set; }

        public bool CashboxOfflineMode { get; set; }

        public WebKassaDriver.CashboxParameters Cashbox { get; set; }

        public int CheckOrderNumber { get; set; }

        public int ShiftNumber { get; set; }

        public string EmployeeName { get; set; }

        public string TicketUrl { get; set; }

        public string TicketPrintUrl { get; set; }
      }
    }

    public class MoneyOperationCommand : WebKassaDriver.WebKassaCommand
    {
      [JsonIgnore]
      public WebKassaDriver.MoneyOperationCommand.MoneyOperationAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<WebKassaDriver.MoneyOperationCommand.MoneyOperationAnswer>(this.AnswerString);
        }
      }

      public override string Method => "MoneyOperation";

      public string CashboxUniqueNumber { get; set; }

      public long OperationType { get; set; }

      public long Sum { get; set; }

      public string ExternalCheckNumber { get; set; }

      public class MoneyOperationAnswer : WebKassaDriver.WebKassaAnswer
      {
        public WebKassaDriver.MoneyOperationCommand.Data Data { get; set; }
      }

      public class Data
      {
        public string DateTime { get; set; }

        public bool OfflineMode { get; set; }

        public bool CashboxOfflineMode { get; set; }

        public Decimal Sum { get; set; }

        public WebKassaDriver.CashboxParameters Cashbox { get; set; }
      }
    }

    public class CashboxParameters
    {
      public string UniqueNumber { get; set; }

      public string RegistrationNumber { get; set; }

      public string IdentityNumber { get; set; }

      public string Address { get; set; }

      public WebKassaDriver.OfdInformation Ofd { get; set; }
    }

    public class OfdInformation
    {
      public string Name { get; set; }

      public string Host { get; set; }

      public int Code { get; set; }
    }
  }
}
