// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Excel.ExcelFile
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

#nullable disable
namespace Gbs.Helpers.Excel
{
  public static class ExcelFile
  {
    public static IWorkbook Read(string path)
    {
      string str1 = FileSystemHelper.TempFolderPath();
      try
      {
        string str2 = Path.Combine(str1, "excelTemp");
        File.Copy(path, str2);
        FileStream fileStream = new FileStream(str2, FileMode.Open, FileAccess.Read);
        return !(new FileInfo(path).Extension.ToLower() == ".xlsx") ? (IWorkbook) new HSSFWorkbook((Stream) fileStream) : (IWorkbook) new XSSFWorkbook((Stream) fileStream);
      }
      catch (OldExcelFormatException ex)
      {
        Directory.Delete(str1, true);
        FileFormatException fileFormatException = new FileFormatException(string.Format(Translate.ExcelFile_Файл_имеет_устаревший_формат__0_Откройте_документ_в_Excel_и_сохраните_его_в_формате__Excel_97_2003__или_новее_, (object) Other.NewLine(2)), (Exception) ex);
        fileFormatException.Data.Add((object) "File path", (object) path);
        throw fileFormatException;
      }
      catch (Exception ex)
      {
        Directory.Delete(str1, true);
        LogHelper.WriteError(ex, "Ошибка чтения файла Ексель");
        throw;
      }
    }

    public static void Write(IWorkbook workbook, string path)
    {
      if (workbook == null)
        throw new ArgumentNullException(nameof (workbook));
      if (path.IsNullOrEmpty())
        throw new ArgumentNullException(nameof (path));
      using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
      {
        workbook.Write((Stream) fileStream);
        workbook.Close();
      }
    }
  }
}
