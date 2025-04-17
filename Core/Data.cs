// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Data
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using LinqToDB;
using LinqToDB.Linq;
using LinqToDB.SqlQuery;
using System;
using System.Linq;
using System.Text;

#nullable enable
namespace Gbs.Core
{
  public static class Data
  {
    public static 
    #nullable disable
    DataBase GetDataBase() => new DataBase();

    public static DataContext GetDataContext()
    {
      DataContext dataContext = new DataContext("Firebird", DataBaseHelper.DbConnectionString);
      dataContext.MappingSchema.SetValueToSqlConverter(typeof (bool), (Action<StringBuilder, SqlDataType, object>) ((builder, type, value) => Data.ConvertBooleanToSql(builder, (bool) value)));
      return dataContext;
    }

    private static void ConvertBooleanToSql(StringBuilder stringBuilder, bool value)
    {
      stringBuilder.Append(value.ToString() ?? "");
    }

    public static IQueryable<T> GetQuery<T>(this DataContext dc, IQueryable<T> query)
    {
      return Internals.CreateExpressionQueryInstance<T>((IDataContext) dc, query.Expression);
    }
  }
}
