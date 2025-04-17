// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodGroupHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodGroupHome
  {
    public Guid Uid { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsCompositeGood { get; set; }

    public string Name { get; set; }

    public Guid ParentGroupUid { get; set; }

    public GlobalDictionaries.GoodTypes GoodsType { get; set; }

    public Guid UnitsUid { get; set; } = Guid.Empty;

    public bool NeedComment { get; set; }

    public bool IsDataParent { get; set; }

    public bool IsRequestCount { get; set; }

    public int TaxRateNumber { get; set; }

    public int KkmSectionNumber { get; set; } = 1;

    public bool IsFreePrice { get; set; }

    public int DecimalPlace { get; set; }

    public GlobalDictionaries.RuTaxSystems RuTaxSystem { get; set; }

    public GlobalDictionaries.RuFfdGoodsTypes RuFfdGoodsType { get; set; }

    public GlobalDictionaries.RuMarkedProductionTypes RuMarkedProductionType { get; set; }

    public GoodGroupHome()
    {
    }

    public GoodGroupHome(GoodGroups.Group group)
    {
      this.Uid = group.Uid;
      this.IsDeleted = group.IsDeleted;
      this.Name = group.Name;
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
      this.IsCompositeGood = group.IsCompositeGood;
    }
  }
}
