// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.PageBarcodeScannerViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public partial class PageBarcodeScannerViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.Devices _devicesConfig = new Gbs.Core.Config.Devices();
    private Gbs.Core.Config.Settings _settingsConfig = new Gbs.Core.Config.Settings();

    public ICommand ShowScannerSettingsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.BarcodeScanner.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom))));
      }
    }

    public ICommand CheckScannerConnection
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ComPortScanner.Stop();
          ComPortScanner.Start(this.DevicesConfig.BarcodeScanner.ComPort);
          MessageBoxHelper.Input(string.Empty, Translate.PageBarcodeScannerViewModel_Установите_курсор_в_поле_ниже_и_отсканируйте_штрих_код);
        }));
      }
    }

    public Dictionary<GlobalDictionaries.Devices.ScannerTypes, string> BarcodeScannerTypesDictionary
    {
      get => GlobalDictionaries.Devices.ScannerTypesDictionary();
    }

    public Visibility ConnectionsConfigVisible
    {
      get
      {
        return this.ScannerType != GlobalDictionaries.Devices.ScannerTypes.ComPort ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SettingsVisibility
    {
      get
      {
        return this.ScannerType != GlobalDictionaries.Devices.ScannerTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility RussianScannerSettingsVisibility
    {
      get
      {
        return this.SettingsConfig.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public GlobalDictionaries.Devices.ScannerTypes ScannerType
    {
      get => this.DevicesConfig.BarcodeScanner.Type;
      set
      {
        this.DevicesConfig.BarcodeScanner.Type = value;
        this.OnPropertyChanged(nameof (ScannerType));
        this.OnPropertyChanged("ConnectionsConfigVisible");
        this.OnPropertyChanged("SettingsVisibility");
      }
    }

    public Gbs.Core.Config.Devices DevicesConfig
    {
      get => this._devicesConfig;
      set
      {
        this._devicesConfig = value;
        this.OnPropertyChanged(nameof (DevicesConfig));
      }
    }

    public Gbs.Core.Config.Settings SettingsConfig
    {
      get => this._settingsConfig;
      set
      {
        this._settingsConfig = value;
        this.OnPropertyChanged(nameof (SettingsConfig));
        this.OnPropertyChanged("RussianScannerSettingsVisibility");
      }
    }

    public PageBarcodeScannerViewModel()
    {
    }

    public PageBarcodeScannerViewModel(Gbs.Core.Config.Devices devicesConfig, Gbs.Core.Config.Settings settings)
    {
      this._devicesConfig = devicesConfig;
      this._settingsConfig = settings;
    }

    public bool ValidationConfig()
    {
      if (this._devicesConfig.BarcodeScanner.Type == GlobalDictionaries.Devices.ScannerTypes.None)
        return true;
      List<string> source = new List<string>();
      source.AddRange((IEnumerable<string>) this._devicesConfig.BarcodeScanner.Prefixes.WeightGoods.Split(GlobalDictionaries.SplitArr, StringSplitOptions.RemoveEmptyEntries));
      source.AddRange((IEnumerable<string>) this._devicesConfig.BarcodeScanner.Prefixes.Users.Split(GlobalDictionaries.SplitArr, StringSplitOptions.RemoveEmptyEntries));
      source.AddRange((IEnumerable<string>) this._devicesConfig.BarcodeScanner.Prefixes.Certificates.Split(GlobalDictionaries.SplitArr, StringSplitOptions.RemoveEmptyEntries));
      source.AddRange((IEnumerable<string>) this._devicesConfig.BarcodeScanner.Prefixes.DiscountCard.Split(GlobalDictionaries.SplitArr, StringSplitOptions.RemoveEmptyEntries));
      source.AddRange((IEnumerable<string>) this._devicesConfig.BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr, StringSplitOptions.RemoveEmptyEntries));
      if (source.GroupBy<string, string>((Func<string, string>) (x => x)).Count<IGrouping<string, string>>((Func<IGrouping<string, string>, bool>) (x => x.Count<string>() > 1)) <= 0)
        return true;
      int num = (int) MessageBoxHelper.Show(Translate.PageBarcodeScannerViewModel_ValidationConfig_Префиксы_в_разделе_Оборудование___Сканер_ШК_НЕ_должны_совпадать_);
      return false;
    }

    public HotKeysHelper.Hotkey GsHotKey
    {
      get => this.DevicesConfig.BarcodeScanner.GsCodeHotKey;
      set
      {
        this.DevicesConfig.BarcodeScanner.GsCodeHotKey = value;
        this.OnPropertyChanged(nameof (GsHotKey));
      }
    }

    public ICommand TestFfd12Command
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new TestScannerForFfd12ViewModel().Test(this.DevicesConfig)));
      }
    }
  }
}
