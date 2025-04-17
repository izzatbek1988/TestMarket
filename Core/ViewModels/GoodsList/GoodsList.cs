// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.GoodItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;

#nullable disable
namespace Gbs.Helpers
{
  [Obsolete("Необходимо использовать другой класс")]
  public class GoodItem : ViewModelWithForm
  {
    private Decimal _buyPrice;
    private string _comment = string.Empty;
    private Decimal _quantity;
    private GoodItem.DiscountItem _saleDiscount = new GoodItem.DiscountItem();
    private Decimal _salePrice;

    public Gbs.Core.Entities.Goods.Good Good { get; private set; }

    public GoodsStocks.GoodStock GoodStock { get; set; } = new GoodsStocks.GoodStock();

    public string Comment
    {
      get => this._comment;
      set
      {
        this._comment = value;
        this.OnPropertyChanged(nameof (Comment));
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
      }
    }

    public Decimal SalePrice
    {
      get => this._salePrice;
      set
      {
        this._salePrice = value;
        this.OnPropertyChanged(nameof (SalePrice));
        this.OnPropertyChanged("SaleSum");
      }
    }

    public Decimal Quantity
    {
      get => this._quantity;
      set
      {
        this._quantity = value;
        this.OnPropertyChanged(nameof (Quantity));
        this.OnPropertyChanged("SaleSum");
      }
    }

    public Decimal SaleSum
    {
      get
      {
        return (this._salePrice - this._salePrice * (this._saleDiscount.Discount / 100M)) * this._quantity;
      }
    }

    public Decimal BuySum
    {
      get
      {
        return (this._buyPrice - this._buyPrice * (this._saleDiscount.Discount / 100M)) * this._quantity;
      }
    }

    private GoodItem()
    {
    }

    public static GoodItem ItemForSale(
      Gbs.Core.Entities.Goods.Good good,
      GoodsStocks.GoodStock stock,
      Decimal quantity,
      Decimal salePrice,
      Decimal saleDiscount,
      string comment = "")
    {
      if (good == null)
        return (GoodItem) null;
      GoodItem goodItem = new GoodItem();
      goodItem.Good = good;
      goodItem.GoodStock = stock;
      goodItem._salePrice = salePrice;
      goodItem.Comment = comment;
      goodItem._quantity = quantity;
      goodItem._saleDiscount = new GoodItem.DiscountItem()
      {
        Discount = saleDiscount
      };
      goodItem.OnPropertyChanged("SaleSum");
      return goodItem;
    }

    public static GoodItem ItemForBuy(
      Gbs.Core.Entities.Goods.Good good,
      Decimal quantity = 0M,
      Decimal buyPrice = 0M,
      Decimal buyDiscount = 0M,
      Decimal salePrice = 0M,
      Guid? uid = null)
    {
      GoodItem goodItem = new GoodItem();
      goodItem.Good = good;
      goodItem._buyPrice = buyPrice;
      goodItem._quantity = quantity;
      goodItem._saleDiscount = new GoodItem.DiscountItem()
      {
        Discount = buyDiscount
      };
      goodItem._salePrice = salePrice;
      goodItem.OnPropertyChanged("SaleSum");
      goodItem.OnPropertyChanged("BuySum");
      return goodItem;
    }

    private class DiscountItem : ViewModelWithForm
    {
      private Decimal _discount;

      public Decimal Discount
      {
        get => this._discount;
        set
        {
          this._discount = value;
          this.OnPropertyChanged(nameof (Discount));
        }
      }
    }
  }
}
