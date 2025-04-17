// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Forms.Goods.GoodCard.Pages.Сertificate;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.FR;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms.Goods
{
  public partial class GoodCardModelView : ViewModelWithForm, ICheckChangesViewModel
  {
    private readonly BuyPriceCounter _buyPriceCounter;
    private ObservableCollection<GoodCardModelView.StockOptionItem> _filterProperties = new ObservableCollection<GoodCardModelView.StockOptionItem>();
    private readonly Gbs.Core.Config.Devices DevicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
    private bool _isEnabledForWaybill;
    public bool IsEditCard;
    private Timer _searchTimer = new Timer();
    private Visibility _textBoxNameVisibility;
    private Visibility _comboBoxNameVisibility = Visibility.Collapsed;
    private List<string> _nameListMiDays = new List<string>();
    public bool IsRandomBarcode;
    private readonly object _extraPricesCollectionLock = new object();
    private ObservableCollection<GoodCardModelView.PriceAndStockView> _extraPricesDataTable = new ObservableCollection<GoodCardModelView.PriceAndStockView>();

    public void GetAmClassifier()
    {
      if (this.VisibilityAmClassifier == Visibility.Collapsed)
        return;
      this.AmClassifiers = new ObservableCollection<Hdm.CategoryItem>(this.CacheAmClassifiers);
      if (this.Good.Properties.All<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid != GlobalDictionaries.AmClassifierIdUid)))
        return;
      this.AmClassSelectedValue = this.Good.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.AmClassifierIdUid)).Value.ToString();
      this.OnPropertyChanged("AmClassSelectedValue");
    }

    public bool SaveAmClassifier()
    {
      if (this.VisibilityAmClassifier == Visibility.Collapsed)
        return true;
      if (this.AmClassSelectedValue.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning(Translate.GoodCardModelView_SaveAmClassifier_Необходимо_указать_классификатор_перед_сохранением_товара);
        return false;
      }
      if (this.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.AmClassifierIdUid)))
      {
        this.Good.Properties[this.Good.Properties.FindIndex((Predicate<EntityProperties.PropertyValue>) (x => x.Type.Uid == GlobalDictionaries.AmClassifierIdUid))].Value = (object) this.AmClassSelectedValue;
      }
      else
      {
        List<EntityProperties.PropertyValue> properties = this.Good.Properties;
        EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
        propertyValue.EntityUid = this.Good.Uid;
        EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
        propertyType.Uid = GlobalDictionaries.AmClassifierIdUid;
        propertyValue.Type = propertyType;
        propertyValue.Value = (object) this.AmClassSelectedValue;
        properties.Add(propertyValue);
      }
      return true;
    }

    public Visibility VisibilityAmClassifier
    {
      get
      {
        return new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Armenia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private List<Hdm.CategoryItem> CacheAmClassifiers { get; set; } = Hdm.CategoryItems;

    public ObservableCollection<Hdm.CategoryItem> AmClassifiers { get; set; }

    public GoodCardModelView()
    {
    }

    public GoodCardModelView(
      Gbs.Core.Entities.Goods.Good good,
      bool isEdit = false,
      GlobalDictionaries.DocumentsTypes goodForDocumentType = GlobalDictionaries.DocumentsTypes.None)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmGoodCard_Загрузка_карточки_товара);
      Other.ConsoleWrite("Начата загрузка карточки товара");
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.ComPortScannerOnBarcodeChanged));
      this.Good = good;
      this.EntityClone = (IEntity) good.Clone<Gbs.Core.Entities.Goods.Good>();
      this.GoodForDocumentType = goodForDocumentType;
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        this.ListGroup = new GoodGroupsRepository(dataBase).GetActiveItems();
        this.GetAmClassifier();
        if (this.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)
        {
          int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Данная_услуга_является_системной__у_нее_нельзя_менять_категорию__добавлять___редактировать___удалять_остатки_, icon: MessageBoxImage.Exclamation);
        }
        this._buyPriceCounter = new BuyPriceCounter();
        this.FormatBarcodes();
        this.InitGroup(isEdit, progressBar);
        this.InitBarcodeSearchTimer();
        this.InitFilterOptions();
        this.ShowExtraPrice();
        this.LoadDocumentsWaybills(good);
        this.ShowProperty();
        this.SetSelectedTab(goodForDocumentType);
        this.InitSubPage();
        progressBar.Close();
        if (!this.Good.IsDeleted)
          return;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.GoodCardModelView_GoodCardModelView_Открыта_карточка_ранее_удаленного_товара__При_сохранении_карточки_будет_возможность_восстановить_данный_товар_));
      }
    }

    private void InitGroup(bool isEdit, ProgressBarHelper.ProgressBar progressBar)
    {
      if (!this.ListGroup.Any<Gbs.Core.Entities.GoodGroups.Group>() && !isEdit)
      {
        progressBar.Close();
        int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Требуется_создать_категорию);
        Gbs.Core.Entities.GoodGroups.Group group;
        if (new FrmGoodGroupCard().ShowGroupCard(Guid.Empty, out group))
          this.ListGroup.Add(group);
        else
          this.IsShowCard = false;
      }
      if (this.ListGroup.Count != 1)
        return;
      this.Good.Group = this.ListGroup.First<Gbs.Core.Entities.GoodGroups.Group>();
    }

    private void InitBarcodeSearchTimer()
    {
      if (!new ConfigsRepository<Integrations>().Get().IsBarcodesMiDays)
        return;
      this._searchTimer = new Timer()
      {
        Interval = 500.0,
        AutoReset = false
      };
      this._searchTimer.Elapsed += new ElapsedEventHandler(this.SearchTimerOnElapsed);
      this._searchTimer.Start();
    }

    private void InitFilterOptions()
    {
      this.Setting = new ConfigsRepository<FilterOptions>().Get();
      ObservableCollection<GoodCardModelView.StockOptionItem> observableCollection = new ObservableCollection<GoodCardModelView.StockOptionItem>();
      observableCollection.Add(new GoodCardModelView.StockOptionItem()
      {
        Option = GoodCardModelView.StockOption.Group,
        IsChecked = this.Setting.GoodsCatalog.IsGroupStock,
        Text = Translate.ГруппироватьОстатки
      });
      observableCollection.Add(new GoodCardModelView.StockOptionItem()
      {
        Option = GoodCardModelView.StockOption.CollapsedNullStock,
        IsChecked = this.Setting.GoodsCatalog.IsCollapsedNullStock,
        Text = Translate.GoodCardModelView_Скрывать_нулевые_остатки
      });
      observableCollection.Add(new GoodCardModelView.StockOptionItem()
      {
        Option = GoodCardModelView.StockOption.CollapsedMinusStock,
        IsChecked = this.Setting.GoodsCatalog.IsCollapsedMinusStock,
        Text = Translate.GoodCardModelView_Скрывать_отрицательные_остатки
      });
      this.FilterProperties = observableCollection;
    }

    private void SetSelectedTab(
      GlobalDictionaries.DocumentsTypes goodForDocumentType)
    {
      Gbs.Core.Entities.GoodGroups.Group group = this.Good.Group;
      if ((group != null ? (group.GoodsType == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) != 0)
        this.SelectedTab = 1;
      else if (goodForDocumentType == GlobalDictionaries.DocumentsTypes.ProductionSet)
        this.SelectedTab = 4;
      this.OnPropertyChanged("SelectedTab");
      this.OnPropertyChanged("IsEnabledSetModificationType");
    }

    [Localizable(false)]
    private void FormatBarcodes()
    {
      this.Barcodes = string.Join("", this.Good.Barcodes.Select<string, string>((Func<string, string>) (b => !b.EndsWith("\r") ? b + "\r" : b)));
    }

    public void InitSubPage()
    {
      this.CertificatePage = new PageСertificate(this.Good.StocksAndPrices, this.Good);
      this.CertificateBasicPage = new PageСertificateBasic(this.PropertyCertificate, this.Good.Uid);
      if (DevelopersHelper.IsUnitTest())
        return;
      this.ImageGood = new PageImageGood(this.Good.Uid);
    }

    private bool IsSystemGood()
    {
      if (!(this.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid))
        return false;
      int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Данная_услуга_является_системной__у_нее_нельзя_менять_категорию__добавлять___редактировать___удалять_остатки_, icon: MessageBoxImage.Exclamation);
      return true;
    }

    public ICommand AddStock
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.IsSystemGood())
            return;
          List<GoodsModifications.GoodModification> modifications = new List<GoodsModifications.GoodModification>();
          if (this.Good.Modifications.Any<GoodsModifications.GoodModification>())
          {
            if (!new FrmSelectGoodStock().SelectedModification(this.Good, out modifications))
              return;
          }
          else if (this.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range)
          {
            int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Создать_остаток_невозможно__так_как_нет_не__одной_модификации_для_данного_товара_ассортимента);
            return;
          }
          FrmGoodStockCard frmGoodStockCard = new FrmGoodStockCard();
          Guid empty = Guid.Empty;
          GoodsStocks.GoodStock goodStock1;
          ref GoodsStocks.GoodStock local = ref goodStock1;
          Gbs.Core.Entities.GoodGroups.Group group = this.Good.Group;
          int num1 = group != null ? (group.GoodsType != GlobalDictionaries.GoodTypes.Service ? 1 : 0) : 1;
          Gbs.Core.Entities.Goods.Good good = this.Good;
          Gbs.Core.Entities.Users.User authUser = this.AuthUser;
          if (!frmGoodStockCard.ShowCardStock(empty, out local, num1 != 0, good, authUser))
            return;
          goodStock1.GoodUid = this.Good.Uid;
          GoodsStocks.GoodStock goodStock2 = goodStock1;
          GoodsModifications.GoodModification goodModification = modifications.SingleOrDefault<GoodsModifications.GoodModification>();
          Guid guid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
          goodStock2.ModificationUid = guid;
          GoodCardModelView.PriceAndStockView stock = new GoodCardModelView.PriceAndStockView()
          {
            GoodStock = goodStock1,
            Modification = modifications.SingleOrDefault<GoodsModifications.GoodModification>()
          };
          stock.ExtraPrice = this.CountExtraPrice(stock).ToList<Decimal>();
          this.AddOrUpdateStock(stock);
        }), (Func<object, bool>) (_ => this.IsSaveGood));
      }
    }

    public ICommand EditStock
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.IsSystemGood())
            return;
          if (this.SelectedStock == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Необходимо_выбрать_один_товарный_остаток);
          }
          else if (((ICollection) obj).Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Требуется_выбрать_только_один_остаток);
          }
          else
          {
            GoodsStocks.GoodStock stockOld = this.SelectedStock.GoodStock.Clone();
            FrmGoodStockCard frmGoodStockCard = new FrmGoodStockCard();
            Guid uid = this.SelectedStock.GoodStock.Uid;
            GoodsStocks.GoodStock stock;
            ref GoodsStocks.GoodStock local = ref stock;
            Gbs.Core.Entities.GoodGroups.Group group = this.Good.Group;
            int num3 = group != null ? (group.GoodsType != GlobalDictionaries.GoodTypes.Service ? 1 : 0) : 1;
            Gbs.Core.Entities.Goods.Good good = this.Good;
            Gbs.Core.Entities.Users.User authUser = this.AuthUser;
            GoodsStocks.GoodStock goodStock = this.SelectedStock.GoodStock;
            if (frmGoodStockCard.ShowCardStock(uid, out local, num3 != 0, good, authUser, goodStock))
            {
              stock.GoodUid = this.Good.Uid;
              int stockIndex = this.FindStockIndex(this.SelectedStock);
              if (stockIndex != -1)
                this.UpdateEditedStock(stockOld, stock, stockIndex);
            }
            this.OnPropertyChanged("ExtraPricesDataTable");
            this.OnPropertyChanged("SumStock");
          }
        }), (Func<object, bool>) (_ => this.IsEnabledGoodStock));
      }
    }

    private void UpdateEditedStock(
      GoodsStocks.GoodStock stockOld,
      GoodsStocks.GoodStock stock,
      int index)
    {
      this.ExtraPricesDataTable[index].GoodStock = stock.Clone();
      this.ExtraPricesDataTable[index].ExtraPrice = this.CountExtraPrice(this.ExtraPricesDataTable[index]).ToList<Decimal>();
      GoodsStocks.GoodStock goodStock = this.Good.StocksAndPrices.Single<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Uid == stockOld.Uid));
      stock.Stock = goodStock.Stock + stock.Stock - stockOld.Stock;
      this.Good.StocksAndPrices[this.Good.StocksAndPrices.FindIndex((Predicate<GoodsStocks.GoodStock>) (x => x.Uid == stockOld.Uid))] = stock;
      this.UpdatePriceForAllStockWithSamePrice(stockOld, stock);
    }

    private void UpdatePriceForAllStockWithSamePrice(
      GoodsStocks.GoodStock stockOld,
      GoodsStocks.GoodStock stock)
    {
      Decimal buyPrice = this._buyPriceCounter.GetBuyPrice(stock.Uid);
      foreach (GoodsStocks.GoodStock stocksAndPrice in this.Good.StocksAndPrices)
      {
        if (stocksAndPrice.Price == stockOld.Price && stocksAndPrice.ModificationUid == stockOld.ModificationUid && stocksAndPrice.Storage.Uid == stockOld.Storage.Uid && this._buyPriceCounter.GetBuyPrice(stocksAndPrice.Uid) == buyPrice)
          stocksAndPrice.Price = stock.Price;
      }
    }

    public ICommand DeleteStock
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.IsSystemGood())
            return;
          List<GoodCardModelView.PriceAndStockView> list = ((IEnumerable) obj).Cast<GoodCardModelView.PriceAndStockView>().ToList<GoodCardModelView.PriceAndStockView>();
          if (!list.Any<GoodCardModelView.PriceAndStockView>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Необходимо_выбрать_товарный_остаток_для_удаления);
          }
          else
          {
            using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeleteGoodStock) && !new Authorization().GetAccess(Actions.DeleteGoodStock).Result || MessageBoxHelper.Show(string.Format(Translate.GoodCardModelView_Уверены__что_хотите_удалить__0__остатков_для_данного_товара__Значение_количества_для_выбранных_остатков_будет_установлено_равное_нулю_ + Translate.GoodCardModelView_, (object) list.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
              foreach (GoodCardModelView.PriceAndStockView priceAndStockView in list)
              {
                GoodCardModelView.PriceAndStockView st = priceAndStockView;
                this.ExtraPricesDataTable.Remove(st);
                st.GoodStock.IsDeleted = true;
                if (!this.IsGroupStock)
                {
                  int index = this.Good.StocksAndPrices.FindIndex((Predicate<GoodsStocks.GoodStock>) (x => x.Uid == st.GoodStock.Uid));
                  if (index != -1)
                    this.Good.StocksAndPrices[index].IsDeleted = true;
                }
                else
                {
                  Decimal buyPrice = this._buyPriceCounter.GetBuyPrice(st.GoodStock.Uid);
                  foreach (GoodsStocks.GoodStock stocksAndPrice in this.Good.StocksAndPrices)
                  {
                    if (stocksAndPrice.Price == st.GoodStock.Price && stocksAndPrice.ModificationUid == st.GoodStock.ModificationUid && stocksAndPrice.Storage.Uid == st.GoodStock.Storage.Uid && this._buyPriceCounter.GetBuyPrice(stocksAndPrice.Uid) == buyPrice)
                      stocksAndPrice.IsDeleted = true;
                  }
                }
              }
              this.OnPropertyChanged("SumStock");
            }
          }
        }), (Func<object, bool>) (_ => this.IsEnabledGoodStock));
      }
    }

    private void AddOrUpdateStock(GoodCardModelView.PriceAndStockView item)
    {
      int stockIndex = this.FindStockIndex(item);
      if (stockIndex != -1 && this.IsGroupStock)
      {
        this.UpdateExistingStock(stockIndex, item);
      }
      else
      {
        this.ExtraPricesDataTable.Add(item);
        this.Good.StocksAndPrices.Add(item.GoodStock);
      }
      this.OnPropertyChanged("ExtraPricesDataTable");
      this.OnPropertyChanged("SumStock");
    }

    private int FindStockIndex(GoodCardModelView.PriceAndStockView item)
    {
      return this.FilterProperties.Any<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.Group && x.IsChecked)) ? this.ExtraPricesDataTable.ToList<GoodCardModelView.PriceAndStockView>().FindIndex((Predicate<GoodCardModelView.PriceAndStockView>) (x =>
      {
        if (!(x.GoodStock.Price == item.GoodStock.Price) || !(x.BuyPrice.GetValueOrDefault() == item.BuyPrice.GetValueOrDefault()) || !(x.GoodStock.Storage.Uid == item.GoodStock.Storage.Uid))
          return false;
        GoodsModifications.GoodModification modification1 = x.Modification;
        // ISSUE: explicit non-virtual call
        Guid guid1 = modification1 != null ? __nonvirtual (modification1.Uid) : Guid.Empty;
        GoodsModifications.GoodModification modification2 = item.Modification;
        // ISSUE: explicit non-virtual call
        Guid guid2 = modification2 != null ? __nonvirtual (modification2.Uid) : Guid.Empty;
        return guid1 == guid2;
      })) : this.ExtraPricesDataTable.ToList<GoodCardModelView.PriceAndStockView>().FindIndex((Predicate<GoodCardModelView.PriceAndStockView>) (x => x.GoodStock.Uid == item.GoodStock.Uid));
    }

    private void UpdateExistingStock(int index, GoodCardModelView.PriceAndStockView item)
    {
      GoodsStocks.GoodStock existingStock = this.ExtraPricesDataTable[index].GoodStock.Clone();
      existingStock.Stock += item.GoodStock.Stock;
      this.Good.StocksAndPrices.Single<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Uid == existingStock.Uid)).Stock += item.GoodStock.Stock;
      this.GroupStock();
    }

    public FilterOptions Setting { get; set; } = new FilterOptions();

    private bool IsGroupStock
    {
      get
      {
        return this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.Group)).IsChecked;
      }
    }

    private bool IsCollapsedNullStock
    {
      get
      {
        return this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedNullStock)).IsChecked;
      }
    }

    private bool IsCollapsedMinusStock
    {
      get
      {
        return this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedMinusStock)).IsChecked;
      }
    }

    public ObservableCollection<GoodCardModelView.StockOptionItem> FilterProperties
    {
      get => this._filterProperties;
      set
      {
        this._filterProperties = value;
        this.GroupStock();
      }
    }

    private void GroupStock()
    {
      List<GoodCardModelView.PriceAndStockView> priceAndStockViewList = new List<GoodCardModelView.PriceAndStockView>(this.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)).Select<GoodsStocks.GoodStock, GoodCardModelView.PriceAndStockView>((Func<GoodsStocks.GoodStock, GoodCardModelView.PriceAndStockView>) (x => new GoodCardModelView.PriceAndStockView()
      {
        GoodStock = x,
        BuyPrice = new Decimal?(this.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set ? this._buyPriceCounter.GetLastBuyPrice(this.Good) : this._buyPriceCounter.GetBuyPrice(x.Uid)),
        Modification = this.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == x.ModificationUid)),
        MarkedCode = x.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.MarkedInfoGood))?.Value?.ToString() ?? "",
        FbNumberForEgais = SharedRepository.GetFbNumberForGoodStock(x)
      })));
      if (this.IsGroupStock)
        priceAndStockViewList = this.GroupStockByProperties(priceAndStockViewList);
      if (this.IsCollapsedNullStock && this.ShouldFilterByStock(this.Good))
        priceAndStockViewList = priceAndStockViewList.Where<GoodCardModelView.PriceAndStockView>((Func<GoodCardModelView.PriceAndStockView, bool>) (x => x.GoodStock.Stock != 0M)).ToList<GoodCardModelView.PriceAndStockView>();
      if (this.IsCollapsedMinusStock && this.ShouldFilterByStock(this.Good))
        priceAndStockViewList = priceAndStockViewList.Where<GoodCardModelView.PriceAndStockView>((Func<GoodCardModelView.PriceAndStockView, bool>) (x => x.GoodStock.Stock >= 0M)).ToList<GoodCardModelView.PriceAndStockView>();
      this.ExtraPricesDataTable = new ObservableCollection<GoodCardModelView.PriceAndStockView>(priceAndStockViewList.Clone<List<GoodCardModelView.PriceAndStockView>>());
      this.ShowExtraPrice();
    }

    private List<GoodCardModelView.PriceAndStockView> GroupStockByProperties(
      List<GoodCardModelView.PriceAndStockView> list)
    {
      return list.GroupBy(x => new
      {
        Price = x.GoodStock.Price,
        BuyPrice = x.BuyPrice,
        Uid = x.GoodStock.Storage.Uid,
        mUid = x.Modification?.Uid,
        MarkedCode = x.MarkedCode,
        FbNumberForEgais = x.FbNumberForEgais
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType43<Decimal, Decimal?, Guid, Guid?, string, string>, GoodCardModelView.PriceAndStockView>, GoodCardModelView.PriceAndStockView>(g =>
      {
        return new GoodCardModelView.PriceAndStockView()
        {
          GoodStock = new GoodsStocks.GoodStock()
          {
            Price = g.First<GoodCardModelView.PriceAndStockView>().GoodStock.Price,
            Uid = g.First<GoodCardModelView.PriceAndStockView>().GoodStock.Uid,
            Stock = g.Sum<GoodCardModelView.PriceAndStockView>((Func<GoodCardModelView.PriceAndStockView, Decimal>) (x => x.GoodStock.Stock)),
            Storage = g.First<GoodCardModelView.PriceAndStockView>().GoodStock.Storage,
            ModificationUid = g.First<GoodCardModelView.PriceAndStockView>().GoodStock.ModificationUid,
            GoodUid = this.Good.Uid,
            Properties = g.First<GoodCardModelView.PriceAndStockView>().GoodStock.Properties
          },
          BuyPrice = g.First<GoodCardModelView.PriceAndStockView>().BuyPrice,
          Modification = g.First<GoodCardModelView.PriceAndStockView>().Modification,
          MarkedCode = g.First<GoodCardModelView.PriceAndStockView>().MarkedCode,
          FbNumberForEgais = g.Key.FbNumberForEgais
        };
      }).ToList<GoodCardModelView.PriceAndStockView>();
    }

    private bool ShouldFilterByStock(Gbs.Core.Entities.Goods.Good good)
    {
      if (!good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Production, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.None))
        return false;
      Gbs.Core.Entities.GoodGroups.Group group = good.Group;
      if (group == null)
        return false;
      return group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight);
    }

    public GoodCardModelView.PriceAndStockView SelectedStock { get; set; }

    public Decimal SumStock
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        return good == null ? 0M : good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
      }
    }

    public Visibility VisibilityStockForGood
    {
      get
      {
        if (this.Good?.Group?.GoodsType.GetValueOrDefault() != GlobalDictionaries.GoodTypes.Service)
        {
          Gbs.Core.Entities.Goods.Good good = this.Good;
          if ((good != null ? (int) good.SetStatus : 0) != 1)
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public Visibility VisibilityFbNumber
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        bool? nullable;
        if (good == null)
        {
          nullable = new bool?();
        }
        else
        {
          List<GoodsStocks.GoodStock> stocksAndPrices = good.StocksAndPrices;
          nullable = stocksAndPrices != null ? new bool?(stocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !SharedRepository.GetFbNumberForGoodStock(x).IsNullOrEmpty()))) : new bool?();
        }
        return !nullable.GetValueOrDefault() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityBuyPrice
    {
      get
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          int visibilityBuyPrice;
          if (this.Good?.Group?.GoodsType.GetValueOrDefault() != GlobalDictionaries.GoodTypes.Service)
          {
            Gbs.Core.Entities.Goods.Good good = this.Good;
            if (!((GlobalDictionaries.GoodsSetStatuses) (good != null ? (int) good.SetStatus : 0)).IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Production) && new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ShowBuyPrice))
            {
              visibilityBuyPrice = 0;
              goto label_5;
            }
          }
          visibilityBuyPrice = 2;
label_5:
          return (Visibility) visibilityBuyPrice;
        }
      }
    }

    public Visibility VisibilityGridStock
    {
      get
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          return new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ViewStock) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityTabControl
    {
      get => this.Good?.Group == null ? Visibility.Collapsed : Visibility.Visible;
    }

    public Visibility VisibilityButtonGroup
    {
      get => this.Good?.Group != null ? Visibility.Collapsed : Visibility.Visible;
    }

    public Gbs.Core.Entities.Goods.Good Good { get; set; }

    public string Barcode
    {
      get => this.Good?.Barcode ?? string.Empty;
      set
      {
        if (this.Good == null)
          return;
        this.Good.Barcode = value;
        this.OnPropertyChanged("Good");
        this.OnPropertyChanged(nameof (Barcode));
        this._searchTimer?.Close();
        this._searchTimer?.Start();
      }
    }

    public int SelectedTab { get; set; }

    public bool IsEnabledForWaybill
    {
      get => this._isEnabledForWaybill;
      set
      {
        this._isEnabledForWaybill = value;
        this.OnPropertyChanged(nameof (IsEnabledForWaybill));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public bool SaveGoodResult { get; set; }

    public bool IsShowCard { get; set; } = true;

    public string Barcodes { get; set; }

    public Visibility VisibilityGridHistory
    {
      get
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          return new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ViewHistory) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityTextBox
    {
      get
      {
        return this.VisibilityGridStock != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityTextBoxHistory
    {
      get
      {
        return this.VisibilityGridHistory != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public bool IsSaveGood { get; set; }

    public GlobalDictionaries.DocumentsTypes GoodForDocumentType { get; set; }

    public bool IsEnabledGoodStock
    {
      get => this.IsSaveGood || this.GoodForDocumentType == GlobalDictionaries.DocumentsTypes.Buy;
    }

    private void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Other.IsActiveForm<FrmGoodCard>())
        return;
      this.Barcode = barcode;
      this.OnPropertyChanged("Good");
    }

    public Visibility TextBoxNameVisibility
    {
      get => this._textBoxNameVisibility;
      set
      {
        this._textBoxNameVisibility = value;
        this.OnPropertyChanged(nameof (TextBoxNameVisibility));
      }
    }

    public Visibility ComboBoxNameVisibility
    {
      get => this._comboBoxNameVisibility;
      set
      {
        this._comboBoxNameVisibility = value;
        this.OnPropertyChanged(nameof (ComboBoxNameVisibility));
      }
    }

    public List<string> NameListMiDays
    {
      get => this._nameListMiDays;
      set
      {
        this._nameListMiDays = value;
        this.OnPropertyChanged(nameof (NameListMiDays));
      }
    }

    private void SearchTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      if (!this.Good.Name.IsNullOrEmpty())
        this.IsRandomBarcode = false;
      else if (this.Good.Barcode.IsNullOrEmpty())
        this.IsRandomBarcode = false;
      else if (this.IsRandomBarcode)
      {
        this.IsRandomBarcode = false;
      }
      else
      {
        List<ItemBarcodesMiDays> all = BarcodesMiDays.GetAll(this.Barcode.Replace("\n", "").Replace("\t", "").Replace("\r", "").Trim());
        if (!all.Any<ItemBarcodesMiDays>())
        {
          this._searchTimer?.Stop();
          this.IsRandomBarcode = false;
        }
        else
        {
          this.Good.Name = all.First<ItemBarcodesMiDays>().name;
          if (all.Count > 1)
          {
            this.NameListMiDays = new List<string>(all.Select<ItemBarcodesMiDays, string>((Func<ItemBarcodesMiDays, string>) (x => x.name)));
            this.TextBoxNameVisibility = Visibility.Collapsed;
            this.ComboBoxNameVisibility = Visibility.Visible;
          }
          this.OnPropertyChanged("Good");
          this._searchTimer?.Stop();
          this.IsRandomBarcode = false;
        }
      }
    }

    private void LoadDocumentsWaybills(Gbs.Core.Entities.Goods.Good g)
    {
      this.IsEnabledForWaybill = false;
      new Stopwatch().Start();
      BindingOperations.EnableCollectionSynchronization((IEnumerable) this._extraPricesDataTable, this._extraPricesCollectionLock);
      this.JournalPageTitle = Translate.GoodCardModelView_загрузка___;
      this.OnPropertyChanged("JournalPageTitle");
      Task.Run((Action) (() =>
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        List<Document> listDoc = GoodCardModelView.GetDocumentsForGood(g, false);
        stopwatch.Stop();
        DevelopersHelper.ShowNotification(string.Format("history load at {0}s", (object) ((double) stopwatch.ElapsedMilliseconds / 1000.0)));
        Task.Run((Action) (() =>
        {
          Application.Current?.Dispatcher?.Invoke((Action) (() =>
          {
            this.JournalPage = new PageJournalGood(g, listDoc, this.AuthUser, this._buyPriceCounter);
            this.OnPropertyChanged("JournalPage");
          }));
          this.JournalPageTitle = Translate.FrmGoodCard_Журнал;
          this.OnPropertyChanged("JournalPageTitle");
          this.IsEnabledForWaybill = true;
        }));
      }));
    }

    public static List<Document> GetDocumentsForGood(Gbs.Core.Entities.Goods.Good g, bool allDoc)
    {
      Performancer performancer = new Performancer(string.Format("Загрузка истории товара: good_uid: {0}", (object) g.Uid));
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
        {
          GoodUid = g.Uid,
          DateStart = (allDoc ? DateTime.Now.AddYears(-10) : DateTime.Now.AddYears(-1)),
          DateEnd = DateTime.Now
        });
        performancer.Stop();
        return itemsWithFilter;
      }
    }

    public bool IsEnabledPageMod
    {
      get
      {
        if (this.Good == null)
          return true;
        if (EgaisHelper.GetAlcoholType(this.Good) != EgaisHelper.AlcoholTypeGorEgais.NoAlcohol)
          return false;
        if (!this.Good.SetContent.Any<GoodsSets.Set>())
          return true;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          foreach (GoodsSets.Set set in this.Good.SetContent)
          {
            Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(set.GoodUid);
            if (EgaisHelper.GetAlcoholType(byUid) == EgaisHelper.AlcoholTypeGorEgais.NoAlcohol && byUid.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.Alcohol)
              return true;
          }
          return false;
        }
      }
    }

    private void Save()
    {
      if (this.GoodForDocumentType == GlobalDictionaries.DocumentsTypes.ProductionSet && !this.Good.SetContent.Any<GoodsSets.Set>())
        MessageBoxHelper.Warning(Translate.GoodCardModelView_Save_Невозможно_сохранить_рецепт__так_как_в_нем_нет_товаров__Добавьте_в_список_товары_для_производства_для_продолжения_);
      else if (this.Good.Name.Length < 3)
      {
        MessageBoxHelper.Warning(Translate.GoodCardModelView_Save_Некорректно_указано_наименование_товара__Название_должно_быть_длиной_не_менее_3_символов_);
      }
      else
      {
        bool isGroupStock = this.IsGroupStock;
        this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.Group)).IsChecked = true;
        this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedNullStock)).IsChecked = false;
        this.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedMinusStock)).IsChecked = false;
        this.GroupStock();
        bool flag = false;
        foreach (GoodsStocks.GoodStock stocksAndPrice in this.Good.StocksAndPrices)
        {
          GoodsStocks.GoodStock stock = stocksAndPrice;
          GoodsStocks.GoodStock stocksByUid = GoodsStocks.GetStocksByUid(stock.Uid);
          GoodsStocks.GoodStock goodStock = ((Gbs.Core.Entities.Goods.Good) this.EntityClone).StocksAndPrices.SingleOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Uid == stock.Uid));
          if (stocksByUid != null && goodStock != null)
          {
            if (stocksByUid.Stock != goodStock.Stock)
            {
              flag = true;
              Decimal num = stock.Stock - goodStock.Stock;
              stock.Stock = stocksByUid.Stock + num;
              goodStock.Stock = stock.Stock;
            }
            if (stocksByUid.Price != goodStock.Price)
            {
              flag = true;
              stock.Price = stocksByUid.Price;
              goodStock.Price = stock.Price;
            }
          }
        }
        if (flag && MessageBoxHelper.Show(Translate.GoodCardModelView_Save_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) == MessageBoxResult.No)
        {
          this.ExtraPricesDataTable = new ObservableCollection<GoodCardModelView.PriceAndStockView>(this.Good.StocksAndPrices.Select<GoodsStocks.GoodStock, GoodCardModelView.PriceAndStockView>((Func<GoodsStocks.GoodStock, GoodCardModelView.PriceAndStockView>) (x => new GoodCardModelView.PriceAndStockView()
          {
            GoodStock = x,
            BuyPrice = new Decimal?(this._buyPriceCounter.GetBuyPrice(x.Uid)),
            Modification = this.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == x.ModificationUid))
          })));
          this.ShowExtraPrice();
        }
        else
        {
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            GoodRepository goodRepository = new GoodRepository(dataBase);
            Gbs.Core.Entities.Goods.Good good = this.Good;
            List<string> stringList;
            if (!this.Barcodes.IsNullOrEmpty())
              stringList = ((IEnumerable<string>) this.Barcodes.Split('\r')).ToList<string>();
            else
              stringList = new List<string>();
            good.Barcodes = (IEnumerable<string>) stringList;
            this.Good.Barcodes = this.Good.Barcodes.Select<string, string>((Func<string, string>) (x => x.Replace("\r", "")));
            this.Good.Barcodes = this.Good.Barcodes.Select<string, string>((Func<string, string>) (x => x.Replace("\n", "")));
            if (goodRepository.Validate(this.Good).Result != ActionResult.Results.Ok)
              return;
            List<EntityProperties.PropertyValue> goodProperties = new List<EntityProperties.PropertyValue>();
            this.ValuesPropertiesList.ToList<GoodCardModelView.ValuesProperty>().ForEach((Action<GoodCardModelView.ValuesProperty>) (x => goodProperties.Add(x.Value)));
            if (this.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              EntityProperties.PropertyValue propertyValue1 = this.PropertyCertificate.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => GlobalDictionaries.CertificateReusableUid == x.Type.Uid));
              EntityProperties.PropertyValue propertyValue2 = propertyValue1;
              object obj1;
              switch (propertyValue1?.Value?.ToString())
              {
                case "True":
                  obj1 = (object) true;
                  break;
                case "False":
                  obj1 = (object) false;
                  break;
                default:
                  obj1 = propertyValue1.Value ?? (object) false;
                  break;
              }
              propertyValue2.Value = obj1;
              this.PropertyCertificate[this.PropertyCertificate.FindIndex((Predicate<EntityProperties.PropertyValue>) (x => GlobalDictionaries.CertificateReusableUid == x.Type.Uid))] = propertyValue1;
              goodProperties.AddRange((IEnumerable<EntityProperties.PropertyValue>) this.PropertyCertificate);
              object obj2 = this.PropertyCertificate.Single<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid)).Value;
              if (obj2 == null || obj2.ToString().IsNullOrEmpty())
              {
                int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Не_указан_номинал_товара_сертификата);
                return;
              }
            }
            this.Good.Properties = goodProperties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Value != null)).ToList<EntityProperties.PropertyValue>();
            if (!this.SaveAmClassifier())
              return;
            if (this.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              if (!this.SaveCertificate())
                return;
            }
            else if (isGroupStock)
            {
              foreach (GoodsStocks.GoodStock stocksAndPrice in this.Good.StocksAndPrices)
              {
                GoodsStocks.GoodStock item = stocksAndPrice;
                if (this.Good.SetStatus != GlobalDictionaries.GoodsSetStatuses.Set)
                {
                  Decimal buyPrice = this._buyPriceCounter.GetBuyPrice(item.Uid);
                  item.IsDeleted = !this.ExtraPricesDataTable.ToList<GoodCardModelView.PriceAndStockView>().Any<GoodCardModelView.PriceAndStockView>((Func<GoodCardModelView.PriceAndStockView, bool>) (x => x.GoodStock.Price == item.Price && x.GoodStock.Storage.Uid == item.Storage.Uid && x.BuyPrice.GetValueOrDefault() == buyPrice && x.GoodStock.ModificationUid == item.ModificationUid));
                }
                else
                  break;
              }
              foreach (IGrouping<\u003C\u003Ef__AnonymousType44<Decimal, Decimal, Guid, Guid, object>, GoodsStocks.GoodStock> source in this.Good.StocksAndPrices.GroupBy(x => new
              {
                Price = x.Price,
                buy = this._buyPriceCounter.GetBuyPrice(x.Uid),
                Uid = x.Storage.Uid,
                mUid = x.ModificationUid,
                Value = x.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.RegIdForGoodStockUidEgais))?.Value
              }).ToList<IGrouping<\u003C\u003Ef__AnonymousType44<Decimal, Decimal, Guid, Guid, object>, GoodsStocks.GoodStock>>())
              {
                GoodsStocks.GoodStock goodStock1 = source.OrderBy<GoodsStocks.GoodStock, Decimal>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)).First<GoodsStocks.GoodStock>();
                if (!(goodStock1.Stock >= 0M))
                {
                  foreach (GoodsStocks.GoodStock goodStock2 in (IEnumerable<GoodsStocks.GoodStock>) source)
                  {
                    if (!(goodStock2.Uid == goodStock1.Uid) && !(goodStock2.Stock <= 0M))
                    {
                      if (goodStock2.Stock + goodStock1.Stock < 0M)
                      {
                        goodStock1.Stock += goodStock2.Stock;
                        goodStock2.Stock = 0M;
                      }
                      else
                      {
                        goodStock2.Stock += goodStock1.Stock;
                        goodStock1.Stock = 0M;
                      }
                    }
                  }
                }
                else
                  break;
              }
            }
            if (this.Good.IsDeleted && MessageBoxHelper.Show(Translate.GoodCardModelView_Save_Данный_товар_был_ранее_удален__Восстановить_его_и_сделать_вновь_доступным_для_использования_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
              this.Good.IsDeleted = false;
            this.SaveGoodResult = !this.IsSaveGood || goodRepository.Save(this.Good);
            if (!this.SaveGoodResult)
              return;
            ((ImageGoodViewModel) this.ImageGood.DataContext).ImageDelete.ForEach(new Action<string>(File.Delete));
            if (this.IsSaveGood)
            {
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Good, this.IsEditCard ? ActionType.Edit : ActionType.Add, GlobalDictionaries.EntityTypes.Good, this.AuthUser), true);
              Task.Run((Action) (() =>
              {
                if (((IEnumerable<string>) new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr)).Any<string>((Func<string, bool>) (x => x == (this.Barcode.Length >= x.Length ? this.Barcode.Substring(0, x.Length) : ""))) || !new ConfigsRepository<Integrations>().Get().IsBarcodesMiDays || this.IsEditCard)
                  return;
                BarcodesMiDays.Add(this.Good);
              }));
            }
            WindowWithSize.IsCancel = false;
            this.CloseAction();
          }
        }
      }
    }

    private bool SaveCertificate()
    {
      if (CertificateViewModel.CertificatesDb.All<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.Certificate.IsDeleted)))
      {
        int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Невозможно_сохранить_данный_товар_без_указания_хотя_бы_одного_сертификата);
        return false;
      }
      Decimal? nominal = CertificateBasicViewModel.Nominal;
      Decimal price1 = CertificateBasicViewModel.Price;
      if (nominal.GetValueOrDefault() < price1 & nominal.HasValue && CertificateBasicViewModel.IsEnabledNominal && MessageBoxHelper.Show(Translate.GoodCardModelView_Номинал_меньше_стоимости_сертификата__Вы_уверены__что_хотите_сохранить_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
        return false;
      nominal = CertificateBasicViewModel.Nominal;
      Decimal price2 = CertificateBasicViewModel.Price;
      if (nominal.GetValueOrDefault() > price2 & nominal.HasValue && MessageBoxHelper.Show(Translate.GoodCardModelView_Номинал_больше_стоимости_сертификата__Уверены__что_хотите_сохранить_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        return false;
      this.Good.StocksAndPrices = CertificateViewModel.CertificatesDb.GroupBy<CertificateBasicViewModel.CertificateView, Guid>((Func<CertificateBasicViewModel.CertificateView, Guid>) (x => x.Certificate.Stock.Uid)).Select<IGrouping<Guid, CertificateBasicViewModel.CertificateView>, GoodsStocks.GoodStock>((Func<IGrouping<Guid, CertificateBasicViewModel.CertificateView>, GoodsStocks.GoodStock>) (x => x.First<CertificateBasicViewModel.CertificateView>().Certificate.Stock)).ToList<GoodsStocks.GoodStock>();
      if (!this.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>() && CertificateBasicViewModel.Price > 0M)
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          dataBase.GetTable<GOODS_STOCK>().Delete<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.GOOD_UID == this.Good.Uid));
        this.Good.StocksAndPrices.Add(new GoodsStocks.GoodStock()
        {
          Price = CertificateBasicViewModel.Price,
          Storage = Storages.GetStorages().First<Storages.Storage>()
        });
      }
      bool r = true;
      CertificateViewModel.CertificatesDb.Where<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.Certificate.Barcode != "")).ToList<CertificateBasicViewModel.CertificateView>().ForEach((Action<CertificateBasicViewModel.CertificateView>) (x => r &= x.Certificate.Save()));
      return r;
    }

    public IEntity EntityClone { get; set; }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public bool HasNoSavedChanges()
    {
      return CertificateViewModel.CertificatesDb.Where<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => !x.Certificate.Barcode.IsNullOrEmpty())).All<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.IsSaveInDb)) & Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Good);
    }

    public ICommand SelectGroupCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.SelectGroup()));
    }

    public ICommand SaveGood => (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));

    public ICommand GeneratedBarcode
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string[] strArray = this.DevicesConfig.BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr);
          if (strArray.Length == 0)
          {
            MessageBoxHelper.Warning(Translate.GoodCardModelView_В_разделе_Файл___Настройки___Оборудование___Сканер_ШК_требуется_указать_префикс_для_генерации_штрих_кода_);
          }
          else
          {
            this.IsRandomBarcode = true;
            this.Barcode = BarcodeHelper.RandomBarcode(strArray[0]);
            this.OnPropertyChanged("Good");
          }
        }));
      }
    }

    public ICommand PrintCardCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          PrintableReportFactory printableReportFactory = new PrintableReportFactory();
          List<Gbs.Core.Entities.Documents.Item> sets = new List<Gbs.Core.Entities.Documents.Item>();
          if (this.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Production) && this.Good.SetContent.Any<GoodsSets.Set>())
          {
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodCardModelView_PrintCardCommand_Подготовка_данных_для_печати);
            using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              foreach (GoodsSets.Set set in this.Good.SetContent.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => !x.IsDeleted)))
              {
                Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(set.GoodUid);
                sets.Add(new Gbs.Core.Entities.Documents.Item()
                {
                  Good = byUid,
                  Quantity = set.Quantity,
                  Discount = set.Discount,
                  SellPrice = byUid.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? byUid.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)) : 0M
                });
              }
              progressBar.Close();
            }
          }
          new FastReportFacade().SelectTemplateAndShowReport(printableReportFactory.CreateForGoodCard(this.Good, sets), this.AuthUser);
        }));
      }
    }

    public ICommand AuthUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (bool Result2, Gbs.Core.Entities.Users.User User2) = new Authorization().GetAccess(Actions.ViewStock);
          if (!Result2)
            return;
          this.AuthUser = User2;
          this.OnPropertyChanged("VisibilityGridStock");
          this.OnPropertyChanged("VisibilityTextBoxHistory");
          this.OnPropertyChanged("VisibilityGridHistory");
          this.OnPropertyChanged("VisibilityTextBox");
          this.OnPropertyChanged("VisibilityBuyPrice");
        }));
      }
    }

    public PageJournalGood JournalPage { get; set; }

    public PageImageGood ImageGood { get; set; }

    public PageСertificateBasic CertificateBasicPage { get; set; }

    public PageСertificate CertificatePage { get; set; }

    public Visibility VisibilityCertificate
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        return (good != null ? (good.Group?.GoodsType.GetValueOrDefault() == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityTabItems
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        return (good != null ? (good.Group?.GoodsType.GetValueOrDefault() == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityBasicTab
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good1 = this.Good;
        if ((good1 != null ? (good1.Group?.GoodsType.GetValueOrDefault() == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) != 0)
          return Visibility.Collapsed;
        Gbs.Core.Entities.Goods.Good good2 = this.Good;
        return (good2 != null ? (good2.SetStatus == GlobalDictionaries.GoodsSetStatuses.Kit ? 1 : 0) : 0) != 0 || this.GoodForDocumentType == GlobalDictionaries.DocumentsTypes.ProductionSet ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityModificationTab
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        bool? nullable;
        if (good == null)
        {
          nullable = new bool?();
        }
        else
        {
          Gbs.Core.Entities.GoodGroups.Group group = good.Group;
          if (group == null)
            nullable = new bool?();
          else
            nullable = new bool?(group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service));
        }
        return nullable.GetValueOrDefault() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public bool IsEnabledSetModificationType
    {
      get
      {
        if (this.GoodForDocumentType == GlobalDictionaries.DocumentsTypes.ProductionSet)
          return false;
        return !this.IsEditCard || this.GoodForDocumentType != GlobalDictionaries.DocumentsTypes.Buy;
      }
    }

    public Visibility VisibilityModification
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        return (good != null ? (good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private ExtraPriceRule ExtraPriceRule { get; set; }

    private IEnumerable<GoodsExtraPrice.GoodExtraPrice> GoodExtraPrices { get; set; } = (IEnumerable<GoodsExtraPrice.GoodExtraPrice>) GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)).ToList<GoodsExtraPrice.GoodExtraPrice>();

    public ObservableCollection<GoodCardModelView.PriceAndStockView> ExtraPricesDataTable
    {
      get => this._extraPricesDataTable;
      set
      {
        this._extraPricesDataTable = value;
        BindingOperations.EnableCollectionSynchronization((IEnumerable) this._extraPricesDataTable, this._extraPricesCollectionLock);
        this.OnPropertyChanged(nameof (ExtraPricesDataTable));
      }
    }

    private void ShowExtraPrice()
    {
      List<ExtraPriceRule> list = new ExtraPriceRulesRepository().GetActiveItems().Where<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => x.Groups.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (g =>
      {
        Guid uid1 = g.Uid;
        Guid? uid2 = this.Good?.Group?.Uid;
        return uid2.HasValue && uid1 == uid2.GetValueOrDefault();
      })))).ToList<ExtraPriceRule>();
      if (list.Count == 0)
        return;
      this.ExtraPriceRule = list.Count == 1 ? list.Single<ExtraPriceRule>() : list.Single<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => x.Uid != GlobalDictionaries.DefaultExtraRuleUid));
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        foreach (GoodCardModelView.PriceAndStockView priceAndStockView in (Collection<GoodCardModelView.PriceAndStockView>) this.ExtraPricesDataTable)
        {
          GoodCardModelView.PriceAndStockView goodStock = priceAndStockView;
          goodStock.ExtraPrice = this.CountExtraPrice(goodStock).ToList<Decimal>();
          goodStock.Modification = GoodsModifications.GetModificationsList(dataBase.GetTable<GOODS_MODIFICATIONS>().Where<GOODS_MODIFICATIONS>((Expression<Func<GOODS_MODIFICATIONS, bool>>) (x => x.UID == goodStock.GoodStock.ModificationUid))).FirstOrDefault<GoodsModifications.GoodModification>();
        }
      }
    }

    private IEnumerable<Decimal> CountExtraPrice(GoodCardModelView.PriceAndStockView stock)
    {
      if (this.ExtraPriceRule == null)
        return (IEnumerable<Decimal>) new List<Decimal>();
      List<Decimal> numList1 = new List<Decimal>();
      foreach (GoodsExtraPrice.GoodExtraPrice goodExtraPrice in this.GoodExtraPrices)
      {
        GoodsExtraPrice.GoodExtraPrice price = goodExtraPrice;
        ExtraPriceRule.ItemPricing itemPricing = this.ExtraPriceRule.Items.FirstOrDefault<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (x => x.Price.Uid == price.Uid));
        if (itemPricing == null)
          numList1.Add(0M * stock.GoodStock.Price);
        else if (itemPricing.Type == GoodsExtraPrice.TypeCoeff.SalePrice)
        {
          numList1.Add(itemPricing.Value * stock.GoodStock.Price);
        }
        else
        {
          List<Decimal> numList2 = numList1;
          Decimal num = itemPricing.Value;
          Decimal? buyPrice = stock.BuyPrice;
          Decimal valueOrDefault = (buyPrice.HasValue ? new Decimal?(num * buyPrice.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault();
          numList2.Add(valueOrDefault);
        }
      }
      return (IEnumerable<Decimal>) numList1;
    }

    public ObservableCollection<GoodCardModelView.ValuesProperty> ValuesPropertiesList { get; set; } = new ObservableCollection<GoodCardModelView.ValuesProperty>();

    private List<EntityProperties.PropertyValue> PropertyCertificate { get; set; } = new List<EntityProperties.PropertyValue>();

    private void ShowProperty()
    {
      List<EntityProperties.PropertyType> list1 = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)).ToList<EntityProperties.PropertyType>();
      List<GoodCardModelView.ValuesProperty> list2 = list1.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (propertyType => propertyType.Uid != GlobalDictionaries.CertificateNominalUid && propertyType.Uid != GlobalDictionaries.CertificateReusableUid)).Select(propertyType =>
      {
        EntityProperties.PropertyType propertyType1 = propertyType;
        EntityProperties.PropertyValue propertyValue;
        if (!this.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == propertyType.Uid)))
          propertyValue = new EntityProperties.PropertyValue()
          {
            EntityUid = this.Good.Uid,
            Type = propertyType
          };
        else
          propertyValue = this.Good.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == propertyType.Uid));
        return new
        {
          propertyType = propertyType1,
          propertyValue = propertyValue
        };
      }).Select(_param1 => new GoodCardModelView.ValuesProperty()
      {
        Value = _param1.propertyValue,
        Type = _param1.propertyType
      }).ToList<GoodCardModelView.ValuesProperty>().Where<GoodCardModelView.ValuesProperty>((Func<GoodCardModelView.ValuesProperty, bool>) (x => x.Type.Uid != GlobalDictionaries.AmClassifierIdUid)).ToList<GoodCardModelView.ValuesProperty>();
      if (EgaisHelper.GetAlcoholType(this.Good) == EgaisHelper.AlcoholTypeGorEgais.NoAlcohol)
        list2 = list2.Where<GoodCardModelView.ValuesProperty>((Func<GoodCardModelView.ValuesProperty, bool>) (x => !x.Type.Uid.IsEither<Guid>(GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid))).ToList<GoodCardModelView.ValuesProperty>();
      this.ValuesPropertiesList = new ObservableCollection<GoodCardModelView.ValuesProperty>((IEnumerable<GoodCardModelView.ValuesProperty>) list2.OrderBy<GoodCardModelView.ValuesProperty, string>((Func<GoodCardModelView.ValuesProperty, string>) (x => x.Type.Name)));
      List<EntityProperties.PropertyValue> propertyCertificate1 = this.PropertyCertificate;
      EntityProperties.PropertyValue propertyValue1;
      if (!this.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid)))
        propertyValue1 = new EntityProperties.PropertyValue()
        {
          EntityUid = this.Good.Uid,
          Type = list1.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.CertificateNominalUid))
        };
      else
        propertyValue1 = this.Good.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid));
      propertyCertificate1.Add(propertyValue1);
      List<EntityProperties.PropertyValue> propertyCertificate2 = this.PropertyCertificate;
      EntityProperties.PropertyValue propertyValue2;
      if (!this.Good.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateReusableUid)))
        propertyValue2 = new EntityProperties.PropertyValue()
        {
          EntityUid = this.Good.Uid,
          Type = list1.Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == GlobalDictionaries.CertificateReusableUid))
        };
      else
        propertyValue2 = this.Good.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateReusableUid));
      propertyCertificate2.Add(propertyValue2);
    }

    public Dictionary<GlobalDictionaries.GoodsSetStatuses, string> GoodSetStatuses { get; set; } = GlobalDictionaries.GoodsSetStatusesDictionary().Where<KeyValuePair<GlobalDictionaries.GoodsSetStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.GoodsSetStatuses, string>, bool>) (x => x.Key.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Production))).ToDictionary<KeyValuePair<GlobalDictionaries.GoodsSetStatuses, string>, GlobalDictionaries.GoodsSetStatuses, string>((Func<KeyValuePair<GlobalDictionaries.GoodsSetStatuses, string>, GlobalDictionaries.GoodsSetStatuses>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.GoodsSetStatuses, string>, string>) (x => x.Value));

    public GlobalDictionaries.GoodsSetStatuses GoodsSetStatus
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good1 = this.Good;
        if ((good1 != null ? (good1.Group?.GoodsType.GetValueOrDefault() == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) != 0)
          return GlobalDictionaries.GoodsSetStatuses.None;
        Action contentEditPageAction = this.SetContentEditPageAction;
        if (contentEditPageAction != null)
          contentEditPageAction();
        Gbs.Core.Entities.Goods.Good good2 = this.Good;
        this.SelectedTab = (good2 != null ? (good2.SetStatus != 0 ? 1 : 0) : 1) != 0 ? 4 : 0;
        this.OnPropertyChanged("VisibilityBasicTab");
        this.OnPropertyChanged("SelectedTab");
        this.OnPropertyChanged("VisibilityModificationTab");
        Gbs.Core.Entities.Goods.Good good3 = this.Good;
        return good3 == null ? GlobalDictionaries.GoodsSetStatuses.None : good3.SetStatus;
      }
      set
      {
        GoodCardModelView.\u003C\u003Ec__DisplayClass218_0 displayClass2180 = new GoodCardModelView.\u003C\u003Ec__DisplayClass218_0();
        displayClass2180.\u003C\u003E4__this = this;
        displayClass2180.origValue = this.GoodsSetStatus;
        Gbs.Core.Entities.Goods.Good good = this.Good;
        if ((good != null ? (good.SetStatus == value ? 1 : 0) : 0) != 0)
          return;
        GoodCardModelView.\u003C\u003Ec__DisplayClass218_1 displayClass2181 = new GoodCardModelView.\u003C\u003Ec__DisplayClass218_1();
        displayClass2181.CS\u0024\u003C\u003E8__locals1 = displayClass2180;
        displayClass2181.db = Gbs.Core.Data.GetDataBase();
        try
        {
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          ParameterExpression parameterExpression3;
          IQueryable<DOCUMENTS> query = displayClass2181.db.GetTable<DOCUMENTS>().SelectMany(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, IEnumerable<DOCUMENT_ITEMS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
          {
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(displayClass2181.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
          }), parameterExpression1), (d, di) => new
          {
            d = d,
            di = di
          }).SelectMany(System.Linq.Expressions.Expression.Lambda<Func<\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>, IEnumerable<GOODS_STOCK>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
          {
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(displayClass2181.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) (x => x.GOOD_UID == this.Good.Uid))
          }), parameterExpression3), (data, gs) => data.d).Distinct<DOCUMENTS>();
          if (new DocumentsRepository(displayClass2181.db).GetByQuery(query).ToList<Document>().Any<Document>())
            Application.Current?.Dispatcher?.BeginInvoke((Delegate) new Action(displayClass2181.CS\u0024\u003C\u003E8__locals1.\u003Cset_GoodsSetStatus\u003Eb__4), DispatcherPriority.ContextIdle, (object[]) null);
          else
            this.Good.SetStatus = value;
        }
        finally
        {
          if (displayClass2181.db != null)
            displayClass2181.db.Dispose();
        }
        this.SetContentEditPageAction();
        this.OnPropertyChanged(nameof (GoodsSetStatus));
        this.OnPropertyChanged("VisibilityStockForGood");
        this.OnPropertyChanged("VisibilityModification");
        this.OnPropertyChanged("VisibilityBasicTab");
        this.OnPropertyChanged("VisibilityModificationTab");
        this.SelectedTab = 4;
        this.OnPropertyChanged("SelectedTab");
      }
    }

    public Action SetContentEditPageAction { private get; set; }

    public string GroupName
    {
      get
      {
        if (this.GroupUid == Guid.Empty)
          return Translate.GoodCardModelView_Выберите;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          return new GoodGroupsRepository(dataBase).GetByUid(this.GroupUid)?.Name ?? Translate.GoodCardModelView_Выберите;
      }
    }

    private Guid GroupUid
    {
      get => this.Good?.Group?.Uid ?? Guid.Empty;
      set
      {
        if (value == Guid.Empty)
          return;
        this.Good.Group = this.ListGroup.First<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == value));
        this.OnPropertyChanged(nameof (GroupUid));
        this.OnPropertyChanged("GroupName");
        this.OnPropertyChanged("VisibilityModificationTab");
        this.OnPropertyChanged("VisibilityBasicTab");
        this.OnPropertyChanged("VisibilityTabItems");
        this.OnPropertyChanged("VisibilityCertificate");
        Gbs.Core.Entities.Goods.Good good = this.Good;
        this.SelectedTab = (good != null ? (good.Group?.GoodsType.GetValueOrDefault() == GlobalDictionaries.GoodTypes.Certificate ? 1 : 0) : 0) != 0 ? 1 : this.SelectedTab;
        this.OnPropertyChanged("SelectedTab");
        this.OnPropertyChanged("VisibilityStockForGood");
        this.OnPropertyChanged("VisibilityBuyPrice");
      }
    }

    private List<Gbs.Core.Entities.GoodGroups.Group> ListGroup { get; set; }

    public string JournalPageTitle { get; set; }

    public Hdm.CategoryItem AmClassSelectedItem { get; set; }

    public string AmClassSelectedValue { get; set; }

    private void SelectGroup()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GoodCardModelView.\u003C\u003Ec__DisplayClass244_0 displayClass2440 = new GoodCardModelView.\u003C\u003Ec__DisplayClass244_0();
      // ISSUE: reference to a compiler-generated field
      displayClass2440.\u003C\u003E4__this = this;
      if (this.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Данная_услуга_является_системной__у_нее_нельзя_менять_категорию__добавлять___редактировать___удалять_остатки_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out displayClass2440.parentGr) || displayClass2440.parentGr == null)
          return;
        // ISSUE: reference to a compiler-generated method
        if (!this.ListGroup.Any<Gbs.Core.Entities.GoodGroups.Group>(new Func<Gbs.Core.Entities.GoodGroups.Group, bool>(displayClass2440.\u003CSelectGroup\u003Eb__0)))
        {
          // ISSUE: reference to a compiler-generated field
          this.ListGroup.Add(displayClass2440.parentGr);
        }
        if (this.Good.Group != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GoodCardModelView.\u003C\u003Ec__DisplayClass244_1 displayClass2441 = new GoodCardModelView.\u003C\u003Ec__DisplayClass244_1()
          {
            CS\u0024\u003C\u003E8__locals1 = displayClass2440
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          displayClass2441.newGroup = displayClass2441.CS\u0024\u003C\u003E8__locals1.parentGr;
          // ISSUE: reference to a compiler-generated field
          if (displayClass2441.newGroup.GoodsType != this.Good.Group.GoodsType)
          {
            // ISSUE: reference to a compiler-generated field
            if (displayClass2441.newGroup.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
            {
              if (this.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Weight, GlobalDictionaries.GoodTypes.Single))
                goto label_9;
            }
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            GoodCardModelView.\u003C\u003Ec__DisplayClass244_2 displayClass2442 = new GoodCardModelView.\u003C\u003Ec__DisplayClass244_2();
            // ISSUE: reference to a compiler-generated field
            displayClass2442.CS\u0024\u003C\u003E8__locals2 = displayClass2441;
            // ISSUE: reference to a compiler-generated field
            displayClass2442.db = Gbs.Core.Data.GetDataBase();
            try
            {
              ParameterExpression parameterExpression1;
              ParameterExpression parameterExpression2;
              ParameterExpression parameterExpression3;
              ParameterExpression parameterExpression4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: type reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: method reference
              IQueryable<DOCUMENTS> query = displayClass2442.db.GetTable<DOCUMENTS>().SelectMany(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, IEnumerable<DOCUMENT_ITEMS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(displayClass2442.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
              }), parameterExpression1), (d, di) => new
              {
                d = d,
                di = di
              }).SelectMany(System.Linq.Expressions.Expression.Lambda<Func<\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>, IEnumerable<GOODS_STOCK>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(displayClass2442.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<GOODS_STOCK, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>.get_di), __typeref (\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_STOCK_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression4, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS_STOCK.get_GOOD_UID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this, typeof (GoodCardModelView)), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GoodCardModelView.get_Good))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression4))
              }), parameterExpression3), (data, gs) => data.d).Distinct<DOCUMENTS>();
              // ISSUE: reference to a compiler-generated field
              if (!new DocumentsRepository(displayClass2442.db).GetByQuery(query).ToList<Document>().Any<Document>())
              {
                Gbs.Core.Entities.Goods.Good good = this.Good;
                if ((good != null ? (good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? 1 : 0) : 0) == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.GroupUid = displayClass2442.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.parentGr.Uid;
                  goto label_19;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int num2 = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_SelectGroup_Смена_категории_невозможна__так_как_типы_товаров_для_этих_категорий_отличаются_ + string.Format(Translate.GoodCardModelView_SelectGroup__0___старая_____1_, (object) this.Good.Group.Name, (object) (GlobalDictionaries.GoodTypesDictionary().SingleOrDefault<KeyValuePair<GlobalDictionaries.GoodTypes, string>>(new Func<KeyValuePair<GlobalDictionaries.GoodTypes, string>, bool>(displayClass2442.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.\u003CSelectGroup\u003Eb__7)).Value ?? "")) + string.Format(Translate.GoodCardModelView_SelectGroup_, (object) displayClass2442.CS\u0024\u003C\u003E8__locals2.newGroup.Name, (object) (GlobalDictionaries.GoodTypesDictionary().SingleOrDefault<KeyValuePair<GlobalDictionaries.GoodTypes, string>>(new Func<KeyValuePair<GlobalDictionaries.GoodTypes, string>, bool>(displayClass2442.CS\u0024\u003C\u003E8__locals2.\u003CSelectGroup\u003Eb__8)).Value ?? "")));
              return;
            }
            finally
            {
              // ISSUE: reference to a compiler-generated field
              if (displayClass2442.db != null)
              {
                // ISSUE: reference to a compiler-generated field
                displayClass2442.db.Dispose();
              }
            }
          }
label_9:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.GroupUid = displayClass2441.CS\u0024\u003C\u003E8__locals1.parentGr.Uid;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.GroupUid = displayClass2440.parentGr.Uid;
        }
label_19:
        this.OnPropertyChanged("VisibilityButtonGroup");
        this.OnPropertyChanged("VisibilityTabControl");
      }
    }

    public enum StockOption
    {
      Group,
      CollapsedNullStock,
      CollapsedMinusStock,
    }

    public class StockOptionItem : ViewModel
    {
      private bool _isChecked;

      public bool IsChecked
      {
        get => this._isChecked;
        set
        {
          this._isChecked = value;
          this.OnPropertyChanged(nameof (IsChecked));
        }
      }

      public GoodCardModelView.StockOption Option { get; set; }

      public string Text { get; set; }
    }

    public class PriceAndStockView : ViewModelWithForm
    {
      private Decimal? _buyPrice;
      private string _markedCode;
      private string _fbNumberForEgais;
      private List<Decimal> _extraPrice;
      private GoodsStocks.GoodStock _goodStock;
      private GoodsModifications.GoodModification _modification;

      public GoodsModifications.GoodModification Modification
      {
        get => this._modification;
        set
        {
          this._modification = value;
          this.OnPropertyChanged(nameof (Modification));
        }
      }

      public Decimal? BuyPrice
      {
        get => this._buyPrice;
        set
        {
          this._buyPrice = value;
          this.OnPropertyChanged(nameof (BuyPrice));
        }
      }

      public string MarkedCode
      {
        get => this._markedCode;
        set
        {
          this._markedCode = value;
          this.OnPropertyChanged(nameof (MarkedCode));
        }
      }

      public string FbNumberForEgais
      {
        get => this._fbNumberForEgais;
        set
        {
          this._fbNumberForEgais = value;
          this.OnPropertyChanged(nameof (FbNumberForEgais));
        }
      }

      public GoodsStocks.GoodStock GoodStock
      {
        get => this._goodStock;
        set
        {
          this._goodStock = value;
          this.OnPropertyChanged(nameof (GoodStock));
        }
      }

      public List<Decimal> ExtraPrice
      {
        get => this._extraPrice;
        set
        {
          this._extraPrice = value;
          this.OnPropertyChanged(nameof (ExtraPrice));
        }
      }
    }

    public class ValuesProperty
    {
      public EntityProperties.PropertyType Type { get; set; }

      public EntityProperties.PropertyValue Value { get; set; }

      public DateTime? DateValue
      {
        get
        {
          DateTime result;
          return DateTime.TryParse(this.Value.Value?.ToString(), out result) ? new DateTime?(result) : new DateTime?();
        }
        set => this.Value.Value = (object) value;
      }

      public string TextValue
      {
        get => this.Value.Value?.ToString();
        set => this.Value.Value = (object) value;
      }

      public string SystemValue
      {
        get => this.Value.Value?.ToString();
        set => this.Value.Value = (object) value;
      }

      public int? IntegerValue
      {
        get
        {
          int result;
          return int.TryParse(this.Value.Value?.ToString(), out result) ? new int?(result) : new int?();
        }
        set => this.Value.Value = (object) value;
      }

      public int? AutoNumValue
      {
        get
        {
          int result;
          if (!int.TryParse(this.Value.Value?.ToString(), out result))
            return new int?();
          return result > 0 ? new int?(result) : new int?();
        }
        set
        {
          EntityProperties.PropertyValue propertyValue = this.Value;
          int? nullable = value;
          int num = 0;
          __Boxed<int?> local = (ValueType) (nullable.GetValueOrDefault() <= num & nullable.HasValue ? new int?() : value);
          propertyValue.Value = (object) local;
        }
      }

      public Decimal? DecimalValue
      {
        get
        {
          Decimal result;
          return Decimal.TryParse(this.Value.Value?.ToString(), out result) ? new Decimal?(result) : new Decimal?();
        }
        set => this.Value.Value = (object) value;
      }

      public Visibility IntegerVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.Integer ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public Visibility DecimalVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.Decimal ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public Visibility TextVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.Text ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public Visibility DateVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.DateTime ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public Visibility AutoNumVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.AutoNum ? Visibility.Collapsed : Visibility.Visible;
        }
      }

      public Visibility SystemVisibility
      {
        get
        {
          return this.Type.Type != GlobalDictionaries.EntityPropertyTypes.System ? Visibility.Collapsed : Visibility.Visible;
        }
      }
    }
  }
}
