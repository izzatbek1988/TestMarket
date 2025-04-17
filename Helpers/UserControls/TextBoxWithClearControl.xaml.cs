// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.TextBoxWithClearControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class TextBoxWithClearControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(nameof (TextFontSize), typeof (double), typeof (TextBoxWithClearControl), new PropertyMetadata((object) 12.0));
    public static readonly DependencyProperty TextStringProperty = DependencyProperty.Register(nameof (TextString), typeof (string), typeof (TextBoxWithClearControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TextBoxWidthProperty = DependencyProperty.Register(nameof (TextBoxWidth), typeof (double), typeof (TextBoxWithClearControl), new PropertyMetadata((object) 50.0));
    public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(nameof (ClearCommand), typeof (ICommand), typeof (TextBoxWithClearControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ConfigCommandProperty = DependencyProperty.Register(nameof (ConfigCommand), typeof (ICommand), typeof (TextBoxWithClearControl), new PropertyMetadata((object) null));
    internal WatermarkTextBox SearchTextBox;
    internal Button ConfigBtn;
    private bool _contentLoaded;

    public double TextFontSize
    {
      get => (double) this.GetValue(TextBoxWithClearControl.TextFontSizeProperty);
      set
      {
        this.SetValue(TextBoxWithClearControl.TextFontSizeProperty, (object) TextBoxWithClearControl.SafeDouble(value));
      }
    }

    public double TextBoxWidth
    {
      get => (double) this.GetValue(TextBoxWithClearControl.TextBoxWidthProperty);
      set
      {
        this.SetValue(TextBoxWithClearControl.TextBoxWidthProperty, (object) TextBoxWithClearControl.SafeDouble(value));
      }
    }

    private static double SafeDouble(double value)
    {
      return !double.IsNaN(value) && !double.IsInfinity(value) && value >= 0.0 && value <= 1000.0 ? value : 12.0;
    }

    public string TextString
    {
      get => (string) this.GetValue(TextBoxWithClearControl.TextStringProperty);
      set => this.SetValue(TextBoxWithClearControl.TextStringProperty, (object) value);
    }

    public ICommand ClearCommand
    {
      get => (ICommand) this.GetValue(TextBoxWithClearControl.ClearCommandProperty);
      set => this.SetValue(TextBoxWithClearControl.ClearCommandProperty, (object) value);
    }

    public Visibility ConfigButtonVisibility
    {
      get => this.ConfigCommand != null ? Visibility.Visible : Visibility.Collapsed;
    }

    public ICommand ConfigCommand
    {
      get => (ICommand) this.GetValue(TextBoxWithClearControl.ConfigCommandProperty);
      set => this.SetValue(TextBoxWithClearControl.ConfigCommandProperty, (object) value);
    }

    public TextBoxWithClearControl()
    {
      try
      {
        this.InitializeComponent();
        this.ClearCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.TextString.IsNullOrEmpty())
            this.TextString = string.Empty;
          this.SearchTextBox.Focus();
        }));
        this.GotFocus += new RoutedEventHandler(this.TextBoxWithClearControl_GotFocus);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void TextBoxWithClearControl_GotFocus(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SearchTextBox.Focus();
        Keyboard.Focus((IInputElement) this.SearchTextBox);
        this.SearchTextBox.SelectionStart = this.SearchTextBox.Text.Length;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/textboxwithclearcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.ConfigBtn = (Button) target;
        else
          this._contentLoaded = true;
      }
      else
        this.SearchTextBox = (WatermarkTextBox) target;
    }
  }
}
