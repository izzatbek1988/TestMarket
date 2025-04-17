// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmFavoritesGoods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmFavoritesGoods : WindowWithSize, IComponentConnector
  {
    private int _prevRowIndex = -1;
    internal Button FindButton;
    internal System.Windows.Controls.DataGrid dgEmployee;
    private bool _contentLoaded;

    public FrmFavoritesGoods()
    {
      this.InitializeComponent();
      this.dgEmployee.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.dgEmployee_PreviewMouseLeftButtonDown);
      this.dgEmployee.Drop += new DragEventHandler(this.dgEmployee_Drop);
      this.Object = (Control) this.dgEmployee;
      this.SetHotKeys();
    }

    public void ShowCard(Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool> action)
    {
      SelectGoodsViewModel dataContext = (SelectGoodsViewModel) this.DataContext;
      dataContext.AddBasket = action;
      dataContext.CloseAction = new Action(((Window) this).Close);
      dataContext.FormToSHow = (WindowWithSize) this;
      Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow(this.dgEmployee);
      this.ShowDialog();
    }

    private void dgEmployee_Drop(object sender, DragEventArgs e)
    {
      if (this._prevRowIndex < 0)
        return;
      int itemCurrentRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmFavoritesGoods.GetDragDropPosition(e.GetPosition));
      if (itemCurrentRowIndex < 0 || itemCurrentRowIndex == this._prevRowIndex)
        return;
      ObservableCollection<SelectGood> selectGoodsList = ((SelectGoodsViewModel) this.DataContext).SelectGoodsList;
      SelectGood selectGood = selectGoodsList[this._prevRowIndex];
      selectGoodsList.RemoveAt(this._prevRowIndex);
      selectGoodsList.Insert(itemCurrentRowIndex, selectGood);
      ((SelectGoodsViewModel) this.DataContext).IsEditing = true;
    }

    private void dgEmployee_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this._prevRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmFavoritesGoods.GetDragDropPosition(((MouseEventArgs) e).GetPosition));
      if (this._prevRowIndex < 0)
        return;
      this.dgEmployee.SelectedIndex = this._prevRowIndex;
      if (!(this.dgEmployee.Items[this._prevRowIndex] is SelectGood data))
        return;
      DragDropEffects allowedEffects = DragDropEffects.Move;
      if (DragDrop.DoDragDrop((DependencyObject) this.dgEmployee, (object) data, allowedEffects) == DragDropEffects.None)
        return;
      this.dgEmployee.SelectedItem = (object) data;
    }

    private bool IsTheMouseOnTargetRow(Visual theTarget, FrmFavoritesGoods.GetDragDropPosition pos)
    {
      return theTarget != null && VisualTreeHelper.GetDescendantBounds(theTarget).Contains(pos((IInputElement) theTarget));
    }

    private DataGridRow GetDataGridRowItem(int index)
    {
      return this.dgEmployee.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated ? (DataGridRow) null : this.dgEmployee.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
    }

    private int GetDataGridItemCurrentRowIndex(FrmFavoritesGoods.GetDragDropPosition pos)
    {
      int itemCurrentRowIndex = -1;
      for (int index = 0; index < this.dgEmployee.Items.Count; ++index)
      {
        if (this.IsTheMouseOnTargetRow((Visual) this.GetDataGridRowItem(index), pos))
        {
          itemCurrentRowIndex = index;
          break;
        }
      }
      return itemCurrentRowIndex;
    }

    private void FrmSelectGoods_OnClosing(object sender, CancelEventArgs e)
    {
      if (!((SelectGoodsViewModel) this.DataContext).SearchQuery.IsNullOrEmpty())
        ((SelectGoodsViewModel) this.DataContext).SearchQuery = string.Empty;
      if (!((SelectGoodsViewModel) this.DataContext).IsEditing)
        return;
      ObservableCollection<SelectGood> selectGoodsList = ((SelectGoodsViewModel) this.DataContext).SelectGoodsList;
      foreach (SelectGood selectGood in (Collection<SelectGood>) selectGoodsList)
      {
        SelectGood item = selectGood;
        item.Index = selectGoodsList.ToList<SelectGood>().FindIndex((Predicate<SelectGood>) (x => x.Uid == item.Uid));
        new SelectGoodsRepository().Save(item);
      }
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        SelectGoodsViewModel model = (SelectGoodsViewModel) this.DataContext;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (obj => model.CloseAction()));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (o => model.AddBasketCommand.Execute((object) this.dgEmployee.SelectedItems)));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            (ICommand) relayCommand2
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand1
          },
          {
            hotKeys.AddItem,
            model.AddCommand
          },
          {
            hotKeys.EditItem,
            model.EditCommand
          },
          {
            hotKeys.DeleteItem,
            model.DeleteCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand1
          },
          {
            new HotKeysHelper.Hotkey(Key.Return),
            (ICommand) relayCommand2
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void FrmFavoritesGoods_OnLoaded(object sender, RoutedEventArgs e)
    {
      SelectGoodsViewModel dataContext = (SelectGoodsViewModel) this.DataContext;
      ContextMenu resource1 = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsSearchModelView.FilterProperty filterProperty in (Collection<GoodsSearchModelView.FilterProperty>) dataContext.FilterProperties)
      {
        ItemCollection items = resource1.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource1.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
      ContextMenu resource2 = (ContextMenu) this.dgEmployee.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.dgEmployee.Columns)
      {
        ItemCollection items = resource2.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = column.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
        newItem.IsCheckable = true;
        newItem.IsChecked = column.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      resource2.Closed += new RoutedEventHandler(this.CmOnClosed);
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.dgEmployee, (ContextMenu) sender);
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      SelectGoodsViewModel model = (SelectGoodsViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>(source.Select<MenuItem, GoodsSearchModelView.FilterProperty>((Func<MenuItem, GoodsSearchModelView.FilterProperty>) (x => new GoodsSearchModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = model.FilterProperties.Single<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString())).Text
      })));
      model.Search();
    }

    private void FrmFavoritesGoods_OnClosed(object sender, EventArgs e)
    {
      SelectGoodsViewModel dataContext = (SelectGoodsViewModel) this.DataContext;
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      dataContext.Setting.FavoritesGoodsSearch = new FavoritesGoodsSearchOptions()
      {
        IsName = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Name")).IsChecked,
        IsAlias = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Alias")).IsChecked
      };
    }

    private void FindButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmfavoritesgoods.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.dgEmployee = (System.Windows.Controls.DataGrid) target;
        else
          this._contentLoaded = true;
      }
      else
      {
        this.FindButton = (Button) target;
        this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClick);
      }
    }

    private delegate Point GetDragDropPosition(IInputElement theElement);
  }
}
