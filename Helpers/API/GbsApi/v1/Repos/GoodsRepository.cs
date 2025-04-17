// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Repos.GoodsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.API.GbsApi.v1.Entities;
using Gbs.Helpers.API.GbsApi.v1.Helpers;
using Gbs.Helpers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Repos
{
  public class GoodsRepository : IRepository
  {
    private List<Gbs.Helpers.API.GbsApi.v1.Entities.Good> GetAllGoods()
    {
      return CachesBox.AllGoods().Select<Gbs.Core.Entities.Goods.Good, Gbs.Helpers.API.GbsApi.v1.Entities.Good>((Func<Gbs.Core.Entities.Goods.Good, Gbs.Helpers.API.GbsApi.v1.Entities.Good>) (x => new Gbs.Helpers.API.GbsApi.v1.Entities.Good(x))).ToList<Gbs.Helpers.API.GbsApi.v1.Entities.Good>();
    }

    public IAnswer GetData(Uri uri)
    {
      Dictionary<string, string> dictionary = uri.DecodeQueryParameters();
      List<Gbs.Helpers.API.GbsApi.v1.Entities.Good> allGoods = this.GetAllGoods();
      string g;
      if (dictionary.Count != 1 || !dictionary.TryGetValue("uid", out g))
        return (IAnswer) new ListOfObjectsAnswer(PagingHelper.GetPage((IEnumerable<IEntity>) allGoods, uri));
      Guid uid = new Guid(g);
      return (IAnswer) new SingleObjectAnswer()
      {
        Status = AnswerStatuses.Ok,
        Data = (IEntity) allGoods.Single<Gbs.Helpers.API.GbsApi.v1.Entities.Good>((Func<Gbs.Helpers.API.GbsApi.v1.Entities.Good, bool>) (x => x.Uid == uid))
      };
    }
  }
}
