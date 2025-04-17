// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.FrmGoodStockCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public class FrmGoodStockCard : WindowWithSize, IComponentConnector
  {
    internal DecimalUpDown PriceUpDown;
    private bool _contentLoaded;

    public FrmGoodStockCard() => this.InitializeComponent();

    public bool ShowCardStock(
      Guid stockUid,
      out GoodsStocks.GoodStock stock,
      bool isEnabledStock,
      Gbs.Core.Entities.Goods.Good good,
      Gbs.Core.Entities.Users.User user,
      GoodsStocks.GoodStock cashStock = null)
    {
      try
      {
        bool flag = true;
        UsersRepository usersRepository = new UsersRepository();
        if (stockUid == Guid.Empty)
        {
          if (!usersRepository.GetAccess(user, Actions.AddGoodStock))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.AddGoodStock);
            if (!access.Result)
            {
              stock = (GoodsStocks.GoodStock) null;
              return false;
            }
            user = access.User;
          }
        }
        else if (!usersRepository.GetAccess(user, Actions.EditQuantityGoodStock) && !usersRepository.GetAccess(user, Actions.EditSalePriceGoodStock))
        {
          Gbs.Core.Entities.Users.User user1 = new Gbs.Core.Entities.Users.User();
          if (!new Authorization().LoginUser(ref user1))
          {
            stock = (GoodsStocks.GoodStock) null;
            return false;
          }
          user = user1;
          isEnabledStock = usersRepository.GetAccess(user, Actions.EditQuantityGoodStock);
          flag = usersRepository.GetAccess(user, Actions.EditSalePriceGoodStock);
          if (!flag && !isEnabledStock)
          {
            int num = (int) MessageBoxHelper.Show(Translate.AuthorizationViewModel_У_вас_недостаточно_прав_для_выполнения_действия_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Hand);
            stock = (GoodsStocks.GoodStock) null;
            return false;
          }
        }
        else
        {
          isEnabledStock = usersRepository.GetAccess(user, Actions.EditQuantityGoodStock);
          flag = usersRepository.GetAccess(user, Actions.EditSalePriceGoodStock);
        }
        stock = stockUid == Guid.Empty ? new GoodsStocks.GoodStock() : cashStock?.Clone() ?? new GoodsStocks.GoodStock();
        isEnabledStock = (new ConfigsRepository<DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home || !(stockUid != Guid.Empty)) && isEnabledStock;
        GoodsStockViewModel goodsStockViewModel = new GoodsStockViewModel(stock, good)
        {
          Close = new Action(((Window) this).Close),
          IsEnabled = stockUid == Guid.Empty,
          IsEnabledStock = isEnabledStock,
          IsEnabledPrice = flag
        };
        this.DataContext = (object) goodsStockViewModel;
        this.ShowDialog();
        return goodsStockViewModel.SaveResult;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке остатка");
        stock = (GoodsStocks.GoodStock) null;
        return false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/frmgoodstockcard.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.PriceUpDown = (DecimalUpDown) target;
      else
        this._contentLoaded = true;
    }
  }
}
