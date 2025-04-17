// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountsForDayOfMonth.DiscountForDayOfMouthViewModel
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
namespace Gbs.Forms.Settings.Discounts.DiscountsForDayOfMonth
{
  public partial class DiscountForDayOfMouthViewModel : ViewModelWithForm
  {
    public ObservableCollection<DiscountForDayOfMonth> ListDiscount { get; set; }

    public DiscountForDayOfMonth SelectedDiscount { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand CopyCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    private Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public DiscountForDayOfMouthViewModel()
    {
      this.Discount.Load();
      this.ListDiscount = new ObservableCollection<DiscountForDayOfMonth>(this.Discount.DiscountForDayOfMonthRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, DiscountForDayOfMonth discount2) = new DiscountForDayOfMouthCardVm().ShowCard(Guid.Empty, this.Discount);
        if (!result2)
          return;
        this.ListDiscount.Add(discount2);
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedDiscount != null)
        {
          (bool result, DiscountForDayOfMonth discount) tuple = new DiscountForDayOfMouthCardVm().ShowCard(this.SelectedDiscount.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          DiscountForDayOfMonth discount = tuple.discount;
          if (num2 == 0)
            return;
          this.ListDiscount[this.ListDiscount.ToList<DiscountForDayOfMonth>().FindIndex((Predicate<DiscountForDayOfMonth>) (x => x.Uid == discount.Uid))] = discount;
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.CopyCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num4 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedDiscount != null)
        {
          if (MessageBoxHelper.Show(Translate.DiscountForWeekDayListViewModel_Вы_уверены__что_хотите_сделать_копию_этого_правила_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          DiscountForDayOfMonth discountForDayOfMonth = this.SelectedDiscount.Clone<DiscountForDayOfMonth>();
          discountForDayOfMonth.Uid = Guid.NewGuid();
          discountForDayOfMonth.IsOff = true;
          new DiscountForDayOfMonthRepository().Save(discountForDayOfMonth);
          this.ListDiscount.Add(discountForDayOfMonth);
        }
        else
        {
          int num5 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedDiscount != null)
        {
          List<DiscountForDayOfMonth> list = ((IEnumerable) obj).Cast<DiscountForDayOfMonth>().ToList<DiscountForDayOfMonth>();
          if (MessageBoxHelper.Question(string.Format(Translate.DiscountForDayOfMouthViewModel_DiscountForDayOfMouthViewModel_Вы_уверены__что_хотите_удалить__0__скидочных_правил_, (object) list.Count)) != MessageBoxResult.Yes)
            return;
          foreach (DiscountForDayOfMonth discountForDayOfMonth in list)
          {
            discountForDayOfMonth.IsDeleted = true;
            new DiscountForDayOfMonthRepository().Save(discountForDayOfMonth);
            this.ListDiscount.Remove(discountForDayOfMonth);
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
