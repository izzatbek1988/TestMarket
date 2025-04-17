// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.OtherDevices.DevicesConnectorDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;

#nullable enable
namespace Gbs.Core.Devices.OtherDevices
{
  public class DevicesConnectorDriver
  {
    private 
    #nullable disable
    LanConnection _lanConnection;

    public DevicesConnectorDriver(LanConnection lanConnection)
    {
      this._lanConnection = lanConnection;
    }

    public void SendCommand(
      DevicesConnectorDriver.DevicesConnectorCommand command)
    {
      LogHelper.Debug("Команда на DevicesConnector:" + command.ToJsonString(true));
      this.DoCommand(command);
      LogHelper.Debug("Ответ DevicesConnector:\r\n" + command.AnswerString);
      DevicesConnectorDriver.Statuses result;
      int num;
      DevicesConnectorDriver.Statuses[] statusesArray;
      do
      {
        Thread.Sleep(100);
        result = this.GetResult(command);
        num = (int) result;
        statusesArray = new DevicesConnectorDriver.Statuses[2]
        {
          DevicesConnectorDriver.Statuses.Run,
          DevicesConnectorDriver.Statuses.Wait
        };
      }
      while (((DevicesConnectorDriver.Statuses) num).IsEither<DevicesConnectorDriver.Statuses>(statusesArray));
      if (result == DevicesConnectorDriver.Statuses.Error)
        throw new DeviceException(Translate.KkmServerDriver_Ошибка_выполнения_команды_на_ККМ_Сервер__ + command.AnswerString);
    }

    private DevicesConnectorDriver.Statuses GetResult(
      DevicesConnectorDriver.DevicesConnectorCommand command)
    {
      DevicesConnectorDriver.GetResultCommand getResultCommand = new DevicesConnectorDriver.GetResultCommand();
      getResultCommand.CommandId = command.CommandId;
      DevicesConnectorDriver.GetResultCommand command1 = getResultCommand;
      this.DoCommand((DevicesConnectorDriver.DevicesConnectorCommand) command1);
      switch (command1.Result.Status)
      {
        case DevicesConnectorDriver.Statuses.Ok:
          command.AnswerString = command1.AnswerString;
          return command1.Result.Status;
        case DevicesConnectorDriver.Statuses.Error:
          command.AnswerString = command1.AnswerString;
          return command1.Result.Status;
        case DevicesConnectorDriver.Statuses.Wait:
        case DevicesConnectorDriver.Statuses.Run:
          return command1.Result.Status;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void DoCommand(
      DevicesConnectorDriver.DevicesConnectorCommand command)
    {
      try
      {
        string str1 = this._lanConnection.UrlAddress;
        if (!str1.ToLower().StartsWith("http://"))
          str1 = "http://" + str1;
        string str2 = str1 + ":" + this._lanConnection.PortNumber.ToString();
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(!(command.HttpMethod == "POST") ? str2 + command.Method : str2 + "/addCommand");
        httpWebRequest.Method = command.HttpMethod;
        httpWebRequest.Timeout = 120000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        if (command.HttpMethod == "POST")
          this.Post(command, httpWebRequest);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.AnswerString = streamReader.ReadToEnd();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки команды на DevicesConnector", false);
        throw new WebException(Translate.DevicesConnectorDriver_Ошибка_отправки_команды_на_DevicesConnector);
      }
    }

    private void Post(
      DevicesConnectorDriver.DevicesConnectorCommand command,
      HttpWebRequest httpWebRequest)
    {
      httpWebRequest.ContentType = "application/json";
      string message = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore
      });
      LogHelper.Debug(message);
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        streamWriter.Write(message);
    }

    public abstract class DevicesConnectorCommand
    {
      [Newtonsoft.Json.JsonIgnore]
      public virtual string HttpMethod { get; }

      [Newtonsoft.Json.JsonIgnore]
      public virtual string Method { get; }

      [Newtonsoft.Json.JsonIgnore]
      public string AnswerString { get; set; }

      public Guid CommandId { get; set; } = Guid.NewGuid();

      public virtual int CommandType { get; }

      public Guid DeviceId { get; set; }
    }

    public class DevicesConnectorAnswer
    {
      [JsonProperty("commandId")]
      public Guid CommandId { get; set; }

      [JsonProperty("status")]
      public DevicesConnectorDriver.Statuses Status { get; set; }

      [JsonProperty("deviceId")]
      public Guid DeviceId { get; set; }
    }

    public class DevicesConnectorAnswer<T> : DevicesConnectorDriver.DevicesConnectorAnswer
    {
      [JsonProperty("data")]
      public T Data { get; set; }
    }

    public class KkmGetStatus : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 1;

      public override string HttpMethod => "POST";

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer<DevicesConnectorDriver.KkmGetStatus.KkmStatus> Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer<DevicesConnectorDriver.KkmGetStatus.KkmStatus>>(this.AnswerString);
        }
      }

      public class KkmStatus
      {
        public DevicesConnectorDriver.Enums.CheckStatuses CheckStatus { get; set; }

        public DevicesConnectorDriver.Enums.SessionStatuses SessionStatus { get; set; }

        public DevicesConnectorDriver.Enums.KkmStatuses KkmState { get; set; }

        public Version DriverVersion { get; set; } = new Version("1.0.0.0");

        public int CheckNumber { get; set; } = 1;

        public int SessionNumber { get; set; } = 1;

        public DateTime? SessionStarted { get; set; }

        public Decimal CashSum { get; set; }

        public string FactoryNumber { get; set; }

        public string SoftwareVersion { get; set; }

        public string Model { get; set; }

        public DateTime FnDateEnd { get; set; }

        public 
        #nullable enable
        DevicesConnectorDriver.KkmGetStatus.KkmStatus.RuKkm? RuKkmInfo { get; set; }

        public class RuKkm
        {
          public DateTime OfdLastSendDateTime { get; set; }

          public int OfdNotSendDocuments { get; set; }
        }
      }
    }

    public class GetResultCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override 
      #nullable disable
      string Method => string.Format("/getResult/{0}", (object) this.CommandId);

      public override string HttpMethod => "GET";

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class KkmOpenSessionCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 2;

      public override string HttpMethod => "POST";

      [JsonProperty("Cashier")]
      public DevicesConnectorDriver.Cashier Cashier { get; set; }

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class CancelFiscalReceiptCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 6;

      public override string HttpMethod => "POST";

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class CutPaperCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 7;

      public override string HttpMethod => "POST";

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class OpenCashBoxCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 8;

      public override string HttpMethod => "POST";

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class KkmGetReportCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 4;

      public override string HttpMethod => "POST";

      public DevicesConnectorDriver.Enums.ReportTypes ReportType { get; set; }

      [JsonProperty("Cashier")]
      public DevicesConnectorDriver.Cashier Cashier { get; set; }

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class KkmCashInOutCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 3;

      public override string HttpMethod => "POST";

      public Decimal Sum { get; set; }

      [JsonProperty("Cashier")]
      public DevicesConnectorDriver.Cashier Cashier { get; set; }

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }
    }

    public class PrintFiscalReceiptCommand : DevicesConnectorDriver.DevicesConnectorCommand
    {
      public override int CommandType => 5;

      public override string HttpMethod => "POST";

      [JsonProperty("ReceiptData")]
      public DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptData Receipt { get; set; }

      [Newtonsoft.Json.JsonIgnore]
      public DevicesConnectorDriver.DevicesConnectorAnswer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<DevicesConnectorDriver.DevicesConnectorAnswer>(this.AnswerString);
        }
      }

      public class ReceiptData
      {
        public DevicesConnectorDriver.Enums.ReceiptFiscalTypes FiscalType { get; set; }

        public DevicesConnectorDriver.Enums.ReceiptOperationTypes OperationType { get; set; }

        public DevicesConnectorDriver.Cashier Cashier { get; set; }

        public 
        #nullable enable
        DevicesConnectorDriver.Contractor? Contractor { get; set; }

        public 
        #nullable disable
        List<DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItem> Items { get; set; } = new List<DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItem>();

        public List<DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment> Payments { get; set; } = new List<DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment>();

        public DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptExtraData CountrySpecificData { get; set; }

        public bool IsPrintReceipt { get; set; } = true;
      }

      public class ReceiptItem
      {
        public string Name { get; set; }

        public 
        #nullable enable
        string? Barcode { get; set; }

        public Decimal Price { get; set; }

        public Decimal Discount { get; set; }

        public Decimal Quantity { get; set; }

        public 
        #nullable disable
        DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData CountrySpecificData { get; set; }

        public 
        #nullable enable
        string? Comment { get; set; }

        public int? TaxRateIndex { get; set; }

        public int DepartmentIndex { get; set; } = 1;

        public Decimal DiscountSum
        {
          get
          {
            return Math.Round(this.Discount / 100M * this.Price * this.Quantity, 2, MidpointRounding.AwayFromZero);
          }
        }
      }

      public class ReceiptExtraData
      {
        public string? DigitalReceiptAddress { get; set; }

        public int TaxVariantIndex { get; set; }
      }

      public class ReceiptItemData
      {
        public 
        #nullable disable
        DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData.RuFfdInfo FfdData { get; set; }

        public DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData.RuMarkingInfo MarkingInfo { get; set; }

        public class RuFfdInfo
        {
          public DevicesConnectorDriver.Enums.FfdUnitsIndex Unit { get; set; }

          public DevicesConnectorDriver.Enums.FfdCalculationSubjects Subject { get; set; }

          public DevicesConnectorDriver.Enums.FfdCalculationMethods Method { get; set; }
        }

        public class RuMarkingInfo
        {
          public DevicesConnectorDriver.Enums.EstimatedStatus EstimatedStatus { get; set; } = DevicesConnectorDriver.Enums.EstimatedStatus.PieceSold;

          public int ValidationResultKkm { get; set; }

          public string RawCode { get; set; }
        }
      }

      public class ReceiptPayment
      {
        public Decimal Sum { get; set; }

        public int MethodIndex { get; set; }
      }
    }

    [System.Text.Json.Serialization.JsonConverter(typeof (JsonStringEnumConverter))]
    public enum Statuses
    {
      Ok,
      Error,
      Wait,
      Run,
    }

    public class Enums
    {
      public enum KkmTypes
      {
        Atol8 = 101, // 0x00000065
        Atol10 = 102, // 0x00000066
        AtolWebServer = 103, // 0x00000067
        ShtrihM = 104, // 0x00000068
        VikiPrint = 105, // 0x00000069
        Mercury = 106, // 0x0000006A
        KkmServer = 107, // 0x0000006B
        PortDriverRu = 108, // 0x0000006C
      }

      public enum CommandTypes
      {
        GetStatus = 1,
        OpenSession = 2,
        CashInOut = 3,
        DoReport = 4,
        PrintFiscalReceipt = 5,
        CancelFiscalReceipt = 6,
        CutPaper = 7,
        OpenCashBox = 8,
      }

      public enum ReportTypes
      {
        ZReport,
        XReport,
        XReportWithGoods,
      }

      public enum ReceiptFiscalTypes
      {
        Fiscal,
        NonFiscal,
        Service,
      }

      public enum ReceiptOperationTypes
      {
        Sale,
        ReturnSale,
        Buy,
        ReturnBuy,
      }

      public enum SessionStatuses
      {
        Unknown,
        Open,
        OpenMore24Hours,
        Close,
      }

      public enum CheckStatuses
      {
        Unknown,
        Open,
        Close,
      }

      public enum KkmStatuses
      {
        Unknown,
        Ready,
        NoPaper,
        OfdDocumentsToMany,
        CoverOpen,
        HardwareError,
        NeedToContinuePrint,
      }

      public enum FFdVersions
      {
        Offline,
        Ffd100,
        Ffd105,
        Ffd110,
        Ffd120,
      }

      public enum OfdAttributes
      {
        ClientEmailPhone = 1008, // 0x000003F0
        CashierName = 1021, // 0x000003FD
        TaxSystem = 1055, // 0x0000041F
        CashierInn = 1203, // 0x000004B3
        ClientName = 1227, // 0x000004CB
        ClientInn = 1228, // 0x000004CC
        UnitCode = 2108, // 0x0000083C
      }

      public enum ErrorTypes
      {
        Unknown = -1, // 0xFFFFFFFF
        NeedService = 1,
        NoPaper = 2,
        SessionMore24Hour = 3,
        UnCorrectPaymentIndex = 4,
        NoConnection = 5,
        NonCorrectData = 6,
        PortBusy = 7,
        CoverOpen = 8,
        TooManyOfflineDocuments = 9,
        UnCorrectDateTime = 10, // 0x0000000A
        ConnectionError = 11, // 0x0000000B
      }

      public enum FfdCalculationMethods
      {
        None,
        PrePaymentFull,
        Prepayment,
        AdvancePayment,
        FullPayment,
        PartPaymentAndCredit,
        FullCredit,
        PaymentForCredit,
      }

      public enum FfdCalculationSubjects
      {
        None = 0,
        SimpleGood = 1,
        ExcisableGood = 2,
        Work = 3,
        Service = 4,
        GamePayment = 5,
        GameWin = 6,
        LotteryPayment = 7,
        LotteryWin = 8,
        Rid = 9,
        Payment = 10, // 0x0000000A
        AgentPayment = 11, // 0x0000000B
        WithPayment = 12, // 0x0000000C
        Other = 13, // 0x0000000D
        PropertyLaw = 14, // 0x0000000E
        NonOperatingIncome = 15, // 0x0000000F
        OtherPayment = 16, // 0x00000010
        TradeFee = 17, // 0x00000011
        ResortFee = 18, // 0x00000012
        Deposit = 19, // 0x00000013
        Expenditure = 20, // 0x00000014
        PensionInsuranceIP = 21, // 0x00000015
        PensionInsurance = 22, // 0x00000016
        MedicalInsuranceIP = 23, // 0x00000017
        MedicalInsurance = 24, // 0x00000018
        SocialInsurance = 25, // 0x00000019
        CasinoPayment = 26, // 0x0000001A
        OutOfFunds = 27, // 0x0000001B
        Atnm = 30, // 0x0000001E
        Atm = 31, // 0x0000001F
        Tnm = 32, // 0x00000020
        Tm = 33, // 0x00000021
      }

      public enum FfdUnitsIndex
      {
        Pieces = 0,
        Gram = 10, // 0x0000000A
        Kilogram = 11, // 0x0000000B
        Ton = 12, // 0x0000000C
        Centimeter = 20, // 0x00000014
        Decimeter = 21, // 0x00000015
        Meter = 22, // 0x00000016
        SquareCentimeter = 30, // 0x0000001E
        SquareDecimeter = 31, // 0x0000001F
        SquareMeter = 32, // 0x00000020
        Milliliter = 40, // 0x00000028
        Liter = 41, // 0x00000029
        CubicMeter = 42, // 0x0000002A
        KilowattHour = 50, // 0x00000032
        Gigacaloria = 51, // 0x00000033
        Day = 70, // 0x00000046
        Hour = 71, // 0x00000047
        Minute = 72, // 0x00000048
        Second = 73, // 0x00000049
        Kilobyte = 80, // 0x00000050
        Megabyte = 81, // 0x00000051
        Gigabyte = 82, // 0x00000052
        Terabyte = 83, // 0x00000053
      }

      public enum EstimatedStatus
      {
        PieceSold = 1,
        DryForSale = 2,
        PieceReturn = 3,
        DryReturn = 4,
        StatusUnchanged = 255, // 0x000000FF
      }
    }

    public class Cashier
    {
      [JsonPropertyName("Name")]
      public string Name { get; set; }

      [JsonPropertyName("TaxId")]
      public 
      #nullable enable
      string? TaxId { get; set; }

      public Cashier()
      {
      }

      public Cashier(
      #nullable disable
      Gbs.Core.Devices.CheckPrinters.Cashier cashier)
      {
        this.Name = cashier.Name;
        this.TaxId = cashier.Inn;
      }
    }

    public class Contractor
    {
      public string Name { get; set; }

      public string TaxId { get; set; }

      public string Email { get; set; }

      public string Phone { get; set; }

      public Contractor()
      {
      }

      public Contractor(Client client)
      {
        this.Name = client.Name;
        this.Phone = client.Phone;
        this.TaxId = client.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString();
        this.Email = client.Email;
      }
    }
  }
}
