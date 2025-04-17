// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.PaymentMethods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class PaymentMethods
  {
    public static List<PaymentMethods.PaymentMethod> GetActionPaymentsList(
      IQueryable<PAYMENT_METHODS> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<PAYMENT_METHODS>();
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        return query.Select<PAYMENT_METHODS, PaymentMethods.PaymentMethod>(Expression.Lambda<Func<PAYMENT_METHODS, PaymentMethods.PaymentMethod>>((Expression) Expression.MemberInit(Expression.New(typeof (PaymentMethods.PaymentMethod)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.set_Uid)), )))); // Unable to render the statement
      }
    }

    public static PaymentMethods.PaymentMethod GetPaymentMethodByUid(Guid uid)
    {
      Other.ConsoleWrite("Загрузка способа оплаты по УИД");
      using (DataBase dataBase = Data.GetDataBase())
        return PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.UID == uid))).Single<PaymentMethods.PaymentMethod>();
    }

    public class PaymentMethod : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; }

      public Guid AccountUid { get; set; }

      public int DisplayIndex { get; set; }

      public Guid SectionUid { get; set; } = Guid.Empty;

      public GlobalDictionaries.KkmPaymentMethods KkmMethod { get; set; }

      public GlobalDictionaries.PaymentMethodsType PaymentMethodsType { get; set; }

      public bool Save()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          dataBase.InsertOrReplace<PAYMENT_METHODS>(new PAYMENT_METHODS()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            NAME = this.Name,
            DISPLAY_INDEX = this.DisplayIndex,
            ACCOUNT_UID = this.AccountUid,
            SECTION_UID = this.SectionUid,
            KKM_METHOD = (int) this.KkmMethod,
            TYPE_METHOD = (int) this.PaymentMethodsType
          });
          return true;
        }
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();
    }
  }
}
