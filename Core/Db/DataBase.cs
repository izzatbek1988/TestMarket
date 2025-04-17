// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.DataBase
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FirebirdSql.Data.FirebirdClient;
using Gbs.Helpers.Logging;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Core.Db
{
  public class DataBase : IDisposable
  {
    public readonly DataBaseConnection Connection;

    public FbConnection FbConnection => (FbConnection) this.Connection.Connection;

    public DataBase() => this.Connection = new DataBaseConnection();

    public IQueryable<T> GetTable<T>() where T : DbTable
    {
      return (IQueryable<T>) this.Connection.GetTable<T>();
    }

    public int InsertOrReplace<T>(T item) where T : DbTable
    {
      return this.Connection.InsertOrReplace<T>(item);
    }

    public DataConnectionTransaction BeginTransaction()
    {
      if (this.Connection.Transaction == null)
        return this.Connection.BeginTransaction();
      LogHelper.Trace("Транзакция уже существует");
      return (DataConnectionTransaction) null;
    }

    public void Dispose() => this.Connection?.Dispose();
  }
}
