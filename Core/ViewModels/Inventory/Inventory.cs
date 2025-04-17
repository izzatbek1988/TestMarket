// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Inventory.Inventory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.Inventory;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

#nullable disable
namespace Gbs.Core.ViewModels.Inventory
{
  public class Inventory : DocumentViewModel<InventoryItem>
  {
    public string TsdDocumentUid { get; set; } = "";

    public string NumberDocument { get; set; } = string.Empty;

    public void EditQuantity(object obj, object color = null)
    {
      List<InventoryItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return;
      (bool result, Decimal? quantity, Decimal? _) = new EditGoodQuantityViewModel().ShowQuantityWithSalePriceEdit(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<InventoryItem>) castedList.ToList<InventoryItem>(), true));
      if (!result)
        return;
      foreach (InventoryItem inventoryItem in castedList)
        inventoryItem.Quantity = quantity ?? inventoryItem.Quantity;
      this.ReCalcTotals();
    }

    public Guid OldDocumentUid { get; set; } = Guid.Empty;

    public string Comment { get; set; } = string.Empty;

    public bool IsFinished { get; set; }

    public Decimal TotalBaseQuantity
    {
      get => this.Items.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => x.BaseQuantity));
    }

    public override ActionResult Save()
    {
      this.Document = this.CreateInventoryDocument();
      this.Document.Number = this.NumberDocument.IsNullOrEmpty() ? Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.Inventory) : this.NumberDocument;
      this.Document.Comment = this.Comment;
      this.Document.UserUid = this.User.Uid;
      this.Document.Status = this.IsFinished ? GlobalDictionaries.DocumentsStatuses.Close : GlobalDictionaries.DocumentsStatuses.Draft;
      using (DataBase dataBase = Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        ActionResult actionResult = documentsRepository.Validate(this.Document);
        if (actionResult.Result != ActionResult.Results.Ok)
          return actionResult;
        if (!documentsRepository.Save(this.Document))
          return new ActionResult(ActionResult.Results.Error, Translate.Inventory_Не_удалось_сохранить_документ_инвентаризации);
        if (this.OldDocumentUid != Guid.Empty)
          this.DeleteOldDocuments();
        return new ActionResult(ActionResult.Results.Ok);
      }
    }

    private void DeleteOldDocuments()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        documentsRepository.Delete(documentsRepository.GetByUid(this.OldDocumentUid));
      }
    }

    private Document CreateInventoryDocument()
    {
      Document document1 = new Document();
      document1.Type = GlobalDictionaries.DocumentsTypes.Inventory;
      document1.Storage = this.Storage;
      Document document2 = this.Document;
      // ISSUE: explicit non-virtual call
      document1.Uid = document2 != null ? __nonvirtual (document2.Uid) : Guid.NewGuid();
      Document inventoryDocument1 = document1;
      foreach (Gbs.Core.Entities.Documents.Item obj in this.Items.Select<InventoryItem, Gbs.Core.Entities.Documents.Item>((Func<InventoryItem, Gbs.Core.Entities.Documents.Item>) (item =>
      {
        Gbs.Core.Entities.Documents.Item inventoryDocument2 = new Gbs.Core.Entities.Documents.Item();
        inventoryDocument2.Quantity = item.Quantity;
        inventoryDocument2.Good = item.Good;
        GoodsModifications.GoodModification goodModification = item.GoodModification;
        // ISSUE: explicit non-virtual call
        inventoryDocument2.ModificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
        inventoryDocument2.SellPrice = item.SalePrice;
        return inventoryDocument2;
      })))
        inventoryDocument1.Items.Add(obj);
      if (!this.TsdDocumentUid.IsNullOrEmpty())
      {
        Document document3 = inventoryDocument1;
        List<EntityProperties.PropertyValue> propertyValueList = new List<EntityProperties.PropertyValue>();
        EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
        propertyValue.EntityUid = inventoryDocument1.Uid;
        EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
        propertyType.EntityType = GlobalDictionaries.EntityTypes.Document;
        propertyType.Type = GlobalDictionaries.EntityPropertyTypes.Text;
        propertyType.Uid = GlobalDictionaries.TsdDocumentNumberUid;
        propertyValue.Type = propertyType;
        propertyValue.Value = (object) this.TsdDocumentUid;
        propertyValueList.Add(propertyValue);
        document3.Properties = propertyValueList;
      }
      return inventoryDocument1;
    }

    public override void ReCalcTotals()
    {
      this.OnPropertyChanged("TotalQuantity");
      this.OnPropertyChanged("TotalBaseQuantity");
    }

    public List<InventoryItem> HasChangedStocks()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> allItems = new GoodRepository(dataBase).GetAllItems();
        List<InventoryItem> inventoryItemList = new List<InventoryItem>();
        foreach (InventoryItem inventoryItem in (Collection<InventoryItem>) this.Items)
        {
          InventoryItem item = inventoryItem;
          Gbs.Core.Entities.Goods.Good good = allItems.Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == item.Good.Uid));
          GoodsModifications.GoodModification goodModification = item.GoodModification;
          // ISSUE: explicit non-virtual call
          Guid modUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
          if (!(good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Price == item.SalePrice && x.Storage.Uid == this.Storage.Uid && !x.IsDeleted && x.ModificationUid == modUid)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)) == item.BaseQuantity))
          {
            InventoryCardViewModel_v2.SetItemDbQtyColor(item, Colors.DarkOrange);
            inventoryItemList.Add(item);
          }
        }
        return inventoryItemList;
      }
    }
  }
}
