// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsStocks
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.HomeOffice.Entity;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsStocks
  {
    public static List<GoodsStocks.GoodStock> GetGoodStockList(IQueryable<GOODS_STOCK> query = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GoodsStocks.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new GoodsStocks.\u003C\u003Ec__DisplayClass0_0();
      LogHelper.OnBegin();
      Performancer performancer = new Performancer("Загрузка остатков товаров");
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<GOODS_STOCK>();
        List<Storages.Storage> list1 = Storages.GetStorages().ToList<Storages.Storage>();
        performancer.AddPoint(string.Format("Склады: {0}", (object) list1.Count));
        List<GOODS_STOCK> list2 = query.ToList<GOODS_STOCK>();
        performancer.AddPoint(string.Format("Запрос к БД: {0}", (object) list2.Count));
        List<GOODS_STOCK> list3 = list2.DistinctBy<GOODS_STOCK, Guid>((Func<GOODS_STOCK, Guid>) (x => x.UID)).ToList<GOODS_STOCK>();
        performancer.AddPoint(string.Format("Очистка от дублей: {0}", (object) list3.Count));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass00.dc = Data.GetDataContext();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        List<EntityProperties.PropertyValue> valuesList = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.GoodStock, cDisplayClass00.dc.GetQuery<GOODS_STOCK>(query).SelectMany<GOODS_STOCK, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>((Expression<Func<GOODS_STOCK, IEnumerable<ENTITY_PROPERTIES_VALUES>>>) (d => cDisplayClass00.dc.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>(Expression.Lambda<Func<ENTITY_PROPERTIES_VALUES, bool>>((Expression) Expression.Equal(x.ENTITY_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS_STOCK.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))), (Expression<Func<GOODS_STOCK, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>>) ((d, ev) => ev)));
        List<GoodsStocks.GoodStock> list4 = list3.Join<GOODS_STOCK, Storages.Storage, Guid, GoodsStocks.GoodStock>((IEnumerable<Storages.Storage>) list1, (Func<GOODS_STOCK, Guid>) (stock => stock.STORAGE_UID), (Func<Storages.Storage, Guid>) (stor => stor.Uid), (Func<GOODS_STOCK, Storages.Storage, GoodsStocks.GoodStock>) ((stock, stor) =>
        {
          return new GoodsStocks.GoodStock()
          {
            Uid = stock.UID,
            GoodUid = stock.GOOD_UID,
            IsDeleted = stock.IS_DELETED,
            ModificationUid = stock.MODIFICATION_UID,
            Price = stock.PRICE,
            Stock = stock.STOCK,
            Storage = stor
          };
        })).AsParallel<GoodsStocks.GoodStock>().ToList<GoodsStocks.GoodStock>().GroupJoin<GoodsStocks.GoodStock, EntityProperties.PropertyValue, Guid, GoodsStocks.GoodStock>((IEnumerable<EntityProperties.PropertyValue>) valuesList, (Func<GoodsStocks.GoodStock, Guid>) (g => g.Uid), (Func<EntityProperties.PropertyValue, Guid>) (p => p.EntityUid), (Func<GoodsStocks.GoodStock, IEnumerable<EntityProperties.PropertyValue>, GoodsStocks.GoodStock>) ((g, p) =>
        {
          g.Properties = p.ToList<EntityProperties.PropertyValue>();
          return g;
        })).AsParallel<GoodsStocks.GoodStock>().ToList<GoodsStocks.GoodStock>();
        performancer.Stop();
        LogHelper.OnEnd();
        return list4;
      }
    }

    public static List<GoodsStocks.GoodStock> GetStocksForGood(Guid goodUid)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<GOODS_STOCK> stocksTable = dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.GOOD_UID == goodUid));
        List<STORAGES> storagesList = dataBase.GetTable<STORAGES>().SelectMany((Expression<Func<STORAGES, IEnumerable<GOODS_STOCK>>>) (s => stocksTable), (s, gs) => new
        {
          s = s,
          gs = gs
        }).Where(data => data.s.UID == data.gs.STORAGE_UID).Select(data => data.s).ToList<STORAGES>();
        return stocksTable.ToList<GOODS_STOCK>().Select<GOODS_STOCK, GoodsStocks.GoodStock>((Func<GOODS_STOCK, GoodsStocks.GoodStock>) (x => new GoodsStocks.GoodStock(x, storagesList.First<STORAGES>((Func<STORAGES, bool>) (s => s.UID == x.STORAGE_UID))))).ToList<GoodsStocks.GoodStock>();
      }
    }

    public static GoodsStocks.GoodStock GetStocksByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return GoodsStocks.GetGoodStockList(dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == uid))).FirstOrDefault<GoodsStocks.GoodStock>();
    }

    public class GoodStock : Gbs.Core.Entities.Entity
    {
      public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

      [Required]
      public Guid GoodUid { get; set; } = Guid.Empty;

      [Required]
      public Storages.Storage Storage { get; set; }

      [Required]
      [Range(-99999999.999, 99999999.999)]
      public Decimal Stock { get; set; }

      [Required]
      [Range(0.0, 2147483647.0)]
      public Decimal Price { get; set; }

      public Guid ModificationUid { get; set; } = Guid.Empty;

      public GoodStock()
      {
      }

      public GoodStock(GOODS_STOCK stockItem, STORAGES storageItem)
      {
        this.Uid = stockItem.UID;
        this.GoodUid = stockItem.GOOD_UID;
        this.ModificationUid = stockItem.MODIFICATION_UID;
        this.Price = stockItem.PRICE;
        this.Stock = stockItem.STOCK;
        Storages.Storage storage1;
        if (storageItem != null)
        {
          Storages.Storage storage2 = new Storages.Storage();
          storage2.Uid = storageItem.UID;
          storage2.IsDeleted = storageItem.IS_DELETED;
          storage2.Name = storageItem.NAME;
          storage1 = storage2;
        }
        else
          storage1 = (Storages.Storage) null;
        this.Storage = storage1;
        this.IsDeleted = stockItem.IS_DELETED;
      }

      public GoodStock(GoodStockHome stockItem)
      {
        this.Uid = stockItem.Uid;
        this.GoodUid = stockItem.GoodUid;
        this.ModificationUid = stockItem.ModificationUid;
        this.Price = stockItem.Price;
        this.Stock = stockItem.Stock;
        Storages.Storage storage = new Storages.Storage();
        storage.Uid = stockItem.StorageUid;
        this.Storage = storage;
        this.IsDeleted = stockItem.IsDeleted;
        this.Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) stockItem.Properties);
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public GoodsStocks.GoodStock Clone() => (GoodsStocks.GoodStock) this.MemberwiseClone();

      public bool Save(DataBase db)
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          throw new Exception(Translate.GoodStock_Ошибка_валидации_данных_остатка_товара);
        GOODS_STOCK goodsStock = new GOODS_STOCK()
        {
          UID = this.Uid,
          IS_DELETED = this.IsDeleted,
          GOOD_UID = this.GoodUid,
          MODIFICATION_UID = this.ModificationUid,
          PRICE = this.Price.ToDbDecimal(),
          STOCK = this.Stock.ToDbDecimal(),
          STORAGE_UID = this.Storage.Uid
        };
        LogHelper.Trace("Запись остатка в БД: " + goodsStock.ToJsonString());
        db.InsertOrReplace<GOODS_STOCK>(goodsStock);
        db.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == this.Uid));
        this.Properties?.ForEach((Action<EntityProperties.PropertyValue>) (x => this.SavePropertyValue(x, db)));
        return true;
      }

      public bool UpdateProperties(DataBase db)
      {
        db.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == this.Uid));
        this.Properties?.ForEach((Action<EntityProperties.PropertyValue>) (x => this.SavePropertyValue(x, db)));
        return true;
      }

      private bool SavePropertyValue(EntityProperties.PropertyValue property, DataBase db)
      {
        try
        {
          if (property.Value == null || property.Value.ToString().IsNullOrEmpty())
            return true;
          if (property.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
          {
            UID = property.Uid,
            ENTITY_UID = property.EntityUid,
            IS_DELETED = property.IsDeleted,
            TYPE_UID = property.Type.Uid,
            CONTENT = JsonConvert.ToString(property.Value)
          });
          return true;
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка сохранения значения доп. поля товарного остатка");
          return false;
        }
      }
    }
  }
}
