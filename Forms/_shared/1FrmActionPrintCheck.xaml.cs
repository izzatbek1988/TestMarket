// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmActionPrintCheck
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmActionPrintCheck : WindowWithSize, IComponentConnector
  {
    private ActionPrintViewModel Model;
    private bool _contentLoaded;

    public FrmActionPrintCheck() => this.InitializeComponent();

    public FrmActionPrintCheck(ActionPrintViewModel model)
    {
      this.InitializeComponent();
      this.Model = model;
      this.SetHotKeys();
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (obj => { }));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.Model.PrintFiscalCheck
          },
          {
            hotKeys.CancelAction,
            this.Model.PrintCancel
          },
          {
            hotKeys.Print,
            this.Model.PrintDocument
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            this.Model.PrintCancel
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmactionprintcheck.xaml", UriKind.Relative));
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
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
