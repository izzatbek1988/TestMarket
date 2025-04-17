// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ListItems.GoodHistoryReportItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ListItems
{
  [Serializable]
  public class GoodHistoryReportItem
  {
    public Guid Uid
    {
      get
      {
        Gbs.Helpers.FR.BackEnd.Entities.Goods.Good good = this.Good;
        return good == null ? Guid.Empty : good.Uid;
      }
    }

    public Decimal MaxPrice { get; set; }

    public Gbs.Helpers.FR.BackEnd.Entities.Goods.Good Good { get; set; }

    public Decimal CountStart { get; set; }

    public Decimal CountFinish { get; set; }
  }
}
