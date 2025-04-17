// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.Discount.DiscountForWeekday
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities.Settings.Discount
{
  public class DiscountForWeekday : Entity
  {
    public string Name { get; set; }

    public Decimal Discount { get; set; }

    public DateTime TimeStart { get; set; } = new DateTime(1, 1, 1, 0, 0, 0);

    public DateTime TimeFinish { get; set; } = new DateTime(1, 1, 1, 23, 59, 59);

    public DateTime DateStart { get; set; } = DateTime.Now;

    public DateTime DateFinish { get; set; } = DateTime.Now;

    public bool IsOff { get; set; }

    public List<WeekDayItem> WeekdaysList { get; set; } = DiscountForWeekday.GetWeekdays();

    public List<GoodGroups.Group> ListGroup { get; set; } = new List<GoodGroups.Group>();

    public static List<WeekDayItem> GetWeekdays()
    {
      return new List<WeekDayItem>()
      {
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Понедельник,
          Weekday = DayOfWeek.Monday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Вторник,
          Weekday = DayOfWeek.Tuesday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Среда,
          Weekday = DayOfWeek.Wednesday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Четверг,
          Weekday = DayOfWeek.Thursday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Пятница,
          Weekday = DayOfWeek.Friday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Суббота,
          Weekday = DayOfWeek.Saturday
        },
        new WeekDayItem()
        {
          IsChecked = false,
          Text = Translate.DiscountForWeekday_Воскресение,
          Weekday = DayOfWeek.Sunday
        }
      };
    }
  }
}
