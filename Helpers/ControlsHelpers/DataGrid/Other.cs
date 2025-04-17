// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.DataGrid.Other
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.DataGrid
{
  public static class Other
  {
    public static void SelectFirstRow(object obj)
    {
      if (obj.GetType() == typeof (System.Windows.Controls.DataGrid))
        Other.FocusRow((System.Windows.Controls.DataGrid) obj);
      if (obj.GetType() != typeof (TreeView))
        return;
      TreeView treeView = (TreeView) obj;
      if (treeView.Items.Count == 0)
        return;
      if (treeView.ItemContainerGenerator.ContainerFromItem(treeView.Items[0]) is TreeViewItem treeViewItem)
        treeViewItem.IsSelected = true;
      treeView.Focus();
    }

    public static void FocusRow(System.Windows.Controls.DataGrid grid, bool toUp = false)
    {
      try
      {
        grid.Dispatcher.Invoke((Action) (() =>
        {
          if (grid.Items.Count == 0)
          {
            Gbs.Helpers.Other.ConsoleWrite("count = 0");
          }
          else
          {
            int selectedIndex = grid.SelectedIndex;
            int index = selectedIndex + (toUp ? -1 : 1);
            Gbs.Helpers.Other.ConsoleWrite(string.Format("ind = {0}", (object) index));
            if (index > grid.Items.Count - 1 || index < 0)
              index = 0;
            DataGridRow dataGridRow = (DataGridRow) grid.ItemContainerGenerator.ContainerFromIndex(index);
            object obj = grid.Items[index];
            grid.SelectedItem = obj;
            if (index == selectedIndex)
              return;
            grid.ScrollIntoView(obj);
            TraversalRequest request = toUp ? new TraversalRequest(FocusNavigationDirection.Previous) : new TraversalRequest(FocusNavigationDirection.Next);
            dataGridRow?.MoveFocus(request);
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
    {
      DependencyObject visualParentByType = startObject;
      while (visualParentByType != null && !type.IsInstanceOfType((object) visualParentByType))
        visualParentByType = VisualTreeHelper.GetParent(visualParentByType);
      return visualParentByType;
    }

    public static void ScrollToSelectedRow(this System.Windows.Controls.DataGrid grid)
    {
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        if (grid.SelectedItem == null)
          return;
        grid.ScrollIntoView(grid.SelectedItem);
      }));
    }
  }
}
