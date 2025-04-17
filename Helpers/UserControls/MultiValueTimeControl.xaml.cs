// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.MultiValueTimeControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class MultiValueTimeControl : 
    UserControl,
    INotifyPropertyChanged,
    IComponentConnector
  {
    public static readonly DependencyProperty ValuesListProperty = DependencyProperty.Register(nameof (Values), typeof (ObservableCollection<MultiValueControl.Value>), typeof (MultiValueTimeControl), new PropertyMetadata((object) null));
    private Visibility _isTimeInputVisibility = Visibility.Collapsed;
    private ObservableCollection<MultiValueControl.Value> _values = new ObservableCollection<MultiValueControl.Value>();
    internal MultiValueTimeControl ctrl;
    internal Grid root;
    private bool _contentLoaded;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged(string info)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(info));
    }

    public ObservableCollection<MultiValueControl.Value> Values
    {
      get
      {
        return (ObservableCollection<MultiValueControl.Value>) this.GetValue(MultiValueTimeControl.ValuesListProperty);
      }
      set => this.SetValue(MultiValueTimeControl.ValuesListProperty, (object) value);
    }

    public Visibility IsTimeInputVisibility
    {
      get => this._isTimeInputVisibility;
      set
      {
        this._isTimeInputVisibility = value;
        this.NotifyPropertyChanged(nameof (IsTimeInputVisibility));
      }
    }

    public DateTime TimeValueToAdd { get; set; }

    public ICommand AddTimeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          string str = this.TimeValueToAdd.ToString("HH:mm");
          this.Values.Add(new MultiValueControl.Value()
          {
            DisplayedValue = str
          });
          this.NotifyPropertyChanged("Values");
          this.IsTimeInputVisibility = Visibility.Collapsed;
        }));
      }
    }

    public ICommand ShowTimeInputCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.IsTimeInputVisibility = Visibility.Visible));
      }
    }

    public ICommand CancelTimeInputCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.IsTimeInputVisibility = Visibility.Collapsed));
      }
    }

    public MultiValueTimeControl() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/multivaluetimecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.root = (Grid) target;
        else
          this._contentLoaded = true;
      }
      else
        this.ctrl = (MultiValueTimeControl) target;
    }
  }
}
