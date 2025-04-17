// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FrmFirstMain
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Main
{
  public class FrmFirstMain : WindowWithSize, IComponentConnector
  {
    private readonly FirstSetupViewModel _model;
    internal System.Windows.Controls.Frame PageContent;
    private bool _contentLoaded;

    public FrmFirstMain()
    {
      this.InitializeComponent();
      this._model = (FirstSetupViewModel) this.DataContext;
      this._model.CloseAction = new Action(((Window) this).Close);
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.OkAction,
          this._model.NextPage
        },
        {
          hotKeys.CancelAction,
          this._model.LastPage
        }
      };
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (!this._model.ConfirmClose)
        return;
      if (MessageBoxHelper.Show(Translate.ЗавершитьПервичнуюНастройкуИЗакрытьПрограмму, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
      {
        e.Cancel = true;
      }
      else
      {
        Other.SetCorrectExit();
        System.Environment.Exit(0);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/main/frmfirstmain.xaml", UriKind.Relative));
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
        this.PageContent = (System.Windows.Controls.Frame) target;
      else
        this._contentLoaded = true;
    }
  }
}
