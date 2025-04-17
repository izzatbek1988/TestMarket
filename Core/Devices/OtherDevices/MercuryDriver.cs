// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.OtherDevices.MercuryDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace Gbs.Core.Devices.OtherDevices
{
  public class MercuryDriver
  {
    private Gbs.Core.Config.Devices _config;

    public string SessionKey { get; set; }

    public MercuryDriver(Gbs.Core.Config.Devices devices) => this._config = devices;

    public void SendCommand(MercuryDriver.MercuryCommand command) => this.DoCommand(command);

    private void DoCommand(MercuryDriver.MercuryCommand command)
    {
      string str1 = JsonConvert.SerializeObject((object) command, Formatting.None, new JsonSerializerSettings()
      {
        NullValueHandling = command.GetType() == typeof (MercuryDriver.KkmOpenSession) ? NullValueHandling.Include : NullValueHandling.Ignore
      });
      LogHelper.Debug("Начинаю выполнять комманду Меркурий: " + Other.NewLine() + str1);
      string str2 = this._config.CheckPrinter.Connection.LanPort.UrlAddress;
      if (str2.IsNullOrEmpty())
        throw new DeviceException("Не указаны параметры подключения к ККТ, проверьте настройки оборудования.");
      if (str2.ToLower().StartsWith("http://"))
        str2 = str2.Replace("http://", "");
      if (this._config.CheckPrinter.IsNewProtocolMercury)
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("http://" + str2 + ":50010/api.json");
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 60000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str1);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.Answer = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ ККМ Меркурий:\r\n" + command.Answer);
      }
      else
      {
        byte[] bytes1 = System.Text.Encoding.UTF8.GetBytes(str1);
        LogHelper.Debug(str1);
        ServicePointManager.Expect100Continue = false;
        byte[] bytes2 = BitConverter.GetBytes(bytes1.Length);
        if (BitConverter.IsLittleEndian)
          Array.Reverse((Array) bytes2);
        byte[] array = ((IEnumerable<byte>) bytes2).Concat<byte>((IEnumerable<byte>) bytes1).ToArray<byte>();
        LogHelper.Debug("Data to send: " + string.Join<byte>(" ", (IEnumerable<byte>) array));
        int valueOrDefault = this._config.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault(50009);
        LogHelper.Debug("Подключение к службе " + str2 + ":" + valueOrDefault.ToString());
        NetworkStream stream = new TcpClient(str2, valueOrDefault).GetStream();
        LogHelper.Debug("Подключение");
        stream.Write(array, 0, array.Length);
        LogHelper.Debug("Данные записаны");
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        LogHelper.Debug("4 байта прочитаны");
        if (BitConverter.IsLittleEndian)
          Array.Reverse((Array) buffer);
        int int32 = BitConverter.ToInt32(buffer, 0);
        byte[] numArray = new byte[int32];
        stream.Read(numArray, 0, int32);
        LogHelper.Debug("Ответ прочитан. Длина ответа: " + int32.ToString());
        if (int32 == 0)
        {
          Mercury.RestartService();
          throw new KkmException((IDevice) new Mercury(), Translate.HrantDriver_DoCommand_Ответ_от_ККМ_не_содержит_данных__Возможно__связь_с_ККМ_не_была_установлена);
        }
        command.Answer = System.Text.Encoding.UTF8.GetString(numArray);
        LogHelper.Debug("Ответ ККМ Меркурий:\r\n" + command.Answer);
      }
    }

    private interface IKkmMercuryCommand
    {
      string command { get; }

      string sessionKey { get; set; }
    }

    public abstract class MercuryCommand
    {
      [JsonIgnore]
      public string Answer { get; set; }
    }

    public class KkmGetInfo : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmGetInfo.KkmInfoAnswer Data
      {
        get => JsonConvert.DeserializeObject<MercuryDriver.KkmGetInfo.KkmInfoAnswer>(this.Answer);
      }

      public string command => "GetStatus";

      public string sessionKey { get; set; }

      public class KkmInfoAnswer : MercuryDriver.MercuryAnswer
      {
        public DateTime dateTime { get; set; }

        public bool paperPresence { get; set; }

        public MercuryDriver.KkmGetInfo.KkmInfoAnswer.ShiftInfo shiftInfo { get; set; }

        public MercuryDriver.KkmGetInfo.KkmInfoAnswer.CheckInfo checkInfo { get; set; }

        public MercuryDriver.KkmGetInfo.KkmInfoAnswer.FnInfo fnInfo { get; set; }

        public class ShiftInfo
        {
          public bool isOpen { get; set; }

          public bool is24Expired { get; set; }

          public int num { get; set; }

          public string lastOpen { get; set; }

          public Decimal cash { get; set; }
        }

        public class CheckInfo
        {
          public bool isOpen { get; set; }

          public int num { get; set; }

          public int goodsQty { get; set; }
        }

        public class FnInfo
        {
          public int status { get; set; }

          public string fnNum { get; set; }

          public MercuryDriver.KkmGetInfo.KkmInfoAnswer.FnInfo.UnsignedDocs unsignedDocs { get; set; }

          public class UnsignedDocs
          {
            public int qty { get; set; }

            public int firstNum { get; set; }

            public string firstDateTime { get; set; }
          }
        }
      }
    }

    public class KkmGetCommonInfo : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmGetCommonInfo.GetCommonInfoAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmGetCommonInfo.GetCommonInfoAnswer>(this.Answer);
        }
      }

      public string command => "GetCommonInfo";

      public string sessionKey { get; set; }

      public class GetCommonInfoAnswer : MercuryDriver.MercuryAnswer
      {
        public string model { get; set; }

        public string kktNum { get; set; }

        public string fnNum { get; set; }

        public string ffdFnVer { get; set; }

        public string ffdKktVer { get; set; }

        public string ffdTotalVer { get; set; }

        public string programVer { get; set; }

        public string programDate { get; set; }

        public string dateTime { get; set; }
      }
    }

    public class KkmOpenSession : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmOpenSession.OpenSessionAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmOpenSession.OpenSessionAnswer>(this.Answer);
        }
      }

      public string portName { get; set; }

      public string command => "OpenSession";

      public string sessionKey { get; set; }

      public bool debug { get; set; } = true;

      public string model { get; set; }

      public string logPath
      {
        get
        {
          return ApplicationInfo.GetInstance().Paths.LogsPath + string.Format("mercury{0:ddMMyyyy}", (object) DateTime.Now);
        }
      }

      public class OpenSessionAnswer : MercuryDriver.MercuryAnswer
      {
        public string sessionKey { get; set; }
      }
    }

    public class KkmCloseSession : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmCloseSession.CloseSessionAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmCloseSession.CloseSessionAnswer>(this.Answer);
        }
      }

      public string command => "CloseSession";

      public string sessionKey { get; set; }

      public class CloseSessionAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class ClearMarkingCodeValidationTable : 
      MercuryDriver.MercuryCommand,
      MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.ClearMarkingCodeValidationTable.ClearMarkingCodeValidationTableAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.ClearMarkingCodeValidationTable.ClearMarkingCodeValidationTableAnswer>(this.Answer);
        }
      }

      public string command => nameof (ClearMarkingCodeValidationTable);

      public string sessionKey { get; set; }

      public class ClearMarkingCodeValidationTableAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class CheckMarkingCode : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.CheckMarkingCode.CheckMarkingCodeAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.CheckMarkingCode.CheckMarkingCodeAnswer>(this.Answer);
        }
      }

      public string command => nameof (CheckMarkingCode);

      public string sessionKey { get; set; }

      public string mc { get; set; }

      public int plannedStatus { get; set; }

      public int qty { get; set; }

      public int measureUnit { get; set; }

      public int processingMode => 0;

      public int timeout { get; set; }

      public MercuryDriver.CheckMarkingCode.PartClass part { get; set; }

      public class PartClass
      {
        public int numerator { get; set; }

        public int denominator { get; set; }
      }

      public class CheckMarkingCodeAnswer : MercuryDriver.MercuryAnswer
      {
        public MercuryDriver.CheckMarkingCode.CheckMarkingCodeAnswer.FnCheck fnCheck { get; set; }

        public MercuryDriver.CheckMarkingCode.CheckMarkingCodeAnswer.McInfo mcInfo { get; set; }

        public bool isOfflineMode { get; set; }

        public class FnCheck
        {
          public int checkResult { get; set; }

          public bool isValid { get; set; }
        }

        public class McInfo
        {
          public int mcType { get; set; }

          public string mcGoodsID { get; set; }
        }
      }
    }

    public class GetMarkingCodeCheckResult : 
      MercuryDriver.MercuryCommand,
      MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.GetMarkingCodeCheckResult.GetMarkingCodeCheckResultAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.GetMarkingCodeCheckResult.GetMarkingCodeCheckResultAnswer>(this.Answer);
        }
      }

      public string command => nameof (GetMarkingCodeCheckResult);

      public string sessionKey { get; set; }

      public class GetMarkingCodeCheckResultAnswer : MercuryDriver.MercuryAnswer
      {
        public MercuryDriver.GetMarkingCodeCheckResult.GetMarkingCodeCheckResultAnswer.OnlineCheck onlineCheck { get; set; }

        public bool isCompleted { get; set; }

        public class OnlineCheck : MercuryDriver.MercuryAnswer
        {
          public int processingResult { get; set; }

          public bool mcCheckResult { get; set; }

          public int plannedStatusCheckResult { get; set; }

          public int mcCheckResultRaw { get; set; }

          public MercuryDriver.GetMarkingCodeCheckResult.GetMarkingCodeCheckResultAnswer.CorrectedData correctedData { get; set; }
        }

        public class CorrectedData
        {
          public int mcType { get; set; }

          public string mcGoodsID { get; set; }

          public int processingMode { get; set; }
        }
      }
    }

    public class AbortMarkingCodeChecking : 
      MercuryDriver.MercuryCommand,
      MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.AbortMarkingCodeChecking.AbortMarkingCodeCheckingAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.AbortMarkingCodeChecking.AbortMarkingCodeCheckingAnswer>(this.Answer);
        }
      }

      public string command => nameof (AbortMarkingCodeChecking);

      public string sessionKey { get; set; }

      public class AbortMarkingCodeCheckingAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class AcceptMarkingCode : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.AcceptMarkingCode.AcceptMarkingCodeAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.AcceptMarkingCode.AcceptMarkingCodeAnswer>(this.Answer);
        }
      }

      public string command => nameof (AcceptMarkingCode);

      public string sessionKey { get; set; }

      public class AcceptMarkingCodeAnswer : MercuryDriver.MercuryAnswer
      {
        public int mcCheckResultRaw { get; set; }
      }
    }

    public class RejectMarkingCode : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.RejectMarkingCode.RejectMarkingCodeAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.RejectMarkingCode.RejectMarkingCodeAnswer>(this.Answer);
        }
      }

      public string command => "AcceptMarkingCode";

      public string sessionKey { get; set; }

      public class RejectMarkingCodeAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmOpenShift : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmOpenShift.OpenShiftAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmOpenShift.OpenShiftAnswer>(this.Answer);
        }
      }

      public bool printDoc { get; set; } = true;

      public MercuryDriver.CashierInfo cashierInfo { get; set; }

      public string command => "OpenShift";

      public string sessionKey { get; set; }

      public class OpenShiftAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmCloseShift : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmCloseShift.OpenShiftAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmCloseShift.OpenShiftAnswer>(this.Answer);
        }
      }

      public bool printDoc { get; set; } = true;

      public MercuryDriver.CashierInfo cashierInfo { get; set; }

      public string command => "CloseShift";

      public string sessionKey { get; set; }

      public class OpenShiftAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class GetDriverInfo : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      public string command => nameof (GetDriverInfo);

      public string sessionKey { get; set; }
    }

    public class KkmOpenCheck : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmOpenCheck.OpenCheckAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmOpenCheck.OpenCheckAnswer>(this.Answer);
        }
      }

      public int checkType { get; set; }

      public int taxSystem { get; set; }

      public bool printDoc { get; set; } = true;

      public MercuryDriver.CashierInfo cashierInfo { get; set; }

      public MercuryDriver.BuyerInfo buyerInfo { get; set; }

      public string command => "OpenCheck";

      public string sessionKey { get; set; }

      public class OpenCheckAnswer : MercuryDriver.MercuryAnswer
      {
        public int shiftNum { get; set; }

        public int checkNum { get; set; }
      }
    }

    public class KkmAddGood : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmAddGood.AddGoodAnswer Data
      {
        get => JsonConvert.DeserializeObject<MercuryDriver.KkmAddGood.AddGoodAnswer>(this.Answer);
      }

      public string nomenclatureCode { get; set; }

      public string markingCode { get; set; }

      public MercuryDriver.KkmAddGood.McInfo mcInfo { get; set; }

      public string productName { get; set; }

      public int? measureUnit { get; set; }

      public int qty { get; set; }

      public int section { get; set; }

      public int taxCode { get; set; }

      public int paymentFormCode { get; set; }

      public int productTypeCode { get; set; }

      public string taxCcountryOfOriginode { get; set; }

      public string customsDeclaration { get; set; }

      public int price { get; set; }

      public int sum { get; set; }

      public string command => "AddGoods";

      public string sessionKey { get; set; }

      public List<MercuryDriver.KkmAddGood.IndustryAttributeItem> industryAttribute { get; set; }

      public class AddGoodAnswer : MercuryDriver.MercuryAnswer
      {
        public int shiftNum { get; set; }

        public int checkNum { get; set; }

        public int goodsNum { get; set; }
      }

      public class McInfo
      {
        public string mc { get; set; }

        public string ean { get; set; }

        public int plannedStatus { get; set; }

        public int processingMode => 0;

        public MercuryDriver.CheckMarkingCode.PartClass part { get; set; }
      }

      public class IndustryAttributeItem
      {
        public string idFOIV { get; set; }

        public string docDate { get; set; }

        public string docNum { get; set; }

        public string value { get; set; }
      }
    }

    public class KkmCloseCheck : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmCloseCheck.CloseCheckAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmCloseCheck.CloseCheckAnswer>(this.Answer);
        }
      }

      public string sendCheckTo { get; set; }

      public MercuryDriver.KkmCloseCheck.Payments payment { get; set; }

      public string command => "CloseCheck";

      public string sessionKey { get; set; }

      public class Payments
      {
        public int cash { get; set; }

        public int ecash { get; set; }

        public int prepayment { get; set; }

        public int credit { get; set; }

        public int consideration { get; set; }
      }

      public class CloseCheckAnswer : MercuryDriver.MercuryAnswer
      {
        public int shiftNum { get; set; }

        public int checkNum { get; set; }

        public int goodsNum { get; set; }
      }
    }

    public class KkmCancelCheck : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmCancelCheck.CancelCheckAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmCancelCheck.CancelCheckAnswer>(this.Answer);
        }
      }

      public string command => "ResetCheck";

      public string sessionKey { get; set; }

      public class CancelCheckAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmClosePorts : MercuryDriver.MercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmClosePorts.KkmClosePortsAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmClosePorts.KkmClosePortsAnswer>(this.Answer);
        }
      }

      public string command => "ClosePorts";

      public class KkmClosePortsAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmOpenBox : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmOpenBox.OpenBoxAnswer Data
      {
        get => JsonConvert.DeserializeObject<MercuryDriver.KkmOpenBox.OpenBoxAnswer>(this.Answer);
      }

      public string command => "OpenBox";

      public string sessionKey { get; set; }

      public class OpenBoxAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmBringMoney : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmBringMoney.BringMoneyAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmBringMoney.BringMoneyAnswer>(this.Answer);
        }
      }

      public MercuryDriver.CashierInfo cashierInfo { get; set; }

      public int cash { get; set; }

      public string command => "BringMoney";

      public string sessionKey { get; set; }

      public class BringMoneyAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmWithdrawMoney : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmWithdrawMoney.WithdrawMoneyAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmWithdrawMoney.WithdrawMoneyAnswer>(this.Answer);
        }
      }

      public MercuryDriver.CashierInfo cashierInfo { get; set; }

      public int cash { get; set; }

      public string command => "WithdrawMoney";

      public string sessionKey { get; set; }

      public class WithdrawMoneyAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmPrintBarCode : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmPrintBarCode.PrintBarCodeAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmPrintBarCode.PrintBarCodeAnswer>(this.Answer);
        }
      }

      public int bcType { get; set; }

      public string value { get; set; }

      public string command => "PrintBarCode";

      public string sessionKey { get; set; }

      public class PrintBarCodeAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmPrintText : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmPrintText.PrintTextAnswer Data
      {
        get
        {
          return JsonConvert.DeserializeObject<MercuryDriver.KkmPrintText.PrintTextAnswer>(this.Answer);
        }
      }

      public bool forcePrint { get; set; }

      public string text { get; set; }

      public string command => "PrintText";

      public string sessionKey { get; set; }

      public class PrintTextAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public class KkmGetReport : MercuryDriver.MercuryCommand, MercuryDriver.IKkmMercuryCommand
    {
      [JsonIgnore]
      public MercuryDriver.KkmGetReport.ReportAnswer Data
      {
        get => JsonConvert.DeserializeObject<MercuryDriver.KkmGetReport.ReportAnswer>(this.Answer);
      }

      public int reportCode { get; set; }

      public string command => "PrintReport";

      public string sessionKey { get; set; }

      public class ReportAnswer : MercuryDriver.MercuryAnswer
      {
      }
    }

    public abstract class MercuryAnswer
    {
      public int result { get; set; }

      public string description { get; set; }
    }

    public class CashierInfo
    {
      public string cashierName { get; set; }

      public string cashierINN { get; set; }

      public CashierInfo(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
      {
        this.cashierName = cashier.Name;
        this.cashierINN = cashier.Inn;
      }
    }

    public class BuyerInfo
    {
      public string buyerName { get; set; }

      public string buyerINN { get; set; }

      public BuyerInfo(Gbs.Core.Entities.Clients.Client client)
      {
        this.buyerName = client?.Name;
        this.buyerINN = (client != null ? client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() : (string) null) ?? "";
      }
    }
  }
}
