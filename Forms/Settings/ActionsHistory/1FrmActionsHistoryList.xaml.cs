// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ActionsHistory.FrmActionsHistoryList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
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
namespace Gbs.Forms.Settings.ActionsHistory
{
  public class FrmActionsHistoryList : WindowWithSize, IComponentConnector
  {
    internal DataGrid DataGridHistory;
    private bool _contentLoaded;

    public FrmActionsHistoryList()
    {
      this.InitializeComponent();
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
        }
      };
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/actionshistory/frmactionshistorylist.xaml", UriKind.Relative));
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
        this.DataGridHistory = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
