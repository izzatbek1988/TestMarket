// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.YmlHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Helpers
{
  public class YmlHelper
  {
    private static Dictionary<Guid, int> _idGroupDictonary = new Dictionary<Guid, int>();

    public static YmlCatalog ConvertToYmlCatalog(List<ExchangeDataHelper.Good> goods)
    {
      YmlHelper._idGroupDictonary.Clear();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodGroups.Group> allItems = new GoodGroupsRepository(dataBase).GetAllItems();
        List<EntityProperties.PropertyValue> valuesList = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.GoodGroup, dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.GroupGoodIdUid && x.IS_DELETED == false)));
        YmlCatalog ymlCatalog = new YmlCatalog();
        SalePoints.SalePoint salePoint = SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
        ymlCatalog.Shop.Name = salePoint.Description.NamePoint;
        ymlCatalog.Shop.Company = salePoint.Organization.Name;
        ymlCatalog.Shop.Email = salePoint.Organization.Email;
        ymlCatalog.Date = DateTime.Now;
        foreach (GoodGroups.Group group in allItems)
        {
          GoodGroups.Group gr = group;
          IEnumerable<EntityProperties.PropertyValue> source = valuesList.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.EntityUid == gr.Uid));
          int num;
          if (!source.Any<EntityProperties.PropertyValue>())
          {
            num = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.GroupGoodIdUid)).Select<ENTITY_PROPERTIES_VALUES, string>((Expression<Func<ENTITY_PROPERTIES_VALUES, string>>) (x => x.CONTENT)).Select<string, int?>((Expression<Func<string, int?>>) (s => Sql.ConvertTo<int?>.From<string>(s))).Max<int?>().GetValueOrDefault() + 1;
            dataBase.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
            {
              ENTITY_UID = gr.Uid,
              TYPE_UID = GlobalDictionaries.GroupGoodIdUid,
              CONTENT = JsonConvert.ToString(num)
            });
          }
          else
            num = Convert.ToInt32(source.First<EntityProperties.PropertyValue>().Value);
          YmlHelper._idGroupDictonary.Add(gr.Uid, num);
        }
        foreach (GoodGroups.Group group in allItems)
        {
          GoodGroups.Group gr = group;
          Category category1 = new Category();
          category1.Text = gr.Name;
          KeyValuePair<Guid, int> keyValuePair = YmlHelper._idGroupDictonary.Single<KeyValuePair<Guid, int>>((Func<KeyValuePair<Guid, int>, bool>) (x => x.Key == gr.Uid));
          category1.Id = keyValuePair.Value;
          Category category2 = category1;
          if (gr.ParentGroupUid != Guid.Empty)
          {
            Category category3 = category2;
            keyValuePair = YmlHelper._idGroupDictonary.Single<KeyValuePair<Guid, int>>((Func<KeyValuePair<Guid, int>, bool>) (x => x.Key == gr.ParentGroupUid));
            int num = keyValuePair.Value;
            category3.ParentId = num;
          }
          ymlCatalog.Shop.Categories.Category.Add(category2);
        }
        foreach (ExchangeDataHelper.Good good1 in goods)
        {
          ExchangeDataHelper.Good good = good1;
          ymlCatalog.Shop.Offers.Offer.Add(new Offer()
          {
            Id = good.Id,
            Name = good.Name,
            Barcode = good.Barcode,
            CategoryId = YmlHelper._idGroupDictonary.Single<KeyValuePair<Guid, int>>((Func<KeyValuePair<Guid, int>, bool>) (x => x.Key == good.GroupUid)).Value,
            Description = good.Description,
            Price = good.MaxPrice,
            Count = good.TotalStock,
            Param = new List<Param>(good.PropertiesList.Where<ExchangeDataHelper.Good.Properties>((Func<ExchangeDataHelper.Good.Properties, bool>) (x =>
            {
              object obj = x.Value;
              if (obj == null)
                return false;
              string str = obj.ToString();
              bool? nullable = str != null ? new bool?(str.IsNullOrEmpty()) : new bool?();
              bool flag = false;
              return nullable.GetValueOrDefault() == flag & nullable.HasValue;
            })).Select<ExchangeDataHelper.Good.Properties, Param>((Func<ExchangeDataHelper.Good.Properties, Param>) (x => new Param(x.Name, x.Value))))
          });
        }
        return ymlCatalog;
      }
    }
  }
}
