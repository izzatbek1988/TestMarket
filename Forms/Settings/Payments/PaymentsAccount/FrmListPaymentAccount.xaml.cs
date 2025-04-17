// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Payments.PaymentsAccount.PaymentAccountListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Payments.PaymentsAccount;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Payments.PaymentsAccount
{
  public partial class PaymentAccountListViewModel : ViewModelWithForm
  {
    public ObservableCollection<PaymentsAccounts.PaymentsAccount> AccountList { get; set; }

    public PaymentsAccounts.PaymentsAccount SelectedAccount { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public PaymentAccountListViewModel()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
          this.AccountList = new ObservableCollection<PaymentsAccounts.PaymentsAccount>(PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => x.IS_DELETED == false))));
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          PaymentsAccounts.PaymentsAccount account;
          if (!new FrmCardPaymentAccount().ShowCard(Guid.Empty, out account))
            return;
          this.AccountList.Add(account);
        }));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            if (this.SelectedAccount == null)
              MessageBoxHelper.Warning(Translate.PaymentAccountListViewModel_Сначала_нужно_выбрать_счет);
            else if (((IEnumerable) obj).Cast<PaymentsAccounts.PaymentsAccount>().ToList<PaymentsAccounts.PaymentsAccount>().Count > 1)
              MessageBoxHelper.Warning(Translate.PaymentAccountListViewModel_Требуется_выбрать_только_один_счет_для_редактирования_);
            else if (PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.ACCOUNT_UID == this.SelectedAccount.Uid && x.SECTION_UID != Guid.Empty))).Any<PaymentMethods.PaymentMethod>())
            {
              PaymentsAccounts.PaymentsAccount accountM;
              if (!new FrmCardPaymentAccount().ShowCard(this.SelectedAccount.Uid, out accountM, false))
                return;
              this.AccountList[this.AccountList.ToList<PaymentsAccounts.PaymentsAccount>().FindIndex((Predicate<PaymentsAccounts.PaymentsAccount>) (x => x.Uid == accountM.Uid))] = accountM;
            }
            else
            {
              PaymentsAccounts.PaymentsAccount account;
              if (!new FrmCardPaymentAccount().ShowCard(this.SelectedAccount.Uid, out account))
                return;
              this.AccountList[this.AccountList.ToList<PaymentsAccounts.PaymentsAccount>().FindIndex((Predicate<PaymentsAccounts.PaymentsAccount>) (x => x.Uid == account.Uid))] = account;
            }
          }
        }));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            PaymentAccountListViewModel.\u003C\u003Ec__DisplayClass20_2 cDisplayClass202 = new PaymentAccountListViewModel.\u003C\u003Ec__DisplayClass20_2();
            if (this.SelectedAccount == null || obj == null)
            {
              MessageBoxHelper.Warning(Translate.PaymentAccountListViewModel_Сначала_нужно_выбрать_счет);
            }
            else
            {
              List<PaymentsAccounts.PaymentsAccount> list = ((IEnumerable) obj).Cast<PaymentsAccounts.PaymentsAccount>().ToList<PaymentsAccounts.PaymentsAccount>();
              if (MessageBoxHelper.Show(string.Format(Translate.PaymentAccountListViewModel_Вы_уверены__что_хотите_удалить__0__счета_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              List<string> values = new List<string>()
              {
                Translate.PaymentAccountListViewModel_PaymentAccountListViewModel_
              };
              IQueryable<SECTIONS> queryable = dataBase.GetTable<SECTIONS>().Where<SECTIONS>((Expression<Func<SECTIONS, bool>>) (x => x.IS_DELETED == false));
              // ISSUE: reference to a compiler-generated field
              cDisplayClass202.sections = queryable;
              foreach (PaymentsAccounts.PaymentsAccount paymentsAccount in list)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                PaymentAccountListViewModel.\u003C\u003Ec__DisplayClass20_3 cDisplayClass203 = new PaymentAccountListViewModel.\u003C\u003Ec__DisplayClass20_3();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass203.CS\u0024\u003C\u003E8__locals1 = cDisplayClass202;
                // ISSUE: reference to a compiler-generated field
                cDisplayClass203.acc = paymentsAccount;
                ParameterExpression parameterExpression1;
                ParameterExpression parameterExpression2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: method reference
                // ISSUE: method reference
                if (PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.ACCOUNT_UID == cDisplayClass203.acc.Uid && x.SECTION_UID != Guid.Empty && !x.IS_DELETED && cDisplayClass203.CS\u0024\u003C\u003E8__locals1.sections.Any<SECTIONS>(System.Linq.Expressions.Expression.Lambda<Func<SECTIONS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(s.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENT_METHODS.get_SECTION_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))))).Any<PaymentMethods.PaymentMethod>())
                {
                  // ISSUE: reference to a compiler-generated field
                  values.Add(cDisplayClass203.acc.Name);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass203.acc.IsDeleted = true;
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass203.acc.Save();
                  // ISSUE: reference to a compiler-generated field
                  this.AccountList.Remove(cDisplayClass203.acc);
                }
              }
              if (values.Count <= 1)
                return;
              MessageBoxHelper.Warning(string.Join("\n", (IEnumerable<string>) values));
            }
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка счетов для оплаты");
      }
    }
  }
}
