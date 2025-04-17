// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentsAccount.PaymentAccountCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentsAccount
{
  public partial class PaymentAccountCardModelView : ViewModelWithForm
  {
    public bool IsEnabledType { get; set; }

    public PaymentsAccounts.PaymentsAccount Account { get; set; }

    public Dictionary<PaymentsAccounts.MoneyType, string> PaymentType { get; set; } = PaymentsAccounts.GetDictionaryPaymentType();

    public Action CloseCardAction { private get; set; }

    public ICommand SaveCommand { get; set; }

    public bool SaveResult { get; private set; }

    public PaymentAccountCardModelView()
    {
    }

    public PaymentAccountCardModelView(PaymentsAccounts.PaymentsAccount account)
    {
      this.Account = account;
      this.SaveCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    private void Save()
    {
      this.SaveResult = this.Account.Save();
      if (!this.SaveResult)
        return;
      this.CloseCardAction();
    }
  }
}
