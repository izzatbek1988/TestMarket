// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ExtraPrice.ExtraPricingRulesViewModel
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
namespace Gbs.Forms.Settings.ExtraPrice
{
  public partial class ExtraPricingRulesViewModel : ViewModelWithForm
  {
    public ObservableCollection<ExtraPriceRule> ListRules { get; set; }

    public ExtraPriceRule SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ExtraPricingRulesViewModel()
    {
      List<ExtraPriceRule> activeItems = new ExtraPriceRulesRepository().GetActiveItems();
      foreach (ExtraPriceRule extraPriceRule in activeItems)
        extraPriceRule.Groups = new List<GoodGroups.Group>(extraPriceRule.Groups.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => !x.IsDeleted)));
      this.ListRules = new ObservableCollection<ExtraPriceRule>(activeItems);
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, ExtraPriceRule rule2) = new ExtraPriceRuleCardViewModel().ShowCard(Guid.Empty);
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
          (bool result, ExtraPriceRule rule) tuple = new ExtraPriceRuleCardViewModel().ShowCard(this.SelectedRule.Uid);
          int num2 = tuple.result ? 1 : 0;
          ExtraPriceRule rule = tuple.rule;
          if (num2 == 0)
            return;
          this.ListRules[this.ListRules.ToList<ExtraPriceRule>().FindIndex((Predicate<ExtraPriceRule>) (x => x.Uid == rule.Uid))] = rule;
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.ExtraPricingRulesViewModel_ExtraPricingRulesViewModel_Требуется_выбрать_правило_доп_цен_для_редактирования_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedRule != null)
        {
          List<ExtraPriceRule> list = ((IEnumerable) obj).Cast<ExtraPriceRule>().ToList<ExtraPriceRule>();
          if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          foreach (ExtraPriceRule extraPriceRule in list)
          {
            if (extraPriceRule.Uid == GlobalDictionaries.DefaultExtraRuleUid)
            {
              int num = (int) MessageBoxHelper.Show(Translate.ExtraPricingRulesViewModel_Правило_по_умолчанию_удалить_невозможно_);
            }
            else
            {
              extraPriceRule.IsDeleted = true;
              new ExtraPriceRulesRepository().Save(extraPriceRule);
              this.ListRules.Remove(extraPriceRule);
            }
          }
        }
        else
        {
          int num4 = (int) MessageBoxHelper.Show(Translate.ListExtraPriceViewModel_Требуется_выбрать_доп__цену, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
    }
  }
}
