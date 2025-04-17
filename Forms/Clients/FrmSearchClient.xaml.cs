// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.SearchClientViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Clients
{
  public partial class SearchClientViewModel : ViewModelWithForm
  {
    private ObservableCollection<Client> _clientsList;
    private string _filterClients = string.Empty;
    private bool _isUser;
    private ObservableCollection<GoodsCatalogModelView.FilterProperty> _filterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>();

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          CacheHelper.Clear(this._isUser ? CacheHelper.CacheTypes.AllUsers : CacheHelper.CacheTypes.AllClients);
          this.GetClients();
        }));
      }
    }

    public bool ResultAction { get; set; }

    public ICommand AddClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (x =>
        {
          ClientAdnSum client = new ClientAdnSum()
          {
            Client = new Client()
          };
          if (double.TryParse(this.FilterClients, out double _))
            client.Client.Phone = this.FilterClients;
          else if (this.FilterClients.Contains("@"))
            client.Client.Email = this.FilterClients;
          else
            client.Client.Name = this.FilterClients;
          if (!new FrmClientCard().ShowCard(client.Client.Uid, out client, client))
            return;
          this.ClientsList.Add(client.Client);
          this.SelectedClient = client.Client;
          this.ResultAction = true;
          this.CloseFrm();
        }));
      }
    }

    public ICommand SelectClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (x =>
        {
          if (this.SelectedClient == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.SearchClientViewModel_Требуется_выбрать_покупателя_);
          }
          else
          {
            this.ResultAction = true;
            this.CloseFrm();
          }
        }));
      }
    }

    public ObservableCollection<Client> ClientsList
    {
      get => this._clientsList;
      set
      {
        this._clientsList = value;
        this.OnPropertyChanged(nameof (ClientsList));
      }
    }

    public Client SelectedClient { get; set; }

    public string FilterClients
    {
      get => this._filterClients;
      set
      {
        this._filterClients = value;
        this.OnPropertyChanged(nameof (FilterClients));
        Task.Run(new Action(this.SearchForFilter));
      }
    }

    public ICommand CloseClient { get; set; }

    public Action CloseFrm { get; set; }

    private bool IsSupplier { get; set; }

    private List<Client> CacheClientsList { get; set; }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Other.IsActiveForm<FrmSearchClient>())
        return;
      this.FilterClients = barcode;
      Task.Run((Action) (() =>
      {
        this.SearchForFilter();
        Thread.Sleep(50);
      }));
    }

    public FilterOptions Setting { get; }

    public SearchClientViewModel(bool isSupplier, bool isUser)
    {
      Performancer per = new Performancer("Загрузка формы поиска контактов");
      this.Setting = new ConfigsRepository<FilterOptions>().Get();
      this.LoadingProperty(this.Setting);
      per.AddPoint("Загрузка настроек");
      ClientsExchangeHelper.GetCashClient();
      per.AddPoint("Получение облачных контактов");
      this.IsSupplier = isSupplier;
      this._isUser = isUser;
      this.GetClients(per);
      per.Stop();
    }

    private void GetClients(Performancer per = null)
    {
      this.CacheClientsList = !this._isUser ? CachesBox.AllClients().Where<Client>((Func<Client, bool>) (x => !x.IsDeleted)).ToList<Client>() : CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsKicked && !x.IsDeleted)).Select<Gbs.Core.Entities.Users.User, Client>((Func<Gbs.Core.Entities.Users.User, Client>) (x => x.Client)).ToList<Client>();
      per?.AddPoint("Загрзука контактов из кэша");
      this.ClientsList = new ObservableCollection<Client>((IEnumerable<Client>) this.CacheClientsList.Where<Client>((Func<Client, bool>) (x => x.Group.IsSupplier == this.IsSupplier)).OrderBy<Client, string>((Func<Client, string>) (x => x.Name)));
    }

    public SearchClientViewModel()
    {
    }

    public void SearchForFilter()
    {
      string filterText = this.FilterClients.ToLower();
      List<Client> source1 = new List<Client>();
      List<Client> list = this.CacheClientsList.Where<Client>((Func<Client, bool>) (x => x.Group.IsSupplier == this.IsSupplier)).ToList<Client>();
      if (this.FilterClients.IsNullOrEmpty())
      {
        this.ClientsList = new ObservableCollection<Client>((IEnumerable<Client>) list.OrderBy<Client, string>((Func<Client, string>) (x => x.Name)));
      }
      else
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
                source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => x.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && int.Parse(p.Value.ToString()) == intValue)))));
            }
            else
              source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => x.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid.ToString() == filterProperty.Name && p.Value.ToString().ToLower().Contains(filterText))))));
          }
          else
          {
            switch (filterProperty.Name)
            {
              case "Name":
                source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => x.Name.ToLower().Contains(filterText))));
                IEnumerable<Client> source2 = ((IEnumerable<string>) filterText.Split(" ".ToCharArray())).Aggregate<string, IEnumerable<Client>>(list.AsEnumerable<Client>(), (Func<IEnumerable<Client>, string, IEnumerable<Client>>) ((current, s) => current.Where<Client>((Func<Client, bool>) (x => x.Name.ToLower().Contains(s)))));
                source1.AddRange((IEnumerable<Client>) source2.ToList<Client>());
                continue;
              case "Barcode":
                source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => x.Barcode.ToLower().Contains(filterText))));
                continue;
              case "Phone":
                string phone = string.Join<char>("", filterText.Where<char>(new Func<char, bool>(char.IsDigit)));
                if (!phone.IsNullOrEmpty())
                {
                  source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => !string.Join<char>("", x.Phone.Where<char>(new Func<char, bool>(char.IsDigit))).IsNullOrEmpty() && string.Join<char>("", x.Phone.Where<char>(new Func<char, bool>(char.IsDigit))).ToLower().Contains(phone))));
                  continue;
                }
                continue;
              case "Email":
                source1.AddRange(list.Where<Client>((Func<Client, bool>) (x => x.Email.ToLower().Contains(filterText))));
                continue;
              default:
                continue;
            }
          }
        }
        this.ClientsList = new ObservableCollection<Client>((IEnumerable<Client>) source1.Distinct<Client>().OrderBy<Client, string>((Func<Client, string>) (x => x.Name)));
        ClientCloud clientCloud = ClientsExchangeHelper.Search(filterText);
        if (clientCloud == null || this.ClientsList.Any<Client>())
          return;
        Client client = ClientsExchangeHelper.SaveClient(clientCloud);
        if (client == null)
          return;
        this.SelectedClient = client;
        this.ResultAction = true;
        Application.Current.Dispatcher.Invoke((Action) (() => this.CloseFrm()));
      }
    }

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
  }
}
