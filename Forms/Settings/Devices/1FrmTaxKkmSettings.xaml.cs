// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.FrmTaxKkmSettings
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public class FrmTaxKkmSettings : WindowWithSize, IComponentConnector
  {
    internal DataGrid GridPaymentsIndex;
    internal Button btnSave;
    private bool _contentLoaded;

    public FrmTaxKkmSettings() => this.InitializeComponent();

    public void ShowConfig(FiscalKkm kkmConfig)
    {
      try
      {
        this.DataContext = (object) new TaxKkmViewModel(kkmConfig)
        {
          Close = new Action(((Window) this).Close)
        };
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме способов оплаты для ККМ");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/devices/frmtaxkkmsettings.xaml", UriKind.Relative));
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
          this.btnSave = (Button) target;
        else
          this._contentLoaded = true;
      }
      else
        this.GridPaymentsIndex = (DataGrid) target;
    }
  }
}
