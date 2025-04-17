// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Payments.PaymentsGroups.PaymentsGroupViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Payments.PaymentsGroups;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Payments.PaymentsGroups
{
  public partial class PaymentsGroupViewModel : ViewModelWithForm
  {
    private ObservableCollection<PaymentsGroupViewModel.GroupWithChilds> _groupList;

    public ObservableCollection<PaymentsGroupViewModel.GroupWithChilds> GroupList
    {
      get => this._groupList;
      set
      {
        this._groupList = value;
        this.OnPropertyChanged(nameof (GroupList));
      }
    }

    public PaymentsGroupViewModel.GroupWithChilds SelectedGroup { get; set; }

    public static Visibility VisibleCheckBox { get; set; }

    private List<PaymentGroups.PaymentGroup> CachedDbGroups { get; set; }

    public PaymentsGroupViewModel()
    {
      this.LoadDataFromDb();
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGroup()));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.EditGroup()));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteGroup()));
    }

    private void LoadDataFromDb()
    {
      using (DataBase dataBase = Data.GetDataBase())
        this.CachedDbGroups = PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>().Where<PAYMENTS_GROUP>((Expression<Func<PAYMENTS_GROUP, bool>>) (x => x.IS_DELETED == false)));
      this.GroupList = new ObservableCollection<PaymentsGroupViewModel.GroupWithChilds>(this.GetGroupsTree());
    }

    private List<PaymentsGroupViewModel.GroupWithChilds> GetGroupsTree()
    {
      List<PaymentsGroupViewModel.GroupWithChilds> groupsTree = new List<PaymentsGroupViewModel.GroupWithChilds>();
      PaymentsGroupViewModel.GroupWithChilds groupWithChilds1 = new PaymentsGroupViewModel.GroupWithChilds();
      PaymentGroups.PaymentGroup paymentGroup1 = new PaymentGroups.PaymentGroup();
      paymentGroup1.Name = Translate.PaymentsGroupViewModel_Доходы;
      paymentGroup1.Uid = Guid.Empty;
      paymentGroup1.VisibleIn = PaymentGroups.VisiblePaymentGroup.Income;
      groupWithChilds1.Group = paymentGroup1;
      PaymentsGroupViewModel.GroupWithChilds groupWithChilds2 = groupWithChilds1;
      PaymentsGroupViewModel.GroupWithChilds groupWithChilds3 = new PaymentsGroupViewModel.GroupWithChilds();
      PaymentGroups.PaymentGroup paymentGroup2 = new PaymentGroups.PaymentGroup();
      paymentGroup2.Name = Translate.PaymentsGroupViewModel_Расходы;
      paymentGroup2.Uid = Guid.Empty;
      paymentGroup2.VisibleIn = PaymentGroups.VisiblePaymentGroup.Expense;
      groupWithChilds3.Group = paymentGroup2;
      PaymentsGroupViewModel.GroupWithChilds groupWithChilds4 = groupWithChilds3;
      groupsTree.AddRange((IEnumerable<PaymentsGroupViewModel.GroupWithChilds>) new PaymentsGroupViewModel.GroupWithChilds[2]
      {
        groupWithChilds2,
        groupWithChilds4
      });
      foreach (PaymentGroups.PaymentGroup paymentGroup3 in this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (g => g.ParentGroup == null && !g.IsDeleted)).ToList<PaymentGroups.PaymentGroup>())
      {
        PaymentsGroupViewModel.GroupWithChilds goodGroup = new PaymentsGroupViewModel.GroupWithChilds()
        {
          Group = paymentGroup3
        };
        this.AddChildrens(goodGroup);
        switch (goodGroup.Group.VisibleIn)
        {
          case PaymentGroups.VisiblePaymentGroup.Income:
            groupWithChilds2.Childrens.Add(goodGroup);
            continue;
          case PaymentGroups.VisiblePaymentGroup.Expense:
            groupWithChilds4.Childrens.Add(goodGroup);
            continue;
          default:
            continue;
        }
      }
      int num;
      foreach (PaymentsGroupViewModel.GroupWithChilds groupWithChilds5 in groupsTree)
      {
        switch (groupWithChilds5.Group.VisibleIn)
        {
          case PaymentGroups.VisiblePaymentGroup.Income:
            PaymentGroups.PaymentGroup group1 = groupWithChilds5.Group;
            string name1 = groupWithChilds5.Group.Name;
            num = this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (g => g.VisibleIn == PaymentGroups.VisiblePaymentGroup.Income)).Count<PaymentGroups.PaymentGroup>();
            string str1 = num.ToString();
            string str2 = name1 + " (" + str1 + ")";
            group1.Name = str2;
            continue;
          case PaymentGroups.VisiblePaymentGroup.Expense:
            PaymentGroups.PaymentGroup group2 = groupWithChilds5.Group;
            string name2 = groupWithChilds5.Group.Name;
            num = this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (g => g.VisibleIn == PaymentGroups.VisiblePaymentGroup.Expense)).Count<PaymentGroups.PaymentGroup>();
            string str3 = num.ToString();
            string str4 = name2 + " (" + str3 + ")";
            group2.Name = str4;
            continue;
          default:
            continue;
        }
      }
      return groupsTree;
    }

    private void AddChildrens(PaymentsGroupViewModel.GroupWithChilds goodGroup)
    {
      foreach (PaymentsGroupViewModel.GroupWithChilds goodGroup1 in this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (c =>
      {
        Guid? uid1 = c.ParentGroup?.Uid;
        Guid uid2 = goodGroup.Group.Uid;
        return uid1.HasValue && uid1.GetValueOrDefault() == uid2;
      })).Select<PaymentGroups.PaymentGroup, PaymentsGroupViewModel.GroupWithChilds>((Func<PaymentGroups.PaymentGroup, PaymentsGroupViewModel.GroupWithChilds>) (group => new PaymentsGroupViewModel.GroupWithChilds()
      {
        Group = group
      })))
      {
        this.AddChildrens(goodGroup1);
        goodGroup.Childrens.Add(goodGroup1);
      }
    }

    public ICommand AddCommand { get; set; }

    private void AddGroup()
    {
      PaymentGroups.PaymentGroup group;
      if (!new FrmPaymentGroupCard().ShowGroupCard(Guid.Empty, out group, this.SelectedGroup?.Group))
        return;
      this.CachedDbGroups.Add(group);
      this.LoadDataFromDb();
    }

    public ICommand EditCommand { get; set; }

    private void EditGroup()
    {
      if (this.SelectedGroup != null)
      {
        if (this.SelectedGroup.Group.Uid == Guid.Empty)
        {
          MessageBoxHelper.Warning(Translate.PaymentsGroupViewModel_Корневой_раздел_изменить_нельзя_);
        }
        else
        {
          PaymentGroups.PaymentGroup group;
          if (!new FrmPaymentGroupCard().ShowGroupCard(this.SelectedGroup.Group.Uid, out group, this.SelectedGroup.Group))
            return;
          this.CachedDbGroups[this.CachedDbGroups.ToList<PaymentGroups.PaymentGroup>().FindIndex((Predicate<PaymentGroups.PaymentGroup>) (x => x.Uid == group.Uid))] = group;
          this.LoadDataFromDb();
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public ICommand DeleteCommand { get; set; }

    private void DeleteGroup()
    {
      if (this.SelectedGroup != null)
      {
        if (this.SelectedGroup.Group.Uid == Guid.Empty)
        {
          MessageBoxHelper.Warning(Translate.PaymentsGroupViewModel_Корневой_раздел_удалить_нельзя_);
        }
        else
        {
          if (MessageBoxHelper.Show(Translate.FrmGoodsGroupsDelete, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          this.SelectedGroup.Group.IsDeleted = true;
          this.SelectedGroup.Group.Save();
          foreach (PaymentsGroupViewModel.GroupWithChilds children in this.SelectedGroup.Childrens)
          {
            children.Group.ParentGroup = (PaymentGroups.PaymentGroup) null;
            children.Group.Save();
          }
          this.CachedDbGroups = this.CachedDbGroups.Where<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (x => x.Uid != this.SelectedGroup.Group.Uid)).ToList<PaymentGroups.PaymentGroup>();
          this.LoadDataFromDb();
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public class GroupWithChilds
    {
      public PaymentGroups.PaymentGroup Group { get; set; } = new PaymentGroups.PaymentGroup();

      public List<PaymentsGroupViewModel.GroupWithChilds> Childrens { get; set; } = new List<PaymentsGroupViewModel.GroupWithChilds>();

      public bool IsChecked { get; set; }
    }
  }
}
