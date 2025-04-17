// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Logging.LogHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.Extensions.Collections;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Gbs.Helpers.Logging
{
  public static class LogHelper
  {
    public static LimitedQueue<string> LogQueue = new LimitedQueue<string>(50);
    private const string MainLoggerName = "mainLogger";
    private const string PerformanceLoggerName = "performanceLogger";
    private const string PrroLoggerName = "prroLogger";
    private const string EgaisLoggerName = "egaisLogger";
    private const string CrptLoggerName = "crptLogger";
    private static Logger _logger;
    private static Logger _performanceLogger;
    private static Logger _prroLogger;
    private static Logger _egaisLogger;
    private static Logger _crptLogger;

    internal static Logger Logger
    {
      get
      {
        if (LogHelper._logger == null)
          LogHelper.ActivateConfig();
        return LogHelper._logger;
      }
    }

    private static Logger PerformanceLogger
    {
      get
      {
        if (LogHelper._performanceLogger == null)
          LogHelper.ActivateConfig();
        return LogHelper._performanceLogger;
      }
    }

    private static Logger PrroLogger
    {
      get
      {
        if (LogHelper._prroLogger == null)
          LogHelper.ActivateConfig();
        return LogHelper._prroLogger;
      }
    }

    private static Logger EgaisLogger
    {
      get
      {
        if (LogHelper._egaisLogger == null)
          LogHelper.ActivateConfig();
        return LogHelper._egaisLogger;
      }
    }

    private static Logger CrptLogger
    {
      get
      {
        if (LogHelper._crptLogger == null)
          LogHelper.ActivateConfig();
        return LogHelper._crptLogger;
      }
    }

    public static void WritePerformanceMessage(string message)
    {
      LogHelper.PerformanceLogger.Info(message);
    }

    [Localizable(false)]
    public static void OnBegin(string message = null)
    {
    }

    [Localizable(false)]
    public static void OnEnd(string message = null)
    {
    }

    [Localizable(false)]
    public static void Trace(string message) => LogHelper.WriteTrace(message);

    private static void WriteTrace(string message)
    {
      try
      {
        LogHelper.Logger.Trace(message);
        LogHelper.AddToLogQueue(message);
      }
      catch (Exception ex)
      {
      }
    }

    private static void AddToLogQueue(string message)
    {
      try
      {
        message = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffff") + " | " + message;
        LogHelper.LogQueue.Enqueue(message);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [Localizable(false)]
    public static void Debug(string message)
    {
      try
      {
        LogHelper.Logger.Debug(message);
        LogHelper.AddToLogQueue(message);
      }
      catch (Exception ex)
      {
        MessageBoxHelper.Error("logger err: " + ex?.ToString());
      }
    }

    [Localizable(false)]
    public static void WriteError(Exception ex, string logMessage = "", bool sendToZidium = true)
    {
      LogHelper.LogErrorInfo logErrorInfo = new LogHelper.LogErrorInfo(ex, logMessage);
      string str = string.Empty;
      try
      {
        str = logErrorInfo.ToJsonString(true).Replace("\\r\\n", "\r\n").Replace("\\n", "\n");
      }
      catch
      {
      }
      if (str.IsNullOrEmpty())
      {
        LogHelper.Logger.Error(ex, logMessage);
        LogHelper.AddToLogQueue(ex.ToString());
      }
      else
      {
        LogHelper.Logger.Error(str);
        LogHelper.AddToLogQueue(str);
      }
      int num = sendToZidium ? 1 : 0;
    }

    [Localizable(true)]
    [Obsolete]
    public static void ShowErrorMgs(Exception ex, string message, LogHelper.MsgTypes type)
    {
      if (type == LogHelper.MsgTypes.None || ex is CancelException)
        return;
      if (GlobalData.IsMarket5ImportAcitve)
      {
        LogHelper.Debug("Сообщение " + message + " не будет показано, т.к. активне процесс импорта из 5й версии");
      }
      else
      {
        string str1 = string.Empty;
        if (ex is IExceptionWithExtMessage exceptionWithExtMessage)
          str1 = exceptionWithExtMessage.ExtMessage;
        MsgException.LevelTypes levelTypes = MsgException.LevelTypes.Error;
        if (ex is ErrorHelper.GbsException gbsException)
        {
          str1 = gbsException.Message;
          levelTypes = gbsException.Level;
        }
        message = message.IsNullOrEmpty() ? string.Empty : message + Gbs.Helpers.Other.NewLine(2);
        string str2 = str1.IsNullOrEmpty() ? ExceptionsConverter.Convert(ex) : str1 + Gbs.Helpers.Other.NewLine(2);
        string message1 = ex?.InnerException?.Message;
        string message2 = (message + str2 + "\n" + message1).Trim(' ', '\n').Replace("\n\n", "\n");
        switch (type)
        {
          case LogHelper.MsgTypes.Notification:
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = string.Format(Translate.LogHelper_ShowErrorMgs_Ошибка_в__0_, (object) PartnersHelper.ProgramName()),
              Text = message2,
              Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
            });
            break;
          case LogHelper.MsgTypes.MessageBox:
          case LogHelper.MsgTypes.Full:
            switch (levelTypes)
            {
              case MsgException.LevelTypes.Info:
                int num = (int) MessageBoxHelper.Show(message2);
                return;
              case MsgException.LevelTypes.Warm:
                MessageBoxHelper.Warning(message2);
                return;
              case MsgException.LevelTypes.Error:
                MessageBoxHelper.Error(message2);
                return;
              default:
                MessageBoxHelper.Error(message2);
                return;
            }
        }
      }
    }

    [Localizable(false)]
    public static void Error(
      Exception ex,
      string message,
      bool showErrorToUser = true,
      bool sendErrorToZidium = true,
      bool writeToFile = true)
    {
      try
      {
        if (writeToFile)
          LogHelper.WriteError(ex, message, false);
        if (DevelopersHelper.IsUnitTest())
          return;
        try
        {
          if (!showErrorToUser)
            return;
          LogHelper.SendToTelemetry(ex);
          if (new ConfigsRepository<Settings>().Get().Interface.Language != GlobalDictionaries.Languages.Russian)
            message = "";
          LogHelper.ShowErrorMgs(ex, message, LogHelper.MsgTypes.Full);
        }
        catch
        {
          int num = (int) MessageBoxHelper.Show("Возникла ошибка в работе программы. \r\nИсключение: " + ex.Message, PartnersHelper.ProgramName(), icon: MessageBoxImage.Hand);
        }
      }
      catch (Exception ex1)
      {
        Gbs.Helpers.Other.ConsoleWrite("Не удалось зафиксировать ошибку в лог" + ex1?.ToString());
      }
    }

    private static void SendToTelemetry(Exception ex)
    {
      try
      {
        if (ex == null)
          return;
        ErrorHelper.GbsException ex1 = !(ex is ErrorHelper.GbsException gbsException) ? new ErrorHelper.GbsException(ex.Message, ex) : gbsException;
        if (ex.InnerException is ErrorHelper.GbsException innerException)
          ex1 = innerException;
        ErrorHelper.SendToGrafana(ex1);
      }
      catch (Exception ex2)
      {
      }
    }

    [Localizable(false)]
    public static void WriteToEgaisLog(string msg, Exception ex = null)
    {
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
        return;
      if (ex == null)
        LogHelper.EgaisLogger.Trace(msg);
      else
        LogHelper.EgaisLogger.Error(ex, msg);
    }

    public static void WriteToCrptLog(string msg, NLog.LogLevel logLevel, Exception ex = null)
    {
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
        return;
      if (ex != null)
      {
        LogHelper.WriteError(ex, msg, false);
        LogHelper.CrptLogger.Log(logLevel, msg + ex.ToJsonString());
      }
      else
      {
        LogHelper.Logger.Log(logLevel, msg);
        LogHelper.CrptLogger.Log(logLevel, msg);
      }
    }

    private static void AddFileLoggingRule(
      LoggingConfiguration cfg,
      LayoutWithHeaderAndFooter layout,
      string fileName,
      NLog.LogLevel minLevel,
      NLog.LogLevel maxLevel,
      string loggerName)
    {
      string str = ApplicationInfo.GetInstance().Paths.LogsPath + "${date:format=yyyy-MM-dd}\\";
      FileTarget fileTarget = new FileTarget(Guid.NewGuid().ToString());
      fileTarget.FileName = (Layout) (str + fileName);
      fileTarget.Layout = (Layout) layout;
      fileTarget.Encoding = System.Text.Encoding.UTF8;
      fileTarget.AutoFlush = true;
      fileTarget.KeepFileOpen = true;
      FileTarget wrappedTarget = fileTarget;
      AsyncTargetWrapper asyncTargetWrapper1 = new AsyncTargetWrapper((Target) wrappedTarget);
      asyncTargetWrapper1.Name = wrappedTarget.Name;
      asyncTargetWrapper1.QueueLimit = 5000;
      asyncTargetWrapper1.OverflowAction = AsyncTargetWrapperOverflowAction.Discard;
      AsyncTargetWrapper asyncTargetWrapper2 = asyncTargetWrapper1;
      SimpleConfigurator.ConfigureForTargetLogging((Target) asyncTargetWrapper2);
      cfg.AddRule(minLevel, maxLevel, (Target) asyncTargetWrapper2, loggerName);
    }

    private static void ActivateConfig()
    {
      try
      {
        LoggingConfiguration cfg = new LoggingConfiguration();
        LogHelper.CreateMainLogger(cfg);
        LogHelper.CreatePerformanceLogger(cfg);
        LogHelper.CreatePrroLogger(cfg);
        LogHelper.CreateEgaisLogger(cfg);
        LogHelper.CreateCrptLogger(cfg);
        LogManager.Configuration = cfg;
        LogHelper._performanceLogger = LogManager.GetLogger("performanceLogger");
        LogHelper._logger = LogManager.GetLogger("mainLogger");
        LogHelper._prroLogger = LogManager.GetLogger("prroLogger");
        LogHelper._egaisLogger = LogManager.GetLogger("egaisLogger");
        LogHelper._crptLogger = LogManager.GetLogger("crptLogger");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBoxHelper.Show(Translate.LogHelper_Ошибка_активации_настроек_логгера__ + ex?.ToString());
      }
    }

    private static void CreateMainLogger(LoggingConfiguration cfg)
    {
      LayoutWithHeaderAndFooter layout1 = new LayoutWithHeaderAndFooter()
      {
        Header = (Layout) "----------NLog Starting---------${newline}Application ver.: ${assembly-version}${newline}${newline}",
        Layout = (Layout) "${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} | ${level:uppercase=true} | ${CallSite:skipFrames=2:includeNamespace=false:fileName=false:includeSourcePath=false} | ${message} | ${exception:toString,Data}",
        Footer = (Layout) "----------NLog  Ending-----------${newline}${newline}"
      };
      LayoutWithHeaderAndFooter layout2 = new LayoutWithHeaderAndFooter()
      {
        Header = (Layout) "----------NLog Starting---------${newline}Application ver.: ${assembly-version}${newline}${newline}",
        Layout = (Layout) "${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} | ${level:uppercase=true} | ${CallSite:skipFrames=1:includeNamespace=false:fileName=true:includeSourcePath=false} | ${message} | ${exception:toString,Data}",
        Footer = (Layout) "----------NLog  Ending-----------${newline}${newline}"
      };
      LogHelper.AddFileLoggingRule(cfg, layout1, "trace.log", NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "mainLogger");
      LogHelper.AddFileLoggingRule(cfg, layout2, "error.log", NLog.LogLevel.Error, NLog.LogLevel.Fatal, "mainLogger");
    }

    private static void CreatePrroLogger(LoggingConfiguration cfg)
    {
      LayoutWithHeaderAndFooter layout = new LayoutWithHeaderAndFooter()
      {
        Header = (Layout) "----------NLog Starting---------${newline}Application ver.: ${assembly-version}${newline}${newline}",
        Layout = (Layout) "${newline}${newline}${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} ${newline} ${message} ",
        Footer = (Layout) "----------NLog  Ending-----------${newline}${newline}"
      };
      LogHelper.AddFileLoggingRule(cfg, layout, "ua_prro.log", NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "prroLogger");
    }

    private static void CreateEgaisLogger(LoggingConfiguration cfg)
    {
      LayoutWithHeaderAndFooter layout = new LayoutWithHeaderAndFooter()
      {
        Header = (Layout) "----------NLog Starting---------${newline}Application ver.: ${assembly-version}${newline}${newline}",
        Layout = (Layout) "${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} | ${level:uppercase=true} | ${CallSite:skipFrames=1:includeNamespace=false:fileName=true:includeSourcePath=false} | ${message} | ${exception:toString,Data}",
        Footer = (Layout) "----------NLog  Ending-----------${newline}${newline}"
      };
      LogHelper.AddFileLoggingRule(cfg, layout, "egais.log", NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "egaisLogger");
    }

    private static void CreateCrptLogger(LoggingConfiguration cfg)
    {
      LayoutWithHeaderAndFooter withHeaderAndFooter = new LayoutWithHeaderAndFooter()
      {
        Header = (Layout) "----------NLog Starting---------${newline}Application ver.: ${assembly-version}${newline}${newline}",
        Layout = (Layout) "${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} | ${level:uppercase=true} | ${message} | ${exception:toString,Data}",
        Footer = (Layout) "----------NLog  Ending-----------${newline}${newline}"
      };
      string str = ApplicationInfo.GetInstance().Paths.CrptLogsPath + "${date:format=yyyy-MM-dd}.log";
      FileTarget fileTarget = new FileTarget(Guid.NewGuid().ToString());
      fileTarget.FileName = (Layout) str;
      fileTarget.Layout = (Layout) withHeaderAndFooter;
      fileTarget.Encoding = System.Text.Encoding.UTF8;
      fileTarget.AutoFlush = true;
      fileTarget.KeepFileOpen = true;
      FileTarget wrappedTarget = fileTarget;
      AsyncTargetWrapper asyncTargetWrapper1 = new AsyncTargetWrapper((Target) wrappedTarget);
      asyncTargetWrapper1.Name = wrappedTarget.Name;
      asyncTargetWrapper1.QueueLimit = 5000;
      asyncTargetWrapper1.OverflowAction = AsyncTargetWrapperOverflowAction.Discard;
      AsyncTargetWrapper asyncTargetWrapper2 = asyncTargetWrapper1;
      SimpleConfigurator.ConfigureForTargetLogging((Target) asyncTargetWrapper2);
      cfg.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, (Target) asyncTargetWrapper2, "crptLogger");
    }

    private static void CreatePerformanceLogger(LoggingConfiguration cfg)
    {
      LayoutWithHeaderAndFooter layout = new LayoutWithHeaderAndFooter()
      {
        Layout = (Layout) "${date:format=dd.MM.yyyy HH\\:mm\\:ss.ffff} | ${CallSite:skipFrames=2:includeNamespace=false:fileName=false:includeSourcePath=false} | ${message} "
      };
      LogHelper.AddFileLoggingRule(cfg, layout, "performace.log", NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "performanceLogger");
    }

    [JsonObject]
    private class LogErrorInfo
    {
      public Exception Exception { get; set; }

      public string Message { get; set; }

      public LogErrorInfo(Exception exception, string message)
      {
        this.Exception = exception;
        this.Message = message;
      }
    }

    public enum MsgTypes
    {
      None,
      Notification,
      MessageBox,
      Full,
    }
  }
}
