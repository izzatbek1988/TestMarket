// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FrmCloseProgram
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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

#nullable disable
namespace Gbs.Forms.Main
{
  public class FrmCloseProgram : WindowWithSize, IComponentConnector
  {
    private CloseViewModel Model;
    internal Button btnCancel;
    private bool _contentLoaded;

    public FrmCloseProgram() => this.InitializeComponent();

    public bool GetClosed()
    {
      CloseViewModel closeViewModel = new CloseViewModel();
      closeViewModel.CloseAction = new Action(((Window) this).Close);
      this.Model = closeViewModel;
      this.DataContext = (object) this.Model;
      this.SetHotKeys();
      this.ShowDialog();
      this.Model.TimerOff.Stop();
      return true;
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (o =>
        {
          if (!this.Model.CancelEnabled)
            return;
          this.Model.CancelOff.Execute((object) null);
        }));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.Model.CloseProgram
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand
          }
        };
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/main/frmcloseprogram.xaml", UriKind.Relative));
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
        this.btnCancel = (Button) target;
      else
        this._contentLoaded = true;
    }
  }
}
