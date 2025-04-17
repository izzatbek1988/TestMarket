// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Excel.ExcelCellValueReader
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.Excel
{
  public static class ExcelCellValueReader
  {
    public static string GetStringCellValue(IRow row, int cellIndex)
    {
      object cellValue = ExcelCellValueReader.GetCellValue(row, cellIndex, ExcelCellValueReader.CellValueTypes.String);
      if (cellValue == null)
        return string.Empty;
      return ((string) cellValue).Trim(' ', '\t');
    }

    public static Decimal GetDecimalCellValue(IRow row, int cellIndex)
    {
      object cellValue = ExcelCellValueReader.GetCellValue(row, cellIndex, ExcelCellValueReader.CellValueTypes.Decimal);
      return cellValue == null ? 0M : (Decimal) cellValue;
    }

    public static object GetCellValue(
      IRow row,
      int cellIndex,
      ExcelCellValueReader.CellValueTypes type,
      bool isNextIfEmpty = false)
    {
      try
      {
        Dictionary<ExcelCellValueReader.CellValueTypes, object> dictionary = new Dictionary<ExcelCellValueReader.CellValueTypes, object>()
        {
          {
            ExcelCellValueReader.CellValueTypes.NotSet,
            (object) null
          },
          {
            ExcelCellValueReader.CellValueTypes.String,
            (object) string.Empty
          },
          {
            ExcelCellValueReader.CellValueTypes.Decimal,
            (object) 0M
          }
        };
        --cellIndex;
        ICell cell = cellIndex < 0 ? (ICell) null : row.GetCell(cellIndex);
        if (cell == null)
          return isNextIfEmpty ? (object) null : dictionary[type];
        try
        {
          if (type == ExcelCellValueReader.CellValueTypes.Decimal)
          {
            if (cell.RichStringCellValue.String == "")
              return isNextIfEmpty ? (object) null : dictionary[type];
          }
        }
        catch
        {
        }
        object rawCellValue = ExcelCellValueReader.GetRawCellValue(cell, type);
        if (rawCellValue == null)
          return isNextIfEmpty ? (object) null : dictionary[type];
        object cellValue;
        switch (type)
        {
          case ExcelCellValueReader.CellValueTypes.NotSet:
            cellValue = rawCellValue;
            break;
          case ExcelCellValueReader.CellValueTypes.String:
            cellValue = (object) rawCellValue.ToString();
            break;
          case ExcelCellValueReader.CellValueTypes.Decimal:
            cellValue = (object) (rawCellValue is double ? Convert.ToDecimal(rawCellValue) : rawCellValue.ToString().ToDecimal());
            break;
          default:
            cellValue = rawCellValue;
            break;
        }
        return cellValue;
      }
      catch
      {
        return (object) null;
      }
    }

    private static object GetRawCellValue(ICell cell, ExcelCellValueReader.CellValueTypes type)
    {
      object rawCellValue = (object) null;
      if (type != ExcelCellValueReader.CellValueTypes.NotSet)
      {
        if (type == ExcelCellValueReader.CellValueTypes.Decimal)
        {
          try
          {
            return (object) cell.NumericCellValue;
          }
          catch
          {
            Decimal result;
            return Decimal.TryParse(cell.StringCellValue, out result) ? (object) result : (object) null;
          }
        }
      }
      switch (cell.CellType)
      {
        case CellType.Numeric:
          rawCellValue = DateUtil.IsCellDateFormatted(cell) ? (object) cell.DateCellValue : (object) cell.NumericCellValue;
          break;
        case CellType.String:
          rawCellValue = (object) cell.StringCellValue;
          break;
        case CellType.Formula:
          switch (cell.CachedFormulaResultType)
          {
            case CellType.Numeric:
              rawCellValue = (object) cell.NumericCellValue;
              break;
            case CellType.String:
              rawCellValue = (object) cell.StringCellValue;
              break;
          }
          break;
      }
      return rawCellValue;
    }

    public enum CellValueTypes
    {
      NotSet,
      String,
      Decimal,
    }
  }
}
