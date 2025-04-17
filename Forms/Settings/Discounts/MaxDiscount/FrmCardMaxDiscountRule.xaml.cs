// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.MaxDiscount.MaxDiscountRuleCardViewModel
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Discounts.MaxDiscount
{
  public partial class MaxDiscountRuleCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public MaxDiscountRule MaxDiscount { get; set; }

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
      this.MaxDiscount.ListGroup = this.GroupsListFilter.ToList<GoodGroups.Group>();
      this._saveResult = this.DiscountSetting.MaxDiscountsRuleRepository.Save(this.MaxDiscount);
      if (!this._saveResult)
        return;
      this.CloseAction();
    }

    public (bool result, MaxDiscountRule discount) ShowCard(
      Guid maxDiscountGuid,
      Gbs.Core.Entities.Settings.Facade.Discount discountSetting)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.MaxDiscount = maxDiscountGuid == Guid.Empty ? new MaxDiscountRule() : this.DiscountSetting.MaxDiscountsRuleRepository.GetByUid(maxDiscountGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = maxDiscountGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.MaxDiscount.ListGroup);
          this.FormToSHow = (WindowWithSize) new FrmCardMaxDiscountRule();
          ((FrmCardMaxDiscountRule) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
          return (this._saveResult, this.MaxDiscount);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила максимальной скидки");
        return (false, (MaxDiscountRule) null);
      }
    }
  }
}
