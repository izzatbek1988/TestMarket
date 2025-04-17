// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Productions.SpeedProductionViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Productions
{
  public class SpeedProductionViewModel : ViewModelWithForm
  {
    private Users.User _authUser;

    public Gbs.Core.ViewModels.Basket.Basket Basket { get; set; } = new Gbs.Core.ViewModels.Basket.Basket();

    public ICommand DoProductionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good()
          {
            SetStatus = GlobalDictionaries.GoodsSetStatuses.Production
          };
          if (!new FrmGoodCard().ShowGoodCard(good.Uid, out good, true, this._authUser, good, false, GlobalDictionaries.DocumentsTypes.ProductionSet))
            return;
          using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
            new GoodRepository(dataBase).Save(good);
            BasketItem basketItem = new BasketItem(good, Guid.Empty, 0M, (Storages.Storage) null);
            basketItem.Good.Group.RuMarkedProductionType = GlobalDictionaries.RuMarkedProductionTypes.None;
            basketItem.Good.Group.IsFreePrice = true;
            GoodsStocks.GoodStock stock = new GoodsStocks.GoodStock();
            while (new FrmGoodStockCard().ShowCardStock(Guid.Empty, out stock, true, basketItem.Good, this._authUser))
            {
              if (stock.Price == 0M || stock.Stock == 0M)
              {
                MessageBoxHelper.Warning(Translate.SpeedProductionViewModel_DoProductionCommand_Необходимо_указать_значения_цены_и_количества_отличное_от_нуля_);
              }
              else
              {
                basketItem.SalePrice = stock.Price;
                basketItem.Storage = stock.Storage;
                basketItem.Quantity = stock.Stock;
                Document newItem = ProductionCardViewModel.DoProduction(new List<BasketItem>()
                {
                  basketItem
                }, dataBase, Guid.NewGuid(), new GoodRepository(dataBase).GetActiveItems(), stock.Storage, this._authUser, (ProgressBarHelper.ProgressBar) null);
                if (newItem == null)
                  return;
                connectionTransaction.Commit();
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) null, (IEntity) newItem, ActionType.Add, GlobalDictionaries.EntityTypes.Document, this._authUser), false);
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.SpeedProductionViewModel_DoProductionCommand_Произведен_товар__0___цена___1_N2___количество___2_N3_, (object) basketItem.DisplayedName, (object) basketItem.SalePrice, (object) basketItem.Quantity)));
                if (MessageBoxHelper.Question(Translate.SpeedProductionViewModel_DoProductionCommand_Добавить_этот_товар_в_активную_корзину_для_продажи_) != MessageBoxResult.Yes)
                  return;
                Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(good.Uid);
                new BasketHelper(((MainWindowViewModel) (Application.Current.Windows.OfType<MainWindow>().SingleOrDefault<MainWindow>() ?? throw new NullReferenceException(Translate.ClientOrderViewModel_SaveCommand_Не_удалось_найти_экземпляр_главного_окна)).DataContext).CurrentBasket).AddItemToBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) new Gbs.Core.Entities.Goods.Good[1]
                {
                  byUid
                });
                return;
              }
            }
            MessageBoxHelper.Warning(Translate.SpeedProductionViewModel_DoProductionCommand_Производство_товара_было_отменено__при_необходимости_повторите_операцию_);
          }
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public void ShowProductionForm()
    {
      using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref this._authUser, Actions.AddSpeedProduction))
        {
          (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.AddSpeedProduction);
          if (!access.Result)
            return;
          this._authUser = access.User;
        }
        this._authUser.Group.Permissions.ForEach((Action<PermissionRules.PermissionRule>) (x => x.IsGranted = true));
        this.DoProductionCommand.Execute((object) null);
      }
    }
  }
}
