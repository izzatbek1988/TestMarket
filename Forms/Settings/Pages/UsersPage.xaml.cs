// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.UsersPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms.Settings.ActionsHistory;
using Gbs.Forms.Settings.Reports.SellerReport;
using Gbs.Forms.Users;
using Gbs.Forms.Users.UsersGroup;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class UsersPageViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();
    private Gbs.Core.Config.Devices _devices;

    public void UpdateEnableRequestAuthorizationOnSale(Gbs.Core.Config.Devices device)
    {
      this._devices = device;
      this.OnPropertyChanged("IsEnableRequestAuthorizationOnSale");
    }

    public bool IsEnableRequestAuthorizationOnSale
    {
      get
      {
        if (this._devices == null)
          return true;
        Gbs.Core.Config.Devices devices1 = this._devices;
        if ((devices1 != null ? (devices1.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? 1 : 0) : 1) == 0)
        {
          Gbs.Core.Config.Devices devices2 = this._devices;
          if ((devices2 != null ? (devices2.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.HiPos ? 1 : 0) : 1) == 0)
          {
            Gbs.Core.Config.Devices devices3 = this._devices;
            if ((devices3 != null ? (devices3.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.Hdm ? 1 : 0) : 1) == 0)
            {
              this.RequestAuthorizationOnSale = true;
              return false;
            }
          }
        }
        return true;
      }
    }

    public bool RequestAuthorizationOnSale
    {
      get => this._settings.Users.RequestAuthorizationOnSale;
      set
      {
        this._settings.Users.RequestAuthorizationOnSale = value;
        this.OnPropertyChanged(nameof (RequestAuthorizationOnSale));
      }
    }

    public Dictionary<Gbs.Core.Config.Users.AuthorizationMethods, string> AuthorizationMethodDictionary { get; set; } = Gbs.Core.Config.Users.AuthorizationMethodsDictionary();

    public Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public Visibility IsBlockForHome
    {
      get
      {
        return SettingsViewModel.DataBaseConfig.ModeProgram != GlobalDictionaries.Mode.Home ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand ShowUsersList { get; set; }

    public ICommand ShowUserGroups { get; set; }

    public ICommand SettingSellerReport
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new SellerReportSettingViewModel().ShowSetting()));
      }
    }

    public ICommand ShowActionsHistoryList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ActionsHistoryListViewModel().ShowHistory()));
      }
    }

    public UsersPageViewModel()
    {
    }

    public UsersPageViewModel(Gbs.Core.Config.Settings settings)
    {
      this.Settings = settings;
      this.ShowUserGroups = (ICommand) new RelayCommand((Action<object>) (obj => new FrmListUserGroups().ShowDialog()));
      this.ShowUsersList = (ICommand) new RelayCommand((Action<object>) (obj => new FrmUserList().ShowDialog()));
    }

    public bool Save() => true;
  }
}
