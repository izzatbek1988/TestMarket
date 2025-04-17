// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Clients.Group
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db.Clients;
using Gbs.Helpers.HomeOffice.Entity.Clients;
using Gbs.Resources.Localizations;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Core.Entities.Clients
{
  public class Group : Gbs.Core.Entities.Entity
  {
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    public GoodsExtraPrice.GoodExtraPrice Price { get; set; }

    [Required]
    [Range(0.0, 100.0)]
    public Decimal Discount { get; set; }

    public Decimal? MaxSumCredit { get; set; }

    public bool IsSupplier { get; set; }

    public bool IsNonUseBonus { get; set; }

    public Group()
    {
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice.Name = Translate.GroupClientCardModelView_Основная;
      goodExtraPrice.Uid = Guid.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CPrice\u003Ek__BackingField = goodExtraPrice;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public Group(CLIENTS_GROUPS dbItem)
    {
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice.Name = Translate.GroupClientCardModelView_Основная;
      goodExtraPrice.Uid = Guid.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CPrice\u003Ek__BackingField = goodExtraPrice;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Uid = dbItem.UID;
      this.Name = dbItem.NAME;
      this.Discount = dbItem.DISCOUNT;
      this.IsSupplier = dbItem.IS_SUPPLIER;
      this.MaxSumCredit = dbItem.MAX_SUM_CREDIT == -1M ? new Decimal?() : new Decimal?(dbItem.MAX_SUM_CREDIT);
      this.IsNonUseBonus = dbItem.IS_NON_USE_BONUS;
      this.IsDeleted = dbItem.IS_DELETED;
    }

    public Group(ClientGroupHome groupHome)
    {
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice1 = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice1.Name = Translate.GroupClientCardModelView_Основная;
      goodExtraPrice1.Uid = Guid.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CPrice\u003Ek__BackingField = goodExtraPrice1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Uid = groupHome.Uid;
      this.Name = groupHome.Name;
      this.Discount = groupHome.Discount;
      this.IsSupplier = groupHome.IsSupplier;
      this.MaxSumCredit = groupHome.MaxSumCredit;
      this.IsNonUseBonus = groupHome.IsNonUseBonus;
      this.IsDeleted = groupHome.IsDeleted;
      GoodsExtraPrice.GoodExtraPrice goodExtraPrice2 = new GoodsExtraPrice.GoodExtraPrice();
      goodExtraPrice2.Uid = groupHome.PriceUid;
      this.Price = goodExtraPrice2;
    }
  }
}
