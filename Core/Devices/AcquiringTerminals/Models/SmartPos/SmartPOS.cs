// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.SmartPOS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using System;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class SmartPOS : IAcquiringTerminal, IDevice
  {
    private SmartPosDriver _driver;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => nameof (SmartPOS);

    private Gbs.Core.Config.Devices DevicesConfig { get; }

    public SmartPOS(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public void ShowProperties()
    {
      DeviceConnection connection = new DeviceConnection()
      {
        ComPort = this.DevicesConfig.AcquiringTerminal.ComPort,
        LanPort = this.DevicesConfig.AcquiringTerminal.LanConnection,
        ConnectionType = GlobalDictionaries.Devices.ConnectionTypes.Lan
      };
      new FrmConnectionSettings().ShowConfig(connection, ConnectionSettingsViewModel.PortsConfig.OnlyLan);
      this.DevicesConfig.AcquiringTerminal.ConnectionType = connection.ConnectionType;
    }

    public void ShowServiceMenu(out string slip) => throw new NotImplementedException();

    private SmartPosDriver.StatusCommand.StatusAnswer WaitOperation(string id)
    {
      SmartPosDriver.StatusCommand command = new SmartPosDriver.StatusCommand()
      {
        ProcessId = id
      };
      do
      {
        this._driver.DoCommand((SmartPosDriver.SmartPosCommand) command);
      }
      while (command.Answer.Status == SmartPosDriver.Status.wait);
      return command.Answer;
    }

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      slip = "";
      method = "";
      SmartPosDriver.PaymentCommand command = new SmartPosDriver.PaymentCommand()
      {
        Amount = (int) sum
      };
      this._driver.DoCommand((SmartPosDriver.SmartPosCommand) command);
      if (command.Answer.Status == SmartPosDriver.Status.fail)
        throw new AcquiringException((IDevice) this, command.Answer.Message);
      SmartPosDriver.StatusCommand.StatusAnswer statusAnswer = this.WaitOperation(command.Answer.ProcessId);
      rrn = statusAnswer.Status != SmartPosDriver.Status.fail ? statusAnswer.TransactionId : throw new AcquiringException((IDevice) this, statusAnswer.Message);
      method = statusAnswer.ChequeInfo.Method;
      return true;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      slip = "";
      SmartPosDriver.RefundPaymentCommand command = new SmartPosDriver.RefundPaymentCommand()
      {
        Amount = (int) sum,
        TransactionId = rrn,
        MethodPayment = method
      };
      this._driver.DoCommand((SmartPosDriver.SmartPosCommand) command);
      if (command.Answer.Status == SmartPosDriver.Status.fail)
        throw new AcquiringException((IDevice) this, command.Answer.Message);
      SmartPosDriver.StatusCommand.StatusAnswer statusAnswer = this.WaitOperation(command.Answer.ProcessId);
      if (statusAnswer.Status == SmartPosDriver.Status.fail)
        throw new AcquiringException((IDevice) this, statusAnswer.Message);
      return true;
    }

    public bool GetReport(out string slip) => throw new NotImplementedException();

    public bool CloseSession(out string slip) => throw new NotImplementedException();

    public bool Connect()
    {
      this._driver = new SmartPosDriver(this.DevicesConfig.AcquiringTerminal.LanConnection);
      return true;
    }

    public bool Disconnect() => true;

    public void EmergencyCancel() => throw new NotImplementedException();
  }
}
