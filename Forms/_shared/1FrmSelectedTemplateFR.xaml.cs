// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmSelectedTemplateFR
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
namespace Gbs.Forms._shared
{
  public class FrmSelectedTemplateFR : WindowWithSize, IComponentConnector
  {
    private TemplateFrViewModel Model;
    internal DataGrid ListTemplates;
    private bool _contentLoaded;

    public FrmSelectedTemplateFR()
    {
      this.InitializeComponent();
      this.Object = (Control) this.ListTemplates;
    }

    public (string Path, bool Result) GetTemplateFR(string directoryPath, Gbs.Core.Entities.Users.User authUser)
    {
      try
      {
        this.Model = new TemplateFrViewModel(directoryPath, new Action(((Window) this).Close))
        {
          AuthUser = authUser
        };
        this.DataContext = (object) this.Model;
        this.SetHotKeys();
        if (!this.Model.CanShow)
          return (string.Empty, false);
        this.ShowDialog();
        return (this.Model.SelectedFile?.FileInfo.FullName, this.Model.ResultAction);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка выбора шаблона для печати");
        return ((string) null, false);
      }
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (o => this.Model.Close()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.Model.SelectedItem
          },
          {
            new HotKeysHelper.Hotkey(Key.Return),
            this.Model.SelectedItem
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmselectedtemplatefr.xaml", UriKind.Relative));
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
        this.ListTemplates = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
