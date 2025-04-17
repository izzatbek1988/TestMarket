// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.TsdSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Tsd;
using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public partial class TsdSettingViewModel : ViewModel
  {
    public Dictionary<GlobalDictionaries.Devices.TsdTypes, string> TsdTypes { get; set; } = GlobalDictionaries.Devices.TsdTypesDictionary();

    public GlobalDictionaries.Devices.TsdTypes TsdType
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? GlobalDictionaries.Devices.TsdTypes.None : devicesConfig.Tsd.Type;
      }
      set
      {
        this.DevicesConfig.Tsd.Type = value;
        this.OnPropertyChanged(nameof (TsdType));
        this.OnPropertyChanged("ConnectionsConfigTsdVisible");
      }
    }

    public Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public TsdSettingViewModel()
    {
    }

    public TsdSettingViewModel(Gbs.Core.Config.Devices devicesConfig)
    {
      this.DevicesConfig = devicesConfig;
    }

    public ICommand ShowTsdSetting
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (TsdHelper tsdHelper = new TsdHelper((IConfig) this.DevicesConfig))
            tsdHelper.ShowProperties();
        }));
      }
    }

    public ICommand CheckTsdConnection
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (TsdHelper tsdHelper = new TsdHelper((IConfig) this.DevicesConfig))
            tsdHelper.TestConnect();
        }));
      }
    }

    public Visibility ConnectionsConfigTsdVisible
    {
      get
      {
        return this.TsdType == GlobalDictionaries.Devices.TsdTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }
  }
}
