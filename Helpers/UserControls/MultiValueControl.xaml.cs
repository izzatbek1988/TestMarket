// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.MultiValueControl
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
  public partial class MultiValueControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(nameof (ValuesList), typeof (ObservableCollection<MultiValueControl.Value>), typeof (MultiValueControl), new PropertyMetadata((object) new ObservableCollection<MultiValueControl.Value>()));
    public static readonly DependencyProperty AddValueCommandProperty = DependencyProperty.Register(nameof (AddValueCommand), typeof (ICommand), typeof (MultiValueControl), new PropertyMetadata((object) null));
    internal MultiValueControl MultiValueCntrl;
    internal ScrollViewer ScrollViewer;
    internal ItemsControl Items;
    private bool _contentLoaded;

    public ICommand AddValueCommand
    {
      get => (ICommand) this.GetValue(MultiValueControl.AddValueCommandProperty);
      set => this.SetValue(MultiValueControl.AddValueCommandProperty, (object) value);
    }

    public ICommand ClearCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => this.ValuesList.Clear()));
    }

    public ICommand DeleteValueCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.ValuesList.Remove((MultiValueControl.Value) obj)));
      }
    }

    public ObservableCollection<MultiValueControl.Value> ValuesList
    {
      get
      {
        return (ObservableCollection<MultiValueControl.Value>) this.GetValue(MultiValueControl.ValuesProperty);
      }
      set => this.SetValue(MultiValueControl.ValuesProperty, (object) value);
    }

    public MultiValueControl() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/multivaluecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MultiValueCntrl = (MultiValueControl) target;
          break;
        case 2:
          this.ScrollViewer = (ScrollViewer) target;
          break;
        case 3:
          this.Items = (ItemsControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public class Value
    {
      public object Object { get; set; }

      public string DisplayedValue { get; set; }
    }
  }
}
