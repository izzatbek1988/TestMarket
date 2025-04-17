// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentsGroups.GroupsSelectedViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentsGroups
{
  public partial class GroupsSelectedViewModel : ViewModelWithForm
  {
    private string _filterText;
    private ObservableCollection<GroupsSelectedViewModel.GroupWithChilds> _groupList;

    public string FilterText
    {
      get => this._filterText;
      set
      {
        this._filterText = value;
        this.OnPropertyChanged(nameof (FilterText));
        this.GetGroupsTree();
      }
    }

    public bool Result { get; set; }

    public GroupsSelectedViewModel.GroupWithChilds SelectedGroup { get; set; }

    public List<GoodGroups.Group> SelectedList { get; set; } = new List<GoodGroups.Group>();

    public ObservableCollection<GroupsSelectedViewModel.GroupWithChilds> GroupList
    {
      get => this._groupList;
      set
      {
        this._groupList = value;
        this.OnPropertyChanged(nameof (GroupList));
      }
    }

    public ICommand GetGroup { get; set; }

    public Action Close { get; set; }

    private List<PaymentGroups.PaymentGroup> CachedDbGroups { get; set; }

    public GroupsSelectedViewModel()
    {
    }

    public GroupsSelectedViewModel(
      List<GoodGroups.Group> selectedList,
      PaymentGroups.VisiblePaymentGroup enumPay)
    {
      GroupsSelectedViewModel selectedViewModel = this;
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.SelectedList = selectedList;
        this.CachedDbGroups = PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>().Where<PAYMENTS_GROUP>((Expression<Func<PAYMENTS_GROUP, bool>>) (x => x.IS_DELETED == false && x.VISIBLE_IN == (int) enumPay)));
        this.GetGroupsTree();
      }
      this.GetGroup = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (selectedViewModel.SelectedGroup != null)
        {
          selectedViewModel.Result = true;
          selectedViewModel.Close();
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.GroupsSelectedViewModel_Необходимо_выбрать_группу_);
        }
      }));
    }

    private void GetGroupsTree()
    {
      List<GroupsSelectedViewModel.GroupWithChilds> list = new List<GroupsSelectedViewModel.GroupWithChilds>();
      if (this.FilterText.IsNullOrEmpty())
      {
        foreach (GroupsSelectedViewModel.GroupWithChilds goodGroup in this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (x => x.ParentGroup == null)).Select<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>((Func<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>) (group => new GroupsSelectedViewModel.GroupWithChilds()
        {
          Group = group
        })))
        {
          this.AddChildrens(goodGroup);
          list.Add(goodGroup);
        }
      }
      else
        list = new List<GroupsSelectedViewModel.GroupWithChilds>((IEnumerable<GroupsSelectedViewModel.GroupWithChilds>) this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (x => x.Name.ToLower().Contains(this.FilterText.ToLower()))).Select<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>((Func<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>) (x => new GroupsSelectedViewModel.GroupWithChilds()
        {
          Group = x
        })).OrderBy<GroupsSelectedViewModel.GroupWithChilds, string>((Func<GroupsSelectedViewModel.GroupWithChilds, string>) (x => x.Group.Name)));
      this.GroupList = new ObservableCollection<GroupsSelectedViewModel.GroupWithChilds>(list);
    }

    private void AddChildrens(GroupsSelectedViewModel.GroupWithChilds goodGroup)
    {
      foreach (GroupsSelectedViewModel.GroupWithChilds goodGroup1 in this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (c =>
      {
        Guid? uid1 = c.ParentGroup?.Uid;
        Guid uid2 = goodGroup.Group.Uid;
        return uid1.HasValue && uid1.GetValueOrDefault() == uid2;
      })).Select<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>((Func<PaymentGroups.PaymentGroup, GroupsSelectedViewModel.GroupWithChilds>) (group => new GroupsSelectedViewModel.GroupWithChilds()
      {
        Group = group
      })))
      {
        this.AddChildrens(goodGroup1);
        goodGroup.Childrens.Add(goodGroup1);
      }
    }

    public class GroupWithChilds
    {
      public PaymentGroups.PaymentGroup Group { get; set; } = new PaymentGroups.PaymentGroup();

      public List<GroupsSelectedViewModel.GroupWithChilds> Childrens { get; set; } = new List<GroupsSelectedViewModel.GroupWithChilds>();
    }
  }
}
