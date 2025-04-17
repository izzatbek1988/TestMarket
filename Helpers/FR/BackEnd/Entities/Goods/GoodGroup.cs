// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.Goods.GoodGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.Goods
{
  public class GoodGroup
  {
    public Guid Uid { get; set; }

    public string Name { get; set; }

    public Guid ParentGroupUid { get; set; } = Guid.Empty;

    public string GoodsType { get; set; }

    public Guid UnitsUid { get; set; } = Guid.Empty;

    public bool NeedComment { get; set; }

    public bool IsDataParent { get; set; }

    public bool IsRequestCount { get; set; }

    public int TaxRateNumber { get; set; }

    public int KkmSectionNumber { get; set; }

    public bool IsFreePrice { get; set; }

    public int DecimalPlace { get; set; }

    public string RuTaxSystem { get; set; }

    public string RuFfdGoodsType { get; set; }

    public string RuMarkedProductionType { get; set; }

    public string TaxRateNumberText { get; set; }
  }
}
