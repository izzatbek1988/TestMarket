// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V9
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V9 : ICorrection
  {
    public bool Do()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      V9.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new V9.\u003C\u003Ec__DisplayClass0_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass00.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        IQueryable<ENTITY_PROPERTIES_TYPES> source = cDisplayClass00.db.GetTable<ENTITY_PROPERTIES_TYPES>().Where<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => x.TYPE == 3));
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        Expression<Func<ENTITY_PROPERTIES_TYPES, IEnumerable<ENTITY_PROPERTIES_VALUES>>> collectionSelector = Expression.Lambda<Func<ENTITY_PROPERTIES_TYPES, IEnumerable<ENTITY_PROPERTIES_VALUES>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass00.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<ENTITY_PROPERTIES_VALUES, bool>>((Expression) Expression.Equal(x.TYPE_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ENTITY_PROPERTIES_TYPES.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
        }), parameterExpression1);
        Expression<Func<ENTITY_PROPERTIES_TYPES, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>> resultSelector = (Expression<Func<ENTITY_PROPERTIES_TYPES, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>>) ((type, value) => value);
        foreach (ENTITY_PROPERTIES_VALUES propertiesValues in source.SelectMany<ENTITY_PROPERTIES_TYPES, ENTITY_PROPERTIES_VALUES, ENTITY_PROPERTIES_VALUES>(collectionSelector, resultSelector).ToList<ENTITY_PROPERTIES_VALUES>())
        {
          try
          {
            JsonConvert.DeserializeObject<DateTime>(propertiesValues.CONTENT);
          }
          catch
          {
            string str = propertiesValues.CONTENT.Replace("\"", "");
            try
            {
              DateTime dateTime = Convert.ToDateTime(str);
              // ISSUE: reference to a compiler-generated field
              cDisplayClass00.db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
              {
                UID = propertiesValues.UID,
                ENTITY_UID = propertiesValues.ENTITY_UID,
                IS_DELETED = propertiesValues.IS_DELETED,
                TYPE_UID = propertiesValues.TYPE_UID,
                CONTENT = JsonConvert.ToString(dateTime)
              });
            }
            catch (Exception ex)
            {
              LogHelper.Error(ex, "Не удалось скорректировать дату");
              return false;
            }
          }
        }
        return true;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass00.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass00.db.Dispose();
        }
      }
    }
  }
}
