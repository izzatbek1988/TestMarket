// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.XAML.CaseConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.XAML
{
  public class CaseConverter : IValueConverter
  {
    public CharacterCasing Case { get; set; }

    public CaseConverter() => this.Case = CharacterCasing.Upper;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is string str))
        return (object) string.Empty;
      switch (this.Case)
      {
        case CharacterCasing.Normal:
          return (object) str;
        case CharacterCasing.Lower:
          return (object) str.ToLower();
        case CharacterCasing.Upper:
          return (object) str.ToUpper();
        default:
          return (object) str;
      }
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
