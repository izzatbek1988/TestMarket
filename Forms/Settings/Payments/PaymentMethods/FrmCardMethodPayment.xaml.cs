// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.MethodsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Payments
{
  public partial class MethodsViewModel : ViewModelWithForm
  {
    private bool _isEnabledIndex;

    public bool SaveResult { get; set; }

    public bool IsEnabledIndex
    {
      get => this._isEnabledIndex;
      set
      {
        this._isEnabledIndex = value;
        if (!value)
          this.Method.DisplayIndex = 0;
        this.OnPropertyChanged(nameof (IsEnabledIndex));
        this.OnPropertyChanged("Method");
      }
    }

    public ICommand SaveMethod { get; set; }

    public Action Close { private get; set; }

    public PaymentMethods.PaymentMethod Method { get; set; }

    public List<PaymentsAccounts.PaymentsAccount> Accounts { get; set; }

    public Dictionary<GlobalDictionaries.KkmPaymentMethods, string> KkmPaymentMethods { get; set; }

    public Dictionary<GlobalDictionaries.PaymentMethodsType, string> PaymentMethodsTypes
    {
      get
      {
        Dictionary<GlobalDictionaries.PaymentMethodsType, string> source = GlobalDictionaries.PaymentMethodsTypeDictionary();
        if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
          source = source.Where<KeyValuePair<GlobalDictionaries.PaymentMethodsType, string>>((Func<KeyValuePair<GlobalDictionaries.PaymentMethodsType, string>, bool>) (x => x.Key != GlobalDictionaries.PaymentMethodsType.Sbp)).ToDictionary<KeyValuePair<GlobalDictionaries.PaymentMethodsType, string>, GlobalDictionaries.PaymentMethodsType, string>((Func<KeyValuePair<GlobalDictionaries.PaymentMethodsType, string>, GlobalDictionaries.PaymentMethodsType>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.PaymentMethodsType, string>, string>) (x => x.Value));
        return source;
      }
    }

    public MethodsViewModel()
    {
    }

    public GlobalDictionaries.KkmPaymentMethods KkmMethod
    {
      get
      {
        PaymentMethods.PaymentMethod method = this.Method;
        return method == null ? GlobalDictionaries.KkmPaymentMethods.Cash : method.KkmMethod;
      }
      set
      {
        this.Method.KkmMethod = value;
        this.OnPropertyChanged("TypeMethodVisibility");
      }
    }

    public Visibility TypeMethodVisibility
    {
      get
      {
        return this.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Cash ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public MethodsViewModel(PaymentMethods.PaymentMethod method)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.Method = method;
        this.IsEnabledIndex = method.DisplayIndex != 0;
        List<PaymentsAccounts.PaymentsAccount> paymentsAccountList = new List<PaymentsAccounts.PaymentsAccount>();
        PaymentsAccounts.PaymentsAccount paymentsAccount = new PaymentsAccounts.PaymentsAccount();
        paymentsAccount.Uid = Guid.Empty;
        paymentsAccount.Name = Translate.PaymentsActionsViewModel_Не_указан;
        paymentsAccountList.Add(paymentsAccount);
        this.Accounts = paymentsAccountList;
        this.Accounts.AddRange((IEnumerable<PaymentsAccounts.PaymentsAccount>) PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => x.IS_DELETED == false))));
      }
      this.KkmPaymentMethods = GlobalDictionaries.KkmPaymentMethodsDictionary().Where<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, bool>) (x => x.Key != GlobalDictionaries.KkmPaymentMethods.Bonus && x.Key != GlobalDictionaries.KkmPaymentMethods.Certificate && x.Key != GlobalDictionaries.KkmPaymentMethods.Credit && x.Key != GlobalDictionaries.KkmPaymentMethods.PrePayment)).ToDictionary<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, GlobalDictionaries.KkmPaymentMethods, string>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, GlobalDictionaries.KkmPaymentMethods>) (x => x.Key), (Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, string>) (x => x.Value));
      this.SaveMethod = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.Method.SectionUid != Guid.Empty && this.Method.AccountUid == Guid.Empty)
        {
          int num = (int) MessageBoxHelper.Show(Translate.MethodsViewModel_Невозможно_сохранить_данный_способ_оплаты_без_указания_счета_);
        }
        else
        {
          this.SaveResult = this.Method.Save();
          if (!this.SaveResult)
            return;
          this.Close();
        }
      }));
    }
  }
}
