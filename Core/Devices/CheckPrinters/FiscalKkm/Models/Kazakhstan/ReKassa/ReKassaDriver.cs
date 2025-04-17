// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan.ReKassaDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

#nullable enable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan
{
  public class ReKassaDriver
  {
    public void SendCommand(
    #nullable disable
    ReKassaDriver.ReKassaCommand command)
    {
      string url = (DevelopersHelper.IsDebug() ? "https://app-test.rekassa.kz" : "https://app.rekassa.kz") + "/partner/api/";
      string str = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore
      });
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.DefaultConnectionLimit = 9999;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      int? port = new int?();
      string body = str;
      RestHelper restHelper = new RestHelper(url, port, body);
      restHelper.CreateCommand(command.Method, command.HttpMethod);
      if (!command.Token.IsNullOrEmpty())
        restHelper.AddHeader("Authorization", "Bearer " + command.Token);
      if (!command.XRequestId.IsNullOrEmpty())
        restHelper.AddHeader("X-Request-ID", command.XRequestId);
      if (!command.СashRegisterPassword.IsNullOrEmpty())
        restHelper.AddHeader("cash-register-password", command.СashRegisterPassword);
      restHelper.DoCommand();
      command.AnswerString = restHelper.Answer;
    }

    public abstract class ReKassaCommand
    {
      [JsonIgnore]
      public virtual TypeRestRequest HttpMethod => TypeRestRequest.Get;

      [JsonIgnore]
      public string Token { get; set; }

      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual string Method { get; }

      [JsonIgnore]
      public string XRequestId { get; set; }

      [JsonIgnore]
      public string СashRegisterPassword { get; set; }
    }

    public class ErrorReKassa
    {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }

      [JsonProperty("meta")]
      public string Meta { get; set; }

      [JsonProperty("url")]
      public string Url { get; set; }
    }

    public class AuthorizationCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      private readonly string _apiKey = DevelopersHelper.IsDebug() ? "c488d6ba-f2cf-45d7-bf38-1cc40d50a59c" : "21B72AC9-F4AA-4A87-A594-6DDF63209378";

      public override TypeRestRequest HttpMethod => TypeRestRequest.Post;

      [JsonIgnore]
      public ReKassaDriver.AuthorizationCommand.AuthorizationAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.AuthorizationCommand.AuthorizationAnswer>(this.AnswerString);
        }
      }

      [JsonProperty("number")]
      public string Number { get; set; }

      [JsonProperty("password")]
      public string Password { get; set; }

      public override string Method => "auth/login?apiKey=" + this._apiKey + "&format=json";

      public class AuthorizationAnswer
      {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
      }
    }

    public class TicketCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      public ReKassaDriver.TicketCommand.TicketAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.TicketCommand.TicketAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest HttpMethod => TypeRestRequest.Post;

      public override string Method => string.Format("crs/{0}/tickets", (object) this.KassaId);

      [JsonIgnore]
      public int KassaId { get; set; }

      [JsonProperty("operation")]
      public string Operation { get; set; }

      [JsonProperty("dateTime")]
      public ReKassaDriver.TicketCommand.DateTimeClass DateTime { get; set; }

      [JsonProperty("domain")]
      public ReKassaDriver.TicketCommand.DomainClass Domain { get; set; }

      [JsonProperty("items")]
      public List<ReKassaDriver.TicketCommand.Item> Items { get; set; } = new List<ReKassaDriver.TicketCommand.Item>();

      [JsonProperty("payments")]
      public List<ReKassaDriver.TicketCommand.Payment> Payments { get; set; } = new List<ReKassaDriver.TicketCommand.Payment>();

      [JsonProperty("amounts")]
      public ReKassaDriver.TicketCommand.AmountsClass Amounts { get; set; }

      public class AmountsClass
      {
        [JsonProperty("total")]
        public ReKassaDriver.SumFormat Total { get; set; }

        [JsonProperty("taken")]
        public ReKassaDriver.SumFormat Taken { get; set; }

        [JsonProperty("change")]
        public ReKassaDriver.SumFormat Сhange { get; set; }

        [JsonProperty("discount")]
        public ReKassaDriver.TicketCommand.DiscountItem Discount { get; set; }
      }

      public class DateTimeClass
      {
        [JsonProperty("date")]
        public ReKassaDriver.TicketCommand.Date Date { get; set; }

        [JsonProperty("time")]
        public ReKassaDriver.TicketCommand.Time Time { get; set; }

        public DateTimeClass(DateTime dateTime)
        {
          this.Date = new ReKassaDriver.TicketCommand.Date()
          {
            Day = (long) dateTime.Day,
            Month = (long) dateTime.Month,
            Year = (long) dateTime.Year
          };
          this.Time = new ReKassaDriver.TicketCommand.Time()
          {
            Hour = (long) dateTime.Hour,
            Minute = (long) dateTime.Minute,
            Second = (long) dateTime.Second
          };
        }
      }

      public class Date
      {
        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("month")]
        public long Month { get; set; }

        [JsonProperty("day")]
        public long Day { get; set; }
      }

      public class Time
      {
        [JsonProperty("hour")]
        public long Hour { get; set; }

        [JsonProperty("minute")]
        public long Minute { get; set; }

        [JsonProperty("second")]
        public long Second { get; set; }
      }

      public class DomainClass
      {
        [JsonProperty("type")]
        public string Type { get; set; }
      }

      public class Item
      {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("commodity")]
        public ReKassaDriver.TicketCommand.Commodity Commodity { get; set; }

        [JsonProperty("discount")]
        public ReKassaDriver.TicketCommand.DiscountItem Discount { get; set; }
      }

      public class DiscountItem
      {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sum")]
        public ReKassaDriver.SumFormat Sum { get; set; }

        [JsonProperty("auxiliary")]
        public List<ReKassaDriver.TicketCommand.Auxiliary> Auxiliary { get; set; }
      }

      public class Commodity
      {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sectionCode")]
        public long SectionCode { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("exciseStamp")]
        public string ExciseStamp { get; set; }

        [JsonProperty("price")]
        public ReKassaDriver.SumFormat Price { get; set; }

        [JsonProperty("sum")]
        public ReKassaDriver.SumFormat Sum { get; set; }

        [JsonProperty("taxes")]
        public List<ReKassaDriver.TicketCommand.Tax> Taxes { get; set; } = new List<ReKassaDriver.TicketCommand.Tax>();

        [JsonProperty("auxiliary")]
        public List<ReKassaDriver.TicketCommand.Auxiliary> Auxiliary { get; set; } = new List<ReKassaDriver.TicketCommand.Auxiliary>();
      }

      public class Auxiliary
      {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
      }

      public class Tax
      {
        [JsonProperty("taxType")]
        public ReKassaDriver.TicketCommand.Tax.TaxTypeEnum Type { get; set; }

        [JsonProperty("taxationType")]
        public ReKassaDriver.TicketCommand.Tax.TaxationTypeEnum TaxationType { get; set; }

        [JsonProperty("percent")]
        public long Percent { get; set; }

        [JsonProperty("sum")]
        public ReKassaDriver.SumFormat Sum { get; set; }

        [JsonProperty("isInTotalSum")]
        public bool IsInTotalSum { get; set; } = true;

        public enum TaxTypeEnum
        {
          VAT = 100, // 0x00000064
        }

        public enum TaxationTypeEnum
        {
          STS = 100, // 0x00000064
          RTS = 101, // 0x00000065
          TRFF = 102, // 0x00000066
          TRBP = 103, // 0x00000067
        }
      }

      public class Payment
      {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sum")]
        public ReKassaDriver.SumFormat Sum { get; set; }
      }

      public class TicketAnswer
      {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("ticketNumber")]
        public object TicketNumber { get; set; }

        [JsonProperty("qrCode")]
        public string QrCode { get; set; }

        [JsonProperty("offlineTicketNumber")]
        public long? OfflineTicketNumber { get; set; }

        [JsonProperty("fdo")]
        public ReKassaDriver.TicketCommand.TicketAnswer.FdoClass Fdo { get; set; }

        [JsonProperty("shiftNumber")]
        public long ShiftNumber { get; set; }

        [JsonProperty("shiftDocumentNumber")]
        public long ShiftDocumentNumber { get; set; }

        [JsonProperty("shiftMessageNumber")]
        public long ShiftMessageNumber { get; set; }

        [JsonProperty("messageTime")]
        public DateTimeOffset MessageTime { get; set; }

        [JsonProperty("requestTime")]
        public object RequestTime { get; set; }

        [JsonProperty("data")]
        public ReKassaDriver.TicketCommand.TicketAnswer.DataClass Data { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cancelable")]
        public bool Cancelable { get; set; }

        [JsonProperty("cancelledBy")]
        public object CancelledBy { get; set; }

        [JsonProperty("operator")]
        public object Operator { get; set; }

        [JsonProperty("errorType")]
        public object ErrorType { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        [JsonProperty("offline")]
        public bool Offline { get; set; }

        [JsonProperty("_links")]
        public ReKassaDriver.TicketCommand.TicketAnswer.LinksClass Links { get; set; }

        public class DataClass
        {
          [JsonProperty("ticket")]
          public ReKassaDriver.TicketCommand Ticket { get; set; }

          [JsonProperty("command")]
          public string Command { get; set; }
        }

        public class OperatorClass
        {
          [JsonProperty("code")]
          public long Code { get; set; }
        }

        public class FdoClass
        {
          [JsonProperty("code")]
          public string Code { get; set; }

          [JsonProperty("nameKk")]
          public string NameKk { get; set; }

          [JsonProperty("nameRu")]
          public string NameRu { get; set; }

          [JsonProperty("url")]
          public string Url { get; set; }
        }

        public class LinksClass
        {
          [JsonProperty("self")]
          public ReKassaDriver.TicketCommand.TicketAnswer.Cancel Self { get; set; }

          [JsonProperty("cancel")]
          public ReKassaDriver.TicketCommand.TicketAnswer.Cancel Cancel { get; set; }
        }

        public class Cancel
        {
          [JsonProperty("href")]
          public Uri Href { get; set; }
        }
      }
    }

    public class CashRegisterCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.CashRegisterCommand.CashRegisterAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;

      public override string Method => "crs";

      public class CashRegisterAnswer
      {
        [JsonProperty("_embedded")]
        public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.EmbeddedClass Embedded { get; set; }

        public class EmbeddedClass
        {
          [JsonProperty("userCashRegisterRoles")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.UserCashRegisterRole[] UserCashRegisterRoles { get; set; }
        }

        public class UserCashRegisterRole
        {
          [JsonProperty("cashRegister")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.CashRegister CashRegister { get; set; }

          [JsonProperty("organization")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Organization Organization { get; set; }

          [JsonProperty("roles")]
          public string[] Roles { get; set; }
        }

        public class CashRegister
        {
          [JsonProperty("id")]
          public long Id { get; set; }

          [JsonProperty("name")]
          public string Name { get; set; }

          [JsonProperty("model")]
          public string Model { get; set; }

          [JsonProperty("serialNumber")]
          public string SerialNumber { get; set; }

          [JsonProperty("year")]
          public long Year { get; set; }

          [JsonProperty("status")]
          public string Status { get; set; }

          [JsonProperty("createDate")]
          public DateTimeOffset CreateDate { get; set; }

          [JsonProperty("registrationDate")]
          public DateTimeOffset RegistrationDate { get; set; }

          [JsonProperty("registrationNumber")]
          public string RegistrationNumber { get; set; }

          [JsonProperty("fdo")]
          public string Fdo { get; set; }

          [JsonProperty("fdoId")]
          public long FdoId { get; set; }

          [JsonProperty("fdoMode")]
          public string FdoMode { get; set; }

          [JsonProperty("shiftNumber")]
          public long ShiftNumber { get; set; }

          [JsonProperty("shiftOpen")]
          public bool ShiftOpen { get; set; }

          [JsonProperty("shiftOpenTime")]
          public DateTime? ShiftOpenTime { get; set; }

          [JsonProperty("shiftCloseTime")]
          public DateTime? ShiftCloseTime { get; set; }

          [JsonProperty("shiftExpireTime")]
          public DateTime? ShiftExpireTime { get; set; }

          [JsonProperty("shiftExpired")]
          public bool ShiftExpired { get; set; }

          [JsonProperty("shiftDocumentNumber")]
          public long ShiftDocumentNumber { get; set; }

          [JsonProperty("shiftMessageNumber")]
          public long ShiftMessageNumber { get; set; }

          [JsonProperty("data")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Data Data { get; set; }

          [JsonProperty("pos")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Pos Pos { get; set; }

          [JsonProperty("offlineStartTime")]
          public DateTime? OfflineStartTime { get; set; }

          [JsonProperty("offlineExpireTime")]
          public DateTime? OfflineExpireTime { get; set; }

          [JsonProperty("blockStartTime")]
          public DateTime? BlockStartTime { get; set; }

          [JsonProperty("shift")]
          public object Shift { get; set; }

          [JsonProperty("deregistrationDate")]
          public object DeregistrationDate { get; set; }

          [JsonProperty("_links")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Links Links { get; set; }
        }

        public class Data
        {
          [JsonProperty("service")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Service Service { get; set; }

          [JsonProperty("preferences")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Preferences Preferences { get; set; }

          [JsonProperty("configuration")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Configuration Configuration { get; set; }
        }

        public class Configuration
        {
          [JsonProperty("name")]
          public string Name { get; set; }

          [JsonProperty("taxMode")]
          public bool TaxMode { get; set; }

          [JsonProperty("timezone")]
          public string Timezone { get; set; }

          [JsonProperty("taxationType")]
          public string TaxationType { get; set; }
        }

        public class Preferences
        {
          [JsonProperty("buyMode")]
          public bool BuyMode { get; set; }

          [JsonProperty("domainType")]
          public string DomainType { get; set; }

          [JsonProperty("simpleMode")]
          public bool SimpleMode { get; set; }

          [JsonProperty("paymentTypes")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.PaymentTypes PaymentTypes { get; set; }

          [JsonProperty("defaultTaxType")]
          public string DefaultTaxType { get; set; }

          [JsonProperty("defaultItemName")]
          public string DefaultItemName { get; set; }

          [JsonProperty("defaultItemType")]
          public string DefaultItemType { get; set; }

          [JsonProperty("operationTotalLimit")]
          public long OperationTotalLimit { get; set; }

          [JsonProperty("businessNameOnReceipt")]
          public string BusinessNameOnReceipt { get; set; }
        }

        public class PaymentTypes
        {
          [JsonProperty("card")]
          public bool Card { get; set; }

          [JsonProperty("tare")]
          public bool Tare { get; set; }

          [JsonProperty("credit")]
          public bool Credit { get; set; }
        }

        public class Service
        {
          [JsonProperty("regInfo")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.RegInfo RegInfo { get; set; }

          [JsonProperty("ticketAds")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.TicketAd[] TicketAds { get; set; }
        }

        public class RegInfo
        {
          [JsonProperty("kkm")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Kkm Kkm { get; set; }

          [JsonProperty("org")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Org Org { get; set; }

          [JsonProperty("pos")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Pos Pos { get; set; }
        }

        public class Kkm
        {
          [JsonProperty("fnsKkmId")]
          public string FnsKkmId { get; set; }

          [JsonProperty("serialNumber")]
          public string SerialNumber { get; set; }
        }

        public class Org
        {
          [JsonProperty("inn")]
          public string Inn { get; set; }

          [JsonProperty("title")]
          public string Title { get; set; }

          [JsonProperty("address")]
          public string Address { get; set; }
        }

        public class Pos
        {
          [JsonProperty("title")]
          public string Title { get; set; }

          [JsonProperty("address")]
          public string Address { get; set; }
        }

        public class TicketAd
        {
          [JsonProperty("info")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Info Info { get; set; }

          [JsonProperty("text")]
          public string Text { get; set; }
        }

        public class Info
        {
          [JsonProperty("type")]
          public string Type { get; set; }

          [JsonProperty("version")]
          public long Version { get; set; }
        }

        public class Links
        {
          [JsonProperty("self")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash Self { get; set; }

          [JsonProperty("self-with-roles")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash SelfWithRoles { get; set; }

          [JsonProperty("shifts")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash Shifts { get; set; }

          [JsonProperty("report-fiscal")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash ReportFiscal { get; set; }

          [JsonProperty("tickets")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash Tickets { get; set; }

          [JsonProperty("cash")]
          public ReKassaDriver.CashRegisterCommand.CashRegisterAnswer.Cash Cash { get; set; }
        }

        public class Cash
        {
          [JsonProperty("href")]
          public Uri Href { get; set; }
        }

        public class Organization
        {
          [JsonProperty("id")]
          public long Id { get; set; }

          [JsonProperty("businessId")]
          public string BusinessId { get; set; }

          [JsonProperty("businessName")]
          public string BusinessName { get; set; }

          [JsonProperty("status")]
          public string Status { get; set; }
        }
      }
    }

    public class XReportCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      public int KassaId { get; set; }

      [JsonIgnore]
      public int SessionNumber { get; set; }

      public override TypeRestRequest HttpMethod => TypeRestRequest.Get;

      [JsonIgnore]
      public ReKassaDriver.XReportCommand.XReportAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.XReportCommand.XReportAnswer>(this.AnswerString);
        }
      }

      public override string Method
      {
        get
        {
          return string.Format("crs/{0}/shifts/{1}/reports/x", (object) this.KassaId, (object) this.SessionNumber);
        }
      }

      public class XReportAnswer
      {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("shiftNumber")]
        public long ShiftNumber { get; set; }

        [JsonProperty("messageTime")]
        public DateTimeOffset MessageTime { get; set; }

        [JsonProperty("requestTime")]
        public DateTime? RequestTime { get; set; }

        [JsonProperty("shiftMessageNumber")]
        public long ShiftMessageNumber { get; set; }

        [JsonProperty("data")]
        public 
        #nullable enable
        ReKassaDriver.XReportCommand.XReportAnswer.DataReport? Data { get; set; }

        [JsonProperty("operator")]
        public 
        #nullable disable
        object Operator { get; set; }

        [JsonProperty("errorType")]
        public object ErrorType { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("offline")]
        public bool Offline { get; set; }

        public class DataReport
        {
          [JsonProperty("dateTime")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Time DateTime { get; set; }

          [JsonProperty("shiftNumber")]
          public long ShiftNumber { get; set; }

          [JsonProperty("sections")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Section[] Sections { get; set; }

          [JsonProperty("operations")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Discount[] Operations { get; set; }

          [JsonProperty("discounts")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Discount[] Discounts { get; set; }

          [JsonProperty("markups")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Discount[] Markups { get; set; }

          [JsonProperty("totalResult")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Discount[] TotalResult { get; set; }

          [JsonProperty("startShiftNonNullableSums")]
          public ReKassaDriver.XReportCommand.XReportAnswer.NonNullableSum[] StartShiftNonNullableSums { get; set; }

          [JsonProperty("ticketOperations")]
          public ReKassaDriver.XReportCommand.XReportAnswer.TicketOperation[] TicketOperations { get; set; }

          [JsonProperty("moneyPlacements")]
          public ReKassaDriver.XReportCommand.XReportAnswer.MoneyPlacement[] MoneyPlacements { get; set; }

          [JsonProperty("cashSum")]
          public ReKassaDriver.SumFormat CashSum { get; set; }

          [JsonProperty("revenue")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Revenue Revenue { get; set; }

          [JsonProperty("nonNullableSums")]
          public ReKassaDriver.XReportCommand.XReportAnswer.NonNullableSum[] NonNullableSums { get; set; }

          [JsonProperty("openShiftTime")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Time OpenShiftTime { get; set; }

          [JsonProperty("checksum")]
          public string Checksum { get; set; }
        }

        public class Time
        {
          [JsonProperty("date")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Date Date { get; set; }

          [JsonProperty("time")]
          public ReKassaDriver.XReportCommand.XReportAnswer.TimeClass TimeTime { get; set; }
        }

        public class Date
        {
          [JsonProperty("year")]
          public long Year { get; set; }

          [JsonProperty("month")]
          public long Month { get; set; }

          [JsonProperty("day")]
          public long Day { get; set; }
        }

        public class TimeClass
        {
          [JsonProperty("hour")]
          public long Hour { get; set; }

          [JsonProperty("minute")]
          public long Minute { get; set; }

          [JsonProperty("second")]
          public long Second { get; set; }
        }

        public class Discount
        {
          [JsonProperty("operation")]
          public string Operation { get; set; }

          [JsonProperty("count")]
          public long Count { get; set; }

          [JsonProperty("sum")]
          public ReKassaDriver.SumFormat Sum { get; set; }
        }

        public class MoneyPlacement
        {
          [JsonProperty("operation")]
          public string Operation { get; set; }

          [JsonProperty("operationsTotalCount")]
          public long OperationsTotalCount { get; set; }

          [JsonProperty("operationsCount")]
          public long OperationsCount { get; set; }

          [JsonProperty("operationsSum")]
          public ReKassaDriver.SumFormat OperationsSum { get; set; }

          [JsonProperty("offlineCount")]
          public long OfflineCount { get; set; }
        }

        public class NonNullableSum
        {
          [JsonProperty("operation")]
          public string Operation { get; set; }

          [JsonProperty("sum")]
          public ReKassaDriver.SumFormat Sum { get; set; }
        }

        public class Revenue
        {
          [JsonProperty("sum")]
          public ReKassaDriver.SumFormat Sum { get; set; }

          [JsonProperty("isNegative")]
          public bool IsNegative { get; set; }
        }

        public class Section
        {
          [JsonProperty("sectionCode")]
          public long SectionCode { get; set; }

          [JsonProperty("operations")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Discount[] Operations { get; set; }
        }

        public class TicketOperation
        {
          [JsonProperty("operation")]
          public string Operation { get; set; }

          [JsonProperty("ticketsTotalCount")]
          public long TicketsTotalCount { get; set; }

          [JsonProperty("ticketsCount")]
          public long TicketsCount { get; set; }

          [JsonProperty("ticketsSum")]
          public ReKassaDriver.SumFormat TicketsSum { get; set; }

          [JsonProperty("payments")]
          public ReKassaDriver.XReportCommand.XReportAnswer.Payment[] Payments { get; set; }

          [JsonProperty("offlineCount")]
          public long OfflineCount { get; set; }

          [JsonProperty("discountSum")]
          public ReKassaDriver.SumFormat DiscountSum { get; set; }

          [JsonProperty("markupSum")]
          public ReKassaDriver.SumFormat MarkupSum { get; set; }

          [JsonProperty("changeSum")]
          public ReKassaDriver.SumFormat ChangeSum { get; set; }
        }

        public class Payment
        {
          [JsonProperty("payment")]
          public string PaymentPayment { get; set; }

          [JsonProperty("sum")]
          public ReKassaDriver.SumFormat Sum { get; set; }

          [JsonProperty("count")]
          public long Count { get; set; }
        }
      }
    }

    public class ZReportCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      public int KassaId { get; set; }

      [JsonIgnore]
      public int SessionNumber { get; set; }

      public override TypeRestRequest HttpMethod => TypeRestRequest.Post;

      [JsonIgnore]
      public ReKassaDriver.XReportCommand.XReportAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.XReportCommand.XReportAnswer>(this.AnswerString);
        }
      }

      public override string Method
      {
        get
        {
          return string.Format("crs/{0}/shifts/{1}/close", (object) this.KassaId, (object) this.SessionNumber);
        }
      }
    }

    public class CashCommand : ReKassaDriver.ReKassaCommand
    {
      [JsonIgnore]
      public int KassaId { get; set; }

      public override TypeRestRequest HttpMethod => TypeRestRequest.Post;

      [JsonIgnore]
      public ReKassaDriver.CashCommand.AuthorizationAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<ReKassaDriver.CashCommand.AuthorizationAnswer>(this.AnswerString);
        }
      }

      [JsonProperty("datetime")]
      public ReKassaDriver.TicketCommand.DateTimeClass DateTime { get; set; }

      [JsonProperty("operation")]
      public string Operation { get; set; }

      [JsonProperty("sum")]
      public ReKassaDriver.SumFormat Sum { get; set; }

      public override string Method => string.Format("crs/{0}/cash", (object) this.KassaId);

      public class AuthorizationAnswer
      {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
      }
    }

    public class SumFormat
    {
      [JsonIgnore]
      public Decimal Sum => (Decimal) this.Bills + (Decimal) this.Coins / 100M;

      [JsonProperty("bills")]
      public long Bills { get; set; }

      [JsonProperty("coins")]
      public long Coins { get; set; }

      public SumFormat(Decimal sum)
      {
        this.Bills = (long) (int) sum;
        this.Coins = (long) ((sum - (Decimal) this.Bills) * 100M);
      }
    }
  }
}
