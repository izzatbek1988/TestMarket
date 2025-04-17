// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.RemoteControlViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Helpers.WebOffice;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class RemoteControlViewModel : ViewModel
  {
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public ICommand OpenRemoteFolder
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Settings.RemoteControl.Cloud.Path.IsNullOrEmpty())
            MessageBoxHelper.Warning(Translate.RemoteControlViewModel_OpenRemoteFolder_Невозможно_перейти_к_папке_обмена__так_как_не_указан_путь_к_ней_);
          else if (!Directory.Exists(this.Settings.RemoteControl.Cloud.Path))
            MessageBoxHelper.Warning(Translate.RemoteControlViewModel_OpenRemoteFolder_Указанная_папка_обмена_не_существует__проверьте_путь_к_папке_);
          else
            FileSystemHelper.OpenFolder(this.Settings.RemoteControl.Cloud.Path);
        }));
      }
    }

    public ICommand CreateArchiveHomeOffice
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => HomeOfficeHelper.CreateArchive(true)));
      }
    }

    public ICommand CreateArchiveCatalog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => ExchangeDataHelper.DoExchangeCatalogForAllPoint(true)));
      }
    }

    public ICommand CreateArchiveWebOffice
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this._settings.RemoteControl.WebOffice.Token.IsNullOrEmpty())
            MessageBoxHelper.Warning(Translate.RemoteControlViewModel_CreateArchiveWebOffice_Для_выгрузки_файла_в_Веб_офис_нужно_сначала_указать_токен__Укажите_токен_и_повторите_попытку_выгрузить_файл_);
          else
            WebOfficeHelper.CreateArchive(true);
        }));
      }
    }

    public ICommand CreateArchiveClients
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => ClientsExchangeHelper.UploadFileClients(true)));
      }
    }

    public ICommand SetConnectionPersonalEmail
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ConnectionSettingsViewModel.ConnectionConfig config = new ConnectionSettingsViewModel.ConnectionConfig(this.Settings.RemoteControl.Email.ConnectionPersonalEmail, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
          {
            NeedAuth = true,
            Type = GlobalDictionaries.Devices.ConnectionTypes.Lan
          };
          new FrmConnectionSettings().ShowConfig(config);
          this.Settings.RemoteControl.Email.ConnectionPersonalEmail = config.Lan;
        }));
      }
    }

    public bool IsNotAuthRequireDb { get; set; }

    public PageRemoteControl Page { get; set; }

    public Visibility IsBlockForHome
    {
      get
      {
        return SettingsViewModel.DataBaseConfig.ModeProgram != GlobalDictionaries.Mode.Home ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility WebOfficeVisibility
    {
      get
      {
        return this.Settings.Interface.Country != GlobalDictionaries.Countries.Ukraine && this.IsBlockForHome == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
      }
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

    public ICommand ClearTableEmail
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (MessageBoxHelper.Show(Translate.RemoteControlViewModel_Вы_уверены__что_хотите_удалить_все_письма__которые_стоят_в_очереди_на_отправку_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              dataBase.GetTable<EMAIL>().Delete<EMAIL>();
            int num = (int) MessageBoxHelper.Show(Translate.RemoteControlViewModel_Очередь_писем_успешно_очищена_);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "Ошбика при очищении очереди печати");
          }
        }));
      }
    }

    public ICommand SelectRemotePathCommand
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
          this.Settings.RemoteControl.Cloud.Path = folderBrowserDialog.SelectedPath;
          this.OnPropertyChanged("Settings");
        }));
      }
    }

    public ObservableCollection<MultiValueControl.Value> EmailScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ObservableCollection<MultiValueControl.Value> CloudScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ObservableCollection<MultiValueControl.Value> WebOfficeScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ObservableCollection<MultiValueControl.Value> EmailValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ICommand AddEmailValue
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          (bool, string) valueTuple = MessageBoxHelper.Input("", Translate.RemoteControlViewModel_Введите_адрес_E_mail__на_который_требуется_отправлять_отчеты_);
          if (!valueTuple.Item1)
            return;
          while (!Other.IsValidateEmail(valueTuple.Item2))
          {
            MessageBoxHelper.Warning(Translate.АдресЭлектроннойПочтыНекорректен);
            valueTuple = MessageBoxHelper.Input(valueTuple.Item2, Translate.RemoteControlViewModel_Введите_адрес_E_mail__на_который_требуется_отправлять_отчеты_);
            if (!valueTuple.Item1)
              return;
          }
          this.EmailValues.Add(new MultiValueControl.Value()
          {
            DisplayedValue = valueTuple.Item2
          });
        }));
      }
    }

    public ICommand SendTestMail
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.ValidationEmail())
            return;
          string str = string.Empty;
          if (!this.EmailValues.Any<MultiValueControl.Value>())
          {
            MessageBoxHelper.Warning(Translate.RemoteControlViewModel_Не_указан_E_mail_для_отправки_тестового_письма_);
          }
          else
          {
            foreach (MultiValueControl.Value emailValue in (Collection<MultiValueControl.Value>) this.EmailValues)
            {
              if (SmtpHelper.Send(new Gbs.Core.Entities.Emails.Email()
              {
                AddressTo = emailValue.DisplayedValue,
                Body = (object) Translate.RemoteControlViewModel_SendTestMail_Если_Вы_читаете_данный_текст__значит_доставка_почты_работает_корректно_,
                Subject = string.Format(Translate.RemoteControlViewModel_SendTestMail_Тестовое_письмо_из__0_, (object) PartnersHelper.ProgramName())
              }))
                str = str + string.Format(Translate.RemoteControlViewModel_Тестовое_письмо_на_адрес__0__было_отправлено_, (object) emailValue.DisplayedValue) + Other.NewLine();
              else
                MessageBoxHelper.Error(string.Format(Translate.RemoteControlViewModel_При_отправке_письма_на_адрес__0__произошла_ошибка__обратитесь_в_службу_тех__поддержки, (object) emailValue.DisplayedValue));
            }
            if (str.IsNullOrEmpty())
              return;
            int num = (int) MessageBoxHelper.Show(str + Other.NewLine() + Translate.RemoteControlViewModel_Если_в_течении_15_минут_какое_либо_из_писем_не_будет_доставлено__обратитесь_в_службу_поддержки_);
          }
        }));
      }
    }

    public RemoteControlViewModel()
    {
    }

    public RemoteControlViewModel(Gbs.Core.Config.Settings settings)
    {
      this.Settings = settings;
      this.IsNotAuthRequireDb = !AuthRequireDb.Get();
      string timesSend1 = settings.RemoteControl.Email.TimesSend;
      char[] separator1 = new char[3]{ ' ', ';', ',' };
      foreach (string str in timesSend1.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
        this.EmailScheduleValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
      string timesSend2 = settings.RemoteControl.Telegram.TimesSend;
      char[] separator2 = new char[3]{ ' ', ';', ',' };
      foreach (string str in timesSend2.Split(separator2, StringSplitOptions.RemoveEmptyEntries))
        this.TelegramScheduleValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
      string emailTo = settings.RemoteControl.Email.EmailTo;
      char[] separator3 = new char[3]{ ' ', ';', ',' };
      foreach (string str in emailTo.Split(separator3, StringSplitOptions.RemoveEmptyEntries))
        this.EmailValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
      string usernameTo = settings.RemoteControl.Telegram.UsernameTo;
      char[] separator4 = new char[3]{ ' ', ';', ',' };
      foreach (string str in usernameTo.Split(separator4, StringSplitOptions.RemoveEmptyEntries))
        this.UsernameValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
      string timesSend3 = settings.RemoteControl.Cloud.TimesSend;
      char[] separator5 = new char[3]{ ' ', ';', ',' };
      foreach (string str in timesSend3.Split(separator5, StringSplitOptions.RemoveEmptyEntries))
        this.CloudScheduleValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
    }

    public bool Save()
    {
      AuthRequireDb.Set(!this.IsNotAuthRequireDb);
      this.Settings.RemoteControl.Email.TimesSend = string.Join("; ", this.EmailScheduleValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      this.Settings.RemoteControl.Email.EmailTo = string.Join("; ", this.EmailValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      this.Settings.RemoteControl.Cloud.TimesSend = string.Join("; ", this.CloudScheduleValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      this.Settings.RemoteControl.Telegram.TimesSend = string.Join("; ", this.TelegramScheduleValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      this.Settings.RemoteControl.Telegram.UsernameTo = string.Join("; ", this.UsernameValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      if ((this.Settings.RemoteControl.Telegram.IsActive && this.IsBlockForHome == Visibility.Visible || this.Settings.RemoteControl.Telegram.IsSendBackUp || this.Settings.RemoteControl.Telegram.IsSendNotificationLicense) && !this.UsernameValues.Any<MultiValueControl.Value>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.RemoteControlViewModel_Save_Не_указан_ID_чата_для_работы_выгрузки_в_Телеграмм__проверьте_настройки_, icon: MessageBoxImage.Exclamation);
        return false;
      }
      if (!this.Settings.RemoteControl.Email.IsActive || this.EmailValues.Any<MultiValueControl.Value>() || this.IsBlockForHome != Visibility.Visible)
        return this.ValidationEmail();
      int num1 = (int) MessageBoxHelper.Show(Translate.RemoteControlViewModel_Не_указан_адрес_электронной_почты_для_отправки_отчетов__проверьте_настройки_, icon: MessageBoxImage.Exclamation);
      return false;
    }

    private bool ValidationEmail()
    {
      List<string> list = ((IEnumerable<string>) this.Settings.RemoteControl.Email.EmailTo.Split(new char[3]
      {
        ' ',
        ';',
        ','
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (email => !Other.IsValidateEmail(email))).Select<string, string>((Func<string, string>) (email => string.Format(Translate.RemoteControlViewModel_Адрес__0____не_корректный, (object) email))).ToList<string>();
      if (!list.Any<string>())
        return true;
      int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) list));
      return false;
    }

    public ICommand SendTestMsgOnTelegram
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.ValidationTelegram())
            return;
          string str = string.Empty;
          if (!this.UsernameValues.Any<MultiValueControl.Value>())
          {
            MessageBoxHelper.Warning(Translate.RemoteControlViewModel_Не_указаны_пользователи_для_отправки_сообщений_);
          }
          else
          {
            foreach (MultiValueControl.Value usernameValue in (Collection<MultiValueControl.Value>) this.UsernameValues)
            {
              if (TelegramHelper.SendTextMessage(usernameValue.DisplayedValue, Translate.RemoteControlViewModel_Если_Вы_читаете_данный_текст__значит_доставка_сообщений_работает_корректно_))
                str = str + string.Format(Translate.RemoteControlViewModel_Тестовое_сообщение_пользователю__0__было_отправлено, (object) usernameValue.DisplayedValue) + Other.NewLine();
              else
                MessageBoxHelper.Error(string.Format(Translate.RemoteControlViewModel_При_отправке_сообщения_пользователю__0__произошла_ошибка__обратитесь_в_службу_тех__поддержки_, (object) usernameValue.DisplayedValue));
            }
            if (str.IsNullOrEmpty())
              return;
            int num = (int) MessageBoxHelper.Show(str + Other.NewLine() + Translate.RemoteControlViewModel_Если_в_течении_15_минут_какое_либо_из_сообщений_не_будет_доставлено__обратитесь_в_службу_поддержки_);
          }
        }));
      }
    }

    public ICommand AddUserNameValue
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.UsernameValues.Any<MultiValueControl.Value>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.RemoteControlViewModel_На_данный_момент_можно_указать_только_один_ChatId_для_доставки_сообщений__если_нужно_отправлять_нескольким_пользователям___создайте_группу_в_Telegram_и_настройте_выгрузку_туда_);
          }
          else
          {
            (bool, string) valueTuple = MessageBoxHelper.Input("", Translate.RemoteControlViewModel_Введите_идентификатор_пользователя__которому_требуется_отправлять_отчеты_);
            if (!valueTuple.Item1)
              return;
            while (!this.IsValidateUsername(valueTuple.Item2))
            {
              MessageBoxHelper.Warning(Translate.RemoteControlViewModel_Введенное_chatID__некорректен__Требуется_вводить_только_цифры_);
              valueTuple = MessageBoxHelper.Input(valueTuple.Item2, Translate.RemoteControlViewModel_Введите_идентификатор_пользователя__которому_требуется_отправлять_отчеты_);
              if (!valueTuple.Item1)
                return;
            }
            this.UsernameValues.Add(new MultiValueControl.Value()
            {
              DisplayedValue = valueTuple.Item2
            });
          }
        }));
      }
    }

    public ObservableCollection<MultiValueControl.Value> TelegramScheduleValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public ObservableCollection<MultiValueControl.Value> UsernameValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public Visibility TelegramTabVisibility
    {
      get => !DevelopersHelper.IsDebug() ? Visibility.Collapsed : Visibility.Visible;
    }

    private bool ValidationTelegram()
    {
      List<string> list = ((IEnumerable<string>) this.Settings.RemoteControl.Telegram.UsernameTo.Split(new char[3]
      {
        ' ',
        ';',
        ','
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (user => !this.IsValidateUsername(user))).Select<string, string>((Func<string, string>) (user => string.Format(Translate.RemoteControlViewModel_ValidationTelegram_Аккаунт__0____не_корректный, (object) user))).ToList<string>();
      if (!list.Any<string>())
        return true;
      int num = (int) MessageBoxHelper.Show(string.Join("\n", (IEnumerable<string>) list));
      return false;
    }

    private bool IsValidateUsername(string name)
    {
      if (name.Length < 3 || name.Contains<char>(' '))
        return false;
      int num = ((IEnumerable<char>) name.ToCharArray()).Count<char>((Func<char, bool>) (x => x == '-'));
      return num <= 1 && (num != 1 || name[0] == '-') && !((IEnumerable<char>) name.ToCharArray()).Any<char>((Func<char, bool>) (x => !char.IsDigit(x) && x != '-'));
    }

    public Dictionary<Gbs.Core.Config.RemoteControl.CreateItemPeriods, string> WebOfficePeriodsDictionary
    {
      get
      {
        return new Dictionary<Gbs.Core.Config.RemoteControl.CreateItemPeriods, string>()
        {
          {
            Gbs.Core.Config.RemoteControl.CreateItemPeriods.None,
            Translate.RemoteControlViewModel_WebOfficePeriodsDictionary_Не_выгружать_доп__архивы_с_данными
          },
          {
            Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery1Hour,
            Translate.DataBasePageViewModel___каждый_час
          },
          {
            Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery3Hours,
            Translate.DataBasePageViewModel___каждые_3_часа
          },
          {
            Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery6Hours,
            Translate.DataBasePageViewModel___каждые_6_часов
          },
          {
            Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery12Hours,
            Translate.DataBasePageViewModel___каждые_12_часов
          }
        };
      }
    }
  }
}
