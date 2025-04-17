// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.FrmSendDigitalCheck
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Other
{
  public partial class FrmSendDigitalCheck : WindowWithSize, IComponentConnector
  {
    private bool _contentLoaded;

    public FrmSendDigitalCheck() => this.InitializeComponent();

    public bool ShowForm(Client clint, out string address)
    {
      SendDigitalCheckViewModel digitalCheckViewModel = new SendDigitalCheckViewModel(clint);
      this.DataContext = (object) digitalCheckViewModel;
      digitalCheckViewModel.CloseAction = new Action(((Window) this).Close);
      this.SetHotKeys();
      this.ShowDialog();
      address = digitalCheckViewModel.Address;
      return digitalCheckViewModel.Result;
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        SendDigitalCheckViewModel dataContext = (SendDigitalCheckViewModel) this.DataContext;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            dataContext.SendCommand
          },
          {
            hotKeys.CancelAction,
            dataContext.CancelCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            dataContext.CancelCommand
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/other/frmsenddigitalcheck.xaml", UriKind.Relative));
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
