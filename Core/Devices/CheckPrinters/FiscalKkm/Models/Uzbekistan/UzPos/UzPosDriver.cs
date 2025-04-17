// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.UzPosDriver
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
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class UzPosDriver
  {
    private readonly Gbs.Core.Config.Devices _config;

    public UzPosDriver(Gbs.Core.Config.Devices devices) => this._config = devices;

    public void DoCommand(UzPosDriver.UzPosCommand command)
    {
      try
      {
        string str1 = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore
        });
        LogHelper.Debug("Начинаю выполнять команду: UzPos" + Environment.NewLine + str1);
        string str2 = this._config.CheckPrinter.Connection.LanPort.UrlAddress;
        if (str2.ToLower().StartsWith("http://"))
          str2 = str2.Replace("http://", "");
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(string.Format("http://{0}:{1}/uzpos", (object) str2, (object) this._config.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault()));
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 120000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str1);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException("Response stream is empty");
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.Answer = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ ПРРО UZPOS:" + command.Answer);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при выполнении команды UzPos: " + ex.Message);
        throw;
      }
    }

    private interface IUzPosCommand
    {
      [JsonProperty("method")]
      string Method { get; }
    }

    public abstract class UzPosCommand
    {
      [JsonProperty("port")]
      public int Port => 3448;

      [JsonProperty("token")]
      public string Token => "DXJFX32CN1296678504F2";

      [JsonIgnore]
      public string Answer { get; set; }
    }

    public class UzPosAnswer
    {
      [JsonProperty("error")]
      public bool Error { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
    }

    public class KkmOpenSession : UzPosDriver.UzPosCommand, UzPosDriver.IUzPosCommand
    {
      [JsonIgnore]
      public UzPosDriver.UzPosAnswer Data
      {
        get => JsonConvert.DeserializeObject<UzPosDriver.UzPosAnswer>(this.Answer);
      }

      public string Method => "openZreport";
    }

    public class KkmCloseSession : UzPosDriver.UzPosCommand, UzPosDriver.IUzPosCommand
    {
      [JsonIgnore]
      public UzPosDriver.UzPosAnswer Data
      {
        get => JsonConvert.DeserializeObject<UzPosDriver.UzPosAnswer>(this.Answer);
      }

      public string Method => "closeZreport";
    }

    public class KkmCheckStatus : UzPosDriver.UzPosCommand, UzPosDriver.IUzPosCommand
    {
      [JsonIgnore]
      public UzPosDriver.UzPosAnswer Data
      {
        get => JsonConvert.DeserializeObject<UzPosDriver.UzPosAnswer>(this.Answer);
      }

      public string Method => "checkStatus";
    }

    public class GetZReportInfo : UzPosDriver.UzPosCommand, UzPosDriver.IUzPosCommand
    {
      [JsonIgnore]
      public UzPosDriver.GetZReportInfo.GetZReportInfoAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<UzPosDriver.GetZReportInfo.GetZReportInfoAnswer>(this.Answer);
        }
      }

      public string Method => "getZreportInfo";

      [JsonProperty("zReportId")]
      public int ZReportId { get; set; }

      public class GetZReportInfoAnswer
      {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("message")]
        public object Info { get; set; }

        public class MessageInfo
        {
          [JsonProperty("paycheck")]
          public string Paycheck { get; set; }

          [JsonProperty("number")]
          public int Number { get; set; }

          [JsonProperty("count")]
          public int Count { get; set; }

          [JsonProperty("totalRefundCount")]
          public int TotalRefundCount { get; set; }

          [JsonProperty("firstReceiptSeq")]
          public string FirstReceiptSeq { get; set; }

          [JsonProperty("lastReceiptSeq")]
          public string LastReceiptSeq { get; set; }

          [JsonProperty("totalSaleCount")]
          public int TotalSaleCount { get; set; }

          [JsonProperty("totalRefundCash")]
          public Decimal TotalRefundCash { get; set; }

          [JsonProperty("totalRefundCard")]
          public Decimal TotalRefundCard { get; set; }

          [JsonProperty("totalRefundVAT")]
          public Decimal TotalRefundVat { get; set; }

          [JsonProperty("openTime")]
          public string OpenTime { get; set; }

          [JsonProperty("terminalID")]
          public string TerminalId { get; set; }

          [JsonProperty("totalSaleCard")]
          public Decimal TotalSaleCard { get; set; }

          [JsonProperty("closeTime")]
          public string CloseTime { get; set; }

          [JsonProperty("appletVersion")]
          public string AppletVersion { get; set; }

          [JsonProperty("totalSaleCash")]
          public Decimal TotalSaleCash { get; set; }

          [JsonProperty("totalSaleVAT")]
          public Decimal TotalSaleVat { get; set; }
        }
      }
    }

    public class OpenCheck : UzPosDriver.UzPosCommand, UzPosDriver.IUzPosCommand
    {
      [JsonIgnore]
      public UzPosDriver.OpenCheck.OpenCheckAnswer Data
      {
        get => JsonConvert.DeserializeObject<UzPosDriver.OpenCheck.OpenCheckAnswer>(this.Answer);
      }

      public string Method { get; set; } = "sale";

      [JsonProperty("companyName")]
      public string CompanyName { get; set; }

      [JsonProperty("companyAddress")]
      public string CompanyAddress { get; set; }

      [JsonProperty("companyINN")]
      public string CompanyInn { get; set; }

      [JsonProperty("staffName")]
      public string UserName { get; set; }

      [JsonProperty("printerSize")]
      public int PrinterSize { get; set; } = 58;

      [JsonProperty("phoneNumber")]
      public string PhoneNumber { get; set; }

      [JsonProperty("companyPhoneNumber")]
      public string CompanyPhoneNumber { get; set; }

      [JsonProperty("params")]
      public UzPosDriver.OpenCheck.Params Parameters { get; set; }

      [JsonProperty("refundInfo")]
      public UzPosDriver.OpenCheck.OpenCheckAnswer.Info RefundInfo { get; set; }

      public class Params
      {
        [JsonProperty("paycheckNumber")]
        public string PaycheckNumber { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("items")]
        public List<UzPosDriver.OpenCheck.Item> Items { get; set; } = new List<UzPosDriver.OpenCheck.Item>();

        [JsonProperty("receivedCash")]
        public int ReceivedCash { get; set; }

        [JsonProperty("receivedCard")]
        public int ReceivedCard { get; set; }
      }

      public class Item
      {
        [JsonProperty("discount")]
        public int Discount { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("vatPercent")]
        public int VatPercent { get; set; }

        [JsonProperty("vat")]
        public int Vat { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("classCode")]
        public string ClassCode { get; set; }

        [JsonProperty("packageCode")]
        public string PackageCode { get; set; }

        [JsonProperty("other")]
        public int Other { get; set; }

        [JsonProperty("ownerType")]
        public int OwnerType { get; set; }
      }

      public class OpenCheckAnswer : UzPosDriver.UzPosAnswer
      {
        [JsonProperty("paycheck")]
        public string Paycheck { get; set; }

        [JsonProperty("info")]
        public UzPosDriver.OpenCheck.OpenCheckAnswer.Info CheckInfo { get; set; }

        [JsonProperty("qrPath")]
        public string QrPath { get; set; }

        [JsonProperty("virtualNumber")]
        public string VirtualNumber { get; set; }

        public class Info
        {
          [JsonProperty("terminalId")]
          public string TerminalId { get; set; }

          [JsonProperty("receiptSeq")]
          public string ReceiptSeq { get; set; }

          [JsonProperty("fiscalSign")]
          public string FiscalSign { get; set; }

          [JsonProperty("qrCodeURL")]
          public string QrCode { get; set; }

          [JsonProperty("dateTime")]
          public string DateTime { get; set; }
        }
      }
    }
  }
}
