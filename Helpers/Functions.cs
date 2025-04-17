// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Functions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Models.GoodInList;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Gbs.Helpers
{
  public static class Functions
  {
    public static void ConvertToPng(string pathAdd, string path)
    {
      try
      {
        Image.FromFile(pathAdd).Save(path, ImageFormat.Png);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось конвертировать фото в формат png");
      }
    }

    public static bool MathCompare(Decimal d1, Decimal d2, string op)
    {
      switch (op)
      {
        case ">":
          return d1 > d2;
        case "<":
          return d1 < d2;
        case "=":
          return d1 == d2;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static string CutName(string fullName, bool isVeryShort = false)
    {
      string[] strArray = fullName.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      string str1;
      if (strArray.Length != 3)
        str1 = fullName;
      else
        str1 = strArray[0] + " " + strArray[1].Substring(0, 1) + ". " + strArray[2].Substring(0, 1) + ".";
      string str2 = str1;
      if (isVeryShort && strArray.Length == 2)
        str2 = strArray[0] + " " + strArray[1].Substring(0, 1) + ".";
      return str2;
    }

    public static bool IsObjectEqual<T>(T obj1, T obj2)
    {
      if ((object) obj1 == null && (object) obj2 == null)
        return true;
      if ((object) obj1 == null || (object) obj2 == null)
        return false;
      JObject jobject1 = JObject.FromObject((object) obj1);
      JObject jobject2 = JObject.FromObject((object) obj2);
      Functions.Sort(jobject1);
      Functions.Sort(jobject2);
      return JToken.DeepEquals((JToken) jobject1, (JToken) jobject2);
    }

    private static void Sort(JObject jObj)
    {
      List<JProperty> list = jObj.Properties().ToList<JProperty>();
      foreach (JToken jtoken in list)
        jtoken.Remove();
      foreach (JProperty content in (IEnumerable<JProperty>) list.OrderBy<JProperty, string>((Func<JProperty, string>) (p => p.Name)))
      {
        jObj.Add((object) content);
        JToken jtoken = content.Value;
        if (!(jtoken is JObject jObj1))
        {
          if (jtoken is JArray)
          {
            int num = content.Value.Count<JToken>();
            if (num > 0)
            {
              content.Value = (JToken) new JArray((object) content.Value.OrderBy<JToken, string>((Func<JToken, string>) (x => x.ToString())));
              for (int key = 0; key < num; ++key)
              {
                if (content.Value[(object) key] is JObject)
                  Functions.Sort((JObject) content.Value[(object) key]);
              }
            }
          }
        }
        else
          Functions.Sort(jObj1);
      }
    }

    [Localizable(false)]
    public static object CreateObject(string programId)
    {
      LogHelper.Debug("Создаю COM-объект: " + programId);
      try
      {
        return Activator.CreateInstance(Type.GetTypeFromProgID(programId, true));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(Translate.Functions_Ошибка_создания_COM_Active_X_объекта___0__1__Возможно__не_установлен_необходимый_драйвер, (object) programId, (object) 46), ex);
      }
    }

    public static bool IsEither<T>(this T obj, IEnumerable<T> variants)
    {
      return obj.IsEither<T>(variants, (IEqualityComparer<T>) EqualityComparer<T>.Default);
    }

    public static bool IsEither<T>(
      this T obj,
      IEnumerable<T> variants,
      IEqualityComparer<T> comparer)
    {
      return variants.Contains<T>(obj, comparer);
    }

    public static bool IsEither<T>(this T obj, params T[] variants)
    {
      return obj.IsEither<T>((IEnumerable<T>) variants);
    }

    public static string ToJsonString(this object obj, bool format = false, bool isIgnoreNull = false)
    {
      try
      {
        return JsonConvert.SerializeObject(Functions.PrepareObjectToJsonString(obj), new JsonSerializerSettings()
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
          Formatting = format ? Formatting.Indented : Formatting.None,
          NullValueHandling = isIgnoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
        });
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось сконвертировать объект в  json string");
        return "";
      }
    }

    public static string TryFormatingJsonString(this string json)
    {
      try
      {
        return JsonConvert.DeserializeObject<JObject>(json).ToJsonString(true);
      }
      catch (Exception ex)
      {
        return json;
      }
    }

    private static object PrepareObjectToJsonString(object obj)
    {
      try
      {
        if (!(obj is BasketItem basketItem1))
          return obj;
        BasketItem jsonString = basketItem1.Clone();
        jsonString.Good.StocksAndPrices = jsonString.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted && x.Stock != 0M)).GroupBy(x => new
        {
          Price = x.Price,
          Uid = x.Storage?.Uid
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType41<Decimal, Guid?>, GoodsStocks.GoodStock>, GoodsStocks.GoodStock>(x =>
        {
          return new GoodsStocks.GoodStock()
          {
            Uid = Guid.Empty,
            Price = x.Key.Price,
            Storage = x.First<GoodsStocks.GoodStock>().Storage,
            Stock = x.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (s => s.Stock))
          };
        }).ToList<GoodsStocks.GoodStock>();
        BasketItem basketItem2 = jsonString;
        Guid? uid = jsonString.Certificate?.Uid;
        Guid empty = Guid.Empty;
        GoodsListItemsCertificate certificate = (uid.HasValue ? (uid.GetValueOrDefault() == empty ? 1 : 0) : 0) != 0 ? (GoodsListItemsCertificate) null : jsonString.Certificate;
        basketItem2.Certificate = certificate;
        return (object) jsonString;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось подготовить объект для записи в json string");
        return (object) "";
      }
    }

    public static string GetSignedNumber(Decimal number)
    {
      return number > 0M ? string.Format("+{0:N2}", (object) number) : number.ToString("N2");
    }

    public static string PadCenter(this string str, int lineLength)
    {
      return str.Length < lineLength ? str.PadLeft((lineLength - str.Length) / 2 + str.Length) : str;
    }

    [Localizable(false)]
    public static Decimal ToDecimal(this string str)
    {
      string sep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      str = ((IEnumerable<string>) new string[2]{ ".", "," }).Aggregate<string, string>(str, (Func<string, string, string>) ((current, s) => current.Replace(s, sep)));
      str = ((IEnumerable<string>) new string[1]{ " " }).Aggregate<string, string>(str, (Func<string, string, string>) ((current, s) => current.Replace(s, string.Empty)));
      Decimal result;
      return !Decimal.TryParse(new Regex("^\\D").Replace(str, string.Empty), out result) ? 0M : result;
    }

    public static string GetOnlyNumbers(this string str)
    {
      return str.Where<char>(new Func<char, bool>(char.IsDigit)).Aggregate<char, string>(string.Empty, (Func<string, char, string>) ((current, t) => current + t.ToString()));
    }
  }
}
