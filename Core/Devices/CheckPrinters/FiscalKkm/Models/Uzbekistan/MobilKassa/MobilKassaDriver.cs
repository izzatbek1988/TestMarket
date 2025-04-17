// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.MobilKassaDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Timers;
using WebSocketSharp;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class MobilKassaDriver
  {
    private LanConnection _lanConnection;
    private WebSocket ws;

    public string KassaKey { get; set; }

    public MobilKassaDriver(LanConnection connection) => this._lanConnection = connection;

    public bool DoCommand(MobilKassaDriver.MobilKassaCommand command)
    {
      try
      {
        command.Key = this.KassaKey;
        this.ws = new WebSocket(string.Format("ws://{0}:{1}/", (object) this._lanConnection.UrlAddress, (object) this._lanConnection.PortNumber), Array.Empty<string>())
        {
          WaitTime = new TimeSpan(0, 2, 0)
        };
        this.ws.OnMessage += (EventHandler<MessageEventArgs>) ((sender, e) =>
        {
          LogHelper.Debug("Answer:" + e.Data);
          command.Answer = e.Data;
          this.ws.Close(CloseStatusCode.Normal);
        });
        this.ws.OnClose += new EventHandler<CloseEventArgs>(this.Ws_OnClose);
        this.ws.OnError += (EventHandler<ErrorEventArgs>) ((sender, args) => LogHelper.Debug("Error: " + args.Message));
        this.ws.OnOpen += new EventHandler(this.Ws_OnOpen);
        this.ws.Connect();
        string data = JsonConvert.SerializeObject((object) command);
        LogHelper.Debug("command: " + data);
        this.ws.Send(data);
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Interval = 120000.0;
        timer.AutoReset = false;
        timer.Elapsed += (ElapsedEventHandler) ((sender, args) =>
        {
          this.ws.Close(CloseStatusCode.NoStatus);
          throw new TimeoutException();
        });
        timer.Start();
        while (this.ws.IsAlive)
          Thread.Sleep(1000);
        return true;
      }
      catch
      {
        LogHelper.Debug("Таймаут для команды на терминал ПриватБанка");
        return false;
      }
    }

    private void Ws_OnOpen(object sender, EventArgs e) => LogHelper.Debug("Open ");

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
      LogHelper.Debug("Closed: " + e.Reason);
    }

    public abstract class MobilKassaCommand
    {
      [JsonProperty("actionCode")]
      public virtual string ActionCode { get; }

      [JsonProperty("key")]
      public string Key { get; set; }

      [JsonProperty("id")]
      public string Id { get; set; } = Guid.NewGuid().ToString();

      [JsonIgnore]
      public string Answer { get; set; }
    }

    public class ZReportCommand : MobilKassaDriver.MobilKassaCommand
    {
      public override string ActionCode => "REPORT_CLOSE";
    }
  }
}
