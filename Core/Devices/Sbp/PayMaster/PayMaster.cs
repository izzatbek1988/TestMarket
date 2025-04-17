// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.PayMaster
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class PayMaster : ISbp, IDevice
  {
    private readonly string _posid;
    private readonly Decimal _sum;
    private long _orderid;
    private long _returnId;

    private PayMasterDriver Driver { get; set; }

    public PayMaster(Decimal sum, long orderId, long returnId)
    {
      SBP sbp = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP;
      this.Driver = new PayMasterDriver(sbp.ClientSecret.DecryptedValue);
      this._sum = sum;
      this._orderid = orderId;
      this._posid = sbp.PayQrClientID;
      this._returnId = returnId;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => nameof (PayMaster);

    public bool GetToken(bool isReturn = true) => true;

    public bool GetQr(out string payLoad, out string rrn, bool isReturn = true)
    {
      rrn = "";
      if (isReturn)
      {
        PayMasterDriver.GenerateQrCodeReturnCommand command = new PayMasterDriver.GenerateQrCodeReturnCommand()
        {
          refundorder = new PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturn()
          {
            amount = this._sum,
            orderid = this._orderid,
            posid = this._posid,
            sequenceid = this._returnId
          }
        };
        this.Driver.DoCommand((PayMasterDriver.PayMasterCommand) command);
        if (!command.Result.retval.IsEither<int>(1072, 0))
          throw new AcquiringException((IDevice) this, command.Result.retdesc);
        payLoad = "";
        return true;
      }
      PayMasterDriver.GenerateQrCodeCommand command1 = new PayMasterDriver.GenerateQrCodeCommand()
      {
        generate_qr_code = new PayMasterDriver.GenerateQrCodeCommand.GenerateQrCode()
        {
          amount = this._sum,
          orderid = this._orderid,
          posid = this._posid
        }
      };
      this.Driver.DoCommand((PayMasterDriver.PayMasterCommand) command1);
      if (command1.Result.retval != 0)
        throw new AcquiringException((IDevice) this, command1.Result.retdesc);
      payLoad = command1.Result.generate_qr_code.qr;
      return true;
    }

    public bool GetStatus(out SpbHelper.EStatusQr status, bool isReturn = true)
    {
      if (isReturn)
      {
        PayMasterDriver.GetStatusReturnCommand command = new PayMasterDriver.GetStatusReturnCommand()
        {
          GetRefunds = new PayMasterDriver.GetStatusReturnCommand.GetRefundsItem()
          {
            Posid = this._posid,
            Orderid = this._orderid
          },
          Posid = this._posid
        };
        this.Driver.DoCommand((PayMasterDriver.PayMasterCommand) command);
        if (command.Result.retval != 0)
          throw new AcquiringException((IDevice) this, command.Result.retdesc);
        switch (command.Result.Refunds.First<PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer.InvoicesItem>((Func<PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer.InvoicesItem, bool>) (x => x.Orderid == this._orderid)).State)
        {
          case 2:
            status = SpbHelper.EStatusQr.RCVD;
            return true;
          case 3:
            status = SpbHelper.EStatusQr.ACWP;
            return true;
          case 4:
            status = SpbHelper.EStatusQr.RJCT;
            return true;
          default:
            status = SpbHelper.EStatusQr.RJCT;
            return false;
        }
      }
      else
      {
        PayMasterDriver.GetStatusPayCommand command = new PayMasterDriver.GetStatusPayCommand()
        {
          Outinvoices = new PayMasterDriver.GetStatusPayCommand.OutinvoicesList()
          {
            Order = new List<PayMasterDriver.GetStatusPayCommand.Order>()
            {
              new PayMasterDriver.GetStatusPayCommand.Order()
              {
                Orderid = this._orderid
              }
            }
          },
          Posid = this._posid
        };
        this.Driver.DoCommand((PayMasterDriver.PayMasterCommand) command);
        if (command.Result.retval != 0)
          throw new AcquiringException((IDevice) this, command.Result.retdesc);
        switch (command.Result.Invoices.First<PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer.InvoicesItem>((Func<PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer.InvoicesItem, bool>) (x => x.Orderid == this._orderid)).State)
        {
          case 4:
            status = SpbHelper.EStatusQr.NTST;
            return true;
          case 5:
            status = SpbHelper.EStatusQr.ACWP;
            return true;
          case 6:
            status = SpbHelper.EStatusQr.RJCT;
            return true;
          case 8:
            status = SpbHelper.EStatusQr.RCVD;
            return true;
          case 12:
            status = SpbHelper.EStatusQr.RJCT;
            return true;
          default:
            status = SpbHelper.EStatusQr.RJCT;
            return false;
        }
      }
    }

    public bool CancelQr()
    {
      PayMasterDriver.CancelOrderCommand command = new PayMasterDriver.CancelOrderCommand()
      {
        CancelOrder = new PayMasterDriver.CancelOrderCommand.CancelOrderItem()
        {
          Orderid = this._orderid,
          Posid = this._posid
        }
      };
      this.Driver.DoCommand((PayMasterDriver.PayMasterCommand) command);
      if (command.Result.retval != 0)
        throw new AcquiringException((IDevice) this, command.Result.retdesc);
      return true;
    }
  }
}
