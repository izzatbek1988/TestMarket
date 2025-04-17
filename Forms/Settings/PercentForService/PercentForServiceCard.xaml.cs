// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PercentForService.PercentForServiceCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PercentForService
{
  public partial class PercentForServiceCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter;
    private bool _saveResult;

    public PercentForServiceSetting PercentForService { get; set; }

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

    private Discount DiscountSetting { get; set; }

    private void Save()
    {
      this.PercentForService.ListGroup = this.GroupsListFilter.ToList<GoodGroups.Group>();
      this._saveResult = this.DiscountSetting.PercentForServiceRuleRepository.Save(this.PercentForService);
      if (!this._saveResult)
        return;
      this.CloseAction();
    }

    public (bool result, PercentForServiceSetting discount) ShowCard(
      Guid percentGuid,
      Discount discountSetting)
    {
      try
      {
        this.DiscountSetting = discountSetting;
        this.PercentForService = percentGuid == Guid.Empty ? new PercentForServiceSetting() : this.DiscountSetting.PercentForServiceRuleRepository.GetByUid(percentGuid);
        using (DataBase dataBase = Data.GetDataBase())
        {
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          this.GroupsListFilter = percentGuid == Guid.Empty ? new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems()) : new ObservableCollection<GoodGroups.Group>(this.PercentForService.ListGroup);
          this.FormToSHow = (WindowWithSize) new Gbs.Forms.Settings.PercentForService.PercentForService();
          ((Gbs.Forms.Settings.PercentForService.PercentForService) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
          this.ShowForm();
          return (this._saveResult, this.PercentForService);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке правила процента за обслуживание");
        return (false, (PercentForServiceSetting) null);
      }
    }
  }
}
