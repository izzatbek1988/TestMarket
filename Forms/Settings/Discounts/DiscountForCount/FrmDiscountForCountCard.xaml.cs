// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountForCountViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.BackEnd.Discount;
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
namespace Gbs.Forms.Settings.Discounts
{
  public partial class DiscountForCountViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public ICommand AddItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.ListItem.Add(new DiscountForCountGood.Item());
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
          List<DiscountForCountGood.Item> list = ((IEnumerable) obj).Cast<DiscountForCountGood.Item>().ToList<DiscountForCountGood.Item>();
          if (!list.Any<DiscountForCountGood.Item>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.PricingRuleCardViewModel_Необходимо_выбрать_строку_для_удаления);
          }
          else
          {
            foreach (DiscountForCountGood.Item obj1 in list)
              this.ListItem.Remove(obj1);
            this.OnPropertyChanged("ListItem");
          }
        }));
      }
    }

    public ObservableCollection<DiscountForCountGood.Item> ListItem { get; set; } = new ObservableCollection<DiscountForCountGood.Item>();

    public DiscountForCountGood Rule { get; set; }

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

    private Gbs.Core.Entities.Settings.Facade.Discount DiscountSetting { get; set; }

    private void Save()
    {
      IEnumerable<DiscountForCountGood> rule = this.DiscountSetting.DiscountForCountGoodRepository.GetActiveItems().Where<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (x => x.Uid != this.Rule.Uid));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => rule.Any<DiscountForCountGood>((Func<DiscountForCountGood, bool>) (d => d.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.DiscountSumSaleCardViewModel_Для_следующих_категорий_товаров_уже_есть_скидочное_правило_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      }
      else if (this.ListItem.ToList<DiscountForCountGood.Item>().GroupBy<DiscountForCountGood.Item, Decimal>((Func<DiscountForCountGood.Item, Decimal>) (x => x.MinCount)).Any<IGrouping<Decimal, DiscountForCountGood.Item>>((Func<IGrouping<Decimal, DiscountForCountGood.Item>, bool>) (x => x.Count<DiscountForCountGood.Item>() != 1)))
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.PricingRuleCardViewModel_Невозможно_указать_несколько_правил_с_одинаковым_диапазоном);
      }
      else
      {
        this.Rule.Groups = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this.Rule.Items = this.ListItem.ToList<DiscountForCountGood.Item>();
        this._saveResult = this.DiscountSetting.DiscountForCountGoodRepository.Save(this.Rule);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, DiscountForCountGood rule) ShowCard(
      Guid ruleGuid,
      Gbs.Core.Entities.Settings.Facade.Discount discountSetting)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.Rule = ruleGuid == Guid.Empty ? new DiscountForCountGood() : this.DiscountSetting.DiscountForCountGoodRepository.GetByUid(ruleGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = ruleGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.Rule.Groups);
          this.ListItem = new ObservableCollection<DiscountForCountGood.Item>(this.Rule.Items);
          this.FormToSHow = (WindowWithSize) new FrmDiscountForCountCard();
          ((FrmDiscountForCountCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
              (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteItemCommand.Execute((object) ((FrmDiscountForCountCard) this.FormToSHow).ItemsGrid.SelectedItems)))
            }
          };
          this.ShowForm();
          return (this._saveResult, this.Rule);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила скидки от количества в чеке");
        return (false, (DiscountForCountGood) null);
      }
    }
  }
}
