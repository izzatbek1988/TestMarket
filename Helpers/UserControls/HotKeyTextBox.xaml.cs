// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.HotKeyTextBox
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class HotKeyTextBox : System.Windows.Controls.UserControl, IComponentConnector
  {
    public static readonly DependencyProperty HotkeyProperty = DependencyProperty.Register(nameof (Hotkey), typeof (HotKeysHelper.Hotkey), typeof (HotKeyTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public static readonly DependencyProperty ActionNameProperty = DependencyProperty.Register(nameof (ActionName), typeof (string), typeof (HotKeyTextBox), new PropertyMetadata((object) "Unknown action"));
    public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(nameof (ClearCommand), typeof (ICommand), typeof (HotKeyTextBox), new PropertyMetadata((object) null));
    internal HotKeyTextBox UserControl;
    internal WatermarkTextBox HotkeyTextBox;
    private bool _contentLoaded;

    public HotKeysHelper.Hotkey Hotkey
    {
      get => (HotKeysHelper.Hotkey) this.GetValue(HotKeyTextBox.HotkeyProperty);
      set => this.SetValue(HotKeyTextBox.HotkeyProperty, (object) value);
    }

    public string ActionName
    {
      get => (string) this.GetValue(HotKeyTextBox.ActionNameProperty);
      set => this.SetValue(HotKeyTextBox.ActionNameProperty, (object) value);
    }

    public HotKeyTextBox()
    {
      this.InitializeComponent();
      this.ClearCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Hotkey = new HotKeysHelper.Hotkey();
        this.HotkeyTextBox.Focus();
      }));
    }

    public ICommand ClearCommand
    {
      get => (ICommand) this.GetValue(HotKeyTextBox.ClearCommandProperty);
      set => this.SetValue(HotKeyTextBox.ClearCommandProperty, (object) value);
    }

    private void HotkeyTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
      e.Handled = true;
      ModifierKeys modifiers = Keyboard.Modifiers;
      Key key = e.Key;
      if (key == Key.System)
        key = e.SystemKey;
      if (modifiers == ModifierKeys.None)
      {
        if (key.IsEither<Key>(Key.Back))
        {
          this.Hotkey = new HotKeysHelper.Hotkey();
          return;
        }
      }
      if (key.IsEither<Key>(Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin, Key.Clear, Key.OemClear, Key.Apps))
        return;
      this.Hotkey = new HotKeysHelper.Hotkey(key, modifiers);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/hotkeytextbox.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
        {
          this.HotkeyTextBox = (WatermarkTextBox) target;
          this.HotkeyTextBox.PreviewKeyDown += new KeyEventHandler(this.HotkeyTextBox_OnPreviewKeyDown);
        }
        else
          this._contentLoaded = true;
      }
      else
        this.UserControl = (HotKeyTextBox) target;
    }
  }
}
