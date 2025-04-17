// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.WriteOff.WriteOffJournalViewModel
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.WriteOff
{
  public partial class WriteOffJournalViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();
    private static Dictionary<Guid, string> UserAliases = new Dictionary<Guid, string>();
    private Visibility _visibilityBuySum = Visibility.Collapsed;

    public Users.User AuthUser { get; set; }

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmWriteOffCard().ShowCard(Guid.Empty, out Document _, this.AuthUser, WriteOffJournalViewModel.CachedDbWriteOff, this.DocWaybills, new Action(this.SearchForFilter))));
      }
    }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand PrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<WriteOffJournalViewModel.WriteOffJournalItem> list = ((IEnumerable) obj).Cast<WriteOffJournalViewModel.WriteOffJournalItem>().ToList<WriteOffJournalViewModel.WriteOffJournalItem>();
          if (!list.Any<WriteOffJournalViewModel.WriteOffJournalItem>() || list.Count > 1)
            MessageBoxHelper.Warning(Translate.WriteOffJournalViewModel_Необходимо_выбрать_только_одну_запись);
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForWriteOff(list.Single<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument), this.AuthUser);
        }));
      }
    }

    public ObservableCollection<WriteOffJournalViewModel.WriteOffJournalItem> WriteOffItemsList { get; set; } = new ObservableCollection<WriteOffJournalViewModel.WriteOffJournalItem>();

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

    public Decimal TotalCount
    {
      get
      {
        return this.WriteOffItemsList.Sum<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, Decimal>) (x => x.TotalGoods));
      }
    }

    public Decimal TotalSaleSum
    {
      get
      {
        return this.WriteOffItemsList.Sum<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, Decimal>) (x => x.TotalSaleSum));
      }
    }

    public Decimal TotalBuySum
    {
      get
      {
        return this.WriteOffItemsList.Sum<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, Decimal>) (x => x.TotalBuySum));
      }
    }

    private static List<WriteOffJournalViewModel.WriteOffJournalItem> CachedDbWriteOff { get; set; } = new List<WriteOffJournalViewModel.WriteOffJournalItem>();

    private List<Document> DocWaybills { get; set; } = new List<Document>();

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

    public WriteOffJournalViewModel()
    {
      using (DataBase dataBase = Data.GetDataBase())
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      this.LoadDocuments();
      this.JournalFilerCommand = (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
      this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditWriteOffItem));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteSelectedWriteOff));
    }

    private void EditWriteOffItem(object obj)
    {
      IList source = (IList) obj;
      List<WriteOffJournalViewModel.WriteOffJournalItem> list = source != null ? source.Cast<WriteOffJournalViewModel.WriteOffJournalItem>().ToList<WriteOffJournalViewModel.WriteOffJournalItem>() : (List<WriteOffJournalViewModel.WriteOffJournalItem>) null;
      if ((list != null ? (!list.Any<WriteOffJournalViewModel.WriteOffJournalItem>() ? 1 : 0) : 1) != 0 || list.Count > 1)
      {
        MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
      }
      else
      {
        Users.User authUser = this.AuthUser.Clone();
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.EditWriteOff))
          {
            (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.EditWriteOff);
            if (!access.Result)
              return;
            authUser = access.User;
          }
          new FrmWriteOffCard().ShowCard(list.Single<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument.Uid, out Document _, authUser, WriteOffJournalViewModel.CachedDbWriteOff, this.DocWaybills, new Action(this.SearchForFilter));
        }
      }
    }

    private void DeleteSelectedWriteOff(object obj)
    {
      IList source = (IList) obj;
      List<WriteOffJournalViewModel.WriteOffJournalItem> list = source != null ? source.Cast<WriteOffJournalViewModel.WriteOffJournalItem>().ToList<WriteOffJournalViewModel.WriteOffJournalItem>() : (List<WriteOffJournalViewModel.WriteOffJournalItem>) null;
      if ((list != null ? (!list.Any<WriteOffJournalViewModel.WriteOffJournalItem>() ? 1 : 0) : 1) != 0 || list.Count > 1)
      {
        MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
      }
      else
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteWriteOff) && !new Authorization().GetAccess(Actions.DeleteWriteOff).Result || MessageBoxHelper.Show(Translate.WriteOffJournalViewModel_Удалить_выбранную_запись_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
          DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
          Document oldItem = list.First<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument.Clone<Document>();
          Document writeOffDocument = list.First<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument;
          if (documentsRepository.Delete(writeOffDocument))
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.MasterReportViewModel_Операция_выполнена,
              Text = Translate.WriteOffJournalViewModel_Документ_списания_успешно_удален
            });
            list.First<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument.IsDeleted = true;
            ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) list.First<WriteOffJournalViewModel.WriteOffJournalItem>().WriteOffDocument, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
            this.LoadDocuments();
          }
          else
            MessageBoxHelper.Error(Translate.WriteOffJournalViewModel_Не_удалось_удалить_документ_списания);
        }
      }
    }

    private void LoadDocuments()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ClientOrderListViewModel_Загрузка_списаний);
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
          List<Document> itemsWithFilter = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.WriteOff);
          this.DocWaybills = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).ToList<Document>();
          WriteOffJournalViewModel.CachedDbWriteOff.Clear();
          WriteOffJournalViewModel.CachedDbWriteOff.AddRange(itemsWithFilter.Select<Document, WriteOffJournalViewModel.WriteOffJournalItem>((Func<Document, WriteOffJournalViewModel.WriteOffJournalItem>) (x => new WriteOffJournalViewModel.WriteOffJournalItem(x, this.DocWaybills))));
          WriteOffJournalViewModel.CachedDbWriteOff = WriteOffJournalViewModel.CachedDbWriteOff.OrderByDescending<WriteOffJournalViewModel.WriteOffJournalItem, DateTime>((Func<WriteOffJournalViewModel.WriteOffJournalItem, DateTime>) (x => x.WriteOffDocument.DateTime)).ToList<WriteOffJournalViewModel.WriteOffJournalItem>();
          this.WriteOffItemsList = new ObservableCollection<WriteOffJournalViewModel.WriteOffJournalItem>(WriteOffJournalViewModel.CachedDbWriteOff);
          this.OnPropertyChanged("WriteOffItemsList");
          this.OnPropertyChanged("TotalCount");
          this.OnPropertyChanged("TotalSaleSum");
          this.SearchForFilter();
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки журнала списаний");
        progressBar.Close();
      }
    }

    public ICommand JournalFilerCommand { get; set; }

    private void SearchForFilter()
    {
      IEnumerable<WriteOffJournalViewModel.WriteOffJournalItem> writeOffJournalItems = WriteOffJournalViewModel.CachedDbWriteOff.Where<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, bool>) (x =>
      {
        DateTime dateTime1 = x.WriteOffDocument.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.WriteOffDocument.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.StorageListFilter.Any<Storages.Storage>())
        writeOffJournalItems = writeOffJournalItems.Where<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.WriteOffDocument.Storage.Uid))));
      this.WriteOffItemsList = new ObservableCollection<WriteOffJournalViewModel.WriteOffJournalItem>(writeOffJournalItems);
      this.OnPropertyChanged("WriteOffItemsList");
      this.OnPropertyChanged("TotalCount");
      this.OnPropertyChanged("TotalSaleSum");
    }

    public Visibility VisibilityBuySum
    {
      get => this._visibilityBuySum;
      set
      {
        this._visibilityBuySum = value;
        this.OnPropertyChanged(nameof (VisibilityBuySum));
      }
    }

    public class WriteOffJournalItem : ViewModel
    {
      private Document _writeOffDocument;

      public Document WriteOffDocument
      {
        get => this._writeOffDocument;
        set
        {
          this._writeOffDocument = value;
          this.OnPropertyChanged(nameof (WriteOffDocument));
          this.OnPropertyChanged("TotalGoods");
          this.OnPropertyChanged("TotalSaleSum");
          this.OnPropertyChanged("TotalBuySum");
        }
      }

      public Decimal TotalGoods
      {
        get
        {
          return this.WriteOffDocument.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
        }
      }

      public Decimal TotalSaleSum
      {
        get
        {
          return this.WriteOffDocument.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity * x.SellPrice));
        }
      }

      public Decimal TotalBuySum { get; set; }

      public string UserAlias
      {
        get
        {
          if (WriteOffJournalViewModel.UserAliases.ContainsKey(this.WriteOffDocument.UserUid))
            return WriteOffJournalViewModel.UserAliases[this.WriteOffDocument.UserUid];
          using (DataBase dataBase = Data.GetDataBase())
          {
            Users.User byUid = new UsersRepository(dataBase).GetByUid(this.WriteOffDocument.UserUid);
            string userAlias = byUid == null ? string.Empty : byUid.Alias;
            WriteOffJournalViewModel.UserAliases.Add(this.WriteOffDocument.UserUid, userAlias);
            return userAlias;
          }
        }
      }

      public WriteOffJournalItem(Document document, List<Document> waybills)
      {
        WriteOffJournalViewModel.WriteOffJournalItem writeOffJournalItem = this;
        this.WriteOffDocument = document;
        Task.Run((Action) (() => document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).ToList<Gbs.Core.Entities.Documents.Item>().ForEach((Action<Gbs.Core.Entities.Documents.Item>) (item =>
        {
          WriteOffJournalViewModel.WriteOffJournalItem writeOffJournalItem1 = writeOffJournalItem;
          Decimal totalBuySum = writeOffJournalItem.TotalBuySum;
          Document document1 = waybills.FirstOrDefault<Document>((Func<Document, bool>) (d => d.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i =>
          {
            GoodsStocks.GoodStock goodStock = i.GoodStock;
            // ISSUE: explicit non-virtual call
            Guid guid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
            Guid? uid = item.GoodStock?.Uid;
            return uid.HasValue && guid == uid.GetValueOrDefault();
          }))));
          Decimal num1 = (document1 != null ? document1.Items.Single<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (it => it.GoodStock.Uid == item.GoodStock.Uid)).BuyPrice : 0M) * item.Quantity;
          Decimal num2 = totalBuySum + num1;
          writeOffJournalItem1.TotalBuySum = num2;
        }))));
      }
    }
  }
}
