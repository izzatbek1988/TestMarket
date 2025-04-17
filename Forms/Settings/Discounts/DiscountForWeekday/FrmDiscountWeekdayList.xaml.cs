// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountForWeekday.DiscountForWeekDayListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

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
namespace Gbs.Forms.Settings.Discounts.DiscountForWeekday
{
  public partial class DiscountForWeekDayListViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday> ListDiscount { get; set; }

    public Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public Gbs.Core.Entities.Settings.Discount.DiscountForWeekday SelectedDiscount { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand CopyCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public DiscountForWeekDayListViewModel()
    {
      this.Discount.Load();
      this.ListDiscount = new ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>(this.Discount.DiscountForWeekdayRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discount2) = new DiscountForWeekdayViewModel().ShowCard(Guid.Empty, this.Discount);
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
          (bool result, Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discount) tuple = new DiscountForWeekdayViewModel().ShowCard(this.SelectedDiscount.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discount = tuple.discount;
          if (num2 == 0)
            return;
          this.ListDiscount[this.ListDiscount.ToList<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>().FindIndex((Predicate<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>) (x => x.Uid == discount.Uid))] = discount;
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
          Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discountForWeekday = this.SelectedDiscount.Clone<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>();
          discountForWeekday.Uid = Guid.NewGuid();
          discountForWeekday.IsOff = true;
          this.Discount.DiscountForWeekdayRepository.Save(discountForWeekday);
          this.ListDiscount.Add(discountForWeekday);
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
          List<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>().ToList<Gbs.Core.Entities.Settings.Discount.DiscountForWeekday>();
          if (MessageBoxHelper.Question(string.Format(Translate.DiscountForDayOfMouthViewModel_DiscountForDayOfMouthViewModel_Вы_уверены__что_хотите_удалить__0__скидочных_правил_, (object) list.Count)) != MessageBoxResult.Yes)
            return;
          foreach (Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discountForWeekday in list)
          {
            discountForWeekday.IsDeleted = true;
            this.Discount.DiscountForWeekdayRepository.Save(discountForWeekday);
            this.ListDiscount.Remove(discountForWeekday);
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
