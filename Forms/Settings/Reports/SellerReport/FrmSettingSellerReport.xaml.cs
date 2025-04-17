// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Reports.SellerReport.SellerReportSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Reports.SellerReport
{
  public partial class SellerReportSettingViewModel : ViewModelWithForm
  {
    public SellerReportSetting Setting { get; set; } = new SellerReportRepository().GetSetting();

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new SellerReportRepository().Save(this.Setting);
          this.CloseAction();
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public void ShowSetting()
    {
      this.FormToSHow = (WindowWithSize) new FrmSettingSellerReport();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
    }
  }
}
