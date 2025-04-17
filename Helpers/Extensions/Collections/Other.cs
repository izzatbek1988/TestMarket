// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Extensions.Collections.Other
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Extensions.Collections
{
  internal static class Other
  {
    public static IEnumerable<TSource> GetDuplicate<TSource, TKey>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> selector)
    {
      return source.GroupBy<TSource, TKey>(selector).Where<IGrouping<TKey, TSource>>((Func<IGrouping<TKey, TSource>, bool>) (i => i.Count<TSource>() > 1)).SelectMany<IGrouping<TKey, TSource>, TSource>((Func<IGrouping<TKey, TSource>, IEnumerable<TSource>>) (i => (IEnumerable<TSource>) i));
    }

    public static IEnumerable<IEnumerable<T>> ToChunks<T>(
      this IEnumerable<T> source,
      int chunksCount)
    {
      int chunkLength = (int) Math.Ceiling((double) source.Count<T>() / (double) chunksCount);
      return Enumerable.Range(0, chunksCount).Select<int, IEnumerable<T>>((Func<int, IEnumerable<T>>) (i => source.Skip<T>(i * chunkLength).Take<T>(chunkLength)));
    }
  }
}
