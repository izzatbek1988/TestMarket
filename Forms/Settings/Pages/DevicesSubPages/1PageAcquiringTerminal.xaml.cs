// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.AcquiringTerminalPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Core.Devices.Sbp;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public class AcquiringTerminalPageViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public Dictionary<GlobalDictionaries.Encoding, string> EncodingDirectory
    {
      get
      {
        return GlobalDictionaries.EncodingDictionary.SkipWhile<KeyValuePair<GlobalDictionaries.Encoding, string>>((Func<KeyValuePair<GlobalDictionaries.Encoding, string>, bool>) (x => x.Key == GlobalDictionaries.Encoding.CPTysso)).ToDictionary<KeyValuePair<GlobalDictionaries.Encoding, string>, GlobalDictionaries.Encoding, string>((Func<KeyValuePair<GlobalDictionaries.Encoding, string>, GlobalDictionaries.Encoding>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.Encoding, string>, string>) (x => x.Value));
      }
    }

    public Visibility VisibilityButtonShowDriver
    {
      get
      {
        return !this.DeviceConfig.AcquiringTerminal.Type.IsEither<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.Acrus2, GlobalDictionaries.Devices.AcquiringTerminalTypes.Sberbank) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityMerchantId
    {
      get
      {
        return !this.DeviceConfig.AcquiringTerminal.Type.IsEither<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.BPOS) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand ShowFolderDriver
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          switch (this.DeviceConfig.AcquiringTerminal.Type)
          {
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.None:
              break;
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.KkmServer:
              break;
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.Sberbank:
              string str1 = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\acquiring\\sberbank");
              FileSystemHelper.ShowFolderDriver(str1);
              if (!Directory.Exists(str1) || ((IEnumerable<string>) Directory.GetFiles(str1)).Any<string>())
                break;
              string str2 = "";
              foreach (string directory in Directory.GetDirectories("C:/"))
              {
                try
                {
                  if (((IEnumerable<string>) Directory.GetFiles(directory)).Any<string>((Func<string, bool>) (file => file.ToLower().Contains("pilot_nt.dll"))))
                  {
                    str2 = directory;
                    break;
                  }
                }
                catch (Exception ex)
                {
                  LogHelper.Error(ex, "Не удалось прочитать данные из папки", false, false);
                }
              }
              if (str2.IsNullOrEmpty())
                break;
              FileSystemHelper.CopyFolder(str2, str1);
              FileSystemHelper.GrantAccess(str1);
              break;
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.Inpas:
              break;
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.PrivatBank:
              break;
            case GlobalDictionaries.Devices.AcquiringTerminalTypes.Acrus2:
              FileSystemHelper.ShowFolderDriver("C:\\Arcus2");
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }));
      }
    }

    public Gbs.Core.Config.Devices DeviceConfig { get; set; } = new Gbs.Core.Config.Devices();

    public GlobalDictionaries.Devices.AcquiringTerminalTypes TerminalType
    {
      get => this.DeviceConfig.AcquiringTerminal.Type;
      set
      {
        this.DeviceConfig.AcquiringTerminal.Type = value;
        this.OnPropertyChanged(nameof (TerminalType));
        this.OnPropertyChanged("SettingsVisibility");
        this.OnPropertyChanged("VisibilityButtonShowDriver");
        this.OnPropertyChanged("VisibilityMerchantId");
      }
    }

    public Visibility SettingsVisibility
    {
      get
      {
        return this.TerminalType != GlobalDictionaries.Devices.AcquiringTerminalTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand ConfigConnectionCommand { get; set; }

    public ICommand TestConnectionSBPCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.AcquiringTerminalPageViewModel_TestConnectionSBPCommand_Тестовый_платеж_по_СБП);
          using (SpbHelper spbHelper = new SpbHelper(1M, long.MaxValue))
          {
            spbHelper.Pay(1M, out string _);
            progressBar.Close();
          }
        }));
      }
    }

    public ICommand TestConnectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.AcquiringTerminalPageViewModel_TestConnectionCommand_Тестовый_платеж_по_терминалу);
          using (AcquiringHelper acquiringHelper = new AcquiringHelper())
            acquiringHelper.DoPayment(1M, out string _, out string _, out string _, out string _, out string _, out string _, out string _);
          progressBar.Close();
        }));
      }
    }

    public Dictionary<GlobalDictionaries.Devices.AcquiringTerminalTypes, string> TerminalTypes
    {
      get
      {
        return GlobalDictionaries.Devices.AcquiringTerminalTypesDictionary().Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>, bool>) (x => x.Country.Any<GlobalDictionaries.Countries>((Func<GlobalDictionaries.Countries, bool>) (c => c.IsEither<GlobalDictionaries.Countries>(this.Settings.Interface.Country, GlobalDictionaries.Countries.NotSet))))).ToDictionary<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>, GlobalDictionaries.Devices.AcquiringTerminalTypes, string>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>, GlobalDictionaries.Devices.AcquiringTerminalTypes>) (x => x.Type), (Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>, string>) (x => x.TypeName));
      }
    }

    public AcquiringTerminalPageViewModel()
    {
    }

    public Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public AcquiringTerminalPageViewModel(Gbs.Core.Config.Devices devicesConfig, Gbs.Core.Config.Settings settings)
    {
      this.DeviceConfig = devicesConfig;
      this.Settings = settings;
      this.ConfigConnectionCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        using (AcquiringHelper acquiringHelper = new AcquiringHelper(devicesConfig))
          acquiringHelper.ShowProperties();
      }));
      this.LoadingAccountNewPay(true);
    }

    public ICommand SelectSlipFilePathCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog();
          if (!openFileDialog.ShowDialog().GetValueOrDefault())
            return;
          this.DeviceConfig.AcquiringTerminal.SlipFilePath = openFileDialog.FileName;
          this.OnPropertyChanged("DeviceConfig");
        }));
      }
    }

    public Visibility OpenBankVisibility
    {
      get
      {
        return this.SbpType != GlobalDictionaries.Devices.SbpTypes.OpenBank ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility PayQrDeviceIdVisibility
    {
      get
      {
        return !this.SbpType.IsEither<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.OpenBank, GlobalDictionaries.Devices.SbpTypes.AtolPay) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility NewPayVisibility
    {
      get
      {
        return this.SbpType != GlobalDictionaries.Devices.SbpTypes.NewPay ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SbpSettingVisibility
    {
      get
      {
        return this.SbpType == GlobalDictionaries.Devices.SbpTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility PayMasterSettingVisibility
    {
      get
      {
        return this.SbpType != GlobalDictionaries.Devices.SbpTypes.PayMaster ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public GlobalDictionaries.Devices.SbpTypes SbpType
    {
      get => this.DeviceConfig.SBP.Type;
      set
      {
        this.DeviceConfig.SBP.Type = value;
        this.OnPropertyChanged("OpenBankVisibility");
        this.OnPropertyChanged("SbpSettingVisibility");
        this.OnPropertyChanged("PayMasterSettingVisibility");
        this.OnPropertyChanged("NewPayVisibility");
        this.OnPropertyChanged("PayQrDeviceIdVisibility");
        this.LoadingAccountNewPay(true);
      }
    }

    public List<NewPayDriver.GetAccountsCommand.Account> ListNewPayKassa { get; set; }

    public string SelectedIdKassa
    {
      get => this.DeviceConfig.SBP.PayQrDeviceID;
      set
      {
        this.DeviceConfig.SBP.PayQrDeviceID = value;
        this.OnPropertyChanged(nameof (SelectedIdKassa));
      }
    }

    public ICommand ReloadListAccount
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.LoadingAccountNewPay(false)));
      }
    }

    private void LoadingAccountNewPay(bool isFirstLoading)
    {
      string oldSelectedAcc = this.SelectedIdKassa;
      this.ListNewPayKassa = new List<NewPayDriver.GetAccountsCommand.Account>()
      {
        new NewPayDriver.GetAccountsCommand.Account()
        {
          Active = 1,
          KeyKassa = "",
          Name = "не указан"
        }
      };
      this.SelectedIdKassa = this.ListNewPayKassa.Single<NewPayDriver.GetAccountsCommand.Account>().KeyKassa;
      if (this.SbpType != GlobalDictionaries.Devices.SbpTypes.NewPay)
      {
        this.OnPropertyChanged("ListNewPayKassa");
        this.OnPropertyChanged("SelectedIdKassa");
      }
      else if (isFirstLoading && this.DeviceConfig.SBP.ClientSecret.DecryptedValue.IsNullOrEmpty())
      {
        this.OnPropertyChanged("ListNewPayKassa");
        this.OnPropertyChanged("SelectedIdKassa");
      }
      else if (this.DeviceConfig.SBP.ClientSecret.DecryptedValue.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning("Необходимо указать токен точки (секретный ключ) для работы с Яндекс Пэй.");
        this.OnPropertyChanged("ListNewPayKassa");
        this.OnPropertyChanged("SelectedIdKassa");
      }
      else
      {
        try
        {
          this.ListNewPayKassa.AddRange(new NewPay(0M, "").GetListAccount().Where<NewPayDriver.GetAccountsCommand.Account>((Func<NewPayDriver.GetAccountsCommand.Account, bool>) (x => x.Active == 1)));
        }
        catch (Exception ex)
        {
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Не удалось получить список касс зарегистрированных в Яндекс Пэй, проверьте корректность введенных настроек.\n\n" + ex.Message));
        }
        this.OnPropertyChanged("ListNewPayKassa");
        if (!this.ListNewPayKassa.Any<NewPayDriver.GetAccountsCommand.Account>((Func<NewPayDriver.GetAccountsCommand.Account, bool>) (x => x.KeyKassa == oldSelectedAcc)))
          this.SelectedIdKassa = "";
        else
          this.SelectedIdKassa = oldSelectedAcc;
      }
    }

    public List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>> DictionarySbpTypes
    {
      get
      {
        return GlobalDictionaries.Devices.SbpTypesDictionary().Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>, bool>) (x => x.Type.IsEither<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.None, GlobalDictionaries.Devices.SbpTypes.NewPay, GlobalDictionaries.Devices.SbpTypes.AtolPay, GlobalDictionaries.Devices.SbpTypes.PayMaster))).ToList<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>>();
      }
    }

    public Visibility VisibilitySBP
    {
      get
      {
        return this._settings.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }
  }
}
