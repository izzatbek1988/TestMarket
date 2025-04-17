// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.MultiValueIntControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class MultiValueIntControl : 
    UserControl,
    INotifyPropertyChanged,
    IComponentConnector
  {
    public static readonly DependencyProperty ValuesListProperty = DependencyProperty.Register(nameof (Values), typeof (ObservableCollection<MultiValueControl.Value>), typeof (MultiValueIntControl), new PropertyMetadata((object) null));
    private Visibility _isIntInputVisibility = Visibility.Collapsed;
    private ObservableCollection<MultiValueControl.Value> _values = new ObservableCollection<MultiValueControl.Value>();
    internal MultiValueIntControl ctrl;
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
        return (ObservableCollection<MultiValueControl.Value>) this.GetValue(MultiValueIntControl.ValuesListProperty);
      }
      set => this.SetValue(MultiValueIntControl.ValuesListProperty, (object) value);
    }

    public Visibility IsIntInputVisibility
    {
      get => this._isIntInputVisibility;
      set
      {
        this._isIntInputVisibility = value;
        this.NotifyPropertyChanged(nameof (IsIntInputVisibility));
      }
    }

    public int IntValueToAdd { get; set; }

    public ICommand AddIntCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          string str = this.IntValueToAdd.ToString("N0");
          if (this.IntValueToAdd == 0)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.MultiValueIntControl_AddIntCommand_Нужно_указать_положительное__отличное_от_0_число_);
          }
          else if (this.Values.Any<MultiValueControl.Value>((Func<MultiValueControl.Value, bool>) (x => int.Parse(x.DisplayedValue) == this.IntValueToAdd)))
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.MultiValueIntControl_AddIntCommand_Данное_число_уже_указано_в_списке_);
          }
          else
          {
            this.Values.Add(new MultiValueControl.Value()
            {
              DisplayedValue = str
            });
            this.NotifyPropertyChanged("Values");
            this.IsIntInputVisibility = Visibility.Collapsed;
          }
        }));
      }
    }

    public ICommand ShowIntInputCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.IsIntInputVisibility = Visibility.Visible));
      }
    }

    public ICommand CancelIntInputCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.IsIntInputVisibility = Visibility.Collapsed));
      }
    }

    public MultiValueIntControl() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/multivalueintcontrol.xaml", UriKind.Relative));
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
        this.ctrl = (MultiValueIntControl) target;
    }
  }
}
