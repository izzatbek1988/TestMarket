// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MarkCodes.DataMatrixHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers.MarkCodes
{
  public static class DataMatrixHelper
  {
    public static string ReplaceSomeCharsToFNC1(string code)
    {
      char newChar = Convert.ToChar(29);
      return code?.Trim().Replace(' ', newChar).Replace('↔', newChar).Replace("<GS>", newChar.ToString() ?? "") ?? "";
    }

    public static DataMatrixHelper.DataMatrixCode ParseGS1DataMatrix(string input)
    {
      input = input.Trim();
      char ch = Convert.ToChar(29);
      if (!input.Contains(new string(new char[1]{ ch })))
      {
        Console.WriteLine("Ошибка: FNC1 отсутствует. Разбор невозможен.");
        return (DataMatrixHelper.DataMatrixCode) null;
      }
      DataMatrixHelper.DataMatrixCode gs1DataMatrix = new DataMatrixHelper.DataMatrixCode()
      {
        FullCode = input
      };
      string[] strArray = input.Split(ch);
label_21:
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str1 = strArray[index];
        int startIndex1 = 0;
        while (startIndex1 < str1.Length && startIndex1 + 2 <= str1.Length)
        {
          string str2 = str1.Substring(startIndex1, 2);
          startIndex1 += 2;
          switch (str2)
          {
            case "01":
              if (startIndex1 + 14 <= str1.Length)
              {
                string str3 = str1.Substring(startIndex1, 14);
                gs1DataMatrix.GTIN_01 = str3;
                startIndex1 += 14;
                continue;
              }
              goto label_21;
            case "17":
              if (startIndex1 + 6 <= str1.Length)
              {
                string str4 = str1.Substring(startIndex1, 6);
                gs1DataMatrix.ExpiredDate_17 = str4;
                startIndex1 += 6;
                continue;
              }
              goto label_21;
            case "10":
            case "21":
            case "91":
            case "92":
            case "93":
              int startIndex2 = startIndex1;
              while (startIndex1 < str1.Length)
                ++startIndex1;
              string str5 = str1.Substring(startIndex2, startIndex1 - startIndex2);
              switch (str2)
              {
                case "10":
                  gs1DataMatrix.PartNumber_10 = str5;
                  continue;
                case "21":
                  gs1DataMatrix.Serial_21 = str5;
                  continue;
                case "91":
                  gs1DataMatrix.Field_91 = str5;
                  continue;
                case "92":
                  gs1DataMatrix.Field_92 = str5;
                  continue;
                case "93":
                  gs1DataMatrix.Field_93 = str5;
                  continue;
                default:
                  continue;
              }
            default:
              Console.WriteLine("Предупреждение: Неизвестный AI " + str2 + ", пропущен.");
              goto label_21;
          }
        }
      }
      return gs1DataMatrix;
    }

    public class DataMatrixCode
    {
      public string FullCode { get; set; }

      public string GTIN_01 { get; set; }

      public string PartNumber_10 { get; set; }

      public string ExpiredDate_17 { get; set; }

      public string Serial_21 { get; set; }

      public string Field_91 { get; set; }

      public string Field_92 { get; set; }

      public string Field_93 { get; set; }
    }
  }
}
