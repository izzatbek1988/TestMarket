// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.PaymentsAccounts
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class PaymentsAccounts
  {
    public static List<PaymentsAccounts.PaymentsAccount> GetPaymentsAccountsList(
      IQueryable<PAYMENTS_ACCOUNT> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<PAYMENTS_ACCOUNT>();
        return query.ToList<PAYMENTS_ACCOUNT>().Select<PAYMENTS_ACCOUNT, PaymentsAccounts.PaymentsAccount>((Func<PAYMENTS_ACCOUNT, PaymentsAccounts.PaymentsAccount>) (u =>
        {
          return new PaymentsAccounts.PaymentsAccount()
          {
            Uid = u.UID,
            Name = u.NAME,
            Type = (PaymentsAccounts.MoneyType) u.TYPE,
            CurrencyCode = u.CURRENCY_CODE,
            IsDeleted = u.IS_DELETED
          };
        })).ToList<PaymentsAccounts.PaymentsAccount>();
      }
    }

    public static PaymentsAccounts.PaymentsAccount GetPaymentsAccountByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => x.UID == uid))).SingleOrDefault<PaymentsAccounts.PaymentsAccount>();
    }

    public static Dictionary<PaymentsAccounts.MoneyType, string> GetDictionaryPaymentType()
    {
      return new Dictionary<PaymentsAccounts.MoneyType, string>()
      {
        {
          PaymentsAccounts.MoneyType.KkmCash,
          Translate.PaymentsAccounts_Наличность_в_ККМ
        },
        {
          PaymentsAccounts.MoneyType.Cash,
          Translate.GlobalDictionaries_Наличные
        },
        {
          PaymentsAccounts.MoneyType.Card,
          Translate.GlobalDictionaries_Карта
        },
        {
          PaymentsAccounts.MoneyType.Bank,
          Translate.PaymentsAccounts_Расчетный_счет
        },
        {
          PaymentsAccounts.MoneyType.EMoney,
          Translate.PaymentsAccounts_Электронные_деньги
        }
      };
    }

    public enum MoneyType
    {
      KkmCash,
      Cash,
      Card,
      Bank,
      EMoney,
    }

    public class PaymentsAccount : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      [Required]
      public PaymentsAccounts.MoneyType Type { get; set; }

      public string CurrencyCode { get; set; } = string.Empty;

      public bool Save()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          dataBase.InsertOrReplace<PAYMENTS_ACCOUNT>(new PAYMENTS_ACCOUNT()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            TYPE = (int) this.Type,
            NAME = this.Name,
            CURRENCY_CODE = this.CurrencyCode
          });
          return true;
        }
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool IsMoneyBox()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          int num;
          if (this.Type == PaymentsAccounts.MoneyType.Cash || this.Type == PaymentsAccounts.MoneyType.KkmCash)
            num = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.SECTION_UID != Guid.Empty))).Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (p => p.AccountUid == this.Uid)) ? 1 : 0;
          else
            num = 0;
          return num != 0;
        }
      }

      public bool IsCurrentMoneyBox()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          ParameterExpression parameterExpression;
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          List<PaymentMethods.PaymentMethod> list = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>(Expression.Lambda<Func<PAYMENT_METHODS, bool>>((Expression) Expression.Equal(x.SECTION_UID, (Expression) Expression.Property((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression))).ToList<PaymentMethods.PaymentMethod>();
          return list.Any<PaymentMethods.PaymentMethod>() && (this.Type == PaymentsAccounts.MoneyType.Cash || this.Type == PaymentsAccounts.MoneyType.KkmCash) && this.Uid == list.First<PaymentMethods.PaymentMethod>().AccountUid;
        }
      }

      public bool IsCurrentAccount()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<PaymentMethods.PaymentMethod> actionPaymentsList = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>());
          return actionPaymentsList.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.AccountUid == this.Uid && x.SectionUid == Sections.GetCurrentSection().Uid)) || actionPaymentsList.All<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.AccountUid != this.Uid)) || actionPaymentsList.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.AccountUid == this.Uid && x.SectionUid == Guid.Empty));
        }
      }
    }
  }
}
