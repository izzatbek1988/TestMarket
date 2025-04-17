// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.GroupClientCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Clients
{
  public partial class GroupClientCardModelView : ViewModelWithForm, ICheckChangesViewModel
  {
    private Action _closeAction;
    public bool _isEdit;

    public bool SaveResult { get; set; }

    public ICommand SaveGroup { get; set; }

    public ICommand CloseFormCommand { get; set; }

    public Gbs.Core.Entities.Clients.Group Group { get; set; }

    public IEnumerable<GoodsExtraPrice.GoodExtraPrice> ExtraPrices { get; set; }

    public GroupClientCardModelView(Action close)
    {
      GoodsExtraPrice.GoodExtraPrice[] first = new GoodsExtraPrice.GoodExtraPrice[1];
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice.Name = Translate.GroupClientCardModelView_Основная;
      goodExtraPrice.Uid = Guid.Empty;
      first[0] = goodExtraPrice;
      // ISSUE: reference to a compiler-generated field
      this.\u003CExtraPrices\u003Ek__BackingField = ((IEnumerable<GoodsExtraPrice.GoodExtraPrice>) first).Concat<GoodsExtraPrice.GoodExtraPrice>(GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)));
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._closeAction = close;
      this.SaveGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.CloseFormCommand = (ICommand) new RelayCommand((Action<object>) (obj => this._closeAction()));
    }

    public GroupClientCardModelView()
    {
      GoodsExtraPrice.GoodExtraPrice[] first = new GoodsExtraPrice.GoodExtraPrice[1];
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice.Name = Translate.GroupClientCardModelView_Основная;
      goodExtraPrice.Uid = Guid.Empty;
      first[0] = goodExtraPrice;
      // ISSUE: reference to a compiler-generated field
      this.\u003CExtraPrices\u003Ek__BackingField = ((IEnumerable<GoodsExtraPrice.GoodExtraPrice>) first).Concat<GoodsExtraPrice.GoodExtraPrice>(GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)));
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private void Save()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        bool flag = new GroupRepository(dataBase).Save(this.Group);
        this.SaveResult = flag;
        if (!flag)
          return;
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Group, this._isEdit ? ActionType.Edit : ActionType.Add, GlobalDictionaries.EntityTypes.ClientGroup, this.AuthUser), true);
        WindowWithSize.IsCancel = false;
        this._closeAction();
      }
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Group);
    }

    public Users.User AuthUser { get; set; }
  }
}
