// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.ProgressBarViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class ProgressBarViewModel : ViewModelWithForm
  {
    private bool _isIndeterminate;
    private string _textBar = string.Empty;
    private int _value;
    private Visibility _visibility = Visibility.Collapsed;

    public AsyncObservableCollection<ProgressBarViewModel.Notification> NotificationsList { get; set; } = new AsyncObservableCollection<ProgressBarViewModel.Notification>();

    public Visibility Visibility
    {
      get => this._visibility;
      set
      {
        this._visibility = value;
        this.OnPropertyChanged(nameof (Visibility));
      }
    }

    public Visibility CloseAllButtonVisibility
    {
      get
      {
        return this.NotificationsList.Count<ProgressBarViewModel.Notification>((Func<ProgressBarViewModel.Notification, bool>) (x => x.Visibility == Visibility.Visible)) <= 1 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand CloseAllCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          foreach (ProgressBarViewModel.Notification notifications in (Collection<ProgressBarViewModel.Notification>) this.NotificationsList)
            notifications.HideCommand.Execute((object) null);
          this.OnPropertyChanged("CloseAllButtonVisibility");
        }));
      }
    }

    public bool IsIndeterminate
    {
      get => this._isIndeterminate;
      set
      {
        this._isIndeterminate = value;
        this.OnPropertyChanged(nameof (IsIndeterminate));
      }
    }

    public string TextBar
    {
      get => this._textBar;
      set
      {
        this._textBar = value;
        this.OnPropertyChanged(nameof (TextBar));
      }
    }

    public int ValueProgress
    {
      get => this._value;
      set
      {
        this._value = value;
        this.OnPropertyChanged(nameof (ValueProgress));
      }
    }

    public void AddNotif(ProgressBarViewModel.Notification n)
    {
      n.VisibilityChange += new EventHandler(this.N_VisibilityChange);
      this.NotificationsList.Add(n);
      LogHelper.Debug("Show notification: " + n.ToJsonString(true));
      this.OnPropertyChanged("NotificationsList");
      this.OnPropertyChanged("CloseAllButtonVisibility");
    }

    private void N_VisibilityChange(object sender, EventArgs e)
    {
      try
      {
        this.OnPropertyChanged("CloseAllButtonVisibility");
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public class Notification : ViewModelWithForm
    {
      private string _actionText;
      private Visibility _visibility;
      private ProgressBarViewModel.Notification.NotificationsTypes _type;

      public event EventHandler VisibilityChange;

      protected virtual void OnVisibilityChanged(EventArgs e = null)
      {
        EventHandler visibilityChange = this.VisibilityChange;
        if (visibilityChange == null)
          return;
        visibilityChange((object) this, e);
      }

      public string Title { get; set; } = PartnersHelper.ProgramName();

      public string Text { get; set; }

      public int TimeOut { get; set; } = 10;

      public ICommand ActionCommand { get; set; }

      public Visibility CommandButtonVisibility
      {
        get
        {
          string actionText = this.ActionText;
          return (actionText != null ? (actionText.Length > 0 ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public string ActionText
      {
        get => this._actionText;
        set
        {
          this._actionText = value;
          this.OnPropertyChanged(nameof (ActionText));
        }
      }

      public Visibility Visibility
      {
        get => this._visibility;
        set
        {
          this.OnVisibilityChanged();
          this._visibility = value;
          this.OnPropertyChanged(nameof (Visibility));
        }
      }

      public ICommand HideCommand { get; set; }

      public Visibility InfoVisibility { get; private set; }

      public Visibility ErrorVisibility { get; private set; }

      public Visibility WarningVisibility { get; private set; }

      public Notification()
      {
        this.Init();
        this.Type = ProgressBarViewModel.Notification.NotificationsTypes.Info;
      }

      private void Init()
      {
        Timer timer = new Timer((double) (this.TimeOut * 1000));
        timer.Start();
        timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
        this.HideCommand = (ICommand) new RelayCommand((Action<object>) (_ => this.Visibility = Visibility.Collapsed));
      }

      public ProgressBarViewModel.Notification.NotificationsTypes Type
      {
        get => this._type;
        set
        {
          this._type = value;
          this.InfoVisibility = Visibility.Collapsed;
          this.WarningVisibility = Visibility.Collapsed;
          this.ErrorVisibility = Visibility.Collapsed;
          switch (this.Type)
          {
            case ProgressBarViewModel.Notification.NotificationsTypes.Info:
              this.InfoVisibility = Visibility.Visible;
              break;
            case ProgressBarViewModel.Notification.NotificationsTypes.Warning:
              this.WarningVisibility = Visibility.Visible;
              break;
            case ProgressBarViewModel.Notification.NotificationsTypes.Error:
              this.ErrorVisibility = Visibility.Visible;
              break;
            default:
              this.InfoVisibility = Visibility.Visible;
              break;
          }
          this.OnPropertyChanged("InfoVisibility");
          this.OnPropertyChanged("WarningVisibility");
          this.OnPropertyChanged("ErrorVisibility");
        }
      }

      public Notification(
        string text,
        ProgressBarViewModel.Notification.NotificationsTypes type = ProgressBarViewModel.Notification.NotificationsTypes.Info)
      {
        this.Init();
        this.Text = text;
        this.Type = type;
      }

      private void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
        this.Visibility = Visibility.Collapsed;
      }

      public enum NotificationsTypes
      {
        Info,
        Warning,
        Error,
      }
    }
  }
}
