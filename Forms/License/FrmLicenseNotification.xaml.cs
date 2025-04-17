// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.License.FrmLicenseNotification
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.License
{
  public partial class FrmLicenseNotification : WindowWithSize, IComponentConnector
  {
    private bool _isCloseMarket;
    private bool _contentLoaded;

    public FrmLicenseNotification() => this.InitializeComponent();

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (((LicenseNotificationViewModel) this.DataContext).ContinueButtonEnable)
        return;
      if (MessageBoxHelper.Show(Translate.LoginUsersViewModel_Закрыть_программу_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.Yes)
        this._isCloseMarket = true;
      else
        e.Cancel = true;
    }

    private void FrmLicenseNotification_OnClosed(object sender, EventArgs e)
    {
      if (!this._isCloseMarket)
        return;
      Other.CloseApplication();
    }

    private void WindowWithSize_Drop(object sender, DragEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/license/frmlicensenotification.xaml", UriKind.Relative));
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
