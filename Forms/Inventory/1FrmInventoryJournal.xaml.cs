// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Inventory.InventoryJournalViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Inventory
{
  public class InventoryJournalViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private GlobalDictionaries.DocumentsStatuses _statusSelected;
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();

    public Users.User AuthUser { get; set; }

    public ICommand EditInventoryCommand { get; set; }

    public ICommand AddInventoryCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmInventoryCard_v2().ShowCard((Document) null, new Func<bool>(this.LoadInventories), this.AuthUser)));
      }
    }

    public ICommand DeleteInventoryCommand { get; set; }

    public ICommand PrintInventoryCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<InventoryJournalViewModel.InventoryJournalItem> list = ((IEnumerable) obj).Cast<InventoryJournalViewModel.InventoryJournalItem>().ToList<InventoryJournalViewModel.InventoryJournalItem>();
          if (!list.Any<InventoryJournalViewModel.InventoryJournalItem>() || list.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.WriteOffJournalViewModel_Необходимо_выбрать_только_одну_запись);
          }
          else
          {
            using (DataBase dataBase = Data.GetDataBase())
            {
              bool access = new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ViewStock);
              new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForInventory(list.Single<InventoryJournalViewModel.InventoryJournalItem>().InventoryDocument, access), this.AuthUser);
            }
          }
        }));
      }
    }

    public List<InventoryJournalViewModel.InventoryJournalItem> InventoriesItemsList { get; set; } = new List<InventoryJournalViewModel.InventoryJournalItem>();

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

    public Dictionary<GlobalDictionaries.DocumentsStatuses, string> DictionaryStatus
    {
      get => GlobalDictionaries.DocumentStatusesDictionary;
    }

    public GlobalDictionaries.DocumentsStatuses StatusSelected
    {
      get => this._statusSelected;
      set
      {
        this._statusSelected = value;
        this.OnPropertyChanged(nameof (StatusSelected));
        this.SearchForFilter();
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

    public string ButtonContentStorage
    {
      get => this._buttonContentStorage;
      set
      {
        this._buttonContentStorage = value;
        this.OnPropertyChanged(nameof (ButtonContentStorage));
      }
    }

    private static List<InventoryJournalViewModel.InventoryJournalItem> CachedDbInventory { get; set; } = new List<InventoryJournalViewModel.InventoryJournalItem>();

    private IEnumerable<Storages.Storage> AllListStorage { get; set; }

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

    public InventoryJournalViewModel()
    {
      this.LoadInventoriesCommand = (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
    }

    public void InitForm()
    {
      using (DataBase dataBase = Data.GetDataBase())
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      this.EditInventoryCommand = (ICommand) new RelayCommand(new Action<object>(this.LoadSelectedInventory));
      this.DeleteInventoryCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteSelectedInventories));
      this.LoadInventories();
    }

    private void LoadSelectedInventory(object obj)
    {
      if (obj == null)
      {
        MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
      }
      else
      {
        List<InventoryJournalViewModel.InventoryJournalItem> list = ((IEnumerable) obj).Cast<InventoryJournalViewModel.InventoryJournalItem>().ToList<InventoryJournalViewModel.InventoryJournalItem>();
        if (list.Count != 1)
          MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Должна_быть_выбрана_только_одна_запись);
        else if (list.Single<InventoryJournalViewModel.InventoryJournalItem>().InventoryDocument.Status != GlobalDictionaries.DocumentsStatuses.Draft)
          MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Редактировать_можно_только_инвентаризации__находящиеся_в_статусе__Черновик_);
        else
          new FrmInventoryCard_v2().ShowCard(list.Single<InventoryJournalViewModel.InventoryJournalItem>().InventoryDocument, new Func<bool>(this.LoadInventories), this.AuthUser);
      }
    }

    private void DeleteSelectedInventories(object obj)
    {
      if (obj == null)
      {
        MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
      }
      else
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteInventory) && !new Authorization().GetAccess(Actions.DeleteInventory).Result)
            return;
          List<InventoryJournalViewModel.InventoryJournalItem> list = ((IEnumerable) obj).Cast<InventoryJournalViewModel.InventoryJournalItem>().ToList<InventoryJournalViewModel.InventoryJournalItem>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            Document selectedItem = list.Single<InventoryJournalViewModel.InventoryJournalItem>().InventoryDocument;
            if (InventoryJournalViewModel.CachedDbInventory.Where<InventoryJournalViewModel.InventoryJournalItem>((Func<InventoryJournalViewModel.InventoryJournalItem, bool>) (x => x.InventoryDocument.Uid != selectedItem.Uid && x.InventoryDocument.DateTime > selectedItem.DateTime && x.InventoryDocument.Status == GlobalDictionaries.DocumentsStatuses.Close && selectedItem.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => x.InventoryDocument.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (g => i.GoodUid == g.GoodUid)))))).ToList<InventoryJournalViewModel.InventoryJournalItem>().Any<InventoryJournalViewModel.InventoryJournalItem>())
            {
              MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_DeleteSelectedInventories_Данный_акт_невозможно_удалить__так_как_по_товарам_из_этого_акта_уже_проводились_повторные_инвентаризации_);
            }
            else
            {
              if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list.Count), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              foreach (InventoryJournalViewModel.InventoryJournalItem source in list)
              {
                InventoryJournalViewModel.InventoryJournalItem inventoryJournalItem = source.Clone<InventoryJournalViewModel.InventoryJournalItem>();
                documentsRepository.Delete(source.InventoryDocument);
                source.InventoryDocument.IsDeleted = true;
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) inventoryJournalItem.InventoryDocument, (IEntity) source.InventoryDocument, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
              }
              this.LoadInventories();
            }
          }
        }
      }
    }

    private bool LoadInventories()
    {
      try
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.InventoryJournalViewModel_Загрузка_журнала_инвентаризаций);
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Inventory);
          InventoryJournalViewModel.CachedDbInventory.Clear();
          InventoryJournalViewModel.CachedDbInventory.AddRange(itemsWithFilter.Select<Document, InventoryJournalViewModel.InventoryJournalItem>((Func<Document, InventoryJournalViewModel.InventoryJournalItem>) (x => new InventoryJournalViewModel.InventoryJournalItem()
          {
            InventoryDocument = x
          })));
          InventoryJournalViewModel.CachedDbInventory = InventoryJournalViewModel.CachedDbInventory.OrderByDescending<InventoryJournalViewModel.InventoryJournalItem, DateTime>((Func<InventoryJournalViewModel.InventoryJournalItem, DateTime>) (x => x.InventoryDocument.DateTime)).ToList<InventoryJournalViewModel.InventoryJournalItem>();
          this.InventoriesItemsList = new List<InventoryJournalViewModel.InventoryJournalItem>((IEnumerable<InventoryJournalViewModel.InventoryJournalItem>) InventoryJournalViewModel.CachedDbInventory);
          this.OnPropertyChanged("InventoriesItemsList");
          this.SearchForFilter();
          progressBar.Close();
          return true;
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка загрузки журнала инвентаризаций");
        return false;
      }
    }

    public ICommand LoadInventoriesCommand { get; set; }

    private void SearchForFilter()
    {
      IEnumerable<InventoryJournalViewModel.InventoryJournalItem> inventoryJournalItems = InventoryJournalViewModel.CachedDbInventory.Where<InventoryJournalViewModel.InventoryJournalItem>((Func<InventoryJournalViewModel.InventoryJournalItem, bool>) (x =>
      {
        DateTime dateTime1 = x.InventoryDocument.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.InventoryDocument.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.StorageListFilter.Any<Storages.Storage>())
        inventoryJournalItems = inventoryJournalItems.Where<InventoryJournalViewModel.InventoryJournalItem>((Func<InventoryJournalViewModel.InventoryJournalItem, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.InventoryDocument.Storage.Uid))));
      if (this.StatusSelected != GlobalDictionaries.DocumentsStatuses.None)
        inventoryJournalItems = inventoryJournalItems.Where<InventoryJournalViewModel.InventoryJournalItem>((Func<InventoryJournalViewModel.InventoryJournalItem, bool>) (x => x.InventoryDocument.Status == this.StatusSelected));
      this.InventoriesItemsList = new List<InventoryJournalViewModel.InventoryJournalItem>(inventoryJournalItems);
      this.OnPropertyChanged("InventoriesItemsList");
    }

    public class InventoryJournalItem
    {
      public Document InventoryDocument { get; set; }

      public string Status
      {
        get
        {
          return GlobalDictionaries.DocumentStatusesDictionary.Single<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x => x.Key == this.InventoryDocument.Status)).Value;
        }
      }

      public string UserAlias
      {
        get
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            Users.User byUid = new UsersRepository(dataBase).GetByUid(this.InventoryDocument.UserUid);
            return byUid == null ? string.Empty : byUid.Alias;
          }
        }
      }
    }
  }
}
