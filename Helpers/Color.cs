// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ColorHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Drawing;

#nullable disable
namespace Gbs.Helpers
{
  internal static class ColorHelper
  {
    public static Color HSL2RGB(this ColorHelper.HSLColor hsl)
    {
      double l = hsl.L;
      double s = hsl.S;
      double h = hsl.H;
      double num1 = l;
      double num2 = l;
      double num3 = l;
      double num4 = l <= 0.5 ? l * (1.0 + s) : l + s - l * s;
      if (num4 > 0.0)
      {
        double num5 = l + l - num4;
        double num6 = (num4 - num5) / num4;
        double num7 = h * 6.0;
        int num8 = (int) num7;
        double num9 = num7 - (double) num8;
        double num10 = num4 * num6 * num9;
        double num11 = num5 + num10;
        double num12 = num4 - num10;
        switch (num8)
        {
          case 0:
            num1 = num4;
            num2 = num11;
            num3 = num5;
            break;
          case 1:
            num1 = num12;
            num2 = num4;
            num3 = num5;
            break;
          case 2:
            num1 = num5;
            num2 = num4;
            num3 = num11;
            break;
          case 3:
            num1 = num5;
            num2 = num12;
            num3 = num4;
            break;
          case 4:
            num1 = num11;
            num2 = num5;
            num3 = num4;
            break;
          case 5:
            num1 = num4;
            num2 = num5;
            num3 = num12;
            break;
        }
      }
      return Color.FromArgb((int) Convert.ToByte(num1 * (double) byte.MaxValue), (int) Convert.ToByte(num2 * (double) byte.MaxValue), (int) Convert.ToByte(num3 * (double) byte.MaxValue));
    }

    public static Color InvertBrightness(this Color color)
    {
      ColorHelper.HSLColor hsl = color.RGB2HSL();
      hsl.L = 1.0 - hsl.L;
      return hsl.HSL2RGB();
    }

    public static ColorHelper.HSLColor RGB2HSL(this Color rgb)
    {
      double val1 = (double) rgb.R / (double) byte.MaxValue;
      double val2_1 = (double) rgb.G / (double) byte.MaxValue;
      double val2_2 = (double) rgb.B / (double) byte.MaxValue;
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = Math.Max(Math.Max(val1, val2_1), val2_2);
      double num4 = Math.Min(Math.Min(val1, val2_1), val2_2);
      double num5 = (num4 + num3) / 2.0;
      ColorHelper.HSLColor hslColor = new ColorHelper.HSLColor();
      if (num5 <= 0.0)
      {
        hslColor.H = num1;
        hslColor.L = num5;
        hslColor.S = num2;
        return hslColor;
      }
      double num6 = num3 - num4;
      double num7 = num6;
      if (num7 > 0.0)
      {
        double num8 = num7 / (num5 <= 0.5 ? num3 + num4 : 2.0 - num3 - num4);
        double num9 = (num3 - val1) / num6;
        double num10 = (num3 - val2_1) / num6;
        double num11 = (num3 - val2_2) / num6;
        double num12 = (val1 != num3 ? (val2_1 != num3 ? (val1 == num4 ? 3.0 + num10 : 5.0 - num9) : (val2_2 == num4 ? 1.0 + num9 : 3.0 - num11)) : (val2_1 == num4 ? 5.0 + num11 : 1.0 - num10)) / 6.0;
        hslColor.H = num12;
        hslColor.L = num5;
        hslColor.S = num8;
        return hslColor;
      }
      hslColor.H = num1;
      hslColor.L = num5;
      hslColor.S = num7;
      return hslColor;
    }

    public class HSLColor
    {
      public double H { get; set; }

      public double S { get; set; }

      public double L { get; set; }
    }
  }
}
