// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageRemoteControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageRemoteControl : Page, IComponentConnector
  {
    internal CheckBox RemoteGroupBoxCb;
    internal RadioButton CloudTimeCb;
    internal CheckBox ClientCheckBox;
    internal CheckBox EmailGroupBoxCb;
    internal CheckBox EmailTimeCb;
    internal CheckBox EmailSumCb;
    internal CheckBox PersonalEmailCheckBox;
    internal CheckBox TelegramGroupBoxCb;
    internal CheckBox TelegramTimeCb;
    internal CheckBox TelegramSumCb;
    private bool _contentLoaded;

    private RemoteControlViewModel Model { get; set; }

    public PageRemoteControl() => this.InitializeComponent();

    public PageRemoteControl(Gbs.Core.Config.Settings setting)
    {
      this.InitializeComponent();
      this.Model = new RemoteControlViewModel(setting)
      {
        Page = this
      };
      this.DataContext = (object) this.Model;
    }

    public bool Save() => this.Model.Save();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pageremotecontrol.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.RemoteGroupBoxCb = (CheckBox) target;
          break;
        case 2:
          this.CloudTimeCb = (RadioButton) target;
          break;
        case 3:
          this.ClientCheckBox = (CheckBox) target;
          break;
        case 4:
          this.EmailGroupBoxCb = (CheckBox) target;
          break;
        case 5:
          this.EmailTimeCb = (CheckBox) target;
          break;
        case 6:
          this.EmailSumCb = (CheckBox) target;
          break;
        case 7:
          this.PersonalEmailCheckBox = (CheckBox) target;
          break;
        case 8:
          this.TelegramGroupBoxCb = (CheckBox) target;
          break;
        case 9:
          this.TelegramTimeCb = (CheckBox) target;
          break;
        case 10:
          this.TelegramSumCb = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
