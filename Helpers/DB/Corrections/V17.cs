// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V17
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  internal class V17 : ICorrection
  {
    public bool Do()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      V17.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new V17.\u003C\u003Ec__DisplayClass0_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass00.db = Data.GetDataBase();
      try
      {
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: field reference
        // ISSUE: method reference
        List<IGrouping<DOCUMENTS, Guid>> list = cDisplayClass00.db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME >= new DateTime(2023, 12, 19).Date && x.DATE_TIME < new DateTime(2023, 12, 27).Date && x.TYPE == 3 && x.STATUS == 2 && x.IS_DELETED == false)).SelectMany(Expression.Lambda<Func<DOCUMENTS, IEnumerable<DOCUMENT_ITEMS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass00.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((Expression) Expression.AndAlso((Expression) Expression.AndAlso((Expression) Expression.Equal(x.DOCUMENT_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (Expression) Expression.Not((Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_IS_DELETED))))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_STOCK_UID))), (Expression) Expression.Field((Expression) null, FieldInfo.GetFieldFromHandle(__fieldref (Guid.Empty))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression2))
        }), parameterExpression1), (doc, docItem) => new
        {
          doc = doc,
          docItem = docItem
        }).GroupBy(data => data.doc, data => data.doc.UID).ToList<IGrouping<DOCUMENTS, Guid>>();
        // ISSUE: reference to a compiler-generated field
        DocumentsRepository documentsRepository = new DocumentsRepository(cDisplayClass00.db);
        foreach (IGrouping<DOCUMENTS, Guid> grouping in list)
        {
          DOCUMENTS key = grouping.Key;
          key.STATUS = 1;
          // ISSUE: reference to a compiler-generated field
          cDisplayClass00.db.InsertOrReplace<DOCUMENTS>(key);
          Document byUid = documentsRepository.GetByUid(key.UID);
          byUid.Status = GlobalDictionaries.DocumentsStatuses.Close;
          documentsRepository.Save(byUid);
        }
        if (list.Any<IGrouping<DOCUMENTS, Guid>>())
          MessageBoxHelper.Warning(string.Format(Translate.V17_Do_, (object) list.Count) + string.Join('\n'.ToString(), list.Select<IGrouping<DOCUMENTS, Guid>, string>((Func<IGrouping<DOCUMENTS, Guid>, string>) (x => string.Format("#{0} от {1:dd MMMM yyyy}", (object) x.Key.NUMBER, (object) x.Key.DATE_TIME.Date)))));
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
