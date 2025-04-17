// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.SecondMonitorPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.DisplayBuyers;
using Gbs.Core.Devices.DisplayNumbers;
using Gbs.Core.Devices.DisplayQR;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public partial class SecondMonitorPageViewModel : ViewModel
  {
    public Dictionary<GlobalDictionaries.Encoding, string> EncodingDirectory
    {
      get => GlobalDictionaries.EncodingDictionary;
    }

    public Visibility VisibilitySettingDisplay
    {
      get => this.Display != DisplayBuyerTypes.None ? Visibility.Visible : Visibility.Collapsed;
    }

    public Visibility VisibilitySettingEcsDisplay
    {
      get => this.Display != DisplayBuyerTypes.EscPos ? Visibility.Collapsed : Visibility.Visible;
    }

    public Visibility VisibilitySettingDisplayQr
    {
      get => this.DisplayQr != DisplayQrTypes.None ? Visibility.Visible : Visibility.Collapsed;
    }

    public Visibility QrDisplayVisibility
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public string Monitor
    {
      get
      {
        return this.DevicesConfig?.SecondMonitor.MonitorName ?? Translate.SecondMonitorPageViewModel_MonitorList_Нет;
      }
      set
      {
        this.DevicesConfig.SecondMonitor.MonitorName = value;
        this.OnPropertyChanged(nameof (Monitor));
        this.OnPropertyChanged("VisibilityImage");
      }
    }

    public DisplayBuyerTypes Display
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? DisplayBuyerTypes.None : devicesConfig.DisplayPayer.Type;
      }
      set
      {
        this.DevicesConfig.DisplayPayer.Type = value;
        this.OnPropertyChanged(nameof (Display));
        this.OnPropertyChanged("VisibilitySettingDisplay");
        this.OnPropertyChanged("VisibilitySettingEcsDisplay");
      }
    }

    public DisplayQrTypes DisplayQr
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? DisplayQrTypes.None : devicesConfig.DisplayQr.Type;
      }
      set
      {
        this.DevicesConfig.DisplayQr.Type = value;
        this.OnPropertyChanged(nameof (DisplayQr));
        this.OnPropertyChanged("VisibilitySettingDisplayQr");
      }
    }

    public DisplayNumbersTypes DisplayNumbers
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? DisplayNumbersTypes.None : devicesConfig.DisplayNumbersPayer.Type;
      }
      set
      {
        this.DevicesConfig.DisplayNumbersPayer.Type = value;
        this.OnPropertyChanged(nameof (DisplayNumbers));
        this.OnPropertyChanged("VisibilitySettingDisplayNumbers");
      }
    }

    public ICommand DoSettingDisplayCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayBuyerHelper displayBuyerHelper = new DisplayBuyerHelper((IConfig) this.DevicesConfig))
            displayBuyerHelper.ShowProperties();
        }));
      }
    }

    public ICommand TestConnectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayBuyerHelper displayBuyerHelper = new DisplayBuyerHelper((IConfig) this.DevicesConfig))
            displayBuyerHelper.WriteText(new List<string>()
            {
              "Test (ABC abc) 123...",
              Translate.MainWindowViewModel_InitDisplayBuyer_Добро_пожаловать_
            }, true);
        }));
      }
    }

    public ICommand TestConnectionDisplayNumbersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayNumbersHelper displayNumbersHelper = new DisplayNumbersHelper((IConfig) this.DevicesConfig))
            displayNumbersHelper.WriteNumber(123.12M);
        }));
      }
    }

    public ICommand DoSettingDisplayNumbersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayNumbersHelper displayNumbersHelper = new DisplayNumbersHelper((IConfig) this.DevicesConfig))
            displayNumbersHelper.ShowProperties();
        }));
      }
    }

    public ICommand DoSettingDisplayQrCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayQrHelper displayQrHelper = new DisplayQrHelper((IConfig) this.DevicesConfig))
            displayQrHelper.ShowProperties();
        }));
      }
    }

    public ICommand TestConnectionQrCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DisplayQrHelper displayQrHelper = new DisplayQrHelper((IConfig) this.DevicesConfig))
            displayQrHelper.WriteQr("https://gbsmarket.ru/");
        }));
      }
    }

    public Visibility VisibilityImage
    {
      get
      {
        return !(this.DevicesConfig?.SecondMonitor.MonitorName == Translate.SecondMonitorPageViewModel_MonitorList_Нет) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public SecondMonitorPageViewModel()
    {
      this.MonitorList.AddRange(((IEnumerable<Screen>) Screen.AllScreens).Select<Screen, string>((Func<Screen, string>) (x => x.DeviceName)));
    }

    public List<string> MonitorList { get; set; } = new List<string>()
    {
      Translate.SecondMonitorPageViewModel_MonitorList_Нет
    };

    public Dictionary<DisplayBuyerTypes, string> DisplayList
    {
      get
      {
        return !DevelopersHelper.IsDebug() ? GlobalDictionaries.Devices.DisplayBuyerTypesDictionary().Where<KeyValuePair<DisplayBuyerTypes, string>>((Func<KeyValuePair<DisplayBuyerTypes, string>, bool>) (x => x.Key.IsEither<DisplayBuyerTypes>(DisplayBuyerTypes.None, DisplayBuyerTypes.ShtrihM, DisplayBuyerTypes.EscPos))).ToDictionary<KeyValuePair<DisplayBuyerTypes, string>, DisplayBuyerTypes, string>((Func<KeyValuePair<DisplayBuyerTypes, string>, DisplayBuyerTypes>) (x => x.Key), (Func<KeyValuePair<DisplayBuyerTypes, string>, string>) (v => v.Value)) : GlobalDictionaries.Devices.DisplayBuyerTypesDictionary();
      }
    }

    public Dictionary<DisplayQrTypes, string> DisplayQrList
    {
      get => GlobalDictionaries.Devices.DisplayQrTypesDictionary();
    }

    public Dictionary<DisplayNumbersTypes, string> DisplayNumbersList
    {
      get => GlobalDictionaries.Devices.DisplayNumbersBuyerTypesDictionary();
    }

    public Visibility VisibilitySettingDisplayNumbers
    {
      get
      {
        return this.DisplayNumbers != DisplayNumbersTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }
  }
}
