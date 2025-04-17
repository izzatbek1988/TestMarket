// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.OrderSeparationViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public partial class OrderSeparationViewModel : ViewModelWithForm
  {
    private bool _saveResult;
    private List<BasketItem> _items = new List<BasketItem>();

    public bool ShowSeparationOrder(Document order)
    {
      this._items = new List<BasketItem>(order.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, order.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
      CafeBasket cafeBasket = new CafeBasket();
      cafeBasket.Items = new ObservableCollection<BasketItem>(this._items.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (x => x.Clone())));
      cafeBasket.Document = order;
      this.BasketForOrder = cafeBasket;
      this.NewBasketForOrder = new CafeBasket();
      this.FormToSHow = (WindowWithSize) new FrmOrderSeparation();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      return this._saveResult;
    }

    public CafeBasket BasketForOrder { get; set; }

    public CafeBasket NewBasketForOrder { get; set; }

    public ICommand AddRightListCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.UpdateItemsOrder(obj, (Gbs.Core.ViewModels.Basket.Basket) this.BasketForOrder, (Gbs.Core.ViewModels.Basket.Basket) this.NewBasketForOrder)));
      }
    }

    public ICommand AddLeftListCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.UpdateItemsOrder(obj, (Gbs.Core.ViewModels.Basket.Basket) this.NewBasketForOrder, (Gbs.Core.ViewModels.Basket.Basket) this.BasketForOrder)));
      }
    }

    public ICommand SaveSeparationCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.BasketForOrder.Items.Any<BasketItem>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.OrderSeparationViewModel_В_старом_заказе_не_осталось_товаров__сохранение_невозможно);
          }
          else if (!this.NewBasketForOrder.Items.Any<BasketItem>())
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.OrderSeparationViewModel_В_новом_заказе_нет_товаров__сохранение_невозможно);
          }
          else
          {
            if (this.BasketForOrder.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)) || this.NewBasketForOrder.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)))
            {
              if (MessageBoxHelper.Show(Translate.OrderSeparationViewModel_SaveSeparationCommand_При_разделении_будет_заново_пересчитан_процент_за_обслуживание_по_этим_заказам__продолжить_разделение_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              this.BasketForOrder.Items = new ObservableCollection<BasketItem>(this.BasketForOrder.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)));
              this.NewBasketForOrder.Items = new ObservableCollection<BasketItem>(this.NewBasketForOrder.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != GlobalDictionaries.PercentForServiceGoodUid)));
            }
            CafeBasket newBasketForOrder = this.NewBasketForOrder;
            newBasketForOrder.Client = new ClientAdnSum()
            {
              Client = new Client()
              {
                Uid = this.BasketForOrder.Document.ContractorUid
              }
            };
            this.BasketForOrder.Storage = this.BasketForOrder.Items.First<BasketItem>().Storage;
            this.NewBasketForOrder.Storage = this.NewBasketForOrder.Items.First<BasketItem>().Storage;
            Document document1 = this.BasketForOrder.Document.Clone<Document>();
            this.BasketForOrder.Document.Number = string.Empty;
            if ((this.BasketForOrder.SaveCafe(false, Guid.Empty, false).Result != ActionResult.Results.Ok ? 0 : (this.NewBasketForOrder.SaveCafe(false, Guid.Empty, false).Result == ActionResult.Results.Ok ? 1 : 0)) == 0)
              return;
            using (DataBase dataBase = Data.GetDataBase())
            {
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              document1.Comment = string.Format(Translate.OrderSeparationViewModel_, (object) this.BasketForOrder.Document.Number, (object) this.NewBasketForOrder.Document.Number);
              document1.Status = GlobalDictionaries.DocumentsStatuses.Close;
              Document document2 = document1;
              documentsRepository.Save(document2, false);
              int num3 = (int) MessageBoxHelper.Show(string.Format(Translate.OrderSeparationViewModel_2, (object) this.NewBasketForOrder.Document.Number, (object) this.BasketForOrder.Document.Number));
              this._saveResult = true;
              this.CloseAction();
            }
          }
        }));
      }
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    private void UpdateItemsOrder(object obj, Gbs.Core.ViewModels.Basket.Basket removeBasket, Gbs.Core.ViewModels.Basket.Basket insertBasket)
    {
      List<BasketItem> list = ((IEnumerable) obj).Cast<BasketItem>().ToList<BasketItem>();
      if (!list.Any<BasketItem>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
      }
      else
      {
        foreach (BasketItem basketItem1 in list)
        {
          BasketItem orderItem = basketItem1;
          BasketItem item = orderItem.Clone();
          if (insertBasket.EditQuantity(item))
          {
            BasketItem basketItem2 = this._items.Single<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == orderItem.Uid));
            if (basketItem2.Quantity < item.Quantity)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.ReturnItemsViewModel_Слишком_больше_кол_во);
            }
            else
            {
              if (insertBasket.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)))
              {
                if (basketItem2.Quantity < insertBasket.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid)).Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity)) + item.Quantity)
                {
                  int num3 = (int) MessageBoxHelper.Show(Translate.ReturnItemsViewModel_Слишком_больше_кол_во);
                  continue;
                }
                BasketItem basketItem3 = insertBasket.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid));
                basketItem3.Quantity = basketItem3.Quantity + item.Quantity;
              }
              else
                insertBasket.Items.Add(item);
              BasketItem basketItem4 = removeBasket.Items.First<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == item.Uid));
              basketItem4.Quantity = basketItem4.Quantity - item.Quantity;
            }
          }
        }
        insertBasket.ReCalcTotals();
        removeBasket.ReCalcTotals();
      }
    }
  }
}
