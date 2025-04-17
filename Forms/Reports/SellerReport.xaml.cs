// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SellerReportViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Settings;
using Gbs.Forms._shared;
using Gbs.Forms.Reports.SummaryReport.Other;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Reports
{
  public partial class SellerReportViewModel : ViewModelWithForm
  {
    private DateTime _selectedDate = DateTime.Now;

    public ICommand PrintReportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForSummaryReport(new Gbs.Helpers.FR.BackEnd.Entities.SummaryReport()
        {
          DateFinish = this.SelectedDate,
          DateStart = this.SelectedDate,
          MoneyIncomeSum = this.InsertSum,
          MoneyOutcomeSum = this.RemoveSum,
          SumCash = this.CashSum,
          TotalSaleSum = this.SaleTotalSum,
          TotalReturnCount = (Decimal) this.ReturnCount,
          TotalSalesCount = (Decimal) this.SaleCount,
          Payments = this.ListPaymentsPaid.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x != null)).GroupBy<Gbs.Core.Entities.Payments.Payment, Guid>((Func<Gbs.Core.Entities.Payments.Payment, Guid>) (x => x.Method.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, CheckPayment>((Func<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, CheckPayment>) (x => new CheckPayment()
          {
            Name = x.First<Gbs.Core.Entities.Payments.Payment>().Method.Name,
            Sum = x.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (s => s.SumIn - s.SumOut))
          })).ToList<CheckPayment>()
        }, true), this.AuthUser)));
      }
    }

    public ICommand UpdateInfoReportCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.GetDataForReport()));
    }

    public ICommand ShowTablePayment
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmPaymentByMethods().ShowDataPayment(this.ListPaymentsPaid)));
      }
    }

    public ICommand ShowJournalSale
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          SaleJournalViewModel journalViewModel = new SaleJournalViewModel();
          DateTime dateTime = this.SelectedDate;
          DateTime date = dateTime.Date;
          dateTime = new DateTime();
          DateTime finish = dateTime;
          Guid uidForm = new Guid();
          journalViewModel.ShowCard(date, finish, uidForm: uidForm, isShowCurrentSection: true);
        }));
      }
    }

    public int SaleCount { get; set; }

    public Decimal SaleTotalSum { get; set; }

    public Visibility VisibilityReturn { get; set; }

    public int ReturnCount { get; set; }

    public Visibility VisibilityRemove { get; set; }

    public Decimal RemoveSum { get; set; }

    public Visibility VisibilityInsert { get; set; }

    public Decimal InsertSum { get; set; }

    public Visibility VisibilityPayment { get; set; }

    public bool IsEnabledPayment { get; set; }

    public Decimal PaymentSum { get; set; }

    public Visibility VisibilityCash { get; set; }

    public Decimal CashSum { get; set; }

    public Visibility VisibilityCashKkm { get; set; }

    public Decimal CashKkm { get; set; }

    public Visibility VisibilityDate { get; set; }

    public DateTime SelectedDate
    {
      get => this._selectedDate;
      set
      {
        this._selectedDate = value;
        this.OnPropertyChanged(nameof (SelectedDate));
        this.GetDataForReport();
      }
    }

    private List<Gbs.Core.Entities.Payments.Payment> ListPaymentsCurrentSections { get; set; }

    private List<Gbs.Core.Entities.Payments.Payment> ListPaymentsPaid { get; set; } = new List<Gbs.Core.Entities.Payments.Payment>();

    private List<Gbs.Core.Entities.Documents.Document> ListDocuments { get; set; }

    private PaymentMethods.PaymentMethod CurrentMethod { get; set; }

    public void GetDataForReport(ProgressBarHelper.ProgressBar p = null)
    {
      Performancer per = new Performancer("Загрука данных для отчета продавца");
      if (p == null)
        p = new ProgressBarHelper.ProgressBar(Translate.SellerReportViewModel_Загрузка_данных_для_отчета_продавца);
      try
      {
        this.SaleCount = 0;
        this.SaleTotalSum = 0M;
        this.ReturnCount = 0;
        this.RemoveSum = 0M;
        this.InsertSum = 0M;
        this.CashSum = 0M;
        this.PaymentSum = 0M;
        this.OnPropertyChanged(isUpdateAllProp: true);
        Sections.Section currentSection = Sections.GetCurrentSection();
        this.CurrentMethod = PaymentMethods.GetActionPaymentsList().FirstOrDefault<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == currentSection.Uid && !x.IsDeleted));
        if (this.CurrentMethod == null)
        {
          MessageBoxHelper.Error(Translate.SellerReportViewModel_GetDataForReport_Не_удалось_получить_информацию_о_текущей_секции__обратитесь_в_службу_технической_поддержки_для_решения_данной_проблемы_);
        }
        else
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            this.ListPaymentsCurrentSections = Gbs.Core.Entities.Payments.GetPaymentsList(dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => !x.IS_DELETED))).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
            {
              Guid? nullable = x.Section?.Uid;
              Guid uid = currentSection.Uid;
              if ((nullable.HasValue ? (nullable.GetValueOrDefault() == uid ? 1 : 0) : 0) == 0)
                return false;
              nullable = x.AccountIn?.Uid;
              Guid accountUid1 = this.CurrentMethod.AccountUid;
              if ((nullable.HasValue ? (nullable.GetValueOrDefault() == accountUid1 ? 1 : 0) : 0) == 0)
              {
                nullable = x.AccountOut?.Uid;
                Guid accountUid2 = this.CurrentMethod.AccountUid;
                if ((nullable.HasValue ? (nullable.GetValueOrDefault() == accountUid2 ? 1 : 0) : 0) == 0)
                {
                  nullable = x.Method?.SectionUid;
                  Guid empty = Guid.Empty;
                  return nullable.HasValue && nullable.GetValueOrDefault() == empty;
                }
              }
              return true;
            })).ToList<Gbs.Core.Entities.Payments.Payment>();
            this.ListDocuments = new DocumentsRepository(dataBase).GetItemsWithFilter(this.SelectedDate, isOnTime: false).Where<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn) && x.Section.Uid == currentSection.Uid)).ToList<Gbs.Core.Entities.Documents.Document>();
            per.AddPoint("Загрузка списка документов");
            Task task = Task.Run((Action) (() =>
            {
              Task.Run(new Action(this.GetCountAndInfoSale)).Wait();
              Task.Run(new Action(this.SetValueSumCash)).Wait();
              Task.Run(new Action(this.SetValueSumPayments)).Wait();
              Task.Run(new Action(this.SetSumCashKkm)).Wait();
              per.Stop();
              p.Close();
            }));
            if (DevelopersHelper.IsUnitTest())
              task.Wait();
            this.OnPropertyChanged(isUpdateAllProp: true);
          }
        }
      }
      catch (Exception ex)
      {
        p.Close();
        LogHelper.Error(ex, "Ошибка при загрузке отчета продавца");
      }
    }

    private void GetCountAndInfoSale()
    {
      Performancer performancer = new Performancer("Расчет кол-ва и информации о продажах для отчета продавца");
      this.SaleCount = this.ListDocuments.Count<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale));
      if (this.VisibilityReturn == Visibility.Visible)
        this.ReturnCount = this.ListDocuments.Count<Gbs.Core.Entities.Documents.Document>((Func<Gbs.Core.Entities.Documents.Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn));
      if (this.VisibilityPayment == Visibility.Visible)
      {
        this.ListPaymentsPaid = this.ListPaymentsCurrentSections.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
        {
          DateTime dateTime = x.Date;
          DateTime date1 = dateTime.Date;
          dateTime = this.SelectedDate;
          DateTime date2 = dateTime.Date;
          if (!(date1 == date2))
            return false;
          return x.Type == GlobalDictionaries.PaymentTypes.MoneyDocumentPayment || x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment;
        })).ToList<Gbs.Core.Entities.Payments.Payment>();
        this.PaymentSum = this.ListPaymentsPaid.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
      }
      this.OnPropertyChanged(isUpdateAllProp: true);
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      foreach (Gbs.Core.Entities.Documents.Document listDocument in this.ListDocuments)
      {
        switch (listDocument.Type)
        {
          case GlobalDictionaries.DocumentsTypes.Sale:
            num1 += SaleHelper.GetSumDocument(listDocument);
            break;
          case GlobalDictionaries.DocumentsTypes.SaleReturn:
            num2 += SaleHelper.GetSumDocument(listDocument);
            num1 -= SaleHelper.GetSumDocument(listDocument);
            break;
        }
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
      this.SaleTotalSum = num1 + num2;
      this.OnPropertyChanged(isUpdateAllProp: true);
      performancer.Stop();
    }

    private void SetValueSumCash()
    {
      if (this.VisibilityCash == Visibility.Collapsed)
        return;
      Performancer performancer = new Performancer("Загрузка суммы наличных для отчета продавца");
      this.CashSum = SaleHelper.GetSumCash(new Guid?(Sections.GetCurrentSection().Uid), (List<Gbs.Core.Entities.Payments.Payment>) null);
      this.OnPropertyChanged("CashSum");
      performancer.Stop();
    }

    private void SetValueSumPayments()
    {
      if (this.VisibilityInsert == Visibility.Collapsed && this.VisibilityRemove == Visibility.Collapsed)
        return;
      Performancer performancer = new Performancer("Загрузка сумм платежей для отчета продавца");
      foreach (Gbs.Core.Entities.Payments.Payment payment in this.ListPaymentsCurrentSections.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
      {
        DateTime dateTime = x.Date;
        DateTime date1 = dateTime.Date;
        dateTime = this.SelectedDate;
        DateTime date2 = dateTime.Date;
        return date1 == date2 && x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment;
      })).AsParallel<Gbs.Core.Entities.Payments.Payment>())
      {
        if (payment.SumIn == 0M)
          this.RemoveSum += payment.SumOut;
        else
          this.InsertSum += payment.SumIn;
        this.OnPropertyChanged("RemoveSum");
        this.OnPropertyChanged("InsertSum");
      }
      performancer.Stop();
    }

    private void SetSumCashKkm()
    {
      try
      {
        if (this.VisibilityCashKkm == Visibility.Collapsed)
          return;
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        if (devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.HiPos)
          KkmHelper.UserUid = this.AuthUser.Uid;
        if (devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm & devicesConfig.CheckPrinter.FiscalKkm.KkmType != 0)
        {
          if (!devicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas, GlobalDictionaries.Devices.FiscalKkmTypes.FsPRRO, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury))
          {
            using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
            {
              Decimal sum;
              kkmHelper.GetCashSum(out sum);
              this.CashKkm = sum;
              this.OnPropertyChanged("CashKkm");
              goto label_11;
            }
          }
        }
        this.VisibilityCashKkm = Visibility.Collapsed;
label_11:
        this.OnPropertyChanged("VisibilityCashKkm");
      }
      catch (Exception ex)
      {
        this.VisibilityCashKkm = Visibility.Collapsed;
        LogHelper.Error(ex, "Ошибка при загрузке суммы в ККМ для отчета продавца", false);
        string str = "";
        if (ex is DeviceException deviceException)
          str = deviceException?.InnerException?.Message ?? "";
        if (ex is WebException webException)
          str = webException?.Message ?? "";
        ProgressBarHelper.Close();
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SellerReportViewModel_SetSumCashKkm_Не_удалось_получить_сумму_в_ККМ_для_отображения_в_отчете_ + "\n\n" + str, ProgressBarViewModel.Notification.NotificationsTypes.Error));
        this.OnPropertyChanged("VisibilityCashKkm");
      }
    }

    private Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public void ShowSellReport()
    {
      (bool Result, Gbs.Core.Entities.Users.User User) access = new Gbs.Forms._shared.Authorization().GetAccess(Gbs.Core.Entities.Actions.ShowSellerReport);
      if (!access.Result)
        return;
      this.AuthUser = access.User;
      KkmHelper.UserUid = access.User.Uid;
      SellerReportSetting setting = new SellerReportRepository().GetSetting();
      new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.VisibilityCash = setting.IsVisibilitySumCash ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityDate = setting.IsVisibilityDate ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityInsert = setting.IsVisibilityInsert ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityPayment = setting.IsVisibilitySumPayment ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityRemove = setting.IsVisibilityRemove ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityReturn = setting.IsVisibilityReturn ? Visibility.Visible : Visibility.Collapsed;
      this.VisibilityCashKkm = setting.IsVisibilitySumKkm ? Visibility.Visible : Visibility.Collapsed;
      this.IsEnabledPayment = setting.IsVisibilityTablePayment;
      this.TitleForm = this.TitleForm + ": " + Sections.GetCurrentSection().Name;
      this.FormToSHow = (WindowWithSize) new SellerReport();
      ProgressBarHelper.ProgressBar p = new ProgressBarHelper.ProgressBar(Translate.SellerReportViewModel_Загрузка_данных_для_отчета_продавца);
      this.GetDataForReport(p);
      this.ShowForm();
      p.Close();
    }

    public string TitleForm { get; set; } = Translate.FrmMainWindow_ОтчетПродавца;
  }
}
