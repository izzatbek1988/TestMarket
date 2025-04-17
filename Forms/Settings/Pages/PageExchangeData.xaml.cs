// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.ExchangeDataViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class ExchangeDataViewModel : ViewModel
  {
    private Gbs.Core.Config.Settings _setting;

    public ExchangeDataViewModel()
    {
    }

    public ExchangeDataViewModel(Gbs.Core.Config.Settings setting)
    {
      this.Setting = setting;
      string time1 = this.Setting.ExchangeData.CatalogExchange.Local.Time;
      char[] separator1 = new char[3]{ ' ', ';', ',' };
      foreach (string str in time1.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
        this.LocalScheduleValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
      string time2 = this.Setting.ExchangeData.CatalogExchange.Ftp.Time;
      char[] separator2 = new char[3]{ ' ', ';', ',' };
      foreach (string str in time2.Split(separator2, StringSplitOptions.RemoveEmptyEntries))
        this.FtpScheduleValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
    }

    public ObservableCollection<MultiValueControl.Value> FtpScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ObservableCollection<MultiValueControl.Value> LocalScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public Gbs.Core.Config.Settings Setting
    {
      get => this._setting;
      set
      {
        this._setting = value;
        this.OnPropertyChanged(nameof (Setting));
      }
    }

    public Dictionary<GlobalDictionaries.Format, string> Formats { get; set; } = GlobalDictionaries.FormatDictionary();

    public ICommand SelectCatalogPathCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
          {
            ShowNewFolderButton = true,
            Description = Translate.RemoteControlViewModel_Выберите_папку_для_обмена_данными
          };
          if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            return;
          this.Setting.ExchangeData.CatalogExchange.Local.Path = folderBrowserDialog.SelectedPath;
          this.OnPropertyChanged("Setting");
        }));
      }
    }

    public ICommand SettingFtpConnection
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          LanConnection lan = new LanConnection()
          {
            UrlAddress = this.Setting.ExchangeData.CatalogExchange.Ftp.Connection.UrlAddress,
            PortNumber = this.Setting.ExchangeData.CatalogExchange.Ftp.Connection.PortNumber,
            UserLogin = CryptoHelper.StringCrypter.Decrypt(this.Setting.ExchangeData.CatalogExchange.Ftp.Connection.UserLogin),
            Password = CryptoHelper.StringCrypter.Decrypt(this.Setting.ExchangeData.CatalogExchange.Ftp.Connection.Password)
          };
          ConnectionSettingsViewModel.ConnectionConfig config = new ConnectionSettingsViewModel.ConnectionConfig(lan, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
          {
            NeedAuth = true
          };
          new FrmConnectionSettings().ShowConfig(config);
          if (!config.Lan.PortNumber.HasValue || config.Lan.UrlAddress.IsNullOrEmpty() || config.Lan.UserLogin.IsNullOrEmpty())
          {
            int num = (int) MessageBoxHelper.Show(Translate.ExchangeDataViewModel_Вы_указали_не_все_данные_для_доступа_к_FTP_серверу);
            this.Setting.ExchangeData.CatalogExchange.Ftp.IsSend = false;
            this.OnPropertyChanged("Setting");
          }
          else
            this.Setting.ExchangeData.CatalogExchange.Ftp.Connection = new LanConnection()
            {
              UrlAddress = lan.UrlAddress,
              PortNumber = lan.PortNumber,
              UserLogin = CryptoHelper.StringCrypter.Encrypt(lan.UserLogin),
              Password = CryptoHelper.StringCrypter.Encrypt(lan.Password)
            };
        }));
      }
    }

    public ICommand CreateFileCatalogLocal
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          bool flag = ExchangeDataHelper.DoOnlyExchangeCatalogLocal(out string _, this.Setting);
          int num = (int) MessageBoxHelper.Show(flag ? Translate.ExchangeDataViewModel_Файл_выгружен_успешно_ : Translate.ExchangeDataViewModel_Ошибка_при_выгрузке_файла, icon: flag ? MessageBoxImage.Asterisk : MessageBoxImage.Exclamation);
        }));
      }
    }

    public ICommand CreateFileCatalogFtp
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          bool flag = ExchangeDataHelper.DoOnlyExchangeCatalogFtp(this.Setting);
          int num = (int) MessageBoxHelper.Show(flag ? Translate.ExchangeDataViewModel_Файл_выгружен_успешно_ : Translate.ExchangeDataViewModel_Ошибка_при_выгрузке_файла, icon: flag ? MessageBoxImage.Asterisk : MessageBoxImage.Exclamation);
        }));
      }
    }

    public bool Save()
    {
      this.Setting.ExchangeData.CatalogExchange.Local.Time = string.Join("; ", this.LocalScheduleValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      this.Setting.ExchangeData.CatalogExchange.Ftp.Time = string.Join("; ", this.FtpScheduleValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      if (!this.Setting.ExchangeData.CatalogExchange.Ftp.Path.Contains(":"))
        return true;
      MessageBoxHelper.Warning(Translate.ExchangeDataViewModel_Save_В_пути_к_папке_на_FTP_сервере_нельзя_указывать_символы______исправьте_и_сохраните_настройки_повторно_);
      return false;
    }
  }
}
