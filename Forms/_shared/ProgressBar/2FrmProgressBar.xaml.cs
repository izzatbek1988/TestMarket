// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.ProgressBarHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms._shared
{
  public static class ProgressBarHelper
  {
    private static FrmProgressBar _currentWindow;
    private static DateTime _startedTime = DateTime.Now;
    public static readonly Dictionary<Guid, string> ProgressesList = new Dictionary<Guid, string>();

    private static bool IsIndeterminate
    {
      get
      {
        return ProgressBarHelper._currentWindow != null && ProgressBarHelper._currentWindow.Model.IsIndeterminate;
      }
      set
      {
        if (ProgressBarHelper._currentWindow == null)
          return;
        ProgressBarHelper._currentWindow.Model.IsIndeterminate = value;
      }
    }

    public static void SetText(string text)
    {
      try
      {
        if (ProgressBarHelper._currentWindow == null)
          return;
        ProgressBarHelper._currentWindow.Visibility = Visibility.Visible;
        ProgressBarHelper._currentWindow.Model.TextBar = text;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    public static void AddNotification(ProgressBarViewModel.Notification n)
    {
      if (DevelopersHelper.IsUnitTest())
        return;
      Task.Run((Action) (() =>
      {
        try
        {
          ProgressBarHelper.ShowForm();
          ProgressBarHelper.WaitCurrentWindow();
          if (ProgressBarHelper._currentWindow == null)
            return;
          ProgressBarHelper._currentWindow.Dispatcher?.Invoke((Action) (() => ProgressBarHelper._currentWindow?.Model.AddNotif(n)));
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "", false);
        }
      }));
    }

    public static void AddNotification(string n)
    {
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(n));
    }

    public static void SetProgressValue(int value)
    {
      try
      {
        if (ProgressBarHelper._currentWindow == null)
          return;
        ProgressBarHelper.IsIndeterminate = false;
        ProgressBarHelper._currentWindow.Model.ValueProgress = value;
        ProgressBarHelper._currentWindow.Model.Visibility = Visibility.Visible;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private static void ShowForm()
    {
      try
      {
        if (DevelopersHelper.IsUnitTest() || ProgressBarHelper._currentWindow != null && ProgressBarHelper._currentWindow.IsVisible)
          return;
        Thread thread = new Thread((ParameterizedThreadStart) (obj =>
        {
          try
          {
            ProgressBarHelper._currentWindow = new FrmProgressBar();
            ProgressBarHelper._currentWindow.Visibility = Visibility.Visible;
            ProgressBarHelper._currentWindow.Topmost = true;
            ProgressBarHelper.IsIndeterminate = true;
            ProgressBarHelper._currentWindow.Show();
            Dispatcher.Run();
          }
          catch (Exception ex)
          {
            LogHelper.Trace(ex.ToString());
            ProgressBarHelper._currentWindow = (FrmProgressBar) null;
          }
        }));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private static void ShowProgress(string text, Guid guid)
    {
      try
      {
        if (!ProgressBarHelper.ProgressesList.ContainsKey(guid))
          ProgressBarHelper.ProgressesList.Add(guid, text);
        ProgressBarHelper.ShowProgress(text);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [Obsolete("Устарело. Нужно использовать класс ProgressBar")]
    private static void ShowProgress(string text)
    {
      Task.Run((Action) (() =>
      {
        try
        {
          LogHelper.Debug("ShowProgressBar. Text:" + text + "; ");
          if (DevelopersHelper.IsUnitTest())
            return;
          ProgressBarHelper._startedTime = DateTime.Now;
          ProgressBarHelper.ShowForm();
          ProgressBarHelper.WaitCurrentWindow();
          if (ProgressBarHelper._currentWindow == null)
            return;
          ProgressBarHelper._currentWindow.Dispatcher?.Invoke((Action) (() =>
          {
            ProgressBarHelper._currentWindow.Model.Visibility = Visibility.Visible;
            ProgressBarHelper.IsIndeterminate = true;
            ProgressBarHelper._currentWindow.Model.TextBar = text;
          }));
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "");
        }
      }));
    }

    private static void WaitCurrentWindow()
    {
      int num = 0;
      int millisecondsTimeout = 10;
      do
      {
        Thread.Sleep(millisecondsTimeout);
        ++num;
        if (num * millisecondsTimeout > 10000)
        {
          LogHelper.Debug("Не удалось отобразить окно прогресс бара\\нотификацию");
          break;
        }
      }
      while (ProgressBarHelper._currentWindow == null);
    }

    public static void Close(Guid guid)
    {
      try
      {
        ProgressBarHelper.ProgressesList.Remove(guid);
        if (!ProgressBarHelper.ProgressesList.Any<KeyValuePair<Guid, string>>())
        {
          ProgressBarHelper.Close();
        }
        else
        {
          string text = ProgressBarHelper.ProgressesList.First<KeyValuePair<Guid, string>>().Value;
          ProgressBarHelper._currentWindow.Dispatcher?.Invoke((Action) (() => ProgressBarHelper._currentWindow.Model.TextBar = text));
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
      }
    }

    public static void Close()
    {
      Task.Run((Action) (() =>
      {
        if (DevelopersHelper.IsUnitTest())
          return;
        try
        {
          ProgressBarHelper.ProgressesList.Clear();
          ProgressBarHelper.WaitCurrentWindow();
          if (ProgressBarHelper._currentWindow == null)
            return;
          double totalMilliseconds = (DateTime.Now - ProgressBarHelper._startedTime).TotalMilliseconds;
          if (!ProgressBarHelper.ProgressesList.Any<KeyValuePair<Guid, string>>())
          {
            if (totalMilliseconds > 1000.0)
            {
              ProgressBarHelper._currentWindow.Dispatcher?.Invoke((Action) (() =>
              {
                ProgressBarHelper._currentWindow.Model.ValueProgress = 100;
                ProgressBarHelper._currentWindow.Model.TextBar = Translate.ProgressBarHelper_Операция_завершена;
              }));
              Thread.Sleep(1000);
            }
            else
              Thread.Sleep(300);
          }
          ProgressBarHelper._currentWindow.Dispatcher?.Invoke((Action) (() =>
          {
            if (ProgressBarHelper.ProgressesList.Any<KeyValuePair<Guid, string>>())
              return;
            ProgressBarHelper._currentWindow.Model.Visibility = Visibility.Collapsed;
          }));
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));
    }

    public class ProgressBar
    {
      private Guid uid = Guid.NewGuid();
      private bool isClosed;

      public ProgressBar(string text)
      {
        ProgressBarHelper.ProgressBar progressBar = this;
        TaskHelper.TaskRun((Action) (() =>
        {
          Thread.Sleep(300);
          if (progressBar.isClosed)
            return;
          ProgressBarHelper.ShowProgress(text, progressBar.uid);
        }), false);
      }

      public void Close()
      {
        this.isClosed = true;
        ProgressBarHelper.Close(this.uid);
      }
    }
  }
}
