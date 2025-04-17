// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.MaxDiscount.MaxDiscountRuleListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings.BackEnd.Discount;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Discounts.MaxDiscount
{
  public partial class MaxDiscountRuleListViewModel : ViewModelWithForm
  {
    public ObservableCollection<MaxDiscountRule> ListMaxDiscount { get; set; }

    public Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public MaxDiscountRule SelectedMaxDiscount { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public MaxDiscountRuleListViewModel()
    {
      this.Discount.Load();
      this.ListMaxDiscount = new ObservableCollection<MaxDiscountRule>(this.Discount.MaxDiscountsRuleRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, MaxDiscountRule discount2) = new MaxDiscountRuleCardViewModel().ShowCard(Guid.Empty, this.Discount);
        if (!result2)
          return;
        this.ListMaxDiscount.Add(discount2);
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedMaxDiscount != null)
        {
          (bool result, MaxDiscountRule discount) tuple = new MaxDiscountRuleCardViewModel().ShowCard(this.SelectedMaxDiscount.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          MaxDiscountRule discount = tuple.discount;
          if (num2 == 0)
            return;
          this.ListMaxDiscount[this.ListMaxDiscount.ToList<MaxDiscountRule>().FindIndex((Predicate<MaxDiscountRule>) (x => x.Uid == discount.Uid))] = discount;
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedMaxDiscount != null)
        {
          List<MaxDiscountRule> list = ((IEnumerable) obj).Cast<MaxDiscountRule>().ToList<MaxDiscountRule>();
          if (MessageBoxHelper.Show(Translate.PaymentAccountListViewModel_Вы_уверены_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          foreach (MaxDiscountRule maxDiscountRule in list)
          {
            maxDiscountRule.IsDeleted = true;
            this.Discount.MaxDiscountsRuleRepository.Save(maxDiscountRule);
            this.ListMaxDiscount.Remove(maxDiscountRule);
          }
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
    }
  }
}
