// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BackgroundTasks.Sheduler
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

#nullable disable
namespace Gbs.Helpers.BackgroundTasks
{
  internal class Sheduler
  {
    private static List<Sheduler.ScheduleItem> Schedule = new List<Sheduler.ScheduleItem>();
    private static bool _isRunning = false;

    public void AddPeriodicAction(Action action, TimeSpan period)
    {
      Sheduler.ScheduleItem scheduleItem = new Sheduler.ScheduleItem()
      {
        NextStart = DateTime.Now.Add(period),
        Action = action,
        Period = period
      };
      Sheduler.Schedule.Add(scheduleItem);
    }

    public void AddScheduleAction(Action action, List<TimeSpan> schedule)
    {
      TimeSpan nowTime = DateTime.Now.TimeOfDay;
      if (!schedule.Any<TimeSpan>())
        return;
      DateTime dateTime;
      if (schedule.Any<TimeSpan>((Func<TimeSpan, bool>) (x => x > nowTime)))
      {
        TimeSpan timeSpan = schedule.Where<TimeSpan>((Func<TimeSpan, bool>) (x => x > nowTime)).OrderBy<TimeSpan, TimeSpan>((Func<TimeSpan, TimeSpan>) (x => x)).First<TimeSpan>();
        dateTime = DateTime.Today.AddHours((double) timeSpan.Hours).AddMinutes((double) timeSpan.Minutes);
      }
      else
      {
        TimeSpan timeSpan = schedule.OrderBy<TimeSpan, TimeSpan>((Func<TimeSpan, TimeSpan>) (x => x)).First<TimeSpan>();
        dateTime = DateTime.Today.AddDays(1.0).AddHours((double) timeSpan.Hours).AddMinutes((double) timeSpan.Minutes);
      }
      Sheduler.ScheduleItem scheduleItem = new Sheduler.ScheduleItem()
      {
        NextStart = dateTime,
        Action = action,
        Schedule = schedule
      };
      Sheduler.Schedule.Add(scheduleItem);
    }

    public void RunScheduler()
    {
      if (Sheduler._isRunning)
      {
        LogHelper.Trace("Управление расписанием задач уже запущено");
      }
      else
      {
        Sheduler._isRunning = true;
        Timer timer = new Timer(500.0);
        timer.Start();
        timer.Elapsed += new ElapsedEventHandler(this.T_Elapsed);
      }
    }

    private void T_Elapsed(object sender, ElapsedEventArgs e)
    {
      List<Sheduler.ScheduleItem> list = Sheduler.Schedule.Where<Sheduler.ScheduleItem>((Func<Sheduler.ScheduleItem, bool>) (x => x.NextStart < DateTime.Now)).OrderBy<Sheduler.ScheduleItem, DateTime>((Func<Sheduler.ScheduleItem, DateTime>) (x => x.NextStart)).ToList<Sheduler.ScheduleItem>();
      if (!list.Any<Sheduler.ScheduleItem>())
        return;
      Sheduler.ScheduleItem t = list.First<Sheduler.ScheduleItem>();
      Sheduler.Schedule.RemoveAll((Predicate<Sheduler.ScheduleItem>) (x => x.Uid == t.Uid));
      BackgroundTasksHelper.AddTaskToQueue((Action) (() =>
      {
        try
        {
          t.Action();
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка выполнения задачи по расписанию");
        }
        TimeSpan period = t.Period;
        this.AddPeriodicAction(t.Action, t.Period);
        if (t.Schedule == null)
          return;
        this.AddScheduleAction(t.Action, t.Schedule);
      }), BackgroundTaskTypes.NotSet);
    }

    public class ScheduleItem
    {
      public Guid Uid { get; } = Guid.NewGuid();

      public DateTime NextStart { get; set; }

      public Action Action { get; set; }

      public TimeSpan Period { get; set; }

      public List<TimeSpan> Schedule { get; set; }
    }
  }
}
