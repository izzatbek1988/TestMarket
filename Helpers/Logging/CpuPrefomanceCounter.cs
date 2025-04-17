// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Logging.CpuPerformanceCounter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

#nullable disable
namespace Gbs.Helpers.Logging
{
  internal class CpuPerformanceCounter
  {
    private static PerformanceCounter _cpuCounter;
    private static readonly Timer Timer = new Timer(1000.0);
    private static readonly Timer ClearTimer = new Timer(60000.0);
    private static bool _isRunning = false;
    private static List<CpuPerformanceCounter.CpuLoadByTimeValue> CpuLoadHistory = new List<CpuPerformanceCounter.CpuLoadByTimeValue>();

    public void StartCpuCounting()
    {
      if (CpuPerformanceCounter._isRunning)
      {
        LogHelper.Trace("Процесс замера производительности уже запущен");
      }
      else
      {
        CpuPerformanceCounter._isRunning = true;
        TaskHelper.TaskRun((Action) (() =>
        {
          CpuPerformanceCounter._cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
          CpuPerformanceCounter.Timer.Elapsed += new ElapsedEventHandler(this._timer_Elapsed);
          CpuPerformanceCounter.Timer.Start();
          CpuPerformanceCounter.ClearTimer.Elapsed += new ElapsedEventHandler(this._clearTimerElapsed);
          CpuPerformanceCounter.ClearTimer.Start();
        }), false);
      }
    }

    private void _timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        float num = CpuPerformanceCounter._cpuCounter.NextValue();
        CpuPerformanceCounter.CpuLoadHistory.Add(new CpuPerformanceCounter.CpuLoadByTimeValue(num));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка основного таймера CPU counter");
      }
    }

    private void _clearTimerElapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        CpuPerformanceCounter.CpuLoadHistory.RemoveAll((Predicate<CpuPerformanceCounter.CpuLoadByTimeValue>) (x => x.Time < DateTime.Now.AddHours(-2.0)));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка таймера очистки в CPU counter ");
      }
    }

    public static CpuPerformanceCounter.CpuValue GetCpuValues(DateTime start, DateTime finish)
    {
      try
      {
        CpuPerformanceCounter.CpuLoadByTimeValue[] array = CpuPerformanceCounter.CpuLoadHistory.ToArray();
        List<CpuPerformanceCounter.CpuLoadByTimeValue> list = ((IEnumerable<CpuPerformanceCounter.CpuLoadByTimeValue>) array).Where<CpuPerformanceCounter.CpuLoadByTimeValue>((Func<CpuPerformanceCounter.CpuLoadByTimeValue, bool>) (x => x.Time >= start && x.Time <= finish)).ToList<CpuPerformanceCounter.CpuLoadByTimeValue>();
        if (!list.Any<CpuPerformanceCounter.CpuLoadByTimeValue>())
        {
          list = ((IEnumerable<CpuPerformanceCounter.CpuLoadByTimeValue>) array).Where<CpuPerformanceCounter.CpuLoadByTimeValue>((Func<CpuPerformanceCounter.CpuLoadByTimeValue, bool>) (x => x.Time > start.AddSeconds(-1.0) && (x.Time - start).TotalSeconds < 1.0)).ToList<CpuPerformanceCounter.CpuLoadByTimeValue>();
          if (!list.Any<CpuPerformanceCounter.CpuLoadByTimeValue>())
            return new CpuPerformanceCounter.CpuValue()
            {
              AVG = -1M,
              MAX = -1M
            };
        }
        return new CpuPerformanceCounter.CpuValue()
        {
          AVG = (Decimal) list.Average<CpuPerformanceCounter.CpuLoadByTimeValue>((Func<CpuPerformanceCounter.CpuLoadByTimeValue, float>) (x => x.Value)),
          MAX = (Decimal) list.Max<CpuPerformanceCounter.CpuLoadByTimeValue>((Func<CpuPerformanceCounter.CpuLoadByTimeValue, float>) (x => x.Value))
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "CPU load counting error", false);
        return new CpuPerformanceCounter.CpuValue()
        {
          AVG = -2M,
          MAX = -2M
        };
      }
    }

    public class CpuLoadByTimeValue
    {
      public DateTime Time { get; set; }

      public float Value { get; set; }

      public CpuLoadByTimeValue(float value)
      {
        this.Time = DateTime.Now;
        this.Value = value;
      }
    }

    public class CpuValue
    {
      public Decimal AVG { get; set; }

      public Decimal MAX { get; set; }
    }
  }
}
