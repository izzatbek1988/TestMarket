// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.FormSelectGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.GoodGroups
{
  public class FormSelectGroup : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal TextBoxWithClearControl SearchTb;
    internal TreeView treeGroup;
    private bool _contentLoaded;

    private GroupsSelectedViewModel ViewModel { get; set; }

    public FormSelectGroup()
    {
      this.InitializeComponent();
      this.Object = (Control) this.treeGroup;
      this.SearchTextBox = this.SearchTb;
      this.SetHotKeys();
    }

    public bool GetSingleSelectedGroupUid(Gbs.Core.Entities.Users.User authUser, out Gbs.Core.Entities.GoodGroups.Group group)
    {
      this.ViewModel = new GroupsSelectedViewModel(new List<Gbs.Core.Entities.GoodGroups.Group>())
      {
        VisibleCheckBox = Visibility.Collapsed,
        Close = new Action(((Window) this).Close),
        TreeView = this.treeGroup,
        AuthUser = authUser
      };
      this.DataContext = (object) this.ViewModel;
      this.CommandEnter = this.ViewModel.GetSelectedGroup;
      this.ShowDialog();
      group = this.ViewModel.SelectedGroup?.Group;
      return this.ViewModel.Result && this.ViewModel.SelectedGroup != null;
    }

    public bool GetSelectedGroupUid(Gbs.Core.Entities.Users.User authUser, ref List<Gbs.Core.Entities.GoodGroups.Group> list)
    {
      this.ViewModel = new GroupsSelectedViewModel(list)
      {
        VisibleCheckBox = Visibility.Visible,
        Close = new Action(((Window) this).Close),
        TreeView = this.treeGroup,
        AuthUser = authUser
      };
      this.DataContext = (object) this.ViewModel;
      this.ShowDialog();
      return this.ViewModel.Result;
    }

    private void TreeGroup_OnSelectedItemChanged(
      object sender,
      RoutedPropertyChangedEventArgs<object> e)
    {
      this.ViewModel.SelectedGroup = (GroupsViewModel.GroupWithChilds) this.treeGroup.SelectedValue;
    }

    private void TreeGroup_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.ViewModel.AddItemInSelectedList();
      this.ViewModel.CountSelectedGroup();
      if (this.ViewModel.VisibleCheckBox == Visibility.Visible)
      {
        GroupsViewModel.GroupWithChilds item = (GroupsViewModel.GroupWithChilds) ((TreeView) sender).SelectedItem;
        if (item == null)
          return;
        if (((ItemsControl) e.Source).ItemContainerGenerator.ContainerFromIndex(0) is TreeViewItem treeViewItem)
        {
          treeViewItem.IsSelected = true;
          treeViewItem.IsSelected = false;
        }
        if (item.IsChecked)
          this.ViewModel.SelectedList.RemoveAt(this.ViewModel.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == item.Group.Uid)));
        else
          this.ViewModel.SelectedList.Add(item.Group);
        item.IsChecked = !item.IsChecked;
        item.IsExpanded = !item.IsExpanded;
      }
      else
      {
        this.ViewModel.GetSelectedGroup.Execute((object) null);
        this.ViewModel.CountSelectedGroup();
      }
    }

    private void TreeGroup_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      if (this.ViewModel.SelectedGroup != null || this.treeGroup.Items.Count == 0)
        return;
      if (this.treeGroup.ItemContainerGenerator.ContainerFromItem(this.treeGroup.Items[0]) is TreeViewItem treeViewItem)
        treeViewItem.IsSelected = true;
      this.treeGroup.Focus();
      this.ViewModel.SelectedGroup = (GroupsViewModel.GroupWithChilds) this.treeGroup.SelectedValue;
    }

    private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
    {
      if (!(sender is TreeViewItem treeViewItem))
        return;
      treeViewItem.Focus();
    }

    private void ExpanderTreeView(object sender, RoutedEventArgs e)
    {
      TreeViewItem item = sender as TreeViewItem;
      if (item == null)
        return;
      if (this.ViewModel.ExpanderList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == ((GroupsViewModel.GroupWithChilds) item.DataContext).Group.Uid)))
      {
        if (item.IsExpanded)
          return;
        this.ViewModel.ExpanderList.RemoveAll((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == ((GroupsViewModel.GroupWithChilds) item.DataContext).Group.Uid));
      }
      else
      {
        if (!item.IsExpanded)
          return;
        this.ViewModel.ExpanderList.Add(((GroupsViewModel.GroupWithChilds) item.DataContext).Group);
      }
    }

    private void Chb1_OnChecked(object sender, RoutedEventArgs e)
    {
      Gbs.Core.Entities.GoodGroups.Group item = (Gbs.Core.Entities.GoodGroups.Group) ((FrameworkElement) sender).Tag;
      if (item == null)
        return;
      if (this.ViewModel.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == item.Uid)) == -1)
        this.ViewModel.SelectedList.Add(item);
      if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        GroupsViewModel.GroupWithChilds groupFromClick = this.GetGroupFromClick(item.Uid);
        if (groupFromClick != null)
        {
          this.ViewModel.SelectedGroup = groupFromClick;
          this.ViewModel.SelectedChildrensGroupCommand.Execute((object) groupFromClick);
        }
      }
      this.ViewModel.CountSelectedGroup();
    }

    private GroupsViewModel.GroupWithChilds GetGroupFromClick(
      Guid uid,
      List<GroupsViewModel.GroupWithChilds> groups = null)
    {
      if (groups == null)
        groups = this.ViewModel.GroupList.ToList<GroupsViewModel.GroupWithChilds>();
      GroupsViewModel.GroupWithChilds groupFromClick1 = groups.FirstOrDefault<GroupsViewModel.GroupWithChilds>((Func<GroupsViewModel.GroupWithChilds, bool>) (g => g.Group.Uid == uid));
      if (groupFromClick1 == null)
      {
        foreach (GroupsViewModel.GroupWithChilds group in groups)
        {
          GroupsViewModel.GroupWithChilds groupFromClick2 = this.GetGroupFromClick(uid, group.Childrens);
          if (groupFromClick2 != null)
            return groupFromClick2;
        }
      }
      return groupFromClick1;
    }

    private void Chb1_OnUnchecked(object sender, RoutedEventArgs e)
    {
      Gbs.Core.Entities.GoodGroups.Group item = (Gbs.Core.Entities.GoodGroups.Group) ((FrameworkElement) sender).Tag;
      if (item == null)
        return;
      int index = this.ViewModel.SelectedList.FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == item.Uid));
      if (index != -1)
        this.ViewModel.SelectedList.RemoveAt(index);
      if (Keyboard.Modifiers == ModifierKeys.Control)
      {
        GroupsViewModel.GroupWithChilds groupFromClick = this.GetGroupFromClick(item.Uid);
        if (groupFromClick != null)
        {
          this.ViewModel.SelectedGroup = groupFromClick;
          this.ViewModel.OffSelectedChildrensGroupCommand.Execute((object) groupFromClick);
        }
      }
      this.ViewModel.CountSelectedGroup();
    }

    private void SetHotKeys()
    {
      try
      {
        if (this.ViewModel == null)
          return;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (obj => this.ViewModel.CloseAction()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            new HotKeysHelper.Hotkey(Key.Return),
            this.ViewModel.EditCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand
          },
          {
            hotKeys.AddItem,
            this.ViewModel.AddCommand
          },
          {
            hotKeys.EditItem,
            this.ViewModel.EditCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goodgroups/formselectgroup.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 2)
      {
        if (connectionId == 3)
        {
          this.treeGroup = (TreeView) target;
          this.treeGroup.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.TreeGroup_OnSelectedItemChanged);
          this.treeGroup.MouseDoubleClick += new MouseButtonEventHandler(this.TreeGroup_OnMouseDoubleClick);
          this.treeGroup.ContextMenuOpening += new ContextMenuEventHandler(this.TreeGroup_OnContextMenuOpening);
        }
        else
          this._contentLoaded = true;
      }
      else
        this.SearchTb = (TextBoxWithClearControl) target;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId != 4)
          return;
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.Chb1_OnChecked);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.Chb1_OnUnchecked);
      }
      else
      {
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseRightButtonDownEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.EventSetter_OnHandler)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = TreeViewItem.ExpandedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.ExpanderTreeView)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = TreeViewItem.CollapsedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.ExpanderTreeView)
        });
      }
    }
  }
}
