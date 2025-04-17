// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PricingRules.PricingRuleCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PricingRules
{
  public partial class PricingRuleCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter = new ObservableCollection<GoodGroups.Group>();
    private bool _saveResult;

    public ICommand AddItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.ListItem.Add(new Gbs.Core.Entities.PricingRules.ItemPricing());
          this.OnPropertyChanged("ListItem");
        }));
      }
    }

    public ICommand DeleteItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<Gbs.Core.Entities.PricingRules.ItemPricing> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.PricingRules.ItemPricing>().ToList<Gbs.Core.Entities.PricingRules.ItemPricing>();
          if (!list.Any<Gbs.Core.Entities.PricingRules.ItemPricing>())
          {
            MessageBoxHelper.Warning(Translate.PricingRuleCardViewModel_Необходимо_выбрать_строку_для_удаления);
          }
          else
          {
            foreach (Gbs.Core.Entities.PricingRules.ItemPricing itemPricing in list)
              this.ListItem.Remove(itemPricing);
            this.OnPropertyChanged("ListItem");
          }
        }));
      }
    }

    public ObservableCollection<Gbs.Core.Entities.PricingRules.ItemPricing> ListItem { get; set; } = new ObservableCollection<Gbs.Core.Entities.PricingRules.ItemPricing>();

    public Gbs.Core.Entities.PricingRules Rule { get; set; }

    public ICommand SaveCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    private IEnumerable<GoodGroups.Group> AllListGroupGood { get; }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
      }
    }

    public PricingRuleCardViewModel()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.AllListGroupGood = (IEnumerable<GoodGroups.Group>) new GoodGroupsRepository(dataBase).GetActiveItems();
    }

    private void Save()
    {
      IEnumerable<Gbs.Core.Entities.PricingRules> rule = new PricingRulesRepository().GetActiveItems().Where<Gbs.Core.Entities.PricingRules>((Func<Gbs.Core.Entities.PricingRules, bool>) (x => x.Uid != this.Rule.Uid));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => rule.Any<Gbs.Core.Entities.PricingRules>((Func<Gbs.Core.Entities.PricingRules, bool>) (d => d.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
        MessageBoxHelper.Warning(Translate.ExtraPriceRuleCardViewModel_Для_следующих_категорий_товаров_уже_есть_правила__по_ценообразованию_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      else if (this.ListItem.ToList<Gbs.Core.Entities.PricingRules.ItemPricing>().GroupBy<Gbs.Core.Entities.PricingRules.ItemPricing, Decimal>((Func<Gbs.Core.Entities.PricingRules.ItemPricing, Decimal>) (x => x.MinSum)).Any<IGrouping<Decimal, Gbs.Core.Entities.PricingRules.ItemPricing>>((Func<IGrouping<Decimal, Gbs.Core.Entities.PricingRules.ItemPricing>, bool>) (x => x.Count<Gbs.Core.Entities.PricingRules.ItemPricing>() != 1)))
      {
        MessageBoxHelper.Warning(Translate.PricingRuleCardViewModel_Невозможно_указать_несколько_правил_с_одинаковым_диапазоном);
      }
      else
      {
        this.Rule.Groups = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this.Rule.Items = this.ListItem.ToList<Gbs.Core.Entities.PricingRules.ItemPricing>();
        this._saveResult = new PricingRulesRepository().Save(this.Rule);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, Gbs.Core.Entities.PricingRules rule) ShowCard(Guid ruleGuid)
    {
      try
      {
        this.Rule = ruleGuid == Guid.Empty ? new Gbs.Core.Entities.PricingRules() : new PricingRulesRepository().GetByUid(ruleGuid);
        this.GroupsListFilter = new ObservableCollection<GoodGroups.Group>(ruleGuid == Guid.Empty ? this.AllListGroupGood : (IEnumerable<GoodGroups.Group>) this.Rule.Groups);
        this.ListItem = new ObservableCollection<Gbs.Core.Entities.PricingRules.ItemPricing>(this.Rule.Items);
        this.FormToSHow = (WindowWithSize) new PricingRuleCard();
        ((PricingRuleCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.FormToSHow.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.SaveCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()))
          },
          {
            hotKeys.AddItem,
            this.AddItemCommand
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteItemCommand.Execute((object) ((PricingRuleCard) this.FormToSHow).ItemsGrid.SelectedItems)))
          }
        };
        this.ShowForm();
        return (this._saveResult, this.Rule);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила ценообразования");
        return (false, (Gbs.Core.Entities.PricingRules) null);
      }
    }
  }
}
