// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Waybill.WaybillItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Documents;
using Gbs.Resources.Localizations;
using System;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace Gbs.Core.ViewModels.Waybill
{
  public class WaybillItem : DocumentItemViewModel
  {
    private Decimal _buyPrice;
    private object _isNew;
    private Decimal _salePrice;

    public string Identity { get; set; }

    public string FbNumberForEgais { get; set; }

    public object IsNewItem
    {
      get => this._isNew;
      set
      {
        this._isNew = value;
        this.OnPropertyChanged(nameof (IsNewItem));
      }
    }

    public GoodsStocks.GoodStock Stock { get; set; }

    public Decimal SalePrice
    {
      get => this._salePrice;
      set
      {
        this._salePrice = value;
        this.OnPropertyChanged(nameof (SalePrice));
        this.OnPropertyChanged("SaleSum");
        this.OnPropertyChanged("Percent");
        this.OnPropertyChanged("ChangeSalePrice");
        this.OnPropertyChanged("SalePriceBackgroud");
        this.OnPropertyChanged("IncomePriceBackgroud");
      }
    }

    public Decimal? OldSalePrice
    {
      get
      {
        return !this.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? new Decimal?() : new Decimal?(this.Good.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)));
      }
    }

    public string OldSalePriceLine
    {
      get
      {
        return this.OldSalePrice.HasValue ? string.Format(Translate.WaybillItem_OldSalePriceLine_Предыдущая_цена___0_N2_, (object) this.OldSalePrice) : Translate.WaybillItem_OldSalePriceLine_Предыдущая_цена_не_указана;
      }
    }

    public string ChangeSalePrice
    {
      get
      {
        Decimal? oldSalePrice = this.OldSalePrice;
        if (!oldSalePrice.HasValue)
          return "";
        Decimal? nullable = oldSalePrice;
        Decimal salePrice1 = this.SalePrice;
        if (nullable.GetValueOrDefault() < salePrice1 & nullable.HasValue)
          return "▲";
        nullable = oldSalePrice;
        Decimal salePrice2 = this.SalePrice;
        return nullable.GetValueOrDefault() > salePrice2 & nullable.HasValue ? "▼" : "=";
      }
    }

    public SolidColorBrush SalePriceBackgroud
    {
      get
      {
        SolidColorBrush salePriceBackgroud = new SolidColorBrush(Colors.Transparent);
        Decimal? oldSalePrice = this.OldSalePrice;
        if (!oldSalePrice.HasValue || !new ConfigsRepository<Settings>().Get().Interface.IsColorEditSalePrice)
          return salePriceBackgroud;
        Decimal? nullable = oldSalePrice;
        Decimal salePrice1 = this.SalePrice;
        if (nullable.GetValueOrDefault() < salePrice1 & nullable.HasValue)
          return new SolidColorBrush(Colors.Green);
        nullable = oldSalePrice;
        Decimal salePrice2 = this.SalePrice;
        return nullable.GetValueOrDefault() > salePrice2 & nullable.HasValue ? new SolidColorBrush(Colors.Red) : salePriceBackgroud;
      }
    }

    public SolidColorBrush IncomePriceBackgroud
    {
      get
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Transparent);
        return this.BuyPrice > this.SalePrice && new ConfigsRepository<Settings>().Get().Interface.IsColorSalePriceMoreBuy ? new SolidColorBrush(Colors.Red) : solidColorBrush;
      }
    }

    public Decimal BuyPrice
    {
      get => this._buyPrice;
      set
      {
        this._buyPrice = value;
        this.OnPropertyChanged(nameof (BuyPrice));
        this.OnPropertyChanged("BuySum");
        this.OnPropertyChanged("Percent");
        this.OnPropertyChanged("IncomePriceBackgroud");
      }
    }

    public Decimal BuySum => this.BuyPrice * this.Quantity;

    public Decimal Percent
    {
      get
      {
        return this.BuyPrice == 0M ? 0M : Math.Round((this.SalePrice - this.BuyPrice) * 100M / this.BuyPrice, 2, MidpointRounding.AwayFromZero);
      }
    }

    public Decimal SaleSum => this.SalePrice * this.Quantity;

    public bool IsReadOnly { get; set; } = true;

    public WaybillItem(
      Good good,
      Decimal quantity = 0M,
      Decimal buyPrice = 0M,
      Decimal buyDiscount = 0M,
      Decimal salePrice = 0M,
      Guid? uid = null)
    {
      this.Good = good;
      this.BuyPrice = buyPrice;
      this.SalePrice = salePrice;
      this.Quantity = quantity;
      this.SalePrice = salePrice;
      this.Uid = uid ?? Guid.NewGuid();
    }

    public WaybillItem()
    {
    }
  }
}
