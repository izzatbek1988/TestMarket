// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BarcodeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public static class BarcodeHelper
  {
    public static (Gbs.Core.Entities.Goods.Good g, Decimal w) GetWeightItem(
      string searchQuery,
      List<Gbs.Core.Entities.Goods.Good> goods)
    {
      Guid pluUid = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().ScaleWithLable.PluUid;
      int plu = Convert.ToInt32(searchQuery.Substring(2, 5));
      Decimal num = Convert.ToDecimal(searchQuery.Substring(7, 5)) / 1000M;
      List<Gbs.Core.Entities.Goods.Good> list = goods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == pluUid && Convert.ToInt32(p.Value) == plu)) && !x.Group.IsCompositeGood)).ToList<Gbs.Core.Entities.Goods.Good>();
      if (list.Any<Gbs.Core.Entities.Goods.Good>())
        return (list.First<Gbs.Core.Entities.Goods.Good>(), num);
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Title = Translate.MainWindowViewModel_Поиск_весового_товара,
        Text = string.Format(Translate.MainWindowViewModel_неУдалосьНайтиТоварСКодом, (object) plu)
      });
      return ((Gbs.Core.Entities.Goods.Good) null, 0M);
    }

    public static string RandomBarcode(string prefix)
    {
      try
      {
        string str1 = prefix;
        Guid guid = Guid.NewGuid();
        double num = (double) Math.Abs(guid.GetHashCode()) * 4.66;
        guid = Guid.NewGuid();
        double hashCode = (double) guid.GetHashCode();
        string str2 = Math.Abs(num + hashCode).ToString("0000000000");
        string str3 = str1 + str2;
        int length = str3.Length > 12 ? 12 : str3.Length;
        string data = str3.Substring(0, length);
        int controlSum;
        if (BarcodeHelper.Ean13ControlSum(data, out controlSum))
          return data + controlSum.ToString();
        LogHelper.Debug("Не удалось просчитать контрольную сумму для EAN13");
        return data;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка генерации ШК");
        return string.Empty;
      }
    }

    public static string RandomPass()
    {
      Random random = new Random();
      string str = "";
      for (int index = 0; index < 6; ++index)
        str += random.Next(0, 9).ToString();
      return str;
    }

    private static bool Ean13ControlSum(string data, out int controlSum)
    {
      try
      {
        if (data.Length != 12)
          throw new ArgumentOutOfRangeException();
        int num = 0;
        for (int startIndex = 0; startIndex <= 11; ++startIndex)
        {
          int int32 = Convert.ToInt32(data.Substring(startIndex, 1));
          num += Convert.ToInt32(startIndex % 2 == 0 ? int32 : int32 * 3);
        }
        string str = (10 - num % 10).ToString();
        controlSum = Convert.ToInt32(str.Substring(str.Length - 1, 1));
        return true;
      }
      catch (Exception ex)
      {
        controlSum = 0;
        LogHelper.Error(ex, "Ошибка расчет контрольной суммы для ШК", false);
        return false;
      }
    }

    public static string GetWeightBarcode(string prefix, int plu, Decimal q)
    {
      string str1 = prefix;
      string str2 = plu.ToString().ToCharArray().Length >= 5 ? (plu.ToString().ToCharArray().Length <= 5 ? str1 + plu.ToString() : str1 + plu.ToString().Remove(5)) : str1 + plu.ToString("00000");
      Decimal num1 = Math.Truncate(q);
      double num2 = (double) (q - num1) * Math.Pow(10.0, 3.0);
      string str3 = num1.ToString("00") + num2.ToString("000");
      string data = str2 + str3;
      int controlSum = 0;
      BarcodeHelper.Ean13ControlSum(data, out controlSum);
      return data + controlSum.ToString();
    }

    private static bool Ean8ControlSum(string data, out int controlSum)
    {
      if (data.Length != 7)
        throw new ArgumentOutOfRangeException();
      int num1 = 0;
      int num2 = 0;
      for (int startIndex = 0; startIndex <= 6; ++startIndex)
      {
        int int32 = Convert.ToInt32(data.Substring(startIndex, 1));
        if ((startIndex + 1) % 2 == 0)
          num1 += int32;
        else
          num2 += int32;
      }
      string str = (10 - (num1 + 3 * num2) % 10).ToString();
      controlSum = Convert.ToInt32(str.Substring(str.Length - 1, 1));
      return true;
    }

    public static bool IsEan13Barcode(string barcode)
    {
      if (barcode.Length != 13 || !((IEnumerable<char>) barcode.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit)))
        return false;
      int controlSum;
      BarcodeHelper.Ean13ControlSum(barcode.Substring(0, 12), out controlSum);
      return controlSum.ToString() == barcode.Substring(12);
    }

    public static bool IsEan8Barcode(string barcode)
    {
      if (barcode.Length != 8 || !((IEnumerable<char>) barcode.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit)))
        return false;
      int controlSum;
      BarcodeHelper.Ean8ControlSum(barcode.Substring(0, 7), out controlSum);
      return controlSum.ToString() == barcode.Substring(7);
    }
  }
}
