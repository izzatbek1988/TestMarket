// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Move
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers;
using System.Linq;

#nullable disable
namespace Gbs.Core.ViewModels
{
  public class Move : Gbs.Core.ViewModels.Basket.Basket
  {
    public override void ReCalcTotals()
    {
      Other.ConsoleWrite("Пересчет итогов в корзине");
      this.RoundQuantity();
      this.RemoveZeroQuantityItems();
      this.OnPropertyChanged("ChangeSum");
      this.OnPropertyChanged("TotalQuantity");
      this.OnPropertyChanged("TotalDiscount");
      this.OnPropertyChanged("TotalSum");
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged("VisibilityInfoCredit");
      if (this.Items.Any<BasketItem>())
        return;
      this.CheckOrder();
    }
  }
}
