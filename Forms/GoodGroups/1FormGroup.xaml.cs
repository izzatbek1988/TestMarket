// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.FormGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.GoodGroups
{
  public class FormGroup : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal TextBoxWithClearControl SearchTb;
    internal TreeView treeGroup;
    internal Button btnAddGroup;
    internal Button btnEditGroup;
    internal Button btnDelGroup;
    private bool _contentLoaded;

    private GroupsViewModel ViewModel { get; }

    public FormGroup()
    {
      this.InitializeComponent();
      this.ViewModel = new GroupsViewModel();
      this.ViewModel.CloseAction = new Action(((Window) this).Close);
      this.DataContext = (object) this.ViewModel;
      this.Object = (Control) this.treeGroup;
      this.SearchTextBox = this.SearchTb;
      this.CommandEnter = this.ViewModel.EditCommand;
      this.SetHotKeys();
    }

    private void TreeGroup_OnSelectedItemChanged(
      object sender,
      RoutedPropertyChangedEventArgs<object> e)
    {
      this.ViewModel.SelectedGroup = (GroupsViewModel.GroupWithChilds) this.treeGroup.SelectedValue;
    }

    private void TreeGroup_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.ViewModel.EditCommand.Execute((object) null);
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (obj => this.ViewModel.CloseAction()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
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
            hotKeys.DeleteItem,
            this.ViewModel.DeleteCommand
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

    private void FormGroup_OnClosed(object sender, EventArgs e)
    {
      CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goodgroups/formgroup.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.SearchTb = (TextBoxWithClearControl) target;
          break;
        case 2:
          this.treeGroup = (TreeView) target;
          this.treeGroup.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.TreeGroup_OnSelectedItemChanged);
          this.treeGroup.MouseDoubleClick += new MouseButtonEventHandler(this.TreeGroup_OnMouseDoubleClick);
          break;
        case 5:
          this.btnAddGroup = (Button) target;
          break;
        case 6:
          this.btnEditGroup = (Button) target;
          break;
        case 7:
          this.btnDelGroup = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 3)
      {
        if (connectionId != 4)
          return;
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
