// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountForCountGoodListViewModel
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
namespace Gbs.Forms.Settings.Discounts
{
  public partial class DiscountForCountGoodListViewModel : ViewModelWithForm
  {
    public ObservableCollection<DiscountForCountGood> ListRules { get; set; }

    public DiscountForCountGood SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    private Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public DiscountForCountGoodListViewModel()
    {
      this.Discount.Load();
      this.ListRules = new ObservableCollection<DiscountForCountGood>(this.Discount.DiscountForCountGoodRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, DiscountForCountGood rule2) = new DiscountForCountViewModel().ShowCard(Guid.Empty, this.Discount);
        if (!result2)
          return;
        this.ListRules.Add(rule2);
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedRule != null)
        {
          (bool result, DiscountForCountGood rule) tuple = new DiscountForCountViewModel().ShowCard(this.SelectedRule.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          DiscountForCountGood rule = tuple.rule;
          if (num2 == 0)
            return;
          this.ListRules[this.ListRules.ToList<DiscountForCountGood>().FindIndex((Predicate<DiscountForCountGood>) (x => x.Uid == rule.Uid))] = rule;
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedRule != null)
        {
          List<DiscountForCountGood> list = ((IEnumerable) obj).Cast<DiscountForCountGood>().ToList<DiscountForCountGood>();
          if (MessageBoxHelper.Question(string.Format(Translate.DiscountForDayOfMouthViewModel_DiscountForDayOfMouthViewModel_Вы_уверены__что_хотите_удалить__0__скидочных_правил_, (object) list.Count)) != MessageBoxResult.Yes)
            return;
          foreach (DiscountForCountGood discountForCountGood in list)
          {
            discountForCountGood.IsDeleted = true;
            this.Discount.DiscountForCountGoodRepository.Save(discountForCountGood);
            this.ListRules.Remove(discountForCountGood);
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
