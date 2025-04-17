// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.JournalGoodViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.DB;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public partial class JournalGoodViewModel : ViewModelWithForm
  {
    private GlobalDictionaries.DocumentsTypes _selectedTypes;
    private bool _isShowBuyPrice;
    private BuyPriceCounter _counter;

    public bool IsEnabledPage { get; set; } = true;

    public ICommand ReloadJournalCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsEnabledPage = false;
          this.OnPropertyChanged("IsEnabledPage");
          ProgressBarHelper.ProgressBar pr = new ProgressBarHelper.ProgressBar(Translate.JournalGoodViewModel_ReloadJournalCommand_Обновление_истории_товара);
          Task.Run((Action) (() =>
          {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            JournalGoodViewModel.Documents = GoodCardModelView.GetDocumentsForGood(JournalGoodViewModel.Good, true);
            stopwatch.Stop();
            DevelopersHelper.ShowNotification(string.Format("history load at {0}s", (object) ((double) stopwatch.ElapsedMilliseconds / 1000.0)));
            this.JournalCash = new List<JournalGoodViewModel.JournalItem>();
            this.GetDocument((IEntity) JournalGoodViewModel.Good, (IEnumerable<Document>) JournalGoodViewModel.Documents);
            this.FilterJournal();
            this.IsEnabledPage = true;
            this.OnPropertyChanged("IsEnabledPage");
            this.VisibilityLoadDocumentByGood = Visibility.Collapsed;
            this.OnPropertyChanged("VisibilityLoadDocumentByGood");
            pr.Close();
          }));
        }));
      }
    }

    public Visibility VisibilityLoadDocumentByGood { get; set; }

    private void SetVisibilityLoadDocumentByGood()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        int count = new DocumentsRepository(dataBase).GetItemsWithFilter((DocumentsRepository.IFilter) new DocumentsRepository.CommonFilter()
        {
          GoodUid = JournalGoodViewModel.Good.Uid,
          DateStart = DateTime.Now.AddYears(-1)
        }).Count;
        int dbDocsCount = DbDirectSQLHelper.GetDbDocsCount(JournalGoodViewModel.Good.Uid);
        if (count == 0 && dbDocsCount > 0)
        {
          this.ReloadJournalCommand.Execute((object) null);
        }
        else
        {
          this.VisibilityLoadDocumentByGood = count == dbDocsCount ? Visibility.Collapsed : Visibility.Visible;
          this.OnPropertyChanged("VisibilityLoadDocumentByGood");
        }
      }
    }

    public List<JournalGoodViewModel.JournalItem> Journal { get; set; } = new List<JournalGoodViewModel.JournalItem>();

    public Dictionary<GlobalDictionaries.DocumentsTypes, string> ActionList { get; set; }

    public GlobalDictionaries.DocumentsTypes SelectedTypes
    {
      get => this._selectedTypes;
      set
      {
        this._selectedTypes = value;
        this.FilterJournal();
      }
    }

    private void FilterJournal()
    {
      this.Journal = this.SelectedTypes == GlobalDictionaries.DocumentsTypes.None ? new List<JournalGoodViewModel.JournalItem>((IEnumerable<JournalGoodViewModel.JournalItem>) this.JournalCash.OrderByDescending<JournalGoodViewModel.JournalItem, DateTime?>((Func<JournalGoodViewModel.JournalItem, DateTime?>) (item => item?.Date))) : new List<JournalGoodViewModel.JournalItem>((IEnumerable<JournalGoodViewModel.JournalItem>) this.JournalCash.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x.Type == this.SelectedTypes)).OrderByDescending<JournalGoodViewModel.JournalItem, DateTime>((Func<JournalGoodViewModel.JournalItem, DateTime>) (item => item.Date)));
      this.OnPropertyChanged("Journal");
      this.CountType();
    }

    private static List<Document> Documents { get; set; }

    private static Gbs.Core.Entities.Goods.Good Good { get; set; }

    private List<JournalGoodViewModel.JournalItem> JournalCash { get; set; } = new List<JournalGoodViewModel.JournalItem>();

    private static List<Client> ListSupplier { get; set; }

    public JournalGoodViewModel()
    {
    }

    public JournalGoodViewModel(
      Gbs.Core.Entities.Goods.Good good,
      List<Document> docs,
      Gbs.Core.Entities.Users.User user,
      BuyPriceCounter counter)
    {
      JournalGoodViewModel journalGoodViewModel = this;
      JournalGoodViewModel.Good = good;
      JournalGoodViewModel.Documents = docs;
      this._counter = counter;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this._isShowBuyPrice = new UsersRepository(dataBase).GetAccess(user, Actions.ShowBuyPrice);
        this.ActionList = new Dictionary<GlobalDictionaries.DocumentsTypes, string>()
        {
          {
            GlobalDictionaries.DocumentsTypes.None,
            Translate.JournalGoodViewModel_Все_действия
          },
          {
            GlobalDictionaries.DocumentsTypes.Buy,
            Translate.JournalGoodViewModel_Поступление
          },
          {
            GlobalDictionaries.DocumentsTypes.BuyReturn,
            Translate.JournalGoodViewModel_Возврат_поступления
          },
          {
            GlobalDictionaries.DocumentsTypes.Move,
            Translate.GlobalDictionaries_Перемещение
          },
          {
            GlobalDictionaries.DocumentsTypes.MoveReturn,
            Translate.JournalGoodViewModel_Возврат_перемещения
          },
          {
            GlobalDictionaries.DocumentsTypes.MoveStorage,
            Translate.GlobalDictionaries_Перемещение_между_складами
          },
          {
            GlobalDictionaries.DocumentsTypes.Sale,
            Translate.JournalGoodViewModel_Продажа
          },
          {
            GlobalDictionaries.DocumentsTypes.SaleReturn,
            Translate.JournalGoodViewModel_Возврат_продажи
          },
          {
            GlobalDictionaries.DocumentsTypes.InventoryAct,
            Translate.FrmMainWindow_Инвентаризация
          },
          {
            GlobalDictionaries.DocumentsTypes.WriteOff,
            Translate.JournalGoodViewModel_Списание
          },
          {
            GlobalDictionaries.DocumentsTypes.ProductionSet,
            Translate.JournalGoodViewModel_JournalGoodViewModel_Производство__списание_
          },
          {
            GlobalDictionaries.DocumentsTypes.ProductionItem,
            Translate.JournalGoodViewModel_JournalGoodViewModel_Производство
          },
          {
            GlobalDictionaries.DocumentsTypes.ClientOrderReserve,
            Translate.JournalGoodViewModel_JournalGoodViewModel_Заказ_резерв_товара
          },
          {
            GlobalDictionaries.DocumentsTypes.UserStockEdit,
            Translate.JournalGoodViewModel_JournalGoodViewModel_Изменение_вручную
          },
          {
            GlobalDictionaries.DocumentsTypes.SetChildStockChange,
            Translate.JournalGoodViewModel_JournalGoodViewModel_Изменение_остатка_в_составе_комплекта
          }
        };
        if (new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        {
          this.ActionList.Add(GlobalDictionaries.DocumentsTypes.BeerProductionSet, "Вскрытие кеги (списание)");
          this.ActionList.Add(GlobalDictionaries.DocumentsTypes.BeerProductionItem, "Вскрытие кеги (начисление)");
        }
        this.SetVisibilityLoadDocumentByGood();
        Task.Run((Action) (() => journalGoodViewModel.GetDocument((IEntity) good, (IEnumerable<Document>) docs)));
      }
    }

    private void GetDocument(IEntity good, IEnumerable<Document> docs)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          JournalGoodViewModel.ListSupplier = new ClientsRepository(dataBase).GetAllItems().ToList<Client>();
          foreach (Document document in docs.Where<Document>((Func<Document, bool>) (x => !x.IsDeleted && x.Type != GlobalDictionaries.DocumentsTypes.Inventory)))
          {
            Document doc = document;
            List<Gbs.Core.Entities.Documents.Item> list = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == good.Uid)).ToList<Gbs.Core.Entities.Documents.Item>();
            if (list.Any<Gbs.Core.Entities.Documents.Item>())
              this.JournalCash.AddRange(list.GroupBy(x => new
              {
                ModificationUid = x.ModificationUid,
                Discount = x.Discount,
                Comment = x.Comment,
                SellPrice = x.SellPrice,
                GoodUid = x.GoodUid,
                BuyPrice = x.BuyPrice
              }).Select<IGrouping<\u003C\u003Ef__AnonymousType8<Guid, Decimal, string, Decimal, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, JournalGoodViewModel.JournalItem>(j => new JournalGoodViewModel.JournalItem(doc, j.ToList<Gbs.Core.Entities.Documents.Item>(), this._isShowBuyPrice, docs.ToList<Document>(), this._counter)));
          }
          this.JournalCash = this.JournalCash.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x.Action != null)).ToList<JournalGoodViewModel.JournalItem>();
          this.Journal = new List<JournalGoodViewModel.JournalItem>(this.JournalCash.OrderBy<JournalGoodViewModel.JournalItem, DateTime?>((Func<JournalGoodViewModel.JournalItem, DateTime?>) (item => item?.Date)).Reverse<JournalGoodViewModel.JournalItem>());
          this.CountType();
          ProgressBarHelper.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки истории товара");
      }
    }

    public Decimal CountWay { get; set; }

    public Decimal CountSale { get; set; }

    public Decimal CountReturn { get; set; }

    public Decimal CountEdit { get; set; }

    public Decimal CountWriteOff { get; set; }

    public Decimal CountInvent { get; set; }

    public Decimal CountMove { get; set; }

    public Decimal CountProduction { get; set; }

    public Decimal CountProductionWriteOff { get; set; }

    public void CountType()
    {
      this.CountWay = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.Buy)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountSale = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.Sale)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountReturn = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountEdit = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.UserStockEdit)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountWriteOff = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.WriteOff)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountInvent = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.InventoryAct)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountMove = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.Move)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountProduction = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.ProductionItem)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.CountProductionWriteOff = this.Journal.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x != null && x.Type == GlobalDictionaries.DocumentsTypes.ProductionSet)).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (s => s.StockDecimal));
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public class JournalItem
    {
      public GlobalDictionaries.DocumentsTypes Type { get; set; }

      public DateTime Date { get; set; }

      public string Action { get; set; }

      public string Stock { get; set; }

      public Decimal StockDecimal { get; set; }

      public JournalItem(
        Document doc,
        List<Gbs.Core.Entities.Documents.Item> items,
        bool isShowBuyPrice,
        List<Document> allDocuments,
        BuyPriceCounter buyPriceCounter,
        Gbs.Core.Entities.Goods.Good good = null)
      {
        if (DevelopersHelper.IsDebug() && good != null)
          JournalGoodViewModel.Good = good;
        this.Date = doc.DateTime;
        this.Type = doc.Type;
        Gbs.Core.Entities.Documents.Item item = items.First<Gbs.Core.Entities.Documents.Item>();
        string str1 = string.Empty;
        Guid? modificationUid = item.GoodStock?.ModificationUid;
        Guid empty = Guid.Empty;
        if ((modificationUid.HasValue ? (modificationUid.GetValueOrDefault() != empty ? 1 : 0) : 1) != 0 && JournalGoodViewModel.Good.Modifications.Any<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x =>
        {
          Guid uid = x.Uid;
          GoodsStocks.GoodStock goodStock = item.GoodStock;
          Guid guid = goodStock != null ? goodStock.ModificationUid : Guid.Empty;
          return uid == guid;
        })))
          str1 = Translate.JournalItem_Модификация__ + JournalGoodViewModel.Good.Modifications.Single<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.GoodStock.ModificationUid)).Name + Other.NewLine(onlyR: true);
        switch (doc.Type)
        {
          case GlobalDictionaries.DocumentsTypes.None:
          case GlobalDictionaries.DocumentsTypes.BuyReturn:
          case GlobalDictionaries.DocumentsTypes.MoveReturn:
          case GlobalDictionaries.DocumentsTypes.Inventory:
          case GlobalDictionaries.DocumentsTypes.CafeOrder:
          case GlobalDictionaries.DocumentsTypes.ClientOrder:
          case GlobalDictionaries.DocumentsTypes.MoveStorageChild:
          case GlobalDictionaries.DocumentsTypes.LablePrint:
          case GlobalDictionaries.DocumentsTypes.ProductionList:
          case GlobalDictionaries.DocumentsTypes.BeerProductionList:
            if (doc.Storage == null || this.Action.IsNullOrEmpty())
              break;
            this.Action += string.Format(Translate.JournalItem_склад, (object) doc.Storage.Name);
            break;
          case GlobalDictionaries.DocumentsTypes.Sale:
            if (JournalGoodViewModel.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              this.Action = string.Format(Translate.JournalItem_СертификатПодажаСтатус, (object) item.Comment);
              this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
            else
            {
              string[] strArray = new string[8]
              {
                Translate.JournalItem_Продажа,
                " #",
                doc.Number,
                Other.NewLine(onlyR: true),
                str1,
                string.Format(Translate.JournalItem_Цена___0___, (object) item.SellPrice.ToString("N")),
                string.Format(Translate.JournalItem_скидка___0__, (object) item.Discount.ToString("N")),
                null
              };
              string str2;
              if (isShowBuyPrice)
              {
                if (item.Good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range))
                {
                  string str3 = Other.NewLine(onlyR: true);
                  string journalItemПрибыль0 = Translate.JournalItem_JournalItem_Прибыль____0_;
                  Decimal sellPrice = item.SellPrice;
                  BuyPriceCounter buyPriceCounter1 = buyPriceCounter;
                  GoodsStocks.GoodStock goodStock = item.GoodStock;
                  // ISSUE: explicit non-virtual call
                  Guid stockUid = goodStock != null ? __nonvirtual (goodStock.Uid) : Guid.Empty;
                  Decimal buyPrice = buyPriceCounter1.GetBuyPrice(stockUid);
                  string str4 = ((sellPrice - buyPrice) * item.Quantity).ToString("N");
                  string str5 = string.Format(journalItemПрибыль0, (object) str4);
                  str2 = str3 + str5;
                  goto label_19;
                }
              }
              str2 = "";
label_19:
              strArray[7] = str2;
              this.Action = string.Concat(strArray);
              this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
          case GlobalDictionaries.DocumentsTypes.SaleReturn:
            this.Action = Translate.JournalItem_Возврат + Other.NewLine(onlyR: true) + str1 + string.Format(Translate.JournalItem_Стоимость___0_N_, (object) item.SellPrice) + Other.NewLine(onlyR: true) + string.Format(Translate.JournalItem_Причина___0_, (object) doc.Comment);
            this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = "+" + this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.Buy:
            string str6 = JournalGoodViewModel.ListSupplier.Any<Client>((Func<Client, bool>) (x => x.Uid == doc.ContractorUid)) ? Other.NewLine(onlyR: true) + string.Format(Translate.JournalItem_Поставщик___0_, (object) JournalGoodViewModel.ListSupplier.Single<Client>((Func<Client, bool>) (x => x.Uid == doc.ContractorUid)).Name) : "";
            this.Action = Translate.JournalItem_Поступление + " #" + doc.Number + Other.NewLine(onlyR: true) + str1 + (isShowBuyPrice ? string.Format(Translate.JournalItem_Закуп__цена___0_, (object) item.BuyPrice.ToString("N")) : "") + str6;
            if (doc.Status == GlobalDictionaries.DocumentsStatuses.Draft)
            {
              string str7 = item.Quantity.ToString("N");
              this.Action = this.Action + Other.NewLine(onlyR: true) + string.Format(Translate.JournalItem_Еще_в_пути____0__, (object) str7);
              this.StockDecimal = 0M;
              this.Stock = 0.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
            else
            {
              this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal > 0M ? "+" + this.StockDecimal.ToString("N") : this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
          case GlobalDictionaries.DocumentsTypes.Move:
            this.Action = Translate.GlobalDictionaries_Перемещение;
            this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.WriteOff:
            this.Action = Translate.JournalItem_Списание + Other.NewLine(onlyR: true) + str1 + string.Format(Translate.JournalItem_JournalItem_Цена___0_, (object) item.SellPrice) + Other.NewLine(onlyR: true) + (doc.Comment.IsNullOrEmpty() ? "" : string.Format(Translate.JournalItem_Причина___0_, (object) doc.Comment));
            this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.UserStockEdit:
            if (JournalGoodViewModel.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
            {
              this.Action = doc.Comment;
              this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal > 0M ? "+" + this.StockDecimal.ToString("N") : this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
            else
            {
              this.Action = str1 + Translate.JournalGoodViewModel_JournalGoodViewModel_Изменение_вручную + Other.NewLine(onlyR: true) + doc.Comment;
              this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal > 0M ? "+" + this.StockDecimal.ToString("N") : this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
          case GlobalDictionaries.DocumentsTypes.InventoryAct:
            if (doc.Status == GlobalDictionaries.DocumentsStatuses.Close)
            {
              this.Action = str1 + string.Format(Translate.JournalItem_JournalItem_Цена___0_, (object) item.SellPrice) + Other.NewLine(onlyR: true) + Translate.FrmMainWindow_Инвентаризация + (doc.Comment.IsNullOrEmpty() ? "" : Other.NewLine(onlyR: true) + doc.Comment);
              this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
              this.Stock = this.StockDecimal > 0M ? "+" + this.StockDecimal.ToString("N") : this.StockDecimal.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
            else
              goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.SetChildStockChange:
            this.Action = this.GetInfoForSetStockChange(doc.ParentUid);
            this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = this.StockDecimal > 0M ? "+" + this.StockDecimal.ToString("N") : this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.MoveStorage:
            Document document1 = allDocuments.SingleOrDefault<Document>((Func<Document, bool>) (x => x.ParentUid == doc.Uid));
            this.Action = Translate.GlobalDictionaries_Перемещение_между_складами + "\r" + str1 + string.Format(Translate.JournalItem_JournalItem_Склад__0______1_, (object) doc.Storage.Name, (object) document1?.Storage.Name);
            if (doc.Status == GlobalDictionaries.DocumentsStatuses.Draft)
            {
              this.Action = this.Action + "\r" + string.Format(Translate.JournalItem_Еще_в_пути____0__, (object) item.Quantity.ToString("N"));
              this.StockDecimal = 0M;
              this.Stock = 0.ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
            else
            {
              this.StockDecimal = 0M;
              this.Stock = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)).ToString("N");
              goto case GlobalDictionaries.DocumentsTypes.None;
            }
          case GlobalDictionaries.DocumentsTypes.ProductionItem:
            this.Action = string.Format(Translate.JournalItem_JournalItem_0, (object) str1, (object) string.Format(Translate.JournalItem_JournalItem_Цена___0_, (object) item.SellPrice));
            this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = "+" + this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.ProductionSet:
            Document document2 = allDocuments.SingleOrDefault<Document>((Func<Document, bool>) (x => x.Uid == doc.ParentUid));
            this.Action = string.Format(Translate.JournalItem_JournalItem_, (object) ((document2 != null ? document2.Items.FirstOrDefault<Gbs.Core.Entities.Documents.Item>()?.Good.Name : (string) null) ?? ""), (object) str1);
            this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.ClientOrderReserve:
            this.Action = string.Format(Translate.JournalItem_JournalItem_ЗаказРезервН, (object) doc.Number);
            this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = "-" + this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.BeerProductionItem:
            JournalGoodViewModel.Documents.SingleOrDefault<Document>((Func<Document, bool>) (x => x.Uid == doc.ParentUid));
            this.Action = string.Format("Вскрытие кеги пива {0}\n{1}", (object) str1, (object) string.Format(Translate.JournalItem_JournalItem_Цена___0_, (object) item.SellPrice));
            this.StockDecimal = items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = "+" + this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          case GlobalDictionaries.DocumentsTypes.BeerProductionSet:
            allDocuments.SingleOrDefault<Document>((Func<Document, bool>) (x => x.Uid == doc.ParentUid));
            this.Action = string.Format("Списание в рамках вскрытия разливного пива");
            this.StockDecimal = -1M * items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
            this.Stock = this.StockDecimal.ToString("N");
            goto case GlobalDictionaries.DocumentsTypes.None;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      private string GetInfoForSetStockChange(Guid documentUid)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        JournalGoodViewModel.JournalItem.\u003C\u003Ec__DisplayClass21_0 cDisplayClass210 = new JournalGoodViewModel.JournalItem.\u003C\u003Ec__DisplayClass21_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass210.documentUid = documentUid;
        string forSetStockChange = string.Empty;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass210.goodsTable = dataBase.GetTable<GOODS>();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass210.docsTable = dataBase.GetTable<DOCUMENTS>();
          IQueryable<DOCUMENT_ITEMS> table = dataBase.GetTable<DOCUMENT_ITEMS>();
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method reference
          // ISSUE: method reference
          DOCUMENTS documents = table.Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.UID == cDisplayClass210.documentUid)).SelectMany<DOCUMENT_ITEMS, DOCUMENTS, DOCUMENTS>((Expression<Func<DOCUMENT_ITEMS, IEnumerable<DOCUMENTS>>>) (di => cDisplayClass210.docsTable.Where<DOCUMENTS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_DOCUMENT_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))), (Expression<Func<DOCUMENT_ITEMS, DOCUMENTS, DOCUMENTS>>) ((di, d) => d)).SingleOrDefault<DOCUMENTS>();
          ParameterExpression parameterExpression3;
          ParameterExpression parameterExpression4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method reference
          // ISSUE: method reference
          GOODS goods = table.Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.UID == cDisplayClass210.documentUid)).SelectMany<DOCUMENT_ITEMS, GOODS, GOODS>((Expression<Func<DOCUMENT_ITEMS, IEnumerable<GOODS>>>) (di => cDisplayClass210.goodsTable.Where<GOODS>(System.Linq.Expressions.Expression.Lambda<Func<GOODS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_GOOD_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4))), (Expression<Func<DOCUMENT_ITEMS, GOODS, GOODS>>) ((di, g) => g)).SingleOrDefault<GOODS>();
          if (documents != null)
          {
            switch (documents.TYPE)
            {
              case 0:
              case 3:
              case 4:
              case 5:
              case 6:
              case 8:
              case 9:
              case 10:
              case 11:
              case 12:
              case 13:
              case 14:
              case 15:
              case 16:
              case 17:
              case 18:
              case 19:
              case 21:
              case 22:
              case 23:
                break;
              case 1:
                forSetStockChange += Translate.JournalGoodViewModel_Продажа;
                break;
              case 2:
                forSetStockChange += Translate.JournalItem_Возврат;
                break;
              case 7:
                forSetStockChange += Translate.JournalItem_Списание;
                break;
              case 20:
                forSetStockChange += Translate.ClientCardViewModel_ActionList_Заказ_резерв;
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
          if (goods != null)
            forSetStockChange = forSetStockChange + 23.ToString() + string.Format(Translate.JournalItem_GetInfoForSetStockChange_В_составе___0_, (object) goods.NAME);
          return forSetStockChange;
        }
      }
    }
  }
}
