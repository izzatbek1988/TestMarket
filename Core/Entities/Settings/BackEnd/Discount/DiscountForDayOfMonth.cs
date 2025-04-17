// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForDayOfMonth
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings.Discount;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd.Discount
{
  public class DiscountForDayOfMonth : Entity
  {
    public string Name { get; set; }

    public Decimal Discount { get; set; }

    public int Day { get; set; } = 1;

    public List<GoodGroups.Group> ListGroup { get; set; } = new List<GoodGroups.Group>();

    public DateTime TimeStart { get; set; } = new DateTime(1, 1, 1, 0, 0, 0);

    public DateTime TimeFinish { get; set; } = new DateTime(1, 1, 1, 23, 59, 59);

    public DateTime DateStart { get; set; } = DateTime.Now;

    public DateTime DateFinish { get; set; } = DateTime.Now;

    public bool IsOff { get; set; }

    public static implicit operator DiscountForDayOfMonth(DiscountForWeekday v)
    {
      throw new NotImplementedException();
    }
  }
}
