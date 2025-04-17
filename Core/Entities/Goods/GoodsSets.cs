// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsSets
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsSets
  {
    public static List<GoodsSets.Set> GetSetsFilteredList(IQueryable<GOODS_SETS> query)
    {
      ParameterExpression parameterExpression;
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      return query.Select<GOODS_SETS, GoodsSets.Set>(Expression.Lambda<Func<GOODS_SETS, GoodsSets.Set>>((Expression) Expression.MemberInit(Expression.New(typeof (GoodsSets.Set)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GoodsSets.Set.set_Uid)), )))); // Unable to render the statement
    }

    public class Set
    {
      public Guid Uid { get; set; } = Guid.NewGuid();

      public Guid GoodUid { get; set; } = Guid.Empty;

      public Guid ModificationUid { get; set; } = Guid.Empty;

      public Guid ParentUid { get; set; } = Guid.Empty;

      public Decimal Quantity { get; set; }

      public Decimal Discount { get; set; }

      public bool IsDeleted { get; set; }
    }
  }
}
