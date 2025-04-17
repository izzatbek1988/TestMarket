// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ReCalcCashAccountHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.ActionsPayments;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public static class ReCalcCashAccountHelper
  {
    public static bool IsReCalcCashAccountNow
    {
      get
      {
        Settings settings = new ConfigsRepository<Settings>().Get();
        if (!settings.Payments.IsReCalcCashAccount)
          return false;
        DateTime dateTime = DateTime.Now;
        int day1 = dateTime.Day;
        dateTime = settings.Payments.DateTimeReCalcCashAccount;
        int day2 = dateTime.Day;
        return day1 != day2;
      }
    }

    public static void DoReCalcCashAccount(bool isShow, Gbs.Core.Entities.Users.User authUser = null)
    {
      if (!ReCalcCashAccountHelper.IsReCalcCashAccountNow && !isShow)
        return;
      IEnumerable<PaymentMethods.PaymentMethod> methods = PaymentMethods.GetActionPaymentsList().Where<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == Sections.GetCurrentSection().Uid && !x.IsDeleted));
      IEnumerable<PaymentsAccounts.PaymentsAccount> source = PaymentsAccounts.GetPaymentsAccountsList().Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted && methods.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (m => m.AccountUid == x.Uid))));
      Decimal sumCash = SaleHelper.GetSumCash(new Guid?(Sections.GetCurrentSection().Uid), (List<Gbs.Core.Entities.Payments.Payment>) null);
      new FrmRemoveCash().CorrectSumByAccount(0M, PaymentsActionsViewModel.ActionsPayment.RecountSumCash, source.First<PaymentsAccounts.PaymentsAccount>(), false, authUser, isNonCancel: !isShow, oldSum: new Decimal?(sumCash));
      if (isShow)
        return;
      Settings config = new ConfigsRepository<Settings>().Get();
      config.Payments.DateTimeReCalcCashAccount = DateTime.Now;
      new ConfigsRepository<Settings>().Save(config);
    }
  }
}
