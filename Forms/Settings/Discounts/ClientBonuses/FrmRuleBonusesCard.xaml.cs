// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.ClientBonuses.ClientBonusesRuleCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.Facade;
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
namespace Gbs.Forms.Settings.Discounts.ClientBonuses
{
  public partial class ClientBonusesRuleCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter = new ObservableCollection<GoodGroups.Group>();
    private bool _saveResult;

    public Gbs.Core.Entities.Settings.Discount.ClientBonuses Rule { get; set; }

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

    private Bonuses Bonuses { get; set; }

    private void Save()
    {
      IEnumerable<Gbs.Core.Entities.Settings.Discount.ClientBonuses> rule = this.Bonuses.ClientBonusesRepository.GetActiveItems().Where<Gbs.Core.Entities.Settings.Discount.ClientBonuses>((Func<Gbs.Core.Entities.Settings.Discount.ClientBonuses, bool>) (x => x.Uid != this.Rule.Uid));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => rule.Any<Gbs.Core.Entities.Settings.Discount.ClientBonuses>((Func<Gbs.Core.Entities.Settings.Discount.ClientBonuses, bool>) (d => d.ListGroups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.ClientBonusesRuleCardViewModel_Для_следующих_категорий_товаров_уже_есть_правила__по_начисления_баллов_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      }
      else
      {
        this.Rule.ListGroups = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this._saveResult = this.Bonuses.ClientBonusesRepository.Save(this.Rule);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, Gbs.Core.Entities.Settings.Discount.ClientBonuses rule) ShowCard(
      Guid ruleUid,
      Bonuses bonuses)
    {
      try
      {
        this.Bonuses = bonuses;
        this.Rule = ruleUid == Guid.Empty ? new Gbs.Core.Entities.Settings.Discount.ClientBonuses() : this.Bonuses.ClientBonusesRepository.GetByUid(ruleUid);
        this.FormToSHow = (WindowWithSize) new FrmRuleBonusesCard();
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = new ObservableCollection<GoodGroups.Group>(ruleUid == Guid.Empty ? new List<GoodGroups.Group>((IEnumerable<GoodGroups.Group>) groupsRepository.GetActiveItems()) : this.Rule.ListGroups);
          ((FrmRuleBonusesCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
        LogHelper.Error(ex, "Ошибка в карточке правила начисления баллов");
        return (false, (Gbs.Core.Entities.Settings.Discount.ClientBonuses) null);
      }
    }
  }
}
