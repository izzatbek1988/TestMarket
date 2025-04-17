// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Inventory.InventoryCardViewModel_v2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Devices.Tsd;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Documents;
using Gbs.Core.ViewModels.Inventory;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms.Inventory
{
  public class InventoryCardViewModel_v2 : ViewModelWithForm, ICheckChangesViewModel
  {
    private readonly Integrations _integrationsConfig = new ConfigsRepository<Integrations>().Get();
    private ObservableCollection<GoodGroups.Group> _selectedGroups = new ObservableCollection<GoodGroups.Group>();
    private Timer _searchTimer = new Timer();
    private bool _isNewDocument;
    private Document _cloneDocument;
    private int _selectedTabIndex;
    private string _filterQueryText;
    private Visibility _dbQuantityVisibility;
    private ObservableCollection<InventoryItem> _currentItemsList = new ObservableCollection<InventoryItem>();
    private readonly Gbs.Core.Config.Devices _devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();

    public Visibility VisibilityHarvester
    {
      get
      {
        return !this._integrationsConfig.IsActiveBarcodeHarvester ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand CopyInHarvesterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string data = string.Empty;
          foreach (InventoryItem currentItems in (Collection<InventoryItem>) this.CurrentItemsList)
          {
            string str1 = ((IEnumerable<string>) currentItems.Good.Barcode.Split(new string[4]
            {
              " ",
              ",",
              ".",
              ";"
            }, StringSplitOptions.RemoveEmptyEntries)).FirstOrDefault<string>() ?? "";
            string str2 = currentItems.DisplayedName.Trim();
            string[] strArray = new string[9]
            {
              data,
              str2,
              "\t",
              str1,
              "\t",
              null,
              null,
              null,
              null
            };
            Decimal num = currentItems.BaseQuantity;
            strArray[5] = num.ToString();
            strArray[6] = "\t";
            num = currentItems.SalePrice;
            strArray[7] = num.ToString();
            strArray[8] = Other.NewLine();
            data = string.Concat(strArray);
          }
          Clipboard.SetData(DataFormats.StringFormat, (object) data);
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.GoodsCatalogModelView_Barcode_Harvester,
            Text = Translate.GoodsCatalogModelView_Данные_для_Barcode_Harvester_скопированы_в_буфер_обмена
          });
        }));
      }
    }

    public ICommand InsertHarvesterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<string[]> list1 = ((IEnumerable<string>) Clipboard.GetText().Split(new string[1]
          {
            Other.NewLine()
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string[]>((Func<string, string[]>) (x => x.Split(new char[1]
          {
            '\t'
          }, StringSplitOptions.RemoveEmptyEntries))).ToList<string[]>();
          string[] strArray = list1.FirstOrDefault<string[]>();
          if ((strArray != null ? (strArray.Length != 6 ? 1 : 0) : 1) != 0)
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.GoodsCatalogModelView_Barcode_Harvester,
              Text = Translate.InventoryDoViewModel_Данные_из_буфера_обмена_невозможно_разобрать
            });
          }
          else
          {
            IEnumerable<InventoryCardViewModel_v2.HarvesterItem> harvesterItems = list1.Select<string[], InventoryCardViewModel_v2.HarvesterItem>((Func<string[], InventoryCardViewModel_v2.HarvesterItem>) (x => new InventoryCardViewModel_v2.HarvesterItem(x)));
            List<InventoryCardViewModel_v2.HarvesterItem> source1 = new List<InventoryCardViewModel_v2.HarvesterItem>();
            List<InventoryCardViewModel_v2.HarvesterItem> source2 = new List<InventoryCardViewModel_v2.HarvesterItem>();
            foreach (InventoryCardViewModel_v2.HarvesterItem harvesterItem in harvesterItems)
            {
              InventoryCardViewModel_v2.HarvesterItem item = harvesterItem;
              List<InventoryItem> list2 = this.Inventory.Items.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.DisplayedName.Trim() == item.Name && x.Good.Barcode.Trim() == item.Barcode && x.SalePrice == item.SalePrice)).ToList<InventoryItem>();
              if (list2.Any<InventoryItem>())
              {
                if (list2.Count<InventoryItem>() == 1)
                {
                  list2.Single<InventoryItem>().Quantity = item.Quantity;
                }
                else
                {
                  list2.First<InventoryItem>().Quantity = item.Quantity;
                  foreach (InventoryItem inventoryItem in list2)
                  {
                    InventoryItem g = inventoryItem;
                    InventoryCardViewModel_v2.SetItemDbQtyColor(this.CurrentItemsList[this.CurrentItemsList.ToList<InventoryItem>().FindIndex((Predicate<InventoryItem>) (x => x.Uid == g.Uid))], Colors.Red);
                  }
                  source1.Add(item);
                }
              }
              else
                source2.Add(item);
            }
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.GoodsCatalogModelView_Barcode_Harvester,
              Text = Translate.InventoryDoViewModel_Данные_для_Barcode_Harvester_успешно_добавлены_в_программу
            });
            if (source1.Any<InventoryCardViewModel_v2.HarvesterItem>())
            {
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
              {
                Title = Translate.GoodsCatalogModelView_Barcode_Harvester,
                Text = Translate.InventoryDoViewModel_Для_некоторых_товаров_найдены_совпадения__кол_во_присвоено_первому_в_списке__проверьте_кол_во_товаров__выделенных_красным
              });
              this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.CurrentItemsList.OrderByDescending<InventoryItem, SolidColorBrush>((Func<InventoryItem, SolidColorBrush>) (x => x.Color)));
            }
            if (!source2.Any<InventoryCardViewModel_v2.HarvesterItem>())
              return;
            MessageBoxHelper.Warning(Translate.InventoryDoViewModel_Некоторые_товары_не_найдены_в_программе__проверьте_следующие_наименования_ + Other.NewLine() + string.Join(Other.NewLine(), source2.Select<InventoryCardViewModel_v2.HarvesterItem, string>((Func<InventoryCardViewModel_v2.HarvesterItem, string>) (x => x.Name + " (" + x.Barcode + ")"))));
          }
        }));
      }
    }

    public ICommand NextPageCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.NextPage()));
    }

    public ICommand PreviousPageCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.PreviousPage()));
    }

    public ICommand EditQuantityCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.Inventory.EditQuantity(obj)));
      }
    }

    public ICommand ItemDeleteCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.DeleteItems));
    }

    public ICommand ReloadBaseQuantity
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.ReloadQtyFromDb()));
    }

    public Decimal TotalFactQuantity
    {
      get
      {
        return this.CurrentItemsList.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => x.Quantity));
      }
    }

    public Decimal TotalBaseQuantity
    {
      get
      {
        return this.CurrentItemsList.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => x.BaseQuantity));
      }
    }

    public Decimal TotalFactSum
    {
      get
      {
        return this.CurrentItemsList.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => x.Quantity * x.SalePrice));
      }
    }

    public Decimal TotalBaseSum
    {
      get
      {
        return this.CurrentItemsList.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => x.BaseQuantity * x.SalePrice));
      }
    }

    public void ReCalcTotals()
    {
      this.OnPropertyChanged("TotalFactQuantity");
      this.OnPropertyChanged("TotalBaseQuantity");
      this.OnPropertyChanged("TotalFactSum");
      this.OnPropertyChanged("TotalBaseSum");
    }

    public List<Storages.Storage> AllStorages { get; } = Storages.GetStorages().Where<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted)).ToList<Storages.Storage>();

    public bool LoadGoodsWithZeroStock { get; set; }

    public ObservableCollection<GoodGroups.Group> SelectedGroups
    {
      get => this._selectedGroups;
      set
      {
        this._selectedGroups = value;
        this.OnPropertyChanged(nameof (SelectedGroups));
      }
    }

    public bool NotShowNotification { get; set; }

    public bool RequestQuantityWhenBarcodeScanned { get; set; }

    public bool EditFirstItemWhenFindSameBarcodes { get; set; }

    private void InitializeSearchTimer()
    {
      this._searchTimer = new Timer();
      this._searchTimer.Interval = 300.0;
      this._searchTimer.AutoReset = false;
      this._searchTimer.Elapsed += new ElapsedEventHandler(this._searchTimer_Elapsed);
    }

    private void _searchTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      bool flag1 = false;
      Decimal? q = new Decimal?();
      bool flag2 = BarcodeHelper.IsEan13Barcode(this._filterQueryText);
      bool flag3 = checkPrefix(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.WeightGoods);
      if (flag2 & flag3)
      {
        (Gbs.Core.Entities.Goods.Good g, Decimal w) goodW = BarcodeHelper.GetWeightItem(this._filterQueryText, this.Inventory.Items.GroupBy<InventoryItem, Guid>((Func<InventoryItem, Guid>) (x => x.Good.Uid)).Select<IGrouping<Guid, InventoryItem>, Gbs.Core.Entities.Goods.Good>((Func<IGrouping<Guid, InventoryItem>, Gbs.Core.Entities.Goods.Good>) (x => x.First<InventoryItem>().Good)).ToList<Gbs.Core.Entities.Goods.Good>());
        if (goodW.g == null)
          return;
        q = new Decimal?(goodW.w);
        flag1 = true;
        this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Good.Uid == goodW.g.Uid)).OrderByDescending<InventoryItem, DateTime>((Func<InventoryItem, DateTime>) (x => x.UpdateTime)));
      }
      else
        this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items.Where<InventoryItem>((Func<InventoryItem, bool>) (i => i.DisplayedName.Contains(this._filterQueryText, StringComparison.OrdinalIgnoreCase) || i.Good.Barcode.Contains(this._filterQueryText, StringComparison.OrdinalIgnoreCase) || i.Good.Barcodes.Any<string>((Func<string, bool>) (b => string.Equals(string.Join<char>("", b.Where<char>(new Func<char, bool>(char.IsLetterOrDigit))), this._filterQueryText, StringComparison.CurrentCultureIgnoreCase))))).OrderByDescending<InventoryItem, DateTime>((Func<InventoryItem, DateTime>) (x => x.UpdateTime)));
      int num = 0;
      if (this._filterQueryText.Length > 8)
        num = this.Inventory.Items.Count<InventoryItem>((Func<InventoryItem, bool>) (i => i.Good.Barcode == this._filterQueryText || i.Good.Barcodes.Any<string>((Func<string, bool>) (b => string.Equals(string.Join<char>("", b.Where<char>(new Func<char, bool>(char.IsLetterOrDigit))), this._filterQueryText, StringComparison.CurrentCultureIgnoreCase)))));
      bool flag4 = num == 1;
      bool flag5 = num > 1;
      bool flag6 = false;
      LogHelper.Debug("filter:" + this._filterQueryText + "; isSingleEan: " + flag4.ToString() + "; isEan:" + flag2.ToString());
      if (this.EditFirstItemWhenFindSameBarcodes && flag5 | flag1)
        flag6 = true;
      if (flag4 | flag1)
        flag6 = true;
      if (!flag6)
        return;
      this.EditFirstBarcodeResult(q);

      bool checkPrefix(string prefix)
      {
        return ((IEnumerable<string>) prefix.Split(GlobalDictionaries.SplitArr)).Any<string>((Func<string, bool>) (x => x == (this._filterQueryText.Length >= x.Length ? this._filterQueryText.Substring(0, x.Length) : "")));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public Func<bool> ReloadJournal { get; set; }

    public Gbs.Core.ViewModels.Inventory.Inventory Inventory { get; set; } = new Gbs.Core.ViewModels.Inventory.Inventory();

    public InventoryCardViewModel_v2(Document document, Gbs.Core.Entities.Users.User user)
    {
      this.AuthUser = user;
      this._isNewDocument = document == null;
      this.InitializeSearchTimer();
      if (document == null)
      {
        this.LoadNewInventory();
      }
      else
      {
        this._cloneDocument = document.Clone<Document>();
        this.LoadInventoryFromDocument(document);
      }
      this.InitComPortScanner();
      this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items);
    }

    public InventoryCardViewModel_v2()
    {
    }

    private void LoadInventoryFromDocument(Document document)
    {
      new InventoryFactory(this.Inventory).LoadInventoryFromDocument(document);
      this.Inventory.NumberDocument = document.Number;
      this.Inventory.TsdDocumentUid = document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.TsdDocumentNumberUid))?.Value.ToString();
      this.OnPropertyChanged("Inventory");
      this.SelectedTabIndex = 1;
    }

    private void LoadNewInventory()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.SelectedGroups = new ObservableCollection<GoodGroups.Group>(new GoodGroupsRepository(dataBase).GetActiveItems());
        this.Inventory.Storage = this.AllStorages.Count == 1 ? this.AllStorages.Single<Storages.Storage>() : (Storages.Storage) null;
      }
    }

    public int SelectedTabIndex
    {
      get => this._selectedTabIndex;
      set
      {
        this._selectedTabIndex = value;
        this.OnPropertyChanged(nameof (SelectedTabIndex));
      }
    }

    public ObservableCollection<InventoryItem> CurrentItemsList
    {
      get => this._currentItemsList;
      set
      {
        this._currentItemsList = value;
        this.OnPropertyChanged(nameof (CurrentItemsList));
        this.ReCalcTotals();
      }
    }

    private void ReloadQtyFromDb()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.InventoryDoViewModel_Обновление_остатков_из_базы_данных);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
        foreach (InventoryItem inventoryItem1 in (Collection<InventoryItem>) this.Inventory.Items)
        {
          InventoryItem inventoryItem = inventoryItem1;
          InventoryCardViewModel_v2.SetItemDbQtyColor(inventoryItem, Colors.Transparent);
          Gbs.Core.Entities.Goods.Good good = activeItems.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == inventoryItem.Good.Uid));
          if (good != null)
          {
            Decimal num = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x =>
            {
              if (!(x.Price == inventoryItem.SalePrice) || !(x.Storage.Uid == this.Inventory.Storage.Uid))
                return false;
              Guid modificationUid = x.ModificationUid;
              GoodsModifications.GoodModification goodModification = inventoryItem.GoodModification;
              // ISSUE: explicit non-virtual call
              Guid guid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
              return modificationUid == guid;
            })).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
            if (inventoryItem.BaseQuantity != num)
            {
              inventoryItem.BaseQuantity = num;
              InventoryCardViewModel_v2.SetItemDbQtyColor(inventoryItem, Colors.Red);
            }
          }
        }
        this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items.OrderByDescending<InventoryItem, string>((Func<InventoryItem, string>) (x => x.Color.ToString())));
        progressBar.Close();
      }
    }

    public static void SetItemDbQtyColor(InventoryItem item, Color brushColor)
    {
      try
      {
        SolidColorBrush sbc = new SolidColorBrush(brushColor);
        sbc.Freeze();
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => item.Color = sbc));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public string FilterQueryText
    {
      get => this._filterQueryText;
      set
      {
        this._filterQueryText = Regex.Replace(value, "\\t|\\n|\\r", string.Empty);
        this.OnPropertyChanged(nameof (FilterQueryText));
        this._searchTimer.Stop();
        if (this._filterQueryText.Length <= 2 && this._filterQueryText.Length != 0)
          return;
        this._searchTimer.Start();
      }
    }

    public Visibility DbQuantityVisibility
    {
      get => this._dbQuantityVisibility;
      set
      {
        this._dbQuantityVisibility = value;
        this.OnPropertyChanged(nameof (DbQuantityVisibility));
      }
    }

    private void InventoryDoViewModelCreate()
    {
      if (this.Inventory.OldDocumentUid != Guid.Empty)
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          this.Inventory.TsdDocumentUid = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Document, dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == this.Inventory.OldDocumentUid && x.TYPE_UID == GlobalDictionaries.TsdDocumentNumberUid))).FirstOrDefault<EntityProperties.PropertyValue>()?.Value.ToString() ?? "";
      }
      this.CurrentItemsList = this.Inventory.Items;
    }

    private void InitComPortScanner()
    {
      ComPortScanner.SetDelegat((ComPortScanner.BarcodeChangeHandler) (barcode =>
      {
        if (!Other.IsActiveForm<FrmInventoryCard_v2>())
          DevelopersHelper.ShowNotification("Форма не активна");
        else
          this.FilterQueryText = barcode;
      }));
    }

    private void EditFirstBarcodeResult(Decimal? q = null)
    {
      InventoryItem firstItem = this.CurrentItemsList.First<InventoryItem>();
      if (this.RequestQuantityWhenBarcodeScanned)
      {
        Application.Current.Dispatcher.Invoke((Action) (() =>
        {
          (bool result2, Decimal? quantity2) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<DocumentItemViewModel>) new List<DocumentItemViewModel>()
          {
            (DocumentItemViewModel) this.CurrentItemsList.First<InventoryItem>()
          }));
          if (!result2)
            return;
          firstItem.Quantity = quantity2.GetValueOrDefault();
        }));
      }
      else
      {
        InventoryItem inventoryItem = firstItem;
        inventoryItem.Quantity = inventoryItem.Quantity + q.GetValueOrDefault(1M);
        this.ShowNotification(firstItem, q.GetValueOrDefault(1M));
      }
      this.FilterQueryText = string.Empty;
      firstItem.UpdateTime = DateTime.Now;
      this.CurrentItemsList = new ObservableCollection<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items.OrderByDescending<InventoryItem, DateTime>((Func<InventoryItem, DateTime>) (x => x.UpdateTime)));
    }

    private void DeleteItems(object obj)
    {
      this.Inventory.DeleteItemCommand.Execute(obj);
      this.CurrentItemsList = new ObservableCollection<InventoryItem>(this.CurrentItemsList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.Inventory.Items.Any<InventoryItem>((Func<InventoryItem, bool>) (i => i.Uid == x.Uid)))));
    }

    private void ShowNotification(InventoryItem singleItem, Decimal q = 1M)
    {
      if (this.NotShowNotification)
        return;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Text = string.Format(Translate.InventoryDoViewModel_К_товару__0__добавлено__1_шт__, (object) singleItem.DisplayedName, (object) q) + Other.NewLine() + string.Format(Translate.InventoryDoViewModel_Всего___0_N3_, (object) singleItem.Quantity),
        Title = Translate.InventoryDoViewModel_Отсканирован_товар
      });
    }

    private void NextPage()
    {
      if (this.SelectedTabIndex == 0)
      {
        if (this.Inventory.Storage == null)
          MessageBoxHelper.Warning(Translate.InventoryStartViewModel_Требуется_выбрать_склад__для_которого_проводится_инвентаризация);
        else if (!this.SelectedGroups.Any<GoodGroups.Group>())
        {
          MessageBoxHelper.Warning(Translate.InventoryStartViewModel_NextPage_Требуется_выбрать_категории__для_которых_нужно_проводить_инвентаризацию_);
        }
        else
        {
          new InventoryFactory(this.Inventory).LoadInventoryFromGoodsList(this.SelectedGroups.ToList<GoodGroups.Group>(), this.LoadGoodsWithZeroStock);
          this.InventoryDoViewModelCreate();
          this.SelectedTabIndex = 1;
        }
      }
      else
      {
        if (this.SelectedTabIndex != 1)
          return;
        (InventorySaveViewModel.ResultTypes result, string comment) = new FrmInventorySave().ShowForm();
        switch (result)
        {
          case InventorySaveViewModel.ResultTypes.Cancel:
            break;
          case InventorySaveViewModel.ResultTypes.Pause:
            this.SaveInventory(false, comment);
            break;
          case InventorySaveViewModel.ResultTypes.Finish:
            this.SaveInventory(true, comment);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private void PreviousPage()
    {
      switch (this.SelectedTabIndex)
      {
        case 0:
          this.CloseAction();
          break;
        case 1:
          this.CloseAction();
          break;
      }
    }

    private void SaveInventory(bool finish, string comment)
    {
      try
      {
        List<InventoryItem> list = this.Inventory.Items.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.BaseQuantity != 0M && x.Quantity == 0M)).ToList<InventoryItem>();
        if (list.Any<InventoryItem>() & finish && MessageBoxHelper.Question(string.Format(Translate.InventoryCardViewModel_new_Для__0__товаров_количество_будет_установлено_0_по_итогам_инвентаризации__продолжить_сохранение_, (object) list.Count<InventoryItem>())) == MessageBoxResult.No)
          return;
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.InventoryCardViewModel_new_Сохранение_документа_инвентаризации);
        this.Inventory.Comment = comment;
        this.Inventory.IsFinished = finish;
        this.Inventory.User = this.AuthUser;
        List<InventoryItem> source = this.Inventory.HasChangedStocks();
        if (source.Any<InventoryItem>() && MessageBoxHelper.Show(string.Format(Translate.InventoryCardViewModel_new_Для__0__товаров_кол_во_в_базе_данных_было_изменено__пока_проводилась_инвентаризация__Продолжить_сохранение_, (object) source.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
          return;
        ActionResult actionResult = this.Inventory.Save();
        progressBar.Close();
        if (actionResult.Result != ActionResult.Results.Ok)
        {
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.InventoryCardViewModel_new_Ошибка_при_сохранении_документа,
            Text = Translate.InventoryCardViewModel_new_При_сохранении_документа_инвентаризации_произошла_ошибка_
          });
        }
        else
        {
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.InventoryCardViewModel_new_Документ_сохранен,
            Text = Translate.InventoryCardViewModel_new_Инвентаризация_успешно_сохранена
          });
          WindowWithSize.IsCancel = false;
          this.CloseAction();
          Func<bool> reloadJournal = this.ReloadJournal;
          if (reloadJournal != null)
          {
            int num = reloadJournal() ? 1 : 0;
          }
          this.WriteUsersHistory();
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка при сохранении инвентаризации");
        throw;
      }
    }

    private void WriteUsersHistory()
    {
      if (this._isNewDocument)
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) null, (IEntity) this.Inventory.Document, ActionType.Add, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
      else
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) this._cloneDocument, (IEntity) this.Inventory.Document, ActionType.Edit, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges() => false;

    public Visibility VisibilityTsd
    {
      get
      {
        return this._devicesConfig.Tsd.Type == GlobalDictionaries.Devices.TsdTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand SendTsdCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Inventory.TsdDocumentUid = this.Inventory.TsdDocumentUid.IsNullOrEmpty() ? Guid.NewGuid().ToString() : this.Inventory.TsdDocumentUid;
          this.OnPropertyChanged("VisibilityWriteTsdCommand");
          this.OnPropertyChanged("VisibilityReadTsdCommand");
          using (TsdHelper tsdHelper = new TsdHelper((IConfig) this._devicesConfig))
            tsdHelper.WriteInventory(this.Inventory.Items.Select<InventoryItem, GoodForTsd>((Func<InventoryItem, GoodForTsd>) (x =>
            {
              GoodForTsd sendTsdCommand = new GoodForTsd();
              sendTsdCommand.Barcode = x.Good.Barcode;
              sendTsdCommand.Name = x.DisplayedName;
              sendTsdCommand.Price = x.SalePrice;
              sendTsdCommand.Quantity = x.BaseQuantity;
              sendTsdCommand.UnitName = "шт.";
              __Boxed<Guid> uid = (ValueType) x.Good.Uid;
              GoodsModifications.GoodModification goodModification = x.GoodModification;
              __Boxed<Guid> local = goodModification != null ? (ValueType) __nonvirtual (goodModification.Uid) : (ValueType) Guid.Empty;
              __Boxed<Decimal> salePrice = (ValueType) x.SalePrice;
              sendTsdCommand.Tag = string.Format("{0}+{1}+{2}", (object) uid, (object) local, (object) salePrice);
              return sendTsdCommand;
            })).ToList<GoodForTsd>(), this.Inventory.TsdDocumentUid.ToString());
          int num = (int) MessageBoxHelper.Show(Translate.InventoryDoViewModel_SendTsdCommand_Товары_из_документа_успешно_выгружено_на_ТСД__продолжите_инвентаризацию_на_устройстве_);
        }));
      }
    }

    public Visibility VisibilityReadTsdCommand
    {
      get
      {
        Gbs.Core.ViewModels.Inventory.Inventory inventory = this.Inventory;
        return (inventory != null ? (inventory.TsdDocumentUid.IsNullOrEmpty() ? 1 : 0) : 1) == 0 && this._devicesConfig.Tsd.Type != GlobalDictionaries.Devices.TsdTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityWriteTsdCommand
    {
      get
      {
        Gbs.Core.ViewModels.Inventory.Inventory inventory = this.Inventory;
        return (inventory != null ? (!inventory.TsdDocumentUid.IsNullOrEmpty() ? 1 : 0) : 1) == 0 && this._devicesConfig.Tsd.Type != GlobalDictionaries.Devices.TsdTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand ReadTsdCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (TsdHelper tsdHelper = new TsdHelper((IConfig) this._devicesConfig))
          {
            List<GoodForTsd> goodForTsdList = tsdHelper.ReadInventory(this.Inventory.TsdDocumentUid);
            Dictionary<string, InventoryItem> dictionary1 = new List<InventoryItem>((IEnumerable<InventoryItem>) this.Inventory.Items).ToDictionary<InventoryItem, string, InventoryItem>((Func<InventoryItem, string>) (item =>
            {
              __Boxed<Guid> uid = (ValueType) item.Good.Uid;
              GoodsModifications.GoodModification goodModification = item.GoodModification;
              __Boxed<Guid> local = goodModification != null ? (ValueType) __nonvirtual (goodModification.Uid) : (ValueType) Guid.Empty;
              __Boxed<Decimal> salePrice = (ValueType) item.SalePrice;
              return string.Format("{0}+{1}+{2}", (object) uid, (object) local, (object) salePrice);
            }), (Func<InventoryItem, InventoryItem>) (item => item));
            Dictionary<InventoryItem, Decimal> dictionary2 = new Dictionary<InventoryItem, Decimal>();
            foreach (GoodForTsd goodForTsd in goodForTsdList)
            {
              InventoryItem key;
              if (!(goodForTsd.Quantity == 0M) && dictionary1.TryGetValue(goodForTsd.Tag, out key) && !(key.Quantity == goodForTsd.Quantity))
                dictionary2.Add(key, goodForTsd.Quantity);
            }
            if (dictionary2.Count == 0)
            {
              MessageBoxHelper.Warning("От ТСД не были получены результаты проведенной инвентаризации.\n\nУбедитесь, что работа с документом на ТСД завершена и документ был успешно сохранен.");
              return;
            }
            if (MessageBoxHelper.Question(string.Format("Из ТСД получены обновленные остатки для {0} товаров. Применить изменения для этого документа?\n\nОбратите внимание, отменить это действие будет нельзя.", (object) dictionary2.Count)) == MessageBoxResult.No)
              return;
            foreach (KeyValuePair<InventoryItem, Decimal> keyValuePair in dictionary2)
              keyValuePair.Key.Quantity = keyValuePair.Value;
          }
          int num = (int) MessageBoxHelper.Show(Translate.InventoryDoViewModel_ReadTsdCommand_Количество_для_товаров_успешно_обновлено_из_ТСД_);
        }));
      }
    }

    public class HarvesterItem
    {
      public string Name { get; set; }

      public string Barcode { get; set; }

      public Decimal BaseQuantity { get; set; }

      public Decimal SalePrice { get; set; }

      public Decimal Quantity { get; set; }

      public HarvesterItem(string[] arr)
      {
        this.Name = arr[0].Trim();
        this.Barcode = arr[1].Trim();
        this.BaseQuantity = Convert.ToDecimal(arr[2]);
        this.SalePrice = Convert.ToDecimal(arr[3]);
        this.Quantity = Convert.ToDecimal(arr[4]);
      }

      public HarvesterItem()
      {
      }
    }
  }
}
