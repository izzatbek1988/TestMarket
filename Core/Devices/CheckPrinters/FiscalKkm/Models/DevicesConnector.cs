// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.DevicesConnector
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class DevicesConnector : IFiscalKkm, IDevice
  {
    private Guid _uidDevice;
    private DevicesConnectorDriver _currentDriver;

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (DevicesConnector);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public DevicesConnectorDriver.PrintFiscalReceiptCommand ReceiptCommand { get; set; }

    public bool IsCanHoldConnection => false;

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = false,
        Type = GlobalDictionaries.Devices.ConnectionTypes.Lan
      });
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.KkmOpenSessionCommand command = new DevicesConnectorDriver.KkmOpenSessionCommand();
      command.DeviceId = this._uidDevice;
      command.Cashier = new DevicesConnectorDriver.Cashier()
      {
        Name = cashier.Name,
        TaxId = cashier.Inn
      };
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
    }

    public void GetReport(Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      DevicesConnectorDriver.KkmGetReportCommand getReportCommand = new DevicesConnectorDriver.KkmGetReportCommand();
      getReportCommand.Cashier = new DevicesConnectorDriver.Cashier()
      {
        Name = cashier.Name,
        TaxId = cashier.Inn
      };
      getReportCommand.DeviceId = this._uidDevice;
      DevicesConnectorDriver.KkmGetReportCommand command = getReportCommand;
      switch (reportType)
      {
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.ZReport:
          command.ReportType = DevicesConnectorDriver.Enums.ReportTypes.ZReport;
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReport:
          command.ReportType = DevicesConnectorDriver.Enums.ReportTypes.XReport;
          break;
        case Gbs.Core.Devices.CheckPrinters.FiscalKkm.ReportTypes.XReportWithGoods:
          command.ReportType = DevicesConnectorDriver.Enums.ReportTypes.XReportWithGoods;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      this._currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      DevicesConnectorDriver.PrintFiscalReceiptCommand fiscalReceiptCommand1 = new DevicesConnectorDriver.PrintFiscalReceiptCommand();
      DevicesConnectorDriver.PrintFiscalReceiptCommand fiscalReceiptCommand2 = fiscalReceiptCommand1;
      DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptData receiptData1 = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptData();
      receiptData1.Cashier = new DevicesConnectorDriver.Cashier(checkData.Cashier);
      receiptData1.Contractor = checkData.Client == null ? (DevicesConnectorDriver.Contractor) null : new DevicesConnectorDriver.Contractor(checkData.Client.Client);
      DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptData receiptData2 = receiptData1;
      DevicesConnectorDriver.Enums.ReceiptFiscalTypes receiptFiscalTypes;
      switch (checkData.FiscalType)
      {
        case CheckFiscalTypes.Fiscal:
          receiptFiscalTypes = DevicesConnectorDriver.Enums.ReceiptFiscalTypes.Fiscal;
          break;
        case CheckFiscalTypes.NonFiscal:
          receiptFiscalTypes = DevicesConnectorDriver.Enums.ReceiptFiscalTypes.NonFiscal;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      receiptData2.FiscalType = receiptFiscalTypes;
      DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptData receiptData3 = receiptData1;
      DevicesConnectorDriver.Enums.ReceiptOperationTypes receiptOperationTypes;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          receiptOperationTypes = DevicesConnectorDriver.Enums.ReceiptOperationTypes.Sale;
          break;
        case CheckTypes.ReturnSale:
          receiptOperationTypes = DevicesConnectorDriver.Enums.ReceiptOperationTypes.ReturnSale;
          break;
        case CheckTypes.Buy:
          receiptOperationTypes = DevicesConnectorDriver.Enums.ReceiptOperationTypes.Buy;
          break;
        case CheckTypes.ReturnBuy:
          receiptOperationTypes = DevicesConnectorDriver.Enums.ReceiptOperationTypes.ReturnBuy;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      receiptData3.OperationType = receiptOperationTypes;
      receiptData1.CountrySpecificData = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptExtraData()
      {
        DigitalReceiptAddress = checkData.AddressForDigitalCheck,
        TaxVariantIndex = (int) checkData.RuTaxSystem
      };
      fiscalReceiptCommand2.Receipt = receiptData1;
      fiscalReceiptCommand1.DeviceId = this._uidDevice;
      this.ReceiptCommand = fiscalReceiptCommand1;
      this.SendDigitalCheck(checkData.AddressForDigitalCheck);
      if (this.DevicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
      {
        LogHelper.Debug("Выключаем печать бумажного чека DevicesConnector");
        this.ReceiptCommand.Receipt.IsPrintReceipt = false;
      }
      return true;
    }

    public bool CloseCheck()
    {
      this._currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) this.ReceiptCommand);
      return true;
    }

    public void CancelCheck()
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.CancelFiscalReceiptCommand command = new DevicesConnectorDriver.CancelFiscalReceiptCommand();
      command.DeviceId = this._uidDevice;
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.KkmCashInOutCommand command = new DevicesConnectorDriver.KkmCashInOutCommand();
      command.DeviceId = this._uidDevice;
      command.Cashier = new DevicesConnectorDriver.Cashier()
      {
        Name = cashier.Name,
        TaxId = cashier.Inn
      };
      command.Sum = -sum;
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.KkmCashInOutCommand command = new DevicesConnectorDriver.KkmCashInOutCommand();
      command.DeviceId = this._uidDevice;
      command.Cashier = new DevicesConnectorDriver.Cashier()
      {
        Name = cashier.Name,
        TaxId = cashier.Inn
      };
      command.Sum = sum;
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
      return true;
    }

    public bool WriteOfdAttribute(Gbs.Core.Devices.CheckPrinters.FiscalKkm.OfdAttributes ofdAttribute, object value)
    {
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      this.ReceiptCommand.Receipt.Items.Add(new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItem()
      {
        Price = good.Price,
        Barcode = good.Barcode,
        Comment = good.Description,
        DepartmentIndex = good.KkmSectionNumber,
        Discount = good.Discount,
        Name = good.Name,
        Quantity = good.Quantity,
        TaxRateIndex = new int?(good.TaxRateNumber),
        CountrySpecificData = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData()
        {
          FfdData = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData.RuFfdInfo()
          {
            Method = (DevicesConnectorDriver.Enums.FfdCalculationMethods) good.RuFfdPaymentModeCode,
            Subject = (DevicesConnectorDriver.Enums.FfdCalculationSubjects) good.RuFfdGoodTypeCode,
            Unit = (DevicesConnectorDriver.Enums.FfdUnitsIndex) good.Unit.RuFfdUnitsIndex
          },
          MarkingInfo = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptItemData.RuMarkingInfo()
          {
            RawCode = good.MarkedInfo?.FullCode ?? string.Empty
          }
        }
      });
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      if (payment.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bonus, GlobalDictionaries.KkmPaymentMethods.Certificate))
        return true;
      List<DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment> payments = this.ReceiptCommand.Receipt.Payments;
      DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment receiptPayment1 = new DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment();
      DevicesConnectorDriver.PrintFiscalReceiptCommand.ReceiptPayment receiptPayment2 = receiptPayment1;
      int num;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          num = 1;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          num = 2;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          num = 2;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          num = 0;
          break;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          num = 104;
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          num = 103;
          break;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          num = 2;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      receiptPayment2.MethodIndex = num;
      receiptPayment1.Sum = payment.Sum;
      payments.Add(receiptPayment1);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._currentDriver = new DevicesConnectorDriver(devicesConfig.CheckPrinter.Connection.LanPort);
      this._uidDevice = Guid.Parse("e4bfd416-8d2f-41ab-9475-2e99208bc330");
      this.DevicesConfig = devicesConfig;
    }

    public bool Disconnect() => true;

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<Gbs.Core.Devices.CheckPrinters.NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper()
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.CutPaperCommand command = new DevicesConnectorDriver.CutPaperCommand();
      command.DeviceId = this._uidDevice;
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
      return true;
    }

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetStatus()
    {
      DevicesConnectorDriver.KkmGetStatus kkmGetStatus = new DevicesConnectorDriver.KkmGetStatus();
      kkmGetStatus.DeviceId = this._uidDevice;
      DevicesConnectorDriver.KkmGetStatus command = kkmGetStatus;
      this._currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
      DevicesConnectorDriver.KkmGetStatus.KkmStatus data = command.Result.Data;
      Gbs.Core.Devices.CheckPrinters.KkmStatus status = new Gbs.Core.Devices.CheckPrinters.KkmStatus();
      status.Model = data.Model;
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus1 = status;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses sessionStatuses;
      switch (data.SessionStatus)
      {
        case DevicesConnectorDriver.Enums.SessionStatuses.Unknown:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Unknown;
          break;
        case DevicesConnectorDriver.Enums.SessionStatuses.Open:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Open;
          break;
        case DevicesConnectorDriver.Enums.SessionStatuses.OpenMore24Hours:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.OpenMore24Hours;
          break;
        case DevicesConnectorDriver.Enums.SessionStatuses.Close:
          sessionStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.SessionStatuses.Close;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowInvalidOperationException();
          break;
      }
      kkmStatus1.SessionStatus = sessionStatuses;
      status.SessionStarted = data.SessionStarted;
      status.CheckNumber = data.CheckNumber;
      status.OfdLastSendDateTime = new DateTime?(data.RuKkmInfo.OfdLastSendDateTime);
      status.OfdNotSendDocuments = data.RuKkmInfo.OfdNotSendDocuments;
      status.FactoryNumber = data.FactoryNumber;
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus2 = status;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses checkStatuses;
      switch (data.CheckStatus)
      {
        case DevicesConnectorDriver.Enums.CheckStatuses.Unknown:
          checkStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Unknown;
          break;
        case DevicesConnectorDriver.Enums.CheckStatuses.Open:
          checkStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Open;
          break;
        case DevicesConnectorDriver.Enums.CheckStatuses.Close:
          checkStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.CheckStatuses.Close;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowInvalidOperationException();
          break;
      }
      kkmStatus2.CheckStatus = checkStatuses;
      status.DriverVersion = data.DriverVersion;
      status.FnDateEnd = data.FnDateEnd;
      status.SessionNumber = data.SessionNumber;
      Gbs.Core.Devices.CheckPrinters.KkmStatus kkmStatus3 = status;
      Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses kkmStatuses;
      switch (data.KkmState)
      {
        case DevicesConnectorDriver.Enums.KkmStatuses.Unknown:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.Unknown;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.Ready:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.Ready;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.NoPaper:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.NoPaper;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.OfdDocumentsToMany:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.OfdDocumentsToMany;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.CoverOpen:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.CoverOpen;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.HardwareError:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.HardwareError;
          break;
        case DevicesConnectorDriver.Enums.KkmStatuses.NeedToContinuePrint:
          kkmStatuses = Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmStatuses.NeedToContinuePrint;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowInvalidOperationException();
          break;
      }
      kkmStatus3.KkmState = kkmStatuses;
      status.SoftwareVersion = data.SoftwareVersion;
      return status;
    }

    public Gbs.Core.Devices.CheckPrinters.KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer()
    {
      DevicesConnectorDriver currentDriver = this._currentDriver;
      DevicesConnectorDriver.OpenCashBoxCommand command = new DevicesConnectorDriver.OpenCashBoxCommand();
      command.DeviceId = this._uidDevice;
      currentDriver.SendCommand((DevicesConnectorDriver.DevicesConnectorCommand) command);
      return true;
    }

    public bool SendDigitalCheck(string adress)
    {
      this.ReceiptCommand.Receipt.CountrySpecificData.DigitalReceiptAddress = adress;
      if (!adress.IsNullOrEmpty() && this.DevicesConfig.CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
      {
        LogHelper.Debug("Выключаем печать бумажного чека DevicesConnector");
        this.ReceiptCommand.Receipt.IsPrintReceipt = false;
      }
      return true;
    }

    public void FeedPaper(int lines) => throw new NotImplementedException();

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
