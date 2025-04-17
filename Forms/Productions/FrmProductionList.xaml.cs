// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ProductionListViewModel
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
namespace Gbs.Forms
{
  public partial class ProductionListViewModel : ViewModelWithForm
  {
    private string _buttonContentStorage = Translate.WaybillsViewModel_Все_склады;
    private DateTime _dateFinish = DateTime.Now;
    private DateTime _dateStart = DateTime.Now.AddYears(-1);
    private List<Storages.Storage> _storageListFilter = new List<Storages.Storage>();
    private bool _isAlcohol;

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Document document;
          List<Document> docs;
          if (!new ProductionCardViewModel().ShowCard(Guid.Empty, out document, out docs))
            return;
          ProductionListViewModel.CachedDbProduction.Add(new ProductionListViewModel.ProductionListItem(document, docs));
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
          if (count < 1)
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Требуется_выбрать_запись);
          else if (count > 1)
          {
            MessageBoxHelper.Warning(Translate.SaleJournalViewModel_Можно_выбрать_только_одну_запись);
          }
          else
          {
            List<Document> productionItems = source.Cast<ProductionListViewModel.ProductionListItem>().Single<ProductionListViewModel.ProductionListItem>().ProductionItems;
            using (DataBase dataBase = Data.GetDataBase())
            {
              List<Document> setsDocuments = new List<Document>();
              foreach (Document document in productionItems)
                setsDocuments.AddRange((IEnumerable<Document>) new DocumentsRepository(dataBase).GetByParentUid(document.Uid));
              new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateProductionReport(productionItems, setsDocuments), (Users.User) null);
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
            ProductionListViewModel.ProductionListItem doc = source.Cast<ProductionListViewModel.ProductionListItem>().Single<ProductionListViewModel.ProductionListItem>();
            if (doc.Document.Type == GlobalDictionaries.DocumentsTypes.BeerProductionList)
            {
              MessageBoxHelper.Warning("На данный момент нельзя удалять документы вскрытия пивной продукции.");
            }
            else
            {
              (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.DeleteProduction);
              if (!access.Result || MessageBoxHelper.Question(Translate.ProductionListViewModel_DeleteCommand_Уверены__что_хотите_удалить_данное_производство__Товары__из_которых_было_сделано_производство_вернутся_на_склад_) != MessageBoxResult.Yes)
                return;
              using (DataBase dataBase = Data.GetDataBase())
              {
                DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
                Document oldItem = doc.Document.Clone<Document>();
                doc.Document.IsDeleted = true;
                doc.ProductionItems.ForEach((Action<Document>) (x => x.IsDeleted = true));
                List<Document> itemsList = new List<Document>();
                foreach (Document productionItem in doc.ProductionItems)
                  itemsList.AddRange((IEnumerable<Document>) new DocumentsRepository(dataBase).GetByParentUid(productionItem.Uid));
                itemsList.ForEach((Action<Document>) (x => x.IsDeleted = true));
                if (!documentsRepository.Save(doc.Document))
                  return;
                documentsRepository.Save(doc.ProductionItems);
                documentsRepository.Delete(itemsList);
                ProductionListViewModel.CachedDbProduction.RemoveAll((Predicate<ProductionListViewModel.ProductionListItem>) (x => x.Document.Uid == doc.Document.Uid));
                this.SearchForFilter();
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) doc.Document, ActionType.Delete, GlobalDictionaries.EntityTypes.Document, access.User), false);
              }
            }
          }
        }));
      }
    }

    public ObservableCollection<ProductionListViewModel.ProductionListItem> ProductionList { get; set; } = new ObservableCollection<ProductionListViewModel.ProductionListItem>();

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
        return this.ProductionList.Sum<ProductionListViewModel.ProductionListItem>((Func<ProductionListViewModel.ProductionListItem, Decimal>) (x => x.TotalGoods));
      }
    }

    public Decimal TotalSaleSum
    {
      get
      {
        return this.ProductionList.Sum<ProductionListViewModel.ProductionListItem>((Func<ProductionListViewModel.ProductionListItem, Decimal>) (x => x.TotalSaleSum));
      }
    }

    private static List<ProductionListViewModel.ProductionListItem> CachedDbProduction { get; set; } = new List<ProductionListViewModel.ProductionListItem>();

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
          List<Document> itemsWithFilter = documentsRepository.GetItemsWithFilter(this._isAlcohol ? GlobalDictionaries.DocumentsTypes.BeerProductionList : GlobalDictionaries.DocumentsTypes.ProductionList);
          List<Document> dlChild = documentsRepository.GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == (int) (this._isAlcohol ? GlobalDictionaries.DocumentsTypes.BeerProductionItem : GlobalDictionaries.DocumentsTypes.ProductionItem) && !x.IS_DELETED && x.PARENT_UID != Guid.Empty)));
          ProductionListViewModel.CachedDbProduction.Clear();
          ProductionListViewModel.CachedDbProduction.AddRange(itemsWithFilter.Select<Document, ProductionListViewModel.ProductionListItem>((Func<Document, ProductionListViewModel.ProductionListItem>) (x => new ProductionListViewModel.ProductionListItem(x, dlChild.Where<Document>((Func<Document, bool>) (c => c.ParentUid == x.Uid)).ToList<Document>()))));
          ProductionListViewModel.CachedDbProduction = new List<ProductionListViewModel.ProductionListItem>(ProductionListViewModel.CachedDbProduction.Where<ProductionListViewModel.ProductionListItem>((Func<ProductionListViewModel.ProductionListItem, bool>) (x => x.Document != null)));
          ProductionListViewModel.CachedDbProduction = ProductionListViewModel.CachedDbProduction.OrderByDescending<ProductionListViewModel.ProductionListItem, DateTime>((Func<ProductionListViewModel.ProductionListItem, DateTime>) (x => x.Document.DateTime)).ToList<ProductionListViewModel.ProductionListItem>();
          this.OnPropertyChanged("ProductionList");
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

    public ICommand FilterCommand { get; set; }

    private void SearchForFilter()
    {
      IEnumerable<ProductionListViewModel.ProductionListItem> source = ProductionListViewModel.CachedDbProduction.Where<ProductionListViewModel.ProductionListItem>((Func<ProductionListViewModel.ProductionListItem, bool>) (x =>
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
        source = source.Where<ProductionListViewModel.ProductionListItem>((Func<ProductionListViewModel.ProductionListItem, bool>) (x => this.StorageListFilter.Any<Storages.Storage>((Func<Storages.Storage, bool>) (s => s.Uid == x.Document.Storage.Uid || s.Uid == x.Document.Storage.Uid))));
      this.ProductionList = new ObservableCollection<ProductionListViewModel.ProductionListItem>((IEnumerable<ProductionListViewModel.ProductionListItem>) source.OrderByDescending<ProductionListViewModel.ProductionListItem, DateTime>((Func<ProductionListViewModel.ProductionListItem, DateTime>) (x => x.Document.DateTime)));
      this.OnPropertyChanged("ProductionList");
      this.OnPropertyChanged("TotalCount");
      this.OnPropertyChanged("TotalSaleSum");
    }

    public void Show(bool isAlcohol = false)
    {
      if (!new Authorization().GetAccess(Actions.ShowProduction).Result)
        return;
      this._isAlcohol = isAlcohol;
      using (DataBase dataBase = Data.GetDataBase())
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      this.FilterCommand = (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
      this.LoadDocuments();
      this.FormToSHow = (WindowWithSize) new FrmProductionList(this);
      if (this._isAlcohol)
        this.FormToSHow.Title = "Журнал вскрытия кег (разливное пиво)";
      this.ShowForm();
    }

    public Visibility VisibilityNoBeerBtn
    {
      get => !this._isAlcohol ? Visibility.Visible : Visibility.Collapsed;
    }

    public class ProductionListItem : ViewModel
    {
      private Document _document;

      public Document Document
      {
        get => this._document;
        set
        {
          this._document = value;
          this.OnPropertyChanged(nameof (Document));
          this.OnPropertyChanged("TotalGoods");
          this.OnPropertyChanged("TotalSaleSum");
        }
      }

      public List<Document> ProductionItems { get; set; }

      public Decimal TotalName
      {
        get
        {
          return (Decimal) this.ProductionItems.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Count<Gbs.Core.Entities.Documents.Item>();
        }
      }

      public Decimal TotalGoods
      {
        get
        {
          return this.ProductionItems.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
        }
      }

      public Decimal TotalSaleSum
      {
        get
        {
          return this.ProductionItems.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity * x.SellPrice));
        }
      }

      public string UserAlias
      {
        get
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            Users.User byUid = new UsersRepository(dataBase).GetByUid(this.Document.UserUid);
            return byUid == null ? string.Empty : byUid.Alias;
          }
        }
      }

      public ProductionListItem(Document document, List<Document> documentItems)
      {
        this.Document = document;
        this.ProductionItems = new List<Document>((IEnumerable<Document>) documentItems);
        this.OnPropertyChanged(nameof (UserAlias));
        this.OnPropertyChanged(nameof (Document));
      }
    }
  }
}
