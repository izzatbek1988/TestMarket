// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.UriHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1
{
  public static class UriHelper
  {
    public static Dictionary<string, string> DecodeQueryParameters(this Uri uri)
    {
      if (uri == (Uri) null)
        throw new ArgumentNullException(nameof (uri));
      if (uri.Query.Length == 0)
        return new Dictionary<string, string>();
      return ((IEnumerable<string>) uri.Query.TrimStart('?').Split(new char[2]
      {
        '&',
        ';'
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string[]>((Func<string, string[]>) (parameter => parameter.Split(new char[1]
      {
        '='
      }, StringSplitOptions.RemoveEmptyEntries))).GroupBy<string[], string, string>((Func<string[], string>) (parts => parts[0]), (Func<string[], string>) (parts =>
      {
        if (parts.Length > 2)
          return string.Join("=", parts, 1, parts.Length - 1);
        return parts.Length <= 1 ? "" : parts[1];
      })).ToDictionary<IGrouping<string, string>, string, string>((Func<IGrouping<string, string>, string>) (grouping => grouping.Key), (Func<IGrouping<string, string>, string>) (grouping => string.Join(",", (IEnumerable<string>) grouping)));
    }
  }
}
