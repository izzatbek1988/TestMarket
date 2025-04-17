// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.EgaisWriteOffActsItemRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Tables.Egais;
using Gbs.Helpers.Egais;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class EgaisWriteOffActsItemRepository : 
    IEntityRepository<EgaisWriteOffActsItems, EGAIS_WRITEOFFACTS_ITEMS>
  {
    public List<EgaisWriteOffActsItems> GetByQuery(IQueryable<EGAIS_WRITEOFFACTS_ITEMS> query)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<EGAIS_WRITEOFFACTS_ITEMS>();
        return query.ToList<EGAIS_WRITEOFFACTS_ITEMS>().Select<EGAIS_WRITEOFFACTS_ITEMS, EgaisWriteOffActsItems>((Func<EGAIS_WRITEOFFACTS_ITEMS, EgaisWriteOffActsItems>) (s => new EgaisWriteOffActsItems()
        {
          Uid = s.UID,
          ActUid = s.ACT_UID,
          Quantity = s.QUANTITY,
          Sum = s.SUM_ITEM,
          FbNumber = s.FB_NUMBER,
          IsDeleted = s.IS_DELETED,
          MarkInfo = s.MARK_INFO,
          StockUid = s.STOCK_UID,
          ActType = (TypeWriteOff1) s.TYPE
        })).ToList<EgaisWriteOffActsItems>();
      }
    }

    public List<EgaisWriteOffActsItems> GetAllItems()
    {
      return this.GetByQuery((IQueryable<EGAIS_WRITEOFFACTS_ITEMS>) null);
    }

    public List<EgaisWriteOffActsItems> GetActiveItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<EGAIS_WRITEOFFACTS_ITEMS>().Where<EGAIS_WRITEOFFACTS_ITEMS>((Expression<Func<EGAIS_WRITEOFFACTS_ITEMS, bool>>) (x => x.IS_DELETED == false)));
    }

    public EgaisWriteOffActsItems GetByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<EGAIS_WRITEOFFACTS_ITEMS>().Where<EGAIS_WRITEOFFACTS_ITEMS>((Expression<Func<EGAIS_WRITEOFFACTS_ITEMS, bool>>) (x => x.UID == uid))).SingleOrDefault<EgaisWriteOffActsItems>();
    }

    public bool Save(EgaisWriteOffActsItems item)
    {
      Data.GetDataContext().InsertOrReplace<EGAIS_WRITEOFFACTS_ITEMS>(new EGAIS_WRITEOFFACTS_ITEMS()
      {
        ACT_UID = item.ActUid,
        FB_NUMBER = item.FbNumber,
        IS_DELETED = item.IsDeleted,
        MARK_INFO = item.MarkInfo,
        QUANTITY = item.Quantity,
        STOCK_UID = item.StockUid,
        SUM_ITEM = item.Sum,
        UID = item.Uid,
        TYPE = (int) item.ActType
      });
      return true;
    }

    public int Save(List<EgaisWriteOffActsItems> itemsList)
    {
      return itemsList.Count<EgaisWriteOffActsItems>(new Func<EgaisWriteOffActsItems, bool>(this.Save));
    }

    public int Delete(List<EgaisWriteOffActsItems> itemsList)
    {
      throw new NotImplementedException();
    }

    public bool Delete(EgaisWriteOffActsItems item) => throw new NotImplementedException();

    public ActionResult Validate(EgaisWriteOffActsItems item)
    {
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
