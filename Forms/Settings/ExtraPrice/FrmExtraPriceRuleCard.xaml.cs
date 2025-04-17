// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ExtraPrice.ExtraPriceRuleCardViewModel
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.ExtraPrice
{
  public partial class ExtraPriceRuleCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public bool IsEnabledGroup
    {
      get
      {
        Guid? uid = this.Rule?.Uid;
        Guid defaultExtraRuleUid = GlobalDictionaries.DefaultExtraRuleUid;
        return !uid.HasValue || uid.GetValueOrDefault() != defaultExtraRuleUid;
      }
    }

    public ObservableCollection<ExtraPriceRule.ItemPricing> ListItem { get; set; } = new ObservableCollection<ExtraPriceRule.ItemPricing>();

    public ExtraPriceRule Rule { get; set; }

    public ICommand SaveCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
      }
    }

    private void Save()
    {
      IEnumerable<ExtraPriceRule> rule = new ExtraPriceRulesRepository().GetActiveItems().Where<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (x => !x.Uid.IsEither<Guid>(this.Rule.Uid, GlobalDictionaries.DefaultExtraRuleUid)));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => rule.Any<ExtraPriceRule>((Func<ExtraPriceRule, bool>) (d => d.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.ExtraPriceRuleCardViewModel_Для_следующих_категорий_товаров_уже_есть_правила__по_ценообразованию_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      }
      else
      {
        this.Rule.Groups = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this.Rule.Items = this.ListItem.ToList<ExtraPriceRule.ItemPricing>();
        this._saveResult = new ExtraPriceRulesRepository().Save(this.Rule);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, ExtraPriceRule rule) ShowCard(Guid ruleGuid)
    {
      try
      {
        this.Rule = ruleGuid == Guid.Empty ? new ExtraPriceRule() : new ExtraPriceRulesRepository().GetByUid(ruleGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = ruleGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.Rule.Groups.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => !x.IsDeleted)));
          this.ListItem = new ObservableCollection<ExtraPriceRule.ItemPricing>(this.Rule.Items.Where<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (x => !x.Price.IsDeleted)));
          this.FormToSHow = (WindowWithSize) new FrmExtraPriceRuleCard();
          ((FrmExtraPriceRuleCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
            }
          };
          this.ShowForm();
          return (this._saveResult, this.Rule);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила ценообразования");
        return (false, (ExtraPriceRule) null);
      }
    }
  }
}
