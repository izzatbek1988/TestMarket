// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.PaymentByMethodsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public partial class PaymentByMethodsViewModel : ViewModelWithForm
  {
    public List<PaymentByMethodsViewModel.PaymentsView> PaymentsViews { get; set; } = new List<PaymentByMethodsViewModel.PaymentsView>();

    public Decimal SumPayments
    {
      get
      {
        return this.PaymentsViews.Sum<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, Decimal>) (x => x.Sum));
      }
    }

    public Decimal SumSalePayments
    {
      get
      {
        return this.PaymentsViews.Sum<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, Decimal>) (x => x.SaleSum));
      }
    }

    public Decimal SumReturnPayments
    {
      get
      {
        return this.PaymentsViews.Sum<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, Decimal>) (x => x.ReturnSum));
      }
    }

    public PaymentByMethodsViewModel()
    {
    }

    public PaymentByMethodsViewModel(List<Gbs.Core.Entities.Payments.Payment> payments)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.PaymentsViews = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>()).Select<PaymentMethods.PaymentMethod, PaymentByMethodsViewModel.PaymentsView>((Func<PaymentMethods.PaymentMethod, PaymentByMethodsViewModel.PaymentsView>) (x => new PaymentByMethodsViewModel.PaymentsView()
        {
          Method = x
        })).ToList<PaymentByMethodsViewModel.PaymentsView>();
        List<PaymentByMethodsViewModel.PaymentsView> paymentsViews = this.PaymentsViews;
        PaymentByMethodsViewModel.PaymentsView paymentsView = new PaymentByMethodsViewModel.PaymentsView();
        PaymentMethods.PaymentMethod paymentMethod = new PaymentMethods.PaymentMethod();
        paymentMethod.Uid = Guid.Empty;
        paymentMethod.Name = Translate.PaymentByMethodsViewModel_Не_определен;
        paymentsView.Method = paymentMethod;
        paymentsViews.Add(paymentsView);
      }
      payments = new List<Gbs.Core.Entities.Payments.Payment>((IEnumerable<Gbs.Core.Entities.Payments.Payment>) payments);
      foreach (Gbs.Core.Entities.Payments.Payment payment1 in payments)
      {
        Gbs.Core.Entities.Payments.Payment payment = payment1;
        int index = this.PaymentsViews.FindIndex((Predicate<PaymentByMethodsViewModel.PaymentsView>) (x =>
        {
          Guid uid = x.Method.Uid;
          PaymentMethods.PaymentMethod method = payment.Method;
          // ISSUE: explicit non-virtual call
          Guid guid = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
          return uid == guid;
        }));
        this.PaymentsViews[index].Sum += payment.SumIn - payment.SumOut;
        this.PaymentsViews[index].ReturnSum -= payment.SumOut;
        this.PaymentsViews[index].SaleSum += payment.SumIn;
      }
      List<PaymentByMethodsViewModel.PaymentsView> source = new List<PaymentByMethodsViewModel.PaymentsView>();
      source.AddRange((IEnumerable<PaymentByMethodsViewModel.PaymentsView>) this.PaymentsViews.Where<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, bool>) (x =>
      {
        if (x.Method.DisplayIndex <= 0)
          return false;
        return !x.Method.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid);
      })).OrderBy<PaymentByMethodsViewModel.PaymentsView, int>((Func<PaymentByMethodsViewModel.PaymentsView, int>) (x => x.Method.DisplayIndex)).ToList<PaymentByMethodsViewModel.PaymentsView>());
      source.AddRange((IEnumerable<PaymentByMethodsViewModel.PaymentsView>) this.PaymentsViews.Where<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, bool>) (x =>
      {
        if (x.Method.DisplayIndex != 0)
          return false;
        return !x.Method.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid);
      })).OrderBy<PaymentByMethodsViewModel.PaymentsView, string>((Func<PaymentByMethodsViewModel.PaymentsView, string>) (x => x.Method.Name)));
      source.AddRange(this.PaymentsViews.Where<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, bool>) (x => x.Method.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid))));
      this.PaymentsViews = new List<PaymentByMethodsViewModel.PaymentsView>(source.Where<PaymentByMethodsViewModel.PaymentsView>((Func<PaymentByMethodsViewModel.PaymentsView, bool>) (x => x.Sum != 0M)));
    }

    public class PaymentsView
    {
      public PaymentMethods.PaymentMethod Method { get; set; }

      public Decimal SaleSum { get; set; }

      public Decimal ReturnSum { get; set; }

      public Decimal Sum { get; set; }
    }
  }
}
