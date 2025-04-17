// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.KkmServer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class KkmServer : IAcquiringTerminal, IDevice
  {
    public void EmergencyCancel() => LogHelper.Debug("Аварийная отмена платежа: не реализовано");

    private KkmServerDriver CurrentDriver { get; set; }

    private LanConnection LanConnection { get; set; }

    public KkmServer(LanConnection lanConnection)
    {
      this.LanConnection = lanConnection;
      this.CurrentDriver = new KkmServerDriver(lanConnection);
    }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new DeviceConnection()
      {
        LanPort = this.LanConnection,
        ComPort = new ComPort(),
        ConnectionType = GlobalDictionaries.Devices.ConnectionTypes.Lan
      }, ConnectionSettingsViewModel.PortsConfig.OnlyLan, true);
    }

    public void ShowServiceMenu(out string slip) => throw new NotImplementedException();

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      method = string.Empty;
      KkmServerDriver.TerminalDoPayment command = new KkmServerDriver.TerminalDoPayment()
      {
        Amount = sum
      };
      int num1 = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command) ? 1 : 0;
      slip = string.Empty;
      rrn = string.Empty;
      if (num1 == 0)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.KkmServer_DoPayment_Ошибка_оплаты_на_терминале__ + command.DeviceAnswer?.Error, icon: MessageBoxImage.Hand);
        return num1 != 0;
      }
      slip = command.Data.Slip;
      rrn = command.Data.UniversalID;
      return num1 != 0;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      KkmServerDriver.TerminalReturnPayment command = new KkmServerDriver.TerminalReturnPayment()
      {
        Amount = sum
      };
      if (!rrn.IsNullOrEmpty())
        command.UniversalID = rrn;
      int num1 = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command) ? 1 : 0;
      if (num1 == 0)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.KkmServer_ReturnPayment_Ошибка_возврата_оплаты_на_терминале__ + command.DeviceAnswer.Error, icon: MessageBoxImage.Hand);
      }
      slip = command.Data.Slip;
      return num1 != 0;
    }

    public bool GetReport(out string slip)
    {
      KkmServerDriver.TerminalGetReport command = new KkmServerDriver.TerminalGetReport();
      if (!this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command))
      {
        slip = string.Empty;
        return false;
      }
      slip = command.Data.Slip;
      return true;
    }

    public bool CloseSession(out string slip)
    {
      KkmServerDriver.TerminalCloseSession command = new KkmServerDriver.TerminalCloseSession();
      int num = this.CurrentDriver.SendCommand((KkmServerDriver.KkmServerCommand) command) ? 1 : 0;
      slip = command.Data.Slip;
      return num != 0;
    }

    public bool Connect() => true;

    public bool Disconnect() => true;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => Translate.KkmServer_Name_ККМ_Сервер;
  }
}
