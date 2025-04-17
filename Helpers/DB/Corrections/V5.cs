// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V5
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Helpers.Logging;
using LinqToDB;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V5 : ICorrection
  {
    public bool Do()
    {
      try
      {
        LogHelper.OnBegin();
        using (DataBase dataBase = Data.GetDataBase())
        {
          IQueryable<PAYMENTS> table = dataBase.GetTable<PAYMENTS>();
          Expression<Func<PAYMENTS, bool>> predicate = (Expression<Func<PAYMENTS, bool>>) (x => x.TYPE == 3 && x.SUM_IN > 0M);
          foreach (PAYMENTS payments in table.Where<PAYMENTS>(predicate).ToList<PAYMENTS>())
          {
            PAYMENTS payment = payments;
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            dataBase.Connection.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.UID == payment.UID)).Update<PAYMENTS>(Expression.Lambda<Func<PAYMENTS, PAYMENTS>>((Expression) Expression.MemberInit(Expression.New(typeof (PAYMENTS)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.set_ACCOUNT_IN_UID)), )))); // Unable to render the statement
          }
        }
        LogHelper.OnEnd();
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка корректировки БД");
        return false;
      }
    }
  }
}
