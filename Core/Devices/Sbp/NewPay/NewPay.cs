// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.NewPay
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class NewPay : ISbp, IDevice
  {
    private readonly Decimal _sum;
    private readonly string _idKassa;
    private string _webSocketServer;
    public static string _saleToken;
    private bool _isGetStatus;
    private ClientWebSocket _clientWebSocket;
    public string _wsAnswer;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    private NewPayDriver Driver { get; set; }

    public NewPay(Decimal sum, string saleToken)
    {
      SBP sbp = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP;
      this.Driver = new NewPayDriver(sbp.ClientSecret.DecryptedValue);
      this._sum = sum;
      NewPay._saleToken = saleToken;
      this._idKassa = sbp.PayQrDeviceID;
    }

    public string Name => "Yandex.Pay";

    public bool GetToken(bool isReturn = true) => true;

    public bool GetQr(out string payLoad, out string rrn, bool isReturn = true)
    {
      payLoad = isReturn ? this.Return() : this.Sale();
      rrn = NewPay._saleToken;
      return true;
    }

    public List<NewPayDriver.GetAccountsCommand.Account> GetListAccount()
    {
      NewPayDriver.GetAccountsCommand command = new NewPayDriver.GetAccountsCommand();
      this.Driver.DoCommand((NewPayDriver.NewPayCommand) command);
      this.CheckError(command.ErrorResult);
      return command.Result;
    }

    private string Sale()
    {
      NewPayDriver.CreateOperationCommand command = new NewPayDriver.CreateOperationCommand()
      {
        Sum = this._sum,
        KeyKassa = this._idKassa
      };
      this.Driver.DoCommand((NewPayDriver.NewPayCommand) command);
      this.CheckError((NewPayDriver.NewPayAnswer) command.Result);
      this._webSocketServer = command.Result.DataSocket.WebSocketServer;
      NewPay._saleToken = command.Result.SaleToken;
      return command.Result.Qr;
    }

    private string Return()
    {
      NewPayDriver.ReturnOperationCommand command = new NewPayDriver.ReturnOperationCommand()
      {
        Sum = this._sum,
        SaleToken = NewPay._saleToken
      };
      this.Driver.DoCommand((NewPayDriver.NewPayCommand) command);
      this.CheckError(command.Result);
      this._webSocketServer = "";
      NewPay._saleToken = "";
      return "";
    }

    public bool GetStatus(out SpbHelper.EStatusQr status, bool isReturn = true)
    {
      if (isReturn)
      {
        status = SpbHelper.EStatusQr.ACWP;
        return true;
      }
      if (this._isGetStatus)
      {
        if (this._wsAnswer.IsNullOrEmpty())
        {
          status = SpbHelper.EStatusQr.RCVD;
          return true;
        }
        NewPayDriver.OperationAnswer operationAnswer = JsonConvert.DeserializeObject<NewPayDriver.OperationAnswer>(this._wsAnswer);
        status = operationAnswer.Result ? SpbHelper.EStatusQr.ACWP : SpbHelper.EStatusQr.RJCT;
        return true;
      }
      this.WebSocketOpen(this._webSocketServer, new NewPayDriver.DataSocketCommand()
      {
        SaleToken = NewPay._saleToken
      }.ToJsonString());
      status = SpbHelper.EStatusQr.RCVD;
      this._isGetStatus = true;
      return true;
    }

    public bool CancelQr()
    {
      this._clientWebSocket?.Abort();
      NewPayDriver.CancelOperationCommand command = new NewPayDriver.CancelOperationCommand()
      {
        KeyKassa = this._idKassa
      };
      this.Driver.DoCommand((NewPayDriver.NewPayCommand) command);
      this.CheckError(command.Result);
      return true;
    }

    public NewPayDriver.GetStatusOperationCommand.Answer GetStatusOperation()
    {
      NewPayDriver.GetStatusOperationCommand command = new NewPayDriver.GetStatusOperationCommand()
      {
        SaleToken = NewPay._saleToken
      };
      this.Driver.DoCommand((NewPayDriver.NewPayCommand) command);
      this.CheckError((NewPayDriver.NewPayAnswer) command.Result);
      return command.Result;
    }

    private void CheckError(NewPayDriver.NewPayAnswer answer)
    {
      if (answer != null && answer.Code != 1)
        throw new DeviceException(string.Format("{0} (код ошибки {1})", (object) answer.Error, (object) answer.CodeError));
    }

    private async void WebSocketOpen(string url, string ct)
    {
      NewPay newPay = this;
      try
      {
        newPay._clientWebSocket = new ClientWebSocket();
        await newPay._clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
        byte[] bytes = Encoding.UTF8.GetBytes(ct);
        await newPay._clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        // ISSUE: reference to a compiler-generated method
        await Task.Run(new System.Action(newPay.\u003CWebSocketOpen\u003Eb__24_0));
      }
      catch
      {
        LogHelper.Debug("Таймаут для команды Яндекс Пэй");
      }
    }
  }
}
