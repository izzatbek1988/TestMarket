// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodGroups
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db.Goods;
using Gbs.Helpers.HomeOffice.Entity;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodGroups
  {
    public class Group : Gbs.Core.Entities.Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 2)]
      public string Name { get; set; } = string.Empty;

      public Guid ParentGroupUid { get; set; } = Guid.Empty;

      [Required]
      public GlobalDictionaries.GoodTypes GoodsType { get; set; }

      public Guid UnitsUid { get; set; } = Guid.Empty;

      public bool NeedComment { get; set; }

      public bool IsDataParent { get; set; }

      public bool IsCompositeGood { get; set; }

      public bool IsRequestCount { get; set; }

      public int TaxRateNumber { get; set; }

      [Range(1, 50)]
      public int KkmSectionNumber { get; set; } = 1;

      public bool IsFreePrice { get; set; }

      [Range(0, 10)]
      public int DecimalPlace { get; set; }

      public GlobalDictionaries.RuTaxSystems RuTaxSystem { get; set; } = GlobalDictionaries.RuTaxSystems.None;

      public GlobalDictionaries.RuFfdGoodsTypes RuFfdGoodsType { get; set; }

      public GlobalDictionaries.RuMarkedProductionTypes RuMarkedProductionType { get; set; }

      public Group()
      {
      }

      public Group(GOODS_GROUPS group)
      {
        this.Uid = group.UID;
        this.IsDeleted = group.IS_DELETED;
        this.Name = group.NAME;
        this.IsRequestCount = group.IS_REQUEST_COUNT;
        this.IsDataParent = group.IS_DATA_PARENT;
        this.GoodsType = (GlobalDictionaries.GoodTypes) group.GOODS_TYPE;
        this.UnitsUid = group.UNITS_UID;
        this.ParentGroupUid = group.PARENT_UID;
        this.IsFreePrice = group.IS_FREE_PRICE;
        this.KkmSectionNumber = group.KKM_SECTION_NUMBER;
        this.TaxRateNumber = group.KKM_TAX_NUMBER;
        this.NeedComment = group.NEED_COMMENT;
        this.DecimalPlace = group.DECIMAL_PLACE;
        this.IsCompositeGood = group.IS_COMPOSITE_GOOD;
        this.RuTaxSystem = (GlobalDictionaries.RuTaxSystems) group.RU_TAX_SYSTEM;
        this.RuFfdGoodsType = (GlobalDictionaries.RuFfdGoodsTypes) group.RU_FFD_GOODS_TYPE;
        this.RuMarkedProductionType = (GlobalDictionaries.RuMarkedProductionTypes) group.RU_MARKED_PRODUCTION_TYPE;
      }

      public Group(GoodGroupHome group)
      {
        this.Uid = group.Uid;
        this.IsDeleted = group.IsDeleted;
        this.Name = group.Name;
        this.IsCompositeGood = group.IsCompositeGood;
        this.IsRequestCount = group.IsRequestCount;
        this.IsDataParent = group.IsDataParent;
        this.GoodsType = group.GoodsType;
        this.UnitsUid = group.UnitsUid;
        this.ParentGroupUid = group.ParentGroupUid;
        this.IsFreePrice = group.IsFreePrice;
        this.KkmSectionNumber = group.KkmSectionNumber;
        this.TaxRateNumber = group.TaxRateNumber;
        this.NeedComment = group.NeedComment;
        this.DecimalPlace = group.DecimalPlace;
        this.RuTaxSystem = group.RuTaxSystem;
        this.RuFfdGoodsType = group.RuFfdGoodsType;
        this.RuMarkedProductionType = group.RuMarkedProductionType;
      }
    }
  }
}
