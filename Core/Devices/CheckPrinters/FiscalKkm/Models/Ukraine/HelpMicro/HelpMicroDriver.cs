// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.HelpMicroDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class HelpMicroDriver
  {
    private readonly LanConnection _lanConnection;

    private string _kkmUserName => "service";

    private string _kkmUserPass => "751426";

    public HelpMicroDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public string SendGetRequest(string command)
    {
      ServicePointManager.Expect100Continue = false;
      LogHelper.Debug("HELP MICRO COMMAND: " + command);
      string str = this._lanConnection.UrlAddress + "/";
      if (!str.ToLower().StartsWith("http://"))
        str = "http://" + str;
      WebRequest webRequest = WebRequest.Create(str + command);
      webRequest.Credentials = (ICredentials) new CredentialCache()
      {
        {
          new Uri(str + command),
          "Digest",
          new NetworkCredential(this._kkmUserName, this._kkmUserPass)
        }
      };
      return new StreamReader(webRequest.GetResponse().GetResponseStream() ?? throw new ArgumentNullException("objStream")).ReadToEnd();
    }

    public string SendPostRequest(HelpMicroDriver.HelpMicroCommand command)
    {
      ServicePointManager.Expect100Continue = false;
      string s = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore
      });
      LogHelper.Debug("HELP MICRO JSON: " + Other.NewLine() + s);
      LogHelper.Debug("HELP MICRO COMMAND: " + command.Command);
      byte[] bytes = Encoding.UTF8.GetBytes(s);
      string str = this._lanConnection.UrlAddress + "/";
      if (!str.ToLower().StartsWith("http://"))
        str = "http://" + str;
      WebRequest webRequest = WebRequest.Create(str + command.Command);
      webRequest.Method = "POST";
      webRequest.ContentLength = (long) bytes.Length;
      webRequest.ContentType = "text/plain;charset=UTF-8";
      webRequest.Credentials = (ICredentials) new CredentialCache()
      {
        {
          new Uri(str + command?.ToString()),
          "Digest",
          new NetworkCredential(this._kkmUserName, this._kkmUserPass)
        }
      };
      using (Stream requestStream = webRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
      StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream() ?? throw new ArgumentNullException("objStream"));
      command.Answer = streamReader.ReadToEnd();
      LogHelper.Debug("HELP MICRO ANSWER: " + Other.NewLine() + command.Answer);
      return command.DeviceAnswer?.err?.e;
    }

    public class KkmAnswer
    {
      public HelpMicroDriver.KkmAnswer.Err err { get; set; }

      public class Err
      {
        public string e { get; set; }

        public int line { get; set; }
      }
    }

    public abstract class HelpMicroCommand
    {
      [JsonIgnore]
      public virtual string Command { get; } = string.Empty;

      [JsonIgnore]
      public string Answer { get; set; }

      [JsonIgnore]
      public HelpMicroDriver.KkmAnswer DeviceAnswer
      {
        get => JsonConvert.DeserializeObject<HelpMicroDriver.KkmAnswer>(this.Answer);
      }
    }

    public class GetXReportCommand : HelpMicroDriver.HelpMicroCommand
    {
      public override string Command => "cgi/proc/printreport?10";
    }

    public class GetZReportCommand : HelpMicroDriver.HelpMicroCommand
    {
      public override string Command => "cgi/proc/printreport?0";
    }

    public class GetZReportAndGoodsCommand : HelpMicroDriver.HelpMicroCommand
    {
      public override string Command => "cgi/proc/printreport?20";
    }

    public class KkmRegistreCheckCommand : HelpMicroDriver.HelpMicroCommand
    {
      public List<HelpMicroDriver.chequeStrings> F = new List<HelpMicroDriver.chequeStrings>();
      public List<HelpMicroDriver.chequeStrings> R = new List<HelpMicroDriver.chequeStrings>();

      public override string Command => "cgi/chk";
    }

    public class KkmPaymentActionCommand : HelpMicroDriver.HelpMicroCommand
    {
      public List<HelpMicroDriver.chequeStrings> IO = new List<HelpMicroDriver.chequeStrings>();

      public override string Command => "cgi/chk";
    }

    public class PrintNonFiscalTextCommand : HelpMicroDriver.HelpMicroCommand
    {
      public List<HelpMicroDriver.chequeStrings> P = new List<HelpMicroDriver.chequeStrings>();

      public override string Command => "cgi/chk";
    }

    public class GetKkmSumCommand
    {
      public string Command => "cgi/rep/pay";

      public string Answer { get; set; }

      public List<HelpMicroDriver.cashItem> CashItems
      {
        get
        {
          LogHelper.Debug("a: " + this.Answer);
          return JsonConvert.DeserializeObject<List<HelpMicroDriver.cashItem>>(this.Answer);
        }
      }
    }

    public class chequeStrings
    {
      public HelpMicroDriver.chequeStrings.comment C;
      public HelpMicroDriver.chequeStrings.discount D;
      public HelpMicroDriver.chequeStrings.inOut IO;
      public HelpMicroDriver.chequeStrings.nonFiscalText N;
      public HelpMicroDriver.chequeStrings.payment P;
      public HelpMicroDriver.chequeStrings.good S;

      public class payment
      {
        public int no;
        public Decimal sum;
        public string rrn;
        public string card;
        public string bank;
        public string term;
        public string authcode;
        public Decimal fee;
      }

      public class discount
      {
        public int all;
        public Decimal sum;
      }

      public class good
      {
        public int code;
        public string name;
        public Decimal price;
        public Decimal qty;
        public int tax;
        [JsonConverter(typeof (JsonHelper.StrAsIntValueConverter))]
        public string uktzed;

        public List<HelpMicroDriver.chequeStrings.exciseStamp> excise { get; set; } = new List<HelpMicroDriver.chequeStrings.exciseStamp>();
      }

      public class exciseStamp
      {
        public string stamp { get; set; }
      }

      public class comment
      {
        public string cm;
      }

      public class inOut
      {
        public Decimal sum;
      }

      public class nonFiscalText
      {
        public string attr;
        public string cm;
      }
    }

    public class cashItem
    {
      public int no;
      public Decimal sum;
    }
  }
}
