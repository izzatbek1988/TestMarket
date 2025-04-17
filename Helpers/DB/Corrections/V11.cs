// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V11
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V11 : ICorrection
  {
    public bool Do()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      V11.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new V11.\u003C\u003Ec__DisplayClass0_0();
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<DOCUMENTS> queryable = dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 11));
        // ISSUE: reference to a compiler-generated field
        cDisplayClass00.orders = queryable;
        IQueryable<DOCUMENTS> table = dataBase.GetTable<DOCUMENTS>();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        Expression<Func<DOCUMENTS, bool>> predicate = (Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 1 && cDisplayClass00.orders.Any<DOCUMENTS>(Expression.Lambda<Func<DOCUMENTS, bool>>((Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_PARENT_UID))), (Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)));
        foreach (DOCUMENTS documents1 in table.Where<DOCUMENTS>(predicate).ToList<DOCUMENTS>())
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          V11.\u003C\u003Ec__DisplayClass0_1 cDisplayClass01 = new V11.\u003C\u003Ec__DisplayClass0_1();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass01.document = documents1;
          // ISSUE: reference to a compiler-generated field
          DOCUMENTS document = cDisplayClass01.document;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DOCUMENTS documents2 = cDisplayClass00.orders.SingleOrDefault<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == cDisplayClass01.document.PARENT_UID));
          // ISSUE: reference to a compiler-generated field
          int num = documents2 != null ? (documents2.IS_FISCAL ? 1 : 0) : (cDisplayClass01.document.IS_FISCAL ? 1 : 0);
          document.IS_FISCAL = num != 0;
          // ISSUE: reference to a compiler-generated field
          dataBase.InsertOrReplace<DOCUMENTS>(cDisplayClass01.document);
        }
        return true;
      }
    }
  }
}
