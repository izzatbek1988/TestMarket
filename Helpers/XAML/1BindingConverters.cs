// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.XAML.StringToIntConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.XAML
{
  [ValueConversion(typeof (string), typeof (int))]
  public class StringToIntConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (value != null ? int.Parse(value.ToString()) : 0);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value == null ? (object) string.Empty : (object) value.ToString();
    }
  }
}
