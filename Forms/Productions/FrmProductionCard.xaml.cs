// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ProductionCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms
{
  public partial class ProductionCardViewModel : ViewModelWithForm
  {
    private Gbs.Core.Entities.Users.User _authUser;
    private List<Document> _listItems = new List<Document>();
    private bool _isResult;
    private static bool _isBeer;

    public string OkButtonText
    {
      get
      {
        return !ProductionCardViewModel._isBeer ? Translate.ПРОИЗВЕСТИ : Translate.ProductionCardViewModel_OkButtonText_ВСКРЫТЬ_ТАРУ;
      }
    }

    public string HeaderQuantity
    {
      get
      {
        return !ProductionCardViewModel._isBeer ? Translate.FrmGoodsQuantity_Количество : Translate.ProductionCardViewModel_HeaderQuantity_Объем;
      }
    }

    public List<Gbs.Core.Entities.Storages.Storage> Storages { get; set; } = new List<Gbs.Core.Entities.Storages.Storage>();

    public Gbs.Core.Entities.Storages.Storage Storage { get; set; }

    public Gbs.Core.ViewModels.Production Production { get; set; } = new Gbs.Core.ViewModels.Production();

    public ICommand AddItem
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Production.IsNeedComment = false;
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(ProductionCardViewModel._isBeer ? GlobalDictionaries.DocumentsTypes.BeerProductionList : GlobalDictionaries.DocumentsTypes.ProductionList, isVisNullStock: true, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
          this.Production.IsNeedComment = true;
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Storage == null)
            MessageBoxHelper.Warning(Translate.ProductionCardViewModel_SaveCommand_Требуется_выбрать_склад_для_производства_товара_);
          else if (!this.Production.Items.Any<BasketItem>())
          {
            MessageBoxHelper.Warning(Translate.ProductionCardViewModel_SaveCommand_Требуется_добавить_хотя_бы_один_рецепт_для_производства_);
          }
          else
          {
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ProductionCardViewModel_SaveCommand_Производство_товара);
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              List<Gbs.Core.Entities.Goods.Good> allItems = new GoodRepository(dataBase).GetAllItems();
              LogHelper.Debug("Начинаем производить товары");
              Guid uidDocList = Guid.NewGuid();
              Document document = ProductionCardViewModel.DoProduction(this.Production.Items.ToList<BasketItem>(), dataBase, uidDocList, allItems, this.Storage, this._authUser, progressBar, this._listItems);
              progressBar.Close();
              if (document == null)
                return;
              this.Production.Document = document;
              this._isResult = true;
              WindowWithSize.IsCancel = false;
              this.CloseAction();
            }
          }
        }));
      }
    }

    public static Document DoProduction(
      List<BasketItem> items,
      Gbs.Core.Db.DataBase db,
      Guid uidDocList,
      List<Gbs.Core.Entities.Goods.Good> goods,
      Gbs.Core.Entities.Storages.Storage storage,
      Gbs.Core.Entities.Users.User authUser,
      ProgressBarHelper.ProgressBar progressBar,
      List<Document> listItems = null)
    {
      List<Document> documentList = new List<Document>();
      Gbs.Core.Entities.GoodGroups.Group group1 = (Gbs.Core.Entities.GoodGroups.Group) null;
      List<(BasketItem, Document, Document)> valueTupleList = new List<(BasketItem, Document, Document)>();
      foreach (BasketItem basketItem1 in items)
      {
        BasketItem item = basketItem1;
        Gbs.Core.Entities.Goods.Good good1 = item.Good.Clone<Gbs.Core.Entities.Goods.Good>();
        BasketItem basketItem2 = item.Clone();
        if (ProductionCardViewModel._isBeer)
        {
          Gbs.Core.Entities.Goods.Good good2 = goods.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.SetContent.Any<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (s => s.GoodUid == item.Good.Uid)) && !x.IsDeleted));
          Guid guid = Guid.NewGuid();
          if (good2 == null)
          {
            while (group1 == null)
            {
              progressBar.Close();
              Gbs.Core.Entities.GoodGroups.Group group2;
              if (!new FormSelectGroup().GetSingleSelectedGroupUid(authUser, out group2))
              {
                ProgressBarHelper.Close();
                return (Document) null;
              }
              if (group2.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))
                MessageBoxHelper.Warning("Для позиций из ЕГАИС можно выбрать категорию только с типом товаров: обычные или весовые.");
              else
                group1 = group2;
              progressBar = new ProgressBarHelper.ProgressBar(Translate.ProductionCardViewModel_SaveCommand_Производство_товара);
            }
            BasketItem basketItem3 = item;
            Gbs.Core.Entities.Goods.Good good3 = new Gbs.Core.Entities.Goods.Good();
            good3.Uid = guid;
            good3.Name = good1.Name + " [1 литр]";
            good3.SetContent = (IEnumerable<GoodsSets.Set>) new GoodsSets.Set[1]
            {
              new GoodsSets.Set()
              {
                GoodUid = good1.Uid,
                Quantity = 0.1M,
                ParentUid = guid
              }
            };
            good3.SetStatus = GlobalDictionaries.GoodsSetStatuses.Production;
            good3.Group = group1;
            Gbs.Core.Entities.Goods.Good good4 = good3;
            basketItem3.Good = good4;
            new GoodRepository(db).Save(item.Good);
            goods.Add(item.Good);
          }
          else
            item.Good = good2;
        }
        else
        {
          item.Good = new GoodRepository(db).GetByUid(item.Good.Uid);
          BasketItem basketItem4 = item;
          if (basketItem4.Good == null)
          {
            Gbs.Core.Entities.Goods.Good good5;
            basketItem4.Good = good5 = good1;
          }
        }
        LogHelper.Debug("Производство товара " + item.DisplayedName);
        // ISSUE: explicit non-virtual call
        Document document1 = new Document()
        {
          ParentUid = uidDocList,
          Type = ProductionCardViewModel._isBeer ? GlobalDictionaries.DocumentsTypes.BeerProductionItem : GlobalDictionaries.DocumentsTypes.ProductionItem,
          Storage = storage,
          UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty,
          Items = new List<Gbs.Core.Entities.Documents.Item>()
          {
            new Gbs.Core.Entities.Documents.Item()
            {
              SellPrice = item.SalePrice,
              Quantity = item.Quantity,
              Good = item.Good
            }
          },
          Status = GlobalDictionaries.DocumentsStatuses.Close
        };
        LogHelper.Debug("Создаем документ для добавления остатков " + document1.ToJsonString(true));
        // ISSUE: explicit non-virtual call
        Document document2 = new Document()
        {
          ParentUid = document1.Uid,
          Type = ProductionCardViewModel._isBeer ? GlobalDictionaries.DocumentsTypes.BeerProductionSet : GlobalDictionaries.DocumentsTypes.ProductionSet,
          Items = item.Good.SetContent.Select<GoodsSets.Set, Gbs.Core.Entities.Documents.Item>((Func<GoodsSets.Set, Gbs.Core.Entities.Documents.Item>) (x => new Gbs.Core.Entities.Documents.Item()
          {
            Good = goods.Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (s => s.Uid == x.GoodUid)),
            Quantity = ProductionCardViewModel._isBeer ? item.Quantity / 10M : x.Quantity * item.Quantity,
            SellPrice = 0M
          })).ToList<Gbs.Core.Entities.Documents.Item>(),
          Storage = storage,
          UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty,
          Status = GlobalDictionaries.DocumentsStatuses.Close
        };
        LogHelper.Debug("Создаем документ для списания остатков " + document2.ToJsonString(true));
        documentList?.Add(document1);
        documentList?.Add(document2);
        listItems?.Add(document1);
        if (ProductionCardViewModel._isBeer)
        {
          (BasketItem, Document, Document) valueTuple = (basketItem2, document2, document1);
          valueTupleList.Add(valueTuple);
        }
      }
      Document document3 = new Document();
      document3.Uid = uidDocList;
      document3.Type = ProductionCardViewModel._isBeer ? GlobalDictionaries.DocumentsTypes.BeerProductionList : GlobalDictionaries.DocumentsTypes.ProductionList;
      // ISSUE: explicit non-virtual call
      document3.UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty;
      document3.Status = GlobalDictionaries.DocumentsStatuses.Close;
      document3.Storage = storage;
      document3.Number = Other.GetNumberDocument(ProductionCardViewModel._isBeer ? GlobalDictionaries.DocumentsTypes.BeerProductionList : GlobalDictionaries.DocumentsTypes.ProductionList);
      Document document4 = document3;
      if (new DocumentsRepository(db).Save(document4))
      {
        LogHelper.Debug(string.Format("Успешно сохранено {0} документов производства из {1}.", (object) new DocumentsRepository(db).Save(documentList), (object) documentList.Count));
        if (ProductionCardViewModel._isBeer)
        {
          List<TrueApiHelper.ConnectTapDocument.CodeBeer> codeBeerList = new List<TrueApiHelper.ConnectTapDocument.CodeBeer>();
          DateTime now = DateTime.Now;
          LogHelper.Debug("НАчинаем отправлять данные в ЕГАИС и ЧЗ: " + valueTupleList.ToJsonString(true));
          foreach ((BasketItem basketItem, Document document5, Document document6) in valueTupleList)
          {
            document5 = documentList.Single<Document>((Func<Document, bool>) (d => d.Uid == document5.Uid));
            ProductionCardViewModel.DoWriteOffBeerKega(basketItem, document5);
            if (!basketItem.Comment.IsNullOrEmpty())
            {
              string str = basketItem.Comment;
              int startIndex = basketItem.Comment.LastIndexOf("93", StringComparison.InvariantCultureIgnoreCase);
              if (startIndex > 0)
                str = str.Remove(startIndex);
              codeBeerList.Add(new TrueApiHelper.ConnectTapDocument.CodeBeer()
              {
                Cis = str,
                ConnectDate = now.Date.ToString("yyyy-MM-dd")
              });
              GoodsStocks.GoodStock stocksByUid = GoodsStocks.GetStocksByUid(document6.Items.Single<Gbs.Core.Entities.Documents.Item>().GoodStock.Uid);
              List<EntityProperties.PropertyValue> properties = stocksByUid.Properties;
              EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
              propertyValue.EntityUid = stocksByUid.Uid;
              EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
              propertyType.Uid = GlobalDictionaries.MarkedInfoGood;
              propertyValue.Type = propertyType;
              propertyValue.Value = (object) basketItem.Comment;
              properties.Add(propertyValue);
              stocksByUid.Save(db);
            }
          }
          LogHelper.Debug("Продолжаем  отправлять данные в  ЧЗ: " + codeBeerList.ToJsonString(true));
          if (codeBeerList.Any<TrueApiHelper.ConnectTapDocument.CodeBeer>())
          {
            while (!DevelopersHelper.IsDebug() && !TrueApiRepository.ConnectTapBeerKega(codeBeerList) && MessageBoxHelper.Show("Не удалось отправить информацию о вскрытии кеги в Честный знак. Необходимо выполнить вскрытие самостоятельно через личный кабинет Честного знака. Попробовать еще раз?", buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) != MessageBoxResult.No)
              ;
          }
        }
        progressBar?.Close();
        return document4;
      }
      progressBar?.Close();
      return (Document) null;
    }

    public static void DoWriteOffBeerKega(BasketItem item, Document document)
    {
      List<EgaisWriteOffActsItems> writeOffActsItemsList = new List<EgaisWriteOffActsItems>();
      foreach (Gbs.Core.Entities.Documents.Item obj in document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.Beer)))
      {
        obj.GoodStock = GoodsStocks.GetStocksByUid(obj.GoodStock.Uid);
        string numberForGoodStock = SharedRepository.GetFbNumberForGoodStock(obj.GoodStock);
        if (!numberForGoodStock.IsNullOrEmpty())
        {
          writeOffActsItemsList.Add(new EgaisWriteOffActsItems()
          {
            FbNumber = numberForGoodStock,
            MarkInfo = item.Comment,
            Quantity = obj.Quantity,
            StockUid = obj.GoodStock.Uid,
            Sum = obj.Quantity * item.SalePrice * 10M,
            ActType = TypeWriteOff1.Реализация
          });
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            obj.GoodStock.Save(dataBase);
        }
      }
      if (!writeOffActsItemsList.Any<EgaisWriteOffActsItems>())
        return;
      new EgaisWriteOffActsItemRepository().Save(writeOffActsItemsList);
    }

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool allCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        switch (good.SetStatus)
        {
          case GlobalDictionaries.GoodsSetStatuses.None:
            if (ProductionCardViewModel._isBeer)
            {
              this.GetStock(good);
              continue;
            }
            break;
          case GlobalDictionaries.GoodsSetStatuses.Production:
            this.GetStock(good);
            continue;
        }
        MessageBoxHelper.Warning(Translate.ProductionCardViewModel_AddItemInBasket_В_данный_документ_можно_добавить_только_товары_с_модификацией__Производство__);
      }
    }

    private void GetStock(Gbs.Core.Entities.Goods.Good good)
    {
      if (!good.SetContent.Any<GoodsSets.Set>() && !ProductionCardViewModel._isBeer)
        MessageBoxHelper.Warning(string.Format(Translate.ProductionCardViewModel_GetStock_В_составе_рецепта___0___нет_ингредиентов_для_производства_, (object) good.Name));
      else if (ProductionCardViewModel._isBeer && this.Production.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == good.Uid)))
      {
        MessageBoxHelper.Warning(string.Format("Товар {0} уже добавлен в документ для вскрытия. Повторно добавить тот же товар нельзя.", (object) good.Name));
      }
      else
      {
        this.Production.DocumentsType = GlobalDictionaries.DocumentsTypes.ProductionSet;
        BasketItem basketItem = new BasketItem(good, Guid.Empty, 0M, this.Storage);
        ActionResult actionResult = this.Production.Add(basketItem, true, false);
        if (actionResult.Result != ActionResult.Results.Ok)
          return;
        if (ProductionCardViewModel._isBeer)
        {
          basketItem.Good.Group.RuMarkedProductionType = GlobalDictionaries.RuMarkedProductionTypes.Alcohol;
          this.Production.EditComment((object) new List<BasketItem>()
          {
            basketItem
          }, false);
        }
        if (actionResult.Result == ActionResult.Results.Error)
        {
          int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Hand);
        }
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
    }

    private IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      this.UpdateDocument((Gbs.Core.ViewModels.Basket.Basket) this.Production);
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Production.Document);
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
      out List<Document> docs,
      Gbs.Core.Entities.Users.User authUser = null,
      bool isBeer = false)
    {
      ProductionCardViewModel._isBeer = isBeer;
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
        document = (Document) null;
        docs = new List<Document>();
        return false;
      }
      if (!Other.IsActiveAndShowForm<FrmProductionCard>(uid.ToString()))
      {
        document = (Document) null;
        docs = new List<Document>();
        return false;
      }
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, Actions.AddProduction))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.AddProduction);
          if (!access.Result)
          {
            document = (Document) null;
            docs = new List<Document>();
            return false;
          }
          authUser = access.User;
        }
        this.Storages = new List<Gbs.Core.Entities.Storages.Storage>(Gbs.Core.Entities.Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => !x.IS_DELETED))));
        if (this.Storages.Count == 1)
        {
          this.Storage = this.Storages.Single<Gbs.Core.Entities.Storages.Storage>();
          this.OnPropertyChanged("Storage");
        }
        this._authUser = authUser;
        document = uid == Guid.Empty ? new Document() : new DocumentsRepository(dataBase).GetByUid(uid);
        this.Production.Document = document;
        this.Production.IsBeer = isBeer;
        this.EntityClone = (IEntity) document.Clone<Document>();
        this.Production.Items = new ObservableCollection<BasketItem>(document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, this.Production.Document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
        this.FormToSHow = (WindowWithSize) new FrmProductionCard();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ShowForm();
        docs = this._isResult ? this._listItems : new List<Document>();
        document = this._isResult ? this.Production.Document : (Document) null;
        return this._isResult;
      }
    }
  }
}
