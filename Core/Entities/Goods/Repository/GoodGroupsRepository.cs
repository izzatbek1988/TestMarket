// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Goods.GoodGroupsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

#nullable disable
namespace Gbs.Core.Entities.Goods
{
  public class GoodGroupsRepository : IEntityRepository<GoodGroups.Group, GOODS_GROUPS>
  {
    public GoodGroupsRepository(DataBase db)
    {
    }

    public List<GoodGroups.Group> GetByQuery(IQueryable<GOODS_GROUPS> query)
    {
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрузка категорий товаров");
      DataContext dataContext = Data.GetDataContext();
      IQueryable<GOODS_GROUPS> query1 = dataContext.GetQuery<GOODS_GROUPS>(query);
      List<GOODS_GROUPS> list1 = dataContext.GetTable<GOODS_GROUPS>().Where<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => !x.IS_DELETED)).ToList<GOODS_GROUPS>();
      performancer.AddPoint("Загрузка всех категорий");
      List<GOODS_GROUPS> list2 = query1.ToList<GOODS_GROUPS>();
      performancer.AddPoint("Загрузка категорий из БД по запросу");
      List<GoodGroups.Group> byQuery = new List<GoodGroups.Group>();
      foreach (GOODS_GROUPS group in list2)
      {
        if (group != null)
        {
          GoodGroups.Group g = new GoodGroups.Group(group);
          if (g.ParentGroupUid != Guid.Empty)
          {
            if (list1.Any<GOODS_GROUPS>((Func<GOODS_GROUPS, bool>) (x => x.UID == g.ParentGroupUid)))
            {
              if (g.IsDataParent)
              {
                GOODS_GROUPS goodsGroups = list1.Find((Predicate<GOODS_GROUPS>) (x => x.UID == g.ParentGroupUid));
                g.GoodsType = (GlobalDictionaries.GoodTypes) goodsGroups.GOODS_TYPE;
                g.IsFreePrice = goodsGroups.IS_FREE_PRICE;
                g.IsCompositeGood = g.IsCompositeGood;
                g.KkmSectionNumber = goodsGroups.KKM_SECTION_NUMBER;
                g.TaxRateNumber = goodsGroups.KKM_TAX_NUMBER;
                g.IsRequestCount = goodsGroups.IS_REQUEST_COUNT;
                g.NeedComment = goodsGroups.NEED_COMMENT;
                g.RuFfdGoodsType = (GlobalDictionaries.RuFfdGoodsTypes) goodsGroups.RU_FFD_GOODS_TYPE;
                g.RuTaxSystem = (GlobalDictionaries.RuTaxSystems) goodsGroups.RU_TAX_SYSTEM;
                g.UnitsUid = goodsGroups.UNITS_UID;
                g.DecimalPlace = goodsGroups.DECIMAL_PLACE;
                g.RuMarkedProductionType = (GlobalDictionaries.RuMarkedProductionTypes) goodsGroups.RU_MARKED_PRODUCTION_TYPE;
              }
            }
            else
            {
              g.IsDataParent = false;
              g.ParentGroupUid = Guid.Empty;
            }
          }
          byQuery.Add(g);
        }
      }
      performancer.Stop();
      LogHelper.OnEnd();
      return byQuery;
    }

    public List<GoodGroups.Group> GetAllItems()
    {
      return this.GetByQuery((IQueryable<GOODS_GROUPS>) Data.GetDataContext().GetTable<GOODS_GROUPS>());
    }

    public List<GoodGroups.Group> GetActiveItems()
    {
      return this.GetByQuery(Data.GetDataContext().GetTable<GOODS_GROUPS>().Where<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => !x.IS_DELETED)));
    }

    public GoodGroups.Group GetByUid(Guid uid)
    {
      Performancer performancer = new Performancer("Загрузка категории товаров по UID категорий товаров");
      DataContext dataContext = Data.GetDataContext();
      GOODS_GROUPS group = dataContext.GetTable<GOODS_GROUPS>().SingleOrDefault<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.UID == uid));
      performancer.AddPoint("Загрузка категорий из БД");
      if (group == null)
        return (GoodGroups.Group) null;
      GoodGroups.Group g = new GoodGroups.Group(group);
      if (g.ParentGroupUid != Guid.Empty)
      {
        GOODS_GROUPS goodsGroups = dataContext.GetTable<GOODS_GROUPS>().SingleOrDefault<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.UID == g.ParentGroupUid && !x.IS_DELETED));
        if (goodsGroups != null)
        {
          if (g.IsDataParent)
          {
            g.GoodsType = (GlobalDictionaries.GoodTypes) goodsGroups.GOODS_TYPE;
            g.IsFreePrice = goodsGroups.IS_FREE_PRICE;
            g.KkmSectionNumber = goodsGroups.KKM_SECTION_NUMBER;
            g.TaxRateNumber = goodsGroups.KKM_TAX_NUMBER;
            g.IsRequestCount = goodsGroups.IS_REQUEST_COUNT;
            g.NeedComment = goodsGroups.NEED_COMMENT;
            g.RuFfdGoodsType = (GlobalDictionaries.RuFfdGoodsTypes) goodsGroups.RU_FFD_GOODS_TYPE;
            g.RuTaxSystem = (GlobalDictionaries.RuTaxSystems) goodsGroups.RU_TAX_SYSTEM;
            g.UnitsUid = goodsGroups.UNITS_UID;
            g.DecimalPlace = goodsGroups.DECIMAL_PLACE;
            g.RuMarkedProductionType = (GlobalDictionaries.RuMarkedProductionTypes) goodsGroups.RU_MARKED_PRODUCTION_TYPE;
            g.IsCompositeGood = goodsGroups.IS_COMPOSITE_GOOD;
          }
        }
        else
        {
          g.IsDataParent = false;
          g.ParentGroupUid = Guid.Empty;
        }
      }
      performancer.Stop();
      return g;
    }

    public bool Save(GoodGroups.Group item) => this.Save(item, true);

    public bool Save(GoodGroups.Group item, bool isWriteJson)
    {
      LogHelper.OnBegin();
      if (isWriteJson)
        new HomeOfficeHelper().PrepareAndSend<GoodGroups.Group>(item, HomeOfficeHelper.EntityEditHome.GoodGroup);
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      using (DataBase dataBase = Data.GetDataBase())
      {
        GoodGroups.Group group = item.IsDataParent ? this.GetByUid(item.ParentGroupUid) : item;
        if (group == null)
        {
          item.IsDataParent = false;
          group = item;
        }
        dataBase.InsertOrReplace<GOODS_GROUPS>(new GOODS_GROUPS()
        {
          UID = item.Uid,
          IS_DELETED = item.IsDeleted,
          NAME = item.Name,
          IS_COMPOSITE_GOOD = item.IsCompositeGood,
          IS_DATA_PARENT = item.IsDataParent,
          GOODS_TYPE = (int) group.GoodsType,
          IS_FREE_PRICE = group.IsFreePrice,
          KKM_SECTION_NUMBER = group.KkmSectionNumber,
          KKM_TAX_NUMBER = group.TaxRateNumber,
          IS_REQUEST_COUNT = group.IsRequestCount,
          NEED_COMMENT = group.NeedComment,
          PARENT_UID = item.ParentGroupUid,
          RU_FFD_GOODS_TYPE = (int) group.RuFfdGoodsType,
          RU_TAX_SYSTEM = (int) group.RuTaxSystem,
          UNITS_UID = group.UnitsUid,
          DECIMAL_PLACE = group.DecimalPlace,
          RU_MARKED_PRODUCTION_TYPE = (int) group.RuMarkedProductionType
        });
        dataBase.GetTable<SETTINGS>().Delete<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.ENTITY_UID == item.Uid));
      }
      LogHelper.OnEnd();
      return true;
    }

    public int Save(List<GoodGroups.Group> itemsList)
    {
      return itemsList.Count<GoodGroups.Group>(new Func<GoodGroups.Group, bool>(this.Save));
    }

    public int Delete(List<GoodGroups.Group> itemsList) => throw new NotImplementedException();

    public bool Delete(GoodGroups.Group item) => throw new NotImplementedException();

    public ActionResult Validate(GoodGroups.Group item)
    {
      (List<string> Message, bool Result) tuple = this.CheckParentGroups(item);
      List<string> message = tuple.Message;
      if (tuple.Result)
        return item.DataValidation();
      int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) message), Translate.Entity_Ошибка_валидации_данных, icon: MessageBoxImage.Exclamation);
      return new ActionResult(ActionResult.Results.Error);
    }

    private (List<string> Message, bool Result) CheckParentGroups(GoodGroups.Group item)
    {
      if (item.Uid == item.ParentGroupUid)
        return (new List<string>()
        {
          Translate.Group_Категория_не_может_быть_родителем_сама_себе
        }, false);
      if (!this.GetAllChild(item).Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == item.ParentGroupUid)))
        return (new List<string>(), true);
      return (new List<string>()
      {
        Translate.Group_Родительская_категория_является_потомком_текущий_и_не_может_быть_ее_родителем
      }, false);
    }

    public IEnumerable<GoodGroups.Group> GetAllChild(GoodGroups.Group item, bool includeSubChild = true)
    {
      using (Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems = this.GetAllItems();
        List<GoodGroups.Group> allChild = new List<GoodGroups.Group>();
        Func<GoodGroups.Group, bool> predicate = (Func<GoodGroups.Group, bool>) (x => x.ParentGroupUid == item.Uid);
        foreach (GoodGroups.Group group in allItems.Where<GoodGroups.Group>(predicate))
        {
          allChild.Add(group);
          if (includeSubChild)
            allChild.AddRange(this.GetAllChild(group));
        }
        return (IEnumerable<GoodGroups.Group>) allChild;
      }
    }
  }
}
