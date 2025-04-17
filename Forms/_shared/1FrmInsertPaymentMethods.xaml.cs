// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmInsertPaymentMethods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmInsertPaymentMethods : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal TextBox TextBoxTotalSum;
    internal TextBox TextBoxSumToPay;
    internal DataGrid GridPayments;
    internal TextBox TextBoxDelivery;
    internal ConfirmPanelControl1 ConfirmPanelControl1;
    private bool _contentLoaded;

    public FrmInsertPaymentMethods()
    {
      this.InitializeComponent();
      TooltipsSetter.Set(this);
    }

    public (bool Result, List<SelectPaymentMethods.PaymentGrid> ListPayment, Decimal Delivery) GetValuePayment(
      Decimal sumDoc,
      Decimal receiveSum,
      bool isCloseIfOne = true,
      bool isReturnSale = false,
      Decimal sumCertificate = 0M,
      Decimal sumBonuses = 0M,
      Decimal sumPrepaid = 0M,
      bool isPaymentPrepaid = false,
      Gbs.Core.ViewModels.Basket.Basket basket = null)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: field reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: field reference
        // ISSUE: method reference
        List<PaymentMethods.PaymentMethod> paymentMethodList1 = PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>(System.Linq.Expressions.Expression.Lambda<Func<PAYMENT_METHODS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.OrElse((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.SECTION_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<System.Linq.Expressions.Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENT_METHODS.get_SECTION_UID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) null, FieldInfo.GetFieldFromHandle(__fieldref (Guid.Empty))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Not((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENT_METHODS.get_IS_DELETED))))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.NotEqual((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (PAYMENT_METHODS.get_UID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) null, FieldInfo.GetFieldFromHandle(__fieldref (GlobalDictionaries.BonusesPaymentUid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Inequality)))), parameterExpression)));
        if (isPaymentPrepaid)
          paymentMethodList1 = paymentMethodList1.Where<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x == null || x.PaymentMethodsType != GlobalDictionaries.PaymentMethodsType.Sbp)).ToList<PaymentMethods.PaymentMethod>();
        if (sumCertificate == 0M)
          paymentMethodList1.RemoveAll((Predicate<PaymentMethods.PaymentMethod>) (x => x.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate));
        if (sumPrepaid > 0M)
          paymentMethodList1.Insert(0, new PaymentMethods.PaymentMethod()
          {
            DisplayIndex = 0,
            KkmMethod = GlobalDictionaries.KkmPaymentMethods.PrePayment,
            Name = Translate.GlobalDictionaries_Предоплата
          });
        Bonuses bonuses = new Bonuses();
        bonuses.Load();
        bool flag = false;
        if (basket != null)
          flag = basket.Client != null && basket.Client.TotalBonusSum > 0M && bonuses.IsActiveBonuses;
        if (flag)
        {
          List<PaymentMethods.PaymentMethod> paymentMethodList2 = paymentMethodList1;
          PaymentMethods.PaymentMethod paymentMethod = new PaymentMethods.PaymentMethod();
          paymentMethod.DisplayIndex = paymentMethodList1.Count;
          paymentMethod.KkmMethod = GlobalDictionaries.KkmPaymentMethods.Bonus;
          paymentMethod.Name = Translate.БАЛЛЫ;
          paymentMethod.Uid = GlobalDictionaries.BonusesPaymentUid;
          paymentMethodList2.Add(paymentMethod);
        }
        if (sumCertificate > sumDoc)
        {
          MessageBoxHelper.Warning(Translate.FrmInsertPaymentMethods_Сумма__оплаченная_сертификатом__больше_чем_сумма_продажи__Все_сертификаты_будут_активированы__а_сумма_уменьшена_до_суммы_продажи);
          sumCertificate = sumDoc;
        }
        if (paymentMethodList1.Count == 1 & isCloseIfOne && sumBonuses <= 0M)
          return (true, new List<SelectPaymentMethods.PaymentGrid>()
          {
            new SelectPaymentMethods.PaymentGrid((SelectPaymentMethods) null, (PaymentMethods.PaymentMethod) null)
            {
              Method = paymentMethodList1.First<PaymentMethods.PaymentMethod>(),
              Sum = new Decimal?(receiveSum > sumDoc ? sumDoc : receiveSum)
            }
          }, paymentMethodList1.Single<PaymentMethods.PaymentMethod>().KkmMethod == GlobalDictionaries.KkmPaymentMethods.Cash ? receiveSum - sumDoc : 0M);
        SelectPaymentMethods selectPaymentMethods = new SelectPaymentMethods(paymentMethodList1, sumDoc, sumCertificate, receiveSum, sumPrepaid, isPaymentPrepaid, sumBonuses, basket)
        {
          Close = new Action(((Window) this).Close),
          IsReturnSale = isReturnSale,
          PrepaidVisibility = sumPrepaid > 0M ? Visibility.Visible : Visibility.Collapsed,
          OkButtonText = isReturnSale ? Translate.FrmInsertPaymentMethods_GetValuePayment_ВЕРНУТЬ : Translate.FrmInsertPaymentMethods_ОПЛАТИТЬ
        };
        this.DataContext = (object) selectPaymentMethods;
        this.SetHotKeys();
        this.ShowDialog();
        return selectPaymentMethods.Result;
      }
    }

    private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
    {
      int result = 0;
      int.TryParse(sender.ToString(), out result);
      ContentPresenter cellContent = (ContentPresenter) this.GridPayments.Columns[0].GetCellContent((DataGridRow) this.GridPayments.ItemContainerGenerator.ContainerFromIndex(result));
      DecimalUpDown dud = (DecimalUpDown) cellContent?.ContentTemplate.FindName("method", (FrameworkElement) cellContent);
      TaskHelper.TaskRun((Action) (() =>
      {
        Thread.Sleep(50);
        Application.Current?.Dispatcher?.Invoke((Action) (() => dud?.Focus()));
      }), false);
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        SelectPaymentMethods model = (SelectPaymentMethods) this.DataContext;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (o =>
        {
          Action closeAction = model.CloseAction;
          if (closeAction == null)
            return;
          closeAction();
        }));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            model.BuySaleCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void UIElement_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      this.ShowKeyboard(sender);
    }

    private void UIElement_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.ActivateOnlyByClick)
        return;
      this.ShowKeyboard(sender);
    }

    private void ShowKeyboard(object sender)
    {
      FrmKeyboard.ShowKeyboard(true);
      FrmKeyboard.SelectAllInControl((Control) sender);
    }

    private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
      WindowWithSize.NumericBoxCheck(e);
    }

    private void FrmInsertPaymentMethods_OnKeyDown(object sender, KeyEventArgs e)
    {
      if (Keyboard.Modifiers != ModifierKeys.Control || (e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9))
        return;
      LogHelper.Debug("Нажата клавиша цифры и CTRL в форме оплаты");
      int? keyNumericValue = e.GetKeyNumericValue();
      int num = keyNumericValue.GetValueOrDefault(-1);
      LogHelper.Debug("Num=" + num.ToString());
      if (!keyNumericValue.HasValue)
        return;
      int? nullable = keyNumericValue;
      num = 0;
      if (nullable.GetValueOrDefault() == num & nullable.HasValue)
        return;
      SelectPaymentMethods dataContext = (SelectPaymentMethods) this.DataContext;
      if (dataContext.Payments.Count < keyNumericValue.Value)
        return;
      SelectPaymentMethods.PaymentGrid payment = dataContext.Payments[keyNumericValue.Value - 1];
      if (payment.Method.KkmMethod.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Certificate, GlobalDictionaries.KkmPaymentMethods.PrePayment, GlobalDictionaries.KkmPaymentMethods.Bonus))
        return;
      payment.GetTotalSumToPayCommand.Execute((object) null);
      this.EventSetter_OnHandler((object) (keyNumericValue.Value - 1), new RoutedEventArgs());
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frminsertpaymentmethods.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBoxTotalSum = (TextBox) target;
          break;
        case 2:
          this.TextBoxSumToPay = (TextBox) target;
          break;
        case 3:
          this.GridPayments = (DataGrid) target;
          this.GridPayments.KeyDown += new KeyEventHandler(this.FrmInsertPaymentMethods_OnKeyDown);
          this.GridPayments.Loaded += new RoutedEventHandler(this.EventSetter_OnHandler);
          break;
        case 5:
          this.TextBoxDelivery = (TextBox) target;
          break;
        case 6:
          this.ConfirmPanelControl1 = (ConfirmPanelControl1) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 4)
        return;
      ((UIElement) target).PreviewMouseUp += new MouseButtonEventHandler(this.UIElement_OnPreviewMouseUp);
      ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.UIElement_OnGotKeyboardFocus);
      ((UIElement) target).PreviewKeyDown += new KeyEventHandler(this.UIElement_OnPreviewKeyDown);
    }
  }
}
