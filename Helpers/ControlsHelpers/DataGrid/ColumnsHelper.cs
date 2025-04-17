// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.DataGrid.ColumnsHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.XAML.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.DataGrid
{
  public static class ColumnsHelper
  {
    public static void CreateContextMenu(this System.Windows.Controls.DataGrid grid, Action actionOnMenuClose = null)
    {
      ContextMenu cm = new ContextMenu();
      grid.ColumnHeaderStyle = new Style()
      {
        BasedOn = (Style) grid.FindResource((object) typeof (DataGridColumnHeader)),
        TargetType = typeof (DataGridColumnHeader)
      };
      grid.ColumnHeaderStyle.Setters.Add((SetterBase) new Setter(FrameworkElement.ContextMenuProperty, (object) cm));
      cm.Opened += (RoutedEventHandler) ((sender, args) =>
      {
        cm.Items.Clear();
        foreach (DataGridColumn element in (IEnumerable<DataGridColumn>) grid.Columns.Where<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) != string.Empty)).OrderBy<DataGridColumn, int>((Func<DataGridColumn, int>) (x => x.DisplayIndex)))
        {
          ItemCollection items = cm.Items;
          items.Add((object) new MenuItem()
          {
            Header = element.Header,
            Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) element),
            IsCheckable = true,
            IsChecked = (element.Visibility == Visibility.Visible)
          });
        }
      });
      cm.Closed += (RoutedEventHandler) ((sender, args) =>
      {
        Gbs.Helpers.Other.IsVisibilityDataGridColumn(grid, (ContextMenu) sender);
        Action action = actionOnMenuClose;
        if (action == null)
          return;
        action();
      });
    }

    public static void AddGoodsPropertiesColumns(this System.Windows.Controls.DataGrid grid)
    {
      Guid[] ignorableTypes = new Guid[6]
      {
        GlobalDictionaries.CertificateNominalUid,
        GlobalDictionaries.CertificateReusableUid,
        GlobalDictionaries.AlcCodeUid,
        GlobalDictionaries.AlcVolumeUid,
        GlobalDictionaries.CapacityUid,
        GlobalDictionaries.ProductCodeUid
      };
      foreach (EntityProperties.PropertyType type in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted && !x.Uid.IsEither<Guid>(ignorableTypes))))
      {
        DataGridTextColumn dataGridTextColumn1 = new DataGridTextColumn();
        dataGridTextColumn1.Header = (object) type.Name;
        dataGridTextColumn1.Width = new DataGridLength(100.0);
        DataGridTextColumn dataGridTextColumn2 = dataGridTextColumn1;
        ColumnsHelper.SetBindings(type, dataGridTextColumn2);
        ColumnsHelper.SetStyles(type, (DataGridColumn) dataGridTextColumn2);
        Gbs.Helpers.Extensions.UIElement.Extensions.SetGuid((DependencyObject) dataGridTextColumn2, type.Uid.ToString());
        grid.Columns.Add((DataGridColumn) dataGridTextColumn2);
      }
    }

    private static void SetBindings(EntityProperties.PropertyType type, DataGridTextColumn column)
    {
      Binding binding1 = new Binding(string.Format("Good.PropertiesDictionary[{0}]", (object) type.Uid));
      binding1.StringFormat = EntityProperties.GetStringFormat(type);
      Binding binding2 = binding1;
      if (type.Type.IsEither<GlobalDictionaries.EntityPropertyTypes>(GlobalDictionaries.EntityPropertyTypes.Decimal))
        binding2.Converter = (IValueConverter) new DecimalToStringConverter();
      column.Binding = (BindingBase) binding2;
    }

    private static void SetStyles(EntityProperties.PropertyType type, DataGridColumn column)
    {
      string str = string.Empty;
      switch (type.Type)
      {
        case GlobalDictionaries.EntityPropertyTypes.Integer:
        case GlobalDictionaries.EntityPropertyTypes.AutoNum:
          str = "IntegerCellStyle";
          break;
        case GlobalDictionaries.EntityPropertyTypes.Decimal:
          str = "numberCellStyle";
          break;
      }
      if (str.IsNullOrEmpty())
        return;
      column.CellStyle = Application.Current?.TryFindResource((object) str) as Style;
    }
  }
}
