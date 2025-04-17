// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.ClientBonuses.BonusesRuleListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings.Facade;
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
namespace Gbs.Forms.Settings.Discounts.ClientBonuses
{
  public partial class BonusesRuleListViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.Settings.Discount.ClientBonuses> BonusesRuleList { get; set; }

    public Gbs.Core.Entities.Settings.Discount.ClientBonuses SelectedRule { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    private Bonuses Bonuses { get; set; } = new Bonuses();

    public BonusesRuleListViewModel()
    {
      this.Bonuses.Load();
      this.BonusesRuleList = new ObservableCollection<Gbs.Core.Entities.Settings.Discount.ClientBonuses>(this.Bonuses.ClientBonusesRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, Gbs.Core.Entities.Settings.Discount.ClientBonuses rule2) = new ClientBonusesRuleCardViewModel().ShowCard(Guid.Empty, this.Bonuses);
        if (!result2)
          return;
        this.BonusesRuleList.Add(rule2);
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedRule != null)
        {
          (bool result, Gbs.Core.Entities.Settings.Discount.ClientBonuses rule) tuple = new ClientBonusesRuleCardViewModel().ShowCard(this.SelectedRule.Uid, this.Bonuses);
          int num2 = tuple.result ? 1 : 0;
          Gbs.Core.Entities.Settings.Discount.ClientBonuses discount = tuple.rule;
          if (num2 == 0)
            return;
          this.BonusesRuleList[this.BonusesRuleList.ToList<Gbs.Core.Entities.Settings.Discount.ClientBonuses>().FindIndex((Predicate<Gbs.Core.Entities.Settings.Discount.ClientBonuses>) (x => x.Uid == discount.Uid))] = discount;
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
          List<Gbs.Core.Entities.Settings.Discount.ClientBonuses> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Settings.Discount.ClientBonuses>().ToList<Gbs.Core.Entities.Settings.Discount.ClientBonuses>();
          if (MessageBoxHelper.Show(Translate.PaymentAccountListViewModel_Вы_уверены_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          foreach (Gbs.Core.Entities.Settings.Discount.ClientBonuses clientBonuses in list)
          {
            clientBonuses.IsDeleted = true;
            this.Bonuses.ClientBonusesRepository.Save(clientBonuses);
            this.BonusesRuleList.Remove(clientBonuses);
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
