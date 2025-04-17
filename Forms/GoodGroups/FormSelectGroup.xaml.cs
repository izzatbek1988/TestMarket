// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.GroupsSelectedViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.GoodGroups
{
  public partial class GroupsSelectedViewModel : ViewModelWithForm
  {
    private string _filterText;
    private ObservableCollection<GroupsViewModel.GroupWithChilds> _groupList;

    public int TotalCountGroup { get; set; }

    public int SelectedCountGroup => this.SelectedList.Count;

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

    public GroupsViewModel.GroupWithChilds SelectedGroup { get; set; }

    public List<Gbs.Core.Entities.GoodGroups.Group> SelectedList { get; set; } = new List<Gbs.Core.Entities.GoodGroups.Group>();

    public List<Gbs.Core.Entities.GoodGroups.Group> ExpanderList { get; set; } = new List<Gbs.Core.Entities.GoodGroups.Group>();

    public Visibility VisibleCheckBox { get; set; }

    public ObservableCollection<GroupsViewModel.GroupWithChilds> GroupList
    {
      get => this._groupList;
      set
      {
        this._groupList = value;
        this.OnPropertyChanged(nameof (GroupList));
        this.OnPropertyChanged("TotalCountGroup");
        this.OnPropertyChanged("SelectedCountGroup");
      }
    }

    public ICommand GetSelectedGroup { get; set; }

    public ICommand SelectedAllGroupsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SelectedList.Clear();
          this.SelectedAllGroups((IEnumerable<GroupsViewModel.GroupWithChilds>) this.GroupList.ToList<GroupsViewModel.GroupWithChilds>());
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand OffSelectedAllGroupsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SelectedList.Clear();
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand SelectedParentGroupsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          foreach (GroupsViewModel.GroupWithChilds group in (Collection<GroupsViewModel.GroupWithChilds>) this.GroupList)
          {
            GroupsViewModel.GroupWithChilds item = group;
            if (this.SelectedList.All<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid != item.Group.Uid)))
              this.SelectedList.Add(item.Group);
          }
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand OffSelectedParentGroupsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          foreach (GroupsViewModel.GroupWithChilds group in (Collection<GroupsViewModel.GroupWithChilds>) this.GroupList)
          {
            GroupsViewModel.GroupWithChilds item = group;
            if (this.SelectedList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == item.Group.Uid)))
              this.SelectedList.Remove(item.Group);
          }
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand SelectedChildrensGroupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          foreach (GroupsViewModel.GroupWithChilds children in this.SelectedGroup.Childrens)
          {
            GroupsViewModel.GroupWithChilds item = children;
            if (this.SelectedList.All<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid != item.Group.Uid)))
              this.SelectedList.Add(item.Group);
            this.SelectedAllGroups((IEnumerable<GroupsViewModel.GroupWithChilds>) item.Childrens);
          }
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand OffSelectedChildrensGroupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          foreach (GroupsViewModel.GroupWithChilds children in this.SelectedGroup.Childrens)
          {
            GroupsViewModel.GroupWithChilds item = children;
            int index = this.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == item.Group.Uid));
            if (index != -1)
              this.SelectedList.RemoveAt(index);
            this.DeleteSelectedGroup(item.Childrens);
          }
          this.GetGroupsTree();
        }));
      }
    }

    public ICommand DownButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.TreeView.Items.Count == 0)
            return;
          int index = this.GroupList.ToList<GroupsViewModel.GroupWithChilds>().FindIndex((Predicate<GroupsViewModel.GroupWithChilds>) (x => x.Group.Uid == (this.SelectedGroup?.Group?.Uid ?? Guid.Empty)));
          if (index == -1)
            return;
          if (this.TreeView.ItemContainerGenerator.ContainerFromItem(this.TreeView.Items[index == this.TreeView.Items.Count - 1 ? 0 : index + 1]) is TreeViewItem treeViewItem2)
            treeViewItem2.IsSelected = true;
          this.TreeView.Focus();
        }));
      }
    }

    public ICommand UpButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.TreeView.Items.Count == 0)
            return;
          int index1 = this.GroupList.ToList<GroupsViewModel.GroupWithChilds>().FindIndex((Predicate<GroupsViewModel.GroupWithChilds>) (x => x.Group.Uid == this.SelectedGroup.Group.Uid));
          int index2;
          switch (index1)
          {
            case -1:
              return;
            case 0:
              index2 = this.TreeView.Items.Count - 1;
              break;
            default:
              index2 = index1 - 1;
              break;
          }
          if (this.TreeView.ItemContainerGenerator.ContainerFromItem(this.TreeView.Items[index2]) is TreeViewItem treeViewItem2)
            treeViewItem2.IsSelected = true;
          this.TreeView.Focus();
        }));
      }
    }

    public ICommand SpaceButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          if (this.SelectedGroup == null)
            return;
          if (this.SelectedGroup.IsChecked)
          {
            this.SelectedList.RemoveAt(this.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == this.SelectedGroup.Group.Uid)));
            this.GroupList[this.GroupList.ToList<GroupsViewModel.GroupWithChilds>().FindIndex((Predicate<GroupsViewModel.GroupWithChilds>) (x => x.Group.Uid == this.SelectedGroup.Group.Uid))].IsChecked = false;
          }
          else
          {
            this.SelectedList.Add(this.SelectedGroup.Group);
            this.GroupList[this.GroupList.ToList<GroupsViewModel.GroupWithChilds>().FindIndex((Predicate<GroupsViewModel.GroupWithChilds>) (x => x.Group.Uid == this.SelectedGroup.Group.Uid))].IsChecked = true;
          }
          this.OnPropertyChanged("GroupList");
        }));
      }
    }

    public Action Close { get; set; }

    public TreeView TreeView { get; set; }

    private List<Gbs.Core.Entities.GoodGroups.Group> CachedDbGroups { get; set; }

    private List<GroupsViewModel.GroupWithChilds> CashGroupList { get; set; } = new List<GroupsViewModel.GroupWithChilds>();

    public Users.User AuthUser { get; set; }

    public ICommand AddCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.AddGroup()));
    }

    public ICommand EditCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditGroup()));
    }

    private void AddGroup()
    {
      Gbs.Core.Entities.GoodGroups.Group group;
      if (!new FrmGoodGroupCard().ShowGroupCard(Guid.Empty, out group, this.AuthUser))
        return;
      this.CachedDbGroups.Add(group);
      this.GetGroupsTree();
    }

    private void EditGroup()
    {
      if (this.SelectedGroup != null)
      {
        Gbs.Core.Entities.GoodGroups.Group group;
        if (!new FrmGoodGroupCard().ShowGroupCard(this.SelectedGroup.Group.Uid, out group, this.AuthUser))
          return;
        this.CachedDbGroups[this.CachedDbGroups.ToList<Gbs.Core.Entities.GoodGroups.Group>().FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == group.Uid))] = group;
        this.GetGroupsTree();
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public GroupsSelectedViewModel()
    {
    }

    public GroupsSelectedViewModel(List<Gbs.Core.Entities.GoodGroups.Group> selectedList)
    {
      this.SelectedList = selectedList;
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.CachedDbGroups = new GoodGroupsRepository(dataBase).GetActiveItems();
        this.GetGroupsTree();
        this.GetSelectedGroup = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.AddItemInSelectedList();
          if (this.SelectedGroup == null && this.VisibleCheckBox == Visibility.Collapsed)
          {
            int num = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу);
          }
          else
          {
            this.Result = true;
            this.Close();
          }
        }));
      }
    }

    private void CheckedItem(GroupsViewModel.GroupWithChilds group)
    {
      foreach (GroupsViewModel.GroupWithChilds children in group.Childrens)
      {
        if (children.IsChecked)
          this.SelectedList.Add(children.Group);
        this.CheckedItem(children);
      }
    }

    public void AddItemInSelectedList()
    {
    }

    private void DeleteSelectedGroup(List<GroupsViewModel.GroupWithChilds> groups)
    {
      foreach (GroupsViewModel.GroupWithChilds group in groups)
      {
        GroupsViewModel.GroupWithChilds item = group;
        int index = this.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == item.Group.Uid));
        if (index != -1)
        {
          Gbs.Core.Entities.GoodGroups.Group selected = this.SelectedList[index];
          this.SelectedList.RemoveAt(index);
          this.DeleteSelectedGroup(item.Childrens);
        }
      }
    }

    public void GetGroupsTree()
    {
      if (this.FilterText.IsNullOrEmpty())
      {
        this.TotalCountGroup = this.CachedDbGroups.Count;
        this.CashGroupList = new List<GroupsViewModel.GroupWithChilds>();
        foreach (GroupsViewModel.GroupWithChilds goodGroup in this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.ParentGroupUid == Guid.Empty)).OrderBy<Gbs.Core.Entities.GoodGroups.Group, string>((Func<Gbs.Core.Entities.GoodGroups.Group, string>) (x => x.Name)).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (group => new GroupsViewModel.GroupWithChilds()
        {
          Group = group,
          IsChecked = this.SelectedList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid)),
          IsExpanded = this.ExpanderList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid))
        })))
        {
          this.AddChildrens(goodGroup);
          this.CashGroupList.Add(goodGroup);
        }
        this.GroupList = new ObservableCollection<GroupsViewModel.GroupWithChilds>(this.CashGroupList);
      }
      else
      {
        this.GroupList = new ObservableCollection<GroupsViewModel.GroupWithChilds>((IEnumerable<GroupsViewModel.GroupWithChilds>) this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Name.ToLower().Contains(this.FilterText.ToLower()))).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (x => new GroupsViewModel.GroupWithChilds()
        {
          Group = x,
          IsChecked = this.SelectedList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (g => g.Uid == x.Uid))
        })).OrderBy<GroupsViewModel.GroupWithChilds, string>((Func<GroupsViewModel.GroupWithChilds, string>) (x => x.Group.Name)));
        this.TotalCountGroup = this.GroupList.Count;
      }
      this.OnPropertyChanged("TotalCountGroup");
      this.OnPropertyChanged("SelectedCountGroup");
    }

    public void CountSelectedGroup() => this.OnPropertyChanged("SelectedCountGroup");

    private void AddChildrens(GroupsViewModel.GroupWithChilds goodGroup)
    {
      foreach (GroupsViewModel.GroupWithChilds goodGroup1 in this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (c => c.ParentGroupUid == goodGroup.Group.Uid)).OrderBy<Gbs.Core.Entities.GoodGroups.Group, string>((Func<Gbs.Core.Entities.GoodGroups.Group, string>) (x => x.Name)).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (group => new GroupsViewModel.GroupWithChilds()
      {
        Group = group,
        IsChecked = this.SelectedList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid)),
        IsExpanded = this.ExpanderList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid))
      })))
      {
        this.AddChildrens(goodGroup1);
        goodGroup.Childrens.Add(goodGroup1);
      }
    }

    private void SelectedAllGroups(
      IEnumerable<GroupsViewModel.GroupWithChilds> groupList)
    {
      foreach (GroupsViewModel.GroupWithChilds group in groupList)
      {
        GroupsViewModel.GroupWithChilds item = group;
        if (this.SelectedList.All<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid != item.Group.Uid)))
          this.SelectedList.Add(item.Group);
        this.SelectedAllGroups((IEnumerable<GroupsViewModel.GroupWithChilds>) item.Childrens);
      }
    }
  }
}
