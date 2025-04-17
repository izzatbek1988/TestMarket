// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Inventory.InventoryItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Documents;
using System;
using System.Windows.Media;

#nullable disable
namespace Gbs.Core.ViewModels.Inventory
{
  public class InventoryItem : DocumentItemViewModel
  {
    private string _info = string.Empty;
    private SolidColorBrush _color;

    public Decimal BaseQuantity { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal QuantityChange => this.Quantity - this.BaseQuantity;

    public DateTime UpdateTime { get; set; }

    public SolidColorBrush Color
    {
      get => this._color;
      set
      {
        this._color = value;
        this.OnPropertyChanged(nameof (Color));
      }
    }
  }
}
