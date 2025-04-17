// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.OtherDevices.AtolServerDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.OtherDevices
{
  public class AtolServerDriver
  {
    private LanConnection _lanConnection;

    public AtolServerDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public bool SendCommand(AtolServerDriver.AtolServerCommand command)
    {
      if (!this.DoCommand(command))
        return false;
      switch (command.DeviceAnswer.results.First<AtolServerDriver.Answer>().status)
      {
        case "ready":
          return true;
        case "error":
        case "interrupted":
          return false;
        case "wait":
        case "inProgress":
          string status;
          string[] strArray;
          do
          {
            Other.ConsoleWrite("Ожидание ответа от устройства...");
            Thread.Sleep(100);
            this.GetResult(command);
            status = command.DeviceAnswer.results.First<AtolServerDriver.Answer>().status;
            strArray = new string[2]{ "wait", "inProgress" };
          }
          while (status.IsEither<string>(strArray));
          break;
      }
      LogHelper.Debug("Ответ ATOL-сервера:\r\n" + command.Answer);
      return command.DeviceAnswer.results.First<AtolServerDriver.Answer>().errorCode == 0;
    }

    private bool GetResult(AtolServerDriver.AtolServerCommand command)
    {
      this._lanConnection.PortNumber = new int?(16732);
      string str = this._lanConnection.UrlAddress;
      if (!str.ToLower().StartsWith("http://"))
        str = "http://" + str;
      WebRequest webRequest = WebRequest.Create(str + ":" + this._lanConnection.PortNumber.ToString() + "/requests" + "/" + command.uuid);
      if (!this._lanConnection.UserLogin.IsNullOrEmpty())
      {
        string base64String = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(this._lanConnection.UserLogin + ":" + this._lanConnection.Password));
        webRequest.Headers.Add("Authorization", "Basic " + base64String);
      }
      using (WebResponse response = webRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            command.Answer = streamReader.ReadToEnd();
        }
      }
      return true;
    }

    private bool DoCommand(AtolServerDriver.AtolServerCommand command)
    {
      try
      {
        this._lanConnection.PortNumber = new int?(16732);
        string str = this._lanConnection.UrlAddress;
        if (!str.ToLower().StartsWith("http://"))
          str = "http://" + str;
        string requestUriString = str + ":" + this._lanConnection.PortNumber.ToString() + "/requests";
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.Method = "POST";
        string base64String = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(this._lanConnection.UserLogin + ":" + this._lanConnection.Password));
        if (!this._lanConnection.UserLogin.IsNullOrEmpty())
          httpWebRequest.Headers.Add("Authorization", "Basic " + base64String);
        httpWebRequest.ContentType = "application/json";
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
          {
            string message = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
            {
              NullValueHandling = NullValueHandling.Ignore
            });
            LogHelper.Debug(message);
            streamWriter.Write(message);
          }
        }
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              Other.ConsoleWrite("response: " + streamReader.ReadToEnd());
          }
        }
        WebRequest webRequest = WebRequest.Create(requestUriString + "/" + command.uuid);
        if (!this._lanConnection.UserLogin.IsNullOrEmpty())
          webRequest.Headers.Add("Authorization", "Basic " + base64String);
        using (WebResponse response = webRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              command.Answer = streamReader.ReadToEnd();
          }
        }
        LogHelper.Debug("ATOL SERVER ANSWER: " + Other.NewLine() + command.Answer);
        return true;
      }
      catch (WebException ex)
      {
        switch (ex.Status)
        {
          case WebExceptionStatus.ConnectFailure:
            LogHelper.Error((Exception) ex, "Ошиба отправки команды на АТОЛ-СЕРВЕР", false);
            int num1 = (int) MessageBoxHelper.Show(string.Format("Не удалось подключиться к АТОЛ-серверу по адресу: {0}:{1}. Убедитесь, что адрес указан корректно и сервер запущен.", (object) this._lanConnection.UrlAddress, (object) this._lanConnection.PortNumber), string.Empty, icon: MessageBoxImage.Hand);
            return false;
          case WebExceptionStatus.ProtocolError:
            if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
            {
              LogHelper.Error((Exception) ex, "Ошибка отправки команды на АТОЛ-Сервер", false);
              int num2 = (int) MessageBoxHelper.Show("Не удалось подключиться к АТОЛ-Серверу: неверный логин или пароль.", string.Empty, icon: MessageBoxImage.Hand);
              return false;
            }
            break;
        }
        LogHelper.Error((Exception) ex, "Ошиба отправки команды на АТОЛ-СЕРВЕР");
        return false;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошиба отправки команды на АТОЛ-СЕРВЕР");
        return false;
      }
    }

    public abstract class AtolServerCommand
    {
      public string uuid { get; set; } = Guid.NewGuid().ToString();

      [JsonIgnore]
      public string Answer { get; set; }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.Answer> DeviceAnswer
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.Answer>>(this.Answer);
        }
      }
    }

    public class KkmOpenSessionCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmOpenSessionCommand.KkmOpenSession Command { get; set; }

      public List<AtolServerDriver.KkmOpenSessionCommand.KkmOpenSession> request
      {
        get
        {
          return new List<AtolServerDriver.KkmOpenSessionCommand.KkmOpenSession>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmOpenSessionCommand.KkmOpenSessionResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmOpenSessionCommand.KkmOpenSessionResult>>(this.Answer);
        }
      }

      public class KkmOpenSession : AtolServerDriver.AtolCommand
      {
        public AtolServerDriver.Cashier @operator { get; set; }

        public string type => "openShift";
      }

      public class KkmOpenSessionResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmOpenSessionCommand.KkmOpenSessionResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmOpenSessionCommand.KkmOpenSessionResult.Result.FiscalParams fiscalParams { get; set; }

          public class FiscalParams
          {
            public int fiscalDocumentNumber { get; set; }

            public string fiscalDocumentSign { get; set; }

            public string fiscalDocumentDateTime { get; set; }

            public int shiftNumber { get; set; }

            public string fnNumber { get; set; }

            public string registrationNumber { get; set; }
          }
        }
      }
    }

    public class KkmCloseSessionCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmCloseSessionCommand.KkmCloseSession Command { get; set; }

      public List<AtolServerDriver.KkmCloseSessionCommand.KkmCloseSession> request
      {
        get
        {
          return new List<AtolServerDriver.KkmCloseSessionCommand.KkmCloseSession>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmCloseSessionCommand.KkmCloseSessionResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmCloseSessionCommand.KkmCloseSessionResult>>(this.Answer);
        }
      }

      public class KkmCloseSession : AtolServerDriver.AtolCommand
      {
        public AtolServerDriver.Cashier @operator { get; set; }

        public string type => "closeShift";
      }

      public class KkmCloseSessionResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmCloseSessionCommand.KkmCloseSessionResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmCloseSessionCommand.KkmCloseSessionResult.Result.FiscalParams fiscalParams { get; set; }

          public class FiscalParams
          {
            public int fiscalDocumentNumber { get; set; }

            public string fiscalDocumentSign { get; set; }

            public string fiscalDocumentDateTime { get; set; }

            public int shiftNumber { get; set; }

            public string fnNumber { get; set; }

            public string registrationNumber { get; set; }

            public int receiptsCount { get; set; }
          }
        }
      }
    }

    public class KkmGetXReportommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetXReportommand.KkmGetXReport Command { get; set; }

      public List<AtolServerDriver.KkmGetXReportommand.KkmGetXReport> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetXReportommand.KkmGetXReport>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetXReportommand.KkmGetXReportResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetXReportommand.KkmGetXReportResult>>(this.Answer);
        }
      }

      public class KkmGetXReport : AtolServerDriver.AtolCommand
      {
        public AtolServerDriver.Cashier @operator { get; set; }

        public string type => "reportX";
      }

      public class KkmGetXReportResult : AtolServerDriver.Answer
      {
      }
    }

    public class KkmGetStatusCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetStatusCommand.KkmGetStatus Command { get; set; } = new AtolServerDriver.KkmGetStatusCommand.KkmGetStatus();

      public List<AtolServerDriver.KkmGetStatusCommand.KkmGetStatus> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetStatusCommand.KkmGetStatus>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult>>(this.Answer);
        }
      }

      public class KkmGetStatus : AtolServerDriver.AtolCommand
      {
        public string type => "getDeviceStatus";
      }

      public class KkmGetStatusResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetStatusCommand.KkmGetStatusResult.Result.DeviceStatus deviceStatus { get; set; }

          public class DeviceStatus
          {
            public string currentDateTime { get; set; }

            public string shift { get; set; }

            public bool blocked { get; set; }

            public bool coverOpened { get; set; }

            public bool paperPresent { get; set; }

            public bool fiscal { get; set; }

            public bool fnFiscal { get; set; }

            public bool fnPresent { get; set; }

            public bool cashDrawerOpened { get; set; }
          }
        }
      }
    }

    public class KkmGetShiftStatusCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatus Command { get; set; } = new AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatus();

      public List<AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatus> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatus>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult>>(this.Answer);
        }
      }

      public class KkmGetShiftStatus : AtolServerDriver.AtolCommand
      {
        public string type => "getShiftStatus";
      }

      public class KkmGetShiftStatusResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetShiftStatusCommand.KkmGetShiftStatusResult.Result.ShiftStatus shiftStatus { get; set; }

          public class ShiftStatus
          {
            public string expiredTime { get; set; }

            public int number { get; set; }

            public string status { get; set; }
          }
        }
      }
    }

    public class KkmGetFnStatusCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetFnStatusCommand.KkmGetFntStatus Command { get; set; } = new AtolServerDriver.KkmGetFnStatusCommand.KkmGetFntStatus();

      public List<AtolServerDriver.KkmGetFnStatusCommand.KkmGetFntStatus> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetFnStatusCommand.KkmGetFntStatus>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult>>(this.Answer);
        }
      }

      public class KkmGetFntStatus : AtolServerDriver.AtolCommand
      {
        public string type => "getFnStatus";
      }

      public class KkmGetFnStatusResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetFnStatusCommand.KkmGetFnStatusResult.Result.FnStatus fnStatus { get; set; }

          public class FnStatus
          {
            public int fiscalReceiptNumber { get; set; }

            public int fiscalDocumentNumber { get; set; }
          }
        }
      }
    }

    public class KkmGetOfdStatusCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdtStatus Command { get; set; } = new AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdtStatus();

      public List<AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdtStatus> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdtStatus>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult>>(this.Answer);
        }
      }

      public class KkmGetOfdtStatus : AtolServerDriver.AtolCommand
      {
        public string type => "ofdExchangeStatus";
      }

      public class KkmGetOfdStatusResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetOfdStatusCommand.KkmGetOfdStatusResult.Result.Status status { get; set; }

          public class Status
          {
            public int notSentCount { get; set; }

            public int notSentFirstDocNumber { get; set; }

            public string notSentFirstDocDateTime { get; set; }
          }
        }
      }
    }

    public class KkmGetDeviceInfoCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfo Command { get; set; } = new AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfo();

      public List<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfo> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfo>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult>>(this.Answer);
        }
      }

      public class KkmGetDeviceInfo : AtolServerDriver.AtolCommand
      {
        public string type => "getDeviceInfo";
      }

      public class KkmGetDeviceInfoResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetDeviceInfoCommand.KkmGetDeviceInfoResult.Result.DeviceInfo deviceInfo { get; set; }

          public class DeviceInfo
          {
            public int model { get; set; }

            public string modelName { get; set; }

            public string serial { get; set; }

            public string firmwareVersion { get; set; }

            public string configurationVersion { get; set; }

            public int receiptLineLength { get; set; }

            public int receiptLineLengthPix { get; set; }

            public string ffdVersion { get; set; }

            public string fnFfdVersion { get; set; }
          }
        }
      }
    }

    public class KkmGetTotalsCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetTotalsCommand.KkmGetTotals Command { get; set; } = new AtolServerDriver.KkmGetTotalsCommand.KkmGetTotals();

      public List<AtolServerDriver.KkmGetTotalsCommand.KkmGetTotals> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetTotalsCommand.KkmGetTotals>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetTotalsCommand.KkmGetTotalsResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetTotalsCommand.KkmGetTotalsResult>>(this.Answer);
        }
      }

      public class KkmGetTotals : AtolServerDriver.AtolCommand
      {
        public string type => "getShiftTotals";
      }

      public class KkmGetTotalsResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetTotalsCommand.KkmGetTotalsResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetTotalsCommand.KkmGetTotalsResult.Result.ShiftTotals shiftTotals { get; set; }

          public class ShiftTotals
          {
            public int shiftNumber { get; set; }

            public AtolServerDriver.KkmGetTotalsCommand.KkmGetTotalsResult.Result.ShiftTotals.CashDrawer cashDrawer { get; set; }

            public class CashDrawer
            {
              public Decimal sum { get; set; }
            }
          }
        }
      }
    }

    public class KkmGetCashDrawerCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawer Command { get; set; } = new AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawer();

      public List<AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawer> request
      {
        get
        {
          return new List<AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawer>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult>>(this.Answer);
        }
      }

      public class KkmGetCashDrawer : AtolServerDriver.AtolCommand
      {
        public string type => "getCashDrawerStatus";
      }

      public class KkmGetCashDrawerResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult.Result result { get; set; }

        public class Result
        {
          public AtolServerDriver.KkmGetCashDrawerCommand.KkmGetCashDrawerResult.Result.Counters counters { get; set; }

          public class Counters
          {
            public Decimal cashSum { get; set; }
          }
        }
      }
    }

    public class KkmRegistreCheckCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck Command { get; set; }

      public List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck> request
      {
        get
        {
          return new List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheckResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheckResult>>(this.Answer);
        }
      }

      public class KkmRegistreCheck : AtolServerDriver.AtolCommand
      {
        public bool? validateMarkingCodes { get; set; }

        [JsonIgnore]
        public CheckTypes Type { get; set; }

        public bool electronically { get; set; }

        public string taxationType { get; set; }

        public AtolServerDriver.Cashier @operator { get; set; }

        public AtolServerDriver.ClientInfo clientInfo { get; set; }

        public List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem> items { get; set; }

        public List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Payments> payments { get; set; }

        public string type
        {
          get
          {
            switch (this.Type)
            {
              case CheckTypes.Sale:
                return "sell";
              case CheckTypes.ReturnSale:
                return "sellReturn";
              case CheckTypes.Buy:
                return "buy";
              case CheckTypes.ReturnBuy:
                return "buyReturn";
              default:
                return "";
            }
          }
        }

        public interface CheckItem
        {
          string type { get; }
        }

        public class NonFiscalString : 
          AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem
        {
          public string type => "text";

          public string text { get; set; }

          public string alignment { get; set; }

          public string wrap { get; set; }

          public int font { get; set; }

          public bool doubleWidth { get; set; }

          public bool doubleHeight { get; set; }

          public NonFiscalString(string _text) => this.text = _text;
        }

        public class Goods : AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.CheckItem
        {
          public string type => "position";

          public string name { get; set; }

          public Decimal price { get; set; }

          public Decimal quantity { get; set; }

          public Decimal amount { get; set; }

          public Decimal infoDiscountAmount { get; set; }

          public string paymentObject { get; set; }

          public string paymentMethod { get; set; }

          public int? measurementUnit { get; set; }

          public int? itemUnits { get; set; }

          public string nomenclatureCode { get; set; }

          public AtolServerDriver.MarkedInfo imcParams { get; set; }

          public List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.IndustryInfo> industryInfo { get; set; }

          public AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods.Tax tax { get; set; } = new AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.Goods.Tax();

          public class Tax
          {
            public string type { get; set; }
          }
        }

        public class Payments
        {
          public string type { get; set; }

          public Decimal sum { get; set; }
        }

        public class IndustryInfo
        {
          public string date { get; set; }

          public string fois { get; set; }

          public string number { get; set; }

          public string industryAttribute { get; set; }
        }
      }

      public class KkmRegistreCheckResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheckResult.Result result { get; set; }

        public class Result
        {
        }
      }
    }

    public class MarkedInfo
    {
      public int imcType { get; set; }

      public string imc { get; set; }

      public int itemEstimatedStatus { get; set; }

      public int imcModeProcessing { get; set; }

      public double? itemQuantity { get; set; }

      public int? itemUnits { get; set; }

      public AtolServerDriver.InfoCheckResult itemInfoCheckResult { get; set; }
    }

    public class InfoCheckResult
    {
      public bool imcCheckFlag { get; set; }

      public bool imcCheckResult { get; set; }

      public bool imcStatusInfo { get; set; }

      public bool imcEstimatedStatusCorrect { get; set; }

      public bool ecrStandAloneFlag { get; set; }
    }

    public class СlearMarkingCodeCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.СlearMarkingCodeCommand.СlearMarkingCode Command { get; set; } = new AtolServerDriver.СlearMarkingCodeCommand.СlearMarkingCode();

      public List<AtolServerDriver.СlearMarkingCodeCommand.СlearMarkingCode> request
      {
        get
        {
          return new List<AtolServerDriver.СlearMarkingCodeCommand.СlearMarkingCode>()
          {
            this.Command
          };
        }
      }

      public class СlearMarkingCode : AtolServerDriver.AtolCommand
      {
        public string type => "clearMarkingCodeValidationResult";
      }
    }

    public class AcceptMarkingCodeCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.AcceptMarkingCodeCommand.AcceptMarkingCode Command { get; set; } = new AtolServerDriver.AcceptMarkingCodeCommand.AcceptMarkingCode();

      public List<AtolServerDriver.AcceptMarkingCodeCommand.AcceptMarkingCode> request
      {
        get
        {
          return new List<AtolServerDriver.AcceptMarkingCodeCommand.AcceptMarkingCode>()
          {
            this.Command
          };
        }
      }

      public class AcceptMarkingCode : AtolServerDriver.AtolCommand
      {
        public string type => "acceptMarkingCode";
      }
    }

    public class СancelMarkingCodeValidationCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.СancelMarkingCodeValidationCommand.СancelMarkingCodeValidation Command { get; set; } = new AtolServerDriver.СancelMarkingCodeValidationCommand.СancelMarkingCodeValidation();

      public List<AtolServerDriver.СancelMarkingCodeValidationCommand.СancelMarkingCodeValidation> request
      {
        get
        {
          return new List<AtolServerDriver.СancelMarkingCodeValidationCommand.СancelMarkingCodeValidation>()
          {
            this.Command
          };
        }
      }

      public class СancelMarkingCodeValidation : AtolServerDriver.AtolCommand
      {
        public string type => "cancelMarkingCodeValidation";
      }
    }

    public class BeginMarkingCodeValidationCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidation Command { get; set; } = new AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidation();

      public List<AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidation> request
      {
        get
        {
          return new List<AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidation>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidationResult Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidationResult>(this.Answer);
        }
      }

      public class BeginMarkingCodeValidation : AtolServerDriver.AtolCommand
      {
        public string type => "beginMarkingCodeValidation";

        public AtolServerDriver.MarkedInfo @params { get; set; }
      }

      public class BeginMarkingCodeValidationResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.BeginMarkingCodeValidationCommand.BeginMarkingCodeValidationResult.OfflineValidation offlineValidation { get; set; }

        public class OfflineValidation
        {
          public bool fmCheck { get; set; }

          public bool fmCheckResult { get; set; }

          public string fmCheckErrorReason { get; set; }
        }
      }
    }

    public class GetMarkingCodeValidationStatusCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatus Command { get; set; } = new AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatus();

      public List<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatus> request
      {
        get
        {
          return new List<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatus>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult>>(this.Answer);
        }
      }

      public class GetMarkingCodeValidationStatus : AtolServerDriver.AtolCommand
      {
        public string type => "getMarkingCodeValidationStatus";
      }

      public class GetMarkingCodeValidationStatusResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult.Result result { get; set; }

        public class Result
        {
          public bool ready { get; set; }

          public bool sentImcRequest { get; set; }

          public AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult.Result.DriverError driverError { get; set; }

          public AtolServerDriver.GetMarkingCodeValidationStatusCommand.GetMarkingCodeValidationStatusResult.Result.OnlineValidation onlineValidation { get; set; }

          public class DriverError
          {
            public int code { get; set; }

            public string error { get; set; }

            public string description { get; set; }
          }

          public class OnlineValidation
          {
            public AtolServerDriver.InfoCheckResult itemInfoCheckResult { get; set; }

            public int imcModeProcessing { get; set; }

            public string markOperatorItemStatus { get; set; }

            public string markOperatorResponseResult { get; set; }

            public string imcType { get; set; }

            public string imcBarcode { get; set; }
          }
        }
      }
    }

    public class KkmPrintNonFiscalCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalText Command { get; set; } = new AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalText();

      public List<AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalText> request
      {
        get
        {
          return new List<AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalText>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalResult>>(this.Answer);
        }
      }

      public class KkmPrintNonFiscalText : AtolServerDriver.AtolCommand
      {
        public List<AtolServerDriver.KkmRegistreCheckCommand.KkmRegistreCheck.NonFiscalString> items { get; set; }

        public string type => "nonFiscal";
      }

      public class KkmPrintNonFiscalResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmPrintNonFiscalCommand.KkmPrintNonFiscalResult.Result result { get; set; }

        public class Result
        {
        }
      }
    }

    public class KkmPrintBarcodeCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode Command { get; set; } = new AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode();

      public List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode> request
      {
        get
        {
          return new List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarcodeResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarcodeResult>>(this.Answer);
        }
      }

      public class KkmPrintBarode : AtolServerDriver.AtolCommand
      {
        public List<AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarode.Barcode> items { get; set; }

        public string type => "nonFiscal";

        public class Barcode
        {
          public string type => "barocde";

          public string barcode { get; set; }

          public int scale { get; set; }

          public string barcodeType { get; set; }

          public string alignment { get; set; }

          public int height { get; set; }

          public bool printText { get; set; }
        }
      }

      public class KkmPrintBarcodeResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmPrintBarcodeCommand.KkmPrintBarcodeResult.Result result { get; set; }

        public class Result
        {
        }
      }
    }

    public class KkmPaymentActionCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction Command { get; set; } = new AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction();

      public List<AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction> request
      {
        get
        {
          return new List<AtolServerDriver.KkmPaymentActionCommand.KkmPaymentAction>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmPaymentActionCommand.KkmPaymentActionResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmPaymentActionCommand.KkmPaymentActionResult>>(this.Answer);
        }
      }

      public class KkmPaymentAction : AtolServerDriver.AtolCommand
      {
        public string Type { get; set; }

        public AtolServerDriver.Cashier @operator { get; set; }

        public Decimal cashSum { get; set; }

        public string type => this.Type;
      }

      public class KkmPaymentActionResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmPaymentActionCommand.KkmPaymentActionResult.Result result { get; set; }

        public class Result
        {
        }
      }
    }

    public class KkmOpenCashDrawerCommand : AtolServerDriver.AtolServerCommand
    {
      [JsonIgnore]
      public AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawer Command { get; set; } = new AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawer();

      public List<AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawer> request
      {
        get
        {
          return new List<AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawer>()
          {
            this.Command
          };
        }
      }

      [JsonIgnore]
      public AtolServerDriver.Results<AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawerResult> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolServerDriver.Results<AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawerResult>>(this.Answer);
        }
      }

      public class KkmOpenCashDrawer : AtolServerDriver.AtolCommand
      {
        public string type => "openCashDrawer";
      }

      public class KkmOpenCashDrawerResult : AtolServerDriver.Answer
      {
        public AtolServerDriver.KkmOpenCashDrawerCommand.KkmOpenCashDrawerResult.Result result { get; set; }

        public class Result
        {
        }
      }
    }

    public class Cashier
    {
      public string name { get; set; }

      public string vatin { get; set; }

      public Cashier(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
      {
        this.name = cashier.Name;
        this.vatin = cashier.Inn;
      }
    }

    public class ClientInfo
    {
      public string name { get; set; }

      public string vatin { get; set; }

      public string emailOrPhone { get; set; }

      public ClientInfo(Client client)
      {
        this.name = client.Name;
        string str = client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
        if (str.IsNullOrEmpty())
          return;
        this.vatin = str;
      }

      public ClientInfo()
      {
      }
    }

    public interface AtolCommand
    {
      string type { get; }
    }

    public class Answer
    {
      public string status { get; set; }

      public int errorCode { get; set; }

      public string errorDescription { get; set; }
    }

    public class Results<T>
    {
      public List<T> results { get; set; }
    }
  }
}
