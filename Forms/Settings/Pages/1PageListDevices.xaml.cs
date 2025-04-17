// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageListDevices
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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
  public class PageListDevices : Page, IComponentConnector
  {
    internal DataGrid DataGridDevices;
    private bool _contentLoaded;

    private ListDevicesViewModel MyViewModel { get; }

    public PageListDevices()
    {
    }

    public PageListDevices(Devices devicesConfig, Gbs.Core.Config.Settings settings)
    {
      this.InitializeComponent();
      this.MyViewModel = new ListDevicesViewModel(devicesConfig, settings);
      this.DataContext = (object) this.MyViewModel;
    }

    public bool Save() => this.MyViewModel.Save();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pagelistdevices.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.DataGridDevices = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
