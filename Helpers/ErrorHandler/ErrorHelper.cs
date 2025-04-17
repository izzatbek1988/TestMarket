// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ErrorHandler.ErrorHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
namespace Gbs.Helpers.ErrorHandler
{
  public static class ErrorHelper
  {
    public static void Add(ErrorHelper.GbsException ex) => throw new NotImplementedException();

    public static void SendToGrafana(ErrorHelper.GbsException ex)
    {
      try
      {
        if (ex.Level.IsEither<MsgException.LevelTypes>(MsgException.LevelTypes.Info, MsgException.LevelTypes.Warm) || ex.MsgType == LogHelper.MsgTypes.None || !ex.SendToTelemetry || DevelopersHelper.IsDebug() || new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Ukraine)
          return;
        TaskHelper.TaskRun((Action) (() => ServerScriptsHelper.SendException(ex)), false);
      }
      catch (Exception ex1)
      {
      }
    }

    public class GbsException : Exception
    {
      [JsonConverter(typeof (StringEnumConverter))]
      public LogHelper.MsgTypes MsgType { get; set; } = LogHelper.MsgTypes.Full;

      [JsonConverter(typeof (StringEnumConverter))]
      public ErrorHelper.ErrorDirections Direction { get; set; }

      [JsonConverter(typeof (StringEnumConverter))]
      public MsgException.LevelTypes Level { get; set; } = MsgException.LevelTypes.Error;

      [JsonIgnore]
      public bool SendToTelemetry { get; set; } = true;

      [JsonIgnore]
      public bool UrlCode { get; set; }

      public GbsException(string msg)
        : base(msg)
      {
      }

      public GbsException(string msg, Exception ex)
        : base(msg, ex)
      {
      }

      public GbsException(
        string msg,
        Exception ex,
        LogHelper.MsgTypes msgType,
        ErrorHelper.ErrorDirections direction,
        bool sendToTelemetry,
        MsgException.LevelTypes level)
        : base(msg, ex)
      {
        this.MsgType = msgType;
        this.Direction = direction;
        this.SendToTelemetry = sendToTelemetry;
        this.Level = level;
      }
    }

    public enum ErrorDirections
    {
      Unknown,
      Inner,
      Outer,
    }
  }
}
