// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.Return.ReturnItemsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Devices.AcquiringTerminals;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.Sbp;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Sale.Return
{
  public class ReturnItemsViewModel : ViewModelWithForm
  {
    public readonly bool CountNotNull = true;
    private bool _isActiveForm = true;

    private bool PrintCheck(Document returnDocument, Gbs.Core.Entities.Users.User user)
    {
      Gbs.Core.Config.Devices config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (config.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None || config.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.None)
        return true;
      List<CheckGood> list1 = returnDocument.Items.Select<Gbs.Core.Entities.Documents.Item, CheckGood>((Func<Gbs.Core.Entities.Documents.Item, CheckGood>) (i => new CheckGood(i.Good, i.SellPrice, i.Discount, i.Quantity, i.Comment, i.ModificationUid != Guid.Empty ? i.Good.Name + " [" + i.Good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Uid == i.ModificationUid))?.Name + "]" : ""))).ToList<CheckGood>();
      List<CheckPayment> list2 = returnDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Select<Gbs.Core.Entities.Payments.Payment, CheckPayment>((Func<Gbs.Core.Entities.Payments.Payment, CheckPayment>) (p => new CheckPayment()
      {
        Name = p.Method.Name,
        Method = p.Method.KkmMethod,
        Sum = p.SumOut
      })).ToList<CheckPayment>();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        Cashier cashier1 = new Cashier();
        cashier1.Name = user.Client.Name;
        cashier1.Inn = user.Client.GetInn();
        Gbs.Core.Entities.Users.User authUser = this.AuthUser;
        // ISSUE: explicit non-virtual call
        cashier1.UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty;
        Cashier cashier2 = cashier1;
        ClientAdnSum clientByUidAndSum = ReturnItemsViewModel.SaleDocument.ContractorUid == Guid.Empty ? (ClientAdnSum) null : new ClientsRepository(dataBase).GetClientByUidAndSum(ReturnItemsViewModel.SaleDocument.ContractorUid);
        string str1 = ReturnItemsViewModel.SaleDocument.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalNumUid))?.Value?.ToString() ?? "";
        string str2 = ReturnItemsViewModel.SaleDocument.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FrNumber))?.Value?.ToString() ?? "";
        List<EntityProperties.PropertyValue> propertyValueList = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) returnDocument.Properties);
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData1 = new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(list1, list2, CheckFiscalTypes.Fiscal, cashier2)
        {
          Number = returnDocument.Number,
          CheckType = CheckTypes.ReturnSale,
          Client = clientByUidAndSum,
          FiscalNum = str1,
          FrNumber = str2,
          Properties = propertyValueList
        };
        foreach (EntityProperties.PropertyValue property1 in ReturnItemsViewModel.SaleDocument.Properties)
        {
          EntityProperties.PropertyValue property = property1;
          if (!checkData1.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
          {
            Guid? uid1 = x.Type?.Uid;
            Guid? uid2 = property.Type?.Uid;
            if (uid1.HasValue != uid2.HasValue)
              return false;
            return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
          })))
            checkData1.Properties.Add(property);
        }
        checkData1.DiscountSum = returnDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut));
        Other.ConsoleWrite(string.Format("discount: {0}", (object) checkData1.DiscountSum));
        CheckFactory checkFactory = new CheckFactory(config.CheckPrinter);
        CheckFactory.PrepareOptions prepareOptions = new CheckFactory.PrepareOptions()
        {
          RoundCheck = false
        };
        Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData2 = checkData1;
        CheckFactory.PrepareOptions options = prepareOptions;
        PreparedCheckData preparedCheckData = checkFactory.PrepareCheckData(checkData2, options);
        LogHelper.Debug(ReturnItemsViewModel.SaleDocument.ToJsonString(true));
        preparedCheckData.CheckData.Properties.AddRange((IEnumerable<EntityProperties.PropertyValue>) ReturnItemsViewModel.SaleDocument.Properties);
        LogHelper.Debug(preparedCheckData.CheckData.ToJsonString(true));
        return new CheckPrinterHelper(config).PrintCheck(preparedCheckData.CheckData);
      }
    }

    private void ReturnSave()
    {
      try
      {
        this.ValidateLists();
        (Gbs.Core.Entities.Payments.Payment paymentSpendBonuses, Decimal sumReturn, Decimal discountOnCheck) = this.GetReturnsSum();
        if (this.Return.SumItemReturn > sumReturn)
          this.ShowReturnSumMessage(sumReturn, paymentSpendBonuses);
        string commentForReturn = ReturnItemsViewModel.GetCommentForReturn(sumReturn);
        if (ReturnItemsViewModel.SaleDocument.IsFiscal)
          this.CheckKkmConnection();
        List<SelectPaymentMethods.PaymentGrid> payments = ReturnItemsViewModel.GetPayments(sumReturn);
        Decimal sum = payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
        {
          PaymentMethods.PaymentMethod method = x.Method;
          return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
        })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal>) (x => x.Sum.GetValueOrDefault()));
        Decimal? nullable = payments.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (p =>
        {
          PaymentMethods.PaymentMethod method = p.Method;
          return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
        })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (p => p.Sum));
        string str1 = ReturnItemsViewModel.SaleDocument.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RrnUid))?.Value.ToString() ?? "";
        string str2 = ReturnItemsViewModel.SaleDocument.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.TypeCardMethodUid))?.Value.ToString() ?? "";
        string rrn1 = ReturnItemsViewModel.SaleDocument.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RrnSbpUid))?.Value.ToString() ?? "";
        string numberDocument = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.SaleReturn);
        if (!this.SBPReturn(nullable.GetValueOrDefault(), rrn1, numberDocument))
          throw new CancelException("Возврат по СБП не прошел или был отменен");
        string rrn2 = str1;
        string typeCardMethod = str2;
        if (!ReturnItemsViewModel.AcquiringReturn(sum, rrn2, typeCardMethod))
          throw new CancelException("Возврат по эквайрингу не прошел или был отменен");
        List<Gbs.Core.Entities.Payments.Payment> docPayments = this.CreateDocPayments(payments, paymentSpendBonuses);
        if (discountOnCheck != 0M)
          docPayments.Add(new Gbs.Core.Entities.Payments.Payment()
          {
            Type = GlobalDictionaries.PaymentTypes.CheckDiscount,
            SumOut = discountOnCheck,
            User = this.AuthUser
          });
        this.AddAnaliticsPf(this.SaveReturnDocument(commentForReturn, docPayments, numberDocument), ReturnItemsViewModel.SaleDocument);
        WindowWithSize.IsCancel = false;
        this.IsActiveForm = true;
        this.CloseFormAction();
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Text = Translate.ReturnItemsViewModel_Возврат_товаров_от_покупателя_успешно_выполнен,
          Title = Translate.ReturnItemsViewModel_Возврат_выполнен
        });
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при возврате товара");
        ProgressBarHelper.Close();
        this.IsActiveForm = true;
      }
    }

    private Decimal CalculateDiscountForCheckValue()
    {
      Decimal num1 = ReturnItemsViewModel.SaleDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
      Decimal num2 = ReturnItemsViewModel.SaleDocument.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => Math.Round(x.Quantity * x.SellPrice * (100.0M - x.Discount) / 100.0M, 2, MidpointRounding.AwayFromZero)));
      return num2 == 0M ? 0M : Math.Round(num1 / num2 * this.Return.SumItemReturn, 2, MidpointRounding.AwayFromZero);
    }

    public List<Gbs.Core.Entities.Payments.Payment> CreateDocPayments(
      List<SelectPaymentMethods.PaymentGrid> listPayment,
      Gbs.Core.Entities.Payments.Payment paymentSpendBonuses)
    {
      List<Gbs.Core.Entities.Payments.Payment> list = listPayment.Select<SelectPaymentMethods.PaymentGrid, Gbs.Core.Entities.Payments.Payment>((Func<SelectPaymentMethods.PaymentGrid, Gbs.Core.Entities.Payments.Payment>) (x => new Gbs.Core.Entities.Payments.Payment()
      {
        ParentUid = ReturnItemsViewModel.SaleDocument.Uid,
        Type = GlobalDictionaries.PaymentTypes.MoneyDocumentPayment,
        User = this.AuthUser,
        Method = x.Method,
        Comment = Translate.ReturnItemsViewModel_Возврат_продажи,
        AccountOut = x.Method.AccountUid == Guid.Empty ? (PaymentsAccounts.PaymentsAccount) null : PaymentsAccounts.GetPaymentsAccountByUid(x.Method.AccountUid),
        SumOut = x.Sum.GetValueOrDefault()
      })).ToList<Gbs.Core.Entities.Payments.Payment>();
      if (paymentSpendBonuses != null)
        list.Add(paymentSpendBonuses);
      return list;
    }

    public (Gbs.Core.Entities.Payments.Payment paymentSpendBonuses, Decimal sumReturn, Decimal discountOnCheck) GetReturnsSum()
    {
      Decimal num1 = ReturnItemsViewModel.SaleDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
      {
        if (x.IsDeleted)
          return false;
        return x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid, GlobalDictionaries.PaymentTypes.CheckDiscount);
      })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)) - ReturnItemsViewModel.SaleOldReturnsDocsList.SelectMany<Document, Gbs.Core.Entities.Payments.Payment>((Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (x => x.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (payment =>
      {
        if (payment.IsDeleted)
          return false;
        return payment.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.MoneyDocumentPayment, GlobalDictionaries.PaymentTypes.Prepaid);
      })))).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (p => p.SumOut));
      Gbs.Core.Entities.Payments.Payment payment1 = (Gbs.Core.Entities.Payments.Payment) null;
      if (ReturnItemsViewModel.SaleDocument.ContractorUid != Guid.Empty)
        payment1 = this.ReturnSpendBonuses(this.Return.SumItemReturn);
      Decimal num2 = !(num1 > this.Return.SumItemReturn) ? num1 : this.Return.SumItemReturn - (payment1 != null ? payment1.SumOut : 0M);
      Decimal discountForCheckValue = this.CalculateDiscountForCheckValue();
      Decimal num3 = num2 - discountForCheckValue;
      return (payment1, num3, discountForCheckValue);
    }

    public Document SaveReturnDocument(
      string comment,
      List<Gbs.Core.Entities.Payments.Payment> payments,
      string numberReturn)
    {
      Guid docUid = Guid.NewGuid();
      List<Gbs.Core.Entities.Documents.Item> list = this.Return.ReturnList.Select<BasketItem, Gbs.Core.Entities.Documents.Item>((Func<BasketItem, Gbs.Core.Entities.Documents.Item>) (x => new Gbs.Core.Entities.Documents.Item(x, docUid))).ToList<Gbs.Core.Entities.Documents.Item>();
      list.ForEach((Action<Gbs.Core.Entities.Documents.Item>) (x => x.Uid = Guid.NewGuid()));
      Document document1 = new Document();
      document1.Uid = docUid;
      document1.Comment = comment;
      document1.Number = numberReturn;
      document1.ParentUid = ReturnItemsViewModel.SaleDocument.Uid;
      document1.Status = GlobalDictionaries.DocumentsStatuses.Close;
      document1.Type = GlobalDictionaries.DocumentsTypes.SaleReturn;
      document1.Storage = ReturnItemsViewModel.SaleDocument.Storage;
      document1.ContractorUid = ReturnItemsViewModel.SaleDocument.ContractorUid;
      document1.Section = Sections.GetCurrentSection();
      Gbs.Core.Entities.Users.User authUser = this.AuthUser;
      // ISSUE: explicit non-virtual call
      document1.UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty;
      document1.Payments = payments;
      document1.Items = list;
      Document document2 = document1;
      if (ReturnItemsViewModel.SaleDocument.IsFiscal)
      {
        document2.Properties.AddRange((IEnumerable<EntityProperties.PropertyValue>) ReturnItemsViewModel.SaleDocument.Properties);
        bool flag = this.PrintCheck(document2, this.AuthUser);
        document2.IsFiscal = flag || MessageBoxHelper.Show(Translate.ReturnItemsViewModelYНеУдалосьНапечататьЧекВозвратаПродолжитьСохранениеДокументаВозвратаБезЧека, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) == MessageBoxResult.Yes ? flag : throw new CancelException("Отказ продолжить сохранение возврата без чека");
      }
      new BonusHelper().UpdateSumBonusesForReturn(document2, this.Return.ReturnList.ToList<BasketItem>());
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        this.SaveResult = new DocumentsRepository(dataBase).Save(document2);
        if (!this.SaveResult)
          throw new ErrorHelper.GbsException(Translate.ReturnItemsViewModel_SaveReturnDocument_Документ_возврата_не_был_сохранен)
          {
            Level = MsgException.LevelTypes.Error
          };
        return document2;
      }
    }

    private void ShowReturnSumMessage(Decimal sumReturn, Gbs.Core.Entities.Payments.Payment paymentSpendBonuses)
    {
      string str = string.Empty;
      if (paymentSpendBonuses != null)
        str = string.Empty + "\n\n" + string.Format(Translate.ReturnItemsViewModel_Также_будет_возвращено___0_N2__баллов, (object) paymentSpendBonuses.SumOut);
      MessageBoxHelper.Warning(Translate.ReturnItemsViewModel_Сумма_к_возврату_меньше_общей_стоимости_возвращаемых_товаров + "\n\n" + string.Format(Translate.ReturnItemsViewModel_Стоимость_возвращаемых_товаров___0_N2_, (object) this.Return.SumItemReturn) + "\n" + string.Format(Translate.ReturnItemsViewModel_Сумма_к_возврату___0_N2_, (object) sumReturn) + str);
    }

    private void CheckKkmConnection()
    {
      Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      try
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.Basket_Save_Проверка_соединения_с_ККМ);
        using (KkmHelper kkmHelper1 = new KkmHelper(devicesConfig))
        {
          KkmHelper kkmHelper2 = kkmHelper1;
          Cashier cashier = new Cashier();
          Gbs.Core.Entities.Users.User authUser1 = this.AuthUser;
          // ISSUE: explicit non-virtual call
          cashier.UserUid = authUser1 != null ? __nonvirtual (authUser1.Uid) : Guid.Empty;
          cashier.Name = this.AuthUser?.Client?.Name ?? string.Empty;
          Gbs.Core.Entities.Users.User authUser2 = this.AuthUser;
          string str;
          if (authUser2 == null)
          {
            str = (string) null;
          }
          else
          {
            Client client = authUser2.Client;
            if (client == null)
            {
              str = (string) null;
            }
            else
            {
              List<EntityProperties.PropertyValue> properties = client.Properties;
              str = properties != null ? properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() : (string) null;
            }
          }
          if (str == null)
            str = string.Empty;
          cashier.Inn = str;
          kkmHelper2.CheckKkmReady(cashier);
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        if (MessageBoxHelper.Show(Translate.ReturnItemsViewModel_CheckKkmConnection_ + ex.Message, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) != MessageBoxResult.Yes)
          throw new CancelException("Отказ продолжить возврат без печати чека");
        ReturnItemsViewModel.SaleDocument.IsFiscal = false;
      }
    }

    private static string GetCommentForReturn(Decimal sumReturn)
    {
      Sales sales = new ConfigsRepository<Settings>().Get().Sales;
      (bool result, string output) = MessageBoxHelper.Input("", string.Format(Translate.ReturnItemsViewModel_Введите_причину_возврата___0___, (object) sumReturn.ToString("N2")), sales.IsReturnEmptyComment ? 0 : 3);
      if (result)
        return output;
      throw new CancelException("Отмена ввода причины возврата");
    }

    private static List<SelectPaymentMethods.PaymentGrid> GetPayments(Decimal sumReturn)
    {
      List<SelectPaymentMethods.PaymentGrid> paymentGridList = new List<SelectPaymentMethods.PaymentGrid>();
      List<SelectPaymentMethods.PaymentGrid> source;
      while (true)
      {
        (bool Result, List<SelectPaymentMethods.PaymentGrid> ListPayment, Decimal _) = new FrmInsertPaymentMethods().GetValuePayment(sumReturn, sumReturn, isReturnSale: true);
        int num1 = Result ? 1 : 0;
        source = ListPayment;
        if (num1 != 0)
        {
          Decimal num2 = ReturnItemsViewModel.SaleDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            PaymentMethods.PaymentMethod method = x.Method;
            return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut));
          Decimal num3 = ReturnItemsViewModel.SaleOldReturnsDocsList.SelectMany<Document, Gbs.Core.Entities.Payments.Payment>((Func<Document, IEnumerable<Gbs.Core.Entities.Payments.Payment>>) (x => (IEnumerable<Gbs.Core.Entities.Payments.Payment>) x.Payments)).Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
          {
            PaymentMethods.PaymentMethod method = x.Method;
            return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
          })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
          Decimal? nullable = source.Where<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, bool>) (x =>
          {
            PaymentMethods.PaymentMethod method = x.Method;
            return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Sbp;
          })).Sum<SelectPaymentMethods.PaymentGrid>((Func<SelectPaymentMethods.PaymentGrid, Decimal?>) (x => x.Sum));
          Decimal num4 = num2 - num3;
          if (!(nullable.GetValueOrDefault() <= num4 & nullable.HasValue))
            MessageBoxHelper.Warning(string.Format(Translate.ReturnItemsViewModel_GetPayments_Нельзя_вернуть_по_СБП_больше__чем_было_оплачено_изначально__Для_возврата_по_СБП_доступно__0_N2_, (object) (num2 - num3)));
          else
            goto label_5;
        }
        else
          break;
      }
      throw new CancelException("Отмена в форме ввода платежей");
label_5:
      return source;
    }

    private Gbs.Core.Entities.Payments.Payment ReturnSpendBonuses(Decimal sumReturn)
    {
      Decimal num1 = ReturnItemsViewModel.SaleDocument.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.BonusesDocumentPayment)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
      if (num1 == 0M)
        return (Gbs.Core.Entities.Payments.Payment) null;
      Decimal num2 = ReturnItemsViewModel.SaleDocument.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity * x.SellPrice * (100M - x.Discount) / 100M));
      Decimal num3 = Math.Round(sumReturn / num2 * num1, 2);
      sumReturn -= num3;
      Gbs.Core.Entities.Payments.Payment payment = new Gbs.Core.Entities.Payments.Payment();
      payment.User = this.AuthUser;
      payment.Comment = Translate.JournalGoodViewModel_Возврат_продажи;
      PaymentMethods.PaymentMethod paymentMethod = new PaymentMethods.PaymentMethod();
      paymentMethod.Uid = GlobalDictionaries.BonusesPaymentUid;
      paymentMethod.KkmMethod = GlobalDictionaries.KkmPaymentMethods.Bonus;
      payment.Method = paymentMethod;
      Client client = new Client();
      client.Uid = ReturnItemsViewModel.SaleDocument.ContractorUid;
      payment.Client = client;
      payment.Type = GlobalDictionaries.PaymentTypes.BonusesDocumentPayment;
      payment.SumOut = num3;
      return payment;
    }

    private void ValidateLists()
    {
      if (!this.Return.ReturnList.Any<BasketItem>())
        throw new ErrorHelper.GbsException(Translate.НетТоваровДляВозврата)
        {
          Level = MsgException.LevelTypes.Warm
        };
    }

    public bool SaveResult { get; set; }

    public bool IsActiveForm
    {
      get => this._isActiveForm;
      set
      {
        this._isActiveForm = value;
        this.OnPropertyChanged(nameof (IsActiveForm));
      }
    }

    public Gbs.Core.ViewModels.Sale.Return Return { get; set; } = new Gbs.Core.ViewModels.Sale.Return();

    public ICommand SaveReturnCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsActiveForm = false;
          this.ReturnSave();
          this.IsActiveForm = true;
        }));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    private static List<Document> SaleOldReturnsDocsList { get; set; }

    private static Document SaleDocument { get; set; }

    private Action CloseFormAction { get; set; }

    public ReturnItemsViewModel()
    {
    }

    public ReturnItemsViewModel(Action close, Guid saleDocUid)
    {
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
        ReturnItemsViewModel.SaleDocument = documentsRepository.GetByUid(saleDocUid);
        Document saleDocument = ReturnItemsViewModel.SaleDocument;
        List<BasketItem> list = saleDocument != null ? saleDocument.Items.Select<Gbs.Core.Entities.Documents.Item, BasketItem>((Func<Gbs.Core.Entities.Documents.Item, BasketItem>) (x => new BasketItem(x.Good, x.ModificationUid, x.SellPrice, ReturnItemsViewModel.SaleDocument.Storage, x.Quantity, x.Discount, x.Uid)
        {
          Comment = x.Comment
        })).ToList<BasketItem>() : (List<BasketItem>) null;
        ReturnItemsViewModel.SaleOldReturnsDocsList = documentsRepository.GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => !x.IS_DELETED && x.PARENT_UID == ReturnItemsViewModel.SaleDocument.Uid && x.TYPE == 2))).ToList<Document>();
        this.CloseFormAction = close;
        this.ShowItemsSale(list);
        this.Return.ReCalc();
        if (!this.Return.Items.Any<BasketItem>())
        {
          this.CountNotNull = false;
          WindowWithSize.IsCancel = false;
          this.CloseFormAction();
          throw new ErrorHelper.GbsException(Translate.ReturnItemsViewModel_Все_товары_из_данной_продажи_уже_вернули_)
          {
            Level = MsgException.LevelTypes.Warm
          };
        }
      }
    }

    private void AddAnaliticsPf(Document returnDoc, Document saleDoc)
    {
      Task.Run((Action) (() => PlanfixHelper.AnaliticHelper.AddReturnAnalitic(returnDoc, saleDoc)));
    }

    public static bool AcquiringReturn(Decimal sum, string rrn, string typeCardMethod)
    {
      Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (devicesConfig.AcquiringTerminal.Type == GlobalDictionaries.Devices.AcquiringTerminalTypes.None || sum <= 0M)
        return true;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ReturnItemsViewModel_Ожидание_ответа_от_терминала);
      using (AcquiringHelper acquiringHelper = new AcquiringHelper(devicesConfig))
      {
        int num1 = acquiringHelper.ReturnPayment(sum, rrn, typeCardMethod) ? 1 : 0;
        progressBar.Close();
        if (num1 == 0)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.ReturnItemsViewModel_Не_удалось_выполнить_возврат_платежа_картой_через_терминал, icon: MessageBoxImage.Hand);
          return false;
        }
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.ReturnItemsViewModel_Возврат_выполнен_успешно,
          Text = string.Format(Translate.ReturnItemsViewModel_Возврат_оплаты_картой_на_сумму__0_N2__успешно_выполнен, (object) sum)
        });
        return true;
      }
    }

    private bool SBPReturn(Decimal sum, string rrn, string numberReturn)
    {
      if (sum <= 0M)
        return true;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP.Type == GlobalDictionaries.Devices.SbpTypes.None)
      {
        MessageBoxHelper.Error(Translate.ReturnItemsViewModel_SBPReturn_Не_указана_система_для_работы_с_СБП__укажите_корректные_настройки_и_повторите_возврат_по_СБП_);
        return false;
      }
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ReturnItemsViewModel_SBPReturn_Возврат_денежных_средств_по_СБП);
      using (SpbHelper spbHelper = new SpbHelper(sum, Convert.ToInt64(ReturnItemsViewModel.SaleDocument.Number), rrn.IsNullOrEmpty() ? numberReturn : rrn))
      {
        int num = spbHelper.Return(sum) ? 1 : 0;
        progressBar.Close();
        return num != 0;
      }
    }

    private void ShowItemsSale(List<BasketItem> items)
    {
      List<BasketItem> listItem = new List<BasketItem>();
      items.ForEach((Action<BasketItem>) (x => listItem.Add(x.Clone())));
      this.Return.Items = new ObservableCollection<BasketItem>(listItem);
      foreach (Document saleOldReturnsDocs in ReturnItemsViewModel.SaleOldReturnsDocsList)
      {
        foreach (Gbs.Core.Entities.Documents.Item obj in saleOldReturnsDocs.Items)
        {
          Gbs.Core.Entities.Documents.Item goodItem = obj;
          int index = this.Return.Items.ToList<BasketItem>().FindIndex((Predicate<BasketItem>) (x =>
          {
            if (!(x.Good.Uid == goodItem.GoodUid) || !(x.SalePrice == goodItem.SellPrice))
              return false;
            Guid? uid = x.GoodModification?.Uid;
            Guid modificationUid = goodItem.ModificationUid;
            return uid.HasValue && uid.GetValueOrDefault() == modificationUid;
          }));
          if (index != -1)
          {
            BasketItem basketItem = this.Return.Items[index];
            basketItem.Quantity = basketItem.Quantity - goodItem.Quantity;
          }
        }
      }
      this.Return.Items = new ObservableCollection<BasketItem>(this.Return.Items.Where<BasketItem>((Func<BasketItem, bool>) (x => x.Quantity != 0M)));
      this.Return.ReCalc();
    }
  }
}
