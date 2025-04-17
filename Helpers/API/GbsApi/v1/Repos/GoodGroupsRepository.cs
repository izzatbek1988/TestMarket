// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Repos.GoodGroupsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers.API.GbsApi.v1.Entities;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Repos
{
  public class GoodGroupsRepository : IRepository
  {
    private GoodGroup GetGroupByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return new GoodGroup(new Gbs.Core.Entities.Goods.GoodGroupsRepository(dataBase).GetByUid(uid));
    }

    private List<GoodGroup> GetAllGroups()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return new Gbs.Core.Entities.Goods.GoodGroupsRepository(dataBase).GetAllItems().Select<GoodGroups.Group, GoodGroup>((Func<GoodGroups.Group, GoodGroup>) (x => new GoodGroup(x))).ToList<GoodGroup>();
    }

    public IAnswer GetData(Uri uri)
    {
      Dictionary<string, string> source = uri.DecodeQueryParameters();
      if (!source.Any<KeyValuePair<string, string>>())
      {
        List<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity> list = this.GetAllGroups().Cast<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity>().ToList<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity>();
        return (IAnswer) new ListOfObjectsAnswer()
        {
          Status = AnswerStatuses.Ok,
          Data = list,
          TotalItems = list.Count
        };
      }
      string g;
      if (source.Count != 1 || !source.TryGetValue("uid", out g))
        throw new InvalidOperationException(Translate.GoodGroupsRepository_GetData_Не_удалось_разобрать_запрос);
      Guid uid = new Guid(g);
      return (IAnswer) new SingleObjectAnswer()
      {
        Status = AnswerStatuses.Ok,
        Data = (Gbs.Helpers.API.GbsApi.v1.Entities.IEntity) this.GetGroupByUid(uid)
      };
    }
  }
}
