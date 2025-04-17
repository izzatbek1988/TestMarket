// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.ConnectionSettingsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public class ConnectionSettingsViewModel : ViewModelWithForm
  {
    private ComPort _comPort;
    private LanConnection _connection;
    private GlobalDictionaries.Devices.ConnectionTypes _selectedConnectionType;

    public static List<int> PortSpeedList
    {
      get
      {
        return new List<int>()
        {
          110,
          300,
          600,
          1200,
          2400,
          4800,
          9600,
          14400,
          19200,
          38400,
          56000,
          57600,
          115200,
          120000,
          256000
        };
      }
    }

    public static List<int> BitDataList
    {
      get => new List<int>() { 4, 5, 6, 7, 8 };
    }

    public static Dictionary<StopBits, Decimal> StopBitList
    {
      get
      {
        return new Dictionary<StopBits, Decimal>()
        {
          {
            StopBits.One,
            1M
          },
          {
            StopBits.OnePointFive,
            1.5M
          },
          {
            StopBits.Two,
            2M
          }
        };
      }
    }

    public static Dictionary<Handshake, string> HandshakesList
    {
      get
      {
        return new Dictionary<Handshake, string>()
        {
          {
            Handshake.None,
            Translate.ConnectionSettingsViewModel_Нет
          },
          {
            Handshake.XOnXOff,
            "Xon / Xoff"
          },
          {
            Handshake.RequestToSend,
            Translate.ConnectionSettingsViewModel_Аппаратное
          }
        };
      }
    }

    public static Dictionary<Parity, string> ParityList
    {
      get
      {
        return new Dictionary<Parity, string>()
        {
          {
            Parity.Even,
            Translate.ConnectionSettingsViewModel_Чет
          },
          {
            Parity.Mark,
            Translate.ConnectionSettingsViewModel_Нечет
          },
          {
            Parity.None,
            Translate.CategoriesGroupEditViewModel_Нет
          },
          {
            Parity.Odd,
            Translate.ConnectionSettingsViewModel_Маркер
          },
          {
            Parity.Space,
            Translate.ConnectionSettingsViewModel_Пробел
          }
        };
      }
    }

    public Dictionary<GlobalDictionaries.Devices.ConnectionTypes, string> ConnectionTypesDictionary
    {
      get => GlobalDictionaries.Devices.ConnectionTypesDictionary();
    }

    public Visibility ComPortVisibility { get; set; }

    public Visibility LanConnectionVisibility { get; set; }

    public Visibility ConnectionTypeSelectorVisibility { get; set; }

    public Visibility AuthVisibility { get; set; }

    public GlobalDictionaries.Devices.ConnectionTypes SelectedConnectionType
    {
      get => this._selectedConnectionType;
      set
      {
        Other.ConsoleWrite("con type set: " + value.ToString());
        this._selectedConnectionType = value;
        this.LanConnectionVisibility = Visibility.Collapsed;
        this.ComPortVisibility = Visibility.Collapsed;
        switch (this._selectedConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            this.ComPortVisibility = Visibility.Visible;
            break;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            this.LanConnectionVisibility = Visibility.Visible;
            break;
        }
        this.OnPropertyChanged("LanConnectionVisibility");
        this.OnPropertyChanged("ComPortVisibility");
        this.OnPropertyChanged(nameof (SelectedConnectionType));
      }
    }

    public LanConnection Connection
    {
      get => this._connection;
      set
      {
        this._connection = value;
        this.OnPropertyChanged(nameof (Connection));
      }
    }

    public ComPort ComPort
    {
      get => this._comPort;
      set
      {
        this._comPort = value;
        this.OnPropertyChanged(nameof (ComPort));
      }
    }

    public List<string> PortsInSystem { get; set; }

    public ICommand SaveSettingsCommand { get; set; }

    public ConnectionSettingsViewModel()
    {
    }

    public ConnectionSettingsViewModel(
      ConnectionSettingsViewModel.ConnectionConfig config)
    {
      this.SaveSettingsCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
      this.AuthVisibility = config.NeedAuth ? Visibility.Visible : Visibility.Collapsed;
      Other.ConsoleWrite("conncetion type: " + config.Type.ToString());
      this.ComPortVisibility = Visibility.Collapsed;
      this.LanConnectionVisibility = Visibility.Collapsed;
      this.ConnectionTypeSelectorVisibility = Visibility.Collapsed;
      switch (config.ConnectionVariant)
      {
        case ConnectionSettingsViewModel.PortsConfig.OnlyCom:
          this.ComPortVisibility = Visibility.Visible;
          break;
        case ConnectionSettingsViewModel.PortsConfig.OnlyLan:
          this.LanConnectionVisibility = Visibility.Visible;
          break;
        case ConnectionSettingsViewModel.PortsConfig.ComOrLan:
          this.ConnectionTypeSelectorVisibility = Visibility.Visible;
          break;
        case ConnectionSettingsViewModel.PortsConfig.ComAndLan:
          this.LanConnectionVisibility = Visibility.Visible;
          this.ComPortVisibility = Visibility.Visible;
          break;
      }
      if (config.ConnectionVariant != ConnectionSettingsViewModel.PortsConfig.ComAndLan)
      {
        this.SelectedConnectionType = config.Type;
        this.OnPropertyChanged(nameof (SelectedConnectionType));
      }
      if (config.ConnectionVariant.IsEither<ConnectionSettingsViewModel.PortsConfig>(ConnectionSettingsViewModel.PortsConfig.ComAndLan, ConnectionSettingsViewModel.PortsConfig.ComOrLan, ConnectionSettingsViewModel.PortsConfig.OnlyCom))
      {
        this.PortsInSystem = ((IEnumerable<string>) SerialPort.GetPortNames()).ToList<string>();
        this.ComPort = config.Com;
      }
      if (config.ConnectionVariant.IsEither<ConnectionSettingsViewModel.PortsConfig>(ConnectionSettingsViewModel.PortsConfig.OnlyLan, ConnectionSettingsViewModel.PortsConfig.ComOrLan, ConnectionSettingsViewModel.PortsConfig.ComAndLan))
        this.Connection = config.Lan;
      this.OnPropertyChanged(nameof (LanConnectionVisibility));
      this.OnPropertyChanged(nameof (ComPortVisibility));
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public enum PortsConfig
    {
      OnlyCom,
      OnlyLan,
      ComOrLan,
      ComAndLan,
    }

    public class ConnectionConfig
    {
      public LanConnection Lan { get; set; }

      public ComPort Com { get; set; }

      public ConnectionSettingsViewModel.PortsConfig ConnectionVariant { get; set; }

      public GlobalDictionaries.Devices.ConnectionTypes Type { get; set; }

      public bool NeedAuth { get; set; }

      public ConnectionConfig(
        LanConnection lan,
        ComPort com,
        ConnectionSettingsViewModel.PortsConfig connectionVariant)
      {
        this.Lan = lan;
        this.Com = com;
        this.ConnectionVariant = connectionVariant;
        GlobalDictionaries.Devices.ConnectionTypes connectionTypes;
        switch (connectionVariant)
        {
          case ConnectionSettingsViewModel.PortsConfig.OnlyCom:
            connectionTypes = GlobalDictionaries.Devices.ConnectionTypes.ComPort;
            break;
          case ConnectionSettingsViewModel.PortsConfig.OnlyLan:
            connectionTypes = GlobalDictionaries.Devices.ConnectionTypes.Lan;
            break;
          default:
            connectionTypes = this.Type;
            break;
        }
        this.Type = connectionTypes;
      }

      public ConnectionConfig(
        DeviceConnection connection,
        ConnectionSettingsViewModel.PortsConfig connectionVariant)
      {
        this.Lan = connection.LanPort;
        this.Com = connection.ComPort;
        this.ConnectionVariant = connectionVariant;
        this.Type = connection.ConnectionType;
      }
    }
  }
}
