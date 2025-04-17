// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.SelectPaymentMethods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class SelectPaymentMethods : ViewModelWithForm
  {
    private readonly bool _isPaymentsPrepaid;
    private ObservableCollection<SelectPaymentMethods.PaymentGrid> _payments = new ObservableCollection<SelectPaymentMethods.PaymentGrid>();
    private Decimal _maxSumBonusesClient;
    private Timer _recalcTimer = new Timer(250.0)
    {
      AutoReset = false
    };
    private bool _isEnableOkCommand = true;

    private Decimal BonusesPayment
    {
      get
      {
        return this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault()));
      }
    }

    public Visibility VisibilitySum
    {
      get => !this._isPaymentsPrepaid ? Visibility.Visible : Visibility.Collapsed;
    }

    public ICommand BuySaleCommand { get; set; }

    public string OkButtonText { get; set; }

    public Action Close { get; set; }

    public bool IsReturnSale { get; set; }

    public (bool Result, List<SelectPaymentMethods.PaymentGrid> listPay, Decimal delivery) Result { get; set; } = (false, (List<SelectPaymentMethods.PaymentGrid>) null, 0M);

    public ObservableCollection<SelectPaymentMethods.PaymentGrid> Payments
    {
      get => this._payments;
      set
      {
        this._payments = value;
        this.OnPropertyChanged(nameof (Payments));
        this.OnPropertyChanged("Delivery");
      }
    }

    public Decimal SumDocument { get; set; }

    public Decimal SumToPay
    {
      get
      {
        Decimal sumDocument = this.SumDocument;
        Decimal? nullable = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bonus, GlobalDictionaries.KkmPaymentMethods.Certificate, GlobalDictionaries.KkmPaymentMethods.PrePayment))).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
        return (nullable.HasValue ? new Decimal?(sumDocument - nullable.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault();
      }
      set => this.OnPropertyChanged(nameof (SumToPay));
    }

    public Decimal SumPaid => this.SumDocument;

    public Decimal Delivery
    {
      get
      {
        Decimal? nullable = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
        Decimal sumPaid = this.SumPaid;
        return nullable.GetValueOrDefault() > sumPaid & nullable.HasValue ? 0M : this.Payments.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault())) - this.SumPaid;
      }
    }

    public Visibility PrepaidVisibility { get; set; } = Visibility.Collapsed;

    public Gbs.Core.ViewModels.Basket.Basket Basket { get; }

    public SelectPaymentMethods()
    {
    }

    public SelectPaymentMethods(
      List<PaymentMethods.PaymentMethod> listPay,
      Decimal sum,
      Decimal sumCertificate,
      Decimal receiveSum,
      Decimal prepaidSum,
      bool isPaymentPrepaid,
      Decimal sumBonuses,
      Gbs.Core.ViewModels.Basket.Basket basket = null)
    {
      this.SumDocument = sum;
      this._isPaymentsPrepaid = isPaymentPrepaid;
      this.Basket = basket;
      this._maxSumBonusesClient = sumBonuses;
      this._recalcTimer.Elapsed += (ElapsedEventHandler) ((o, a) => this.DoRecalc());
      this.OnPropertyChanged(nameof (VisibilitySum));
      listPay.ForEach((Action<PaymentMethods.PaymentMethod>) (x => this.Payments.Add(new SelectPaymentMethods.PaymentGrid(this, x))));
      SelectPaymentMethods.PaymentGrid paymentGrid = this.Payments.FirstOrDefault<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.Uid == (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Payments.DefaultMethodPaymentUid ?? Guid.Empty)));
      if (paymentGrid != null)
        paymentGrid.Sum = new Decimal?(receiveSum - sumCertificate - prepaidSum > 0M ? receiveSum - sumCertificate - prepaidSum : 0M);
      else
        LogHelper.Debug("Не найден подходящий способ оплаты для установки суммы по умолчанию");
      if (sumCertificate > 0M)
      {
        int index = this.Payments.ToList<SelectPaymentMethods.PaymentGrid>().FindIndex((Predicate<SelectPaymentMethods.PaymentGrid>) (x => x.Method.Uid == GlobalDictionaries.CertificatePaymentUid));
        this.Payments[index].Sum = new Decimal?(sumCertificate);
        this.Payments[index].IsReadOnly = true;
        this.Payments[index].IsEnabled = false;
      }
      if (prepaidSum > 0M)
      {
        int index = this.Payments.ToList<SelectPaymentMethods.PaymentGrid>().FindIndex((Predicate<SelectPaymentMethods.PaymentGrid>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.PrePayment));
        this.Payments[index].Sum = new Decimal?(prepaidSum);
        this.Payments[index].IsReadOnly = true;
        this.Payments[index].IsEnabled = false;
      }
      List<SelectPaymentMethods.PaymentGrid> list = new List<SelectPaymentMethods.PaymentGrid>();
      list.AddRange(this.Payments.Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.DisplayIndex != this.Payments.First<SelectPaymentMethods.PaymentGrid>().Method.DisplayIndex)) ? (IEnumerable<SelectPaymentMethods.PaymentGrid>) this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => !IsBCP(x))).OrderBy<SelectPaymentMethods.PaymentGrid, int>((Func<SelectPaymentMethods.PaymentGrid, int>) (x => x.Method.DisplayIndex)).ThenBy<SelectPaymentMethods.PaymentGrid, string>((Func<SelectPaymentMethods.PaymentGrid, string>) (x => x.Method.Name)).ToList<SelectPaymentMethods.PaymentGrid>() : (IEnumerable<SelectPaymentMethods.PaymentGrid>) this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => !IsBCP(x))).OrderBy<SelectPaymentMethods.PaymentGrid, string>((Func<SelectPaymentMethods.PaymentGrid, string>) (x => x.Method.Name)).ToList<SelectPaymentMethods.PaymentGrid>());
      list.AddRange(this.Payments.Where<SelectPaymentMethods.PaymentGrid>(new Func<SelectPaymentMethods.PaymentGrid, bool>(IsBCP)));
      this.Payments = new ObservableCollection<SelectPaymentMethods.PaymentGrid>(list);
      this.BuySaleCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Sale()));

      static bool IsBCP(SelectPaymentMethods.PaymentGrid x)
      {
        return x.Method.KkmMethod.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.PrePayment, GlobalDictionaries.KkmPaymentMethods.Certificate, GlobalDictionaries.KkmPaymentMethods.Bonus);
      }
    }

    private void Sale()
    {
      Decimal? nullable1 = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
      {
        if (p.Type == GlobalDictionaries.KkmPaymentMethods.Cash)
          return false;
        return !p.Method.KkmMethod.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bonus, GlobalDictionaries.KkmPaymentMethods.Certificate);
      })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
      Decimal sumPaid1 = this.SumPaid;
      if (nullable1.GetValueOrDefault() > sumPaid1 & nullable1.HasValue && !this._isPaymentsPrepaid)
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.SelectPaymentMethods_Sale_Сумма_безналичных_платежей_не_может_быть_больше_чем_итоговая_сумма_чека_, icon: MessageBoxImage.Exclamation);
      }
      else if (this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p => p.Type == GlobalDictionaries.KkmPaymentMethods.Cash)).Count<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
      {
        Decimal? sum = x.Sum;
        Decimal num10 = 0M;
        return sum.GetValueOrDefault() > num10 & sum.HasValue;
      })) > 1)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.SelectPaymentMethods_Возможно_оплачивать_только_на_один_наличный_счет);
      }
      else if (this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
      {
        PaymentMethods.PaymentMethod method = p.Method;
        if (method == null)
          return false;
        return method.PaymentMethodsType.IsEither<GlobalDictionaries.PaymentMethodsType>(GlobalDictionaries.PaymentMethodsType.Sbp, GlobalDictionaries.PaymentMethodsType.Acquiring);
      })).Count<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
      {
        Decimal? sum = x.Sum;
        Decimal num9 = 0M;
        return sum.GetValueOrDefault() > num9 & sum.HasValue;
      })) > 1)
        MessageBoxHelper.Warning(Translate.SelectPaymentMethods_Sale_Для_оплаты_нужно_выбрать_один_из_безналичных_способов_платежа__через_терминал_эквайринга_или_по_СБП__Скорректируйте_способы_платежа_и_повторите_оплату_);
      else if (this.IsReturnSale && this.Delivery != 0M)
      {
        int num3 = (int) MessageBoxHelper.Show(Translate.SelectPaymentMethods_При_возврате_требуется_вернуть_всю_сумму_);
      }
      else
      {
        Decimal delivery = this.Delivery;
        if (this.Delivery >= 0M && !this._isPaymentsPrepaid)
        {
          ObservableCollection<SelectPaymentMethods.PaymentGrid> payments = this.Payments;
          LogHelper.Debug("Лист способов оплаты:" + (payments != null ? payments.ToJsonString(true) : (string) null));
          int index = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p => p != null && p.Type == GlobalDictionaries.KkmPaymentMethods.Cash)).Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
          {
            if (x == null)
              return false;
            Decimal? sum = x.Sum;
            Decimal num4 = 0M;
            return sum.GetValueOrDefault() > num4 & sum.HasValue;
          })) ? this.Payments.ToList<SelectPaymentMethods.PaymentGrid>().FindIndex((Predicate<SelectPaymentMethods.PaymentGrid>) (x =>
          {
            if (x.Type != GlobalDictionaries.KkmPaymentMethods.Cash)
              return false;
            Decimal? sum = x.Sum;
            Decimal num5 = 0M;
            return sum.GetValueOrDefault() > num5 & sum.HasValue;
          })) : this.Payments.ToList<SelectPaymentMethods.PaymentGrid>().FindIndex((Predicate<SelectPaymentMethods.PaymentGrid>) (x =>
          {
            if (x.Type != GlobalDictionaries.KkmPaymentMethods.Cash)
              return false;
            Decimal? sum = x.Sum;
            Decimal num6 = 0M;
            return sum.GetValueOrDefault() >= num6 & sum.HasValue;
          }));
          if (index == -1)
          {
            SelectPaymentMethods.PaymentGrid paymentGrid = this.Payments.First<SelectPaymentMethods.PaymentGrid>();
            Decimal sumPaid2 = this.SumPaid;
            nullable1 = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x != this.Payments.First<SelectPaymentMethods.PaymentGrid>())).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
            Decimal? nullable2 = nullable1.HasValue ? new Decimal?(sumPaid2 - nullable1.GetValueOrDefault()) : new Decimal?();
            paymentGrid.Sum = nullable2;
          }
          else
          {
            SelectPaymentMethods.PaymentGrid payment = this.Payments[index];
            Decimal sumPaid3 = this.SumPaid;
            nullable1 = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x != this.Payments[index])).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
            Decimal? nullable3 = nullable1.HasValue ? new Decimal?(sumPaid3 - nullable1.GetValueOrDefault()) : new Decimal?();
            payment.Sum = nullable3;
          }
          this.Payments = new ObservableCollection<SelectPaymentMethods.PaymentGrid>(this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
          {
            Decimal? sum = x.Sum;
            Decimal num7 = 0M;
            return sum.GetValueOrDefault() > num7 & sum.HasValue;
          })));
          this.Result = (true, this.Payments.ToList<SelectPaymentMethods.PaymentGrid>(), delivery);
          this.Close();
        }
        else
        {
          this.Payments = new ObservableCollection<SelectPaymentMethods.PaymentGrid>(this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
          {
            Decimal? sum = x.Sum;
            Decimal num8 = 0M;
            return sum.GetValueOrDefault() > num8 & sum.HasValue;
          })));
          this.Result = (true, this.Payments.ToList<SelectPaymentMethods.PaymentGrid>(), delivery);
          this.Close();
        }
      }
    }

    public bool IsEnableOkCommand
    {
      get => this._isEnableOkCommand;
      set
      {
        this._isEnableOkCommand = value;
        this.OnPropertyChanged(nameof (IsEnableOkCommand));
      }
    }

    private void Recalc()
    {
      this.IsEnableOkCommand = false;
      this._recalcTimer.Stop();
      this._recalcTimer.Start();
    }

    private void DoRecalc()
    {
      this.CheckBonusesMaxSum();
      if (this.Basket != null)
      {
        this.Basket.Payments = this.Payments.ToList<SelectPaymentMethods.PaymentGrid>();
        this.Basket.ReCalcTotals();
        this.SumDocument = this.Basket.TotalSum;
      }
      this.OnPropertyChanged("Payments");
      this.OnPropertyChanged("Delivery");
      this.OnPropertyChanged("SumPaid");
      this.OnPropertyChanged("SumDocument");
      this.OnPropertyChanged("SumToPay");
      this.IsEnableOkCommand = true;
    }

    private void CheckBonusesMaxSum()
    {
      Decimal num1 = this._maxSumBonusesClient > this.SumDocument ? this.SumDocument : this._maxSumBonusesClient;
      Bonuses bonuses = new Bonuses();
      bonuses.Load();
      Decimal num2 = bonuses.MaxValueBonuses / 100M * this.SumDocument;
      if (num2 < num1)
        num1 = num2;
      Decimal? nullable = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
      Decimal num3 = num1;
      if (!(nullable.GetValueOrDefault() > num3 & nullable.HasValue))
        return;
      List<SelectPaymentMethods.PaymentGrid> list = this.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type != GlobalDictionaries.KkmPaymentMethods.Bonus)).ToList<SelectPaymentMethods.PaymentGrid>();
      SelectPaymentMethods.PaymentGrid paymentGrid = this.Payments.FirstOrDefault<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Type == GlobalDictionaries.KkmPaymentMethods.Bonus));
      if (paymentGrid == null)
        return;
      paymentGrid.Sum = new Decimal?(num1);
      list.Add(paymentGrid);
      this.Payments = new ObservableCollection<SelectPaymentMethods.PaymentGrid>(list);
      int num4 = (int) MessageBoxHelper.Show(string.Format(Translate.SelectPaymentMethods_CheckBonusesMaxSum_Сумма_оплаты_баллами_не_может_быть_больше__0_, (object) num1));
    }

    public class PaymentGrid : ViewModel
    {
      private bool _isEnabled = true;
      private bool _isReadOnly;
      private PaymentMethods.PaymentMethod _method;
      private Decimal? _sum = new Decimal?(0M);
      private string _name;

      public ICommand GetTotalSumToPayCommand { get; set; }

      public PaymentMethods.PaymentMethod Method
      {
        get => this._method;
        set
        {
          this._method = value;
          this.OnPropertyChanged(nameof (Method));
        }
      }

      public Decimal? Sum
      {
        get => this._sum;
        set
        {
          this._sum = new Decimal?(value.GetValueOrDefault());
          this.OnPropertyChanged(nameof (Sum));
          this.Parent?.Recalc();
        }
      }

      public string Name
      {
        get => this._name;
        set
        {
          this._name = value;
          this.OnPropertyChanged(nameof (Name));
        }
      }

      public bool IsReadOnly
      {
        get => this._isReadOnly;
        set
        {
          this._isReadOnly = value;
          this.OnPropertyChanged(nameof (IsReadOnly));
        }
      }

      public bool IsEnabled
      {
        get => this._isEnabled;
        set
        {
          this._isEnabled = value;
          this.OnPropertyChanged(nameof (IsEnabled));
        }
      }

      public GlobalDictionaries.KkmPaymentMethods Type { get; set; }

      public string Comment { get; set; } = string.Empty;

      private SelectPaymentMethods Parent { get; set; }

      public PaymentGrid()
      {
      }

      public PaymentGrid(SelectPaymentMethods parent, PaymentMethods.PaymentMethod method)
      {
        if (parent == null)
          return;
        this.Parent = parent;
        this.Method = method;
        this.Type = this.Method.KkmMethod;
        this.GetTotalSumToPayCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<GlobalDictionaries.KkmPaymentMethods> extPayments = new List<GlobalDictionaries.KkmPaymentMethods>()
          {
            GlobalDictionaries.KkmPaymentMethods.PrePayment,
            GlobalDictionaries.KkmPaymentMethods.Certificate,
            GlobalDictionaries.KkmPaymentMethods.Bonus
          };
          foreach (SelectPaymentMethods.PaymentGrid paymentGrid in this.Parent.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
          {
            if (x.Method == this.Method || x.Method.KkmMethod.IsEither<GlobalDictionaries.KkmPaymentMethods>((IEnumerable<GlobalDictionaries.KkmPaymentMethods>) extPayments))
              return false;
            Decimal? sum = x.Sum;
            Decimal num = 0M;
            return !(sum.GetValueOrDefault() == num & sum.HasValue);
          })))
            paymentGrid.Sum = new Decimal?(0M);
          Decimal sumPaid = this.Parent.SumPaid;
          Decimal? nullable1 = this.Parent.Payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x => x.Method.KkmMethod.IsEither<GlobalDictionaries.KkmPaymentMethods>((IEnumerable<GlobalDictionaries.KkmPaymentMethods>) extPayments))).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
          Decimal? nullable2 = nullable1.HasValue ? new Decimal?(sumPaid - nullable1.GetValueOrDefault()) : new Decimal?();
          nullable1 = nullable2;
          Decimal num1 = 0M;
          this.Sum = nullable1.GetValueOrDefault() < num1 & nullable1.HasValue ? new Decimal?(0M) : nullable2;
        }));
      }
    }
  }
}
