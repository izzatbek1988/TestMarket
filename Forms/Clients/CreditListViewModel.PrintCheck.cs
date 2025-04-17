// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.CreditListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.Sale;
using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WebSocketSharp;

#nullable disable
namespace Gbs.Forms.Clients
{
  public class CreditListViewModel : ViewModelWithForm
  {
    private Client _client;
    private bool _isCheckedClient;
    private DateTime _valueDateTimeEnd = DateTime.Now.Date;
    private DateTime _valueDateTimeStart = new DateTime(2018, 1, 1);

    private bool PrintCheck(
      CreditListViewModel.CreditItem item,
      List<SelectPaymentMethods.PaymentGrid> paymentsList)
    {
      Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (devicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || !item.Document.IsFiscal)
        return true;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CreditListViewModel_Печать_чека_внесения_платежа);
      if (devicesConfig.CheckPrinter.FiscalKkm.FfdVersion > GlobalDictionaries.Devices.FfdVersions.Ffd100)
      {
        if (!this.PrintFfd105Check(item, paymentsList) && MessageBoxHelper.Show(Translate.ReturnItemsViewModelYНеУдалосьНапечататьЧекПродолжитьСохранениеБезЧека, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) != MessageBoxResult.Yes)
        {
          progressBar.Close();
          return false;
        }
        progressBar.Close();
        return true;
      }
      if (devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne)
      {
        string docId = item.Document.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalNumUid))?.Value.ToString();
        if (!Ext.IsNullOrEmpty(docId))
        {
          using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
          {
            int num1 = kkmHelper.CreditPay(paymentsList, this.AuthUser, docId) ? 1 : 0;
            if (num1 == 0)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Не_удалось_внести_наличность_в_ККМ_по_возврату_задолженности, icon: MessageBoxImage.Hand);
            }
            progressBar.Close();
            return num1 != 0;
          }
        }
      }
      int num = this.DoCashIn(paymentsList, devicesConfig) ? 1 : 0;
      progressBar.Close();
      return num != 0;
    }

    private bool DoCashIn(
      List<SelectPaymentMethods.PaymentGrid> paymentsList,
      Gbs.Core.Config.Devices devicesConfig)
    {
      Decimal? nullable1 = paymentsList.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p => p.Type == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (p => p.Sum));
      Decimal? nullable2 = nullable1;
      Decimal num1 = 0M;
      if (nullable2.GetValueOrDefault() <= num1 & nullable2.HasValue)
        return true;
      using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
      {
        int num2 = kkmHelper.CashIn(nullable1.GetValueOrDefault(), new Cashier()
        {
          Name = this.AuthUser.Client.Name,
          Inn = this.AuthUser.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? ""
        }) ? 1 : 0;
        if (num2 == 0)
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Не_удалось_внести_наличность_в_ККМ_по_возврату_задолженности, icon: MessageBoxImage.Hand);
        }
        return num2 != 0;
      }
    }

    private bool PrintFfd105Check(
      CreditListViewModel.CreditItem item,
      List<SelectPaymentMethods.PaymentGrid> paymentsList)
    {
      CheckFactory.PrepareCheckForCreditPayment(item, paymentsList, this.AuthUser);
      return true;
    }

    public AsyncObservableCollection<CreditListViewModel.CreditItem> CreditItems { get; set; }

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set => this._valueDateTimeStart = value;
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set => this._valueDateTimeEnd = value;
    }

    public Decimal TotalSumCredit
    {
      get
      {
        AsyncObservableCollection<CreditListViewModel.CreditItem> creditItems = this.CreditItems;
        return creditItems == null ? 0M : creditItems.Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit));
      }
    }

    public ICommand GetClientFilter { get; set; }

    public ICommand ShowSale { get; set; }

    public ICommand InsertCash { get; set; }

    public bool IsCheckedClient
    {
      get => this._isCheckedClient;
      set
      {
        this._isCheckedClient = value;
        this.OnPropertyChanged(nameof (IsCheckedClient));
      }
    }

    private Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public Client Client
    {
      get => this._client;
      set
      {
        if (this._client == value)
          return;
        this._client = value;
        this.LoadCredits();
      }
    }

    public ICommand UpdateDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          CacheHelper.Clear(CacheHelper.CacheTypes.ClientsCredits);
          this.LoadCredits();
        }));
      }
    }

    private void ShowSaleCard(object obj)
    {
      List<CreditListViewModel.CreditItem> list = ((IEnumerable) obj).Cast<CreditListViewModel.CreditItem>().ToList<CreditListViewModel.CreditItem>();
      if (!list.Any<CreditListViewModel.CreditItem>())
        MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
      else if (list.Count > 1)
        MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
      else
        new FrmCardSale().ShowSaleCard(list.Single<CreditListViewModel.CreditItem>().Document);
    }

    private void UpdateSumCreditForDocument(CreditListViewModel.CreditItem item)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Document byUid = new DocumentsRepository(dataBase).GetByUid(item.Document.Uid);
        List<Document> byParentUid = new DocumentsRepository(dataBase).GetByParentUid(item.Document.Uid);
        item.Document = byUid;
        item.ReturnDocuments = byParentUid.ToList<Document>();
        item.SumCredit = ClientsRepository.GetCreditSumForDocuments(byUid, byParentUid);
      }
    }

    public CreditListViewModel()
    {
      this.LoadCreditsCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.LoadCredits()));
      this.ShowSale = (ICommand) new RelayCommand(new Action<object>(this.ShowSaleCard));
      this.InsertCash = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
        }
        else
        {
          List<CreditListViewModel.CreditItem> list = ((IEnumerable) obj).Cast<CreditListViewModel.CreditItem>().ToList<CreditListViewModel.CreditItem>();
          if (!list.Any<CreditListViewModel.CreditItem>())
          {
            MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
          }
          else
          {
            foreach (CreditListViewModel.CreditItem creditItem in list)
              this.UpdateSumCreditForDocument(creditItem);
            if (list.Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit)) <= 0M)
            {
              MessageBoxHelper.Warning(Translate.CreditListViewModel_По_всем_выбранным_записям_долга_уже_нет__обновите_список_задолженности_);
              this.UpdateDataCommand.Execute((object) null);
            }
            else
            {
              List<SelectPaymentMethods.PaymentGrid> paymentGridList1 = new List<SelectPaymentMethods.PaymentGrid>();
              List<SelectPaymentMethods.PaymentGrid> paymentGridList2;
              while (true)
              {
                (bool Result2, List<SelectPaymentMethods.PaymentGrid> ListPayment2, Decimal _) = new FrmInsertPaymentMethods().GetValuePayment(list.Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit)), 0M, false);
                if (Result2)
                {
                  paymentGridList2 = ListPayment2;
                  if (paymentGridList2.Any<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
                  {
                    PaymentMethods.PaymentMethod method = x.Method;
                    return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
                  })))
                    MessageBoxHelper.Warning(Translate.CreditListViewModel_CreditListViewModel_На_данный_момент_внести_платеж_по_СБП_для_продажи_в_долг_нельзя__выберите_другой_способ_оплаты_);
                  else
                    goto label_15;
                }
                else
                  break;
              }
              return;
label_15:
              if (list.Count > 1)
              {
                Decimal? nullable = paymentGridList2.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
                Decimal num2 = list.Sum<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit));
                if (!(nullable.GetValueOrDefault() == num2 & nullable.HasValue))
                {
                  MessageBoxHelper.Warning(Translate.CreditListViewModel_При_оплате_нескольких_задолженностей_сумма_оплаты_должна_быть_равна_сумме_долга);
                  return;
                }
                if (paymentGridList2.Count > 1)
                {
                  MessageBoxHelper.Warning(Translate.CreditListViewModel_При_оплате_нескольких_задолженностей_можно_использовать_только_один_способ_платежа);
                  return;
                }
                foreach (CreditListViewModel.CreditItem creditItem in list)
                {
                  paymentGridList2.Single<SelectPaymentMethods.PaymentGrid>().Sum = new Decimal?(creditItem.SumCredit);
                  this.AddPayment(creditItem, paymentGridList2);
                }
              }
              else
                this.AddPayment(list.Single<CreditListViewModel.CreditItem>(), paymentGridList2);
              CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.ClientsCredits);
            }
          }
        }
      }));
    }

    private void AddPayment(
      CreditListViewModel.CreditItem item,
      List<SelectPaymentMethods.PaymentGrid> list)
    {
      Decimal? nullable1 = list.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (i =>
      {
        PaymentMethods.PaymentMethod method = i.Method;
        return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
      })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (i => i.Sum));
      Decimal? nullable2 = list.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
      {
        PaymentMethods.PaymentMethod method = p.Method;
        return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
      })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (p => p.Sum));
      if (!this.AcquiringPayment(nullable1.GetValueOrDefault(), out string _, out string _, out string _, out string _, out string _, out string _, out string _) || !this.PaySBP(nullable2.GetValueOrDefault()) || !this.PrintCheck(item, list))
        return;
      List<Gbs.Core.Entities.Payments.Payment> paymentList = new List<Gbs.Core.Entities.Payments.Payment>();
      foreach (SelectPaymentMethods.PaymentGrid paymentGrid in list)
      {
        Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment()
        {
          Client = item.Client,
          SumIn = paymentGrid.Sum.GetValueOrDefault(),
          AccountIn = paymentGrid.Method.AccountUid == Guid.Empty ? (PaymentsAccounts.PaymentsAccount) null : PaymentsAccounts.GetPaymentsAccountByUid(paymentGrid.Method.AccountUid),
          Method = paymentGrid.Method,
          ParentUid = item.Document.Uid,
          Type = GlobalDictionaries.PaymentTypes.MoneyDocumentPayment,
          User = this.AuthUser
        };
        paymentList.Add(payment);
        if (!payment.Save())
          return;
      }
      CreditListViewModel.CreditItem creditItem = this.CreditItems[this.CreditItems.ToList<CreditListViewModel.CreditItem>().FindIndex((Predicate<CreditListViewModel.CreditItem>) (x => x.Document.Uid == item.Document.Uid))];
      Decimal num = list.Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault()));
      creditItem.SumCredit -= num;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Document byUid = new DocumentsRepository(dataBase).GetByUid(creditItem.Document.Uid);
        creditItem.Document = byUid;
        if (creditItem.SumCredit <= 0M)
          this.CreditItems.Remove(creditItem);
        this.OnPropertyChanged("CreditItems");
        this.OnPropertyChanged("TotalSumCredit");
        Task.Run((Action) (() =>
        {
          try
          {
            PlanfixHelper.AnaliticHelper.AddPaymentCreditAnalitic(item.Document, paymentList);
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex);
          }
        }));
      }
    }

    private bool AcquiringPayment(
      Decimal sum,
      out string rrn,
      out string method,
      out string approvalCode,
      out string issuerName,
      out string terminalId,
      out string cardNumber,
      out string paymentSystem)
    {
      using (AcquiringHelper acquiringHelper = new AcquiringHelper())
        return acquiringHelper.DoPayment(sum, out rrn, out method, out approvalCode, out issuerName, out terminalId, out cardNumber, out paymentSystem);
    }

    private bool PaySBP(Decimal sum)
    {
      int num = sum <= 0M ? 1 : 0;
      return true;
    }

    public static List<CreditListViewModel.CreditItem> UpdateCreditsCache()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreditListViewModel.\u003C\u003Ec__DisplayClass49_0 cDisplayClass490 = new CreditListViewModel.\u003C\u003Ec__DisplayClass49_0();
      Performancer performancer = new Performancer("Загрузка должников.  ");
      // ISSUE: reference to a compiler-generated field
      cDisplayClass490.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        List<Gbs.Core.Entities.Users.User> allItems = new UsersRepository(cDisplayClass490.db).GetAllItems();
        performancer.AddPoint("Сотрудники");
        // ISSUE: reference to a compiler-generated field
        List<Client> list1 = new ClientsRepository(cDisplayClass490.db).GetAllItems().ToList<Client>();
        performancer.AddPoint("Контакты");
        // ISSUE: reference to a compiler-generated field
        DocumentsRepository documentsRepository = new DocumentsRepository(cDisplayClass490.db);
        documentsRepository.MultiThreadMode = false;
        List<CreditListViewModel.CreditItem> creditItemList = new List<CreditListViewModel.CreditItem>();
        ParameterExpression right;
        ParameterExpression parameterExpression;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        List<Document> list2 = documentsRepository.GetByQuery(CreditListViewModel.LoadCreditsDocsFromDb(cDisplayClass490.db).SelectMany<Guid, DOCUMENTS, DOCUMENTS>(System.Linq.Expressions.Expression.Lambda<Func<Guid, IEnumerable<DOCUMENTS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
        {
          (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass490.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
          (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.OrElse((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) right, false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_PARENT_UID))), (System.Linq.Expressions.Expression) right, false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression))
        }), right), (Expression<Func<Guid, DOCUMENTS, DOCUMENTS>>) ((npd, d) => d))).ToList<Document>();
        IEnumerable<Document> documents = list2.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale));
        List<Document> list3 = list2.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)).ToList<Document>();
        performancer.AddPoint("Загрузка документов");
        foreach (Document document in documents)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CreditListViewModel.\u003C\u003Ec__DisplayClass49_1 cDisplayClass491 = new CreditListViewModel.\u003C\u003Ec__DisplayClass49_1();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass491.sale = document;
          // ISSUE: reference to a compiler-generated field
          if (!(cDisplayClass491.sale.ContractorUid == Guid.Empty))
          {
            // ISSUE: reference to a compiler-generated method
            List<Document> list4 = list3.Where<Document>(new Func<Document, bool>(cDisplayClass491.\u003CUpdateCreditsCache\u003Eb__5)).ToList<Document>();
            // ISSUE: reference to a compiler-generated field
            Decimal creditSumForDocuments = ClientsRepository.GetCreditSumForDocuments(cDisplayClass491.sale, list4.ToList<Document>());
            if (!(Math.Abs(creditSumForDocuments) <= 0.05M))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              CreditListViewModel.CreditItem creditItem = new CreditListViewModel.CreditItem(cDisplayClass491.sale, creditSumForDocuments, list4)
              {
                Client = list1.SingleOrDefault<Client>(new Func<Client, bool>(cDisplayClass491.\u003CUpdateCreditsCache\u003Eb__6)),
                User = allItems.SingleOrDefault<Gbs.Core.Entities.Users.User>(new Func<Gbs.Core.Entities.Users.User, bool>(cDisplayClass491.\u003CUpdateCreditsCache\u003Eb__7))
              };
              creditItemList.Add(creditItem);
            }
          }
        }
        performancer.Stop();
        return creditItemList;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass490.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass490.db.Dispose();
        }
      }
    }

    private static IQueryable<Guid> LoadCreditsDocsFromDb(Gbs.Core.Db.DataBase db)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreditListViewModel.\u003C\u003Ec__DisplayClass50_0 cDisplayClass500 = new CreditListViewModel.\u003C\u003Ec__DisplayClass50_0();
      db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false));
      IQueryable<DOCUMENT_ITEMS> queryable1 = db.GetTable<DOCUMENT_ITEMS>().Where<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, bool>>) (x => x.IS_DELETED == false));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.docItems = queryable1;
      IQueryable<PAYMENTS> queryable2 = db.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.IS_DELETED == false));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.docPayments = queryable2;
      IQueryable<DOCUMENTS> queryable3 = db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.IS_DELETED == false && x.TYPE == 2));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass500.returns = queryable3;
      ParameterExpression parameterExpression1;
      ParameterExpression parameterExpression2;
      ParameterExpression parameterExpression3;
      ParameterExpression parameterExpression4;
      ParameterExpression parameterExpression5;
      ParameterExpression parameterExpression6;
      ParameterExpression parameterExpression7;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      // ISSUE: method reference
      IQueryable<Guid> source = db.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE == 1)).Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (sale => Math.Abs(cDisplayClass500.docItems.Where<DOCUMENT_ITEMS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(item.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)).Sum<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, double>>) (item => (double) item.QUANTITY * (double) item.SALE_PRICE * (100.0 - (double) item.DISCOUNT) / 100.0)) - (cDisplayClass500.docPayments.Where<PAYMENTS>(System.Linq.Expressions.Expression.Lambda<Func<PAYMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(pay.PARENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_TYPE))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 6, typeof (int)))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_TYPE))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 5, typeof (int)))), parameterExpression3)).Sum<PAYMENTS>((Expression<Func<PAYMENTS, double?>>) (pay => (double?) (double) (pay.SUM_IN - pay.SUM_OUT))) ?? 0.0) + ((double?) cDisplayClass500.returns.Where<DOCUMENTS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(ret.PARENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4)).Sum<DOCUMENTS>((Expression<Func<DOCUMENTS, double>>) (ret => cDisplayClass500.docItems.Where<DOCUMENT_ITEMS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(item.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression6)).Sum<DOCUMENT_ITEMS>((Expression<Func<DOCUMENT_ITEMS, double>>) (item => (double) item.QUANTITY * (double) item.SALE_PRICE * (100.0 - (double) item.DISCOUNT) / 100.0)) + cDisplayClass500.docPayments.Where<PAYMENTS>(System.Linq.Expressions.Expression.Lambda<Func<PAYMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(pay.PARENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression7)).Sum<PAYMENTS>((Expression<Func<PAYMENTS, double>>) (pay => (double) (pay.SUM_IN - pay.SUM_OUT))))) ?? 0.0)) > 0.03)).OrderByDescending<DOCUMENTS, DateTime>((Expression<Func<DOCUMENTS, DateTime>>) (x => x.DATE_TIME)).Select<DOCUMENTS, Guid>((Expression<Func<DOCUMENTS, Guid>>) (x => x.UID));
      if (false)
        return source;
      List<Guid> list = source.ToList<Guid>();
      db.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.DocumentInCreditUid));
      foreach (Guid guid in list)
        db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
        {
          UID = Guid.NewGuid(),
          IS_DELETED = false,
          ENTITY_UID = guid,
          TYPE_UID = GlobalDictionaries.DocumentInCreditUid,
          CONTENT = "1"
        });
      return db.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.DocumentInCreditUid && x.CONTENT == "1")).Select<ENTITY_PROPERTIES_VALUES, Guid>((Expression<Func<ENTITY_PROPERTIES_VALUES, Guid>>) (x => x.ENTITY_UID));
    }

    public ICommand LoadCreditsCommand { get; set; }

    public void LoadCredits()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.CreditListViewModel_Загрузка_списка_задолженностей);
      this.CreditItems = new AsyncObservableCollection<CreditListViewModel.CreditItem>((IEnumerable<CreditListViewModel.CreditItem>) CacheHelper.Get<List<CreditListViewModel.CreditItem>>(CacheHelper.CacheTypes.ClientsCredits, new Func<List<CreditListViewModel.CreditItem>>(CreditListViewModel.UpdateCreditsCache)).Where<CreditListViewModel.CreditItem>((Func<CreditListViewModel.CreditItem, bool>) (x =>
      {
        DateTime dateTime1 = x.Document.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.ValueDateTimeStart;
        DateTime date2 = dateTime1.Date;
        if (date1 >= date2)
        {
          DateTime dateTime2 = x.Document.DateTime;
          DateTime date3 = dateTime2.Date;
          dateTime2 = this.ValueDateTimeEnd;
          DateTime date4 = dateTime2.Date;
          if (date3 <= date4 && (this.Client == null || x.Document.ContractorUid == this.Client.Uid))
            return Math.Abs(x.SumCredit) > 0.01M;
        }
        return false;
      })).ToList<CreditListViewModel.CreditItem>().OrderByDescending<CreditListViewModel.CreditItem, DateTime>((Func<CreditListViewModel.CreditItem, DateTime>) (x => x.Document.DateTime)).ThenByDescending<CreditListViewModel.CreditItem, Decimal>((Func<CreditListViewModel.CreditItem, Decimal>) (x => x.SumCredit)).ToList<CreditListViewModel.CreditItem>());
      this.OnPropertyChanged("CreditItems");
      this.OnPropertyChanged("TotalSumCredit");
      progressBar.Close();
    }

    public void ShowCreditList(
      Client c = null,
      Gbs.Core.Entities.Users.User authUser = null,
      DateTime dateStart = default (DateTime),
      DateTime dateFinish = default (DateTime))
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(ref authUser, Gbs.Core.Entities.Actions.ShowCredits))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) = new Authorization().GetAccess(Gbs.Core.Entities.Actions.ShowCredits);
            if (!Result)
              return;
            authUser = User;
          }
          this.AuthUser = authUser;
          if (c != null)
          {
            this._client = c;
            this.Client = c;
            this.IsCheckedClient = true;
            this.OnPropertyChanged("IsCheckedClient");
          }
          this.ValueDateTimeStart = dateStart != new DateTime() ? dateStart : new DateTime(2018, 1, 1);
          this.ValueDateTimeEnd = dateFinish != new DateTime() ? dateFinish : DateTime.Now;
          this.FormToSHow = (WindowWithSize) new FrmCreditClients();
          ((FrmCreditClients) this.FormToSHow).ClientSelectionControl.Client = this.Client;
          ((FrmCreditClients) this.FormToSHow).ClientSelectionControl.IsCheckedClient = this.Client != null;
          this.ShowForm();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в списке должников");
      }
    }

    public class CreditItem : ViewModel
    {
      private Decimal _sumCredit;

      public Document Document { get; set; }

      public List<Document> ReturnDocuments { get; set; } = new List<Document>();

      public Client Client { get; set; }

      public Decimal SumCredit
      {
        get => this._sumCredit;
        set
        {
          this._sumCredit = value;
          this.OnPropertyChanged(nameof (SumCredit));
        }
      }

      public Decimal SumPayment
      {
        get
        {
          return this.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)) - this.ReturnDocuments.SelectMany<Document, Gbs.Core.Entities.Payments.Payment>((Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (x => (IEnumerable<Gbs.Core.Entities.Payments.Payment>) x.Payments)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
        }
      }

      public Decimal TotalDocument
      {
        get
        {
          return SaleHelper.GetSumDocument(this.Document) - this.ReturnDocuments.Sum<Document>(new Func<Document, Decimal>(SaleHelper.GetSumDocument));
        }
      }

      public Gbs.Core.Entities.Users.User User { get; set; }

      public CreditItem(Document document, Decimal sum, List<Document> returnDocuments)
      {
        this.Document = document;
        this.ReturnDocuments = new List<Document>((IEnumerable<Document>) returnDocuments);
        this.SumCredit = sum;
      }
    }
  }
}
