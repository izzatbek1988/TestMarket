// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Basket.BasketItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Models.GoodInList;
using Gbs.Core.ViewModels.Documents;
using Gbs.Core.ViewModels.Documents.Sales;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Core.ViewModels.Basket
{
  public class BasketItem : DocumentItemViewModel, IEntity
  {
    private string _comment;
    private Decimal _totalSumStock;
    private SolidColorBrush _commentColor;
    private string _errorStr;
    private SolidColorBrush _priceColor;
    private string _errorStrForPrice;
    private SolidColorBrush _displayedNameColor;
    private string _errorStrForDisplayedName;

    public InfoToTapBeer InfoToTapBeer { get; set; }

    public Decimal TobaccoSalePrice { get; set; }

    public string ErrorStr
    {
      get => this._errorStr;
      set
      {
        this._errorStr = value;
        this.OnPropertyChanged(nameof (ErrorStr));
        this.OnPropertyChanged("Cursor");
      }
    }

    public Cursor Cursor => this.ErrorStr != null ? Cursors.Hand : Cursors.Arrow;

    public Cursor CursorWithPrice => this.ErrorStrForPrice != null ? Cursors.Hand : Cursors.Arrow;

    public Cursor CursorWithDisplayedName
    {
      get => this.ErrorStrForDisplayedName != null ? Cursors.Hand : Cursors.Arrow;
    }

    public Guid GoodStockUid { get; set; }

    public SaleDiscount Discount { get; set; } = new SaleDiscount();

    public bool IsPriceEditByUser { get; set; }

    public Decimal SalePrice { get; set; }

    public Decimal BasePrice { get; set; }

    public Decimal BuyPrice { get; set; }

    public string Comment
    {
      get => this._comment;
      set
      {
        this._comment = value;
        this.OnPropertyChanged(nameof (Comment));
      }
    }

    public SolidColorBrush CommentColor
    {
      set
      {
        this._commentColor = value;
        this.OnPropertyChanged(nameof (CommentColor));
      }
      get
      {
        if (this._commentColor == null)
        {
          SolidColorBrush defBrush = new SolidColorBrush(Colors.Transparent);
          defBrush.Freeze();
          Dispatcher.CurrentDispatcher.Invoke((Action) (() => this._commentColor = defBrush));
        }
        return this._commentColor;
      }
    }

    public SolidColorBrush PriceColor
    {
      set
      {
        this._priceColor = value;
        this.OnPropertyChanged(nameof (PriceColor));
      }
      get
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Transparent);
        if (this._priceColor == null)
          this._priceColor = solidColorBrush;
        return this._priceColor;
      }
    }

    public string ErrorStrForPrice
    {
      get => this._errorStrForPrice;
      set
      {
        this._errorStrForPrice = value;
        this.OnPropertyChanged(nameof (ErrorStrForPrice));
        this.OnPropertyChanged("CursorWithPrice");
      }
    }

    public SolidColorBrush DisplayedNameColor
    {
      set
      {
        this._displayedNameColor = value;
        this.OnPropertyChanged(nameof (DisplayedNameColor));
      }
      get
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Transparent);
        if (this._displayedNameColor == null)
          this._displayedNameColor = solidColorBrush;
        return this._displayedNameColor;
      }
    }

    public string ErrorStrForDisplayedName
    {
      get => this._errorStrForDisplayedName;
      set
      {
        this._errorStrForDisplayedName = value;
        this.OnPropertyChanged(nameof (ErrorStrForDisplayedName));
        this.OnPropertyChanged("CursorWithDisplayedName");
      }
    }

    public override string DisplayedName
    {
      get
      {
        GoodsListItemsCertificate certificate = this.Certificate;
        if ((certificate != null ? (certificate.IsCertificate ? 1 : 0) : 0) == 0)
          return base.DisplayedName;
        return this.Good.Name + " [" + Translate.BasketItem_номинал__ + this.Certificate.Nominal.ToString() + "]";
      }
    }

    public Decimal TotalSumStock
    {
      get => this._totalSumStock;
      set
      {
        this._totalSumStock = value;
        this.OnPropertyChanged(nameof (TotalSumStock));
      }
    }

    public GoodsListItemsCertificate Certificate { get; set; } = new GoodsListItemsCertificate();

    public Decimal DiscountSum
    {
      get
      {
        return ItemsTotalSumCalculator.DiscountForPosition(this.SalePrice, this.Quantity, this.Discount.Value);
      }
    }

    public Decimal TotalSum
    {
      get
      {
        return ItemsTotalSumCalculator.SumForGoodPosition(new ItemsTotalSumCalculator.GoodItem(this.Quantity, this.SalePrice, this.Discount.Value));
      }
    }

    public Storages.Storage Storage { get; set; }

    public BasketItem()
    {
    }

    public BasketItem(
      Gbs.Core.Entities.Goods.Good good,
      Guid modificationUid,
      Decimal price,
      Storages.Storage storage,
      Decimal q = 1M,
      Decimal discount = 0M,
      Guid guid = default (Guid),
      string comment = "",
      Guid goodStockUid = default (Guid))
    {
      BasketItem basketItem = this;
      this.Good = good;
      this.TotalSumStock = good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production) ? good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)) : GoodsSearchModelView.GetStockSet(good, (List<Gbs.Core.Entities.Goods.Good>) null);
      GoodsModifications.GoodModification goodModification1 = good.Modifications.SingleOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == modificationUid));
      if (modificationUid != Guid.Empty && goodModification1 != null)
      {
        this.GoodModification = good.Modifications.Single<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == modificationUid));
      }
      else
      {
        GoodsModifications.GoodModification goodModification2 = new GoodsModifications.GoodModification();
        goodModification2.Uid = Guid.Empty;
        this.GoodModification = goodModification2;
      }
      this.Quantity = q;
      this.SalePrice = price;
      this.BasePrice = price;
      this.Discount = new SaleDiscount();
      this.Discount.SetDiscount(discount, SaleDiscount.ReasonEnum.None, this, (Gbs.Core.ViewModels.Basket.Basket) null);
      this.Storage = storage;
      this.Uid = guid == new Guid() ? Guid.NewGuid() : guid;
      this.Comment = comment;
      this.GoodStockUid = goodStockUid;
      this.Discount.PropertyChanged += (PropertyChangedEventHandler) ((_, __) => basketItem.OnPropertyChanged(nameof (TotalSum)));
    }

    public BasketItem Clone()
    {
      JsonSerializerSettings settings = new JsonSerializerSettings()
      {
        ObjectCreationHandling = ObjectCreationHandling.Replace
      };
      BasketItem basketItem = JsonConvert.DeserializeObject<BasketItem>(JsonConvert.SerializeObject((object) this), settings);
      basketItem.Discount.SetDiscount(this.Discount.Value, basketItem.Discount.Reason, basketItem, (Gbs.Core.ViewModels.Basket.Basket) null);
      return basketItem;
    }

    public bool IsDeleted { get; set; }

    public override string ToString() => base.ToString();
  }
}
