// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmartDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart
{
  public class AzSmartDriver
  {
    private readonly LanConnection _lanConnection;
    private readonly string _merchantId;

    public AzSmartDriver(LanConnection lanConnection, string merchantId)
    {
      this._lanConnection = lanConnection;
      this._merchantId = merchantId;
    }

    public void SendCommand(AzSmartDriver.IKkmAzSmart command) => this.DoCommand(command);

    private void DoCommand(AzSmartDriver.IKkmAzSmart command)
    {
      LogHelper.Debug("Начинаю выполнять комманду AZ smart: " + command.Command + Other.NewLine());
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this._lanConnection.UrlAddress + string.Format(":{0}", (object) this._lanConnection.PortNumber) + "/" + command.Command);
      httpWebRequest.ContentType = "application/json; charset=UTF-8";
      httpWebRequest.Method = "POST";
      httpWebRequest.Timeout = 120000;
      httpWebRequest.ReadWriteTimeout = 120000;
      httpWebRequest.KeepAlive = false;
      string str1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command.Data, Formatting.None, new JsonSerializerSettings()))).Replace("=", "%3D");
      string s;
      using (SHA1Managed shA1Managed = new SHA1Managed())
      {
        byte[] hash = shA1Managed.ComputeHash(Encoding.UTF8.GetBytes(str1.Replace("%3D", "=") + this._merchantId));
        StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("x2"));
        s = stringBuilder.ToString();
      }
      string str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace("=", "%3D");
      string str3 = "data=" + str1 + "&sign=" + str2;
      LogHelper.Debug("commnand: " + str3);
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        streamWriter.Write(str3);
      Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
      if (responseStream == null)
        throw new InvalidOperationException();
      using (StreamReader streamReader = new StreamReader(responseStream))
        command.AnswerString = streamReader.ReadToEnd();
      LogHelper.Debug("Ответ ККМ Az Smart:\r\n" + command.AnswerString);
    }

    private class Base64Command
    {
      [JsonProperty("data")]
      public string Data { get; set; }

      [JsonProperty("sign")]
      public string Sign { get; set; }
    }

    public interface IKkmAzSmart
    {
      [JsonIgnore]
      string Command { get; }

      [JsonProperty("data")]
      object Data { get; set; }

      [JsonIgnore]
      string AnswerString { get; set; }
    }

    public abstract class AzSmartCommand<T> : AzSmartDriver.IKkmAzSmart
    {
      public virtual string Command { get; }

      public string Answer { get; set; }

      public virtual object Data { get; set; }

      public string Sign { get; set; }

      public string AnswerString { get; set; }

      [JsonIgnore]
      public T Result => JsonConvert.DeserializeObject<T>(this.AnswerString);
    }

    public class SaleCommand : AzSmartDriver.AzSmartCommand<AzSmartDriver.SaleCommand.SaleAnswer>
    {
      public override string Command => "sale";

      public override object Data { get; set; } = (object) new AzSmartDriver.SaleCommand.SaleData();

      public class SaleData
      {
        [JsonProperty("docTime")]
        public string DocTime { get; set; }

        [JsonProperty("docNumber")]
        public string DocNumber { get; set; }

        [JsonProperty("wsName")]
        public string WsName { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("items")]
        public List<AzSmartDriver.SaleCommand.SaleData.Item> Items { get; set; } = new List<AzSmartDriver.SaleCommand.SaleData.Item>();

        [JsonProperty("payments")]
        public AzSmartDriver.SaleCommand.SaleData.Payment Payments { get; set; } = new AzSmartDriver.SaleCommand.SaleData.Payment();

        [JsonProperty("fiscalID")]
        public string FiscalID { get; set; }

        [JsonProperty("printFooter")]
        public string PrintFooter { get; set; }

        public class Item
        {
          [JsonProperty("itemId")]
          public string ItemId { get; set; }

          [JsonProperty("itemName")]
          public string ItemName { get; set; }

          [JsonProperty("itemAttr")]
          public int ItemAttr { get; set; }

          [JsonProperty("itemQty")]
          public int ItemQty { get; set; }

          [JsonProperty("itemAmount")]
          public int ItemAmount { get; set; }

          [JsonProperty("discount")]
          public int Discount { get; set; }

          [JsonProperty("itemMarginSum")]
          public int ItemMarginSum { get; set; }

          [JsonProperty("itemMarginPrice")]
          public int ItemMarginPrice { get; set; }

          [JsonProperty("itemTaxes")]
          public List<AzSmartDriver.SaleCommand.SaleData.Item.ItemTax> ItemsTaxes { get; set; } = new List<AzSmartDriver.SaleCommand.SaleData.Item.ItemTax>();

          public class ItemTax
          {
            [JsonProperty("taxName")]
            public string TaxName { get; set; }

            [JsonProperty("taxPrc")]
            public int TaxPrc { get; set; }

            [JsonProperty("calcType")]
            public int CalcType { get; set; }
          }
        }

        public class Payment
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
        }
      }

      public class SaleAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("printError")]
        public int PrintError { get; set; }

        [JsonProperty("documentID")]
        public int DocumentID { get; set; }

        [JsonProperty("fiscalID")]
        public string FiscalID { get; set; }
      }
    }

    public class ReturnCommand : AzSmartDriver.AzSmartCommand<AzSmartDriver.SaleCommand.SaleAnswer>
    {
      public override string Command => "refund";

      public override object Data { get; set; } = (object) new AzSmartDriver.ReturnCommand.ReturnData();

      public class ReturnData : AzSmartDriver.SaleCommand.SaleData
      {
        [JsonProperty("parentDocID")]
        public string ParentDocID { get; set; }

        [JsonProperty("parentDocNum ")]
        public string ParentDocNum { get; set; }
      }
    }

    public class CheckStatusCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.CheckStatusCommand.CheckStatusAnswer>
    {
      public override string Command => "check_status";

      public override object Data { get; set; } = (object) new AzSmartDriver.CheckStatusCommand.CheckStatusData();

      public class CheckStatusData
      {
        [JsonProperty("documentID")]
        public string DocumentID { get; set; }
      }

      public class CheckStatusAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("docStatus")]
        public int DocStatus { get; set; }

        [JsonProperty("cashAmount")]
        public int CashAmount { get; set; }

        [JsonProperty("cashlessAmount")]
        public int CashlessAmount { get; set; }

        [JsonProperty("transactionID")]
        public string TransactionID { get; set; }

        [JsonProperty("authCode")]
        public string AuthCode { get; set; }

        [JsonProperty("cardNum")]
        public string CardNum { get; set; }

        [JsonProperty("creditAmount")]
        public string CreditAmount { get; set; }

        [JsonProperty("bonusesAmount")]
        public int BonusesAmount { get; set; }

        [JsonProperty("prepaymentAmount")]
        public int PrepaymentAmount { get; set; }
      }
    }

    public class XReportCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.XReportCommand.XReportAnswer>
    {
      public override string Command => "x_report";

      public override object Data { get; set; } = (object) new AzSmartDriver.XReportCommand.XReportData();

      public class XReportData
      {
        [JsonProperty("shiftID")]
        public string ShiftID { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
      }

      public class XReportAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("cash")]
        public int Cash { get; set; }
      }
    }

    public class OpenShiftCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.OpenShiftCommand.OpenShiftAnswer>
    {
      public override string Command => "open_shift";

      public override object Data { get; set; } = (object) new AzSmartDriver.OpenShiftCommand.OpenShiftData();

      public class OpenShiftData
      {
        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("pincode")]
        public string PinCode { get; set; }
      }

      public class OpenShiftAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("fiscalShiftID")]
        public string FiscalShiftID { get; set; }

        [JsonProperty("shiftID")]
        public string ShiftID { get; set; }
      }
    }

    public class CloseShiftCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.CloseShiftCommand.CloseShiftAnswer>
    {
      public override string Command => "close_shift";

      public override object Data { get; set; } = (object) new AzSmartDriver.CloseShiftCommand.CloseShiftData();

      public class CloseShiftData
      {
        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("wsName")]
        public string WsName { get; set; }
      }

      public class CloseShiftAnswer : AzSmartDriver.AnswerAzSmart
      {
      }
    }

    public class CheckShiftCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.CheckShiftCommand.CheckShiftAnswer>
    {
      public override string Command => "check_shift";

      public override object Data { get; set; } = (object) new AzSmartDriver.CheckShiftCommand.CheckShiftData();

      public class CheckShiftData
      {
        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }
      }

      public class CheckShiftAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("isShiftOpen")]
        public bool IsShiftOpen { get; set; }
      }
    }

    public class InputCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.InputCommand.DepositAnswer>
    {
      public override string Command => "deposit";

      public override object Data { get; set; } = (object) new AzSmartDriver.InputCommand.DepositData();

      public class DepositData
      {
        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("docNumber")]
        public string DocNumber { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
      }

      public class DepositAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("documentID")]
        public int DocumentID { get; set; }

        [JsonProperty("fiscalID")]
        public string FiscalID { get; set; }
      }
    }

    public class OutputCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.InputCommand.DepositAnswer>
    {
      public override string Command => "withdraw";

      public override object Data { get; set; } = (object) new AzSmartDriver.InputCommand.DepositData();
    }

    public class GetInfoCommand : 
      AzSmartDriver.AzSmartCommand<AzSmartDriver.GetInfoCommand.GetInfoAnswer>
    {
      public override string Command => "get_info";

      public override object Data { get; set; } = (object) new AzSmartDriver.GetInfoCommand.GetInfoData();

      public class GetInfoData
      {
        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }
      }

      public class GetInfoAnswer : AzSmartDriver.AnswerAzSmart
      {
        [JsonProperty("serial_num")]
        public string SerialNum { get; set; }

        [JsonProperty("cashbox_app_version")]
        public string CashBoxAppVersion { get; set; }

        [JsonProperty("cashbox_factory_number")]
        public string CashBoxFactoryNumber { get; set; }

        [JsonProperty("cashbox_tax_number")]
        public string CashBoxTaxNumber { get; set; }

        [JsonProperty("cashregister_factory_number")]
        public string CashRegisterFactoryNumber { get; set; }

        [JsonProperty("cashregister_model")]
        public string CashRegisterModel { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("company_tax_number")]
        public string CompanyTaxNumber { get; set; }

        [JsonProperty("firmware_version")]
        public string FirmwareVersion { get; set; }

        [JsonProperty("fiscal_core_version")]
        public string FiscalCoreVersion { get; set; }

        [JsonProperty("last_online_time")]
        public DateTime LastOnlineTime { get; set; }

        [JsonProperty("not_after")]
        public string NotAfter { get; set; }

        [JsonProperty("not_before")]
        public string NotBefore { get; set; }

        [JsonProperty("object_address")]
        public string ObjectAddress { get; set; }

        [JsonProperty("object_name")]
        public string ObjectName { get; set; }

        [JsonProperty("object_tax_number")]
        public string ObjectTaxNumber { get; set; }

        [JsonProperty("oldest_document_time")]
        public DateTime? OldestDocumentTime { get; set; }

        [JsonProperty("qr_code_url")]
        public string QrCodeUrl { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("terminalID")]
        public string TerminalId { get; set; }
      }
    }

    public abstract class AnswerAzSmart
    {
      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("code")]
      public int Code { get; set; }
    }
  }
}
