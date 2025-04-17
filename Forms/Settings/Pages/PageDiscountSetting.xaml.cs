// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageDiscountSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Forms.Settings.Discounts;
using Gbs.Forms.Settings.Discounts.ClientBonuses;
using Gbs.Forms.Settings.Discounts.DiscountForWeekday;
using Gbs.Forms.Settings.Discounts.DiscountFromSumInCheck;
using Gbs.Forms.Settings.Discounts.DiscountsForDayOfMonth;
using Gbs.Forms.Settings.Discounts.DiscountSumSale;
using Gbs.Forms.Settings.Discounts.MaxDiscount;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class PageDiscountSettingViewModel : ViewModelWithForm
  {
    private ObservableCollection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>> _paymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>();
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public string TextPropButton
    {
      get
      {
        int num = this.PaymentMethodList.Count<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => x.IsChecked));
        if (num == this.PaymentMethodList.Count<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>())
          return Translate.SaleJournalViewModel_TextPropButton_Все_способы;
        return num != 1 ? string.Format(Translate.SaleJournalViewModel_TextPropButton_Способов___0_, (object) num) : this.PaymentMethodList.First<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => x.IsChecked)).Item.Name;
      }
    }

    public ObservableCollection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>> PaymentMethodList
    {
      get => this._paymentMethodList;
      set
      {
        this._paymentMethodList = value;
        this.OnPropertyChanged(nameof (PaymentMethodList));
        this.OnPropertyChanged("TextPropButton");
      }
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

    public ICommand DiscountForDayOfMonth
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmDiscountsForDayOfMonth().ShowDialog()));
      }
    }

    public ICommand DiscountForWeekDay
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmDiscountWeekdayList().ShowDialog()));
      }
    }

    public ICommand MaxDiscountGroup
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmMaxDiscountRuleList().ShowDialog()));
      }
    }

    public ICommand DiscountFromSumInCheck
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmDiscountSumInCheck().ShowDialog()));
      }
    }

    public ICommand DiscountForCountGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmDiscountForCountGood().ShowDialog()));
      }
    }

    public ICommand DiscountFromSumSale
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmDiscountSumSale().ShowDialog()));
      }
    }

    public ICommand ShowBonusesRule
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmListBonusesRule().ShowDialog()));
      }
    }

    public Bonuses BonusesSetting { get; set; } = new Bonuses();

    public Gbs.Core.Entities.Settings.Facade.Discount DiscountSetting { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public Dictionary<Bonuses.BonusesOption, string> DictionaryOptionBonuses
    {
      get
      {
        return new Dictionary<Bonuses.BonusesOption, string>()
        {
          {
            Bonuses.BonusesOption.AllSale,
            Translate.PageDiscountSettingViewModel_Начислять_баллы_на_всю_сумму_покупки
          },
          {
            Bonuses.BonusesOption.SaleOffBonuses,
            Translate.PageDiscountSettingViewModel_Не_начислять_баллы_на_сумму_оплаченную_баллами
          },
          {
            Bonuses.BonusesOption.OffBonuses,
            Translate.PageDiscountSettingViewModel_Не_начислять_баллы_на_покупку
          }
        };
      }
    }

    public PageDiscountSettingViewModel()
    {
    }

    public PageDiscountSettingViewModel(Gbs.Core.Config.Settings settings)
    {
      this.Settings = settings;
      this.BonusesSetting.Load();
      this.DiscountSetting.Load();
      Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForBirthday discountForBirthday = this.DiscountSetting.DiscountForBirthdayRepository.GetAllItems().Single<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForBirthday>();
      this.CountDay = discountForBirthday.CountDay;
      this.DiscountForBirthday = discountForBirthday.Discount;
      this.IsDiscountForBirthday = discountForBirthday.IsActive;
      this.IsValidityPeriodBonuses = this.BonusesSetting.ValidityPeriodBonuses != -1;
      this.ValidityPeriodBonuses = this.BonusesSetting.ValidityPeriodBonuses == -1 ? new int?() : new int?(this.BonusesSetting.ValidityPeriodBonuses);
      this.LoadingPaymentMethods();
    }

    public void LoadingPaymentMethods()
    {
      if (this.PaymentMethodList.Any<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>())
        this.BonusesSetting.ListMethod = this.PaymentMethodList.Where<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => x.IsChecked)).Select<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, Guid>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, Guid>) (x => x.Item.Method.Uid)).ToList<Guid>();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>> list1 = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => !x.IS_DELETED))).Select<PaymentMethods.PaymentMethod, SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<PaymentMethods.PaymentMethod, SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>) (x => new SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>()
        {
          Item = new PageDiscountSettingViewModel.PaymentMethodView(x)
        })).ToList<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>();
        List<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>> list2 = list1.Where<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => !x.Item.Method.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid, GlobalDictionaries.BonusesPaymentUid))).OrderBy<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, int>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, int>) (x => x.Item.Method.DisplayIndex)).ToList<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>();
        list2.AddRange(list1.Where<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => x.Item.Method.Uid.IsEither<Guid>(GlobalDictionaries.CertificatePaymentUid))));
        foreach (SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView> itemSelected1 in list2)
        {
          SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView> itemSelected = itemSelected1;
          itemSelected.IsChecked = this.BonusesSetting.ListMethod.Any<Guid>((Func<Guid, bool>) (x => x == itemSelected.Item.Method.Uid));
        }
        this.PaymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>(list2);
        Action methodsContextMenuGrid = this.LoadingPaymentMethodsContextMenuGrid;
        if (methodsContextMenuGrid == null)
          return;
        methodsContextMenuGrid();
      }
    }

    public int CountDay { get; set; }

    public Decimal DiscountForBirthday { get; set; }

    public bool IsDiscountForBirthday { get; set; }

    public int? ValidityPeriodBonuses { get; set; }

    public bool IsValidityPeriodBonuses { get; set; }

    public bool Save()
    {
      this.BonusesSetting.ListMethod = this.PaymentMethodList.Where<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (x => x.IsChecked)).Select<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, Guid>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, Guid>) (x => x.Item.Method.Uid)).ToList<Guid>();
      Bonuses bonusesSetting = this.BonusesSetting;
      int num;
      if (this.IsValidityPeriodBonuses)
      {
        int? validityPeriodBonuses = this.ValidityPeriodBonuses;
        if (validityPeriodBonuses.HasValue)
        {
          validityPeriodBonuses = this.ValidityPeriodBonuses;
          num = validityPeriodBonuses.Value;
          goto label_4;
        }
      }
      num = -1;
label_4:
      bonusesSetting.ValidityPeriodBonuses = num;
      return this.DiscountSetting.DiscountForBirthdayRepository.Save(new Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountForBirthday()
      {
        CountDay = this.CountDay,
        Discount = this.DiscountForBirthday,
        IsActive = this.IsDiscountForBirthday
      }) && this.DiscountSetting.Save() && this.BonusesSetting.Save();
    }

    public Action LoadingPaymentMethodsContextMenuGrid { get; set; }

    public class PaymentMethodView
    {
      public string Name { get; set; }

      public PaymentMethods.PaymentMethod Method { get; set; }

      public PaymentMethodView()
      {
      }

      public PaymentMethodView(PaymentMethods.PaymentMethod method)
      {
        this.Method = method;
        this.Name = this.Method.Name;
        if (!(this.Method.SectionUid != Guid.Empty))
          return;
        this.Name = this.Name + " (" + Sections.GetSectionByUid(method.SectionUid).Name + ")";
      }
    }
  }
}
