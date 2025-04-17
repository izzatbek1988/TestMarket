// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Waybills.WaybillCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Waybill;
using Gbs.Forms._shared;
using Gbs.Forms.ActionsPayments;
using Gbs.Forms.Clients;
using Gbs.Forms.Excel;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods;
using Gbs.Forms.SendWaybills;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Waybills
{
  public partial class WaybillCardViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    public static List<Guid> _listUidSendWaybill = new List<Guid>();
    private BuyPriceCounter _buyPriceCounter;
    private static int ErrorCountDeveloper = 0;
    public Action UpdateEgaisAction;

    public Visibility VisibilityNoForReturn
    {
      get => !this.IsReturnBuy ? Visibility.Visible : Visibility.Collapsed;
    }

    public static string UidBuySum { get; set; } = "43450469-B88F-4A65-BA2A-44085EEA77D9";

    public bool IsEdit { get; set; }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public static object IconNew { get; set; }

    public static string AlsoMenuKey => "AlsoMenu";

    public static string MoreMenuKey => "MoreMenu";

    public bool SaveResult { get; private set; }

    public Document Document { get; set; } = new Document();

    public List<Storages.Storage> ListStorage { get; set; }

    public bool IsEnabledStorage { get; set; } = true;

    public Gbs.Core.ViewModels.Waybill.Waybill Waybill { get; set; }

    public bool InWay
    {
      get => this.Document.Status == GlobalDictionaries.DocumentsStatuses.Draft;
      set
      {
        this.Document.Status = value ? GlobalDictionaries.DocumentsStatuses.Draft : GlobalDictionaries.DocumentsStatuses.Open;
      }
    }

    public string ContractorName
    {
      get
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          return this.Document.ContractorUid == Guid.Empty ? Translate.GoodCardModelView_Выберите : new ClientsRepository(dataBase).GetByUid(this.Document.ContractorUid)?.Name ?? Translate.GoodCardModelView_Выберите;
      }
    }

    public ICommand AddItemFromNewGoodCommand { get; set; }

    public ICommand AddItemFromCatalogCommand { get; set; }

    public ICommand AddItemFromExcelCommand { get; set; }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.EgaisDocument != null)
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_DeleteCommand_В_накладной__которая_добавлена_из_ЕГАИС_нельзя_удалять_позиции__Возможно_только_изменить_количество_до_0_у_нужных_позиций_);
          else
            this.Waybill.DeleteItemCommand.Execute(obj);
        }));
      }
    }

    public ICommand EditGoodCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.SelectedItems.Any<WaybillItem>() || this.SelectedItems.Count > 1)
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выбрать_один_товар_);
          }
          else
          {
            Gbs.Core.Entities.Goods.Good goodNew = this.ListGoodSave.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => this.SelectedItems.Single<WaybillItem>().Good.Uid == x.Uid));
            Gbs.Core.Entities.Goods.Good good;
            if (!new FrmGoodCard().ShowGoodCard(this.SelectedItems.Single<WaybillItem>().Good.Uid, out good, true, this.AuthUser, goodNew, false, GlobalDictionaries.DocumentsTypes.Buy))
              return;
            WaybillItem waybillItem = this.SelectedItems.Single<WaybillItem>().Clone<WaybillItem>();
            waybillItem.Good = good;
            this.Waybill.Items[this.Waybill.Items.ToList<WaybillItem>().FindIndex((Predicate<WaybillItem>) (x => x.Uid == this.SelectedItems.Single<WaybillItem>().Uid))] = waybillItem;
            if (this.ListGoodSave.Any<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == good.Uid)))
              this.ListGoodSave[this.ListGoodSave.FindIndex((Predicate<Gbs.Core.Entities.Goods.Good>) (x => x.Uid == good.Uid))] = good;
            else
              this.ListGoodSave.Add(good);
            this.OnPropertyChanged("Items");
          }
        }));
      }
    }

    public ICommand AddItemFromPointCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.EgaisDocument != null)
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_AddItemFromPointCommand_В_накладную__которая_добавлена_из_ЕГАИС__нельзя_добавлять_новые_позиции_);
          }
          else
          {
            MoveHelper.DisplayDocumentSend doc = new MoveListViewModel().GetMoveDocument(this.AuthUser);
            if (doc == null)
              return;
            if (WaybillCardViewModel._listUidSendWaybill.Any<Guid>((Func<Guid, bool>) (x => x == doc.Document.Document.Uid)))
            {
              int num = (int) MessageBoxHelper.Show(Translate.WaybillCardViewModel_AddItemFromPointCommand_Данное_перемещение_уже_открыто_в_одной_из_карточек_поступления__проверьте_открытые_окна_, icon: MessageBoxImage.Exclamation);
              Other.IsActiveAndShowForm<FrmWaybillCard>(doc.Document.Document.Uid.ToString());
            }
            else
            {
              this.FormToSHow.Uid = doc.Document.Document.Uid.ToString();
              WaybillCardViewModel._listUidSendWaybill.Add(doc.Document.Document.Uid);
              this.DocMoveUid = doc.Document.Document.Uid;
              this.DocFileMoveUid = doc.Document.Uid;
              this.VisibilityAddFromPoint = Visibility.Collapsed;
              this.OnPropertyChanged("VisibilityAddFromPoint");
              using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
              {
                if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Gbs.Core.Entities.Actions.ShowBuyPrice))
                {
                  System.Windows.Controls.DataGrid gridWaybillItems = ((FrmWaybillCard) this.FormToSHow).GridWaybillItems;
                  gridWaybillItems.Columns.Remove(gridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
                  gridWaybillItems.Columns.Remove(gridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillCardViewModel.UidBuySum)));
                  this.Waybill.IsShowBuyPrice = false;
                  this.VisibilityBuyPrice = Visibility.Collapsed;
                  this.OnPropertyChanged("VisibilityBuyPrice");
                }
                this.ListGoodSave.AddRange(doc.Document.GoodList.GroupBy<Gbs.Core.Entities.Goods.Good, Guid>((Func<Gbs.Core.Entities.Goods.Good, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>, Gbs.Core.Entities.Goods.Good>((Func<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>, Gbs.Core.Entities.Goods.Good>) (x => x.First<Gbs.Core.Entities.Goods.Good>())));
                this.ListEntityTypeSave.AddRange(doc.Document.GoodList.SelectMany<Gbs.Core.Entities.Goods.Good, EntityProperties.PropertyValue>((Func<Gbs.Core.Entities.Goods.Good, IEnumerable<EntityProperties.PropertyValue>>) (x => (IEnumerable<EntityProperties.PropertyValue>) x.Properties)).GroupBy<EntityProperties.PropertyValue, Guid>((Func<EntityProperties.PropertyValue, Guid>) (x => x.Type.Uid)).Select<IGrouping<Guid, EntityProperties.PropertyValue>, EntityProperties.PropertyType>((Func<IGrouping<Guid, EntityProperties.PropertyValue>, EntityProperties.PropertyType>) (x => x.First<EntityProperties.PropertyValue>().Type)));
                foreach (Gbs.Core.Entities.Documents.Item obj1 in doc.Document.DocumentEntity.Items)
                {
                  Gbs.Core.Entities.Documents.Item x = obj1;
                  this.Waybill.AddGood(new WaybillItem(x.Good, x.Quantity, x.BuyPrice, salePrice: x.SellPrice)
                  {
                    GoodModification = x.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == x.ModificationUid)),
                    FbNumberForEgais = x.FbNumberForEgais
                  });
                }
                this.Document.Comment = string.Format(Translate.WaybillCardViewModel_AddItemFromPointCommand_Перемещение_товара_из_точки__0__от__1_, (object) doc.Document.SenderPointName, (object) doc.Document.Document.DateTime.ToString("dd.MM.yyyy HH:mm"));
                this.OnPropertyChanged("Document");
              }
            }
          }
        }));
      }
    }

    public ICommand AddItemFromUpd
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "XML (*.xml)|*.xml",
            Multiselect = false,
            DefaultExt = ".xml"
          };
          Gbs.Core.Entities.GoodGroups.Group group;
          if (openFileDialog.ShowDialog() != DialogResult.OK || !new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out group))
            return;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.WaybillCardViewModel_AddItemFromUpd_Загрузка_данных_из_документа__ЭДО_);
          try
          {
            List<Gbs.Core.Entities.Goods.Good> listForSaveGood;
            foreach (WaybillItem waybillItem in EdoHelper.DeserializeUpd(openFileDialog.FileName, group, out listForSaveGood).Select<BasketItem, WaybillItem>((Func<BasketItem, WaybillItem>) (x => new WaybillItem(x.Good, x.Quantity, x.BuyPrice, salePrice: x.SalePrice))))
              this.Waybill.AddGood(waybillItem);
            this.ListGoodSave.AddRange((IEnumerable<Gbs.Core.Entities.Goods.Good>) listForSaveGood);
            progressBar.Close();
          }
          catch (Exception ex)
          {
            progressBar.Close();
            throw ex;
          }
        }));
      }
    }

    public Guid DocMoveUid { get; set; } = Guid.Empty;

    private Guid DocFileMoveUid { get; set; } = Guid.Empty;

    public Visibility VisibilityAddFromPoint { get; set; }

    public Visibility VisibilityAddFromEdo
    {
      get
      {
        return new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand SelectContractorCommand { get; set; }

    public ICommand CopyItemCommand { get; set; }

    public ICommand ShowMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<WaybillItem> list = ((IEnumerable) obj).Cast<WaybillItem>().ToList<WaybillItem>();
          if (!list.Any<WaybillItem>())
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выделить_хотя_бы_одну_позицию_);
          }
          else
          {
            this.SelectedItems = list;
            this.ShowMenu();
          }
        }));
      }
    }

    public ICommand PricingItemCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.ReCalcSalePrice()));
    }

    public ICommand ReplaceGoodCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.SelectedItems.Any<WaybillItem>() || this.SelectedItems.Count > 1)
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выбрать_один_товар_);
          else if (this.SelectedItems.Single<WaybillItem>().IsNewItem == null)
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Возможно_заменить_только_новый_товар__добавленный_из_Excel);
          }
          else
          {
            (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Buy, isVisNullStock: true);
            if (!tuple.goods.Any<Gbs.Core.Entities.Goods.Good>())
              return;
            if (tuple.goods.Count > 1)
            {
              MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выбрать_один_товар_);
            }
            else
            {
              WaybillItem waybillItem = this.SelectedItems.Single<WaybillItem>();
              this.Waybill.Items[this.Waybill.Items.ToList<WaybillItem>().FindIndex((Predicate<WaybillItem>) (x => x.Uid == this.SelectedItems.Single<WaybillItem>().Uid))] = new WaybillItem(tuple.goods.Single<Gbs.Core.Entities.Goods.Good>(), waybillItem.Quantity, waybillItem.BuyPrice, salePrice: waybillItem.SalePrice, uid: new Guid?(waybillItem.Uid));
              this.OnPropertyChanged("Items");
            }
          }
        }));
      }
    }

    public Action ShowMenu { get; set; }

    public ICommand SaveWaybillCommand { get; set; }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Close()));
    }

    public Action UpdateSortGrid { get; set; }

    public List<WaybillsViewModel.WaybillItemsInfoGrid> ListDoc { get; set; }

    public List<Gbs.Core.Entities.Goods.Good> ListGoodSave { get; set; } = new List<Gbs.Core.Entities.Goods.Good>();

    private List<EntityProperties.PropertyType> ListEntityTypeSave { get; set; } = new List<EntityProperties.PropertyType>();

    private List<WaybillItem> SelectedItems { get; set; } = new List<WaybillItem>();

    public WaybillCardViewModel()
    {
    }

    public WaybillCardViewModel(
      Document doc,
      bool visibilityBuyPrice,
      bool isReturnBuy,
      FrmWaybillCard w)
    {
      this.FormToSHow = (WindowWithSize) w;
      this.IsReturnBuy = isReturnBuy;
      this.LoadWaybill(doc);
      this.Waybill.IsShowBuyPrice = visibilityBuyPrice;
      this.VisibilityAddFromPoint = this.IsReturnBuy || !MoveHelper.IsNewDocument() ? Visibility.Collapsed : Visibility.Visible;
      this.AddItemFromNewGoodCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.EgaisDocument != null)
        {
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_AddItemFromPointCommand_В_накладную__которая_добавлена_из_ЕГАИС__нельзя_добавлять_новые_позиции_);
        }
        else
        {
          Gbs.Core.Entities.Goods.Good good;
          if (!new FrmGoodCard().ShowGoodCard(Guid.Empty, out good, authUser: this.AuthUser, isSave: false))
            return;
          if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Невозможно_добавить_товар_сертификат_в_накладную);
          else if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_WaybillCardViewModel_Невозможно_добавить_услугу_в_накладную);
          }
          else
          {
            this.AddListItems((IEnumerable<Gbs.Core.Entities.Goods.Good>) new List<Gbs.Core.Entities.Goods.Good>()
            {
              good
            });
            this.ListGoodSave.Add(good);
          }
        }
      }));
      this.AddItemFromCatalogCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.EgaisDocument != null)
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_AddItemFromPointCommand_В_накладную__которая_добавлена_из_ЕГАИС__нельзя_добавлять_новые_позиции_);
        else
          this.AddListItems((IEnumerable<Gbs.Core.Entities.Goods.Good>) new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Buy, isVisNullStock: true, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddListItems), user: this.AuthUser).goods);
      }));
      this.CopyItemCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.EgaisDocument != null)
        {
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_AddItemFromPointCommand_В_накладную__которая_добавлена_из_ЕГАИС__нельзя_добавлять_новые_позиции_);
        }
        else
        {
          List<WaybillItem> list1 = ((IEnumerable) obj).Cast<WaybillItem>().ToList<WaybillItem>();
          if (!list1.Any<WaybillItem>())
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выделить_хотя_бы_одну_позицию_);
          }
          else
          {
            using (Gbs.Core.Db.DataBase db = Gbs.Core.Data.GetDataBase())
            {
              if (list1.Count == 1)
              {
                Gbs.Core.Entities.Goods.Good good1 = new GoodRepository(db).SaveCopyGood(list1.Single<WaybillItem>().Good, false).Good;
                Gbs.Core.Entities.Goods.Good good2;
                if (!new FrmGoodCard().ShowGoodCard(Guid.Empty, out good2, authUser: this.AuthUser, goodNew: good1, isSave: false))
                  return;
                this.AddListItems((IEnumerable<Gbs.Core.Entities.Goods.Good>) new List<Gbs.Core.Entities.Goods.Good>()
                {
                  good2
                });
                this.ListGoodSave.Add(good2);
              }
              else
              {
                if (MessageBoxHelper.Question(string.Format(Translate.WaybillCardViewModel_Вы_уверены__что_хотите_сделать_копии__0__позиций_, (object) ((ICollection) obj).Count)) != MessageBoxResult.Yes)
                  return;
                List<Gbs.Core.Entities.Goods.Good> list2 = list1.Select<WaybillItem, Gbs.Core.Entities.Goods.Good>((Func<WaybillItem, Gbs.Core.Entities.Goods.Good>) (x => new GoodRepository(db).SaveCopyGood(x.Good, false).Good)).ToList<Gbs.Core.Entities.Goods.Good>();
                this.AddListItems((IEnumerable<Gbs.Core.Entities.Goods.Good>) list2);
                this.ListGoodSave.AddRange((IEnumerable<Gbs.Core.Entities.Goods.Good>) list2);
              }
            }
          }
        }
      }));
      this.AddItemFromExcelCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.EgaisDocument != null)
        {
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_AddItemFromPointCommand_В_накладную__которая_добавлена_из_ЕГАИС__нельзя_добавлять_новые_позиции_);
        }
        else
        {
          (bool Result2, List<GoodItem> goodItemList3, List<GoodItem> goodItemList4) = new FrmWaybillOutExcel().ImportOutExcel(this.AuthUser);
          if (!Result2)
            return;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.WaybillCardViewModel_WaybillCardViewModel_Загрузка_данных_из_Excel_в_накладную);
          this.ListGoodSave.AddRange(goodItemList4.Where<GoodItem>((Func<GoodItem, bool>) (x => !x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Service, GlobalDictionaries.GoodTypes.Certificate))).Select<GoodItem, Gbs.Core.Entities.Goods.Good>((Func<GoodItem, Gbs.Core.Entities.Goods.Good>) (x => x.Good)));
          foreach (GoodItem goodItem in goodItemList3.Where<GoodItem>((Func<GoodItem, bool>) (x => !x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))).AsParallel<GoodItem>())
          {
            GoodItem x = goodItem;
            this.Waybill.AddGood(new WaybillItem(x.Good, x.Quantity, x.BuyPrice, salePrice: x.SalePrice)
            {
              IsNewItem = goodItemList4.Any<GoodItem>((Func<GoodItem, bool>) (g => g.Good.Uid == x.Good.Uid)) ? WaybillCardViewModel.IconNew : (object) null
            });
          }
          progressBar.Close();
          if (!goodItemList3.Any<GoodItem>((Func<GoodItem, bool>) (x => x.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))))
            return;
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_WaybillCardViewModel_Сертификаты_и_услуги_нельзя_добавить_в_накладную__все_товары_с_указанными_типами_были_удалены_из_накладной_);
        }
      }));
      this.SaveWaybillCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.SelectContractorCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.GetContractor()));
    }

    private void LoadWaybill(Document doc)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.Waybill = new Gbs.Core.ViewModels.Waybill.Waybill();
      this.Document = doc;
      this.Waybill.Payments = new ObservableCollection<Gbs.Core.Entities.Payments.Payment>(doc.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)));
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        this.ListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false))).ToList<Storages.Storage>();
      Other.ConsoleWrite("storages: " + ((double) stopwatch.ElapsedMilliseconds / 1000.0).ToString());
      if (this.ListStorage.Count<Storages.Storage>() == 1 && doc.Storage == null)
        doc.Storage = this.ListStorage.First<Storages.Storage>();
      if (doc.Status == GlobalDictionaries.DocumentsStatuses.None)
        this.Document.Status = GlobalDictionaries.DocumentsStatuses.Open;
      List<WaybillItem> list = new List<WaybillItem>();
      foreach (Gbs.Core.Entities.Documents.Item obj in doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)))
      {
        Gbs.Core.Entities.Documents.Item good = obj;
        WaybillItem waybillItem1 = new WaybillItem(good.Good, good.Quantity, good.BuyPrice, good.Discount, good.SellPrice, new Guid?(good.Uid));
        waybillItem1.Stock = good.GoodStock;
        waybillItem1.GoodModification = good.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == good.ModificationUid));
        WaybillItem waybillItem2 = waybillItem1;
        waybillItem2.Identity = good.Comment;
        list.Add(waybillItem2);
      }
      this.Waybill.Items = new ObservableCollection<WaybillItem>(list);
      double num = (double) stopwatch.ElapsedMilliseconds / 1000.0;
      Other.ConsoleWrite("foreach: " + num.ToString());
      this.UpdateDocument();
      num = (double) stopwatch.ElapsedMilliseconds / 1000.0;
      Other.ConsoleWrite("update doc: " + num.ToString());
      this.EntityClone = (IEntity) this.Document.Clone<Document>();
      num = (double) stopwatch.ElapsedMilliseconds / 1000.0;
      Other.ConsoleWrite("total: " + num.ToString());
      this.OnPropertyChanged("Waybill");
    }

    private void UpdateDocument()
    {
      List<Gbs.Core.Entities.Documents.Item> objList = new List<Gbs.Core.Entities.Documents.Item>();
      foreach (WaybillItem waybillItem in (Collection<WaybillItem>) this.Waybill.Items)
      {
        if (waybillItem.Stock != null)
          waybillItem.Stock.Storage = this.Document.Storage;
        objList.Add(new Gbs.Core.Entities.Documents.Item(waybillItem, this.Document.Uid)
        {
          Comment = waybillItem.Identity
        });
      }
      foreach (Gbs.Core.Entities.Documents.Item obj in this.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => this.Waybill.Items.All<WaybillItem>((Func<WaybillItem, bool>) (g => g.Uid != x.Uid)))))
      {
        obj.IsDeleted = true;
        objList.Add(obj);
      }
      this.Document.Items = objList;
      List<Gbs.Core.Entities.Payments.Payment> paymentList = new List<Gbs.Core.Entities.Payments.Payment>();
      foreach (Gbs.Core.Entities.Payments.Payment payment in (Collection<Gbs.Core.Entities.Payments.Payment>) this.Waybill.Payments)
        paymentList.Add(payment);
      foreach (Gbs.Core.Entities.Payments.Payment payment in this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => this.Waybill.Payments.All<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (g => g.Uid != x.Uid)))))
      {
        payment.IsDeleted = true;
        paymentList.Add(payment);
      }
      this.Document.Payments = paymentList;
      if (!this.ListGoodSave.Any<Gbs.Core.Entities.Goods.Good>())
        return;
      this.ListGoodSave = new List<Gbs.Core.Entities.Goods.Good>(this.ListGoodSave.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => this.Document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == g.Uid)))));
    }

    private void ReCalcSalePrice()
    {
      List<PricingRules> list1 = new PricingRulesRepository().GetActiveItems().ToList<PricingRules>();
      if (!list1.Any<PricingRules>())
      {
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.WaybillCardViewModel_Пересчет_розничных_цен,
          Text = Translate.WaybillCardViewModel_В_настройках_не_указано_ни_одного_правила_расчета_розничной_цены__
        });
      }
      else
      {
        int num = 0;
        foreach (WaybillItem selectedItem in this.SelectedItems)
        {
          WaybillItem item = selectedItem;
          PricingRules pricingRules = list1.FirstOrDefault<PricingRules>((Func<PricingRules, bool>) (x => x.Groups.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (g => g.Uid == item.Good.Group.Uid))));
          List<PricingRules.ItemPricing> list2 = pricingRules != null ? pricingRules.Items.Where<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (i => i.MinSum <= item.BuyPrice)).ToList<PricingRules.ItemPricing>() : (List<PricingRules.ItemPricing>) null;
          if (list2 != null && list2.Any<PricingRules.ItemPricing>())
          {
            Decimal max = list2.Max<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, Decimal>) (m => m.MinSum));
            PricingRules.ItemPricing itemPricing = pricingRules != null ? pricingRules.Items.FirstOrDefault<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (x => x.MinSum == max)) : (PricingRules.ItemPricing) null;
            if (itemPricing != null)
            {
              item.SalePrice = HelpClassExcel.RoundSum(item.BuyPrice * (1M + itemPricing.Margin / 100M), itemPricing.RoundingValue);
              ++num;
            }
          }
        }
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.WaybillCardViewModel_Пересчет_розничных_цен,
          Text = string.Format(Translate.WaybillCardViewModel_Цена_рассчитана_в_соответствии_с_правилами_для__0___1__товаров, (object) num, (object) this.SelectedItems.Count)
        });
        this.Waybill.ReCalcTotals();
      }
    }

    private void AddListItems(IEnumerable<Gbs.Core.Entities.Goods.Good> listGood, bool addAllCount = false, bool checkMinus = true)
    {
      bool previousBuyPrice = new ConfigsRepository<Settings>().Get().Waybill.IsOfferPreviousBuyPrice;
      bool previousSalePrice = new ConfigsRepository<Settings>().Get().Waybill.IsOfferPreviousSalePrice;
      if (previousBuyPrice && this._buyPriceCounter == null)
        this._buyPriceCounter = new BuyPriceCounter();
      List<GoodsModifications.GoodModification> modifications = new List<GoodsModifications.GoodModification>();
      foreach (Gbs.Core.Entities.Goods.Good good in listGood)
      {
        WaybillItem source = new WaybillItem(good);
        if (good.Modifications.Count<GoodsModifications.GoodModification>() != 0 && !new FrmSelectGoodStock().SelectedModification(good, out modifications, true))
          break;
        if (previousBuyPrice)
          source.BuyPrice = this._buyPriceCounter.GetLastBuyPrice(good);
        if (previousSalePrice && good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
          source.SalePrice = SaleHelper.GetSalePriceForGood(good, new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType, this.Document.Storage).GetValueOrDefault();
        if (modifications.Any<GoodsModifications.GoodModification>())
        {
          foreach (GoodsModifications.GoodModification goodModification in modifications)
          {
            WaybillItem waybillItem = source.Clone<WaybillItem>();
            waybillItem.Uid = Guid.NewGuid();
            waybillItem.GoodModification = goodModification;
            if (this.Waybill.EditQuantity((object) new List<WaybillItem>()
            {
              waybillItem
            }, false))
              this.Waybill.AddGood(waybillItem);
          }
        }
        else if (this.Waybill.EditQuantity((object) new List<WaybillItem>()
        {
          source
        }, false))
          this.Waybill.AddGood(source);
        this.OnPropertyChanged("TotalPayment");
        this.OnPropertyChanged("TotalCredit");
      }
    }

    private void Save()
    {
      DataConnectionTransaction connectionTransaction = (DataConnectionTransaction) null;
      try
      {
        this.UpdateDocument();
        if (this.DocFileMoveUid != Guid.Empty && !MoveHelper.IsExistsDocumentCloud(this.DocFileMoveUid))
        {
          if (MessageBoxHelper.Question(Translate.WaybillCardViewModel_Save_) != MessageBoxResult.Yes)
            return;
          this.Waybill.Items = new ObservableCollection<WaybillItem>();
          this.OnPropertyChanged("Items");
          this.DocFileMoveUid = Guid.Empty;
          this.DocMoveUid = Guid.Empty;
          this.VisibilityAddFromPoint = this.IsReturnBuy || !MoveHelper.IsNewDocument() ? Visibility.Collapsed : Visibility.Visible;
          this.OnPropertyChanged("VisibilityAddFromPoint");
        }
        else if (this.Document.Storage == null)
        {
          MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Необходимо_выбрать_склад);
        }
        else
        {
          Gbs.Core.Entities.Users.User authUser = this.AuthUser;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.WaybillCardViewModel_Сохранение_накладной);
          this.Document.Type = this.IsReturnBuy ? GlobalDictionaries.DocumentsTypes.BuyReturn : GlobalDictionaries.DocumentsTypes.Buy;
          Document document1 = this.Document;
          if (document1.Section == null)
            document1.Section = Sections.GetCurrentSection();
          this.Document.Number = this.Document.Number.IsNullOrEmpty() ? Other.GetNumberDocument(this.IsReturnBuy ? GlobalDictionaries.DocumentsTypes.BuyReturn : GlobalDictionaries.DocumentsTypes.Buy) : this.Document.Number;
          this.Document.UserUid = this.Document.UserUid == Guid.Empty ? authUser.Uid : this.Document.UserUid;
          Document document2 = this.Document;
          DateTime date1 = this.Document.DateTime.Date;
          DateTime dateTime1 = ((Document) this.EntityClone).DateTime;
          DateTime date2 = dateTime1.Date;
          DateTime dateTime2;
          if (!(date1 == date2))
          {
            dateTime1 = this.Document.DateTime;
            dateTime2 = dateTime1.Date;
          }
          else
            dateTime2 = this.Document.DateTime;
          document2.DateTime = dateTime2;
          this.Document.ParentUid = this.DocMoveUid == Guid.Empty ? this.Document.ParentUid : this.DocMoveUid;
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            connectionTransaction = dataBase.BeginTransaction();
            GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
            List<Gbs.Core.Entities.GoodGroups.Group> groups = groupsRepository.GetActiveItems();
            HomeOfficeHelper homeOfficeHelper = new HomeOfficeHelper();
            homeOfficeHelper.Prepare();
            if (this.ListGoodSave.Any<Gbs.Core.Entities.Goods.Good>() && new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
            {
              homeOfficeHelper.CreateEditFile<List<Gbs.Core.Entities.Goods.Good>>(this.ListGoodSave, HomeOfficeHelper.EntityEditHome.GoodList);
              homeOfficeHelper.CreateEditFile<List<Gbs.Core.Entities.GoodGroups.Group>>(this.ListGoodSave.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Group != null && groups.All<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (g => g.Uid != x.Group.Uid)))).Select<Gbs.Core.Entities.Goods.Good, Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.Goods.Good, Gbs.Core.Entities.GoodGroups.Group>) (x => x.Group)).GroupBy<Gbs.Core.Entities.GoodGroups.Group, Guid>((Func<Gbs.Core.Entities.GoodGroups.Group, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.GoodGroups.Group>, Gbs.Core.Entities.GoodGroups.Group>((Func<IGrouping<Guid, Gbs.Core.Entities.GoodGroups.Group>, Gbs.Core.Entities.GoodGroups.Group>) (x => x.First<Gbs.Core.Entities.GoodGroups.Group>())).ToList<Gbs.Core.Entities.GoodGroups.Group>(), HomeOfficeHelper.EntityEditHome.GoodGroupList);
            }
            if (!this.SendEgaisTtn(true))
            {
              connectionTransaction?.Rollback();
              progressBar.Close();
            }
            else
            {
              GoodRepository goodRepository = new GoodRepository(dataBase);
              if (new ConfigsRepository<Settings>().Get().Waybill.RePriceRule == WaybillConfig.RePriceVariants.RequestForEachWaybill)
                DocumentsRepository.RePriceVariants = MessageBoxHelper.Show(Translate.WaybillCardViewModel_Переоценивать_имеющиеся_остатки_товаров_по_ценам_из_накладной_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.Yes ? WaybillConfig.RePriceVariants.RePriceExitsStocks : WaybillConfig.RePriceVariants.CreateStocksWithNewPrice;
              bool flag = true;
              foreach (IGrouping<Guid, Gbs.Core.Entities.Goods.Good> source in this.ListGoodSave.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => this.Document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == x.Uid)))).GroupBy<Gbs.Core.Entities.Goods.Good, Guid>((Func<Gbs.Core.Entities.Goods.Good, Guid>) (x => x.Uid)))
              {
                flag &= goodRepository.Save(source.First<Gbs.Core.Entities.Goods.Good>(), false);
                if (DevelopersHelper.IsDebug() && WaybillCardViewModel.ErrorCountDeveloper >= 3)
                {
                  WaybillCardViewModel.ErrorCountDeveloper = 0;
                  throw new Exception("Тестовая ошибка для девелоп режима");
                }
                int num = flag ? 1 : 0;
              }
              // ISSUE: explicit non-virtual call
              foreach (Gbs.Core.Entities.GoodGroups.Group group1 in this.ListGoodSave.Select<Gbs.Core.Entities.Goods.Good, Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.Goods.Good, Gbs.Core.Entities.GoodGroups.Group>) (x => x.Group)).GroupBy<Gbs.Core.Entities.GoodGroups.Group, Guid>((Func<Gbs.Core.Entities.GoodGroups.Group, Guid>) (x => x == null ? Guid.Empty : __nonvirtual (x.Uid))).Select<IGrouping<Guid, Gbs.Core.Entities.GoodGroups.Group>, Gbs.Core.Entities.GoodGroups.Group>((Func<IGrouping<Guid, Gbs.Core.Entities.GoodGroups.Group>, Gbs.Core.Entities.GoodGroups.Group>) (x => x.First<Gbs.Core.Entities.GoodGroups.Group>())))
              {
                Gbs.Core.Entities.GoodGroups.Group group = group1;
                if (!groups.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid)))
                {
                  flag = groupsRepository.Save(group, false);
                  int num = flag ? 1 : 0;
                }
              }
              List<EntityProperties.PropertyType> list = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)).ToList<EntityProperties.PropertyType>();
              foreach (EntityProperties.PropertyType propertyType in this.ListEntityTypeSave)
              {
                EntityProperties.PropertyType prop = propertyType;
                if (!list.Any<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == prop.Uid)))
                {
                  flag = prop.Save();
                  int num = flag ? 1 : 0;
                }
              }
              this.SaveResult = new DocumentsRepository(dataBase).Save(this.Document, true, homeOfficeHelper);
              this.SaveResult &= flag;
              this.SaveResult &= this.SavePayment();
              if (this.SaveResult)
              {
                ++WaybillCardViewModel.ErrorCountDeveloper;
                connectionTransaction?.Commit();
                this.SaveRefIdForGoodStock(homeOfficeHelper);
                homeOfficeHelper.Send();
                if (this.DocFileMoveUid != Guid.Empty)
                {
                  MoveHelper.DeleteDocumentCloud(this.DocFileMoveUid);
                  if (this.DocMoveUid != Guid.Empty)
                    WaybillCardViewModel._listUidSendWaybill.RemoveAll((Predicate<Guid>) (x => x == this.DocMoveUid));
                }
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Document, this.IsEdit ? ActionType.Edit : ActionType.Add, GlobalDictionaries.EntityTypes.Document, this.AuthUser), true);
                ActionsHistoryHelper.AddActionListGoodThread(this.ListGoodSave, this.AuthUser);
                BuyPriceCounter buyPriceCounter = new BuyPriceCounter(true);
                Task.Run((Action) (() =>
                {
                  Settings settings = new ConfigsRepository<Settings>().Get();
                  if (!settings.RemoteControl.Cloud.IsAutoSend || !settings.RemoteControl.Cloud.IsActive)
                    return;
                  HomeOfficeHelper.CreateArchive();
                }));
                Task.Run((Action) (() =>
                {
                  try
                  {
                    PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
                    if (!planfix.IsActive)
                      return;
                    using (Gbs.Core.Db.DataBase dbForPf = Gbs.Core.Data.GetDataBase())
                      PlanfixHelper.UpdateGoodPf(this.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Kit, GlobalDictionaries.GoodsSetStatuses.Production, GlobalDictionaries.GoodsSetStatuses.Set))).Select<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Documents.Item, Gbs.Core.Entities.Goods.Good>) (x => new GoodRepository(dbForPf).GetByUid(x.GoodUid))).ToList<Gbs.Core.Entities.Goods.Good>(), planfix);
                  }
                  catch (Exception ex)
                  {
                    LogHelper.Error(ex, "Ошибка выгрузки данных в ПФ из карточки накладной", false);
                  }
                }));
                WindowWithSize.IsCancel = false;
                this.CloseAction();
                Action updateEgaisAction = this.UpdateEgaisAction;
                if (updateEgaisAction != null)
                  updateEgaisAction();
                this.UpdateGridDocuments();
              }
              else
                connectionTransaction?.Rollback();
              progressBar.Close();
            }
          }
        }
      }
      catch (Exception ex)
      {
        connectionTransaction?.Rollback();
        Other.ConsoleWrite((object) ex);
        throw;
      }
    }

    private bool SavePayment()
    {
      bool flag = true;
      Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      foreach (Gbs.Core.Entities.Payments.Payment payment in (Collection<Gbs.Core.Entities.Payments.Payment>) this.Waybill.Payments)
      {
        if (payment.Save() && devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && payment.IsFiscal)
        {
          KkmHelper kkmHelper = new KkmHelper(devicesConfig);
          Cashier cashier = new Cashier()
          {
            Name = this.AuthUser?.Client.Name ?? "",
            Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
            {
              Guid entityUid = x.EntityUid;
              Guid? uid = this.AuthUser?.Client.Uid;
              return (uid.HasValue ? (entityUid == uid.GetValueOrDefault() ? 1 : 0) : 0) != 0 && x.Type.Uid == GlobalDictionaries.InnUid;
            }))?.Value.ToString() ?? ""
          };
          if (payment.AccountOut.Type == PaymentsAccounts.MoneyType.KkmCash)
            flag &= kkmHelper.CashOut(payment.SumOut, cashier);
          kkmHelper.Dispose();
        }
      }
      return flag;
    }

    private void Close()
    {
      if (this.EgaisDocument != null)
      {
        if (MessageBoxHelper.Show(Translate.WaybillCardViewModel_Close_Отправить_акт_об_отказе_от_накладной_в_ЕГАИС_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
          if (!this.SendEgaisTtn(false))
            return;
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            this.Document.Payments.Clear();
            this.Document.Items.Clear();
            this.Document.Type = GlobalDictionaries.DocumentsTypes.Buy;
            this.Document.IsDeleted = true;
            new DocumentsRepository(dataBase).Save(this.Document);
            this.EgaisDocument = (EgaisDocument) null;
          }
        }
        else
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.WaybillCardViewModel_Close_Статус_накладной__0__не_изменился_, (object) this.EgaisDocument.Form2.WBRegId)));
      }
      Action updateEgaisAction = this.UpdateEgaisAction;
      if (updateEgaisAction != null)
        updateEgaisAction();
      this.CloseAction();
    }

    private void UpdateGridDocuments()
    {
      if (this.ListDoc == null)
        return;
      if (this.ListDoc.Any<WaybillsViewModel.WaybillItemsInfoGrid>((Func<WaybillsViewModel.WaybillItemsInfoGrid, bool>) (x => x.Document.Uid == this.Document.Uid)))
        UpdateInfoGrid(this.Document);
      else
        this.ListDoc.Add(new WaybillsViewModel.WaybillItemsInfoGrid(this.Document));
      this.UpdateSortGrid();

      void UpdateInfoGrid(Document doc)
      {
        int index = this.ListDoc.ToList<WaybillsViewModel.WaybillItemsInfoGrid>().FindIndex((Predicate<WaybillsViewModel.WaybillItemsInfoGrid>) (x => x.Document.Uid == doc.Uid));
        this.ListDoc[index].Document = doc;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          this.ListDoc[index].NameContractor = doc.ContractorUid == Guid.Empty ? "" : new ClientsRepository(dataBase).GetByUid(doc.ContractorUid)?.Name;
          this.ListDoc[index].NameUser = doc.UserUid == Guid.Empty ? "" : new UsersRepository(dataBase).GetByUid(doc.UserUid)?.Alias;
          this.ListDoc[index].SumCountItems = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
          this.ListDoc[index].SumItems = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.BuyPrice * x.Quantity));
        }
      }
    }

    private void GetContractor()
    {
      (Client client, bool result) client1 = new FrmSearchClient().GetClient(true);
      if (!client1.result)
        return;
      Document document = this.Document;
      Client client2 = client1.client;
      // ISSUE: explicit non-virtual call
      Guid guid = client2 != null ? __nonvirtual (client2.Uid) : Guid.Empty;
      document.ContractorUid = guid;
      this.OnPropertyChanged(nameof (GetContractor));
      this.OnPropertyChanged("ContractorName");
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      this.UpdateDocument();
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Document);
    }

    public Visibility VisibilityBuyPrice { get; set; }

    public ICommand AddPayment
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
            Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment()
            {
              ParentUid = this.Document.Uid
            };
            new FrmRemoveCash().RemoveCash(ref payment, isSavePayment: false, user: this.AuthUser, isVisibilityGroup: false, isVisibilityClient: false);
            if (payment == null || !(payment.SumIn != 0M) && !(payment.SumOut != 0M))
              return;
            this.Waybill.Payments.Add(payment);
            this.Waybill.ReCalcTotals();
          }
        }));
      }
    }

    public ICommand DeletePayment
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
            List<Gbs.Core.Entities.Payments.Payment> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Payments.Payment>().ToList<Gbs.Core.Entities.Payments.Payment>();
            if (!list.Any<Gbs.Core.Entities.Payments.Payment>())
            {
              MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выбрать_платежи_для_удаления);
            }
            else
            {
              if (MessageBoxHelper.Question(Translate.WaybillCardViewModelВыУвереныЧтоХотитеУдалитьВсеВыделенныеПлатежиWaybillCardViewModelВыУвереныЧтоХотитеУдалитьВсеВыделенныеПлатежи) == MessageBoxResult.No)
                return;
              foreach (Gbs.Core.Entities.Payments.Payment payment in list)
                this.Waybill.Payments.Remove(payment);
              this.Waybill.ReCalcTotals();
            }
          }
        }));
      }
    }

    private bool IsReturnBuy { get; set; }

    public EgaisDocument EgaisDocument { get; set; }

    public Visibility IsDebugMode
    {
      get => !DevelopersHelper.IsDebug() ? Visibility.Collapsed : Visibility.Visible;
    }

    private bool SendEgaisTtn(bool isAccept)
    {
      if (!new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        return true;
      try
      {
        if (this.EgaisDocument == null)
          return true;
        LogHelper.Debug(this.EgaisDocument.Waybill.Items.ToJsonString(true));
        LogHelper.Debug(this.Waybill.Items.ToJsonString(true));
        LogHelper.Debug(this.Document.Items.ToJsonString(true));
        bool flag = this.EgaisDocument.Waybill.Items.Any<PositionType>((Func<PositionType, bool>) (x =>
        {
          Decimal quantity = x.Quantity;
          WaybillItem waybillItem = this.Waybill.Items.SingleOrDefault<WaybillItem>((Func<WaybillItem, bool>) (i => i.Good.Uid == x.UidGoodInDb && i.Identity == x.Identity));
          // ISSUE: explicit non-virtual call
          Decimal num = waybillItem != null ? __nonvirtual (waybillItem.Quantity) : x.Quantity;
          return quantity != num;
        }));
        string comment = "";
        if (isAccept)
        {
          if (MessageBoxHelper.Question("Данные в накладную были загружены из ЕГАИС.\n\nОтправить акт подтверждения накладной в ЕГАИС и сохранить накладную в базу?") == MessageBoxResult.No)
            return false;
          if (flag)
          {
            (bool result, string output) = MessageBoxHelper.Input(Translate.WaybillCardViewModel_SendEgaisTTN_Причина_расхождений, Translate.WaybillCardViewModel_SendEgaisTTN_Вы_изменили_количество_для_некоторых_позиций__Введите_причину_отправки_акта_расхождений_, 3);
            if (!result)
              return false;
            comment = output;
          }
        }
        else
        {
          (bool result, string output) = MessageBoxHelper.Input(Translate.WaybillCardViewModel_SendEgaisTTN_Причина_отказа, Translate.WaybillCardViewModel_SendEgaisTTN_Вы_отказались_от_накладной__Введите_причину_отказа_от_накладной_, 3);
          if (!result)
            return false;
          comment = output;
        }
        if (this.EgaisDocument.Waybill.Version != 4)
        {
          int num = (int) MessageBoxHelper.Show(Translate.WaybillCardViewModel_SendEgaisTTN_Возможно_принять_только_накладную_отправленную_из_4_версии_ЕГАИС);
          return false;
        }
        AcceptType3 acceptType = !isAccept ? AcceptType3.Rejected : (flag ? AcceptType3.Differences : AcceptType3.Accepted);
        Document document = this.Document;
        new EgaisRepository().DoWaybillItem(this.EgaisDocument, ref document, acceptType, comment);
        this.Document = document;
        return true;
      }
      catch (Exception ex)
      {
        MessageBoxHelper.Error(string.Format(Translate.WaybillCardViewModel_SendEgaisTtn_НеУдалосьОтправитьДокументОПолученииНакладнойвУТМ, (object) ex.Message));
        LogHelper.WriteToEgaisLog("Не удалось отправить документ о полученнии накладной в УТМ", ex);
        return false;
      }
    }

    private void SaveRefIdForGoodStock(HomeOfficeHelper homeOffice)
    {
      if (!new ConfigsRepository<Integrations>().Get().Egais.IsActive || this.EgaisDocument == null && this.Waybill.Items.All<WaybillItem>((Func<WaybillItem, bool>) (x => x.FbNumberForEgais.IsNullOrEmpty())))
        return;
      if (this.EgaisDocument != null)
      {
        LogHelper.Debug("Waybill = " + this.EgaisDocument.Waybill.DictionaryInformBRegId.ToJsonString(true));
        LogHelper.Debug("Form2 = " + this.EgaisDocument.Form2.DictionaryInformBRegId.ToJsonString(true));
      }
      List<GoodsStocks.GoodStock> goodStockList = new List<GoodsStocks.GoodStock>();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        foreach (Gbs.Core.Entities.Documents.Item obj in this.Document.Items)
        {
          Gbs.Core.Entities.Documents.Item documentItem = obj;
          if ((this.Waybill.Items.SingleOrDefault<WaybillItem>((Func<WaybillItem, bool>) (x => x.Uid == documentItem.Uid))?.FbNumberForEgais ?? "").IsNullOrEmpty() && this.EgaisDocument != null)
          {
            KeyValuePair<string, string> keyValuePair = this.EgaisDocument.Waybill.DictionaryInformBRegId.SingleOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Key == documentItem.Comment));
            string str = keyValuePair.Value ?? "";
            if (str.IsNullOrEmpty())
            {
              keyValuePair = this.EgaisDocument.Form2.DictionaryInformBRegId.SingleOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Key == documentItem.Comment));
              str = keyValuePair.Value ?? "";
              if (str.IsNullOrEmpty())
              {
                bool result;
                do
                {
                  (result, str) = MessageBoxHelper.Input("FB-", "Для товара " + documentItem.Good.Name + " не удалось получить номер справки из накладной ЕГАИС. Необходимо указать номер справки вручную.", 16);
                }
                while (!result);
              }
            }
            documentItem.GoodStock = GoodsStocks.GetStocksByUid(documentItem.GoodStock.Uid);
            if (!SharedRepository.GetFbNumberForGoodStock(documentItem.GoodStock).IsNullOrEmpty())
            {
              documentItem.GoodStock.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RegIdForGoodStockUidEgais)).Value = (object) str;
            }
            else
            {
              GoodsStocks.GoodStock goodStock = documentItem.GoodStock;
              if (goodStock.Properties == null)
              {
                List<EntityProperties.PropertyValue> propertyValueList;
                goodStock.Properties = propertyValueList = new List<EntityProperties.PropertyValue>();
              }
              List<EntityProperties.PropertyValue> properties = documentItem.GoodStock.Properties;
              EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
              propertyValue.EntityUid = documentItem.GoodStock.Uid;
              EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
              propertyType.Uid = GlobalDictionaries.RegIdForGoodStockUidEgais;
              propertyValue.Type = propertyType;
              propertyValue.Value = (object) str;
              properties.Add(propertyValue);
            }
            documentItem.GoodStock.Save(dataBase);
            goodStockList.Add(documentItem.GoodStock);
          }
        }
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home || !goodStockList.Any<GoodsStocks.GoodStock>())
          return;
        homeOffice.CreateEditFile<List<GoodsStocks.GoodStock>>(goodStockList, HomeOfficeHelper.EntityEditHome.GoodStockList);
      }
    }
  }
}
