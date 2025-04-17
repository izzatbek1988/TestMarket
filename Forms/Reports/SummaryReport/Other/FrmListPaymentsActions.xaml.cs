// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.PaymentsActionListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Payments;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.ActionsPayments;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public partial class PaymentsActionListViewModel : ViewModelWithForm
  {
    private DateTime _dateFinish;
    private DateTime _dateStart;
    private PaymentGroups.PaymentGroup _selectedGroup;
    private Gbs.Core.Entities.Sections.Section _selectedSection;
    private Gbs.Core.Entities.Users.User _selectedUser;
    private static Guid DocumentGroupUid = Guid.NewGuid();

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public ICommand AddPayments { get; set; }

    public ICommand DeletePayments { get; set; }

    public List<PaymentsActionListViewModel.PaymentActions> Payments { get; set; }

    public Decimal TotalSum { get; set; }

    public IEnumerable<PaymentGroups.PaymentGroup> Groups { get; set; }

    public PaymentGroups.PaymentGroup SelectedGroup
    {
      get => this._selectedGroup;
      set
      {
        this._selectedGroup = value;
        this.GetSourcePayment();
      }
    }

    public IEnumerable<Gbs.Core.Entities.Users.User> Users { get; set; }

    public Gbs.Core.Entities.Users.User SelectedUser
    {
      get => this._selectedUser;
      set
      {
        this._selectedUser = value;
        this.GetSourcePayment();
      }
    }

    public IEnumerable<Gbs.Core.Entities.Sections.Section> Sections { get; set; }

    public Gbs.Core.Entities.Sections.Section SelectedSection
    {
      get => this._selectedSection;
      set
      {
        this._selectedSection = value;
        this.GetSourcePayment();
      }
    }

    public DateTime DateStart
    {
      get => this._dateStart;
      set => this._dateStart = value;
    }

    public DateTime DateFinish
    {
      get => this._dateFinish;
      set => this._dateFinish = value;
    }

    private PaymentsActionsViewModel.ActionsPayment Type { get; set; }

    public PaymentsActionListViewModel()
    {
      PaymentGroups.PaymentGroup paymentGroup = new PaymentGroups.PaymentGroup();
      paymentGroup.Name = Translate.GoodsCatalogModelView_Все_категории;
      paymentGroup.Uid = Guid.Empty;
      this._selectedGroup = paymentGroup;
      Gbs.Core.Entities.Sections.Section section = new Gbs.Core.Entities.Sections.Section();
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      section.Uid = Guid.Empty;
      this._selectedSection = section;
      Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
      user.Alias = Translate.PaymentsActionListViewModel_Все_пользователи;
      user.Uid = Guid.Empty;
      this._selectedUser = user;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public PaymentsActionListViewModel(
      PaymentsActionsViewModel.ActionsPayment type,
      DateTime start,
      DateTime finish)
    {
      PaymentGroups.PaymentGroup paymentGroup1 = new PaymentGroups.PaymentGroup();
      paymentGroup1.Name = Translate.GoodsCatalogModelView_Все_категории;
      paymentGroup1.Uid = Guid.Empty;
      this._selectedGroup = paymentGroup1;
      Gbs.Core.Entities.Sections.Section section1 = new Gbs.Core.Entities.Sections.Section();
      section1.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      section1.Uid = Guid.Empty;
      this._selectedSection = section1;
      Gbs.Core.Entities.Users.User user1 = new Gbs.Core.Entities.Users.User();
      user1.Alias = Translate.PaymentsActionListViewModel_Все_пользователи;
      user1.Uid = Guid.Empty;
      this._selectedUser = user1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Type = type;
      this.DateStart = start;
      this.DateFinish = finish;
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<PaymentGroups.PaymentGroup> first1 = new List<PaymentGroups.PaymentGroup>();
        PaymentGroups.PaymentGroup paymentGroup2 = new PaymentGroups.PaymentGroup();
        paymentGroup2.Name = Translate.GoodsCatalogModelView_Все_категории;
        paymentGroup2.Uid = Guid.Empty;
        first1.Add(paymentGroup2);
        PaymentGroups.PaymentGroup paymentGroup3 = new PaymentGroups.PaymentGroup();
        paymentGroup3.Name = Translate.GlobalDictionaries_Платеж_по_документу;
        paymentGroup3.Uid = PaymentsActionListViewModel.DocumentGroupUid;
        first1.Add(paymentGroup3);
        this.Groups = first1.Concat<PaymentGroups.PaymentGroup>((IEnumerable<PaymentGroups.PaymentGroup>) PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>().Where<PAYMENTS_GROUP>((Expression<Func<PAYMENTS_GROUP, bool>>) (x => x.VISIBLE_IN == (int) this.Type && !x.IS_DELETED))));
        List<Gbs.Core.Entities.Users.User> first2 = new List<Gbs.Core.Entities.Users.User>();
        Gbs.Core.Entities.Users.User user2 = new Gbs.Core.Entities.Users.User();
        user2.Alias = Translate.PaymentsActionListViewModel_Все_пользователи;
        user2.Uid = Guid.Empty;
        first2.Add(user2);
        this.Users = first2.Concat<Gbs.Core.Entities.Users.User>((IEnumerable<Gbs.Core.Entities.Users.User>) new UsersRepository(dataBase).GetByQuery(dataBase.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED))));
        List<Gbs.Core.Entities.Sections.Section> first3 = new List<Gbs.Core.Entities.Sections.Section>();
        Gbs.Core.Entities.Sections.Section section2 = new Gbs.Core.Entities.Sections.Section();
        section2.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
        section2.Uid = Guid.Empty;
        first3.Add(section2);
        this.Sections = first3.Concat<Gbs.Core.Entities.Sections.Section>((IEnumerable<Gbs.Core.Entities.Sections.Section>) Gbs.Core.Entities.Sections.GetSectionsList(dataBase.GetTable<SECTIONS>().Where<SECTIONS>((Expression<Func<SECTIONS, bool>>) (x => !x.IS_DELETED))));
      }
      this.LoadPaymentsCommand = (ICommand) new RelayCommand((Action<object>) (o => this.GetSourcePayment()));
      this.AddPayments = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        switch (this.Type)
        {
          case PaymentsActionsViewModel.ActionsPayment.Insert:
            new FrmRemoveCash().InsertCash(user: this.AuthUser);
            break;
          case PaymentsActionsViewModel.ActionsPayment.Remove:
            Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
            new FrmRemoveCash().RemoveCash(ref payment, user: this.AuthUser);
            break;
        }
        this.GetSourcePayment();
      }));
      this.DeletePayments = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        List<PaymentsActionListViewModel.PaymentActions> list = ((IEnumerable) obj).Cast<PaymentsActionListViewModel.PaymentActions>().ToList<PaymentsActionListViewModel.PaymentActions>();
        if (!list.Any<PaymentsActionListViewModel.PaymentActions>())
        {
          int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionListViewModel_Необходимо_выбрать_хотя_бы_одну_запись_для_удаления);
        }
        else
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.DeletePayment) && !new Authorization().GetAccess(Actions.DeletePayment).Result || MessageBoxHelper.Show(string.Format(Translate.PaymentsActionListViewModel_, (object) list.Count, (object) list.Sum<PaymentsActionListViewModel.PaymentActions>((Func<PaymentsActionListViewModel.PaymentActions, Decimal>) (x => x.Sum)).ToString("N2")), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
              return;
            foreach (PaymentsActionListViewModel.PaymentActions paymentActions in list)
            {
              Gbs.Core.Entities.Payments.Payment oldItem = paymentActions.Payment.Clone<Gbs.Core.Entities.Payments.Payment>();
              paymentActions.Payment.IsDeleted = true;
              paymentActions.Payment.Save();
              Gbs.Core.Entities.Payments.Payment payment = paymentActions.Payment;
              Gbs.Core.Entities.Users.User authUser = this.AuthUser;
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) payment, ActionType.Delete, GlobalDictionaries.EntityTypes.Payment, authUser), true);
            }
            this.GetSourcePayment();
          }
        }
      }));
      this.GetSourcePayment();
    }

    public ICommand LoadPaymentsCommand { get; set; }

    private void GetSourcePayment()
    {
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PaymentsActionListViewModel.\u003C\u003Ec__DisplayClass63_0 cDisplayClass630 = new PaymentsActionListViewModel.\u003C\u003Ec__DisplayClass63_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass630.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass630.db = Data.GetDataBase();
        try
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PaymentsActionListViewModel.\u003C\u003Ec__DisplayClass63_1 cDisplayClass631 = new PaymentsActionListViewModel.\u003C\u003Ec__DisplayClass63_1();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass631.CS\u0024\u003C\u003E8__locals1 = cDisplayClass630;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          IQueryable<PAYMENTS> queryable = cDisplayClass631.CS\u0024\u003C\u003E8__locals1.db.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED && x.TYPE == 0 && x.DATE_TIME.Date >= this.DateStart.Date && x.DATE_TIME.Date <= this.DateFinish.Date));
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          // ISSUE: method reference
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          IQueryable<DOCUMENTS> query = queryable.SelectMany<PAYMENTS, DOCUMENTS, DOCUMENTS>(System.Linq.Expressions.Expression.Lambda<Func<PAYMENTS, IEnumerable<DOCUMENTS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
          {
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass631.CS\u0024\u003C\u003E8__locals1.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
            (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENTS.get_PARENT_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
          }), parameterExpression1), (Expression<Func<PAYMENTS, DOCUMENTS, DOCUMENTS>>) ((p, doc) => doc));
          List<Gbs.Core.Entities.Payments.Payment> paymentsList = Gbs.Core.Entities.Payments.GetPaymentsList(queryable);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cDisplayClass631.documents = new DocumentsRepository(cDisplayClass631.CS\u0024\u003C\u003E8__locals1.db).GetByQuery(query);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cDisplayClass631.clients = new ClientsRepository(cDisplayClass631.CS\u0024\u003C\u003E8__locals1.db).GetAllItems();
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.Payments = this.Type == PaymentsActionsViewModel.ActionsPayment.Remove ? paymentsList.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumIn == 0M)).Select<Gbs.Core.Entities.Payments.Payment, PaymentsActionListViewModel.PaymentActions>(new Func<Gbs.Core.Entities.Payments.Payment, PaymentsActionListViewModel.PaymentActions>(cDisplayClass631.\u003CGetSourcePayment\u003Eb__4)).ToList<PaymentsActionListViewModel.PaymentActions>() : paymentsList.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.SumOut == 0M)).Select<Gbs.Core.Entities.Payments.Payment, PaymentsActionListViewModel.PaymentActions>(new Func<Gbs.Core.Entities.Payments.Payment, PaymentsActionListViewModel.PaymentActions>(cDisplayClass631.\u003CGetSourcePayment\u003Eb__6)).ToList<PaymentsActionListViewModel.PaymentActions>();
          this.Payments = this.Payments.OrderByDescending<PaymentsActionListViewModel.PaymentActions, DateTime>((Func<PaymentsActionListViewModel.PaymentActions, DateTime>) (p => p.Payment.Date)).ToList<PaymentsActionListViewModel.PaymentActions>();
          if (this.SelectedGroup.Uid != Guid.Empty)
            this.Payments = this.Payments.Where<PaymentsActionListViewModel.PaymentActions>((Func<PaymentsActionListViewModel.PaymentActions, bool>) (x => x.Group.Uid == this.SelectedGroup.Uid)).ToList<PaymentsActionListViewModel.PaymentActions>();
          if (this.SelectedUser.Uid != Guid.Empty)
            this.Payments = this.Payments.Where<PaymentsActionListViewModel.PaymentActions>((Func<PaymentsActionListViewModel.PaymentActions, bool>) (x => x.Payment.User.Uid == this.SelectedUser.Uid)).ToList<PaymentsActionListViewModel.PaymentActions>();
          if (this.SelectedSection.Uid != Guid.Empty)
            this.Payments = this.Payments.Where<PaymentsActionListViewModel.PaymentActions>((Func<PaymentsActionListViewModel.PaymentActions, bool>) (x =>
            {
              Gbs.Core.Entities.Sections.Section section = x.Payment.Section;
              // ISSUE: explicit non-virtual call
              return (section != null ? __nonvirtual (section.Uid) : Guid.Empty) == this.SelectedSection.Uid;
            })).ToList<PaymentsActionListViewModel.PaymentActions>();
          this.TotalSum = this.Payments.Sum<PaymentsActionListViewModel.PaymentActions>((Func<PaymentsActionListViewModel.PaymentActions, Decimal>) (x => x.Sum));
          this.OnPropertyChanged("Payments");
          this.OnPropertyChanged("TotalSum");
        }
        finally
        {
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass630.db != null)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass630.db.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при загрузке списка движения средств");
      }
    }

    public class PaymentActions
    {
      public Gbs.Core.Entities.Payments.Payment Payment { get; set; }

      public PaymentsAccounts.PaymentsAccount Account { get; set; }

      public PaymentGroups.PaymentGroup Group { get; set; }

      public Decimal Sum { get; set; }

      public PaymentActions(
        Gbs.Core.Entities.Payments.Payment payment,
        PaymentsActionsViewModel.ActionsPayment type,
        List<Document> documents,
        List<Client> clients)
      {
        Document doc = documents.SingleOrDefault<Document>((Func<Document, bool>) (x => x.Uid == payment.ParentUid));
        if (doc != null)
          payment.Comment = (payment.Comment.IsNullOrEmpty() ? "" : payment.Comment + ": ") + Translate.PaymentActions_PaymentActions_ + doc.Number + (doc.ContractorUid != Guid.Empty ? " [" + (clients.SingleOrDefault<Client>((Func<Client, bool>) (x => x.Uid == doc.ContractorUid))?.Name ?? string.Empty) + "]" : "");
        this.Payment = payment;
        this.Account = type == PaymentsActionsViewModel.ActionsPayment.Remove ? payment.AccountOut : payment.AccountIn;
        this.Sum = type == PaymentsActionsViewModel.ActionsPayment.Remove ? payment.SumOut : payment.SumIn;
        PaymentGroups.PaymentGroup paymentGroup1 = PaymentGroups.GetPaymentGroupByUid(payment.ParentUid);
        if (paymentGroup1 == null)
        {
          PaymentGroups.PaymentGroup paymentGroup2 = new PaymentGroups.PaymentGroup();
          paymentGroup2.Name = Translate.GlobalDictionaries_Платеж_по_документу;
          paymentGroup2.Uid = PaymentsActionListViewModel.DocumentGroupUid;
          paymentGroup1 = paymentGroup2;
        }
        this.Group = paymentGroup1;
      }
    }
  }
}
