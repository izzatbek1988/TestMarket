// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.PageBarcodeScanner
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
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public class PageBarcodeScanner : Page, IComponentConnector
  {
    internal ComboBox cbScannerType;
    private bool _contentLoaded;

    public PageBarcodeScanner(Devices devicesConfig, Gbs.Core.Config.Settings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) new PageBarcodeScannerViewModel(devicesConfig, settings);
    }

    public PageBarcodeScanner() => this.InitializeComponent();

    public bool ValidationConfig()
    {
      return ((PageBarcodeScannerViewModel) this.DataContext).ValidationConfig();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/devicessubpages/pagebarcodescanner.xaml", UriKind.Relative));
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
        this.cbScannerType = (ComboBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
