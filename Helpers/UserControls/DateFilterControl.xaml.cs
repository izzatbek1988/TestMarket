// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.DateFilterControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class DateFilterControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty CommandOnUpdateProperty = DependencyProperty.Register(nameof (CommandOnUpdate), typeof (RelayCommand), typeof (DateFilterControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ValueDateTimeStartProperty;
    public static readonly DependencyProperty ValueDateTimeEndProperty;
    public static readonly DependencyProperty ContentDescriptionProperty;
    private Timer Timer = new Timer();
    internal DateFilterControl This;
    internal DatePicker DateStart;
    internal DatePicker DateEnd;
    private bool _contentLoaded;

    private void DoCommand()
    {
      this.Timer.Stop();
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        LogHelper.Trace("Вызов команды обновления данных из контрола выбора дат");
        this.CommandOnUpdate?.Execute((object) null);
      }));
    }

    public RelayCommand CommandOnUpdate
    {
      get => (RelayCommand) this.GetValue(DateFilterControl.CommandOnUpdateProperty);
      set => this.SetValue(DateFilterControl.CommandOnUpdateProperty, (object) value);
    }

    public DateTime ValueDateTimeStart
    {
      get => (DateTime) this.GetValue(DateFilterControl.ValueDateTimeStartProperty);
      set => this.SetValue(DateFilterControl.ValueDateTimeStartProperty, (object) value);
    }

    public DateTime ValueDateTimeEnd
    {
      get => (DateTime) this.GetValue(DateFilterControl.ValueDateTimeEndProperty);
      set => this.SetValue(DateFilterControl.ValueDateTimeEndProperty, (object) value);
    }

    public string ContentDescription
    {
      get => (string) this.GetValue(DateFilterControl.ContentDescriptionProperty);
      set => this.SetValue(DateFilterControl.ContentDescriptionProperty, (object) value);
    }

    public ICommand SetLastDay
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          DateTime dateTime = this.ValueDateTimeStart;
          DateTime start = dateTime.AddDays(-1.0);
          dateTime = this.ValueDateTimeEnd;
          DateTime end = dateTime.AddDays(-1.0);
          this.SetValues(start, end);
        }));
      }
    }

    public ICommand SetYesterday
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          DateTime today = DateTime.Today;
          DateTime start = today.AddDays(-1.0);
          today = DateTime.Today;
          DateTime end = today.AddDays(-1.0);
          this.SetValues(start, end);
        }));
      }
    }

    public ICommand SetTodayDay
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.SetValues(DateTime.Today, DateTime.Today)));
      }
    }

    public ICommand SetWeek
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.SetValues(DateTime.Today.AddDays(-7.0), DateTime.Today)));
      }
    }

    public ICommand SetMonth
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.SetValues(DateTime.Today.AddMonths(-1), DateTime.Today)));
      }
    }

    public ICommand SetQuarter
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.SetValues(DateTime.Today.AddMonths(-3), DateTime.Today)));
      }
    }

    public ICommand SetYear
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.SetValues(DateTime.Today.AddYears(-1), DateTime.Today)));
      }
    }

    private void SetValues(DateTime start, DateTime end)
    {
      this.ValueDateTimeStart = start;
      this.ValueDateTimeEnd = end;
    }

    static DateFilterControl()
    {
      Type propertyType = typeof (DateTime);
      Type ownerType = typeof (DateFilterControl);
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      PropertyMetadata typeMetadata = new PropertyMetadata((object) dateTime.AddMonths(-1));
      DateFilterControl.ValueDateTimeStartProperty = DependencyProperty.Register(nameof (ValueDateTimeStart), propertyType, ownerType, typeMetadata);
      DateFilterControl.ValueDateTimeEndProperty = DependencyProperty.Register(nameof (ValueDateTimeEnd), typeof (DateTime), typeof (DateFilterControl), new PropertyMetadata((object) DateTime.Now.Date));
      DateFilterControl.ContentDescriptionProperty = DependencyProperty.Register(nameof (ContentDescription), typeof (string), typeof (DateFilterControl), new PropertyMetadata((object) string.Empty));
    }

    public DateFilterControl()
    {
      this.Timer.Interval = 500.0;
      this.Timer.AutoReset = false;
      this.Timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
      this.InitializeComponent();
    }

    private void RestartTimer()
    {
      this.Timer.Stop();
      this.Timer.Start();
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e) => this.DoCommand();

    private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((FrameworkElement) sender).Name == "DateEnd" && this.ValueDateTimeEnd < this.ValueDateTimeStart)
        this.ValueDateTimeStart = this.ValueDateTimeEnd;
      if (((FrameworkElement) sender).Name == "DateStart" && this.ValueDateTimeEnd < this.ValueDateTimeStart)
        this.ValueDateTimeEnd = this.ValueDateTimeStart;
      this.RestartTimer();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/datefiltercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.This = (DateFilterControl) target;
          break;
        case 2:
          this.DateStart = (DatePicker) target;
          this.DateStart.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(this.DatePicker_OnSelectedDateChanged);
          break;
        case 3:
          this.DateEnd = (DatePicker) target;
          this.DateEnd.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(this.DatePicker_OnSelectedDateChanged);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
