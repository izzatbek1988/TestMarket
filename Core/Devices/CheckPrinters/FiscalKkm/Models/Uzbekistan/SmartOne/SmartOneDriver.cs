// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.SmartOneDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class SmartOneDriver
  {
    private LanConnection _lanConfig;

    public string MerchantId { get; set; }

    public SmartOneDriver(LanConnection config) => this._lanConfig = config;

    public void DoCommand(SmartOneDriver.SmartOneCommand command)
    {
      try
      {
        LogHelper.Debug("Начинаю выполнять комманду SmartOne:\n" + command.ToJsonString(true));
        string str1 = this._lanConfig.UrlAddress + ":" + this._lanConfig.PortNumber.GetValueOrDefault(8008).ToString();
        if (str1.IsNullOrEmpty())
          throw new DeviceException(Translate.SmartOneDriver_DoCommand_Не_указаны_параметры_подключения_к_ККТ__проверьте_настройки_оборудования_);
        if (str1.ToLower().StartsWith("https://"))
          str1 = str1.ToLower().Replace("https://", "");
        if (!str1.ToLower().StartsWith("http://"))
          str1 = "http://" + str1;
        if (str1.ToLower().EndsWith("/"))
          str1 = str1.Remove(str1.Length - 1);
        string requestUriString = str1 + command.Method;
        string base64String1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(command.ToJsonString(isIgnoreNull: true)));
        string base64String2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(CryptoHelper.GetSHA1Hash(base64String1 + this.MerchantId)));
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.ContentType = "text/plain;";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 60000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        string str2 = "data=" + base64String1.Replace("=", "%3D") + "&sign=" + base64String2.Replace("=", "%3D");
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str2);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.AnswerString = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ ККМ SmartOne:\r\n" + command.AnswerString);
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        string end = new StreamReader(responseStream)?.ReadToEnd();
        LogHelper.Error((Exception) ex, end, false);
        command.AnswerString = end;
      }
    }

    [Localizable(false)]
    public class SmartOneCommand
    {
      [JsonIgnore]
      public string AnswerString { get; set; }

      [JsonIgnore]
      public virtual string Method { get; }
    }

    public class SmartOneAnswer
    {
      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("code")]
      public int Code { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
    }

    public class DocumentCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public CheckTypes CheckType { get; set; }

      [JsonIgnore]
      public SmartOneDriver.DocumentCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.DocumentCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => this.CheckType != CheckTypes.Sale ? "/refund" : "/sale";

      [JsonIgnore]
      public DateTime DateTime { get; set; }

      [JsonProperty("docTime")]
      public string DocTime => this.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

      [JsonProperty("docNumber")]
      public string DocNumber { get; set; }

      [JsonProperty("wsName")]
      public string WsName { get; set; }

      [JsonProperty("departmentName")]
      public string DepartmentName { get; set; }

      [JsonProperty("departmentCode")]
      public string DepartmentCode { get; set; }

      [JsonProperty("employeeName")]
      public string EmployeeName { get; set; }

      [JsonProperty("amount")]
      public int Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency => "UZS";

      [JsonProperty("fiscalID")]
      public string FiscalId { get; set; }

      [JsonProperty("printFooter")]
      public string PrintFooter { get; set; }

      [JsonProperty("creditContract")]
      public string CreditContract { get; set; }

      [JsonProperty("prepayDocID")]
      public string PrepayDocId { get; set; }

      [JsonProperty("prepayDocNum")]
      public string PrepayDocNum { get; set; }

      [JsonProperty("clientPhone")]
      public string ClientPhone { get; set; }

      [JsonProperty("clientName")]
      public string ClientName { get; set; }

      [JsonProperty("items")]
      public List<SmartOneDriver.DocumentItem> Items { get; set; } = new List<SmartOneDriver.DocumentItem>();

      [JsonProperty("payments")]
      public SmartOneDriver.DocumentPayments Payments { get; set; } = new SmartOneDriver.DocumentPayments();

      [JsonProperty("parentDocID")]
      public string ParentDocId { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
        [JsonProperty("printError")]
        public string PrintError { get; set; }

        [JsonProperty("documentID")]
        public string DocumentId { get; set; }

        [JsonProperty("fiscalID")]
        public string FiscalId { get; set; }

        [JsonProperty("fiscalNum")]
        public string FiscalNum { get; set; }

        [JsonProperty("rrn")]
        public string Rrn { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        [JsonProperty("cardNum")]
        public string CardNum { get; set; }

        [JsonProperty("checkNum")]
        public string CheckNum { get; set; }
      }
    }

    public class CreditPayCommand : SmartOneDriver.DocumentCommand
    {
      public override string Method => "/creditpay";

      [JsonProperty("documentID")]
      public string DocumentId { get; set; }
    }

    public class GetInfoCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.GetInfoCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.GetInfoCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/get_info";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class OpenShiftCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.OpenShiftCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.OpenShiftCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/open_shift";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      [JsonProperty("pincode")]
      public string Password { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class CloseShiftCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.CloseShiftCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.CloseShiftCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/close_shift";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class CheckShiftCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.CheckShiftCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.CheckShiftCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/check_shift";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
        [JsonProperty("isShiftOpen")]
        public string IsShiftOpen { get; set; }
      }
    }

    public class DepositCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.DepositCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.DepositCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/deposit";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      [JsonProperty("amount")]
      public int Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class WithdrawCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.WithdrawCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.WithdrawCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/withdraw";

      [JsonProperty("employeeName")]
      public string UserName { get; set; }

      [JsonProperty("amount")]
      public int Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class XReportCommand : SmartOneDriver.SmartOneCommand
    {
      [JsonIgnore]
      public SmartOneDriver.XReportCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<SmartOneDriver.XReportCommand.Answer>(this.AnswerString);
        }
      }

      public override string Method => "/x_report";

      public class Answer : SmartOneDriver.SmartOneAnswer
      {
      }
    }

    public class DocumentItem
    {
      [JsonProperty("itemId")]
      public string ItemId { get; set; }

      [JsonProperty("itemName")]
      public string ItemName { get; set; }

      [JsonProperty("itemUnit")]
      public string ItemUnit { get; set; }

      [JsonProperty("itemUnitCode")]
      public string ItemUnitCode { get; set; }

      [JsonProperty("itemBarcode")]
      public string ItemBarcode { get; set; }

      [JsonProperty("itemQty")]
      public int ItemQty { get; set; }

      [JsonProperty("itemCode")]
      public string ItemCode { get; set; }

      [JsonProperty("itemAmount")]
      public int ItemAmount { get; set; }

      [JsonProperty("discount")]
      public int Discount { get; set; }

      [JsonProperty("itemTaxes")]
      public List<SmartOneDriver.DocumentItem.Tax> ItemTaxes { get; set; } = new List<SmartOneDriver.DocumentItem.Tax>();

      public class Tax
      {
        [JsonProperty("taxName")]
        public string TaxName { get; set; }

        [JsonProperty("taxPrc")]
        public int TaxPrc { get; set; }

        [JsonProperty("calcType")]
        public int CalcType { get; set; } = 1;
      }
    }

    public class DocumentPayments
    {
      [JsonProperty("cashAmount")]
      public int CashAmount { get; set; }

      [JsonProperty("cashlessAmount")]
      public int CashlessAmount { get; set; }

      [JsonProperty("creditAmount")]
      public int CreditAmount { get; set; }

      [JsonProperty("bonusesAmount")]
      public int BonusesAmount { get; set; }

      [JsonProperty("prepaymentAmount")]
      public int PrepaymentAmount { get; set; }

      [JsonProperty("prepaymentCashlessAmount")]
      public int PrepaymentCashlessAmount { get; set; }

      [JsonProperty("invoiceAmount")]
      public int InvoiceAmount { get; set; }

      [JsonProperty("rrn")]
      public int? Rrn { get; set; }

      [JsonProperty("checkNum")]
      public int? CheckNum { get; set; }

      [JsonProperty("originAmount")]
      public int? OriginAmount { get; set; }
    }
  }
}
