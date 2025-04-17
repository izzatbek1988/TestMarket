// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Helpers.PagingHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.API.GbsApi.v1.Entities;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Helpers
{
  public static class PagingHelper
  {
    public static PagingHelper.PagerInfo GetPage(IEnumerable<IEntity> DataToFiltered, Uri uri)
    {
      List<IEntity> list = DataToFiltered.ToList<IEntity>();
      Dictionary<string, string> dictionary = uri.DecodeQueryParameters();
      int num1 = 1;
      if (dictionary.ContainsKey("page"))
        num1 = int.Parse(dictionary["page"]);
      int count1 = 100;
      if (dictionary.ContainsKey("page_size"))
        count1 = int.Parse(dictionary["page_size"]);
      int count2 = list.Count;
      int num2 = (int) Math.Ceiling((double) count2 * 1.0 / (double) count1);
      if (num1 > num2)
        throw new ArgumentOutOfRangeException("page", string.Format(Translate.PagingHelper_GetPage_Страницы_с_заданным_номером_не_существует__Всего_страниц__0_, (object) num2));
      return new PagingHelper.PagerInfo()
      {
        CurrentPage = num1,
        FilteredItems = list.Skip<IEntity>((num1 - 1) * count1).Take<IEntity>(count1).ToList<IEntity>(),
        TotalItems = count2,
        PageSize = count1,
        TotalPages = num2
      };
    }

    public class PagerInfo
    {
      public int TotalPages { get; set; }

      public int CurrentPage { get; set; }

      public int PageSize { get; set; }

      public int TotalItems { get; set; }

      public List<IEntity> FilteredItems { get; set; }
    }
  }
}
