// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.AtolPay
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Resources.Localizations;
using System;
using WebSocketSharp;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class AtolPay : ISbp, IDevice
  {
    private string _orderId;
    private readonly Decimal _sum;
    private readonly string _idKassa;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    private AtolPayDriver Driver { get; set; }

    public AtolPay(Decimal sum, string returnId)
    {
      SBP sbp = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP;
      this.Driver = new AtolPayDriver(sbp.ClientSecret.DecryptedValue);
      this._sum = sum;
      this._idKassa = Ext.IsNullOrEmpty(sbp.PayQrDeviceID) ? (string) null : sbp.PayQrDeviceID;
      this._orderId = returnId;
    }

    public string Name => nameof (AtolPay);

    public bool GetToken(bool isReturn = true) => true;

    public bool GetQr(out string payLoad, out string rrn, bool isReturn = true)
    {
      payLoad = isReturn ? this.Return() : this.Sale();
      rrn = this._orderId;
      return true;
    }

    private string Sale()
    {
      AtolPayDriver.CreateOperationCommand command = new AtolPayDriver.CreateOperationCommand()
      {
        Sum = (int) (this._sum * 100M),
        KassaId = Ext.IsNullOrEmpty(this._idKassa) ? (string) null : this._idKassa
      };
      this.Driver.DoCommand((AtolPayDriver.AtolPayCommand) command);
      this.CheckError((AtolPayDriver.AtolPayAnswer) command.Result);
      this._orderId = command.Result.Data.OrderId;
      return command.Result.Data.Qr;
    }

    private string Return()
    {
      AtolPayDriver.ReturnOperationCommand command = new AtolPayDriver.ReturnOperationCommand()
      {
        Sum = (int) (this._sum * 100M),
        OrderId = this._orderId
      };
      this.Driver.DoCommand((AtolPayDriver.AtolPayCommand) command);
      this.CheckError(command.Result);
      return "";
    }

    public bool GetStatus(out SpbHelper.EStatusQr status, bool isReturn = true)
    {
      AtolPayDriver.GetStatusOperationCommand command = new AtolPayDriver.GetStatusOperationCommand()
      {
        OrderId = this._orderId
      };
      this.Driver.DoCommand((AtolPayDriver.AtolPayCommand) command);
      this.CheckError((AtolPayDriver.AtolPayAnswer) command.Result);
      string status1 = command.Result.Data.Status;
      status = status1 == "COMPLETED" || status1 == "REFUNDED" ? SpbHelper.EStatusQr.ACWP : SpbHelper.EStatusQr.RCVD;
      return true;
    }

    public bool CancelQr() => true;

    private void CheckError(AtolPayDriver.AtolPayAnswer answer)
    {
      if (answer != null && answer.Status != "success")
        throw new DeviceException(string.Format(Translate.HdmDriver_CheckError__0___код_ошибки__1__, (object) answer.ErrorMessage, (object) answer.ErrorCode));
    }
  }
}
