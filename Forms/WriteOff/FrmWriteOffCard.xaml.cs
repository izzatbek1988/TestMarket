// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.WriteOff.WriteOffCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.WriteOff;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.WriteOff
{
  public partial class WriteOffCardViewModel : ViewModel, ICheckChangesViewModel
  {
    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public Action CloseAction { get; set; }

    public Gbs.Core.ViewModels.WriteOff.WriteOff WriteOff { get; set; } = new Gbs.Core.ViewModels.WriteOff.WriteOff();

    public ICommand AddItem
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.WriteOff, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket));
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.WriteOff.User = this.AuthUser;
          this.WriteOff.Document.Status = GlobalDictionaries.DocumentsStatuses.Close;
          if (this.WriteOff.Save().Result != ActionResult.Results.Ok)
            return;
          WriteOffCardViewModel.WriteOffEgais(this.WriteOff.Document.Items);
          this.UpdateGridDocuments();
          WindowWithSize.IsCancel = false;
          this.CloseAction();
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) null, (IEntity) this.WriteOff.Document, ActionType.Add, GlobalDictionaries.EntityTypes.Document, this.AuthUser), false);
        }));
      }
    }

    public static void WriteOffEgais(List<Gbs.Core.Entities.Documents.Item> items)
    {
      if (!new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        return;
      List<EgaisWriteOffActsItems> writeOffActsItemsList = new List<EgaisWriteOffActsItems>();
      foreach (Gbs.Core.Entities.Documents.Item obj in items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) != EgaisHelper.AlcoholTypeGorEgais.NoAlcohol && !EgaisHelper.IsBeerKega(x.Good))))
      {
        EgaisHelper.AlcoholTypeGorEgais alcoholType = EgaisHelper.GetAlcoholType(obj.Good);
        GoodsStocks.GoodStock stocksByUid = GoodsStocks.GetStocksByUid(obj.GoodStock.Uid);
        string numberForGoodStock = SharedRepository.GetFbNumberForGoodStock(stocksByUid);
        if (!numberForGoodStock.IsNullOrEmpty())
          writeOffActsItemsList.Add(new EgaisWriteOffActsItems()
          {
            FbNumber = numberForGoodStock,
            MarkInfo = alcoholType == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol ? obj.Comment : "",
            Quantity = obj.Quantity,
            StockUid = stocksByUid.Uid,
            Sum = SaleHelper.GetSumItemInDocument(obj),
            ActType = TypeWriteOff1.Производственныепотери
          });
      }
      if (!writeOffActsItemsList.Any<EgaisWriteOffActsItems>())
        return;
      new EgaisWriteOffActsItemRepository().Save(writeOffActsItemsList);
    }

    public WriteOffCardViewModel()
    {
    }

    public WriteOffCardViewModel(Document document)
    {
      this.WriteOff.Document = document;
      this.EntityClone = (IEntity) document.Clone<Document>();
      this.WriteOff.Items = new ObservableCollection<WriteOffItem>(document.Items.GroupBy(x => new
      {
        GoodUid = x.GoodUid,
        SellPrice = x.SellPrice,
        ModificationUid = x.ModificationUid,
        Discount = x.Discount
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType9<Guid, Decimal, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, WriteOffItem>(x => new WriteOffItem(x.First<Gbs.Core.Entities.Documents.Item>().Good, x.Key.ModificationUid, x.Key.SellPrice, document.Storage, x.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (s => s.Quantity)), x.Key.Discount)));
    }

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool allCount = false, bool checkMinus = true)
    {
      List<string> stringList = new List<string>();
      Sales sales = new ConfigsRepository<Settings>().Get().Sales;
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        switch (good.SetStatus)
        {
          case GlobalDictionaries.GoodsSetStatuses.None:
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              MessageBoxHelper.Warning(Translate.WriteOffCardViewModel_Невозможно_списать_сертификат);
              continue;
            }
            if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
            {
              MessageBoxHelper.Warning(Translate.WriteOffCardViewModel_Невозможно_списать_услугу);
              continue;
            }
            goto case GlobalDictionaries.GoodsSetStatuses.Production;
          case GlobalDictionaries.GoodsSetStatuses.Set:
            this.GetStock(good, sales.AllowSalesToMinus, stringList, checkMinus);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Kit:
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              using (IEnumerator<GoodsSets.Set> enumerator = good.SetContent.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  GoodsSets.Set current = enumerator.Current;
                  this.GetStock(new GoodRepository(dataBase).GetByUid(current.GoodUid), sales.AllowSalesToMinus, stringList, allCount);
                }
                continue;
              }
            }
          case GlobalDictionaries.GoodsSetStatuses.Range:
            this.GetStock(good, sales.AllowSalesToMinus, stringList, allCount);
            continue;
          case GlobalDictionaries.GoodsSetStatuses.Production:
            this.GetStock(good, sales.AllowSalesToMinus, stringList, allCount);
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      if (!stringList.Any<string>())
        return;
      MessageBoxHelper.Warning(string.Join(Other.NewLine(), (IEnumerable<string>) stringList));
    }

    private void GetStock(Gbs.Core.Entities.Goods.Good good, bool allowSalesToMinus, List<string> msgError, bool allCount)
    {
      List<GoodsStocks.GoodStock> stocks;
      if (!new FrmSelectGoodStock().SelectedStock(good, out stocks, false))
        return;
      foreach (GoodsStocks.GoodStock goodStock in stocks)
      {
        Decimal q = !allCount ? (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M) : (goodStock.Stock > 0M ? goodStock.Stock : (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M));
        ActionResult actionResult = this.WriteOff.Add(new WriteOffItem(good, goodStock.ModificationUid, goodStock.Price, goodStock.Storage, q));
        if (actionResult.Result == ActionResult.Results.Error)
        {
          int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Exclamation);
        }
      }
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public Action UpdateSortGrid { get; set; }

    public List<WriteOffJournalViewModel.WriteOffJournalItem> ListDoc { get; set; }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      this.WriteOff.UpdateDocument();
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.WriteOff.Document);
    }

    private void UpdateGridDocuments()
    {
      if (this.ListDoc == null)
        return;
      if (this.ListDoc.Any<WriteOffJournalViewModel.WriteOffJournalItem>((Func<WriteOffJournalViewModel.WriteOffJournalItem, bool>) (x => x.WriteOffDocument.Uid == this.WriteOff.Document.Uid)))
        this.ListDoc[this.ListDoc.ToList<WriteOffJournalViewModel.WriteOffJournalItem>().FindIndex((Predicate<WriteOffJournalViewModel.WriteOffJournalItem>) (x => x.WriteOffDocument.Uid == this.WriteOff.Document.Uid))].WriteOffDocument = this.WriteOff.Document;
      else
        this.ListDoc.Add(new WriteOffJournalViewModel.WriteOffJournalItem(this.WriteOff.Document, this.DocWaybills));
      this.UpdateSortGrid();
    }

    public List<Document> DocWaybills { get; set; } = new List<Document>();

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }
  }
}
