// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.HotKeys
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.Config
{
  public class HotKeys
  {
    public HotKeysHelper.Hotkey FiscalLastSaleAction { get; set; } = new HotKeysHelper.Hotkey(Key.F11, ModifierKeys.Control);

    public HotKeysHelper.Hotkey OkAction { get; set; } = new HotKeysHelper.Hotkey(Key.Return, ModifierKeys.Control);

    public HotKeysHelper.Hotkey CancelAction { get; set; } = new HotKeysHelper.Hotkey(Key.X, ModifierKeys.Control);

    public HotKeysHelper.Hotkey DeleteItem { get; set; } = new HotKeysHelper.Hotkey(Key.Delete, ModifierKeys.Control);

    public HotKeysHelper.Hotkey AddItem { get; set; } = new HotKeysHelper.Hotkey(Key.Insert, ModifierKeys.Control);

    public HotKeysHelper.Hotkey EditItem { get; set; } = new HotKeysHelper.Hotkey(Key.E, ModifierKeys.Control);

    public HotKeysHelper.Hotkey Print { get; set; } = new HotKeysHelper.Hotkey(Key.P, ModifierKeys.Control);

    public HotKeysHelper.Hotkey DiscountForItem { get; set; } = new HotKeysHelper.Hotkey(Key.F5);

    public HotKeysHelper.Hotkey DiscountForCheck { get; set; } = new HotKeysHelper.Hotkey(Key.F5, ModifierKeys.Control);

    public HotKeysHelper.Hotkey InsertPayments { get; set; } = new HotKeysHelper.Hotkey(Key.Insert);

    public HotKeysHelper.Hotkey NextBasket { get; set; } = new HotKeysHelper.Hotkey(Key.Next);

    public HotKeysHelper.Hotkey PrevBasket { get; set; } = new HotKeysHelper.Hotkey(Key.Prior);

    public HotKeysHelper.Hotkey SelectClient { get; set; } = new HotKeysHelper.Hotkey(Key.U, ModifierKeys.Control);

    public HotKeysHelper.Hotkey FavoritesGoods { get; set; } = new HotKeysHelper.Hotkey(Key.F11);

    public HotKeysHelper.Hotkey ShowCafeForm { get; set; } = new HotKeysHelper.Hotkey(Key.F2);

    public HotKeysHelper.Hotkey KkmGetXReport { get; set; } = new HotKeysHelper.Hotkey(Key.X, ModifierKeys.Control | ModifierKeys.Shift);

    public HotKeysHelper.Hotkey KkmGetZReport { get; set; } = new HotKeysHelper.Hotkey(Key.Z, ModifierKeys.Control | ModifierKeys.Shift);

    public HotKeysHelper.Hotkey CashOut { get; set; } = new HotKeysHelper.Hotkey(Key.O, ModifierKeys.Control);

    public HotKeysHelper.Hotkey CashIn { get; set; } = new HotKeysHelper.Hotkey(Key.I, ModifierKeys.Control);

    public HotKeysHelper.Hotkey SearchByMarkCode { get; set; } = new HotKeysHelper.Hotkey(Key.F7);
  }
}
