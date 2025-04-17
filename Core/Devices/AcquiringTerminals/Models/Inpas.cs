// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.Inpas
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class Inpas : IAcquiringTerminal, IDevice
  {
    private Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver.InpasDriver _currentDriver;
    private static string _terminalId;

    public void EmergencyCancel() => LogHelper.Debug("Аварийная отмена платежа: не реализовано");

    private Gbs.Core.Config.Devices DevicesConfig { get; }

    public Inpas(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool CloseSession(out string slip)
    {
      InpasResponse inpasResponse = this._currentDriver.DoCommand(new InpasRequest()
      {
        Field = new List<Field>()
        {
          new Field() { Id = "25", Text = "59" },
          new Field() { Id = "27", Text = Inpas._terminalId }
        }
      });
      slip = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "90"))?.Text ?? string.Empty;
      return true;
    }

    public bool Connect()
    {
      this._currentDriver = new Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver.InpasDriver(this.DevicesConfig.AcquiringTerminal.LanConnection);
      if (new ServiceController("DCService").Status != ServiceControllerStatus.Running)
        this._currentDriver.RestartService();
      if (Inpas._terminalId.IsNullOrEmpty())
        Inpas._terminalId = this._currentDriver.DoCommand(new InpasRequest()
        {
          Field = new List<Field>()
          {
            new Field() { Id = "25", Text = "26" }
          }
        }).Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "27"))?.Text ?? string.Empty;
      return true;
    }

    public bool Disconnect() => true;

    public void ShowServiceMenu(out string slip) => throw new NotImplementedException();

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      method = string.Empty;
      rrn = string.Empty;
      return this.DoLinkPayment(1, sum, out slip, ref rrn);
    }

    public bool GetReport(out string slip)
    {
      InpasResponse inpasResponse = this._currentDriver.DoCommand(new InpasRequest()
      {
        Field = new List<Field>()
        {
          new Field() { Id = "25", Text = "63" },
          new Field() { Id = "27", Text = Inpas._terminalId },
          new Field() { Id = "65", Text = "21" }
        }
      });
      slip = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "90"))?.Text ?? string.Empty;
      return true;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      return this.DoLinkPayment(4, sum, out slip, ref rrn) || this.DoLinkPayment(29, sum, out slip, ref rrn);
    }

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

    private bool DoLinkPayment(int codeOperation, Decimal sum, out string slip, ref string rrn)
    {
      InpasRequest command = new InpasRequest()
      {
        Field = new List<Field>()
        {
          new Field() { Id = "25", Text = codeOperation.ToString() },
          new Field() { Id = "27", Text = Inpas._terminalId },
          new Field()
          {
            Id = "0",
            Text = (sum * 100M).ToString((IFormatProvider) CultureInfo.InvariantCulture)
          },
          new Field() { Id = "4", Text = "643" }
        }
      };
      if (!rrn.IsNullOrEmpty())
        command.Field.Add(new Field()
        {
          Id = "14",
          Text = rrn
        });
      InpasResponse inpasResponse = this._currentDriver.DoCommand(command);
      string str = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "39"))?.Text ?? "0";
      if (str == "1")
      {
        slip = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "90"))?.Text ?? string.Empty;
        rrn = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "14"))?.Text ?? string.Empty;
        return true;
      }
      string deviceMessage = inpasResponse.Field.SingleOrDefault<Field>((Func<Field, bool>) (x => x.Id == "19"))?.Text ?? string.Empty + Other.NewLine(2);
      switch (str)
      {
        case "0":
          deviceMessage += Translate.Inpas_Не_удалось_получить_статус_операции__повторите_попытку_оплаты_;
          break;
        case "16":
          deviceMessage += Translate.Inpas_Отказано_в_проведении_операции;
          break;
      }
      if (codeOperation != 4)
        throw new AcquiringException((IDevice) this, deviceMessage);
      LogHelper.Debug("Ошибка при отмене операции " + deviceMessage);
      slip = "";
      return false;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => nameof (Inpas);
  }
}
