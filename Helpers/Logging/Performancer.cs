// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Performancer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;

#nullable disable
namespace Gbs.Helpers
{
  [Localizable(false)]
  public class Performancer
  {
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly Timer _timer = new Timer(500.0);
    private long _lastActionElapsedMs;
    private string _messagesCache = string.Empty;
    private DateTime _start;
    private DateTime _finish;
    private readonly object lockObject = new object();

    public bool Enable { get; set; } = true;

    public Performancer(string taskDescription, bool enable = true)
    {
      Performancer performancer = this;
      this.Enable = enable;
      if (!this.Enable)
        return;
      TaskHelper.TaskRun((Action) (() =>
      {
        performancer.Enable = true;
        performancer.UpdateMessageForLog(string.Format("Perfomance counting\r\n{0:dd.MM.yyyy HH:mm:ss.ffff} | Action: {1} \r\n", (object) DateTime.Now, (object) taskDescription));
        performancer._stopwatch.Start();
        performancer._start = DateTime.Now;
      }), false);
    }

    public void AddPoint(string message)
    {
      if (!this.Enable)
        return;
      TaskHelper.TaskRun((Action) (() =>
      {
        long elapsedMilliseconds = this._stopwatch.ElapsedMilliseconds;
        this.UpdateMessageForLog(string.Format("{0:dd.MM.yyyy HH:mm:ss.ffff} | + {1:N3} = {2:N3}  : {3}\r\n", (object) DateTime.Now, (object) ((double) (elapsedMilliseconds - this._lastActionElapsedMs) / 1000.0), (object) ((double) elapsedMilliseconds / 1000.0), (object) message));
        this._lastActionElapsedMs = elapsedMilliseconds;
      }), false);
    }

    public void AddComment(string comment)
    {
      if (!this.Enable)
        return;
      this._messagesCache = this._messagesCache + "    " + comment + Other.NewLine();
    }

    public void Stop(string comment = "")
    {
      if (!this.Enable)
        return;
      TaskHelper.TaskRun((Action) (() =>
      {
        this.AddPoint("DONE. " + comment);
        this._finish = DateTime.Now;
        CpuPerformanceCounter.CpuValue cpuValues = CpuPerformanceCounter.GetCpuValues(this._start, this._finish);
        Decimal avg = cpuValues.AVG;
        Decimal max = cpuValues.MAX;
        this.UpdateMessageForLog(string.Format("TOTAL: {0}\r\n", (object) ((double) this._stopwatch.ElapsedMilliseconds / 1000.0)) + string.Format("CPU usage: AVG {0:N2}%; MAX {1:N2}% \r\n------------------------------------\r\n", (object) avg, (object) max));
        this._stopwatch.Stop();
        this._timer.Stop();
        LogHelper.WritePerformanceMessage(this.GetMessageForLog());
      }), false);
    }

    public void UpdateMessageForLog(string addedString)
    {
      lock (this.lockObject)
        this._messagesCache = this.GetMessageForLog() + addedString;
    }

    public string GetMessageForLog()
    {
      lock (this.lockObject)
        return this._messagesCache;
    }
  }
}
