// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MathHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers
{
  public class MathHelper
  {
    public static Decimal RoundToCoefficient(Decimal sum, Decimal coef, MathHelper.RoundWays way = MathHelper.RoundWays.Down)
    {
      if (way == MathHelper.RoundWays.Down)
        return Math.Floor(sum / coef) * coef;
      if (way == MathHelper.RoundWays.Up)
        return Math.Ceiling(sum / coef) * coef;
      if (way == MathHelper.RoundWays.Math)
        return Math.Ceiling(Math.Round(sum / coef, 0, MidpointRounding.AwayFromZero)) * coef;
      throw new ArgumentOutOfRangeException();
    }

    public static bool CompareNumbers(Decimal? a, Decimal? b, string op)
    {
      switch (op)
      {
        case "=":
          Decimal? nullable1 = a;
          Decimal? nullable2 = b;
          return nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue;
        case ">":
          Decimal? nullable3 = a;
          Decimal? nullable4 = b;
          return nullable3.GetValueOrDefault() > nullable4.GetValueOrDefault() & nullable3.HasValue & nullable4.HasValue;
        case "<":
          Decimal? nullable5 = a;
          Decimal? nullable6 = b;
          return nullable5.GetValueOrDefault() < nullable6.GetValueOrDefault() & nullable5.HasValue & nullable6.HasValue;
        default:
          throw new ArgumentOutOfRangeException("Unknown operator");
      }
    }

    public enum RoundWays
    {
      Down,
      Up,
      Math,
    }
  }
}
