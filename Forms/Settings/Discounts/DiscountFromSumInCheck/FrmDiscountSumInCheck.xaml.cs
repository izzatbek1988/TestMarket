// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountFromSumInCheck.DiscountSumInCheckViewModel
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
namespace Gbs.Forms.Settings.Discounts.DiscountFromSumInCheck
{
  public partial class DiscountSumInCheckViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck> ListRules { get; set; }

    public Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

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
            Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck oldRule = this.SelectedRule.Clone<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>();
            oldRule.Uid = Guid.NewGuid();
            oldRule.Name += Translate.DiscountSumSaleViewModel_CopyCommand___копия_;
            oldRule.Groups.Clear();
            (bool result2, Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck rule2) = new DiscountSumCheckViewModel().ShowCard(oldRule.Uid, this.Discount, oldRule);
            if (!result2)
              return;
            this.ListRules.Add(rule2);
          }
        }));
      }
    }

    public ICommand DeleteCommand { get; set; }

    private Gbs.Core.Entities.Settings.Facade.Discount Discount { get; set; } = new Gbs.Core.Entities.Settings.Facade.Discount();

    public DiscountSumInCheckViewModel()
    {
      this.Discount.Load();
      this.ListRules = new ObservableCollection<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>(this.Discount.DiscountFromSumInCheckRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck rule2) = new DiscountSumCheckViewModel().ShowCard(Guid.Empty, this.Discount);
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
          (bool result, Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck rule) tuple = new DiscountSumCheckViewModel().ShowCard(this.SelectedRule.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck rule = tuple.rule;
          if (num2 == 0)
            return;
          this.ListRules[this.ListRules.ToList<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>().FindIndex((Predicate<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>) (x => x.Uid == rule.Uid))] = rule;
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
          List<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>().ToList<Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck>();
          if (MessageBoxHelper.Question(string.Format(Translate.DiscountForDayOfMouthViewModel_DiscountForDayOfMouthViewModel_Вы_уверены__что_хотите_удалить__0__скидочных_правил_, (object) list.Count)) != MessageBoxResult.Yes)
            return;
          foreach (Gbs.Core.Entities.Settings.BackEnd.Discount.DiscountFromSumInCheck discountFromSumInCheck in list)
          {
            discountFromSumInCheck.IsDeleted = true;
            this.Discount.DiscountFromSumInCheckRepository.Save(discountFromSumInCheck);
            this.ListRules.Remove(discountFromSumInCheck);
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
