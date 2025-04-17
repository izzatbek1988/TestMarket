// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountSumSale.DiscountSumSaleCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.Discount;
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
namespace Gbs.Forms.Settings.Discounts.DiscountSumSale
{
  public partial class DiscountSumSaleCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public ICommand AddItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.ListItem.Add(new Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item());
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
          List<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>().ToList<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>();
          if (!list.Any<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.PricingRuleCardViewModel_Необходимо_выбрать_строку_для_удаления);
          }
          else
          {
            foreach (Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item obj1 in list)
              this.ListItem.Remove(obj1);
            this.OnPropertyChanged("ListItem");
          }
        }));
      }
    }

    public ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item> ListItem { get; set; } = new ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>();

    public Gbs.Core.Entities.Settings.Discount.DiscountSumSale Rule { get; set; }

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
      IEnumerable<Gbs.Core.Entities.Settings.Discount.DiscountSumSale> rule = new DiscountSumSaleRepository().GetActiveItems().Where<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>((Func<Gbs.Core.Entities.Settings.Discount.DiscountSumSale, bool>) (x => x.Uid != this.Rule.Uid));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => rule.Any<Gbs.Core.Entities.Settings.Discount.DiscountSumSale>((Func<Gbs.Core.Entities.Settings.Discount.DiscountSumSale, bool>) (d => d.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.DiscountSumSaleCardViewModel_Для_следующих_категорий_товаров_уже_есть_скидочное_правило_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      }
      else if (this.ListItem.ToList<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>().GroupBy<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item, Decimal>((Func<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item, Decimal>) (x => x.MinSum)).Any<IGrouping<Decimal, Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>>((Func<IGrouping<Decimal, Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>, bool>) (x => x.Count<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>() != 1)))
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.PricingRuleCardViewModel_Невозможно_указать_несколько_правил_с_одинаковым_диапазоном);
      }
      else
      {
        this.Rule.Groups = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this.Rule.Items = this.ListItem.ToList<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>();
        this._saveResult = this.DiscountSetting.DiscountSumSaleRepository.Save(this.Rule);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, Gbs.Core.Entities.Settings.Discount.DiscountSumSale rule) ShowCard(
      Guid ruleGuid,
      Gbs.Core.Entities.Settings.Facade.Discount discountSetting,
      Gbs.Core.Entities.Settings.Discount.DiscountSumSale copyRule = null)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.Rule = ruleGuid == Guid.Empty ? new Gbs.Core.Entities.Settings.Discount.DiscountSumSale() : copyRule ?? this.DiscountSetting.DiscountSumSaleRepository.GetByUid(ruleGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = ruleGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.Rule.Groups);
          this.ListItem = new ObservableCollection<Gbs.Core.Entities.Settings.Discount.DiscountSumSale.Item>(this.Rule.Items);
          this.FormToSHow = (WindowWithSize) new FrmDiscountSumSaleCard();
          ((FrmDiscountSumSaleCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
              (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteItemCommand.Execute((object) ((FrmDiscountSumSaleCard) this.FormToSHow).ItemsGrid.SelectedItems)))
            }
          };
          this.ShowForm();
          return (this._saveResult, this.Rule);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила скидки от суммы покупок");
        return (false, (Gbs.Core.Entities.Settings.Discount.DiscountSumSale) null);
      }
    }
  }
}
