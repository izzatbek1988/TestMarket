// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentMethods.PaymentMethodsViewModel
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentMethods
{
  public class PaymentMethodsViewModel : ViewModelWithForm
  {
    public ObservableCollection<PaymentMethodsViewModel.PaymentMethodView> PaymentMethods { get; set; }

    public PaymentMethodsViewModel.PaymentMethodView SelectedMethod { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public PaymentMethodsViewModel()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.PaymentMethods.PaymentMethod> source = new List<Gbs.Core.Entities.PaymentMethods.PaymentMethod>((IEnumerable<Gbs.Core.Entities.PaymentMethods.PaymentMethod>) Gbs.Core.Entities.PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.IS_DELETED == false && x.UID != GlobalDictionaries.CertificatePaymentUid && x.UID != GlobalDictionaries.BonusesPaymentUid))).OrderBy<Gbs.Core.Entities.PaymentMethods.PaymentMethod, int>((Func<Gbs.Core.Entities.PaymentMethods.PaymentMethod, int>) (x => x.DisplayIndex)));
        List<Sections.Section> sections = Sections.GetSectionsList();
        List<PaymentsAccounts.PaymentsAccount> paymentAccounts = PaymentsAccounts.GetPaymentsAccountsList();
        this.PaymentMethods = new ObservableCollection<PaymentMethodsViewModel.PaymentMethodView>(source.Select<Gbs.Core.Entities.PaymentMethods.PaymentMethod, PaymentMethodsViewModel.PaymentMethodView>((Func<Gbs.Core.Entities.PaymentMethods.PaymentMethod, PaymentMethodsViewModel.PaymentMethodView>) (x =>
        {
          PaymentMethodsViewModel.PaymentMethodView paymentMethodView = new PaymentMethodsViewModel.PaymentMethodView();
          PaymentsAccounts.PaymentsAccount paymentsAccount = paymentAccounts.FirstOrDefault<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (a => a.Uid == x.AccountUid));
          if (paymentsAccount == null)
            paymentsAccount = new PaymentsAccounts.PaymentsAccount()
            {
              Name = Translate.GlobalDictionaries_Не_указано
            };
          paymentMethodView.Account = paymentsAccount;
          paymentMethodView.Section = sections.FirstOrDefault<Sections.Section>((Func<Sections.Section, bool>) (s => s.Uid == x.SectionUid)) ?? new Sections.Section();
          paymentMethodView.Method = x;
          paymentMethodView.TypeMethod = GlobalDictionaries.KkmPaymentMethodsDictionary().First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, bool>) (m => m.Key == x.KkmMethod)).Value;
          return paymentMethodView;
        })));
      }
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.PaymentMethods.PaymentMethod method;
        if (!new FrmCardMethodPayment().ShowCard(Guid.Empty, out method))
          return;
        this.PaymentMethods.Add(new PaymentMethodsViewModel.PaymentMethodView(method));
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (obj != null && ((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.PaymentMethodsViewModel_Может_быть_выбрана_только_одна_запись_для_редактирования);
        }
        else if (this.SelectedMethod != null)
        {
          Gbs.Core.Entities.PaymentMethods.PaymentMethod property;
          if (!new FrmCardMethodPayment().ShowCard(this.SelectedMethod.Method.Uid, out property))
            return;
          this.PaymentMethods[this.PaymentMethods.ToList<PaymentMethodsViewModel.PaymentMethodView>().FindIndex((Predicate<PaymentMethodsViewModel.PaymentMethodView>) (x => x.Method.Uid == property.Uid))] = new PaymentMethodsViewModel.PaymentMethodView(property);
        }
        else
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.PaymentMethodsViewModel_Требуется_выбрать_способ_оплаты, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        List<PaymentMethodsViewModel.PaymentMethodView> list1 = ((IEnumerable) obj).Cast<PaymentMethodsViewModel.PaymentMethodView>().ToList<PaymentMethodsViewModel.PaymentMethodView>();
        if (this.SelectedMethod != null)
        {
          if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list1.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          List<Sections.Section> sections = Sections.GetSectionsList();
          List<PaymentMethodsViewModel.PaymentMethodView> list2 = list1.Where<PaymentMethodsViewModel.PaymentMethodView>((Func<PaymentMethodsViewModel.PaymentMethodView, bool>) (x =>
          {
            if (!(x.Method.SectionUid != Guid.Empty))
              return false;
            Sections.Section section = sections.SingleOrDefault<Sections.Section>((Func<Sections.Section, bool>) (s => s.Uid == x.Method.SectionUid));
            // ISSUE: explicit non-virtual call
            return section != null && !__nonvirtual (section.IsDeleted);
          })).ToList<PaymentMethodsViewModel.PaymentMethodView>();
          if (list2.Any<PaymentMethodsViewModel.PaymentMethodView>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.PaymentMethodsViewModel_ + string.Join("\n", list2.Select<PaymentMethodsViewModel.PaymentMethodView, string>((Func<PaymentMethodsViewModel.PaymentMethodView, string>) (x => x.Method.Name + " (" + x.Section.Name + ")"))), icon: MessageBoxImage.Exclamation);
          }
          foreach (PaymentMethodsViewModel.PaymentMethodView paymentMethodView in list1)
          {
            PaymentMethodsViewModel.PaymentMethodView m = paymentMethodView;
            if (!list2.Any<PaymentMethodsViewModel.PaymentMethodView>((Func<PaymentMethodsViewModel.PaymentMethodView, bool>) (x => x.Method.Uid == m.Method.Uid)))
            {
              m.Method.IsDeleted = true;
              m.Method.Save();
              this.PaymentMethods.Remove(m);
            }
          }
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.PaymentMethodsViewModel_Требуется_выбрать_способ_оплаты, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
    }

    public class PaymentMethodView
    {
      public Gbs.Core.Entities.PaymentMethods.PaymentMethod Method { get; set; }

      public Sections.Section Section { get; set; }

      public PaymentsAccounts.PaymentsAccount Account { get; set; }

      public string TypeMethod { get; set; }

      public PaymentMethodView(Gbs.Core.Entities.PaymentMethods.PaymentMethod method)
      {
        this.TypeMethod = GlobalDictionaries.KkmPaymentMethodsDictionary().First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, string>, bool>) (m => m.Key == method.KkmMethod)).Value;
        this.Section = Sections.GetSectionByUid(method.SectionUid) ?? new Sections.Section();
        PaymentsAccounts.PaymentsAccount paymentsAccount = PaymentsAccounts.GetPaymentsAccountByUid(method.AccountUid);
        if (paymentsAccount == null)
          paymentsAccount = new PaymentsAccounts.PaymentsAccount()
          {
            Name = Translate.GlobalDictionaries_Не_указано
          };
        this.Account = paymentsAccount;
        this.Method = method;
      }

      public PaymentMethodView()
      {
      }
    }
  }
}
