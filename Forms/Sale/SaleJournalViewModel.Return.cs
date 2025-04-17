// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SaleJournalViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents.Sales;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Goods;
using Gbs.Forms.Sale;
using Gbs.Forms.Sale.Return;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms
{
  public class SaleJournalViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodsSearchModelView.FilterProperty> _filterProperties;
    private List<SaleJournalViewModel.SaleItemsInfoGrid> _selectedList;
    public ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> _storageListFilter;
    private bool _isCancelThread;
    private ViewSaleJournal _viewSaleJournal;
    private bool _isFilterForClient;
    private List<Document> _listDocuments;
    private Decimal _totalGoodsStock;
    private Decimal _totalSaleCount;
    private Decimal _totalSaleName;
    private Decimal _totalSaleSum;
    private Guid _clientUidByCache;
    private List<SaleJournalViewModel.SaleItemsInfoGrid> _cachedDbGoods;
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private string _filterTextGroups;
    private Guid _sectionSelectedUid;
    private Client _client;
    private bool _isEnabledClient;
    private Gbs.Core.Entities.Users.User _selectedUser;
    private ObservableCollection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>> _paymentMethodList;
    private DateTime _valueDateTimeStart;
    private DateTime _valueDateTimeEnd;
    private string _filterSales;
    private SaleJournalViewModel.ExtraOption _selectedExtraOption;
    private string _selectedTypePayment;

    private void DoReturn(object obj)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        List<SaleJournalViewModel.SaleItemsInfoGrid> list = ((IEnumerable) obj).Cast<SaleJournalViewModel.SaleItemsInfoGrid>().ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (!list.Any<SaleJournalViewModel.SaleItemsInfoGrid>())
          throw new ErrorHelper.GbsException(Translate.SaleJournalViewModel_Необходимо_выбрать_продажу_для_возврата)
          {
            Level = MsgException.LevelTypes.Warm
          };
        if (list.Any<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Uid != list.First<SaleJournalViewModel.SaleItemsInfoGrid>().Document.Uid)))
          throw new ErrorHelper.GbsException(Translate.SaleJournalViewModel_Невозможно_оформить_возврат_на_товары_из_разных_продаж)
          {
            Level = MsgException.LevelTypes.Warm
          };
        (List<BasketItem> list, bool result) tuple = new FrmReturnSales().ShowReturns(this.SelectedSale.Document, this.AuthUser);
        if (!tuple.result)
          return;
        Gbs.Core.Config.Sales sales = new ConfigsRepository<Settings>().Get().Sales;
        foreach (BasketItem basketItem1 in tuple.list)
        {
          BasketItem item = basketItem1;
          int index = this.Sale.SaleItemsList.ToList<SaleJournalViewModel.SaleItemsInfoGrid>().FindIndex((Predicate<SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.Item.Uid == item.Uid));
          if (index != -1)
          {
            SaleJournalViewModel.SaleItemsInfoGrid saleItems = this.Sale.SaleItemsList[index];
            saleItems.Color = Color.Red.Name;
            if (sales.IsMinusForReturnInSale)
            {
              BasketItem basketItem2 = saleItems.Item;
              basketItem2.Quantity = basketItem2.Quantity - item.Quantity;
            }
          }
        }
        if (!sales.IsMinusForReturnInSale)
          return;
        this.SetCountItems();
      }
    }

    public ICommand ReloadData
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Entities.Documents.Cache.ClearAndLoadCache();
          this.GetSalesList();
        }));
      }
    }

    private void LoadingProperty(FilterOptions setting)
    {
      ObservableCollection<GoodsSearchModelView.FilterProperty> collection = new ObservableCollection<GoodsSearchModelView.FilterProperty>();
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Name",
        Text = Translate.HandbookGoodSettingViewModel_Название_товара,
        IsChecked = setting.SaleJournalSearch.GoodProp.IsCheckedName
      });
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Barcode",
        Text = Translate.FrmAuthorization_ШтрихКод,
        IsChecked = setting.SaleJournalSearch.GoodProp.IsCheckedBarcode
      });
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Barcodes",
        Text = Translate.ExcelDataViewModel_Доп__штрих_коды,
        IsChecked = setting.SaleJournalSearch.GoodProp.IsCheckedDescription
      });
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Sum",
        Text = this._viewSaleJournal == ViewSaleJournal.ListSale ? Translate.SaleJournalViewModel_LoadingProperty_Сумма_чека : Translate.AnaliticSettingViewModel_Стоимость_товара,
        IsChecked = setting.SaleJournalSearch.IsSum
      });
      collection.Add(new GoodsSearchModelView.FilterProperty()
      {
        Name = "Number",
        Text = Translate.SaleJournalViewModel_LoadingProperty_Номер_чека,
        IsChecked = setting.SaleJournalSearch.IsNumberCheck
      });
      this.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>((IEnumerable<GoodsSearchModelView.FilterProperty>) collection);
    }

    public Timer TimerSearch { get; }

    public string TextSearchButton
    {
      get
      {
        int num = this.FilterProperties.Count<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked));
        if (num == this.FilterProperties.Count<GoodsSearchModelView.FilterProperty>())
          return Translate.GoodsSearchModelView_Все_поля;
        return num != 1 ? Translate.GoodsSearchModelView_Полей__ + num.ToString() : this.FilterProperties.First<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)).Text;
      }
    }

    public FilterOptions Setting { get; set; }

    public ObservableCollection<GoodsSearchModelView.FilterProperty> FilterProperties
    {
      get => this._filterProperties;
      set
      {
        this._filterProperties = value;
        new ConfigsRepository<FilterOptions>().Save(this.Setting);
        this.OnPropertyChanged("TextSearchButton");
        if (value.Any<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
          return;
        MessageBoxHelper.Warning(Translate.GoodsSearchModelView_Нет_выбранных_полей__по_которым_происходит_поиск_);
      }
    }

    public ICommand CopySaleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this._selectedList.Any<SaleJournalViewModel.SaleItemsInfoGrid>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_CopySaleCommand_Необходимо_выбрать_продажу_для_копирования_, icon: MessageBoxImage.Exclamation);
          }
          else if (this._selectedList.Any<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Uid != this._selectedList.First<SaleJournalViewModel.SaleItemsInfoGrid>().Document.Uid)))
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_CopySaleCommand_Необходимо_выбрать_только_одну_продажу_для_копирования_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            Gbs.Core.ViewModels.Basket.Basket currentBasket = ((MainWindowViewModel) (Application.Current.Windows.OfType<MainWindow>().SingleOrDefault<MainWindow>() ?? throw new NullReferenceException(Translate.ClientOrderViewModel_SaveCommand_Не_удалось_найти_экземпляр_главного_окна)).DataContext).CurrentBasket;
            if (currentBasket.Items.Any<BasketItem>() && MessageBoxHelper.Show(Translate.SaleJournalViewModel_CopySaleCommand_В_чеке_уже_есть_товары__уверены__что_хотите_скопировать_продажу__В_этом_случае_корзина_будет_очищена_и_добавлены_товары_из_выбранной_продажи_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              Gbs.Core.ViewModels.Basket.Basket basket = new Gbs.Core.ViewModels.Basket.Basket()
              {
                Client = this.SelectedSale.Document.ContractorUid != Guid.Empty ? new ClientsRepository(dataBase).GetClientByUidAndSum(this.SelectedSale.Client.Uid) : (ClientAdnSum) null,
                Items = new ObservableCollection<BasketItem>(),
                Storage = this.SelectedSale.Document.Storage.Clone<Storages.Storage>()
              };
              List<GoodsModifications.GoodModification> modificationsList = GoodsModifications.GetModificationsList();
              foreach (Gbs.Core.Entities.Documents.Item obj1 in this.SelectedSale.Document.Items)
              {
                Gbs.Core.Entities.Documents.Item documentItem = obj1;
                BasketItem basketItem = new BasketItem()
                {
                  Good = documentItem.Good.Clone<Gbs.Core.Entities.Goods.Good>(),
                  BasePrice = documentItem.BaseSalePrice,
                  Comment = documentItem.Comment,
                  GoodModification = modificationsList.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == documentItem.ModificationUid)),
                  SalePrice = documentItem.SellPrice,
                  Quantity = documentItem.Quantity,
                  Storage = this.SelectedSale.Document.Storage,
                  IsDeleted = documentItem.IsDeleted
                };
                basketItem.Discount.SetDiscount(documentItem.Discount, SaleDiscount.ReasonEnum.UserEdit, basketItem, (Gbs.Core.ViewModels.Basket.Basket) null);
                basket.Items.Add(basketItem);
              }
              BasketHelper basketHelper = new BasketHelper(currentBasket);
              currentBasket.Storage = basket.Storage;
              currentBasket.Client = basket.Client.Clone<ClientAdnSum>();
              basketHelper.AddItemCopySale(basket.Items.ToList<BasketItem>(), basket.Client?.Client);
              currentBasket.ReCalcTotals();
              Gbs.Helpers.Other.IsActiveAndShowForm<MainWindow>();
            }
          }
        }));
      }
    }

    public static string MoreMenuKey => "MoreMenu";

    public Action ShowMoreMenu { get; set; }

    public ICommand ShowMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._selectedList = ((IEnumerable) obj).Cast<SaleJournalViewModel.SaleItemsInfoGrid>().ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
          this.ShowMoreMenu();
        }));
      }
    }

    public string TextPropButton
    {
      get
      {
        int num = this.PaymentMethodList.Count<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => x.IsChecked));
        if (num == this.PaymentMethodList.Count<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>())
          return Translate.SaleJournalViewModel_TextPropButton_Все_способы;
        return num != 1 ? string.Format(Translate.SaleJournalViewModel_TextPropButton_Способов___0_, (object) num) : this.PaymentMethodList.First<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => x.IsChecked)).Item.Name;
      }
    }

    private IEnumerable<Storages.Storage> AllListStorage { get; }

    public string ButtonContentStorage
    {
      get
      {
        int num = this._storageListFilter.Count<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked));
        if (num == this.AllListStorage.Count<Storages.Storage>())
          return Translate.WaybillsViewModel_Все_склады;
        return num != 1 ? Translate.WaybillsViewModel_Складов_ + num.ToString() : this._storageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked)).Item.Name;
      }
    }

    public ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> StorageListFilter
    {
      get => this._storageListFilter;
      set
      {
        this._storageListFilter = value;
        this.OnPropertyChanged(nameof (StorageListFilter));
        this.OnPropertyChanged("ButtonContentStorage");
        this.SearchForFilter();
      }
    }

    public ICommand PrintSaleOrder
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          PrintableReportFactory printableReportFactory = new PrintableReportFactory();
          List<SaleJournalViewModel.SaleItemsInfoGrid> list = this.Sale.SaleItemsList.ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
          ClientAdnSum client = new ClientAdnSum();
          client.Client = this.Client;
          DateTime valueDateTimeStart = this.ValueDateTimeStart;
          DateTime valueDateTimeEnd = this.ValueDateTimeEnd;
          new FastReportFacade().SelectTemplateAndShowReport(printableReportFactory.CreateForSaleOrder((IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) list, client, valueDateTimeStart, valueDateTimeEnd), this.AuthUser);
        }));
      }
    }

    public Gbs.Core.ViewModels.Sale.Sale Sale { get; set; }

    public static string PrintMenuKey => nameof (PrintMenuKey);

    public SaleJournalViewModel.SaleItemsInfoGrid SelectedSale { get; set; }

    public ICommand ShowSaleCard { get; set; }

    public ICommand ReturnCommand { get; set; }

    public ICommand DeleteSales { get; set; }

    public ICommand GetClient { get; set; }

    public ICommand PrintCommand { get; set; }

    public ICommand PrintDocument { get; set; }

    public ICommand PrintCheckCommand { get; set; }

    private Action ShowMenuAction { get; set; }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public SaleJournalViewModel()
    {
      List<Sections.Section> sectionList = new List<Sections.Section>();
      Sections.Section section = new Sections.Section();
      section.Uid = Guid.Empty;
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      sectionList.Add(section);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListSections\u003Ek__BackingField = sectionList;
      this._sectionSelectedUid = Guid.Empty;
      Gbs.Core.Entities.Users.User user1 = new Gbs.Core.Entities.Users.User();
      user1.Alias = Translate.SaleJournalViewModel__selectedUser_Все_сотрудники;
      user1.Uid = Guid.Parse("11111111-1111-1111-1111-111111111111");
      this._selectedUser = user1;
      this._paymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>();
      this._valueDateTimeStart = DateTime.Now.Date;
      this._valueDateTimeEnd = DateTime.Now.Date;
      // ISSUE: reference to a compiler-generated field
      this.\u003CListTypePayment\u003Ek__BackingField = new List<string>()
      {
        Translate.SaleJournalViewModel_ListTypePayment_Все,
        Translate.SaleJournalViewModel_ListTypePayment_Фискально,
        Translate.SaleJournalViewModel_ListTypePayment_Нефискально
      };
      // ISSUE: reference to a compiler-generated field
      this.\u003CListExtraOption\u003Ek__BackingField = new Dictionary<SaleJournalViewModel.ExtraOption, string>()
      {
        {
          SaleJournalViewModel.ExtraOption.None,
          Translate.SaleJournalViewModel_ListExtraOption_Все_статусы_оплаты
        },
        {
          SaleJournalViewModel.ExtraOption.OnlyCredit,
          Translate.SaleJournalViewModel_ListExtraOption_Неоплаченные
        },
        {
          SaleJournalViewModel.ExtraOption.CreditAndPayment,
          Translate.SaleJournalViewModel_ListExtraOption_Оплаченные_частично
        },
        {
          SaleJournalViewModel.ExtraOption.OnlyFullPayment,
          Translate.SaleJournalViewModel_ListExtraOption_Оплаченные_полностью
        },
        {
          SaleJournalViewModel.ExtraOption.Credit,
          Translate.SaleJournalViewModel_Полностью_или_частично_неоплаченные
        }
      };
      this._selectedTypePayment = Translate.SaleJournalViewModel_ListTypePayment_Все;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
        this._storageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(this.AllListStorage.Select<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
        {
          IsChecked = true,
          Item = x
        })));
        this.ListSections.AddRange(Sections.GetSectionsList().Where<Sections.Section>((Func<Sections.Section, bool>) (x => !x.IsDeleted)));
        List<Gbs.Core.Entities.Users.User> userList = new List<Gbs.Core.Entities.Users.User>();
        Gbs.Core.Entities.Users.User user2 = new Gbs.Core.Entities.Users.User();
        user2.Alias = Translate.SaleJournalViewModel__selectedUser_Все_сотрудники;
        user2.Uid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        userList.Add(user2);
        Gbs.Core.Entities.Users.User user3 = new Gbs.Core.Entities.Users.User();
        user3.Alias = Translate.PaymentsActionsViewModel_Не_указан;
        user3.Uid = Guid.Empty;
        userList.Add(user3);
        this.UsersList = userList;
        this.UsersList.AddRange((IEnumerable<Gbs.Core.Entities.Users.User>) new UsersRepository(dataBase).GetByQuery(dataBase.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED && !x.IS_KICKED))));
        List<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>> list1 = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => !x.IS_DELETED))).Select<PaymentMethods.PaymentMethod, SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<PaymentMethods.PaymentMethod, SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>) (x => new SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>()
        {
          Item = x
        })).ToList<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>();
        List<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>> list2 = list1.Where<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => !x.Item.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid))).OrderBy<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, int>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, int>) (x => x.Item.DisplayIndex)).ToList<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>();
        list2.AddRange(list1.Where<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => x.Item.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid))));
        this.PaymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>(list2);
      }
      this.LoadJournalCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.GetSalesList()));
      this.ShowSaleCard = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        List<SaleJournalViewModel.SaleItemsInfoGrid> list = ((IEnumerable) obj).Cast<SaleJournalViewModel.SaleItemsInfoGrid>().ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (!list.Any<SaleJournalViewModel.SaleItemsInfoGrid>())
          MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись_для_просмотра_карточки_продажи);
        else
          new FrmCardSale().ShowSaleCard(list.First<SaleJournalViewModel.SaleItemsInfoGrid>().Document);
      }));
      this.ReturnCommand = (ICommand) new RelayCommand(new Action<object>(this.DoReturn));
      this.DeleteSales = (ICommand) new RelayCommand(new Action<object>(this.DeleteSale));
      this.GetClient = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (Client client, bool result) client1 = new FrmSearchClient().GetClient();
        Client client2 = client1.client;
        if (!client1.result)
          return;
        this.Client = client2;
      }));
      this.PrintDocument = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedSale == null)
          return;
        PrintableReportFactory printableReportFactory = new PrintableReportFactory();
        Document document1 = this.SelectedSale.Document.Clone<Document>();
        document1.Items = new List<Gbs.Core.Entities.Documents.Item>(document1.Items.GroupBy(x => new
        {
          GoodUid = x.GoodUid,
          SellPrice = x.SellPrice,
          ModificationUid = x.ModificationUid,
          Discount = x.Discount,
          Comment = x.Comment
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType37<Guid, Decimal, Guid, Decimal, string>, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Documents.Item>(x =>
        {
          return new Gbs.Core.Entities.Documents.Item()
          {
            Uid = x.First<Gbs.Core.Entities.Documents.Item>().Uid,
            Good = x.First<Gbs.Core.Entities.Documents.Item>().Good,
            GoodStock = x.First<Gbs.Core.Entities.Documents.Item>().GoodStock,
            Quantity = x.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (g => g.Quantity)),
            Comment = x.Key.Comment,
            BaseSalePrice = x.First<Gbs.Core.Entities.Documents.Item>().BaseSalePrice,
            BuyPrice = x.First<Gbs.Core.Entities.Documents.Item>().BuyPrice,
            Discount = x.Key.Discount,
            DocumentUid = x.First<Gbs.Core.Entities.Documents.Item>().DocumentUid,
            ModificationUid = x.Key.ModificationUid,
            SellPrice = x.Key.SellPrice
          };
        }));
        Document document2 = document1;
        new FastReportFacade().SelectTemplateAndShowReport(printableReportFactory.CreateForSaleDocument(document2), this.AuthUser);
      }));
      this.PrintCheckCommand = (ICommand) new RelayCommand((Action<object>) (obj => SaleHelper.PrintNoFiscalCheck(this.SelectedSale.Document)));
      this.PrintCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        List<SaleJournalViewModel.SaleItemsInfoGrid> list = ((IEnumerable) obj).Cast<SaleJournalViewModel.SaleItemsInfoGrid>().ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (!list.Any<SaleJournalViewModel.SaleItemsInfoGrid>())
          MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
        else if (list.GroupBy<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Document.Uid)).ToList<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>>().Count<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>>() > 1)
        {
          MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
        }
        else
        {
          this.SelectedSale = list.First<SaleJournalViewModel.SaleItemsInfoGrid>();
          Action showMenuAction = this.ShowMenuAction;
          if (showMenuAction == null)
            return;
          showMenuAction();
        }
      }));
    }

    private void DeleteSale(object obj)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteSale) && !new Authorization().GetAccess(Actions.DeleteSale).Result)
            return;
          IList source = (IList) obj;
          int count = source.Count;
          if (count > 1)
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Можно_выбрать_только_одну_запись);
          else if (count < 1)
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            Document document = source.Cast<SaleJournalViewModel.SaleItemsInfoGrid>().First<SaleJournalViewModel.SaleItemsInfoGrid>().Document;
            if (document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)))
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_DeleteSale_Уделение_продаж__в_которых_есть_товары_типа__комплект__не_поддерживается_в_этой_версии_программы_, icon: MessageBoxImage.Exclamation);
            }
            else
            {
              if (MessageBoxHelper.Question(Translate.SaleJournalViewModel_УвереныЧтоХОтитеУДалитьВыбраннуюПродажу + (document.IsFiscal ? "\n\n" + Translate.SaleJournalViewModel_DeleteSale_ОбратитеВниманиеПродажаФискализирована : "")) != MessageBoxResult.Yes)
                return;
              Document oldItem = document.Clone<Document>();
              if (!new DocumentsRepository(dataBase).Delete(document))
                return;
              document.IsDeleted = true;
              new BonusHelper().UpdateSumBonusesForReturn(document, (List<BasketItem>) null);
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) document, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
              this.GetSalesList();
            }
          }
        }
      }
    }

    public void ShowCard(
      DateTime start = default (DateTime),
      DateTime finish = default (DateTime),
      bool isVisibilityIncome = false,
      Client client = null,
      Gbs.Core.Entities.Users.User user = null,
      Guid uidForm = default (Guid),
      bool isShowCurrentSection = false)
    {
      if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmMagazineSale>(uidForm.ToString()))
        return;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref user, Actions.ShowJournalSale))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ShowJournalSale);
          if (!access.Result)
            return;
          user = access.User;
        }
        this._viewSaleJournal = new ConfigsRepository<Settings>().Get().Interface.ViewSaleJournal;
        this.Setting = new ConfigsRepository<FilterOptions>().Get();
        this.LoadingProperty(this.Setting);
        this.TimerSearch.Elapsed += new ElapsedEventHandler(this.TimerSearchOnElapsed);
        this.AuthUser = user;
        this._valueDateTimeStart = start == new DateTime() ? DateTime.Now : start;
        this._valueDateTimeEnd = finish == new DateTime() ? start : finish;
        if (client != null)
        {
          this._isEnabledClient = true;
          this._client = client;
        }
        this.IsVisibilityIncome = isVisibilityIncome;
        if (isShowCurrentSection)
          this._sectionSelectedUid = Sections.GetCurrentSection().Uid;
        FrmMagazineSale frmMagazineSale = new FrmMagazineSale(this);
        frmMagazineSale.Uid = uidForm.ToString();
        this.FormToSHow = (WindowWithSize) frmMagazineSale;
        this.OnPropertyChanged("SectionSelectedUid");
        ((FrmMagazineSale) this.FormToSHow).ClientSelectionControl.Client = this.Client;
        ((FrmMagazineSale) this.FormToSHow).ClientSelectionControl.IsCheckedClient = this.Client != null;
        this.ShowMenuAction = new Action(((FrmMagazineSale) this.FormToSHow).ShowPrintMenu);
        this.ShowMoreMenu = new Action(((FrmMagazineSale) this.FormToSHow).ShowMoreMenu);
        this._isFilterForClient = true;
        if (!isVisibilityIncome)
        {
          ((FrmMagazineSale) this.FormToSHow).ListSaleItems.Columns.Remove(((FrmMagazineSale) this.FormToSHow).ListSaleItems.Columns.First<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "A7AE73E1-A03B-4596-991A-828025773FAC")));
          ((FrmMagazineSale) this.FormToSHow).ListSaleItems.Columns.Remove(((FrmMagazineSale) this.FormToSHow).ListSaleItems.Columns.First<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "78384F7C-8458-4ED4-97F8-0D6A923DD9A7")));
        }
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.SaleJournalViewModel_ShowCard_Открыт_журнал_продаж, this.AuthUser), true);
        this.ShowForm(false);
        this._isCancelThread = true;
      }
    }

    private void TimerSearchOnElapsed(object sender, ElapsedEventArgs e)
    {
      this.TimerSearch.Stop();
      try
      {
        this.SearchForFilter();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка фильтрации журнала продаж");
      }
    }

    public Decimal TotalSaleItemsCount
    {
      get => this._totalGoodsStock;
      set
      {
        this._totalGoodsStock = value;
        this.OnPropertyChanged(nameof (TotalSaleItemsCount));
      }
    }

    public Decimal TotalSaleCount
    {
      get => this._totalSaleCount;
      set
      {
        this._totalSaleCount = value;
        this.OnPropertyChanged(nameof (TotalSaleCount));
      }
    }

    public Decimal TotalSaleName
    {
      get => this._totalSaleName;
      set
      {
        this._totalSaleName = value;
        this.OnPropertyChanged(nameof (TotalSaleName));
      }
    }

    public Decimal TotalSaleSum
    {
      get => this._totalSaleSum;
      set
      {
        this._totalSaleSum = value;
        this.OnPropertyChanged(nameof (TotalSaleSum));
      }
    }

    public Decimal TotalIncome
    {
      get
      {
        Gbs.Core.ViewModels.Sale.Sale sale = this.Sale;
        Decimal? nullable;
        if (sale == null)
        {
          nullable = new Decimal?();
        }
        else
        {
          AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid> saleItemsList = sale.SaleItemsList;
          nullable = saleItemsList != null ? saleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal?>) (x => x.SumIncome)) : new Decimal?();
        }
        return nullable.GetValueOrDefault();
      }
    }

    private bool IsVisibilityIncome { get; set; }

    public Visibility VisibilityIncome
    {
      get => !this.IsVisibilityIncome ? Visibility.Collapsed : Visibility.Visible;
    }

    public ICommand LoadJournalCommand { get; set; }

    private List<Document> _setChildList { get; set; }

    private void GetSalesList()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this._isCancelThread = true;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SaleJournalViewModel_GetSalesList_Загрузка_данных_о_продажах);
      Performancer per = new Performancer("Загрузка журнала продаж");
      this.CachedDbGoods = new List<SaleJournalViewModel.SaleItemsInfoGrid>();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        DocumentsRepository documentsRepository1 = new DocumentsRepository(dataBase);
        Client client1 = this.Client;
        // ISSUE: explicit non-virtual call
        this._clientUidByCache = client1 != null ? __nonvirtual (client1.Uid) : Guid.Empty;
        DocumentsRepository documentsRepository2 = documentsRepository1;
        DocumentsRepository.CommonFilter commonFilter1 = new DocumentsRepository.CommonFilter();
        commonFilter1.DateStart = this.ValueDateTimeStart;
        commonFilter1.DateEnd = this.ValueDateTimeEnd;
        commonFilter1.IncludeDeleted = false;
        commonFilter1.IgnoreTime = true;
        DocumentsRepository.CommonFilter commonFilter2 = commonFilter1;
        Client client2 = this.Client;
        // ISSUE: explicit non-virtual call
        Guid guid1 = client2 != null ? __nonvirtual (client2.Uid) : Guid.Empty;
        commonFilter2.ContractorUid = guid1;
        commonFilter1.Types = new GlobalDictionaries.DocumentsTypes[1]
        {
          GlobalDictionaries.DocumentsTypes.Sale
        };
        DocumentsRepository.CommonFilter commonFilter3 = commonFilter1;
        this._listDocuments = documentsRepository2.GetItemsWithFilter((DocumentsRepository.IFilter) commonFilter3);
        DocumentsRepository documentsRepository3 = documentsRepository1;
        DocumentsRepository.CommonFilter commonFilter4 = new DocumentsRepository.CommonFilter();
        commonFilter4.DateStart = this.ValueDateTimeStart;
        commonFilter4.DateEnd = this.ValueDateTimeEnd;
        commonFilter4.IncludeDeleted = false;
        commonFilter4.IgnoreTime = true;
        DocumentsRepository.CommonFilter commonFilter5 = commonFilter4;
        Client client3 = this.Client;
        // ISSUE: explicit non-virtual call
        Guid guid2 = client3 != null ? __nonvirtual (client3.Uid) : Guid.Empty;
        commonFilter5.ContractorUid = guid2;
        commonFilter4.Types = new GlobalDictionaries.DocumentsTypes[1]
        {
          GlobalDictionaries.DocumentsTypes.SetChildStockChange
        };
        DocumentsRepository.CommonFilter commonFilter6 = commonFilter4;
        this._setChildList = documentsRepository3.GetItemsWithFilter((DocumentsRepository.IFilter) commonFilter6);
        List<Document> setChildList = this._setChildList;
        DocumentsRepository documentsRepository4 = documentsRepository1;
        DocumentsRepository.CommonFilter commonFilter7 = new DocumentsRepository.CommonFilter();
        commonFilter7.DateStart = DateTime.MinValue;
        commonFilter7.DateEnd = DateTime.Now;
        commonFilter7.IgnoreTime = true;
        DocumentsRepository.CommonFilter commonFilter8 = commonFilter7;
        Client client4 = this.Client;
        // ISSUE: explicit non-virtual call
        Guid guid3 = client4 != null ? __nonvirtual (client4.Uid) : Guid.Empty;
        commonFilter8.ContractorUid = guid3;
        commonFilter7.Types = new GlobalDictionaries.DocumentsTypes[5]
        {
          GlobalDictionaries.DocumentsTypes.ProductionSet,
          GlobalDictionaries.DocumentsTypes.ProductionItem,
          GlobalDictionaries.DocumentsTypes.BeerProductionSet,
          GlobalDictionaries.DocumentsTypes.BeerProductionItem,
          GlobalDictionaries.DocumentsTypes.MoveStorageChild
        };
        DocumentsRepository.CommonFilter commonFilter9 = commonFilter7;
        List<Document> itemsWithFilter = documentsRepository4.GetItemsWithFilter((DocumentsRepository.IFilter) commonFilter9);
        setChildList.AddRange((IEnumerable<Document>) itemsWithFilter);
        per.AddPoint("Документы продаж загружены");
        if (!this._listDocuments.Any<Document>())
        {
          this.Sale.SaleItemsList = new AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid>();
          this.SetCountItems();
          per.Stop();
          progressBar.Close();
        }
        else
        {
          SaleJournalViewModel.ReturnSales = documentsRepository1.GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
          {
            DateStart = this.ValueDateTimeStart,
            DateEnd = DateTime.Now,
            IgnoreTime = true,
            Types = new GlobalDictionaries.DocumentsTypes[1]
            {
              GlobalDictionaries.DocumentsTypes.SaleReturn
            }
          });
          per.AddPoint("Документы возвратов загружены");
          try
          {
            this.LoadData(per);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "Ошибка загрузки данных журнала продаж");
          }
          progressBar.Close();
          stopwatch.Stop();
          per.Stop();
        }
      }
    }

    private void LoadData(Performancer per)
    {
      Gbs.Core.Config.Sales sales = new ConfigsRepository<Settings>().Get().Sales;
      List<\u003C\u003Ef__AnonymousType38<Document, IEnumerable<IGrouping<(Guid, Decimal, string, Decimal, Guid), Gbs.Core.Entities.Documents.Item>>>> list1 = this._listDocuments.AsParallel<Document>().Select(item => new
      {
        Document = item,
        GroupedItems = item.Items.GroupBy<Gbs.Core.Entities.Documents.Item, (Guid, Decimal, string, Decimal, Guid)>((Func<Gbs.Core.Entities.Documents.Item, (Guid, Decimal, string, Decimal, Guid)>) (x => (x.ModificationUid, x.Discount, x.Comment, x.SellPrice, x.GoodUid)))
      }).ToList();
      per.AddPoint("step 10");
      bool _isMinusForReturn = sales.IsMinusForReturnInSale;
      List<\u003C\u003Ef__AnonymousType39<Document, IGrouping<(Guid, Decimal, string, Decimal, Guid), Gbs.Core.Entities.Documents.Item>>> list2 = list1.AsParallel().SelectMany(doc => doc.GroupedItems.Select(group => new
      {
        Document = doc.Document,
        group = group
      })).ToList();
      per.AddPoint("step 15");
      List<SaleJournalViewModel.SaleItemsInfoGrid> list3 = list2.AsParallel().Select(item => new SaleJournalViewModel.SaleItemsInfoGrid(item.Document, (IEnumerable<Gbs.Core.Entities.Documents.Item>) item.group, _isMinusForReturn, this._viewSaleJournal)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
      per.AddPoint("step 20");
      this.CachedDbGoods.AddRange((IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) list3);
      per.AddPoint("Добавление записей в CachedDbGoods");
      if (this._viewSaleJournal == ViewSaleJournal.ListSale)
      {
        List<SaleJournalViewModel.SaleItemsInfoGrid> collection = new List<SaleJournalViewModel.SaleItemsInfoGrid>();
        foreach (IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid> grouping in (IEnumerable<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>>) this.CachedDbGoods.ToLookup<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Document.Uid)))
        {
          IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid> doc = grouping;
          Decimal num = SaleJournalViewModel.ReturnSales.Where<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Key)).SelectMany<Document, Gbs.Core.Entities.Payments.Payment>((Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (x => (IEnumerable<Gbs.Core.Entities.Payments.Payment>) x.Payments)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
          SaleJournalViewModel.SaleItemsInfoGrid saleItemsInfoGrid = doc.First<SaleJournalViewModel.SaleItemsInfoGrid>();
          saleItemsInfoGrid.SumCountItemReturn = doc.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.SumCountItemReturn));
          saleItemsInfoGrid.SumDiscountReturn = doc.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.SumDiscountReturn)) + num;
          saleItemsInfoGrid.SumItemReturn = doc.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.SumItemReturn)) - num;
          collection.Add(saleItemsInfoGrid);
        }
        this.CachedDbGoods = new List<SaleJournalViewModel.SaleItemsInfoGrid>((IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) collection);
      }
      per.AddPoint("Цикл");
      this.Sale.SaleItemsList = new AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid>(this.CachedDbGoods.OrderBy<SaleJournalViewModel.SaleItemsInfoGrid, DateTime>((Func<SaleJournalViewModel.SaleItemsInfoGrid, DateTime>) (x => x.Document.DateTime)).Reverse<SaleJournalViewModel.SaleItemsInfoGrid>());
      per.AddPoint("Сортировка");
      this.SetCountItems();
      per.AddPoint("Подсчет количетва");
      this.SearchForFilter();
      per.AddPoint("Фильтрация");
      ProgressBarHelper.Close();
      this._isCancelThread = false;
      Task.Run((Action) (() =>
      {
        try
        {
          this.CountIncome();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка расчета прибыли");
        }
      }));
      Task.Run((Action) (() =>
      {
        try
        {
          this.SetClientsAndUsers();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка установки контактов и сотрудников");
        }
      }));
    }

    private void SetClientsAndUsers()
    {
      if (this._isCancelThread)
        return;
      using (Data.GetDataBase())
      {
        List<Client> clientList = CachesBox.AllClients();
        List<Gbs.Core.Entities.Users.User> userList = CachesBox.AllUsers();
        foreach (SaleJournalViewModel.SaleItemsInfoGrid saleItemsInfoGrid in this.CachedDbGoods.AsParallel<SaleJournalViewModel.SaleItemsInfoGrid>())
        {
          SaleJournalViewModel.SaleItemsInfoGrid item = saleItemsInfoGrid;
          item.Client = clientList.Find((Predicate<Client>) (x => x.Uid == item.Document.ContractorUid));
          item.User = userList.Find((Predicate<Gbs.Core.Entities.Users.User>) (x => x.Uid == item.Document.UserUid));
        }
      }
    }

    private void CountIncome()
    {
      if (!this.IsVisibilityIncome)
        return;
      if (this._isCancelThread)
        return;
      Gbs.Core.Config.Sales config = new ConfigsRepository<Settings>().Get().Sales;
      BuyPriceCounter buyPriceCounter = new BuyPriceCounter();
      foreach (Document listDocument in this._listDocuments)
      {
        Document document = listDocument;
        foreach (IGrouping<\u003C\u003Ef__AnonymousType40<Guid, Decimal, string, Decimal, Guid>, Gbs.Core.Entities.Documents.Item> grouping in document.Items.GroupBy(x => new
        {
          ModificationUid = x.ModificationUid,
          Discount = x.Discount,
          Comment = x.Comment,
          SellPrice = x.SellPrice,
          GoodUid = x.GoodUid
        }))
        {
          IGrouping<\u003C\u003Ef__AnonymousType40<Guid, Decimal, string, Decimal, Guid>, Gbs.Core.Entities.Documents.Item> docItem = grouping;
          Gbs.Core.Entities.Documents.Item docItemFirst = docItem.First<Gbs.Core.Entities.Documents.Item>();
          List<Gbs.Core.Entities.Documents.Item> itemsReturn = SaleJournalViewModel.ReturnSales.Where<Document>((Func<Document, bool>) (x => x.ParentUid == docItem.First<Gbs.Core.Entities.Documents.Item>().DocumentUid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.ModificationUid == docItemFirst.ModificationUid && i.SellPrice == docItemFirst.SellPrice && i.GoodUid == docItemFirst.GoodUid)).ToList<Gbs.Core.Entities.Documents.Item>();
          List<Document> allDocs = new List<Document>();
          allDocs.AddRange((IEnumerable<Document>) this._listDocuments);
          allDocs.AddRange((IEnumerable<Document>) this._setChildList);
          Decimal num1 = docItem.Select<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Documents.Item>) (x => x)).Sum<Gbs.Core.Entities.Documents.Item>(new Func<Gbs.Core.Entities.Documents.Item, Decimal>(GetIncome));
          int index = this._viewSaleJournal != ViewSaleJournal.ListGood ? this.CachedDbGoods.FindIndex((Predicate<SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.Document.Uid == document.Uid)) : this.CachedDbGoods.FindIndex((Predicate<SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.Item.Uid == docItemFirst.Uid));
          if (index != -1)
          {
            SaleJournalViewModel.SaleItemsInfoGrid cachedDbGood1 = this.CachedDbGoods[index];
            Decimal? sumIncome = cachedDbGood1.SumIncome;
            sumIncome.GetValueOrDefault();
            if (!sumIncome.HasValue)
            {
              Decimal num2 = 0M;
              Decimal? nullable;
              cachedDbGood1.SumIncome = nullable = new Decimal?(num2);
            }
            SaleJournalViewModel.SaleItemsInfoGrid cachedDbGood2 = this.CachedDbGoods[index];
            sumIncome = cachedDbGood2.SumIncome;
            Decimal num3 = num1;
            cachedDbGood2.SumIncome = sumIncome.HasValue ? new Decimal?(sumIncome.GetValueOrDefault() + num3) : new Decimal?();
          }

          Decimal GetIncome(Gbs.Core.Entities.Documents.Item x)
          {
            if (x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            {
              Decimal num = config.IsMinusForReturnInSale ? itemsReturn.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (r => x.GoodUid == r.GoodUid && x.SellPrice == r.SellPrice)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (r => r.Quantity)) : 0M;
              return ItemsTotalSumCalculator.SumForItem(x.Quantity - num, x.SellPrice, x.Discount);
            }
            if (x.GoodStock == null && x.Good.SetStatus != GlobalDictionaries.GoodsSetStatuses.Set)
              return x.SellPrice * x.Quantity * (100.0M - x.Discount) / 100.0M;
            Decimal returnQuantity = config.IsMinusForReturnInSale ? itemsReturn.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (r =>
            {
              Guid? uid1 = x.GoodStock?.Uid;
              Guid? uid2 = r.GoodStock?.Uid;
              if (uid1.HasValue != uid2.HasValue)
                return false;
              return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
            })).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (r => r.Quantity)) : 0M;
            return SaleHelper.GetSumIncomeItemInDocument(x, (IEnumerable<Document>) allDocs, buyPriceCounter, returnQuantity);
          }
        }
        this.OnPropertyChanged("TotalIncome");
      }
    }

    private List<SaleJournalViewModel.SaleItemsInfoGrid> CachedDbGoods
    {
      get => this._cachedDbGoods;
      set
      {
        this._cachedDbGoods = value.Distinct<SaleJournalViewModel.SaleItemsInfoGrid>().ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        this.OnPropertyChanged(nameof (CachedDbGoods));
      }
    }

    private static List<Document> ReturnSales { get; set; } = new List<Document>();

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
        this.SearchForFilter();
      }
    }

    public string FilterTextGroups
    {
      get => this._filterTextGroups;
      set
      {
        this._filterTextGroups = value;
        this.OnPropertyChanged(nameof (FilterTextGroups));
      }
    }

    public List<Sections.Section> ListSections { get; set; }

    public Guid SectionSelectedUid
    {
      get => this._sectionSelectedUid;
      set
      {
        this._sectionSelectedUid = value;
        this.OnPropertyChanged(nameof (SectionSelectedUid));
        this.SearchForFilter();
      }
    }

    public Client Client
    {
      get => this._client;
      set
      {
        this._client = value;
        if (!this._isFilterForClient)
          return;
        this.SearchForFilter();
      }
    }

    public bool IsEnabledClient
    {
      get => this._isEnabledClient;
      set
      {
        this._isEnabledClient = value;
        this.OnPropertyChanged(nameof (IsEnabledClient));
      }
    }

    public List<Gbs.Core.Entities.Users.User> UsersList { get; set; }

    public Gbs.Core.Entities.Users.User SelectedUser
    {
      get => this._selectedUser;
      set
      {
        this._selectedUser = value;
        this.OnPropertyChanged(nameof (SelectedUser));
        this.SearchForFilter();
      }
    }

    public ObservableCollection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>> PaymentMethodList
    {
      get => this._paymentMethodList;
      set
      {
        this._paymentMethodList = value;
        this.OnPropertyChanged(nameof (PaymentMethodList));
        this.OnPropertyChanged("TextPropButton");
        this.SearchForFilter();
      }
    }

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set
      {
        if (!(this._valueDateTimeStart.Date != value.Date))
          return;
        this._valueDateTimeStart = value;
        this.OnPropertyChanged(nameof (ValueDateTimeStart));
      }
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set
      {
        if (!(this._valueDateTimeEnd.Date != value.Date))
          return;
        this._valueDateTimeEnd = value;
        this.OnPropertyChanged(nameof (ValueDateTimeEnd));
      }
    }

    public string FilterSales
    {
      get => this._filterSales;
      set
      {
        this._filterSales = value;
        this.OnPropertyChanged(nameof (FilterSales));
        if (this.TimerSearch.Enabled)
        {
          this.TimerSearch.Stop();
          this.TimerSearch.Start();
        }
        this.TimerSearch.Start();
      }
    }

    private void UsualSearch(
      string filterText,
      List<SaleJournalViewModel.SaleItemsInfoGrid> saleList)
    {
      if (filterText.IsNullOrEmpty())
      {
        saleList.AddRange((IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) this.CachedDbGoods);
      }
      else
      {
        foreach (GoodsSearchModelView.FilterProperty filterProperty in this.FilterProperties.Where<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (x => x.IsChecked)))
        {
          switch (filterProperty.Name)
          {
            case "Name":
              saleList.AddRange(this._viewSaleJournal == ViewSaleJournal.ListGood ? this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Item.Good.Name.ToLower().Contains(filterText))) : this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.TextSaleContent.ToLower().Contains(filterText))));
              continue;
            case "Barcode":
              saleList.AddRange(this._viewSaleJournal == ViewSaleJournal.ListGood ? this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Item.Good.Barcode.ToLower().Contains(filterText))) : this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.Good.Barcode.Contains(filterText))))));
              continue;
            case "Barcodes":
              saleList.AddRange(this._viewSaleJournal == ViewSaleJournal.ListGood ? this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Item.Good.Barcodes.Any<string>((Func<string, bool>) (b => b.Contains(filterText))))) : this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.Good.Barcodes.Any<string>((Func<string, bool>) (b => b.Contains(filterText))))))));
              continue;
            case "Sum":
              if (this._viewSaleJournal == ViewSaleJournal.ListGood)
              {
                Decimal sum;
                if (Decimal.TryParse(filterText.Replace('.', ','), out sum))
                {
                  saleList.AddRange(this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => Math.Abs(x.Item.TotalSum - sum) < 1M)));
                  continue;
                }
                continue;
              }
              Decimal sum1;
              if (Decimal.TryParse(filterText.Replace('.', ','), out sum1))
              {
                saleList.AddRange(this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => Math.Abs(x.Sum - sum1) < 1M)));
                continue;
              }
              continue;
            case "Number":
              saleList.AddRange(this.CachedDbGoods.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Number.ToLower().Contains(filterText))));
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void SearchForFilter()
    {
      if (this.CachedDbGoods == null)
        return;
      if (this._clientUidByCache != Guid.Empty)
      {
        Guid clientUidByCache = this._clientUidByCache;
        Client client = this._client;
        // ISSUE: explicit non-virtual call
        Guid guid = client != null ? __nonvirtual (client.Uid) : Guid.Empty;
        if (clientUidByCache != guid)
        {
          this.GetSalesList();
          return;
        }
      }
      string filterText = this.FilterSales == null ? string.Empty : this.FilterSales.ToLower();
      List<SaleJournalViewModel.SaleItemsInfoGrid> saleItemsInfoGridList = new List<SaleJournalViewModel.SaleItemsInfoGrid>();
      this.UsualSearch(filterText, saleItemsInfoGridList);
      if (!saleItemsInfoGridList.Any<SaleJournalViewModel.SaleItemsInfoGrid>())
      {
        this.Sale.SaleItemsList = new AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid>();
        this.SetCountItems();
      }
      else
      {
        List<SaleJournalViewModel.SaleItemsInfoGrid> source1 = this._viewSaleJournal != ViewSaleJournal.ListGood ? saleItemsInfoGridList.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document != null)).GroupBy<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Document.Uid)).Select<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>((Func<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.First<SaleJournalViewModel.SaleItemsInfoGrid>())).ToList<SaleJournalViewModel.SaleItemsInfoGrid>() : saleItemsInfoGridList.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Item != null)).GroupBy<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Item.Uid)).Select<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>((Func<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.First<SaleJournalViewModel.SaleItemsInfoGrid>())).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.GroupsListFilter.Any<GoodGroups.Group>())
          source1 = this._viewSaleJournal == ViewSaleJournal.ListGood ? source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => this.GroupsListFilter.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Item.Good.Group.Uid)))).ToList<SaleJournalViewModel.SaleItemsInfoGrid>() : source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => this.GroupsListFilter.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == i.Good.Group.Uid)))))).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.Client != null)
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.ContractorUid == this.Client.Uid)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.SelectedUser != null && this.SelectedUser.Uid != Guid.Parse("11111111-1111-1111-1111-111111111111"))
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.UserUid == this.SelectedUser.Uid)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.PaymentMethodList.All<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => !x.IsChecked)))
          source1 = source1 != null ? source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => !x.Document.Payments.Any<Gbs.Core.Entities.Payments.Payment>())).ToList<SaleJournalViewModel.SaleItemsInfoGrid>() : (List<SaleJournalViewModel.SaleItemsInfoGrid>) null;
        else if (this.PaymentMethodList.Any<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => !x.IsChecked)))
        {
          List<SaleJournalViewModel.SaleItemsInfoGrid> source2 = new List<SaleJournalViewModel.SaleItemsInfoGrid>();
          foreach (SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod> itemSelected in this.PaymentMethodList.Where<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (x => x.IsChecked)))
          {
            SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod> methodSelected = itemSelected;
            source2.AddRange(source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p =>
            {
              Guid uid = methodSelected.Item.Uid;
              PaymentMethods.PaymentMethod method = p.Method;
              // ISSUE: explicit non-virtual call
              Guid guid = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
              return uid == guid;
            })))));
          }
          source1 = source2.GroupBy<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Item.Uid)).Select<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>((Func<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, SaleJournalViewModel.SaleItemsInfoGrid>) (x => x.First<SaleJournalViewModel.SaleItemsInfoGrid>())).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        }
        if (this.SelectedExtraOption != SaleJournalViewModel.ExtraOption.None)
        {
          switch (this.SelectedExtraOption)
          {
            case SaleJournalViewModel.ExtraOption.OnlyCredit:
              source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => !x.Document.Payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)) || x.Document.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn - p.SumOut)) == 0M)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
              break;
            case SaleJournalViewModel.ExtraOption.CreditAndPayment:
              List<SaleJournalViewModel.SaleItemsInfoGrid> collection = new List<SaleJournalViewModel.SaleItemsInfoGrid>();
              foreach (SaleJournalViewModel.SaleItemsInfoGrid saleItemsInfoGrid in source1)
              {
                if (!(saleItemsInfoGrid.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn - p.SumOut)) < 0M) && Math.Abs(saleItemsInfoGrid.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn - p.SumOut)) - SaleHelper.GetSumDocument(saleItemsInfoGrid.Document)) > 0.01M && saleItemsInfoGrid.Document.Payments.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)))
                  collection.Add(saleItemsInfoGrid);
              }
              source1 = new List<SaleJournalViewModel.SaleItemsInfoGrid>((IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) collection);
              break;
            case SaleJournalViewModel.ExtraOption.OnlyFullPayment:
              source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn - p.SumOut)) >= SaleHelper.GetSumDocument(x.Document))).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
              break;
            case SaleJournalViewModel.ExtraOption.Credit:
              source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.Payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumIn - p.SumOut)) < SaleHelper.GetSumDocument(x.Document))).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
              break;
          }
        }
        if (this.SelectedTypePayment == Translate.SaleJournalViewModel_ListTypePayment_Фискально)
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => x.Document.IsFiscal)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.SelectedTypePayment == Translate.SaleJournalViewModel_ListTypePayment_Нефискально)
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => !x.Document.IsFiscal)).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this._sectionSelectedUid != Guid.Empty)
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x =>
          {
            Guid? uid = x.Document.Section?.Uid;
            Guid sectionSelectedUid = this._sectionSelectedUid;
            return uid.HasValue && uid.GetValueOrDefault() == sectionSelectedUid;
          })).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        if (this.StorageListFilter.Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => !x.IsChecked)))
          source1 = source1.Where<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, bool>) (x => this.StorageListFilter.Where<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (s => s.IsChecked)).Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (s => s.Item.Uid == x.Document.Storage.Uid)))).ToList<SaleJournalViewModel.SaleItemsInfoGrid>();
        this.Sale.SaleItemsList = new AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid>((source1 != null ? source1.OrderBy<SaleJournalViewModel.SaleItemsInfoGrid, DateTime>((Func<SaleJournalViewModel.SaleItemsInfoGrid, DateTime>) (x => x.Document.DateTime)).Reverse<SaleJournalViewModel.SaleItemsInfoGrid>() : (IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) null) ?? (IEnumerable<SaleJournalViewModel.SaleItemsInfoGrid>) new List<SaleJournalViewModel.SaleItemsInfoGrid>());
        this.SetCountItems();
      }
    }

    private void SetCountItems()
    {
      List<Document> list = this.Sale.SaleItemsList.GroupBy<SaleJournalViewModel.SaleItemsInfoGrid, Guid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Guid>) (x => x.Document.Uid)).Select<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, Document>((Func<IGrouping<Guid, SaleJournalViewModel.SaleItemsInfoGrid>, Document>) (x => x.First<SaleJournalViewModel.SaleItemsInfoGrid>().Document)).ToList<Document>();
      if (this._viewSaleJournal == ViewSaleJournal.ListGood)
      {
        this.TotalSaleItemsCount = this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.Item.Quantity));
        this.TotalSaleSum = this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.Item.TotalSum));
        this.TotalSaleName = (Decimal) this.Sale.SaleItemsList.Count;
      }
      else
      {
        this.TotalSaleItemsCount = this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.Document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => i.Quantity)) - x.SumCountItemReturn));
        this.TotalSaleSum = this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => SaleHelper.GetSumDocument(x.Document))) - this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, Decimal>) (x => x.SumItemReturn));
        this.TotalSaleName = (Decimal) this.Sale.SaleItemsList.Sum<SaleJournalViewModel.SaleItemsInfoGrid>((Func<SaleJournalViewModel.SaleItemsInfoGrid, int>) (x => x.CountItem));
      }
      this.TotalSaleCount = (Decimal) list.Count<Document>();
      this.OnPropertyChanged("TotalIncome");
    }

    public List<string> ListTypePayment { get; set; }

    public Dictionary<SaleJournalViewModel.ExtraOption, string> ListExtraOption { get; set; }

    public SaleJournalViewModel.ExtraOption SelectedExtraOption
    {
      get => this._selectedExtraOption;
      set
      {
        this._selectedExtraOption = value;
        this.OnPropertyChanged(nameof (SelectedExtraOption));
        this.SearchForFilter();
      }
    }

    public string SelectedTypePayment
    {
      get => this._selectedTypePayment;
      set
      {
        this._selectedTypePayment = value;
        this.OnPropertyChanged(nameof (SelectedTypePayment));
        this.SearchForFilter();
      }
    }

    public class SaleItemsInfoGrid : ViewModelWithForm
    {
      private string _color = WindowWithSize.DefaultForeground;
      private Decimal? _sumIncome;
      private Client _client = new Client();
      private Gbs.Core.Entities.Users.User _user = new Gbs.Core.Entities.Users.User();

      public string Uid { get; set; } = Guid.NewGuid().ToString();

      public Decimal? SumIncome
      {
        get => this._sumIncome;
        set
        {
          this._sumIncome = value;
          this.OnPropertyChanged(nameof (SumIncome));
        }
      }

      public int SaleNumber
      {
        get => this.Document.Number.IsNullOrEmpty() ? 0 : int.Parse(this.Document.Number);
      }

      public Document Document { get; set; }

      public BasketItem Item { get; set; }

      public string TextSaleContent
      {
        get
        {
          return string.Join("\r", this.Document.Items.GroupBy(x => new
          {
            ModificationUid = x.ModificationUid,
            Discount = x.Discount,
            Comment = x.Comment,
            SellPrice = x.SellPrice,
            GoodUid = x.GoodUid
          }).Select<IGrouping<\u003C\u003Ef__AnonymousType40<Guid, Decimal, string, Decimal, Guid>, Gbs.Core.Entities.Documents.Item>, string>(x => string.Format("{0} {1:N2}*{2:N3};", (object) x.First<Gbs.Core.Entities.Documents.Item>().Good.Name, (object) x.First<Gbs.Core.Entities.Documents.Item>().SellPrice, (object) x.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)))));
        }
      }

      public Visibility BorderVisibility
      {
        get => this.SaleContentList.Count <= 1 ? Visibility.Collapsed : Visibility.Visible;
      }

      public ObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid.SaleContentItem> SaleContentList
      {
        get
        {
          IEnumerable<IGrouping<\u003C\u003Ef__AnonymousType40<Guid, Decimal, string, Decimal, Guid>, Gbs.Core.Entities.Documents.Item>> groupings = this.Document.Items.GroupBy(x => new
          {
            ModificationUid = x.ModificationUid,
            Discount = x.Discount,
            Comment = x.Comment,
            SellPrice = x.SellPrice,
            GoodUid = x.GoodUid
          });
          List<SaleJournalViewModel.SaleItemsInfoGrid.SaleContentItem> list = new List<SaleJournalViewModel.SaleItemsInfoGrid.SaleContentItem>();
          foreach (IGrouping<\u003C\u003Ef__AnonymousType40<Guid, Decimal, string, Decimal, Guid>, Gbs.Core.Entities.Documents.Item> source in groupings)
          {
            string name = source.First<Gbs.Core.Entities.Documents.Item>().Good.Name;
            string empty = string.Empty;
            if (name.Length > 50)
            {
              string str1 = name.Substring(0, 47) + "...";
            }
            string str2 = string.Format("{0:N2}*{1:N3}", (object) source.First<Gbs.Core.Entities.Documents.Item>().SellPrice, (object) source.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity))).PadLeft(20);
            list.Add(new SaleJournalViewModel.SaleItemsInfoGrid.SaleContentItem()
            {
              Name = name,
              PriceAndQty = str2
            });
          }
          return new ObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid.SaleContentItem>(list);
        }
      }

      public string TextMethodPayment
      {
        get
        {
          return this.Document.Payments.Count<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn > 0M)) == 1 ? this.Document.Payments.Single<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn > 0M)).Method?.Name : (this.Document.Payments.Count<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn > 0M)) == 0 ? Translate.SaleItemsInfoGrid_TextMethodPayment_Не_оплачено : string.Join(", ", this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).GroupBy<Gbs.Core.Entities.Payments.Payment, Guid?>((Func<Gbs.Core.Entities.Payments.Payment, Guid?>) (x => x.Method?.Uid)).Select<IGrouping<Guid?, Gbs.Core.Entities.Payments.Payment>, string>((Func<IGrouping<Guid?, Gbs.Core.Entities.Payments.Payment>, string>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method?.Name))));
        }
      }

      public int CountItem => this.Document.Items.Count;

      public Decimal QuantitySum
      {
        get
        {
          return this.Document.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)) - this.SumCountItemReturn;
        }
      }

      public Decimal SumLessDiscount
      {
        get
        {
          return SaleHelper.GetSumDocument(this.Document) + SaleHelper.GetSumDiscountDocument(this.Document) + this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) - this.SumItemReturn - this.SumDiscountReturn;
        }
      }

      public Decimal SumDiscount
      {
        get
        {
          return SaleHelper.GetSumDiscountDocument(this.Document) + this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) - this.SumDiscountReturn;
        }
      }

      public Decimal Sum => SaleHelper.GetSumDocument(this.Document) - this.SumItemReturn;

      public Decimal SumPayment
      {
        get
        {
          return this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        }
      }

      public Decimal SumCountItemReturn { get; set; }

      public Decimal SumItemReturn { get; set; }

      public Decimal SumDiscountReturn { get; set; }

      public string Color
      {
        get => this._color;
        set
        {
          this._color = value;
          this.OnPropertyChanged(nameof (Color));
        }
      }

      public Client Client
      {
        get => this._client;
        set
        {
          this._client = value;
          this.OnPropertyChanged(nameof (Client));
        }
      }

      public Gbs.Core.Entities.Users.User User
      {
        get => this._user;
        set
        {
          this._user = value;
          this.OnPropertyChanged(nameof (User));
        }
      }

      public SaleItemsInfoGrid(
        Document doc,
        IEnumerable<Gbs.Core.Entities.Documents.Item> items,
        bool isMinusReturn,
        ViewSaleJournal viewSaleJournal)
      {
        Gbs.Core.Entities.Documents.Item it = items.First<Gbs.Core.Entities.Documents.Item>();
        Gbs.Core.Entities.Goods.Good good = it.Good;
        this.Document = doc;
        this.Item = new BasketItem(good, it.ModificationUid, it.SellPrice, it.GoodStock?.Storage, items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)), it.Discount, it.Uid)
        {
          Comment = it.Comment
        };
        Color red;
        if (SaleJournalViewModel.ReturnSales.Any<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Uid && x.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == it.GoodUid && i.ModificationUid == it.ModificationUid && i.SellPrice == it.SellPrice)))))
        {
          red = Color.Red;
          this.Color = red.Name;
          if (isMinusReturn)
          {
            Decimal num1 = 0M;
            Decimal num2 = 0M;
            Decimal num3 = 0M;
            foreach (Gbs.Core.Entities.Documents.Item i in SaleJournalViewModel.ReturnSales.Where<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Uid)).SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.ModificationUid == it.ModificationUid && i.SellPrice == it.SellPrice && i.GoodUid == it.GoodUid)))
            {
              num1 += i.Quantity;
              num2 += SaleHelper.GetSumItemInDocument(i);
              num3 += SaleHelper.GetSumDiscountItem(i);
              if (i.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None)
                break;
            }
            BasketItem basketItem = this.Item;
            basketItem.Quantity = basketItem.Quantity - num1;
            this.SumCountItemReturn = num1;
            this.SumItemReturn = num2;
            this.SumDiscountReturn = num3;
          }
        }
        if (viewSaleJournal != ViewSaleJournal.ListSale || !SaleJournalViewModel.ReturnSales.Any<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Uid)))
          return;
        red = Color.Red;
        this.Color = red.Name;
      }

      public class SaleContentItem
      {
        public string Name { get; set; }

        public string PriceAndQty { get; set; }
      }
    }

    public class ItemSelected<T>
    {
      public bool IsChecked { get; set; } = true;

      public T Item { get; set; }
    }

    public enum ExtraOption
    {
      None,
      OnlyCredit,
      CreditAndPayment,
      OnlyFullPayment,
      Credit,
    }
  }
}
