// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SendWaybills.SendWaybillCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
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
namespace Gbs.Forms.SendWaybills
{
  public partial class SendWaybillCardViewModel : ViewModel, ICheckChangesViewModel
  {
    public Action ActionUpdate;

    public List<HomeOfficeHelper.PointInfo> Points { get; set; } = new List<HomeOfficeHelper.PointInfo>();

    public HomeOfficeHelper.PointInfo PointMove { get; set; }

    public bool IsResult { get; set; }

    public Users.User AuthUser { get; set; }

    public Action CloseAction { get; set; }

    public Move SendWaybill { get; set; } = new Move();

    public ICommand AddItem
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SendWaybill.IsNeedComment = false;
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.Move, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
          this.SendWaybill.IsNeedComment = true;
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.SendWaybill.Items.Any<BasketItem>())
            MessageBoxHelper.Warning(Translate.SendWaybillCardViewModel_Необходимо_добавить_товары_для_перемещения);
          else if (this.PointMove == null)
          {
            MessageBoxHelper.Warning(Translate.SendWaybillCardViewModel_Необходимо_выбрать_точку_получатель);
          }
          else
          {
            this.UpdateDocument((Gbs.Core.ViewModels.Basket.Basket) this.SendWaybill);
            using (DataBase dataBase = Data.GetDataBase())
            {
              this.SendWaybill.Document.Number = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.Move);
              this.SendWaybill.Document.Status = GlobalDictionaries.DocumentsStatuses.Close;
              this.SendWaybill.Document.Type = GlobalDictionaries.DocumentsTypes.Move;
              Document document = this.SendWaybill.Document;
              Users.User authUser = this.AuthUser;
              Guid guid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty;
              document.UserUid = guid;
              this.SendWaybill.Document.ContractorUid = this.PointMove.DbUid;
              this.SendWaybill.Document.Storage = this.SendWaybill.Storage;
              if (!new DocumentsRepository(dataBase).Save(this.SendWaybill.Document))
                return;
              this.SendWaybill.Document.Items.ForEach((Action<Gbs.Core.Entities.Documents.Item>) (x =>
              {
                if (x.GoodStock == null)
                  return;
                x.GoodStock = GoodsStocks.GetStocksByUid(x.GoodStock.Uid);
              }));
              if (!MoveHelper.CreateMoveFile(this.SendWaybill.Document, this.PointMove.DbUid))
                return;
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
              {
                Title = Translate.GlobalDictionaries_Перемещение,
                Text = string.Format(Translate.SendWaybillCardViewModel_Накладная_успешно_отправлена_в_точку__0_, (object) this.PointMove.InfoDataBase.NameDataBase)
              });
              this.IsResult = true;
              WindowWithSize.IsCancel = false;
              Action actionUpdate = this.ActionUpdate;
              if (actionUpdate != null)
                actionUpdate();
              this.CloseAction();
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) null, (IEntity) this.SendWaybill.Document, ActionType.Add, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
            }
          }
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public SendWaybillCardViewModel()
    {
    }

    public SendWaybillCardViewModel(Document document, Action close)
    {
      this.SendWaybill.Document = document;
      this.SendWaybill.DocumentsType = GlobalDictionaries.DocumentsTypes.Move;
      this.EntityClone = (IEntity) document.Clone<Document>();
      this.CloseAction = close;
      this.SendWaybill.Items = new ObservableCollection<BasketItem>(document.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
      this.Points = new List<HomeOfficeHelper.PointInfo>(HomeOfficeHelper.GetPointFromCloud().Where<HomeOfficeHelper.PointInfo>((Func<HomeOfficeHelper.PointInfo, bool>) (x => x.DbUid != UidDb.GetUid().EntityUid)));
      if (!this.Points.Any<HomeOfficeHelper.PointInfo>())
      {
        MessageBoxHelper.Warning(Translate.SendWaybillCardViewModel_неНайденыДругиеТочки);
        this.IsShow = false;
      }
      if (this.Points.Count != 1)
        return;
      this.PointMove = this.Points.First<HomeOfficeHelper.PointInfo>();
      this.OnPropertyChanged(nameof (PointMove));
    }

    public bool IsShow { get; set; } = true;

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool allCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        switch (good.SetStatus)
        {
          case GlobalDictionaries.GoodsSetStatuses.None:
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              MessageBoxHelper.Warning(Translate.WriteOffCardViewModel_Невозможно_списать_сертификат);
              continue;
            }
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            {
              MessageBoxHelper.Warning(Translate.WriteOffCardViewModel_Невозможно_списать_услугу);
              continue;
            }
            this.GetStock(good, allCount);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Set:
            MessageBoxHelper.Warning(Translate.WriteOffCardViewModel_Невозможно_списать_комплект);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Kit:
            using (DataBase dataBase = Data.GetDataBase())
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
        ActionResult actionResult = this.SendWaybill.Add(new BasketItem(good, goodStock.ModificationUid, goodStock.Price, goodStock.Storage, q));
        if (actionResult.Result == ActionResult.Results.Error)
        {
          int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Hand);
        }
      }
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      this.UpdateDocument((Gbs.Core.ViewModels.Basket.Basket) this.SendWaybill);
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.SendWaybill.Document);
    }

    public void UpdateDocument(Gbs.Core.ViewModels.Basket.Basket basket)
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
  }
}
