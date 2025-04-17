// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.PaymentAccountsAndSumViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Forms.ActionsPayments;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public partial class PaymentAccountsAndSumViewModel : ViewModelWithForm
  {
    private Gbs.Core.Entities.Users.User _authUser;

    public Decimal TotalSum { get; set; }

    public ObservableCollection<PaymentAccountsAndSumViewModel.AccountPaymentSum> AccountPayments { get; set; } = new ObservableCollection<PaymentAccountsAndSumViewModel.AccountPaymentSum>();

    public void ShowSumAccounts(Gbs.Core.Entities.Users.User user)
    {
      this._authUser = user;
      this.LoadingData();
      this.FormToSHow = (WindowWithSize) new FrmPaymentAccountSum();
      this.ShowForm();
    }

    public ICommand EditSumCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            List<PaymentAccountsAndSumViewModel.AccountPaymentSum> list = ((IEnumerable) obj).Cast<PaymentAccountsAndSumViewModel.AccountPaymentSum>().ToList<PaymentAccountsAndSumViewModel.AccountPaymentSum>();
            if (!list.Any<PaymentAccountsAndSumViewModel.AccountPaymentSum>())
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.PaymentAccountsAndSumViewModel_Необходимо_выбрать_счет_для_изменения_суммы);
            }
            else if (list.Count > 1)
            {
              int num3 = (int) MessageBoxHelper.Show(Translate.PaymentAccountsAndSumViewModel_Необходимо_выбрать_только_один_счет_для_изменения_суммы);
            }
            else
            {
              PaymentAccountsAndSumViewModel.AccountPaymentSum accountPaymentSum = list.Single<PaymentAccountsAndSumViewModel.AccountPaymentSum>();
              new FrmRemoveCash().CorrectSumByAccount(accountPaymentSum.Sum, PaymentsActionsViewModel.ActionsPayment.Correct, accountPaymentSum.Account, false, this._authUser, true);
              this.LoadingData();
            }
          }
        }));
      }
    }

    public static List<PaymentAccountsAndSumViewModel.AccountPaymentSum> GetAccountsSum()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<PaymentsAccounts.PaymentsAccount> list = PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => !x.IS_DELETED))).ToList<PaymentsAccounts.PaymentsAccount>();
        List<PaymentAccountsAndSumViewModel.AccountPaymentSum> accountsSum = new List<PaymentAccountsAndSumViewModel.AccountPaymentSum>();
        foreach (PaymentsAccounts.PaymentsAccount paymentsAccount in list)
          accountsSum.Add(new PaymentAccountsAndSumViewModel.AccountPaymentSum()
          {
            Account = paymentsAccount,
            Sum = PaymentAccountsAndSumViewModel.GetSumForAccount(paymentsAccount.Uid)
          });
        return accountsSum;
      }
    }

    public static Decimal GetSumForAccount(Guid uid, DateTime? date = null)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<PAYMENTS> source = dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED && x.TYPE != 7));
        IQueryable<PAYMENTS> query;
        if (uid != Guid.Empty)
          query = source.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.ACCOUNT_IN_UID == uid || x.ACCOUNT_OUT_UID == uid));
        else
          query = source.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.ACCOUNT_IN_UID != Guid.Empty || x.ACCOUNT_OUT_UID != Guid.Empty));
        List<Gbs.Core.Entities.Payments.Payment> list = Gbs.Core.Entities.Payments.GetPaymentsList(query).ToList<Gbs.Core.Entities.Payments.Payment>();
        if (date.HasValue)
          list = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Date <= date.Value)).ToList<Gbs.Core.Entities.Payments.Payment>();
        Decimal num1;
        Decimal num2;
        if (uid == Guid.Empty)
        {
          num1 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            Guid? uid1 = x.AccountIn?.Uid;
            Guid empty = Guid.Empty;
            return !uid1.HasValue || uid1.GetValueOrDefault() != empty;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
          num2 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            Guid? uid2 = x.AccountOut?.Uid;
            Guid empty = Guid.Empty;
            return !uid2.HasValue || uid2.GetValueOrDefault() != empty;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        }
        else
        {
          num1 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            PaymentsAccounts.PaymentsAccount accountIn = x.AccountIn;
            // ISSUE: explicit non-virtual call
            return accountIn != null && __nonvirtual (accountIn.Uid) == uid;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
          num2 = list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            PaymentsAccounts.PaymentsAccount accountOut = x.AccountOut;
            // ISSUE: explicit non-virtual call
            return accountOut != null && __nonvirtual (accountOut.Uid) == uid;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        }
        return num1 - num2;
      }
    }

    private void LoadingData()
    {
      this.AccountPayments = new ObservableCollection<PaymentAccountsAndSumViewModel.AccountPaymentSum>(PaymentAccountsAndSumViewModel.GetAccountsSum());
      this.TotalSum = this.AccountPayments.Sum<PaymentAccountsAndSumViewModel.AccountPaymentSum>((Func<PaymentAccountsAndSumViewModel.AccountPaymentSum, Decimal>) (x => x.Sum));
      this.OnPropertyChanged("AccountPayments");
      this.OnPropertyChanged("TotalSum");
    }

    public class AccountPaymentSum
    {
      public PaymentsAccounts.PaymentsAccount Account { get; set; }

      public Decimal Sum { get; set; }
    }
  }
}
