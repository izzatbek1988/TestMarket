// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Excel.ExcelFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Resources.Localizations;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Gbs.Helpers.Excel
{
  public static class ExcelFactory
  {
    public static XSSFWorkbook Create(DataTable dataTable)
    {
      XSSFWorkbook xssfWorkbook = new XSSFWorkbook();
      ISheet sheet = xssfWorkbook.CreateSheet();
      XSSFCreationHelper creationHelper = (XSSFCreationHelper) xssfWorkbook.GetCreationHelper();
      int rownum = 0;
      foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
      {
        IRow row2 = sheet.CreateRow(rownum);
        int column = 0;
        foreach (object obj in row1.ItemArray)
        {
          string str = obj.ToString();
          double result1;
          if (double.TryParse(str.Replace(" ", ""), out result1) && !BarcodeHelper.IsEan13Barcode(str) && !BarcodeHelper.IsEan8Barcode(str) && !Regex.IsMatch(str, "^0+[1-9]+"))
          {
            row2.CreateCell(column, CellType.Numeric).SetCellValue(result1);
          }
          else
          {
            DateTime result2;
            if (DateTime.TryParse(str, out result2) && !BarcodeHelper.IsEan13Barcode(str) && !BarcodeHelper.IsEan8Barcode(str))
            {
              ICell cell = row2.CreateCell(column);
              XSSFCellStyle cellStyle = (XSSFCellStyle) xssfWorkbook.CreateCellStyle();
              string format = result2.TimeOfDay.TotalMinutes > 0.0 ? "dd.MM.yyyy HH:mm" : "dd.MM.yyyy";
              cellStyle.SetDataFormat((int) creationHelper.CreateDataFormat().GetFormat(format));
              cell.SetCellValue(result2);
              cell.CellStyle = (ICellStyle) cellStyle;
            }
            else
            {
              ICell cell = row2.CreateCell(column, CellType.String);
              if (str.Split('\r').Length > 1)
              {
                XSSFCellStyle cellStyle = (XSSFCellStyle) xssfWorkbook.CreateCellStyle();
                cellStyle.WrapText = true;
                cell.CellStyle = (ICellStyle) cellStyle;
              }
              cell.SetCellValue(str);
            }
          }
          ++column;
        }
        ++rownum;
      }
      return xssfWorkbook;
    }

    public static XSSFWorkbook Create(List<ExchangeDataHelper.Good> goods)
    {
      XSSFWorkbook xssfWorkbook = new XSSFWorkbook();
      ISheet sheet = xssfWorkbook.CreateSheet(Translate.Other_Лист_1);
      IRow row1 = sheet.CreateRow(0);
      int column1 = 0;
      foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) typeof (ExchangeDataHelper.Good).GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (x => !x.Name.IsEither<string>("PropertiesList", "NamePoint", "UidPoint"))))
      {
        row1.CreateCell(column1).SetCellValue(propertyInfo.Name);
        sheet.AutoSizeColumn(column1);
        ++column1;
      }
      Dictionary<Guid, int> source = new Dictionary<Guid, int>();
      foreach (EntityProperties.PropertyType propertyType in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((System.Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)))
      {
        row1.CreateCell(column1).SetCellValue(propertyType.Name);
        sheet.AutoSizeColumn(column1);
        source.Add(propertyType.Uid, column1);
        ++column1;
      }
      int index = 0;
      foreach (ExchangeDataHelper.Good good1 in goods)
      {
        IRow row2 = sheet.CreateRow(index + 1);
        int column2 = 0;
        foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) good1.GetType().GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (x => !x.Name.IsEither<string>("PropertiesList", "NamePoint", "UidPoint"))))
        {
          ICell cell = row2.CreateCell(column2);
          ExchangeDataHelper.Good good2 = goods[index];
          string s = propertyInfo.GetValue((object) good2)?.ToString() ?? "";
          if (Decimal.TryParse(s, out Decimal _) & (!s.StartsWith("0") | s.StartsWith("0,") | s.Length == 1))
          {
            cell.SetCellType(CellType.Numeric);
            cell.SetCellValue(Convert.ToDouble(s));
          }
          else
          {
            cell.SetCellType(CellType.String);
            cell.SetCellValue(s);
          }
          ++column2;
        }
        foreach (ExchangeDataHelper.Good.Properties properties in good1.PropertiesList)
        {
          ExchangeDataHelper.Good.Properties prop = properties;
          int column3 = source.FirstOrDefault<KeyValuePair<Guid, int>>((System.Func<KeyValuePair<Guid, int>, bool>) (x => x.Key == prop.UidType)).Value;
          ICell cell = row2.CreateCell(column3);
          cell.SetCellType(CellType.String);
          cell.SetCellValue(prop.Value.ToString());
          ++column2;
        }
        ++index;
      }
      return xssfWorkbook;
    }

    public static XSSFWorkbook Create<T>(List<T> dataList)
    {
      XSSFWorkbook xssfWorkbook = new XSSFWorkbook();
      ISheet sheet = xssfWorkbook.CreateSheet(Translate.Other_Лист_1);
      IRow row1 = sheet.CreateRow(0);
      int column1 = 0;
      foreach (string str in ((IEnumerable<PropertyInfo>) typeof (T).GetProperties()).Select<PropertyInfo, string>((System.Func<PropertyInfo, string>) (x => x.Name)))
      {
        row1.CreateCell(column1).SetCellValue(str);
        sheet.AutoSizeColumn(column1);
        ++column1;
      }
      for (int index = 0; index < dataList.Count; ++index)
      {
        IRow row2 = sheet.CreateRow(index + 1);
        int column2 = 0;
        foreach (PropertyInfo property in dataList[index].GetType().GetProperties())
        {
          ICell cell = row2.CreateCell(column2);
          // ISSUE: variable of a boxed type
          __Boxed<T> data = (object) dataList[index];
          string s = property.GetValue((object) data)?.ToString() ?? "";
          if (Decimal.TryParse(s, out Decimal _) & (!s.StartsWith("0") | s.StartsWith("0,") | s.Length == 1))
          {
            cell.SetCellValue(Convert.ToDouble(s));
            cell.SetCellType(CellType.Numeric);
          }
          else
          {
            cell.SetCellValue(s);
            cell.SetCellType(CellType.String);
          }
          ++column2;
        }
      }
      return xssfWorkbook;
    }
  }
}
