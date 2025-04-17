// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.MobilKassa
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class MobilKassa : IFiscalKkm, IDevice
  {
    private MobilKassaDriver _driver;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name { get; }

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection { get; }

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          this._driver.DoCommand((MobilKassaDriver.MobilKassaCommand) new MobilKassaDriver.ZReportCommand());
          break;
        case ReportTypes.XReport:
          break;
        case ReportTypes.XReportWithGoods:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      throw new NotImplementedException();
    }

    public bool CloseCheck() => throw new NotImplementedException();

    public void CancelCheck() => throw new NotImplementedException();

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      throw new NotImplementedException();
    }

    public bool GetCashSum(out Decimal sum)
    {
      sum = -1M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      throw new NotImplementedException();
    }

    public bool RegisterPayment(CheckPayment payment) => throw new NotImplementedException();

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      throw new NotImplementedException();
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      if (onlyDriverLoad)
        return;
      if (devicesConfig == null)
        devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this._driver = new MobilKassaDriver(devicesConfig.CheckPrinter.Connection.LanPort);
      this._driver.KassaKey = "41192fcccd93a9097843057e60e1da1904798f9f";
    }

    public bool Disconnect() => throw new NotImplementedException();

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper() => throw new NotImplementedException();

    public KkmStatus GetStatus()
    {
      return new KkmStatus()
      {
        KkmState = KkmStatuses.Ready
      };
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => throw new NotImplementedException();

    public bool SendDigitalCheck(string adress) => throw new NotImplementedException();

    public void FeedPaper(int lines) => throw new NotImplementedException();

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
