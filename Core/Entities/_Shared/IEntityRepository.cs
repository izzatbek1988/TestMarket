// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.IEntityRepository`2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public interface IEntityRepository<TEntity, in TTable>
    where TEntity : IEntity
    where TTable : DbTable
  {
    List<TEntity> GetByQuery(IQueryable<TTable> query);

    List<TEntity> GetAllItems();

    List<TEntity> GetActiveItems();

    TEntity GetByUid(Guid uid);

    bool Save(TEntity item);

    int Save(List<TEntity> itemsList);

    int Delete(List<TEntity> itemsList);

    bool Delete(TEntity item);

    ActionResult Validate(TEntity item);
  }
}
