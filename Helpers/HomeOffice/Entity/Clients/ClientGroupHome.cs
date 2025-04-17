// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.Clients.ClientGroupHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity.Clients
{
  public class ClientGroupHome
  {
    public Guid Uid { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; }

    public Guid PriceUid { get; set; }

    public Decimal Discount { get; set; }

    public Decimal? MaxSumCredit { get; set; }

    public bool IsSupplier { get; set; }

    public bool IsNonUseBonus { get; set; }

    public ClientGroupHome()
    {
    }

    public ClientGroupHome(Gbs.Core.Entities.Clients.Group group)
    {
      this.Name = group.Name;
      this.Uid = group.Uid;
      this.IsDeleted = group.IsDeleted;
      GoodsExtraPrice.GoodExtraPrice price = group.Price;
      // ISSUE: explicit non-virtual call
      this.PriceUid = price != null ? __nonvirtual (price.Uid) : Guid.Empty;
      this.Discount = group.Discount;
      this.MaxSumCredit = group.MaxSumCredit;
      this.IsSupplier = group.IsSupplier;
      this.IsNonUseBonus = group.IsNonUseBonus;
    }
  }
}
