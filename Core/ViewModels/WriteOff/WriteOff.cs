// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.WriteOff.WriteOff
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.ViewModels.WriteOff
{
  public class WriteOff : DocumentViewModel<WriteOffItem>
  {
    public override ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditQuantity(obj)));
    }

    public Decimal TotalSaleSum
    {
      get
      {
        return this.Items.Sum<WriteOffItem>((Func<WriteOffItem, Decimal>) (x => x.SalePrice * x.Quantity));
      }
    }

    public override ActionResult Save()
    {
      if (!this.Items.Any<WriteOffItem>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.WriteOff_В_списке_нет_ни_одного_товара_для_списания);
        return new ActionResult(ActionResult.Results.Error);
      }
      if (this.Document.Comment.IsNullOrEmpty())
      {
        int num = (int) MessageBoxHelper.Show(Translate.WriteOff_Требуется_ввести_причину_списания_товаров);
        return new ActionResult(ActionResult.Results.Error);
      }
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.WriteOff_Сохранение_документа_списания);
      this.UpdateDocument();
      this.Document.Type = GlobalDictionaries.DocumentsTypes.WriteOff;
      Document document = this.Document;
      Users.User user = this.User;
      // ISSUE: explicit non-virtual call
      Guid guid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
      document.UserUid = guid;
      this.Document.Number = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.WriteOff);
      this.Document.Storage = this.Storage;
      this.Document.Section = Sections.GetCurrentSection();
      using (DataBase dataBase = Data.GetDataBase())
      {
        int num = new DocumentsRepository(dataBase).Save(this.Document) ? 1 : 0;
        progressBar.Close();
        return num != 0 ? new ActionResult(ActionResult.Results.Ok) : new ActionResult(ActionResult.Results.Error);
      }
    }

    public void UpdateDocument()
    {
      List<Gbs.Core.Entities.Documents.Item> objList = new List<Gbs.Core.Entities.Documents.Item>();
      foreach (WriteOffItem writeOffItem in (Collection<WriteOffItem>) this.Items)
      {
        if (writeOffItem.Storage != null)
          writeOffItem.Storage = this.Document.Storage;
        Gbs.Core.Entities.Documents.Item obj = new Gbs.Core.Entities.Documents.Item(writeOffItem, this.Document.Uid);
        objList.Add(obj);
      }
      foreach (Gbs.Core.Entities.Documents.Item obj in this.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => this.Items.All<WriteOffItem>((Func<WriteOffItem, bool>) (g => g.Uid != x.Uid)))))
      {
        obj.IsDeleted = true;
        objList.Add(obj);
      }
      this.Document.Items = objList;
    }

    public override ActionResult Add(WriteOffItem item)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (this.Storage != null && this.Storage.Uid != item.Storage.Uid)
        return new ActionResult(ActionResult.Results.Error, Translate.WriteOff_В_одном_списании_могут_быть_товары_только_с_одного_склада_);
      this.Storage = item.Storage;
      if (item.Good.Group.GoodsType != GlobalDictionaries.GoodTypes.Weight && this.Items.Any<WriteOffItem>(new Func<WriteOffItem, bool>(Predicate)))
      {
        if (item.Good.Group.IsRequestCount)
        {
          if (!this.EditQuantity((object) new List<WriteOffItem>()
          {
            item
          }))
          {
            this.Storage = (Storages.Storage) null;
            return new ActionResult(ActionResult.Results.Warning);
          }
        }
        WriteOffItem writeOffItem1 = this.Items.First<WriteOffItem>(new Func<WriteOffItem, bool>(Predicate));
        this.Items.Remove(writeOffItem1);
        WriteOffItem writeOffItem2 = writeOffItem1;
        writeOffItem2.Quantity = writeOffItem2.Quantity + item.Quantity;
        this.Items.Add(writeOffItem1);
        this.SelectedItem = writeOffItem1;
      }
      else
      {
        if (item.Good.Group.IsRequestCount)
        {
          if (!this.EditQuantity((object) new List<WriteOffItem>()
          {
            item
          }))
          {
            this.Storage = (Storages.Storage) null;
            return new ActionResult(ActionResult.Results.Warning);
          }
        }
        this.Items.Add(item);
        this.SelectedItem = item;
      }
      this.OnPropertyChanged("Items");
      this.ReCalcTotals();
      return new ActionResult(ActionResult.Results.Ok);

      bool Predicate(WriteOffItem x)
      {
        if (x.SalePrice == item.SalePrice && x.Good.Uid == item.Good.Uid)
        {
          Guid? uid1 = x.GoodModification?.Uid;
          Guid? uid2 = item.GoodModification?.Uid;
          if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() == uid2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
            return x.Comment.IsNullOrEmpty();
        }
        return false;
      }
    }

    private void RemoveZeroQuantityItems()
    {
      this.Items = new ObservableCollection<WriteOffItem>(this.Items.Where<WriteOffItem>((Func<WriteOffItem, bool>) (i => i.Quantity > 0M)));
    }

    private bool EditQuantity(object obj)
    {
      List<WriteOffItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return false;
      (bool result, Decimal? quantity1) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) castedList.Select<WriteOffItem, BasketItem>((Func<WriteOffItem, BasketItem>) (x =>
      {
        Gbs.Core.Entities.Goods.Good good = x.Good;
        GoodsModifications.GoodModification goodModification = x.GoodModification;
        // ISSUE: explicit non-virtual call
        Guid modificationUid = goodModification != null ? __nonvirtual (goodModification.Uid) : Guid.Empty;
        Decimal salePrice = x.SalePrice;
        Storages.Storage storage = x.Storage;
        Decimal quantity2 = x.Quantity;
        Guid uid = x.Uid;
        string comment = x.Comment;
        Guid goodStockUid = new Guid();
        return new BasketItem(good, modificationUid, salePrice, storage, quantity2, guid: uid, comment: comment, goodStockUid: goodStockUid);
      })).ToList<BasketItem>(), false));
      if (!result)
        return false;
      foreach (WriteOffItem writeOffItem in castedList)
        writeOffItem.Quantity = quantity1 ?? writeOffItem.Quantity;
      this.ReCalcTotals();
      return true;
    }

    public override void ReCalcTotals()
    {
      this.RemoveZeroQuantityItems();
      this.OnPropertyChanged("TotalSaleSum");
      this.OnPropertyChanged("TotalQuantity");
      this.OnPropertyChanged("Items");
    }
  }
}
