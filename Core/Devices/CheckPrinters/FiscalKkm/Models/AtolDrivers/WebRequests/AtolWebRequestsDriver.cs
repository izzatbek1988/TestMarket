// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolWebRequestsDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.AtolDrivers.WebRequests;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class AtolWebRequestsDriver
  {
    private readonly Gbs.Core.Config.Devices _settingDevice;
    private RestHelper _restHelper;
    private AtolWebRequestsDriver.CommandToQueue _currentCommand;

    public AtolWebRequestsDriver(Gbs.Core.Config.Devices settingDevice)
    {
      this._settingDevice = settingDevice;
    }

    public void DoCommand(AtolWebRequestsDriver.AtolRequest command)
    {
      this._currentCommand = new AtolWebRequestsDriver.CommandToQueue()
      {
        Requests = new List<AtolWebRequestsDriver.AtolRequest>()
        {
          command
        }
      };
      this._restHelper = new RestHelper(this._settingDevice.CheckPrinter.Connection.LanPort.UrlAddress, new int?(this._settingDevice.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault(16732)), this._currentCommand.ToJsonString(true));
      this._restHelper.CreateCommand("/api/v2/requests", TypeRestRequest.Post);
      this.SetDeviceId();
      this.GetAuthorization();
      this._restHelper.DoCommand();
      this._currentCommand.AnswerString = this._restHelper.Answer;
      this.CheckError(this._currentCommand.Result.Error);
      this.GetStatusCommand();
      command.AnswerString = this._currentCommand.AnswerString;
    }

    private void GetAuthorization()
    {
      if (this._settingDevice.CheckPrinter.Connection.LanPort.UserLogin.IsNullOrEmpty())
        return;
      this._restHelper.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format(Translate.AtolWebRequestsDriver_GetAuthorization__0___1_, (object) this._settingDevice.CheckPrinter.Connection.LanPort.UserLogin, (object) this._settingDevice.CheckPrinter.Connection.LanPort.Password))));
    }

    private void SetDeviceId()
    {
      if (this._settingDevice.CheckPrinter.FiscalKkm.Model.IsNullOrEmpty())
        return;
      this._restHelper.AddHeader("deviceID", this._settingDevice.CheckPrinter.FiscalKkm.Model);
    }

    private void CheckError(AtolWebRequestsDriver.Error error)
    {
      if (error != null && error.Code != 0)
        throw new DeviceException(string.Format("{0} (код ошибки: {1})", (object) error.Description, (object) error.Code));
    }

    private void GetStatusCommand()
    {
      this._restHelper.CreateCommand("/api/v2/requests/" + this._currentCommand.Uid.ToString(), TypeRestRequest.Get);
      this.SetDeviceId();
      this.GetAuthorization();
      this._restHelper.DoCommand();
      AtolWebRequestsDriver.StatusProcessCommand statusProcessCommand1 = new AtolWebRequestsDriver.StatusProcessCommand();
      statusProcessCommand1.AnswerString = this._restHelper.Answer;
      AtolWebRequestsDriver.StatusProcessCommand statusProcessCommand2 = statusProcessCommand1;
      this.CheckError(statusProcessCommand2.Result.Processes.Single<AtolWebRequestsDriver.StatusProcessCommand.Answer.ResultProcess>().Error);
      if (statusProcessCommand2.Result.Processes.Single<AtolWebRequestsDriver.StatusProcessCommand.Answer.ResultProcess>().Status.IsEither<ResultProcessCommand>(ResultProcessCommand.InProgress, ResultProcessCommand.Wait))
      {
        Thread.Sleep(300);
        this.GetStatusCommand();
      }
      statusProcessCommand2.AnswerString = this._restHelper.Answer;
      this._currentCommand.AnswerString = statusProcessCommand2.Result.Processes.Single<AtolWebRequestsDriver.StatusProcessCommand.Answer.ResultProcess>().Result.ToJsonString(true);
    }

    public class FiscalDocument : AtolWebRequestsDriver.AtolRequest
    {
      [JsonProperty("operator")]
      public AtolWebRequestsDriver.Cashier Cashier { get; set; }

      [JsonProperty("clientInfo")]
      public AtolWebRequestsDriver.Client Client { get; set; }

      [JsonProperty("type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public DocumentTypeEnum DocumentType { get; set; }

      [JsonProperty("ignoreNonFiscalPrintErrors")]
      public bool IgnoreNonFiscalPrintErrors { get; set; } = true;

      [JsonProperty("validateMarkingCodes")]
      public bool? ValidateMarkingCodes { get; set; }

      [JsonProperty("electronically")]
      public bool Electronically { get; set; } = true;

      [JsonProperty("taxationType")]
      [JsonConverter(typeof (StringEnumConverter))]
      public TaxSystemTypeEnum TaxationSystemType { get; set; }

      [JsonProperty("paymentsPlace")]
      public string PaymentsPlace { get; set; }

      [JsonProperty("paymentsAddress")]
      public string PaymentsAddress { get; set; }

      [JsonProperty("machineNumber")]
      public string MachineNumber { get; set; }

      [JsonProperty("total")]
      public string Total { get; set; }

      [JsonProperty("items")]
      public List<AtolWebRequestsDriver.Item> Items { get; set; } = new List<AtolWebRequestsDriver.Item>();

      [JsonProperty("payments")]
      public List<AtolWebRequestsDriver.FiscalDocument.Payment> Payments { get; set; } = new List<AtolWebRequestsDriver.FiscalDocument.Payment>();

      public class Position : AtolWebRequestsDriver.Item
      {
        public override ItemType Type => ItemType.Position;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("quantity")]
        public Decimal Quantity { get; set; }

        [JsonProperty("amount")]
        public Decimal Amount { get; set; }

        [JsonProperty("infoDiscountAmount")]
        public Decimal InfoDiscountAmount { get; set; }

        [JsonProperty("department")]
        public int Department { get; set; }

        [JsonProperty("measurementUnit")]
        public int Unit { get; set; }

        [JsonProperty("piece")]
        public bool Piece { get; set; }

        [JsonProperty("paymentMethod")]
        [JsonConverter(typeof (StringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [JsonProperty("paymentObject")]
        public string PaymentObject { get; set; }

        [JsonProperty("tax")]
        public AtolWebRequestsDriver.Tax Tax { get; set; }

        [JsonProperty("imcParams")]
        public AtolWebRequestsDriver.MarkingCode MarkingInfo { get; set; }

        [JsonProperty("industryInfo")]
        public List<AtolWebRequestsDriver.IndustryInfo> IndustryInfos { get; set; }
      }

      public class Payment
      {
        [JsonProperty("type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public PaymentType Type { get; set; }

        [JsonProperty("sum")]
        public Decimal Sum { get; set; }

        [JsonProperty("printItems")]
        public List<AtolWebRequestsDriver.PrintNonFiscal.Text> PrintItems { get; set; }
      }
    }

    public class ValidationMarkingCode : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "beginMarkingCodeValidation";

      [JsonProperty("params")]
      public AtolWebRequestsDriver.MarkingCode MarkingCode { get; set; }

      [JsonIgnore]
      public AtolWebRequestsDriver.ValidationMarkingCode.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.ValidationMarkingCode.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("fmCheck")]
        public bool IsFnCheck { get; set; }

        [JsonProperty("fmCheckResult")]
        public bool FnCheckResult { get; set; }

        [JsonProperty("fmCheckErrorReason")]
        [JsonConverter(typeof (StringEnumConverter))]
        public CheckFnMarkedErrorEnum ErrorReason { get; set; }
      }
    }

    public class StatusValidationMarkingCode : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getMarkingCodeValidationStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.StatusValidationMarkingCode.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.StatusValidationMarkingCode.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("ready")]
        public bool IsReady { get; set; }

        [JsonProperty("sentImcRequest")]
        public bool IsSentImcRequest { get; set; }

        [JsonProperty("onlineValidation")]
        public AtolWebRequestsDriver.StatusValidationMarkingCode.ResultIOnlineValidation OnlineValidation { get; set; }
      }

      public class ResultIOnlineValidation
      {
        [JsonProperty("itemInfoCheckResult")]
        public AtolWebRequestsDriver.ResultCheckMarkingCode Result { get; set; }
      }
    }

    public class AcceptMarkingCode : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "acceptMarkingCode";
    }

    public class CancelMarkingCodeValidation : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "cancelMarkingCodeValidation";
    }

    public class ClearMarkingCodeValidationResult : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "clearMarkingCodeValidationResult";
    }

    public class ContinuePrint : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "continuePrint";
    }

    public class XReport : AtolWebRequestsDriver.AtolRequest
    {
      [JsonProperty("operator")]
      public AtolWebRequestsDriver.Cashier Cashier { get; set; }

      public override string Type => "reportX";
    }

    public class ZReport : AtolWebRequestsDriver.AtolRequest
    {
      [JsonProperty("operator")]
      public AtolWebRequestsDriver.Cashier Cashier { get; set; }

      public override string Type => "closeShift";
    }

    public class OpenShift : AtolWebRequestsDriver.AtolRequest
    {
      [JsonProperty("operator")]
      public AtolWebRequestsDriver.Cashier Cashier { get; set; }

      public override string Type => "openShift";
    }

    public class OpenCashDrawer : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "openCashDrawer";
    }

    public class GetDeviceInfo : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getDeviceInfo";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetDeviceInfo.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetDeviceInfo.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("deviceInfo")]
        public AtolWebRequestsDriver.GetDeviceInfo.Answer.DeviceInfo Info { get; set; }

        public class DeviceInfo
        {
          [JsonProperty("model")]
          public int Model { get; set; }

          [JsonProperty("modelName")]
          public string ModelName { get; set; }

          [JsonProperty("serial")]
          public string Serial { get; set; }

          [JsonProperty("firmwareVersion")]
          public string FirmwareVersion { get; set; }

          [JsonProperty("configurationVersion")]
          public string ConfigurationVersion { get; set; }

          [JsonProperty("receiptLineLength")]
          public int ReceiptLineLength { get; set; }

          [JsonProperty("receiptLineLengthPix")]
          public int ReceiptLineLengthPix { get; set; }

          [JsonProperty("ffdVersion")]
          public string FfdVersion { get; set; }

          [JsonProperty("fnFfdVersion")]
          public string FnFfdVersion { get; set; }
        }
      }
    }

    public class GetDeviceStatus : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getDeviceStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetDeviceStatus.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetDeviceStatus.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("deviceStatus")]
        public AtolWebRequestsDriver.GetDeviceStatus.Answer.DeviceStatus Status { get; set; }

        public class DeviceStatus
        {
          [JsonProperty("shift")]
          [JsonConverter(typeof (StringEnumConverter))]
          public ShiftStatusEnum ShiftStatus { get; set; }

          [JsonProperty("currentDateTime")]
          public DateTime CurrentDateTime { get; set; }

          [JsonProperty("blocked")]
          public bool Blocked { get; set; }

          [JsonProperty("coverOpened")]
          public bool CoverOpened { get; set; }

          [JsonProperty("paperPresent")]
          public bool PaperPresent { get; set; }

          [JsonProperty("fiscal")]
          public bool Fiscal { get; set; }

          [JsonProperty("fnFiscal")]
          public bool FnFiscal { get; set; }

          [JsonProperty("fnPresent")]
          public bool FnPresent { get; set; }

          [JsonProperty("cashDrawerOpened")]
          public bool CashDrawerOpened { get; set; }
        }
      }
    }

    public class GetShiftStatus : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getShiftStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetShiftStatus.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetShiftStatus.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("shiftStatus")]
        public AtolWebRequestsDriver.GetShiftStatus.Answer.ShiftStatus Status { get; set; }

        public class ShiftStatus
        {
          [JsonProperty("state")]
          [JsonConverter(typeof (StringEnumConverter))]
          public ShiftStatusEnum Status { get; set; }

          [JsonProperty("documentsCount")]
          public int DocumentsCount { get; set; }

          [JsonProperty("expiredTime")]
          public DateTime ExpiredTime { get; set; }

          [JsonProperty("number")]
          public int Number { get; set; }
        }
      }
    }

    public class GetFnStatus : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getFnStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetFnStatus.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetFnStatus.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("fnStatus")]
        public AtolWebRequestsDriver.GetFnStatus.Answer.FnStatus Status { get; set; }

        public class FnStatus
        {
          [JsonProperty("warnings")]
          public AtolWebRequestsDriver.GetFnStatus.Answer.FnWarning Warnings { get; set; }

          [JsonProperty("fiscalDocumentNumber")]
          public int FiscalDocumentNumber { get; set; }

          [JsonProperty("fiscalReceiptNumber")]
          public int FiscalReceiptNumber { get; set; }
        }

        public class FnWarning
        {
          [JsonProperty("memoryOverflow")]
          public bool MemoryOverflow { get; set; }

          [JsonProperty("needReplacement")]
          public bool NeedReplacement { get; set; }

          [JsonProperty("ofdTimeout")]
          public bool OfdTimeout { get; set; }

          [JsonProperty("resourceExhausted")]
          public bool ResourceExhausted { get; set; }

          [JsonProperty("criticalError")]
          public bool CriticalError { get; set; }
        }
      }
    }

    public class GetFnInfo : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getFnInfo";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetFnInfo.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetFnInfo.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("fnInfo")]
        public AtolWebRequestsDriver.GetFnInfo.Answer.FnInfo Info { get; set; }

        public class FnInfo
        {
          [JsonProperty("serial")]
          public string Serial { get; set; }

          [JsonProperty("version")]
          public string Version { get; set; }

          [JsonProperty("execution")]
          public string Execution { get; set; }

          [JsonProperty("numberOfRegistrations")]
          public int NumberOfRegistrations { get; set; }

          [JsonProperty("registrationsRemaining")]
          public int RegistrationsRemaining { get; set; }

          [JsonProperty("validityDate")]
          public DateTime ValidityDate { get; set; }

          [JsonProperty("ffdVersion")]
          public string FfdVersion { get; set; }

          [JsonProperty("fnFfdVersion")]
          public string FnFfdVersion { get; set; }
        }
      }
    }

    public class GetOfdStatus : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "ofdExchangeStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetOfdStatus.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetOfdStatus.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("status")]
        public AtolWebRequestsDriver.GetOfdStatus.Answer.OfdStatus Status { get; set; }

        [JsonProperty("errors")]
        public AtolWebRequestsDriver.GetOfdStatus.Answer.OfdError Errors { get; set; }

        public class OfdStatus
        {
          [JsonProperty("notSentCount")]
          public int NotSentCount { get; set; }

          [JsonProperty("notSentFirstDocNumber")]
          public int NotSentFirstDocNumber { get; set; }

          [JsonProperty("notSentFirstDocDateTime")]
          public DateTime NotSentFirstDocDateTime { get; set; }

          [JsonProperty("lastSuccessKeysUpdate")]
          public DateTime LastSuccessKeysUpdate { get; set; }
        }

        public class OfdError
        {
          [JsonProperty("lastSuccessConnectionDateTime")]
          public DateTime LastSuccessConnectionDateTime { get; set; }
        }
      }
    }

    public class GetCashSum : AtolWebRequestsDriver.AtolRequest
    {
      public override string Type => "getCashDrawerStatus";

      [JsonIgnore]
      public AtolWebRequestsDriver.GetCashSum.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.GetCashSum.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("cashDrawerStatus")]
        public AtolWebRequestsDriver.GetCashSum.Answer.СashDrawerStatus DrawerStatus { get; set; }

        [JsonProperty("counters")]
        public AtolWebRequestsDriver.GetCashSum.Answer.СashDrawerInfo Counters { get; set; }

        public class СashDrawerStatus
        {
          [JsonProperty("cashDrawerOpened")]
          public bool IsOpened { get; set; }
        }

        public class СashDrawerInfo
        {
          [JsonProperty("cashSum")]
          public Decimal CashSum { get; set; }
        }
      }
    }

    public class PrintNonFiscal : AtolWebRequestsDriver.AtolRequest
    {
      [JsonProperty("items")]
      public List<AtolWebRequestsDriver.Item> Items { get; set; }

      public override string Type => "nonFiscal";

      public class Text : AtolWebRequestsDriver.Item
      {
        [JsonProperty("text")]
        public string Value { get; set; }

        [JsonProperty("alignment")]
        [JsonConverter(typeof (StringEnumConverter))]
        public AlignmentEnum Alignment { get; set; }

        [JsonProperty("wrap")]
        [JsonConverter(typeof (StringEnumConverter))]
        public WrapEnum Wrap { get; set; }

        [JsonProperty("font")]
        public int Font { get; set; }

        [JsonProperty("doubleWidth")]
        public bool DoubleWidth { get; set; }

        [JsonProperty("doubleHeight")]
        public bool DoubleHeight { get; set; }

        [JsonProperty("storeInJournal")]
        public bool? StoreInJournal { get; set; }

        public override ItemType Type => ItemType.Text;
      }

      public class Barcode : AtolWebRequestsDriver.Item
      {
        [JsonProperty("barcode")]
        public string BarcodeText { get; set; }

        [JsonProperty("barcodeType")]
        [JsonConverter(typeof (StringEnumConverter))]
        public AtolWebRequestsDriver.PrintNonFiscal.Barcode.BarcodeTypeEnum BarcodeType { get; set; }

        [JsonProperty("alignment")]
        [JsonConverter(typeof (StringEnumConverter))]
        public AlignmentEnum Alignment { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; } = 1;

        [JsonProperty("height")]
        public int Height { get; set; } = 30;

        [JsonProperty("printText")]
        public bool PrintText { get; set; }

        [JsonProperty("storeInJournal")]
        public bool? StoreInJournal { get; set; }

        public override ItemType Type => ItemType.Barcode;

        public enum BarcodeTypeEnum
        {
          [EnumMember(Value = "EAN8")] Ean8,
          [EnumMember(Value = "EAN13")] Ean13,
          [EnumMember(Value = "CODE128")] Code128,
          [EnumMember(Value = "GS1_128")] Gs1_128,
          [EnumMember(Value = "QR")] Qr,
        }
      }
    }

    public class OperationСash : AtolWebRequestsDriver.AtolRequest
    {
      [JsonIgnore]
      public AtolWebRequestsDriver.OperationСash.TypeOperation Operation { get; set; }

      [JsonProperty("operator")]
      public AtolWebRequestsDriver.Cashier Cashier { get; set; }

      [JsonProperty("cashSum")]
      public Decimal СashSum { get; set; }

      public override string Type
      {
        get
        {
          return this.Operation != AtolWebRequestsDriver.OperationСash.TypeOperation.In ? "cashOut" : "cashIn";
        }
      }

      public enum TypeOperation
      {
        In,
        Out,
      }
    }

    public class Error
    {
      [JsonProperty("code")]
      public int Code { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }
    }

    public class Cashier
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("vatin")]
      public string Inn { get; set; }

      public Cashier(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
      {
        this.Name = cashier.Name;
        this.Inn = cashier.Inn;
      }
    }

    public class MarkingCode
    {
      [JsonProperty("imc")]
      public string Code { get; set; }

      [JsonProperty("imcType")]
      [JsonConverter(typeof (StringEnumConverter))]
      public MarkingCodeTypeEnum Type { get; set; }

      [JsonProperty("itemEstimatedStatus")]
      [JsonConverter(typeof (StringEnumConverter))]
      public MarkingCodeStatusEnum Status { get; set; }

      [JsonProperty("itemQuantity")]
      public Decimal? Quantity { get; set; }

      [JsonProperty("itemUnits")]
      public int? Unit { get; set; }

      [JsonProperty("imcModeProcessing")]
      public int ModeProcessing { get; set; }

      [JsonProperty("itemFractionalAmount")]
      public string FractionalAmount { get; set; }

      [JsonProperty("imcBarcode")]
      public string Barcode { get; set; }

      [JsonProperty("itemInfoCheckResult")]
      public AtolWebRequestsDriver.ResultCheckMarkingCode Result { get; set; }

      public MarkingCode(CheckGood good, CheckTypes type, bool forPosition)
      {
        this.Code = RuOnlineKkmHelper.Base64Encode(DataMatrixHelper.ReplaceSomeCharsToFNC1(good.MarkedInfo.FullCode));
        this.Type = MarkingCodeTypeEnum.Auto;
        this.ModeProcessing = 0;
        MarkingCodeStatusEnum markingCodeStatusEnum;
        switch (RuOnlineKkmHelper.GetMarkingCodeStatus(good, type))
        {
          case 1:
            markingCodeStatusEnum = MarkingCodeStatusEnum.ItemPieceSold;
            break;
          case 2:
            markingCodeStatusEnum = MarkingCodeStatusEnum.ItemDryForSale;
            break;
          case 3:
            markingCodeStatusEnum = MarkingCodeStatusEnum.ItemPieceReturn;
            break;
          case 4:
            markingCodeStatusEnum = MarkingCodeStatusEnum.ItemDryReturn;
            break;
          default:
            markingCodeStatusEnum = MarkingCodeStatusEnum.ItemStatusUnchanged;
            break;
        }
        this.Status = markingCodeStatusEnum;
        if (forPosition)
          return;
        if (!this.Status.IsEither<MarkingCodeStatusEnum>(MarkingCodeStatusEnum.ItemDryForSale, MarkingCodeStatusEnum.ItemDryReturn))
          return;
        this.Quantity = new Decimal?(good.Quantity);
        GoodsUnits.GoodUnit unit = good.Unit;
        this.Unit = new int?(unit != null ? unit.RuFfdUnitsIndex : 0);
      }
    }

    public class IndustryInfo
    {
      [JsonProperty("date")]
      public string Date { get; set; }

      [JsonProperty("fois")]
      public string Fois { get; set; }

      [JsonProperty("number")]
      public string Number { get; set; }

      [JsonProperty("industryAttribute")]
      public string IndustryAttribute { get; set; }
    }

    public class ResultCheckMarkingCode
    {
      [JsonProperty("imcCheckFlag")]
      public bool IsCheckedFn { get; set; }

      [JsonProperty("imcCheckResult")]
      public bool IsPositiveResult { get; set; }

      [JsonProperty("imcStatusInfo")]
      public bool IsCheckedIsm { get; set; }

      [JsonProperty("imcEstimatedStatusCorrect")]
      public bool IsEstimatedStatusCorrect { get; set; }

      [JsonProperty("ecrStandAloneFlag")]
      public bool IsStandAlon { get; set; }
    }

    public class Tax
    {
      [JsonProperty("type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public TaxTypeEnum Type { get; set; }

      [JsonProperty("sum")]
      public Decimal? Sum { get; set; }
    }

    public class Client
    {
      [JsonProperty("emailOrPhone")]
      public string EmailOrPhone { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("vatin")]
      public string Inn { get; set; }

      [JsonProperty("birthDate")]
      public string BirthDate { get; set; }

      [JsonProperty("citizenship")]
      public string Citizenship { get; set; }

      [JsonProperty("identityDocumentCode")]
      public string IdentityDocumentCode { get; set; }

      [JsonProperty("identityDocumentData")]
      public string IdentityDocumentData { get; set; }

      [JsonProperty("address")]
      public string Address { get; set; }

      public Client(ClientAdnSum client)
      {
        this.Name = client.Client.Name;
        this.Inn = client.Client.GetInn();
        this.Address = client.Client.Address;
        DateTime? birthday = client.Client.Birthday;
        ref DateTime? local = ref birthday;
        this.BirthDate = local.HasValue ? local.GetValueOrDefault().ToString("dd.MM.yyyy") : (string) null;
      }

      public Client()
      {
      }
    }

    public abstract class Item
    {
      [JsonProperty("type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public virtual ItemType Type { get; }
    }

    private class CommandToQueue : AtolWebRequestsDriver.ServiceRequest
    {
      [JsonProperty("uuid")]
      public Guid Uid { get; set; } = Guid.NewGuid();

      [JsonProperty("request")]
      public List<AtolWebRequestsDriver.AtolRequest> Requests { get; set; } = new List<AtolWebRequestsDriver.AtolRequest>();

      [JsonIgnore]
      public AtolWebRequestsDriver.CommandToQueue.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.CommandToQueue.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("uuid")]
        public Guid Uid { get; set; } = Guid.NewGuid();

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("error")]
        public AtolWebRequestsDriver.Error Error { get; set; }
      }
    }

    private class StatusProcessCommand : AtolWebRequestsDriver.ServiceRequest
    {
      [JsonIgnore]
      public AtolWebRequestsDriver.StatusProcessCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<AtolWebRequestsDriver.StatusProcessCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("results")]
        public List<AtolWebRequestsDriver.StatusProcessCommand.Answer.ResultProcess> Processes { get; set; } = new List<AtolWebRequestsDriver.StatusProcessCommand.Answer.ResultProcess>();

        public class ResultProcess
        {
          [JsonProperty("error")]
          public AtolWebRequestsDriver.Error Error { get; set; }

          [JsonProperty("status")]
          [JsonConverter(typeof (StringEnumConverter))]
          public ResultProcessCommand Status { get; set; }

          [JsonProperty("result")]
          public object Result { get; set; }
        }
      }
    }

    public abstract class AtolRequest
    {
      [JsonProperty("type")]
      public virtual string Type { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public abstract class ServiceRequest
    {
      [JsonIgnore]
      public string AnswerString { get; set; }
    }
  }
}
