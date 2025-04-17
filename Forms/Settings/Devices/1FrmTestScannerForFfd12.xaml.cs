// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.FrmTestScannerForFfd12
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WindowsInput;
using WindowsInput.Native;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public class FrmTestScannerForFfd12 : WindowWithSize, IComponentConnector
  {
    private HotKeysHelper.Hotkey GsKey;
    private TestScannerForFfd12ViewModel Model;
    private string _pushedKeys = string.Empty;
    internal TextBox MatrixTextBox;
    private bool _contentLoaded;

    public FrmTestScannerForFfd12() => this.InitializeComponent();

    public FrmTestScannerForFfd12(HotKeysHelper.Hotkey gsHotKey, TestScannerForFfd12ViewModel model)
    {
      this.InitializeComponent();
      this.PreviewKeyDown += new KeyEventHandler(this.FrmTestScannerForFfd12_PreviewKeyDown);
      this.Closed += new EventHandler(this.FrmTestScannerForFfd12_Closed);
      this.GsKey = gsHotKey;
      this.Model = model;
      this.SetGsHotKey();
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.Model.ComPortScannerOnBarcodeChanged));
    }

    private void SetGsHotKey()
    {
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          this.GsKey,
          (ICommand) new RelayCommand((Action<object>) (o => new InputSimulator().Keyboard.KeyPress(VirtualKeyCode.SPACE)))
        }
      };
    }

    private void FrmTestScannerForFfd12_Closed(object sender, EventArgs e)
    {
      LogHelper.Trace("В форме проверки сканера были нажаты: " + this._pushedKeys);
    }

    private void FrmTestScannerForFfd12_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      this._pushedKeys = this._pushedKeys + " " + e.Key.ToString();
      ((TestScannerForFfd12ViewModel) this.DataContext).CurrentKey = e.Key;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/devices/frmtestscannerforffd12.xaml", UriKind.Relative));
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
      if (connectionId == 1)
        this.MatrixTextBox = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
