// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.DataBaseConnection
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using LinqToDB.Common;
using LinqToDB.Data;
using System;
using System.Linq.Expressions;
using System.Text;

#nullable disable
namespace Gbs.Core.Db
{
  public class DataBaseConnection : DataConnection
  {
    public DataBaseConnection()
      : base("Firebird", DataBaseHelper.DbConnectionString)
    {
    }

    private static void ConvertBooleanToSql(StringBuilder stringBuilder, bool value)
    {
      stringBuilder.Append(value.ToString() ?? "");
    }

    public class BoolToIntConverter : ValueConverter<bool, int>
    {
      public BoolToIntConverter()
        : base((Expression<Func<bool, int>>) (b => b ? 1 : 0), (Expression<Func<int, bool>>) (i => i == 1), false)
      {
      }
    }
  }
}
