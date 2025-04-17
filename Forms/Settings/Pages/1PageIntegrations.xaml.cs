// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageIntegrations
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
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageIntegrations : Page, IComponentConnector
  {
    internal CheckBox IsJsonApiEnabled;
    internal CheckBox IsActivePlanFix;
    internal CheckBox CheckBoxSale;
    internal WatermarkPasswordBox PasswordBox;
    internal WatermarkPasswordBox PassSmsPasswordBox;
    internal CheckBox DaDataIsActiveCb;
    internal CheckBox IsActivePolyCard;
    private bool _contentLoaded;

    private IntegrationViewModel MyViewModel { get; }

    public PageIntegrations() => this.InitializeComponent();

    public PageIntegrations(Integrations integrations, Gbs.Core.Config.Settings settings)
    {
      this.InitializeComponent();
      this.MyViewModel = new IntegrationViewModel(integrations, settings);
      this.DataContext = (object) this.MyViewModel;
      this.PasswordBox.Password = ((IntegrationViewModel) this.DataContext).Integrations.Sms.ApiKey.DecryptedValue;
      this.PassSmsPasswordBox.Password = ((IntegrationViewModel) this.DataContext).Integrations.Sms.Password.DecryptedValue;
    }

    public bool Save() => this.MyViewModel.Save();

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      ((IntegrationViewModel) this.DataContext).Integrations.Sms.ApiKey.DecryptedValue = ((WatermarkPasswordBox) sender).Password;
    }

    private void PassSmsPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      ((IntegrationViewModel) this.DataContext).Integrations.Sms.Password.DecryptedValue = ((WatermarkPasswordBox) sender).Password;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pageintegrations.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.IsJsonApiEnabled = (CheckBox) target;
          break;
        case 2:
          this.IsActivePlanFix = (CheckBox) target;
          break;
        case 3:
          this.CheckBoxSale = (CheckBox) target;
          break;
        case 4:
          this.PasswordBox = (WatermarkPasswordBox) target;
          this.PasswordBox.PasswordChanged += new RoutedEventHandler(this.PasswordBox_OnPasswordChanged);
          break;
        case 5:
          this.PassSmsPasswordBox = (WatermarkPasswordBox) target;
          this.PassSmsPasswordBox.PasswordChanged += new RoutedEventHandler(this.PassSmsPasswordBox_OnPasswordChanged);
          break;
        case 6:
          this.DaDataIsActiveCb = (CheckBox) target;
          break;
        case 7:
          this.IsActivePolyCard = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
