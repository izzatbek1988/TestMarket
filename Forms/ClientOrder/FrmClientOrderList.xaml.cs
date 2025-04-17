// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ClientOrder.ClientOrderListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.ClientOrder
{
  public partial class ClientOrderListViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private ObservableCollection<ClientOrderListViewModel.ClientOrderInfo> _clientOrderList = new ObservableCollection<ClientOrderListViewModel.ClientOrderInfo>();
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private GlobalDictionaries.DocumentsStatuses _selectedStatus;
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();
    private Client _client;
    private bool _isCheckedClient;

    public void UpdateOrderList() => this.LoadDocuments();

    public ObservableCollection<ClientOrderListViewModel.ClientOrderInfo> ClientOrderList
    {
      get => this._clientOrderList;
      set
      {
        this._clientOrderList = value;
        this.OnPropertyChanged(nameof (ClientOrderList));
      }
    }

    public ICommand AddOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Document document;
          if (!new ClientOrderViewModel().ShowOrder(Guid.Empty, out document, this.AuthUser, new Action(this.LoadDocuments)))
            return;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            this.CachedDbOrder.Add(new ClientOrderListViewModel.ClientOrderInfo(document, new ClientsRepository(dataBase).GetByUid(document.ContractorUid), new UsersRepository(dataBase).GetByUid(document.UserUid)));
            this.SearchForFilter();
          }
        }));
      }
    }

    public ICommand EditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          IList source = (IList) obj;
          List<ClientOrderListViewModel.ClientOrderInfo> list = source != null ? source.Cast<ClientOrderListViewModel.ClientOrderInfo>().ToList<ClientOrderListViewModel.ClientOrderInfo>() : (List<ClientOrderListViewModel.ClientOrderInfo>) null;
          if ((list != null ? (!list.Any<ClientOrderListViewModel.ClientOrderInfo>() ? 1 : 0) : 1) != 0 || list.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
          }
          else
          {
            Document oldItem = list.Single<ClientOrderListViewModel.ClientOrderInfo>().Document.Clone<Document>();
            Document doc;
            if (!new ClientOrderViewModel().ShowOrder(list.Single<ClientOrderListViewModel.ClientOrderInfo>().Document.Uid, out doc, this.AuthUser, new Action(this.LoadDocuments)))
              return;
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              ClientOrderListViewModel.ClientOrderInfo clientOrderInfo = new ClientOrderListViewModel.ClientOrderInfo(doc, new ClientsRepository(dataBase).GetByUid(doc.ContractorUid), new UsersRepository(dataBase).GetByUid(doc.UserUid));
              this.CachedDbOrder[this.CachedDbOrder.ToList<ClientOrderListViewModel.ClientOrderInfo>().FindIndex((Predicate<ClientOrderListViewModel.ClientOrderInfo>) (x => x.Document.Uid == doc.Uid))] = clientOrderInfo;
              this.ClientOrderList[this.ClientOrderList.ToList<ClientOrderListViewModel.ClientOrderInfo>().FindIndex((Predicate<ClientOrderListViewModel.ClientOrderInfo>) (x => x.Document.Uid == doc.Uid))] = clientOrderInfo;
              this.OnPropertyChanged("ClientOrderList");
              this.SearchForFilter();
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) clientOrderInfo.Document, ActionType.Edit, GlobalDictionaries.EntityTypes.Document, this.AuthUser), true);
            }
          }
        }));
      }
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            IList source = (IList) obj;
            List<ClientOrderListViewModel.ClientOrderInfo> list = source != null ? source.Cast<ClientOrderListViewModel.ClientOrderInfo>().ToList<ClientOrderListViewModel.ClientOrderInfo>() : (List<ClientOrderListViewModel.ClientOrderInfo>) null;
            if ((list != null ? (!list.Any<ClientOrderListViewModel.ClientOrderInfo>() ? 1 : 0) : 1) != 0)
            {
              MessageBoxHelper.Warning(Translate.InventoryDoViewModel_Необходимо_выбрать_хотя_бы_одну_запись_);
            }
            else
            {
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              {
                if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteClientOrder))
                {
                  (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.DeleteClientOrder);
                  if (!access.Result)
                    return;
                  this.AuthUser = access.User;
                }
                if (list.Any<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)))
                {
                  List<ClientOrderListViewModel.ClientOrderInfo> isCloseOrder = list.Where<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)).ToList<ClientOrderListViewModel.ClientOrderInfo>();
                  MessageBoxHelper.Warning(string.Format(Translate.ClientOrderListViewModel_НевозможноУдалитьЗаказыОниЗакрыты, (object) string.Join(", ", isCloseOrder.Select<ClientOrderListViewModel.ClientOrderInfo, string>((Func<ClientOrderListViewModel.ClientOrderInfo, string>) (x => x.Document.Number)))));
                  list.RemoveAll((Predicate<ClientOrderListViewModel.ClientOrderInfo>) (x => isCloseOrder.Any<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (d => x.Document.Uid == d.Document.Uid))));
                  if (!list.Any<ClientOrderListViewModel.ClientOrderInfo>())
                    return;
                }
                if (MessageBoxHelper.Question(string.Format(Translate.ClientOrderListViewModel_Вы_уверены__что_хотите_удалить__0__заказов_, (object) list.Count)) == MessageBoxResult.No)
                  return;
                DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
                foreach (ClientOrderListViewModel.ClientOrderInfo clientOrderInfo1 in list)
                {
                  ClientOrderListViewModel.ClientOrderInfo order = clientOrderInfo1;
                  ClientOrderListViewModel.ClientOrderInfo clientOrderInfo2 = order.Clone<ClientOrderListViewModel.ClientOrderInfo>();
                  order.Document.IsDeleted = true;
                  documentsRepository.Save(order.Document, false);
                  foreach (Gbs.Core.Entities.Payments.Payment payment in order.Document.Payments)
                  {
                    payment.IsDeleted = true;
                    payment.Save();
                  }
                  List<Document> byParentUid = new DocumentsRepository(dataBase).GetByParentUid(order.Document.Uid);
                  if (byParentUid != null)
                    new DocumentsRepository(dataBase).Delete(byParentUid);
                  this.CachedDbOrder.RemoveAll((Predicate<ClientOrderListViewModel.ClientOrderInfo>) (x => x.Document.Uid == order.Document.Uid));
                  ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) clientOrderInfo2.Document, (IEntity) order.Document, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
                }
                this.SearchForFilter();
              }
            }
          }
        }));
      }
    }

    public Decimal TotalSumOrder
    {
      get
      {
        ObservableCollection<ClientOrderListViewModel.ClientOrderInfo> clientOrderList = this.ClientOrderList;
        return clientOrderList == null ? 0M : clientOrderList.Sum<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, Decimal>) (x => x.Sum));
      }
    }

    public DateTime DateStart
    {
      get => this._dateStart;
      set => this._dateStart = value;
    }

    public DateTime DateFinish
    {
      get => this._dateFinish;
      set => this._dateFinish = value;
    }

    public Client Client
    {
      get => this._client;
      set
      {
        this._client = value;
        this.SearchForFilter();
      }
    }

    public bool IsCheckedClient
    {
      get => this._isCheckedClient;
      set
      {
        this._isCheckedClient = value;
        this.OnPropertyChanged(nameof (IsCheckedClient));
      }
    }

    public ICommand SelectedStorage
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FrmSelectedStorage frmSelectedStorage = new FrmSelectedStorage();
          List<Storages.Storage> collection = new List<Storages.Storage>((IEnumerable<Storages.Storage>) this.StorageListFilter);
          ref List<Storages.Storage> local = ref collection;
          if (!frmSelectedStorage.GetListSelectedStorages(ref local))
            return;
          this.StorageListFilter = new List<Storages.Storage>((IEnumerable<Storages.Storage>) collection);
        }));
      }
    }

    public ICommand PrintOrdersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ClientOrderListViewModel.ClientOrderInfo> list = ((IEnumerable) obj).Cast<ClientOrderListViewModel.ClientOrderInfo>().ToList<ClientOrderListViewModel.ClientOrderInfo>();
          if (!list.Any<ClientOrderListViewModel.ClientOrderInfo>())
            MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_Требуется_выбрать_хотя_бы_одно_действие_для_редактирования_товаров_);
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateClientOrderReport(list.Select<ClientOrderListViewModel.ClientOrderInfo, Document>((Func<ClientOrderListViewModel.ClientOrderInfo, Document>) (x => x.Document))), this.AuthUser);
        }));
      }
    }

    public string ButtonContentStorage
    {
      get => this._buttonContentStorage;
      set
      {
        this._buttonContentStorage = value;
        this.OnPropertyChanged(nameof (ButtonContentStorage));
      }
    }

    public Dictionary<GlobalDictionaries.DocumentsStatuses, string> Statuses { get; } = new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
    {
      {
        GlobalDictionaries.DocumentsStatuses.None,
        Translate.ClientOrderListViewModel_Все_статусы
      }
    };

    public GlobalDictionaries.DocumentsStatuses SelectedStatus
    {
      get => this._selectedStatus;
      set
      {
        this._selectedStatus = value;
        this.SearchForFilter();
      }
    }

    private Gbs.Core.Entities.Users.User AuthUser { get; set; }

    private List<ClientOrderListViewModel.ClientOrderInfo> CachedDbOrder { get; set; } = new List<ClientOrderListViewModel.ClientOrderInfo>();

    private IEnumerable<Storages.Storage> AllListStorage { get; }

    private List<Storages.Storage> StorageListFilter
    {
      get => this._storageListFilter;
      set
      {
        this._storageListFilter = value;
        this.OnPropertyChanged(nameof (StorageListFilter));
        int count = this._storageListFilter.Count;
        this.ButtonContentStorage = count == 0 || count == this.AllListStorage.Count<Storages.Storage>() ? (count != 1 ? Translate.WaybillsViewModel_Все_склады : this._storageListFilter.First<Storages.Storage>().Name) : (count == 1 ? this._storageListFilter.First<Storages.Storage>().Name : Translate.WaybillsViewModel_Складов_ + count.ToString());
        this.SearchForFilter();
      }
    }

    public ClientOrderListViewModel(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      this.LoadDocuments();
      this.AllListStorage = (IEnumerable<Storages.Storage>) new List<Storages.Storage>(Storages.GetStorages().Where<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted)));
      this.Statuses = this.Statuses.Concat<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((IEnumerable<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>) GlobalDictionaries.ClientOrderStatusDictionary).ToDictionary<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, GlobalDictionaries.DocumentsStatuses, string>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, GlobalDictionaries.DocumentsStatuses>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, string>) (v => v.Value));
    }

    public ClientOrderListViewModel()
    {
    }

    private void LoadDocuments()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ClientOrderListViewModel_Загрузка_списаний);
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.ClientOrder);
          List<Client> clients = new ClientsRepository(dataBase).GetActiveItems();
          List<Gbs.Core.Entities.Users.User> users = new UsersRepository(dataBase).GetAllItems().ToList<Gbs.Core.Entities.Users.User>();
          this.CachedDbOrder.Clear();
          this.CachedDbOrder.AddRange(itemsWithFilter.Select<Document, ClientOrderListViewModel.ClientOrderInfo>((Func<Document, ClientOrderListViewModel.ClientOrderInfo>) (x => new ClientOrderListViewModel.ClientOrderInfo(x, clients.ToList<Client>(), users))));
          this.CachedDbOrder = this.CachedDbOrder.OrderByDescending<ClientOrderListViewModel.ClientOrderInfo, DateTime>((Func<ClientOrderListViewModel.ClientOrderInfo, DateTime>) (x => x.Document.DateTime)).ToList<ClientOrderListViewModel.ClientOrderInfo>();
          this.ClientOrderList = new ObservableCollection<ClientOrderListViewModel.ClientOrderInfo>(this.CachedDbOrder);
          this.SearchForFilter();
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки журнала заказов резервов");
        progressBar.Close();
      }
    }

    public ICommand JournalFilterCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
    }

    private void SearchForFilter()
    {
      IEnumerable<ClientOrderListViewModel.ClientOrderInfo> clientOrderInfos = this.CachedDbOrder.Where<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x =>
      {
        DateTime dateTime1 = x.Document.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.Document.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.StorageListFilter.Any<Storages.Storage>())
        clientOrderInfos = clientOrderInfos.Where<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.Document.Storage.Uid))));
      if (this.SelectedStatus != GlobalDictionaries.DocumentsStatuses.None)
        clientOrderInfos = clientOrderInfos.Where<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x => x.Document.Status == this.SelectedStatus));
      if (this.IsCheckedClient)
        clientOrderInfos = clientOrderInfos.Where<ClientOrderListViewModel.ClientOrderInfo>((Func<ClientOrderListViewModel.ClientOrderInfo, bool>) (x =>
        {
          Guid contractorUid = x.Document.ContractorUid;
          Guid? uid = this.Client?.Uid;
          return uid.HasValue && contractorUid == uid.GetValueOrDefault();
        }));
      this.ClientOrderList = new ObservableCollection<ClientOrderListViewModel.ClientOrderInfo>(clientOrderInfos);
      this.OnPropertyChanged("TotalSumOrder");
    }

    public class ClientOrderInfo
    {
      public Document Document { get; set; }

      public string ClientName { get; set; }

      public string UserName { get; set; }

      public Decimal TotalCountGood
      {
        get
        {
          Document document = this.Document;
          return document == null ? 0M : document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
        }
      }

      public Decimal TotalSumPayment
      {
        get
        {
          Document document = this.Document;
          return document == null ? 0M : document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        }
      }

      public Decimal Sum => SaleHelper.GetSumDocument(this.Document);

      public string Status
      {
        get
        {
          return GlobalDictionaries.ClientOrderStatusDictionary.SingleOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x =>
          {
            int key = (int) x.Key;
            GlobalDictionaries.DocumentsStatuses? status = this.Document?.Status;
            int valueOrDefault = (int) status.GetValueOrDefault();
            return key == valueOrDefault & status.HasValue;
          })).Value ?? "";
        }
      }

      public ClientOrderViewModel.OptionalClientOrder OptionalOrder
      {
        get
        {
          Document document = this.Document;
          string str = (document != null ? document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid))?.Value?.ToString() : (string) null) ?? "";
          if (str.IsNullOrEmpty())
            return new ClientOrderViewModel.OptionalClientOrder();
          ClientOrderViewModel.OptionalClientOrder optionalOrder = JsonConvert.DeserializeObject<ClientOrderViewModel.OptionalClientOrder>(str);
          optionalOrder.ActualityOrderDate = optionalOrder.IsActualityOrder ? optionalOrder.ActualityOrderDate : new DateTime?();
          return optionalOrder;
        }
      }

      public ClientOrderInfo(Document document, List<Client> clients, List<Gbs.Core.Entities.Users.User> users)
      {
        this.Document = document;
        if (clients != null)
          this.ClientName = document.ContractorUid == Guid.Empty ? Translate.PaymentsActionsViewModel_Не_указан : clients.FirstOrDefault<Client>((Func<Client, bool>) (x => x.Uid == document.ContractorUid))?.Name;
        if (users == null)
          return;
        this.UserName = document.UserUid == Guid.Empty ? Translate.PaymentsActionsViewModel_Не_указан : users.FirstOrDefault<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Uid == document.UserUid))?.Alias;
      }

      public ClientOrderInfo()
      {
      }

      public ClientOrderInfo(Document document, Client client, Gbs.Core.Entities.Users.User user)
      {
        this.Document = document;
        if (client != null)
          this.ClientName = document.ContractorUid == Guid.Empty ? Translate.PaymentsActionsViewModel_Не_указан : client.Name;
        if (user == null)
          return;
        this.UserName = document.UserUid == Guid.Empty ? Translate.PaymentsActionsViewModel_Не_указан : user.Alias;
      }
    }
  }
}
