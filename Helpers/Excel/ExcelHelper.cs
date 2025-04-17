// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Excel.ExcelHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.Excel
{
  public static class ExcelHelper
  {
    public static string ColumnNumberToAbc(int columnNumber)
    {
      if (columnNumber == 0)
        return string.Empty;
      string abc = string.Empty;
      do
      {
        abc = Convert.ToChar(65 + (columnNumber - 1) % 26).ToString() + abc;
        columnNumber = (columnNumber - 1) / 26;
      }
      while (columnNumber != 0);
      return abc;
    }
  }
}
