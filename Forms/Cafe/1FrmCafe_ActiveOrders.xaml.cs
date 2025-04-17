// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.CafeActiveOrdersViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Sale;
using Gbs.Forms.Sale.Return;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public class CafeActiveOrdersViewModel : ViewModelWithForm
  {
    private ObservableCollection<CafeActiveOrdersViewModel.CafeOrder> _activeOrders;
    private bool _isLoadingCache;
    private CafeActiveOrdersViewModel.CafeOrder _selectedOrder;
    private List<CafeActiveOrdersViewModel.CafeOrder> _selectedOrderForAlsoMenu;
    private Action<Document> _addInBasketAction;
    private DateTime _valueDateTimeStart;
    private DateTime _valueDateTimeEnd;
    private int? _numberTable;
    private bool _isCheckedClient;
    private Client _client;
    private GlobalDictionaries.DocumentsStatuses _selectedStatus;

    public Visibility VisibilityReturnSale { get; set; }

    public ICommand ReturnOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this._selectedOrderForAlsoMenu.Any<CafeActiveOrdersViewModel.CafeOrder>())
            throw new ErrorHelper.GbsException(Translate.CafeActiveOrdersViewModel_ReturnOrderCommand_Необходимо_выбрать_заказ_для_возврата_товара)
            {
              Direction = ErrorHelper.ErrorDirections.Outer,
              Level = MsgException.LevelTypes.Warm
            };
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Document doc = new DocumentsRepository(dataBase).GetByParentUid(this._selectedOrder.Document.Uid).FirstOrDefault<Document>();
            if (doc == null || doc.IsDeleted)
              MessageBoxHelper.Warning(Translate.CafeActiveOrdersViewModel_ReturnOrderCommand_По_данному_заказу_нельзя_выполнить_возврат__так_как_за_ним_не_закреплена_продажа_или_она_была_удалена_);
            else
              new FrmReturnSales().ShowReturns(doc);
          }
        }));
      }
    }

    public void ShowActiveOrder(Action<Document> addAction)
    {
      this.LoadOrders();
      this._addInBasketAction = addAction;
      this.FormToSHow = (WindowWithSize) new FrmCafeActiveOrders();
      this.ShowMenuAction = new Action(((FrmCafeActiveOrders) this.FormToSHow).ShowPrintMenu);
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this._isLoadingCache = true;
      this.ShowForm();
    }

    public static string AlsoMenuKey => "AlsoMenu";

    public ObservableCollection<BasketItem> DocumentItems { get; set; }

    public ObservableCollection<CafeActiveOrdersViewModel.CafeOrder> ActiveOrders
    {
      get => this._activeOrders;
      set
      {
        this._activeOrders = value;
        this.OnPropertyChanged(nameof (ActiveOrders));
      }
    }

    public Decimal TotalSumOrders
    {
      get
      {
        return this.ActiveOrders.Sum<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, Decimal>) (x => x.Sum));
      }
    }

    private List<CafeActiveOrdersViewModel.CafeOrder> CafeOrdersCash { get; set; }

    private void LoadOrders()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CafeActiveOrdersViewModel_LoadOrders_Загрузка_заказов);
      Performancer performancer = new Performancer("Загрзука заказов  кафе");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        List<Client> clients = new ClientsRepository(dataBase).GetActiveItems();
        performancer.AddPoint("Загрзука клиентов");
        List<Document> source;
        if (!this._isLoadingCache)
          source = documentsRepository.GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 11 && x.IS_DELETED == false && x.STATUS == 1)));
        else
          source = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.CafeOrder, true);
        performancer.AddPoint("Загрузка документов");
        Func<Document, CafeActiveOrdersViewModel.CafeOrder> selector = (Func<Document, CafeActiveOrdersViewModel.CafeOrder>) (d => new CafeActiveOrdersViewModel.CafeOrder()
        {
          Document = d,
          IsEnabledRow = d.Status != GlobalDictionaries.DocumentsStatuses.Close,
          Client = clients.FirstOrDefault<Client>((Func<Client, bool>) (x => x.Uid == d.ContractorUid))
        });
        this.CafeOrdersCash = new List<CafeActiveOrdersViewModel.CafeOrder>((IEnumerable<CafeActiveOrdersViewModel.CafeOrder>) source.Select<Document, CafeActiveOrdersViewModel.CafeOrder>(selector).ToList<CafeActiveOrdersViewModel.CafeOrder>());
        this.OnPropertyChanged("ActiveOrders");
        this.OnPropertyChanged("TotalSumOrders");
        performancer.Stop();
        progressBar.Close();
      }
    }

    public CafeActiveOrdersViewModel.CafeOrder SelectedOrder
    {
      get => this._selectedOrder;
      set
      {
        this._selectedOrder = value;
        this.DocumentItems = value == null ? new ObservableCollection<BasketItem>() : new ObservableCollection<BasketItem>(value.Document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, value.Document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
        this.OnPropertyChanged("DocumentItems");
        this.OnPropertyChanged("IsEnabledButton");
        this.OnPropertyChanged("TotalCountGood");
        this.OnPropertyChanged("TotalSumItems");
        this.OnPropertyChanged("TotalSumDiscount");
      }
    }

    public Decimal TotalCountGood
    {
      get => this.DocumentItems.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity));
    }

    public Decimal TotalSumItems
    {
      get => this.DocumentItems.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.TotalSum));
    }

    public Decimal TotalSumDiscount
    {
      get => this.DocumentItems.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.DiscountSum));
    }

    public ICommand ShowAlsoMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._selectedOrderForAlsoMenu = ((IEnumerable) obj).Cast<CafeActiveOrdersViewModel.CafeOrder>().ToList<CafeActiveOrdersViewModel.CafeOrder>();
          this.VisibilityReturnSale = this._selectedOrderForAlsoMenu.Count<CafeActiveOrdersViewModel.CafeOrder>() != 1 || !this._selectedOrderForAlsoMenu.All<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)) ? Visibility.Collapsed : Visibility.Visible;
          this.OnPropertyChanged("VisibilityReturnSale");
          this.ShowMenuAction();
        }));
      }
    }

    public ICommand SeparationOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else if (this._selectedOrderForAlsoMenu.Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else if (this.SelectedOrder.Document.IsDeleted)
          {
            int num3 = (int) MessageBoxHelper.Show(Translate.CafeActiveOrdersViewModel_Данный_заказ_уже_удален);
          }
          else if (this.SelectedOrder.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)
          {
            int num4 = (int) MessageBoxHelper.Show(Translate.CafeActiveOrdersViewModel_Невозможно_разделить_уже_закрытый_заказ);
          }
          else
          {
            if (!new OrderSeparationViewModel().ShowSeparationOrder(this.SelectedOrder.Document))
              return;
            this._isLoadingCache = false;
            this.LoadOrders();
            this.SearchFilter();
            this._isLoadingCache = true;
          }
        }));
      }
    }

    public ICommand CombineOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          if (!new OrderCombineViewModel().ShowCombineOrder(this.ActiveOrders.ToList<CafeActiveOrdersViewModel.CafeOrder>()))
            return;
          this._isLoadingCache = false;
          this.LoadOrders();
          this.SearchFilter();
          this._isLoadingCache = true;
        }));
      }
    }

    public ICommand ShowSaleCard
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись_для_просмотра_карточки_продажи);
          }
          else if (this._selectedOrderForAlsoMenu.Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else if (this.SelectedOrder.Document.Status != GlobalDictionaries.DocumentsStatuses.Close)
          {
            int num3 = (int) MessageBoxHelper.Show(Translate.CafeActiveOrdersViewModel_Данный_заказ_еще_не_закрыт__открыть_карточку_продажи_невозможно);
          }
          else
          {
            MainWindowViewModel.DoWithPause();
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              Document doc = new DocumentsRepository(dataBase).GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.PARENT_UID == this.SelectedOrder.Document.Uid))).FirstOrDefault<Document>();
              if (doc != null)
              {
                new FrmCardSale().ShowSaleCard(doc);
              }
              else
              {
                int num4 = (int) MessageBoxHelper.Show(Translate.CafeActiveOrdersViewModel_Продажа_для_данного_заказа_не_найдена_);
              }
            }
          }
        }));
      }
    }

    public ICommand DeleteOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<CafeActiveOrdersViewModel.CafeOrder> list = ((IEnumerable) obj).Cast<CafeActiveOrdersViewModel.CafeOrder>().ToList<CafeActiveOrdersViewModel.CafeOrder>();
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            if (list.Any<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.IsDeleted)))
            {
              List<CafeActiveOrdersViewModel.CafeOrder> isDeletedOrder = list.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.IsDeleted)).ToList<CafeActiveOrdersViewModel.CafeOrder>();
              int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.CafeActiveOrdersViewModel_ЗаказыУжеУдалены, (object) string.Join(", ", isDeletedOrder.Select<CafeActiveOrdersViewModel.CafeOrder, string>((Func<CafeActiveOrdersViewModel.CafeOrder, string>) (x => x.Document.Number)))));
              list.RemoveAll((Predicate<CafeActiveOrdersViewModel.CafeOrder>) (x => isDeletedOrder.Any<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (d => x.Document.Uid == d.Document.Uid))));
            }
            if (list.Any<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)))
            {
              List<CafeActiveOrdersViewModel.CafeOrder> isCloseOrder = list.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)).ToList<CafeActiveOrdersViewModel.CafeOrder>();
              int num3 = (int) MessageBoxHelper.Show(string.Format(Translate.CafeActiveOrdersViewModel_НевозможноУдалитьЗАказыОниУдалены, (object) string.Join(", ", isCloseOrder.Select<CafeActiveOrdersViewModel.CafeOrder, string>((Func<CafeActiveOrdersViewModel.CafeOrder, string>) (x => x.Document.Number)))));
              list.RemoveAll((Predicate<CafeActiveOrdersViewModel.CafeOrder>) (x => isCloseOrder.Any<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (d => x.Document.Uid == d.Document.Uid))));
            }
            if (!list.Any<CafeActiveOrdersViewModel.CafeOrder>() || MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list.Count), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
              return;
            Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              if (!new UsersRepository(dataBase).GetAccess(user, Actions.DeleteOrderCafe))
              {
                (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.DeleteOrderCafe);
                if (!access.Result)
                  return;
                user = access.User;
              }
              (bool result2, string output2) = MessageBoxHelper.Input("", Translate.CafeActiveOrdersViewModel_Укажите_причину_удаления_заказа_, 5);
              if (!result2)
                return;
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              foreach (CafeActiveOrdersViewModel.CafeOrder cafeOrder in list)
              {
                Document document = cafeOrder.Document.Clone<Document>();
                ExtraPrinters.PrepareExtraPrint(document);
                cafeOrder.Document.Comment = string.Format(Translate.CafeActiveOrdersViewModel_Удален___0_, (object) output2);
                cafeOrder.Document.IsDeleted = true;
                cafeOrder.Document.Status = GlobalDictionaries.DocumentsStatuses.Close;
                documentsRepository.Save(cafeOrder.Document, false);
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) document, (IEntity) cafeOrder.Document, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, user), false);
                document.Items.Clear();
                ExtraPrinters.Print(document);
              }
              this.SearchFilter();
            }
          }
        }));
      }
    }

    public ICommand UpdateCommentCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else if (((ICollection) obj).Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            (bool result2, string output2) = MessageBoxHelper.Input(this.SelectedOrder.Document.Comment, Translate.CafeActiveOrdersViewModel_Введите_комментарий_к_заказу);
            if (!result2)
              return;
            this.SelectedOrder.Document.Comment = output2;
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              new DocumentsRepository(dataBase).Save(this.SelectedOrder.Document);
              this.ActiveOrders[this.ActiveOrders.ToList<CafeActiveOrdersViewModel.CafeOrder>().FindIndex((Predicate<CafeActiveOrdersViewModel.CafeOrder>) (x => x.Document.Uid == this.SelectedOrder.Document.Uid))] = this.SelectedOrder;
              this.ActiveOrders = new ObservableCollection<CafeActiveOrdersViewModel.CafeOrder>((IEnumerable<CafeActiveOrdersViewModel.CafeOrder>) this.ActiveOrders);
            }
          }
        }));
      }
    }

    public ICommand PrintCheckCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else if (((ICollection) obj).Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
            SaleHelper.PrintNoFiscalCheck(this.SelectedOrder.Document);
        }));
      }
    }

    public ICommand AddBasketOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedOrder == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else if (((ICollection) obj).Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else if (this.SelectedOrder.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)
          {
            int num3 = (int) MessageBoxHelper.Show(Translate.CafeActiveOrdersViewModel_Невозможно_добавить_в_корзину_уже_закрытый_заказ);
          }
          else
          {
            this._addInBasketAction(this.SelectedOrder.Document);
            this.CloseAction();
          }
        }));
      }
    }

    public bool IsEnabledButton
    {
      get
      {
        CafeActiveOrdersViewModel.CafeOrder selectedOrder = this.SelectedOrder;
        return selectedOrder != null && selectedOrder.IsEnabledRow;
      }
    }

    private Action ShowMenuAction { get; set; }

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set => this._valueDateTimeStart = value;
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set => this._valueDateTimeEnd = value;
    }

    public int? NumberTable
    {
      get => this._numberTable;
      set
      {
        int? numberTable = this._numberTable;
        int? nullable = value;
        if (numberTable.GetValueOrDefault() == nullable.GetValueOrDefault() & numberTable.HasValue == nullable.HasValue)
          return;
        this._numberTable = value;
        this.SearchFilter();
      }
    }

    public bool IsCheckedClient
    {
      get => this._isCheckedClient;
      set
      {
        this._isCheckedClient = value;
        this.OnPropertyChanged(nameof (IsCheckedClient));
        if (value && this.Client == null)
          this.GetClient();
        if (value)
          return;
        this.Client = (Client) null;
      }
    }

    public string ClientName => this.Client?.Name ?? Translate.MainWindowViewModel_Выберите_клиента;

    private Client Client
    {
      get => this._client;
      set
      {
        this._client = value;
        this.OnPropertyChanged("ClientName");
        this.SearchFilter();
      }
    }

    public ICommand GetClientCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.GetClient()));
    }

    private void GetClient()
    {
      if (!this.IsCheckedClient)
      {
        this.Client = (Client) null;
      }
      else
      {
        (Client client, bool result) client1 = new FrmSearchClient().GetClient();
        Client client2 = client1.client;
        if (!client1.result)
        {
          this.IsCheckedClient = false;
        }
        else
        {
          this.Client = client2;
          this.IsCheckedClient = this.Client != null;
          this.OnPropertyChanged("ClientName");
        }
      }
    }

    public static Dictionary<GlobalDictionaries.DocumentsStatuses, string> Statuses
    {
      get
      {
        return new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
        {
          {
            GlobalDictionaries.DocumentsStatuses.None,
            Translate.ClientOrderListViewModel_Все_статусы
          },
          {
            GlobalDictionaries.DocumentsStatuses.Open,
            Translate.CafeActiveOrdersViewModel_Statuses_Удален
          }
        }.Concat<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((IEnumerable<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>) GlobalDictionaries.CafeOrderStatusDictionary).ToDictionary<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, GlobalDictionaries.DocumentsStatuses, string>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, GlobalDictionaries.DocumentsStatuses>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, string>) (v => v.Value));
      }
    }

    public GlobalDictionaries.DocumentsStatuses SelectedStatus
    {
      get => this._selectedStatus;
      set
      {
        this._selectedStatus = value;
        this.SearchFilter();
      }
    }

    public Visibility VisibilityNumTableFilter
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsTableAndGuest ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand LoadOrdersCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.SearchFilter()));
    }

    private void SearchFilter()
    {
      if (this._isLoadingCache && this.SelectedStatus != GlobalDictionaries.DocumentsStatuses.Draft)
      {
        this.LoadOrders();
        this._isLoadingCache = false;
      }
      IEnumerable<CafeActiveOrdersViewModel.CafeOrder> source = new List<CafeActiveOrdersViewModel.CafeOrder>((IEnumerable<CafeActiveOrdersViewModel.CafeOrder>) this.CafeOrdersCash).Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x =>
      {
        DateTime dateTime1 = x.Document.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.ValueDateTimeStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.Document.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.ValueDateTimeEnd;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.IsCheckedClient && this.Client != null)
        source = source.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x =>
        {
          Client client = x.Client;
          // ISSUE: explicit non-virtual call
          return (client != null ? __nonvirtual (client.Uid) : Guid.Empty) == this.Client.Uid;
        }));
      if (this.NumberTable.HasValue)
        source = source.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x =>
        {
          int? numTable = x.NumTable;
          int? numberTable = this.NumberTable;
          return numTable.GetValueOrDefault() == numberTable.GetValueOrDefault() & numTable.HasValue == numberTable.HasValue;
        }));
      if (this.SelectedStatus != GlobalDictionaries.DocumentsStatuses.None)
        source = this.SelectedStatus == GlobalDictionaries.DocumentsStatuses.Open ? source.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.IsDeleted)) : source.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.Status == this.SelectedStatus && !x.Document.IsDeleted));
      this.ActiveOrders = new ObservableCollection<CafeActiveOrdersViewModel.CafeOrder>((IEnumerable<CafeActiveOrdersViewModel.CafeOrder>) source.OrderByDescending<CafeActiveOrdersViewModel.CafeOrder, DateTime>((Func<CafeActiveOrdersViewModel.CafeOrder, DateTime>) (x => x.Document.DateTime)));
      this.OnPropertyChanged("ActiveOrders");
      this.OnPropertyChanged("TotalSumOrders");
    }

    public CafeActiveOrdersViewModel()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      this._valueDateTimeStart = dateTime.AddYears(-1);
      this._valueDateTimeEnd = DateTime.Now.Date;
      this._selectedStatus = GlobalDictionaries.DocumentsStatuses.Draft;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class CafeOrder : ViewModel
    {
      private Document _document;

      public bool IsEnabledRow { get; set; } = true;

      public Document Document
      {
        get => this._document;
        set
        {
          this._document = value;
          this.OnPropertyChanged(nameof (Document));
        }
      }

      public int CountGuest
      {
        get
        {
          Document document = this.Document;
          return Convert.ToInt32((document != null ? document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CountGuestUid))?.Value : (object) null) ?? (object) 1);
        }
      }

      public int? NumTable
      {
        get
        {
          Document document = this.Document;
          return new int?(Convert.ToInt32(document != null ? document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.NumTableUid))?.Value : (object) null));
        }
      }

      public Decimal Sum => SaleHelper.GetSumDocument(this.Document);

      public string Status
      {
        get
        {
          return CafeActiveOrdersViewModel.Statuses.SingleOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x => x.Key == this.Document.Status)).Value ?? "";
        }
      }

      public Client Client { get; set; }
    }
  }
}
