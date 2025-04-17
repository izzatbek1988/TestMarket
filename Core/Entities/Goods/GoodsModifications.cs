// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsModifications
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsModifications
  {
    public static List<GoodsModifications.GoodModification> GetModificationsList(
      IQueryable<GOODS_MODIFICATIONS> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<GOODS_MODIFICATIONS>();
        return query.ToList<GOODS_MODIFICATIONS>().Select<GOODS_MODIFICATIONS, GoodsModifications.GoodModification>((Func<GOODS_MODIFICATIONS, GoodsModifications.GoodModification>) (m =>
        {
          return new GoodsModifications.GoodModification()
          {
            Uid = m.UID,
            IsDeleted = m.IS_DELETED,
            Name = m.NAME,
            GoodUid = m.GOOD_UID,
            Barcode = m.BARCODE,
            Comment = m.COMMENT
          };
        })).ToList<GoodsModifications.GoodModification>();
      }
    }

    public class GoodModification : Entity
    {
      public Guid GoodUid { get; set; } = Guid.Empty;

      public string Name { get; set; } = string.Empty;

      public string Barcode { get; set; } = string.Empty;

      public string Comment { get; set; } = string.Empty;

      public GoodModification()
      {
      }

      public GoodModification(GOODS_MODIFICATIONS dbItem)
      {
        this.Uid = dbItem.UID;
        this.GoodUid = dbItem.GOOD_UID;
        this.Name = dbItem.NAME;
        this.Barcode = dbItem.BARCODE;
        this.Comment = dbItem.COMMENT;
        this.IsDeleted = dbItem.IS_DELETED;
      }
    }
  }
}
