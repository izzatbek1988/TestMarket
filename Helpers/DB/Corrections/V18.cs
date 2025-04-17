// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V18
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  internal class V18 : ICorrection
  {
    public bool Do()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<DOCUMENTS> table = dataBase.GetTable<DOCUMENTS>();
        Expression<Func<DOCUMENTS, bool>> predicate = (Expression<Func<DOCUMENTS, bool>>) (x => x.STATUS == 3 && x.TYPE == 3);
        foreach (DOCUMENTS documents in table.Where<DOCUMENTS>(predicate).ToList<DOCUMENTS>())
        {
          documents.STATUS = 2;
          dataBase.InsertOrReplace<DOCUMENTS>(documents);
        }
        return true;
      }
    }
  }
}
