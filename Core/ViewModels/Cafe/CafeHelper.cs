// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Cafe.CafeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.ViewModels.Cafe
{
  internal class CafeHelper : ViewModel
  {
    private CafeBasket CurrentBasket { get; set; }

    public CafeHelper(CafeBasket basket) => this.CurrentBasket = basket;

    public void AddItemCafeOrder(Document document)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.CurrentBasket.CafeOrderUid = document.Uid;
        List<BasketItem> basketItemList = new List<BasketItem>(document.Items.Select<Item, BasketItem>((Func<Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, document.Storage, x.Quantity, x.Discount, x.Uid, x.Comment))));
        this.CurrentBasket.Items?.Clear();
        Action<BasketItem> action = (Action<BasketItem>) (x => this.CurrentBasket.AddItem(x, false));
        basketItemList.ForEach(action);
        this.CurrentBasket.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(document.ContractorUid);
        this.CurrentBasket.IsCheckedClient = true;
        this.CurrentBasket.SaleNumber = document.Number;
        this.CurrentBasket.CountGuest = Convert.ToInt32(document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CountGuestUid))?.Value ?? (object) 1);
        this.CurrentBasket.NumTable = Convert.ToInt32(document.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.NumTableUid))?.Value ?? (object) 0);
        this.CurrentBasket.Comment = document.Comment;
      }
    }
  }
}
