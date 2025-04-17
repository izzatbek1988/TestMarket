// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.DataGrid.ExcelToRowConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.DataGrid
{
  internal class ExcelToRowConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter == null)
        return (object) null;
      int index = int.Parse(parameter.ToString());
      // ISSUE: explicit non-virtual call
      return !(value is List<string> stringList) ? (object) null : (object) __nonvirtual (stringList[index]);
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
