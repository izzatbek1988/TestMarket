// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.BoolToStringConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Db
{
  public class BoolToStringConverter : ValueConverter<bool, string>
  {
    public BoolToStringConverter()
    {
      ParameterExpression parameterExpression;
      // ISSUE: method reference
      // ISSUE: explicit constructor call
      base.\u002Ector((Expression<Func<bool, string>>) (b => b ? "true" : "false"), Expression.Lambda<Func<string, bool>>((Expression) Expression.Equal((Expression) Expression.Call(s, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), Array.Empty<Expression>()), (Expression) Expression.Constant((object) "true", typeof (string))), parameterExpression), false);
    }
  }
}
