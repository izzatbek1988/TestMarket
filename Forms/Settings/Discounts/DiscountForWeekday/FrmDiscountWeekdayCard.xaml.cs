// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountForWeekday.DiscountForWeekdayViewModel
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Discounts.DiscountForWeekday
{
  public partial class DiscountForWeekdayViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter = new ObservableCollection<GoodGroups.Group>();
    private bool _saveResult;

    public Gbs.Core.Entities.Settings.Discount.DiscountForWeekday Discount { get; set; }

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
      this.Discount.ListGroup = this.GroupsListFilter.ToList<GoodGroups.Group>();
      this._saveResult = this.DiscountSetting.DiscountForWeekdayRepository.Save(this.Discount);
      if (!this._saveResult)
        return;
      this.CloseAction();
    }

    public (bool result, Gbs.Core.Entities.Settings.Discount.DiscountForWeekday discount) ShowCard(
      Guid discountGuid,
      Gbs.Core.Entities.Settings.Facade.Discount discountSetting)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.Discount = discountGuid == Guid.Empty ? new Gbs.Core.Entities.Settings.Discount.DiscountForWeekday() : this.DiscountSetting.DiscountForWeekdayRepository.GetByUid(discountGuid);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = discountGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.Discount.ListGroup);
          this.FormToSHow = (WindowWithSize) new FrmDiscountWeekdayCard();
          ((FrmDiscountWeekdayCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
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
        LogHelper.Error(ex, "Ошибка в карточке скидки по дням недели");
        return (false, (Gbs.Core.Entities.Settings.Discount.DiscountForWeekday) null);
      }
    }
  }
}
