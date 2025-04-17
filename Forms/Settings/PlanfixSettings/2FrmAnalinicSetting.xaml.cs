// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.FrmAnaliticSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Planfix.Api.Entities.Analitics;
using Planfix.Api.Entities.Handbooks;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings
{
  public class FrmAnaliticSetting : WindowWithSize, IComponentConnector
  {
    private bool _contentLoaded;

    public FrmAnaliticSetting() => this.InitializeComponent();

    public void ShowSettingAnalitic(
      Integrations setting,
      AnaliticSettingViewModel.TypeAnalitic typeAnalitic,
      AnaliticInfo analiticInfo)
    {
      AnaliticSettingViewModel settingViewModel = new AnaliticSettingViewModel(setting, typeAnalitic, analiticInfo);
      settingViewModel.CloseAction = new Action(((Window) this).Close);
      this.DataContext = (object) settingViewModel;
      this.ShowDialog();
    }

    public void ShowSettingGoodHandbook(Integrations setting, Handbook handbook)
    {
      HandbookGoodSettingViewModel settingViewModel = new HandbookGoodSettingViewModel(setting, handbook);
      settingViewModel.CloseAction = new Action(((Window) this).Close);
      this.DataContext = (object) settingViewModel;
      this.ShowDialog();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/planfixsettings/frmanalinicsetting.xaml", UriKind.Relative));
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
