// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.LevinshtaingHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public class LevinshtaingHelper
  {
    public static int DamerauLevenshteinDistance(string string1, string string2)
    {
      if (string.IsNullOrEmpty(string1))
        return !string.IsNullOrEmpty(string2) ? string2.Length : 0;
      if (string.IsNullOrEmpty(string2))
        return string.IsNullOrEmpty(string1) ? 0 : string1.Length;
      int[,] numArray = new int[string1.Length + 1, string2.Length + 1];
      for (int index = 0; index <= numArray.GetUpperBound(0); ++index)
        numArray[index, 0] = index;
      for (int index = 0; index <= numArray.GetUpperBound(1); ++index)
        numArray[0, index] = index;
      for (int index1 = 1; index1 <= numArray.GetUpperBound(0); ++index1)
      {
        for (int index2 = 1; index2 <= numArray.GetUpperBound(1); ++index2)
        {
          int num = (int) string1[index1 - 1] != (int) string2[index2 - 1] ? 1 : 0;
          int val1_1 = numArray[index1 - 1, index2] + 1;
          int val1_2 = numArray[index1, index2 - 1] + 1;
          int val2 = numArray[index1 - 1, index2 - 1] + num;
          numArray[index1, index2] = Math.Min(val1_1, Math.Min(val1_2, val2));
          if (index1 > 1 && index2 > 1 && (int) string1[index1 - 1] == (int) string2[index2 - 2] && (int) string1[index1 - 2] == (int) string2[index2 - 1])
            numArray[index1, index2] = Math.Min(numArray[index1, index2], numArray[index1 - 2, index2 - 2] + num);
        }
      }
      return numArray[numArray.GetUpperBound(0), numArray.GetUpperBound(1)];
    }

    public static int IsSimilarTo(string name, string search)
    {
      name = name.ToLower();
      search = search.ToLower();
      List<string> list = ((IEnumerable<string>) name.Split(' ')).Where<string>((Func<string, bool>) (x => x.Length > 2)).ToList<string>();
      return !list.Any<string>() ? 9999 : Math.Min(list.Min<string>((Func<string, int>) (x => LevinshtaingHelper.DamerauLevenshteinDistance(x, search))), LevinshtaingHelper.DamerauLevenshteinDistance(name, search) / list.Count);
    }
  }
}
