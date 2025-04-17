// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.MovingStorage.SendStorageJournalViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
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
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.MovingStorage
{
  public partial class SendStorageJournalViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Document document;
          Document childDoc;
          if (!new SendStorageCardViewModel().ShowCard(Guid.Empty, out document, out childDoc))
            return;
          SendStorageJournalViewModel.CachedDbSendStorage.Add(new SendStorageJournalViewModel.SendStorageJournalItem(document, childDoc));
          this.SearchForFilter();
        }));
      }
    }

    public ICommand PrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
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
            SendStorageJournalViewModel.SendStorageJournalItem storageJournalItem = source.Cast<SendStorageJournalViewModel.SendStorageJournalItem>().Single<SendStorageJournalViewModel.SendStorageJournalItem>();
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForMoveStorageDocument(storageJournalItem.SendStorageDocument, storageJournalItem.ThisStorageDocument.Storage.Name), (Users.User) null);
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
            (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.DeleteMoveStorage);
            if (!access.Result || MessageBoxHelper.Question(Translate.SendStorageJournalViewModel_DeleteCommand_Уверены__что_хотите_удалить_данное_перемещение__Товары_вернутся_на_склад__с_которого_было_сделано_перемещение_) != MessageBoxResult.Yes)
              return;
            SendStorageJournalViewModel.SendStorageJournalItem doc = source.Cast<SendStorageJournalViewModel.SendStorageJournalItem>().Single<SendStorageJournalViewModel.SendStorageJournalItem>();
            using (DataBase dataBase = Data.GetDataBase())
            {
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              SendStorageJournalViewModel.SendStorageJournalItem storageJournalItem = doc.Clone<SendStorageJournalViewModel.SendStorageJournalItem>();
              doc.ThisStorageDocument.IsDeleted = true;
              if (!documentsRepository.Delete(doc.SendStorageDocument) || !documentsRepository.Save(doc.ThisStorageDocument))
                return;
              doc.SendStorageDocument.IsDeleted = true;
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) storageJournalItem.SendStorageDocument, (IEntity) doc.SendStorageDocument, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, access.User), false);
              SendStorageJournalViewModel.CachedDbSendStorage.RemoveAll((Predicate<SendStorageJournalViewModel.SendStorageJournalItem>) (x => x.SendStorageDocument.Uid == doc.SendStorageDocument.Uid));
              this.SearchForFilter();
            }
          }
        }));
      }
    }

    public ObservableCollection<SendStorageJournalViewModel.SendStorageJournalItem> SendStorageItemsList { get; set; } = new ObservableCollection<SendStorageJournalViewModel.SendStorageJournalItem>();

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

    public ICommand FilterJournalCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
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
        return this.SendStorageItemsList.Sum<SendStorageJournalViewModel.SendStorageJournalItem>((Func<SendStorageJournalViewModel.SendStorageJournalItem, Decimal>) (x => x.TotalGoods));
      }
    }

    public Decimal TotalSaleSum
    {
      get
      {
        return this.SendStorageItemsList.Sum<SendStorageJournalViewModel.SendStorageJournalItem>((Func<SendStorageJournalViewModel.SendStorageJournalItem, Decimal>) (x => x.TotalSaleSum));
      }
    }

    private static List<SendStorageJournalViewModel.SendStorageJournalItem> CachedDbSendStorage { get; set; } = new List<SendStorageJournalViewModel.SendStorageJournalItem>();

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

    private void LoadDocuments()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SendWaybillsJournalViewModel_LoadDocuments_Загрузка_журнала_перемещений);
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
          List<Document> itemsWithFilter = documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.MoveStorage);
          List<Document> dlChild = documentsRepository.GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 15 && !x.IS_DELETED && x.PARENT_UID != Guid.Empty)));
          SendStorageJournalViewModel.CachedDbSendStorage.Clear();
          SendStorageJournalViewModel.CachedDbSendStorage.AddRange(itemsWithFilter.Select<Document, SendStorageJournalViewModel.SendStorageJournalItem>((Func<Document, SendStorageJournalViewModel.SendStorageJournalItem>) (x => new SendStorageJournalViewModel.SendStorageJournalItem(x, dlChild.SingleOrDefault<Document>((Func<Document, bool>) (c => c.ParentUid == x.Uid))))));
          SendStorageJournalViewModel.CachedDbSendStorage = new List<SendStorageJournalViewModel.SendStorageJournalItem>(SendStorageJournalViewModel.CachedDbSendStorage.Where<SendStorageJournalViewModel.SendStorageJournalItem>((Func<SendStorageJournalViewModel.SendStorageJournalItem, bool>) (x => x.ThisStorageDocument != null)));
          SendStorageJournalViewModel.CachedDbSendStorage = SendStorageJournalViewModel.CachedDbSendStorage.OrderByDescending<SendStorageJournalViewModel.SendStorageJournalItem, DateTime>((Func<SendStorageJournalViewModel.SendStorageJournalItem, DateTime>) (x => x.ThisStorageDocument.DateTime)).ToList<SendStorageJournalViewModel.SendStorageJournalItem>();
          this.OnPropertyChanged("SendStorageItemsList");
          this.OnPropertyChanged("TotalCount");
          this.OnPropertyChanged("TotalSaleSum");
          this.SearchForFilter();
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки журнала перемещений");
        progressBar.Close();
      }
    }

    private void SearchForFilter()
    {
      IEnumerable<SendStorageJournalViewModel.SendStorageJournalItem> source = SendStorageJournalViewModel.CachedDbSendStorage.Where<SendStorageJournalViewModel.SendStorageJournalItem>((Func<SendStorageJournalViewModel.SendStorageJournalItem, bool>) (x =>
      {
        DateTime dateTime1 = x.ThisStorageDocument.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.ThisStorageDocument.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.StorageListFilter.Any<Storages.Storage>())
        source = source.Where<SendStorageJournalViewModel.SendStorageJournalItem>((Func<SendStorageJournalViewModel.SendStorageJournalItem, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.ThisStorageDocument.Storage.Uid || s.Uid == x.SendStorageDocument.Storage.Uid))));
      this.SendStorageItemsList = new ObservableCollection<SendStorageJournalViewModel.SendStorageJournalItem>((IEnumerable<SendStorageJournalViewModel.SendStorageJournalItem>) source.OrderByDescending<SendStorageJournalViewModel.SendStorageJournalItem, DateTime>((Func<SendStorageJournalViewModel.SendStorageJournalItem, DateTime>) (x => x.ThisStorageDocument.DateTime)));
      this.OnPropertyChanged("SendStorageItemsList");
      this.OnPropertyChanged("TotalCount");
      this.OnPropertyChanged("TotalSaleSum");
    }

    public void ShowListSendStorage()
    {
      using (DataBase dataBase = Data.GetDataBase())
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      this.LoadDocuments();
      this.FormToSHow = (WindowWithSize) new FrmMovingStorageList(this);
      this.ShowForm();
    }

    public class SendStorageJournalItem : ViewModel
    {
      private Document _sendStorageDocument;
      private Document _thisStorageDocument;

      public Document SendStorageDocument
      {
        get => this._sendStorageDocument;
        set
        {
          this._sendStorageDocument = value;
          this.OnPropertyChanged(nameof (SendStorageDocument));
          this.OnPropertyChanged("TotalGoods");
          this.OnPropertyChanged("TotalSaleSum");
        }
      }

      public Document ThisStorageDocument
      {
        get => this._thisStorageDocument;
        set
        {
          this._thisStorageDocument = value;
          this.OnPropertyChanged(nameof (ThisStorageDocument));
          this.OnPropertyChanged("TotalGoods");
          this.OnPropertyChanged("TotalSaleSum");
        }
      }

      public Decimal TotalGoods
      {
        get => this.ThisStorageDocument.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
      }

      public Decimal TotalSaleSum
      {
        get
        {
          return this.ThisStorageDocument.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity * x.SellPrice));
        }
      }

      public string UserAlias
      {
        get
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            Users.User byUid = new UsersRepository(dataBase).GetByUid(this.ThisStorageDocument.UserUid);
            return byUid == null ? string.Empty : byUid.Alias;
          }
        }
      }

      public SendStorageJournalItem(Document documentSend, Document documentThis)
      {
        this.SendStorageDocument = documentSend;
        this.ThisStorageDocument = documentThis;
      }
    }
  }
}
