// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Extensions.Numeric.NumberExtensions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers.Extensions.Numeric
{
  public static class NumberExtensions
  {
    private static readonly IDictionary<Key, int> NumericKeys = (IDictionary<Key, int>) new Dictionary<Key, int>()
    {
      {
        Key.D0,
        0
      },
      {
        Key.D1,
        1
      },
      {
        Key.D2,
        2
      },
      {
        Key.D3,
        3
      },
      {
        Key.D4,
        4
      },
      {
        Key.D5,
        5
      },
      {
        Key.D6,
        6
      },
      {
        Key.D7,
        7
      },
      {
        Key.D8,
        8
      },
      {
        Key.D9,
        9
      },
      {
        Key.NumPad0,
        0
      },
      {
        Key.NumPad1,
        1
      },
      {
        Key.NumPad2,
        2
      },
      {
        Key.NumPad3,
        3
      },
      {
        Key.NumPad4,
        4
      },
      {
        Key.NumPad5,
        5
      },
      {
        Key.NumPad6,
        6
      },
      {
        Key.NumPad7,
        7
      },
      {
        Key.NumPad8,
        8
      },
      {
        Key.NumPad9,
        9
      }
    };

    public static bool IsBetween(this DateTime time, DateTime startTime, DateTime endTime)
    {
      if (time.TimeOfDay == startTime.TimeOfDay || time.TimeOfDay == endTime.TimeOfDay)
        return true;
      return startTime.TimeOfDay <= endTime.TimeOfDay ? time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay : !(time.TimeOfDay >= endTime.TimeOfDay) || !(time.TimeOfDay <= startTime.TimeOfDay);
    }

    public static Decimal ToDbDecimal(this Decimal source)
    {
      int length = ((int) Decimal.Truncate(source)).ToString().Length;
      return Math.Round(source, 18 - length);
    }

    public static Decimal GetNdsSum(this Decimal source, Decimal nds)
    {
      return nds < 0M ? 0M : Math.Round(source / (1M + nds / 100M) * nds / 100M, 2);
    }

    public static bool GetBit(this int c, int bitNumber) => (c & 1 << bitNumber) != 0;

    public static int? GetKeyNumericValue(this KeyEventArgs e)
    {
      return NumberExtensions.NumericKeys.ContainsKey(e.Key) ? new int?(NumberExtensions.NumericKeys[e.Key]) : new int?();
    }
  }
}
