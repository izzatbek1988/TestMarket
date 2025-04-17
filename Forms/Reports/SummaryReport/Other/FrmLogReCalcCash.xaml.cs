// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.LogReCalcCashViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public partial class LogReCalcCashViewModel : ViewModelWithForm
  {
    private LogReCalcCashViewModel.DifferenceTypeEnum _selectedDifferenceType;
    private Guid _selectedUserUid = Guid.Empty;
    private Guid _selectedPaymentsAccountUid = Guid.Empty;
    public bool IsReCalcCash = true;
    private ObservableCollection<LogReCalcCashViewModel.ReCalcCashItem> _items = new ObservableCollection<LogReCalcCashViewModel.ReCalcCashItem>();
    private DateTime _valueDateTimeStart = DateTime.Now.Date;
    private DateTime _valueDateTimeEnd = DateTime.Now.Date;
    private List<LogReCalcCashViewModel.ReCalcCashItem> _cacheItems;
    private List<Payments.Payment> _allPayments;

    public LogReCalcCashViewModel.DifferenceTypeEnum SelectedDifferenceType
    {
      get => this._selectedDifferenceType;
      set
      {
        this._selectedDifferenceType = value;
        this.Search();
      }
    }

    public Dictionary<LogReCalcCashViewModel.DifferenceTypeEnum, string> DictionaryDifferenceType { get; set; } = new Dictionary<LogReCalcCashViewModel.DifferenceTypeEnum, string>()
    {
      {
        LogReCalcCashViewModel.DifferenceTypeEnum.All,
        Translate.WaybillsViewModel_Statuses_Все_статусы
      },
      {
        LogReCalcCashViewModel.DifferenceTypeEnum.Zero,
        Translate.LogReCalcCashViewModel_DictionaryDifferenceType_Без_изменений
      },
      {
        LogReCalcCashViewModel.DifferenceTypeEnum.Shortage,
        Translate.LogReCalcCashViewModel_DictionaryDifferenceType_Недостача
      },
      {
        LogReCalcCashViewModel.DifferenceTypeEnum.Surplus,
        Translate.LogReCalcCashViewModel_DictionaryDifferenceType_Избыток
      }
    };

    public IEnumerable<Gbs.Core.Entities.Users.User> Users { get; set; }

    public Guid SelectedUserUid
    {
      get => this._selectedUserUid;
      set
      {
        this._selectedUserUid = value;
        this.Search();
      }
    }

    public IEnumerable<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount> PaymentsAccounts { get; set; }

    public Guid SelectedPaymentsAccountUid
    {
      get => this._selectedPaymentsAccountUid;
      set
      {
        this._selectedPaymentsAccountUid = value;
        this.Search();
      }
    }

    public Decimal TotalDifference
    {
      get
      {
        return this.Items.Sum<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, Decimal>) (x => x.Difference));
      }
    }

    public ObservableCollection<LogReCalcCashViewModel.ReCalcCashItem> Items
    {
      get => this._items;
      set
      {
        this._items = value;
        this.OnPropertyChanged(nameof (Items));
        this.OnPropertyChanged("TotalDifference");
      }
    }

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set
      {
        if (!(this._valueDateTimeStart.Date != value.Date))
          return;
        this._valueDateTimeStart = value;
        this.OnPropertyChanged(nameof (ValueDateTimeStart));
        this.GetLogData();
      }
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set
      {
        if (!(this._valueDateTimeEnd.Date != value.Date))
          return;
        this._valueDateTimeEnd = value;
        this.OnPropertyChanged(nameof (ValueDateTimeEnd));
        this.GetLogData();
      }
    }

    private void Search()
    {
      List<LogReCalcCashViewModel.ReCalcCashItem> reCalcCashItemList = new List<LogReCalcCashViewModel.ReCalcCashItem>((IEnumerable<LogReCalcCashViewModel.ReCalcCashItem>) this._cacheItems);
      if (this.SelectedPaymentsAccountUid != Guid.Empty)
        reCalcCashItemList = reCalcCashItemList.Where<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, bool>) (x =>
        {
          Guid? uid = x.Account?.Uid;
          Guid paymentsAccountUid = this.SelectedPaymentsAccountUid;
          return uid.HasValue && uid.GetValueOrDefault() == paymentsAccountUid;
        })).ToList<LogReCalcCashViewModel.ReCalcCashItem>();
      if (this.SelectedUserUid != Guid.Empty)
        reCalcCashItemList = reCalcCashItemList.Where<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, bool>) (x => x.User.Uid == this.SelectedUserUid)).ToList<LogReCalcCashViewModel.ReCalcCashItem>();
      if (this.SelectedDifferenceType != LogReCalcCashViewModel.DifferenceTypeEnum.All)
      {
        switch (this.SelectedDifferenceType)
        {
          case LogReCalcCashViewModel.DifferenceTypeEnum.Zero:
            reCalcCashItemList = reCalcCashItemList.Where<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, bool>) (x => x.Difference == 0M)).ToList<LogReCalcCashViewModel.ReCalcCashItem>();
            break;
          case LogReCalcCashViewModel.DifferenceTypeEnum.Shortage:
            reCalcCashItemList = reCalcCashItemList.Where<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, bool>) (x => x.Difference < 0M)).ToList<LogReCalcCashViewModel.ReCalcCashItem>();
            break;
          case LogReCalcCashViewModel.DifferenceTypeEnum.Surplus:
            reCalcCashItemList = reCalcCashItemList.Where<LogReCalcCashViewModel.ReCalcCashItem>((Func<LogReCalcCashViewModel.ReCalcCashItem, bool>) (x => x.Difference > 0M)).ToList<LogReCalcCashViewModel.ReCalcCashItem>();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      this.Items = new ObservableCollection<LogReCalcCashViewModel.ReCalcCashItem>(reCalcCashItemList);
      this.OnPropertyChanged("TotalDifference");
    }

    private void GetLogData()
    {
      List<Payments.Payment> list = this._allPayments.Where<Payments.Payment>((Func<Payments.Payment, bool>) (x =>
      {
        if (x.Type == (this.IsReCalcCash ? GlobalDictionaries.PaymentTypes.RecountSumCash : GlobalDictionaries.PaymentTypes.BalanceCorrection))
        {
          DateTime date1 = x.Date.Date;
          DateTime dateTime = this.ValueDateTimeStart;
          DateTime date2 = dateTime.Date;
          if (date1 >= date2)
          {
            dateTime = x.Date;
            DateTime date3 = dateTime.Date;
            dateTime = this.ValueDateTimeEnd;
            DateTime date4 = dateTime.Date;
            return date3 <= date4;
          }
        }
        return false;
      })).OrderBy<Payments.Payment, DateTime>((Func<Payments.Payment, DateTime>) (x => x.Date)).ToList<Payments.Payment>();
      this._cacheItems = new List<LogReCalcCashViewModel.ReCalcCashItem>();
      Sections.Section currentSection = Sections.GetCurrentSection();
      Func<Payments.Payment, bool> predicate = (Func<Payments.Payment, bool>) (x => x.Section.Uid == currentSection.Uid);
      foreach (Payments.Payment payment1 in list.Where<Payments.Payment>(predicate))
      {
        Payments.Payment payment = payment1;
        Decimal num = this.IsReCalcCash ? SaleHelper.GetSumCash(new Guid?(currentSection.Uid), (List<Payments.Payment>) null, payment.Date) : this._allPayments.Where<Payments.Payment>((Func<Payments.Payment, bool>) (x => x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.RecountSumCash, GlobalDictionaries.PaymentTypes.BalanceCorrection) && x.Date < payment.Date && x.Section.Uid == currentSection.Uid)).Sum<Payments.Payment>((Func<Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
        this._cacheItems.Add(new LogReCalcCashViewModel.ReCalcCashItem()
        {
          Difference = payment.SumIn - payment.SumOut,
          DateTime = payment.Date,
          User = payment.User,
          OldSum = num,
          NewSum = num + (payment.SumIn - payment.SumOut),
          Comment = payment.Comment,
          Account = payment.AccountIn ?? payment.AccountOut
        });
      }
      this.Search();
    }

    public string TitleForm
    {
      get
      {
        return !this.IsReCalcCash ? Translate.LogReCalcCashViewModel_TitleForm_Журнал_изменения_расхождений_по_кассе : Translate.LogReCalcCashViewModel_TitleForm_Журнал_пересчета_наличных_в_кассе;
      }
    }

    public void ShowLog(
      List<Payments.Payment> payments,
      bool isReCalcCash,
      DateTime start = default (DateTime),
      DateTime finish = default (DateTime))
    {
      this._allPayments = new List<Payments.Payment>((IEnumerable<Payments.Payment>) payments);
      this.IsReCalcCash = isReCalcCash;
      if (start != new DateTime())
        this.ValueDateTimeStart = start;
      if (finish != new DateTime())
        this.ValueDateTimeEnd = finish;
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Users.User> first1 = new List<Gbs.Core.Entities.Users.User>();
        Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
        user.Alias = Translate.PaymentsActionListViewModel_Все_пользователи;
        user.Uid = Guid.Empty;
        first1.Add(user);
        this.Users = first1.Concat<Gbs.Core.Entities.Users.User>((IEnumerable<Gbs.Core.Entities.Users.User>) new UsersRepository(dataBase).GetActiveItems());
        List<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount> first2 = new List<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount>();
        Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount paymentsAccount = new Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount();
        paymentsAccount.Name = Translate.MasterReportViewModel_Accounts_Все_счета;
        paymentsAccount.Uid = Guid.Empty;
        first2.Add(paymentsAccount);
        this.PaymentsAccounts = first2.Concat<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount>(Gbs.Core.Entities.PaymentsAccounts.GetPaymentsAccountsList().Where<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount>((Func<Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted)));
        this.GetLogData();
        this.FormToSHow = (WindowWithSize) new FrmLogReCalcCash();
        this.ShowForm();
      }
    }

    public enum DifferenceTypeEnum
    {
      All,
      Zero,
      Shortage,
      Surplus,
    }

    public class ReCalcCashItem
    {
      public DateTime DateTime { get; set; }

      public Decimal OldSum { get; set; }

      public Decimal NewSum { get; set; }

      public Decimal Difference { get; set; }

      public Gbs.Core.Entities.Users.User User { get; set; }

      public string Comment { get; set; }

      public Gbs.Core.Entities.PaymentsAccounts.PaymentsAccount Account { get; set; }
    }
  }
}
