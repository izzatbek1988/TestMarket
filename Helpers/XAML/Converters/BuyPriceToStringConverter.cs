// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.XAML.Converters.BuyPriceToStringConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.XAML.Converters
{
  public class BuyPriceToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) null;
      Decimal result;
      if (!Decimal.TryParse(value.ToString(), out result))
        return (object) null;
      Decimal num1 = result - Math.Truncate(result);
      int num2 = num1.ToString((IFormatProvider) CultureInfo.InvariantCulture).Length - 2;
      string format = "N";
      if (new ConfigsRepository<Settings>().Get().Interface.IsHideExtraZerosPrice && num1 == 0M || num1 > 0M)
      {
        if (num2 > 0 && num1 != 0M)
        {
          int num3 = !new ConfigsRepository<Settings>().Get().Waybill.IsMoreDecimalPlaceBuyPrice ? (num2 > 3 ? 3 : num2) : (num2 > 4 ? 4 : num2);
          format += num3.ToString();
        }
        else
          format += 0.ToString();
      }
      return (object) result.ToString(format);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
