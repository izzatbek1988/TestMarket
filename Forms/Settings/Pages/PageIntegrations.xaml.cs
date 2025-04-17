// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.IntegrationViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class IntegrationViewModel : ViewModel
  {
    private Integrations _integrations = new Integrations();
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public Visibility VisibilityDaData
    {
      get
      {
        Gbs.Core.Config.Settings settings = this._settings;
        bool? nullable;
        if (settings == null)
        {
          nullable = new bool?();
        }
        else
        {
          Interface @interface = settings.Interface;
          if (@interface == null)
            nullable = new bool?();
          else
            nullable = new bool?(@interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Belarus, GlobalDictionaries.Countries.Kazakhstan));
        }
        return !nullable.GetValueOrDefault(true) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilitySms
    {
      get
      {
        Gbs.Core.Config.Settings settings = this._settings;
        bool? nullable;
        if (settings == null)
        {
          nullable = new bool?();
        }
        else
        {
          Interface @interface = settings.Interface;
          if (@interface == null)
            nullable = new bool?();
          else
            nullable = new bool?(@interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Kazakhstan));
        }
        return !nullable.GetValueOrDefault(true) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityRuSystem
    {
      get
      {
        return this._settings.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityPolycard
    {
      get
      {
        return this.Settings.Interface.Country != GlobalDictionaries.Countries.Ukraine ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
        this.OnPropertyChanged("VisibilityPolycard");
      }
    }

    public Visibility VisibilitySettingSms
    {
      get
      {
        return this.SmsServiceType != GlobalDictionaries.SmsServiceType.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public GlobalDictionaries.SmsServiceType SmsServiceType
    {
      get => this.Integrations.Sms.Type;
      set
      {
        this.Integrations.Sms.Type = value;
        this.OnPropertyChanged("VisibilitySettingSms");
        this.OnPropertyChanged("SmsApiVisibility");
        this.OnPropertyChanged("SmsLoginVisibility");
        this.OnPropertyChanged("SmsPasswordVisibility");
        this.OnPropertyChanged("SmsSenderVisibility");
      }
    }

    public ICommand SetSettingPlanfix
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ConfigManager.Config = new Planfix.Api.Config(this.Integrations.Planfix.AccountName, this.Integrations.Planfix.ApiUrl, this.Integrations.Planfix.DecryptedKeyApi, this.Integrations.Planfix.DecryptedToken);
          new PlanfixSettingViewModel().ShowSetting(this.Integrations);
        }));
      }
    }

    public Integrations Integrations
    {
      get => this._integrations;
      set
      {
        this._integrations = value;
        this.OnPropertyChanged(nameof (Integrations));
      }
    }

    public List<string> ApiUrls { get; set; } = new List<string>()
    {
      "https://apiru.planfix.ru/xml/",
      "https://api.planfix.com/xml/"
    };

    public List<Gbs.Core.Entities.Clients.Group> ClientGroups { get; set; } = new List<Gbs.Core.Entities.Clients.Group>();

    public IntegrationViewModel()
    {
    }

    public IntegrationViewModel(Integrations integrations, Gbs.Core.Config.Settings settings)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.Integrations = integrations;
        this.Settings = settings;
        this.ClientGroups = new List<Gbs.Core.Entities.Clients.Group>((IEnumerable<Gbs.Core.Entities.Clients.Group>) new GroupRepository(dataBase).GetActiveItems());
        if (this.Settings.Interface.Country != GlobalDictionaries.Countries.Ukraine)
          return;
        this.ApiUrls = this.ApiUrls.Skip<string>(1).ToList<string>();
      }
    }

    public bool Save()
    {
      if (!this._integrations.Planfix.ValidateSettingPlanfix())
      {
        int num = (int) MessageBoxHelper.Show(Translate.IntegrationViewModel_Для_работы_программы_с_ПланФиксом__требуется_указать_связи_для_всех_сущностей);
        return false;
      }
      ConfigsRepository<Integrations> configsRepository = new ConfigsRepository<Integrations>();
      return this.ValidateSmsSetting() && configsRepository.Save(this._integrations);
    }

    private bool ValidateSmsSetting()
    {
      if (this._integrations.Sms.Type == GlobalDictionaries.SmsServiceType.None || !this._integrations.Sms.TextSmsForCode.IsNullOrEmpty() && this._integrations.Sms.TextSmsForCode.Contains("{code}"))
        return true;
      MessageBoxHelper.Warning(Translate.IntegrationViewModel_Save_Необходимо_указать_текст_сообщения__которое_будет_отправлено_для_подтверждения_операций__В_сообщении_обязательно_должно_быть_указать_спец__фраза_в_формате__code__);
      return false;
    }

    public ICommand SmsBalanceCommand
    {
      get
      {
        int num;
        return (ICommand) new RelayCommand((Action<object>) (obj => num = (int) MessageBoxHelper.Show(string.Format(Translate.IntegrationViewModel_SmsBalanceCommand_Баланс_вашего_счета___0_N2_, (object) new SmsHelper().GetBalance()))));
      }
    }

    public ICommand SendTestSmsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.ValidateSmsSetting())
            return;
          (bool result, string output) tuple = MessageBoxHelper.Input("", Translate.IntegrationViewModel_SendTestSmsCommand_Укажите_номер_телефона_для_отправки_тестового_СМС_в_формате_79005550000_, 5);
          if (!tuple.result)
            return;
          string str1 = tuple.output.ClearPhone();
          string str2 = this.Integrations.Sms.TextSmsForCode.Replace("{code}", BarcodeHelper.RandomPass());
          SmsHelper smsHelper = new SmsHelper();
          string text = str2;
          List<string> phones = new List<string>();
          phones.Add(str1);
          List<string> values;
          ref List<string> local = ref values;
          if (smsHelper.SendSms(text, phones, out local) == 1)
          {
            int num = (int) MessageBoxHelper.Show(string.Format(Translate.IntegrationViewModel_SendTestSmsCommand_Сообщение_успешно_отправлено_на_номер__0_, (object) str1));
          }
          else
            MessageBoxHelper.Error(string.Format(Translate.IntegrationViewModel_SendTestSmsCommand_, (object) string.Join("\n", (IEnumerable<string>) values)));
        }));
      }
    }

    public List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>> SmsServiceTypeDictionary
    {
      get
      {
        return GlobalDictionaries.SmsServiceTypeDictionary().Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>, bool>) (x =>
        {
          List<GlobalDictionaries.Countries> country1 = x.Country;
          Gbs.Core.Config.Settings settings = this.Settings;
          int country2 = settings != null ? (int) settings.Interface.Country : 0;
          return country1.Contains((GlobalDictionaries.Countries) country2) || x.Country.Contains(GlobalDictionaries.Countries.NotSet);
        })).ToList<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>>();
      }
    }

    public Visibility SmsApiVisibility
    {
      get
      {
        return !this.SmsServiceType.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsRu, GlobalDictionaries.SmsServiceType.SmsAero) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SmsLoginVisibility
    {
      get
      {
        return !this.SmsServiceType.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAgent, GlobalDictionaries.SmsServiceType.SmsCenter, GlobalDictionaries.SmsServiceType.SmsAero) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SmsPasswordVisibility
    {
      get
      {
        return !this.SmsServiceType.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAgent, GlobalDictionaries.SmsServiceType.SmsCenter) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SmsSenderVisibility
    {
      get
      {
        return !this.SmsServiceType.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAgent, GlobalDictionaries.SmsServiceType.SmsCenter, GlobalDictionaries.SmsServiceType.SmsAero) ? Visibility.Collapsed : Visibility.Visible;
      }
    }
  }
}
