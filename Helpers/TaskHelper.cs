// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TaskHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers
{
  [Serializable]
  public static class TaskHelper
  {
    public static void RunList(this List<Task> list, bool multiThreadMode)
    {
      LogHelper.Trace(string.Format("Запус выполнения задач из списка. Tasks count: {0}; MultiThread: {1} ", (object) list.Count, (object) multiThreadMode));
      if (multiThreadMode)
      {
        foreach (Task task in list)
          task?.Start();
        foreach (Task task in list)
          task?.Wait();
      }
      else
      {
        foreach (Task task in list)
        {
          task?.Start();
          task?.Wait();
        }
      }
    }

    [HandleProcessCorruptedStateExceptions]
    public static T TaskRunAndWait<T>(Func<T> action)
    {
      try
      {
        T result = default (T);
        Exception ex = (Exception) null;
        bool taskFinished = false;
        new Task<T>((Func<T>) (() =>
        {
          try
          {
            result = action();
            taskFinished = true;
          }
          catch (Exception ex1)
          {
            taskFinished = true;
            ex = ex1;
          }
          return result;
        })).RunSynchronously();
        if (!taskFinished && ex == null)
          throw new Exception(Translate.TaskHelper_TaskRunAndWait_Выполнение_метода_в_потоке_не_было_завершено);
        if (ex != null)
        {
          LogHelper.WriteError(ex, "Исключение внутри TaskRunAndWait");
          throw ex;
        }
        return result;
      }
      catch (AccessViolationException ex)
      {
        throw new Exception("Throw AccessViolationException", (Exception) ex);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    [HandleProcessCorruptedStateExceptions]
    public static void TaskRunAndWait(Action action)
    {
      TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() =>
      {
        action();
        return true;
      }));
    }

    public static void TaskRun(Action action, bool throwExceptions = true)
    {
      Exception ex = (Exception) null;
      try
      {
        new Task((Action) (() =>
        {
          try
          {
            action();
          }
          catch (Exception ex1)
          {
            LogHelper.Error(ex1, "Ошиба выполнения задачи", false);
            ex = ex1;
          }
        })).Start();
      }
      catch (Exception ex2)
      {
        LogHelper.WriteError(ex2);
        throw;
      }
      if (ex == null)
        return;
      LogHelper.Debug("Исключение внутри TaskRun");
      if (throwExceptions)
        throw ex;
      LogHelper.Trace(string.Format("Исключение не будет вызвано: {0}", (object) ex));
    }
  }
}
