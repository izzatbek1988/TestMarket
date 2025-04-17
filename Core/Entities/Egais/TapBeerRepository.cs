// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.TapBeerRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class TapBeerRepository : IEntityRepository<TapBeer, TAP_BEER>
  {
    public List<TapBeer> GetByQuery(IQueryable<TAP_BEER> query)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<TAP_BEER>();
        return query.ToList<TAP_BEER>().Select<TAP_BEER, TapBeer>((Func<TAP_BEER, TapBeer>) (s =>
        {
          return new TapBeer()
          {
            Uid = s.UID,
            IsDeleted = s.IS_DELETED,
            Name = s.NAME,
            Index = s.ROW_INDEX
          };
        })).ToList<TapBeer>();
      }
    }

    public List<TapBeer> GetAllItems() => this.GetByQuery((IQueryable<TAP_BEER>) null);

    public List<TapBeer> GetActiveItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<TAP_BEER>().Where<TAP_BEER>((Expression<Func<TAP_BEER, bool>>) (x => x.IS_DELETED == false)));
    }

    public TapBeer GetByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<TAP_BEER>().Where<TAP_BEER>((Expression<Func<TAP_BEER, bool>>) (x => x.UID == uid))).SingleOrDefault<TapBeer>();
    }

    public bool Save(TapBeer item)
    {
      Data.GetDataContext().InsertOrReplace<TAP_BEER>(new TAP_BEER()
      {
        UID = item.Uid,
        IS_DELETED = item.IsDeleted,
        NAME = item.Name,
        ROW_INDEX = item.Index
      });
      return true;
    }

    public int Save(List<TapBeer> itemsList)
    {
      return itemsList.Count<TapBeer>(new Func<TapBeer, bool>(this.Save));
    }

    public int Delete(List<TapBeer> itemsList) => throw new NotImplementedException();

    public bool Delete(TapBeer item) => throw new NotImplementedException();

    public ActionResult Validate(TapBeer item)
    {
      return item.Name.IsNullOrEmpty() ? new ActionResult(ActionResult.Results.Error, "Не указано название пивного крана в системе.") : new ActionResult(ActionResult.Results.Ok);
    }
  }
}
