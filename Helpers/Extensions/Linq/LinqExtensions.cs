// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Extensions.Linq.LinqExtensions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Extensions.Linq
{
  public static class LinqExtensions
  {
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      HashSet<TKey> seenKeys = new HashSet<TKey>();
      foreach (TSource source1 in source.Where<TSource>((Func<TSource, bool>) (element => seenKeys.Add(keySelector(element)))))
        yield return source1;
    }

    public static TSource SingleOrNull<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate = null)
      where TSource : class
    {
      if (predicate != null)
        source = source.Where<TSource>(predicate);
      source = (IEnumerable<TSource>) source.ToList<TSource>();
      return source.Count<TSource>() != 1 ? default (TSource) : source.Single<TSource>();
    }
  }
}
