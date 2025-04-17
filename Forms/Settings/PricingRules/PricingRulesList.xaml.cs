// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PricingRules.PricingRulesViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
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
namespace Gbs.Forms.Settings.PricingRules
{
  public partial class PricingRulesViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.PricingRules> ListRules { get; set; }

    public Gbs.Core.Entities.PricingRules SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public PricingRulesViewModel()
    {
      this.ListRules = new ObservableCollection<Gbs.Core.Entities.PricingRules>(new PricingRulesRepository().GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, Gbs.Core.Entities.PricingRules rule2) = new PricingRuleCardViewModel().ShowCard(Guid.Empty);
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
          (bool result, Gbs.Core.Entities.PricingRules rule) tuple = new PricingRuleCardViewModel().ShowCard(this.SelectedRule.Uid);
          int num2 = tuple.result ? 1 : 0;
          Gbs.Core.Entities.PricingRules rule = tuple.rule;
          if (num2 == 0)
            return;
          this.ListRules[this.ListRules.ToList<Gbs.Core.Entities.PricingRules>().FindIndex((Predicate<Gbs.Core.Entities.PricingRules>) (x => x.Uid == rule.Uid))] = rule;
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
          List<Gbs.Core.Entities.PricingRules> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.PricingRules>().ToList<Gbs.Core.Entities.PricingRules>();
          if (MessageBoxHelper.Show(string.Format(Translate.PricingRulesViewModel_Вы_уверены__что_хотите_удалить__0__правил_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          foreach (Gbs.Core.Entities.PricingRules pricingRules in list)
          {
            pricingRules.IsDeleted = true;
            new PricingRulesRepository().Save(pricingRules);
            this.ListRules.Remove(pricingRules);
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
