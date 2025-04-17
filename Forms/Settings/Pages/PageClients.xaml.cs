// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.ClientsPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.PropertiesEntities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class ClientsPageViewModel : ViewModelWithForm
  {
    public Visibility VisibilitySettingSms
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        return (settings != null ? (int) settings.Interface.Country : 1) != 1 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand CreateArchiveClients
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => ClientsExchangeHelper.UploadFileClients(true)));
      }
    }

    public Gbs.Core.Config.Settings Settings { get; set; }

    private Integrations _integrationsSettings { get; set; }

    public Dictionary<GlobalDictionaries.ClientSyncModes, string> SyncModes
    {
      get => GlobalDictionaries.ClientsSyncModesDictionary();
    }

    public void OnReLoad() => this.OnPropertyChanged("IsEnabledAcceptSms");

    public bool IsEnabledAcceptSms
    {
      get
      {
        Integrations integrationsSettings = this._integrationsSettings;
        return integrationsSettings == null || integrationsSettings.Sms.Type != 0;
      }
    }

    public GlobalDictionaries.ClientSyncModes SyncMode
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        return settings == null ? GlobalDictionaries.ClientSyncModes.None : settings.Clients.SyncMode;
      }
      set
      {
        this.Settings.Clients.SyncMode = value;
        this.OnPropertyChanged("FileSyncConfigsVisibility");
      }
    }

    public ICommand ShowPropClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmPropertyList().ShowProperties(GlobalDictionaries.EntityTypes.Client)));
      }
    }

    public Visibility FileSyncConfigsVisibility
    {
      get
      {
        return this.SyncMode != GlobalDictionaries.ClientSyncModes.FileSync ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ClientsPageViewModel()
    {
    }

    public ClientsPageViewModel(Gbs.Core.Config.Settings settings, Integrations integrations)
    {
      this.Settings = settings;
      this._integrationsSettings = integrations;
    }

    public Dictionary<GlobalDictionaries.ActionAuthType, string> ActionAuthTypeDictionary
    {
      get => GlobalDictionaries.ActionAuthTypeDictionary();
    }

    public bool Save()
    {
      if (this._integrationsSettings.Sms.Type.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAero, GlobalDictionaries.SmsServiceType.SmsCenter))
      {
        if (!this.Settings.Clients.BonusesAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Call))
        {
          if (!this.Settings.Clients.CreditAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Call))
            goto label_4;
        }
        MessageBoxHelper.Warning(Translate.SmsCenter_GetCodeForCall_К_сожалению__осуществлять_подтверждение_звонком_через_СМС_Центр_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_вариант_подтвержления_по_СМС_);
        return false;
      }
label_4:
      if (this._integrationsSettings.Sms.Type.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsRu))
      {
        if (!this.Settings.Clients.BonusesAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Wa))
        {
          if (!this.Settings.Clients.CreditAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Wa))
            goto label_8;
        }
        MessageBoxHelper.Warning(Translate.SmsCenter_SendMsgByWhatsApp_К_сожалению__отправлять_сообщения_с_помощью_WhatsApp_через_этот_СМС_провайдер_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_другой_вариант_вариант_отпрвки_данных_);
        return false;
      }
label_8:
      if (this._integrationsSettings.Sms.Type.IsEither<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsRu))
      {
        if (!this.Settings.Clients.BonusesAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Viber))
        {
          if (!this.Settings.Clients.CreditAuthType.IsEither<GlobalDictionaries.ActionAuthType>(GlobalDictionaries.ActionAuthType.Viber))
            goto label_12;
        }
        MessageBoxHelper.Warning(Translate.SmsRu_SendMsgByViber_К_сожалению__отправлять_сообщения_с_помощью_Viber_через_этот_СМС_провайдер_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_другой_вариант_вариант_отпрвки_данных_);
        return false;
      }
label_12:
      if (this.Settings.Clients.BonusesAuthType == GlobalDictionaries.ActionAuthType.None && this.Settings.Clients.CreditAuthType == GlobalDictionaries.ActionAuthType.None || this._integrationsSettings.Sms.Type != GlobalDictionaries.SmsServiceType.None)
        return true;
      MessageBoxHelper.Warning(Translate.ClientsPageViewModel_Save_Необходимо_выбрать_СМС_провайдер_для_отправки_уведомление__либо_отключить_подтверждение_операций_кодом_звонком_);
      return false;
    }
  }
}
