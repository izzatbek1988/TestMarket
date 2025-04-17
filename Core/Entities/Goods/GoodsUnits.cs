// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsUnits
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsUnits
  {
    public static IEnumerable<GoodsUnits.GoodUnit> GetUnitsListWithFilter(
      IQueryable<GOODS_UNITS> query = null,
      bool isDeletedLoad = true)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GOODS_UNITS> list = dataBase.GetTable<GOODS_UNITS>().ToList<GOODS_UNITS>();
        if (!isDeletedLoad)
          list = list.Where<GOODS_UNITS>((Func<GOODS_UNITS, bool>) (x => !x.IS_DELETED)).ToList<GOODS_UNITS>();
        if (query != null)
          list = query.ToList<GOODS_UNITS>();
        return (IEnumerable<GoodsUnits.GoodUnit>) list.Select<GOODS_UNITS, GoodsUnits.GoodUnit>((Func<GOODS_UNITS, GoodsUnits.GoodUnit>) (group =>
        {
          return new GoodsUnits.GoodUnit()
          {
            Uid = group.UID,
            FullName = group.FULL_NAME,
            ShortName = group.SHORT_NAME,
            Code = group.CODE,
            IsDeleted = group.IS_DELETED,
            RuFfdUnitsIndex = group.RU_FFD_UNITS_INDEX
          };
        })).ToList<GoodsUnits.GoodUnit>();
      }
    }

    public static GoodsUnits.GoodUnit GetByUid(Guid uid)
    {
      if (uid == Guid.Empty)
        return (GoodsUnits.GoodUnit) null;
      using (DataBase dataBase = Data.GetDataBase())
        return GoodsUnits.GetUnitsListWithFilter(dataBase.GetTable<GOODS_UNITS>().Where<GOODS_UNITS>((Expression<Func<GOODS_UNITS, bool>>) (x => x.UID == uid))).FirstOrDefault<GoodsUnits.GoodUnit>();
    }

    public class GoodUnit : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string FullName { get; set; } = string.Empty;

      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string ShortName { get; set; } = string.Empty;

      [StringLength(100)]
      public string Code { get; set; } = string.Empty;

      [Required]
      public int RuFfdUnitsIndex { get; set; }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase = Data.GetDataBase())
          dataBase.InsertOrReplace<GOODS_UNITS>(new GOODS_UNITS()
          {
            UID = this.Uid,
            CODE = this.Code,
            FULL_NAME = this.FullName,
            SHORT_NAME = this.ShortName,
            IS_DELETED = this.IsDeleted,
            RU_FFD_UNITS_INDEX = this.RuFfdUnitsIndex
          });
        return true;
      }
    }
  }
}
