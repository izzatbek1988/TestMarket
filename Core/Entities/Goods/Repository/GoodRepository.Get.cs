// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Goods.GoodRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Core.Entities.Goods
{
  public class GoodRepository : IEntityRepository<Good, GOODS>
  {
    private readonly Gbs.Core.Db.DataBase _db;

    public bool MultiThreadMode { get; set; } = true;

    public GoodRepository(Gbs.Core.Db.DataBase db) => this._db = db;

    public GoodRepository()
    {
    }

    public List<Good> GetAllItems()
    {
      return this.GetByQuery((IQueryable<GOODS>) Gbs.Core.Data.GetDataContext().GetTable<GOODS>());
    }

    public List<Good> GetByQuery(IQueryable<GOODS> query)
    {
      Performancer performancer = new Performancer("Загрузка товаров с запросом");
      DataContext dataContext = Gbs.Core.Data.GetDataContext();
      if (query == null)
        query = (IQueryable<GOODS>) dataContext.GetTable<GOODS>();
      List<GoodsModifications.GoodModification> goodModifications = new List<GoodsModifications.GoodModification>();
      List<Task> list1 = new List<Task>();
      performancer.AddPoint("Проверяем категории");
      DataContext dc1;
      list1.Add(new Task((Action) (() =>
      {
        dc1 = Gbs.Core.Data.GetDataContext();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: method reference
        goodModifications = GoodsModifications.GetModificationsList(dc1.GetQuery<GOODS>(query).SelectMany<GOODS, GOODS_MODIFICATIONS, GOODS_MODIFICATIONS>((Expression<Func<GOODS, IEnumerable<GOODS_MODIFICATIONS>>>) (g => dc1.GetTable<GOODS_MODIFICATIONS>().Where<GOODS_MODIFICATIONS>(Expression.Lambda<Func<GOODS_MODIFICATIONS, bool>>((Expression) Expression.AndAlso(x.IS_DELETED == false, (Expression) Expression.Equal(x.GOOD_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression2))), (Expression<Func<GOODS, GOODS_MODIFICATIONS, GOODS_MODIFICATIONS>>) ((g, gm) => gm)).Distinct<GOODS_MODIFICATIONS>());
      })));
      List<GoodsSets.Set> goodSets = new List<GoodsSets.Set>();
      DataContext dc2;
      list1.Add(new Task((Action) (() =>
      {
        dc2 = Gbs.Core.Data.GetDataContext();
        ParameterExpression parameterExpression3;
        ParameterExpression parameterExpression4;
        // ISSUE: method reference
        // ISSUE: method reference
        goodSets = GoodsSets.GetSetsFilteredList(dc2.GetQuery<GOODS>(query).SelectMany<GOODS, GOODS_SETS, GOODS_SETS>((Expression<Func<GOODS, IEnumerable<GOODS_SETS>>>) (g => dc2.GetTable<GOODS_SETS>().Where<GOODS_SETS>(Expression.Lambda<Func<GOODS_SETS, bool>>((Expression) Expression.AndAlso(x.IS_DELETED == false, (Expression) Expression.Equal(x.PARENT_UID, (Expression) Expression.Property((Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression4))), (Expression<Func<GOODS, GOODS_SETS, GOODS_SETS>>) ((g, gs) => gs)).Distinct<GOODS_SETS>());
      })));
      List<GoodsStocks.GoodStock> goodStocks = new List<GoodsStocks.GoodStock>();
      DataContext dc3;
      list1.Add(new Task((Action) (() =>
      {
        dc3 = Gbs.Core.Data.GetDataContext();
        ParameterExpression parameterExpression5;
        ParameterExpression parameterExpression6;
        // ISSUE: method reference
        // ISSUE: method reference
        goodStocks = GoodsStocks.GetGoodStockList(dc3.GetQuery<GOODS>(query).SelectMany((Expression<Func<GOODS, IEnumerable<GOODS_STOCK>>>) (g => dc3.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>(Expression.Lambda<Func<GOODS_STOCK, bool>>((Expression) Expression.Equal(x.GOOD_UID, (Expression) Expression.Property((Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression6))), (g, gs) => new
        {
          g = g,
          gs = gs
        }).Where(data => data.gs.IS_DELETED == false).Select(data => data.gs));
      })));
      List<GoodGroups.Group> goodGroups = new List<GoodGroups.Group>();
      list1.Add(new Task((Action) (() => goodGroups = new GoodGroupsRepository(this._db).GetAllItems())));
      performancer.AddPoint("Категории товаров");
      List<EntityProperties.PropertyValue> goodPropertyValues = new List<EntityProperties.PropertyValue>();
      DataContext dc4;
      list1.Add(new Task((Action) (() =>
      {
        dc4 = Gbs.Core.Data.GetDataContext();
        ParameterExpression parameterExpression7;
        ParameterExpression parameterExpression8;
        // ISSUE: method reference
        // ISSUE: method reference
        goodPropertyValues = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Good, dc4.GetQuery<GOODS>(query).SelectMany<GOODS, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>((Expression<Func<GOODS, IEnumerable<ENTITY_PROPERTIES_VALUES>>>) (g => dc4.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>(Expression.Lambda<Func<ENTITY_PROPERTIES_VALUES, bool>>((Expression) Expression.Equal(x.ENTITY_UID, (Expression) Expression.Property((Expression) parameterExpression7, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression8))), (Expression<Func<GOODS, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>>) ((g, ev) => ev)));
      })));
      List<GOODS> goods = new List<GOODS>();
      list1.Add(new Task((Action) (() => goods = Gbs.Core.Data.GetDataContext().GetQuery<GOODS>(query).ToList<GOODS>().DistinctBy<GOODS, Guid>((Func<GOODS, Guid>) (x => x.UID)).ToList<GOODS>())));
      list1.RunList(this.MultiThreadMode);
      performancer.AddPoint("Ожидание задач");
      List<Good> list2 = goods.Join<GOODS, GoodGroups.Group, Guid, Good>((IEnumerable<GoodGroups.Group>) goodGroups, (Func<GOODS, Guid>) (g => g.GROUP_UID), (Func<GoodGroups.Group, Guid>) (gg => gg.Uid), (Func<GOODS, GoodGroups.Group, Good>) ((g, gg) =>
      {
        Good byQuery = new Good();
        byQuery.Uid = g.UID;
        byQuery.Barcode = g.BARCODE;
        Good good = byQuery;
        List<string> stringList;
        if (!g.BARCODES.IsNullOrEmpty())
          stringList = ((IEnumerable<string>) g.BARCODES.Split('\n')).ToList<string>();
        else
          stringList = new List<string>();
        good.Barcodes = (IEnumerable<string>) stringList;
        byQuery.DateAdd = g.DATE_ADD == new DateTime(2001, 1, 1) ? new DateTime(2018, 1, 1) : g.DATE_ADD;
        byQuery.DateUpdate = g.DATE_UPDATE;
        byQuery.Description = g.DESCRIPTION;
        byQuery.Name = g.NAME;
        byQuery.Group = gg;
        byQuery.IsDeleted = g.IS_DELETED;
        byQuery.SetStatus = (GlobalDictionaries.GoodsSetStatuses) g.SET_STATUS;
        byQuery.Modifications = goodModifications.Where<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.GoodUid == g.UID));
        byQuery.SetContent = goodSets.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => x.ParentUid == g.UID));
        return byQuery;
      })).AsParallel<Good>().ToList<Good>().GroupJoin<Good, GoodsStocks.GoodStock, Guid, Good>((IEnumerable<GoodsStocks.GoodStock>) goodStocks, (Func<Good, Guid>) (g => g.Uid), (Func<GoodsStocks.GoodStock, Guid>) (s => s.GoodUid), (Func<Good, IEnumerable<GoodsStocks.GoodStock>, Good>) ((g, s) =>
      {
        g.StocksAndPrices = s.ToList<GoodsStocks.GoodStock>();
        return g;
      })).AsParallel<Good>().ToList<Good>().GroupJoin<Good, EntityProperties.PropertyValue, Guid, Good>((IEnumerable<EntityProperties.PropertyValue>) goodPropertyValues, (Func<Good, Guid>) (g => g.Uid), (Func<EntityProperties.PropertyValue, Guid>) (p => p.EntityUid), (Func<Good, IEnumerable<EntityProperties.PropertyValue>, Good>) ((g, p) =>
      {
        g.Properties = p.DistinctBy<EntityProperties.PropertyValue, Guid>((Func<EntityProperties.PropertyValue, Guid>) (x => x.Type.Uid)).ToList<EntityProperties.PropertyValue>();
        return g;
      })).AsParallel<Good>().ToList<Good>();
      List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
      foreach (Good good in list2)
      {
        Dictionary<Guid, object> d = good.Properties.ToDictionary<EntityProperties.PropertyValue, Guid, object>((Func<EntityProperties.PropertyValue, Guid>) (y => y.Type.Uid), (Func<EntityProperties.PropertyValue, object>) (z => z.Value));
        foreach (EntityProperties.PropertyType propertyType in typesList.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (t => !d.Any<KeyValuePair<Guid, object>>((Func<KeyValuePair<Guid, object>, bool>) (di => di.Key == t.Uid)))))
          d.Add(propertyType.Uid, (object) null);
        good.PropertiesDictionary = d;
      }
      performancer.Stop("Товаров: " + list2.Count.ToString());
      DevelopersHelper.IsDebug();
      return list2;
    }

    public List<Good> GetActiveItems()
    {
      return this.GetByQuery(Gbs.Core.Data.GetDataContext().GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => !x.IS_DELETED)));
    }

    public Good GetByUid(Guid uid)
    {
      return this.GetByQuery(Gbs.Core.Data.GetDataContext().GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.UID == uid))).SingleOrDefault<Good>();
    }

    public bool Save(Good good) => this.Save(good, true);

    public bool Save(Good good, bool isWriteJson, bool saveStock = true)
    {
      DataConnectionTransaction connectionTransaction = (DataConnectionTransaction) null;
      try
      {
        LogHelper.OnBegin();
        if (!DevelopersHelper.IsUnitTest() && isWriteJson)
          new HomeOfficeHelper().PrepareAndSend<Good>(good, HomeOfficeHelper.EntityEditHome.Good);
        if (this.Validate(good).Result == ActionResult.Results.Error)
          return false;
        LogHelper.Trace(string.Format("Сохранение товара uid: {0}; name: {1}; barcode: {2}", (object) good.Uid, (object) good.Name, (object) good.Barcode));
        if (good.Properties != null)
        {
          List<EntityProperties.PropertyValue> properties1 = good.Properties;
          bool? nullable;
          if (properties1 == null)
          {
            nullable = new bool?();
          }
          else
          {
            EntityProperties.PropertyValue propertyValue = properties1.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid));
            if (propertyValue == null)
            {
              nullable = new bool?();
            }
            else
            {
              object obj = propertyValue.Value;
              nullable = obj != null ? new bool?(obj.ToString().IsNullOrEmpty()) : new bool?();
            }
          }
          if (nullable.GetValueOrDefault(true))
          {
            int index = good.Properties.FindIndex((Predicate<EntityProperties.PropertyValue>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid));
            if (index == -1)
            {
              List<EntityProperties.PropertyValue> properties2 = good.Properties;
              EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
              EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
              propertyType.Uid = GlobalDictionaries.GoodIdUid;
              propertyValue.Type = propertyType;
              propertyValue.Value = (object) null;
              properties2.Add(propertyValue);
            }
            else
              good.Properties[index].Value = (object) null;
          }
        }
        good.Barcodes = (IEnumerable<string>) good.Barcodes.Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))).ToList<string>();
        string str = string.Join(new string(new char[2]
        {
          '\r',
          '\n'
        }), good.Barcodes).Replace(" ", string.Empty);
        connectionTransaction = this._db.BeginTransaction();
        GOODS goods = new GOODS()
        {
          UID = good.Uid,
          NAME = good.Name,
          DATE_ADD = good.DateAdd,
          DATE_UPDATE = good.DateUpdate,
          BARCODE = good.Barcode,
          BARCODES = str.Length > 1000 ? str.Substring(0, 1000) : str,
          DESCRIPTION = good.Description ?? string.Empty,
          GROUP_UID = good.Group.Uid,
          SET_STATUS = (int) good.SetStatus,
          IS_DELETED = good.IsDeleted
        };
        LogHelper.Trace("Запись в БД: " + goods.ToJsonString());
        this._db.InsertOrReplace<GOODS>(goods);
        foreach (GoodsStocks.GoodStock goodStock in good.StocksAndPrices.AsEnumerable<GoodsStocks.GoodStock>())
        {
          GoodsStocks.GoodStock stock = goodStock;
          if (saveStock)
          {
            stock.GoodUid = good.Uid;
            bool flag = false;
            if (stock.IsDeleted)
            {
              GOODS_STOCK goodsStock = this._db.GetTable<GOODS_STOCK>().FirstOrDefault<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == stock.Uid));
              if (goodsStock == null)
                goodsStock = new GOODS_STOCK()
                {
                  STOCK = 0M,
                  UID = Guid.Empty
                };
              flag = goodsStock.IS_DELETED;
            }
            SaveDocumentEditStock(stock);
            if (stock.IsDeleted && !flag)
              stock.Stock = 0M;
            stock.Save(this._db);
          }
          else
            break;
        }
        this._db.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == good.Uid));
        this._db.GetTable<GOODS_SETS>().Where<GOODS_SETS>((Expression<Func<GOODS_SETS, bool>>) (x => x.PARENT_UID == good.Uid)).Set<GOODS_SETS, bool>((Expression<Func<GOODS_SETS, bool>>) (y => y.IS_DELETED), true).Update<GOODS_SETS>();
        if (good.Properties != null)
        {
          foreach (EntityProperties.PropertyValue property in good.Properties)
          {
            property.EntityUid = good.Uid;
            switch (property.Type.Type)
            {
              case GlobalDictionaries.EntityPropertyTypes.Integer:
                if (!(property.Type.Uid == GlobalDictionaries.CertificateReusableUid))
                {
                  long result;
                  if (long.TryParse(property.Value?.ToString(), out result))
                  {
                    property.Value = (object) result;
                    continue;
                  }
                  break;
                }
                continue;
              case GlobalDictionaries.EntityPropertyTypes.Decimal:
                Decimal result1;
                if (Decimal.TryParse(property.Value?.ToString(), out result1))
                {
                  property.Value = (object) result1;
                  continue;
                }
                break;
              case GlobalDictionaries.EntityPropertyTypes.DateTime:
                DateTime result2;
                if (DateTime.TryParse(property.Value?.ToString(), out result2))
                {
                  property.Value = (object) result2;
                  continue;
                }
                break;
              default:
                continue;
            }
            property.Value = (object) null;
          }
        }
        foreach (GoodsModifications.GoodModification modification in good.Modifications)
          modification.GoodUid = good.Uid;
        foreach (GoodsSets.Set set in good.SetContent)
          set.ParentUid = good.Uid;
        bool flag1 = good.Properties != null && good.Properties.All<EntityProperties.PropertyValue>(new Func<EntityProperties.PropertyValue, bool>(SavePropertyValue)) && good.Modifications.All<GoodsModifications.GoodModification>(new Func<GoodsModifications.GoodModification, bool>(SaveModification));
        foreach (GoodsSets.Set set in good.SetContent)
          SaveSetContent(set);
        connectionTransaction?.Commit();
        LogHelper.OnEnd();
        return flag1;
      }
      catch (Exception ex)
      {
        connectionTransaction?.Rollback();
        LogHelper.Error(ex, "Ошибка сохранения товара");
        return false;
      }

      bool SaveDocumentEditStock(GoodsStocks.GoodStock stock)
      {
        GOODS_STOCK goodsStock1 = this._db.GetTable<GOODS_STOCK>().FirstOrDefault<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == stock.Uid));
        if (goodsStock1 == null)
          goodsStock1 = new GOODS_STOCK()
          {
            STOCK = 0M,
            UID = Guid.Empty
          };
        GOODS_STOCK goodsStock2 = goodsStock1;
        if (goodsStock2.STOCK == stock.Stock && goodsStock2.IS_DELETED == stock.IsDeleted || goodsStock2.UID == Guid.Empty && stock.IsDeleted)
          return true;
        Document document1 = new Document()
        {
          Type = GlobalDictionaries.DocumentsTypes.UserStockEdit,
          ParentUid = stock.GoodUid,
          Storage = stock.Storage,
          Section = Sections.GetCurrentSection()
        };
        Gbs.Core.Entities.Documents.Item obj1 = new Gbs.Core.Entities.Documents.Item();
        Good good = new Good();
        good.Uid = stock.GoodUid;
        obj1.Good = good;
        obj1.ModificationUid = stock.ModificationUid;
        obj1.DocumentUid = document1.Uid;
        obj1.GoodStock = stock;
        obj1.Quantity = stock.Stock - goodsStock2.STOCK;
        Gbs.Core.Entities.Documents.Item obj2 = obj1;
        document1.Items.Add(obj2);
        if (!goodsStock2.IS_DELETED && stock.IsDeleted)
        {
          obj2.Quantity = -1M * stock.Stock;
          document1.Comment = string.Format(Translate.Good_Save_Удалено__0_, (object) stock.Stock.ToString("N2"));
        }
        else if (goodsStock2.UID == Guid.Empty)
        {
          document1.Comment = string.Format(Translate.Good_Save_Добавлено__0_, (object) obj2.Quantity.ToString("N2"));
        }
        else
        {
          Document document2 = document1;
          string goodSaveИзменено01 = Translate.Good_Save_Изменено__0______1_;
          Decimal stock1 = goodsStock2.STOCK;
          string str1 = stock1.ToString("N2");
          stock1 = stock.Stock;
          string str2 = stock1.ToString("N2");
          string str3 = string.Format(goodSaveИзменено01, (object) str1, (object) str2);
          document2.Comment = str3;
        }
        document1.Comment += string.Format(Translate.Good__0_Цена___1_N2_, (object) "\r\n", (object) stock.Price);
        LogHelper.Trace(string.Format("Сохранение документа ручного редактирования остатка. stock.uid: {0}; doc.comment: {1}", (object) stock.Uid, (object) document1.Comment));
        return new DocumentsRepository(this._db).Save(document1);
      }

      void SaveSetContent(GoodsSets.Set set)
      {
        this._db.InsertOrReplace<GOODS_SETS>(new GOODS_SETS()
        {
          UID = set.Uid == Guid.Empty ? Guid.NewGuid() : set.Uid,
          IS_DELETED = set.IsDeleted,
          PARENT_UID = set.ParentUid,
          GOOD_UID = set.GoodUid,
          DISCOUNT = set.Discount,
          QUANTITY = set.Quantity,
          MODIFICATION_UID = set.ModificationUid
        });
      }

      bool SaveModification(GoodsModifications.GoodModification modification)
      {
        try
        {
          this._db.InsertOrReplace<GOODS_MODIFICATIONS>(new GOODS_MODIFICATIONS()
          {
            UID = modification.Uid == Guid.Empty ? Guid.NewGuid() : modification.Uid,
            BARCODE = modification.Barcode,
            COMMENT = modification.Comment,
            GOOD_UID = modification.GoodUid,
            IS_DELETED = modification.IsDeleted,
            NAME = modification.Name
          });
          return true;
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка сохранения модификации товара");
          return false;
        }
      }

      bool SavePropertyValue(EntityProperties.PropertyValue property)
      {
        try
        {
          if (property.Type.Uid == GlobalDictionaries.GoodIdUid && property.Value == null)
          {
            int num = this._db.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == property.Type.Uid)).Select<ENTITY_PROPERTIES_VALUES, string>((Expression<Func<ENTITY_PROPERTIES_VALUES, string>>) (x => x.CONTENT)).Select<string, int?>((Expression<Func<string, int?>>) (s => Sql.ConvertTo<int?>.From<string>(s))).Max<int?>().GetValueOrDefault() + 1;
            property.Value = (object) num;
          }
          if (property.Value == null || property.Value.ToString().IsNullOrEmpty())
            return true;
          if (property.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          this._db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
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
          LogHelper.Error(ex, "Ошибка сохранения значения доп. поля товара");
          return false;
        }
      }
    }

    public (bool Result, Good Good) SaveCopyGood(Good good, bool isSave = true)
    {
      Good good1 = good.Clone<Good>();
      good1.Uid = Guid.NewGuid();
      good1.DateAdd = DateTime.Now;
      Good good2 = good1;
      GoodGroups.Group group = new GoodGroups.Group();
      group.Uid = good.Group.Uid;
      group.Name = good.Group.Name;
      good2.Group = group;
      good1.StocksAndPrices.Clear();
      good1.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.Uid = Guid.NewGuid()));
      good1.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid)).ToList<EntityProperties.PropertyValue>().ForEach((Action<EntityProperties.PropertyValue>) (x => x.Value = (object) null));
      good1.Modifications.ToList<GoodsModifications.GoodModification>().ForEach((Action<GoodsModifications.GoodModification>) (x => x.Uid = Guid.NewGuid()));
      good1.SetContent.ToList<GoodsSets.Set>().ForEach((Action<GoodsSets.Set>) (x => x.Uid = Guid.NewGuid()));
      try
      {
        string path = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, good.Uid.ToString());
        string str = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, good1.Uid.ToString());
        Directory.CreateDirectory(str);
        if (Directory.Exists(path))
        {
          foreach (string file in Directory.GetFiles(path))
          {
            string extension = Path.GetExtension(file);
            FileSystemHelper.CopyFile(file, Path.Combine(str, DateTime.Now.ToString("ddMMyyyyHHmmssfff") + extension));
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка копирования фото при создании дубля товара", false);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.GoodRepository_Не_удалось_скопировать_изображение_для_нового_товара_));
      }
      return (!isSave || new GoodRepository(this._db).Save(good1), good1);
    }

    public void RemoveGood(List<Good> goods, Guid uid)
    {
      try
      {
        LogHelper.OnBegin();
        string str = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, uid.ToString());
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        foreach (Good good1 in goods)
        {
          Good good = good1;
          good.StocksAndPrices.Clear();
          List<GOODS_STOCK> list1 = this._db.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.GOOD_UID == good.Uid)).ToList<GOODS_STOCK>();
          list1.ForEach((Action<GOODS_STOCK>) (x => x.GOOD_UID = uid));
          list1.ForEach((Action<GOODS_STOCK>) (x => this._db.InsertOrReplace<GOODS_STOCK>(x)));
          List<DOCUMENT_ITEMS> list2 = this._db.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.GOOD_UID == good.Uid)).ToList<DOCUMENT_ITEMS>();
          list2.ForEach((Action<DOCUMENT_ITEMS>) (x => x.GOOD_UID = uid));
          list2.ForEach((Action<DOCUMENT_ITEMS>) (x => this._db.InsertOrReplace<DOCUMENT_ITEMS>(x)));
          List<ACTIONS_HISTORY> list3 = this._db.GetTable<ACTIONS_HISTORY>().Where<ACTIONS_HISTORY>((Expression<Func<ACTIONS_HISTORY, bool>>) (x => x.ENTITY_UID == good.Uid)).ToList<ACTIONS_HISTORY>();
          list3.ForEach((Action<ACTIONS_HISTORY>) (x => x.ENTITY_UID = uid));
          list3.ForEach((Action<ACTIONS_HISTORY>) (x => this._db.InsertOrReplace<ACTIONS_HISTORY>(x)));
          good.IsDeleted = true;
          string path = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, good.Uid.ToString());
          if (Directory.Exists(path) && ((IEnumerable<string>) Directory.GetFiles(path)).Any<string>())
          {
            foreach (string file in Directory.GetFiles(path))
              Functions.ConvertToPng(file, Path.Combine(str, DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png"));
          }
        }
        this.Save(goods);
        LogHelper.OnEnd();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось объеденить товары");
      }
    }

    public bool Delete(Good item) => throw new NotImplementedException();

    public ActionResult Validate(Good good)
    {
      this.CorrectLongBarcode(good);
      ActionResult actionResult1 = ValidationHelper.DataValidation((Entity) good);
      if (actionResult1.Result != ActionResult.Results.Ok)
        return actionResult1;
      foreach (Entity property in good.Properties)
      {
        ActionResult actionResult2 = ValidationHelper.DataValidation(property);
        if (actionResult2.Result != ActionResult.Results.Ok)
          return actionResult2;
      }
      foreach (Entity modification in good.Modifications)
      {
        ActionResult actionResult3 = ValidationHelper.DataValidation(modification);
        if (actionResult3.Result != ActionResult.Results.Ok)
          return actionResult3;
      }
      return new ActionResult(ActionResult.Results.Ok);
    }

    private void CorrectLongBarcode(Good good)
    {
      string barcode = good.Barcode;
      if (barcode.Length < 100)
        return;
      List<string> list = ((IEnumerable<string>) barcode.Split(',', ';', ' ')).ToList<string>();
      if (list.Count > 1)
      {
        good.Barcode = list.First<string>();
        list.Remove(list.First<string>());
        good.Barcodes = good.Barcodes.Concat<string>((IEnumerable<string>) list);
      }
      else
        good.Barcode = good.Barcode.Substring(0, 100);
    }

    public int Save(List<Good> itemsList, bool isWriteJson)
    {
      return itemsList.Count<Good>((Func<Good, bool>) (x => this.Save(x, isWriteJson)));
    }

    public int Save(List<Good> itemsList) => itemsList.Count<Good>(new Func<Good, bool>(this.Save));

    public int Delete(List<Good> itemsList) => throw new NotImplementedException();
  }
}
