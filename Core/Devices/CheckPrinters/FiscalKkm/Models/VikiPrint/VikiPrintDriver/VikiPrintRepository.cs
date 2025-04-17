// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.VikiPrintRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class VikiPrintRepository
  {
    private readonly LanConnection _lanConnection;
    private readonly string _plantNumber;

    public VikiPrintRepository(LanConnection lanConnection, string plantNumber)
    {
      this._lanConnection = lanConnection;
      this._plantNumber = plantNumber;
    }

    public void DoCommand(VikiPrintRepository.VikiCommand command)
    {
      RestHelper restHelper = new RestHelper(this._lanConnection.UrlAddress, this._lanConnection.PortNumber, command.ToJsonString(isIgnoreNull: true));
      restHelper.CreateCommand(command.Path, command.Type);
      if (!this._plantNumber.IsNullOrEmpty())
        restHelper.AddHeader("PlantNumber", this._plantNumber);
      restHelper.DoCommand();
      command.AnswerString = restHelper.Answer;
    }

    public abstract class VikiCommand : RestHelper.RestCommand
    {
    }

    public class VikiAnswer
    {
      [JsonProperty("error")]
      public int Error { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
    }

    public class CreateDocumentCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/documents/purchase";

      [JsonProperty("cashier")]
      public VikiPrintRepository.Cashier Cashier { get; set; }

      [JsonProperty("buyer")]
      public VikiPrintRepository.Buyer Buyer { get; set; }

      [JsonProperty("calc_address")]
      public string SalePointAddress { get; set; }

      [JsonProperty("sno")]
      [JsonConverter(typeof (StringEnumConverter))]
      public SnoTypes? Sno { get; set; }

      [JsonProperty("type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public DocumentTypes DocumentType { get; set; }

      [JsonProperty("phone_email")]
      public string PhoneOrEmailForClient { get; set; }

      [JsonProperty("discount")]
      public VikiPrintRepository.CreateDocumentCommand.DiscountItem Discount { get; set; }

      [JsonProperty("payments")]
      public List<VikiPrintRepository.CreateDocumentCommand.PaymentItem> Payments { get; set; } = new List<VikiPrintRepository.CreateDocumentCommand.PaymentItem>();

      [JsonProperty("positions")]
      public List<VikiPrintRepository.CreateDocumentCommand.PositionItem> Positions { get; set; } = new List<VikiPrintRepository.CreateDocumentCommand.PositionItem>();

      public class DiscountItem
      {
        [JsonProperty("type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public DiscountTypes Type { get; set; }

        [JsonProperty("sum")]
        public Decimal Sum { get; set; }

        [JsonProperty("name")]
        public string Description { get; set; }
      }

      public class IndustryInfo
      {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
      }

      public class PaymentItem
      {
        [JsonProperty("type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public VikiPrintRepository.CreateDocumentCommand.PaymentItem.PaymentTypes Type { get; set; }

        [JsonProperty("sum")]
        public Decimal Sum { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public enum PaymentTypes
        {
          [EnumMember(Value = "CASH")] Cash,
          [EnumMember(Value = "CASHLESS")] Bank,
          [EnumMember(Value = "ADVANCE")] Prepaid,
          [EnumMember(Value = "CREDIT")] Credit,
          [EnumMember(Value = "EXCHANGE")] Exchange,
        }
      }

      public class PositionItem
      {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("quantity")]
        public Decimal Quantity { get; set; }

        [JsonProperty("partial_content")]
        public int PartialContent { get; set; }

        [JsonProperty("unit_price")]
        public Decimal Price { get; set; }

        [JsonProperty("code_mark")]
        public string CodeMark { get; set; }

        [JsonProperty("discount")]
        public VikiPrintRepository.CreateDocumentCommand.DiscountItem Discount { get; set; }

        [JsonProperty("credit")]
        public Decimal CreditSum { get; set; }

        [JsonProperty("nds")]
        [JsonConverter(typeof (StringEnumConverter))]
        public NdsTypes Nds { get; set; }

        [JsonProperty("clc_type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public CalculationTypes CalculationType { get; set; }

        [JsonProperty("goods_type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public GoodCalculationTypes GoodCalculationType { get; set; }

        [JsonProperty("measure")]
        [JsonConverter(typeof (StringEnumConverter))]
        public UnitTypes Unit { get; set; }

        [JsonProperty("industry_props")]
        public VikiPrintRepository.CreateDocumentCommand.IndustryInfo IndustryInfo { get; set; }
      }
    }

    public class ActionWithShiftCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path
      {
        get
        {
          switch (this.ActionShift)
          {
            case VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType.OpenShift:
              return "/documents/openShift";
            case VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType.CloseShift:
              return "/documents/closeShift";
            default:
              throw new NotImplementedException();
          }
        }
      }

      [JsonProperty("cashier")]
      public VikiPrintRepository.Cashier Cashier { get; set; }

      [JsonProperty("calc_address")]
      public string SalePointAddress { get; set; }

      [JsonProperty("clc_place")]
      public string SalePointName { get; set; }

      [JsonIgnore]
      public VikiPrintRepository.ActionWithShiftCommand.ActionWithShiftType ActionShift { get; set; }

      public enum ActionWithShiftType
      {
        OpenShift,
        CloseShift,
      }
    }

    public class XReportCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/reports/xReport";

      [JsonProperty("cashier")]
      public VikiPrintRepository.Cashier Cashier { get; set; }
    }

    public class InfoKkmCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/values";

      [JsonProperty("values")]
      public List<string> Values
      {
        get
        {
          return new List<string>()
          {
            "fs_number",
            "fs_software_params",
            "current_shift",
            "next_receipt_num",
            "ofd_state",
            "printer_state",
            "fs_last_doc_num",
            "firmware_id",
            "amount_of_cash_in_drawer",
            "date_of_fs_finished",
            "date_time_of_open_shift",
            "printer_firmware_version",
            "plant_number"
          };
        }
      }

      public class InfoKkmAnswer : VikiPrintRepository.VikiAnswer
      {
        [JsonProperty("fs_number")]
        public string FsNumber { get; set; }

        [JsonProperty("plant_number")]
        public string FactoryNumber { get; set; }

        [JsonProperty("current_shift")]
        public string CurrentShiftNum { get; set; }

        [JsonProperty("amount_of_cash_in_drawer")]
        public Decimal AmountOfCashInDrawer { get; set; }

        [JsonProperty("next_receipt_num")]
        public int NextReceiptNum { get; set; }

        [JsonProperty("fs_last_doc_num")]
        public string FnLastDocNum { get; set; }

        [JsonProperty("date_time_of_open_shift")]
        public string DateTimeOfOpenShift { get; set; }

        [JsonProperty("date_of_fs_finished")]
        public string DateTimeOfFnFinished { get; set; }

        [JsonProperty("printer_firmware_version")]
        public string PrinterFirmwareVersion { get; set; }

        [JsonProperty("ofd_state")]
        public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer.OfdState Ofd { get; set; }

        [JsonProperty("printer_state")]
        public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer.PrinterState PrinterAnswer { get; set; }

        [JsonProperty("common_amounts")]
        public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer.CommonAmounts CommonAmount { get; set; }

        public class OfdState
        {
          [JsonProperty("available_doc_date")]
          public string DateFirstDocNotSent { get; set; }

          [JsonProperty("available_doc_num")]
          public int NumberFirstDocNotSent { get; set; }

          [JsonProperty("available_docs")]
          public int CountNotSentDocument { get; set; }

          [JsonProperty("status")]
          public int Status { get; set; }
        }

        public class PrinterState
        {
          [JsonProperty("current_state")]
          public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer.PrinterState.CurrentState CurrentStateKkm { get; set; }

          [JsonProperty("open_document_type")]
          [JsonConverter(typeof (StringEnumConverter))]
          public VikiPrintRepository.InfoKkmCommand.InfoKkmAnswer.PrinterState.OpenDocumentTypes OpenDocumentType { get; set; }

          [JsonProperty("open_document_state")]
          public int OpenDocumentState { get; set; }

          public enum OpenDocumentTypes
          {
            [EnumMember(Value = "CLOSED")] Closed,
            [EnumMember(Value = "SERVICE")] Service,
            [EnumMember(Value = "SALE")] Sale,
            [EnumMember(Value = "REFUND")] Refund,
            [EnumMember(Value = "INSERTION")] Insertion,
            [EnumMember(Value = "RESERVE")] Reserve,
            [EnumMember(Value = "OUTFLOW")] Outflow,
            [EnumMember(Value = "OUTFLOW_REFUND")] OutflowRefund,
            [EnumMember(Value = "SALE_CORRECTION")] SaleCorrection,
            [EnumMember(Value = "OUTFLOW_CORRECTION")] OutflowCorrection,
            [EnumMember(Value = "REFUND_CORRECTION")] RefundCorrection,
            [EnumMember(Value = "OUTFLOW_REFUND_CORRECTION")] OutflowRefundCorrection,
          }

          public class CurrentState
          {
            [JsonProperty("closed_fs_archive")]
            public bool IsClosedFnArchive { get; set; }

            [JsonProperty("fs_is_not_registered")]
            public bool IsNotRegisteredFn { get; set; }

            [JsonProperty("need_start_initialization")]
            public bool IsNeedStartInitialization { get; set; }

            [JsonProperty("not_fiscal_mode_enabled")]
            public bool IsNotFiscalModeEnabled { get; set; }

            [JsonProperty("shift_more24h")]
            public bool IsShiftMore24 { get; set; }

            [JsonProperty("shift_opened")]
            public bool IsShiftOpened { get; set; }
          }
        }

        public class CommonAmounts
        {
        }
      }
    }

    public class SumInfoKkmCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.SumInfoKkmCommand.InfoKkmAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<VikiPrintRepository.SumInfoKkmCommand.InfoKkmAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/values";

      [JsonProperty("values")]
      public List<string> Values
      {
        get
        {
          return new List<string>()
          {
            "amount_of_cash_in_drawer"
          };
        }
      }

      public class InfoKkmAnswer : VikiPrintRepository.VikiAnswer
      {
        [JsonProperty("amount_of_cash_in_drawer")]
        public Decimal AmountOfCashInDrawer { get; set; }
      }
    }

    public class OpenDrawerCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/devices/openDrawer?pulse_duration=300";
    }

    public class CheckCodeMarkCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.CheckCodeMarkCommand.CheckCodeMarkAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<VikiPrintRepository.CheckCodeMarkCommand.CheckCodeMarkAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/documents/purchase/checkCodeMark";

      [JsonProperty("code_mark")]
      public string CodeMark { get; set; }

      [JsonProperty("document_type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public DocumentTypes DocumentType { get; set; }

      [JsonProperty("quantity")]
      public Decimal Quantity { get; set; }

      [JsonProperty("measure")]
      [JsonConverter(typeof (StringEnumConverter))]
      public UnitTypes Unit { get; set; }

      [JsonProperty("partial_content")]
      public int PartialContent { get; set; }

      public class CheckCodeMarkAnswer : VikiPrintRepository.VikiAnswer
      {
        [JsonProperty("check_code_mark_status")]
        public VikiPrintRepository.CheckCodeMarkCommand.CheckCodeMarkAnswer.StatusCodeMark Result { get; set; }

        public class StatusCodeMark
        {
          [JsonProperty("fs_check_code_mark_failure_reason")]
          public int CheckCodeMarkFailureReason { get; set; }

          [JsonProperty("fs_check_code_mark_status")]
          public int CheckCodeMarkStatus { get; set; }

          [JsonProperty("goods_status_info")]
          public int GoodsStatusInfo { get; set; }

          [JsonProperty("request_result")]
          public int RequestResult { get; set; }

          [JsonProperty("request_result_code")]
          public int RequestResultCode { get; set; }
        }
      }
    }

    public class AcceptCodeMarkCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Put;

      public override string Path => "/documents/purchase/adoptCodeMark";
    }

    public class PrintTextCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/printText";

      [JsonProperty("text")]
      public string Text { get; set; }
    }

    public class FeedLineCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/lineFeed";
    }

    public class PrintBarcodeCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/printBarcode";

      [JsonProperty("barcode_type")]
      public BarcodeType BarcodeType { get; set; }

      [JsonProperty("width")]
      public int Width { get; set; }

      [JsonProperty("height")]
      public int Height { get; set; }

      [JsonProperty("barcode")]
      public string Barcode { get; set; }

      [JsonProperty("output_flags")]
      public string OutputFlags => "OUTPUT_DISABLE";
    }

    public class CashOperationCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.VikiAnswer Answer
      {
        get => JsonConvert.DeserializeObject<VikiPrintRepository.VikiAnswer>(this.AnswerString);
      }

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/documents/cashDrawerOperation";

      [JsonProperty("cashier")]
      public VikiPrintRepository.Cashier Cashier { get; set; }

      [JsonProperty("calc_address")]
      public string SalePointAddress { get; set; }

      [JsonProperty("sno")]
      [JsonConverter(typeof (StringEnumConverter))]
      public SnoTypes? Sno { get; set; }

      [JsonProperty("type")]
      [JsonConverter(typeof (StringEnumConverter))]
      public VikiPrintRepository.CashOperationCommand.OperationTypes OperationType { get; set; }

      [JsonProperty("amount")]
      public Decimal Amount { get; set; }

      [JsonProperty("banknote_name")]
      public string BanknoteName => "RUB";

      public enum OperationTypes
      {
        [EnumMember(Value = "RESERVE")] Reserve,
        [EnumMember(Value = "INSERTION")] Insertion,
      }
    }

    public class GetPrintersCommand : VikiPrintRepository.VikiCommand
    {
      [JsonIgnore]
      public VikiPrintRepository.GetPrintersCommand.PrintersAnswer Answer
      {
        get
        {
          return JsonConvert.DeserializeObject<VikiPrintRepository.GetPrintersCommand.PrintersAnswer>(this.AnswerString);
        }
      }

      public override TypeRestRequest Type => TypeRestRequest.Get;

      public override string Path => "/devices/printers";

      public class PrintersAnswer : VikiPrintRepository.VikiAnswer
      {
        [JsonProperty("printers")]
        public List<VikiPrintRepository.GetPrintersCommand.PrintersAnswer.Printer> Printers { get; set; }

        public class Printer
        {
          [JsonProperty("plant_number")]
          public string PlantNumber { get; set; }
        }
      }
    }

    public class Cashier
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("inn")]
      public string Inn { get; set; }
    }

    public class Buyer
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("inn")]
      public string Inn { get; set; }

      [JsonProperty("date_of_birth")]
      public string DateOfBirth { get; set; }
    }
  }
}
