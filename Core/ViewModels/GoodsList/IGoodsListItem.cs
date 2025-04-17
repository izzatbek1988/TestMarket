// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Models.GoodInList.IGoodsListItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using System;

#nullable disable
namespace Gbs.Core.Models.GoodInList
{
  public interface IGoodsListItem
  {
    Good Good { get; set; }

    GoodsModifications.GoodModification GoodModification { get; set; }

    Decimal Quantity { get; set; }

    Guid Uid { get; set; }
  }
}
