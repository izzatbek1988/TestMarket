// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountSumSale.DiscountSumSaleViewModel
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
namespace Gbs.Forms.Settings.Discounts.DiscountSumSale
{
  public partial class DiscountSumSaleViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountSumSale> ListRules { get; set; }

    public Gbs.Core.Entities.Settings.Discount.DiscountSumSale SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand CopyCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (((ICollection) obj).Count != 1)
          {
            int num = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
          else
          {
            Gbs.Core.Entities.Settings.Discount.DiscountSumSale copyRule = this.SelectedRule.Clone<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>();
            copyRule.Uid = Guid.NewGuid();
            copyRule.Name += Translate.DiscountSumSaleViewModel_CopyCommand___копия_;
            copyRule.Groups.Clear();
            (bool result2, Gbs.Core.Entities.Settings.Discount.DiscountSumSale rule2) = new DiscountSumSaleCardViewModel().ShowCard(copyRule.Uid, this.Discount, copyRule);
            if (!result2)
              return;
            this.ListRules.Add(rule2);
          }
        }));
      }
    }

    private Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public DiscountSumSaleViewModel()
    {
      this.Discount.Load();
      this.ListRules = new ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>(this.Discount.DiscountSumSaleRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, Gbs.Core.Entities.Settings.Discount.DiscountSumSale rule2) = new DiscountSumSaleCardViewModel().ShowCard(Guid.Empty, this.Discount);
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
          (bool result, Gbs.Core.Entities.Settings.Discount.DiscountSumSale rule) tuple = new DiscountSumSaleCardViewModel().ShowCard(this.SelectedRule.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          Gbs.Core.Entities.Settings.Discount.DiscountSumSale rule = tuple.rule;
          if (num2 == 0)
            return;
          this.ListRules[this.ListRules.ToList<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>().FindIndex((Predicate<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>) (x => x.Uid == rule.Uid))] = rule;
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
          List<Gbs.Core.Entities.Settings.Discount.DiscountSumSale> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>().ToList<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>();
          if (MessageBoxHelper.Question(string.Format(Translate.DiscountForDayOfMouthViewModel_DiscountForDayOfMouthViewModel_Вы_уверены__что_хотите_удалить__0__скидочных_правил_, (object) list.Count)) != MessageBoxResult.Yes)
            return;
          foreach (Gbs.Core.Entities.Settings.Discount.DiscountSumSale discountSumSale in list)
          {
            discountSumSale.IsDeleted = true;
            this.Discount.DiscountSumSaleRepository.Save(discountSumSale);
            this.ListRules.Remove(discountSumSale);
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
