// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.SaleCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.FR;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Sale
{
  public partial class SaleCardViewModel : ViewModelWithForm
  {
    public static bool PrintFiscalCheck(Document document)
    {
      KkmHelper.UserUid = document.UserUid;
      foreach (Gbs.Core.Entities.Payments.Payment payment in document.Payments)
      {
        if (payment.Type == GlobalDictionaries.PaymentTypes.Prepaid)
        {
          payment.Method = payment.Method.Clone<PaymentMethods.PaymentMethod>();
          payment.Method.KkmMethod = GlobalDictionaries.KkmPaymentMethods.PrePayment;
        }
      }
      int num = SaleHelper.PrintFiscalCheck(document) ? 1 : 0;
      KkmHelper.UserUid = Guid.Empty;
      if (num == 0)
        return false;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        DOCUMENTS documents = dataBase.GetTable<DOCUMENTS>().Single<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.UID == document.Uid));
        documents.IS_FISCAL = true;
        dataBase.InsertOrReplace<DOCUMENTS>(documents);
        document.IsFiscal = true;
        return true;
      }
    }

    public Visibility VisibilityPrintFiscalCheck { get; set; } = Visibility.Collapsed;

    public ICommand PrintFiscalCheckCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Show(Translate.УвереныЧтоХотитеРаспечататьФискальныйЧекДляДаннойПродажиИнформацияВТакомСлучаеБудетОтправленаВККМ, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No || !SaleCardViewModel.PrintFiscalCheck(this.Document))
            return;
          this.VisibilityPrintFiscalCheck = Visibility.Collapsed;
          this.OnPropertyChanged("VisibilityPrintFiscalCheck");
          this.OnPropertyChanged("FiscalText");
        }));
      }
    }

    public static string PrintMenuKey => nameof (PrintMenuKey);

    public string HeaderTabReturn
    {
      get
      {
        return Translate.FrmCardSale_Возвраты + string.Format(" ({0})", (object) this.ReturnItems.Count);
      }
    }

    public string FiscalText
    {
      get
      {
        Document document = this.Document;
        return (document != null ? (document.IsFiscal ? 1 : 0) : 0) == 0 ? Translate.FrmCardSale_НефискальнаяПродажа : Translate.SaleCardViewModel_IsFiscalText_Фискальная_продажа;
      }
    }

    public ICommand PrintCheckCommand { get; set; }

    public ICommand PrintDocumentCommand { get; set; }

    public ICommand PaymentDeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else if (this.Document.ContractorUid == Guid.Empty)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.SaleCardViewModel_PaymentDeleteCommand_Нельзя_удалить_платеж__так_как_к_продаже_не_привязан_контакт__покупатель___Если_нужно_скорректировать_способ_оплаты___сделайте_возврат_и_проведите_продажу_заново_, icon: MessageBoxImage.Exclamation);
          }
          else if (this.Document.IsFiscal)
          {
            int num3 = (int) MessageBoxHelper.Show(Translate.SaleCardViewModel_PaymentDeleteCommand_Нельзя_удалить_платеж__так_как_продажа_является_фискальной__уже_проведена_через_ККМ___Если_нужно_скорректировать_способ_оплаты___сделайте_возврат_и_проведите_продажу_заново_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            List<SaleCardViewModel.PaymentView> list = ((IEnumerable) obj).Cast<SaleCardViewModel.PaymentView>().ToList<SaleCardViewModel.PaymentView>();
            if (!list.Any<SaleCardViewModel.PaymentView>() | list.Count > 1)
              MessageBoxHelper.Warning(Translate.InventoryJournalViewModel_Необходимо_выбрать_одну_запись);
            else if (list.First<SaleCardViewModel.PaymentView>().Sum < 0M)
            {
              MessageBoxHelper.Warning(Translate.SaleCardViewModel_Невозможно_удалить_платеж_с_отрицательной_суммой_);
            }
            else
            {
              if (list.First<SaleCardViewModel.PaymentView>().Payment.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)
                throw new ErrorHelper.GbsException(Translate.SaleCardViewModel_PaymentDeleteCommand_Удаление_скидки_на_чек_недопустимо)
                {
                  Level = MsgException.LevelTypes.Warm
                };
              (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Gbs.Core.Entities.Actions.DeletePayment);
              if (!access.Result || MessageBoxHelper.Question(Translate.SaleCardViewModel_Вы_уверены__что_хотите_удалить_данный_платеж_) == MessageBoxResult.No)
                return;
              Gbs.Core.Entities.Payments.Payment p = list.First<SaleCardViewModel.PaymentView>().Payment;
              Gbs.Core.Entities.Payments.Payment oldItem = p.Clone<Gbs.Core.Entities.Payments.Payment>();
              p.IsDeleted = true;
              if (!p.Save())
                return;
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) p, ActionType.Delete, GlobalDictionaries.EntityTypes.Payment, access.User), true);
              this.Document.Payments[this.Document.Payments.FindIndex((Predicate<Gbs.Core.Entities.Payments.Payment>) (x => x.Uid == p.Uid))].IsDeleted = true;
              this.ListPayments.Remove(list.First<SaleCardViewModel.PaymentView>());
              this.OnPropertyChanged("ListPayments");
              this.OnPropertyChanged("SumPayments");
            }
          }
        }));
      }
    }

    public Decimal TotalSumBonuses { get; set; }

    public Document Document { get; set; }

    public string NameClient { get; set; } = string.Empty;

    public string NameUser { get; set; } = string.Empty;

    public SaleCardViewModel.SaleItems Items { get; set; }

    public List<SaleCardViewModel.ReturnItem> ReturnItems { get; set; } = new List<SaleCardViewModel.ReturnItem>();

    public Decimal GoodCountReturn
    {
      get
      {
        return this.ReturnItems.Sum<SaleCardViewModel.ReturnItem>((Func<SaleCardViewModel.ReturnItem, Decimal>) (x => x.ReturnCount));
      }
    }

    public ObservableCollection<SaleCardViewModel.PaymentView> ListPayments { get; set; } = new ObservableCollection<SaleCardViewModel.PaymentView>();

    public Decimal SumPayments
    {
      get
      {
        return this.ListPayments.Where<SaleCardViewModel.PaymentView>((Func<SaleCardViewModel.PaymentView, bool>) (x => x.Payment.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<SaleCardViewModel.PaymentView>((Func<SaleCardViewModel.PaymentView, Decimal>) (x => x.Sum));
      }
    }

    private static List<Gbs.Core.Entities.Goods.Good> ListGoodSale { get; set; }

    public SaleCardViewModel(Guid docUid, DataGrid grid)
    {
      this.Items = new SaleCardViewModel.SaleItems();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.Document = new DocumentsRepository(dataBase).GetByUid(docUid);
        foreach (Gbs.Core.Entities.Documents.Item obj in this.Document.Items)
        {
          Gbs.Core.Entities.Documents.Item item = obj;
          this.TotalSumBonuses = this.TotalSumBonuses + dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.PARENT_UID == item.Uid && x.TYPE == 5)).Sum<PAYMENTS>((Expression<Func<PAYMENTS, Decimal>>) (x => x.SUM_OUT - x.SUM_IN));
        }
        this.GetGoodsFromDocument();
        this.LoadPayments();
        if (this.Document.ContractorUid != Guid.Empty)
        {
          Client client = new ClientsRepository(dataBase).GetAllItems().FirstOrDefault<Client>((Func<Client, bool>) (x => x.Uid == this.Document.ContractorUid));
          if (client == null)
          {
            this.NameClient = Translate.GlobalDictionaries_Не_указано;
          }
          else
          {
            this.NameClient = client.Name;
            if (!client.Phone.IsNullOrEmpty())
              this.NameClient = this.NameClient + " (" + client.Phone + ")";
          }
        }
        else
          this.NameClient = Translate.GlobalDictionaries_Не_указано;
        this.NameUser = this.Document.UserUid == Guid.Empty ? Translate.GlobalDictionaries_Не_указано : new UsersRepository(dataBase).GetByUid(this.Document.UserUid).Alias;
        foreach (BasketItem basketItem in this.Document.Items.GroupBy(x => new
        {
          Comment = x.Comment,
          Discount = x.Discount,
          GoodUid = x.GoodUid,
          ModificationUid = x.ModificationUid,
          SellPrice = x.SellPrice
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType35<string, Decimal, Guid, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, BasketItem>(item => new BasketItem(item.First<Gbs.Core.Entities.Documents.Item>().Good, item.First<Gbs.Core.Entities.Documents.Item>().ModificationUid, item.First<Gbs.Core.Entities.Documents.Item>().SellPrice, item.First<Gbs.Core.Entities.Documents.Item>().GoodStock?.Storage, item.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)), item.First<Gbs.Core.Entities.Documents.Item>().Discount, comment: item.First<Gbs.Core.Entities.Documents.Item>().Comment)))
          this.Items.Items.Add(basketItem);
        this.Items.SumLessDiscount = this.Items.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.TotalSum)) + this.Items.Items.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.DiscountSum)) - this.ReturnItems.Sum<SaleCardViewModel.ReturnItem>((Func<SaleCardViewModel.ReturnItem, Decimal>) (x => x.ReturnCount * x.SalePrice));
        this.Items.TotalSaleDiscountSumMinusReturn = SaleHelper.GetSumDiscountDocument(this.Document) + this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) - this.ReturnItems.GroupBy<SaleCardViewModel.ReturnItem, Guid>((Func<SaleCardViewModel.ReturnItem, Guid>) (x => x.Document.Uid)).Sum<IGrouping<Guid, SaleCardViewModel.ReturnItem>>((Func<IGrouping<Guid, SaleCardViewModel.ReturnItem>, Decimal>) (r => SaleHelper.GetSumDiscountDocument(r.First<SaleCardViewModel.ReturnItem>().Document) - r.First<SaleCardViewModel.ReturnItem>().Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))));
        this.OnPropertyChanged(nameof (SumPayments));
        this.OnPropertyChanged(nameof (GoodCountReturn));
        this.PrintCheckCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.PrintCheck()));
        this.PrintDocumentCommand = (ICommand) new RelayCommand((Action<object>) (obj => new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForSaleDocument(this.Document), (Gbs.Core.Entities.Users.User) null)));
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        this.VisibilityPrintFiscalCheck = this.Document.IsFiscal || this.ReturnItems.Any<SaleCardViewModel.ReturnItem>() || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || checkPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None ? Visibility.Collapsed : Visibility.Visible;
        this.OnPropertyChanged(nameof (VisibilityPrintFiscalCheck));
      }
    }

    private void LoadPayments()
    {
      this.ListPayments = new ObservableCollection<SaleCardViewModel.PaymentView>(this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesCorrection, GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment) && !x.IsDeleted)).Select<Gbs.Core.Entities.Payments.Payment, SaleCardViewModel.PaymentView>((Func<Gbs.Core.Entities.Payments.Payment, SaleCardViewModel.PaymentView>) (x => new SaleCardViewModel.PaymentView(x))));
      foreach (IEnumerable<Gbs.Core.Entities.Payments.Payment> source in this.ReturnItems.GroupBy<SaleCardViewModel.ReturnItem, Guid>((Func<SaleCardViewModel.ReturnItem, Guid>) (x => x.Document.Uid)).Select<IGrouping<Guid, SaleCardViewModel.ReturnItem>, SaleCardViewModel.ReturnItem>((Func<IGrouping<Guid, SaleCardViewModel.ReturnItem>, SaleCardViewModel.ReturnItem>) (x => x.First<SaleCardViewModel.ReturnItem>())).Select<SaleCardViewModel.ReturnItem, List<Gbs.Core.Entities.Payments.Payment>>((Func<SaleCardViewModel.ReturnItem, List<Gbs.Core.Entities.Payments.Payment>>) (x => x.Document.Payments)))
        this.ListPayments = new ObservableCollection<SaleCardViewModel.PaymentView>(this.ListPayments.Concat<SaleCardViewModel.PaymentView>(source.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)).Select<Gbs.Core.Entities.Payments.Payment, SaleCardViewModel.PaymentView>((Func<Gbs.Core.Entities.Payments.Payment, SaleCardViewModel.PaymentView>) (x => new SaleCardViewModel.PaymentView(x)))));
      this.ListPayments = new ObservableCollection<SaleCardViewModel.PaymentView>((IEnumerable<SaleCardViewModel.PaymentView>) this.ListPayments.OrderByDescending<SaleCardViewModel.PaymentView, DateTime>((Func<SaleCardViewModel.PaymentView, DateTime>) (x => x.Payment.Date)));
      foreach (SaleCardViewModel.PaymentView paymentView in this.ListPayments.Where<SaleCardViewModel.PaymentView>((Func<SaleCardViewModel.PaymentView, bool>) (x => x.Payment.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)))
      {
        paymentView.Payment.Comment = string.Empty;
        paymentView.Payment.Method = new PaymentMethods.PaymentMethod()
        {
          Name = Translate.FrmCardSale_СкидкаПоЧеку
        };
      }
    }

    public SaleCardViewModel()
    {
    }

    public void GetGoodsFromDocument()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SaleCardViewModel.\u003C\u003Ec__DisplayClass62_0 cDisplayClass620 = new SaleCardViewModel.\u003C\u003Ec__DisplayClass62_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass620.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass620.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass620.dataBase = cDisplayClass620.db;
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        IQueryable<GOODS> query1 = cDisplayClass620.dataBase.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.DOCUMENT_UID == this.Document.Uid)).SelectMany<DOCUMENT_ITEMS, GOODS, GOODS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, IEnumerable<GOODS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
        {
          (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass620.dataBase, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
          (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<GOODS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_GOOD_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
        }), parameterExpression1), (Expression<Func<DOCUMENT_ITEMS, GOODS, GOODS>>) ((di, g) => g));
        // ISSUE: reference to a compiler-generated field
        SaleCardViewModel.ListGoodSale = new GoodRepository(cDisplayClass620.db).GetByQuery(query1);
        // ISSUE: reference to a compiler-generated field
        DocumentsRepository documentsRepository = new DocumentsRepository(cDisplayClass620.db);
        // ISSUE: reference to a compiler-generated field
        IQueryable<DOCUMENTS> query2 = cDisplayClass620.db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 2 && x.PARENT_UID == this.Document.Uid));
        // ISSUE: explicit non-virtual call
        foreach (Document document in __nonvirtual (documentsRepository.GetByQuery(query2)))
        {
          Document doc = document;
          this.ReturnItems.AddRange(doc.Items.GroupBy(x => new
          {
            BuyPrice = x.BuyPrice,
            GoodUid = x.GoodUid,
            Comment = x.Comment,
            Discount = x.Discount,
            ModificationUid = x.ModificationUid,
            SellPrice = x.SellPrice
          }).Select<IGrouping<\u003C\u003Ef__AnonymousType36<Decimal, Guid, string, Decimal, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, SaleCardViewModel.ReturnItem>(item => new SaleCardViewModel.ReturnItem()
          {
            Date = doc.DateTime,
            Document = doc,
            SalePrice = item.Key.SellPrice,
            SaleDiscount = Math.Round(item.Key.SellPrice * item.Key.Discount / 100M, 2),
            User = doc.UserUid == Guid.Empty ? (Gbs.Core.Entities.Users.User) null : new UsersRepository(db).GetByUid(doc.UserUid),
            Name = item.First<Gbs.Core.Entities.Documents.Item>().Good.Name,
            ReturnCount = item.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)),
            SaleCount = this.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Key.GoodUid && x.Comment == item.Key.Comment && x.Discount == item.Key.Discount && x.ModificationUid == item.Key.ModificationUid && x.SellPrice == item.Key.SellPrice)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity)),
            GoodModification = item.First<Gbs.Core.Entities.Documents.Item>().Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == item.First<Gbs.Core.Entities.Documents.Item>().ModificationUid))
          }));
        }
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass620.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass620.db.Dispose();
        }
      }
    }

    private void PrintCheck() => SaleHelper.PrintNoFiscalCheck(this.Document);

    public class SaleItems : Gbs.Core.ViewModels.Sale.Sale
    {
      public Decimal SumLessDiscount { get; set; }

      public Decimal TotalSaleDiscountSumMinusReturn { get; set; }

      public Decimal TotalSaleSumMinusReturn
      {
        get => this.SumLessDiscount - this.TotalSaleDiscountSumMinusReturn;
      }
    }

    public class ReturnItem
    {
      public DateTime Date { get; set; }

      public Document Document { get; set; }

      public Decimal SalePrice { get; set; }

      public Decimal SaleDiscount { get; set; }

      public GoodsModifications.GoodModification GoodModification { get; set; }

      public string Name { get; set; }

      public string DisplayedName
      {
        get
        {
          return this.Name + (this.GoodModification == null || !(this.GoodModification.Uid != Guid.Empty) ? string.Empty : " [" + this.GoodModification?.Name + "]");
        }
      }

      public Decimal SaleCount { get; set; }

      public Decimal ReturnCount { get; set; }

      public Gbs.Core.Entities.Users.User User { get; set; }
    }

    public class PaymentView
    {
      public Gbs.Core.Entities.Payments.Payment Payment { get; set; }

      public Decimal Sum { get; set; }

      public PaymentView(Gbs.Core.Entities.Payments.Payment payment)
      {
        this.Payment = payment;
        this.Sum = payment.SumIn == 0M ? payment.SumOut * -1M : payment.SumIn;
      }
    }
  }
}
