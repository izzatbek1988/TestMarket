// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Clients.GroupRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Clients
{
  public class GroupRepository
  {
    public GroupRepository(DataBase db)
    {
    }

    public GroupRepository()
    {
    }

    public bool IsExistClientInGroup(Group item)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return dataBase.GetTable<CLIENTS>().Any<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.GROUP_UID == item.Uid && !x.IS_DELETED));
    }

    public List<Group> GetAllItems()
    {
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрузка всех групп контактов");
      ITable<CLIENTS_GROUPS> table = Data.GetDataContext().GetTable<CLIENTS_GROUPS>();
      List<GoodsExtraPrice.GoodExtraPrice> prices = GoodsExtraPrice.GetGoodExtraPriceList();
      performancer.AddPoint("Доп. цены");
      List<CLIENTS_GROUPS> list1 = table.ToList<CLIENTS_GROUPS>();
      performancer.AddPoint("Запрос к БД");
      Func<CLIENTS_GROUPS, Group> selector = (Func<CLIENTS_GROUPS, Group>) (group =>
      {
        GoodsExtraPrice.GoodExtraPrice goodExtraPrice1 = prices.SingleOrDefault<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => x.Uid == group.PRICE_UID));
        Group allItems = new Group(group);
        GoodsExtraPrice.GoodExtraPrice goodExtraPrice2;
        if (!(group.PRICE_UID == Guid.Empty) && goodExtraPrice1 != null)
        {
          goodExtraPrice2 = goodExtraPrice1;
        }
        else
        {
          goodExtraPrice2 = new GoodsExtraPrice.GoodExtraPrice();
          goodExtraPrice2.Uid = Guid.Empty;
          goodExtraPrice2.Name = Translate.GroupClientCardModelView_Основная;
        }
        allItems.Price = goodExtraPrice2;
        return allItems;
      });
      List<Group> list2 = list1.Select<CLIENTS_GROUPS, Group>(selector).ToList<Group>();
      performancer.Stop();
      LogHelper.OnEnd();
      return list2;
    }

    public List<Group> GetActiveItems()
    {
      Other.ConsoleWrite("Загрузка активных групп клиентов");
      return this.GetAllItems().Where<Group>((Func<Group, bool>) (x => !x.IsDeleted)).ToList<Group>();
    }

    public Group GetByUid(Guid uid)
    {
      LogHelper.OnBegin();
      CLIENTS_GROUPS group = Data.GetDataContext().GetTable<CLIENTS_GROUPS>().SingleOrDefault<CLIENTS_GROUPS>((Expression<Func<CLIENTS_GROUPS, bool>>) (x => x.UID == uid));
      if (group == null)
        return (Group) null;
      List<GoodsExtraPrice.GoodExtraPrice> goodExtraPriceList = GoodsExtraPrice.GetGoodExtraPriceList();
      Group byUid = new Group(group);
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice1;
      if (!(group.PRICE_UID == Guid.Empty))
      {
        GoodsExtraPrice.GoodExtraPrice goodExtraPrice2 = goodExtraPriceList.FirstOrDefault<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => x.Uid == group.PRICE_UID));
        if (goodExtraPrice2 == null)
        {
          GoodsExtraPrice.GoodExtraPrice goodExtraPrice3 = new GoodsExtraPrice.GoodExtraPrice();
          goodExtraPrice3.Uid = Guid.Empty;
          goodExtraPrice3.Name = Translate.GroupClientCardModelView_Основная;
          goodExtraPrice1 = goodExtraPrice3;
        }
        else
          goodExtraPrice1 = goodExtraPrice2;
      }
      else
      {
        goodExtraPrice1 = new GoodsExtraPrice.GoodExtraPrice();
        goodExtraPrice1.Uid = Guid.Empty;
        goodExtraPrice1.Name = Translate.GroupClientCardModelView_Основная;
      }
      byUid.Price = goodExtraPrice1;
      LogHelper.OnEnd();
      return byUid;
    }

    public bool Save(Group item, bool isWriteJson = true)
    {
      LogHelper.OnBegin();
      if (!DevelopersHelper.IsUnitTest() && isWriteJson)
        new HomeOfficeHelper().PrepareAndSend<Group>(item, HomeOfficeHelper.EntityEditHome.ClientGroup);
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      DataContext dataContext = Data.GetDataContext();
      CLIENTS_GROUPS clientsGroups = new CLIENTS_GROUPS();
      clientsGroups.UID = item.Uid;
      clientsGroups.NAME = item.Name;
      clientsGroups.DISCOUNT = item.Discount.ToDbDecimal();
      clientsGroups.PRICE_UID = item.Price.Uid;
      clientsGroups.IS_SUPPLIER = item.IsSupplier;
      Decimal? maxSumCredit = item.MaxSumCredit;
      ref Decimal? local = ref maxSumCredit;
      clientsGroups.MAX_SUM_CREDIT = local.HasValue ? local.GetValueOrDefault().ToDbDecimal() : -1M;
      clientsGroups.IS_DELETED = item.IsDeleted;
      clientsGroups.IS_NON_USE_BONUS = item.IsNonUseBonus;
      dataContext.InsertOrReplace<CLIENTS_GROUPS>(clientsGroups);
      LogHelper.OnEnd();
      return true;
    }

    public bool Delete(Group item)
    {
      item.IsDeleted = true;
      return this.Save(item);
    }

    public ActionResult Validate(Group item) => ValidationHelper.DataValidation((Entity) item);

    public Group CreateDefaultClientGroup()
    {
      Group group = new Group()
      {
        Name = Translate.FrmListClients_Контакты,
        Discount = 0M
      };
      return !this.Save(group) ? (Group) null : group;
    }
  }
}
