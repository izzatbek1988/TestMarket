// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.MovingStorage.SendStorageCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.MovingStorage
{
  public partial class SendStorageCardViewModel : ViewModelWithForm
  {
    private Gbs.Core.Entities.Users.User _authUser;
    private Document _childDocument;
    private bool _isResult;

    public List<Gbs.Core.Entities.Storages.Storage> Storages { get; set; } = new List<Gbs.Core.Entities.Storages.Storage>();

    public Gbs.Core.Entities.Storages.Storage StorageTarget { get; set; }

    public Move SendStorage { get; set; } = new Move();

    public ICommand AddItem
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SendStorage.IsNeedComment = false;
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.MoveStorage, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
          this.SendStorage.IsNeedComment = true;
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.SendStorage.Items.Any<BasketItem>())
            MessageBoxHelper.Warning(Translate.SendWaybillCardViewModel_Необходимо_добавить_товары_для_перемещения);
          else if (this.StorageTarget == null)
            MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_SaveCommand_Необходимо_выбрать_склад_получатель);
          else if (this.StorageTarget.Uid == this.SendStorage.Storage.Uid)
          {
            MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_SaveCommand_Склад_отправителя_не_должен_совпадать_со_складом_назначения);
          }
          else
          {
            this.UpdateDocument((Gbs.Core.ViewModels.Basket.Basket) this.SendStorage);
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              this.SendStorage.Document.Storage = this.SendStorage.Storage;
              this.SendStorage.Document.Number = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.MoveStorage);
              this.SendStorage.Document.Status = GlobalDictionaries.DocumentsStatuses.Close;
              this.SendStorage.Document.Type = GlobalDictionaries.DocumentsTypes.MoveStorage;
              Document document = this.SendStorage.Document;
              Gbs.Core.Entities.Users.User authUser1 = this._authUser;
              Guid guid1 = authUser1 != null ? __nonvirtual (authUser1.Uid) : Guid.Empty;
              document.UserUid = guid1;
              this.SendStorage.User = this._authUser;
              if (!new DocumentsRepository(dataBase).Save(this.SendStorage.Document))
                return;
              BuyPriceCounter counter = new BuyPriceCounter(true);
              this._childDocument = this.SendStorage.Document.Clone<Document>();
              this._childDocument.Storage = this.StorageTarget;
              Document childDocument = this._childDocument;
              Gbs.Core.Entities.Users.User authUser2 = this._authUser;
              Guid guid2 = authUser2 != null ? __nonvirtual (authUser2.Uid) : Guid.Empty;
              childDocument.UserUid = guid2;
              ProfitHelper profitHelper = new ProfitHelper(counter);
              Dictionary<Guid, GoodsStocks.GoodStock> dictionary1 = new Dictionary<Guid, GoodsStocks.GoodStock>();
              foreach (Gbs.Core.Entities.Documents.Item obj1 in this._childDocument.Items)
              {
                Gbs.Core.Entities.Documents.Item item = obj1;
                item.Uid = Guid.NewGuid();
                Dictionary<Guid, GoodsStocks.GoodStock> dictionary2 = dictionary1;
                Guid uid3 = item.Uid;
                IEnumerable<GoodsStocks.GoodStock> source = this.SendStorage.Document.Items.SelectMany<Gbs.Core.Entities.Documents.Item, GoodsStocks.GoodStock>((Func<Gbs.Core.Entities.Documents.Item, IEnumerable<GoodsStocks.GoodStock>>) (x =>
                {
                  Gbs.Core.Entities.Goods.Good good = x.Good;
                  return good == null ? (IEnumerable<GoodsStocks.GoodStock>) null : (IEnumerable<GoodsStocks.GoodStock>) good.StocksAndPrices;
                }));
                GoodsStocks.GoodStock goodStock = source != null ? source.SingleOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st =>
                {
                  Guid? uid4 = st?.Uid;
                  Guid? uid5 = item.GoodStock?.Uid;
                  if (uid4.HasValue != uid5.HasValue)
                    return false;
                  return !uid4.HasValue || uid4.GetValueOrDefault() == uid5.GetValueOrDefault();
                })) : (GoodsStocks.GoodStock) null;
                dictionary2[uid3] = goodStock;
                item.GoodStock.Storage = this.StorageTarget;
                item.BuyPrice = profitHelper.GetBuyPriceForItem(this._childDocument, item);
                item.GoodStock = (GoodsStocks.GoodStock) null;
              }
              this._childDocument.ParentUid = this.SendStorage.Document.Uid;
              this._childDocument.Uid = Guid.NewGuid();
              this._childDocument.Type = GlobalDictionaries.DocumentsTypes.MoveStorageChild;
              if (!new DocumentsRepository(dataBase).Save(this._childDocument))
                return;
              this._childDocument = new DocumentsRepository(dataBase).GetByUid(this._childDocument.Uid);
              foreach (Gbs.Core.Entities.Documents.Item obj2 in this._childDocument.Items)
              {
                Gbs.Core.Entities.Documents.Item item = obj2;
                GoodsStocks.GoodStock goodStock;
                dictionary1.TryGetValue(item.Uid, out goodStock);
                if (goodStock != null)
                {
                  item.GoodStock.Properties = goodStock.Properties;
                  item.GoodStock.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.Uid = Guid.NewGuid()));
                  item.GoodStock.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.EntityUid = item.GoodStock.Uid));
                  item.GoodStock.Save(dataBase);
                }
              }
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
              {
                Title = Translate.GlobalDictionaries_Перемещение,
                Text = string.Format(Translate.SendStorageCardViewModel_SaveCommand_Товары_успешно_перемещены_со_склада__0__на_склад__1_, (object) this.SendStorage.Storage.Name, (object) this._childDocument.Storage.Name)
              });
              this._isResult = true;
              WindowWithSize.IsCancel = false;
              Action closeAction = this.CloseAction;
              if (closeAction != null)
                closeAction();
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) null, (IEntity) this.SendStorage.Document, ActionType.Add, GlobalDictionaries.EntityTypes.Document, this._authUser), false);
            }
          }
        }));
      }
    }

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool allCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        switch (good.SetStatus)
        {
          case GlobalDictionaries.GoodsSetStatuses.None:
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_AddItemInBasket_Невозможно_переместить_сертификат_);
              continue;
            }
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            {
              MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_AddItemInBasket_Невозможно_переместить_услугу_);
              continue;
            }
            goto case GlobalDictionaries.GoodsSetStatuses.Production;
          case GlobalDictionaries.GoodsSetStatuses.Set:
            MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_AddItemInBasket_Невозможно_переместить_комплект_);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Kit:
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              using (IEnumerator<GoodsSets.Set> enumerator = good.SetContent.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  GoodsSets.Set current = enumerator.Current;
                  this.GetStock(new GoodRepository(dataBase).GetByUid(current.GoodUid), allCount);
                }
                continue;
              }
            }
          case GlobalDictionaries.GoodsSetStatuses.Range:
            this.GetStock(good, allCount);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Production:
            this.GetStock(good, allCount);
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private void GetStock(Gbs.Core.Entities.Goods.Good good, bool allCount)
    {
      List<GoodsStocks.GoodStock> stocks;
      if (!new FrmSelectGoodStock().SelectedStock(good, out stocks, false))
        return;
      foreach (GoodsStocks.GoodStock goodStock in stocks)
      {
        Decimal q = !allCount ? (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M) : (goodStock.Stock > 0M ? goodStock.Stock : (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M));
        Guid? uid1 = goodStock.Storage?.Uid;
        Guid? uid2 = this.StorageTarget?.Uid;
        if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() != uid2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
        {
          ActionResult actionResult = this.SendStorage.Add(new BasketItem(good, goodStock.ModificationUid, goodStock.Price, goodStock.Storage, q));
          if (actionResult.Result == ActionResult.Results.Error)
          {
            int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Exclamation);
          }
        }
        else
          MessageBoxHelper.Warning(string.Format(Translate.SendStorageCardViewModel_GetStock_Невозможно_добавить_остаток__0___так_как_склад_списания_совпадает_со_складом_назначения_, (object) good.Name));
      }
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    private IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      this.UpdateDocument((Gbs.Core.ViewModels.Basket.Basket) this.SendStorage);
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.SendStorage.Document);
    }

    private void UpdateDocument(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      List<Gbs.Core.Entities.Documents.Item> objList = new List<Gbs.Core.Entities.Documents.Item>();
      foreach (BasketItem basketItem in (Collection<BasketItem>) basket.Items)
      {
        if (basketItem.Storage != null)
          basketItem.Storage = basket.Storage;
        Gbs.Core.Entities.Documents.Item obj = new Gbs.Core.Entities.Documents.Item(basketItem, basket.Document.Uid);
        objList.Add(obj);
      }
      foreach (Gbs.Core.Entities.Documents.Item obj in basket.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => basket.Items.All<BasketItem>((Func<BasketItem, bool>) (g => g.Uid != x.Uid)))))
      {
        obj.IsDeleted = true;
        objList.Add(obj);
      }
      basket.Document.Items = objList;
    }

    public bool ShowCard(
      Guid uid,
      out Document document,
      out Document childDoc,
      Gbs.Core.Entities.Users.User authUser = null)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
        document = (Document) null;
        childDoc = (Document) null;
        return false;
      }
      if (!Other.IsActiveAndShowForm<FrmSendStorageCard>(uid.ToString()))
      {
        document = (Document) null;
        childDoc = (Document) null;
        return false;
      }
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, Actions.AddMoveWaybill))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.AddMoveStorage);
          if (!access.Result)
          {
            document = (Document) null;
            childDoc = (Document) null;
            return false;
          }
          authUser = access.User;
        }
        this._authUser = authUser;
        document = uid == Guid.Empty ? new Document() : new DocumentsRepository(dataBase).GetByUid(uid);
        this.SendStorage.Document = document;
        this.SendStorage.DocumentsType = GlobalDictionaries.DocumentsTypes.MoveStorage;
        this.EntityClone = (IEntity) document.Clone<Document>();
        this.SendStorage.Items = new ObservableCollection<BasketItem>(document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, this.SendStorage.Document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
        this.Storages = new List<Gbs.Core.Entities.Storages.Storage>(Gbs.Core.Entities.Storages.GetStorages().Where<Gbs.Core.Entities.Storages.Storage>((Func<Gbs.Core.Entities.Storages.Storage, bool>) (x => !x.IsDeleted)));
        if (this.Storages.Count == 1)
        {
          MessageBoxHelper.Warning(Translate.SendStorageCardViewModel_ShowCard_Нет_доступных_складов_для_перемещения__Создайте_другой_склад_);
          childDoc = (Document) null;
          return false;
        }
        this.FormToSHow = (WindowWithSize) new FrmSendStorageCard(this);
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ShowForm();
        childDoc = this._childDocument;
        return this._isResult;
      }
    }
  }
}
