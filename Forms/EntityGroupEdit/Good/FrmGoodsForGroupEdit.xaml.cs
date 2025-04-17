// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodGroupEdit.GoodsGroupEditViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.GoodGroupEdit;
using Gbs.Forms.GoodGroupEdit;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods.GoodGroupEdit
{
  public partial class GoodsGroupEditViewModel : ViewModelWithForm
  {
    public bool IsVisibleMessage { get; private set; }

    public GroupEditing GroupEditing { get; set; } = new GroupEditing();

    public ICommand DoGroupEditingCommand { get; set; }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public ICommand AddItem { get; set; }

    public GoodsGroupEditViewModel()
    {
      this.AddItem = (ICommand) new RelayCommand((Action<object>) (obj => this.AddItems()));
      this.DoGroupEditingCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.DoGroupEditing()));
    }

    private void AddItems()
    {
      (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.None, isVisNullStock: true, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemsGood));
      this.AddItemsGood((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
    }

    private void AddItemsGood(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool isAllCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good1 in goods)
      {
        Gbs.Core.Entities.Goods.Good good = good1;
        if (!this.GroupEditing.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Uid == good.Uid)))
        {
          Decimal price = !good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? 0M : good.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price));
          this.GroupEditing.Items.Add(new BasketItem(good, Guid.Empty, price, (Storages.Storage) null));
        }
      }
      this.OnPropertyChanged("Items");
      this.IsVisibleMessage = true;
    }

    private void DoGroupEditing()
    {
      if (!this.GroupEditing.Items.Any<BasketItem>())
      {
        MessageBoxHelper.Warning(Translate.LablePrintViewModel_В_списке_нет_товаров_);
      }
      else
      {
        List<string> listEdit;
        bool flag = new ActionGoodEditViewModel().DoAction(this.GroupEditing.Items.Select<BasketItem, Gbs.Core.Entities.Goods.Good>((Func<BasketItem, Gbs.Core.Entities.Goods.Good>) (x => x.Good)).ToList<Gbs.Core.Entities.Goods.Good>(), this.AuthUser, out listEdit);
        this.IsVisibleMessage = !flag;
        if (!flag)
          return;
        string separator = new string(new char[2]
        {
          '\r',
          '\n'
        });
        string str;
        if (this.GroupEditing.Items.Count > 10)
          str = Translate.GoodsGroupEditViewModel_DoGroupEditing_Товары_ + separator + string.Join(separator, this.GroupEditing.Items.Take<BasketItem>(10).Select<BasketItem, string>((Func<BasketItem, string>) (x => x.DisplayedName))) + string.Format(Translate.GoodsGroupEditViewModel_DoGroupEditing__и_еще__0__товаров_, (object) (this.GroupEditing.Items.Count - 10)) + separator;
        else
          str = Translate.GoodsGroupEditViewModel_DoGroupEditing_Товары_ + separator + string.Join(separator, this.GroupEditing.Items.Select<BasketItem, string>((Func<BasketItem, string>) (x => x.DisplayedName))) + separator;
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateTextHistory(str + Translate.GoodsGroupEditViewModel_DoGroupEditing_Изменения_ + separator + string.Join(separator, (IEnumerable<string>) listEdit), GlobalDictionaries.EntityTypes.GroupEditGood, this.AuthUser), false);
        this.CloseAction();
      }
    }

    public Users.User AuthUser { get; set; }
  }
}
