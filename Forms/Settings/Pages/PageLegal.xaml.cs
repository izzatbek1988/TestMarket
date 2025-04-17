// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.SettingBillViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Settings.Egais;
using Gbs.Forms.Settings.Legal;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class SettingBillViewModel : ViewModel
  {
    public ICommand SettingMarkGroupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new MarkedTypeListViewModel().ShowMarkGroups()));
      }
    }

    public Visibility VisibilityBlockHome
    {
      get
      {
        DataBase dataBase = new ConfigsRepository<DataBase>().Get();
        return (dataBase != null ? (dataBase.ModeProgram == GlobalDictionaries.Mode.Home ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Gbs.Core.Config.Settings Settings { get; set; }

    public Integrations Integrations { get; set; }

    private bool ValidationCrptConfig()
    {
      if (!this.Settings.Sales.IsCheckMarkInfoTrueApi || this.Settings.Interface.Country != GlobalDictionaries.Countries.Russia || new ConfigsRepository<DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion < GlobalDictionaries.Devices.FfdVersions.Ffd120 || !this.Integrations.Crpt.Token.IsNullOrEmpty() && Guid.TryParse(this.Integrations.Crpt.Token, out Guid _))
        return true;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Для работы с Честным знаком нужно указать корректный токен для взаимодействия.\n\nТокен необходимо сформировать в личном кабинете Честного знака и указать его в программе."));
      return true;
    }

    public bool IsCheckMarkInfoTrueApi
    {
      get => this.Settings?.Sales?.IsCheckMarkInfoTrueApi.GetValueOrDefault();
      set
      {
        this.Settings.Sales.IsCheckMarkInfoTrueApi = value;
        if (value)
          return;
        MessageBoxHelper.Warning("Отключение работы в рамках 'Разрешительного режима' может привести к нарушению требований законодательства. Рекомендуется НЕ отключать данную опцию!");
      }
    }

    public bool IsTabooSaleNoСorrected
    {
      get => this.Settings?.Sales?.IsTabooSaleNoСorrected.GetValueOrDefault();
      set
      {
        this.Settings.Sales.IsTabooSaleNoСorrected = value;
        if (value)
          return;
        MessageBoxHelper.Warning(Translate.SettingBillViewModel_IsTabooSaleNoСorrected_Отключение_запрета_продаж_с_некорректными_кодами_маркировки_может_привести_к_нарушению_требований_законодательства__Рекомендуется_НЕ_отключать_даную_опцию_);
      }
    }

    public ObservableCollection<MultiValueControl.Value> SmokeValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public Visibility VisibilityOnlyRus
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        return (settings != null ? (settings.Interface.Country == GlobalDictionaries.Countries.Russia ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand EgaisOpenUtmCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Integrations.Egais.PathUtm.IsNullOrEmpty())
            MessageBoxHelper.Warning("Не указан путь для подключения к УТМ. Введите адрес в формате 127.0.0.1:8080 и повторите попытку.");
          else
            FileSystemHelper.OpenSite(this.Integrations.Egais.PathUtm);
        }));
      }
    }

    public ICommand EgaisGetOldStocksOneRegister
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            new EgaisRepository().GetOldStockForOneRegister();
            int num = (int) MessageBoxHelper.Show("Запрос на получение остатков алкогольной продукции с первого регистра в ЕГАИС успешно сформирован, ожидайте уведомление об обработке запроса.");
          }
          catch (Exception ex)
          {
            MessageBoxHelper.Error("Не удалось запросить остатки алкогольной продукции из УТМ с первого регистра.\n\n" + ex.Message);
          }
        }));
      }
    }

    public ICommand EgaisGetOldStocksTwoRegister
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            new EgaisRepository().GetOldStockForTwoRegister();
            int num = (int) MessageBoxHelper.Show("Запрос на получение остатков алкогольной продукции со второго регистра в ЕГАИС успешно сформирован, ожидайте уведомление об обработке запроса.");
          }
          catch (Exception ex)
          {
            MessageBoxHelper.Error("Не удалось запросить остатки товаров из УТМ со второго регистра.\n\n" + ex.Message);
          }
        }));
      }
    }

    public ICommand EgaisGetInformBRegIdOneRegister
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            new EgaisRepository().GetOldStockForOneRegister();
            int num = (int) MessageBoxHelper.Show("Запрос на обновление справок для товаров с первого регистра в ЕГАИС успешно сформирован, ожидайте уведомление об обработке запроса.");
          }
          catch (Exception ex)
          {
            MessageBoxHelper.Error("Не удалось обновить номера справок для товаров с первого регистра.\n\n" + ex.Message);
          }
        }));
      }
    }

    public ICommand EgaisGetInformBRegIdTwoRegister
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            new EgaisRepository().GetOldStockForTwoRegister();
            int num = (int) MessageBoxHelper.Show("Запрос на обновление справок для товаров со второго регистра в ЕГАИС успешно сформирован, ожидайте уведомление об обработке запроса.");
          }
          catch (Exception ex)
          {
            MessageBoxHelper.Error("Не удалось обновить номера справок для товаров со вторрого регистра.\n\n" + ex.Message);
          }
        }));
      }
    }

    public ICommand ShowBeerTapsListCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ListBeerTapsViewModel().Show()));
      }
    }

    private bool ValidationEgaisConfig()
    {
      ActionResult actionResult = this.Integrations.Egais.ValidateSetting();
      if (actionResult.Result == ActionResult.Results.Error)
      {
        MessageBoxHelper.Warning("Для работы с ЕГАИС нужно корректно заполнить все поля.\n\n" + string.Join("\n\n", (IEnumerable<string>) actionResult.Messages) + "\n\nЧтобы сохранить настройки заполните нужные поля или отключите работу с ЕГАИС.");
        this.BillTabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "EgaisTabItem")).IsSelected = true;
        return false;
      }
      if (this.Integrations.Egais.IsActive)
      {
        SalePoints.SalePoint salePoint = SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
        if (!salePoint.Organization.Address.IsNullOrEmpty())
        {
          EntityProperties.PropertyValue propertyValue = salePoint.Organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid));
          bool? nullable;
          if (propertyValue == null)
          {
            nullable = new bool?();
          }
          else
          {
            object obj = propertyValue.Value;
            if (obj == null)
            {
              nullable = new bool?();
            }
            else
            {
              string str = obj.ToString();
              nullable = str != null ? new bool?(str.IsNullOrEmpty()) : new bool?();
            }
          }
          if (!nullable.GetValueOrDefault(true))
            goto label_11;
        }
        ProgressBarHelper.Close();
        MessageBoxHelper.Warning("Необходимо указать ИНН/КПП и адрес организации для работы с ЕГАИС. \n\nЧтобы сохранить настройки заполните нужные поля или отключите работу с ЕГАИС.");
        new FrmClientCard().ShowCard(salePoint.Organization.Uid, out ClientAdnSum _, action: Actions.ClientsEdit);
        return false;
      }
label_11:
      return true;
    }

    public SettingBillViewModel()
    {
    }

    public SettingBillViewModel(Gbs.Core.Config.Settings settings, Integrations integrations)
    {
      this.Settings = settings;
      this.Integrations = integrations;
      string smokeBlockValues = settings.Sales.SmokeBlockValues;
      char[] separator = new char[3]{ ' ', ';', ',' };
      foreach (string str in smokeBlockValues.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        this.SmokeValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
    }

    public bool Save()
    {
      this.Settings.Sales.SmokeBlockValues = string.Join("; ", this.SmokeValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      return this.ValidationEgaisConfig() && this.ValidationCrptConfig();
    }

    public TabControl BillTabControl { get; set; }
  }
}
