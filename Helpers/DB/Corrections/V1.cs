// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Helpers.Logging;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V1 : ICorrection
  {
    private bool GeneratedIdForGood()
    {
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        V1.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new V1.\u003C\u003Ec__DisplayClass0_0();
        LogHelper.Debug("Корректировка БД: присвоение ID для товаров");
        using (DataBase dataBase = Data.GetDataBase())
        {
          IQueryable<ENTITY_PROPERTIES_VALUES> queryable = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => !x.IS_DELETED && x.TYPE_UID == GlobalDictionaries.GoodIdUid));
          // ISSUE: reference to a compiler-generated field
          cDisplayClass00.prop = queryable;
          IQueryable<GOODS> table = dataBase.GetTable<GOODS>();
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method reference
          // ISSUE: method reference
          Expression<Func<GOODS, bool>> predicate = (Expression<Func<GOODS, bool>>) (x => cDisplayClass00.prop.All<ENTITY_PROPERTIES_VALUES>(Expression.Lambda<Func<ENTITY_PROPERTIES_VALUES, bool>>((Expression) Expression.NotEqual(p.ENTITY_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Inequality))), parameterExpression2)) && !x.IS_DELETED);
          foreach (GOODS goods in table.Where<GOODS>(predicate).ToList<GOODS>())
          {
            int num = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.GoodIdUid)).Select<ENTITY_PROPERTIES_VALUES, string>((Expression<Func<ENTITY_PROPERTIES_VALUES, string>>) (x => x.CONTENT)).Select<string, int?>((Expression<Func<string, int?>>) (s => Sql.ConvertTo<int?>.From<string>(s))).Max<int?>().GetValueOrDefault() + 1;
            dataBase.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
            {
              UID = Guid.NewGuid(),
              ENTITY_UID = goods.UID,
              TYPE_UID = GlobalDictionaries.GoodIdUid,
              CONTENT = JsonConvert.ToString(num)
            });
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при генерации ID для товаров", false);
        return false;
      }
    }

    private bool FixedIdForGood()
    {
      try
      {
        LogHelper.Debug("Корректировка БД: исправление ID с кавычками для товаров");
        using (DataBase dataBase = Data.GetDataBase())
        {
          IQueryable<ENTITY_PROPERTIES_VALUES> table = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>();
          Expression<Func<ENTITY_PROPERTIES_VALUES, bool>> predicate = (Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => !x.IS_DELETED && x.TYPE_UID == GlobalDictionaries.GoodIdUid && x.CONTENT.Contains("\""));
          foreach (ENTITY_PROPERTIES_VALUES propertiesValues in table.Where<ENTITY_PROPERTIES_VALUES>(predicate).ToList<ENTITY_PROPERTIES_VALUES>())
          {
            int result;
            if (int.TryParse(string.Join<char>("", propertiesValues.CONTENT.Where<char>(new Func<char, bool>(char.IsDigit))), out result))
              dataBase.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
              {
                UID = propertiesValues.UID,
                ENTITY_UID = propertiesValues.ENTITY_UID,
                TYPE_UID = propertiesValues.TYPE_UID,
                IS_DELETED = propertiesValues.IS_DELETED,
                CONTENT = JsonConvert.ToString(result)
              });
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при исправленнии ID с кавычками", false);
        return false;
      }
    }

    private bool GeneratedDbUid()
    {
      try
      {
        LogHelper.Debug("Корректировка БД: генерация ID для базы данных");
        if (!(UidDb.GetUid().EntityUid == Guid.Empty))
          return true;
        Guid uid = Guid.NewGuid();
        return UidDb.SetUid(uid, uid.ToString());
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при генерации ID базы данных", false);
        return false;
      }
    }

    public bool Do()
    {
      LogHelper.Debug("Корректировка БД в.1");
      return this.FixedIdForGood() && this.GeneratedIdForGood() && this.GeneratedDbUid();
    }
  }
}
