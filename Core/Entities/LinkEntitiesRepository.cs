// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.LinkEntitiesRepository
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
namespace Gbs.Core.Entities
{
  public class LinkEntitiesRepository : IRepository<LinkEntities>
  {
    public int Delete(List<LinkEntities> itemsList) => throw new NotImplementedException();

    public bool Delete(LinkEntities item)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        dataBase.GetTable<LINK_ENTITIES>().Where<LINK_ENTITIES>((Expression<Func<LINK_ENTITIES, bool>>) (x => x.ENTITY_UID == item.EntityUid)).Delete<LINK_ENTITIES>();
        return true;
      }
    }

    public List<LinkEntities> GetActiveItems()
    {
      return this.GetAllItems().Where<LinkEntities>((Func<LinkEntities, bool>) (x => !x.IsDeleted)).ToList<LinkEntities>();
    }

    public List<LinkEntities> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return dataBase.GetTable<LINK_ENTITIES>().AsParallel<LINK_ENTITIES>().Select<LINK_ENTITIES, LinkEntities>((Func<LINK_ENTITIES, LinkEntities>) (linq =>
        {
          return new LinkEntities()
          {
            Uid = linq.UID,
            EntityUid = linq.ENTITY_UID,
            Type = (TypeEntity) linq.TYPE,
            Id = linq.ID
          };
        })).ToList<LinkEntities>();
    }

    public LinkEntities GetByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<LINK_ENTITIES> source = dataBase.GetTable<LINK_ENTITIES>().Where<LINK_ENTITIES>((Expression<Func<LINK_ENTITIES, bool>>) (x => x.ENTITY_UID == uid));
        LinkEntities byUid;
        if (!source.Any<LINK_ENTITIES>())
        {
          byUid = (LinkEntities) null;
        }
        else
        {
          byUid = new LinkEntities();
          byUid.Uid = source.First<LINK_ENTITIES>().UID;
          byUid.Type = (TypeEntity) source.First<LINK_ENTITIES>().TYPE;
          byUid.Id = source.First<LINK_ENTITIES>().ID;
          byUid.EntityUid = source.First<LINK_ENTITIES>().ENTITY_UID;
        }
        return byUid;
      }
    }

    public bool Save(LinkEntities item)
    {
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      using (DataBase dataBase = Data.GetDataBase())
      {
        dataBase.InsertOrReplace<LINK_ENTITIES>(new LINK_ENTITIES()
        {
          UID = item.Uid,
          ENTITY_UID = item.EntityUid,
          ID = item.Id,
          TYPE = (int) item.Type
        });
        return true;
      }
    }

    public int Save(List<LinkEntities> itemsList)
    {
      return itemsList.Count<LinkEntities>(new Func<LinkEntities, bool>(this.Save));
    }

    public ActionResult Validate(LinkEntities item) => new ActionResult(ActionResult.Results.Ok);
  }
}
