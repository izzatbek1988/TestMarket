// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.FrmConnectionSettings
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
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class FrmConnectionSettings : WindowWithSize, IComponentConnector
  {
    internal WatermarkPasswordBox PasswordBox;
    private bool _contentLoaded;

    public FrmConnectionSettings() => this.InitializeComponent();

    public void ShowConfig(
      DeviceConnection connection,
      ConnectionSettingsViewModel.PortsConfig connectionVariants,
      bool needAuth = false)
    {
      ConnectionSettingsViewModel settingsViewModel1 = new ConnectionSettingsViewModel(new ConnectionSettingsViewModel.ConnectionConfig(connection, connectionVariants)
      {
        NeedAuth = needAuth
      });
      settingsViewModel1.CloseAction = new Action(((Window) this).Close);
      ConnectionSettingsViewModel settingsViewModel2 = settingsViewModel1;
      this.DataContext = (object) settingsViewModel2;
      this.SetPassValue();
      this.ShowDialog();
      connection.ComPort = settingsViewModel2.ComPort;
      connection.LanPort = settingsViewModel2.Connection;
      connection.ConnectionType = settingsViewModel2.SelectedConnectionType;
    }

    private void SetPassValue()
    {
      ConnectionSettingsViewModel dataContext = (ConnectionSettingsViewModel) this.DataContext;
      if (dataContext == null)
        return;
      this.PasswordBox.Password = dataContext?.Connection?.Password ?? string.Empty;
    }

    public void ShowConfig(
      ConnectionSettingsViewModel.ConnectionConfig config)
    {
      try
      {
        ConnectionSettingsViewModel settingsViewModel1 = new ConnectionSettingsViewModel(config);
        settingsViewModel1.CloseAction = new Action(((Window) this).Close);
        ConnectionSettingsViewModel settingsViewModel2 = settingsViewModel1;
        this.DataContext = (object) settingsViewModel2;
        this.SetPassValue();
        this.ShowDialog();
        config.Type = settingsViewModel2.SelectedConnectionType;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме настроек подключения по сети");
      }
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      string password = ((WatermarkPasswordBox) sender).Password;
      ConnectionSettingsViewModel dataContext = (ConnectionSettingsViewModel) this.DataContext;
      if (dataContext.Connection == null)
        return;
      dataContext.Connection.Password = password;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/devices/frmconnectionsettings.xaml", UriKind.Relative));
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
      {
        this.PasswordBox = (WatermarkPasswordBox) target;
        this.PasswordBox.PasswordChanged += new RoutedEventHandler(this.PasswordBox_OnPasswordChanged);
      }
      else
        this._contentLoaded = true;
    }
  }
}
