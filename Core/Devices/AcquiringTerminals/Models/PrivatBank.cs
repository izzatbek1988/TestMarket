// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.PrivatBank
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Globalization;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class PrivatBank : IAcquiringTerminal, IDevice
  {
    public void EmergencyCancel() => LogHelper.Debug("Аварийная отмена платежа: не реализовано");

    private PrivatBankDriver CurrentDriver { get; set; }

    private LanConnection LanConnection { get; set; }

    public PrivatBank(LanConnection lanConnection)
    {
      this.LanConnection = lanConnection;
      this.CurrentDriver = new PrivatBankDriver(lanConnection);
    }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new DeviceConnection()
      {
        LanPort = this.LanConnection,
        ComPort = new ComPort(),
        ConnectionType = GlobalDictionaries.Devices.ConnectionTypes.Lan
      }, ConnectionSettingsViewModel.PortsConfig.OnlyLan);
    }

    public void ShowServiceMenu(out string slip) => throw new NotImplementedException();

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      throw new AcquiringException((IDevice) this, "Old method");
    }

    public bool DoPayment(
      Decimal sum,
      out string slip,
      out string rrn,
      out string method,
      out string approvalCode,
      out string issuerName,
      out string terminalId,
      out string cardNumber,
      out string paymentSystem)
    {
      method = string.Empty;
      PrivatBankDriver.PurchaseCommand command = new PrivatBankDriver.PurchaseCommand()
      {
        Params = {
          Amount = Convert.ToString(sum, (IFormatProvider) CultureInfo.InvariantCulture)
        }
      };
      if (this.CurrentDriver.DoCommand((PrivatBankDriver.PrivatBankCommand) command))
      {
        this.CheckResult((PrivatBankDriver.Answer) command.Data, command.Data.Params.ResponseCode);
        slip = command.Data.Params.Receipt;
        rrn = command.Data.Params.Rrn;
        approvalCode = command.Data.Params.ApprovalCode;
        issuerName = command.Data.Params.BankAcquirer;
        terminalId = command.Data.Params.TerminalId;
        cardNumber = command.Data.Params.CardNumber;
        paymentSystem = command.Data.Params.IssuerName;
        LogHelper.Debug("rrn = " + rrn);
        LogHelper.Debug("approvalCode = " + approvalCode);
        LogHelper.Debug("bankAcquirer = " + issuerName);
        LogHelper.Debug("terminalId = " + terminalId);
        LogHelper.Debug("cardNumber = " + cardNumber);
        LogHelper.Debug("PaymentSystem = " + paymentSystem);
        return true;
      }
      slip = "";
      rrn = "";
      approvalCode = "";
      issuerName = "";
      terminalId = "";
      cardNumber = "";
      paymentSystem = "";
      return false;
    }

    private void CheckResult(PrivatBankDriver.Answer answer, string responseCode)
    {
      if (answer.Error)
        throw new DeviceException(Translate.PrivatBank_CheckResult_Ошибка_при_выполнении_команды_на_терминале_ПриватБанк__ + answer.ErrorDescription);
      if (responseCode != "0000")
        throw new DeviceException(Translate.PrivatBank_CheckResult_Ошибка_при_выполнении_команды_на_терминале_ПриватБанк__ + Translate.PrivatBank_CheckResult_Код_ошибки__ + responseCode);
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      if (string.IsNullOrEmpty(rrn))
      {
        (bool result, string output) tuple = MessageBoxHelper.Input("", Translate.PrivatBank_Для_возврата_введите_номер_транзакции__RRN_);
        if (!tuple.result)
        {
          slip = string.Empty;
          return false;
        }
        rrn = tuple.output;
      }
      PrivatBankDriver.RefundCommand command = new PrivatBankDriver.RefundCommand()
      {
        Params = {
          Amount = Convert.ToString(sum, (IFormatProvider) CultureInfo.InvariantCulture),
          Rrn = rrn
        }
      };
      int num = this.CurrentDriver.DoCommand((PrivatBankDriver.PrivatBankCommand) command) ? 1 : 0;
      this.CheckResult((PrivatBankDriver.Answer) command.Data, command.Data.Params.ResponseCode);
      slip = command.Data.Params.Receipt;
      return num != 0;
    }

    public bool GetReport(out string slip)
    {
      PrivatBankDriver.AuditCommand command = new PrivatBankDriver.AuditCommand();
      int num = this.CurrentDriver.DoCommand((PrivatBankDriver.PrivatBankCommand) command) ? 1 : 0;
      this.CheckResult((PrivatBankDriver.Answer) command.Data, command.Data.Params.ResponseCode);
      slip = command.Data.Params.Receipt;
      return num != 0;
    }

    public bool CloseSession(out string slip)
    {
      PrivatBankDriver.VerifyCommand command = new PrivatBankDriver.VerifyCommand();
      int num = this.CurrentDriver.DoCommand((PrivatBankDriver.PrivatBankCommand) command) ? 1 : 0;
      this.CheckResult((PrivatBankDriver.Answer) command.Data, command.Data.Params.ResponseCode);
      slip = command.Data.Params.Receipt;
      return num != 0;
    }

    public bool Connect() => true;

    public bool Disconnect() => true;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => Translate.PrivatBank_Name_Private_Bank;
  }
}
