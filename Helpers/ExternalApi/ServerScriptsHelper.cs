// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ServerScriptsHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  public class ServerScriptsHelper
  {
    private static List<string> UrlsList = new List<string>()
    {
      "https://stat.app-pos.ru/",
      "https://stat.app-pos.com/"
    };
    private static string _url;

    private static void PostRequest(ServerScriptsHelper.ServerRequest request, bool writeLog)
    {
      foreach (string urls in ServerScriptsHelper.UrlsList)
      {
        if (NetworkHelper.PingHost(new Uri(urls).Host))
        {
          ServerScriptsHelper._url = urls;
          break;
        }
      }
      if (ServerScriptsHelper._url.IsNullOrEmpty())
        return;
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.DefaultConnectionLimit = 9999;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      RestHelper restHelper = new RestHelper(ServerScriptsHelper._url, new int?(), request.ToJsonString(true), writeLog);
      restHelper.CreateCommand(request.Path, TypeRestRequest.Post);
      restHelper.AddHeader("X-Api-Key", "ad4152a1-fd89-4649-a2a4-303ab42767f2");
      restHelper.DoCommand();
      request.AnswerString = restHelper.Answer;
    }

    public static void SendException(ErrorHelper.GbsException exception)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      ServerScriptsHelper.GrafanaExceptionRequest request = new ServerScriptsHelper.GrafanaExceptionRequest()
      {
        Content = exception.ToJsonString(true),
        Message = exception.Message,
        Direction = exception.Direction,
        Logs = string.Join("\r\n", (IEnumerable<string>) LogHelper.LogQueue)
      };
      ServerScriptsHelper.PostRequest((ServerScriptsHelper.ServerRequest) request, true);
      if (request.Response.Result != ServerScriptsHelper.ResultEnum.Ok)
        LogHelper.Debug("Не удалось передать данные в Графану, message = " + request.Response.Message);
      stopwatch.Stop();
      DevelopersHelper.ShowNotification("send to grafana: " + ((double) stopwatch.ElapsedMilliseconds / 1000.0).ToString());
    }

    public static void SendPing(string geoInfo)
    {
      Settings settings = new ConfigsRepository<Settings>().Get();
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      Gbs.Core.Config.DataBase dataBase1 = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      Integrations integrations = new ConfigsRepository<Integrations>().Get();
      using (Gbs.Core.Db.DataBase dataBase2 = Data.GetDataBase())
      {
        IQueryable<DOCUMENTS> table = dataBase2.GetTable<DOCUMENTS>();
        List<GoodGroups.Group> activeItems = new GoodGroupsRepository(dataBase2).GetActiveItems();
        ServerScriptsHelper.PingsRequest pingsRequest1 = new ServerScriptsHelper.PingsRequest();
        ServerScriptsHelper.PingsRequest pingsRequest2 = pingsRequest1;
        ServerScriptsHelper.PingsRequest.Content content1 = new ServerScriptsHelper.PingsRequest.Content();
        content1.Environment = new ServerScriptsHelper.PingsRequest.Environment()
        {
          Os = new ServerScriptsHelper.PingsRequest.Environment.OsInfo()
          {
            Version = HardwareInfo.OsVersion()
          },
          Cpu = new ServerScriptsHelper.PingsRequest.Environment.CpuInfo()
          {
            Name = HardwareInfo.CpuName(),
            Manufacturer = HardwareInfo.CpuManufacturer()
          }
        };
        content1.GeoLocation = geoInfo.IsNullOrEmpty() ? (object) null : JsonConvert.DeserializeObject(geoInfo);
        ServerScriptsHelper.PingsRequest.Content content2 = content1;
        ServerScriptsHelper.PingsRequest.Statistics statistics1 = new ServerScriptsHelper.PingsRequest.Statistics();
        ServerScriptsHelper.PingsRequest.Statistics statistics2 = statistics1;
        ServerScriptsHelper.PingsRequest.Main main1 = new ServerScriptsHelper.PingsRequest.Main();
        main1.Country = settings.Interface.Country.ToString();
        main1.Language = settings.Interface.Language.ToString();
        main1.UpdateChannel = settings.Other.UpdateConfig.VersionUpdate.ToString();
        main1.WorkMode = dataBase1.ModeProgram.ToString();
        ServerScriptsHelper.PingsRequest.Main main2 = main1;
        DOCUMENTS documents = table.FirstOrDefault<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME > new DateTime(2018, 1, 1)));
        DateTime dateTime = documents != null ? documents.DATE_TIME : DateTime.MinValue;
        main2.DateStart = dateTime;
        ServerScriptsHelper.PingsRequest.Main main3 = main1;
        statistics2.Main = main3;
        ServerScriptsHelper.PingsRequest.Statistics statistics3 = statistics1;
        ServerScriptsHelper.PingsRequest.DataAmount dataAmount1 = new ServerScriptsHelper.PingsRequest.DataAmount();
        dataAmount1.Clients = dataBase2.GetTable<CLIENTS>().Count<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.IS_DELETED == false));
        dataAmount1.Goods = dataBase2.GetTable<GOODS>().Count<GOODS>((Expression<Func<GOODS, bool>>) (x => x.IS_DELETED == false));
        dataAmount1.Users = dataBase2.GetTable<USERS>().Count<USERS>((Expression<Func<USERS, bool>>) (x => x.IS_DELETED == false));
        dataAmount1.GoodGroups = activeItems.Count;
        dataAmount1.DocSales = table.Count<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false && x.TYPE == 1));
        dataAmount1.DocWaybills = table.Count<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false && x.TYPE == 3));
        dataAmount1.DocOther = table.Count<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false && x.TYPE != 1 && x.TYPE != 3));
        dataAmount1.Sections = dataBase2.GetTable<SECTIONS>().Count<SECTIONS>((Expression<Func<SECTIONS, bool>>) (x => x.IS_DELETED == false));
        ServerScriptsHelper.PingsRequest.DataAmount dataAmount2 = dataAmount1;
        statistics3.DataAmount = dataAmount2;
        statistics1.Devices = new ServerScriptsHelper.PingsRequest.Devices()
        {
          Aquaring = devices.AcquiringTerminal.Type.ToString(),
          CheckPrinter = new ServerScriptsHelper.PingsRequest.CheckPrinter()
          {
            FfdVersion = devices.CheckPrinter.FiscalKkm.FfdVersion.ToString(),
            KkmType = devices.CheckPrinter.FiscalKkm.KkmType.ToString(),
            Type = devices.CheckPrinter.Type.ToString()
          },
          LabelScale = devices.ScaleWithLable.Type.ToString(),
          Sbp = devices.SBP.Type.ToString(),
          Scale = devices.Scale.Type.ToString()
        };
        statistics1.Legally = new ServerScriptsHelper.PingsRequest.Legally()
        {
          Crpt = new ServerScriptsHelper.PingsRequest.Crpt()
          {
            Groups = activeItems.GroupBy<GoodGroups.Group, GlobalDictionaries.RuMarkedProductionTypes>((Func<GoodGroups.Group, GlobalDictionaries.RuMarkedProductionTypes>) (x => x.RuMarkedProductionType)).Where<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>>((Func<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>, bool>) (x => x.Key != 0)).Select<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>, string>((Func<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>, string>) (x => x.Key.ToString())).ToList<string>(),
            OnlineCheck = settings.Sales.IsCheckMarkInfoTrueApi,
            SalesBlock = settings.Sales.IsTabooSaleNoСorrected
          },
          Egais = integrations.Egais.IsActive
        };
        statistics1.RemoteControl = new ServerScriptsHelper.PingsRequest.RemoteControl()
        {
          Cloud = settings.RemoteControl.Cloud.IsActive,
          Email = settings.RemoteControl.Email.IsActive,
          Tg = settings.RemoteControl.Telegram.IsActive
        };
        ServerScriptsHelper.PingsRequest.Statistics statistics4 = statistics1;
        content2.UsageStatistic = statistics4;
        ServerScriptsHelper.PingsRequest.Content content3 = content1;
        pingsRequest2.ContentInfo = content3;
        ServerScriptsHelper.PingsRequest request = pingsRequest1;
        ServerScriptsHelper.PostRequest((ServerScriptsHelper.ServerRequest) request, false);
        ServerScriptsHelper.ServerResponse response = request.Response;
        if ((response != null ? (response.Result != 0 ? 1 : 0) : 1) == 0)
          return;
        LogHelper.Debug("Не удалось передать данные в Графану, message = " + request.Response.Message);
      }
    }

    public abstract class ServerRequest
    {
      [JsonIgnore]
      public virtual string Path { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public class ServerResponse
    {
      [JsonProperty("result")]
      public ServerScriptsHelper.ResultEnum Result { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
    }

    public enum ResultEnum
    {
      [EnumMember(Value = "ok")] Ok,
      [EnumMember(Value = "error")] Error,
    }

    public class GrafanaExceptionRequest : ServerScriptsHelper.ServerRequest
    {
      [JsonIgnore]
      public ServerScriptsHelper.ServerResponse Response
      {
        get => JsonConvert.DeserializeObject<ServerScriptsHelper.ServerResponse>(this.AnswerString);
      }

      public override string Path => "write_event.php";

      [JsonProperty("dateTime")]
      public DateTime DateTime { get; set; } = DateTime.Now;

      [JsonProperty("gbsId")]
      public string GbsId { get; set; } = LicenseHelper.GetInfo().GbsId;

      [JsonProperty("type")]
      public string Type { get; set; } = "error";

      [JsonProperty("content")]
      public string Content { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }

      [JsonProperty("version")]
      public string Version { get; set; } = ApplicationInfo.GetInstance().GbsVersion.ToString();

      [JsonProperty("direction")]
      [JsonConverter(typeof (StringEnumConverter))]
      public ErrorHelper.ErrorDirections Direction { get; set; }

      [JsonProperty("logs")]
      public string Logs { get; set; }
    }

    public class PingsRequest : ServerScriptsHelper.ServerRequest
    {
      [JsonIgnore]
      public ServerScriptsHelper.ServerResponse Response
      {
        get => JsonConvert.DeserializeObject<ServerScriptsHelper.ServerResponse>(this.AnswerString);
      }

      public override string Path => "write_event-pings.php";

      [JsonProperty("gbsId")]
      public string GbsId { get; set; } = LicenseHelper.GetInfo().GbsId;

      [JsonProperty("content")]
      public ServerScriptsHelper.PingsRequest.Content ContentInfo { get; set; }

      [JsonProperty("version")]
      public string Version { get; set; } = ApplicationInfo.GetInstance().GbsVersion.ToString();

      public class Content
      {
        [JsonProperty("geolocation")]
        public object GeoLocation { get; set; }

        [JsonProperty("usageStatistic")]
        public ServerScriptsHelper.PingsRequest.Statistics UsageStatistic { get; set; }

        [JsonProperty("environment")]
        public ServerScriptsHelper.PingsRequest.Environment Environment { get; set; }

        [JsonProperty("dbUid")]
        public Guid DbUid => UidDb.GetUid().EntityUid;

        [JsonProperty("vendorCodeName")]
        public string VendorCodeName => Vendor.GetConfig()?.CodeName;
      }

      public class Environment
      {
        [JsonProperty("cpu")]
        public ServerScriptsHelper.PingsRequest.Environment.CpuInfo Cpu { get; set; }

        [JsonProperty("os")]
        public ServerScriptsHelper.PingsRequest.Environment.OsInfo Os { get; set; }

        public class CpuInfo
        {
          [JsonProperty("name")]
          public string Name { get; set; }

          [JsonProperty("manufacturer")]
          public string Manufacturer { get; set; }
        }

        public class OsInfo
        {
          [JsonProperty("version")]
          public string Version { get; set; }

          [JsonProperty("bitSystem")]
          public string BitSystem => !System.Environment.Is64BitOperatingSystem ? "x32" : "x64";

          [JsonProperty("bitProgram")]
          public string BitProgram => !System.Environment.Is64BitProcess ? "x32" : "x64";
        }
      }

      public class CheckPrinter
      {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("kkmType")]
        public string KkmType { get; set; }

        [JsonProperty("ffdVersion")]
        public string FfdVersion { get; set; }
      }

      public class Crpt
      {
        [JsonProperty("onlineCheck")]
        public bool OnlineCheck { get; set; }

        [JsonProperty("salesBlock")]
        public bool SalesBlock { get; set; }

        [JsonProperty("groups")]
        public List<string> Groups { get; set; }
      }

      public class DataAmount
      {
        [JsonProperty("goodGroups")]
        public int GoodGroups { get; set; }

        [JsonProperty("goods")]
        public int Goods { get; set; }

        [JsonProperty("docSales")]
        public int DocSales { get; set; }

        [JsonProperty("docWaybills")]
        public int DocWaybills { get; set; }

        [JsonProperty("docOther")]
        public int DocOther { get; set; }

        [JsonProperty("clients")]
        public int Clients { get; set; }

        [JsonProperty("users")]
        public int Users { get; set; }

        [JsonProperty("sections")]
        public int Sections { get; set; }
      }

      public class Devices
      {
        [JsonProperty("checkPrinter")]
        public ServerScriptsHelper.PingsRequest.CheckPrinter CheckPrinter { get; set; }

        [JsonProperty("aquaring")]
        public string Aquaring { get; set; }

        [JsonProperty("sbp")]
        public string Sbp { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }

        [JsonProperty("labelScale")]
        public string LabelScale { get; set; }
      }

      public class Legally
      {
        [JsonProperty("egais")]
        public bool Egais { get; set; }

        [JsonProperty("crpt")]
        public ServerScriptsHelper.PingsRequest.Crpt Crpt { get; set; }
      }

      public class Main
      {
        [JsonProperty("workMode")]
        public string WorkMode { get; set; }

        [JsonProperty("dateStart")]
        public DateTime DateStart { get; set; }

        [JsonProperty("updateChannel")]
        public string UpdateChannel { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
      }

      public class RemoteControl
      {
        [JsonProperty("cloud")]
        public bool Cloud { get; set; }

        [JsonProperty("email")]
        public bool Email { get; set; }

        [JsonProperty("tg")]
        public bool Tg { get; set; }
      }

      public class Statistics
      {
        [JsonProperty("main")]
        public ServerScriptsHelper.PingsRequest.Main Main { get; set; }

        [JsonProperty("dataAmount")]
        public ServerScriptsHelper.PingsRequest.DataAmount DataAmount { get; set; }

        [JsonProperty("devices")]
        public ServerScriptsHelper.PingsRequest.Devices Devices { get; set; }

        [JsonProperty("remoteControl")]
        public ServerScriptsHelper.PingsRequest.RemoteControl RemoteControl { get; set; }

        [JsonProperty("legally")]
        public ServerScriptsHelper.PingsRequest.Legally Legally { get; set; }
      }
    }
  }
}
