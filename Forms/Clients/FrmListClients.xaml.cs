// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.CatalogClientsModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Forms.Excel;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api;
using Planfix.Api.Entities.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Clients
{
  public partial class CatalogClientsModelView : ViewModelWithForm
  {
    private CatalogClientsModelView.FilterEqualEnum _selectedFilterEqual;
    private ListBoxItem _selectedFilterSaleSum;
    private Decimal? _filterSaleSum;
    private ListBoxItem _selectedFilterCreditSum;
    private Decimal? _filterCreditSum;
    private ListBoxItem _selectedFilterBonusesSum;
    private Decimal? _filterBonusesSum;
    private ObservableCollection<GoodsCatalogModelView.FilterProperty> _filterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
    private ObservableCollection<CatalogClientsModelView.ClientInfo> _clientsList = new ObservableCollection<CatalogClientsModelView.ClientInfo>();
    private string _filter = string.Empty;
    private Gbs.Core.Entities.Clients.Group _selectGroup;
    private Decimal _sumBonuses;
    private Decimal _sumCredit;
    private Decimal _sumSale;

    public Visibility FindOrgByInnVisibility
    {
      get
      {
        DaData daData = new ConfigsRepository<Integrations>().Get().DaData;
        GlobalDictionaries.Countries country = new ConfigsRepository<Settings>().Get().Interface.Country;
        if (daData.IsActive)
        {
          if (country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Belarus, GlobalDictionaries.Countries.Kazakhstan))
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public ICommand FindOrgByInn
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => DaDataRepository.Search()));
    }

    public CatalogClientsModelView.FilterEqualEnum SelectedFilterEqual
    {
      get => this._selectedFilterEqual;
      set
      {
        this._selectedFilterEqual = value;
        this.SearchForFilter();
      }
    }

    public Dictionary<CatalogClientsModelView.FilterEqualEnum, string> ListFilterEqual { get; set; } = new Dictionary<CatalogClientsModelView.FilterEqualEnum, string>()
    {
      {
        CatalogClientsModelView.FilterEqualEnum.All,
        Translate.CatalogClientsModelView_ListFilterEqual_Все_контакты
      },
      {
        CatalogClientsModelView.FilterEqualEnum.EqualName,
        Translate.CatalogClientsModelView_ListFilterEqual_Одинаковое_ФИО
      },
      {
        CatalogClientsModelView.FilterEqualEnum.EqualBarcode,
        Translate.CatalogClientsModelView_ListFilterEqual_Одинаковый_номер_карты
      },
      {
        CatalogClientsModelView.FilterEqualEnum.EqualPhone,
        Translate.CatalogClientsModelView_ListFilterEqual_Одинаковый_номер_телефона
      }
    };

    public string SaleSumFilterConditionText { get; set; }

    public ListBoxItem SelectedFilterSaleSum
    {
      get => this._selectedFilterSaleSum;
      set
      {
        this._selectedFilterSaleSum = value;
        this.SaleSumFilterConditionText = value.Content.ToString();
        if (!this.FilterSaleSum.HasValue)
          return;
        this.SearchForFilter();
      }
    }

    public Decimal? FilterSaleSum
    {
      get => this._filterSaleSum;
      set
      {
        this._filterSaleSum = value;
        this.SearchForFilter();
      }
    }

    public string CreditSumFilterConditionText { get; set; }

    public ListBoxItem SelectedFilterCreditSum
    {
      get => this._selectedFilterCreditSum;
      set
      {
        this._selectedFilterCreditSum = value;
        this.CreditSumFilterConditionText = value.Content.ToString();
        if (!this.FilterCreditSum.HasValue)
          return;
        this.SearchForFilter();
      }
    }

    public Decimal? FilterCreditSum
    {
      get => this._filterCreditSum;
      set
      {
        this._filterCreditSum = value;
        this.SearchForFilter();
      }
    }

    public string BonusesSumFilterConditionText { get; set; }

    public ListBoxItem SelectedFilterBonusesSum
    {
      get => this._selectedFilterBonusesSum;
      set
      {
        this._selectedFilterBonusesSum = value;
        this.BonusesSumFilterConditionText = value.Content.ToString();
        if (!this.FilterBonusesSum.HasValue)
          return;
        this.SearchForFilter();
      }
    }

    public Decimal? FilterBonusesSum
    {
      get => this._filterBonusesSum;
      set
      {
        this._filterBonusesSum = value;
        this.SearchForFilter();
      }
    }

    private static List<ListBoxItem> _filters
    {
      get
      {
        List<ListBoxItem> filters = new List<ListBoxItem>();
        ListBoxItem listBoxItem1 = new ListBoxItem();
        listBoxItem1.Content = (object) "=";
        filters.Add(listBoxItem1);
        ListBoxItem listBoxItem2 = new ListBoxItem();
        listBoxItem2.Content = (object) ">";
        filters.Add(listBoxItem2);
        ListBoxItem listBoxItem3 = new ListBoxItem();
        listBoxItem3.Content = (object) "<";
        filters.Add(listBoxItem3);
        return filters;
      }
    }

    public List<ListBoxItem> FilterItems_sales { get; set; } = CatalogClientsModelView._filters;

    public List<ListBoxItem> FilterItems_credits { get; set; } = CatalogClientsModelView._filters;

    public List<ListBoxItem> FilterItems_bonuses { get; set; } = CatalogClientsModelView._filters;

    private void LoadingProperty(FilterOptions setting)
    {
      ObservableCollection<GoodsCatalogModelView.FilterProperty> observableCollection1 = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.FrmClientCard_ФИО,
        IsChecked = setting.ClientSearch.IsName
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Barcode",
        Text = Translate.FrmSearchClient_Карта,
        IsChecked = setting.ClientSearch.IsBarcode
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Phone",
        Text = Translate.FrmClientCard_Телефон,
        IsChecked = setting.ClientSearch.IsPhone
      });
      observableCollection1.Add(new GoodsCatalogModelView.FilterProperty()
      {
        Name = "Email",
        Text = Translate.ExcelClientsViewModel_E_mail,
        IsChecked = setting.ClientSearch.IsEmail
      });
      ObservableCollection<GoodsCatalogModelView.FilterProperty> collection = observableCollection1;
      foreach (EntityProperties.PropertyType propertyType in (IEnumerable<EntityProperties.PropertyType>) EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client, false).OrderBy<EntityProperties.PropertyType, string>((Func<EntityProperties.PropertyType, string>) (x => x.Name)))
      {
        EntityProperties.PropertyType type = propertyType;
        ObservableCollection<GoodsCatalogModelView.FilterProperty> observableCollection2 = collection;
        GoodsCatalogModelView.FilterProperty filterProperty = new GoodsCatalogModelView.FilterProperty();
        filterProperty.Name = type.Uid.ToString();
        filterProperty.Text = type.Name;
        GoodProp.PropItem propItem = setting.ClientSearch.PropList.FirstOrDefault<GoodProp.PropItem>((Func<GoodProp.PropItem, bool>) (x => x.Uid == type.Uid));
        filterProperty.IsChecked = propItem != null && propItem.IsChecked;
        observableCollection2.Add(filterProperty);
      }
      this.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>((IEnumerable<GoodsCatalogModelView.FilterProperty>) collection);
    }

    public FilterOptions Setting { get; }

    public ObservableCollection<GoodsCatalogModelView.FilterProperty> FilterProperties
    {
      get => this._filterProperties;
      set
      {
        this._filterProperties = value;
        new ConfigsRepository<FilterOptions>().Save(this.Setting);
        this.OnPropertyChanged("TextPropButton");
        if (value.Any<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
          return;
        int num = (int) MessageBoxHelper.Show(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
      }
    }

    public string TextPropButton
    {
      get
      {
        int num = this.FilterProperties.Count<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked));
        if (num == this.FilterProperties.Count<GoodsCatalogModelView.FilterProperty>())
          return Translate.GoodsSearchModelView_Все_поля;
        return num != 1 ? Translate.GoodsSearchModelView_Полей__ + num.ToString() : this.FilterProperties.First<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)).Text;
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { private get; set; }

    public IEnumerable<Gbs.Core.Entities.Clients.Group> ListGroups { get; set; } = CatalogClientsModelView.GetClientGroup().Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (gr => !gr.IsDeleted));

    public ObservableCollection<CatalogClientsModelView.ClientInfo> ClientsList
    {
      get => this._clientsList;
      set
      {
        this._clientsList = value;
        this.CountSumClients();
        this.OnPropertyChanged(nameof (ClientsList));
      }
    }

    public CatalogClientsModelView.ClientInfo SelectedClient { get; set; }

    public Gbs.Core.Entities.Clients.Group SelectedGroup
    {
      get => this._selectGroup;
      set
      {
        this._selectGroup = value;
        this.StartSearch();
      }
    }

    public string Filter
    {
      get => this._filter;
      set
      {
        this._filter = value;
        this.OnPropertyChanged(nameof (Filter));
        this.StartSearch();
      }
    }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand ExcelImportCommand { get; set; }

    public ICommand PrintClients
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.ClientsList.Any<CatalogClientsModelView.ClientInfo>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_В_списке_нет_покупателей);
          }
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForClients(this.ClientsList.Select<CatalogClientsModelView.ClientInfo, ClientAdnSum>((Func<CatalogClientsModelView.ClientInfo, ClientAdnSum>) (x => x.Client))), this.AuthUser);
        }));
      }
    }

    public ICommand JoinClientsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<CatalogClientsModelView.ClientInfo> list = ((IEnumerable) obj).Cast<CatalogClientsModelView.ClientInfo>().ToList<CatalogClientsModelView.ClientInfo>();
          Gbs.Core.Entities.Users.User user = this.AuthUser.Clone();
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ClientJoin))
            {
              (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ClientJoin);
              if (!access.Result)
                return;
              user = access.User;
            }
            if (list.Count < 2)
            {
              int num1 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_JoinClientsCommand_Необходимо_выбрать_2_или_более_покупателя_для_объединения);
            }
            else
            {
              CatalogClientsModelView.ClientInfo itemFirst = list.First<CatalogClientsModelView.ClientInfo>();
              if (list.Any<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Group.IsSupplier != itemFirst.Client.Client.Group.IsSupplier)))
              {
                int num2 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_JoinClientsCommand_Все_контакты_должны_быть_одного_типа__поставщики_или_покупатели__);
              }
              else
              {
                if (MessageBoxHelper.Show(string.Format(string.Format(Translate.CatalogClientsModelView_JoinClientsCommand_Вы_уверены__что_хотите_объединить__0__контактов_, (object) list.Count), (object) ((ICollection) obj).Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                  return;
                ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.UserGroupViewModel_UserGroupViewModel_Объединение_покупателей);
                list.RemoveAll((Predicate<CatalogClientsModelView.ClientInfo>) (x => x.Client.Client.Uid == itemFirst.Client.Client.Uid));
                new ClientsRepository(dataBase).RemoveClient(list.Select<CatalogClientsModelView.ClientInfo, Client>((Func<CatalogClientsModelView.ClientInfo, Client>) (x => x.Client.Client)).ToList<Client>(), itemFirst.Client.Client.Uid);
                ClientAdnSum clientByUidAndSum = new ClientsRepository(dataBase).GetClientByUidAndSum(itemFirst.Client.Client.Uid);
                clientByUidAndSum.Client.Barcode = clientByUidAndSum.Client.Barcode.IsNullOrEmpty() ? list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !x.Client.Client.Barcode.IsNullOrEmpty()))?.Client.Client.Barcode ?? "" : clientByUidAndSum.Client.Barcode;
                clientByUidAndSum.Client.Phone = clientByUidAndSum.Client.Phone.IsNullOrEmpty() ? list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !x.Client.Client.Phone.IsNullOrEmpty()))?.Client.Client.Phone ?? "" : clientByUidAndSum.Client.Phone;
                clientByUidAndSum.Client.Address = clientByUidAndSum.Client.Address.IsNullOrEmpty() ? list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !x.Client.Client.Address.IsNullOrEmpty()))?.Client.Client.Address ?? "" : clientByUidAndSum.Client.Address;
                clientByUidAndSum.Client.Email = clientByUidAndSum.Client.Email.IsNullOrEmpty() ? list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !x.Client.Client.Email.IsNullOrEmpty()))?.Client.Client.Email ?? "" : clientByUidAndSum.Client.Email;
                Client client = clientByUidAndSum.Client;
                if (!client.Birthday.HasValue)
                {
                  DateTime? birthday;
                  client.Birthday = birthday = (DateTime?) list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Birthday.HasValue))?.Client.Client.Birthday;
                }
                clientByUidAndSum.Client.Comment = clientByUidAndSum.Client.Comment.IsNullOrEmpty() ? list.FirstOrDefault<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !x.Client.Client.Comment.IsNullOrEmpty()))?.Client.Client.Comment ?? "" : clientByUidAndSum.Client.Comment;
                foreach (EntityProperties.PropertyType types in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client))
                {
                  EntityProperties.PropertyType clientProperty = types;
                  if (clientByUidAndSum.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == clientProperty.Uid))?.Value == null)
                  {
                    EntityProperties.PropertyValue propertyValue = list.SelectMany<CatalogClientsModelView.ClientInfo, EntityProperties.PropertyValue>((Func<CatalogClientsModelView.ClientInfo, IEnumerable<EntityProperties.PropertyValue>>) (x => (IEnumerable<EntityProperties.PropertyValue>) x.Client.Client.Properties)).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == clientProperty.Uid && x.Value != null));
                    if (propertyValue != null)
                    {
                      clientByUidAndSum.Client.Properties.RemoveAll((Predicate<EntityProperties.PropertyValue>) (x => x.Type.Uid == clientProperty.Uid));
                      clientByUidAndSum.Client.Properties.Add(new EntityProperties.PropertyValue()
                      {
                        Type = clientProperty,
                        Value = propertyValue.Value
                      });
                    }
                  }
                }
                itemFirst.Client = clientByUidAndSum;
                itemFirst.Client.Client.IsDeleted = false;
                new ClientsRepository(dataBase).Save(itemFirst.Client.Client);
                foreach (CatalogClientsModelView.ClientInfo source in list)
                {
                  CatalogClientsModelView.ClientInfo clientInfo = source.Clone<CatalogClientsModelView.ClientInfo>();
                  this.ClientsList.Remove(source);
                  ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((Gbs.Core.Entities.IEntity) clientInfo.Client.Client, (Gbs.Core.Entities.IEntity) itemFirst.Client.Client, ActionType.JoinGood, GlobalDictionaries.EntityTypes.Client, user), false);
                }
                itemFirst = new CatalogClientsModelView.ClientInfo()
                {
                  Client = new ClientsRepository(dataBase).GetClientByUidAndSum(itemFirst.Client.Client.Uid)
                };
                this.ClientsList[this.ClientsList.ToList<CatalogClientsModelView.ClientInfo>().FindIndex((Predicate<CatalogClientsModelView.ClientInfo>) (x => x.Client.Client.Uid == itemFirst.Client.Client.Uid))] = itemFirst;
                this.CachedDbClients[this.CachedDbClients.ToList<CatalogClientsModelView.ClientInfo>().FindIndex((Predicate<CatalogClientsModelView.ClientInfo>) (x => x.Client.Client.Uid == itemFirst.Client.Client.Uid))] = itemFirst;
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
                {
                  Title = Translate.CatalogClientsModelView_JoinClientsCommand_Объединение_контактов,
                  Text = string.Format(Translate.CatalogClientsModelView_JoinClientsCommand_Объединение_контактов_выполнено__все_данные_перенесены_в_контакт__0_, (object) clientByUidAndSum.Client.Name)
                });
                CacheHelper.Clear(CacheHelper.CacheTypes.ClientsCredits);
                CacheHelper.Clear(CacheHelper.CacheTypes.Clients);
                progressBar.Close();
                this.CountSumClients();
              }
            }
          }
        }));
      }
    }

    public ICommand AddRangeClientInPlanFix
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<LinkEntities> list = new LinkEntitiesRepository().GetAllItems().Where<LinkEntities>((Func<LinkEntities, bool>) (x => x.Type == TypeEntity.Client)).ToList<LinkEntities>();
          if (!this.ClientsList.Any<CatalogClientsModelView.ClientInfo>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_Список_не_может_быть_пустым);
          }
          else
          {
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CatalogClientsModelView_Выгрузка_контактов_в_ПланФикс);
            ContactRepository contactRepository = new ContactRepository();
            int num2 = 0;
            int num3 = 0;
            foreach (CatalogClientsModelView.ClientInfo clients in (Collection<CatalogClientsModelView.ClientInfo>) this.ClientsList)
            {
              CatalogClientsModelView.ClientInfo client = clients;
              if (list.Any<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == client.Client.Client.Uid)))
              {
                ++num3;
              }
              else
              {
                PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
                ConfigManager.Config = new Planfix.Api.Config(planfix.AccountName, planfix.ApiUrl, planfix.DecryptedKeyApi, planfix.DecryptedToken);
                Contact planfixContact = PlanfixHelper.GbsContactToPlanfixContact(client.Client.Client, planfix);
                Planfix.Api.Answer answer = contactRepository.Add(ref planfixContact);
                if (answer.Result == Planfix.Api.Answer.ResultTypes.Error)
                {
                  MessageBoxHelper.Error(Translate.GoodsCatalogModelView_Во_время_выгрузки_произошла_ошибка__проверьте_настройки_интеграции_с_ПланФиксом);
                  return;
                }
                if (answer.Result == Planfix.Api.Answer.ResultTypes.Ok)
                {
                  ++num2;
                  new LinkEntitiesRepository().Save(new LinkEntities()
                  {
                    EntityUid = client.Client.Client.Uid,
                    Type = TypeEntity.Client,
                    Id = planfixContact.id
                  });
                }
                else if (answer.ErrorCode == Planfix.Api.Answer.ErrorCodes.WrongApiKey)
                  break;
              }
            }
            progressBar.Close();
            int num4 = (int) MessageBoxHelper.Show(string.Format(Translate.CatalogClientsModelView_Перенесено__0___1__контактов_2_, (object) num2, (object) this.ClientsList.Count, num3 > 0 ? (object) (Gbs.Helpers.Other.NewLine(2) + string.Format(Translate.CatalogClientsModelView__0___1__выгружено_ранее, (object) num3, (object) this.ClientsList.Count)) : (object) ""));
          }
        }));
      }
    }

    public List<CatalogClientsModelView.ClientInfo> CachedDbClients { get; set; }

    public Decimal SumSaleClients
    {
      get => this._sumSale;
      set
      {
        this._sumSale = value;
        this.OnPropertyChanged(nameof (SumSaleClients));
      }
    }

    public Decimal SumCreditClients
    {
      get => this._sumCredit;
      set
      {
        this._sumCredit = value;
        this.OnPropertyChanged(nameof (SumCreditClients));
      }
    }

    public Decimal SumBonusesClients
    {
      get => this._sumBonuses;
      set
      {
        this._sumBonuses = value;
        this.OnPropertyChanged(nameof (SumBonusesClients));
      }
    }

    public Visibility VisibilityMenuPlanFix
    {
      get
      {
        return !new ConfigsRepository<Integrations>().Get().Planfix.IsActive ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public CatalogClientsModelView()
    {
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<FrmListClients>())
        return;
      this.Filter = barcode;
      Task.Run((Action) (() =>
      {
        this.StartSearch();
        Thread.Sleep(50);
      }));
    }

    public CatalogClientsModelView(bool flag)
    {
      try
      {
        this.LoadDataFromDb();
        this.Setting = new ConfigsRepository<FilterOptions>().Get();
        this.LoadingProperty(this.Setting);
        this.SelectedGroup = this.ListGroups.Single<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.Uid == Guid.Empty));
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddClient()));
        this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditClient));
        this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteClient));
        this.ExcelImportCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.ImportFromExcel()));
        this.SelectedFilterSaleSum = this.FilterItems_sales[1];
        this.SelectedFilterCreditSum = this.FilterItems_credits[1];
        this.SelectedFilterBonusesSum = this.FilterItems_bonuses[1];
        this.SaleSumFilterConditionText = this.SelectedFilterSaleSum.Content.ToString();
        this.CreditSumFilterConditionText = this.SelectedFilterCreditSum.Content.ToString();
        this.BonusesSumFilterConditionText = this.SelectedFilterBonusesSum.Content.ToString();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private void ImportFromExcel()
    {
      List<Client> source = new FrmExcelClients().Import();
      this.ListGroups = (IEnumerable<Gbs.Core.Entities.Clients.Group>) CatalogClientsModelView.GetClientGroup().Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (gr => !gr.IsDeleted)).ToList<Gbs.Core.Entities.Clients.Group>();
      if ((source != null ? (source.Any<Client>() ? 1 : 0) : 0) == 0)
        return;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CatalogClientsModelView_Обновление_списка_покупателей);
      this.LoadDataFromDb();
      this.ListGroups = CatalogClientsModelView.GetClientGroup().Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (gr => !gr.IsDeleted));
      this.OnPropertyChanged("ListGroups");
      progressBar.Close();
    }

    private void DeleteClient(object obj)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ClientsDelete) && !new Authorization().GetAccess(Actions.ClientsDelete).Result)
          return;
        if (this.SelectedClient != null)
        {
          List<CatalogClientsModelView.ClientInfo> list = ((IEnumerable) obj).Cast<CatalogClientsModelView.ClientInfo>().ToList<CatalogClientsModelView.ClientInfo>();
          if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          ClientsRepository clientsRepository = new ClientsRepository(dataBase);
          foreach (CatalogClientsModelView.ClientInfo clientInfo in list)
          {
            Client oldItem = clientInfo.Client.Client.Clone<Client>();
            clientInfo.Client.Client.IsDeleted = true;
            clientsRepository.Save(clientInfo.Client.Client);
            this.SumSaleClients -= clientInfo.Client.TotalSalesSum;
            this.SumCreditClients -= clientInfo.Client.TotalCreditSum;
            this.ClientsList.Remove(clientInfo);
            this.CachedDbClients.Remove(clientInfo);
            Client client = clientInfo.Client.Client;
            Gbs.Core.Entities.Users.User authUser = this.AuthUser;
            ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((Gbs.Core.Entities.IEntity) oldItem, (Gbs.Core.Entities.IEntity) client, ActionType.Delete, GlobalDictionaries.EntityTypes.Client, authUser), false);
          }
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_Требуется_выбрать_покупателя, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }
    }

    private void EditClient(object obj)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ClientsEdit) && !new Authorization().GetAccess(Actions.ClientsEdit).Result)
          return;
        if (this.SelectedClient != null)
        {
          if (((IEnumerable) obj).Cast<CatalogClientsModelView.ClientInfo>().ToList<CatalogClientsModelView.ClientInfo>().Count > 1)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else if (CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsDeleted)).ToList<Gbs.Core.Entities.Users.User>().Any<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Client.Uid == this.SelectedClient.Client.Client.Uid)))
          {
            MessageBoxHelper.Warning(string.Format(Translate.CatalogClientsModelView_EditClient_Редактирование_карточки_контакта___0____являющегося_сотрудником__допустимо_только_из_настроек_программы, (object) this.SelectedClient.Client.Client.Name));
          }
          else
          {
            ClientAdnSum client;
            if (!new FrmClientCard().ShowCard(this.SelectedClient.Client.Client.Uid, out client, authUser: this.AuthUser, action: Actions.ClientsEdit))
              return;
            CatalogClientsModelView.ClientInfo clientInfo = new CatalogClientsModelView.ClientInfo()
            {
              Client = client
            };
            DateTime? nullable1 = clientInfo.Client.Client.Birthday;
            DateTime minValue = DateTime.MinValue;
            if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() == minValue ? 1 : 0) : 0) != 0)
            {
              Client client1 = clientInfo.Client.Client;
              nullable1 = new DateTime?();
              DateTime? nullable2 = nullable1;
              client1.Birthday = nullable2;
            }
            this.ClientsList[this.ClientsList.ToList<CatalogClientsModelView.ClientInfo>().FindIndex((Predicate<CatalogClientsModelView.ClientInfo>) (x => x.Client.Client.Uid == client.Client.Uid))] = clientInfo;
            this.CachedDbClients[this.CachedDbClients.ToList<CatalogClientsModelView.ClientInfo>().FindIndex((Predicate<CatalogClientsModelView.ClientInfo>) (x => x.Client.Client.Uid == client.Client.Uid))] = clientInfo;
          }
        }
        else
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.CatalogClientsModelView_Требуется_выбрать_покупателя, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }
    }

    private void AddClient()
    {
      ClientAdnSum client1;
      if (!new FrmClientCard().ShowCard(Guid.Empty, out client1, authUser: this.AuthUser))
        return;
      CatalogClientsModelView.ClientInfo clientInfo = new CatalogClientsModelView.ClientInfo()
      {
        Client = client1
      };
      DateTime? nullable1 = clientInfo.Client.Client.Birthday;
      DateTime minValue = DateTime.MinValue;
      if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() == minValue ? 1 : 0) : 0) != 0)
      {
        Client client2 = clientInfo.Client.Client;
        nullable1 = new DateTime?();
        DateTime? nullable2 = nullable1;
        client2.Birthday = nullable2;
      }
      this.SumSaleClients += clientInfo.Client.TotalSalesSum;
      this.SumCreditClients += clientInfo.Client.TotalCreditSum;
      this.ClientsList.Add(clientInfo);
      this.CachedDbClients.Add(clientInfo);
    }

    private static List<CatalogClientsModelView.ClientInfo> GetClientsList()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        return new ClientsRepository(dataBase).GetListActiveItemAndSum().Select<ClientAdnSum, CatalogClientsModelView.ClientInfo>((Func<ClientAdnSum, CatalogClientsModelView.ClientInfo>) (client => new CatalogClientsModelView.ClientInfo()
        {
          Client = client
        })).ToList<CatalogClientsModelView.ClientInfo>();
    }

    public ICommand UpdateDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          CacheHelper.Clear(CacheHelper.CacheTypes.ClientsCredits);
          this.LoadDataFromDb();
        }));
      }
    }

    private void LoadDataFromDb()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CatalogClientsModelView_Загрузка_списка_покупателей);
      this.CachedDbClients = CatalogClientsModelView.GetClientsList().OrderBy<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Name)).ToList<CatalogClientsModelView.ClientInfo>();
      this.ClientsList = new ObservableCollection<CatalogClientsModelView.ClientInfo>(this.CachedDbClients);
      this.CountSumClients();
      progressBar.Close();
    }

    private void CountSumClients()
    {
      Task.Run((Action) (() =>
      {
        this.SumCreditClients = 0M;
        this.SumSaleClients = 0M;
        this.SumBonusesClients = 0M;
        this.SumSaleClients += this.ClientsList.Sum<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, Decimal>) (x => x.Client.TotalSalesSum));
        this.SumCreditClients += this.ClientsList.Sum<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, Decimal>) (x => x.Client.TotalCreditSum));
        this.SumBonusesClients += this.ClientsList.Sum<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, Decimal>) (x => x.Client.TotalBonusSum));
      }));
    }

    private static IEnumerable<Gbs.Core.Entities.Clients.Group> GetClientGroup()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Clients.Group> clientGroup = new List<Gbs.Core.Entities.Clients.Group>();
        Gbs.Core.Entities.Clients.Group group = new Gbs.Core.Entities.Clients.Group();
        group.Name = Translate.CatalogClientsModelView_Все_группы;
        group.Uid = Guid.Empty;
        clientGroup.Add(group);
        clientGroup.AddRange((IEnumerable<Gbs.Core.Entities.Clients.Group>) new GroupRepository(dataBase).GetActiveItems());
        return (IEnumerable<Gbs.Core.Entities.Clients.Group>) clientGroup;
      }
    }

    private void StartSearch() => Task.Run(new Action(this.SearchForFilter));

    public void SearchForFilter()
    {
      List<CatalogClientsModelView.ClientInfo> source1 = new List<CatalogClientsModelView.ClientInfo>();
      string filterText = this.Filter.ToLower();
      if (!filterText.IsNullOrEmpty())
      {
        foreach (GoodsCatalogModelView.FilterProperty filterProperty1 in this.FilterProperties.Where<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          GoodsCatalogModelView.FilterProperty filterProperty = filterProperty1;
          Guid result;
          if (Guid.TryParse(filterProperty.Name, out result))
          {
            if (result == GlobalDictionaries.GoodIdUid)
            {
              int intValue;
              if (int.TryParse(filterText, out intValue))
                source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && int.Parse(p.Value.ToString()) == intValue)))));
            }
            else
              source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && p.Value.ToString().ToLower().Contains(filterText))))));
          }
          else
          {
            switch (filterProperty.Name)
            {
              case "Name":
                source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Name.ToLower().Contains(filterText))));
                IEnumerable<CatalogClientsModelView.ClientInfo> source2 = ((IEnumerable<string>) filterText.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<CatalogClientsModelView.ClientInfo>>(this.CachedDbClients.AsEnumerable<CatalogClientsModelView.ClientInfo>(), (Func<IEnumerable<CatalogClientsModelView.ClientInfo>, string, IEnumerable<CatalogClientsModelView.ClientInfo>>) ((current, s) => current.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Name.ToLower().Contains(s)))));
                source1.AddRange((IEnumerable<CatalogClientsModelView.ClientInfo>) source2.ToList<CatalogClientsModelView.ClientInfo>());
                continue;
              case "Barcode":
                source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Barcode.ToLower().Contains(filterText))));
                continue;
              case "Phone":
                string phone = string.Join<char>("", filterText.Where<char>(new Func<char, bool>(char.IsDigit)));
                if (!phone.IsNullOrEmpty())
                {
                  source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => !string.Join<char>("", x.Client.Client.Phone.Where<char>(new Func<char, bool>(char.IsDigit))).IsNullOrEmpty() && string.Join<char>("", x.Client.Client.Phone.Where<char>(new Func<char, bool>(char.IsDigit))).ToLower().Contains(phone))));
                  continue;
                }
                continue;
              case "Email":
                source1.AddRange(this.CachedDbClients.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Email.ToLower().Contains(filterText))));
                continue;
              default:
                continue;
            }
          }
        }
      }
      else
        source1 = new List<CatalogClientsModelView.ClientInfo>((IEnumerable<CatalogClientsModelView.ClientInfo>) (this.CachedDbClients ?? new List<CatalogClientsModelView.ClientInfo>()));
      switch (this.SelectedFilterEqual)
      {
        case CatalogClientsModelView.FilterEqualEnum.EqualName:
          source1 = source1.GetDuplicate<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Name.Trim().ToLower())).OrderBy<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Name)).ToList<CatalogClientsModelView.ClientInfo>();
          break;
        case CatalogClientsModelView.FilterEqualEnum.EqualBarcode:
          source1 = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Barcode.Trim() != string.Empty)).GetDuplicate<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Barcode.Trim())).OrderBy<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Barcode)).ToList<CatalogClientsModelView.ClientInfo>();
          break;
        case CatalogClientsModelView.FilterEqualEnum.EqualPhone:
          source1 = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x => x.Client.Client.Phone.Trim() != string.Empty)).GetDuplicate<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Phone.ClearPhone())).OrderBy<CatalogClientsModelView.ClientInfo, string>((Func<CatalogClientsModelView.ClientInfo, string>) (x => x.Client.Client.Phone)).ToList<CatalogClientsModelView.ClientInfo>();
          break;
      }
      if (this.FilterSaleSum.HasValue)
      {
        List<CatalogClientsModelView.ClientInfo> clientInfoList;
        switch (this.SaleSumFilterConditionText)
        {
          case "=":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalSalesSum = x.Client.TotalSalesSum;
              Decimal? filterSaleSum = this.FilterSaleSum;
              Decimal valueOrDefault = filterSaleSum.GetValueOrDefault();
              return totalSalesSum == valueOrDefault & filterSaleSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case ">":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalSalesSum = x.Client.TotalSalesSum;
              Decimal? filterSaleSum = this.FilterSaleSum;
              Decimal valueOrDefault = filterSaleSum.GetValueOrDefault();
              return totalSalesSum > valueOrDefault & filterSaleSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case "<":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalSalesSum = x.Client.TotalSalesSum;
              Decimal? filterSaleSum = this.FilterSaleSum;
              Decimal valueOrDefault = filterSaleSum.GetValueOrDefault();
              return totalSalesSum < valueOrDefault & filterSaleSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          default:
            clientInfoList = source1;
            break;
        }
        source1 = clientInfoList;
      }
      if (this.FilterCreditSum.HasValue)
      {
        List<CatalogClientsModelView.ClientInfo> clientInfoList;
        switch (this.CreditSumFilterConditionText)
        {
          case "=":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalCreditSum = x.Client.TotalCreditSum;
              Decimal? filterCreditSum = this.FilterCreditSum;
              Decimal valueOrDefault = filterCreditSum.GetValueOrDefault();
              return totalCreditSum == valueOrDefault & filterCreditSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case ">":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalCreditSum = x.Client.TotalCreditSum;
              Decimal? filterCreditSum = this.FilterCreditSum;
              Decimal valueOrDefault = filterCreditSum.GetValueOrDefault();
              return totalCreditSum > valueOrDefault & filterCreditSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case "<":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalCreditSum = x.Client.TotalCreditSum;
              Decimal? filterCreditSum = this.FilterCreditSum;
              Decimal valueOrDefault = filterCreditSum.GetValueOrDefault();
              return totalCreditSum < valueOrDefault & filterCreditSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          default:
            clientInfoList = source1;
            break;
        }
        source1 = clientInfoList;
      }
      if (this.FilterBonusesSum.HasValue)
      {
        List<CatalogClientsModelView.ClientInfo> clientInfoList;
        switch (this.BonusesSumFilterConditionText)
        {
          case "=":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalBonusSum = x.Client.TotalBonusSum;
              Decimal? filterBonusesSum = this.FilterBonusesSum;
              Decimal valueOrDefault = filterBonusesSum.GetValueOrDefault();
              return totalBonusSum == valueOrDefault & filterBonusesSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case ">":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalBonusSum = x.Client.TotalBonusSum;
              Decimal? filterBonusesSum = this.FilterBonusesSum;
              Decimal valueOrDefault = filterBonusesSum.GetValueOrDefault();
              return totalBonusSum > valueOrDefault & filterBonusesSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          case "<":
            clientInfoList = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (x =>
            {
              Decimal totalBonusSum = x.Client.TotalBonusSum;
              Decimal? filterBonusesSum = this.FilterBonusesSum;
              Decimal valueOrDefault = filterBonusesSum.GetValueOrDefault();
              return totalBonusSum < valueOrDefault & filterBonusesSum.HasValue;
            })).ToList<CatalogClientsModelView.ClientInfo>();
            break;
          default:
            clientInfoList = source1;
            break;
        }
        source1 = clientInfoList;
      }
      if (this.SelectedGroup != null && this.SelectedGroup.Uid != Guid.Empty)
        source1 = source1.Where<CatalogClientsModelView.ClientInfo>((Func<CatalogClientsModelView.ClientInfo, bool>) (c => c.Client.Client.Group.Uid == this.SelectedGroup.Uid)).ToList<CatalogClientsModelView.ClientInfo>();
      this.ClientsList = new ObservableCollection<CatalogClientsModelView.ClientInfo>(source1.GroupBy<CatalogClientsModelView.ClientInfo, Guid>((Func<CatalogClientsModelView.ClientInfo, Guid>) (x => x.Client.Client.Uid)).Select<IGrouping<Guid, CatalogClientsModelView.ClientInfo>, CatalogClientsModelView.ClientInfo>((Func<IGrouping<Guid, CatalogClientsModelView.ClientInfo>, CatalogClientsModelView.ClientInfo>) (x => x.First<CatalogClientsModelView.ClientInfo>())));
    }

    public enum FilterEqualEnum
    {
      All,
      EqualName,
      EqualBarcode,
      EqualPhone,
    }

    public class ClientInfo
    {
      public ClientAdnSum Client { get; set; }
    }
  }
}
