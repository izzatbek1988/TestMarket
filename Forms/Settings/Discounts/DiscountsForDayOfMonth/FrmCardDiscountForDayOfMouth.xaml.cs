// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountsForDayOfMonth.DiscountForDayOfMouthCardVm
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Discounts.DiscountsForDayOfMonth
{
  public partial class DiscountForDayOfMouthCardVm : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public DiscountForDayOfMonth Discount { get; set; }

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
      IEnumerable<DiscountForDayOfMonth> discount = this.DiscountSetting.DiscountForDayOfMonthRepository.GetActiveItems().Where<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (x => x.Day == this.Discount.Day && x.Uid != this.Discount.Uid));
      List<GoodGroups.Group> list = this.GroupsListFilter.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => discount.Any<DiscountForDayOfMonth>((Func<DiscountForDayOfMonth, bool>) (d => d.ListGroup.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == x.Uid)))))).ToList<GoodGroups.Group>();
      if (list.Any<GoodGroups.Group>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.DiscountForDayOfMouthCardVm_Для_следующих_категорий_товаров_уже_есть_правила_за_сегодняшних_день_ + Other.NewLine() + string.Join("\n", list.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))));
      }
      else
      {
        this.Discount.ListGroup = this.GroupsListFilter.ToList<GoodGroups.Group>();
        this._saveResult = this.DiscountSetting.DiscountForDayOfMonthRepository.Save(this.Discount);
        if (!this._saveResult)
          return;
        this.CloseAction();
      }
    }

    public (bool result, DiscountForDayOfMonth discount) ShowCard(
      Guid discountGuid,
      Gbs.Core.Entities.Settings.Facade.Discount discountSetting)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.Discount = discountGuid == Guid.Empty ? new DiscountForDayOfMonth() : this.DiscountSetting.DiscountForDayOfMonthRepository.GetByUid(discountGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = discountGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.Discount.ListGroup);
          this.FormToSHow = (WindowWithSize) new FrmCardDiscountForDayOfMouth();
          ((FrmCardDiscountForDayOfMouth) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
          return (this._saveResult, this.Discount);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке скидки по дням месяца");
        return (false, (DiscountForDayOfMonth) null);
      }
    }
  }
}
