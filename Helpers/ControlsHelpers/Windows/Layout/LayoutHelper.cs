// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.Windows.Layout.LayoutHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Config.Files;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.Windows.Layout
{
  public class LayoutHelper
  {
    private WindowOptions.FileWindow _windowOption;
    private Window _window;

    private (WindowOptions.FileWindow option, bool res) GetFileWindow(Window win)
    {
      string str = Path.Combine(ApplicationInfo.GetInstance().Paths.ConfigsPath, "WindowOptions");
      string path = Path.Combine(str, win.GetType().Name + ".json");
      if (!File.Exists(path))
      {
        WindowOptions.FileWindow fileWindow = new WindowOptions.FileWindow()
        {
          Option = new WindowOptions.WindowOption()
          {
            Key = win.GetType().Name
          }
        };
        Directory.CreateDirectory(str);
        File.WriteAllText(path, fileWindow.ToJsonString(true));
        return (fileWindow, false);
      }
      return (JsonConvert.DeserializeObject<WindowOptions.FileWindow>(File.ReadAllText(path), new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace
      }), true);
    }

    private bool SaveOptionWindow(WindowOptions.FileWindow obj)
    {
      try
      {
        File.WriteAllText(Path.Combine(ApplicationInfo.GetInstance().Paths.ConfigsPath, "WindowOptions", this._window.GetType().Name + ".json"), obj.ToJsonString(true));
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void RestoreDataGridSort(DataGrid grid, WindowOptions.DataGridView op)
    {
      List<Type> variants = new List<Type>()
      {
        typeof (FrmSearchGoods),
        typeof (FrmGoodsCatalog)
      };
      if (!this._window.GetType().IsEither<Type>((IEnumerable<Type>) variants))
        return;
      try
      {
        if (op.SortOrders == null)
          return;
        grid.EnableRowVirtualization = true;
        grid.SetValue(VirtualizingPanel.IsVirtualizingProperty, (object) true);
        grid.SetValue(VirtualizingPanel.VirtualizationModeProperty, (object) VirtualizationMode.Recycling);
        grid.Items.SortDescriptions.Clear();
        foreach (WindowOptions.SortOrder sortOrder1 in op.SortOrders)
        {
          WindowOptions.SortOrder sortOrder = sortOrder1;
          DataGridColumn dataGridColumn = grid.Columns.FirstOrDefault<DataGridColumn>((Func<DataGridColumn, bool>) (c => c.SortMemberPath == sortOrder.Column));
          if (dataGridColumn != null)
          {
            ListSortDirection direction = sortOrder.Direction;
            grid.Items.SortDescriptions.Add(new SortDescription(sortOrder.Column, direction));
            dataGridColumn.SortDirection = new ListSortDirection?(direction);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "datagrid sort error");
      }
    }

    public LayoutHelper(Window window)
    {
      this._window = window;
      string title = window.Title;
      LogHelper.Debug("Загружаем размеры для формы " + title);
      (WindowOptions.FileWindow option, bool res) fileWindow = this.GetFileWindow(window);
      WindowOptions.FileWindow option = fileWindow.option;
      if (!fileWindow.res)
        LogHelper.Debug("Нет данных для формы " + title + ", не изменяем ничего");
      else if (option == null)
      {
        LogHelper.Debug("win option is null");
      }
      else
      {
        if (option.Option == null)
          option.Option = new WindowOptions.WindowOption();
        this._windowOption = option;
      }
    }

    public void LoadOption()
    {
      if (this._windowOption == null || this._window.GetType() == typeof (FrmSelectGoodStock) || this._window.GetType() == typeof (Gbs.Forms.Other.MessageBox))
        return;
      List<DataGrid> dataGridList = (List<DataGrid>) null;
      try
      {
        dataGridList = this.GetListDataGridInWindow();
        List<Gbs.Core.Entities.Users.User> source = new List<Gbs.Core.Entities.Users.User>();
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        {
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            source = new UsersRepository(dataBase).GetAllItems();
        }
        else
          source = CachesBox.AllUsers();
        List<Gbs.Core.Entities.Users.User> list = source.Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsDeleted && !x.IsKicked && x.OnlineOnSectionUid == Sections.GetCurrentSection().Uid)).ToList<Gbs.Core.Entities.Users.User>();
        double num = 0.0;
        if (list.Any<Gbs.Core.Entities.Users.User>())
          num = list.Count == 1 ? (double) list.Single<Gbs.Core.Entities.Users.User>().FontSize : (double) list.Max<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, Decimal>) (x => x.FontSize));
        foreach (DataGrid dataGrid in dataGridList)
        {
          string gridName = dataGrid.Name;
          if (!gridName.IsEither<string>("ListUsersAuth", "GridPayments", "ListTemplates", "ListPoint"))
            dataGrid.FontSize = Math.Abs(num) <= 0.0 ? dataGrid.FontSize : num;
          LogHelper.Trace("Изменяем визуальные настройки для грида " + gridName);
          if (this._windowOption.DataGridList.Any<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == gridName)))
          {
            WindowOptions.DataGridView dataGridView = this._windowOption.DataGridList.First<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == gridName));
            LogHelper.Trace("Изменяем визуальные настройки столбцов грида " + gridName);
            foreach (DataGridColumn column1 in (Collection<DataGridColumn>) dataGrid.Columns)
            {
              DataGridColumn column = column1;
              WindowOptions.ColumnOption columnOption = dataGridView.ColumnOptions.FirstOrDefault<WindowOptions.ColumnOption>((Func<WindowOptions.ColumnOption, bool>) (x => x.Name == this.getColumnId((DependencyObject) column)));
              if (columnOption != null)
              {
                LogHelper.Trace(string.Format("Столбец {0}: ширина - {1} (тип - {2}), видимость - {3}, позиция - {4}", column.Header, (object) columnOption.Width, (object) columnOption.Type, (object) columnOption.IsVisibility, (object) columnOption.Position));
                column.Width = new DataGridLength(columnOption.Width.Value, columnOption.Type);
                if (gridName != "StockGrid")
                  column.Visibility = columnOption.IsVisibility ? Visibility.Visible : Visibility.Collapsed;
                if (columnOption.Position != -1 && columnOption.Position < dataGrid.Columns.Count)
                  column.DisplayIndex = columnOption.Position;
              }
            }
          }
        }
        foreach (RowDefinition rowDefinition in this.GetListRowInWindow(this._window).Where<RowDefinition>((Func<RowDefinition, bool>) (x => !x.Name.IsNullOrEmpty())))
        {
          RowDefinition row = rowDefinition;
          if (this._windowOption.RowList.Any<WindowOptions.GridRowDefinition>((Func<WindowOptions.GridRowDefinition, bool>) (x => x.Name == row.Name)))
          {
            WindowOptions.GridRowDefinition gridRowDefinition = this._windowOption.RowList.First<WindowOptions.GridRowDefinition>((Func<WindowOptions.GridRowDefinition, bool>) (x => x.Name == row.Name));
            LogHelper.Trace(string.Format("Изменяем высоту строки {0}: высота - {1}", (object) row.Name, (object) gridRowDefinition.Height));
            row.Height = gridRowDefinition.Height;
          }
        }
        foreach (ColumnDefinition columnDefinition1 in this.GetLisColumnInWindow(this._window).Where<ColumnDefinition>((Func<ColumnDefinition, bool>) (x => !x.Name.IsNullOrEmpty())))
        {
          ColumnDefinition column = columnDefinition1;
          if (this._windowOption.ColumnDefinitionsList.Any<WindowOptions.GridColumnDefinition>((Func<WindowOptions.GridColumnDefinition, bool>) (x => x.Name == column.Name)))
          {
            WindowOptions.GridColumnDefinition columnDefinition2 = this._windowOption.ColumnDefinitionsList.First<WindowOptions.GridColumnDefinition>((Func<WindowOptions.GridColumnDefinition, bool>) (x => x.Name == column.Name));
            LogHelper.Trace(string.Format("Изменяем ширину столбца {0}: ibhbyf - {1}", (object) column.Name, (object) columnDefinition2.Width));
            column.Width = columnDefinition2.Width;
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Debug("Grid list: " + (dataGridList != null ? dataGridList.ToJsonString(true) : (string) null));
        LogHelper.Error(ex, "Ошибка загрузки свойств формы", false);
      }
    }

    public void LoadWindowsSize()
    {
      if (this._windowOption == null)
        return;
      try
      {
        if (this._window.ResizeMode != ResizeMode.NoResize)
        {
          LogHelper.Trace(string.Format("Изменяем размеры формы {0}: высота - {1}, ширина - {2}", (object) this._window.Title, (object) this._windowOption.Option.Height, (object) this._windowOption.Option.Width));
          if (!this._windowOption.Option.IsOpenFullScreen)
          {
            this._window.Width = this._windowOption.Option.Width;
            this._window.Height = this._windowOption.Option.Height;
          }
          this._window.WindowState = this._windowOption.Option.IsOpenFullScreen ? WindowState.Maximized : WindowState.Normal;
        }
        else
          LogHelper.Trace("Форма " + this._window.Name + " NoResize, не изменяем размеры");
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public void LoadSort()
    {
      if (this._windowOption == null)
        return;
      try
      {
        foreach (DataGrid grid in this.GetListDataGridInWindow())
        {
          string gridName = grid.Name;
          if (this._windowOption.DataGridList.Any<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == gridName)))
          {
            WindowOptions.DataGridView op = this._windowOption.DataGridList.First<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == gridName));
            this.RestoreDataGridSort(grid, op);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public void LoadColumn(DataGrid grid, DataGridTextColumn column)
    {
      if (this._windowOption == null)
        return;
      try
      {
        WindowOptions.DataGridView dataGridView = this._windowOption.DataGridList.FirstOrDefault<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == grid.Name));
        WindowOptions.ColumnOption columnOption = dataGridView != null ? dataGridView.ColumnOptions.FirstOrDefault<WindowOptions.ColumnOption>((Func<WindowOptions.ColumnOption, bool>) (x => x.Name == this.getColumnId((DependencyObject) column))) : (WindowOptions.ColumnOption) null;
        if (columnOption == null)
          return;
        column.Width = new DataGridLength(columnOption.Width.Value, columnOption.Type);
        column.Visibility = columnOption.IsVisibility ? Visibility.Visible : Visibility.Collapsed;
        if (columnOption.Position == -1 || columnOption.Position >= grid.Columns.Count)
          return;
        column.DisplayIndex = columnOption.Position;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        throw;
      }
    }

    public bool UpdateOption()
    {
      try
      {
        WindowOptions.FileWindow fileWindow = this.GetFileWindow(this._window).option ?? new WindowOptions.FileWindow();
        if (this._window.ResizeMode != ResizeMode.NoResize)
        {
          string title = this._window.Title;
          LogHelper.Debug("Обновляем настройки размеров для формы " + title + "; " + this._window.GetType().Name);
          LogHelper.Trace(string.Format("Обновляем настройки размеров формы {0}: высота - {1}, ширина - {2}", (object) title, (object) this._window.Height, (object) this._window.Width));
          if (this._window.WindowState != WindowState.Maximized)
          {
            fileWindow.Option.Height = this._window.Height;
            fileWindow.Option.Width = this._window.Width;
          }
          fileWindow.Option.IsOpenFullScreen = this._window.WindowState == WindowState.Maximized;
        }
        else
          LogHelper.Trace("Форма NoResize, не сохраняем размеры");
        foreach (DataGrid dataGrid in this.GetListDataGridInWindow())
        {
          string gridName = dataGrid.Name ?? string.Empty;
          LogHelper.Trace("Обновляем визуальные настройки столбцов датагрида " + gridName);
          WindowOptions.DataGridView dataGridView = fileWindow != null ? fileWindow.DataGridList.FirstOrDefault<WindowOptions.DataGridView>((Func<WindowOptions.DataGridView, bool>) (x => x.Name == gridName)) : (WindowOptions.DataGridView) null;
          if (dataGridView == null)
          {
            dataGridView = new WindowOptions.DataGridView()
            {
              Name = gridName
            };
            fileWindow.DataGridList.Add(dataGridView);
          }
          foreach (DataGridColumn column1 in (Collection<DataGridColumn>) dataGrid.Columns)
          {
            DataGridColumn column = column1;
            WindowOptions.ColumnOption columnOption = dataGridView != null ? dataGridView.ColumnOptions.FirstOrDefault<WindowOptions.ColumnOption>((Func<WindowOptions.ColumnOption, bool>) (x => x.Name == this.getColumnId((DependencyObject) column))) : (WindowOptions.ColumnOption) null;
            if (columnOption == null)
            {
              columnOption = new WindowOptions.ColumnOption();
              dataGridView?.ColumnOptions.Add(columnOption);
            }
            columnOption.Name = this.getColumnId((DependencyObject) column);
            columnOption.IsVisibility = column.Visibility == Visibility.Visible;
            columnOption.Position = column.DisplayIndex;
            columnOption.Width = (DataGridLength) column.Width.Value;
            columnOption.Type = column.Width.UnitType;
            LogHelper.Trace(string.Format("Обновляем файл настроек: столбец {0}: ширина - {1} (тип - {2}), видимость - {3}, позиция - {4}", column.Header, (object) column.Width.Value, (object) column.Width.UnitType, (object) column.Visibility, (object) column.DisplayIndex));
          }
          dataGridView.SortOrders = dataGrid.Items.SortDescriptions.Select<SortDescription, WindowOptions.SortOrder>((Func<SortDescription, WindowOptions.SortOrder>) (sd => new WindowOptions.SortOrder()
          {
            Column = sd.PropertyName,
            Direction = sd.Direction
          })).ToList<WindowOptions.SortOrder>();
        }
        foreach (RowDefinition rowDefinition in this.GetListRowInWindow(this._window).Where<RowDefinition>((Func<RowDefinition, bool>) (x => !x.Name.IsNullOrEmpty())))
        {
          RowDefinition row = rowDefinition;
          LogHelper.Trace("Обновляем визуальные настройки строк грида " + ((FrameworkElement) row.Parent).Name);
          WindowOptions.GridRowDefinition gridRowDefinition = fileWindow.RowList.FirstOrDefault<WindowOptions.GridRowDefinition>((Func<WindowOptions.GridRowDefinition, bool>) (x => x.Name == row.Name));
          if (gridRowDefinition == null)
          {
            fileWindow.RowList.Add(new WindowOptions.GridRowDefinition()
            {
              Name = row.Name,
              Height = row.Height
            });
          }
          else
          {
            gridRowDefinition.Height = row.Height;
            LogHelper.Trace(string.Format("Обновляем файл настроек: строка {0}:высота - {1}", (object) gridRowDefinition.Name, (object) gridRowDefinition.Height));
          }
        }
        foreach (ColumnDefinition columnDefinition1 in this.GetLisColumnInWindow(this._window).Where<ColumnDefinition>((Func<ColumnDefinition, bool>) (x => !x.Name.IsNullOrEmpty())))
        {
          ColumnDefinition column = columnDefinition1;
          LogHelper.Trace("Обновляем визуальные настройки столбцов грида " + ((FrameworkElement) column.Parent).Name);
          WindowOptions.GridColumnDefinition columnDefinition2 = fileWindow.ColumnDefinitionsList.FirstOrDefault<WindowOptions.GridColumnDefinition>((Func<WindowOptions.GridColumnDefinition, bool>) (x => x.Name == column.Name));
          if (columnDefinition2 == null)
          {
            fileWindow.ColumnDefinitionsList.Add(new WindowOptions.GridColumnDefinition()
            {
              Name = column.Name,
              Width = column.Width
            });
          }
          else
          {
            columnDefinition2.Width = column.Width;
            LogHelper.Trace(string.Format("Обновляем файл настроек: столбцов {0}: ширина - {1}", (object) columnDefinition2.Name, (object) columnDefinition2.Width));
          }
        }
        return this.SaveOptionWindow(fileWindow);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошика сохранения размера формы", false);
        return false;
      }
    }

    private string getColumnId(DependencyObject column)
    {
      string guid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid(column);
      if (!guid.IsNullOrEmpty())
        return guid;
      return column.GetType() != typeof (DataGridTemplateColumn) ? ((Binding) ((DataGridBoundColumn) column).Binding).Path.Path : Guid.NewGuid().ToString();
    }

    private List<DataGrid> GetListDataGridInWindow()
    {
      if (this._window.Content == null)
        return new List<DataGrid>();
      if (this._window.Content?.GetType() != typeof (Grid))
        return new List<DataGrid>();
      List<DataGrid> source = new List<DataGrid>();
      foreach (object child in ((Panel) this._window.Content).Children)
      {
        if (child.GetType() == typeof (DataGrid))
          source.Add((DataGrid) child);
        if (child.GetType().IsEither<Type>(typeof (Grid), typeof (StackPanel)))
          source.AddRange((IEnumerable<DataGrid>) this.GetListChild((System.Windows.UIElement) child));
        if (child.GetType() == typeof (GroupBox))
          source.AddRange((IEnumerable<DataGrid>) this.GetListChild((System.Windows.UIElement) ((ContentControl) child).Content));
        if (child.GetType() == typeof (TabControl))
          source.AddRange((IEnumerable<DataGrid>) this.GetListGridInTabControl((TabControl) child));
      }
      return source.Where<DataGrid>((Func<DataGrid, bool>) (x => x.Name != null)).ToList<DataGrid>();
    }

    private List<DataGrid> GetListChild(System.Windows.UIElement element)
    {
      List<DataGrid> listChild = new List<DataGrid>();
      Grid grid = (Grid) null;
      StackPanel stackPanel = (StackPanel) null;
      if (element.GetType() == typeof (Grid))
      {
        grid = (Grid) element;
      }
      else
      {
        if (!(element.GetType() == typeof (StackPanel)))
          return new List<DataGrid>();
        stackPanel = (StackPanel) element;
      }
      foreach (object obj in grid == null ? stackPanel.Children : grid.Children)
      {
        if (obj.GetType() == typeof (DataGrid))
          listChild.Add((DataGrid) obj);
        if (obj.GetType() == typeof (GroupBox))
          listChild.AddRange((IEnumerable<DataGrid>) this.GetListChild((System.Windows.UIElement) ((ContentControl) obj).Content));
        if (obj.GetType().IsEither<Type>(typeof (Grid), typeof (StackPanel)))
          listChild.AddRange((IEnumerable<DataGrid>) this.GetListChild((System.Windows.UIElement) obj));
        if (obj.GetType() == typeof (TabControl))
          listChild.AddRange((IEnumerable<DataGrid>) this.GetListGridInTabControl((TabControl) obj));
      }
      return listChild;
    }

    private List<DataGrid> GetListGridInTabControl(TabControl tab)
    {
      List<DataGrid> gridInTabControl = new List<DataGrid>();
      foreach (TabItem tabItem in (IEnumerable) tab.Items)
        gridInTabControl.AddRange((IEnumerable<DataGrid>) this.GetListChild((System.Windows.UIElement) tabItem.Content));
      return gridInTabControl;
    }

    private List<RowDefinition> GetListRowInWindow(Window window)
    {
      if (window.Content == null)
        return new List<RowDefinition>();
      if (window.Content?.GetType() != typeof (Grid))
        return new List<RowDefinition>();
      List<RowDefinition> listRowInWindow = new List<RowDefinition>();
      listRowInWindow.AddRange((IEnumerable<RowDefinition>) ((Grid) window.Content).RowDefinitions);
      foreach (object child in ((Panel) window.Content).Children)
      {
        if (child.GetType() == typeof (Grid))
          listRowInWindow.AddRange((IEnumerable<RowDefinition>) this.GetListChildRow((System.Windows.UIElement) child));
      }
      return listRowInWindow;
    }

    private List<RowDefinition> GetListChildRow(System.Windows.UIElement element)
    {
      List<RowDefinition> listChildRow = new List<RowDefinition>();
      if (!(element.GetType() == typeof (Grid)))
        return new List<RowDefinition>();
      Grid grid = (Grid) element;
      listChildRow.AddRange((IEnumerable<RowDefinition>) grid.RowDefinitions);
      foreach (object child in grid.Children)
      {
        if (child.GetType() == typeof (Grid))
          listChildRow.AddRange((IEnumerable<RowDefinition>) this.GetListChildRow((System.Windows.UIElement) child));
      }
      return listChildRow;
    }

    private List<ColumnDefinition> GetLisColumnInWindow(Window window)
    {
      if (window.Content == null)
        return new List<ColumnDefinition>();
      if (window.Content?.GetType() != typeof (Grid))
        return new List<ColumnDefinition>();
      List<ColumnDefinition> lisColumnInWindow = new List<ColumnDefinition>();
      lisColumnInWindow.AddRange((IEnumerable<ColumnDefinition>) ((Grid) window.Content).ColumnDefinitions);
      foreach (object child in ((Panel) window.Content).Children)
      {
        if (child.GetType() == typeof (Grid))
          lisColumnInWindow.AddRange((IEnumerable<ColumnDefinition>) this.GetListChildColumn((System.Windows.UIElement) child));
      }
      return lisColumnInWindow;
    }

    private List<ColumnDefinition> GetListChildColumn(System.Windows.UIElement element)
    {
      List<ColumnDefinition> listChildColumn = new List<ColumnDefinition>();
      if (!(element.GetType() == typeof (Grid)))
        return new List<ColumnDefinition>();
      Grid grid = (Grid) element;
      listChildColumn.AddRange((IEnumerable<ColumnDefinition>) grid.ColumnDefinitions);
      foreach (object child in grid.Children)
      {
        if (child.GetType() == typeof (Grid))
          listChildColumn.AddRange((IEnumerable<ColumnDefinition>) this.GetListChildColumn((System.Windows.UIElement) child));
      }
      return listChildColumn;
    }
  }
}
