// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.XAML.InverseBooleanConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.XAML
{
  [ValueConversion(typeof (bool), typeof (bool))]
  public class InverseBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (targetType != typeof (bool))
        throw new InvalidOperationException("The target must be a boolean");
      return (object) (bool) (value == null ? 0 : (!(bool) value ? 1 : 0));
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) (bool) this.Convert(value, targetType, parameter, culture);
    }
  }
}
