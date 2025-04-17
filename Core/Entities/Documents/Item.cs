// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.Item
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Waybill;
using Gbs.Core.ViewModels.WriteOff;
using Gbs.Helpers.Cache;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public class Item : Gbs.Core.Entities.Entity
  {
    private Decimal? _baseSalePrice;
    private Good _good;
    private Guid _goodUid = Guid.Empty;

    [Required]
    public Guid DocumentUid { get; set; }

    public Guid GoodUid
    {
      get => this._goodUid;
      set => this._goodUid = value;
    }

    [Required]
    public Good Good
    {
      get
      {
        if (this._good != null)
          return this._good;
        Good good = CachesBox.AllGoods().SingleOrDefault<Good>((Func<Good, bool>) (x => x.Uid == this._goodUid));
        if (good != null)
        {
          this._good = good;
          return this._good;
        }
        this._good = new GoodRepository().GetByUid(this._goodUid);
        return this._good;
      }
      set
      {
        this._good = value;
        this._goodUid = this._good.Uid;
      }
    }

    public Guid ModificationUid { get; set; } = Guid.Empty;

    public GoodsStocks.GoodStock GoodStock { get; set; }

    [Range(-99999999.9, 99999999.9)]
    public Decimal Quantity { get; set; }

    [Required]
    [Range(0.0, 100.0)]
    public Decimal Discount { get; set; }

    public string Comment { get; set; } = string.Empty;

    [Required]
    public Decimal BuyPrice { get; set; }

    [Required]
    public Decimal SellPrice { get; set; }

    public string FbNumberForEgais { get; set; }

    public Decimal BaseSalePrice
    {
      get => this._baseSalePrice ?? this.SellPrice;
      set => this._baseSalePrice = new Decimal?(value);
    }

    public Item()
    {
    }

    public Item(BasketItem item, Guid docUid)
    {
      this.Uid = item.Uid;
      this.Quantity = item.Quantity;
      this.Discount = item.Discount.Value;
      this.DocumentUid = docUid;
      this.SellPrice = item.SalePrice;
      this.Comment = item.Comment;
      this.Good = item.Good;
      this.ModificationUid = item.GoodModification.Uid;
      this.Uid = item.Uid;
    }

    public Item(Gbs.Helpers.HomeOffice.Entity.Item item, Guid storageUid)
    {
      this.Uid = item.Uid;
      this.IsDeleted = item.IsDeleted;
      this.Quantity = item.Quantity;
      this.Discount = item.Discount;
      this.DocumentUid = item.DocumentUid;
      this.SellPrice = item.SellPrice;
      this.Comment = item.Comment;
      Good good = new Good();
      good.Uid = item.GoodUid;
      this.Good = good;
      this.ModificationUid = item.ModificationUid;
      this.BuyPrice = item.BuyPrice;
      GoodsStocks.GoodStock goodStock1;
      if (!(item.GoodStockUid == Guid.Empty))
      {
        GoodsStocks.GoodStock goodStock2 = new GoodsStocks.GoodStock();
        goodStock2.Uid = item.GoodStockUid;
        Storages.Storage storage = new Storages.Storage();
        storage.Uid = storageUid;
        goodStock2.Storage = storage;
        goodStock2.GoodUid = item.GoodUid;
        goodStock1 = goodStock2;
      }
      else
        goodStock1 = (GoodsStocks.GoodStock) null;
      this.GoodStock = goodStock1;
      this.FbNumberForEgais = item.FbNumberForEgais;
    }

    public Item(Item item)
    {
      this.Uid = item.Uid;
      this.IsDeleted = item.IsDeleted;
      this.Quantity = item.Quantity;
      this.Discount = item.Discount;
      this.DocumentUid = item.DocumentUid;
      this.SellPrice = item.SellPrice;
      this.Comment = item.Comment;
      Good good = new Good();
      good.Uid = item.GoodUid;
      GoodGroups.Group group = new GoodGroups.Group();
      group.Uid = item.Good.Group.Uid;
      good.Group = group;
      good.Name = item.Good.Name;
      good.SetStatus = item.Good.SetStatus;
      this.Good = good;
      this.ModificationUid = item.ModificationUid;
      this.BuyPrice = item.BuyPrice;
      GoodsStocks.GoodStock goodStock1 = item.GoodStock;
      GoodsStocks.GoodStock goodStock2;
      // ISSUE: explicit non-virtual call
      if (!((goodStock1 != null ? __nonvirtual (goodStock1.Uid) : Guid.Empty) == Guid.Empty))
      {
        GoodsStocks.GoodStock goodStock3 = new GoodsStocks.GoodStock();
        goodStock3.Uid = item.GoodStock.Uid;
        Storages.Storage storage1 = new Storages.Storage();
        Storages.Storage storage2 = item.GoodStock.Storage;
        // ISSUE: explicit non-virtual call
        storage1.Uid = storage2 != null ? __nonvirtual (storage2.Uid) : Guid.Empty;
        goodStock3.Storage = storage1;
        goodStock3.GoodUid = item.GoodUid;
        goodStock3.Price = item.GoodStock.Price;
        goodStock3.ModificationUid = item.GoodStock.ModificationUid;
        goodStock3.Stock = item.GoodStock.Stock;
        goodStock2 = goodStock3;
      }
      else
        goodStock2 = (GoodsStocks.GoodStock) null;
      this.GoodStock = goodStock2;
      this.FbNumberForEgais = item.FbNumberForEgais;
    }

    public Item(WaybillItem item, Guid docUid)
    {
      this.Uid = item.Uid;
      this.Quantity = item.Quantity;
      this.DocumentUid = docUid;
      this.BuyPrice = item.BuyPrice;
      this.SellPrice = item.SalePrice;
      this.Good = item.Good;
      this.GoodStock = item.Stock;
      GoodsModifications.GoodModification goodModification = item.GoodModification;
      // ISSUE: explicit non-virtual call
      this.ModificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
    }

    public Item(WriteOffItem item, Guid docUid)
    {
      this.Uid = item.Uid;
      this.Quantity = item.Quantity;
      this.DocumentUid = docUid;
      this.BuyPrice = item.BuyPrice;
      this.SellPrice = item.SalePrice;
      this.Good = item.Good;
      GoodsModifications.GoodModification goodModification = item.GoodModification;
      // ISSUE: explicit non-virtual call
      this.ModificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
    }

    public Item(DOCUMENT_ITEMS dbDocItem, GOODS_STOCK stockItem, STORAGES storageItem)
    {
      this.Uid = dbDocItem.UID;
      this.IsDeleted = dbDocItem.IS_DELETED;
      this.Quantity = dbDocItem.QUANTITY;
      this.Comment = dbDocItem.COMMENT;
      this.Discount = dbDocItem.DISCOUNT;
      this.DocumentUid = dbDocItem.DOCUMENT_UID;
      this.BuyPrice = dbDocItem.BUY_PRICE;
      this.SellPrice = dbDocItem.SALE_PRICE;
      this.GoodUid = dbDocItem.GOOD_UID;
      this.ModificationUid = dbDocItem.MODIFICATION_UID;
      this.GoodStock = stockItem == null ? (GoodsStocks.GoodStock) null : new GoodsStocks.GoodStock(stockItem, storageItem);
    }
  }
}
