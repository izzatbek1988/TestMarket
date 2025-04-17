// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Inventory.InventoryFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Core.ViewModels.Inventory
{
  public class InventoryFactory
  {
    private readonly Gbs.Core.ViewModels.Inventory.Inventory _inventory;

    public InventoryFactory(Gbs.Core.ViewModels.Inventory.Inventory inventory)
    {
      this._inventory = inventory;
    }

    public void LoadInventoryFromGoodsList(
      List<GoodGroups.Group> goodsGroups,
      bool loadZeroStockGoods)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<Gbs.Core.Entities.Goods.Good> source1 = new GoodRepository(dataBase).GetActiveItems().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g =>
        {
          if (g.IsDeleted || g.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate || g.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            return false;
          return g.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production);
        }));
        if (goodsGroups.Any<GoodGroups.Group>())
          source1 = source1.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => goodsGroups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (gr => gr.Uid == g.Group.Uid))));
        List<InventoryItem> list = new List<InventoryItem>();
        foreach (Gbs.Core.Entities.Goods.Good good1 in source1)
        {
          Gbs.Core.Entities.Goods.Good good = good1;
          if (good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range)
          {
            foreach (GoodsModifications.GoodModification goodModification in good.Modifications.Where<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => !m.IsDeleted)))
            {
              GoodsModifications.GoodModification modification = goodModification;
              IEnumerable<IGrouping<Decimal, GoodsStocks.GoodStock>> source2 = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.ModificationUid == modification.Uid && s.Storage.Uid == this._inventory.Storage.Uid)).GroupBy<GoodsStocks.GoodStock, Decimal>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price));
              list.AddRange(source2.Select<IGrouping<Decimal, GoodsStocks.GoodStock>, InventoryItem>((Func<IGrouping<Decimal, GoodsStocks.GoodStock>, InventoryItem>) (stock => this.GetItemFromGood(good, stock.Key, modification))));
            }
          }
          else
          {
            IEnumerable<IGrouping<Decimal, GoodsStocks.GoodStock>> source3 = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == this._inventory.Storage.Uid)).GroupBy<GoodsStocks.GoodStock, Decimal>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price));
            list.AddRange(source3.Select<IGrouping<Decimal, GoodsStocks.GoodStock>, InventoryItem>((Func<IGrouping<Decimal, GoodsStocks.GoodStock>, InventoryItem>) (stock => this.GetItemFromGood(good, stock.Key, (GoodsModifications.GoodModification) null))));
          }
        }
        if (!loadZeroStockGoods)
          list.RemoveAll((Predicate<InventoryItem>) (x => x.BaseQuantity == 0M));
        this._inventory.Items = new ObservableCollection<InventoryItem>(list);
      }
    }

    private InventoryItem GetItemFromGood(
      Gbs.Core.Entities.Goods.Good good,
      Decimal price,
      GoodsModifications.GoodModification modification)
    {
      // ISSUE: explicit non-virtual call
      Guid modificationUid = modification != null ? __nonvirtual (modification.Uid) : Guid.Empty;
      Decimal num = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == this._inventory.Storage.Uid && !s.IsDeleted && s.Price == price && s.ModificationUid == modificationUid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
      InventoryItem inventoryItem = new InventoryItem();
      inventoryItem.Good = good;
      inventoryItem.Quantity = 0M;
      inventoryItem.GoodModification = modification;
      inventoryItem.BaseQuantity = num;
      inventoryItem.SalePrice = price;
      InventoryItem itemFromGood = inventoryItem;
      if (modification != null && !modification.Barcode.IsNullOrEmpty())
      {
        itemFromGood.Good = itemFromGood.Good.Clone<Gbs.Core.Entities.Goods.Good>();
        Gbs.Core.Entities.Goods.Good good1 = itemFromGood.Good;
        if (good1.Barcodes == null)
        {
          IEnumerable<string> strings;
          good1.Barcodes = strings = (IEnumerable<string>) new List<string>();
        }
        itemFromGood.Good.Barcodes = (IEnumerable<string>) new List<string>(itemFromGood.Good.Barcodes.Append<string>(modification.Barcode));
      }
      return itemFromGood;
    }

    public void LoadInventoryFromDocument(Document document)
    {
      this._inventory.OldDocumentUid = document.Status == GlobalDictionaries.DocumentsStatuses.Draft ? document.Uid : throw new NotSupportedException();
      this._inventory.Storage = document.Storage;
      foreach (Gbs.Core.Entities.Documents.Item obj in document.Items)
      {
        Gbs.Core.Entities.Documents.Item item = obj;
        GoodsModifications.GoodModification goodModification;
        if (!(item.ModificationUid == Guid.Empty))
        {
          goodModification = item.Good.Modifications.Single<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == item.ModificationUid));
        }
        else
        {
          goodModification = new GoodsModifications.GoodModification();
          goodModification.Uid = Guid.Empty;
        }
        GoodsModifications.GoodModification modification = goodModification;
        InventoryItem itemFromGood = this.GetItemFromGood(item.Good, item.SellPrice, modification);
        itemFromGood.Quantity = item.Quantity;
        this._inventory.Items.Add(itemFromGood);
      }
      List<InventoryItem> source = this._inventory.HasChangedStocks();
      if (!source.Any<InventoryItem>())
        return;
      int num1 = (int) MessageBoxHelper.Show(string.Format(Translate.InventoryFactory_Для__0__товаров_количество_изменилось_за_время__пока_инвентаризация_находилась_в_статусе__Черновик_, (object) source.Count), string.Empty, icon: MessageBoxImage.Exclamation);
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> allItems = new GoodRepository(dataBase).GetAllItems();
        foreach (InventoryItem inventoryItem in (Collection<InventoryItem>) this._inventory.Items)
        {
          InventoryItem item = inventoryItem;
          Gbs.Core.Entities.Goods.Good good = allItems.Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == item.Good.Uid));
          GoodsModifications.GoodModification goodModification = item.GoodModification;
          // ISSUE: explicit non-virtual call
          Guid modificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
          Decimal num2 = good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Storage.Uid == this._inventory.Storage.Uid && !s.IsDeleted && s.Price == item.SalePrice && s.ModificationUid == modificationUid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
          item.BaseQuantity = num2;
        }
      }
    }
  }
}
