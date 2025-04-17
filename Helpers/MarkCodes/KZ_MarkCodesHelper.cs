// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MarkCodes.KZ_MarkCodesHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;

#nullable disable
namespace Gbs.Helpers.MarkCodes
{
  public static class KZ_MarkCodesHelper
  {
    public static string PrepareCodeForKkm(string fullCode)
    {
      string str = KZ_MarkCodesHelper.PrepareCode(fullCode);
      LogHelper.Debug("Исходный код: " + fullCode + "; обработанный: " + str);
      return str;
    }

    private static string PrepareCode(string fullCode)
    {
      if (fullCode == null)
        return (string) null;
      string input = DataMatrixHelper.ReplaceSomeCharsToFNC1(fullCode);
      if (input.Length < 29)
        return input;
      if (input.Length == 29)
        return input.Substring(0, 21);
      DataMatrixHelper.DataMatrixCode gs1DataMatrix = DataMatrixHelper.ParseGS1DataMatrix(input);
      if (gs1DataMatrix != null)
        return "01" + gs1DataMatrix.GTIN_01 + "21" + gs1DataMatrix.Serial_21;
      char ch = Convert.ToChar(29);
      if (input.Contains(new string(new char[1]{ ch })))
        input = input.Replace(new string(new char[1]{ ch }), "");
      if (!input.StartsWith("01"))
        return input;
      string str1 = string.Empty;
      string str2 = string.Empty;
      switch (input.Length)
      {
        case 31:
        case 35:
          str1 = input.Substring(2, 14);
          str2 = input.Substring(2 + str1.Length + 2, 7);
          break;
        case 37:
        case 83:
        case (int) sbyte.MaxValue:
          str1 = input.Substring(2, 14);
          str2 = input.Substring(2 + str1.Length + 2, 13);
          break;
      }
      return str1.Length > 0 && str2.Length > 0 ? "01" + str1 + "21" + str2 : input;
    }
  }
}
