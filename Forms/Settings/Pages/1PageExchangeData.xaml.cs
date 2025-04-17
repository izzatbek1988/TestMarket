// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageExchangeData
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
  public class PageExchangeData : Page, IComponentConnector
  {
    internal CheckBox LocalCb;
    internal CheckBox FtpCb;
    private bool _contentLoaded;

    public PageExchangeData() => this.InitializeComponent();

    public PageExchangeData(Gbs.Core.Config.Settings setting)
    {
      this.InitializeComponent();
      this.DataContext = (object) new ExchangeDataViewModel(setting);
    }

    public bool Save() => ((ExchangeDataViewModel) this.DataContext).Save();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pageexchangedata.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.FtpCb = (CheckBox) target;
        else
          this._contentLoaded = true;
      }
      else
        this.LocalCb = (CheckBox) target;
    }
  }
}
