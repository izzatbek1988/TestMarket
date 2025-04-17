// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.EgaisWriteOffActRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers.Egais;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class EgaisWriteOffActRepository : IEntityRepository<EgaisWriteOffAct, EGAIS_WRITEOFFACTS>
  {
    public List<EgaisWriteOffAct> GetByQuery(IQueryable<EGAIS_WRITEOFFACTS> query)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<EGAIS_WRITEOFFACTS>();
        List<EgaisWriteOffActsItems> items = new EgaisWriteOffActsItemRepository().GetActiveItems();
        return query.ToList<EGAIS_WRITEOFFACTS>().Select<EGAIS_WRITEOFFACTS, EgaisWriteOffAct>((Func<EGAIS_WRITEOFFACTS, EgaisWriteOffAct>) (s => new EgaisWriteOffAct()
        {
          Uid = s.UID,
          IsDeleted = s.IS_DELETED,
          DateTime = s.DATE_TIME,
          Status = (EgaisWriteOffActStatus) s.STATUS,
          Type = (TypeWriteOff1) s.TYPE,
          ReplayUid = s.REPLAY_UID,
          Items = items.Where<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, bool>) (x => x.ActUid == s.UID)).ToList<EgaisWriteOffActsItems>(),
          Comment = s.COMMENT
        })).ToList<EgaisWriteOffAct>();
      }
    }

    public List<EgaisWriteOffAct> GetAllItems()
    {
      return this.GetByQuery((IQueryable<EGAIS_WRITEOFFACTS>) null);
    }

    public List<EgaisWriteOffAct> GetActiveItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<EGAIS_WRITEOFFACTS>().Where<EGAIS_WRITEOFFACTS>((Expression<Func<EGAIS_WRITEOFFACTS, bool>>) (x => x.IS_DELETED == false)));
    }

    public EgaisWriteOffAct GetByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<EGAIS_WRITEOFFACTS>().Where<EGAIS_WRITEOFFACTS>((Expression<Func<EGAIS_WRITEOFFACTS, bool>>) (x => x.UID == uid))).SingleOrDefault<EgaisWriteOffAct>();
    }

    public bool Save(EgaisWriteOffAct item)
    {
      Data.GetDataContext().InsertOrReplace<EGAIS_WRITEOFFACTS>(new EGAIS_WRITEOFFACTS()
      {
        UID = item.Uid,
        DATE_TIME = item.DateTime,
        IS_DELETED = item.IsDeleted,
        REPLAY_UID = item.ReplayUid,
        STATUS = (int) item.Status,
        COMMENT = item.Comment,
        TYPE = (int) item.Type
      });
      return true;
    }

    public int Save(List<EgaisWriteOffAct> itemsList)
    {
      return itemsList.Count<EgaisWriteOffAct>(new Func<EgaisWriteOffAct, bool>(this.Save));
    }

    public int Delete(List<EgaisWriteOffAct> itemsList) => throw new NotImplementedException();

    public bool Delete(EgaisWriteOffAct item) => throw new NotImplementedException();

    public ActionResult Validate(EgaisWriteOffAct item)
    {
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
