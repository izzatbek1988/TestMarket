// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Files.WindowOptions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Gbs.Core.Config.Files
{
  public class WindowOptions
  {
    public class WindowOption
    {
      public string Key { get; set; }

      public double Width { get; set; }

      public double Height { get; set; }

      public bool IsOpenFullScreen { get; set; }
    }

    public class DataGridView
    {
      public string Name { get; set; }

      public List<WindowOptions.ColumnOption> ColumnOptions { get; set; } = new List<WindowOptions.ColumnOption>();

      public List<WindowOptions.SortOrder> SortOrders { get; set; }
    }

    public class SortOrder
    {
      public string Column { get; set; }

      [JsonConverter(typeof (StringEnumConverter))]
      public ListSortDirection Direction { get; set; }
    }

    public class ColumnOption
    {
      public string Name { get; set; }

      public DataGridLength Width { get; set; }

      public DataGridLengthUnitType Type { get; set; }

      public bool IsVisibility { get; set; }

      public int Position { get; set; }
    }

    public class GridRowDefinition
    {
      public string Name { get; set; }

      public GridLength Height { get; set; }
    }

    public class GridColumnDefinition
    {
      public string Name { get; set; }

      public GridLength Width { get; set; }
    }

    public class FileWindow
    {
      public WindowOptions.WindowOption Option { get; set; } = new WindowOptions.WindowOption();

      public List<WindowOptions.DataGridView> DataGridList { get; set; } = new List<WindowOptions.DataGridView>();

      public List<WindowOptions.GridRowDefinition> RowList { get; set; } = new List<WindowOptions.GridRowDefinition>();

      public List<WindowOptions.GridColumnDefinition> ColumnDefinitionsList { get; set; } = new List<WindowOptions.GridColumnDefinition>();
    }
  }
}
