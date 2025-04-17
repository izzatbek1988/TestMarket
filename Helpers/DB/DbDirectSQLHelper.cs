// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.DbDirectSQLHelper
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
namespace Gbs.Helpers.DB
{
  public static class DbDirectSQLHelper
  {
    public static int GetDbDocsCount(Guid goodUid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (di => di.GOOD_UID == goodUid && di.IS_DELETED == false)).Select<DOCUMENT_ITEMS, Guid>((Expression<Func<DOCUMENT_ITEMS, Guid>>) (di => di.DOCUMENT_UID)).Distinct<Guid>().Count<Guid>();
    }
  }
}
