// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Sale.Sale
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents;
using Gbs.Forms;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Core.ViewModels.Sale
{
  public class Sale : DocumentViewModel<BasketItem>
  {
    private AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid> _saleItemsList = new AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid>();

    public AsyncObservableCollection<SaleJournalViewModel.SaleItemsInfoGrid> SaleItemsList
    {
      get => this._saleItemsList;
      set
      {
        this._saleItemsList = value;
        this.OnPropertyChanged(nameof (SaleItemsList));
      }
    }

    public Decimal TotalSaleDiscountSum
    {
      get => this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.DiscountSum));
    }

    public Decimal TotalSaleSum
    {
      get => this.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.TotalSum));
    }

    public override ActionResult Save() => throw new NotImplementedException();
  }
}
