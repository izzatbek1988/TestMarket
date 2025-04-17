// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.SendDigitalCheckViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Other
{
  public class SendDigitalCheckViewModel : ViewModelWithForm
  {
    private string _address = "";

    public string Name
    {
      get
      {
        return this.Client != null && !this.Client.Name.IsNullOrEmpty() ? this.Client.Name : Translate.SendDigitalCheckViewModel_Name__не_указано_;
      }
    }

    public string Phone
    {
      get
      {
        return this.Client != null && !this.Client.Phone.IsNullOrEmpty() ? this.Client.Phone : Translate.SendDigitalCheckViewModel_Name__не_указано_;
      }
    }

    public string Email
    {
      get
      {
        return this.Client != null && !this.Client.Email.IsNullOrEmpty() ? this.Client.Email : Translate.SendDigitalCheckViewModel_Name__не_указано_;
      }
    }

    public Client Client { get; set; }

    public bool Result { get; set; }

    public string Address
    {
      get => this._address;
      set
      {
        this._address = value;
        this.OnPropertyChanged(nameof (Address));
      }
    }

    public ICommand SetPhoneCommand { get; set; }

    public ICommand SetEmailCommand { get; set; }

    public ICommand CancelCommand { get; set; }

    public ICommand SendCommand { get; set; }

    public bool IsEnabledEmail
    {
      get
      {
        Client client = this.Client;
        return client != null && !client.Email.IsNullOrEmpty();
      }
    }

    public bool IsEnabledPhone
    {
      get
      {
        Client client = this.Client;
        return client != null && !client.Phone.IsNullOrEmpty();
      }
    }

    public SendDigitalCheckViewModel()
    {
    }

    public SendDigitalCheckViewModel(Client client)
    {
      SendDigitalCheckViewModel digitalCheckViewModel = this;
      this.Client = client;
      this.OnPropertyChanged(nameof (Client));
      this.OnPropertyChanged(nameof (IsEnabledEmail));
      this.OnPropertyChanged(nameof (IsEnabledPhone));
      if (client != null)
      {
        this.Address = !this.Client.Email.IsNullOrEmpty() ? client?.Email ?? string.Empty : (client.Phone.IsNullOrEmpty() ? string.Empty : this.ClearPhone(client.Phone));
        this.SetPhoneCommand = (ICommand) new RelayCommand((Action<object>) (obj => digitalCheckViewModel.Address = digitalCheckViewModel.ClearPhone(client.Phone)));
        this.SetEmailCommand = (ICommand) new RelayCommand((Action<object>) (obj => digitalCheckViewModel.Address = client.Email));
      }
      this.SendCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        bool isSaveInfoClient = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsSaveInfoClient;
        if (digitalCheckViewModel.Address.IsNullOrEmpty())
        {
          MessageBoxHelper.Warning(Translate.SendDigitalCheckViewModel_SendDigitalCheckViewModel_Необходимо_указать_адрес_электронной_почты_или_номер_телефона_для_отправки_чека_);
        }
        else
        {
          if (digitalCheckViewModel.Address.Contains("@"))
          {
            try
            {
              MailAddress mailAddress = new MailAddress(digitalCheckViewModel.Address);
            }
            catch (FormatException ex)
            {
              MessageBoxHelper.Warning(Translate.SendDigitalCheckViewModel_Введен_некорректный_адрес_электронной_почты);
              return;
            }
            if (((digitalCheckViewModel.Client == null || string.Equals(digitalCheckViewModel.Client.Email, digitalCheckViewModel.Address, StringComparison.CurrentCultureIgnoreCase) ? 0 : (!digitalCheckViewModel.Address.IsNullOrEmpty() ? 1 : 0)) & (isSaveInfoClient ? 1 : 0)) != 0 && MessageBoxHelper.Question(Translate.SendDigitalCheckViewModel_SendDigitalCheckViewModel_Сохранить_данный_E_mail_для_выбранного_контакта_) == MessageBoxResult.Yes)
            {
              digitalCheckViewModel.Client.Email = digitalCheckViewModel.Address;
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
                new ClientsRepository(dataBase).Save(digitalCheckViewModel.Client);
            }
          }
          else
          {
            if (!new Regex("\\+7(\\d){10}").Match(digitalCheckViewModel.Address).Success)
            {
              MessageBoxHelper.Warning(Translate.SendDigitalCheckViewModel_Введен_некорректный_номер_телефона__Телефон_должен_быть_в_формате__78007005295);
              return;
            }
            if (((digitalCheckViewModel.Client == null || !(digitalCheckViewModel.Client.Phone != digitalCheckViewModel.Address) ? 0 : (!digitalCheckViewModel.Address.IsNullOrEmpty() ? 1 : 0)) & (isSaveInfoClient ? 1 : 0)) != 0 && MessageBoxHelper.Show(Translate.SendDigitalCheckViewModel_SendDigitalCheckViewModel_Сохранить_данный_номер_телефона_для_выбранного_контакта_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
              digitalCheckViewModel.Client.Phone = digitalCheckViewModel.Address;
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
                new ClientsRepository(dataBase).Save(digitalCheckViewModel.Client);
            }
          }
          digitalCheckViewModel.Result = true;
          digitalCheckViewModel.CloseAction();
        }
      }));
      this.CancelCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        digitalCheckViewModel.Result = false;
        digitalCheckViewModel.Address = string.Empty;
        digitalCheckViewModel.CloseAction();
      }));
    }

    private string ClearPhone(string phone)
    {
      phone = ((IEnumerable<string>) new string[3]
      {
        " ",
        ")",
        "-"
      }).Aggregate<string, string>(phone, (Func<string, string, string>) ((current, c) => current.Replace(c, string.Empty)));
      if (phone.Length > 10)
        phone = phone.Substring(phone.Length - 10);
      if (phone.Length == 10)
        phone = "+7" + phone;
      return phone;
    }
  }
}
