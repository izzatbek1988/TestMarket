// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SendWaybills.SendWaybillsJournalViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Lable;
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
namespace Gbs.Forms.SendWaybills
{
  public partial class SendWaybillsJournalViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();
    private string _selectedPointSale = Translate.SendWaybillsJournalViewModel__selectedPointSale_Все_точки;

    public SendWaybillsJournalViewModel()
    {
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public static List<HomeOfficeHelper.PointInfo> Points { get; set; }

    public static List<Gbs.Core.Entities.Users.User> Users { get; set; }

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmSendWaybillCard().ShowCard(Guid.Empty, out Document _, updateAction: new Action(this.LoadDocuments))));
      }
    }

    public ICommand DeleteCommand { get; set; }

    public ICommand PrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedItems.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            SendWaybillsJournalViewModel.SendWaybillJournalItem waybillJournalItem = this.SelectedItems.First<SendWaybillsJournalViewModel.SendWaybillJournalItem>();
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForMoveDocument(waybillJournalItem.SendWaybillDocument, waybillJournalItem.PointSale), this.AuthUser);
          }
        }));
      }
    }

    public ObservableCollection<SendWaybillsJournalViewModel.SendWaybillJournalItem> SendWaybillItemsList { get; set; } = new ObservableCollection<SendWaybillsJournalViewModel.SendWaybillJournalItem>();

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

    public List<string> PointSales { get; set; } = new List<string>()
    {
      Translate.SendWaybillsJournalViewModel__selectedPointSale_Все_точки
    };

    public string SelectedPointSale
    {
      get => this._selectedPointSale;
      set
      {
        this._selectedPointSale = value;
        this.OnPropertyChanged(nameof (SelectedPointSale));
        this.SearchForFilter();
      }
    }

    public Decimal TotalCount
    {
      get
      {
        return this.SendWaybillItemsList.Sum<SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, Decimal>) (x => x.TotalCount));
      }
    }

    public Decimal TotalSaleSum
    {
      get
      {
        return this.SendWaybillItemsList.Sum<SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, Decimal>) (x => x.TotalSaleSum));
      }
    }

    private static List<SendWaybillsJournalViewModel.SendWaybillJournalItem> CachedDbSendWaybills { get; set; } = new List<SendWaybillsJournalViewModel.SendWaybillJournalItem>();

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

    public SendWaybillsJournalViewModel(Action showMoreMenu, Action showPrintMenu)
    {
      this.ShowMoreMenu = showMoreMenu;
      this.ShowPrintMenu = showPrintMenu;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteSelectedMove));
      this.FilterJournalCommand = (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
      this.LoadDocuments();
      this.PointSales.AddRange(SendWaybillsJournalViewModel.CachedDbSendWaybills.GroupBy<SendWaybillsJournalViewModel.SendWaybillJournalItem, string>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, string>) (x => x.PointSale)).Select<IGrouping<string, SendWaybillsJournalViewModel.SendWaybillJournalItem>, string>((Func<IGrouping<string, SendWaybillsJournalViewModel.SendWaybillJournalItem>, string>) (x => x.Key)));
    }

    private void DeleteSelectedMove(object obj)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteMoveWaybill) && !new Authorization().GetAccess(Actions.DeleteMoveWaybill).Result)
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
            if (MessageBoxHelper.Question(Translate.SendWaybillsJournalViewModel_УвереныЧтоХотитеУдалитьВыбранноеПеремещение) != MessageBoxResult.Yes)
              return;
            Document doc = source.Cast<SendWaybillsJournalViewModel.SendWaybillJournalItem>().First<SendWaybillsJournalViewModel.SendWaybillJournalItem>().SendWaybillDocument;
            DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
            Document oldItem = doc.Clone<Document>();
            Document document = doc;
            if (!documentsRepository.Delete(document))
              return;
            doc.IsDeleted = true;
            ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) doc, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
            SendWaybillsJournalViewModel.CachedDbSendWaybills.RemoveAll((Predicate<SendWaybillsJournalViewModel.SendWaybillJournalItem>) (x => x.SendWaybillDocument.Uid == doc.Uid));
            this.SearchForFilter();
          }
        }
      }
    }

    private void LoadDocuments()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SendWaybillsJournalViewModel_LoadDocuments_Загрузка_журнала_перемещений);
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Move);
          SendWaybillsJournalViewModel.CachedDbSendWaybills.Clear();
          SendWaybillsJournalViewModel.Points = new List<HomeOfficeHelper.PointInfo>((IEnumerable<HomeOfficeHelper.PointInfo>) HomeOfficeHelper.GetPointFromCloud());
          SendWaybillsJournalViewModel.Users = new UsersRepository(dataBase).GetAllItems();
          SendWaybillsJournalViewModel.CachedDbSendWaybills.AddRange(itemsWithFilter.Select<Document, SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<Document, SendWaybillsJournalViewModel.SendWaybillJournalItem>) (x => new SendWaybillsJournalViewModel.SendWaybillJournalItem(x))));
          SendWaybillsJournalViewModel.CachedDbSendWaybills = SendWaybillsJournalViewModel.CachedDbSendWaybills.OrderByDescending<SendWaybillsJournalViewModel.SendWaybillJournalItem, DateTime>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, DateTime>) (x => x.SendWaybillDocument.DateTime)).ToList<SendWaybillsJournalViewModel.SendWaybillJournalItem>();
          this.SendWaybillItemsList = new ObservableCollection<SendWaybillsJournalViewModel.SendWaybillJournalItem>(SendWaybillsJournalViewModel.CachedDbSendWaybills);
          this.OnPropertyChanged("SendWaybillItemsList");
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

    public ICommand FilterJournalCommand { get; set; }

    private void SearchForFilter()
    {
      IEnumerable<SendWaybillsJournalViewModel.SendWaybillJournalItem> source = SendWaybillsJournalViewModel.CachedDbSendWaybills.Where<SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, bool>) (x =>
      {
        DateTime dateTime1 = x.SendWaybillDocument.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.SendWaybillDocument.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.StorageListFilter.Any<Storages.Storage>())
        source = source.Where<SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.SendWaybillDocument.Storage.Uid))));
      if (this.SelectedPointSale != Translate.SendWaybillsJournalViewModel__selectedPointSale_Все_точки)
        source = source.Where<SendWaybillsJournalViewModel.SendWaybillJournalItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, bool>) (x => x.PointSale == this.SelectedPointSale));
      this.SendWaybillItemsList = new ObservableCollection<SendWaybillsJournalViewModel.SendWaybillJournalItem>((IEnumerable<SendWaybillsJournalViewModel.SendWaybillJournalItem>) source.OrderByDescending<SendWaybillsJournalViewModel.SendWaybillJournalItem, DateTime>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, DateTime>) (x => x.SendWaybillDocument.DateTime)));
      this.OnPropertyChanged("SendWaybillItemsList");
      this.OnPropertyChanged("TotalCount");
      this.OnPropertyChanged("TotalSaleSum");
    }

    public static string MoreMenuKey => "MoreMenu";

    public static string PrintMenuKey => "PrintMenu";

    private List<SendWaybillsJournalViewModel.SendWaybillJournalItem> SelectedItems { get; set; } = new List<SendWaybillsJournalViewModel.SendWaybillJournalItem>();

    public Action ShowMoreMenu { get; set; }

    public Action ShowPrintMenu { get; set; }

    public ICommand ShowMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<SendWaybillsJournalViewModel.SendWaybillJournalItem> list = ((IEnumerable) obj).Cast<SendWaybillsJournalViewModel.SendWaybillJournalItem>().ToList<SendWaybillsJournalViewModel.SendWaybillJournalItem>();
          if (!list.Any<SendWaybillsJournalViewModel.SendWaybillJournalItem>())
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            this.SelectedItems = list;
            this.ShowMoreMenu();
          }
        }));
      }
    }

    public ICommand ShowPrintMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<SendWaybillsJournalViewModel.SendWaybillJournalItem> list = ((IEnumerable) obj).Cast<SendWaybillsJournalViewModel.SendWaybillJournalItem>().ToList<SendWaybillsJournalViewModel.SendWaybillJournalItem>();
          if (!list.Any<SendWaybillsJournalViewModel.SendWaybillJournalItem>())
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          }
          else
          {
            this.SelectedItems = list;
            this.ShowPrintMenu();
          }
        }));
      }
    }

    public ICommand RepeatSendWaybillCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else if (this.SelectedItems.Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Можно_выбрать_только_одну_запись);
          }
          else
          {
            if (MessageBoxHelper.Question(Translate.SendWaybillsJournalViewModel_УвереныЧТоХОтитеПовторитьПеремещение) != MessageBoxResult.Yes)
              return;
            Document sendWaybillDocument = this.SelectedItems.First<SendWaybillsJournalViewModel.SendWaybillJournalItem>().SendWaybillDocument;
            if (!MoveHelper.CreateMoveFile(sendWaybillDocument, sendWaybillDocument.ContractorUid))
              return;
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.GlobalDictionaries_Перемещение,
              Text = Translate.SendWaybillsJournalViewModel_Накладная_повторно_отправлена_в_точку_получатель
            });
          }
        }));
      }
    }

    public ICommand PrintLableForWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLablePrint().Print(LablePrintViewModel.Types.Labels, this.SelectedItems.SelectMany<SendWaybillsJournalViewModel.SendWaybillJournalItem, BasketItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, IEnumerable<BasketItem>>) (d => d.SendWaybillDocument.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage, x.Quantity))))).ToList<BasketItem>())));
      }
    }

    public ICommand PrintTagForWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmLablePrint().Print(LablePrintViewModel.Types.PriceTags, this.SelectedItems.SelectMany<SendWaybillsJournalViewModel.SendWaybillJournalItem, BasketItem>((Func<SendWaybillsJournalViewModel.SendWaybillJournalItem, IEnumerable<BasketItem>>) (d => d.SendWaybillDocument.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.GoodStock.ModificationUid, x.SellPrice, x.GoodStock.Storage))))).ToList<BasketItem>())));
      }
    }

    public class SendWaybillJournalItem : ViewModel
    {
      private Document _sendWaybillDocument;

      public Document SendWaybillDocument
      {
        get => this._sendWaybillDocument;
        set
        {
          this._sendWaybillDocument = value;
          this.OnPropertyChanged(nameof (SendWaybillDocument));
          this.OnPropertyChanged("TotalCount");
          this.OnPropertyChanged("TotalName");
          this.OnPropertyChanged("TotalSaleSum");
        }
      }

      public Decimal TotalSaleSum
      {
        get
        {
          return this.SendWaybillDocument.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity * x.SellPrice));
        }
      }

      public int TotalName => this.SendWaybillDocument.Items.Count;

      public Decimal TotalCount
      {
        get => this.SendWaybillDocument.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
      }

      public string Status => "ToDo";

      public string PointSale { get; set; }

      public string UserAlias
      {
        get
        {
          Gbs.Core.Entities.Users.User user = SendWaybillsJournalViewModel.Users.FirstOrDefault<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Uid == this.SendWaybillDocument.UserUid));
          return user != null ? user.Alias : string.Empty;
        }
      }

      public SendWaybillJournalItem(Document document)
      {
        this.SendWaybillDocument = document;
        this.PointSale = SendWaybillsJournalViewModel.Points.FirstOrDefault<HomeOfficeHelper.PointInfo>((Func<HomeOfficeHelper.PointInfo, bool>) (x => x.DbUid == document.ContractorUid))?.InfoDataBase.NameDataBase ?? "";
      }
    }
  }
}
