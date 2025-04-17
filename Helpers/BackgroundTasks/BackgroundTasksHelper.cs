// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BackgroundTasks.BackgroundTasksHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

#nullable disable
namespace Gbs.Helpers.BackgroundTasks
{
  internal static class BackgroundTasksHelper
  {
    private static readonly Queue<BackgroundTask> BackgroundTasksQueue = new Queue<BackgroundTask>();
    private static readonly System.Timers.Timer QueueTimer = new System.Timers.Timer(1000.0);
    private static BackgroundTask RunningTask;

    internal static void AddTaskToQueue(Action action, BackgroundTaskTypes type)
    {
      BackgroundTasksHelper.AddTaskToQueue(new BackgroundTask(action, type));
    }

    internal static void AddTaskToQueue(BackgroundTask task)
    {
      try
      {
        if (BackgroundTasksHelper.OnlyOneTimeRunning.Contains(task.Type) && BackgroundTasksHelper.BackgroundTasksQueue.Any<BackgroundTask>((Func<BackgroundTask, bool>) (x => x.Type == task.Type)))
        {
          LogHelper.Trace(string.Format("В очереди уже есть задача с типом {0}", (object) task.Type));
        }
        else
        {
          if (BackgroundTasksHelper.OnlyOneTimeRunning.Contains(task.Type))
          {
            BackgroundTaskTypes? type1 = BackgroundTasksHelper.RunningTask?.Type;
            BackgroundTaskTypes type2 = task.Type;
            if (type1.GetValueOrDefault() == type2 & type1.HasValue)
            {
              LogHelper.Trace(string.Format("Уже выполняется задача с типом {0}", (object) task.Type));
              return;
            }
          }
          BackgroundTasksHelper.BackgroundTasksQueue.Enqueue(task);
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private static List<BackgroundTaskTypes> OnlyOneTimeRunning
    {
      get
      {
        return new List<BackgroundTaskTypes>()
        {
          BackgroundTaskTypes.DataBaseSyncDataPreparing,
          BackgroundTaskTypes.ClientsSyncDataPreparing,
          BackgroundTaskTypes.CheckUpdate,
          BackgroundTaskTypes.UpdateCache
        };
      }
    }

    static BackgroundTasksHelper()
    {
      BackgroundTasksHelper.QueueTimer.Elapsed += new ElapsedEventHandler(BackgroundTasksHelper.CheckQueue);
      BackgroundTasksHelper.QueueTimer.Start();
    }

    private static bool CpuIsBusy()
    {
      return CpuPerformanceCounter.GetCpuValues(DateTime.Now.AddMinutes(-1.0), DateTime.Now).AVG > 75M;
    }

    private static DateTime PausedTo { get; set; } = DateTime.Now;

    private static void CheckQueue(object sender, ElapsedEventArgs e)
    {
      if (!BackgroundTasksHelper.BackgroundTasksQueue.Any<BackgroundTask>())
        return;
      BackgroundTasksHelper.WriteQueueToConsole();
      if (BackgroundTasksHelper.RunningTask != null || BackgroundTasksHelper.PausedTo > DateTime.Now)
        return;
      if (BackgroundTasksHelper.CpuIsBusy())
      {
        LogHelper.Trace("Повышенная нагрузка на ЦП. Выполнение фоновых задач приостановлено на 1 мин.");
        BackgroundTasksHelper.PausedTo = DateTime.Now.AddMinutes(1.0);
      }
      else
      {
        BackgroundTask t = BackgroundTasksHelper.BackgroundTasksQueue.Dequeue();
        BackgroundTasksHelper.RunningTask = t;
        Task.Run((Action) (() =>
        {
          try
          {
            LogHelper.Trace(string.Format("Запуск на выполнение фоновой задачи с типом {0}. В очереди осталось {1} задач", (object) t.Type, (object) BackgroundTasksHelper.BackgroundTasksQueue.Count));
            Action action = t.Action;
            if (action != null)
              action();
            Thread.Sleep(1000);
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Ошибка при выполнении фоновой задачи");
            BackgroundTasksHelper.RunningTask = (BackgroundTask) null;
          }
        })).ContinueWith((Action<Task>) (_ => BackgroundTasksHelper.RunningTask = (BackgroundTask) null));
      }
    }

    private static void WriteQueueToConsole()
    {
      try
      {
        string empty = string.Empty;
        foreach (BackgroundTask backgroundTasks in BackgroundTasksHelper.BackgroundTasksQueue)
          empty += string.Format(" - {0}\r\n", (object) backgroundTasks.Type);
      }
      catch (Exception ex)
      {
        Other.ConsoleWrite(ex.ToString());
      }
    }
  }
}
