// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.TypePaymentViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class TypePaymentViewModel : ViewModelWithForm
  {
    public Action Close { get; set; }

    public ICommand Save { get; set; }

    public List<TypePaymentViewModel.Payment> ListPayment { get; set; }

    public TypePaymentViewModel()
    {
    }

    public TypePaymentViewModel(FiscalKkm kkmConfig)
    {
      TypePaymentViewModel paymentViewModel = this;
      this.ListPayment = new List<TypePaymentViewModel.Payment>();
      foreach (KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string> keyValuePair in GlobalDictionaries.KkmPaymentMethodsDictionary().Where<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, bool>) (x => x.Key != GlobalDictionaries.KkmPaymentMethods.Bonus && x.Key != GlobalDictionaries.KkmPaymentMethods.Certificate)))
      {
        KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string> payment = keyValuePair;
        int num = kkmConfig.PaymentsMethods.Any<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Key)) ? kkmConfig.PaymentsMethods.First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Key)).Value : (int) payment.Key;
        this.ListPayment.Add(new TypePaymentViewModel.Payment()
        {
          Types = payment.Key,
          NamePayment = payment.Value,
          Number = num
        });
      }
      this.Save = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        kkmConfig.PaymentsMethods = new Dictionary<GlobalDictionaries.KkmPaymentMethods, int>();
        foreach (TypePaymentViewModel.Payment payment in paymentViewModel.ListPayment)
          kkmConfig.PaymentsMethods.Add(payment.Types, payment.Number);
        paymentViewModel.Close();
      }));
    }

    public class Payment
    {
      public GlobalDictionaries.KkmPaymentMethods Types { get; set; }

      public string NamePayment { get; set; }

      public int Number { get; set; }
    }
  }
}
