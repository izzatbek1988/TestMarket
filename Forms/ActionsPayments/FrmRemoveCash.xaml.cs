// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ActionsPayments.PaymentsActionsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.ActionsPayments
{
  public partial class PaymentsActionsViewModel : ViewModelWithForm
  {
    private Decimal _sum;

    public bool IsSavePayment { get; set; } = true;

    public Visibility VisibilityNonFiscal { get; set; } = Visibility.Collapsed;

    public bool IsNonFiscal { get; set; }

    public ICommand ActoinsPayment { get; set; }

    public bool IsSupplier { get; set; }

    public ICommand GetClient { get; set; }

    public Action Close { get; set; }

    public PaymentsActionsViewModel.ActionsPayment Type { get; set; }

    public ICommand ClosePayment
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Payment = (Gbs.Core.Entities.Payments.Payment) null;
          this.Close();
        }));
      }
    }

    public string ClientName
    {
      get
      {
        return this.Payment.Client != null ? this.Payment.Client.Name : Translate.PaymentsActionsViewModel_Не_указан;
      }
    }

    public string ContentButtonOk { get; set; }

    public Gbs.Core.Entities.Payments.Payment Payment { get; set; } = new Gbs.Core.Entities.Payments.Payment();

    public PaymentsAccounts.PaymentsAccount AccountOut
    {
      get => this.Payment.AccountOut;
      set
      {
        this.Payment.AccountOut = value;
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        if (!checkPrinter.FiscalKkm.IsLetNonFiscal || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
          return;
        PaymentsAccounts.PaymentsAccount accountOut = this.AccountOut;
        if ((accountOut != null ? (accountOut.IsMoneyBox() ? 1 : 0) : 0) == 0)
        {
          PaymentsAccounts.PaymentsAccount accountIn = this.AccountIn;
          if ((accountIn != null ? (accountIn.IsMoneyBox() ? 1 : 0) : 0) == 0)
          {
            this.VisibilityNonFiscal = Visibility.Collapsed;
            goto label_5;
          }
        }
        this.VisibilityNonFiscal = Visibility.Visible;
label_5:
        if (this.Type.IsEither<PaymentsActionsViewModel.ActionsPayment>(PaymentsActionsViewModel.ActionsPayment.Correct, PaymentsActionsViewModel.ActionsPayment.RecountSumCash, PaymentsActionsViewModel.ActionsPayment.BalanceCorrect))
          this.VisibilityNonFiscal = Visibility.Collapsed;
        this.OnPropertyChanged("VisibilityNonFiscal");
      }
    }

    public PaymentsAccounts.PaymentsAccount AccountIn
    {
      get => this.Payment.AccountIn;
      set
      {
        this.Payment.AccountIn = value;
        Gbs.Core.Config.CheckPrinter checkPrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter;
        if (!checkPrinter.FiscalKkm.IsLetNonFiscal || checkPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
          return;
        PaymentsAccounts.PaymentsAccount accountOut = this.AccountOut;
        if ((accountOut != null ? (accountOut.IsMoneyBox() ? 1 : 0) : 1) == 0)
        {
          PaymentsAccounts.PaymentsAccount accountIn = this.AccountIn;
          if ((accountIn != null ? (accountIn.IsMoneyBox() ? 1 : 0) : 1) == 0)
          {
            this.VisibilityNonFiscal = Visibility.Collapsed;
            goto label_5;
          }
        }
        this.VisibilityNonFiscal = Visibility.Visible;
label_5:
        if (this.Type == PaymentsActionsViewModel.ActionsPayment.Correct)
          this.VisibilityNonFiscal = Visibility.Collapsed;
        this.OnPropertyChanged("VisibilityNonFiscal");
      }
    }

    public List<PaymentGroups.PaymentGroup> Groups { get; set; }

    public Decimal Sum
    {
      get => this._sum;
      set
      {
        this._sum = Math.Round(value, 2);
        this.OnPropertyChanged(nameof (Sum));
      }
    }

    public Gbs.Core.Entities.Users.User User { get; set; }

    public List<PaymentsAccounts.PaymentsAccount> Accounts { get; set; }

    public string ContentButton
    {
      get
      {
        return this.VisibilityClients != Visibility.Visible ? Translate.FrmCardMethodPayment_ДенежныйСчет : Translate.FrmRemoveCash_СчетДляСнятия;
      }
    }

    public Visibility VisibilityAccIn { get; set; }

    public Visibility VisibilityAccOut { get; set; }

    public bool IsEnabledAccOut { get; set; } = true;

    public Decimal OldSum { get; set; }

    public Visibility VisibilityGroup { get; set; }

    public Visibility VisibilityClients { get; set; }

    public PaymentsActionsViewModel()
    {
    }

    private bool PrintCheck(Gbs.Core.Config.Devices deviceConfig)
    {
      try
      {
        if (deviceConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && this.IsNonFiscal || deviceConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
          return true;
        KkmHelper kkmHelper = new KkmHelper(deviceConfig);
        Cashier cashier = new Cashier()
        {
          Name = this.User.Client.Name,
          Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
          {
            Guid entityUid = x.EntityUid;
            Guid? uid = this.User?.Client.Uid;
            return (uid.HasValue ? (entityUid == uid.GetValueOrDefault() ? 1 : 0) : 0) != 0 && x.Type.Uid == GlobalDictionaries.InnUid;
          }))?.Value.ToString() ?? "",
          UserUid = this.User.Uid
        };
        bool flag = true;
        switch (this.Type)
        {
          case PaymentsActionsViewModel.ActionsPayment.None:
          case PaymentsActionsViewModel.ActionsPayment.Correct:
          case PaymentsActionsViewModel.ActionsPayment.BalanceCorrect:
          case PaymentsActionsViewModel.ActionsPayment.RecountSumCash:
            kkmHelper.Dispose();
            return flag;
          case PaymentsActionsViewModel.ActionsPayment.Insert:
            if (this.Payment.AccountIn.Type == PaymentsAccounts.MoneyType.KkmCash)
            {
              flag = kkmHelper.CashIn(this.Sum, cashier);
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
            else
              goto case PaymentsActionsViewModel.ActionsPayment.None;
          case PaymentsActionsViewModel.ActionsPayment.Remove:
            if (this.Payment.AccountOut.Type == PaymentsAccounts.MoneyType.KkmCash)
            {
              flag = kkmHelper.CashOut(this.Sum, cashier);
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
            else
              goto case PaymentsActionsViewModel.ActionsPayment.None;
          case PaymentsActionsViewModel.ActionsPayment.Send:
            if (this.Payment.AccountOut.Type == PaymentsAccounts.MoneyType.KkmCash)
              flag = kkmHelper.CashOut(this.Sum, cashier);
            if (this.Payment.AccountIn.Type == PaymentsAccounts.MoneyType.KkmCash)
            {
              flag = kkmHelper.CashIn(this.Sum, cashier);
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
            else
              goto case PaymentsActionsViewModel.ActionsPayment.None;
          default:
            throw new ArgumentOutOfRangeException("Type", (object) this.Type, (string) null);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex.InnerException, "Ошибка действия с денежными средствами.");
        return false;
      }
    }

    public PaymentsActionsViewModel(
      PaymentsActionsViewModel.ActionsPayment type = PaymentsActionsViewModel.ActionsPayment.None,
      PaymentsAccounts.PaymentsAccount account = null,
      bool isActive = false,
      Gbs.Core.Entities.Payments.Payment payment = null,
      bool isAllAcountNoDelete = false)
    {
      PaymentsActionsViewModel actionsViewModel = this;
      this.Payment = payment ?? new Gbs.Core.Entities.Payments.Payment();
      this.Type = type;
      this.Payment.AccountOut = account;
      Gbs.Core.Config.Devices deviceConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.Groups = PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>().Where<PAYMENTS_GROUP>((Expression<Func<PAYMENTS_GROUP, bool>>) (x => x.VISIBLE_IN == (int) type && !x.IS_DELETED)));
        if (isAllAcountNoDelete)
        {
          this.Accounts = PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => !x.IS_DELETED))).ToList<PaymentsAccounts.PaymentsAccount>();
        }
        else
        {
          List<PaymentsAccounts.PaymentsAccount> list;
          if (!isActive)
            list = PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => !x.IS_DELETED))).Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => x.IsCurrentAccount())).ToList<PaymentsAccounts.PaymentsAccount>();
          else
            list = PaymentsAccounts.GetPaymentsAccountsList(dataBase.GetTable<PAYMENTS_ACCOUNT>().Where<PAYMENTS_ACCOUNT>((Expression<Func<PAYMENTS_ACCOUNT, bool>>) (x => !x.IS_DELETED))).Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => x.IsCurrentMoneyBox())).ToList<PaymentsAccounts.PaymentsAccount>();
          this.Accounts = list;
        }
      }
      List<PaymentsAccounts.PaymentsAccount> accounts = this.Accounts;
      // ISSUE: explicit non-virtual call
      if ((accounts != null ? (__nonvirtual (accounts.Count) == 1 ? 1 : 0) : 0) != 0)
      {
        this.AccountIn = this.Accounts.Single<PaymentsAccounts.PaymentsAccount>();
        this.AccountOut = this.Accounts.Single<PaymentsAccounts.PaymentsAccount>();
      }
      List<PaymentGroups.PaymentGroup> groups = this.Groups;
      // ISSUE: explicit non-virtual call
      if ((groups != null ? (__nonvirtual (groups.Count) == 1 ? 1 : 0) : 0) != 0)
        this.Payment.ParentUid = this.Groups.Single<PaymentGroups.PaymentGroup>().Uid;
      this.ActoinsPayment = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (closure_0.Sum == 0M)
        {
          if (!closure_0.Type.IsEither<PaymentsActionsViewModel.ActionsPayment>(PaymentsActionsViewModel.ActionsPayment.BalanceCorrect, PaymentsActionsViewModel.ActionsPayment.Correct, PaymentsActionsViewModel.ActionsPayment.RecountSumCash))
          {
            MessageBoxHelper.Warning(Translate.PaymentsActionsViewModel_Невозможно_провести_операцию__так_как_сумма_равна_нулю);
            return;
          }
        }
        closure_0.Payment.IsFiscal = !closure_0.IsNonFiscal;
        switch (type)
        {
          case PaymentsActionsViewModel.ActionsPayment.None:
            closure_0.Payment.Date = DateTime.Now;
            switch (closure_0.Type)
            {
              case PaymentsActionsViewModel.ActionsPayment.None:
                closure_0.Payment.User = closure_0.User;
                if (!closure_0.IsSavePayment)
                {
                  closure_0.Close();
                  return;
                }
                if (closure_0.Payment.VerifyBeforeSave().Result == ActionResult.Results.Ok)
                {
                  bool flag = true;
                  while (flag)
                  {
                    if (!closure_0.PrintCheck(deviceConfig))
                    {
                      switch (MessageBoxHelper.Show(Translate.Basket_Чек_не_удалось_распечатать__Попробовать_еще_раз_, buttons: MessageBoxButton.YesNoCancel, icon: MessageBoxImage.Hand))
                      {
                        case MessageBoxResult.Cancel:
                          return;
                        case MessageBoxResult.Yes:
                          continue;
                        case MessageBoxResult.No:
                          closure_0.Payment.Save();
                          break;
                        default:
                          throw new ArgumentOutOfRangeException();
                      }
                    }
                    else
                    {
                      closure_0.Payment.Save();
                      if (closure_0.Payment.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BalanceCorrection, GlobalDictionaries.PaymentTypes.MoneyCorrection))
                      {
                        string str = closure_0.AccountIn == null ? closure_0.AccountOut?.Name : closure_0.AccountIn?.Name;
                        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateTextHistory((closure_0.Payment.Type == GlobalDictionaries.PaymentTypes.BalanceCorrection ? Translate.PaymentsActionsViewModel_PaymentsActionsViewModel_Корректировка_суммы_расхождения_баланса_ : string.Format(Translate.PaymentsActionsViewModel_PaymentsActionsViewModel_Корректировка_суммы_на_денежном_счете___0___, (object) str)) + string.Format(Translate.PaymentsActionsViewModel_PaymentsActionsViewModel_, (object) closure_0.OldSum, (object) closure_0.Sum), GlobalDictionaries.EntityTypes.Payment, closure_0.User), false);
                      }
                    }
                    flag = false;
                  }
                }
                Task.Run((Action) (() =>
                {
                  try
                  {
                    Settings settings = new ConfigsRepository<Settings>().Get();
                    if (!settings.RemoteControl.Cloud.IsAutoSend || !settings.RemoteControl.Cloud.IsActive)
                      return;
                    LogHelper.Debug("Выгрузка данных для дом офис после снятия/внесения наличных");
                    HomeOfficeHelper.CreateArchive();
                  }
                  catch (Exception ex)
                  {
                    LogHelper.WriteError(ex);
                  }
                }));
                closure_0.Close();
                return;
              case PaymentsActionsViewModel.ActionsPayment.Insert:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.MoneyPayment;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              case PaymentsActionsViewModel.ActionsPayment.Remove:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.MoneyPayment;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              case PaymentsActionsViewModel.ActionsPayment.Send:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.MoneyMovement;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              case PaymentsActionsViewModel.ActionsPayment.Correct:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.MoneyCorrection;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              case PaymentsActionsViewModel.ActionsPayment.BalanceCorrect:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.BalanceCorrection;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              case PaymentsActionsViewModel.ActionsPayment.RecountSumCash:
                closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.RecountSumCash;
                goto case PaymentsActionsViewModel.ActionsPayment.None;
              default:
                throw new ArgumentOutOfRangeException();
            }
          case PaymentsActionsViewModel.ActionsPayment.Insert:
            if (closure_0.Payment.AccountIn == null)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_денежный_счет_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            if (closure_0.Payment.ParentUid == Guid.Empty)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_группу_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            closure_0.Payment.SumIn = closure_0.Sum;
            closure_0.Payment.AccountOut = (PaymentsAccounts.PaymentsAccount) null;
            goto case PaymentsActionsViewModel.ActionsPayment.None;
          case PaymentsActionsViewModel.ActionsPayment.Remove:
            if (closure_0.Payment.AccountOut == null)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_денежный_счет_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            if (closure_0.Payment.ParentUid == Guid.Empty)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_группу_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            closure_0.Payment.SumOut = closure_0.Sum;
            closure_0.Payment.AccountIn = (PaymentsAccounts.PaymentsAccount) null;
            goto case PaymentsActionsViewModel.ActionsPayment.None;
          case PaymentsActionsViewModel.ActionsPayment.Send:
            if (closure_0.Payment.AccountIn == null || closure_0.Payment.AccountOut == null)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_денежный_счет_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            if (closure_0.Payment.AccountIn.IsMoneyBox() && closure_0.Payment.AccountOut.IsMoneyBox())
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Невозможно_переместить_средства_с_одной_секции_на_другую, icon: MessageBoxImage.Exclamation);
              break;
            }
            if (closure_0.Payment.AccountIn.Uid == closure_0.Payment.AccountOut.Uid)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Требуется_выбрать_разные_счета_для_перемещения, icon: MessageBoxImage.Exclamation);
              break;
            }
            closure_0.Payment.SumIn = closure_0.Sum;
            closure_0.Payment.SumOut = closure_0.Sum;
            closure_0.Payment.Type = GlobalDictionaries.PaymentTypes.MoneyMovement;
            goto case PaymentsActionsViewModel.ActionsPayment.None;
          case PaymentsActionsViewModel.ActionsPayment.Correct:
          case PaymentsActionsViewModel.ActionsPayment.RecountSumCash:
            if (closure_0.Payment.AccountOut == null)
            {
              int num = (int) MessageBoxHelper.Show(Translate.PaymentsActionsViewModel_Укажите_денежный_счет_для_операции, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
              break;
            }
            closure_0.Payment.AccountIn = (PaymentsAccounts.PaymentsAccount) null;
            closure_0.Payment.Section = Sections.GetCurrentSection();
            if (closure_0.OldSum > closure_0.Sum)
            {
              closure_0.Payment.SumOut = closure_0.OldSum - closure_0.Sum;
              closure_0.Payment.AccountIn = (PaymentsAccounts.PaymentsAccount) null;
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
            else
            {
              closure_0.Payment.SumIn = closure_0.Sum - closure_0.OldSum;
              Gbs.Core.Entities.Payments.Payment payment1 = closure_0.Payment;
              PaymentsAccounts.PaymentsAccount accountOut = closure_0.Payment.AccountOut;
              PaymentsAccounts.PaymentsAccount paymentsAccount = accountOut != null ? accountOut.Clone<PaymentsAccounts.PaymentsAccount>() : (PaymentsAccounts.PaymentsAccount) null;
              payment1.AccountIn = paymentsAccount;
              closure_0.Payment.AccountOut = (PaymentsAccounts.PaymentsAccount) null;
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
          case PaymentsActionsViewModel.ActionsPayment.BalanceCorrect:
            closure_0.Payment.AccountIn = (PaymentsAccounts.PaymentsAccount) null;
            closure_0.Payment.AccountOut = (PaymentsAccounts.PaymentsAccount) null;
            closure_0.Payment.Section = Sections.GetCurrentSection();
            if (closure_0.OldSum > closure_0.Sum)
            {
              closure_0.Payment.SumOut = closure_0.OldSum - closure_0.Sum;
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
            else
            {
              closure_0.Payment.SumIn = closure_0.Sum - closure_0.OldSum;
              goto case PaymentsActionsViewModel.ActionsPayment.None;
            }
          default:
            throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        }
      }));
      this.GetClient = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Client client1 = new Client();
        Client client2;
        bool result;
        if (actionsViewModel.IsSupplier)
          (client2, result) = new FrmSearchClient().GetClient(true);
        else
          (client2, result) = new FrmSearchClient().GetClient(isUser: new ConfigsRepository<Settings>().Get().Payments.IsPayeeOnlyUser);
        if (!result)
          return;
        actionsViewModel.Payment.Client = client2;
        actionsViewModel.OnPropertyChanged(nameof (ClientName));
      }));
      this.OnPropertyChanged(nameof (Payment));
    }

    public Visibility VisibilityCancelButton { get; set; }

    public enum ActionsPayment
    {
      None = -1, // 0xFFFFFFFF
      Insert = 0,
      Remove = 1,
      Send = 2,
      Correct = 3,
      BalanceCorrect = 4,
      RecountSumCash = 5,
    }
  }
}
