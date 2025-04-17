// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PaymentsPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms.Payments.PaymentsAccount;
using Gbs.Forms.Payments.PaymentsGroups;
using Gbs.Forms.Settings.Payments.PaymentMethods;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class PaymentsPageViewModel : ViewModelWithForm
  {
    private ObservableCollection<Gbs.Core.Entities.PaymentMethods.PaymentMethod> _methods = new ObservableCollection<Gbs.Core.Entities.PaymentMethods.PaymentMethod>();
    private Gbs.Core.Config.Settings _settings;
    private Action _loadingPaymentMethodsForDiscount;

    public ObservableCollection<Gbs.Core.Entities.PaymentMethods.PaymentMethod> Methods
    {
      get => this._methods;
      set
      {
        this._methods = value;
        this.OnPropertyChanged(nameof (Methods));
      }
    }

    public Guid DefaultMethodPaymentUid
    {
      get => (Guid?) this._settings?.Payments?.DefaultMethodPaymentUid ?? Guid.Empty;
      set
      {
        this._settings.Payments.DefaultMethodPaymentUid = new Guid?(value);
        this.OnPropertyChanged(nameof (DefaultMethodPaymentUid));
      }
    }

    public Visibility VisibilityDebug
    {
      get => !DevelopersHelper.IsDebug() ? Visibility.Collapsed : Visibility.Visible;
    }

    public Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public ICommand ShowPaymentMethods { get; set; }

    public ICommand ShowGroupPayments { get; set; }

    public ICommand ShowAccountPayments { get; set; }

    public PaymentsPageViewModel()
    {
    }

    public PaymentsPageViewModel(Gbs.Core.Config.Settings settings, Action loadingPaymentMethodsForDiscount)
    {
      this.Settings = settings;
      this._loadingPaymentMethodsForDiscount = loadingPaymentMethodsForDiscount;
      this.LoadingPaymentMethods();
      this.ShowPaymentMethods = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        new FrmPaymentMethods().ShowDialog();
        this.LoadingPaymentMethods();
        this._loadingPaymentMethodsForDiscount();
      }));
      this.ShowGroupPayments = (ICommand) new RelayCommand((Action<object>) (obj => new FrmListPaymentGroup().ShowDialog()));
      this.ShowAccountPayments = (ICommand) new RelayCommand((Action<object>) (obj => new FrmListPaymentAccount().ShowDialog()));
    }

    private void LoadingPaymentMethods()
    {
      List<Gbs.Core.Entities.PaymentMethods.PaymentMethod> list = Gbs.Core.Entities.PaymentMethods.GetActionPaymentsList().Where<Gbs.Core.Entities.PaymentMethods.PaymentMethod>((Func<Gbs.Core.Entities.PaymentMethods.PaymentMethod, bool>) (x =>
      {
        if (x.IsDeleted || !(x.SectionUid == Guid.Empty) && !(x.SectionUid == Sections.GetCurrentSection().Uid))
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid);
      })).ToList<Gbs.Core.Entities.PaymentMethods.PaymentMethod>();
      List<Gbs.Core.Entities.PaymentMethods.PaymentMethod> paymentMethodList = list;
      Gbs.Core.Entities.PaymentMethods.PaymentMethod paymentMethod = new Gbs.Core.Entities.PaymentMethods.PaymentMethod();
      paymentMethod.Uid = Guid.Empty;
      paymentMethod.Name = Translate.Devices_Нет;
      paymentMethodList.Insert(0, paymentMethod);
      this.Methods = new ObservableCollection<Gbs.Core.Entities.PaymentMethods.PaymentMethod>(list);
      if (!this.Methods.Any<Gbs.Core.Entities.PaymentMethods.PaymentMethod>((Func<Gbs.Core.Entities.PaymentMethods.PaymentMethod, bool>) (x => x.Uid == this.DefaultMethodPaymentUid)))
        this.DefaultMethodPaymentUid = Guid.Empty;
      this.OnPropertyChanged("DefaultMethodPaymentUid");
    }
  }
}
